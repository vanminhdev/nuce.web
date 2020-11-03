<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyDichVu_CapLaiTheSinhVien.ascx.cs" Inherits="nuce.ad.ctsv.QuanLyDichVu_CapLaiTheSinhVien" %>
<link id="bsdp-css" href="/bootstrap-datepicker/css/bootstrap-datepicker3.min.css" rel="stylesheet">
<script src="/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
<script src="/bootstrap-datepicker/locales/bootstrap-datepicker.uk.min.js" charset="UTF-8"></script>
<script src="/bootstrap-datepicker/locales/bootstrap-datepicker.vi.min.js" charset="UTF-8"></script>
<script src="/Resources/Shared/nuce/core.js" charset="UTF-8"></script>
<style>
    .loader {
        display: none;
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(255,255,255,0.8) url(/images/loader.gif) center center no-repeat;
        z-index: 10000;
    }
</style>
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
                <th style="text-align: center; vertical-align: top;">STT</th>
                <th style="text-align: center; vertical-align: top;">Thời gian gửi yêu cầu</th>
                <th style="text-align: center; vertical-align: top;">Tên SV</th>
                <th style="text-align: center; vertical-align: top;">Mã SV</th>
                <th style="text-align: center; vertical-align: top;">Nội dung</th>
                <th style="text-align: center; vertical-align: top;">Trạng thái</th>
                <th style="text-align: center; vertical-align: top;"></th>
                <th style="text-align: center; vertical-align: top;"></th>
            </tr>
        </thead>
        <tbody id="tbContent">
        </tbody>
    </table>
</div>
<span id="spThamSo" runat="server"></span>
<span id="spScriptInit" runat="server"></span>
<span id="spScript" runat="server"></span>
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
                    <label for="TrangThai">Trạng thái:</label>
                    <select class="form-control" id="TrangThai" onchange="QuanLyDichVu_CapLaiTheSinhVien.changeTrangThai();">
                        <option value="3">Đã tiếp nhận và đang xử lý</option>
                        <option value="4">Đã xử lý và có lịch hẹn</option>
                        <option value="5">Từ chối dịch vụ</option>
                    </select>
                </div>

                <div style="padding-bottom: 0px;" id="divNgayHen">
                    <table class="table">
                        <tr>
                            <td colspan="6" style="font-weight: bold;">Thời gian hẹn sinh viên:</td>
                        </tr>
                        <tr>
                            <td colspan="6" style="font-weight: bold;">Ngày giờ bắt đầu:</td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; border: none; text-align: center; vertical-align: middle;">ngày (dd/MM/yyyy): </td>
                            <td style="width: 30%; border: none;">
                                <input type="text" class="form-control" id="txtNgayBatDau"></td>
                            <td style="font-weight: bold; width: 8%; border: none; text-align: center; vertical-align: middle;">Giờ:</td>
                            <td style="width: 12%; border: none;">
                                <input type="text" class="form-control" id="txtGioBatDau"></td>
                            <td style="font-weight: bold; width: 8%; border: none; text-align: center; vertical-align: middle;">Phút:</td>
                            <td style="width: 12%; border: none;">
                                <input type="text" class="form-control" id="txtPhutBatDau"></td>
                        </tr>
                        <tr>
                            <td colspan="6" style="font-weight: bold;">Ngày giờ kết thúc</td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; width: 30%; border: none; text-align: center; vertical-align: middle;">ngày (dd/MM/yyyy): </td>
                            <td style="width: 30%; border: none;">
                                <input type="text" class="form-control" id="txtNgayKetThuc"></td>
                            <td style="font-weight: bold; width: 8%; border: none; text-align: center; vertical-align: middle;">Giờ:</td>
                            <td style="width: 12%; border: none;">
                                <input type="text" class="form-control" id="txtGioKetThuc"></td>
                            <td style="font-weight: bold; width: 8%; border: none; text-align: center; vertical-align: middle;">Phút:</td>
                            <td style="width: 12%; border: none;">
                                <input type="text" class="form-control" id="txtPhutKetThuc"></td>
                        </tr>
                    </table>
                </div>
                <div class="form-group" id="divPhanHoi">
                    <label for="PhanHoi">Phản hồi:</label>
                    <textarea class="form-control" id="PhanHoi"></textarea>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                <button type="button" class="btn btn-default" id="btnCapNhat" onclick=" QuanLyDichVu_CapLaiTheSinhVien.update();">Cập nhật</button>
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
                <h4 class="modal-title" id="lblThongBaoXoa" style="text-align:center;">Bạn có chắc chắn muốn thực hiện ?</h4>
                <div id="edit_header_ThongBao1"></div>
            </div>
            <div class="modal-body" style="text-align: center;">
                <button type="button" class="btn btn-default" id="btnKhong" data-dismiss="modal">Không</button>
                <button type="button" class="btn btn-danger" id="btnXoa" onclick=" QuanLyDichVu_CapLaiTheSinhVien.delete();">Thực hiện</button>
            </div>
        </div>
    </div>
