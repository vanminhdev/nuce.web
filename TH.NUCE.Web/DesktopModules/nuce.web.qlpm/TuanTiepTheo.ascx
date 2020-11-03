<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TuanTiepTheo.ascx.cs" Inherits="nuce.web.qlpm.TuanTiepTheo" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div id="divTaoMoiTuanLamViec" style="text-align: center; font-weight: bold;" runat="server">
    <table width="300" style="margin-left: auto; margin-right: auto">
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
                <asp:Label ID="lblTenTuan" runat="server" Text="Tên tuần: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTenTuan" runat="server" Width="100px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClick="btnUpdate_Click" />
            </td>
        </tr>
    </table>
</div>
<div style="text-align: center; font-weight: bold;" id="divFilter" runat="server">
    Chọn phòng:  
    <asp:DropDownList ID="ddlPhongHoc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPhongHoc_SelectedIndexChanged1"></asp:DropDownList>
</div>

<div id="divContent" runat="server"></div>
<div id="divAction" runat="server" style="text-align: right;">
    <asp:Button ID="btnChuyenSangHienTai" runat="server" Text="Cập nhật sang tuần hiện tại" OnClick="btnChuyenSangHienTai_Click" />
</div>
<script>
    function goiTrangSuDangKiPhongHoc(url) {
        // dnnModal.show(url, true, 550, 950, true);
        window.location.assign(url);
    };
</script>
