$(document).on('click', '.btn-delete-hocsinh', function () {
    let id = $(this).data('id');
    $.ajax({
        url: '/HocSinh/Delete/' + id,
        type: 'GET',
        success: function (response) {
            $('#modalContainer').html(response);
            const modal = new bootstrap.Modal(document.getElementById('deleteHocSinhModal')
            );
            modal.show();
        },
        error: function () {
            alert('Cannot load delete form');
        }
    });
});
$(document).on('submit', '#deleteHocSinhForm', function (e) {
    e.preventDefault();
    let form = $(this);
    $.ajax({
        url: '/HocSinh/Delete',
        type: 'POST',
        data: form.serialize(),
        success: function (response) {
            if (response.success) {
                // Close modal
                const modalElement = document.getElementById('deleteHocSinhModal');
                const modal = bootstrap.Modal.getInstance(modalElement);
                modal.hide();
                // Reload table
                loadHocSinhs(currentPage);
            }
            else {
                alert('Delete failed');
            }
        },
        error: function () {
            alert('Delete failed');
        }
    });
});