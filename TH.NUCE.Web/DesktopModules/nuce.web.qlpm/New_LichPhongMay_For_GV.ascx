<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="New_LichPhongMay_For_GV.ascx.cs" Inherits="nuce.web.qlpm.New_LichPhongMay_For_GV" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center; font-weight: bold;" id="divFilter" runat="server">
    <table style="width: 100%; border: 1px;">
        <tr>
            <td style="width: 25%;">
                <asp:Label ID="lblChonPhong" runat="server" Text=" Chọn phòng : "></asp:Label>
                <asp:DropDownList ID="ddlPhongHoc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPhongHoc_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td style="width: 20%">
                <asp:Label ID="lblChonTuan" runat="server" Text=" Chọn Tuần : "></asp:Label>
                <asp:DropDownList ID="ddlTuan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTuan_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td style="width: 10%">
                <asp:Button ID="btnTuanTruoc" runat="server" Text="Tuần trước" OnClick="btnTuanTruoc_Click" />
            </td>
            <td style="width: 10%">
                <asp:Button ID="btnTuanHienTai" runat="server" Text="Tuần hiện tại" OnClick="btnTuanHienTai_Click" />
            </td>
            <td style="text-align: left; padding-bottom: 5px;">
                <asp:Button ID="btnTuanSau" runat="server" Text="Tuần sau" OnClick="btnTuanSau_Click" />
            </td>
        </tr>
    </table>


</div>
<div id="divContent" runat="server"></div>
