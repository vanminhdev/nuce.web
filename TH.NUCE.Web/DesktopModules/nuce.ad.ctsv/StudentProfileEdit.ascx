<%@ control language="C#" autoeventwireup="true" codebehind="StudentProfileEdit.ascx.cs" inherits="nuce.ad.ctsv.StudentProfileEdit" %>
<style>
#myBtn {
  display: none;
  position: fixed;
  bottom: 20px;
  right: 30px;
  z-index: 99;
  font-size: 18px;
  border: none;
  outline: none;
  background-color: red;
  color: white;
  cursor: pointer;
  padding: 15px;
  border-radius: 4px;
}

#myBtn:hover {
  background-color: #555;
}
</style>
<link id="bsdp-css" href="/bootstrap-datepicker/css/bootstrap-datepicker3.min.css" rel="stylesheet">
<script src="/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
<script src="/bootstrap-datepicker/locales/bootstrap-datepicker.uk.min.js" charset="UTF-8"></script>
<script src="/bootstrap-datepicker/locales/bootstrap-datepicker.vi.min.js" charset="UTF-8"></script>
<div class="row" style="padding-top: 5px; text-align: center; font-weight: bold;" runat="server" id="divSinhVien">
</div>
<div class="row" style="padding-top: 5px; text-align: center; font-weight: bold; color: red;" runat="server" id="divThongBao">
</div>
<div class="form-group">
    <label for="Anh">Ảnh:</label>
    <img class="img-rounded" runat="server" id="imgAnh" width="200">
    <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="form-control" />
</div>
<div class="form-group">
    <label for="Email">Email:</label>
    <asp:TextBox ID="txtEmail" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="EmailNhaTruong">Email nhà trường cấp:</label>
    <asp:TextBox ID="txtEmailNhaTruong" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="checkbox">
    <label>
        <asp:CheckBox ID="chkDaXacThucEmailNhaTruong" runat="server" />
        Đã xác thực email</label>
</div>
<div class="form-group">
    <label for="Mobile">Mobile:</label>
    <asp:TextBox ID="txtMobile" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="GioiTinh">Giới tính:</label>
    <asp:TextBox ID="txtGioiTinh" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="NgaySinh">Ngày sinh (dd/mm/yyyy):</label>
    <asp:TextBox ID="txtNgaySinh" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="NoiSinh">Nơi sinh:</label>
    <asp:TextBox ID="txtNoiSinh" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="DanToc">Dân tộc:</label>
    <asp:TextBox ID="txtDanToc" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="TonGiao">Tôn giáo:</label>
    <asp:TextBox ID="txtTonGiao" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="row" style="padding-left: 10px; text-align: left; font-weight: bold; color: orangered;">
    Hộ khẩu thường trú
</div>
<div class="form-group">
    <label for="HKTT_SoNha">Số nhà:</label>
    <asp:TextBox ID="txtHKTT_SoNha" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="HKTT_Pho">Phố/Thôn:</label>
    <asp:TextBox ID="txtHKTT_Pho" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="HKTT_Phuong">Phường/Xã:</label>
    <asp:TextBox ID="txtHKTT_Phuong" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="HKTT_Huyen">Quận/Huyện:</label>
    <asp:TextBox ID="txtHKTT_Huyen" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="HKTT_Tinh">Thành phố/Tỉnh:</label>
    <asp:TextBox ID="txtHKTT_Tinh" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="row" style="padding-left: 10px; text-align: left; font-weight: bold; color: orangered;">
    Chứng minh thư nhân dân
</div>
<div class="form-group">
    <label for="CMT">Số:</label>
    <asp:TextBox ID="txtCMT" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="CMTNgayCap">Ngày cấp (dd/mm/yyyy):</label>
    <asp:TextBox ID="txtCMTNGayCap" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="CMTNoiCap">Nơi cấp:</label>
    <asp:TextBox ID="txtCMTNoiCap" runat="server" class="form-control"></asp:TextBox>
</div>

<div class="form-group">
    <label for="NamTotNghiepPTTH">Tốt nghiệp THPT(BTVH) năm:</label>
    <asp:TextBox ID="txtNamTotNghiepPTTH" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="NgayVaoDoan">Ngày vào Đoàn (dd/mm/yyyy):</label>
    <asp:TextBox ID="txtNgayVaoDoan" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="NgayVaoDang">Ngày vào Đảng (dd/mm/yyyy):</label>
    <asp:TextBox ID="txtNgayVaoDang" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="DiemThiPTTH">Điểm thi PTTH Quốc gia:</label>
    <asp:TextBox ID="txtDiemThiPTTH" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="KhuVucHKTT">Hộ khẩu TT thuộc khu vực:</label>
    <asp:TextBox ID="txtKhuVucHKTT" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="DoiTuongUuTien">Đối tượng ưu tiên:</label>
    <asp:TextBox ID="txtDoiTuongUuTien" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="checkbox">
    <label>
        <asp:CheckBox ID="chkDaTungLamCanBoLop" runat="server" />
        Đã từng làm cán bộ lớp</label>
