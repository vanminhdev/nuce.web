<%@ Page Title="Điều hướng" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="DieuHuong.aspx.cs" Inherits="Nuce.CTSV.DieuHuong" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
             
                 </div>
                 <div class="modal-footer">
                     <button
                         class="btn btn-secondary"
                         type="button"
                         data-dismiss="modal">
                         Thoát
                     </button>
                 </div>
             </div>
         </div>
    </div>
    <span runat="server" id="spAlert"></span>
</asp:Content>

