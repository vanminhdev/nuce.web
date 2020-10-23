<%@ Page Title="Dịch vụ sinh viên" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MuonHocBaGoc_YeuCauMoi.aspx.cs" Inherits="Nuce.CTSV.MuonHocBaGoc_YeuCauMoi" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link id="bsdp-css" href="/bootstrap-datepicker/css/bootstrap-datepicker3.min.css" rel="stylesheet">
    <script src="/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
    <script src="/bootstrap-datepicker/locales/bootstrap-datepicker.uk.min.js" charset="UTF-8"></script>
    <script src="/bootstrap-datepicker/locales/bootstrap-datepicker.vi.min.js" charset="UTF-8"></script>
    <script src='https://www.google.com/recaptcha/api.js'></script>
    <style>
        ul.breadcrumb {
            padding: 10px 16px;
            list-style: none;
            background-color: #fafafd;
        }

            ul.breadcrumb li {
                display: inline;
                font-size: 14px;
            }

                ul.breadcrumb li + li:before {
                    padding: 8px;
                    color: black;
                    content: "/\00a0";
                }

                ul.breadcrumb li a {
                    color: #0275d8;
                    text-decoration: none;
                }

                    ul.breadcrumb li a:hover {
                        color: #01447e;
                        text-decoration: underline;
                    }
    </style>
    <div class="container-fluid">
        <div class="row row-no-gutters">
            <div class="col-sm-12">
                <ul class="breadcrumb">
                    <li><a href="/dichvusinhvien">Dịch Vụ</a></li>
                    <li><a href="/dichvu/MuonHocBaGoc">Mượn bằng gốc</a></li>
                    <li>Yêu cầu mới</li>
                </ul>
            </div>
        </div>
        <div class="card shadow mb-4">
            <div style="width: 50%;">
                <div class="col-sm-12">
                    <div style="padding-top: 5px; text-align: center; font-weight: bold; color: red;" runat="server" id="divThongBao">
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="form-group">
                        <label for="Email">Lý do:</label>
                        <asp:TextBox ID="txtLyDoXacNhan" runat="server" class="form-control" TextMode="MultiLine" placeholder="Lý do"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="NgayTra">Ngày trả (dd/mm/yyyy):</label>
                        <asp:TextBox ID="txtNgayTra" runat="server" class="form-control" placeholder="dd/mm/yyyy"></asp:TextBox>
                    </div>
                    <%--   <div class="form-group">
                        <label for="Image2">Captcha:</label>
                        <asp:Image ID="Image2" runat="server" Height="55px" ImageUrl="/Extent/Captcha.aspx" Width="186px" />
                    </div>
                    <div class="form-group">
                        <label for="captcha">Nhập mã captcha:</label>
                        <asp:TextBox ID="txtCaptcha" runat="server" class="form-control" TextMode="SingleLine"></asp:TextBox>
                    </div>--%>
                    <div class='g-recaptcha' data-sitekey='6Lf3Lc8ZAAAAANyHCgqSpM_NDwBTJQZIsEnUQJ1s'></div>
                    <div class="form-group">
                        <button type="button" id="btnModalUpdate" class="btn btn-primary mt-2" data-toggle="modal" data-target="#myModalUpdate">Gửi yêu cầu</button>
                        <%-- <asp:Button class="btn btn-primary" ID="btnCapNhat" runat="server" Text="Gửi yêu cầu" OnClick="btnCapNhat_Click" />--%>
                    </div>
                </div>
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
                    <h5 class="modal-title" id="exampleModalLabel">Bạn có chắc chắn muốn gửi yêu cầu tới Nhà trường ?</h5>
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
                    <button type="button" class="btn btn-primary" id="btnCapNhat1" onclick="muonhocbagoc_yeucaumoi.update();">Xác nhận</button>
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
                    <button type="button" class="btn btn-primary" id="btnVeDanhSach" onclick="muonhocbagoc_yeucaumoi.vedanhsach();">Quay lại danh sách</button>
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
        var muonhocbagoc_yeucaumoi = {
            update: function () {
                document.getElementById("<%=btnCapNhat.ClientID %>").click();
            },
            vedanhsach: function () {
                window.location.href = "/dichvu/MuonHocBaGoc";
            }
        };

        $(document).ready(function () {
            var date_input = $('#'+"<%=txtNgayTra.ClientID %>"); //our date input has the name "date"
            //var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
            var options = {
                format: 'dd/mm/yyyy',
            //    container: container,
                todayHighlight: true,
                autoclose: true,
            };
            date_input.datepicker(options);
        })
    </script>
       <span runat="server" id="spScript"></span>
</asp:Content>
