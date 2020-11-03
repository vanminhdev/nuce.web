<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportExcelInLopHoc.ascx.cs" Inherits="nuce.web.commons.ImportExcelInLopHoc" %>
<%@ Register TagPrefix="dnn" TagName="URLControl" Src="../../controls/URLControl.ascx" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divInportExcel_Upload" runat="server">
    <div style="text-align: center;">
        Chọn file : 
            <asp:FileUpload ID="FU1" runat="server" />
        <asp:Button ID="btnUpload" runat="server" Text="Tải file" OnClick="btnUpload_Click" />
    </div>
    <div style="text-align: center;">Nội dung file excel</div>
    <div style="text-align: center;" runat="server" id="divContentExcel"></div>
    <div style="text-align: right;">
        <asp:Button ID="btnImport" runat="server" Text="Cập nhật dữ liệu" OnClick="btnImport_Click" />
        <asp:Button ID="btnQuayTroLaiMain" runat="server" Text="Quay trở lại danh sách" OnClick="btnQuayTroLaiMain_Click" />
    </div>
</div>
