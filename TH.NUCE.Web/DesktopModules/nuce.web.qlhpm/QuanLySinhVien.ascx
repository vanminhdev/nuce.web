<%@ control language="C#" autoeventwireup="true" codebehind="QuanLySinhVien.ascx.cs" inherits="nuce.web.qlhpm.QuanLySinhVien" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td>
                <asp:Label ID="lblMaSV" runat="server" Text="Mã SV"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMaSV" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMay" runat="server" Text="Máy tính : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMay" runat="server" AutoPostBack="false">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTrangThai" runat="server" Text="Trạng thái : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlTrangThai" runat="server" AutoPostBack="false">
                    <asp:ListItem Value="1">Bình thường</asp:ListItem>
                    <asp:ListItem Value="2">Khoá trao đổi</asp:ListItem>
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
    <asp:TextBox ID="txtCaLopHoc" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

</div>
