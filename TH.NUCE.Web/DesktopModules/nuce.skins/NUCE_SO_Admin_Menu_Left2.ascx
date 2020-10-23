<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NUCE_SO_Admin_Menu_Left2.ascx.cs"
    Inherits="DotNetNuke.UI.Skins.Controls.NUCE_SO_Admin_Menu_Left2" %>
<div class="category1 fl" id="divContent" runat="server">
</div>

<script>
    $(document).ready(function () {
        // second example
        $("#ulMenuLeft").treeview({
            animated: "normal",
            persist: "cookie"
        });
    });
</script>
