const userModal = new bootstrap.Modal(document.getElementById('userModal'));

document.addEventListener("DOMContentLoaded", function () {
    loadUsers();
});

// واکشی و رندر لیست کاربران
async function loadUsers() {
    const container = document.getElementById('usersContainer');
    container.innerHTML = '<div class="text-center mt-4"><div class="spinner-border text-primary"></div></div>';

    try {
        const response = await fetch('/Users/GetAll');
        if (!response.ok) throw new Error("خطا در واکشی اطلاعات.");
        const users = await response.json();

        container.innerHTML = '';
        if (users.length === 0) {
            container.innerHTML = '<div class="col-12 text-center text-muted mt-4">کاربری یافت نشد.</div>';
            return;
        }

        users.forEach(user => {
            // تعیین رنگ برچسب نقش‌ها
            let roleClass = "bg-secondary";
            if (user.role === 0) roleClass = "bg-danger"; // Admin
            if (user.role === 1) roleClass = "bg-info text-dark"; // Client
            if (user.role === 2) roleClass = "bg-primary"; // Lawyer

            const statusClass = user.isActive ? "text-success" : "text-danger";
            const statusIcon = user.isActive ? "bi-check-circle-fill" : "bi-x-circle-fill";

            const card = `
                <div class="col-12 col-md-6 col-lg-4 mb-3">
                    <div class="card h-100 shadow-sm border-0 border-top border-3 ${user.isActive ? 'border-primary' : 'border-secondary'}">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-start mb-2">
                                <div>
                                    <h6 class="fw-bold mb-1">${user.FullName}</h6>
                                    <span class="text-muted small" dir="ltr">@${user.Username}</span>
                                </div>
                                <span class="badge ${roleClass}">${user.RoleName}</span>
                            </div>
                            
                            <hr class="text-muted opacity-25">
                            
                            <div class="small text-muted mb-1"><i class="bi bi-telephone me-1"></i> ${user.PoneNumber || 'ثبت نشده'}</div>
                            <div class="small text-muted mb-3"><i class="bi bi-envelope me-1"></i> ${user.Email || 'ثبت نشده'}</div>
                            
                            <div class="d-flex justify-content-between align-items-center mt-auto">
                                <span class="small ${statusClass} fw-bold cursor-pointer" onclick="toggleStatus(${user.Id})" title="تغییر وضعیت">
                                    <i class="bi ${statusIcon}"></i> ${user.IsActive ? 'فعال' : 'غیرفعال'}
                                </span>
                                <div>
                                    <button class="btn btn-sm btn-outline-primary px-3" onclick="editUser(${user.Id})">
                                        <i class="bi bi-pencil"></i> ویرایش
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
            container.insertAdjacentHTML('beforeend', card);
        });
    } catch (error) {
        container.innerHTML = '<div class="alert alert-danger">خطا در دریافت لیست کاربران.</div>';
    }
}

// باز کردن مودال در حالت ثبت جدید
function openUserModal() {
    document.getElementById('userForm').reset();
    document.getElementById('userId').value = "0";
    document.getElementById('modalTitle').innerText = "ایجاد کاربر جدید";
    document.getElementById('password').required = true;
    userModal.show();
}

// باز کردن مودال در حالت ویرایش
async function editUser(id) {
    try {
        const response = await fetch(`/Users/GetById/${id}`);
        if (!response.ok) throw new Error("خطا در دریافت اطلاعات کاربر.");
        const user = await response.json();

        document.getElementById('userId').value = user.id;
        document.getElementById('firstName').value = user.firstName;
        document.getElementById('lastName').value = user.lastName;
        document.getElementById('username').value = user.username;
        document.getElementById('phoneNumber').value = user.phoneNumber || '';
        document.getElementById('email').value = user.email || '';
        document.getElementById('role').value = user.role;
        document.getElementById('password').value = "";
        document.getElementById('password').required = false;

        document.getElementById('modalTitle').innerText = "ویرایش کاربر";
        userModal.show();
    } catch (error) {
        alert("خطا در ارتباط با سرور");
    }
}

// ارسال داده‌ها به سرور
async function saveUser() {
    const form = document.getElementById('userForm');
    if (!form.checkValidity()) {
        form.reportValidity();
        return;
    }

    const payload = {
        id: parseInt(document.getElementById('userId').value),
        firstName: document.getElementById('firstName').value,
        lastName: document.getElementById('lastName').value,
        username: document.getElementById('username').value,
        password: document.getElementById('password').value,
        phoneNumber: document.getElementById('phoneNumber').value,
        email: document.getElementById('email').value,
        role: parseInt(document.getElementById('role').value)
    };

    try {
        const response = await fetch('/Users/Save', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        const data = await response.json();
        if (response.ok) {
            // ToastManager.success(data.message); // در صورت وجود ToastManager در پروژه
            alert(data.message);
            userModal.hide();
            loadUsers();
        } else {
            alert(data.message || "خطا در اعتبارسنجی اطلاعات");
        }
    } catch (error) {
        alert("خطا در ارسال اطلاعات به سرور");
    }
}

// تغییر وضعیت (فعال/غیرفعال)
async function toggleStatus(id) {
    if (!confirm("آیا از تغییر وضعیت این کاربر مطمئن هستید؟")) return;

    try {
        const response = await fetch(`/Users/ToggleStatus/${id}`, { method: 'POST' });
        const data = await response.json();
        if (response.ok) {
            loadUsers();
        } else {
            alert(data.message);
        }
    } catch (error) {
        alert("خطا در برقراری ارتباط");
    }
}
