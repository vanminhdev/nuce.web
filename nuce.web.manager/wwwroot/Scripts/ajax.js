
function hello (){
    $.ajax({
        type: "POST",
        url: "/WebService1.asmx/HelloWorld",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (req) {
            alert(req.d + "!")
           
        },
        error: function (msg) {
            // err msg
            alert(msg.d);
        }
    });
}
function checkOption(idcautraloi, idbaitest) {
    $.ajax({
        type: "POST",
        url: "/WebService1.asmx/checkOption",
        data: "{idcautraloi:" + idcautraloi + ",idbaitest:" + idbaitest + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (req) {
           // alert(req.d + "!")

        },
        error: function (msg) {
            // err msg
           // alert(msg.d);
        }
    });
}
function khachvanglai_checkOption(idcautraloi, idbaitest) {
    $.ajax({
        type: "POST",
        url: "/WebService1.asmx/khachvanglai_checkOption",
        data: "{idcautraloi:" + idcautraloi + ",idbaitest_khachvanglai:" + idbaitest + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (req) {
            // alert(req.d + "!")

        },
        error: function (msg) {
            // err msg
            // alert(msg.d);
        }
    });
}