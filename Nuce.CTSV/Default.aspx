<%@ Page Title="Trang chủ" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Nuce.CTSV._Default" %>
<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    <style>
        .service-banner {
            background: url(/style/public/images/news-background.png) rgba(0, 0, 0, 0.45);
        }
    </style>
    tin tức
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Begin Page Content -->
    <div class="container service-categories-wrp news-wrp">
        <div class="row font-13-sm new-item-wrp mb-3">
            <div class="col-12 col-md-6">Lý do xác nhận:</div>
            <div class="col-12 col-md-6" id="divThongBaoSinhVien" runat="server">
                <a href="" style="display: block" class="main-color">Thông báo kiểm tra 1: 22-07-2020
                    <img src="/style/public/images/icons/warning.png"
                        alt=""
                        class="ml-3"
                    />
                </a>
            </div>
        </div>
        <div class="row font-13-sm new-item-wrp mb-3">
            <div class="col-12 col-md-6">Văn bản biểu mẫu:</div>
            <div class="col-12 col-md-6" id="divVanBan" runat="server"></div>
        </div>
        <div class="row font-13-sm new-item-wrp mb-3">
            <div class="col-12 col-md-6">Tuyển dụng:</div>
            <div class="col-12 col-md-6" id="divTuyenDung" runat="server"></div>
        </div>
        <div class="row font-13-sm new-item-wrp mb-3">
            <div class="col-12 col-md-6">Học bổng:</div>
            <div class="col-12 col-md-6" id="divHocBong" runat="server"></div>
        </div>
    </div>
      <%--  <!-- DataTales Example -->
        <div class="card shadow mb-4">
            <div class="card-body">
                <div class="card mb-3">
                    <div class="card-header">
                        Thông báo sinh viên
                   
                        <div class="d-inline-block float-right">
                            <button class="btn btn-primary">
                                <i class="fas fa-sync"></i>
                            </button>
                        </div>
                    </div>
                    <div class="card-body" id="divThongBaoSinhVien" runat="server">
                        <blockquote class="blockquote mb-0">
                            <div style="font-size: 1rem;">
                                <i class="fas fa-exclamation"></i>
                                <a href="#">Thông báo kiểm tra 1 - 22-07-2020</a>
                            </div>
                               <div style="font-size: 1rem;">
                                <i class="fas fa-exclamation"></i>
                                <a href="#">Thông báo kiểm tra 2 - 22-07-2020</a>
                            </div>
                        </blockquote>
                    </div>
                </div>
                <div class="card mb-3">
                    <div class="card-header">
                        Văn bản biểu mẫu
                   
                        <div class="d-inline-block float-right">
                            <button class="btn btn-primary">
                                <i class="fas fa-sync"></i>
                            </button>
                        </div>
                    </div>
                    <div class="card-body" id="divVanBan" runat="server">
                        <blockquote class="blockquote mb-0">
                            <div style="font-size: 1rem;">
                                <i class="fas fa-check"></i>
                                <a href="#">Quy chế miễn giảm học phí và hỗ trợ thực tập - 31-07-2020</a>
                            </div>

                        </blockquote>
                    </div>
                </div>
                <div class="card mb-3">
                    <div class="card-header">
                        Tuyển dụng
                   
                        <div class="d-inline-block float-right">
                            <button class="btn btn-primary">
                                <i class="fas fa-sync"></i>
                            </button>
                        </div>
                    </div>
                    <div class="card-body" id="divTuyenDung" runat="server">
                        <blockquote class="blockquote mb-0">
                        </blockquote>
                    </div>
                </div>
                <div class="card mb-3">
                    <div class="card-header">
                        Học bổng
                   
                        <div class="d-inline-block float-right">
                            <button class="btn btn-primary">
                                <i class="fas fa-sync"></i>
                            </button>
                        </div>
                    </div>
                    <div class="card-body"  id="divHocBong" runat="server">
                        <blockquote class="blockquote mb-0">
                   
                        </blockquote>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>
    <!-- /.container-fluid -->


</asp:Content>
