// Await document loading before executing scripts
document.addEventListener('DOMContentLoaded', init);

// --- MAIN INITIALIZATION ---
function init() {
    initDatePicker();
    loadDropdowns();

    // Set initial date and load today's appointments
    const dateInput = document.getElementById('appointmentDate');
    if (dateInput) {
        // Set to today's date in 'fa-IR' format
        dateInput.value = new Date().toLocaleDateString('fa-IR-u-nu-latn');
        loadAppointmentsForSelectedDate();

        // Add event listener for date changes
        dateInput.addEventListener('change', loadAppointmentsForSelectedDate);
    }
}

// --- DATEPICKER SETUP ---
function initDatePicker() {
    if (typeof jalaliDatepicker !== 'undefined') {
        jalaliDatepicker.startWatch({
            selector: '[data-jdp]',
            showTodayBtn: true,
            showEmptyBtn: true,
            autoHide: true,
            persianDigits: true
        });
    }
}

// --- DATA LOADING ---
async function loadDropdowns() {
    await Promise.all([
        populateSelectFromUrl('/Appointments/GetLawyers', 'lawyerId', 'وکیل'),
        populateSelectFromUrl('/Appointments/GetClients', 'clientId', 'موکل')
    ]);
}

async function populateSelectFromUrl(url, selectId, placeholder) {
    const select = document.getElementById(selectId);
    if (!select) return;

    try {
        const response = await fetch(url);
        if (!response.ok) throw new Error(`Network response was not ok for ${url}`);

        const data = await response.json();

        select.innerHTML = `<option value="">انتخاب ${placeholder}</option>`;
        data.forEach(item => {
            select.innerHTML += `<option value="${item.Id}">${item.FullName || item.Name}</option>`;
        });
    } catch (error) {
        console.error(`Error loading ${selectId}:`, error);
        select.innerHTML = `<option value="">خطا در بارگذاری</option>`;
    }
}

async function loadAppointmentsForSelectedDate() {
    const dateInput = document.getElementById('appointmentDate');
    const appointmentsListDiv = document.getElementById('appointmentsList');
    if (!dateInput || !appointmentsListDiv) return;

    const selectedDate = dateInput.value;
    if (!selectedDate) return;

    appointmentsListDiv.innerHTML = '<p class="text-center">در حال بارگذاری نوبت‌ها...</p>';

    try {
        const response = await fetch(`/Appointments/GetByDate?date=${encodeURIComponent(selectedDate)}`);
        if (!response.ok) throw new Error('Failed to fetch appointments.');

        const appointments = await response.json();
        renderAppointments(appointments);
    } catch (error) {
        console.error('Error loading appointments:', error);
        appointmentsListDiv.innerHTML = '<p class="text-danger text-center">خطا در بارگذاری نوبت‌ها.</p>';
    }
}

