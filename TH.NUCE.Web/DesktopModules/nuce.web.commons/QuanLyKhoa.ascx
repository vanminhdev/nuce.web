<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyDotTraoDoi.ascx.cs" Inherits="nuce.web.commons.QuanLyKhoa" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td width="15%">
                <asp:Label ID="lblMa" runat="server" Text="Ma"></asp:Label>
            </td>
            <td width="85%">
                <asp:TextBox ID="txtMa" runat="server" Width="30%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTen" runat="server" Text="Tên"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTen" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTeTiengAnh" runat="server" Text="Tên tiếng anh"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTenTiengAnh" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDiaChi" runat="server" Text="Địa chỉ"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDiaChi" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMoTa" runat="server" Text="Mô tả"></asp:Label>
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
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
    <asp:DropDownList ID="ddlTruong" runat="server">
        <asp:ListItem Selected="True" Value="1">Nuce</asp:ListItem>
    </asp:DropDownList>
</div>
