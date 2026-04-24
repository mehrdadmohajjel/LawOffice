document.addEventListener('DOMContentLoaded', function () {

    // ۱. مدیریت نمایش و مخفی کردن رمز عبور
    const togglePasswordBtn = document.getElementById('togglePassword');
    if (togglePasswordBtn) {
        togglePasswordBtn.addEventListener('click', function () {
            const passwordInput = document.getElementById('password');
            const icon = this.querySelector('i');

            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                icon.classList.remove('bi-eye');
                icon.classList.add('bi-eye-slash');
            } else {
                passwordInput.type = 'password';
                icon.classList.remove('bi-eye-slash');
                icon.classList.add('bi-eye');
            }
        });
    }

    // ۲. مدیریت ارسال فرم لاگین
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', async function (e) {
            e.preventDefault();

            const btnLogin = document.getElementById('btnLogin');
            const errorAlert = document.getElementById('errorAlert');

            // تغییر وضعیت دکمه به حالت لودینگ
            btnLogin.disabled = true;
            btnLogin.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>در حال ورود...';
            errorAlert.classList.add('d-none');

            const formData = {
                username: document.getElementById('username').value,
                password: document.getElementById('password').value,
                rememberMe: document.getElementById('rememberMe').checked
            };

            try {
                const response = await fetch('/Auth/Login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(formData)
                });

                const result = await response.json();

                // بررسی موفقیت‌آمیز بودن عملیات
                if (result.success || result.Success) {

                    // فقط اطلاعات کاربر را ذخیره می‌کنیم
                    if (typeof AuthManager !== 'undefined' && typeof AuthManager.setUser === 'function') {
                        const userData = result.User || result.user;
                        AuthManager.setUser(userData);
                    }

                    // هدایت به داشبورد
                    window.location.href = '/Dashboard/Index';
                } else {
                    // نمایش خطای بازگشتی از سرور
                    errorAlert.textContent = result.message || result.Message || 'نام کاربری یا رمز عبور اشتباه است.';
                    errorAlert.classList.remove('d-none');
                }
            } catch (error) {
                console.error('Login error:', error);
                errorAlert.textContent = 'خطا در برقراری ارتباط با سرور.';
                errorAlert.classList.remove('d-none');
            } finally {
                // بازگرداندن وضعیت دکمه به حالت اولیه
                btnLogin.disabled = false;
                btnLogin.innerHTML = '<i class="bi bi-box-arrow-in-left me-2"></i>ورود به سیستم';
            }
        });
    }
});