// --- UI RENDERING ---
function renderAppointments(appointments) {
    const listDiv = document.getElementById('appointmentsList');
    listDiv.innerHTML = '';

    if (!appointments || appointments.length === 0) {
        listDiv.innerHTML = '<p class="text-muted text-center">هیچ نوبتی در این تاریخ ثبت نشده است.</p>';
        return;
    }
    appointments.forEach(app => {
        // یکسان‌سازی نام فیلدها برای جلوگیری از خطای بزرگ و کوچک بودن حروف
        const appointmentId = app.id || app.Id;
        const clientName = app.clientName || app.ClientName || 'نامشخص';
        const lawyerName = app.lawyerName || app.LawyerName || 'نامشخص';
        const startTime = app.startTime || app.StartTime || app.AppointmentTime;
        const endTime = app.endTime || app.EndTime || 'نامشخص';
        const notes = app.notes || app.Notes || 'توضیحات ثبت نشده است.';
        const status = app.status || app.Status;

        const item = document.createElement('div');
        // استایل‌دهی مدرن به آیتم لیست
        item.className = 'list-group-item list-group-item-action mb-3 shadow-sm border border-light rounded-3 p-3 appointment-item';

        item.innerHTML = `
        <div class="d-flex w-100 justify-content-between align-items-center mb-3">
            <div>
                <h5 class="mb-1 fw-bold text-primary">
                    <i class="bi bi-person-circle me-1"></i> ${clientName}
                </h5>
                <div class="mt-2">
                    <span class="badge bg-light text-dark border me-2 py-2 px-3">
                        <i class="bi bi-clock text-success"></i> شروع: ${startTime}
                    </span>
                    <span class="badge bg-light text-dark border py-2 px-3">
                        <i class="bi bi-clock-history text-danger"></i> پایان: ${endTime}
                    </span>
                </div>
            </div>
            <div class="text-end">
                 ${getStatusBadge(status)}
            </div>
        </div>

        <div class="row mb-3 bg-light p-3 rounded-2 mx-0 border-start border-4 border-secondary">
            <div class="col-md-5 mb-2 mb-md-0">
                <strong class="text-muted small d-block mb-1">وکیل پرونده:</strong>
                <span class="fw-semibold"><i class="bi bi-briefcase me-1"></i> ${lawyerName}</span>
            </div>
            <div class="col-md-7">
                <strong class="text-muted small d-block mb-1">توضیحات:</strong>
                <span class="text-secondary small">${notes}</span>
            </div>
        </div>

        <div class="d-flex justify-content-between align-items-center border-top pt-3 mt-2">
            <div class="d-flex align-items-center bg-light p-1 rounded-pill border">
                <span class="ms-3 me-2 small text-muted fw-bold">تغییر وضعیت:</span>
                <!-- حذف حاشیه سلکت باکس برای زیبایی بیشتر داخل باکس گرد -->
                <div style="min-width: 120px;">
                    ${getStatusButtons(appointmentId, status)}
                </div>
            </div>
            <div class="btn-group shadow-sm">
                <!-- استفاده از کل آبجکت برای جلوگیری از نیاز به دریافت مجدد اطلاعات از سرور هنگام ویرایش -->
                <button class="btn btn-sm btn-outline-primary" onclick='editAppointment(${JSON.stringify(app)})'>
                    <i class="bi bi-pencil-square me-1"></i> ویرایش
                </button>
                <button class="btn btn-sm btn-outline-danger" onclick="deleteAppointment(${appointmentId})">
                    <i class="bi bi-trash me-1"></i> حذف
                </button>
            </div>
        </div>
    `;
        listDiv.appendChild(item);
    });

}

// --- CRUD OPERATIONS ---
async function saveAppointment() {
    const idValue = document.getElementById('appointmentId')?.value;
    const clientIdValue = document.getElementById('clientId')?.value;
    const lawyerIdValue = document.getElementById('lawyerId')?.value;

    // دریافت مقادیر زمان
    const startTimeValue = document.getElementById('startTime').value;
    const endTimeValue = document.getElementById('endTime')?.value;

    // تبدیل فرمت "HH:mm" به "HH:mm:00" برای سازگاری با TimeSpan در C#
    const formattedStartTime = startTimeValue && startTimeValue.length === 5 ? startTimeValue + ':00' : startTimeValue;
    const formattedEndTime = endTimeValue && endTimeValue.length === 5 ? endTimeValue + ':00' : endTimeValue;

    const data = {
        Id: idValue ? parseInt(idValue) : 0,
        LawyerId: lawyerIdValue ? parseInt(lawyerIdValue) : 0,
        ClientId: clientIdValue ? parseInt(clientIdValue) : null,
        CaseId: null, // اگر در فرم نیست، مقدار تهی ارسال شود تا خطا ندهد
        AppointmentDate: document.getElementById('appointmentDate').value,
        StartTime: formattedStartTime,
        EndTime: formattedEndTime, // در صورت نیاز بک‌اند
        Notes: document.getElementById('description')?.value || null // تغییر Description به Notes
    };

    // بررسی اعتبارسنجی سمت کاربر
    if (!data.LawyerId || !data.AppointmentDate || !data.StartTime) {
        ToastManager.error("لطفاً فیلدهای ضروری (وکیل، تاریخ و ساعت شروع) را پر کنید.");
        return;
    }

    try {
        const url = data.Id === 0 ? '/Appointments/Create' : '/Appointments/Update';
        const method = data.Id === 0 ? 'POST' : 'PUT';

        const response = await AuthManager.apiCall(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            // اگر خطای 400 (Bad Request) بود، سعی می‌کنیم جزئیات خطا را بخوانیم
            if (response.status === 400) {
                const errorDetails = await response.json();
                console.error("Validation Errors:", errorDetails.errors);
            }
            throw new Error(`خطای ارتباط با سرور: ${response.status}`);
        }

        const result = await response.json();

        if (result.success) {
            ToastManager.success(result.message || 'نوبت با موفقیت ثبت شد', 4000);
            const modalElement = document.getElementById('appointmentModal');
            if (modalElement) bootstrap.Modal.getInstance(modalElement).hide();
            loadAppointmentsForSelectedDate();
        } else {
            ToastManager.error(result.message || "خطا در ذخیره اطلاعات");
        }

    } catch (err) {
        console.error(err);
        ToastManager.error("خطا در ارسال اطلاعات به سرور. .");
    }
}

