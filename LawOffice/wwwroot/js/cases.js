let casesData = [];
let clientsList = [];
let lawyersList = [];

document.addEventListener('DOMContentLoaded', function () {
    loadCases();
    loadCaseStatuses();
    loadClientsForDropdown();
    loadLawyersForDropdown();
    loadCaseTypesForDropdown();
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
// تابع برای لود کردن تخصص‌ها از دیتابیس
async function loadCaseTypesForDropdown() {
    try {
        const response = await AuthManager.apiCall('/Cases/GetCaseTypes');
        if (!response.ok) return;
        const types = await response.json();
        const select = document.getElementById('caseTypeId');

        if (select) {
            select.innerHTML = '<option value="">انتخاب کنید...</option>';
            types.forEach(t => {
                select.innerHTML += `<option value="${t.Id}">${t.PersianTitle} -${t.Perfix}</option>`;
            });
        }
    } catch (e) {
        console.error('Error loading case types:', e);
    }
}
async function loadCases() {
    try {
        const response = await AuthManager.apiCall('/Cases/GetAll');
        if (!response.ok) throw new Error('Network response was not ok');
        const cases = await response.json();
        const tbody = document.querySelector('#casesTable tbody');
        if (!tbody) return;

        tbody.innerHTML = '';
        cases.forEach(caseItem => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
               <td>${caseItem.CaseCode || '-'}</td>
                <td>${caseItem.Title}</td>
            <td>${caseItem.CaseTypeTitle}</td>
                <td>${caseItem.ClientName || '-'}</td>
                <td>${caseItem.LawyerName || '-'}</td>
                  <td>${getStatusBadge(caseItem.CaseStatusId)}</td>
                <td>
                
                    <button class="btn btn-sm btn-primary" title="مشاهده" onclick="viewCase(${caseItem.Id})" >  <i class="bi bi-card-checklist"></i>  </button>
                    <button class="btn btn-sm btn-warning" title="ویرایش" onclick="editCase(${caseItem.Id})"><i class="bi bi-pencil-square"></i>  </button> 
                                <button class="btn btn-sm btn-info" onclick="openUploadModal(${caseItem.Id})" title="آپلود فایل">
                <i class="bi bi-upload"></i>
            </button>
            <button class="btn btn-sm btn-success" onclick="PaymentManager.openModal(${caseItem.Id}, ${caseItem.ClientId})" title="ثبت پرداختی">
    <i class="bi bi-cash"></i>
</button>

            <button class="btn btn-sm btn-info" onclick="printContract(${caseItem.Id})" title="چاپ قرارداد">
    <i class="bi bi-printer"></i> 
</button>
            <button class="btn btn-sm btn-secondary" onclick="openDocumentsModal(${caseItem.Id})" title="مشاهده فایل‌ها">
               <i class="bi bi-folder2-open"></i>

            </button>

                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error) {
        console.error('Error loading cases:', error);
    }
}
function getStatusBadge(status) {
    switch (status) {
        case 1:
        case 'Active':
            return '<span class="badge bg-success">فعال</span>';
        case 2:
        case 'Pending':
            return '<span class="badge bg-warning text-dark">در انتظار</span>';
        case 3:
        case 'Closed':
            return '<span class="badge bg-secondary">بسته شده</span>';
        case 4:
        case 'Archived':
            return '<span class="badge bg-dark">آرشیو</span>';
        default:
            return '<span class="badge bg-info">-</span>';
    }
}
async function loadClientsForDropdown() {
    try {
        const response = await AuthManager.apiCall('/Clients/GetAll');
        const result = await response.json();

        if (result.success) {
            clientsList = result.data;
            const select = document.getElementById('clientId');
            select.innerHTML = '<option value="">انتخاب موکل</option>' +
                clientsList.map(c => `<option value="${c.Id}">${c.FirstName} ${c.LastName}</option>`).join('');
        }
    } catch (error) {
        console.error('Error loading clients:', error);
    }
}

async function loadLawyersForDropdown() {
    try {
        const response = await AuthManager.apiCall('/Lawyers/GetAll');
        const result = await response.json();

            lawyersList = result.data;
            const select = document.getElementById('lawyerId');
            select.innerHTML = '<option value="">انتخاب وکیل</option>' +
                lawyersList.map(l => `<option value="${l.Id}">${l.FirstName} ${l.LastName}</option>`).join('');
       
    } catch (error) {
        console.error('Error loading lawyers:', error);
    }
}


function openCaseModal(caseId = null) {
    const modal = document.getElementById('caseModal');
    const form = document.getElementById('caseForm');
    const title = document.getElementById('caseModalTitle');

    form.reset();
    document.getElementById('caseId').value = '';

    if (caseId) {
        title.textContent = 'ویرایش پرونده';
        loadCaseData(caseId);
    } else {
        title.textContent = 'پرونده جدید';
    }
}
async function loadCaseStatuses() {
    try {
        // فرض بر این است که از AuthManager یا fetch استفاده می‌کنید
        const response = await fetch('/Cases/GetStatuses');
        const statuses = await response.json();

        const statusSelect = document.getElementById('caseStatusId');
        if (!statusSelect) return; 

        statusSelect.innerHTML = '<option value="">انتخاب وضعیت...</option>';

        // اگر response شما داخل فیلد data است (result.data) کد را متناسب با آن تغییر دهید
        const statusList = statuses.data ? statuses.data : statuses;

        statusList.forEach(status => {
            const option = document.createElement('option');
            option.value = status.id;
            // استفاده از persianTitle برای نمایش و name برای دیتا در صورت نیاز
            option.textContent = status.persianTitle;
            statusSelect.appendChild(option);
        });
    } catch (error) {
        console.error('خطا در بارگذاری وضعیت‌های پرونده:', error);
    }
}
async function loadCaseData(caseId) {
    try {
        const response = await AuthManager.apiCall(`/Cases/Get/${caseId}`);
        const result = await response.json();

        if (result) {
            const caseData = result;
            document.getElementById('caseId').value = caseData.Id;
            document.getElementById('title').value = caseData.Title;
            document.getElementById('caseTypeId').value = caseData.CaseTypeId;
            document.getElementById('clientId').value = caseData.ClientId;
            document.getElementById('lawyerId').value = caseData.LawyerId;
            document.getElementById('courtName').value = caseData.CourtName || '';
            document.getElementById('courtBranch').value = caseData.CourtBranch || '';
            document.getElementById('courtCaseNumber').value = caseData.CourtCaseNumber || '';
            document.getElementById('openDate').value = caseData.OpenDate;
            document.getElementById('caseStatusId').value = caseData.CaseStatusId;
            document.getElementById('description').value = caseData.Description || '';
            document.getElementById('totalFee').value = caseData.TotalFee || 0;
        }
    } catch (error) {
        console.error('Error loading case:', error);
        showError('خطا در بارگذاری اطلاعات پرونده');
    }
}

async function saveCase() {
    const form = document.getElementById('caseForm');
    if (!form.checkValidity()) {
        form.reportValidity();
        return;
    }

    const caseId = document.getElementById('caseId').value;

    const data = {
        id: caseId ? parseInt(caseId) : 0,
        title: document.getElementById('title').value,
        caseTypeId: parseInt(document.getElementById('caseTypeId').value),
        clientId: parseInt(document.getElementById('clientId').value),
        lawyerId: parseInt(document.getElementById('lawyerId').value),
        courtName: document.getElementById('courtName').value || null,
        courtBranch: document.getElementById('courtBranch').value || null,
        courtCaseNumber: document.getElementById('courtCaseNumber').value || null,
        openDate: document.getElementById('openDate').value || null,
        caseStatusId: parseInt(document.getElementById('caseStatusId').value),
        description: document.getElementById('description').value || null,
        totalFee:parseInt(document.getElementById('totalFee').value )

    };

    try {
        const url = caseId ? '/Cases/Update' : '/Cases/Create';
        const response = await AuthManager.apiCall(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (result) {
            showSuccess( result);
            bootstrap.Modal.getInstance(document.getElementById('caseModal')).hide();
            loadCases();
        } else {
            showError(result.message || 'خطا در ذخیره اطلاعات');
        }
    } catch (error) {
        console.error('Error saving case:', error);
        showError('خطا در برقراری ارتباط با سرور');
    }
}

function editCase(caseId) {
    openCaseModal(caseId);
    const modal = new bootstrap.Modal(document.getElementById('caseModal'));
    modal.show();
}

async function deleteCase(caseId) {
    if (!confirm('آیا از حذف این پرونده اطمینان دارید؟')) {
        return;
    }

    try {
        const response = await AuthManager.apiCall(`/Cases/Delete/${caseId}`, {
            method: 'POST'
        });

        const result = await response.json();

        if (result.success) {
            showSuccess('پرونده با موفقیت حذف شد');
            loadCases();
        } else {
            showError(result.message || 'خطا در حذف پرونده');
        }
    } catch (error) {
        console.error('Error deleting case:', error);
        showError('خطا در برقراری ارتباط با سرور');
    }
}

function viewCase(caseId) {
    window.location.href = `/Cases/Details/${caseId}`;
}

function manageDocs(caseId) {
    window.location.href = `/Cases/Documents/${caseId}`;
}

function showSuccess(message) {
     ToastManager.success(message);
}

function showError(message) {
    ToastManager.error(message);
}
// ==========================================
// توابع مدیریت فایل‌ها
// ==========================================
// باز کردن مودال آپلود
function openUploadModal(caseId) {
    document.getElementById('uploadFileForm').reset();
    document.getElementById('uploadCaseId').value = caseId;
    $('#uploadFileModal').modal('show');
}
// ارسال فایل به سرور
async function submitUploadFile() {
    const form = document.getElementById('uploadFileForm');
    if (!form.checkValidity()) {
        form.reportValidity();
        return;
    }


    const formData = new FormData(form);
    const caseIdValue = document.getElementById('uploadCaseId').value;
    if (!formData.has('CaseId')) {
        formData.append('CaseId', caseIdValue);
    }

    try {
        const response = await fetch('/Cases/UploadDocument', {
            method: 'POST',
            body: formData // در ارسال فایل نباید Content-Type را به صورت دستی ست کنید
        });

        const result = await response.json();

        if (response.ok) {
            ToastManager.success(result.message);
            $('#uploadFileModal').modal('hide');
        } else {
            ToastManager.error(result.message || "خطا در آپلود فایل.");
        }
    } catch (error) {
        console.error("Upload error:", error);
        ToastManager.error("خطای ارتباط با سرور.");
    }
}

// باز کردن لیست فایل‌ها
let currentCaseDocsId = 0;
async function openDocumentsModal(caseId) {
    currentCaseDocsId = caseId;
    await loadDocuments(caseId);
    $('#caseDocumentsModal').modal('show');
}

// لود کردن فایل‌ها از سرور
async function loadDocuments(caseId) {
    const tbody = document.getElementById('documentsTableBody');
    tbody.innerHTML = '<tr><td colspan="5" class="text-center">در حال بارگذاری...</td></tr>';

    try {
        const response = await fetch(`/Cases/GetDocuments?caseId=${caseId}`);
        const result = await response.json();

        tbody.innerHTML = '';

        if (result.data && result.data.length > 0) {
            result.data.forEach(doc => {
                const tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${doc.FileName}</td>
                    <td>${doc.Description || '-'}</td>
                    <td dir="ltr">${doc.FileSize} KB</td>
                    <td dir="ltr">${doc.CreatedAt}</td>
                    <td>
                        <a href="${doc.RelativePath}" download="${doc.FileName}" target="_blank" class="btn btn-sm btn-primary" title="دانلود">
                            <i class="bi bi-download"></i>
                        </a>
                        <button class="btn btn-sm btn-danger" onclick="deleteDocument(${doc.Id})" title="حذف">
                    <i class="bi bi-trash"></i>
                        </button>
                    </td>
                `;
                tbody.appendChild(tr);
            });
        } else {
            tbody.innerHTML = '<tr><td colspan="5" class="text-center text-muted">هیچ فایلی برای این پرونده یافت نشد.</td></tr>';
        }
    } catch (error) {
        tbody.innerHTML = '<tr><td colspan="5" class="text-center text-danger">خطا در دریافت لیست فایل‌ها.</td></tr>';
    }
}
// حذف فایل
async function deleteDocument(docId) {
    if (!confirm('آیا از حذف این فایل اطمینان دارید؟ این عمل غیرقابل بازگشت است.')) return;

    try {
        const response = await fetch(`/Cases/DeleteDocument/${docId}`, {
            method: 'POST'
        });

        const result = await response.json();

        if (response.ok) {
            ToastManager.success(result.message);
            // رفرش کردن جدول فایل‌ها
            loadDocuments(currentCaseDocsId);
        } else {
            ToastManager.error(result.message || "خطا در حذف فایل");
        }
    } catch (error) {
        ToastManager.error("خطای ارتباط با سرور");
    }
}
function printContract(caseId) {
    window.open(`/PrintContract/${caseId}`, '_blank');
}

// --- مدیریت پرداخت‌های مالی پرونده ---
const PaymentManager = {
    // باز کردن مودال و مقداردهی اولیه
    openModal: function (caseId, clientId) {
        document.getElementById('paymentForm').reset();

        // تنظیم شناسه‌های پرونده و موکل
        document.getElementById('pay_caseId').value = caseId;
        document.getElementById('pay_clientId').value = clientId || '';

        // ریست کردن ظاهر فیلدها
        this.toggleChequeFields();
        this.loadCasePayments(caseId);

        // نمایش مودال
        const modal = new bootstrap.Modal(document.getElementById('paymentModal'));
        modal.show();

        // فعال‌سازی تقویم شمسی در صورت نیاز به مقداردهی مجدد
        if (typeof jalaliDatepicker !== 'undefined') {
            jalaliDatepicker.startWatch();
        }
    },

    // نمایش/مخفی کردن فیلدهای مربوط به چک و حواله
    toggleChequeFields: function () {
        const method = document.getElementById('pay_method').value;
        const chequeFields = document.getElementById('chequeDetails');

        // اگر چک (3) یا حواله (2) انتخاب شد، فیلد شماره پیگیری نمایش داده شود
        if (method === "3" || method === "2") {
            chequeFields.style.display = 'block';
        } else {
            chequeFields.style.display = 'none';
            document.getElementById('pay_referenceNumber').value = ''; // پاک کردن مقدار
        }
    },

    loadCasePayments: async function (caseId) {
        const tbody = document.getElementById('casePaymentsList');
        if (!tbody) return;

        tbody.innerHTML = '<tr><td colspan="5" class="text-center">در حال بارگذاری...</td></tr>';

        try {
            const fetchResponse = await AuthManager.apiCall(`/Financials/GetAll?caseId=${caseId}`, { method: 'GET' });

            // بررسی وضعیت موفقیت‌آمیز بودن درخواست HTTP
            if (fetchResponse && fetchResponse.ok) {

                // تبدیل پاسخ خام به JSON
                const response = await fetchResponse.json();

                if (response.success && response.data && response.data.length > 0) {
                    tbody.innerHTML = '';
                    response.data.forEach(payment => {
                        let chequeStatusObj = payment.ChequeStatus;
                        let statusText = chequeStatusObj != null ? (chequeStatusObj === 0 ? "در انتظار" : chequeStatusObj === 1 ? "پاس شده" : "برگشتی") : "-";

                        tbody.innerHTML += `
                            <tr>
                                <td>${payment.TransactionDate || '-'}</td>
                                <td>${payment.Amount.toLocaleString()}</td>
                                <td>${payment.PaymentMethod === 2 ? 'چک' : (payment.PaymentMethod === 1 ? 'کارت به کارت' : 'نقدی')}</td>
                                <td>${payment.BankName || '-'} / ${payment.ReferenceNumber || '-'}</td>
                                <td>${payment.PaymentMethod === 2 ? statusText : '-'}</td>
                            </tr>
                        `;
                    });
                } else {
                    tbody.innerHTML = '<tr><td colspan="5" class="text-center text-muted">هیچ پرداختی برای این پرونده ثبت نشده است.</td></tr>';
                }
            } else {
                tbody.innerHTML = '<tr><td colspan="5" class="text-center text-danger">خطا در دریافت اطلاعات از سرور.</td></tr>';
            }
        } catch (error) {
            console.error("Error loading payments:", error);
            tbody.innerHTML = '<tr><td colspan="5" class="text-center text-danger">خطا در پردازش اطلاعات.</td></tr>';
        }
    },

    // ارسال اطلاعات به سرور
    save: async function () {
        const form = document.getElementById('paymentForm');
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        const methodValue = document.getElementById('pay_method').value;
        // اصلاح خطای دوم: تعریف متغیر isCheque (فرض بر این است که متد 2 مربوط به چک است)
        const isCheque = (methodValue === "2");

        const payload = {
            CaseId: parseInt(document.getElementById('pay_caseId').value),
            ClientId: parseInt(document.getElementById('pay_clientId').value),
            Amount: parseFloat(document.getElementById('pay_amount').value),
            PaymentMethod: parseInt(methodValue),
            TransactionDateShamsi: document.getElementById('pay_transactionDate').value || null,
            Description: document.getElementById('pay_description').value || null,
            ReferenceNumber: document.getElementById('pay_referenceNumber').value || null,
            Type: 1, // TransactionType.Income (درآمد/حق‌الوکاله)
            bankName: isCheque ? document.getElementById('pay_bankName').value : null, // ارسال نام بانک
            dueDateShamsi: isCheque ? document.getElementById('pay_dueDate').value : null
        };

        try {
            const response = await AuthManager.apiCall('/Financials/AddPayment', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            // اگر apiCall خروجی را parse می‌کند، نیازی به response.json() نیست، اما اگر آبجکت Response برمی‌گرداند خط زیر صحیح است:
            const result = await response.json();

            // بررسی موفقیت‌آمیز بودن عملیات
            if (result && result.success !== false) {
                showSuccess(result.message || 'پرداخت با موفقیت ثبت شد.');
                bootstrap.Modal.getInstance(document.getElementById('paymentModal')).hide();

                // بروزرسانی جدول پرداختی‌ها
                this.loadCasePayments(payload.CaseId);
            } else {
                showError(result.message || 'خطا در ثبت اطلاعات مالی.');
            }
        } catch (error) {
            console.error('Error saving payment:', error);
            showError('خطا در برقراری ارتباط با سرور');
        }
    }
};
