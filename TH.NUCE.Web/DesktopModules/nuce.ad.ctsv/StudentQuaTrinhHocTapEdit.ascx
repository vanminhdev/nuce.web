<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StudentQuaTrinhHocTapEdit.ascx.cs" Inherits="nuce.ad.ctsv.StudentQuaTrinhHocTapEdit" %>
<div class="row" style="padding-top: 5px; text-align: center; font-weight: bold;" runat="server" id="divSinhVien">
</div>
<div class="table-responsive">
    <table class="table">
        <thead>
            <tr>
                <th style="text-align: center; vertical-align: top;">Thời gian</th>
                <th style="text-align: center; vertical-align: top;">Tên trường</th>
                <th style="text-align: center; vertical-align: top;">Thứ tự</th>
                <th style="width: 5%; text-align: center; vertical-align: top;"></th>
                <th style="width: 5%; text-align: center; vertical-align: top;"></th>
                <th style="width: 5%; text-align: center; vertical-align: top;"></th>
            </tr>
        </thead>
        <tbody id="tbContent">
        </tbody>
    </table>
</div>
<div class="row" style="padding: 5px; text-align: right; font-weight: bold;">
    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal" onclick="StudentQuaTrinhHocTapEdit.add();">Thêm mới</button>
</div>
<span id="spThamSo" runat="server"></span>
<!-- Modal them sua -->
<div id="myModal" class="modal fade" role="dialog">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="edit_header" style="text-align:center;">Modal Header</h4>
                <div id="edit_header_ThongBao" style="color: red;"></div>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="ThoiGian">Thời gian:</label>
                    <input type="text" class="form-control" id="ThoiGian">
                </div>
                <div class="form-group">
                    <label for="TenTruong">Tên trường:</label>
                    <input type="text" class="form-control" id="TenTruong">
                </div>
                <div class="form-group">
                    <label for="Count">Thứ tự:</label>
                    <input type="text" class="form-control" id="Count">
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                <button type="button" class="btn btn-default" id="btnCapNhat" onclick=" StudentQuaTrinhHocTapEdit.update();">Cập nhật</button>
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
                <h4 class="modal-title" id="lblThongBaoXoa" style="text-align:center;">Bạn có chắc chắn muốn xóa ?</h4>
                <div id="edit_header_ThongBao1"></div>
            </div>
            <div class="modal-body" style="text-align: center;">
                <button type="button" class="btn btn-default" id="btnKhong" data-dismiss="modal">Không</button>
                <button type="button" class="btn btn-danger" id="btnXoa" onclick=" StudentQuaTrinhHocTapEdit.delete();">Thực hiện</button>
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
    var StudentQuaTrinhHocTapEdit = {
        ID: -1,
        Status: 1,
        Data: [],
        url: "/handler/nuce.ad.ctsv/",
        initData: function () {
        },
        bindData: function () {
            $('#tbContent').html("");
            $.getJSON(this.url + "ad_ctsv_studentsquatrinhhoctapget.ashx?StudentID=" + StudentID, function (data) {
                //alert(data);
                StudentQuaTrinhHocTapEdit.Data = data;
                var strHtml = "";
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td>" + item.ThoiGian + "</td>";
                    strHtml += "<td>" + item.TenTruong + "</td>";
                    strHtml += "<td>" + item.Count + "</td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"StudentQuaTrinhHocTapEdit.fillData(" + item.ID + ");\">Sửa</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"StudentQuaTrinhHocTapEdit.add();\">Thêm mới</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"StudentQuaTrinhHocTapEdit.initDelete(" + item.ID + ",4,'Bạn có chắc chắn muốn xóa ?');\">Xóa</button></td>";
                    strHtml += "</tr>";

                });
                $('#tbContent').html(strHtml);
            });
        },
        add: function () {
            StudentQuaTrinhHocTapEdit.ID = -1;
            $("#edit_header").html("Thêm mới");
            $("#edit_header_ThongBao").hide();
            $("#ThoiGian").val("");
            $("#TenTruong").val("");
            $("#Count").val("");
        },
        fillData: function (id) {
            StudentQuaTrinhHocTapEdit.ID = id;
            $("#edit_header").html("Sửa thông tin");
            $("#edit_header_ThongBao").hide();

            $.getJSON(this.url + "ad_ctsv_studentsquatrinhhoctapgetchitiet.ashx?ID=" + id, function (data) {
                //alert(data[0].ID);
                $.each(data, function (i, item) {
                    $("#ThoiGian").val(item.ThoiGian);
                    $("#TenTruong").val(item.TenTruong);
                    $("#Count").val(item.Count);
                });
            });

        },
        update: function () {
            // Kiem tra ma trang
            var strThoiGian = $("#ThoiGian").val().trim();
            var strTenTruong = $("#TenTruong").val().trim();
            var strCount = $("#Count").val().trim();
            if (strThoiGian == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Không được để Thời gian trắng");
                return;
            }
            if (strTenTruong == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Không được để Tên trường trắng");
                return;
            }
            if (StudentQuaTrinhHocTapEdit.ID > 0) {
                //Update
                $.getJSON(this.url + "ad_ctsv_studentsquatrinhhoctapupdate.ashx?ID=" + StudentQuaTrinhHocTapEdit.ID + "&&StudentID=" + StudentID + "&&StudentCode=" + StudentCode + "&&ThoiGian=" + strThoiGian + "&&TenTruong=" + strTenTruong + "&&Count=" + strCount, function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        StudentQuaTrinhHocTapEdit.bindData();
                    }
                    else {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }
            else {
                $.getJSON(this.url + "ad_ctsv_studentsquatrinhhoctapinsert.ashx?ID=" + StudentQuaTrinhHocTapEdit.ID + "&&StudentID=" + StudentID + "&&StudentCode=" + StudentCode + "&&ThoiGian=" + strThoiGian + "&&TenTruong=" + strTenTruong + "&&Count=" + strCount, function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        StudentQuaTrinhHocTapEdit.bindData();
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
            StudentQuaTrinhHocTapEdit.ID = id;
            StudentQuaTrinhHocTapEdit.Status = status;
            $("#lblThongBaoXoa").html(message);
            $("#edit_header_ThongBao1").hide();
            $('#btnXoa').show();
            //$("#btnXoa").on("click", StudentQuaTrinhHocTapEdit.delete);
        },
        delete: function () {
            //Update
            $.getJSON(this.url + "ad_ctsv_studentsquatrinhhoctapdelete.ashx?ID=" + StudentQuaTrinhHocTapEdit.ID + "&&Status=" + StudentQuaTrinhHocTapEdit.Status, function (data) {
                if (data == 1) {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Thực hiện thành công");
                    $('#btnKhong').text("Thoát");
                    $('#btnXoa').hide();
                    StudentQuaTrinhHocTapEdit.bindData();
                }
                else {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Lỗi hệ thống");
                }
            });
        }
    };
    StudentQuaTrinhHocTapEdit.initData();
    StudentQuaTrinhHocTapEdit.bindData();
</script>
