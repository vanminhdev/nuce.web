<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SynthesizedResults.ascx.cs" Inherits="nuce.web.ks.analytics.SynthesizedResults" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: left; padding: 5px;" id="divSummary" runat="server">
</div>
<div id="divContent" style="padding: 5px;text-align: left; " runat="server">
</div>

<div style="padding: 5px;text-align: right; ">
    <asp:Button ID="btnSynthesized" runat="server" OnClick="btnSynthesized_Click" Text="Tổng hợp kết quả" />
</div>
<table border="1" style="padding:5px;"><tr><td colspan="4" rowspan="2" style="text-align:center;">Bảng thống kê tổng hợp theo Group</td></tr>
</table>