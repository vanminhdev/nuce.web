<%@ Page Title="Đăng nhập" Language="C#" MasterPageFile="~/Site.Login.Master" Async="true" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Nuce.CTSV.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        form.user .form-control-user {
            padding: .375rem .75rem;
            font-size: 1rem;
            color: #fff;
        }
    </style>
    <div class="row justify-content-center" style="position: relative">
        <div id="overlay"></div>
        <div class="col-12">
            <div class="main-name font-20-sm font-32-md font-42 text-light">
                hệ thống đăng ký thủ tục hành chính sinh viên online
            </div>
        </div>
        <div id="login-container" class="col-12 col-md-6">
            <a
                href="/Extent/LoginWithGoogle"
                style="display: inline-block"
                class="btn background-main-color w-100 text-light font-14-sm font-18 login-google-btn">
                <i class="fab fa-google-f fa-fw"></i>
                    Đăng nhập qua email @nuce.edu.vn
            </a>
            <div class="row extra-part text-light">
            <div class="col-5 pr-0"><hr class="extra-line" /></div>
            <div class="col-2 pl-0 pr-0 text-center font-18">Hoặc</div>
            <div class="col-5 pl-0"><hr class="extra-line" /></div>
            </div>
            <div class="input-group">
                <label for="username" class="w-100 font-13-sm text-light"
                    >Tên đăng nhập</label
                >
                <asp:TextBox runat="server" ID="txtMaDangNhap" 
                    class="form-control custom-input"
                    name="username"
                    placeholder="Tên đăng nhập"></asp:TextBox>
            </div>
            <div class="input-group mb-3">
                <label for="password" class="w-100 font-13-sm text-light"
                    >Mật khẩu</label
                >
                <asp:TextBox runat="server" ID="txtMatKhau" 
                        class="form-control custom-input"
                        type="password" name="password"
                        TextMode="Password" placeholder="Mật khẩu"></asp:TextBox>
            </div>
            <div class="checkbox mb-2">
                <input
                    type="checkbox"
                    id="isRememberPassword"
                    name="isRememberPassword"
                />
                <label for="isRememberPassword "></label>
                <label for="isRememberPassword" class="ml-2 text-light">
                    Ghi nhớ mật khẩu
                </label>
            </div>
            <asp:Button
                class="btn w-100 text-light font-14-sm login-btn"
                runat="server" 
                ID="btnDangNhap" 
                type="button"
                Text="Đăng nhập" OnClick="btnDangNhap_Click" />
        </div>
        <!-- MODAL THONG BAO-->
        <div
        class="modal fade"
        id="myModalThongBao"
        tabindex="-1"
        role="dialog"
        aria-labelledby="exampleModalLabel"
        aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel1">Thông báo</h5>
                    <button
                        class="close"
                        type="button"
                        data-dismiss="modal"
                        aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body" id="divThongBaoCapNhat" runat="server">
                    
                </div>
                <div class="modal-footer">
                    <button
                        class="btn btn-secondary"
                        type="button"
                        data-dismiss="modal">
                        Thoát
                    </button>
                </div>
            </div>
        </div>
    </div>
        <!-- TOAST -->
        <%--<div aria-live="polite" aria-atomic="true" 
            class="d-flex justify-content-center align-items-center custom-toast">
          <!-- Then put toasts within -->
          <div class="toast" role="alert" 
              aria-live="assertive" aria-atomic="true"
              data-autohide="false">
            <div class="toast-header">
              <strong class="mr-auto">Thông báo</strong>
              <button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="toast-body" id="divThongBaoCapNhat" runat="server">
              Hello, world! This is a toast message.
            </div>
          </div>
        </div>--%>
    </div>
    <hr />
    <span runat="server" id="spAlert"></span>
    <script>
        $(document).ready(function () {
            $("#MainContent_txtMaDangNhap").addClass("form-control");
            $("#MainContent_txtMaDangNhap").addClass("form-control-user");
            $("#MainContent_txtMatKhau").addClass("form-control");
            $("#MainContent_txtMatKhau").addClass("form-control-user");
            $('<%= spAlert.ClientID %>').html('');
            
            /*if(($("#myModalThongBao").data('bs.modal') || {})._isShown) {
                $('#myModalThongBao').hide();
            }*/
        });
    </script>
</asp:Content>
