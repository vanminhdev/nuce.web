<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DanhSachDeTrongBoDe.ascx.cs" Inherits="nuce.web.thichungchi.DanhSachDeTrongBoDe" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align:center; margin-top:10px;">
Đề:<asp:DropDownList ID="ddlMaDe"  runat="server" AutoPostBack="false"></asp:DropDownList><asp:Button Text="Hiển thị đề"  runat="server" ID="btnShow" OnClick="btnShow_Click"/><asp:Button Text="Quay lại"  runat="server" ID="btnReturn" OnClick="btnReturn_Click"/>
   
</div>
<div runat="server" id="divContent" style="margin-top:10px;"></div>
