<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyDangKiDiemDanh.ascx.cs" Inherits="nuce.web.qlpm.QuanLyDangKiDiemDanh" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td>
                <asp:Label ID="lblTen" runat="server" Text="Tên"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTen" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNgay" runat="server" Text="Ngày"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNgay" runat="server" Width="100px"></asp:TextBox>
                <asp:HyperLink ID="lnkNgayBatDau" runat="server">
                    <asp:Image ID="imgNgayBatDau" runat="server" resourceKey="CalendarAlt.Text" />
                </asp:HyperLink>
                (dd/mm/yyyy)
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPhongHoc" runat="server" Text="Phòng Học"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlPhongHoc" runat="server"></asp:DropDownList>
            </td>
        </tr>
                <tr>
            <td>
                <asp:Label ID="lblCaHoc" runat="server" Text="Ca học"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlCaHoc" runat="server"></asp:DropDownList>
            </td>
        </tr>
                <tr>
            <td>
                <asp:Label ID="lblLopHoc" runat="server" Text="Lớp học"></asp:Label>
            </td>
            <td>
              <asp:DropDownList ID="ddlLopHoc" runat="server" AutoPostBack="False"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblGhiChu" runat="server" Text="Ghi Chú"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtGhiChu" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
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
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
</div>
