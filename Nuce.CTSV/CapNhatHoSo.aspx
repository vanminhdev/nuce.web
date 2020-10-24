<%@ Page Title="Cập nhật hồ sơ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CapNhatHoSo.aspx.cs" Inherits="Nuce.CTSV.CapNhatHoSo" %>
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
        <div class="row">
        <%--<div class="row">
            <div class="col-12 col-md-6">
            <div class="row">
                <div class="col-6">
                <div class="font-14-sm font-weight-bold mb-2">
                Ảnh đại diện <span class="sub-color">*</span>
                </div>
                <div class="custom-file">
                <input
                    id="input-file"
                    type="file"
                    class="custom-file-input"
                />
                <label
                    for="input-file"
                    class="custom-file-label text-truncate text-center"
                >
                    <img
                    src="../../public/images/icons/upload.png"
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
                    src="../../public/images/person1.png"
                    alt=""
                    class="upload-user-avatar"
                    />
                </div>
            </div>
        </div>--%>
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
        var CapNhatHoSo = {
            hienthithongbao: function () {
                $('#myModal').modal('show');
                $('#btnModalUpdate').hide();

            },
            back: function () {
                window.location.href = "/hososinhvien";
            },
            update: function () {
                console.log('sdsf');
                document.getElementById("<%=btnCapNhat.ClientID %>").click();
                console.log('sdsf', "<%=btnCapNhat.ClientID %>");
            },
            showFormUpdate: function () {
                // alert('hi hi');
                $('#logoutModal').modal('show');

            }
        };
    </script>
    <span id="spScript" runat="server"></span>
</asp:Content>
