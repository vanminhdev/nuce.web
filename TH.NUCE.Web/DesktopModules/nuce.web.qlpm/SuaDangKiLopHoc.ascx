<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuaDangKiLopHoc.ascx.cs" Inherits="nuce.web.qlpm.SuaDangKiLopHoc" %>
<div style="text-align: center; color: red;" runat="server" id="divAnnounce"></div>
<table border="1" width="100%">
    <tr>
        <td width="10%">Lớp học</td>

        <td>
            <asp:DropDownList ID="ddlLopHoc" runat="server" AutoPostBack="False"></asp:DropDownList></td>
    </tr>
    <tr>
        <td>Ghi chú</td>
        <td><asp:textbox ID="txtGhiChu" runat="server"></asp:textbox></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật" OnClick="btnCapNhat_Click" />
            <asp:Button ID="btnHuy" runat="server" Text="Hủy đăng kí" OnClick="btnHuy_Click" />
            <asp:Button ID="btnChuyenDiemDanh" runat="server" Text="Chuyển điểm danh" OnClick="btnChuyenDiemDanh_Click" />
        </td>
    </tr>
</table>
<div style="display: none;">
    <asp:textbox ID="txtID" runat="server"></asp:textbox>
    <asp:textbox ID="txtType" runat="server"></asp:textbox>
</div>
