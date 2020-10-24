<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLySinhVienTrongLopHoc.ascx.cs" Inherits="nuce.web.commons.QuanLySinhVienTrongLopHoc" %>
<div id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter">
    <span>Chọn lớp học: </span>
    <span id="spLopHoc">
        <select id="slLopHoc">
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
        <tr>
            <td width="15%">Mã SV: 
            </td>
            <td width="85%">
                <input type="text" name="masv" id="txtMaSV" />
            </td>
        </tr>
        <tr>
            <td>Mô tả:
            </td>
            <td>
                <textarea style="width: 250px; height: 150px;" name="ghichu" id="txtGhiChu"></textarea>
            </td>
        </tr>
         <tr>
            <td width="15%">Thứ tự: 
            </td>
            <td width="85%">
                <input type="text" name="order" id="txtOrder"  style="width:80px;"/>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <button type="button" id="btnUpdate" onclick="quanlysinhvientronglophoc.capnhat();">Cập nhật</button>
                <button type="button" id="btnQuayLai" onclick="quanlysinhvientronglophoc.quaylai();">Quay lại</button>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <input type="text" id="txtType" value="insert" />
</div>
<span runat="server" id="spLinkImport" style="display:none;"></span>
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
    $("#slLopHoc").change(function () {
        //alert($('#slKhoa').val());
        quanlysinhvientronglophoc.bindata($('#slLopHoc').val());
    });
    var quanlysinhvientronglophoc = {
        url: "/handler/nuce.web.commons/",
        m_lophocid: -1,
        init: function () {
            $('#divEdit').hide();
            //Tao commbox lop hoc
            $.getJSON(this.url + "GetLopHocNameByStatus.ashx?status=1", function (data) {
                //alert(data);
                var lophocid= nuce.getQueryString("lophocid", "-1");
                $.each(data, function (i, item) {
                    //if (i == 0) { quanlysinhvientronglophoc.bindata(item.LopHocID); }
                    if (i == 0 && lophocid=="-1") { lophocid = item.LopHocID; }
                    $('#slLopHoc').append($('<option>', {
                        value: item.LopHocID,
                        text: item.Ten
                    }));
                });
                quanlysinhvientronglophoc.bindata(lophocid);
                setTimeout(function() {
                    $("#slLopHoc").val(lophocid);
                }, 30);
            });
        },

        bindata: function (lophocid) {
            this.m_lophocid = lophocid;
            var strUrlImport = $("#<%=spLinkImport.ClientID%>").html() + '/' + lophocid;
            $.getJSON(this.url + "GetLopHocSinhVienByLopHoc.ashx?lophocid=" + lophocid, function (data) {
                //alert(data);
                var strTable = "<div>" +
                    "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách sinh viên trong lớp (<a href='" + strUrlImport + "'>ImportExcel</a>)</div>" +
                    "<div class='welcome_b'></div>" +
                    "<div class='nd fl'>" +
                    "<div class='noidung'>";
                strTable += "<div class='nd_t fl'>";
                strTable += "<div class='nd_t1 fl'>STT</div>";
                strTable += "<div class='nd_t1 fl'>Mã</div>";
                strTable += "<div class='nd_t3 fl'>Tên</div>";
                strTable += "<div class='nd_t4 fl'>Thứ tự</div>";
                strTable += "<div class='nd_t3 fl'>Ghi chú</div>";
                strTable += "<div class='nd_t4 fl'>Có vân tay</div>";
                strTable += "<div class='nd_t4 fl'>Trạng thái</div>";
                strTable += "<div class='nd_t33 fl'>Chuyển</div>";
                strTable += "<div class='nd_t4 fl'>Chỉnh sửa</div>";
                strTable += "<div class='nd_t4 fl'>Xóa</div>";
                strTable += "<div class='nd_t4 fl'><a href='javascript:quanlysinhvientronglophoc.themmoi()'>Thêm mới</a></div>";
                strTable += "</div>";
                strTable += "<div class='nd_b fl'>";
                $.each(data, function (i, item) {
                    strTable += " <div class='nd_b1 fl'>";
                    strTable += "<div class='nd_t1 fl'>" + (i + 1) + "</div>";
                    strTable += "<div class='nd_t1 fl'>" + item.MaSV + "</div>";
                    strTable += "<div class='nd_t3 fl'>" + item.Ho + ' ' + item.Ten + "</div>";
                    strTable += "<div class='nd_t4 fl'>" + item.Order + "</div>";
                    strTable += "<div class='nd_t3 fl'>- " + item.GhiChu +" </div>";

                    var strVanTay = item.InsertedUser;
                    if(strVanTay==null)
                        strTable += "<div class='nd_t4 fl'>Chưa có</div>";
                    else strTable += "<div class='nd_t4 fl'>Đã có</div>";
                    var Status = item.trangthai;
                    if (Status == "1") {
                        strTable += "<div class='nd_t4 fl'>Hoạt động</div>";
                        strTable += "<div class='nd_t33 fl'><a href='javascript:quanlysinhvientronglophoc.chuyen(" + item.LopHoc_SinhVienID + ",3)'>Chuyển tạm dừng</a></div>";
                    } else {
                        if (Status == "3") {
                            strTable += "<div class='nd_t4 fl'>Tạm dừng</div>";
                            strTable += "<div class='nd_t33 fl'><a href='javascript:quanlysinhvientronglophoc.chuyen(" + item.LopHoc_SinhVienID + ",1)'>Chuyển hoạt động</a></div>";
                        }
                    }

                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlysinhvientronglophoc.sua(";
                    strTable += '"' + item.MaSV + '","' + item.GhiChu + '"' + ',"' + item.Order + '"';
                    strTable += ")'>Sửa</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlysinhvientronglophoc.xoa(" + item.LopHoc_SinhVienID + ")'>Xóa</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlysinhvientronglophoc.themmoi()'>Thêm mới</a></div>";
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
            $("#divAnnouce").html("Thêm mới sinh viên");
            $('#divEdit').show();
            $('#divContent').hide();
            ;
            $("#slLopHoc").attr('disabled', 'disabled');
            $("#txtType").val("insert");
            quanlysinhvientronglophoc.resetForm();
        }
        ,
        sua: function (masv,GhiChu,Order) {
            $("#divAnnouce").html("Chỉnh sửa thông tin");
            $('#divEdit').show();
            $('#divContent').hide();
            $("#txtType").val("update");

            $("#slLopHoc").attr('disabled', 'disabled');

            $("#txtMaSV").val(masv);
            $("#txtGhiChu").val(GhiChu);
            $("#txtOrder").val(Order);
        }
        ,
        xoa: function (lophoc_sinhvienid) {
            var str = "lophoc_sinhvienid=" + lophoc_sinhvienid + "&status=4";
            var url = quanlysinhvientronglophoc.url + "UpdateLopHocSinhVienStatus.ashx?" + str;
            $.get(url, function (data) {
                if (data = "1") {
                    alert("Xóa thành công");
                    quanlysinhvientronglophoc.quaylai();
                } else {
                    alert("Xóa thất bại");
                }
            });
        },
        chuyen: function (lophoc_sinhvienid, status) {
            var str = "lophoc_sinhvienid=" + lophoc_sinhvienid + "&status=" + status;
            var url = quanlysinhvientronglophoc.url + "UpdateLopHocSinhVienStatus.ashx?" + str;
            $.get(url, function (data) {
                if (data = "1") {
                    alert("Chuyển thành công");
                    quanlysinhvientronglophoc.quaylai();
                } else {
                    alert("Chuyển thất bại");
                }
            });
        },
        capnhat: function () {
            if ($("#txtMaSV").val().trim() == "") {
                alert("Không được để mã hoặc tên trắng");
            }
            else {
                $("#txtLopHocid").val(this.m_lophocid);
                var str = $("#divEdit").serializeAny();
                var strType = $("#txtType").val();
                var url = quanlysinhvientronglophoc.url + "InsertLopHocSinhVien.ashx?" + str;
//                alert(str);
                if (strType == "update") {
                    url = quanlysinhvientronglophoc.url + "InsertLopHocSinhVien.ashx?" + str;
                }
                $.get(url, function (data) {
                    if (data = "1") {
                        alert("Cập nhật thành công");
                        quanlysinhvientronglophoc.quaylai();
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
            $("#slLopHoc").removeAttr('disabled');
            quanlysinhvientronglophoc.bindata(this.m_lophocid);
        },
        resetForm: function () {
            $("#txtMaSV").val("");
            $("#txtGhiChu").val("");
            $("#txtOrder").val("1");
        }
    };
    quanlysinhvientronglophoc.init();

</script>
