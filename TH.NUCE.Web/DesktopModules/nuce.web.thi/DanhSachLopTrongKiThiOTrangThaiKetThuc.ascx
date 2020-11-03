<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DanhSachLopTrongKiThiOTrangThaiKetThuc.ascx.cs" Inherits="nuce.web.thi.DanhSachLopTrongKiThiOTrangThaiKetThuc" %>
<div style="text-align:center; font-weight:bold;color:red; padding-bottom:10px;">DANH SÁCH CÁC LỚP Ở TRONG KÌ THI Ở TRẠNG THÁI THI XONG</div>
<div id="divMain" runat="server">
    <table style="border: 1px; width: 100%;" border="1">
        <tr>
            <td style="color: black; text-align: center; vertical-align: top; font-weight: bold;">DANH SÁCH CÁC LỚP
            </td>
            <td style="color: black; text-align: center; vertical-align: top; font-weight: bold;">DANH SÁCH SINH VIÊN
            </td>
        </tr>
        <tr>
            <td style="width: 30%; vertical-align: top;" id="tdMenu" runat="server"></td>
            <td style="width: 70%; vertical-align: top;" id="tdContent" runat="server">
                <div id="divContent" runat="server"></div>
                <div id="divAction" runat="server" style="text-align: right;">
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtKiThiLopHocID" runat="server"></asp:TextBox>
    <asp:TextBox runat="server" ID="txtData"></asp:TextBox>
</div>

