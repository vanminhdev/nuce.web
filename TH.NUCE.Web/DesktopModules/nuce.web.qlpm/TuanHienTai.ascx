<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TuanHienTai.ascx.cs" Inherits="nuce.web.qlpm.TuanHienTai" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center; font-weight: bold;" id="divFilter" runat="server">
    Chọn phòng:  
    <asp:DropDownList ID="ddlPhongHoc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPhongHoc_SelectedIndexChanged1"></asp:DropDownList>
</div>

<div id="divContent" runat="server"></div>
<div id="divAction" runat="server" style="text-align: right;">
    <asp:Button ID="btnChotTuanHienTai" runat="server" Text="Chốt dữ liệu tuần hiện tại" OnClick="btnChotTuanHienTai_Click" />
</div>

<div>
    <table style="padding: 5px; margin: 5px;">
        <tr>
            <td style="background-color:#d3d3d3; width: 50px;">
            </td>
            <td>
                : Chưa gửi yêu cầu
            </td>
        </tr>
        <tr>
            <td style="background-color: blue; width: 50px;">
            </td>
            <td>
                : Đã gửi yêu cầu điểm danh tự động
            </td>
        </tr>
          <tr>
            <td style="background-color: red; width: 50px;">
            </td>
            <td>
                : Đang điểm danh
            </td>
        </tr>
          <tr>
            <td style="background-color: green; width: 50px;">
            </td>
            <td>
                : Đã điểm danh tự động xong
            </td>
        </tr>
         <tr>
        <td style="background-color: yellow; width: 50px;">
            </td>
            <td>
                : Chờ gửi tự động lại
            </td>
        </tr>
    </table>
</div>
<script>
    function goiTrangSuDangKiPhongHoc(url) {
        // dnnModal.show(url, true, 550, 950, true);
        window.location.assign(url);
    };
</script>