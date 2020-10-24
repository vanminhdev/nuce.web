﻿<%@ Page Title="Dịch vụ sinh viên" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DichVuSinhVien.aspx.cs" Inherits="Nuce.CTSV.DichVuSinhVien" %>

<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    <style>
        .service-banner {
            background: url(style/public/images/service-background.png) rgba(0, 0, 0, 0.45);
        }
    </style>
    dịch vụ
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Begin Page Content -->
    <div class="container-fluid">
        <div class="row">
            <div class="col-12 col-md-4 service-avatar-wrp cursor-pointer">
                <a
                href="/dichvu/xacnhan.aspx"
                class="row justify-content-center">
                <img
                    src="/Data/icons/giay-xac-nhan.jpg"
                    alt="service"
                    class="service-avatar col-10" />
                <div class="col-10 mt-2 text-center text-uppercase main-color fw-600 font-14-sm font-18-md font-18 title-service">
                    giấy xác nhận
                </div>
                </a>
            </div>
            <div class="col-12 col-md-4 service-avatar-wrp cursor-pointer">
                <a
                    href="/dichvu/gioithieu.aspx"
                    class="row justify-content-center">
                <img
                    src="/Data/icons/Giay-Gioi-Thieu-Max-Quality.jpg"
                    alt="service"
                    class="service-avatar col-10" />
                <div
                    class="col-10 mt-2 text-center text-uppercase main-color fw-600 font-14-sm font-18-md font-18 title-service">
                    giấy giới thiệu
                </div>
                </a>
            </div>
            <div class="col-12 col-md-4 service-avatar-wrp cursor-pointer">
                <a
                href="/dichvu/thuenhasinhvien.aspx"
                class="row justify-content-center">
                <img
                    src="/Data/icons/KTX.jpg"
                    alt="service"
                    class="service-avatar col-10" />
                <div
                    class="col-10 mt-2 text-center text-uppercase main-color fw-600 font-14-sm font-18-md font-18 title-service">
                    Thuê KTX Pháp Vân - Tứ Hiệp
                </div>
                </a>
            </div>
            <div class="col-12 col-md-4 service-avatar-wrp cursor-pointer">
                <a
                href="/dichvu/xacnhanuudaitronggiaoduc.aspx"
                class="row justify-content-center">
                <img
                    src="/Data/icons/Uu-dai-giao-duc-Max-Quality.jpg"
                    alt="service"
                    class="service-avatar col-10" />
                <div
                    class="col-10 mt-2 text-center text-uppercase main-color fw-600 font-14-sm font-18-md font-18 title-service">
                    ưu đãi trong giáo dục
                </div>
                </a>
            </div>
            <div class="col-12 col-md-4 service-avatar-wrp cursor-pointer">
                <a
                href="/dichvu/vayvonnganhang.aspx"
                class="row justify-content-center">
                <img
                    src="/Data/icons/VAY_VON.jpg"
                    alt="service"
                    class="service-avatar col-10"/>
                <div
                    class="col-10 mt-2 text-center text-uppercase main-color fw-600 font-14-sm font-18-md font-18 title-service">
                    vay vốn ngân hàng
                </div>
                </a>
            </div>
        </div>
    </div>
    <!-- /.container-fluid -->
</asp:Content>