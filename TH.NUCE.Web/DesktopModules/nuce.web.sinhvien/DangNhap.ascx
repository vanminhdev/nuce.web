<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DangNhap.ascx.cs" Inherits="nuce.web.sinhvien.DangNhap" %>
<div class="content-fn">
    <table  style="width: 450px; margin: 60 auto; border-collapse: separate;">
        <tr>
            <td colspan="2" runat="server" id="tdAnnounce" style="color:red; text-align:center;">

            </td>
        </tr>
        <tr style="padding-bottom:10px;">
            <td style="width:150px; text-align:left">Tên đăng nhập</td>
            <td style="width:300px;">
                <asp:TextBox runat="server" ID="txtTen" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width:150px; text-align:left">Mật khẩu</td>
            <td>
                <asp:TextBox runat="server" ID="txtMauKhau" TextMode="Password" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:right; padding-right:10px;">
                <asp:Button runat="server" Text="Đăng nhập"  ID="btnDangNhap" OnClick="btnDangNhap_Click"/>
            </td>
        </tr>
    </table>
</div>
