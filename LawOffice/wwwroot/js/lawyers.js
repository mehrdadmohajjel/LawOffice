// تعریف شیء مودال در محدوده سراسری فایل
let lawyerModal;

document.addEventListener('DOMContentLoaded', function () {
    // مقداردهی اولیه مودال بوت‌استرپ
    lawyerModal = new bootstrap.Modal(document.getElementById('lawyerModal'));

    // ۱. بارگذاری لیست وکلا
    loadLawyers();

    // ۲. راه‌اندازی Select2 روی فیلد حساب کاربری
    $('#userId').select2({
        dropdownParent: $('#lawyerModal'), // حل مشکل z-index در مودال بوت‌استرپ
        placeholder: 'جستجوی کاربر (نام، نام خانوادگی یا موبایل)...',
        allowClear: true,
        language: {
            noResults: function () {
                return "کاربری یافت نشد";
            }
        }
    });

    // ۳. دریافت لیست کاربران از سرور برای پر کردن کمبوباکس
    loadUsersForDropdown();
});

// تابع واکشی لیست کاربران (با استفاده از AuthManager)
async function loadUsersForDropdown() {
    try {
        const response = await AuthManager.apiCall('/Users/GetAll', { method: 'GET' });
        const result = await response.json();

        // بررسی ساختار پاسخ
        const users = result.data ? result.data : result;

        // اصلاح ۱: استفاده از UserId با U بزرگ دقیقاً مشابه HTML
        const userSelect = $('#UserId');
        userSelect.empty();
        userSelect.append(new Option('-- انتخاب کاربر --', '', true, true));

        users.forEach(user => {
            // اصلاح ۲: استفاده از Username با N کوچک مطابق با خروجی دیباگر
            const identifier = user.Username || user.Mobile || user.PhoneNumber || '';
            const fullName = `${user.FullName || ''} (${identifier})`.trim();

            // اصلاح ۳: استفاده از user.Id با I بزرگ مطابق با خروجی دیباگر
            const newOption = new Option(fullName, user.Id, false, false);
            userSelect.append(newOption);
        });

        // بروزرسانی ظاهر Select2
        userSelect.trigger('change');
    } catch (error) {
        console.error('Error fetching users:', error);
        ToastManager.error("خطا در دریافت لیست کاربران");
    }
}

// تابع بارگذاری لیست وکلا
async function loadLawyers() {
    try {
        const response = await AuthManager.apiCall('/Lawyers/GetAll', { method: 'GET' });
        const result = await response.json();

        const tbody = document.getElementById('lawyersTableBody');
        tbody.innerHTML = '';

        result.data.forEach(lawyer => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${lawyer.FirstName} ${lawyer.LastName}</td>
                <td>${lawyer.BarNumber || '---'}</td>
                <td><span class="badge bg-info text-dark">${lawyer.Specialization || 'عمومی'}</span></td>
                <td>${lawyer.PhoneNumber || '---'}</td>
                <td>
                    <button class="btn btn-sm btn-outline-info" onclick="viewLawyer(${lawyer.Id})">مشاهده</button>
                    <button class="btn btn-sm btn-outline-warning" onclick="editLawyer(${lawyer.Id})">ویرایش</button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteLawyer(${lawyer.Id})">حذف</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error(err);
        ToastManager.error("خطا در دریافت لیست وکلا");
    }
}

// تابع باز کردن مودال برای ثبت وکیل جدید
function openLawyerModal() {
    document.getElementById('lawyerForm').reset();
    document.getElementById('lawyerId').value = "0";

    // ریست کردن Select2 به حالت انتخاب نشده
    $('#userId').val('').trigger('change');

    document.getElementById('modalTitle').innerText = "ثبت وکیل جدید";
    lawyerModal.show();
}

// تابع باز کردن مودال برای ویرایش وکیل
async function editLawyer(id) {
    try {
        const response = await AuthManager.apiCall(`/Lawyers/Get/${id}`, { method: 'GET' });
        const result = await response.json();

        if (result.success) {
            const l = result.data;
            document.getElementById('lawyerId').value = l.Id;
            document.getElementById('firstName').value = l.FirstName || "";
            document.getElementById('lastName').value = l.LastName || "";
            document.getElementById('barNumber').value = l.BarNumber || "";
            document.getElementById('specialization').value = l.Specialization || "";
            document.getElementById('phoneNumber').value = l.PhoneNumber || "";
            document.getElementById('bio').value = l.Bio || "";
            document.getElementById('fatherName').value = l.FatherName || "";
            document.getElementById('nationalCode').value = l.NationalCode || "";
            document.getElementById('level').value = l.Level || "";

            // مقداردهی Select2 و اعمال تغییرات ظاهری با trigger
            $('#userId').val(l.userId || "").trigger('change');

            document.getElementById('modalTitle').innerText = "ویرایش وکیل";
            lawyerModal.show();
        } else {
            ToastManager.error(result.message);
        }
    } catch (err) {
        console.error(err);
        ToastManager.error("خطا در دریافت اطلاعات وکیل");
    }
}

// تابع ذخیره اطلاعات وکیل (ایجاد یا ویرایش)
async function saveLawyer() {
    const data = {
        id: parseInt(document.getElementById('lawyerId').value || 0),
        userId: parseInt(document.getElementById('UserId').value || 0),
        firstName: document.getElementById('firstName').value.trim(),
        lastName: document.getElementById('lastName').value.trim(),
        barNumber: document.getElementById('barNumber').value.trim() || null,
        specialization: document.getElementById('specialization').value.trim() || null,
        phoneNumber: document.getElementById('phoneNumber').value.trim() || null,
        bio: document.getElementById('bio').value.trim() || null,
        fatherName: document.getElementById('fatherName').value.trim() || null,
        nationalCode: document.getElementById('nationalCode').value.trim() || null,
        level: document.getElementById('level').value.trim() || null
    };

    if (!data.firstName || !data.lastName) {
        ToastManager.error("نام و نام خانوادگی الزامی است");
        return;
    }
    if (!data.nationalCode) {
        ToastManager.error("کد ملی الزامی است");
        return;
    }
    if (!data.fatherName) {
        ToastManager.error("نام پدر الزامی است");
        return;
    }
    try {
        const url = data.id === 0 ? '/Lawyers/Create' : '/Lawyers/Update';
        const response = await AuthManager.apiCall(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (result.success) {
            ToastManager.success(result.message);
            lawyerModal.hide();
            loadLawyers();
        } else {
            ToastManager.error(result.message);
        }
    } catch (err) {
        console.error(err);
        ToastManager.error("خطا در ذخیره اطلاعات");
    }
}

// تابع حذف وکیل
async function deleteLawyer(id) {
    if (!confirm("آیا از حذف این وکیل اطمینان دارید؟")) return;

    try {
        const response = await AuthManager.apiCall(`/Lawyers/Delete/${id}`, { method: 'POST' });
        const result = await response.json();

        if (result.success) {
            ToastManager.success(result.message);
            loadLawyers();
        } else {
            ToastManager.error(result.message);
        }
    } catch (err) {
        console.error(err);
        ToastManager.error("خطا در حذف وکیل");
    }
}

// هدایت به صفحه جزئیات وکیل
function viewLawyer(id) {
    window.location.href = `/Lawyers/Details/${id}`;
}
