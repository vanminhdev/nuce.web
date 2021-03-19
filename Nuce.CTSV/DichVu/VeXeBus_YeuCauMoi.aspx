<%@ Page Title="Dịch vụ sinh viên" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="VeXeBus_YeuCauMoi.aspx.cs" Inherits="Nuce.CTSV.VeXeBus_YeuCauMoi" %>

<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    dịch vụ
</asp:Content>

<asp:Content ID="BreadCrumContent" ContentPlaceHolderID="BreadCrum" runat="server">
    <div class="d-flex align-items-center">
        <a href="/dichvusinhvien.aspx">dịch vụ</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="d-flex align-items-center">
        <a href="/dichvu/vexebus.aspx">làm vé tháng xe bus</a>
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
        <a href="/dichvu/vexebus.aspx">làm vé tháng xe bus</a>
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
                làm vé tháng xe bus
            </div>
            <div class="form-group">
                <div class="font-14-sm fw-600 font-18 mb-3">Loại thẻ</div>
                <asp:RadioButtonList ID="radioLoaiThe"
                    runat="server"
                    RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">Một tuyến</asp:ListItem>
                    <asp:ListItem Value="2" >Liên tuyến</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="form-group" id="divTuyen">
                <div class="font-14-sm fw-600 font-18">Tuyến</div>
                <select class="form-control mt-3" name="slTuyen" id="slTuyen" runat="server"> 
                    <option value="1">01-Bến xe Gia Lâm - Bến xe Yên Nghĩa</option>
                    <option value="2">02-Bác Cổ - Bến xe Yên Nghĩa</option>
                    <option value="3">03A-Bến xe Giáp Bát - Bến xe Gia Lâm</option>
                    <option value="803">03B-Bến xe Nước Ngầm - Giang Biên (Long Biên)</option>
                    <option value="4">04-Long Biên - Bệnh viện Nội tiết trung ương cơ sở 2</option>
                                                    
                    <option value="5">05-KĐT Linh Đàm - Phú Diễn</option>
                                                    
                    <option value="6">06A-Bến xe Giáp Bát - Cầu Giẽ</option>
                                                    
                    <option value="806">06B-Bến xe Giáp Bát - Hồng Vân</option>
                                                    
                    <option value="866">06C-Bến xe Giáp Bát - Phú Minh</option>
                                                    
                    <option value="876">06D-Bến xe Giáp Bát - Tân Dân</option>
                                                    
                    <option value="886">06E-Bến xe Giáp Bát - Phú Túc</option>
                                                    
                    <option value="7">07-Cầu Giấy - Nội Bài</option>
                                                    
                    <option value="8">08A-Long Biên - Đông Mỹ</option>
                                                    
                    <option value="708">08B-Long Biên - Vạn Phúc</option>
                                                    
                    <option value="409">09A-Bờ Hồ - Cầu Giấy - Khu Liên cơ quan Sở ngành HN</option>
                                                    
                    <option value="609">09B-Bờ Hồ - Công viên Thống Nhất - Bến xe Mỹ Đình</option>
                                                    
                    <option value="100">100-Long Biên - Khu đô thị Đặng Xá</option>
                                                    
                    <option value="101">101A-Bến xe Giáp Bát - Vân Đình</option>
                                                    
                    <option value="901">101B-Bến xe Giáp Bát - Đại Cường (Ứng Hòa)</option>
                                                    
                    <option value="102">102-Bến xe Yên Nghĩa - Vân Đình</option>
                                                    
                    <option value="103">103A-Bến xe Mỹ Đình - Hương Sơn</option>
                                                    
                    <option value="903">103B-Bến xe Mỹ Đình - Hồng Quang - Hương Sơn</option>
                                                    
                    <option value="104">104-Mỹ Đình - BX Nước Ngầm</option>
                                                    
                    <option value="105">105-Đô Nghĩa - Cầu Giấy</option>
                                                    
                    <option value="106">106-KĐT Mỗ Lao - TTTM Aeon Mall Long Biên</option>
                                                    
                    <option value="107">107-Kim Mã - Làng văn hóa du lịch các dân tộc VN</option>
                                                    
                    <option value="108">108-Bến xe Thường Tín - Minh Tân</option>
                                                    
                    <option value="109">109-Mỹ Đình - Nội Bài</option>
                                                    
                    <option value="10">10A-Long Biên - Từ Sơn</option>
                                                    
                    <option value="810">10B-Long Biên - Trung Mầu</option>
                                                    
                    <option value="11">11-Công viên Thống Nhất - Học Viện Nông Nghiệp</option>
                                                    
                    <option value="110">110-BX Sơn Tây - Vườn Quốc gia Ba Vì - Đá Chông</option>
                                                    
                    <option value="111">111-BX Sơn Tây - Bất Bạt</option>
                                                    
                    <option value="112">112-Nam Thăng Long - Thạch Đà (Mê Linh)</option>
                                                    
                    <option value="113">113-Đại Thắng (Phú Xuyên) - Bến đò Vườn Chuối</option>
                                                    
                    <option value="12">12-Công viên Nghĩa Đô - Đại Áng</option>
                                                    
                    <option value="13">13-CV nước Hồ Tây - Cổ Nhuế (HVCS)</option>
                                                    
                    <option value="14">14-Bờ Hồ - Cổ Nhuế</option>
                                                    
                    <option value="15">15-Bến xe Gia Lâm - Phố Nỉ</option>
                                                    
                    <option value="816">16-Bến xe Mỹ Đình - Bến xe Nước Ngầm</option>
                                                    
                    <option value="17">17-Long Biên - Nội Bài</option>
                                                    
                    <option value="18">18-ĐH Kinh tế QD - ĐH Kinh tế QD</option>
                                                    
                    <option value="19">19-Trần Khánh Dư - Học viện chính sách và phát triển</option>
                                                    
                    <option value="20">20A-Cầu Giấy - Bến xe Sơn Tây</option>
                                                    
                    <option value="820">20B-Nhổn - Bến xe Sơn Tây</option>
                                                    
                    <option value="21">21A-Bến xe Giáp Bát - Bến xe Yên Nghĩa</option>
                                                    
                    <option value="821">21B-Khu đô thị Pháp Vân - Bến xe Mỹ Đình</option>
                                                    
                    <option value="22">22A-Bến xe Gia Lâm - KĐT Trung Văn</option>
                                                    
                    <option value="822">22B-BX Mỹ Đình - KĐT Kiến Hưng</option>
                                                    
                    <option value="922">22C-Bến xe Giáp Bát - Khu đô thị Dương Nội</option>
                                                    
                    <option value="23">23-Nguyễn Công Trứ - Nguyễn Công Trứ</option>
                                                    
                    <option value="24">24-Long Biên - Cầu Giấy</option>
                                                    
                    <option value="25">25-BV Nhiệt đới TW Cơ sở 2 - Bến xe Giáp Bát</option>
                                                    
                    <option value="26">26-Mai Động - SVĐ Quốc gia</option>
                                                    
                    <option value="27">27-Bến xe Yên Nghĩa - Bến xe Nam Thăng Long</option>
                                                    
                    <option value="28">28-BX Nước Ngầm - Đông Ngạc - Đại học Mỏ</option>
                                                    
                    <option value="29">29-BX Giáp Bát - Tân Lập</option>
                                                    
                    <option value="30">30-Khu đô thị Gamuda - Bến xe Mỹ Đình</option>
                                                    
                    <option value="31">31-Bách Khoa - Chèm (ĐH Mỏ)</option>
                                                    
                    <option value="32">32-Bến xe Giáp Bát - Nhổn</option>
                                                    
                    <option value="33">33-Cụm Công nghiệp Thanh Oai - Xuân Đỉnh</option>
                                                    
                    <option value="34">34-Bến xe Mỹ Đình - Bến xe Gia Lâm</option>
                                                    
                    <option value="35">35A-Trần Khánh Dư - Nam Thăng Long</option>
                                                    
                    <option value="835">35B-Nam Thăng Long - Thanh Lâm</option>
                                                    
                    <option value="36">36-Yên Phụ - Khu đô thị Linh Đàm</option>
                                                    
                    <option value="37">37-Bến xe Giáp Bát - Chương Mỹ</option>
                                                    
                    <option value="38">38-BX Nam Thăng Long - Mai Động</option>
                                                    
                    <option value="39">39-Công viên Nghĩa Đô - Bệnh viện Nội tiết TW CS II</option>
                                                    
                    <option value="40">40-Công viên Thống Nhất - Văn Lâm</option>
                                                    
                    <option value="41">41-Nghi Tàm - BX Giáp Bát</option>
                                                    
                    <option value="42">42-Giáp Bát - Đức Giang</option>
                                                    
                    <option value="43">43-Công viên Thống Nhất - Thị trấn Đông Anh</option>
                                                    
                    <option value="44">44-Trần Khánh Dư - BX Mỹ Đình</option>
                                                    
                    <option value="45">45-Time City - Bến xe Nam Thăng Long</option>
                                                    
                    <option value="46">46-Bến xe Mỹ Đình - Thị trấn Đông Anh</option>
                                                    
                    <option value="47">47A-Long Biên - Bát Tràng</option>
                                                    
                    <option value="847">47B-Đại học Kinh tế Quốc dân - Kiêu Kỵ</option>
                                                    
                    <option value="48">48-Bến xe Nước Ngầm - Chương Dương - Phúc Lợi (Long Biên)</option>
                                                    
                    <option value="49">49-Trần Khánh Dư - Nhổn</option>
                                                    
                    <option value="50">50-Long Biên - Khu đô thị Vân Canh</option>
                                                    
                    <option value="51">51-Trần Khánh Dư - Trần Vỹ (Học vện Tư Pháp)</option>
                                                    
                    <option value="52">52A-Công viên Thống nhất - Lệ Chi</option>
                                                    
                    <option value="852">52B-Công viên Thống Nhất - Đặng Xá</option>
                                                    
                    <option value="53">53A-Hoàng Quốc Việt - Đông Anh</option>
                                                    
                    <option value="853">53B-Bến xe Mỹ Đình - Kim Hoa (Mê Linh)</option>
                                                    
                    <option value="54">54-Long Biên - Bắc Ninh</option>
                                                    
                    <option value="55">55A-Khu đô thị Times City - Cầu Giấy</option>
                                                    
                    <option value="855">55B-TTTM Aeon Mall - Cầu Giấy</option>
                                                    
                    <option value="56">56A-Bến xe Nam Thăng Long - Núi Đôi</option>
                                                    
                    <option value="856">56B-Học viện Phật Giáo VN - Xuân Giang - Bắc Phú - Học viện Phật Giáo VN</option>
                                                    
                    <option value="57">57-Nam Thăng Long - Khu công nghiệp Phú Nghĩa</option>
                                                    
                    <option value="58">58-Long Biên - Thạch Đà</option>
                                                    
                    <option value="59">59-TT Đông Anh - Học viện Nông nghiệp Việt Nam</option>
                                                    
                    <option value="60">60A-KĐT Pháp Vân, Tứ Hiệp - Công Viên Nước Hồ Tây</option>
                                                    
                    <option value="860">60B-Bến xe Nước Ngầm - BV Bệnh nhiệt đới TƯ (CS2)</option>
                                                    
                    <option value="861">61-Vân Hà (Đông Anh) - CV Cầu Giấy</option>
                                                    
                    <option value="62">62-Bến xe Yên Nghĩa - Bến xe Thường Tín</option>
                                                    
                    <option value="63">63-Khu công nghiệp Bắc Thăng Long - Tiến Thịnh (Mê Linh)</option>
                                                    
                    <option value="64">64-BX Mỹ Đình - Phố Nỉ - TTTM Bình An</option>
                                                    
                    <option value="65">65-Thụy Lâm (Đông Anh) - Long Biên (ĐCT Long Biên)</option>
                                                    
                    <option value="72">72-Bến xe Yên Nghĩa - Xuân Mai</option>
                                                    
                    <option value="74">74-Bến xe Mỹ Đình - Xuân Khanh</option>
                                                    
                    <option value="84">84-Cầu Diễn - Khu đô thị Linh Đàm</option>
                                                    
                    <option value="85">85-Công viên Nghĩa Đô - Khu đô thị Thanh Hà</option>
                                                    
                    <option value="87">87-Bến xe Mỹ Đình - Quốc Oai - Xuân Mai</option>
                                                    
                    <option value="88">88-Bến xe Mỹ Đình - Hòa Lạc - Xuân Mai</option>
                                                    
                    <option value="89">89-Bến xe Yên Nghĩa - Thạch Thất - Bến xe Sơn Tây</option>
                                                    
                    <option value="90">90-Hào Nam - Cầu Nhật Tân - Sân bay Nội Bài</option>
                                                    
                    <option value="91">91-Bến xe Yên Nghĩa - Kim Bài - Phú Túc</option>
                                                    
                    <option value="92">92-Nhổn - Sơn Tây - Tây Đằng</option>
                                                    
                    <option value="93">93-Nam Thăng Long - Bắc Sơn (Sóc Sơn)</option>
                                                    
                    <option value="94">94-Bến xe Giáp Bát - Kim Bài</option>
                                                    
                    <option value="95">95-Nam Thăng Long - Xuân Hòa</option>
                                                    
                    <option value="96">96-Công viên Nghĩa Đô - Đông Anh</option>
                                                    
                    <option value="97">97-Hoài Đức - Công viên Nghĩa Đô</option>
                                                    
                    <option value="98">98-Yên Phụ - Gia Thụy - Aeon Mall Long Biên</option>
                                                    
                    <option value="99">99-Kim Mã - Ngũ Hiệp (Thanh Trì)</option>
                                                    
                    <option value="918">BRT01-Bến xe Yên Nghĩa - Kim Mã</option>
                                                    
                    <option value="205">CNG01-Bến xe Mỹ Đình - Bến xe Sơn Tây</option>
                                                    
                    <option value="206">CNG02-Bến xe Yên Nghĩa - Khu đô thị Đặng Xá</option>
                                                    
                    <option value="207">CNG03-Bệnh viện Bệnh nhiệt đới Trung ương cơ sở 2 - Khu đô thị Times City.</option>
                                                    
                    <option value="208">CNG04-Kim Lũ (Sóc Sơn) - Nam Thăng Long</option>
                                                    
                    <option value="209">CNG05-Cầu Giấy - Tam Hiệp (Thanh Trì)</option>
                                                    
                    <option value="210">CNG06-Nhổn - Thọ An (Đan Phượng)</option>
                                                    
                    <option value="211">CNG07-BX Yên Nghĩa - Hoài Đức</option>
                                                    
                </select>
            </div>
            <div class="form-group">
                <div class="font-14-sm fw-600 font-18">Điểm nhận thẻ</div>
                <select class="form-control mt-3" name="slNoiNhan" id="slNoiNhan" runat="server">
                    <option value="1">A1-BX Giáp Bát</option>
                    <option value="2">A2-Học viện Nông Nghiệp</option>
                    <option value="3">A3-Công viên Thống nhất</option>
                    <option value="4">A4-Công viên Thủ Lệ</option>
                    <option value="5">A5-BX Gia Lâm</option>
                    <option value="6">A6-Bãi đỗ xe Trần Khánh Dư</option>
                    <option value="7">A7-BX Nam Thăng Long</option>
                    <option value="8">A8-BX Mỹ Đình</option>
                    <option value="9">A9-479 Hoàng Quốc Việt</option>
                    <option value="10">A10-Học viện Bưu chính viễn thông</option>
                    <option value="11">A11-Học viện Quân Y- Hà Đông</option>
                    <option value="12">A12-Đối diện 346  Kim Ngưu</option>
                    <option value="13">A13-32 Nguyễn Công Trứ</option>
                    <option value="14">A14-Điểm trung chuyển Long Biên</option>
                    <option value="15">A15-Toà nhà PTA - Số 01 Kim Mã</option>
                    <option value="16">A16-Bách Khoa- Đường Lê Thanh Nghị</option>
                    <option value="17">A17-BX Yên Nghĩa</option>
                    <option value="18">A18-Kiều Mai- Ngõ 91 Cầu Diễn</option>
                    <option value="19">A19-Công viên Nghĩa Đô</option>
                    <option value="20">A20-Bộ Y Tế - 138 Giảng Võ</option>
                    <option value="21">A21-Siêu thị HC – 549 Ng Văn Cừ</option>
                    <option value="22">A22-Khu SV- Nguyễn Cơ Thạch</option>
                    <option value="23">A23-BX Thường Tín</option>
                    <option value="24">A24-BX Phùng</option>
                    <option value="25">A25-Điểm trung chuyển Cầu Giấy</option>
                    <option value="26">A26-Điểm trung chuyển Long Biên</option>
                    <option value="27">A27-Điểm trung chuyển Nhổn</option>
                    <option value="28">A28-Học viện Ngân Hàng</option>
                    <option value="29">A29-Nhà máy xà phòng- Nguyễn Trãi</option>
                    <option value="30">A30-Đại học Quốc Gia- Xuân Thuỷ</option>
                    <option value="31">A31-Khu đô thị Linh Đàm</option>
                    <option value="32">A32-Công ty CPVTTM và DL Đông Anh- Thị trấn Đông Anh</option>
                    <option value="33">A33-Ngã 3 xay sát Đông Quan</option>
                    <option value="34">A34-Điểm trung chuyển H.Quốc Việt</option>
                    <option value="35">A35-Ngã tư thị trấn Sóc Sơn</option>
                    <option value="36">A36-Trung tâm thương mại Mê Linh Palaza</option>
                    <option value="37">A37-Tổng cục Hải Quan- 162 NVCừ</option>
                    <option value="38">A38-Đại học Thương Mại</option>
                    <option value="39">A39-Đại học Thủy Lợi</option>
                    <option value="40">A40-Học viên Thanh thiếu niên- Nguyễn Chí Thanh</option>
                    <option value="41">A41-ĐH Lao động xã hội</option>
                    <option value="42">A42-Kim Chung</option>
                    <option value="43">A43-BX Sơn Tây</option>
                    <option value="44">A44-Yên Viên</option>
                    <option value="45">A45-Chèm</option>
                    <%--<option value="1">A01-BX Giáp Bát</option>
                                                    
                    <option value="40">A02-Học viện Nông Nghiệp</option>
                                                    
                    <option value="2">A03-Công viên Thống nhất</option>
                                                    
                    <option value="3">A04-Công viên Thủ Lệ</option>
                                                    
                    <option value="4">A05-BX Gia Lâm</option>
                                                    
                    <option value="6">A07-Công viên Hòa Bình</option>
                                                    
                    <option value="46">A09-Bx Mỹ Đình</option>
                                                    
                    <option value="10">A11-Học viện Bưu chính viễn thông</option>
                                                    
                    <option value="11">A12-Học Viện Quân y</option>
                                                    
                    <option value="12">A13-Kim Ngưu</option>
                                                    
                    <option value="70">A14-Nguyễn Công Trứ</option>
                                                    
                    <option value="14">A15-Long Biên</option>
                                                    
                    <option value="16">A17-Bách Khoa</option>
                                                    
                    <option value="17">A18-BX Yên Nghĩa</option>
                                                    
                    <option value="53">A19-Kiều Mai</option>
                                                    
                    <option value="19">A20-Công viên Nghĩa Đô</option>
                                                    
                    <option value="68">A21-Giảng Võ</option>
                                                    
                    <option value="58">A22-Hoàng Đạo Thúy</option>
                                                    
                    <option value="42">A23-Siêu thị HC 549 Nguyễn Văn Cừ (Đức Giang)</option>
                                                    
                    <option value="45">A45-ĐH Lao động và Xã hội</option>
                                                    
                    <option value="44">A46-Kim Chung</option>--%>
                                                    
                </select>
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
                window.location.href = "/dichvu/vexebus";
            },
        };
        
        var init = function() {
            var checkbox = $('#<%= radioLoaiThe.ClientID %>').find('input');
            $.each(checkbox, function(idx, item) {
                item.addEventListener('click', function(event) { 
                    var value = event.target.defaultValue;
                    var checked = event.target.checked;
                    if (!checked) return;
                    if (value === "1") {
                        $('#divTuyen').show();
                    } else if (value === "2") {
                        $('#divTuyen').hide();
                    }
                });
            });

            var td = $('#<%= radioLoaiThe.ClientID %>').find('td');
            $.each(td, function(idx, item) {
                const cb = item.querySelector('input');
                const lb = item.querySelector('label');
                Object.assign(cb.style, { cursor: 'pointer' });
                Object.assign(lb.style, { cursor: 'pointer' });

                lb.classList.add('mr-3');
            });
        }

        init();

        $(document).ready(function () {
            
        });
    </script>
    <span runat="server" id="spScript"></span>
</asp:Content>
