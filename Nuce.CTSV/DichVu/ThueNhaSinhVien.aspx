<%@ Page Title="Dịch vụ sinh viên" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ThueNhaSinhVien.aspx.cs" Inherits="Nuce.CTSV.ThueNhaSinhVien" %>

<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    dịch vụ
</asp:Content>

<asp:Content ID="BreadCrumContent" ContentPlaceHolderID="BreadCrum" runat="server">
    <div class="d-flex align-items-center">
        <a href="/dichvusinhvien.aspx">dịch vụ</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="main-color text-decoration-none">thuê ktx pháp vân - tứ hiệp</div>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="edit_header">Modal Header</h4>
                    <div id="edit_header_ThongBao" style="color: red;"></div>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="TrangThai">Nhập mã:</label>
                        <input type="text" class="form-control" id="txtMa">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                    <button type="button" class="btn btn-default" id="btnThueNhaSinhVienXa" onclick=" ThueNhaSinhVien.update();">Xác nhận mã</button>
                </div>
            </div>

        </div>
    </div>
    <!-- Modal xoa -->
    <div id="myModal1" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="lblThongBaoXoa">Bạn có chắc chắn muốn thực hiện ?</h4>
                    <div id="edit_header_ThongBao1"></div>
                </div>
                <div class="modal-body" style="text-align: center;">
                    <button type="button" class="btn btn-default" id="btnKhong" data-dismiss="modal">Không</button>
                    <button type="button" class="btn btn-danger" id="btnXoa" onclick=" ThueNhaSinhVien.delete();">Thực hiện</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Hoan thanh -->
    <div id="myModal2" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="lblThongHoanThanh">Bạn có chắc chắn muốn thực hiện ?</h4>
                    <div id="edit_header_ThongBao2"></div>
                </div>
                <div class="modal-body" style="text-align: center;">
                    <button type="button" class="btn btn-default" id="btnKhong2" data-dismiss="modal">Không</button>
                    <button type="button" class="btn btn-danger" id="btnHoanThanh" onclick=" ThueNhaSinhVien.complete();">Thực hiện</button>
                </div>
            </div>
        </div>
    </div>




    <div class="container-fluid">
        <div class="col-12">
            <div class="row justify-content-end">
                <div class="col-12 col-md-3">
                    <button
                        class="confirm-btn text-light w-100 text-uppercase font-14-sm pt-2 pb-2"
                        type="button"
                        onclick="ThueNhaSinhVien.ThemMoi();"
                        >
                        Thêm mới
                    </button>
                </div>
            </div>
        </div>

        <div class="col-12">
            <div
                class="text-uppercase font-15-sm fw-600 font-25 service-title"
                >
                đã gửi và xác nhận
            </div>

            <table class="table table-bordered custom-table">
                <thead class="text-uppercase text-center text-light">
                    <tr>
                        <th scope="col">STT</th>
                        <th scope="col">ngày tạo</th>
                        <th scope="col">nội dung</th>
                        <th scope="col">trạng thái</th>
                    </tr>
                </thead>
                <tbody runat="server" id="tbContent"></tbody>
            </table>
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
        var ThueNhaSinhVien = {
            ID: -1,
            Status: 1,
            Data: [],
            url: "/Handler/",
            initData: function () {
            },
            fillData: function (id) {
                ThueNhaSinhVien.ID = id;
                $("#edit_header").html("Nhập mã xác nhận");
                $("#edit_header_ThongBao").hide();
            },
            update: function () {
                // Kiem tra ma trang
                var strMa = $("#txtMa").val().trim();

                if (strMa == "") {
                    $("#edit_header_ThongBao").show();
                    $("#edit_header_ThongBao").html("Bạn hãy nhập mã");
                    return;
                }

                if (ThueNhaSinhVien.ID > 0) {
                    try {
                        //Update
                        $.getJSON(this.url + "CapNhatMaYeuCauDichVu.aspx?ID=" + ThueNhaSinhVien.ID + "&&Ma=" + strMa + "&&Type=1", function (data) {
                            $("#edit_header_ThongBao").show();
                            if (data == 1) {
                                $("#edit_header_ThongBao").html("Cập nhật thành công");
                                setTimeout(function () {
                                    window.location.href = "/dichvu/ThueNhaSinhVien";
                                }, 2000)
                            }
                            else {
                                if (data == -1) {
                                    $("#edit_header_ThongBao").html("Bạn nhập sai mã xác thực");
                                }
                                else {
                                    $("#edit_header_ThongBao").html(data);
                                }
                            }
                        });
                    }
                    catch (ex) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html(ex);
                    }
                }
            },
            initDelete: function (id, message) {
                ThueNhaSinhVien.ID = id;
                $("#lblThongBaoXoa").html(message);
                $("#edit_header_ThongBao1").hide();
                $('#btnXoa').show();
                //$("#btnXoa").on("click", ThueNhaSinhVien.delete);
            },
            delete: function () {
                //Update
                $("#edit_header_ThongBao1").show();
                try {
                    $.getJSON(this.url + "HuyDichVu.aspx?ID=" + ThueNhaSinhVien.ID + "&&Type=1", function (data) {
                        if (data == 1) {

                            $("#edit_header_ThongBao1").html("Thực hiện thành công");
                            $('#btnKhong').text("Thoát");
                            $('#btnXoa').hide();
                            setTimeout(function () {
                                window.location.href = "/dichvu/ThueNhaSinhVien";
                            }, 2000)
                        }
                        else {
                            if (data == -1) {
                                $("#edit_header_ThongBao1").html("Lỗi hệ thống");
                            }
                            else {
                                $("#edit_header_ThongBao1").html(data);
                            }
                        }
                    });
                }
                catch (ex) {
                    $("#edit_header_ThongBao1").html(ex);
                }
            },
            initComplete: function (id, message) {
                ThueNhaSinhVien.ID = id;
                $("#lblThongHoanThanh").html(message);
                $("#edit_header_ThongBao2").hide();
                $('#btnHoanThanh').show();
                //$("#btnXoa").on("click", ThueNhaSinhVien.delete);
            },
            complete: function () {
                //Update
                $("#edit_header_ThongBao2").show();
                try {
                    $.getJSON(this.url + "HoanThanhDichVu.aspx?ID=" + ThueNhaSinhVien.ID + "&&Type=1", function (data) {
                        if (data == 1) {

                            $("#edit_header_ThongBao2").html("Thực hiện thành công");
                            $('#btnKhong2').text("Thoát");
                            $('#btnHoanThanh').hide();
                            setTimeout(function () {
                                window.location.href = "/dichvu/ThueNhaSinhVien";
                            }, 2000)
                        }
                        else {
                            if (data == -1) {
                                $("#edit_header_ThongBao2").html("Lỗi hệ thống");
                            }
                            else {
                                $("#edit_header_ThongBao2").html(data);
                            }
                        }
                    });
                }
                catch (ex) {
                    $("#edit_header_ThongBao2").html(ex);
                }
            },
            ThemMoi: function () {
                window.location.href = "/dichvu/thuenhasinhvien_yeucaumoi";
            }
        };
    </script>
</asp:Content>
