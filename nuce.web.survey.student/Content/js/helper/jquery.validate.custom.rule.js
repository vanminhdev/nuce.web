/*custom validator*/
jQuery.validator.addMethod("dateLessThanOrEqual", function (value, element, param) {
    let value1 = $(element).val();
    let value2 = $(param).val();
    if (value1 === "" || value1 === null || value2 === "" || value2 === null) {
        return true;
    }
    let date1 = new Date(value1);
    let date2 = new Date(value2);
    if (date1 <= date2) {
        return true;
    }
    return false;
});

jQuery.validator.addMethod("dateGreaterThanOrEqual", function (value, element, param) {
    let value1 = $(element).val();
    let value2 = $(param).val();
    if (value1 === "" || value1 === null || value2 === "" || value2 === null) {
        return true;
    }
    let date1 = new Date(value1);
    let date2 = new Date(value2);
    if (date1 >= date2) {
        return true;
    }
    return false;
});

//giá trị cách nhau bởi dấu ,
jQuery.validator.addMethod("onlyAcceptListValue", function (value, element, param) {
    param = '' + param; //đảm bảo là string
    let listVal = param.split(",");
    let val = $(element).val(); //đảm bảo trường hợp là select k phải text nên k dùng value
    if (listVal.indexOf('' + val) > -1) {
        return true;
    }
    return false;
});

jQuery.validator.addMethod("phoneNumberMobile", function (value, element, param) {
    let val = $(element).val();
    var patt = /^[0-9]{10}$/;
    return patt.test(val);
});

jQuery.validator.addMethod("emailNotEdu", function (value, element, param) {
    let val = $(element).val();
    var patt = /(?=.*@)(?!.*edu).*/;
    return patt.test(val);
});


