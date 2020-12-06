$(document).ready(function () {
    $('.auto-validate').each(function () {
        let form = $(this);
        let validator = form.validate({
            errorPlacement: function (error, element) {
                if (element.hasClass('selectpicker')) {
                    element.parent().tooltip({ title: error[0].innerText });
                    element.parent().addClass('is-invalid');
                }
                else {
                    element.addClass('is-invalid');
                    element.tooltip({ title: error[0].innerText });
                }
            },
            success: function (label, element) {
                let input = $(element);
                input.removeClass('error');
                input.removeClass('is-invalid');
                input.tooltip('dispose');
                if (input.hasClass('selectpicker')) {
                    input.parent().removeClass('is-invalid');
                    input.parent().tooltip('dispose');
                }
            }
        });
    });
});

function getFormData(id) {
    let data = {};
    let form = $(id);
    let elements = form.find('input,textarea,select');
    for (let i = 0; i < elements.length; i++) {
        let element = $(elements[i]);
        let val = element.val().trim();
        data[element.attr('name')] = val;
    }
    return data;
}

function fillFormData(id, data) {
    for (const property in data) {
        let element = $(`${id} [name=${property}]`);
        if (element.attr('type') === 'date') {
            let date = new Date(data[property]);
            element.val(getDateyyyyMMdd(date));
        } else if (element.prop("tagName") === "SELECT") {
            element.val(data[property]).trigger('change');
        } else {
            element.val(data[property]);
        }
    }
}
