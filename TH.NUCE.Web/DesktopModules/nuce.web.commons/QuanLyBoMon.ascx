<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyBoMon.ascx.cs" Inherits="nuce.web.commons.QuanLyBoMon" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter" runat="server">
    <span>Chọn khoa: </span>
    <span><asp:DropDownList ID="ddlKhoa" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlKhoa_SelectedIndexChanged">
                </asp:DropDownList></span>
</div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td>Khoa: 
            </td>
            <td>
                <div runat="server" id="divNameKhoa" style="font-weight: bold; color: red;"></div>
            </td>
        </tr>
        <tr>
            <td width="15%">
                <asp:Label ID="lblMa" runat="server" Text="Mã : "></asp:Label>
            </td>
            <td width="85%">
                <asp:TextBox ID="txtMa" runat="server" Width="30%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTen" runat="server" Text="Tên : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTen" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTeTiengAnh" runat="server" Text="Tên tiếng anh : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTenTiengAnh" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDiaChi" runat="server" Text="Địa chỉ : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDiaChi" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMoTa" runat="server" Text="Mô tả : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMoTa" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnQuayLai" runat="server" Text="Quay lại" OnClick="btnQuayLai_Click" />

            </td>
        </tr>
    </table>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtKhoaID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

</div>


<script>
    var quanlybomon = {
        url: "/handler/nuce.web.commons/",
        m_bomonid: -1,
        m_ten: '',
        showdetailcanbotrongbomon: function(bomonid, ten) {
            m_bomonid = bomonid;
            m_ten = ten;
            this.display();
        },
        display:function (){
        // alert(bomonid);
            //lấy danh sách cán bộ trong bộ môn
            $.getJSON(this.url + "GetNguoiDungByBoMon.ashx?bomonid=" + m_bomonid + "&&role=-1", function (data) {
                var strTable = "<div>" +
                    "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sách cán bộ trong bộ môn - " + m_ten + "</div>" +
                    "<div class='welcome_b'></div>" +
                    "<div class='nd fl'>" +
                    "<div class='noidung'>";
                strTable += "<div class='nd_t fl'>";
                strTable += "<div class='nd_t1 fl'>STT</div>";
                strTable += "<div class='nd_t1 fl'>Username</div>";
                strTable += "<div class='nd_t2 fl'>DisplayName</div>";
                strTable += "<div class='nd_t4 fl'>Xóa</div>";
                strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.themmoi();'>Thêm mới</a></div>";
                strTable += "</div>";
                strTable += "<div class='nd_b fl'>";
                $.each(data, function (i, item) {
                    strTable += " <div class='nd_b1 fl'>";
                    strTable += "<div class='nd_t1 fl'>" + (i + 1) + "</div>";
                    strTable += "<div class='nd_t1 fl'>" + item.Username + "</div>";
                    strTable += "<div class='nd_t2 fl'>" + item.DisplayName + "</div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.xoa(" + item.NguoiDung_BoMonID + ")'>Xóa</a></div>";
                    strTable += "<div class='nd_t4 fl'><a href='javascript:quanlybomon.themmoi();'>Thêm mới</a></div>";
                    strTable += "</div>";
                });
                strTable += "</div>";
                strTable += "</div>";
                strTable += "</div>";
                strTable += "</div>";
                $("#divGiaoVienTrongBoMon").html(strTable);
            });
        },
        xoa: function (nguoidung_bomonid) {
            var str = "nguoidung_bomonid=" + nguoidung_bomonid + "&status=4";
            var url = quanlybomon.url + "UpdateNguoDungBoMonStatus.ashx?" + str;
            $.get(url, function (data) {
                if (data = "1") {
                    alert("Cập nhật thành công");
                    quanlybomon.display();
                } else {
                    alert("Cập nhật thất bại");
                }
            });
        },
        themmoi: function() {
            $.getJSON(this.url + "GetNguoiDungByBoMon.ashx?bomonid=-1&&role=8", function (data) {
                var strTable = "<div style='text-align: center;'> Chọn người dùng: <select id='slUser'>";
                $.each(data, function (i, item) {
                    strTable += "<option value='" + item.UserID+ "'>";
                    strTable += item.DisplayName;
                    strTable += "</option>";
                });
                strTable += "</select>";
                strTable += '<button type="button" id="btnUpdate" onclick="quanlybomon.capnhat();">Thêm vào bộ môn</button><button type="button" id="btnQuayLai" onclick="quanlybomon.display();">Quay lại</button></div>';
                $("#divGiaoVienTrongBoMon").html(strTable);
            });
        },
        capnhat:function() {
            //alert($('#slUser').val());
            var url = quanlybomon.url + "InsertNguoDungBoMon.ashx?bomonid=" + m_bomonid + "&userid=" + $('#slUser').val();
            $.get(url, function (data) {
                if (data = "1") {
                    alert("Thêm mới thành công");
                    quanlybomon.display();
                } else {
                    alert("Thêm mới thất bại");
                }
            });
        }
    };
</script>