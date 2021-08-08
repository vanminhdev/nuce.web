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
        <a href="/dichvu/miengiamhocphi.aspx">miễn giảm học phí</a>
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
    <div class="main-color text-decoration-none">miễn giảm học phí</div>
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
            <div class="form-group">
                <asp:RadioButtonList ID="radioDoiTuong"
                    runat="server"
                    RepeatDirection="Vertical">
                    <asp:ListItem Value="CO_CONG_CACH_MANG" Selected="True">&nbsp;Là con của người có công với cách mạng được hưởng ưu đãi
                        <br/><span class="font-italic font-13">(Kèm theo Bản sao giấy khai sinh; Bản sao công chứng thẻ Thương bệnh binh của bố/mẹ; Giấy xác nhận của Sở, Phòng lao động thương binh và xã hội)</span>
                    </asp:ListItem>
                    <asp:ListItem Value="SV_VAN_BANG_1">&nbsp;Là sinh viên học đại học văn bằng thứ nhất từ 16 tuổi đến 22 tuổi thuộc một trong các trường hợp quy định tại
                        Khoản 1 Điều 5 Nghị định số 136/2013/NĐ-CP ngày 21/10/2013 của Chính phủ quy định chính sách trợ giúp xã hội
                        <br/><span class="font-italic font-13">(Kèm theo Bản sao giấy khai sinh; Giấy xác nhận đối tượng hưởng trợ cấp của UBND cấp phường, xã cấp; Bản sao Quyết định về việc hưởng trợ cấp xã hội của UBND cấp quận, huyện)</span>
                    </asp:ListItem>
                    <asp:ListItem Value="TAN_TAT_KHO_KHAN_KINH_TE">&nbsp;Bản thân bị khuyết tật, tàn tật có khó khăn về kinh tế
                        <br/><span class="font-italic font-13">(Kèm theo Bản sao giấy khai sinh; Giấy giám định y khoa và Kết luận của Hội đồng xét duyệt trợ cấp xã hội cấp xã; Giấy chứng nhận hộ nghèo hoặc hộ cận nghèo của UBND cấp xã cấp)</span>
                    </asp:ListItem>
                    <asp:ListItem Value="DAN_TOC_HO_NGHEO">&nbsp;Bản thân là người dân tộc thiểu số thuộc hộ nghèo, hộ cận nghèo
                        <br/><span class="font-italic font-13">(Kèm theo Bản sao giấy khai sinh; Bản sao sổ khẩu thường trú; Giấy chứng nhận dân tộc (nếu có); Giấy chứng nhận hộ nghèo hoặc hộ cận nghèo do UBND cấp xã cấp)</span>
                    </asp:ListItem>
                    <asp:ListItem Value="DAN_TOC_IT_NGUOI_VUNG_KHO_KHAN">&nbsp;Bản thân là người dân tộc thiểu số rất ít người và ở vùng có điều kiện
                        kinh tế - xã hội khó khăn hoặc đặc biệt khó khăn
                        <br/><span class="font-italic font-13">(Kèm theo Bản sao giấy khai sinh; Giấy chứng nhận dân tộc (nếu có); Bản sao sổ khẩu thường trú; Giấy chứng nhận dân tộc thiểu số rất ít người, ở vùng có điều kiện kinh tế - xã hội khó khăn và đặc biệt khó khăn của Ủy ban dân tộc Cấp tỉnh cấp)</span>
                    </asp:ListItem>
                    <asp:ListItem Value="DAN_TOC_VUNG_KHO_KHAN">&nbsp;Bản thân là người dân tộc thiểu số và ở vùng có điều kiện
                        kinh tế - xã hội đặc biệt khó khăn
                        <br/><span class="font-italic font-13">(Kèm theo Bản sao giấy khai sinh; Giấy chứng nhận dân tộc (nếu có); Bản sao sổ khẩu thường trú; Giấy chứng nhận dân tộc thiểu số ở vùng có điều kiện kinh tế - xã hội đặc biệt khó khăn của Ủy ban dân tộc Cấp tỉnh cấp)</span>
                    </asp:ListItem>
                    <asp:ListItem Value="CHA_ME_TAI_NAN_DUOC_TRO_CAP">&nbsp;Là con của cán bộ, công nhân, viên chức mà cha hoặc mẹ bị tai nạn lao động
                        hoặc mắc bệnh nghề nghiệp được hưởng trợ cấp thường xuyên
                        <br/><span class="font-italic font-13">(Kèm theo Bản sao giấy khai sinh;Bản sao Quyết định của cơ quan cha, mẹ bị tai nạn lao động; Bản sao Sổ hưởng trợ cấp hàng tháng do tổ chức bảo hiểm xã hội cấp do tai nạn lao động của cha, mẹ)</span>
                    </asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class='g-recaptcha' data-sitekey='6Lf3Lc8ZAAAAANyHCgqSpM_NDwBTJQZIsEnUQJ1s'></div>
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
