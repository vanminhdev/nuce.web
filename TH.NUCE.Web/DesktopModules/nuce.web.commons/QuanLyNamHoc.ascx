<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyNamHoc.ascx.cs" Inherits="nuce.web.commons.QuanLyNamHoc" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td>
                <asp:Label ID="lblTen" runat="server" Text="Tên"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTen" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
        <tr>
            <td>
                <asp:Label ID="lblNgayBatDau" runat="server" Text="Ngày bắt đầu: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNgayBatDau" runat="server" Width="100px"></asp:TextBox>
                <asp:HyperLink ID="lnkNgayBatDau" runat="server">
                    <asp:Image ID="imgNgayBatDau" runat="server" resourceKey="CalendarAlt.Text" />
                </asp:HyperLink>
                (dd/mm/yyyy)
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNgayKetThuc" runat="server" Text="Ngày kết thúc: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNgayKetThuc" runat="server" Width="100px"></asp:TextBox>
                <asp:HyperLink ID="lnkNgayKetThuc" runat="server">
                    <asp:Image ID="imgNgayKetThuc" runat="server" resourceKey="CalendarAlt.Text" />
                </asp:HyperLink>
                (dd/mm/yyyy)
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
