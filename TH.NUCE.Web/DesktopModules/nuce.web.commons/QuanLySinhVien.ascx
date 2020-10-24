<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLySinhVien.ascx.cs" Inherits="nuce.web.commons.QuanLySinhVien" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter" runat="server">
    <span>Nhập mã lớp: </span>
    <span>
        <asp:TextBox ID="txtMaLop" runat="server" Width="20%"></asp:TextBox>
        <asp:Button ID="btnTimLop" runat="server" Text="Tìm" OnClick="btnTimLop_Click" />
    </span>
</div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td width="15%">
                <asp:Label ID="lblMaSV" runat="server" Text="Mã SV : "></asp:Label>
            </td>
            <td width="85%">
                <asp:TextBox ID="txtMaSV" runat="server" Width="30%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTen" runat="server" Text="Tên : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTen" runat="server" Width="30%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblHo" runat="server" Text="Họ : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtHo" runat="server" Width="70%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblQueQuan" runat="server" Text="Quê quán : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtQueQuan" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNgayThangNamSinh" runat="server" Text="Ngày tháng năm sinh: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNgaySinh" runat="server" Width="100px"></asp:TextBox>
                <asp:HyperLink ID="lnkNgaySinh" runat="server">
                    <asp:Image ID="imgNgaySinh" runat="server" resourceKey="CalendarAlt.Text" />
                </asp:HyperLink>
                (dd/mm/yyyy)
            </td>
            <tr>
        <tr>
            <td>
                <asp:Label ID="lblMoTa" runat="server" Text="Mô tả : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMoTa" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNgayBatDau" runat="server" Text="Ngày bắt đầu: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNgayBatDau" runat="server" Width="100px"></asp:TextBox>
                <asp:HyperLink ID="lnkNgayBatDau" runat="server">
                    <asp:Image ID="imgNgayBatDau" runat="server" resourceKey="CalendarAlt.Text" />
                </asp:HyperLink>
                (dd/mm/yyyy)
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNgayKetThuc" runat="server" Text="Ngày kết thúc: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNgayKetThuc" runat="server" Width="100px"></asp:TextBox>
                <asp:HyperLink ID="lnkNgayKetThuc" runat="server">
                    <asp:Image ID="imgNgayKetThuc" runat="server" resourceKey="CalendarAlt.Text" />
                </asp:HyperLink>
                (dd/mm/yyyy)
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblThuTu" runat="server" Text="Thứ tự: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtOrder" runat="server" Text="1" Width="6%"></asp:TextBox>
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
    <asp:TextBox ID="txtLopQuanLyID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

</div>
