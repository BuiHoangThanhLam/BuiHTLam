// ===============================
// Load hocsinhs bằng AJAX
// ===============================
function loadHocSinhs(page = 1) {
    currentPage = page;
    $.ajax({
        url: '/HocSinh/GetHocSinhs',
        type: 'GET',
        data: {
            page: currentPage,
            pageSize: currentPageSize
        },
        beforeSend: function () {
            $('#hocsinhTableContainer').html(`
                <div class="text-center p-5">
                    <div class="spinner-border text-primary"
                         role="status">
                    </div>
                    <div class="mt-2">
                        Loading hocsinhs...
                    </div>
                </div>
            `);
        },
        success: function (response) {
            $('#hocsinhTableContainer').html(response);
        },
        error: function () {
            $('#hocsinhTableContainer').html(`
                <div class="alert alert-danger">
                    Error loading hocsinhs.
                </div>
            `);
        }
    });
}
// ===============================
// Click pagination
// ===============================
$(document).on('click', '.pagination-link', function (e) {
    e.preventDefault();
    let page = $(this).data('page');
    // Không load nếu disabled
    if ($(this).parent().hasClass('disabled')) {
        return;
    }
    loadHocSinhs(page);
});
// ===============================
// Change page size
// ===============================
$(document).on('change', '#pageSizeSelect', function () {
    currentPageSize = $(this).val();
    // Reset về trang đầu
    currentPage = 1;
    loadHocSinhs(currentPage);
});