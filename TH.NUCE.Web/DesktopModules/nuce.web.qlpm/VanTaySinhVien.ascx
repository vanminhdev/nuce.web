<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VanTaySinhVien.ascx.cs" Inherits="nuce.web.qlpm.VanTaySinhVien" %>
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
            <asp:Label ID="lblTrangThai" runat="server" Text="Trạng thái lấy mẫu: "></asp:Label>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlStatus" AutoPostBack="false">
                <asp:ListItem Value="-1" Selected="True">Tất cả</asp:ListItem>
                <asp:ListItem Value="1">Bình thường</asp:ListItem>
                <asp:ListItem Value="2">Lấy mẫu lại</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="lblMaSV" runat="server" Text="Mã SV: "></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtMaSV" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click" />
        </td>
    </table>


</div>
<div id="divContent" runat="server"></div>
<div style="display: none;">
    <asp:TextBox ID="txtMASVUpdate" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
    <asp:Button ID="btnCapNhat" runat="server" OnClick="btnCapNhat_Click" />
</div>

<script>
    function chuyentrangthailaymau(masv, status) {
        if (confirm("Bạn muốn chuyển !")) {
            $("#<%=txtMASVUpdate.ClientID%>").val(masv);
            $("#<%=txtStatus.ClientID%>").val(status);
            __doPostBack('btnCapNhat', 'btnCapNhat');
        }
    };
    function xoadulieulaymau(masv, status) {
        if (confirm("Bạn muốn xóa !")) {
            $("#<%=txtMASVUpdate.ClientID%>").val(masv);
            $("#<%=txtStatus.ClientID%>").val(status);
            __doPostBack('btnCapNhat', 'btnXoa');
        }
        };
        function suamasv(masv) {
            var inputMaSv = prompt("Nhập mã sv mới");
            if (inputMaSv != null) {
                $("#<%=txtMASVUpdate.ClientID%>").val(masv);
            $("#<%=txtStatus.ClientID%>").val(inputMaSv);
            __doPostBack('btnCapNhat', 'btnCapNhatMaSV');
        }


    };
</script>
