<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyMonHoc.ascx.cs" Inherits="nuce.web.commons.QuanLyMonHoc" %>
<div id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter">
    <span>Chọn khoa: </span>
    <span id="spKhoa">
        <select id="slKhoa">
        </select>
    </span>
    <span>Chọn bộ môn: </span>
    <span id="spBoMon">
        <select id="slBoMon">
        </select>
    </span>
</div>
<div id="divContent"></div>
<div id="divEdit" style="padding-top: 30px;">
    <table width="100%">
        <tr style="display: none;">
            <td width="15%">Môn học: 
            </td>
            <td width="85%">
                <input type="text" name="monhocid" id="txtMonHocID" />
            </td>
        </tr>
        <tr style="display: none;">
            <td width="15%">Bộ môn: 
            </td>
            <td width="85%">
                <input type="text" name="bomonid" id="txtBoMonID" />
            </td>
        </tr>
        <tr>
            <td width="15%">Mã: 
            </td>
            <td width="85%">
                <input type="text" name="ma" id="txtMa" />
            </td>
        </tr>
        <tr>
            <td>Tên:
            </td>
            <td>
                <input type="text" name="ten" id="txtTen" />
            </td>
        </tr>
        <tr>
            <td>Tên tiếng anh: 
            </td>
            <td>
                <input type="text" name="tentienganh" id="txtTenTiengAnh"/>
            </td>
        </tr>
        <tr>
            <td>Số tín chỉ:
            </td>
            <td>
                <input type="text" name="sotinchi" id="txtSoTinChi" value="2" onblur="Format_onblur(this,0);"/>
            </td>
        </tr>
        <tr>
            <td>Mô tả:
            </td>
            <td>
                <textarea style="width: 250px; height: 150px;" name="mota" id="txtMoTa"></textarea>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <button type="button" id="btnUpdate" onclick="quanlybomon.capnhat();">Cập nhật</button>
                <button type="button" id="btnQuayLai" onclick="quanlybomon.quaylai();">Quay lại</button>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <input type="text" id="txtType" value="insert" />
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
    $("#slKhoa").change(function () {
        //alert($('#slKhoa').val());
        quanlybomon.initBoMon($('#slKhoa').val());
    });
    $("#slBoMon").change(function () {
        //alert($('#slKhoa').val());
        quanlybomon.bindata($('#slBoMon').val());
    });

    var quanlybomon = {
        url: "/handler/nuce.web.commons/",
        initBoMon: function (khoaid) {
            $('#slBoMon').html("");
            $.getJSON(this.url + "GetBoMonByKhoa.ashx?khoaid=" + khoaid, function (data) {
                //alert(data);
                $.each(data, function (i, item) {
                    if (i == 0) { quanlybomon.bindata(item.BoMonID); }
                    $('#slBoMon').append($('<option>', {
                        value: item.BoMonID,
                        text: item.Ten
                    }));
                });
            });
        },
        init: function () {
            $('#divEdit').hide();
            //Tao commbox khoa
            $.getJSON(this.url + "GetKhoa.ashx", function (data) {
                //alert(data);
                $.each(data, function (i, item) {
                    if (i == 0) { quanlybomon.initBoMon(item.KhoaID); }
                    $('#slKhoa').append($('<option>', {
                        value: item.KhoaID,
                        text: item.Ten
                    }));
                });
            });
        },
        bindata: function (bomonid) {
            $("#txtBoMonID").val(bomonid);
            $.getJSON(this.url + "GetMonHocByBoMon.ashx?bomonid=" + bomonid, function (data) {
                //alert(data);
                var strTable = "<div>" +
                    "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách môn học</div>" +
                    "<div class='welcome_b'></div>" +
                    "<div class='nd fl'>" +
                    "<div class='noidung'>";
                strTable += "<div class='nd_t fl'>";
                strTable += "<div class='nd_t1 fl'>STT</div>";
                strTable += "<div class='nd_t1 fl'>Mã</div>";
                strTable += "<div class='nd_t22 fl'>Tên</div>";
                strTable += "<div class='nd_t33 fl'>Số tín chỉ</div>";
                strTable += "<div class='nd_t33 fl'>Giảng viên</div>";
                strTable += "<div class='nd_t4 fl'>Chỉnh sửa</div>";
                strTable += "<div class='nd_t4 fl'>Xóa</div>";
                strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.themmoi()'>Thêm mới</a></div>";
                strTable += "</div>";
                strTable += "<div class='nd_b fl'>";
                $.each(data, function (i, item) {
                    strTable += " <div class='nd_b1 fl'>";
                    strTable += "<div class='nd_t1 fl'>" + (i + 1) + "</div>";
                    strTable += "<div class='nd_t1 fl'>" + item.Ma + "</div>";
                    strTable += "<div class='nd_t22 fl'>" + item.Ten + "</div>";
                    strTable += "<div class='nd_t33 fl'>" + item.SoTinChi + "</div>";
                    strTable += "<div class='nd_t33 fl'><a href='javascript:quanlybomon.showgiangvien(" + item.MonHocID;
                    strTable += ',"' + item.Ten + '"';
                    strTable += ")'>Giảng viên</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.sua(" + item.MonHocID + ")'>Sửa</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.xoa(" + item.MonHocID + ")'>Xóa</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.themmoi()'>Thêm mới</a></div>";
                    strTable += "</div>";
                });
                strTable += "</div>";
                strTable += "</div>";
                strTable += "</div>";
                strTable += "</div>";
                strTable += "<div  class='nd fl' id='divGiaoVienGiangDayMon'><div>";
                $("#divContent").html(strTable);
            });
        },
        themmoi: function () {
            $("#divAnnouce").html("Thêm mới môn học");
            $('#divEdit').show();
            $('#divContent').hide();
            ;
            $("#slKhoa").attr('disabled', 'disabled');
            $("#slBoMon").attr('disabled', 'disabled');
            $("#txtType").val("insert");
            quanlybomon.resetForm();
        }
        ,
        sua: function (monhocid) {
            $("#divAnnouce").html("Chỉnh sửa thông tin");
            $('#divEdit').show();
            $('#divContent').hide();
            $("#txtType").val("update");

            $("#slKhoa").attr('disabled', 'disabled');
            $("#slBoMon").attr('disabled', 'disabled');

            $("#txtMonHocID").val(monhocid);

            $.getJSON(this.url + "GetMonHoc.ashx?monhocid=" + monhocid, function (data) {
                $("#txtMa").val(data[0].Ma);
                $("#txtTen").val(data[0].Ten);
                $("#txtTenTiengAnh").val(data[0].TenTiengAnh);
                $("#txtSoTinChi").val(data[0].SoTinChi);
                $("#txtMoTa").val(data[0].MoTa);
            });
        }
        ,
        xoa: function (monhocid) {
            var str = "monhocid=" + monhocid + "&status=4";
            var url = quanlybomon.url + "UpdateMonHocStatus.ashx?" + str;
            $.get(url, function (data) {
                if (data = "1") {
                    alert("Xóa thành công");
                    quanlybomon.quaylai();
                } else {
                    alert("Xóa thất bại");
                }
            });
        },
        capnhat: function () {
            // Kiem tra lai tham so co de ma va ten trang khong
            if ($("#txtMa").val().trim() == "" || $("#txtTen").val().trim()=="") {
                alert("Không được để mã hoặc tên trắng");
            }
            else
            {
                    var str = $("#divEdit").serializeAny();
                    var strType = $("#txtType").val();
                    var url = quanlybomon.url + "InsertMonHoc.ashx?"+str;
                    if (strType == "update") {
                        url = quanlybomon.url + "UpdateMonHoc.ashx?"+str;
                    }
                    $.get(url, function (data) {
                        if (data = "1") {
                            alert("Cập nhật thành công");
                            quanlybomon.quaylai();
                        } else {
                            alert("Cập nhật thất bại");
                        }
                    });
            }
        },
        quaylai: function () {
            $("#divAnnouce").html("");
            $('#divEdit').hide();
            $('#divContent').show();
            ;
            $("#slKhoa").removeAttr('disabled');
            $("#slBoMon").removeAttr('disabled');
            quanlybomon.bindata($("#txtBoMonID").val());
        },
        resetForm:function() {
            $("#txtMa").val("");
            $("#txtTen").val("");
            $("#txtTenTiengAnh").val("");
            $("#txtSoTinChi").val(2);
            $("#txtMoTa").val("");
        },
        m_monhocid: -1,
        m_tenmonhoc:'',
        showgiangvien:function(monhocID,Ten) {
            m_monhocid = monhocID;
            m_tenmonhoc = Ten;
            this.displaygiangvien();
        },
        displaygiangvien: function () {
            // alert(bomonid);
            //lấy danh sách cán bộ trong bộ môn
            $.getJSON(this.url + "GetNguoiDungByMonHoc.ashx?monhocid=" + m_monhocid + "&&role=-1", function (data) {
                var strTable = "<div>" +
                    "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách giang viên giảng dạy môn - " + m_tenmonhoc + "</div>" +
                    "<div class='welcome_b'></div>" +
                    "<div class='nd fl'>" +
                    "<div class='noidung'>";
                strTable += "<div class='nd_t fl'>";
                strTable += "<div class='nd_t1 fl'>STT</div>";
                strTable += "<div class='nd_t1 fl'>Username</div>";
                strTable += "<div class='nd_t2 fl'>DisplayName</div>";
                strTable += "<div class='nd_t4 fl'>Xóa</div>";
                strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.themmoigiangvien();'>Thêm mới</a></div>";
                strTable += "</div>";
                strTable += "<div class='nd_b fl'>";
                $.each(data, function (i, item) {
                    strTable += " <div class='nd_b1 fl'>";
                    strTable += "<div class='nd_t1 fl'>" + (i + 1) + "</div>";
                    strTable += "<div class='nd_t1 fl'>" + item.Username + "</div>";
                    strTable += "<div class='nd_t2 fl'>" + item.DisplayName + "</div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.xoagiangvien(" + item.NguoiDung_MonHocID + ")'>Xóa</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.themmoigiangvien();'>Thêm mới</a></div>";
                    strTable += "</div>";
                });
                strTable += "</div>";
                strTable += "</div>";
                strTable += "</div>";
                strTable += "</div>";
                $("#divGiaoVienGiangDayMon").html(strTable);
            });
        },
        themmoigiangvien:function() {
            $.getJSON(this.url + "GetNguoiDungByBoMon.ashx?bomonid="+$("#txtBoMonID").val()+"&&role=7", function (data) {
                var strTable = "<div style='text-align: center;'> Chọn người dùng: <select id='slUser'>";
                $.each(data, function (i, item) {
                    strTable += "<option value='" + item.UserID + "'>";
                    strTable += item.DisplayName;
                    strTable += "</option>";
                });
                strTable += "</select>";
                strTable += '<button type="button" id="btnUpdate" onclick="quanlybomon.capnhatgiangvien();">Thêm giảng dạy môn ' + m_tenmonhoc + '</button><button type="button" id="btnQuayLai" onclick="quanlybomon.displaygiangvien();">Quay lại</button></div>';
                $("#divGiaoVienGiangDayMon").html(strTable);
            });
        },
        capnhatgiangvien:function() {
            var url = quanlybomon.url + "InsertNguoDungMonHoc.ashx?monhocid=" + m_monhocid + "&userid=" + $('#slUser').val();
            $.get(url, function (data) {
                if (data = "1") {
                    alert("Thêm mới thành công");
                    quanlybomon.displaygiangvien();
                } else {
                    alert("Thêm mới thất bại");
                }
            });
        },
        xoagiangvien:function(nguoidung_monhocid) {
            var str = "nguoidung_monhocid=" + nguoidung_monhocid + "&status=4";
            var url = quanlybomon.url + "UpdateNguoDungMonHocStatus.ashx?" + str;
            $.get(url, function (data) {
                if (data = "1") {
                    alert("Cập nhật thành công");
                    quanlybomon.displaygiangvien();
                } else {
                    alert("Cập nhật thất bại");
                }
            });
        }
    };
    quanlybomon.init();
</script>
