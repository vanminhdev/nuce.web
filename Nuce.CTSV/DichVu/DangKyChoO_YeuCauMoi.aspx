<%@ Page Title="Dịch vụ sinh viên" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="DangKyChoO_YeuCauMoi.aspx.cs" Inherits="Nuce.CTSV.DangKyChoO_YeuCauMoi" %>

<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    dịch vụ
</asp:Content>

<asp:Content ID="BreadCrumContent" ContentPlaceHolderID="BreadCrum" runat="server">
    <div class="d-flex align-items-center">
        <a href="/dichvusinhvien.aspx">dịch vụ</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="d-flex align-items-center">
        <a href="/dichvu/dangkychoo.aspx">đăng ký chỗ ở</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="main-color text-decoration-none">yêu cầu mới</div>
</asp:Content>
<asp:Content ID="BreadCrumContentMobile" ContentPlaceHolderID="BreadCrumMobile" runat="server">
    <div class="d-flex align-items-center">
        <a href="/dichvusinhvien.aspx">dịch vụ</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="d-flex align-items-center">
        <a href="/dichvu/dangkychoo.aspx">đăng ký chỗ ở</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="main-color text-decoration-none">yêu cầu mới</div>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src='https://www.google.com/recaptcha/api.js'></script>
    <div class="col-sm-12 my-4">
        <div style="text-align: center; font-weight: bold; color: red;" runat="server" id="divThongBao"></div>
    </div>
    <div class="col-12">
        <div class="row justify-content-end">
        <div class="col-12">
            <div class="main-color text-uppercase font-15-sm fw-600 font-25 service-title">
                đăng ký chỗ ở
            </div>
            <div class="form-group">
                <div class="font-14-sm fw-600 font-18 mb-3">Nhu cầu nhà ở</div>
                <asp:RadioButtonList ID="radioNhuCau"
                    runat="server"
                    RepeatDirection="Vertical">
                    <asp:ListItem Value="KTX" Selected="True">&nbsp;Ký túc xá ĐHXD</asp:ListItem>
                    <asp:ListItem Value="PHAP_VAN">&nbsp;Khu nhà ở Pháp Vân</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="form-group">
                <div class="font-14-sm fw-600 font-18 mb-3">Đối tượng ưu tiên</div>
                <asp:RadioButtonList ID="radioDoiTuong"
                    runat="server"
                    RepeatDirection="Vertical">
                    <asp:ListItem Value="NHOM_1" Selected="True">&nbsp;Nhóm 1: Sinh viên đang ở Ký túc xá ĐHXD</asp:ListItem>
                    <asp:ListItem Value="NHOM_2">&nbsp;Nhóm 2: Sinh viên thuộc đối tượng chính sách, hoàn cảnh khó khăn; Sinh viên nữ</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class='g-recaptcha' data-sitekey='6Lf3Lc8ZAAAAANyHCgqSpM_NDwBTJQZIsEnUQJ1s'></div>
            <%--<div class="form-group">
                <button type="button" id="btnModalUpdate" class="btn btn-primary mt-2" data-toggle="modal" data-target="#myModalUpdate">Gửi yêu cầu</button>
            </div>--%>
        </div>

        <div class="col-12 col-md-3 mt-4" runat="server" id="divBtnContainer">
            <button type="button" id="btnModalUpdate" 
                    class="confirm-btn text-light w-100 text-uppercase font-14-sm pt-2 pb-2"
                    data-toggle="modal" data-target="#myModalUpdate">Gửi yêu cầu</button>
        </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button class="btn btn-primary" ID="btnCapNhat" runat="server" Text="Gửi yêu cầu" OnClick="btnCapNhat_Click" />
    </div>
    <div
        class="modal fade"
        id="myModalUpdate"
        tabindex="-1"
        role="dialog"
        aria-labelledby="exampleModalLabel"
        aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Bạn có chắc chắn muốn gửi yêu cầu tới Nhà trường?</h5>
                    <button
                        class="close"
                        type="button"
                        data-dismiss="modal"
                        aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    Nhấn nút xác nhận nếu bạn chắc chắn muốn gửi yêu cầu.
                </div>
                <div class="modal-footer">
                    <button
                        class="btn btn-secondary"
                        type="button"
                        data-dismiss="modal">
                        Thoát
                    </button>
                    <button type="button" class="btn btn-primary" id="btnCapNhat1" onclick="xacnhan_yeucaumoi.update();">Xác nhận</button>
                </div>
            </div>
        </div>
    </div>
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
                    Nhấn nút xác nhận nếu bạn chắc chắn muốn gửi yêu cầu.
                </div>
                <div class="modal-footer">
                    <button
                        class="btn btn-secondary"
                        type="button"
                        data-dismiss="modal">
                        Thoát
           
                    </button>
                    <button type="button" class="btn btn-primary" id="btnVeDanhSach" onclick="xacnhan_yeucaumoi.vedanhsach();">Quay lại danh sách</button>
                </div>
            </div>
        </div>
    </div>
    <script>
        (function ($) {
            $.fn.serializeAny = function () {
                var ret = [];
                $.each($(this).find(':input'), function () {
                    if (this.name != "")
                        ret.push(encodeURIComponent(this.name) + "=" + encodeURIComponent($(this).val()));
                });

                return ret.join("&").replace(/%20/g, "+");
            }
        })(jQuery);
        var onChangeLoaiThe = function(event){
            event.preventDefault();
            console.log($('#MainContent_radioLoaiThe').find(':checked').val());
        };
        var xacnhan_yeucaumoi = {
            update: function () {
                document.getElementById("<%=btnCapNhat.ClientID %>").click();
            },
            vedanhsach: function () {
                window.location.href = "/dichvu/dangkychoo";
            },
        };
        
        var init = function() {
            
        }

        init();

        $(document).ready(function () {
            
        });
    </script>
    <span runat="server" id="spScript"></span>
</asp:Content>
