// ===============================
// Open Create Modal
// ===============================
$(document).on('click', '#btnAddHocSinh', function () {

    $.ajax({

        url: '/HocSinh/Create',

        type: 'GET',

        success: function (response) {

            $('#modalContainer').html(response);

            const modalEl = document.getElementById('createHocSinhModal');
            const modal = new bootstrap.Modal(modalEl);

            modal.show();

        },

        error: function (xhr) {

            console.log(xhr.responseText);
            alert('Cannot load create form');

        }

    });

});


// ===============================
// Submit Create Form
// ===============================
$(document).on('submit', '#createHocSinhForm', function (e) {

    e.preventDefault();

    let form = $(this);

    $.ajax({

        url: form.attr('action'),

        type: 'POST',

        data: form.serialize(),

        success: function (response) {

            console.log(response);

            if (response.success === true) {

                window.location.href = response.redirectUrl || '/HocSinh/Index';

                return;
            }

            if (response.errors && response.errors.length > 0) {

                alert(response.errors.join('\n'));

                return;
            }

            alert('Không lưu được học sinh nhưng server không trả lỗi rõ ràng.');
        },

        error: function (xhr) {

            console.log(xhr.status);
            console.log(xhr.responseText);

            alert('Create failed. Mở Console để xem lỗi chi tiết.');
        }

    });

});