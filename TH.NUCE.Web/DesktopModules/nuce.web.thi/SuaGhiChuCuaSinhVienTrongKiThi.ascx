<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuaGhiChuCuaSinhVienTrongKiThi.ascx.cs" Inherits="nuce.web.thi.SuaGhiChuCuaSinhVienTrongKiThi" %>
<table style="width: 100%;border-collapse:separate;">
    <tr style="padding-bottom:20px;">
        <td colspan="2" style="text-align:center;font-weight:bold;color:red;">
            Chỉnh sửa thông tin ghi chú
        </td>
    </tr>
    <tr style="vertical-align: top;">
        <td style="width: 10%;">Ghi chú
        </td>
        <td style="width: 90%;">
            <asp:TextBox Style="width: 100%; height: 200px;" ID="txtGhiChu" runat="server" TextMode="MultiLine"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button runat="server" ID="btnCapNhat" Text="Cập nhật" OnClick="btnCapNhat_Click" />
            <asp:Button runat="server" ID="btnQuayLai" Text="Quay lại" OnClick="btnQuayLai_Click" />
        </td>
    </tr>
</table>
<div style="display: none;">
    <asp:TextBox ID="txtKiThiLopHocID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtPreTabid" runat="server"></asp:TextBox>
</div>

