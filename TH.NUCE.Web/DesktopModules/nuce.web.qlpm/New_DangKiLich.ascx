<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="New_DangKiLich.ascx.cs" Inherits="nuce.web.qlpm.New_DangKiLich" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center; width: 500px;"></div>
<div style="text-align: center; font-weight: bold; width: 500px;" id="divFilter" runat="server">
    <table style="width: 600px; border: 1px; text-align: left;">
        <tr>
            <td style="width: 20%; text-align: left;">Mã:</td>
            <td>
                <asp:TextBox ID="txtMa" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">Ngày: </td>
            <td>
                <asp:TextBox ID="txtNgay" runat="server" Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">Phòng: </td>
            <td style="width: 25%;">
                <asp:DropDownList Enabled="false" ID="ddlPhongHoc" runat="server" AutoPostBack="false"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">CaHoc:</td>
            <td>
                <asp:TextBox ID="txtCaHoc" runat="server" Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">Mã CB:</td>
            <td>
                <asp:TextBox ID="txtMaCB" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">HoTen CB:</td>
            <td>
                <asp:TextBox ID="txtHoVaTenCB" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">Lớp:</td>
            <td>
                <asp:TextBox ID="txtLop" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">Môn học:</td>
            <td>
                <asp:TextBox ID="txtMonHoc" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">Số sinh viên:</td>
            <td>
                <asp:TextBox ID="txtSoSinhVien" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">Mô tả:</td>
            <td>
                <asp:TextBox ID="txtMoTa" runat="server" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 20%; text-align: left;">Cập nhật tất cả theo mã:</td>
            <td>
                <asp:CheckBox runat="server" ID="chkCapNhatTheoMa" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" Text="Cập nhật" ID="btnCapNhat" OnClick="btnCapNhat_Click" />
                <asp:Button runat="server" Text="QuayLai" ID="btnQuayLai" OnClick="btnQuayLai_Click" />
            </td>
        </tr>
    </table>


</div>
<div id="divContent" runat="server"></div>
