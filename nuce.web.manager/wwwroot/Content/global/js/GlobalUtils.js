function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

// kiểm tra địa chỉ ví bitAddrses có thực hay không
function checkBitAddress(address) {
    var result = false;

    if (address.length > 34 || address.length < 27)  /* độ dài không hợp lệ */
        return result;

    if (/[0OIl]/.test(address))   /* base58 encoding */
        return result;
    $.ajax({
        url: "https://blockchain.info/it/q/addressbalance/" + address,   /* trả về balance với lượng tính bằng satoshi */
        crossDomain: true, // cross origin resource sharing
        async: false
    }).done(function (data) {
        var isnum = /^\d+$/.test(data);
        if (isnum) {                         /* nếu data trả về là kiểu int thì là hợp lệ */
            //console.log("data is integer");
            result = true;
        }
    });
    return result;
};

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

// http://www.designchemical.com/blog/index.php/jquery/8-useful-jquery-snippets-for-urls-querystrings/

function getAllParametersFromUrlAsArray(URL) {
    var vars = [], hash;
    var q = URL.split('?')[1];
    if (q != undefined) {
        q = q.split('&');
        for (var i = 0; i < q.length; i++) {
            hash = q[i].split('=');
            vars.push(hash[1]);
            vars[hash[0]] = hash[1];
        }
    }

    return vars;
};

function getAllParametersFromUrl(URL) {
    var hash;
    var q = URL.split('?')[1];
    if (q != undefined) {
        return q;
    }

    return "";
};

function getCurrentDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = dd + '/' + mm + '/' + yyyy;
    return today;
};