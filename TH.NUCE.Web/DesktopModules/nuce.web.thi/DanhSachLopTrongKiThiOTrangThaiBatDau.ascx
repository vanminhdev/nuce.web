<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DanhSachLopTrongKiThiOTrangThaiBatDau.ascx.cs" Inherits="nuce.web.thi.DanhSachLopTrongKiThiOTrangThaiBatDau" %>
<div style="text-align: center; font-weight: bold; color: red; padding-bottom: 10px;">DANH SÁCH CÁC LỚP Ở TRONG KÌ THI GỐC</div>
<div id="divMain" runat="server">
    <table style="border: 1px;" border="1">
        <tr>
            <td style="color: black; text-align: center; vertical-align: top; font-weight: bold;">DANH SÁCH CÁC LỚP
            </td>
            <td style="color: black; text-align: center; vertical-align: top; font-weight: bold;">DANH SÁCH SINH VIÊN
            </td>
        </tr>
        <tr>
            <td style="width: 30%; vertical-align: top;" id="tdMenu" runat="server"></td>
            <td style="width: 70%; vertical-align: top;" id="tdContent" runat="server">
                <div id="divContent" runat="server"></div>
                <div id="divAction" runat="server" style="text-align: right;">
                    <div id="divExcel" runat="server"></div>
                    <asp:Button runat="server" ID="btnChuyenKiThiSangThi" Text="Chuyển lớp sang thi" OnClick="btnChuyenKiThiSangThi_Click" />
                    <asp:Button runat="server" ID="btnChuyenSangThi" Text="Chuyển SV sang chuẩn bị thi" OnClientClick=" return collectData();" OnClick="btnChuyenSangThi_Click" />
                    <asp:Button runat="server" ID="btnCamThi" Text="Chuyển SV sang cấm thi" OnClientClick=" return collectData();" OnClick="btnCamThi_Click" />
                    <asp:Button runat="server" ID="btnChuyenVeBatDau" Text="Chuyển SV về trạng thái gốc" OnClientClick=" return collectData();" OnClick="btnChuyenVeBatDau_Click" />
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtKiThiLopHocID" runat="server"></asp:TextBox>
    <asp:TextBox runat="server" ID="txtData"></asp:TextBox>
</div>

<script>
    function collectData() {
        //Lay du lieu cua cac o tich 
        //Dua du lieu vao txtData
        //Sau do cap nhat
        var strData = ",";
        $("#<%=divContent.ClientID%>").find("input:checked").each(function (i, ob) {
            // roles.push($(ob).val());
            if (ob.name == "namesinhvien")
                strData = strData + ob.id + ",";
            //alert(ob.id);

        });
        if (strData == ",") {
            alert("Bạn chưa chọn dữ liệu cập nhật");
            return false;
        }
        else {
            //alert(strData);
            $("#<%=txtData.ClientID%>").val(strData);
        }
        //alert(strData);
        return true;
    };
    function selectAll(o) {
        if (o.checked) {
            $("#<%=divContent.ClientID%>").find("input[type=checkbox]").each(function (i, ob) {
                // roles.push($(ob).val());
                if (ob.name == "namesinhvien")
                    ob.checked = true;
                //alert(ob.id);

            });
        } else {
            //Checkbox has been unchecked
            $("#<%=divContent.ClientID%>").find("input[type=checkbox]").each(function (i, ob) {
                // roles.push($(ob).val());
                if (ob.name == "namesinhvien")
                    ob.checked = false;
                //alert(ob.id);

            });
        }
    };
</script>
