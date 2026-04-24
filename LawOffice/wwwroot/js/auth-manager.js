const AuthManager = (() => {
    let _isRefreshing = false;
    let _refreshQueue = [];

    const _endpoints = {
        login: '/Auth/Login',
        logout: '/Auth/Logout',
        refresh: '/Auth/RefreshToken'
    };

    const _storageKeys = {
        user: 'lawOffice_user'
    };

    // ----------------------------------------------------
    // UTILITIES
    // ----------------------------------------------------

    function parseJwt(token) {
        try {
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));

            return JSON.parse(jsonPayload);
        } catch (e) {
            console.error("خطا در پارس کردن JWT", e);
            return null;
        }
    }

    // ----------------------------------------------------
    // TOKEN MANAGEMENT
    // ----------------------------------------------------

    function setUser(userData) {
        if (!userData) return;
        localStorage.setItem(_storageKeys.user, JSON.stringify(userData));
    }

    function getUser() {
        const stored = localStorage.getItem(_storageKeys.user);
        return stored ? JSON.parse(stored) : null;
    }

    function clearAuth() {
        localStorage.removeItem(_storageKeys.user);
    }

    function isAuthenticated() {
        const user = getUser();
        return !!user && !!user.Id;
    }

    // ----------------------------------------------------
    // API METHODS
    // ----------------------------------------------------

    //async function apiCall(url, options = {}) {
    //    options.headers = {
    //        'Content-Type': 'application/json',
    //        ...options.headers
    //    };
    //    options.credentials = 'include';

    //    let response = await fetch(url, options);

    //    if (response.status === 401) {
    //        const refreshed = await refreshToken();
    //        if (refreshed) {
    //            response = await fetch(url, options);
    //        } else {
    //            await logout();
    //            return null;
    //        }
    //    }

    //    return response;
    //}
    async function apiCall(url, options = {}) {
        options.headers = {
            'Content-Type': 'application/json',
            ...options.headers
        };
        options.credentials = 'include';

        let response;
        try {
            response = await fetch(url, options);
        } catch (e) {
            console.error('Network error:', e);
            return null;
        }

        if (response.status === 401) {
            const refreshed = await refreshToken();
            if (refreshed) {
                response = await fetch(url, options);
            } else {
                clearAuth();
                window.location.href = '/Auth/Login';
                // یک Promise که هیچوقت resolve نمیشه تا کد بعدی اجرا نشه
                return new Promise(() => { });
            }
        }

        return response;
    }


    async function refreshToken() {
        if (_isRefreshing) {
            return new Promise((resolve) => {
                _refreshQueue.push(resolve);
            });
        }

        _isRefreshing = true;

        try {
            const response = await fetch(_endpoints.refresh, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                credentials: 'include'
            });

            if (response.ok) {
                const result = await response.json();
                if (result.success && result.accessToken) {
                    updateUserFromToken(result.accessToken);
                }

                _refreshQueue.forEach(cb => cb(true));
                _refreshQueue = [];
                _isRefreshing = false;
                return true;
            }

            throw new Error('Refresh failed');
        } catch (error) {
            console.error('Token refresh error:', error);
            _refreshQueue.forEach(cb => cb(false));
            _refreshQueue = [];
            _isRefreshing = false;
            return false;
        }
    }

    async function login(username, password, rememberMe = false) {
        try {
            const response = await fetch(_endpoints.login, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                credentials: 'include',
                body: JSON.stringify({ username, password, rememberMe })
            });

            const result = await response.json();

            if (result.success) {
                setUser(result.user);
                return { success: true, user: result.user };
            } else {
                return { success: false, message: result.message };
            }
        } catch (error) {
            console.error('Login error:', error);
            return { success: false, message: 'خطا در برقراری ارتباط با سرور' };
        }
    }

    async function logout() {
        clearAuth(); // اول auth رو پاک کن
        try {
            await fetch(_endpoints.logout, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                credentials: 'include'
            });
        } catch (error) {
            // مهم نیست، داریم logout می‌کنیم
        }
        window.location.href = '/Auth/Login';
    }

    function forceLogout() {
        clearAuth();
        window.location.href = '/Auth/Login';
    }

    function updateUserFromToken(token) {
        const decoded = parseJwt(token);
        if (decoded) {
            const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || decoded.sub || decoded.id;
            const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decoded.role || decoded.Role;
            const fullName = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || decoded.FullName || decoded.fullName || decoded.name;

            setUser({
                id: userId,
                FullName: fullName,
                Role: role
            });
        }
    }

    // ----------------------------------------------------
    // UI MANAGEMENT
    // ----------------------------------------------------

    function updateUI() {
        const user = getUser();
        if (!user) return;

        const fullName = user.FullName || user.fullName || '';
        const role = user.Role || user.role || '';

        const navUserName = document.getElementById('navUserName');
        if (navUserName) {
            navUserName.textContent = fullName;
        }

        const userFullName = document.getElementById('userFullName');
        if (userFullName) {
            userFullName.textContent = fullName;
        }

        const userRoleBadge = document.getElementById('navUserRole');
        if (userRoleBadge) {
            const roleNames = {
                'Admin': 'مدیر',
                'Lawyer': 'وکیل',
                'Staff': 'کارمند',
                'Client': 'موکل'
            };
            userRoleBadge.textContent = roleNames[role] || role;
        }

        const menuItems = document.querySelectorAll('.role-menu[data-roles]');
        menuItems.forEach(item => {
            if (!role) {
                item.style.display = 'none';
                return;
            }

            // تبدیل به حروف کوچک برای جلوگیری از خطای عدم تطابق حروف بزرگ/کوچک
            const userRoleLower = role.toLowerCase();
            const allowedRoles = item.getAttribute('data-roles').toLowerCase().split(',').map(r => r.trim());
            item.style.display = allowedRoles.includes(userRoleLower) ? '' : 'none';
        });
    }

    function setupEventListeners() {
        const logoutBtn = document.getElementById('btnLogout');
        if (logoutBtn) {
            logoutBtn.addEventListener('click', (e) => {
                e.preventDefault();
                logout();
            });
        }
        // رویداد سایدبار از اینجا حذف شد تا تداخل با AdminLTE ایجاد نکند
    }
    document.addEventListener('DOMContentLoaded', () => {
        updateUI();
    });
    async function init() {
        let user = getUser();

        if (!user) {
            if (!window.location.pathname.toLowerCase().includes('/auth/login')) {
                try {
                    const response = await fetch('/Auth/RefreshToken', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        credentials: 'include'
                    });

                    if (response.ok) {
                        const result = await response.json();
                        if (result.success && result.accessToken) {
                            updateUserFromToken(result.accessToken);
                            user = getUser();
                        } else {
                            await logout();
                            return;
                        }
                    } else {
                        await logout();
                        return;
                    }
                } catch (e) {
                    await logout();
                    return;
                }
            } else {
                return;
            }
        }

        updateUI();
        setupEventListeners();
    }

    return {
        login,
        logout,
        refreshToken,
        apiCall,
        isAuthenticated,
        getUser,
        setUser,
        clearAuth,
        updateUI,
        forceLogout,
        init
    };
})();

// حذف رویداد DOMContentLoaded اضافی برای جلوگیری از اجرای دوبار

window.AuthManager = AuthManager;
