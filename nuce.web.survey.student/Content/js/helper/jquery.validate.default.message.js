﻿jQuery.extend(jQuery.validator.messages, {
    required: "Trường không được bỏ trống.",
    remote: "Please fix this field.",
    email: "Email không hợp lệ.",
    url: "URL không hợp lệ.",
    date: "Ngày không hợp lệ.",
    dateISO: "Nhập ngày định dạng (ISO).",
    number: "Số không hợp lệ.",
    digits: "Chỉ nhập số nguyên dương hoặc bằng 0.",
    creditcard: "Nhập số credit card không hợp lệ.",
    equalTo: "Giá trị nhập lại không khớp.",
    accept: "Please enter a value with a valid extension.",
    maxlength: jQuery.validator.format("Số ký tự không vượt quá {0}."),
    minlength: jQuery.validator.format("Số ký tự không nhỏ hơn {0}."),
    rangelength: jQuery.validator.format("Nhập số ký tự nằm trong khoảng từ {0} tới {1}."),
    range: jQuery.validator.format("Nhập giá trị nằm trong khoảng từ {0} tới {1}."),
    max: jQuery.validator.format("Nhập giá trị nhỏ hơn hoặc bằng {0}."),
    min: jQuery.validator.format("Nhập giá trị lớn hơn hoặc bằng {0}."),
    //custom rules
    dateLessThanOrEqual: "Từ ngày phải nhỏ hơn hoặc bằng đến ngày",
    dateGreaterThanOrEqual: "Đến ngày phải lớn hơn hoặc bằng từ ngày",
    onlyAcceptListValue: "Giá trị không hợp lệ",
    phoneNumberMobile: "Số điện thoại di động phải có 10 số",
    emailNotEdu: "Địa chỉ email không được sử dụng tài khoản edu, khuyên dùng tài khoản gmail"
});