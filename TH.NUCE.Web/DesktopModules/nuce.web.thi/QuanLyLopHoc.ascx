<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyLopHoc.ascx.cs" Inherits="nuce.web.thi.QuanLyLopHoc" %>
<link type="text/css" rel="Stylesheet" href="/Resources/Shared/jquery-ui-1.11.4.custom/jquery-ui.css" media="screen" />
<script type="text/javascript" src="/Resources/Shared/jquery-ui-1.11.4.custom/jquery-ui.js"></script>
<div id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter">
    <span>Chọn môn học: </span>
    <span id="spMonHoc">
        <select id="slMonHoc">
        </select>
    </span>
</div>
<div id="divContent"></div>
<div id="divEdit" style="padding-top: 30px;">
    <table width="100%">

        <tr style="display: none;">
            <td width="15%">Lớp học: 
            </td>
            <td width="85%">
                <input type="text" name="lophocid" id="txtLopHocid" />
            </td>
        </tr>
        <tr style="display: none;">
            <td width="15%">Block học: 
            </td>
            <td width="85%">
                <input type="text" name="blockid" id="txtBlockid" />
            </td>
        </tr>

        <tr style="display: none;">
            <td width="15%">Người dùng: 
            </td>
            <td width="85%">
                <input type="text" name="userid" id="txtUserid" />
            </td>
        </tr>
        <tr style="display: none;">
            <td width="15%">Môn hoc: 
            </td>
            <td width="85%">
                <input type="text" name="monhocid" id="txtMonHocid" />
            </td>
        </tr>

<%--        <tr>
            <td width="15%">Giảng viên: 
            </td>
            <td width="85%">
                <select id="slGiangVien">
                </select>
            </td>
        </tr>--%>
        <tr>
            <td width="15%">block: 
            </td>
            <td width="85%">
                <select id="slBlock">
                </select>
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
            <td>Số tín chỉ:
            </td>
            <td>
                <input type="text" name="sotinchi" id="txtSoTinChi" value="2" onblur="Format_onblur(this,0);" />
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
            <td>Ngày bắt đầu:
            </td>
            <td>
                <input type="text" name="ngaybatdau" id="txtNgayBatDau" />
            </td>
        </tr>
        <tr>
            <td>Ngày kết thúc:
            </td>
            <td>
                <input type="text" name="ngayketthuc" id="txtNgayKetThuc" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <button type="button" id="btnUpdate" onclick="quanlylophoc.capnhat();">Cập nhật</button>
                <button type="button" id="btnQuayLai" onclick="quanlylophoc.quaylai();">Quay lại</button>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <input type="text" id="txtType" value="insert" />
