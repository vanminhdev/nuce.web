<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DoiMatKhau.ascx.cs" Inherits="nuce.web.sinhvien.DoiMatKhau" %>
<div class="content-fn">
     <table style="width: 500px; margin: 60 auto;">
        <tr>
            <td colspan="2" runat="server" id="tdAnnounce" style="color: red; text-align: center;"></td>
        </tr>
        <tr>
            <td style="width: 180px; text-align: left">Mật khẩu cũ</td>
            <td  style="width: 320px;">
                <asp:TextBox runat="server" ID="txtMauKhauOld" TextMode="Password" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 180px; text-align: left">Mật khẩu mới </td>
            <td>
                <asp:TextBox runat="server" ID="txtMauKhauNew" TextMode="Password" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 180px; text-align: left">Mật khẩu mới (nhập lại)</td>
            <td>
                <asp:TextBox runat="server" ID="txtMauKhauNewRepeat" TextMode="Password" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: right;">
                <asp:Button runat="server" Text="Cập nhật" ID="btnCapNhat" OnClick="btnCapNhat_Click" />
            </td>
        </tr>
    </table>
</div>
