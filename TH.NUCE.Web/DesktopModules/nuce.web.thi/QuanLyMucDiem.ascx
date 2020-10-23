<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyMucDiem.ascx.cs" Inherits="nuce.web.commons.QuanLyMucDiem" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter" runat="server">
    <span>Chọn thang điểm: </span>
    <span>
        <asp:DropDownList ID="ddlThangDiem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlThangDiem_SelectedIndexChanged">
        </asp:DropDownList></span>
</div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="30%" cellpadding="10" cellspacing="10">
        <tr>
            <td>Thang điểm: 
            </td>
            <td>
                <div runat="server" id="divNameThangDiem" style="font-weight: bold; color: red;"></div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDoKho" runat="server" Text="Độ khóa : "></asp:Label>
            </td>
            <td>
                      <asp:dropdownlist runat="server" ID="ddlDoKho" ></asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDiem" runat="server" Text="Điểm : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDiem" runat="server" Text="1"></asp:TextBox>
          
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnQuayLai" runat="server" Text="Quay lại" OnClick="btnQuayLai_Click" />

            </td>
        </tr>
    </table>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtThangDiemID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

</div>
<script>
    $(document).ready(function () {
        $("#" + "<%=txtDiem.ClientID%>").blur(function () {
            Format_onblur(this, 2);
        });
<%--        $("#" + "<%=txtDoKho.ClientID%>").blur(function () {
            Format_onblur(this, 0);
        });--%>

    });
</script>
