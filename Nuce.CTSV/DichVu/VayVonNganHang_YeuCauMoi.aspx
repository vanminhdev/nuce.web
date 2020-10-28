<%@ Page Title="Vay Vốn Ngân Hàng" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="VayVonNganHang_YeuCauMoi.aspx.cs" Inherits="Nuce.CTSV.VayVonNganHang_YeuCauMoi" %>

<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    dịch vụ
</asp:Content>

<asp:Content ID="BreadCrumContent" ContentPlaceHolderID="BreadCrum" runat="server">
    <div class="d-flex align-items-center">
        <a href="/dichvusinhvien.aspx">dịch vụ</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="d-flex align-items-center">
        <a href="/dichvu/vayvonnganhang.aspx">vay vốn ngân hàng</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="main-color text-decoration-none">yêu cầu mới</div>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src='https://www.google.com/recaptcha/api.js'></script>
    <div class="col-sm-12">
        <div style="padding-top: 5px; text-align: center; font-weight: bold; color: red;" runat="server" id="divThongBao">
        </div>
    </div>
    <div class="col-12">
        <div class="row justify-content-end">
            <div class="col-sm-12">
                <div class="form-group">
                    <div class="font-14-sm fw-600 font-18">Thuộc diện:</div>
                    <select runat="server" class="form-control mt-3" id="slThuocDien" name="slLyDo">
                        <option value="1">Không miễn giảm</option>
                        <option value="2">Giảm học phí</option>
                        <option value="3">Miễn học phí</option>
                    </select>
                </div>
                <div class="form-group">
                    <div class="font-14-sm fw-600 font-18">Thuộc đối tượng:</div>
                    <select runat="server" class="form-control mt-3" id="slThuocDoiTuong" name="slThuocDoiTuong">
                        <option value="1">Mồ côi</option>
                        <option value="2">Không mồ côi</option>
                    </select>
                </div>
                <div class="form-group" runat="server" id="frmPhuong">
                    <label class="font-14-sm fw-600 font-18" for="phuong">Phường/Xã:</label>
                    <asp:TextBox ID="txtPhuong" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="form-group" runat="server" id="frmQuan">
                    <label class="font-14-sm fw-600 font-18" for="quan">Quận/Huyện:</label>
                    <asp:TextBox ID="txtQuan" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="form-group" runat="server" id="frmTinh">
                    <label class="font-14-sm fw-600 font-18" for="tinh">Tỉnh/Thành phố:</label>
                    <asp:TextBox ID="txtTinh" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="form-group" runat="server" id="frmCmnd">
                    <label class="font-14-sm fw-600 font-18" for="cmt">Số CMT:</label>
                    <asp:TextBox ID="txtCmnd" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="form-group" runat="server" id="frmNoiCap">
                    <label class="font-14-sm fw-600 font-18" for="cmtNoiCap">Nơi cấp:</label>
                    <asp:TextBox ID="txtNoiCap" runat="server" class="form-control" placeholder="Nơi cấp CMT"></asp:TextBox>
                </div>
                <div class="form-group" runat="server" id="frmNgayCap">
                    <label class="font-14-sm fw-600 font-18" for="cmtNgayCap">Ngày cấp:</label>
                    <asp:TextBox ID="txtNgayCap" runat="server" textmode="Date" class="form-control" placeholder="dd/mm/yyyy"></asp:TextBox>
                </div>
                <div class='g-recaptcha' data-sitekey='6Lf3Lc8ZAAAAANyHCgqSpM_NDwBTJQZIsEnUQJ1s'></div>
                <div class="form-group">
                    <div style="display: none;">
                        <asp:Button class="btn btn-primary" ID="btnCapNhat" runat="server" Text="Gửi yêu cầu" OnClick="btnCapNhat_Click" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-3 mt-4">
                <button type="button" id="btnModalUpdate" 
                        class="confirm-btn text-light w-100 text-uppercase font-14-sm pt-2 pb-2"
                        data-toggle="modal" data-target="#myModalUpdate">Gửi yêu cầu</button>
            </div>
        </div>
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
                    <button type="button" class="btn btn-primary" id="btnCapNhat1" onclick="vayvonnganhang_yeucaumoi.update();">Xác nhận</button>
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
                    <button type="button" class="btn btn-primary" id="btnVeDanhSach" onclick="vayvonnganhang_yeucaumoi.vedanhsach();">Quay lại danh sách</button>
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
        var vayvonnganhang_yeucaumoi = {
            update: function () {
                document.getElementById("<%= btnCapNhat.ClientID %>").click();
            },
            vedanhsach: function () {
                window.location.href = "/dichvu/VayVonNganHang";
            }
        };
    </script>
    <span runat="server" id="spScript"></span>
</asp:Content>
