<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiemDanh.ascx.cs" Inherits="nuce.web.qlpm.DiemDanh" %>
<div style="text-align: center; color: red;" runat="server" id="divAnnounce"></div>
<div id="divTaoDiemDanh" runat="server" style="text-align: center;">
    <asp:Button runat="server" ID="txtTaoDiemDanh" Text="Tạo điểm danh" OnClick="txtTaoDiemDanh_Click" />
     <input type="button" value="Quay lại" onclick="goBack();" />
</div>
<div id="divC" runat="server" style="width: 100%;">
    <div id="divContentC" runat="server" style="width: 100%;">
    </div>
    <div style="width: 100%; text-align: right;">
        <asp:Button runat="server" ID="btnCapNhat" Text="Cập nhật" OnClientClick=" return capnhatdulieu();" OnClick="btnCapNhat_Click" />
        <asp:Button runat="server" ID="btnCapNhatTatCa" Text="Cập nhật tất cả" OnClientClick="return capnhattatcadulieu();" OnClick="btnCapNhatTatCa_Click" />

        <asp:Button runat="server" ID="btnCapNhatTuDong" Text="Cập nhật tự động điểm danh sang" OnClientClick="capnhattudong();" OnClick="btnCapNhatTuDong_Click" />
        <asp:Button runat="server" ID="btnCapNhatTuDongTatCa" Text="Cập nhật tự động tất cả" OnClick="btnCapNhatTuDongTatCa_Click" />

        <asp:Button runat="server" ID="btnChotDuLieu" Text="Chốt dữ liệu" OnClick="btnChotDuLieu_Click" />
        <asp:Button runat="server" ID="btnGuiYeuCau" Text="Gửi yêu cầu điểm danh tự động" OnClientClick="return kiemtraguitudong();" OnClick="btnGuiYeuCau_Click" />
        <input type="button" value="Quay lại" onclick="goBack();" />
    </div>
</div>
<div id="divContentH" runat="server" style="width: 100%;"></div>
<div style="display: none;">
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtRequestStatus" runat="server"></asp:TextBox>
</div>
<div style="display: none;">
    <asp:TextBox runat="server" ID="txtData"></asp:TextBox>
</div>
<div style="text-align: right;" id="divQuayLai" runat="server">
    <input type="button" value="Quay lại" onclick="goBack();" />
</div>
<script>
    function goBack() {
        window.history.back();
    }
    function kiemtraguitudong() {
        var status = $("#<%=txtRequestStatus.ClientID%>").val();
        if (status == "-1" || status == "3" || status == "5")
            return true;
        else {
            return false;
        }
    }
    function xulydulieubandau() {
        var strData = $("#<%=txtData.ClientID%>").val();
        if (strData != "") {
            var res = strData.split(",");
            for (var i = 0; i < res.length; i++) {
                $("#" + res[i]).prop({ checked: true });
            }
        }
    };

    function capnhattatcadulieu() {
        //Lay du lieu cua cac o tich 
        //Dua du lieu vao txtData
        //Sau do cap nhat
        var strData = "";
        $("#<%=divContentC.ClientID%>").find("input:checkbox").each(function (i, ob) {
            // roles.push($(ob).val());
            if (ob.name == "namekiemtradanhdau")
                strData = strData + ob.id + "$$$" + $("#iddiemdanh_" + ob.id).prop('checked') + "$$$" + $("#idghichu" + ob.id).val() + "$$$" + $("#spsvid_" + ob.id).html() + "|||";
            //alert(ob.id);

        });
        if (strData == "") {
            alert("Ban chua chon du lieu cap nhat");
            return false;
        }
        else {
            //alert(strData);
            $("#<%=txtData.ClientID%>").val(strData);
        }

        return true;
    }
    function capnhatdulieu() {
        //Lay du lieu cua cac o tich 
        //Dua du lieu vao txtData
        //Sau do cap nhat
        var strData = "";
        $("#<%=divContentC.ClientID%>").find("input:checked").each(function (i, ob) {
            // roles.push($(ob).val());
            if (ob.name == "namekiemtradanhdau")
                strData = strData + ob.id + "$$$" + $("#iddiemdanh_" + ob.id).prop('checked') + "$$$" + $("#idghichu" + ob.id).val() + "$$$" + $("#spsvid_" + ob.id).html() + "|||";
            //alert(ob.id);

        });
        if (strData == "") {
            alert("Ban chua chon du lieu cap nhat");
            return false;
        }
        else {
            //alert(strData);
            $("#<%=txtData.ClientID%>").val(strData);
        }

        return true;
    };
    function capnhattudong() {
        //Lay du lieu cua cac o tich 
        //Dua du lieu vao txtData
        //Sau do cap nhat
        var strData = "";
        $("#<%=divContentC.ClientID%>").find("input:checked").each(function (i, ob) {
            // roles.push($(ob).val());
            if (ob.name == "namekiemtradanhdau")
                strData = strData + ob.id + "$$$" + $("#iddiemdanhtudong_" + ob.id).prop('checked') + "$$$" + $("#idghichu" + ob.id).val() + "$$$" + $("#spsvid_" + ob.id).html() + "|||";
            //alert(ob.id);

        });
        if (strData == "") {
            alert("Ban chua chon du lieu cap nhat");
            return false;
        }
        else {
            //alert(strData);
            $("#<%=txtData.ClientID%>").val(strData);
        }
        return true;
    };
    xulydulieubandau();
</script>
