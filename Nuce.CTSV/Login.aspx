<%@ Page Title="Đăng nhập" Language="C#" MasterPageFile="~/Site.Login.Master" Async="true" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Nuce.CTSV.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="text-center" style="font-weight:900;font-size:15px; color:RGB(45,62,153);padding:5px;">
        <div >ĐẠI HỌC XÂY DỰNG</div>
        <div>HỆ THỐNG ĐĂNG KÝ THỦ TỤC HÀNH CHÍNH SINH VIÊN ONLINE</div>
    </div>
    <div class="form-group">
        <asp:TextBox runat="server" ID="txtMaDangNhap" placeholder="Tên đăng nhập"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:TextBox runat="server" ID="txtMatKhau" TextMode="Password" placeholder="Mật khẩu"></asp:TextBox>
    </div>
  <%--  <div class="form-group">
        <div class="custom-control custom-checkbox small">
            <input
                type="checkbox"
                class="custom-control-input"
                id="customCheck" />
            <label class="custom-control-label" for="customCheck">
                Remember Me</label>
        </div>
    </div>--%>
    <asp:Button CssClass="btn btn-primary btn-user btn-block" 
                runat="server" 
                ID="btnDangNhap" 
                Text="Đăng nhập" OnClick="btnDangNhap_Click" />
    <hr />
    <a
        href="/Extent/LoginWithGoogle"
        class="btn btn-facebook btn-user btn-block">
        <i class="fab fa-google-f fa-fw"></i>Login with
                        Google
                      </a>
<%--    <div class="text-center">
        <a class="small" href="/Extent/LoginWithGoogle">Đăng nhập với Google</a>
    </div>--%>

    <span runat="server" id="spAlert"></span>
    <script>
        $(document).ready(function () {
            $("#MainContent_txtMaDangNhap").addClass("form-control");
            $("#MainContent_txtMaDangNhap").addClass("form-control-user");
            $("#MainContent_txtMatKhau").addClass("form-control");
            $("#MainContent_txtMatKhau").addClass("form-control-user");

        });
    </script>
</asp:Content>