</div>
<script>
    $("#txtNgayBatDau").datepicker({
        todayBtn: "linked",
        language: "it",
        autoclose: true,
        todayHighlight: true,
        dateFormat: 'dd/mm/yy'
    });
    $("#txtNgayKetThuc").datepicker({
        todayBtn: "linked",
        language: "it",
        autoclose: true,
        todayHighlight: true,
        dateFormat: 'dd/mm/yy'
    });

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
    $("#slMonHoc").change(function () {
        quanlylophoc.bindata($('#slMonHoc').val());
    });
    var quanlylophoc = {
        url: "/handler/nuce.web.commons/",
        m_monhocid: -1,
        initMonHoc: function () {
            $('#slMonHoc').html("");
            $.getJSON(this.url + "GetMonHocName.ashx", function (data) {
                //alert(data);
                $.each(data, function (i, item) {
                    if (i == 0) { quanlylophoc.bindata(item.MonHocID); }
                    $('#slMonHoc').append($('<option>', {
                        value: item.MonHocID,
                        text: item.Ten
                    }));
                });
            });
        },
        init: function () {
            $('#divEdit').hide();
            //Tao combox block
            //Tao commbox khoa
            quanlylophoc.initMonHoc();
            $.getJSON(this.url + "GetBlockNameActive.ashx", function (data) {
                //alert(data);
                $.each(data, function (i, item) {
                    $('#slBlock').append($('<option>', {
                        value: item.BlockHocID,
                        text: item.TenDayDu
                    }));
                });
            });
        },

        bindata: function (monhocid) {
            this.m_monhocid = monhocid;
            $.getJSON(this.url + "GetLopHocByMonHoc.ashx?monhocid=" + monhocid, function (data) {
                //alert(data);
                var strTable = "<div>" +
                    "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách lớp học</div>" +
                    "<div class='welcome_b'></div>" +
                    "<div class='nd fl'>" +
                    "<div class='noidung'>";
                strTable += "<div class='nd_t fl'>";
                strTable += "<div class='nd_t1 fl'>STT</div>";
                strTable += "<div class='nd_t1 fl'>Mã</div>";
                //strTable += "<div class='nd_t33 fl'>Tên</div>";
                strTable += "<div class='nd_t4 fl'>Số TC</div>";
                strTable += "<div class='nd_t33 fl'>Giảng viên</div>";
                strTable += "<div class='nd_t3 fl'>Block học</div>";
                strTable += "<div class='nd_t4 fl'>Trạng thái</div>";
                strTable += "<div class='nd_t33 fl'>Chuyển</div>";
                strTable += "<div class='nd_t4 fl'>Danh sách SV</div>";
                strTable += "<div class='nd_t4 fl'>Chỉnh sửa</div>";
                strTable += "<div class='nd_t4 fl'>Xóa</div>";
                strTable += "<div class='nd_t4 fl'><a href='javascript:quanlylophoc.themmoi()'>Thêm mới</a></div>";
                strTable += "</div>";
                strTable += "<div class='nd_b fl'>";
                $.each(data, function (i, item) {
                    strTable += " <div class='nd_b1 fl'>";
                    strTable += "<div class='nd_t1 fl'>" + (i + 1) + "</div>";
                    strTable += "<div class='nd_t1 fl'>" + item.Ma + "</div>";
                    //strTable += "<div class='nd_t33 fl'>" + item.Ten + "</div>";
                    strTable += "<div class='nd_t4 fl'>" + item.SoChi + "</div>";
                    strTable += "<div class='nd_t33 fl'>" + item.DisplayName + "</div>";
                    strTable += "<div class='nd_t3 fl'>" + item.TenDayDu + "</div>";

                    var Status = item.Status;
                    if (Status == "1") {
                        strTable += "<div class='nd_t4 fl'>Hoạt động</div>";
                        strTable += "<div class='nd_t33 fl'><a href='javascript:quanlylophoc.chuyen(" + item.LopHocID + ",3)'>Chuyển tạm dừng</a></div>";
                    } else {
                        if (Status == "3") {
                            strTable += "<div class='nd_t4 fl'>Tạm dừng</div>";
                            strTable += "<div class='nd_t33 fl'><a href='javascript:quanlylophoc.chuyen(" + item.LopHocID + ",1)'>Chuyển hoạt động</a></div>";
                        }
                    }
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlylophoc.danhsachsinhvien(" + item.LopHocID + ")'>Danh sách SV</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlylophoc.sua(" + item.LopHocID + ")'>Sửa</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlylophoc.xoa(" + item.LopHocID + ")'>Xóa</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlylophoc.themmoi()'>Thêm mới</a></div>";
                    strTable += "</div>";
                });
                strTable += "</div>";
                strTable += "</div>";
                strTable += "</div>";
                strTable += "</div>";
                $("#divContent").html(strTable);
            });
        },
        themmoi: function () {
            $("#divAnnouce").html("Thêm mới lớp học");
            $('#divEdit').show();
            $('#divContent').hide();
            ;
            $("#slMonHoc").attr('disabled', 'disabled');
            $("#txtType").val("insert");
            quanlylophoc.resetForm();
        }
        ,
        sua: function (lophocid) {
            $("#divAnnouce").html("Chỉnh sửa thông tin");
            $('#divEdit').show();
            $('#divContent').hide();
            $("#txtType").val("update");

            $("#slMonHoc").attr('disabled', 'disabled');

            $("#txtLopHocid").val(lophocid);

            $.getJSON(this.url + "GetLopHoc.ashx?lophocid=" + lophocid, function (data) {
                $("#txtMa").val(data[0].Ma);
                $("#txtTen").val(data[0].Ten);
                $("#txtSoTinChi").val(data[0].SoChi);
                $("#txtMoTa").val(data[0].MoTa);
                $("#txtNgayBatDau").val(data[0].NgayBatDauvn);
                $("#txtNgayKetThuc").val(data[0].NgayKetThucvn);
                $("#slBlock").val(data[0].BlockID);
                $("#txtUserid").val(data[0].UserID);
            });
        }
        ,
        xoa: function (lophocid) {
            var str = "lophocid=" + lophocid + "&status=4";
            var url = quanlylophoc.url + "UpdateLopHocStatus.ashx?" + str;
            $.get(url, function (data) {
                if (data = "1") {
                    alert("Xóa thành công");
                    quanlylophoc.quaylai();
                } else {
                    alert("Xóa thất bại");
                }
            });
        },
        chuyen:function(lophocid, status) {
            var str = "lophocid=" + lophocid + "&status=" + status;
            var url = quanlylophoc.url + "UpdateLopHocStatus.ashx?" + str;
            $.get(url, function (data) {
                if (data = "1") {
                    alert("Chuyển thành công");
                    quanlylophoc.quaylai();
                } else {
                    alert("Chuyển thất bại");
                }
            });
        },
        capnhat: function () {
            // Kiem tra lai tham so co de ma va ten trang khong
            $("#txtBlockid").val($("#slBlock").val());
           
            //$("#txtUserid").val($("#slGiangVien").val());
            $("#txtMonHocid").val(this.m_monhocid);

            if ($("#txtMa").val().trim() == "" || $("#txtTen").val().trim() == "") {
                alert("Không được để mã hoặc tên trắng");
            }
            else {
                var str = $("#divEdit").serializeAny();
                var strType = $("#txtType").val();
                var url = quanlylophoc.url + "InsertLopHoc1.ashx?" + str;
//                alert(str);
                if (strType == "update") {
                    url = quanlylophoc.url + "UpdateLopHoc1.ashx?" + str;
                }
                $.get(url, function (data) {
                    if (data = "1") {
                        alert("Cập nhật thành công");
                        quanlylophoc.quaylai();
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
            $("#slMonHoc").removeAttr('disabled');
            quanlylophoc.bindata(this.m_monhocid);
        },
        resetForm: function () {
            $("#txtMa").val("");
            $("#txtTen").val("");
            $("#txtSoTinChi").val(2);
            $("#txtMoTa").val("");
        },
        danhsachsinhvien: function (lophocid) {
            var url = '/Home/Quan-ly-sinh-vien-trong-lop-hoc?lophocid=' + lophocid ;
            window.location.assign(url);
            //alert("chuyển đến danh sách sinh viên" + lophocid);
        }
    };
    quanlylophoc.init();

</script>
