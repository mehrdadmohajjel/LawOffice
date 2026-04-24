
let courtSessionsData = [];
let casesList = [];
let lawyersList = [];

document.addEventListener('DOMContentLoaded', function () {
    // Initialize modals, listeners, and load initial data
    initializePage();
});

async function initializePage() {
    try {
        // اجرای همزمان هر دو تابع و انتظار برای پایان هر دو
        await Promise.all([
            loadCourtSessions(),
            loadDropdowns()
        ]);

        console.log("Both data functions loaded successfully.");
    } catch (error) {
        console.error("Error loading initial data:", error);
    }

    // Setup form submission listener
    const courtSessionForm = document.getElementById('courtSessionForm');
    if (courtSessionForm) {
        courtSessionForm.addEventListener('submit', function (e) {
            e.preventDefault();
            saveSession();
        });
    }

    // Setup Jalali datepicker
    if (typeof jalaliDatepicker !== 'undefined') {
        jalaliDatepicker.startWatch({
            hideAfterChange: true,
            autoHide: true,
            showTodayBtn: true,
            showEmptyBtn: true
        });
    } else {
        console.error("Critical Error: jalalidatepicker is not loaded on the page.");
    }
}


// --- Data Fetching Functions ---

async function loadCourtSessions() {
    try {
        const response = await AuthManager.apiCall('/CourtSessions/GetAll');
        if (!response.ok) {
            throw new Error(`Network response was not ok, status: ${response.status}`);
        }
        courtSessionsData = await response.json();
        renderSessions(courtSessionsData);
    } catch (error) {
        console.error('Error loading court sessions:', error);
        const sessionsTableBody = document.getElementById('courtSessionsTableBody');
        if (sessionsTableBody) {
            sessionsTableBody.innerHTML = `<tr><td colspan="8" class="text-center text-danger">خطا در بارگذاری لیست جلسات.</td></tr>`;
        }
    }
}

async function loadDropdowns() {
    console.log("loadDropdowns running...");

    try {
        // ۱. بارگذاری لیست پرونده‌ها
        const casesResponse = await AuthManager.apiCall('/CourtSessions/GetForDropdown', { method: 'GET' });
        if (casesResponse.ok) {
            const casesData = await casesResponse.json();
            const caseSelect = document.getElementById('caseId');
            caseSelect.innerHTML = '<option value="">انتخاب پرونده...</option>';

            casesData.forEach(c => {
                caseSelect.innerHTML += `<option value="${c.id || c.Id}">${c.text || c.Text || c.caseNumber || 'پرونده ' + (c.id || c.Id)}</option>`;
            });
        }

        // ۲. بارگذاری لیست وکلا
        const lawyersResponse = await AuthManager.apiCall('/CourtSessions/GetLawyers', { method: 'GET' });
        if (lawyersResponse.ok) {
            const lawyersData = await lawyersResponse.json();
            const lawyerSelect = document.getElementById('lawyerId');
            lawyerSelect.innerHTML = '<option value="">انتخاب وکیل...</option>';

            lawyersData.forEach(l => {
                lawyerSelect.innerHTML += `<option value="${l.Id}">${l.FullName} </option>`;
            });
        }

        // بخش Select2 کاملا حذف شد تا از <select> استاندارد بوت‌استرپ استفاده شود

    } catch (error) {
        console.error("خطا در بارگذاری اطلاعات دراپ‌داون‌ها:", error);
    }
}


function renderSessions(sessions) {
    const sessionsTableBody = document.getElementById('courtSessionsTableBody');
    if (!sessionsTableBody) return;

    sessionsTableBody.innerHTML = ''; // Clear previous data
    if (!sessions || sessions.length === 0) {
        sessionsTableBody.innerHTML = `<tr><td colspan="8" class="text-center">هیچ جلسه‌ای یافت نشد.</td></tr>`;
        return;
    }

    sessions.forEach(session => {
        const statusInfo = getStatusInfo(session.Status);
        const rowHtml = `
            <tr>
                <td>${session.CaseCode || '---'}</td>
                <td>${session.ClientName || '---'}</td>
                <td>${session.LawyerName || '---'}</td>
                <td>${session.SessionDate || '---'}</td>
                <td>${session.SessionTime || '---'}</td>
                <td>${session.CourtName} / ${session.Branch || '---'}</td>
                <td><span class="badge bg-${statusInfo.color}">${statusInfo.text}</span></td>
                <td>
                    <button class="btn btn-sm btn-outline-info" onclick="openEditModal(${session.Id})">
                        <i class="bi bi-pencil"></i> ویرایش
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteSession(${session.Id})">
                        <i class="bi bi-trash"></i> حذف
                    </button>
                </td>
            </tr>`;
        sessionsTableBody.insertAdjacentHTML('beforeend', rowHtml);
    });
}

