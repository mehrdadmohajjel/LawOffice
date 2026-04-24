const CaseDetail = (function () {
    let currentCaseId = 0;
    let noteModalInstance = null;

    function init() {
        // دریافت ID پرونده از مقدار hidden درون صفحه
        const idInput = document.getElementById('caseIdHidden');
        if (idInput && idInput.value) {
            currentCaseId = parseInt(idInput.value);
            noteModalInstance = new bootstrap.Modal(document.getElementById('noteModal'));
            loadCaseData();
        } else {
            ToastManager.error('شناسه پرونده یافت نشد.');
        }
    }

    function loadCaseData() {
        // فراخوانی اکشن Get از CasesController که قبلا در سیستم وجود داشت
        fetch(`/Cases/Get/${currentCaseId}`)
            .then(response => response.json())
            .then(data => {
                // با توجه به استاندارد قبلی پروژه‌تان، فرض بر این است که خروجی شامل data.success و data.data است
                if (data.success !== false && data.data) {
                    populateUI(data.data);
                } else if (data.Title || data.CaseCode) {
                    // در صورتی که ساختار خروجی مستقیم مدل باشد
                    populateUI(data);
                } else {
                    ToastManager.error(data.message || 'پرونده مورد نظر یافت نشد.');
                }
            })
            .catch(error => {
                console.error('Error fetching details:', error);
                ToastManager.error('خطای ارتباط با سرور در دریافت اطلاعات.');
            });
    }

    function populateUI(caseData) {
        // اطلاعات سرآیند
        document.getElementById('pageTitle').innerText = `جزئیات پرونده: ${caseData.Title || ''}`;

        // اطلاعات اصلی
        document.getElementById('lblCaseCode').innerText = caseData.CaseCode || '-';
        document.getElementById('lblTitle').innerText = caseData.Title || '-';
        document.getElementById('lblCaseType').innerText = caseData.CaseTypeTitle || '-';
        document.getElementById('lblStatus').innerText = caseData.CaseStatusTitle || '-';

        // تبدیل تاریخ‌ها (در صورت نیاز به شمسی، در بک‌اند یا با توابع JS)
        document.getElementById('lblOpenDate').innerText = caseData.OpenDate || '-';
        document.getElementById('lblCloseDate').innerText = caseData.CloseDate || 'باز است';
        document.getElementById('lblDescription').innerText = caseData.Description || 'توضیحاتی ثبت نشده است.';

        // اطلاعات دادگاه
        document.getElementById('lblCourtName').innerText = caseData.CourtName || '-';
        document.getElementById('lblCourtBranch').innerText = caseData.CourtBranch || '-';
        document.getElementById('lblCourtCaseNumber').innerText = caseData.CourtCaseNumber || '-';

        // اطلاعات اشخاص
        document.getElementById('lblClientName').innerText = caseData.ClientName || '-';
        document.getElementById('lblClientPhone').innerText = caseData.ClientPhoneNumber || '';
        document.getElementById('lblLawyerName').innerText = caseData.LawyerName || '-';
        document.getElementById('lblOpponentName').innerText = caseData.OpponentName || '-';
        document.getElementById('lblOpponentLawyer').innerText = caseData.OpponentLawyer || '-';

        // یادداشت‌ها
        renderNotes(caseData.CaseNotes || []);
    }

    function renderNotes(notes) {
        const container = document.getElementById('notesContainer');
        if (!container) return;

        // بررسی خالی بودن لیست
        if (!notes || notes.length === 0) {
            container.innerHTML = '<div class="alert alert-info text-center">یادداشتی برای این پرونده ثبت نشده است.</div>';
            return;
        }

        let html = '<div class="list-group list-group-flush">';
        notes.forEach(note => {
            html += `
            <div class="list-group-item py-3">
                <div class="d-flex justify-content-between align-items-center mb-2">
                    <h6 class="mb-0 fw-bold text-primary">${note.Title || 'بدون عنوان'}</h6>
                    <small class="text-muted bg-light px-2 py-1 rounded border">${note.CreatedAt || ''}</small>
                </div>
                <p class="mb-0 text-secondary" style="white-space: pre-line;">${note.Content || ''}</p>
            </div>`;
        });
        html += '</div>';
        container.innerHTML = html;
    }

    function openNoteModal() {
        document.getElementById('noteId').value = '0';
        document.getElementById('noteTitle').value = '';
        document.getElementById('noteContent').value = '';
        noteModalInstance.show();
    }

    function saveNote() {
        const title = document.getElementById('noteTitle').value;
        const content = document.getElementById('noteContent').value;

        if (!content) {
            ToastManager.error('وارد کردن متن یادداشت الزامی است.');
            return;
        }

        const payload = {
            id: 0,
            caseId: currentCaseId,
            title: title,
            content: content
        };

        fetch('/Cases/SaveNote', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    ToastManager.success('یادداشت با موفقیت ثبت شد.');
                    noteModalInstance.hide();
                    loadCaseData(); // بارگذاری مجدد برای نمایش یادداشت جدید
                } else {
                    ToastManager.error(data.message || 'خطا در ثبت یادداشت.');
                }
            })
            .catch(error => {
                console.error('Save Note Error:', error);
                ToastManager.error('خطای ارتباط با سرور.');
            });
    }

    return {
        init: init,
        openNoteModal: openNoteModal,
        saveNote: saveNote
    };
})();

// راه‌اندازی پس از بارگذاری کامل DOM
document.addEventListener('DOMContentLoaded', CaseDetail.init);
