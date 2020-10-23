<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ThiOnline.ascx.cs" Inherits="nuce.web.sinhvien.ThiOnline" %>
<div id="divInitData" runat="server"></div>
<div class="menu-frame" runat="server" id="divMenu">
    <div style="color: rgb(255, 0, 0); text-align: center; font-size: 20px; width: 100%;">
        <span id="spMinutes"></span>
        <span style="font-weight: bold;">Phút : </span>
        <span id="spSeconds"></span>
        <span style="font-weight: bold;">Giây </span>
    </div>
    <div style="width: 100%; text-align: center; padding-bottom: 10px; padding-top: 10px;" id="divMenuCauHoi" runat="server">
    </div>
    <div style="width: 100%; text-align: center; padding-bottom: 10px; padding-top: 10px;" id="divAction">
        <asp:Button runat="server" ID="btnNopBai" Text="Nộp bài" OnClientClick=" return collectAnswares();" OnClick="btnNopBai_Click" />
    </div>
</div>
<div class="content-frame">
    <div style="width: 100%;" runat="server" id="divContent">
    </div>
</div>

<div style="display: none;">
    <asp:TextBox runat="server" ID="txtAnswares"></asp:TextBox>
</div>
<div id="divProcessData" runat="server"></div>
<script>
    var m_strAnwares = '';
    //var totalTime = 90 * 60 + 10;
    var seconds = totalTime % 60;
    //seconds = Math.round(seconds);
    var minutes = (totalTime - seconds) / 60;
    //minutes = Math.round(minutes);
    $('#spMinutes').html(minutes);
    $('#spSeconds').html(seconds);
    IDTime = window.setTimeout("updateTime();", 1000);

    function updateTime() {
        totalTime = totalTime - 1;
        if (totalTime > -1) {
            seconds = totalTime % 60;
            //seconds = Math.round(seconds);
            minutes = (totalTime - seconds) / 60;
            //minutes = Math.round(minutes);
            $('#spMinutes').html(minutes);
            $('#spSeconds').html(seconds);
            IDTime = window.setTimeout("updateTime();", 1000);

            if((totalTime+10)% 300==0)
            {
                window.setTimeout("doPostBackAnswares2();", 5000);
            }
        }
        else {
            //checkOnbeforeunload = 1;
            doPostBackAnswares();
        }
    };
    function doPostBackAnswares() {
        if (checkOnbeforeunload == 0) {
            checkOnbeforeunload = 1;
            collectAnswares();
            $.post("/Handler/nuce.web.sinhvien/NopBai.aspx",
                { kithilophocsinhvien: iIDKiThiLopHocSinhVien, bailam: m_strAnwares, action: "chamdiem" },
                function (data) {
                    $("#<%=divContent.ClientID%>").html(data);
                    $("#<%=divMenuCauHoi.ClientID%>").html("");
                     $("#divAction").html("");
                });
            }
    };
    function doPostBackAnswares1() {
        if (checkOnbeforeunload == 0) {
            collectAnswares();
            $.post("/Handler/nuce.web.sinhvien/NopBai.aspx", { kithilophocsinhvien: iIDKiThiLopHocSinhVien, bailam: m_strAnwares, action: "nopbai" });
        }
    };
    function doPostBackAnswares2() {
        if (checkOnbeforeunload == 0) {
            collectAnswares();
            $.post("/Handler/nuce.web.sinhvien/NopBai.aspx", { kithilophocsinhvien: iIDKiThiLopHocSinhVien, bailam: m_strAnwares, action: "dangthi" });
        }
    };
        window.onbeforeunload = function (e) {
            doPostBackAnswares1();
            var dialogText = 'Ban muon thoat khoi trinh duyet ?';
            e.returnValue = dialogText;
            return dialogText;
        };
        function gotocauhoi(thu_i) {
            $(document).scrollTop($("#divCauHoi_" + thu_i).offset().top - 50);
        };

        // the selector will match all input controls of type :checkbox
        // and attach a click event handler 
        $("input:checkbox").on('click', function () {
            // in the handler, 'this' refers to the box clicked on
            var $box = $(this);
            //nameCauHoi_{0}
            var i = $box.attr("value").replace('vcauhoi_', '');
            if ($box.is(":checked")) {
                // the name of the box is retrieved using the .attr() method
                // as it is assumed and expected to be immutable
                var group = "input:checkbox[name='" + $box.attr("name") + "']";
                // the checked state of the group/box on the other hand will change
                // and the current value is retrieved using .prop() method
                $(group).prop("checked", false);
                $box.prop("checked", true);
                $('#tdMenuCauHoi_' + i).css({ "color": "blue" });
                // Chuyen style cua bang sang mau xanh blue
                //tdMenuCauHoi_

            } else {
                $box.prop("checked", false);
                $('#tdMenuCauHoi_' + i).css({ "color": "red" });

                $("input:checkbox[value=vcauhoi_"+i+"]:checked").each(function () {
                    $('#tdMenuCauHoi_' + i).css({ "color": "blue" });
                });
                // Chuyen style cua bang sang mau xanh red
            }
        });

        function collectAnswares() {
            m_strAnwares = '';
            $("#<%=divContent.ClientID%>").find("input:checked").each(function (i, ob) {
                // roles.push($(ob).val());
                var name = $(ob).val();
                if (name.indexOf("vcauhoi_") >= 0) {
                    var id = ob.id.replace('id_', '');
                    //id_{0}_{1}
                    m_strAnwares = m_strAnwares + id + ";";
                }
            });
            $("#<%=txtAnswares.ClientID%>").val(m_strAnwares);
            //alert(m_strAnwares);
            return true;
        };
        window.setTimeout("InitData();", 1000);
</script>
