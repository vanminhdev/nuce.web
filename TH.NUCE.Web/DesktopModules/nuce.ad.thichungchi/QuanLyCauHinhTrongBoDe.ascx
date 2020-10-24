<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyCauHinhTrongBoDe.ascx.cs" Inherits="nuce.web.thichungchi.QuanLyCauHinhTrongBoDe" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div id="divThongKe" runat="server" style="border:1px;">Thống kê số câu hỏi</div>
<div id="divDanhSach" runat="server">
    <div runat="server" id="divContent"></div>
    <div style="float: right;">
        <asp:Button ID="btnTaoDe" runat="server" Text="Tạo đề" OnClick="btnTaoDe_Click" />
  <%--      <asp:Button ID="btnNhanBan" runat="server" Text="Nhân bản" OnClick="btnNhanBan_Click" />--%>
        <asp:Button ID="btnQuayLaiDanhSachBoDe" runat="server" Text="Quay lại" OnClick="btnQuayLaiDanhSachBoDe_Click" />
    </div>
</div>
<div runat="server" id="divEditPhan">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td colspan="2" runat="server" style="text-align:center; font-weight:bold; color:red; font-size:16;" id="tdAnnouncePhan"></td>
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
<div id="divEditPhanCauHinh" runat="server">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr style="display: none;">
            <td>
                <asp:Label ID="lblBoDe_Phan" runat="server" Text="Bo đề phần ID : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtBoDe_PhanID" runat="server" Width="20%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align:center; font-weight:bold; color:red; font-size:16;" colspan="2" runat="server" id="tdAnnounceBoDePhan"></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBoCauHoi" runat="server" Text="Bộ câu hỏi : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlBoCauHoi" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlBoCauHoi_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDoKho" runat="server" Text="Độ khó : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlDoKho" AutoPostBack="true" runat="server" Width="6%" OnSelectedIndexChanged="ddlDoKho_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td></td>
            <td style="text-align:left; font-weight:bold; color:blue;" runat="server" id="tdSoCauHoiToiDa"></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSoCauHoi" runat="server" Text="Số câu hỏi: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSoCauHoi" runat="server" Text="1" Width="6%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnCapNhatBoDe_Phan" runat="server" Text="Cập nhật" OnClick="btnCapNhatBoDe_Phan_Click" />
                <asp:Button ID="btnQuayLaiBoDe_Phan" runat="server" Text="Quay lại" OnClick="btnQuayLai_Click" />

            </td>
        </tr>
    </table>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtNguoiDung_MonHocID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtBoDeID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

    <asp:TextBox ID="txtSoCauHoiToiDaDuocChon" runat="server"></asp:TextBox>
</div>