/**
 * Converts a status code to a displayable badge object.
 * @param {number} status - The status code from the server.
 * @returns {{text: string, color: string}}
 */
function getStatusInfo(status) {
    const statuses = {
        1: { text: 'برنامه‌ریزی شده', color: 'primary' },
        2: { text: 'برگزار شده', color: 'success' },
        4: { text: 'لغو شده', color: 'warning' },
        3: { text: 'تجدید شده', color: 'info' }
    };
    return statuses[status] || { text: 'نامشخص', color: 'secondary' };
}


// --- Modal and Form Handling ---

/**
 * Opens the modal for creating a new session.
 */
window.openModal = function () {
    document.getElementById('courtSessionForm').reset();
    document.getElementById('sessionId').value = '0';
    document.getElementById('modalTitle').textContent = 'افزودن جلسه جدید';

    // Reset Select2 fields
    $('#caseId').val(null).trigger('change');
    $('#lawyerId').val(null).trigger('change');

    const modal = new bootstrap.Modal(document.getElementById('courtSessionModal'));
    modal.show();
}

/**
 * Opens the modal to edit an existing session, loading its data first.
 * @param {number} sessionId - The ID of the session to edit.
 */
window.openEditModal = async function (sessionId) {
    try {
        const response = await AuthManager.apiCall(`/CourtSessions/GetById/${sessionId}`);

        if (!response.ok) {
            throw new Error(`خطا در دریافت اطلاعات جلسه: ${response.status}`);
        }

        // ✅ اصلاح: parse امن
        let session;
        try {
            const text = await response.text();
            if (!text || text.trim().length === 0) {
                throw new Error('پاسخ سرور خالی است.');
            }
            session = JSON.parse(text);
        } catch (parseErr) {
            console.error('JSON parse error on edit load:', parseErr);
            throw new Error('پاسخ سرور قابل پردازش نیست.');
        }

        // Fill form
        document.getElementById('sessionId').value = session.id || session.Id || '';
        document.getElementById('sessionDate').value = convertToShamsi(session.sessionDate) || convertToShamsi(session.SessionDate) || '';
        document.getElementById('sessionTime').value = session.sessionTime || session.SessionTime || '';
        document.getElementById('courtName').value = session.courtName || session.CourtName || '';
        document.getElementById('branch').value = session.branch || session.Branch || '';
        document.getElementById('notes').value = session.notes || session.Notes || '';

        // Select2 — Case
        const caseVal = session.caseId || session.CaseId;
        const caseTitle = session.caseTitle || session.CaseTitle || '';
        if (caseVal) {
            if ($('#caseId').find(`option[value='${caseVal}']`).length === 0) {
                const newOption = new Option(caseTitle, caseVal, true, true);
                $('#caseId').append(newOption);
            }
            $('#caseId').val(caseVal).trigger('change');
        }

        // Select2 — Lawyer
        const lawyerVal = session.lawyerId || session.LawyerId;
        const lawyerName = session.lawyerName || session.LawyerName || '';
        if (lawyerVal) {
            if ($('#lawyerId').find(`option[value='${lawyerVal}']`).length === 0) {
                const newOption = new Option(lawyerName, lawyerVal, true, true);
                $('#lawyerId').append(newOption);
            }
            $('#lawyerId').val(lawyerVal).trigger('change');
        }

        document.getElementById('modalTitle').textContent = 'ویرایش جلسه دادگاه';
        $('#courtSessionModal').modal('show');

    } catch (error) {
        console.error('Error opening edit modal:', error);
        Swal.fire('خطا', error.message, 'error');
    }
};

/**
 * Saves a new or existing session based on the form data.
 */