async function editAppointment(id) {
    alert("قابلیت ویرایش نیازمند پیاده‌سازی متد دریافت اطلاعات بر اساس شناسه (GetById) است.");
    // مثال پیاده‌سازی:
    // const response = await fetch(`/Appointments/GetById/${id}`);
    // const data = await response.json();
    // document.getElementById('appointmentId').value = data.id;
    // ... تنظیم سایر فیلدها و باز کردن مودال
}

async function deleteAppointment(appointmentId) {
    if (!window.confirm("آیا از حذف این نوبت مطمئن هستید؟")) return;

    try {
        const response = await AuthManager.apiCall(`/Appointments/Delete/${appointmentId}`, { method: 'DELETE' });

        if (!response.ok) {
            throw new Error(`خطای ارتباط با سرور: ${response.status}`);
        }

        const result = await response.json();

        if (result.success) {
            ToastManager.success("نوبت با موفقیت حذف شد");
            loadAppointmentsForSelectedDate();
        } else {
            ToastManager.error(result.message || 'خطا در حذف نوبت');
        }
    } catch (error) {
        console.error('Error deleting appointment:', error);
        ToastManager.error(error.message || 'خطا در برقراری ارتباط با سرور');
    }
}

function clearForm() {
    const form = document.getElementById('appointmentForm');
    if (!form) return;

    const currentDate = document.getElementById('appointmentDate')?.value;
    form.reset();
    document.getElementById('appointmentId').value = '';

    if (currentDate) {
        document.getElementById('appointmentDate').value = currentDate;
    }
}
function getStatusButtons(appointmentId, currentStatus) {
    return `
        <select class="form-select form-select-sm border-0 bg-transparent text-dark fw-bold shadow-none" 
                style="cursor: pointer;"
                onchange="changeAppointmentStatus(${appointmentId}, parseInt(this.value))">
            <option value="1" ${currentStatus === 1 ? 'selected' : ''}>آزاد</option>
            <option value="2" ${currentStatus === 2 ? 'selected' : ''}>رزرو شده</option>
            <option value="3" ${currentStatus === 3 ? 'selected' : ''}>انجام شده</option>
            <option value="4" ${currentStatus === 4 ? 'selected' : ''}>لغو شده</option>
        </select>
    `;
}
function getStatusBadge(statusId) {
    switch (statusId) {
        case 1: return '<span class="badge bg-primary px-3 py-2">آزاد</span>';
        case 2: return '<span class="badge bg-warning text-dark px-3 py-2">رزرو شده</span>';
        case 3: return '<span class="badge bg-success px-3 py-2">انجام شده</span>';
        case 4: return '<span class="badge bg-danger px-3 py-2">لغو شده</span>';
        default: return '<span class="badge bg-secondary px-3 py-2">نامشخص</span>';
    }
}

async function changeAppointmentStatus(id, newStatus) {
    try {
        const payload = { status: newStatus };

        // اصلاح ساختار پارامتر دوم (options)
        const response = await AuthManager.apiCall(`/${id}/status`, {
            method: 'PUT',
            body: JSON.stringify(payload) // تبدیل آبجکت به رشته JSON
        });

        if (response) {
            ToastManager.success("وضعیت نوبت تغییر کرد.");
            loadAppointmentsForSelectedDate();
        }
    } catch (error) {
        console.error(error);
        ToastManager.error("خطا در ارتباط با سرور برای تغییر وضعیت.");
    }
}
