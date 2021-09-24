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
        <div class="main-color font-weight-bold mb-3">
            Xem thêm tin tức tại 
            <a href="http://ctsv.nuce.edu.vn" target="_blank">trang web</a> hoặc
            <a href="https://www.facebook.com/Ph%C3%B2ng-C%C3%B4ng-t%C3%A1c-Ch%C3%ADnh-tr%E1%BB%8B-v%C3%A0-Qu%E1%BA%A3n-l%C3%BD-sinh-vi%C3%AAn-NUCE-113567187012488/?modal=admin_todo_tour" target="_blank">
                fan page
            </a>
            của <span>Phòng Công tác Chính trị và Quản lý Sinh viên</span>
        </div>
        <div class="row font-13-sm new-item-wrp mb-3">
            <div class="col-12 col-md-6">Thông báo:</div>
            <div class="col-12 col-md-6" id="divThongBaoSinhVien" runat="server"></div>
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
    <!-- End Page Content -->
</asp:Content>
