<%@ Page Title="Cập nhật hồ sơ" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="CapNhatHoSo.aspx.cs" Inherits="Nuce.CTSV.CapNhatHoSo" %>
<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    <style>
        .service-banner {
            background: url(/style/public/images/update_profile-background.png) rgba(0, 0, 0, 0.45);
        }
    </style>
    cập nhật hồ sơ
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Begin Page Content -->
    <div class="container service-categories-wrp news-wrp">
        <div class="text-uppercase fw-600 main-color font-15-sm font-25 mb-4">
            cập nhật các thông tin cơ bản
        </div>
        <div class="row">
            <div class="col-12">
                <span class="list-inline-item mt-2 h6" style="text-align: center; font-weight: bold; color: red;" runat="server" id="divThongBao"></span>
            </div>
        </div>
        <div class="row" style="display: none;">
            <div class="col-12 col-md-6">
                <div class="row">
                    <div class="col-6">
                        <div class="font-14-sm font-weight-bold mb-2">
                        Ảnh đại diện <span class="sub-color">*</span>
                        </div>
                    <div class="custom-file">
                        <asp:FileUpload ID="fileAvatar" runat="server"
                                        type="file"
                                        CssClass="custom-file-input" />
                        <label
                            for="<%= fileAvatar.ClientID %>"
                            class="custom-file-label text-truncate text-center"
                        >
                            <img
                            src="/style/public/images/icons/upload.png"
                            alt="upload-icon"
                            class="mr-1"
                            />
                            Choose file</label
                        >
                    </div>
                    <div class="font-10-sm mt-2 sub-upload-file">
                        Dung lượng < 1MB. Định dạng hỗ trợ: jpg, jpeg, png, gift
                    </div>
                </div>
                    <div class="col-6">
                        <img
                            id="imgAvaPreview"
                            src="/style/public/images/person1.png"
                            alt=""
                            class="upload-user-avatar"
                        />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-6">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        Email <span class="sub-color">*</span>
                    </div>
                    <asp:TextBox ID="txtEmail" runat="server" class="form-control mt-3" name="subject"></asp:TextBox>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group">
                    <div class="form-group">
                        <div class="fw-700 font-14-sm">
                            Điện thoại <span class="sub-color">*</span>
                        </div>
                        <asp:TextBox ID="txtMobile" runat="server" class="form-control mt-3" name="subject"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-6">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        Họ tên người nhận <span class="sub-color">*</span>
                    </div>
                    <asp:TextBox ID="txtBaoTin_HoVaTen" runat="server" 
                        class="form-control mt-3" name="subject" placeholder="Họ tên"></asp:TextBox>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group">
                <div class="fw-700 font-14-sm">
                    Địa chỉ bố, mẹ nuôi dưỡng
                    <span class="sub-color">(Báo tin khi cần)</span>
                </div>
                <asp:TextBox ID="txtBaoTin_DiaChi" runat="server" 
                    class="form-control mt-3" placeholder="Địa chỉ..." name="subject"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-6">
                <div class="fw-700 font-14-sm">
                    Email người nhận <span class="sub-color">*</span>
                </div>
                <asp:TextBox ID="txtBaoTin_Email" runat="server" 
                    class="form-control mt-3" name="subject" placeholder="Email"></asp:TextBox>    
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group">
                <div class="fw-700 font-14-sm">
                    Địa chỉ người nhận <span class="sub-color">*</span>
                </div>
                <asp:TextBox ID="txtBaoTin_DiaChiNguoiNhan" runat="server" 
                    class="form-control mt-3" name="subject" placeholder="Địa chỉ..."></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-6">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        Số điện thoại người nhận <span class="sub-color">*</span>
                    </div>
                    <asp:TextBox ID="txtBaoTin_SoDienThoai" runat="server" 
                        class="form-control mt-3" name="subject" placeholder="Số điện thoại..."></asp:TextBox>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        Địa chỉ đăng ký nhận chuyển phát nhanh
                    </div>
                    <asp:TextBox ID="txtBaoTin_DiaChiChuyenPhatNhanh" runat="server" 
                        class="form-control mt-3" name="subject" placeholder="Địa chỉ chuyển phát nhanh..."></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-6">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        Dân tộc
                    </div>
                    <select class="form-control" enableviewstate="true" runat="server" id="slDanToc">
                        <option value="">---Chọn---</option>
		                <option selected="selected" value="01">Kinh</option>
		                <option value="02">Tày</option>
		                <option value="03">Thái</option>
		                <option value="04">Hoa</option>
		                <option value="05">Khơ-me</option>
		                <option value="06">Mường</option>
		                <option value="07">Nùng</option>
		                <option value="08">Hmông</option>
		                <option value="09">Dao</option>
		                <option value="10">Gia-rai</option>
		                <option value="11">Ngái</option>
		                <option value="12">Ê-đê</option>
		                <option value="13">Ba-na</option>
		                <option value="14">Xơ-đăng</option>
		                <option value="15">Sán Chay</option>
		                <option value="16">Cơ-ho</option>
		                <option value="17">Chăm</option>
		                <option value="18">Sán Dìu</option>
		                <option value="19">Hrê</option>
		                <option value="20">Mnông</option>
		                <option value="21">Ra-glai</option>
		                <option value="22">Xtiêng</option>
		                <option value="23">Bru-Vân Kiều</option>
		                <option value="24">Thổ</option>
		                <option value="25">Giáy</option>
		                <option value="26">Cơ-tu</option>
		                <option value="27">Gié-Triêng</option>
		                <option value="28">Mạ</option>
		                <option value="29">Khơ-mú</option>
		                <option value="30">Co</option>
		                <option value="31">Ta-ôi</option>
		                <option value="32">Chơ-ro</option>
		                <option value="33">Kháng</option>
		                <option value="34">Xinh-mun</option>
		                <option value="35">Hà Nhì</option>
		                <option value="36">Chu-ru</option>
		                <option value="37">Lào</option>
		                <option value="38">La Chi</option>
		                <option value="39">La Ha</option>
		                <option value="40">Phù Lá</option>
		                <option value="41">La Hủ</option>
		                <option value="42">Lự</option>
		                <option value="43">Lô Lô</option>
		                <option value="44">Chứt</option>
		                <option value="45">Mảng</option>
		                <option value="46">Pà Thẻn</option>
		                <option value="47">Cơ Lao</option>
		                <option value="48">Cống</option>
		                <option value="49">Bố Y</option>
		                <option value="50">Si La</option>
		                <option value="51">Pu Péo</option>
		                <option value="52">Brâu</option>
		                <option value="53">Ơ Đu</option>
		                <option value="54">Rơ-măm</option>
		                <option value="55">Người nước ngoài</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-6">
                <div class="form-group">
                    <label class="cursor-pointer">
                        <asp:CheckBox ID="chkLaNoiTru" runat="server" class="mt-3" />
                        <span class="fw-700 font-14-sm">Có nơi ở cụ thể</span>
                    </label>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-12">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        Địa chỉ nơi ở cụ thể <span class="sub-color">*</span>
                    </div>
                    <asp:TextBox ID="txtDiaChiCuThe" runat="server" 
                                class="form-control mt-3" name="subject" placeholder="Địa chỉ..."></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-12">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        <span class="sub-color">Hộ khẩu thường trú:</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-12">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        Tỉnh/thành phố <span class="sub-color">*</span>
                    </div>
                    <select class="form-control" onchange="CapNhatHoSo.changeTinh(this)" runat="server" id="slThanhPho">
                    </select>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-12">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        Quận/huyện <span class="sub-color">*</span>
                    </div>
                    <select class="form-control" enableviewstate="true" onchange="CapNhatHoSo.changeQuan(this)" runat="server" id="slQuan">
                    </select>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12 col-md-12">
                <div class="form-group">
                    <div class="fw-700 font-14-sm">
                        Phường/xã <span class="sub-color">*</span>
                    </div>
                    <select class="form-control" enableviewstate="true" runat="server" id="slPhuong">
                    </select>
                </div>
            </div>
        </div>
        <div class="row mt-3 justify-content-center">
            <div class="col-12 col-md-3">
                <button
                    class="confirm-btn text-light w-100 text-uppercase font-14-sm pt-2 pb-2"
                    type="button"
                    data-toggle="modal" data-target="#myModalUpdate">
                    cập nhật
                </button>
            </div>
        </div>
    </div>
    <!-- /.container-fluid -->
    <div style="display: none;">
        <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật" OnClick="btnCapNhat_Click" />
    </div>
    <!-- Modal xoa -->
    <div
        class="modal fade"
        id="myModal"
        tabindex="-1"
        role="dialog"
        aria-labelledby="exampleModalLabel"
        aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">THÔNG BÁO</h5>
                    <button
                        class="close"
                        type="button"
                        data-dismiss="modal"
                        aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    Chưa đến thời gian cập nhật hồ sơ. Xin cảm ơn bạn!
                </div>
                <div class="modal-footer">
                    <button
                        class="btn btn-secondary"
                        type="button" onclick="CapNhatHoSo.back();">
                        Quay trở lại trang hồ sơ
                    </button>
                </div>
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
                    <h5 class="modal-title" id="exampleModalLabel">Bạn có chắc chắn muốn cập nhật?</h5>
                    <button
                        class="close"
                        type="button"
                        data-dismiss="modal"
                        aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    Bạn hãy chọn "Cập nhật" dưới đây nếu thực sự muốn cập nhật thông tin.
                </div>
                <div class="modal-footer">
                    <button
                        class="btn btn-secondary"
                        type="button"
                        data-dismiss="modal">
                        Thoát
           
                    </button>
                    <button type="button" class="btn btn-danger" id="btnCapNhat1" onclick="CapNhatHoSo.update();">Cập nhật</button>
                </div>
            </div>
        </div>
    </div>
    <script src="/js/phuong-quan-thanh-pho.js"></script>
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
        var init = function() {
            const elFile = document.getElementById('<%= fileAvatar.ClientID %>');
            elFile.addEventListener('change', function() {
                CapNhatHoSo.onChangeFileUpload();
            });

            $("#imgAvaPreview").attr('src', '<%= m_SinhVien.IMG %>?<%= DateTime.Now.ToFileTime() %>');
        };
        var CapNhatHoSo = {
            hienthithongbao: function () {
                $('#myModal').modal('show');
                $('#btnModalUpdate').hide();
            },
            back: function () {
                window.location.href = "/hososinhvien";
            },
            update: function () {
                document.getElementById("<%=btnCapNhat.ClientID %>").click();
            },
            onChangeFileUpload: function() {
                const elFile = document.getElementById('<%= fileAvatar.ClientID %>');
                if (!elFile) return;
                
                const file = elFile.files[0];
                console.log(elFile, file, elFile.files);

                const reader = new FileReader();
                reader.onload = function(e) {
                    $("#imgAvaPreview").attr('src', e.target.result);
                }; 
                reader.readAsDataURL(file);
            },
            showFormUpdate: function () {
                $('#logoutModal').modal('show');
            },
            initSelectForm: function(tinhValue = '', quanValue = '', phuongValue = '') {
                let tinhOptions = '';
                let maTinhValue = tinhList[0].MATP;
                tinhList.forEach(item => {
                    let selected = '';
                    if (tinhValue === item.TENTPVN) {
                        selected = 'selected';
                        maTinhValue = item.MATP;
                    }
                    tinhOptions += `<option ${selected} data-ma-tinh="${item.MATP}" value="${item.TENTPVN}">${item.TENTPVN}</option>`;
                });
                $(`#<%= slThanhPho.ClientID %>`).html(tinhOptions);

                let quanOptions = '';
                let maQuanValue = quanList[0].MAQH;
                quanList.filter(item => item.MATP === maTinhValue).forEach(item => {
                    let selected = '';
                    if (quanValue === item.TENQHVN) {
                        selected = 'selected';
                        maQuanValue = item.MAQH;
                    }
                    quanOptions += `<option ${selected} data-ma-tinh="${item.MATP}" data-ma-quan="${item.MAQH}" value="${item.TENQHVN}">${item.TENQHVN}</option>`;
                });
                $(`#<%= slQuan.ClientID %>`).html(quanOptions);

                let phuongOptions = '';
                phuongList.filter(item => item.maqh === maQuanValue).forEach(item => {
                    let selected = phuongValue === item.tenpxvn ? 'selected' : '';
                    phuongOptions += `<option ${selected} data-ma-tinh="${item.matp}" data-ma-quan="${item.maqh}" data-ma-phuong="${item.mapx}" value="${item.tenpxvn}">${item.tenpxvn}</option>`;
                });
                $(`#<%= slPhuong.ClientID %>`).html(phuongOptions);
            },
            changeTinh: function(e) {
                const maTinh = e.options[e.selectedIndex].getAttribute('data-ma-tinh');

                let quanOptions = '';
                var filteredQuan = quanList.filter(item => item.MATP === maTinh);
                filteredQuan.forEach(item => {
                    quanOptions += `<option data-ma-tinh="${item.MATP}" data-ma-quan="${item.MAQH}" value="${item.TENQHVN}">${item.TENQHVN}</option>`;
                });
                $(`#<%= slQuan.ClientID %>`).html(quanOptions);

                let phuongOptions = '';
                phuongList.filter(item => item.maqh === filteredQuan[0].MAQH).forEach(item => {
                    phuongOptions += `<option data-ma-tinh="${item.matp}" data-ma-quan="${item.maqh}" data-ma-phuong="${item.mapx}" value="${item.tenpxvn}">${item.tenpxvn}</option>`;
                });
                $(`#<%= slPhuong.ClientID %>`).html(phuongOptions);
            },
            changeQuan: function(e) {
                const maQuan = e.options[e.selectedIndex].getAttribute('data-ma-quan');

                let phuongOptions = '';
                phuongList.filter(item => item.maqh === maQuan).forEach(item => {
                    phuongOptions += `<option data-ma-tinh="${item.matp}" data-ma-quan="${item.maqh}" data-ma-phuong="${item.mapx}" value="${item.tenpxvn}">${item.tenpxvn}</option>`;
                });
                $(`#<%= slPhuong.ClientID %>`).html(phuongOptions);
            },
        };
        init();
    </script>
    <span id="spScript" runat="server"></span>
</asp:Content>