</div>
<div id="myModalThongBao" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="h_noidung_thongbao" style="text-align:center;"></h4>
            </div>
            <div class="modal-body" style="text-align: center;">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
            </div>
        </div>
    </div>
</div>
<div class="loader"></div>
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
    var QuanLyDichVu_CapLaiTheSinhVien = {
        ID: -1,
        Status: 1,
        Type: 3,
        Data: [],
        url: "/handler/nuce.ad.ctsv/",
        initData: function () {
        },
        bindData: function () {
            $('#tbContent').html("");
            $.getJSON(this.url + "ad_ctsv_qldv_xacnhan_get.ashx?Type=" + QuanLyDichVu_CapLaiTheSinhVien.Type + "&&search=" + $('#myInput').val(), function (data) {
                //alert(data);
                QuanLyDichVu_CapLaiTheSinhVien.Data = data;
                var strHtml = "";
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td style=\"text-align: center; vertical-align: top;\">" + (i + 1) + "</td>";
                    strHtml += "<td style=\"text-align: center; vertical-align: top;\">" + item.Ngay + "</td>";
                    strHtml += "<td style=\"text-align: center; vertical-align: top;\">" + item.StudentName + "</td>";
                    strHtml += "<td style=\"text-align: center; vertical-align: top;\">" + item.StudentCode + "</td>";
                    var strLydo = item.LyDo;
                    switch (item.Status) {
                        case 2:
                            strHtml += "<td style=\"text-align: left; vertical-align: top;\"><div>" + strLydo + "</div></td>";
                            strHtml += "<td style='font-weight: bold;color:green;text-align: center; vertical-align: top;'>Gửi yêu cầu lên nhà trường</td>";
                            break;
                        case 3:
                            strHtml += "<td style=\"text-align: left; vertical-align: top;\"><div>" + strLydo + "</div></td>";
                            strHtml += "<td style='font-weight: bold;color:blue;text-align: center; vertical-align: top;'>Đã tiếp nhận và đang xử lý</td>";
                            break;
                        case 4:
                            strHtml += "<td style=\"text-align: left; vertical-align: top;\"><div>" + strLydo + "</div><div style='color:blue;'>* Ngày hẹn: Từ  <b>" + item.NgayHen_BatDau_Gio + "</b> giờ - <b>" + item.NgayHen_BatDau_Phut + "</b> phút - Ngày <b>" + item.NgayHen_BatDau_Ngay + "</b></br>Đến <b>" + item.NgayHen_KetThuc_Gio + "</b> giờ - <b>" + item.NgayHen_KetThuc_Phut + "</b> phút - Ngày <b>" + item.NgayHen_KetThuc_Ngay + " </b></div></td>";
                            strHtml += "<td style='font-weight: bold;color:#e2a535;text-align: center; vertical-align: top;'>Đã xử lý và có lịch hẹn</td>";
                            break;
                        case 5:
                            strHtml += "<td style=\"text-align: left; vertical-align: top;\"><div>" + strLydo + "</div><div style='color:red;'>- Phản hồi: " + item.PhanHoi + "</div></td>";
                            strHtml += "<td style='font-weight: bold;color:red;text-align: center; vertical-align: top;'>Từ chôi dịch vụ</td>";
                            break;
                        case 6:
                            strHtml += "<td style=\"text-align: left; vertical-align: top;\"><div>" + strLydo + "</div><div style='color:blue;'>* Ngày hẹn: Từ  <b>" + item.NgayHen_BatDau_Gio + "</b> giờ - <b>" + item.NgayHen_BatDau_Phut + "</b> phút - Ngày <b>" + item.NgayHen_BatDau_Ngay + "</b></br>Đến <b>" + item.NgayHen_KetThuc_Gio + "</b> giờ - <b>" + item.NgayHen_KetThuc_Phut + "</b> phút - Ngày <b>" + item.NgayHen_KetThuc_Ngay + " </b></div></td>";
                            strHtml += "<td style='font-weight: bold;color:black;text-align: center; vertical-align: top;'>Hoàn thành</td>";
                            break;
                        default: break;
                    }
                    if (item.Status < 4 && item.Status > 1) {
                        strHtml += "<td style=\"text-align: center; vertical-align: top;\"><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"QuanLyDichVu_CapLaiTheSinhVien.fillData(" + item.ID + ");\">Chuyển trạng thái</button></td>";
                    }
                    else {
                        strHtml += "<td></td>";
                    }
                    strHtml += "<td style=\"text-align: center; vertical-align: top;\"><button type=\"button\" class=\"btn btn-primary\"  onclick=\"QuanLyDichVu_CapLaiTheSinhVien.print('" + item.ID + "');\">In</button></td>";
                    strHtml += "</tr>";

                });
                $('#tbContent').html(strHtml);
            });
        },
        fillData: function (id) {
            QuanLyDichVu_CapLaiTheSinhVien.ID = id;
            $("#edit_header").html("Chuyển trạng thái");
            $("#edit_header_ThongBao").hide();
            $.each(QuanLyDichVu_CapLaiTheSinhVien.Data, function (i, item) {
                if (item.ID == id) {
                    $("#PhanHoi").val(item.PhanHoi);
                    $("#TrangThai").val(item.Status);
                    $("#txtNgayBatDau").val(item.NgayHen_BatDau_Ngay);
                    $("#txtGioBatDau").val(item.NgayHen_BatDau_Gio);
                    $("#txtPhutBatDau").val(item.NgayHen_BatDau_Phut);

                    $("#txtNgayKetThuc").val(item.NgayHen_KetThuc_Ngay);
                    $("#txtGioKetThuc").val(item.NgayHen_KetThuc_Gio);
                    $("#txtPhutKetThuc").val(item.NgayHen_KetThuc_Phut);

                    QuanLyDichVu_CapLaiTheSinhVien.showHideFormTrangThai(item.Status);
                }

            });
        },
        showHideFormTrangThai: function (input) {
            $("#divNgayHen").hide();
            if (input == 5) {
                $("#divPhanHoi").show();
                //$("#divNgayHen").hide();
            }
            else {
                $("#divPhanHoi").hide();
                //$("#divNgayHen").show();
            }
        },
        changeTrangThai: function () {
            QuanLyDichVu_CapLaiTheSinhVien.showHideFormTrangThai($("#TrangThai").val());
        },
        update: function () {
            // Kiem tra ma trang
            var strPhanHoi = $("#PhanHoi").val().trim();
            var strStatus = $("#TrangThai").val();
            if (strStatus == -1) {
                if (($("#txtNgayBatDau").val() == "" || $("#txtNgayKetThuc").val() == "")) {
                    $("#edit_header_ThongBao").show();
                    $("#edit_header_ThongBao").html("Bạn hãy chọn thời gian hẹn");
                    return;
                }
                else {
                    if (convertNumberFromText($("#txtNgayBatDau").val()) >= convertNumberFromText($("#txtNgayKetThuc").val())) {
                        //$("#txtNgayKetThuc").focus();
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Bạn hãy chọn ngày hẹn KẾT THÚC lớn hơn ngày hẹn BẮT ĐẦU");
                        return;
                    }
                }
                if ($("#txtGioBatDau").val() == "") {
                    $("#txtGioBatDau").val("00");
                }
                else {
                    var hh = parseInt($("#txtGioBatDau").val());
                    if (isNaN(hh))
                        hh = 0;
                    if (hh < 0 || hh > 24)
                        $("#txtGioBatDau").val("00");
                    else if (hh < 10)
                        $("#txtGioBatDau").val("0" + hh);
                }
                if ($("#txtPhutBatDau").val() == "") {
                    $("#txtPhutBatDau").val("00");
                }
                else {
                    var mm = parseInt($("#txtPhutBatDau").val());
                    if (isNaN(mm))
                        mm = 0;
                    if (mm < 0 || mm > 60)
                        $("#txtPhutBatDau").val("00");
                    else if (mm < 10)
                        $("#txtPhutBatDau").val("0" + mm);
                }
                if ($("#txtGioKetThuc").val() == "") {
                    $("#txtGioKetThuc").val("00");
                }
                else {
                    var hh = parseInt($("#txtGioKetThuc").val());
                    if (isNaN(hh))
                        hh = 0;
                    if (hh < 0 || hh > 24)
                        $("#txtGioKetThuc").val("00");
                    else if (hh < 10)
                        $("#txtGioKetThuc").val("0" + hh);
                }
                if ($("#txtPhutKetThuc").val() == "") {
                    $("#txtPhutKetThuc").val("00");
                }
                else {
                    var mm = parseInt($("#txtPhutKetThuc").val());
                    if (isNaN(mm))
                        mm = 0;
                    if (mm < 0 || mm > 60)
                        $("#txtPhutKetThuc").val("00");
                    else if (mm < 10)
                        $("#txtPhutKetThuc").val("0" + mm);
                }
            }
            var strNgayBatDau = "01/01/1990 00:00";
            var strNgayKetThuc = "01/01/1990 00:00";
            if ($("#txtNgayBatDau").val() != "")
                strNgayBatDau = $("#txtNgayBatDau").val() + " " + $("#txtGioBatDau").val() + ":" + $("#txtPhutBatDau").val();
            if ($("#txtNgayKetThuc").val() != "")
                strNgayKetThuc = $("#txtNgayKetThuc").val() + " " + $("#txtGioKetThuc").val() + ":" + $("#txtPhutKetThuc").val();
            if (strStatus == null && strStatus != "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Bạn hãy chọn trạng thái chuyển");
                return;
            }
            $('.loader').show();
            $('#myModal').modal('hide')

            if (QuanLyDichVu_CapLaiTheSinhVien.ID > 0) {
                //Update
                $.getJSON(this.url + "ad_ctsv_qldv_xacnhan_changestatus.ashx?Type=" + QuanLyDichVu_CapLaiTheSinhVien.Type + "&&ID=" + QuanLyDichVu_CapLaiTheSinhVien.ID + "&&STATUS=" + strStatus + "&&PhanHoi=" + strPhanHoi + "&&NgayBatDau=" + strNgayBatDau + "&&NgayKetThuc=" + strNgayKetThuc, function (data) {
                    $('.loader').hide();
                    if (data == 1) {
                        $("#h_noidung_thongbao").html("Cập nhật thành công");
                        $('#myModalThongBao').modal();
                        QuanLyDichVu_XacNhan.bindData();
                    }
                    else {
                        $('#myModal').modal();
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }
        }
        ,
        print: function (input) {
            window.location.href = '/tabid/' + tabid + '/default.aspx?act=print&&search=' + input;
        }
    };
    QuanLyDichVu_CapLaiTheSinhVien.initData();
    QuanLyDichVu_CapLaiTheSinhVien.bindData();
</script>
<script>
    $(document).ready(function () {
        $(document).keypress(function (event) {

            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                QuanLyDichVu_CapLaiTheSinhVien.bindData();
                event.preventDefault();
            }

        });
        var date_input_ngaybatdau = $('#txtNgayBatDau'); //our date input has the name "date"
        var date_input_ngayketthuc = $('#txtNgayKetThuc'); //our date input has the name "date"
        var options = {
            format: 'dd/mm/yyyy',
            //    container: container,
            todayHighlight: true,
            autoclose: true,
        };
        date_input_ngaybatdau.datepicker(options);
        date_input_ngayketthuc.datepicker(options);
    });
</script>

