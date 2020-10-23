<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateMac.aspx.cs" Inherits="nuce.web.UpdateMac" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="/Resources/Shared/scripts/jquery/jquery.js"></script>
<body>
    <form id="form1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>Chọn máy
                </td>
                <td>
                    <asp:DropDownList ID="ddlMay" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>macaddress</td>
                <td id="tdMac" runat="server"></td>
            </tr>
            <tr>
                <td>macaddress</td>
                <td>
                    <asp:TextBox ID="txtMac" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>macaddress sao chép</td>
                <td id="tdMacSaoChep" runat="server"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClick="btnUpdate_Click" />
                </td>
            </tr>
        </table>


    </form>
    <script>
        function showMacAddress() {
            var obj = new ActiveXObject("WbemScripting.SWbemLocator");
            var s = obj.ConnectServer(".");
            var properties = s.ExecQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
            var e = new Enumerator(properties);
            var output = '';
            while (!e.atEnd()) {
                e.moveNext();
                var p = e.item();
                if (!p) continue;
                //var Caption = p.Caption.indexOf("John");
                var Caption = p.Caption;
                var Pos = p.Caption.indexOf("Realtek");
                var Mac = p.MACAddress;
                try {
                    //if (Mac.length > 0) {
                    if (Pos > 0) {
                        output = p.MACAddress;
                    }
                }
                catch (ex) {

                }
            }
            var txtMacClient = '<%= txtMac.ClientID %>';
            $("#" + txtMacClient).val(output);
            document.getElementById(txtMacClient).value = output;
            document.getElementById('tdMac').innerHTML = output;
        };
        showMacAddress();

    </script>
</body>
</html>
