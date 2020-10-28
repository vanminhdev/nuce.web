

//var baz = getParameterByName('baz'); // "" (present with no value)
//var qux = getParameterByName('qux'); // null (absent)s

(function () {

    //toastr.options = {
    //    "closeButton": true,
    //    "debug": false,
    //    "positionClass": "toast-top-right",
    //    "onclick": null,
    //    "showDuration": "1000",
    //    "hideDuration": "1000",
    //    "timeOut": "5000",
    //    "extendedTimeOut": "1000",
    //    "showEasing": "swing",
    //    "hideEasing": "linear",
    //    "showMethod": "fadeIn",
    //    "hideMethod": "fadeOut"
    //}

    Date.prototype.ToDateTime = function () {
        var year = this.getFullYear();
        var mm = this.getMonth() + 1; // getMonth() is zero-based
        var dd = this.getDate();
        var hour = this.getHours();
        var minute = this.getMinutes();
        var sec = this.getSeconds();
        var miniSecs = this.getMilliseconds();

        var result = "" + year;
        if (mm.toString().length == 1) {
            result = result + "0" + mm;
        } else {
            result = result + mm.toString();
        }

        if (dd.toString().length == 1) {
            result = result + "0" + dd;
        } else {
            result = result + dd.toString();
        }

        if (hour.toString().length == 1) {
            result = result + "0" + hour;
        } else {
            result = result + hour.toString();
        }

        if (minute.toString().length == 1) {
            result = result + "0" + minute;
        } else {
            result = result + minute.toString();
        }

        if (sec.toString().length == 1) {
            result = result + "0" + sec;
        } else {
            result = result + sec.toString();
        }

        result = result + this.getMilliseconds();

        return result;
    };

    if (!String.prototype.format) {
        String.prototype.format = function () {
            var args = arguments;
            return this.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] != 'undefined'
                  ? args[number]
                  : match
                ;
            });
        };
    }

    //var date = new Date();
    //console.log(date.ToDateTime());

})();


// ---------------------------------- notify ------------------------ //

var showSuccess = function (message) {
    notyfy({
        text: message,
        type: 'success',
        layout: 'bottomRight',
        timeout: 5000
        //closeWith: ['hover']
    });
};

var showError = function (message) {
    notyfy({
        text: message,
        type: 'error',
        layout: 'bottomRight',
        closeWith: ['hover']
    });
};

var showInfo = function (message) {
    notyfy({
        text: message,
        type: 'info',
        layout: 'bottomRight',
        timeout: 5000
        //closeWith: ['hover']
    });
};

var showServerError = function () {
    showError("Đã có lỗi xảy ra, Vui lòng liên hệ với ban quản trị !");
};

var showSuccessDefault = function () {
    showSuccess("Updated successfully !");
};

// ---------------------------------- /notify ------------------------ //

// -------------------------------- Template notify --------------------------// 

var nofifySuccess = function (header, message) {
    $.jGrowl(message, {
        header: header,
        theme: 'bg-primary'
    });
};

var nofifySuccess = function (header, message) {
    $.jGrowl(message, {
        header: "Success",
        theme: 'bg-primary'
    });
};

var nofifyError = function (message) {
    $.jGrowl(message, {
        header: 'Error!',
        theme: 'bg-danger'
    });
};

var nofifyInfo = function (message) {
    $.jGrowl(message, {
        header: 'Info !',
        theme: 'bg-info'
    });
};


// -------------------------------- /Template notify --------------------------// 


//(function () {

//    'use strict';

//    //intercept API error responses
//    mainModule.config(["$httpProvider", "$provide", function ($httpProvider, $provide) {

//        $httpProvider.interceptors.push(["$q", "$window", "$location", function ($q, $window, $location) {
//            return {
//                'request': function (config) {
//                    console.log("====== REQUEST");
//                    config.headers['X-Requested-With'] = 'XMLHttpRequest';
//                    return config;
//                },

//                'response': function (response) {
//                    //console.log("status code from module/config.js response callback : " + response.status);
//                    if (response.data) {

//                        if (response.data.IsSuccess === true) {
//                            //notificationProvider.notify(response.data);
//                        }
//                        if (response.data.IsSuccess === false) {
//                            if (response.data.Message)
//                                console.log(response.data.Message);
//                            else
//                                console.log("Server Error");
//                        }
//                    }

//                    return response;
//                },
//                'responseError': function (rejection) {
//                    //console.log("status code from module/config.js responseError callback " + rejection.status);
//                    console.log(rejection.config.url + "<br />" + rejection.data.Message);

//                    //if (rejection.status == 401) {
//                    //    console.log("status code 401");
//                    //    //$location.url("/Authentication/login");
//                    //    $window.location.href = '/Authentication/login';
//                    //} else if (rejection.status == 403) {
//                    //    console.log("status code 403");
//                    //    //$location.url("/global/global/accessdenied");
//                    //    $window.location.href = '/global/Redirect/accessdenied';
//                    //} else if (rejection.status == 404) {
//                    //    console.log("status code 404");
//                    //    //$location.url("/global/global/notFound");
//                    //    $window.location.href = '/global/Redirect/notFound';
//                    //} else if (rejection.status == 500) {
//                    //    console.log("status code 500");
//                    //    //$location.url("/global/global/internalServerError");
//                    //    $window.location.href = '/global/Redirect/internalServerError';
//                    //}

//                    return $q.reject(rejection);
//                }
//            };
//        }]);

//        $provide.decorator("$exceptionHandler", ['$delegate', function ($delegate) {
//            return function (exception, cause) {
//                console.error(exception);
//            };
//        }]);

//    }]);
//})();













/*
Javascript HMAC SHA256 : Dùng để tạo token

https://www.jokecamp.com/blog/examples-of-creating-base64-hashes-using-hmac-sha256-in-different-languages/

Run the code online with this jsfiddle. Dependent upon an open source js library called http://code.google.com/p/crypto-js/.

<script src="http://crypto-js.googlecode.com/svn/tags/3.0.2/build/rollups/hmac-sha256.js"></script>
<script src="http://crypto-js.googlecode.com/svn/tags/3.0.2/build/components/enc-base64-min.js"></script>

<script>
  var hash = CryptoJS.HmacSHA256("Message", "secret");
  var hashInBase64 = CryptoJS.enc.Base64.stringify(hash);
  document.write(hashInBase64);
</script>

*/

