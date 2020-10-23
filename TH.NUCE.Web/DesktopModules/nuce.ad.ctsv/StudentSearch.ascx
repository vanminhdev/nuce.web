<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StudentSearch.ascx.cs" Inherits="nuce.ad.ctsv.StudentSearch" %>
<div class="row" style="padding-top: 5px;">
    <div class="col-sm-2">
    </div>
    <div class="col-sm-8">
        <input class="form-control" id="myInput" type="text" placeholder="Search..">
    </div>
    <div class="col-sm-2">
    </div>
</div>
<div class="table-responsive">
    <table class="table">
        <thead>
            <tr>
                <th>#</th>
                <th>Mã sinh viên</th>
                <th>Tên sinh viên</th>
                <th>Lớp quản lý</th>
                <th style="width: 20%;" colspan="4">Chi tiết thông tin</th>
            </tr>
        </thead>
        <tbody id="tbContent">
        </tbody>
    </table>
</div>
<span id="spScript" runat="server"></span>
<script>
    (function ($) {
        $.fn.serializeAny = function () {
            var ret = [];
            $.each($(this).find(':input'), function () {
                if (this.name != "")
                    ret.push(encodeURIComponent(this.name) + "=" + encodeURIComponent($(this).val()));
            });

            return ret.join("&").replace(/%20/g, "+");
        }
    })(jQuery);
    var StudentSearch = {
        ID: -1,
        Data: [],
        DataDanhMuc: [],
        url: "/handler/nuce.ad.ctsv/",
        init: function () {
            StudentSearch.bindData($('#myInput').val());
        },
        bindData1: function () {
            StudentSearch.bindData($('#myInput').val());
        },
        bindData: function (text) {
            $('#tbContent').html("");
            $.getJSON(this.url + "ad_ctsv_studentsearch.ashx?search=" + text + "&&nocache='" + (new Date()).getTime(), function (data) {
                //alert(data);
                StudentSearch.Data = data;
                var strHtml = "";
                var count = 1;
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td>" + count + "</td>";
                    strHtml += "<td>" + item.Code + "</td>";
                    strHtml += "<td>" + item.FulName + "</td>";
                    strHtml += "<td>" + item.ClassCode + "</td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\"  onclick=\"StudentSearch.updateThongTin(" + item.ID + ");\">Cơ bản</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\"  onclick=\"StudentSearch.updateThongTinGiaDinh(" + item.ID + ");\">Gia đình</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\"  onclick=\"StudentSearch.updateThongTinQuaTrinhHocTap(" + item.ID + ");\">Học tập</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\"  onclick=\"StudentSearch.updateThongTinThiHSG(" + item.ID + ");\">Thi HSG</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-primary\"  onclick=\"StudentSearch.print('" + item.Code + "');\">In</button></td>";
                    strHtml += "</tr>";
                    count++;
                });
                $('#tbContent').html(strHtml);
            });
        },
        updateThongTin: function (input) {
            window.open('/tabid/35/default.aspx?ID=' + input);
        }
        ,
        updateThongTinGiaDinh: function (input) {
            window.open('/tabid/36/default.aspx?ID=' + input);
        }
        ,
        updateThongTinQuaTrinhHocTap: function (input) {
            window.open('/tabid/37/default.aspx?ID=' + input);
        }
        ,
        updateThongTinThiHSG: function (input) {
            window.open('/tabid/38/default.aspx?ID=' + input);
        }
        ,
        print: function (input) {
            window.location.href = '/tabid/21/default.aspx?act=print&&search='+input;
        }
    };
    StudentSearch.init();

</script>
<script>
    $(document).ready(function () {
        $(document).keypress(function (event) {

            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                var search = $('#myInput').val();
                StudentSearch.bindData1();
                event.preventDefault();
            }

        });
    });
</script>
