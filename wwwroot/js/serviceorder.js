function formatDate(date) {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = ('0' + (d.getMonth() + 1)).slice(-2); // add leading zero, if needed
    const day = ('0' + d.getDate()).slice(-2);
    const hours = ('0' + d.getHours()).slice(-2);
    const minutes = ('0' + d.getMinutes()).slice(-2);
    const seconds = ('0' + d.getSeconds()).slice(-2);
    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
}   

// Function to initialize event listeners on radio buttons
function initRadioListeners() {
    const radios = document.querySelectorAll('input[type="radio"]');

    radios.forEach(radio => {
        radio.addEventListener('change', () => {
            const row = radio.closest('tr');
            const status = radio.value;
            const tbody = row.closest('tbody');
            const categoryCell = tbody.querySelector('.category-header');

            updateRowColor(row, status);
            updateCategoryColor(tbody, categoryCell);
        });
    });
}

// Function to update row color based on status
function updateRowColor(row, status) {
    const tds = row.querySelectorAll('td:not(.category-header)');
    tds.forEach(td => {
        td.className = "";
        td.classList.add(`status-${status.toLowerCase().replace(/\s+/g, '-')}`);
    });
}

// Function to update category row color based on item status
function updateCategoryColor(tbody, categoryCell) {
    const rows = tbody.querySelectorAll('tr');
    const checkedRows = Array.from(rows).filter(row => row.querySelector('input[type="radio"]:checked'));

    if (checkedRows.length === rows.length) {
        categoryCell.classList.add("all-checked");
    } else {
        categoryCell.classList.remove("all-checked");
    }
}

// Pageload
document.addEventListener('DOMContentLoaded', function () {
    initRadioListeners();

});