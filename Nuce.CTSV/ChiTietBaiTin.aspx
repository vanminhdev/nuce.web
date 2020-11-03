<%@ Page Title="Chi tiết bài tin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChiTietBaiTin.aspx.cs" Inherits="Nuce.CTSV.ChiTietBaiTin" %>

<asp:Content ID="BannerContent" ContentPlaceHolderID="Banner" runat="server">
    tin tức
</asp:Content>

<asp:Content ID="BreadCrumContent" ContentPlaceHolderID="BreadCrum" runat="server">
    <div class="d-flex align-items-center">
        <a href="/default.aspx">tin tức</a>
        <div id="circle" style="display: inline-block" class="ml-3 mr-3"></div>
    </div>
    <div class="main-color text-decoration-none" id="divThisArticle" runat="server"></div>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid service-categories-wrp news-wrp">
        <div class="row">
            <div runat="server" id="div_title" 
                class="col-12 text-uppercase main-color font-15-sm fw-600 mb-3">
                thông báo kiểm tra: 22-07-2020
            </div>
            <div runat="server" id="div_description" 
                class="col-12 font-13-sm font-weight-bold mb-3">
                Veniam ex nulla culpa cupidatat occaecat tempor deserunt cupidatat
                reprehenderit voluptate mollit dolore commodo minim. Officia enim
                dolore id qui laborum dolore reprehenderit pariatur reprehenderit.
                Lorem magna aute enim occaecat enim occaecat cupidatat incididunt
                consectetur. Ipsum ea laborum sit labore incididunt anim nostrud.
            </div>
            <div runat="server" id="div_new_content" class="col-12 font-13-sm">
                Veniam ex nulla culpa cupidatat occaecat tempor deserunt cupidatat
                reprehenderit voluptate mollit dolore commodo minim. Officia enim
                dolore id qui laborum dolore reprehenderit pariatur reprehenderit.
                Lorem magna aute enim occaecat enim occaecat cupidatat incididunt
                consectetur. Ipsum ea laborum sit labore incididunt anim nostrud.
            </div>
        </div>
    </div>
</asp:Content>
