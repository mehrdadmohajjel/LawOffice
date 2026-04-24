// wwwroot/js/clients.js

let clientsData = [];
document.addEventListener('focusin', function (e) {
    // بررسی می‌کنیم که آیا عنصری که کاربر روی آن کلیک کرده داخل کادر تقویم است یا خیر
    const jdpContainer = document.querySelector('jdp-container') || document.querySelector('.jdp-container');

    if (jdpContainer && jdpContainer.contains(e.target)) {
        // متوقف کردن رفتار پیش‌فرض بوت‌استرپ برای دزدیدن فوکوس
        e.stopImmediatePropagation();
    }
}, true);
$(document).ready(function () {
    if (typeof loadClients === "function") {
        loadClients();
    }

    // مدیریت چک‌باکس با جی‌کوئری
    $('#createUserAccount').on('change', function () {
        if ($(this).is(':checked')) {
            $('#userAccountFields').removeClass('d-none');
        } else {
            $('#userAccountFields').addClass('d-none');
        }
    });

    // راه‌اندازی jalalidatepicker
    if (typeof jalaliDatepicker !== 'undefined') {
        jalaliDatepicker.startWatch({
            hideAfterChange: true,
            autoHide: true,
            showTodayBtn: true,
            showEmptyBtn: true
        });
        // نیازی به رویداد jdp:change برای تبدیل تاریخ نیست، چون بک‌اند این کار را انجام می‌دهد.
    } else {
        console.error("خطا: jalalidatepicker در صفحه بارگذاری نشده است.");
    }
});

function renderClientsTable() {
    const tbody = document.getElementById('clientsTableBody');

    if (clientsData.length === 0) {
        tbody.innerHTML = '<tr><td colspan="6" class="text-center text-muted py-4">موکلی ثبت نشده است</td></tr>';
        return;
    }

    tbody.innerHTML = clientsData.map(client => {
        // فرض بر این است که از این پس بک‌اند تاریخ را به صورت شمسی (رشته) در فیلد createdAt می‌فرستد
        // اگر هنوز میلادی می‌فرستد، بهتر است در API بک‌اند آن را با ToShamsiDateString() تبدیل کنید
        let displayDate = client.BirthDate ? client.BirthDate.split('T')[0] : '';

        return `
            <tr>
                <td>${client.FirstName} ${client.LastName}</td>
                <td>${client.NationalCode}</td>
                <td>${client.PhoneNumber}</td>
                <td><span class="badge bg-info">${client.CasesCount || 0}</span></td>
                <td>${displayDate}</td>
                <td>
                                    <button class="btn btn-sm btn-outline-primary" onclick="editClient(${client.Id})" title="ویرایش">
                        <i class="bi bi-pencil"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-info" onclick="viewClient(${client.Id})" title="مشاهده">
                        <i class="bi bi-eye"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteClient(${client.Id})" title="حذف">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            </tr>
        `;
    }).join('');
}

function openClientModal(clientId = null) {
    const modal = document.getElementById('clientModal');
    const form = document.getElementById('clientForm');
    const title = document.getElementById('clientModalTitle');

    form.reset();
    document.getElementById('clientId').value = '';
    // فیلد birthDateGregorian حذف شده است، پس نیازی به پاک کردن آن نیست
    document.getElementById('userAccountFields').classList.add('d-none');

    if (clientId) {
        title.textContent = 'ویرایش موکل';
        loadClientData(clientId);
    } else {
        title.textContent = 'موکل جدید';
    }
}

async function loadClientData(clientId) {
    try {
        const response = await AuthManager.apiCall(`/Clients/Get/${clientId}`);

        if (!response.ok) {
            throw new Error(`خطای سرور: ${response.status}`);
        }

        const contentType = response.headers.get("content-type");
        if (!contentType || !contentType.includes("application/json")) {
            throw new Error("پاسخ دریافتی از سرور JSON معتبر نیست.");
        }

        const result = await response.json();

        if (result.success) {
            const client = result.data;
            document.getElementById('clientId').value = client.Id;
            document.getElementById('firstName').value = client.FirstName;
            document.getElementById('lastName').value = client.LastName;
            document.getElementById('nationalCode').value = client.NationalCode;

            // اکنون سرور باید تاریخ را مستقیماً به صورت رشته شمسی برگرداند
            document.getElementById('birthDate').value = client.BirthDate || '';

            document.getElementById('phoneNumber').value = client.PhoneNumber;
            document.getElementById('email').value = client.Eemail || '';
            document.getElementById('address').value = client.Address || '';
            document.getElementById('education').value = client.Education || '';
            document.getElementById('occupation').value = client.Occupation || '';
        } else {
            showError(result.message || 'خطا در دریافت اطلاعات موکل');
        }
    } catch (error) {
        console.error('Error loading client:', error);
        showError(error.message || 'خطا در بارگذاری اطلاعات موکل');
    }
}

