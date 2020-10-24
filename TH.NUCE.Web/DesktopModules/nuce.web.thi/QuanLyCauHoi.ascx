<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyCauHoi.ascx.cs" Inherits="nuce.web.thi.QuanLyCauHoi" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="../../controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URLControl" Src="../../controls/URLControl.ascx" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter" runat="server">
    <span>Nhập mã bộ câu hỏi: </span>
    <span>
        <asp:TextBox ID="txtMaBoCauHoi" runat="server" Width="150px"></asp:TextBox></span>
    <span>Từ khóa: </span>
    <span>
        <asp:TextBox ID="txtTuKhoa" Width="250px" runat="server"></asp:TextBox></span>
    <span>
        <asp:Button ID="btnTimBoCauHoi" runat="server" Text="Tìm" OnClick="btnTimBoCauHoi_Click" />
    </span>
</div>
<div style="text-align: center; margin-top:10px;" id="divInportExcel" runat="server" visible="false">
    <div style="text-align: center;" id="divInportExcel_Action" runat="server">
        <asp:LinkButton ID="btnShowFormImport" runat="server" Text="ImportExcel" OnClick="btnShowFormImport_Click" ></asp:LinkButton></div>
    <div style="text-align: center;" id="divInportExcel_Upload" runat="server">
        <div style="text-align: center;">
            Chọn file : 
            <asp:FileUpload ID="FU1" runat="server" />
            <asp:Button ID="btnUpload" runat="server" Text="Tải file" OnClick="btnUpload_Click" />
        </div>
        <div style="text-align: center;">Nội dung file excel</div>
        <div style="text-align: center;" runat="server" id="divContentExcel"></div>
        <div style="text-align: right;">
            <asp:Button ID="btnImport" runat="server" Text="Cập nhật dữ liệu" OnClick="btnImport_Click" />
            <asp:Button ID="btnQuayTroLaiMain" runat="server" Text="Quay trở lại danh sách" OnClick="btnQuayTroLaiMain_Click" />
        </div>

    </div>
</div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td width="15%">
                <asp:Label ID="lblMa" runat="server" Text="Mã : "></asp:Label>
            </td>
            <td width="85%" colspan="2">
                <asp:TextBox ID="txtMa" runat="server" Width="20%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td width="15%">
                <asp:Label ID="lblNoiDung" runat="server" Text="Nội dung : "></asp:Label>
            </td>
            <td width="85%" colspan="2">
                <dnn:TextEditor ID="txtNoiDung" runat="server" Mode="Rich" Width="100%" Height="250"></dnn:TextEditor>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblImage" runat="server" Text="Ảnh : "></asp:Label>
            </td>
            <td width="30%">
                <input id="txtImage" type="file" name="txtImage" runat="server" visible="False" />
                <dnn:URLControl ID="urlImage" ShowFiles="true" ShowLog="False" ShowTrack="False"
                    FileFilter="png,jpg,jpeg" ShowUrls="False" ShowTabs="False" Required="false" runat="server"
                    ShowNone="false" ShowSecure="false" ShowUpLoad="True" ShowUsers="false" Width="300" />
               <%-- <input id="File1" type="file" name="urlImage" runat="server" visible="False" />--%>
            </td>
            <td width="55%">
                <img runat="server" id="imgImage" width="200" />
                ( Ảnh Cũ: <span id="spImage" runat="server"></span>)
               <%-- <img  id="imgImage" runat="server" width="200px;"/> ( Ảnh Cũ: <span id="spImage" runat="server"></span> )--%>
            </td>
        </tr>
        <tr>
            <td width="15%">
                <asp:Label ID="lblGiaiTich" runat="server" Text="Giải thích: "></asp:Label>
            </td>
            <td width="85%" colspan="2">
                <dnn:TextEditor ID="txtGiaiThich" runat="server" Mode="Rich" Width="100%" Height="250"></dnn:TextEditor>
            </td>
        </tr>
        <tr style="display: none;">
            <td>
                <asp:Label ID="lblBoCauHoiID" runat="server" Text="Bo Cau Hoi ID : "></asp:Label>
            </td>
            <td width="85%" colspan="2">
                <asp:TextBox ID="txtBoCauHoiID" runat="server" Width="20%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDoKho" runat="server" Text="Độ khó : "></asp:Label>
            </td>
            <td width="85%" colspan="2">
                <asp:DropDownList ID="ddlDoKho" runat="server" Width="6%">
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="display: none;">
            <td>
                <asp:Label ID="lblCount" runat="server" Text="Count : "></asp:Label>
            </td>
            <td width="85%" colspan="2">
                <asp:TextBox ID="txtCount" runat="server" Width="20%" Text="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblThuTu" runat="server" Text="Thứ tự: "></asp:Label>
            </td>
            <td width="85%" colspan="2">
                <asp:TextBox ID="txtOrder" runat="server" Text="1" Width="6%"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td>
                <asp:Label ID="lblLevel" runat="server" Text="Level: "></asp:Label>
            </td>
            <td width="85%" colspan="2">
                <asp:TextBox ID="txtLevel" runat="server" Width="5%"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td>
                <asp:Label ID="lblType" runat="server" Text="Level: "></asp:Label>
            </td>
            <td width="85%" colspan="2">
                <asp:TextBox ID="txtTypeCauHoi" runat="server" Width="20%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div runat="server" id="divDapAn">
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="right">
                <asp:Button ID="btnUpdate" runat="server" OnClientClick="return quanlycauhoi.fillData();" Text="Cập nhật" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnQuayLai" runat="server" Text="Quay lại" OnClick="btnQuayLai_Click" />

            </td>
        </tr>
    </table>
</div>

<div style="display: none;">
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtParent" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtNguoiDungMonHoc" runat="server"></asp:TextBox>

    <asp:TextBox ID="txtNoiDungDapAn" runat="server"></asp:TextBox>

    <asp:TextBox ID="txtImageOld" runat="server" Visible="false"></asp:TextBox>
</div>


<script type="text/javascript">


    $(document).ready(function () {
        $("#" + "<%=txtOrder.ClientID%>").blur(function () {
            Format_onblur(this, 0);
        });
    });


</script>
