<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StudentGiaDinhEdit.ascx.cs" Inherits="nuce.ad.ctsv.StudentGiaDinhEdit" %>
<div class="row" style="padding-top: 5px; text-align: center; font-weight: bold;" runat="server" id="divSinhVien">
</div>
<div class="table-responsive">
    <table class="table">
        <thead>
            <tr>
                <th style="text-align: center; vertical-align: top;">Mối quan hệ</th>
                <th style="text-align: center; vertical-align: top;">Họ và tên</th>
                <th style="text-align: center; vertical-align: top;">Năm sinh</th>
                <th style="text-align: center; vertical-align: top;">Quốc tịch</th>
                <th style="text-align: center; vertical-align: top;">Dân tộc</th>
                <th style="text-align: center; vertical-align: top;">Tôn giáo</th>
                <th style="text-align: center; vertical-align: top;">Nghề nghiệp</th>
                <th style="text-align: center; vertical-align: top;">Chức vụ</th>
                <th style="text-align: center; vertical-align: top;">Nơi công tác</th>
                <th style="text-align: center; vertical-align: top;">Nơi ở hiện tại</th>
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
    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal" onclick="StudentGiaDinhEdit.add();">Thêm mới</button>
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
                    <label for="MoiQuanHe">Mối quan hệ:</label>
                    <input type="text" class="form-control" id="MoiQuanHe">
                </div>
                <div class="form-group">
                    <label for="HoVaTen">Họ và tên:</label>
                    <input type="text" class="form-control" id="HoVaTen">
                </div>
                <div class="form-group">
                    <label for="NamSinh">Nam Sinh:</label>
                    <input type="text" class="form-control" id="NamSinh">
                </div>
                <div class="form-group">
                    <label for="QuocTich">Quốc tịch:</label>
                    <input type="text" class="form-control" id="QuocTich">
                </div>
                <div class="form-group">
                    <label for="DanToc">Dân tộc:</label>
                    <input type="text" class="form-control" id="DanToc">
                </div>
                <div class="form-group">
                    <label for="TonGiao">Tôn giáo:</label>
                    <input type="text" class="form-control" id="TonGiao">
                </div>
                <div class="form-group">
                    <label for="NgheNghiep">Nghề nghiệp:</label>
                    <input type="text" class="form-control" id="NgheNghiep">
                </div>
                <div class="form-group">
                    <label for="ChucVu">Chức vụ:</label>
                    <input type="text" class="form-control" id="ChucVu">
                </div>
                <div class="form-group">
                    <label for="NoiCongTac">Nơi công tác:</label>
                    <input type="text" class="form-control" id="NoiCongTac">
                </div>
                <div class="form-group">
                    <label for="NoiOHienTai">Nơi ở hiện tại:</label>
                    <input type="text" class="form-control" id="NoiOHienTai">
                </div>
                <div class="form-group">
                    <label for="Count">Thứ tự:</label>
                    <input type="text" class="form-control" id="Count">
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                <button type="button" class="btn btn-default" id="btnCapNhat" onclick=" StudentGiaDinhEdit.update();">Cập nhật</button>
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
                <button type="button" class="btn btn-danger" id="btnXoa" onclick=" StudentGiaDinhEdit.delete();">Thực hiện</button>
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
    var StudentGiaDinhEdit = {
        ID: -1,
        Status: 1,
        Data: [],
        url: "/handler/nuce.ad.ctsv/",
        initData: function () {
        },
        bindData: function () {
            $('#tbContent').html("");
            $.getJSON(this.url + "ad_ctsv_studentgiadinhget.ashx?StudentID=" + StudentID, function (data) {
                //alert(data);
                StudentGiaDinhEdit.Data = data;
                var strHtml = "";
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td>" + item.MoiQuanHe + "</td>";
                    strHtml += "<td>" + item.HoVaTen + "</td>";
                    strHtml += "<td>" + item.NamSinh + "</td>";
                    strHtml += "<td>" + item.QuocTich + "</td>";
                    strHtml += "<td>" + item.DanToc + "</td>";
                    strHtml += "<td>" + item.TonGiao + "</td>";
                    strHtml += "<td>" + item.NgheNghiep + "</td>";
                    
                    strHtml += "<td>" + item.ChucVu + "</td>";
                    strHtml += "<td>" + item.NoiCongTac + "</td>";
                    strHtml += "<td>" + item.NoiOHienNay + "</td>";
                    strHtml += "<td>" + item.Count + "</td>";
                    var trangthai = item.Status;
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"StudentGiaDinhEdit.fillData(" + item.ID + ");\">Sửa</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"StudentGiaDinhEdit.add();\">Thêm mới</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"StudentGiaDinhEdit.initDelete(" + item.ID + ",4,'Bạn có chắc chắn muốn xóa ?');\">Xóa</button></td>";
                    strHtml += "</tr>";

                });
                $('#tbContent').html(strHtml);
            });
        },
        add: function () {
            StudentGiaDinhEdit.ID = -1;
            $("#edit_header").html("Thêm mới");
            $("#edit_header_ThongBao").hide();
            $("#MoiQuanHe").val("");
            $("#HoVaTen").val("");
            $("#NamSinh").val("");
            $("#QuocTich").val("");
            $("#DanToc").val("");
            $("#TonGiao").val("");
            $("#ChucVu").val("");
            $("#NoiCongTac").val("");
            $("#NoiOHienTai").val("");
            $("#Count").val("");
        },
        fillData: function (id) {
            StudentGiaDinhEdit.ID = id;
            $("#edit_header").html("Sửa thông tin");
            $("#edit_header_ThongBao").hide();

            $.getJSON(this.url + "ad_ctsv_studentgiadinhgetchitiet.ashx?ID=" + id, function (data) {
                //alert(data[0].ID);
                $.each(data, function (i, item) {
                    $("#MoiQuanHe").val(item.MoiQuanHe);
                    $("#HoVaTen").val(item.HoVaTen);
                    $("#NamSinh").val(item.NamSinh);
                    $("#QuocTich").val(item.QuocTich);
                    $("#DanToc").val(item.DanToc);
                    $("#TonGiao").val(item.TonGiao);
                    $("#ChucVu").val(item.ChucVu);
                    $("#NoiCongTac").val(item.NoiCongTac);
                    $("#NoiOHienTai").val(item.NoiOHienTai);
                    $("#Count").val(item.Count);
                });
            });

        },
        update: function () {
            // Kiem tra ma trang
            var strMoiQuanHe = $("#MoiQuanHe").val().trim();
            var strHoVaTen = $("#HoVaTen").val().trim();
            var strNamSinh = $("#NamSinh").val().trim();
            var strQuocTich = $("#QuocTich").val().trim();
            var strDanToc = $("#DanToc").val().trim();
            var strTonGiao = $("#TonGiao").val().trim();
            var strChucVu = $("#ChucVu").val().trim();
            var strNgheNghiep = $("#NgheNghiep").val().trim();
            var strNoiCongTac = $("#NoiCongTac").val().trim();
            var strNoiOHienTai = $("#NoiOHienTai").val().trim();
            var strCount = $("#Count").val().trim();
            if (strMoiQuanHe == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Không được để mối quan hệ trắng");
                return;
            }
            if (strHoVaTen == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Không được để họ và tên trắng");
                return;
            }
            if (StudentGiaDinhEdit.ID > 0) {
                //Update
                $.getJSON(this.url + "ad_ctsv_studentgiadinhupdate.ashx?ID=" + StudentGiaDinhEdit.ID + "&&StudentID=" + StudentID + "&&StudentCode=" + StudentCode + "&&MoiQuanHe=" + strMoiQuanHe + "&&HoVaTen=" + strHoVaTen + "&&NamSinh=" + strNamSinh + "&&QuocTich=" + strQuocTich + "&&DanToc=" + strDanToc + "&&TonGiao=" + strTonGiao + "&&NgheNghiep=" + strNgheNghiep + "&&ChucVu=" + strChucVu + "&&NoiCongTac=" + strNoiCongTac + "&&NoiOHienTai=" + strNoiOHienTai + "&&Count=" + strCount, function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        StudentGiaDinhEdit.bindData();
                    }
                    else {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }
            else {
                $.getJSON(this.url + "ad_ctsv_studentgiadinhinsert.ashx?ID=" + StudentGiaDinhEdit.ID + "&&StudentID=" + StudentID + "&&StudentCode=" + StudentCode + "&&MoiQuanHe=" + strMoiQuanHe + "&&HoVaTen=" + strHoVaTen + "&&NamSinh=" + strNamSinh + "&&QuocTich=" + strQuocTich + "&&DanToc=" + strDanToc + "&&TonGiao=" + strTonGiao + "&&NgheNghiep=" + strNgheNghiep + "&&ChucVu=" + strChucVu + "&&NoiCongTac=" + strNoiCongTac + "&&NoiOHienTai=" + strNoiOHienTai + "&&Count=" + strCount, function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        StudentGiaDinhEdit.bindData();
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
            StudentGiaDinhEdit.ID = id;
            StudentGiaDinhEdit.Status = status;
            $("#lblThongBaoXoa").html(message);
            $("#edit_header_ThongBao1").hide();
            $('#btnXoa').show();
            //$("#btnXoa").on("click", StudentGiaDinhEdit.delete);
        },
        delete: function () {
            //Update
            $.getJSON(this.url + "ad_ctsv_studentsgiadinhdelete.ashx?ID=" + StudentGiaDinhEdit.ID + "&&Status=" + StudentGiaDinhEdit.Status, function (data) {
                if (data == 1) {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Thực hiện thành công");
                    $('#btnKhong').text("Thoát");
                    $('#btnXoa').hide();
                    StudentGiaDinhEdit.bindData();
                }
                else {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Lỗi hệ thống");
                }
            });
        }
    };
    StudentGiaDinhEdit.initData();
    StudentGiaDinhEdit.bindData();
</script>
