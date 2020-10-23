<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchByMH.ascx.cs" Inherits="nuce.web.ks.analytics.SearchByMH" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center; padding: 5px;" id="divFilter">
    <span>Chọn mã môn: </span>
    <span>
        <asp:TextBox runat="server" ID="txtKeyword"></asp:TextBox></span>
    <span>
        <asp:Button runat="server" ID="btnTimKiem" Text="Search" OnClick="btnTimKiem_Click" /></span>
</div>
<div id="divContent" style="padding: 5px;">
    <div id="divSummary" runat="server" style="color: blue;"></div>
    <div>
        <asp:GridView ID="grdData" runat="server"
            AutoGenerateColumns="False" CellPadding="4" PageSize="100"
            ForeColor="#333333" GridLines="None" Width="100%" AllowPaging="True" AllowSorting="True"   
            OnPageIndexChanging="grdData_PageIndexChanging" onsorting="grdData_Sorting">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
            <Columns>
                <asp:BoundField DataField="F_MANH" HeaderText="Lớp môn học" ItemStyle-Width="10%" SortExpression="F_MANH"></asp:BoundField>
                <asp:BoundField DataField="S_NAMHOC" HeaderText="Năm học" ItemStyle-Width="10%"  SortExpression="S_NAMHOC"></asp:BoundField>
                <asp:BoundField DataField="SO_SV" HeaderText="Số SV" ItemStyle-Width="8%" SortExpression="SO_SV"></asp:BoundField>
                <asp:BoundField DataField="d2" DataFormatString="{0:0.00}" HeaderText="%(>2)" ItemStyle-Width="8%" SortExpression="d2"></asp:BoundField>
                <asp:BoundField DataField="d3" DataFormatString="{0:0.00}" HeaderText="%(>3)" ItemStyle-Width="8%" SortExpression="d3"></asp:BoundField>
                <asp:BoundField DataField="d4" DataFormatString="{0:0.00}" HeaderText="%(>4)" ItemStyle-Width="8%" SortExpression="d4"></asp:BoundField>
                <asp:BoundField DataField="d5" DataFormatString="{0:0.00}" HeaderText="%(>5)" ItemStyle-Width="8%" SortExpression="d5"></asp:BoundField>
                <asp:BoundField DataField="d6" DataFormatString="{0:0.00}" HeaderText="%(>6)" ItemStyle-Width="8%" SortExpression="d6"></asp:BoundField>
                <asp:BoundField DataField="DQT_TBC" DataFormatString="{0:0.00}" HeaderText="DQT_TB" ItemStyle-Width="8%" SortExpression="DQT_TBC"></asp:BoundField>
                <asp:BoundField DataField="DQT_SD" DataFormatString="{0:0.00}" HeaderText="DQT_SD" ItemStyle-Width="8%" SortExpression="DQT_SD"></asp:BoundField>
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
</div>
