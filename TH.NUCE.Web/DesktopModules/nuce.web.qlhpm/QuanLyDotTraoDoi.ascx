<%@ control language="C#" autoeventwireup="true" codebehind="QuanLyDotTraoDoi.ascx.cs" inherits="nuce.web.qlhpm.QuanLyDotTraoDoi" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div runat="server" id="divCaHoc" style="font-weight: bold; color: red; text-align: center;"></div>
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
                <asp:Label ID="lblBatDau" runat="server" Text="Bắt đầu : "></asp:Label>
            </td>
            <td>
                <table>
                    <tr>
                        <td>Giờ:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGioBatDau" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Phút:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPhutBatDau" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblKetThuc" runat="server" Text="Kết thúc : "></asp:Label>
            </td>
            <td>
                <table>
                    <tr>
                        <td>Giờ:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGioKetThuc" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Phút:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPhutKetThuc" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblFileChoPhep" runat="server" Text="File cho phép"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFileChoPhep" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td style="width:20%;">
                <asp:Label ID="lblDungLuongChoPhep" runat="server" Text="Dung lượng tối đa cho phép(B):"></asp:Label>
            </td>
            <td style="width:80%;">
                <asp:TextBox ID="txtDungLuong" runat="server" Width="30%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLoaiTraoDoi" runat="server" Text="Loại : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlLoai" runat="server" AutoPostBack="false">
                    <asp:ListItem Value="1">Loại 1</asp:ListItem>
                    <asp:ListItem Value="2">Loại 2</asp:ListItem>
                </asp:DropDownList>
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
                <asp:Label ID="lblTrangThai" runat="server" Text="Trạng thái : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlTrangThai" runat="server" AutoPostBack="false">
                    <asp:ListItem Value="1">Gốc</asp:ListItem>
                    <asp:ListItem Value="2">Thực hiện</asp:ListItem>
                    <asp:ListItem Value="3">Hoàn thành</asp:ListItem>
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
    <asp:TextBox ID="txtCaHoc" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

</div>
