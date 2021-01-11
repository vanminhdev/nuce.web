var dateString = function(today) {
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    return dd + '/' + mm + '/' + yyyy;
}

var dateTimeString = function(today) {
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();

    var hours = String(today.getHours()).padStart(2, '0');
    var minutes = String(today.getMinutes()).padStart(2, '0');
    return `${dd}/${mm}/${yyyy} ${hours}:${minutes}`;
}


//2020-01-01T00:00:00 -> dd/MM/yyyy
var getDateString = function(date) {
    return dateString(new Date(date));
}

var getDateStringFromServerType = function(date) {
    if (date == null || date === "") {
        return '';
    }
    let regex = /(\d+)/g;
    return dateString(new Date(+date.match(regex)[0]));
}

var getDateTimeStringFromServerType = function(date) {
    if (date == null || date === "") {
        return '';
    }
    let regex = /(\d+)/g;
    return dateTimeString(new Date(+date.match(regex)[0]));
}

var getDateyyyyMMdd = function(date) {
    var mm = date.getMonth() + 1;
    var dd = date.getDate();

    return [date.getFullYear(),
    (mm > 9 ? '' : '0') + mm,
    (dd > 9 ? '' : '0') + dd
    ].join('-');
};

/*const debounce = (func, delay) {
    let debounceTimer;
    return function () {
        const context = this;
        const args = arguments;
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => func.apply(context, args), delay);
    }
}*/


$('.toast').toast({
    delay: 4000
});

var toastSuccess = function(message) {
    if (message == undefined || message == "") {
        message = "Thành công";
        console.log("default toast message");
    }
    $('#toast-success-message').html(message);
    $('#toast-success').toast('show');
}

var toastWarning = function(message) {
    if (message == undefined || message == "") {
        message = "Cảnh báo";
        console.log("default toast message");
    }
    $('#toast-warning-message').html(message);
    $('#toast-warning').toast('show');
}

var toastError = function(message) {
    if (message == undefined || message == "") {
        message = "Lỗi";
        console.log("default toast message");
    }
    $('#toast-error-message').html(message);
    $('#toast-error').toast('show');
}

$('.toast').each((index, el) => {
    $(el).on('show.bs.toast', function () {
        $(el).css({
            display: 'block'
        });
    });
    $(el).on('hidden.bs.toast', function () {
        $(el).css({
            display: 'none',
        });
    });
});
