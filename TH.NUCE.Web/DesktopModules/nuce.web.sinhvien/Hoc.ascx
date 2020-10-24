<%@ control language="C#" autoeventwireup="true" codebehind="Hoc.ascx.cs" inherits="nuce.web.sinhvien.Hoc" %>
<div class="content-fn">
    <div id="divUpload" runat="server" style="border:1px;padding:5px; margin:5px;">
        <table style="width: 100%;">
            <tr>
                <td style="width:10%">
                    File:
                </td>
                <td>
                    <asp:FileUpload ID="FileUploadControl" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Mô tả</td>
                <td>
                    <asp:TextBox ID="txtMoTa" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Đợt trao đổi
                </td>
                <td>
                    <asp:DropDownList ID="ddlDotTraoDoi" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="UploadButton" Text="Cập nhật" OnClick="UploadButton_Click" />

                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="StatusLabel" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divContent" runat="server">
    </div>
</div>

    <script>
        function nopbai(id)
        {
            $.post("/Handler/nuce.web.sinhvien/HocUpdateStatus.aspx",
               { id: id, status: 2, action: "capnhat" },
               function (data) {
                   alert("Nộp bài thành công");
                   location.reload();
               });
        };
    </script>