const ToastManager = (() => {
    let container = null;

    function getContainer() {
        if (!container) {
            container = document.createElement('div');
            container.id = 'toast-container';
            container.style.cssText = `
                position: fixed;
                top: 1rem;
                left: 1rem;
                z-index: 9999;
                display: flex;
                flex-direction: column;
                gap: 0.5rem;
                min-width: 280px;
                max-width: 380px;
            `;
            document.body.appendChild(container);
        }
        return container;
    }

    function show(message, type = 'info', duration = 4000) {
        const c = getContainer();
        const id = 'toast-' + Date.now();

        const colors = {
            success: { bg: '#198754', icon: '✓' },
            error: { bg: '#dc3545', icon: '✕' },
            warning: { bg: '#ffc107', icon: '⚠' },
            info: { bg: '#0dcaf0', icon: 'ℹ' }
        };

        const { bg, icon } = colors[type] || colors.info;

        const toast = document.createElement('div');
        toast.id = id;
        toast.style.cssText = `
            background: ${bg};
            color: ${type === 'warning' ? '#000' : '#fff'};
            padding: 0.75rem 1rem;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.2);
            display: flex;
            align-items: center;
            gap: 0.6rem;
            font-family: Vazirmatn, sans-serif;
            font-size: 0.9rem;
            opacity: 0;
            transform: translateX(-20px);
            transition: all 0.3s ease;
            cursor: pointer;
        `;

        toast.innerHTML = `
            <span style="font-size:1.1rem;font-weight:bold;">${icon}</span>
            <span style="flex:1;">${message}</span>
            <span style="opacity:0.7;font-size:1rem;margin-right:4px;">×</span>
        `;

        toast.addEventListener('click', () => dismiss(id));
        c.appendChild(toast);

        requestAnimationFrame(() => {
            toast.style.opacity = '1';
            toast.style.transform = 'translateX(0)';
        });

        if (duration > 0) {
            setTimeout(() => dismiss(id), duration);
        }

        return id;
    }

    function dismiss(id) {
        const toast = document.getElementById(id);
        if (!toast) return;
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(-20px)';
        setTimeout(() => toast.remove(), 300);
    }

    return {
        success: (msg, duration) => show(msg, 'success', duration),
        error: (msg, duration) => show(msg, 'error', duration),
        warning: (msg, duration) => show(msg, 'warning', duration),
        info: (msg, duration) => show(msg, 'info', duration),
        dismiss
    };
})();
