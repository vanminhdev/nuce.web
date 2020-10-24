<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyNhom.ascx.cs" Inherits="nuce.web.ks.analytics.QuanLyNhom" %>
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
                <asp:Label ID="lblMoTa" runat="server" Text="Mô tả"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMoTa" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLoaiNhom" runat="server" Text="Loại nhóm"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlLoaiNhom" runat="server" AutoPostBack="false">
                    <asp:ListItem Selected="True" Value="1">Loại 1 - Tìm kiếm trên tất cả</asp:ListItem>
                    <asp:ListItem Value="2">Loai 2 - Chỉ tìm kiếm trên câu hỏi tích cực</asp:ListItem>
                    <asp:ListItem Value="3">Loại 3 - Chỉ tìm kiếm trên câu hỏi không tích cực</asp:ListItem>
                    <asp:ListItem Value="4">Loại 4 - Chỉ tìm kiếm trên câu hỏi đóng góp</asp:ListItem>
                    <asp:ListItem Value="5">Loại 5 - Chỉ tìm kiếm trên câu hỏi đóng góp và câu hỏi tích cực</asp:ListItem>
                    <asp:ListItem Value="6">Loại 6 - Chỉ tìm kiếm trên câu hỏi đóng góp và câu hỏi không tích cực</asp:ListItem>
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
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
</div>
