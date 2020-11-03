<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DanhSachLopTrongKiThiOTrangThaiDangThi.ascx.cs" Inherits="nuce.web.thi.DanhSachLopTrongKiThiOTrangThaiDangThi" %>
<div style="text-align:center; font-weight:bold;color:red; padding-bottom:10px;">DANH SÁCH CÁC LỚP Ở TRONG KÌ THI ĐANG Ở TRẠNG THÁI ĐANG THI</div>
<div id="divMain" runat="server">
    <table style="border: 1px; width: 100%;" border="1">
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
                    <asp:Button runat="server" ID="btnChuyenKiThiSangKetThucThi" Text="Chuyển lớp sang kết thúc thi" OnClick="btnChuyenKiThiSangKetThucThi_Click"/>
                    <asp:Button runat="server" ID="btnChuyenTrangThaiThiLai" Text="Chuyển sinh viên sang thi lại" OnClientClick=" return collectData();" OnClick="btnChuyenTrangThaiThiLai_Click" />
                    <asp:Button runat="server" ID="btnChamDiem" Text="Chấm điểm sinh viên" OnClientClick=" return collectData();" OnClick="btnChamDiem_Click" />
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
