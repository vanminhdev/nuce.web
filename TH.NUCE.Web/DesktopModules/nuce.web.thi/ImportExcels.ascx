<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportExcels.ascx.cs" Inherits="nuce.web.thi.ImportExcels" %>
<div style="text-align: center; font-weight:bold; color:red;" runat="server" id="divAnnounce">Import dữ liệu</div>
<div style="text-align: center;">
    Chọn file :  <asp:FileUpload ID="FU1"  runat="server" /> <asp:Button ID="btnUpload" runat="server" Text="Tải file" OnClick="btnUpload_Click" />
</div>
<div style="text-align: center;">Nội dung file excel</div>
<div style="text-align: center;" runat="server" id="divContent">Nội dung file excel</div>
<div style="text-align: right;">  <asp:Button ID="btnImport" runat="server" Text="Cập nhật dữ liệu" /></div>


