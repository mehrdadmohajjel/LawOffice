// wwwroot/js/financials.js

let financialsData = [];

document.addEventListener('DOMContentLoaded', function () {
    loadFinancials();
    loadCasesForDropdown();
});

async function loadFinancials() {
    try {
        const response = await AuthManager.apiCall('/Financials/GetAll');
        const result = await response.json();

        if (result.success) {
            financialsData = result.data;
            renderFinancialsTable();
            updateSummary();
        }
    } catch (error) {
        console.error('Error loading financials:', error);
    }
}

async function loadCasesForDropdown() {
    try {
        const response = await AuthManager.apiCall('/Cases/GetAll');
        const result = await response.json();

        if (result.success) {
            const select = document.getElementById('financialCaseId');
            select.innerHTML = '<option value="">انتخاب پرونده (اختیاری)</option>' +
                result.data.map(c => `<option value="${c.id}">${c.caseCode} - ${c.title}</option>`).join('');
        }
    } catch (error) {
        console.error('Error loading cases:', error);
    }
}

function renderFinancialsTable() {
    const tbody = document.getElementById('financialsTableBody');

    if (financialsData.length === 0) {
        tbody.innerHTML = '<tr><td colspan="6" class="text-center text-muted py-4">تراکنشی ثبت نشده است</td></tr>';
        return;
    }

    const typeLabels = ['دریافتی', 'پرداختی'];
    const typeColors = ['success', 'danger'];

    tbody.innerHTML = financialsData.map(item => `
        <tr>
            <td>${new Date(item.transactionDate).toLocaleDateString('fa-IR')}</td>
            <td><span class="badge bg-${typeColors[item.type]}">${typeLabels[item.type]}</span></td>
            <td>${item.amount.toLocaleString('fa-IR')} ریال</td>
            <td>${item.description}</td>
            <td>${item.caseCode || '-'}</td>
            <td>
                <button class="btn btn-sm btn-outline-danger" onclick="deleteFinancial(${item.id})" title="حذف">
                    <i class="bi bi-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');
}

function updateSummary() {
    const income = financialsData.filter(f => f.type === 0).reduce((sum, f) => sum + f.amount, 0);
    const expense = financialsData.filter(f => f.type === 1).reduce((sum, f) => sum + f.amount, 0);
    const balance = income - expense;

    document.getElementById('totalIncome').textContent = income.toLocaleString('fa-IR');
    document.getElementById('totalExpense').textContent = expense.toLocaleString('fa-IR');
    document.getElementById('balance').textContent = balance.toLocaleString('fa-IR');
}

async function saveFinancial() {
    const form = document.getElementById('financialForm');
    if (!form.checkValidity()) {
        form.reportValidity();
        return;
    }

    const data = {
        type: parseInt(document.getElementById('financialType').value),
        amount: parseFloat(document.getElementById('financialAmount').value),
        description: document.getElementById('financialDescription').value,
        transactionDate: document.getElementById('financialDate').value,
        caseId: document.getElementById('financialCaseId').value || null
    };

    try {
        const response = await AuthManager.apiCall('/Financials/Create', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (result.success) {
            showSuccess('تراکنش با موفقیت ثبت شد');
            bootstrap.Modal.getInstance(document.getElementById('financialModal')).hide();
            loadFinancials();
        } else {
            showError(result.message || 'خطا در ثبت تراکنش');
        }
    } catch (error) {
        console.error('Error saving financial:', error);
        showError('خطا در برقراری ارتباط با سرور');
    }
}

async function deleteFinancial(id) {
    if (!confirm('آیا از حذف این تراکنش اطمینان دارید؟')) {
        return;
    }

    try {
        const response = await AuthManager.apiCall(`/Financials/Delete/${id}`, {
            method: 'POST'
        });

        const result = await response.json();

        if (result.success) {
            showSuccess('تراکنش با موفقیت حذف شد');
            loadFinancials();
        } else {
            showError(result.message || 'خطا در حذف تراکنش');
        }
    } catch (error) {
        console.error('Error deleting financial:', error);
        showError('خطا در برقراری ارتباط با سرور');
    }
}

function showSuccess(message) {
    alert(message);
}

function showError(message) {
    alert(message);
}
