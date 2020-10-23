<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyPhongHoc.ascx.cs" Inherits="nuce.web.commons.QuanLyPhongHoc" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter" runat="server">
    <span>Chọn tòa nhà: </span>
    <span>
        <asp:DropDownList ID="ddlToaNha" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlToaNha_SelectedIndexChanged">
        </asp:DropDownList></span>
</div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td  width="10%">Tòa nhà : 
            </td>
            <td>
                <div runat="server" id="divNameToaNha" style="font-weight: bold; color: red;"></div>
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
                <asp:Label ID="lblIp1" runat="server" Text="Ip1 : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtIp1" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSubnetMask1" runat="server" Text="SubnetMask 1 : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubnetMask1" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDefaultgateway1" runat="server" Text="Defaultgateway1 : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDefaultgateway1" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblProxy1" runat="server" Text="Proxy1 : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtProxy1" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblIp2" runat="server" Text="AddInfo 1 : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtIp2" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSubnetMask2" runat="server" Text="AddInfo 2 : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubnetMask2" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDefaultgateway2" runat="server" Text="AddInfo 3 : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDefaultgateway2" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblProxy2" runat="server" Text="AddInfo 3 : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtProxy2" runat="server" Width="100%"></asp:TextBox>
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
            <td colspan="2" align="right">
                <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnQuayLai" runat="server" Text="Quay lại" OnClick="btnQuayLai_Click" />

            </td>
        </tr>
    </table>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtToaNhaID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

</div>
