<%@ Page Title="Hồ sơ sinh viên" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="HoSoSinhVien.aspx.cs" Inherits="Nuce.CTSV.HoSoSinhVien" %>
<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    <style>
        .service-banner {
            background: url(style/public/images/profile-background.png) rgba(0, 0, 0, 0.45);
        }
    </style>
    hồ sơ
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="text-uppercase font-25 font-15-sm fw-600 main-color mb-4">
    thông tin cá nhân
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Họ và Tên:</div>
            <div id="hovaten" runat="server" class="col-12 col-md-7 text-uppercase">phạm minh khôi</div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Số điện thoại:</div>
            <div runat="server" id="sdt" class="col-12 col-md-7">0912987567</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
        <div class="col-12 col-md-5 fw-700">Tài khoản:</div>
        <div runat="server" id="masv" class="col-12 col-md-7 text-uppercase">125960</div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
        <div class="col-12 col-md-5 fw-700">Email:</div>
        <div runat="server" id="mail" class="col-12 col-md-7">namthanhbk@gmail.com</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Năm sinh:</div>
            <div runat="server" id="namsinhsv" class="col-12 col-md-7">22-06-1995</div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Số CMT:</div>
            <div runat="server" id="socmnd" class="col-12 col-md-7">123456789012</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
        <div class="col-12 col-md-5 fw-700">Nơi sinh:</div>
        <div class="col-12 col-md-7" runat="server" id="noisinhsv">
            Xã Hoằng Vinh-Huyện Hoằng Hóa-Tỉnh Thanh Hóa
        </div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
        <div class="col-12 col-md-5 fw-700">Dân tộc:</div>
        <div runat="server" id="dantocsv" class="col-12 col-md-7">Kinh</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Nơi cấp:</div>
            <div runat="server" id="cmndnoicap" class="col-12 col-md-7">Cục quản cư dân Quốc gia</div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Ngày Cấp:</div>
            <div runat="server" id="cmndngaycap" class="col-12 col-md-7">22/07/2016</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Tôn giáo:</div>
            <div runat="server" id="tongiaosv" class="col-12 col-md-7">Không</div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
        <div class="col-12 col-md-5 fw-700">Ngày vào đoàn:</div>
        <div runat="server" id="ngayvaodoansv" class="col-12 col-md-7">22/07/2016</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Hộ khẩu thường trú:</div>
            <div class="col-12 col-md-7"></div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Ngày vào đảng:</div>
            <div runat="server" id="ngayvaodangsv" class="col-12 col-md-7">22/07/2016</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Số nhà:</div>
            <div id="hktt_sonhasv" runat="server" class="col-12 col-md-7">12</div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
        <div class="col-12 col-md-5 fw-700">
            Tốt nghiệp THPT (BTVH) năm:
        </div>
        <div runat="server" id="namtotnghiepthptsv" class="col-12 col-md-7">2013</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
        <div class="col-12 col-md-5 fw-700">Phố / Thôn:</div>
        <div id="hktt_phothonsv" runat="server" class="col-12 col-md-7">Vọng</div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">
                Điểm thi PTTH Quốc gia:
            </div>
            <div runat="server" id="diemthithptqgsv"  class="col-12 col-md-7">20</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">Phường / Xã:</div>
            <div runat="server" id="hktt_phuongxasv" class="col-12 col-md-7">Đồng Tâm</div>
        </div>
    </div>
    <div class="col-6">
        <div class="row">
            <div class="col-12 col-md-5 fw-700">
                Hộ khẩu thường trú thuộc khu vực:
            </div>
            <div runat="server" id="hktttkvsv" class="col-12 col-md-7">2NT</div>
        </div>
    </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
        <div class="col-6">
            <div class="row">
                <div class="col-12 col-md-5 fw-700">Quận / Huyện:</div>
                <div runat="server" id="hktt_quanhuyensv" class="col-12 col-md-7">Hai Bà Trưng</div>
            </div>
        </div>
        <div class="col-6">
            <div class="row">
                <div class="col-12 col-md-5 fw-700">Đối tượng ưu tiên:</div>
                <div runat="server" id="dtutsv" class="col-12 col-md-7">Không</div>
            </div>
        </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
        <div class="col-6">
            <div class="row">
                <div class="col-12 col-md-5 fw-700">Tỉnh / Thành phố:</div>
                <div runat="server" id="hktt_thanhphosv" class="col-12 col-md-7">Hai Bà Trưng</div>
            </div>
        </div>
        <div class="col-6">
            <%--<div class="row">
                <div class="col-12 col-md-5 fw-700">Đối tượng ưu tiên:</div>
                <div runat="server" id="Div2" class="col-12 col-md-7">Không</div>
            </div>--%>
        </div>
    </div>
    <div class="row font-13-sm font-16 mb-3">
        <div class="col-6">
            <div class="row">
                <div class="col-12 col-md-5 fw-700">Có ở nội trú không:</div>
                <div runat="server" id="coonoitrukhongsv" class="col-12 col-md-7">Không</div>
            </div>
        </div>
        <div class="col-6">
            <div class="row">
                <div class="col-12 col-md-5 fw-700">Địa chỉ nơi ở cụ thể:</div>
                <div runat="server" id="diachicuthesv" class="col-12 col-md-7">Phòng 101</div>
            </div>
        </div>
    </div>

    <div class="font-14-sm font-18 fw-700 mt-4 mb-4">
        Quá trình học tập (Ghi rõ thời gian / trường học PTTH )
    </div>
    <div class="row font-13-sm font-16">
    <div class="col-6 col-md-3">
        <div class="row" id="qtht_thoigian" runat="server">
            <div class="col-12 fw-700">Thời gian</div>
            <div class="col-12">t1/2014 đến t3/2016</div>
            <div class="col-12">t4/2016 đến t10/2020</div>
        </div>
    </div>
    <div class="col-6 col-md-3">
        <div class="row" id="qtht_truong" runat="server">
            <div class="col-12 fw-700">Trường</div>
            <div class="col-12">Giao Thủy</div>
            <div class="col-12">Thành phố Nam Định</div>
        </div>
    </div>
    <div class="col-6 mt-2 col-md-3">
        <div class="row">
            <div class="col-12 fw-700">Tên hoạt động</div>
            <div class="col-12">Đã từng làm cán bộ</div>
            <div class="col-12">Đã từng làm cán bộ đoàn</div>
            <div class="col-12">Đã tham gia đội tuyển thi HSG</div>
        </div>
    </div>
    <div class="col-6 mt-2 col-md-3">
        <div class="row">
            <div class="col-12 fw-700">Tham gia (Có / Không)</div>
            <div id="datunglamcanbolop" runat="server" class="col-12">Có</div>
            <div id="datunglamcanbodoan" runat="server" class="col-12">Có</div>
            <div id="dathamgiadoituyenthihsg" runat="server" class="col-12">Có</div>
        </div>
    </div>
    </div>

    <div class="row font-13-sm font-16 mb-3 display-lg mt-2">
        <div class="col-3" id="hsg_capthi" runat="server">
            <div class="fw-700">Cấp thi:</div>
            <div class=" ">Cấp 1</div>
        </div>
        <div class="col-3" id="hsg_monthi" runat="server">
            <div class="fw-700">Môn thi:</div>
            <div class=" ">Toán</div>
        </div>
        <div class="col-3" id="hsg_giai" runat="server">
            <div class="fw-700">Đạt giải:</div>
            <div class=" ">Nhì</div>
        </div>
    </div>

    <hr class="profile-line mt-2 mb-2" />

    <div class="row align-items-end">
        <div class="col-12 col-md-6 text-uppercase font-15-sm font-25 fw-600 main-color mt-4 mb-4">
            thông tin gia đình
        </div>
        <div class="col-12 col-md-6 font-15-sm fw-700 mb-4">
            Địa chỉ báo tin cho Bố, Mẹ hoặc người nuôi dưỡng khi cần:
        </div>
    </div>
    <!-- thông tin gia đình -->
    <div class="row font-13-sm mb-3">
        <!-- cột 1 -->
        <div class="col-6" id="divGiaDinh" runat="server">
            
        </div>
        <!-- cột 2 -->
        <div class="col-6">
            <div class="row mb-3">
                <div class="col-12 col-md-5 fw-700">Nơi ở hiện tại:</div>
                <div class="col-12 col-md-7">Nam Định</div>
            </div>
            <div class="row mb-3">
                <div class="col-12 col-md-5 fw-700">Họ tên người nhận:</div>
                <div runat="server" id="baotinhovatensv" class="col-12 col-md-7 text-uppercase">phạm văn a</div>
            </div>
            <div class="row mb-3">
                <div class="col-12 col-md-5 fw-700">Địa chỉ người nhận:</div>
                <div runat="server" id="baotindiachinguoinhansv" class="col-12 col-md-7">Không</div>
            </div>
            <div class="row mb-3">
                <div class="col-12 col-md-5 fw-700">
                    Số điện thoại người nhận:
                </div>
                <div runat="server" id="baotinsodienthoainguoinhansv" class="col-12 col-md-7">Không</div>
            </div>
        </div>
    </div>
    <!-- End of Main Content -->


</asp:Content>
