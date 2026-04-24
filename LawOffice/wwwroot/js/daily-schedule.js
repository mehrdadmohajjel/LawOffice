document.addEventListener("DOMContentLoaded", function () {
    // مقداردهی دیت‌پیکر جلالی
    jalaliDatepicker.startWatch({
        time: false,
        hasTime: false
    });

    const dateInput = document.getElementById('scheduleDate');
    const scheduleContainer = document.getElementById('scheduleContainer');
    const btnToday = document.getElementById('btnToday');

    // مقداردهی تاریخ اولیه (امروز)
    const today = getTodayJalali();
    dateInput.value = today;

    // بارگذاری اولیه داده‌ها
    loadSchedule(today);

    // رویداد تغییر تاریخ در دیت‌پیکر
    dateInput.addEventListener("jdp:change", function () {
        loadSchedule(this.value);
    });

    // دکمه بازگشت به تاریخ امروز
    btnToday.addEventListener("click", function () {
        dateInput.value = getTodayJalali();
        loadSchedule(dateInput.value);
    });

    // جایگزین این تابع شوید
    function getTodayJalali() {
        // استفاده از API استاندارد مرورگر برای دریافت تاریخ شمسی
        // 'fa-IR-u-nu-latn' تضمین می‌کند که اعداد به صورت لاتین (1, 2, 3) باشند
        const today = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
        }).format(new Date());

        return today; // خروجی: "1405/02/01" (برای مثال)
    }

    async function loadSchedule(shamsiDate) {
        scheduleContainer.innerHTML = '<div class="text-center mt-5"><div class="spinner-border text-primary" role="status"></div><p>در حال بارگذاری...</p></div>';

        try {
            const response = await fetch(`/DailySchedule/GetDailySchedule?shamsiDate=${encodeURIComponent(shamsiDate)}`);
            if (!response.ok) throw new Error("خطا در دریافت اطلاعات");

            const data = await response.json();
            renderSchedule(data);
        } catch (error) {
            console.error(error);
            scheduleContainer.innerHTML = '<div class="alert alert-danger text-center">خطا در ارتباط با سرور.</div>';
        }
    }

    function renderSchedule(items) {
        scheduleContainer.innerHTML = '';

        if (!items || items.length === 0) {
            scheduleContainer.innerHTML = `
                <div class="col-12 mt-4 text-center">
                    <div class="alert alert-light border text-muted">
                        <i class="bi bi-inbox fs-1 d-block mb-2"></i>
                        هیچ برنامه‌ای (نوبت یا جلسه دادگاه) برای این تاریخ ثبت نشده است.
                    </div>
                </div>`;
            return;
        }

        items.forEach(item => {
            // تعیین استایل و آیکون بر اساس نوع رکورد
            const isCourt = item.type === "CourtSession";
            const iconClass = isCourt ? "bi-bank text-danger" : "bi-people text-primary";
            const badgeLabel = isCourt ? "جلسه دادگاه" : "نوبت ملاقات";
            const badgeClass = isCourt ? "bg-danger" : "bg-primary";

            // قالب HTML یک کارت ریسپانسیو
            const cardHtml = `
                <div class="col-12 col-md-6 col-lg-4 mb-3">
                    <div class="card h-100 shadow-sm border-0 border-start border-4 ${isCourt ? 'border-danger' : 'border-primary'}">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center mb-2">
                                <span class="badge ${badgeClass} bg-opacity-10 text-dark fw-bold px-2 py-1">
                                    <i class="bi ${iconClass} me-1"></i> ${badgeLabel}
                                </span>
                                <h5 class="mb-0 text-dark fw-bold" dir="ltr"><i class="bi bi-clock me-1"></i>${item.Time}</h5>
                            </div>
                            <h6 class="card-title fw-bold mt-3">${item.Title}</h6>
                            <hr class="text-muted" />
                            <div class="small text-muted mb-1">
                                <i class="bi bi-person me-1"></i> <strong>موکل:</strong> ${item.ClientName}
                            </div>
                            <div class="small text-muted mb-2">
                                <i class="bi bi-briefcase me-1"></i> <strong>وکیل:</strong> ${item.LawyerName}
                            </div>
                            ${item.Description ? `<p class="card-text small text-secondary bg-light p-2 rounded">${item.Description}</p>` : ''}
                        </div>
                    </div>
                </div>
            `;
            scheduleContainer.insertAdjacentHTML('beforeend', cardHtml);
        });
    }
});
