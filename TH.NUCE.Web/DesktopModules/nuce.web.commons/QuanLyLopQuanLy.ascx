<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyLopQuanLy.ascx.cs" Inherits="nuce.web.commons.QuanLyLopQuanLy" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter" runat="server">
    <span>Chọn khoa: </span>
    <span>
        <asp:DropDownList ID="ddlKhoa" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlKhoa_SelectedIndexChanged">
        </asp:DropDownList></span>
</div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td>Khoa: 
            </td>
            <td>
                <div runat="server" id="divNameKhoa" style="font-weight: bold; color: red;"></div>
            </td>
        </tr>
        <tr>
            <td width="15%">
                <asp:Label ID="lblNamHoc" runat="server" Text="Năm học : "></asp:Label>
            </td>
            <td width="85%">
                <asp:DropDownList ID="ddlNamHoc" runat="server">
                </asp:DropDownList>
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
            <td colspan="2" align="right">
                <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnQuayLai" runat="server" Text="Quay lại" OnClick="btnQuayLai_Click" />

            </td>
        </tr>
    </table>
</div>
<div runat="server" id="divChangeKhoa">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td width="15%">Chọn khoa chuyển:
            </td>
            <td width="85%">

                <asp:DropDownList ID="ddlChangeKhoa" runat="server">
                </asp:DropDownList></span></td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnChuyenKhoa" runat="server" Text="Chuyển khoa" OnClick="btnChuyenKhoa_Click"/>
            </td>
        </tr>
    </table>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtKhoaID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
</div>
