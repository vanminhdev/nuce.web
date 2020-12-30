function dateString(today) {
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    return dd + '/' + mm + '/' + yyyy;
}

function dateTimeString(today) {
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();

    var hours = String(today.getHours()).padStart(2, '0');
    var minutes = String(today.getMinutes()).padStart(2, '0');
    return `${dd}/${mm}/${yyyy} ${hours}:${minutes}`;
}


//2020-01-01T00:00:00 -> dd/MM/yyyy
function getDateString(date) {
    return dateString(new Date(date));
}

function getDateStringFromServerType(date) {
    if (date == null || date === "") {
        return '';
    }
    let regex = /(\d+)/g;
    return dateString(new Date(+date.match(regex)[0]));
}

function getDateTimeStringFromServerType(date) {
    if (date == null || date === "") {
        return '';
    }
    let regex = /(\d+)/g;
    return dateTimeString(new Date(+date.match(regex)[0]));
}

function getDateyyyyMMdd(date) {
    var mm = date.getMonth() + 1;
    var dd = date.getDate();

    return [date.getFullYear(),
    (mm > 9 ? '' : '0') + mm,
    (dd > 9 ? '' : '0') + dd
    ].join('-');
};

const debounce = (func, delay) => {
    let debounceTimer;
    return function () {
        const context = this;
        const args = arguments;
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => func.apply(context, args), delay);
    }
}


$('.toast').toast({
    delay: 4000
});

function toastSuccess(message) {
    if (message == undefined || message == "") {
        message = "Thành công";
        console.log("default toast message");
    }
    $('#toast-success-message').html(message);
    $('#toast-success').toast('show');
}

function toastWarning(message) {
    if (message == undefined || message == "") {
        message = "Cảnh báo";
        console.log("default toast message");
    }
    $('#toast-warning-message').html(message);
    $('#toast-warning').toast('show');
}

function toastError(message) {
    if (message == undefined || message == "") {
        message = "Lỗi";
        console.log("default toast message");
    }
    $('#toast-error-message').html(message);
    $('#toast-error').toast('show');
}