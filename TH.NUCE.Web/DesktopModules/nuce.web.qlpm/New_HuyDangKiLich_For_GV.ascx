<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="New_HuyDangKiLich_For_GV.ascx.cs" Inherits="nuce.web.qlpm.New_HuyDangKiLich_For_GV" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center; width: 500px;"></div>
<div style="text-align: center; font-weight: bold; width: 500px;" id="divFilter" runat="server">
    <table style="width: 600px; border: 1px; text-align: left;">
        <tr>
            <td style="width: 30%; text-align: left;">Mô tả:</td>
            <td>
                <asp:TextBox ID="txtMoTa" runat="server" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
        <tr style="display:none;">
            <td style="width: 30%; text-align: left;">Cập nhật tất cả theo mã:</td>
            <td>
                <asp:CheckBox runat="server" ID="chkCapNhatTheoMa" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" Text="Hủy lịch" ID="btnHuyLich" OnClick="btnHuyLich_Click"  />
                <asp:Button runat="server" Text="QuayLai" ID="btnQuayLai" OnClick="btnQuayLai_Click" />
            </td>
        </tr>
    </table>


</div>
<div id="divContent" runat="server"></div>
