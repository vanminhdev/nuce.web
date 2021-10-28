<%@ Page Title="Dịch vụ sinh viên" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="MienGiamHocPhi_YeuCauMoi.aspx.cs" Inherits="Nuce.CTSV.MienGiamHocPhi_YeuCauMoi" %>

<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    dịch vụ
</asp:Content>

<asp:Content ID="BreadCrumContent" ContentPlaceHolderID="BreadCrum" runat="server">
    <div class="d-flex align-items-center">
        <a href="/dichvusinhvien.aspx">dịch vụ</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="d-flex align-items-center">
        <a href="/dichvu/miengiamhocphi.aspx">Đơn xin miễn giảm học phí</a>
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
        <a href="/dichvu/miengiamhocphi.aspx">Đơn xin miễn giảm học phí đăng ký chỗ ở</a>
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
                Đơn xin miễn giảm học phí
            </div>
            <div class="font-14-sm fw-600 font-18 mb-3">
                Sinh viên thuộc đối tượng được hưởng chính sách miễn, giảm học phí (tích vào ô dưới đây):
            </div>
            <div class="form-group mb-0">
                <asp:RadioButtonList ID="radioDoiTuong"
                    runat="server"
                    RepeatDirection="Vertical">
                    <asp:ListItem Value="CO_CONG_CACH_MANG" Selected="True">&nbsp;Các đối tượng theo quy định tại Pháp lệnh Ưu đãi người có công với cách mạng nếu đang theo học tại các cơ sở giáo dục thuộc hệ thống giáo dục quốc dân.
                    </asp:ListItem>
                    <asp:ListItem Value="SV_VAN_BANG_1">&nbsp;Sinh viên từ 16 đến 22 tuổi không có nguồn nuôi dưỡng đang học đại học văn bằng thứ nhất thuộc đối tượng hưởng trợ cấp xã hội hàng tháng theo quy định tại khoản 1 và 2 Điều 5 Nghị định số 20/2021/NĐ-CP ngày 15/3/2021 của Chính phủ.
                    </asp:ListItem>
                    <asp:ListItem Value="TAN_TAT_KHO_KHAN_KINH_TE">&nbsp;Sinh viên khuyết tật
                    </asp:ListItem>
                    <asp:ListItem Value="DAN_TOC_HO_NGHEO">&nbspSinh viên người dân tộc thiểu số có cha hoặc mẹ hoặc cả cha và mẹ hoặc ông bà (trong trường hợp ở với ông bà) thuộc hộ nghèo và hộ cận nghèo theo quy định của Thủ tướng Chính phủ.
                    </asp:ListItem>
                    <asp:ListItem Value="DAN_TOC_IT_NGUOI_VUNG_KHO_KHAN">&nbsp;Sinh viên là người dân tộc thiểu số rất ít người (Cống, Mảng, Pu Péo, Si La, Cờ Lao, Bố Y, La Ha, Ngái, Chứt, Ơ Đu, Brâu, Rơ Măm, Lô Tô, Lự, Pà Thẻn, La Hủ) ở vùng có điều kiện kinh tế - xã hội khó khăn hoặc đặc biệt khó khăn
                    </asp:ListItem>
                    <asp:ListItem Value="DAN_TOC_VUNG_KHO_KHAN">&nbsp;Sinh viên hệ cử tuyển
                    </asp:ListItem>
                    <asp:ListItem Value="CHA_ME_TAI_NAN_DUOC_TRO_CAP">&nbsp;Sinh viên thuộc các đối tượng của chương trình, đề án được miễn giảm học phí theo quy định của Chính phủ.
                    </asp:ListItem>
                    <asp:ListItem Value="KHU_VUC_III">&nbsp;Sinh viên là người dân tộc thiểu số (ngoài đối tượng dân tộc thiểu số rất ít người) ở thôn/bản đặc biệt khó khăn, xã khu vực III vùng dân tộc và miền núi, xã đặc biệt khó khăn vùng bãi ngang ven biển hải đảo theo quy định của cơ quan có thẩm quyền.
                    </asp:ListItem>
                    <asp:ListItem Value="CON_CAN_BO_DUOC_TRO_CAP_THUONG_XUYEN">&nbsp;Sinh viên là con cán bộ, công chức, viên chức, công nhân mà cha hoặc mẹ bị tai nạn lao động hoặc mắc bệnh nghề nghiệp được hưởng trợ cấp thường xuyên.
                    </asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="font-14-sm fw-600 font-18 mb-3">
                Số điện thoại liên lạc:
            </div>
            <div class="form-group">
                <asp:TextBox ID="textBoxSdt" runat="server" MaxLength="10">
                </asp:TextBox>
            </div>
            <div class='g-recaptcha mt-5' data-sitekey='6Lf3Lc8ZAAAAANyHCgqSpM_NDwBTJQZIsEnUQJ1s'></div>
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
                window.location.href = "/dichvu/miengiamhocphi";
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