</div>
<div class="checkbox">
    <label>
        <asp:CheckBox ID="chkDatTungLamCanBoDoan" runat="server" />
        Đã từng làm cán bộ Đoàn</label>
</div>
<div class="checkbox">
    <label>
        <asp:CheckBox ID="chkDaThamGiaDoiTuyenThiHSG" runat="server" />
        Đã tham gia đội tuyển thi HSG</label>
</div>
<div class="row" style="padding-left: 10px; text-align: left; font-weight: bold; color: orangered;">
    Báo tin khi cần
</div>
<div class="form-group">
    <label for="BaoTin_DiaChi">Địa chỉ Bố, Mẹ hoặc Người nuôi dưỡng:</label>
    <asp:TextBox ID="txtBaoTin_DiaChi" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="BaoTin_HoVaTen">Họ tên người nhận:</label>
    <asp:TextBox ID="txtBaoTin_HoVaTen" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="BaoTin_DiaChiNguoiNhan">Địa chỉ người nhận:</label>
    <asp:TextBox ID="txtBaoTin_DiaChiNguoiNhan" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="BaoTin_SoDienThoai">Số điện thoại người nhận:</label>
    <asp:TextBox ID="txtBaoTin_SoDienThoai" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="BaoTin_Email">Email người nhận:</label>
    <asp:TextBox ID="txtBaoTin_Email" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="checkbox">
    <label>
        <asp:CheckBox ID="chkLaNoiTru" runat="server" />
        Có ở nội trú</label>
</div>
<div class="form-group">
    <label for="DiaChiCuThe">Địa chỉ nơi ở cụ thể:</label>
    <asp:TextBox ID="txtDiaChiCuThe" runat="server" class="form-control"></asp:TextBox>
</div>
<div class="form-group">
    <label for="HoSoDinhKem">Giấy khai sinh:</label>
    <span id="spHoSoDinhKem" runat="server"></span>
    <asp:FileUpload ID="fuHoSoDinhKem" runat="server" CssClass="form-control" />
</div>
<div class="row" style="padding: 5px; text-align: center; font-weight: bold;">
      <button type="button" class="btn btn-primary" id="btnModalUpdate" data-toggle="modal" data-target="#myModalUpdate">Cập nhật</button>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
     <asp:Button class="btn btn-primary" ID="btnCapNhat" runat="server" Text="Cập nhật" OnClick="btnCapNhat_Click" />
</div>
<div id="myModalUpdate" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="lblThongBaoXoa" style="text-align:center;">Bạn có chắc chắn muốn thực hiện ?</h4>
                <div id="edit_header_ThongBao1"></div>
            </div>
            <div class="modal-body" style="text-align: center;">
                <button type="button" class="btn btn-default" id="btnKhong" data-dismiss="modal">Không</button>
                <button type="button" class="btn btn-danger" id="btnXoa" onclick="StudentProfileEdit.update();">Thực hiện</button>
            </div>
        </div>
    </div>
</div>
<button onclick="topFunction()" id="myBtn" title="Go to top">Top</button>
<script>
//Get the button
var mybutton = document.getElementById("myBtn");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function() {scrollFunction()};

function scrollFunction() {
  if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
    mybutton.style.display = "block";
  } else {
    mybutton.style.display = "none";
  }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    var top = document.getElementById("topHeader").offsetTop; //Getting Y of target element
    window.scrollTo(0, top);
}
</script>
<script>
    var StudentProfileEdit = {
            update: function () {
                document.getElementById("<%=btnCapNhat.ClientID %>").click();
            }
        };
    $(document).ready(function () {

        var date_input_ngaysinh = $('#' + "<%=txtNgaySinh.ClientID %>"); //our date input has the name "date"
        var date_input_cmtngaycap = $('#' + "<%=txtCMTNGayCap.ClientID %>"); //our date input has the name "date
        var date_input_ngayvaodoan = $('#' + "<%=txtNgayVaoDoan.ClientID %>"); //our date input has the name "date"
        var date_input_cmtngayvaodang = $('#' + "<%=txtNgayVaoDang.ClientID %>"); //our date input has the name "date"


        //var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
        var options = {
            format: 'dd/mm/yyyy',
            //    container: container,
            todayHighlight: true,
            autoclose: true,
        };
        date_input_ngaysinh.datepicker(options);
        date_input_cmtngaycap.datepicker(options);
        date_input_ngayvaodoan.datepicker(options);
        date_input_cmtngayvaodang.datepicker(options);
    });
</script>
