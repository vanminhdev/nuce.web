<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="nuce.web.ks.analytics.Search" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center; padding: 5px;" id="divFilter">
    <span>Chọn từ khoá: </span>
    <span>
        <asp:TextBox runat="server" ID="txtKeyword"></asp:TextBox></span>
    <span>
        <asp:Button runat="server" ID="btnTimKiem" Text="Search" OnClick="btnTimKiem_Click" /></span>
    <span>Chọn câu hỏi: </span>
    <span>
        <asp:DropDownList ID="ddlCauHoi" runat="server" AutoPostBack="false">
            <asp:ListItem Selected="True" Value="-1">Tất cả</asp:ListItem>
            <asp:ListItem  Value="1">Câu hỏi 1</asp:ListItem>
            <asp:ListItem  Value="2">Câu hỏi 2</asp:ListItem>
            <asp:ListItem  Value="3">Câu hỏi 3</asp:ListItem>
        </asp:DropDownList> </span>
</div>
<div style="text-align: left; padding: 2px;">Câu hỏi 1: Hãy nêu một điều bạn thích nhất về giảng viên của môn học này</div>
<div style="text-align: left; padding: 2px;">Câu hỏi 2: Hãy nêu một điều bạn không thích nhất về giảng viên môn học này</div>
<div style="text-align: left; padding: 2px;">Câu hỏi 3: Ý kiến đóng góp để sinh viên tiếp thu môn học này tốt hơn.</div>
<div id="divContent" style="padding: 5px;">
    <div id="divSummary" runat="server" style="color: red;"></div>
    <div>
        <asp:GridView ID="grdData" runat="server"
            AutoGenerateColumns="False" CellPadding="4" PageSize="100"
            ForeColor="#333333" GridLines="None" Width="100%" AllowPaging="True"
            OnPageIndexChanging="grdData_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
            <Columns>
                <%--                <asp:BoundField DataField="Text" HeaderText="Bỏ dấu"></asp:BoundField>--%>
                <asp:BoundField DataField="TextOld" HeaderText="Nguyên bản"></asp:BoundField>
                <asp:BoundField DataField="SubjectName" HeaderText="Môn học" ItemStyle-Width="16%"></asp:BoundField>
                <asp:BoundField DataField="Type" HeaderText="Câu hỏi" ItemStyle-Width="5%"></asp:BoundField>
                <asp:BoundField DataField="Survey" HeaderText="Đợt khảo sát"></asp:BoundField>
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
