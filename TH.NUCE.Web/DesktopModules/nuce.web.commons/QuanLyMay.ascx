<%@ control language="C#" autoeventwireup="true" codebehind="QuanLyMay.ascx.cs" inherits="nuce.web.commons.QuanLyMay" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter" runat="server">
    <span>Chọn Phòng: </span>
    <span>
        <asp:DropDownList ID="ddlPhong" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPhong_SelectedIndexChanged">
        </asp:DropDownList></span>
</div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td width="10%">Phòng : 
            </td>
            <td>
                <div runat="server" id="divNamePhong" style="font-weight: bold; color: red;"></div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMa" runat="server" Text="Mã : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMa" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMac" runat="server" Text="MacAddress : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMac" runat="server" Width="100%"></asp:TextBox>
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
                <asp:Label ID="lblGhiChu" runat="server" Text="Chi Chú : "></asp:Label>
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
    <asp:TextBox ID="txtPhongID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
</div>
