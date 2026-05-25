$(document).on('click', '.btn-edit-hocsinh', function () {
    let id = $(this).data('id');
    $.ajax({
        url: '/HocSinh/Edit/' + id,
        type: 'GET',
        success: function (response) {
            $('#modalContainer').html(response);
            const modal = new bootstrap.Modal(
                document.getElementById('editHocSinhModal')
            );
            modal.show();
        },
        error: function () {
            alert('Cannot load edit form');
        }
    });
});
$(document).on('submit', '#editHocSinhForm', function (e) {
    e.preventDefault();
    let form = $(this);
    $.ajax({
        url: '/HocSinh/Edit',
        type: 'POST',
        data: form.serialize(),
        success: function (response) {
            if (response.success) {
                // Close modal
                const modalElement = document.getElementById('editHocSinhModal');
                const modal = bootstrap.Modal.getInstance(modalElement);
                modal.hide();
                // Reload table
                loadHocSinhs(currentPage);
            }
            else {
                $('#modalContainer').html(response);
                const modal = new bootstrap.Modal(document.getElementById('editHocSinhModal')
                );
                modal.show();
            }
        },
        error: function () {
            alert('Update failed');
        }
    });
});