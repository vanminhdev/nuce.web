<%@ Page Title="Cập nhật hồ sơ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CapNhatHoSoDayDu.aspx.cs" Inherits="Nuce.CTSV.CapNhatHoSoDayDu" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Begin Page Content -->
    <div class="container-fluid">
        <!-- Page Heading -->
        <h1 class="h3 mb-2 text-gray-800 text-center">Cập nhật hồ sơ
        </h1>

        <!-- DataTales Example -->
        <div class="card shadow mb-4">
            <div style="width: 50%;">
                <div class="modal-body ml-5 mt-3">
                    <span class="list-inline-item mt-2 h6" style="text-align: center; font-weight: bold; color: red;" runat="server" id="divThongBao"></span>
                    </br>
                    <span class="list-inline-item mt-2 h6">Email:</span>
                    <asp:TextBox ID="txtEmail" runat="server" class="form-control" name="subject"></asp:TextBox>
                    <span class="list-inline-item mt-2 h6">Mobile:</span>
                    <asp:TextBox ID="txtMobile" runat="server" class="form-control" name="subject"></asp:TextBox>
                    <span class="list-inline-item mt-2 h6">Địa chỉ bố, mẹ nuôi dưỡng
                    <span style="color: red;">(Báo tin khi cần)</span>:</span>
                    <asp:TextBox ID="txtBaoTin_DiaChi" runat="server" class="form-control" name="subject"></asp:TextBox>
                    <span class="list-inline-item mt-2 h6">Họ tên người nhận:</span>
                    <asp:TextBox ID="txtBaoTin_HoVaTen" runat="server" class="form-control" name="subject"></asp:TextBox>
                    <span class="list-inline-item mt-2 h6">Địa chỉ người nhận:</span>
                    <asp:TextBox ID="txtBaoTin_DiaChiNguoiNhan" runat="server" class="form-control" name="subject"></asp:TextBox>
                    <span class="list-inline-item mt-2 h6">Số điện thoại người nhận:</span>
                    <asp:TextBox ID="txtBaoTin_SoDienThoai" runat="server" class="form-control" name="subject"></asp:TextBox>
                    <span class="list-inline-item mt-2 h6">Email người nhận:</span>
                    <asp:TextBox ID="txtBaoTin_Email" runat="server" class="form-control" name="subject"></asp:TextBox>
                    <asp:CheckBox ID="chkLaNoiTru" runat="server" class="mt-3" />
                    Có nơi ở cụ thể
                  <br />
                    <span class="list-inline-item mt-2 h6">Địa chỉ nơi ở cụ thể:</span>
                    <asp:TextBox ID="txtDiaChiCuThe" runat="server" class="form-control" name="subject"></asp:TextBox>
                    <button type="button" id="btnModalUpdate" class="btn btn-primary mt-2" data-toggle="modal" data-target="#myModalUpdate">Cập nhật</button>
                </div>
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
                   Bạn không có quyền cập nhật. Xin cảm ơn bạn!
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
                document.getElementById("<%=btnCapNhat.ClientID %>").click();
            },
            showFormUpdate: function () {
                // alert('hi hi');
                $('#logoutModal').modal('show');
               
            }
        };
    </script>
    <span id="spScript" runat="server"></span>
</asp:Content>