async function saveSession() {
    try {
        const sessionId = document.getElementById('sessionId')?.value;
        const isCreating = !sessionId || sessionId === '0';

        const sessionData = {
            id: parseInt(sessionId) || 0,
            caseId: parseInt($('#caseId').val()) || 0,
            lawyerId: parseInt($('#lawyerId').val()) || 0,
            sessionDate: document.getElementById('sessionDate')?.value || '',
            sessionTime: document.getElementById('sessionTime')?.value || '',
            courtName: document.getElementById('courtName')?.value || '',
            branch: document.getElementById('branch')?.value || '',
            notes: document.getElementById('notes')?.value || ''
        };

        // Validation
        if (!sessionData.caseId || !sessionData.sessionDate || !sessionData.courtName) {
            Swal.fire('خطا', 'لطفاً فیلدهای ضروری را پر کنید.', 'warning');
            return;
        }

        const url = isCreating ? '/CourtSessions/Create' : `/CourtSessions/Edit/${sessionData.id}`;

        const response = await AuthManager.apiCall(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(sessionData)
        });

        // ✅ اصلاح: ابتدا بررسی وضعیت، سپس parse امن JSON
        if (!response.ok) {
            let errorMsg = `خطای سرور: ${response.status}`;
            try {
                const errData = await response.json();
                if (errData.message) errorMsg = errData.message;
            } catch (_) { /* body is not JSON, use default message */ }
            throw new Error(errorMsg);
        }

        // ✅ اصلاح: parse امن JSON
        let result;
        try {
            const text = await response.text();
            if (!text || text.trim().length === 0) {
                throw new Error('پاسخ سرور خالی است.');
            }
            result = JSON.parse(text);
        } catch (parseErr) {
            console.error('JSON parse error:', parseErr);
            throw new Error('پاسخ سرور قابل پردازش نیست.');
        }

        if (result.success) {
            Swal.fire('موفق', result.message || 'عملیات با موفقیت انجام شد.', 'success');
            $('#courtSessionModal').modal('hide');
            await loadCourtSessions();
        } else {
            throw new Error(result.message || 'خطا در ذخیره اطلاعات جلسه.');
        }

    } catch (error) {
        console.error('Error saving session:', error);
        Swal.fire('خطا', error.message, 'error');
    }
}

/**
 * Deletes a session after user confirmation.
 * @param {number} sessionId - The ID of the session to delete.
 */
window.deleteSession = function (sessionId) {
    Swal.fire({
        title: 'آیا از حذف این جلسه مطمئن هستید؟',
        text: "این عمل قابل بازگشت نخواهد بود!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'بله، حذف کن!',
        cancelButtonText: 'انصراف'
    }).then(async (result) => {
        if (result.isConfirmed) {
            try {
                const response = await AuthManager.apiCall(`/CourtSessions/Delete/${sessionId}`, { method: 'DELETE' });

                if (!response.ok) {
                    throw new Error('خطا در حذف جلسه');
                }

                const resJson = await response.json();

                if (resJson.success) {
                    Swal.fire('حذف شد!', resJson.message, 'success');
                    loadCourtSessions();
                } else {
                    throw new Error(resJson.message || 'خطا در حذف جلسه.');
                }
            } catch (error) {
                console.error(`Error deleting session ${sessionId}:`, error);
                Swal.fire('خطا', error.message, 'error');
            }
        }
    });
}

//function convertToShamsi(dateString) {
//    if (!dateString) return '';

//    const date = new Date(dateString);

//    // بررسی معتبر بودن تاریخ
//    if (isNaN(date.getTime())) return dateString;

//    // فرمت کردن به شمسی
//    const options = {
//        year: 'numeric',
//        month: 'long', // می‌توانید '2-digit' بگذارید تا عدد نمایش دهد (مثلا 02)
//        day: 'numeric'

//    };

//    return new Intl.DateTimeFormat('fa-IR', options).format(date);
//}

function convertToShamsi(dateString) {
    if (!dateString) return '';

    const date = new Date(dateString);
    if (isNaN(date.getTime())) return dateString;

    const shamsiDate = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    }).format(date);

    // خروجی: 1405/02/15
    return shamsiDate;
}
