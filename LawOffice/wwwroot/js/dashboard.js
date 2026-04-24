document.addEventListener('DOMContentLoaded', () => {
    // ۱. بررسی لاگین بودن کاربر از طریق AuthManager
    if (typeof AuthManager === 'undefined' || !AuthManager.isAuthenticated()) {
        window.location.href = '/Auth/Login';
        return;
    }

    // ۲. فراخوانی توابع برای بارگذاری داده‌های داشبورد
    loadDashboardStats();
    loadRecentActivities();
    loadUpcomingEvents();
});

// --------------------------------------------------------
// تابع بارگذاری آمارهای بالای صفحه
// --------------------------------------------------------
async function loadDashboardStats() {
    try {
        const res = await AuthManager.apiCall('/Dashboard/GetStats');
        // اگر پاسخ 401 باشد، متد apiCall خودش کاربر را به لاگین هدایت می‌کند
        if (!res || !res.ok) return;

        const result = await res.json();
        if (result.success && result.data) {

            // پر کردن مقادیر پرونده‌های فعال و نوبت‌های امروز
            document.getElementById('stat-cases').textContent = result.data.activeCases || 0;
            document.getElementById('stat-appointments').textContent = result.data.todayAppointments || 0;

            // بررسی و نمایش تعداد موکلین (اگر مقدار برگردانده شده باشد)
            const clientsContainer = document.getElementById('stat-clients-container');
            if (result.data.totalClients !== undefined) {
                clientsContainer.style.display = 'block';
                document.getElementById('stat-clients').textContent = result.data.totalClients;
            } else if (clientsContainer) {
                clientsContainer.style.display = 'none';
            }

            // بررسی و نمایش جلسات آتی (مخفی برای نقش‌های غیرمجاز)
            const sessionsContainer = document.getElementById('stat-sessions-container');
            if (result.data.upcomingSessions !== undefined) {
                sessionsContainer.style.display = 'block';
                document.getElementById('stat-sessions').textContent = result.data.upcomingSessions;
            } else if (sessionsContainer) {
                sessionsContainer.style.display = 'none';
            }
        }
    } catch (err) {
        console.error('خطا در دریافت آمار داشبورد:', err);
    }
}

// --------------------------------------------------------
// تابع بارگذاری فعالیت‌های اخیر
// --------------------------------------------------------
async function loadRecentActivities() {
    const container = document.getElementById('recentActivities');

    try {
        const res = await AuthManager.apiCall('/Dashboard/GetRecentActivities');
        if (!res || !res.ok) {
            container.innerHTML = '<div class="text-center text-danger py-4">خطا در بارگذاری فعالیت‌ها</div>';
            return;
        }

        const result = await res.json();
        // پشتیبانی از هر دو ساختار احتمالی دیتا (data یا items)
        const items = result.data || result.items || [];

        if (items.length === 0) {
            container.innerHTML = '<div class="text-center text-muted py-4">هیچ فعالیتی یافت نشد.</div>';
            return;
        }

        // رندر کردن آیتم‌ها با استفاده از کلاس‌های قالب Boltz
        container.innerHTML = items.map(activity => `
            <div class="media mb-3 align-items-center pb-3 border-bottom">
                <div class="media-body">
                    <h5 class="mt-0 mb-1">${activity.title || activity.description || 'بدون عنوان'}</h5>
                <p class="mb-0 text-muted">${activity.date || convertToShamsi(activity.createdAt) || ''}</p>
                </div>
            </div>
        `).join('');

    } catch (err) {
        console.error('خطا در دریافت فعالیت‌های اخیر:', err);
        container.innerHTML = '<div class="text-center text-danger py-4">خطای سرور در دریافت اطلاعات</div>';
    }
}
function convertToShamsi(dateString) {
    if (!dateString) return '';

    const date = new Date(dateString);

    // بررسی معتبر بودن تاریخ
    if (isNaN(date.getTime())) return dateString;

    // فرمت کردن به شمسی
    const options = {
        year: 'numeric',
        month: 'long', // می‌توانید '2-digit' بگذارید تا عدد نمایش دهد (مثلا 02)
        day: 'numeric'
       
    };

    return new Intl.DateTimeFormat('fa-IR', options).format(date);
}
// --------------------------------------------------------
// تابع بارگذاری رویدادهای پیش رو (تایم‌لاین)
// --------------------------------------------------------
async function loadUpcomingEvents() {
    const container = document.getElementById('upcomingEvents');

    try {
        const res = await AuthManager.apiCall('/Dashboard/GetUpcomingEvents');
        if (!res || !res.ok) {
            container.innerHTML = '<div class="text-center text-danger py-4">خطا در بارگذاری رویدادها</div>';
            return;
        }

        const result = await res.json();
        const items = result.data || result.items || [];

        if (items.length === 0) {
            container.innerHTML = '<div class="text-center text-muted py-4">رویداد پیش‌رو وجود ندارد.</div>';
            return;
        }

        // رندر کردن آیتم‌ها در ساختار Timeline قالب Boltz
        container.innerHTML = `
            <ul class="timeline">
                ${items.map(event => `
                    <li>
                        <div class="timeline-panel">
                            <div class="media me-2 media-info">
                                <i class="flaticon-381-calendar-1"></i>
                            </div>
                            <div class="media-body">
                                <h5 class="mb-1">${event.title || 'رویداد سیستم'}</h5>
                                <small class="d-block text-muted">${ convertToShamsi(event.date)} - ${event.startTime || ''}</small>
                            </div>
                        </div>
                    </li>
                `).join('')}
            </ul>
        `;

    } catch (err) {
        console.error('خطا در دریافت رویدادهای پیش رو:', err);
        container.innerHTML = '<div class="text-center text-danger py-4">خطای سرور در دریافت اطلاعات</div>';
    }
}
