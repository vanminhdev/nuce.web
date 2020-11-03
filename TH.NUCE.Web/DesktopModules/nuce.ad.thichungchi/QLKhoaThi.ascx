<%@ control language="C#" autoeventwireup="true" codebehind="QLKhoaThi.ascx.cs" inherits="nuce.ad.thichungchi.QLKhoaThi" %>
<div class="table-responsive">
    <table class="table">
        <thead>
            <tr>
                <th>Tên</th>
                <th>Môn học</th>
                <th>Trạng thái</th>
                <th style="width: 5%;"></th>
                <th style="width: 5%;"></th>
                <th style="width: 5%;"></th>
                <th style="width: 5%;"></th>
            </tr>
        </thead>
        <tbody id="tbContent">
            <tr>
                <td>Anna</td>
                <td>Pitt</td>
                <td>
                    <button type="button" class="btn btn-info">Sửa</button>
                </td>
                <td>
                    <button type="button" class="btn btn-danger">Xóa</button>
                </td>
                <td>
                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal">Thêm mới</button>
                </td>
                <td>
                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal">Thêm mới</button>
                </td>
            </tr>
        </tbody>
    </table>
</div>

<!-- Modal them sua -->
<div id="myModal" class="modal fade" role="dialog">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="edit_header">Modal Header</h4>
                <div id="edit_header_ThongBao" style="color: red;"></div>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="Ten">Tên:</label>
                    <input type="text" class="form-control" id="Ten">
                </div>
                <div class="form-group">
                    <label for="MoTa">Môn học:</label>
                    <select class="form-control" name="MonHoc" id="slMonThi">
                        <option>1</option>
                        <option>2</option>
                        <option>3</option>
                    </select>
                </div>
                <div class="form-group">
                    <label for="MoTa">Mô tả:</label>
                    <input type="text" class="form-control" id="MoTa">
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                <button type="button" class="btn btn-default" id="btnCapNhat" onclick=" QuanLyKhoaThi.update();">Cập nhật</button>
            </div>
        </div>

    </div>
</div>
<!-- Modal xoa -->
<div id="myModal1" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="lblThongBaoXoa">Bạn có chắc chắn muốn xóa ?</h4>
                <div id="edit_header_ThongBao1"></div>
            </div>
            <div class="modal-body" style="text-align: center;">
                <button type="button" class="btn btn-default" id="btnKhong" data-dismiss="modal">Không</button>
                <button type="button" class="btn btn-danger" id="btnXoa" onclick=" QuanLyKhoaThi.delete();">Thực hiện</button>
            </div>
        </div>
    </div>
</div>

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
    var QuanLyKhoaThi = {
        ID: -1,
        Status: 1,
        Data: [],
        url: "/handler/nuce.ad.thichungchi/",
        initData: function () {
            $.getJSON(this.url + "ad_tcc_getmonthi.ashx", function (data) {
                var strHtml = "";
                $.each(data, function (i, item) {
                    strHtml += "<option value=\"" + item.ID + "\">" + item.Ten + "</option>";
                });
                $("#slMonThi").html(strHtml);
            });
        },
        bindData: function () {
            $('#tbContent').html("");
            $.getJSON(this.url + "ad_tcc_getKhoaThi.ashx", function (data) {
                //alert(data);
                QuanLyKhoaThi.Data = data;
                var strHtml = "";
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td>" + item.Ten + "</td>";
                    strHtml += "<td>" + item.TenMonThi + "</td>";
                    var trangthai = item.Status;
                    var strTrangThai = "";
                    var strHtmlDongMo = "";
                    if (trangthai < 3) {
                        strTrangThai = "Đang mở";
                        strHtmlDongMo = "<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"QuanLyKhoaThi.initDelete(" + item.ID + ",3,'Bạn có chắc chắn muốn đóng khóa thi?');\">Đóng</button></td>";
                    }
                    else if (trangthai == 3) {
                        strTrangThai = "Đóng";
                        strHtmlDongMo = "<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"QuanLyKhoaThi.initDelete(" + item.ID + ",3,'Bạn có chắc chắn muốn mở khóa thi?');\">Mở</button></td>";
                    }
                    strHtml += "<td>" + strTrangThai + "</td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"QuanLyKhoaThi.fillData(" + item.ID + ");\">Sửa</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"QuanLyKhoaThi.add();\">Thêm mới</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"QuanLyKhoaThi.initDelete(" + item.ID + ",4,'Bạn có chắc chắn muốn xóa ?');\">Xóa</button></td>";

                    strHtml += strHtmlDongMo;
                    strHtml += "</tr>";

                });
                $('#tbContent').html(strHtml);
            });
        },
        add: function () {
            QuanLyKhoaThi.ID = -1;
            $("#edit_header").html("Thêm mới");
            $("#edit_header_ThongBao").hide();
            $("#Ma").val("");
            $("#Ten").val("");
            $("#MoTa").val("");
        },
        fillData: function (id) {
            QuanLyKhoaThi.ID = id;
            $("#edit_header").html("Sửa thông tin");
            $("#edit_header_ThongBao").hide();

            $.getJSON(this.url + "ad_tcc_getchitietKhoaThi.ashx?ID=" + id, function (data) {
                //alert(data[0].ID);
                $.each(data, function (i, item) {
                    $("#slMonThi").val(item.MonThiID);
                    $("#Ten").val(item.Ten);
                    $("#MoTa").val(item.MoTa);
                });
            });

        },
        update: function () {
            // Kiem tra ma trang
            strMonThi = $("#slMonThi").val().trim();
            strTen = $("#Ten").val().trim();
            strMoTa = $("#MoTa").val().trim();
            if (strMonThi == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Bạn hãy lựa chọn môn thi");
                return;
            }
            if (strTen == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Không được để tên trắng");
                return;
            }
            if (QuanLyKhoaThi.ID > 0) {
                //Update
                $.getJSON(this.url + "ad_tcc_updateKhoaThi.ashx?ID=" + QuanLyKhoaThi.ID + "&&MonThi=" + strMonThi + "&&Ten=" + strTen + "&&MoTa=" + strMoTa, function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        QuanLyKhoaThi.bindData();
                    }
                    else {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }
            else {
                $.getJSON(this.url + "ad_tcc_insertKhoaThi.ashx?ID=" + QuanLyKhoaThi.ID + "&&MonThi=" + strMonThi + "&&Ten=" + strTen + "&&MoTa=" + strMoTa, function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        QuanLyKhoaThi.bindData();
                    }
                    else {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }

            //alert(this.Data.length);
        },
        initDelete: function (id, status, message) {
            QuanLyKhoaThi.ID = id;
            QuanLyKhoaThi.Status = status;
            $("#lblThongBaoXoa").html(message);
            $("#edit_header_ThongBao1").hide();
            $('#btnXoa').show();
            //$("#btnXoa").on("click", QuanLyKhoaThi.delete);
        },
        delete: function () {
            //Update
            $.getJSON(this.url + "ad_tcc_deleteKhoaThi.ashx?ID=" + QuanLyKhoaThi.ID + "&&Status=" + QuanLyKhoaThi.Status, function (data) {
                if (data == 1) {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Thực hiện thành công");
                    $('#btnKhong').text("Thoát");
                    $('#btnXoa').hide();
                    QuanLyKhoaThi.bindData();
                }
                else {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Lỗi hệ thống");
                }
            });
        }
    };
    QuanLyKhoaThi.initData();
    QuanLyKhoaThi.bindData();
</script>
