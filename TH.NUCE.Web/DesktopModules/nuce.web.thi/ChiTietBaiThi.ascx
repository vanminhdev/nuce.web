<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChiTietBaiThi.ascx.cs" Inherits="nuce.web.thi.ChiTietBaiThi" %>
<div id="divInitData" runat="server"></div>
<div class="menu-frame" runat="server" id="divMenu">
    <div style="width: 100%; text-align: center; padding-bottom: 10px; padding-top: 10px;" id="divMenuCauHoi" runat="server">
    </div>
</div>
<div class="content-frame">
    <div style="width: 100%;" runat="server" id="divContent">
    </div>
    <div style="float:right; padding-right:10px; color:red; cursor:pointer;" onclick="javascript:window.history.back();">Quay trở lại</div>
</div>
<div id="divProcessData" runat="server"></div>
<script>
    function gotocauhoi(thu_i) {
        $(document).scrollTop($("#divCauHoi_" + thu_i).offset().top - 50);
    };
    window.setTimeout("InitData();", 1000);
</script>
