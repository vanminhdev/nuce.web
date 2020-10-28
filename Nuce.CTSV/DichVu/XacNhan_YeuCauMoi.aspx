<%@ Page Title="Dịch vụ sinh viên" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="XacNhan_YeuCauMoi.aspx.cs" Inherits="Nuce.CTSV.XacNhan_YeuCauMoi" %>

<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    dịch vụ
</asp:Content>

<asp:Content ID="BreadCrumContent" ContentPlaceHolderID="BreadCrum" runat="server">
    <div class="d-flex align-items-center">
        <a href="/dichvusinhvien.aspx">dịch vụ</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="d-flex align-items-center">
        <a href="/dichvu/xacnhan.aspx">giấy xác nhận</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="main-color text-decoration-none">yêu cầu mới</div>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src='https://www.google.com/recaptcha/api.js'></script>
    <div class="col-sm-12">
        <div style="text-align: center; font-weight: bold; color: red;" runat="server" id="divThongBao"></div>
    </div>
    <div class="col-12">
        <div class="row justify-content-end">
        <div class="col-12">
            <div class="main-color text-uppercase font-15-sm fw-600 font-25 service-title">
                giấy xác nhận
            </div>
            <div class="form-group">
                <div class="font-14-sm fw-600 font-18">Lý do xác nhận</div>
                <select
                    class="form-control mt-3"
                    id="slLyDo" name="slLyDo" onchange="xacnhan_yeucaumoi.setValueLyDo(this);">
                    <option value="Xin đăng ký tạm trú, tạm vắng">1. Xin đăng ký tạm trú, tạm vắng</option>
                    <option value="Xin tạm hoãn nghĩa vụ quân sự">2. Xin tạm hoãn nghĩa vụ quân sự</option>
                    <option value="Xin xác nhận để người nhà thi vào bộ đội, công an.">3. Xin xác nhận để người nhà thi vào bộ đội, công an</option>
                    <option value="Xin xác nhận để người nhà kết hôn với người trong lực lượng vũ trang: bộ đội, công an">4. Xin xác nhận để người nhà kết hôn với người trong lực lượng vũ trang: bộ đội, công an</option>
                    <option value="Xin xác nhận để thuê nhà ở sinh viên tại làng sinh viên Hacinco">5. Xin xác nhận để thuê nhà ở sinh viên tại làng sinh viên Hacinco</option>
                    <option value="Xin xác nhận để làm hồ sơ đi du học">6. Xin xác nhận để làm hồ sơ đi du học</option>
                    <option value="Xác nhận để làm visa đi du lịch nước ngoài">7. Xác nhận để làm visa đi du lịch nước ngoài</option>
                    <option value="Xác nhận để được hưởng học bổng khuyến học ở địa phương">8. Xác nhận để được hưởng học bổng khuyến học ở địa phương</option>
                    <option value="Xin miễn giảm thuế thu nhập cá nhân cho người thân">9. Xin miễn giảm thuế thu nhập cá nhân cho người thân</option>
                    <option value="-1">10. Khác</option>
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
            <div class="form-group" runat="server" id="frmDiaChi">
                <label class="font-14-sm fw-600 font-18" for="diachi">Địa chỉ tạm trú:</label>
                <asp:TextBox ID="txtDiaChi" runat="server" class="form-control"></asp:TextBox>
            </div>
            <div class="form-group mt-3" id="frmXacNhan" style="display:none;">
                <asp:TextBox ID="txtLyDoXacNhan" runat="server" class="form-control" TextMode="MultiLine" placeholder="Bạn hãy nhập lý do khác" Text="Xin đăng ký tạm trú, tạm vắng"></asp:TextBox>
            </div>
            <div class='g-recaptcha' data-sitekey='6Lf3Lc8ZAAAAANyHCgqSpM_NDwBTJQZIsEnUQJ1s'></div>
            <%--<div class="form-group">
                <button type="button" id="btnModalUpdate" class="btn btn-primary mt-2" data-toggle="modal" data-target="#myModalUpdate">Gửi yêu cầu</button>
            </div>--%>
        </div>

        <div class="col-12 col-md-3 mt-4">
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
        var xacnhan_yeucaumoi = {
            update: function () {
                document.getElementById("<%=btnCapNhat.ClientID %>").click();
                //alert($("#" + "<%=txtLyDoXacNhan.ClientID %>").val());
            },
            vedanhsach: function () {
                window.location.href = "/dichvu/XacNhan";
            },
            setValueLyDo: function (obj)
            {
                //alert(obj.value);
                if (obj.value == "-1") {
                    $("#" + "<%=txtLyDoXacNhan.ClientID %>").val("");
                    $("#frmXacNhan").show();
                }
                else {
                    $("#frmXacNhan").hide();
                    $("#" + "<%=txtLyDoXacNhan.ClientID %>").val(obj.value);
                }
            }
        };
        $(document).ready(function () {
            
        });
    </script>
    <span runat="server" id="spScript"></span>
</asp:Content>
