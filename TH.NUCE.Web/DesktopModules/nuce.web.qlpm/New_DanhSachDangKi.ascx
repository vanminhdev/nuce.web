<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="New_DanhSachDangKi.ascx.cs" Inherits="nuce.web.qlpm.New_LichSuDangKi" %>

<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center; font-weight: bold;" id="divFilter" runat="server">
    <table style="margin: 5px; padding: 5px;">
        <td>
            <asp:Label ID="lblNgayBatDau" runat="server" Text="Ngày bắt đầu: "></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtNgayBatDau" runat="server" Width="100px"></asp:TextBox>
            <asp:HyperLink ID="lnkNgayBatDau" runat="server">
                <asp:Image ID="imgNgayBatDau" runat="server" resourceKey="CalendarAlt.Text" />
            </asp:HyperLink>
            (dd/mm/yyyy)
        </td>
        <td>
            <asp:Label ID="lblNgayKetThuc" runat="server" Text="Ngày kết thúc: "></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtNgayKetThuc" runat="server" Width="100px"></asp:TextBox>
            <asp:HyperLink ID="lnkNgayKetThuc" runat="server">
                <asp:Image ID="imgNgayKetThuc" runat="server" resourceKey="CalendarAlt.Text" />
            </asp:HyperLink>
            (dd/mm/yyyy)
        </td>
        <td>
            <asp:Label ID="lblChonPhong" runat="server" Text=" Chọn phòng : "></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlPhongHoc" runat="server" AutoPostBack="False"></asp:DropDownList>
        </td>
        <td>
            <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click" />
        </td>
        <td>
              <asp:Button ID="btnDuyetAll" runat="server" Text="Duyệt tất cả" OnClick="btnDuyetAll_Click"/>
        </td>
    </table>
</div>
<div id="divNoiDungHanhDong" runat="server" style="display:none;">
    <table>
         <tr>
             <td colspan="2" id="tdThongBao"></td>
         </tr>
        <tr>
            <td>Nội dung: </td>
            <td>
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtNoiDung"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Button ID="txtCapNhat" runat="server" Text="Cập nhật" OnClick="txtCapNhat_Click" />
                <asp:Button ID="txtQuayLai" runat="server" Text="Quay lại" OnClientClick="quaylai();" />
                </td>
            <td></td>
        </tr>
    </table>

</div>

<div id="divContent" runat="server"></div>
<div style="display:none;">
    <asp:TextBox ID="txtTypeAction" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtIDAction" runat="server"></asp:TextBox>
</div>
<div id="divJavascript" runat="server"></div>
<script>
    $('#' + divNoiDungHanhDong).hide();
    function quaylai()
    {
        $('#' + divFilter).show();
        $('#' + divContent).show();
        $('#' + divNoiDungHanhDong).hide();
    }
    function duyet(id) {
        $('#tdThongBao').html("Duyệt");
        $('#' + txtTypeAction).val('1');
        $('#' + txtIDAction).val(id);
        $('#' + txtNoiDung).val('');
        $('#' + divFilter).hide();
        $('#' + divContent).hide();
        $('#' + divNoiDungHanhDong).show();

    }
    function huy(id) {
        $('#tdThongBao').html("Huỷ");
        $('#' + txtTypeAction).val('2');
        $('#' + txtIDAction).val(id);
        $('#' + txtNoiDung).val('');
        $('#' + divFilter).hide();
        $('#' + divContent).hide();
        $('#' + divNoiDungHanhDong).show();
    }
</script>
