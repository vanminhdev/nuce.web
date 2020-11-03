<%@ control language="C#" autoeventwireup="true" codebehind="QLKyThi.ascx.cs" inherits="nuce.ad.thichungchi.QLKyThi" %>
<div class="table-responsive">
    <table class="table">
        <thead>
            <tr>
                <th>#</th>
                <th>Tên</th>
                <th>Bộ đề</th>
                <th>Trạng thái</th>
                <th style="width: 5%"></th>
                <th style="width: 5%;"></th>
                <th style="width: 5%;"></th>
                <th style="width: 5%;"></th>
               <%-- <th style="width: 5%;"></th>--%>
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
                    <label for="BoDe">Bộ đề :</label>
                    <select class="form-control" name="BoDe" id="slBoDe">
                        <option>1</option>
                        <option>2</option>
                        <option>3</option>
                    </select>
                </div>
                <div class="form-group">
                    <label for="MoTa">Phòng thi:</label>
                    <input type="text" class="form-control" id="PhongThi">
                </div>
                <div class="form-group">
                    <label for="MoTa">Mô tả:</label>
                    <input type="text" class="form-control" id="MoTa">
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                <button type="button" class="btn btn-default" id="btnCapNhat" onclick=" QuanLyKyThi.update();">Cập nhật</button>
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
                <button type="button" class="btn btn-danger" id="btnXoa" onclick=" QuanLyKyThi.delete();">Thực hiện</button>
            </div>
        </div>
    </div>
</div>
<!-- Modal Danh sach sinh vien -->
<div id="myModal2" class="modal fade" role="dialog">
    <div class="modal-dialog" style="width: 100%;">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <table class="table table-bordered">
                    <tbody>
                        <tr>
                            <td style="width: 47%; font-weight: bold; color: blue;">Danh sách tất cả các học viên</td>
                            <td style="vertical-align: middle;" class="align-middle" rowspan="3">
                                <div style="height: 100%; align-items: center;">
                                    <button type="button" class="btn" onclick="QuanLyKyThi.ChuyenSangThi();">>>></button>
                                    <div style="height: 5px;"></div>
                                    <button type="button" class="btn" onclick="QuanLyKyThi.ChuyenSangKhongThi();"><<<</button>
                                </div>
                            </td>
                            <td style="width: 47%; font-weight: bold; color: dodgerblue;">Danh sách học viên trong kì thi</td>
                        </tr>
                        <tr>
                            <td>
                                <div class="row" style="padding-top: 5px;">
                                    <div class="col-sm-6">
                                        <input class="form-control" id="myInput1" type="text" placeholder="Search..">
                                    </div>
                                    <div class="col-sm-6">
                                        <select class="form-control" id="slDanhMucSearch1" style="width: 200px;" onchange='QuanLyKyThi.changeSlDanhMucSearch1(this.value)'>
                                        </select>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="row" style="padding-top: 5px;">
                                    <div class="col-sm-6">
                                        <input class="form-control" id="myInput2" type="text" placeholder="Search..">
                                    </div>
                                    <div class="col-sm-6">
                                        <select class="form-control" id="slDanhMucSearch2" style="width: 200px;" onchange='QuanLyKyThi.changeSlDanhMucSearch2(this.value)'>
                                        </select>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="table-responsive">
                                    <table class="table" style="font-size: smaller">
                                        <thead>
                                            <tr>
                                                <th>
                                                    <input type="checkbox" onclick="QuanLyKyThi.checkAll(1);" id="chbAll1" value="all1">
                                                </th>
                                                <th>Mã</th>
                                                <th>Họ và tên</th>
                                                <th>CMT</th>
                                            </tr>
                                        </thead>
                                        <tbody id="tbContent1">
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <div class="table-responsive">
                                    <table class="table" style="font-size: smaller">
                                        <thead>
                                            <tr>
                                                <th>
                                                    <input style="width: 5px; height: 5px;" type="checkbox" onclick="QuanLyKyThi.checkAll(2);" id="chbAll2" value="all2">
                                                </th>
                                                <th>Mã</th>
                                                <th>Họ và tên</th>
                                                <th>CMT</th>
                                            </tr>
                                        </thead>
                                        <tbody id="tbContent2">
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="modal-body" style="text-align: center;">
            </div>
        </div>
    </div>
