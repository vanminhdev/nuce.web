<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyThangDiem.ascx.cs" Inherits="nuce.web.commons.QuanLyThangDiem" %>
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
</div>