async function loadClientDataa(clientId) {
    try {
        const response = await AuthManager.apiCall(`/Clients/Get/${clientId}`);

        if (!response.ok) {
            throw new Error(`خطای سرور: ${response.status}`);
        }

        const contentType = response.headers.get("content-type");
        if (!contentType || !contentType.includes("application/json")) {
            throw new Error("پاسخ دریافتی از سرور JSON معتبر نیست.");
        }

        const result = await response.json();

        if (result.success) {
            const client = result.data;

            // تابع کمکی برای جلوگیری از خطای توقف کد در صورت پیدا نشدن ID در HTML
            const setValueSafe = (elementId, value) => {
                const el = document.getElementById(elementId);
                if (el) {
                    el.value = value || '';
                } else {
                    console.warn(`هشدار: المان با آیدی '${elementId}' در فرم پیدا نشد.`);
                }
            };

            // استفاده از تابع ایمن برای تخصیص مقادیر
            setValueSafe('ClientId', client.Id);
            setValueSafe('FirstName', client.FirstName);
            setValueSafe('LastName', client.LastName);
            setValueSafe('NationalCode', client.NationalCode);
            setValueSafe('BirthDate', client.BirthDate); // مقدار تقویم شمسی

            setValueSafe('phoneNumber', client.PhoneNumber);
            setValueSafe('email', client.Email);
            setValueSafe('address', client.Address);
            setValueSafe('education', client.Education);
            setValueSafe('occupation', client.Occupation);


        } else {
            showError(result.message || 'خطا در دریافت اطلاعات موکل');
        }
    } catch (error) {
        console.error('Error loading client:', error);
        showError(error.message || 'خطا در بارگذاری اطلاعات موکل');
    }
}

async function saveClient() {
    const data = {
        id: parseInt(document.getElementById('clientId').value || 0),
        firstName: document.getElementById('firstName').value.trim(),
        lastName: document.getElementById('lastName').value.trim(),
        nationalCode: document.getElementById('nationalCode').value.trim(),
        education: document.getElementById('education').value || null ,
        birthDate: document.getElementById('birthDate').value.trim() || null,
        phoneNumber: document.getElementById('phoneNumber').value.trim(),
        email: document.getElementById('email').value.trim() || null,
        address: document.getElementById('address').value.trim() || null
    };

    try {
        const url = data.id === 0 ? '/Clients/Create' : '/Clients/Update';

        const response = await AuthManager.apiCall(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error(`خطای ارتباط با سرور: ${response.status}`);
        }

        const contentType = response.headers.get("content-type");
        if (!contentType || !contentType.includes("application/json")) {
            throw new Error("پاسخ دریافتی از سرور JSON معتبر نیست.");
        }

        const result = await response.json();

        if (result.success) {
            ToastManager.success(result.message, 4000);
            bootstrap.Modal.getInstance(document.getElementById('clientModal')).hide();
            if (typeof loadClients === "function") {
                loadClients();
            }
        } else {
            ToastManager.error(result.message || "خطا در ذخیره اطلاعات");
        }

    } catch (err) {
        console.error(err);
        ToastManager.error(err.message || "خطا در ارتباط با سرور");
    }
}

function editClient(clientId) {
    openClientModal(clientId);
    const modal = new bootstrap.Modal(document.getElementById('clientModal'));
    modal.show();
}

async function deleteClient(clientId) {
    if (!window.confirm("آیا از حذف این موکل مطمئن هستید؟")) return;

    try {
        const response = await AuthManager.apiCall(`/Clients/Delete/${clientId}`, { method: 'DELETE' });

        if (!response.ok) {
            throw new Error(`خطای ارتباط با سرور: ${response.status}`);
        }

        const contentType = response.headers.get("content-type");
        if (!contentType || !contentType.includes("application/json")) {
            throw new Error("پاسخ دریافتی از سرور JSON معتبر نیست.");
        }

        const result = await response.json();

        if (result.success) {
            ToastManager.success("موکل با موفقیت حذف شد");
            if (typeof loadClients === "function") {
                loadClients();
            }
        } else {
            ToastManager.error(result.message || 'خطا در حذف موکل');
        }
    } catch (error) {
        console.error('Error deleting client:', error);
        showError(error.message || 'خطا در برقراری ارتباط با سرور');
    }
}
// تابع دریافت لیست مشتریان از بک‌اند
async function loadClients() {
    try {
        // ارسال درخواست GET به کنترلر
        const response = await AuthManager.apiCall('/Clients/GetAll');

        if (!response.ok) {
            throw new Error(`خطای سرور: ${response.status}`);
        }

        const contentType = response.headers.get("content-type");
        if (!contentType || !contentType.includes("application/json")) {
            throw new Error("پاسخ دریافتی از سرور JSON معتبر نیست.");
        }

        const result = await response.json();

        if (result.success) {
            clientsData = result.data || []; // ذخیره داده‌ها در متغیر سراسری
            renderClientsTable();            // رندر مجدد جدول
        } else {
            showError(result.message || 'خطا در دریافت لیست مشتریان');
        }
    } catch (error) {
        console.error('Error loading clients:', error);
        showError(error.message || 'خطا در برقراری ارتباط با سرور');

        // نمایش پیام خطا در جدول در صورت عدم موفقیت
        const tbody = document.getElementById('clientsTableBody');
        if (tbody) {
            tbody.innerHTML = '<tr><td colspan="6" class="text-center text-danger py-4">خطا در بارگذاری اطلاعات</td></tr>';
        }
    }
}

function viewClient(clientId) {
    window.location.href = `/Clients/Details/${clientId}`;
}

function showSuccess(message) {
    if (typeof ToastManager !== 'undefined') ToastManager.success(message);
    else ToastManager.error(message);

}

function showError(message) {
    if (typeof ToastManager !== 'undefined') ToastManager.error(message);
    else ToastManager.error(message);
}
