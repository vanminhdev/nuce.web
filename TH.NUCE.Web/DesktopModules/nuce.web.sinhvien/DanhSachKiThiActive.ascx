<%@ control language="C#" autoeventwireup="true" codebehind="DanhSachKiThiActive.ascx.cs" inherits="nuce.web.sinhvien.DanhSachKiThiActive" %>
<div class="content-fn" runat="server" id="divContent">
    <%-- <table style="width: 450px; margin: 60 auto;">
        <tr>
            <td colspan="2" runat="server" id="tdAnnounce" style="color:red; text-align:center;">

            </td>
        </tr>
        <tr style="padding-bottom:10px;">
            <td style="width:150px; text-align:left">Tên đăng nhập</td>
            <td style="width:300px;">
                <asp:TextBox runat="server" ID="txtTen" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width:150px; text-align:left">Mật khẩu</td>
            <td>
                <asp:TextBox runat="server" ID="txtMauKhau" TextMode="Password" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:right; padding-right:10px;">
                <asp:Button runat="server" Text="Đăng nhập"  ID="btnDangNhap" OnClick="btnDangNhap_Click"/>
            </td>
        </tr>
    </table>--%>
</div>
<%--<script src="http://code.jquery.com/jquery-1.10.2.js"></script>
<script>
    $(document).ready(function () {
        $(window).bind("beforeunload", function () {
            alert('test');
            return confirm("You are about to close the window");
        });
    });
</script>--%>
<div style="display: none;">
    <div id="divMac"></div>
</div>


    <script>
        var m_mac='';
        function showMacAddress() {
            var obj = new ActiveXObject("WbemScripting.SWbemLocator");
            var s = obj.ConnectServer(".");
            var properties = s.ExecQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
            var e = new Enumerator(properties);
            var output = '';
            while (!e.atEnd()) {
                e.moveNext();
                var p = e.item();
                if (!p) continue;
                //var Caption = p.Caption.indexOf("John");
                var Caption = p.Caption;
                var Pos = p.Caption.indexOf("Realtek");
                var Mac = p.MACAddress;
                try {
                    //if (Mac.length > 0) {
                    if (Pos > 0) {
                        output = p.MACAddress;
                    }
                }
                catch (ex) {

                }
            }
            document.getElementById('divMac').innerHTML = output;
            m_mac=output;
        };
        showMacAddress();
        //xac thuc
        function xacthuc(calophocsinhvien)
        {
            $.post("/Handler/nuce.web.sinhvien/XacThucMac.aspx",
                { calophocsinhvien: calophocsinhvien, mac: m_mac, action: "xacthuc" },
                function (data) {
                    switch (data) {
                        case "1":
                            alert("Xác thực thành công");
                            break;
                        case "-3":
                            alert("MacAddress để trắng");
                            break;
                        case "-2":
                            alert("MacAddress không tồn tại");
                            break;
                        case "-1":
                            alert("Đã tồn tại máy");
                            break;
                        default:
                            break;
                    };
                    location.reload();
                });
        };
        function huyxacthuc(calophocsinhvien)
        {
            $.post("/Handler/nuce.web.sinhvien/XacThucMac.aspx",
               { calophocsinhvien: calophocsinhvien, mac: m_mac, action: "huyxacthuc" },
               function (data) {
                   alert("Huỷ xác thực thành công");
                   location.reload();
               });
        };
    </script>
