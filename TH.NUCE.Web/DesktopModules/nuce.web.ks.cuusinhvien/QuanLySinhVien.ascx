<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLySinhVien.ascx.cs" Inherits="nuce.web.ks.cuusinhvien.QuanLySinhVien" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divImportDuLieu">
    <div style="text-align: center;" id="divInportExcel_Upload" runat="server">
        <div style="text-align: center;">
            Chọn file : 
            <asp:FileUpload ID="FU1" runat="server" />
            <asp:Button ID="btnUpload" runat="server" Text="Tải file" OnClick="btnUpload_Click" />
            <asp:Button ID="btnCapNhatLan1" runat="server" Text="Update" OnClick="btnCapNhatLan1_Click" />
            <asp:Button ID="btnCapNhatLai_lan_1" runat="server" Text="Update_lai" OnClick="btnCapNhatLai_lan_1_Click" />
        </div>
        <div style="text-align: center;">Nội dung file excel</div>
        <div style="text-align: center;" runat="server" id="divContentExcel"></div>
    </div>
</div>
<div style="text-align: center;" id="divUpdateImportDuLieu">
    <div style="text-align: center;" id="divUpdateInportExcel_Upload" runat="server">
        <div style="text-align: center;">
            Chọn file : 
            <asp:FileUpload ID="FU2" runat="server" />
            <asp:Button ID="btnUpload2" runat="server" Text="Tải file" OnClick="btnUpload2_Click" />
            <asp:Button ID="btnCapNhatLan2" runat="server" Text="Update" OnClick="btnCapNhatLan2_Click"/>
        </div>
        <div style="text-align: center;">Nội dung file excel</div>
        <div style="text-align: center;" runat="server" id="divContentExcel2"></div>
    </div>
</div>
<div>
      <asp:Button ID="btnExportExcelByClass" runat="server" Text="ExportExcelByClass" OnClick="btnExportExcelByClass_Click" />
</div>
<div style="text-align: center;" id="divFilter">
    <span>Chọn đợt khảo sát: </span>
    <span>
        <asp:DropDownList ID="ddlDotKhaoSat" runat="server">
            <asp:ListItem Value="-1">All</asp:ListItem>
            <asp:ListItem Value="1">2016-2017</asp:ListItem>
        </asp:DropDownList></span>

    <span>Chọn masv: </span>
    <span>
        <asp:TextBox runat="server" ID="txtMaSv"></asp:TextBox></span>
    <span>
        <asp:Button runat="server" ID="btnTimKiem" Text="Search" OnClick="btnTimKiem_Click" /></span>
</div>

<div id="divContent">
    <asp:GridView ID="grdData" runat="server"
        AutoGenerateColumns="False" CellPadding="4" PageSize="50"
        ForeColor="#333333" GridLines="None" Width="100%" AllowPaging="True"
        OnPageIndexChanging="grdData_PageIndexChanging">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
        <Columns>
            <asp:BoundField DataField="masv" HeaderText="masv"></asp:BoundField>
            <asp:BoundField DataField="tensinhvien" HeaderText="tensinhvien"></asp:BoundField>
            <asp:BoundField DataField="manganh" HeaderText="manganh"></asp:BoundField>
            <asp:BoundField DataField="tenchnga" HeaderText="tenchnga"></asp:BoundField>
            <asp:BoundField DataField="tennganh" HeaderText="tennganh"></asp:BoundField>
            <asp:BoundField DataField="hedaotao" HeaderText="hedaotao"></asp:BoundField>
            <asp:BoundField DataField="khoahoc" HeaderText="khoahoc"></asp:BoundField>
            <asp:BoundField DataField="mobile" HeaderText="mobile"></asp:BoundField>
            <asp:BoundField DataField="email" HeaderText="email"></asp:BoundField>
        </Columns>
        <EditRowStyle BackColor="#999999"></EditRowStyle>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True"
            ForeColor="White"></FooterStyle>
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True"
            ForeColor="White"></HeaderStyle>
        <PagerStyle BackColor="#284775" ForeColor="White"
            HorizontalAlign="Center"></PagerStyle>
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True"
            ForeColor="#333333"></SelectedRowStyle>
        <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>
        <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>
        <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>
        <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
    </asp:GridView>
</div>