</div>
<!-- Theo gioi ki thi -->
<div id="myModal3" class="modal fade" role="dialog">
    <div class="modal-dialog" style="width: 100%;">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <div class="btn-group" style="border: 1px; padding-bottom: 5px;">
                    <button type="button" class="btn btn-default">Nộp bài thi</button>
                    <button type="button" class="btn btn-default">Thi lại</button>
                    <button type="button" class="btn btn-default">Tải danh sách lớp</button>
                    <button type="button" class="btn btn-default">Tải danh sách điểm</button>
                    <button type="button" class="btn btn-default">Tải phiếu điểm</button>
                </div>
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th style="text-align: center;">
                                <input type="checkbox" onclick="QuanLyKyThi.TheoGioiKiThi_checkAll(1);" id="tgkt_chbAll1" value="tgkt_chbAll1"></th>
                            <th style="text-align: center;">Mã</th>
                            <th style="text-align: center;">Họ và tên</th>
                            <th style="text-align: center;">Mã đề thi</th>
                            <th style="text-align: center;">Điểm</th>
                            <th style="text-align: center;">Tình trạng</th>
                        </tr>
                    </thead>
                    <tbody id="tbContent_tgkt">
                    </tbody>
                </table>
            </div>
            <div class="modal-body" style="text-align: center;">
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
    var QuanLyKyThi = {
        ID: -1,
        Status: 1,
        DataDanhMuc:[],
        Data: [],
        url: "/handler/nuce.ad.thichungchi/",
        initData: function () {
            $.getJSON(this.url + "ad_tcc_getdanhmuc.ashx", function (data) {
                QuanLyKyThi.DataDanhMuc = data;
                var strHtmlSearch = "<option value=\"-1\">Lựa chọn Danh mục</option>";
                $.each(data, function (i, item) {
                    strHtmlSearch += "<option value=\"" + item.ID + "\">" + item.Ten + "</option>";
                });
                $("#slDanhMucSearch1").html(strHtmlSearch);
                $("#slDanhMucSearch2").html(strHtmlSearch);
            });


            $.getJSON(this.url + "ad_tcc_getBoDe.ashx?search=-1", function (data) {
                var strHtml = "";
                $.each(data, function (i, item) {
                    strHtml += "<option value=\"" + item.BoDeID + "\">" + item.Ten + "</option>";
                });
                $("#slBoDe").html(strHtml);
            });
        },
        changeSlDanhMucSearch1: function (giatri) {
            QuanLyKyThi.searchDanhSachThiSinh();
        },
        changeSlDanhMucSearch2: function (giatri) {
            QuanLyKyThi.searchDanhSachThiSinh();
        },
        bindData: function () {
            $('#tbContent').html("");
            $.getJSON(this.url + "ad_tcc_getkythi.ashx?search=-1", function (data) {
                QuanLyKyThi.Data = data;
                var strHtml = "";
                var count = 1;
                $.each(data, function (i, item) {
                    var status = item.Status;
                    strHtml += "<tr>";
                    strHtml += "<td>" + count + "</td>";
                    strHtml += "<td>" + item.Ten + "</td>";
                    strHtml += "<td>" + item.TenBoDe + "</td>";
                    switch (status) {
                        case 1: strHtml += "<td style='color:blue;'>Mới</td>";
                            strHtml += "<td></td>";
                            strHtml += "<td><button type=\"button\" class=\"btn btn-success\" data-toggle=\"modal\" data-target=\"#myModal2\" onclick=\"QuanLyKyThi.initQuanLyDanhSachThiSinh(" + item.KiThiID + ",2,'vào danh sách thi ?');\">Danh sách thí sinh</button></td>";
                            strHtml += "<td><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"QuanLyKyThi.initDelete(" + item.KiThiID + ",2,'Bạn có chắc chắn muốn chuyển sang thi ?');\" style=\"width: 130px;\">Chuyển thi</button></td>";
                            break;
                        case 2: strHtml += "<td style='color:red;'>Đang thi</td>";
                            strHtml += "<td><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal3\" onclick=\"QuanLyKyThi.TheoGioiKiThi_init(" + item.KiThiID + ",2,'Theo giõi kì thi ?');\">Theo giõi kì thi</button></td>";
                            strHtml += "<td><button type=\"button\" class=\"btn btn-success\" data-toggle=\"modal\" data-target=\"#myModal2\" onclick=\"QuanLyKyThi.initQuanLyDanhSachThiSinh(" + item.KiThiID + ",2,'vào danh sách thi?');\">Danh sách thí sinh</button></td>";
                            strHtml += "<td><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"QuanLyKyThi.initDelete(" + item.KiThiID + ",3,'Bạn có chắc chắn muốn chuyển sang kết thúc thi ?');\" style=\"width: 130px;\">Chuyển kết thúc</button></td>";
                            break;
                        case 3: strHtml += "<td>Kết thúc thi</td><td></td><td></td><td></td>";
                            break;
                        default: break;
                    }
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"QuanLyKyThi.fillData(" + item.KiThiID + ");\">Sửa</button></td>";
                   // strHtml += "<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"QuanLyKyThi.add();\">Thêm mới</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"QuanLyKyThi.initDelete(" + item.KiThiID + ",4,'Bạn có chắc chắn muốn xóa ?');\">Xóa</button></td>";
                    strHtml += "</tr>";
                    count++;
                });
                strHtml += "<tr>";
                strHtml += "<td colspan='9' style='text-align:right;'><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"QuanLyKyThi.add();\">Thêm mới</button></td>";
                strHtml += "</tr>";
                $('#tbContent').html(strHtml);
            });
        },
        add: function () {
            QuanLyKyThi.ID = -1;
            $("#edit_header").html("Thêm mới");
            $("#edit_header_ThongBao").hide();
            $("#Ten").val("");
            $("#MoTa").val("");
        },
        fillData: function (id) {
            QuanLyKyThi.ID = id;
            $("#edit_header").html("Sửa thông tin");
            $("#edit_header_ThongBao").hide();

            $.each(QuanLyKyThi.Data, function (i, item) {
                if (item.KiThiID == id) {
                    $("#slBoDe").val(item.BoDeID);
                    $("#Ten").val(item.Ten);
                    $("#MoTa").val(item.MoTa);
                    $("#PhongThi").val(item.PhongThi);
                }
            });

        },
        update: function () {
            // Kiem tra ma trang
            strBoDe = $("#slBoDe").val().trim();
            strTen = $("#Ten").val().trim();
            strMoTa = $("#MoTa").val().trim();
            strPhongThi = $("#PhongThi").val().trim();
            if (strBoDe == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Bạn hãy lựa chọn Bộ đề");
                return;
            }
            if (strTen == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Không được để tên trắng");
                return;
            }
            if (QuanLyKyThi.ID > 0) {
                //Update
                $.getJSON(this.url + "ad_tcc_updatekithi.ashx?ID=" + QuanLyKyThi.ID + "&&BoDe=" + strBoDe + "&&Ten=" + strTen + "&&MoTa=" + strMoTa + "&&PhongThi=" + strPhongThi, function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        QuanLyKyThi.bindData();
                    }
                    else {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }
            else {
                $.getJSON(this.url + "ad_tcc_insertkithi.ashx?ID=" + QuanLyKyThi.ID + "&&BoDe=" + strBoDe + "&&Ten=" + strTen + "&&MoTa=" + strMoTa + "&&PhongThi=" + strPhongThi, function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        QuanLyKyThi.bindData();
                    }
                    else {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }

            //alert(this.Data.length);
        },
        initQuanLyDanhSachThiSinh: function (id, status, message) {
            QuanLyKyThi.ID = id;
            QuanLyKyThi.Status = status;
            //Load du lieu danh sach thí sinh
            QuanLyKyThi.searchDanhSachThiSinh();
        },
        searchDanhSachThiSinh: function () {
            $("#chbAll1").prop("checked", false);
            $("#chbAll2").prop("checked", false);
            var search1 = $('#myInput1').val();
            var danhmuc1 = $("#slDanhMucSearch1").val();
            $.getJSON(this.url + "ad_tcc_getnguoithibykithi.ashx?ID=" + QuanLyKyThi.ID + "&&search=" + search1+"&&danhmuc="+danhmuc1, function (data) {
                var strHtml = "";
                var count = 1;
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td> <input type=\"checkbox\" name=\"v1nguoithi_" + item.ID + "\" id=\"id_" + item.ID + "\" value=\"id_" + item.ID + "\"></td>";
                    strHtml += "<td>" + item.Ma + "</td>";
                    strHtml += "<td>" + item.Ho + " " + item.Ten + "</td>";
                    strHtml += "<td>" + item.CMT + "</td>";
                    strHtml += "</tr>";
                    count++;
                });
                $('#tbContent1').html(strHtml);
            });
            var search2 = $('#myInput2').val();
            var danhmuc2 = $("#slDanhMucSearch2").val();
            $.getJSON(this.url + "ad_tcc_getnguoithitrongkithi.ashx?Type=1&&ID=" + QuanLyKyThi.ID + "&&search=" + search2+"&&danhmuc="+danhmuc2, function (data) {
                var strHtml = "";
                var count = 1;
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td> <input type=\"checkbox\" name=\"v2nguoithi_" + item.ID + "\" id=\"id_" + item.ID + "\" value=\"id_" + item.ID + "\"></td>";
                    strHtml += "<td>" + item.Ma + "</td>";
                    strHtml += "<td>" + item.Ho + " " + item.Ten + "</td>";
                    strHtml += "<td>" + item.CMT + "</td>";
                    strHtml += "</tr>";
                    count++;
                });
                $('#tbContent2').html(strHtml);
            });
        },
        initDelete: function (id, status, message) {
            QuanLyKyThi.ID = id;
            QuanLyKyThi.Status = status;
            $("#lblThongBaoXoa").html(message);
            $("#edit_header_ThongBao1").hide();
            $('#btnXoa').show();
            //$("#btnXoa").on("click", QuanLyKyThi.delete);
        },
        delete: function () {
            //Update
            $.getJSON(this.url + "ad_tcc_deletekithi.ashx?ID=" + QuanLyKyThi.ID + "&&Status=" + QuanLyKyThi.Status, function (data) {
                if (data == 1) {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Thực hiện thành công");
                    $('#btnKhong').text("Thoát");
                    $('#btnXoa').hide();
                    QuanLyKyThi.bindData();
                }
                else {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Lỗi hệ thống");
                }
            });
        },
        TheoGioiKiThi_init: function (id, status, message) {
            QuanLyKyThi.ID = id;
            QuanLyKyThi.Status = status;
            //Load du lieu danh sach thí sinh
            QuanLyKyThi.TheoGioiKiThi_searchDanhSachThiSinh();

        },
        TheoGioiKiThi_searchDanhSachThiSinh: function () {
            //Load danh sach thi sinh du thi
            $.getJSON(this.url + "ad_tcc_getnguoithitrongkithi.ashx?Type=2&&ID=" + QuanLyKyThi.ID + "&&search= &&DanhMuc=-1", function (data) {
                var strHtml = "";
                var count = 1;
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td> <input type=\"checkbox\" name=\"tgkt_v2nguoithi_" + item.ID + "\" id=\"tgkt_id_" + item.ID + "\" value=\"tgkt_id_" + item.ID + "\"></td>";
                    strHtml += "<td>" + item.Ma + "</td>";
                    strHtml += "<td>" + item.Ho + " " + item.Ten + "</td>";
                    strHtml += "<td>" + item.MaDe + "</td>";
                    if (item.Diem > -1)
                        strHtml += "<td>" + item.Diem + "</td>";
                    else
                        strHtml += "<td></td>";
                    var Status = item.Status;
                    var strStatus = "";
                    switch (Status) {
                        case 1: strStatus = "Chuẩn bị thi";
                            strHtml += "<td style='color:red;'>" + strStatus + "</td>";
                            break;
                        case 2: strStatus = "Đang thi";
                            strHtml += "<td style='color:blue;'>" + strStatus + "</td>";
                            break;
                        case 3: strStatus = "Đang tạm dừng"; 
                            strHtml += "<td style='color:green;'>" + strStatus + "</td>";
                            break;
                        case 4: strStatus = "Đã Thi xong"; 
                            strHtml += "<td style='color:black;font-wight:bold;'>" + strStatus + "</td>";
                            break;
                        default:
                            strHtml += "<td></td>";
                            break;
                    }

                    strHtml += "</tr>";
                    count++;
                });
                $('#tbContent_tgkt').html(strHtml);
            });
        },
        checkAll: function (index) {
            $("#tbContent" + index).find("input[type=checkbox]").each(function (i, ob) {
                if ($('#chbAll' + index).is(":checked")) {
                    $(ob).prop("checked", true);
                }
                else
                    $(ob).prop("checked", false);
            });
        },
        ChuyenSangThi: function () {
            var strSinhViens = '';
            $("#tbContent1").find("input:checked").each(function (i, ob) {
                var name = $(ob).val();
                if (name.indexOf("id_") >= 0) {
                    var id = ob.id.replace('id_', '');
                    //id_{0}_{1}
                    strSinhViens = strSinhViens + id + ";";
                }
            });
            if (strSinhViens != '') {
                $.getJSON(this.url + "ad_tcc_updatethisinhtrongkithi.ashx?Type=1&&ID=" + QuanLyKyThi.ID + "&&SinhVien=" + strSinhViens, function (data) {
                    QuanLyKyThi.searchDanhSachThiSinh();
                });
            }
        },
        ChuyenSangKhongThi: function () {
            var strSinhViens = '';
            $("#tbContent2").find("input:checked").each(function (i, ob) {
                var name = $(ob).val();
                if (name.indexOf("id_") >= 0) {
                    var id = ob.id.replace('id_', '');
                    //id_{0}_{1}
                    strSinhViens = strSinhViens + id + ";";
                }
            });
            if (strSinhViens != '') {
                $.getJSON(this.url + "ad_tcc_updatethisinhtrongkithi.ashx?Type=2&&ID=" + QuanLyKyThi.ID + "&&SinhVien=" + strSinhViens, function (data) {
                    QuanLyKyThi.searchDanhSachThiSinh();
                });
            }
        }
    };
    QuanLyKyThi.initData();
    QuanLyKyThi.bindData();
</script>
<script>
    $(document).ready(function () {
        $(document).keypress(function (event) {

            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                QuanLyKyThi.searchDanhSachThiSinh();
                event.preventDefault();
            }

        });
    });

</script>
