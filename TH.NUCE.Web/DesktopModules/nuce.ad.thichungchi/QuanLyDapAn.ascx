<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyDapAn.ascx.cs" Inherits="nuce.web.thichungchi.QuanLyDapAn" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="../../controls/TextEditor.ascx" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div runat="server" id="divContent" class="table-responsive"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td width="15%;">
                <asp:Label ID="lblNoiDung" runat="server" Text="Nội dung : "></asp:Label>
            </td>
            <td width="85%">
                <dnn:texteditor id="txtNoiDung" runat="server" mode="Rich" width="100%" height="250"></dnn:texteditor>
            </td>
        </tr>
        <tr id="trMatching" runat="server">
            <td width="15%">
                <asp:Label ID="lblMatching" runat="server" Text="Nội dung ghép: "></asp:Label>
            </td>
            <td width="85%">
                <dnn:texteditor id="txtMatching" runat="server" mode="Rich" width="100%" height="250"></dnn:texteditor>
            </td>
        </tr>
        <tr style="display: none;">
            <td>
                <asp:Label ID="lblCauHoiID" runat="server" Text="Cau Hoi ID : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCauHoiID" runat="server" Width="20%"></asp:TextBox>
                <asp:TextBox ID="txtMaCauHoi" runat="server" Width="20%"></asp:TextBox>
                <asp:TextBox ID="txtMaBoCauHoi" runat="server" Width="20%"></asp:TextBox>
                <asp:TextBox ID="txtMonHoc" runat="server" Width="20%"></asp:TextBox>
            </td>
        </tr>
        <tr id="trIsChecked" runat="server">
            <td>
                <asp:Label ID="lblIsChecked" runat="server" Text="Là đáp án: "></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsChecked" runat="server" />
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
                <asp:Button ID="btnUpdate" runat="server" OnClientClick="return quanlycauhoi.fillData();" Text="Cập nhật" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnQuayLai" runat="server" Text="Quay lại" OnClick="btnQuayLai_Click" />

            </td>
        </tr>
    </table>
</div>

<div style="display: none;">
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtTuKhoa" runat="server"></asp:TextBox>

</div>


<script type="text/javascript">


    $(document).ready(function () {
        $("#" + "<%=txtOrder.ClientID%>").blur(function () {
            Format_onblur(this, 0);
        });
    });


</script>
