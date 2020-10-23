<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyBoCauHoi.ascx.cs" Inherits="nuce.web.thi.QuanLyBoCauHoi" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter" runat="server">
    <span>Chọn Môn học: </span>
    <span>
        <asp:DropDownList ID="ddlNguoiDung_MonHoc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNguoiDung_MonHoc_SelectedIndexChanged">
        </asp:DropDownList></span>
</div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td>Tên môn học: 
            </td>
            <td>
                <div runat="server" id="divNameNguoiDung_MonHoc" style="font-weight: bold; color: red;"></div>
            </td>
        </tr>
        <tr>
            <td width="15%">
                <asp:Label ID="lblMa" runat="server" Text="Mã : "></asp:Label>
            </td>
            <td width="85%">
                <asp:TextBox ID="txtMa" runat="server" Width="30%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTen" runat="server" Text="Tên : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTen" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLoaiBoCauHoi" runat="server" Text="Loại bộ câu hỏi : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlLoaiBoCauHoi" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblThangDiem" runat="server" Text="Thang điểm : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlThangDiem" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnQuayLai" runat="server" Text="Quay lại" OnClick="btnQuayLai_Click" />

            </td>
        </tr>
    </table>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtNguoiDung_MonHocID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

</div>
