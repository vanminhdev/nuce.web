<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="New_LichSuDangKi.ascx.cs" Inherits="nuce.web.qlpm.New_DanhSachDangKi" %>
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
    </table>
    
   
</div>
<div id="divContent" runat="server"></div>
