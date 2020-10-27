(function () {
    'use strict';

    //intercept API error responses
    mainModule.config(["$httpProvider", "$provide", function ($httpProvider, $provide) {

        $httpProvider.interceptors.push(["$q", "$window", "$location", "sessionStorageSvc", "localStorageSvc",
                                                    function ($q, $window, $location, sessionStorageSvc, localStorageSvc) {
            return {
                'request': function (config) {
                    //testHmac();
                    //var httpMethod = config.method;
                    //var timestamp = new Date().ToDateTime();
                    //var uri = config.url; //+ "?abc=1&b1=2";
                    //uri = uri.toLowerCase();
                    //var requestBodyJson = config.data;
                    //$window.localStorage.setItem(_SESSION_STORAGE.USER_SCRETKEY, angular.toJson(data.HashedPassword));

                    //var hashedPassword = localStorageSvc.getItem(_SESSION_STORAGE.USER_SCRETKEY); //"71b74b33d3516506325f189a3cb6f281"; //.toUpperCase(); // hashedpass as private key
                    //var requestBodyString = "";
                    //var paramString = getAllParametersFromUrl(uri);
                    //console.log(paramString);

                    //$.each(requestBodyJson, function (key, val) {
                    //    requestBodyString = requestBodyString + key + "=" + val + "&";
                    //});

                    //if (requestBodyString.length > 0) {
                    //    requestBodyString = requestBodyString.substring(0, requestBodyString.length - 1);
                    //}

                    //if (paramString != "") {
                    //    uri = uri.replace(new RegExp(paramString, 'g'), ""); 
                    //    uri = uri.replace(/\?/g, '')
                    //}

                    //var message = httpMethod + "\n";
                    //message = message + timestamp + "\n";
                    //message = message + uri;
                    //if (requestBodyJson != "" && requestBodyJson != undefined) {
                    //    message = message + "\n";
                    //    message = message + JSON.stringify(requestBodyJson);
                    //}

                    //if (paramString != "") {
                    //    message = message + "\n";
                    //    message = message + paramString;
                    //}

                    //message = message + "\n";
                    //console.log("message = ");
                    //console.log("dkm nghiên cứu lòi mắt ra vẫn đéo chạy đc đkm");

                    //var signatureArr = sha256.hmac(hashedPassword, message); // dạng Uint8Arrays 
                    // giải mã Uint8Array hoặc Array of bytes thành string sử dụng Base-64 encoding.
                    //var signature = nacl.util.encodeBase64(signatureArr); //new TextDecoder("utf-8").decode(signatureArr);
                    //for (var i = 0; i < signatureArr.byteLength; i++) {
                    //    signature += String.fromCharCode(signatureArr[i])
                    //}

                    // ----------
                    //console.log(message);
                    //var hash = CryptoJS.HmacSHA256(message, hashedPassword);
                    //var hashInBase64 = CryptoJS.enc.Base64.stringify(hash);
                    //var hashInBase64 = "";
                    // ---------

                    //console.log("signature = ");
                    //console.log(signature);   
                    // signature của user
                    //config.headers['Timestamp'] = timestamp;
                    //config.headers['Username'] = localStorageSvc.getItem(_SESSION_STORAGE.USER); //$window.sessionStorage[_SESSION_STORAGE.USER]; // sessionStorageSvc.getStorageKey(_SESSION_STORAGE.USER);  //'@BizzWebCore.Context.ApplicationContext.CurrentUsername';
                    //config.headers['Authentication'] = localStorageSvc.getItem(_SESSION_STORAGE.USER) + ":" + hashInBase64; // {username}:{signature}
                    
                    //console.log(config);
                    // thêm header để server recognize ajax request
                    config.headers['X-Requested-With'] = 'XMLHttpRequest';
                    return config;
                },

                'response': function (response) {
                    //console.log("status code from module/config.js response callback : " + response.status);
                    if (response.data) {

                        if (response.data.IsSuccess === true) {
                            //notificationProvider.notify(response.data);
                        }
                        if (response.data.IsSuccess === false) {
                            //showServerError();
                            //if (response.data.Message)
                            //    console.log(response.data.Message);
                            //else
                            //    console.log("Server Error");
                        }
                    }

                    return response;
                },
                'responseError': function (rejection) {
                    showServerError();
                    console.log(" INTERCEPTOR : status code from module/config.js responseError callback " + rejection.status);
                    //console.log(rejection.config.url + "<br />" + rejection.data.Message);
                    //if (rejection.status == 401) {
                    //    console.log("status code 401");
                    //    //$location.url("/Authentication/login");
                    //    $window.location.href = '/Authentication/login';
                    //} else if (rejection.status == 403) {
                    //    console.log("status code 403");
                    //    //$location.url("/global/global/accessdenied");
                    //    $window.location.href = '/global/Redirect/accessdenied';
                    //} else if (rejection.status == 404) {
                    //    console.log("status code 404");
                    //    //$location.url("/global/global/notFound");
                    //    $window.location.href = '/global/Redirect/notFound';
                    //} else if (rejection.status == 500) {
                    //    console.log("status code 500");
                    //    //$location.url("/global/global/internalServerError");
                    //    $window.location.href = '/global/Redirect/internalServerError';
                    //}

                    return $q.reject(rejection);
                }
            };
        }]);
        
        $provide.decorator("$exceptionHandler", ['$delegate', function ($delegate) {
            return function (exception, cause) {
                console.error(exception);
            };
        }]);
        
    }]);
})();

var testHmac = function () {
    var hashedPassword = "123";
    var message = "123";
    var signatureArr = sha256.hmac(hashedPassword, message);
    signatureArr = nacl.util.encodeBase64(signatureArr);

    var hash = CryptoJS.HmacSHA256("123", "123");
    var hashInBase64 = CryptoJS.enc.Base64.stringify(hash);

    console.log("hashInBase64 = " + hashInBase64);
    var mac_hex = HMAC_SHA256_MAC(hashedPassword, message);

    //HMAC_SHA256_init("123");
    //HMAC_SHA256_write("123");
    //mac = HMAC_SHA256_finalize();
    //mac_hex = array_to_hex_string(mac);

    console.log("mac_hex = " + mac_hex);
};