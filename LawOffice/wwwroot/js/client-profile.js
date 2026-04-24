const ClientProfile = (() => {
    let clientId = null;
    let clientData = null;
    const editModal = () => new bootstrap.Modal(document.getElementById('editModal'));

    async function init() {
        const params = new URLSearchParams(window.location.search);
        clientId = params.get('id');
        if (!clientId) {
            ToastManager.error('شناسه موکل نامعتبر است');
            setTimeout(() => window.location.href = '/Clients', 2000);
            return;
        }
        await loadClient();
        await loadCases();
        await loadAppointments();
        initPersianDates();
    }

    async function loadClient() {
        try {
            const res = await AuthManager.fetch(`/api/clients/${clientId}`);
            if (!res.ok) {
                ToastManager.error('موکل یافت نشد');
                setTimeout(() => window.location.href = '/Clients', 2000);
                return;
            }
            clientData = await res.json();
            renderClientInfo();
        } catch {
            ToastManager.error('خطا در بارگذاری اطلاعات');
        }
    }

    function renderClientInfo() {
        const c = clientData;
        const fullName = `${c.firstName} ${c.lastName}`;

        document.getElementById('clientName').textContent = fullName;
        document.getElementById('clientFullName').textContent = fullName;
        document.getElementById('clientAvatar').textContent = c.firstName.charAt(0);
        document.getElementById('clientNationalId').textContent = `کد ملی: ${c.nationalId}`;

        const educationMap = {
            'Diploma': 'دیپلم',
            'Associate': 'کاردانی',
            'Bachelor': 'کارشناسی',
            'Master': 'کارشناسی ارشد',
            'PhD': 'دکتری'
        };

        document.getElementById('clientDetails').innerHTML = `
            <div class="border-top pt-3">
                ${c.phone ? `
                <div class="d-flex justify-content-between mb-2">
                    <span class="text-muted small"><i class="bi bi-phone me-1"></i>تلفن</span>
                    <span class="small">${c.phone}</span>
                </div>` : ''}
                ${c.email ? `
                <div class="d-flex justify-content-between mb-2">
                    <span class="text-muted small"><i class="bi bi-envelope me-1"></i>ایمیل</span>
                    <span class="small">${c.email}</span>
                </div>` : ''}
                ${c.birthDatePersian ? `
                <div class="d-flex justify-content-between mb-2">
                    <span class="text-muted small"><i class="bi bi-calendar me-1"></i>تاریخ تولد</span>
                    <span class="small">${c.birthDatePersian}</span>
                </div>` : ''}
                ${c.education ? `
                <div class="d-flex justify-content-between mb-2">
                    <span class="text-muted small"><i class="bi bi-mortarboard me-1"></i>تحصیلات</span>
                    <span class="small">${educationMap[c.education] || c.education}</span>
                </div>` : ''}
                ${c.address ? `
                <div class="mt-2">
                    <span class="text-muted small d-block mb-1"><i class="bi bi-geo-alt me-1"></i>آدرس</span>
                    <span class="small">${c.address}</span>
                </div>` : ''}
            </div>
        `;

        renderStats(c);
    }

    function renderStats(c) {
        document.getElementById('clientStats').innerHTML = `
            <div class="row g-2 text-center">
                <div class="col-6">
                    <div class="bg-primary-subtle rounded p-2">
                        <div class="fw-bold text-primary fs-5">${c.totalCases ?? 0}</div>
                        <small class="text-muted">پرونده</small>
                    </div>
                </div>
                <div class="col-6">
                    <div class="bg-success-subtle rounded p-2">
                        <div class="fw-bold text-success fs-5">${c.activeCases ?? 0}</div>
                        <small class="text-muted">فعال</small>
                    </div>
                </div>
                <div class="col-6">
                    <div class="bg-info-subtle rounded p-2">
                        <div class="fw-bold text-info fs-5">${c.totalAppointments ?? 0}</div>
                        <small class="text-muted">نوبت</small>
                    </div>
                </div>
                <div class="col-6">
                    <div class="bg-warning-subtle rounded p-2">
                        <div class="fw-bold text-warning fs-5">${c.upcomingAppointments ?? 0}</div>
                        <small class="text-muted">آینده</small>
                    </div>
                </div>
            </div>
        `;
    }

    async function loadCases() {
        try {
            const res = await AuthManager.fetch(`/api/cases?clientId=${clientId}`);
            if (!res.ok) return;
            const cases = await res.json();
            document.getElementById('caseCount').textContent = cases.length;
            renderCases(cases);
        } catch { }
    }

    function renderCases(cases) {
        const container = document.getElementById('casesList');
        if (!cases.length) {
            container.innerHTML = `
                <div class="text-center py-4 text-muted">
                    <i class="bi bi-folder-x fs-2 d-block mb-2"></i>
                    پرونده‌ای ثبت نشده
                </div>`;
            return;
        }

        const statusMap = {
            'Open': { label: 'فعال', cls: 'bg-success' },
            'Closed': { label: 'بسته شده', cls: 'bg-secondary' },
            'Pending': { label: 'در انتظار', cls: 'bg-warning text-dark' }
        };

        const typeMap = {
            'Civil': 'حقوقی',
            'Criminal': 'کیفری',
            'Family': 'خانواده',
            'Commercial': 'تجاری',
            'Administrative': 'اداری',
            'Other': 'سایر'
        };

        container.innerHTML = `
            <div class="list-group list-group-flush">
                ${cases.map(c => {
            const st = statusMap[c.status] || { label: c.status, cls: 'bg-secondary' };
            return `
                    <a href="/Cases/Detail?id=${c.id}" 
                       class="list-group-item list-group-item-action d-flex justify-content-between align-items-center">
                        <div>
                            <div class="fw-semibold">${c.caseCode}</div>
                            <small class="text-muted">${typeMap[c.caseType] || c.caseType} — ${c.filingDatePersian}</small>
                        </div>
                        <span class="badge ${st.cls}">${st.label}</span>
                    </a>`;
        }).join('')}
            </div>`;
    }

    async function loadAppointments() {
        try {
            const res = await AuthManager.fetch(`/api/appointments?clientId=${clientId}&upcoming=true`);
            if (!res.ok) return;
            const appointments = await res.json();
            document.getElementById('appointmentCount').textContent = appointments.length;
            renderAppointments(appointments);
        } catch { }
    }

    function renderAppointments(appointments) {
        const container = document.getElementById('appointmentsList');
        if (!appointments.length) {
            container.innerHTML = `
                <div class="text-center py-4 text-muted">
                    <i class="bi bi-calendar-x fs-2 d-block mb-2"></i>
                    نوبت آینده‌ای وجود ندارد
                </div>`;
            return;
        }

        container.innerHTML = `
            <div class="list-group list-group-flush">
                ${appointments.map(a => `
                <div class="list-group-item d-flex justify-content-between align-items-center">
                    <div>
                        <div class="fw-semibold">${a.appointmentDatePersian}</div>
                        <small class="text-muted">
                            <i class="bi bi-clock me-1"></i>${a.startTime} - ${a.endTime}
                        </small>
                    </div>
                    <div class="text-end">
                        <small class="text-muted d-block">${a.lawyerName || ''}</small>
                        ${a.reminderSent
                ? '<span class="badge bg-success-subtle text-success"><i class="bi bi-bell-fill me-1"></i>یادآوری ارسال شد</span>'
                : '<span class="badge bg-secondary-subtle text-secondary">در انتظار یادآوری</span>'}
                    </div>
                </div>`).join('')}
            </div>`;
    }

    function openEditModal() {
        if (!clientData) return;
        const c = clientData;
        document.getElementById('clientId').value = c.id;
        document.getElementById('firstName').value = c.firstName;
        document.getElementById('lastName').value = c.lastName;
        document.getElementById('nationalId').value = c.nationalId;
        document.getElementById('birthDate').value = c.birthDatePersian || '';
        document.getElementById('phone').value = c.phone || '';
        document.getElementById('email').value = c.email || '';
        document.getElementById('education').value = c.education || '';
        document.getElementById('address').value = c.address || '';
        editModal().show();
    }

    async function saveEdit() {
        const form = document.getElementById('editForm');
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        const payload = {
            id: parseInt(document.getElementById('clientId').value),
            firstName: document.getElementById('firstName').value.trim(),
            lastName: document.getElementById('lastName').value.trim(),
            nationalId: document.getElementById('nationalId').value.trim(),
            birthDate: document.getElementById('birthDate').value.trim() || null,
            phone: document.getElementById('phone').value.trim() || null,
            email: document.getElementById('email').value.trim() || null,
            education: document.getElementById('education').value || null,
            address: document.getElementById('address').value.trim() || null
        };

        try {
            const res = await AuthManager.fetch(`/api/clients/${payload.id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            if (res.ok) {
                bootstrap.Modal.getInstance(document.getElementById('editModal')).hide();
                ToastManager.success('اطلاعات موکل ذخیره شد');
                await loadClient();
            } else {
                const err = await res.json();
                ToastManager.error(err.message || 'خطا در ذخیره اطلاعات');
            }
        } catch {
            ToastManager.error('خطا در ارتباط با سرور');
        }
    }

    function initPersianDates() {
        document.querySelectorAll('.persian-date').forEach(input => {
            input.addEventListener('input', e => {
                let val = e.target.value.replace(/[^\d/]/g, '');
                e.target.value = val;
            });
        });
    }

    document.addEventListener('DOMContentLoaded', init);

    return { openEditModal, saveEdit };
})();
