//auto clear form
$(document).ready(function () {
    $('.auto-clear-form-on-open').each(function () {
        let seft = $(this);
        seft.on('show.bs.modal', function (event) {
            //console.log(seft.find('input'));
            let inputs = seft.find('input,textarea');
            let element;
            for (let i = 0; i < inputs.length; i++) {
                element = $(inputs[i]);
                element.val('');
                element.removeClass('error');
                element.removeClass('is-invalid');
                element.tooltip('dispose');
            }

            let selects = seft.find('select');
            for (let i = 0; i < selects.length; i++) {
                element = $(selects[i]);
                if (element.hasClass('selectpicker')) {
                    element.removeClass('error');
                    element.parent().removeClass('is-invalid');
                    element.parent().tooltip('dispose');
                }
                else {
                    element.removeClass('error');
                    element.removeClass('is-invalid');
                    element.tooltip('dispose');
                }
            }
        });
    });
});

