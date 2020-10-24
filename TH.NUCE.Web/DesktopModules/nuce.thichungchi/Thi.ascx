<%@ control language="C#" autoeventwireup="true" codebehind="Thi.ascx.cs" inherits="nuce.ad.thichungchi.Thi" %>
<div id="divInitData" runat="server"></div>
<div class="row">
    <div class="col-sm-7" style="padding: 2px;">
        <div class="panel panel-default" style="height: 100px; margin: 0px;">
            <div class="panel-heading" style="height: 20px; padding: 1px; text-align: center; font-weight: bold;">Thông tin chung</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6">
                        <div class="row">
                            <div class="col-sm-5">Tên kì thi:</div>
                            <div class="col-sm-7" style="font-weight: bold;" id="divTenKiThi" runat="server"></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-5">Thời gian:</div>
                            <div class="col-sm-7" style="font-weight: bold;" id="divThoiGian" runat="server"></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-5">Phòng thi:</div>
                            <div class="col-sm-7" style="font-weight: bold;" id="divPhongThi" runat="server"></div>
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <div class="row">
                            <div class="col-sm-5">Mã thí sinh:</div>
                            <div class="col-sm-7" style="font-weight: bold;" id="divMaThiSinh" runat="server"></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-5">Tên thí sinh:</div>
                            <div class="col-sm-7" style="font-weight: bold;" id="divTenThiSinh" runat="server"></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-5">CMT:</div>
                            <div class="col-sm-7" style="font-weight: bold;" id="divCMT" runat="server"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-2" style="padding: 2px;">
        <div class="panel panel-default" style="height: 100px; margin: 0px;">
            <div class="panel-heading" style="height: 20px; padding: 1px; text-align: center; font-weight: bold;">Mã đề</div>
            <div class="panel-body" style="text-align: center;"><span id="spMaDe" runat="server"></span></div>
        </div>

    </div>
    <div class="col-sm-3" style="padding: 2px;">
        <div class="panel panel-default" style="height: 100px; margin: 0px;">
            <div class="panel-heading" style="height: 20px; padding: 1px; text-align: center; font-weight: bold;">Thời gian làm bài</div>
            <div class="panel-body">
                <div style="color: rgb(255, 0, 0); text-align: center; font-size: 30px; width: 100%;">
                    <span id="spMinutes"></span>
                    <span style="font-weight: bold;">Phút : </span>
                    <span id="spSeconds"></span>
                    <span style="font-weight: bold;">Giây </span>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-9" style="padding: 2px;">
        <div class="row" style="margin: 0px;">
            <div class="panel panel-default" style="min-height: 365px; margin: 0px;">
                <div class="panel-body" id="divNoiDungCauHoi"></div>
            </div>
        </div>
        <div class="row" style="margin: 0px;">
            <div class="col-sm-3" style="padding: 2px;">
                <div class="panel panel-default" style="height: 80px;">
                    <div class="panel-heading" style="height: 20px; padding: 1px; text-align: center; font-weight: bold;">Phần chức năng</div>
                    <div class="panel-body">
                        <button type="button" class="btn btn-primary" onclick="Thi.NopBai();">Nộp bài</button>
                        <button type="button" class="btn btn-default" onclick="Thi.RoiPhong();">Rời phòng</button>
                    </div>
                </div>
            </div>
            <div class="col-sm-8" style="padding: 2px;">
                <div class="panel panel-default" style="height: 80px;">
                    <div class="panel-heading" style="height: 20px; padding: 1px; text-align: center; font-weight: bold;">Thông tin chi tiết</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-4">
                                <div class="row">
                                    <div class="col-sm-9">Tổng số câu:</div>
                                    <div class="col-sm-3" id="divTongSoCau" runat="server" style="font-weight:bold;"></div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-9">Số câu đã làm:</div>
                                    <div class="col-sm-3" id="divTongSoCauDaLam" style="font-weight:bold;"></div>
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="row">
                                    <div class="col-sm-9">Số câu chưa làm:</div>
                                    <div class="col-sm-3" id="divTongSoCauChuaDaLam" style="font-weight:bold;"></div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-9">Số câu làm đúng:</div>
                                    <div class="col-sm-3" id="divTongSoCauLamDung" runat="server" style="font-weight:bold;"></div>
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="row">
                                    <div class="col-sm-9">Số câu làm sai:</div>
                                    <div class="col-sm-3" id="divTongSoCauLamSai" runat="server" style="font-weight:bold;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-1" style="padding: 2px;">
                <div class="panel panel-default" style="height: 80px;">
                    <div class="panel-heading" style="height: 20px; padding: 1px; text-align: center; font-weight: bold;">Điểm</div>
                    <div class="panel-body">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-3" style="padding: 2px;">
        <div class="panel panel-default" style="height: 80px; margin: 0px;">
            <div class="panel-heading" style="height: 20px; padding: 1px; text-align: center; font-weight: bold;">Chọn câu trả lời</div>
            <div class="panel-body">
                <div id="divCauTraLoi" style="display: none;">
                    <table style="width: 60%; margin: 0 auto; border-color: cornflowerblue;" border="1px" id="tblCauTraLoi">
                        <tbody>
                            <tr id="trCauTraLoi" style="text-align: center; font-weight: bold;">
                                <td></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="panel panel-default" style="margin: 0px; min-height: 367px;">
            <div class="panel-body" id="divMenuCauHoi" runat="server">
            </div>
        </div>
    </div>
</div>
<div style="display: none;" id="divDanhSachDapAn" runat="server">
    <table>
        <tr id="trCauTraLoi1" style="text-align: center; font-weight: bold;">
            <td onclick="clickKetQua" style="height: 36px; background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;">A</td>
            <td style="height: 36px; background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;">B</td>
            <td style="height: 36px; background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;">C</td>
            <td style="height: 36px; background: rgb(230, 230, 230) none repeat scroll 0% 0%; text-align: center; font-weight: bold; padding: 3px; color: red; cursor: pointer;">D</td>
        </tr>
    </table>
</div>
<div style="display: none;" id="divDanhSachCauHoi" runat="server">
</div>
<div style="display: none;" id="divCheckBoxDapAn" runat="server">
</div>
<div style="display: none;">
    <asp:TextBox runat="server" ID="txtAnswares"></asp:TextBox>
</div>
<script>
    var Thi = {
        ID: -1,
        Status: 1,
        RoiPhong: function () {
            window.location.href = "/Thi/DanhSachKiThi";
        },
        NopBai: function () {
            doPostBackAnswares();
            location.reload(true);
        }
    };
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
            if ((totalTime + 10) % 300 == 0) {
                window.setTimeout("doPostBackAnswares2();", 5000);
            }
        }
        else {
            //checkOnbeforeunload = 1;
            doPostBackAnswares();
            location.reload(true);
        }
    };
    function gotocauhoi(id) {
        $("#divCauTraLoi").show();
        $("#trCauTraLoi").html($("#trCauTraLoi" + id).html());
        $('#divNoiDungCauHoi').html($("#divCauHoi_" + id).html());
        $("#" + strCtl + "divMenuCauHoi td").css({ "color": "red" });
        //Duyet tat cac cac o da check
        collectAnswares();
        $("#tdMenuCauHoi_" + id).css({ "color": "blue" });
    };
    function clickKetQua(id) {
        $("#id_" + id).prop("checked", true);
        var $box = $("#id_" + id);
        if ($box.is(":checked")) {
            // the name of the box is retrieved using the .attr() method
            // as it is assumed and expected to be immutable
            var group = "input:checkbox[name='" + $box.attr("name") + "']";
            // the checked state of the group/box on the other hand will change
            // and the current value is retrieved using .prop() method
            $(group).prop("checked", false);
            $box.prop("checked", true);

        } else {
            $box.prop("checked", false);
        };
        $("#trCauTraLoi td").css({ "color": "red" });
        var n = id.indexOf("_");
        var idCauHoi = id.substring(0, n);
        $("#trCauTraLoi" + idCauHoi).find("td").css({ "color": "red" });
        $("#trCauTraLoi" + idCauHoi + " td").css({ "color": "red" });
        $("#td_ket_qua_" + id).css({ "color": "blue" });
        doPostBackAnswares2();
    };
    function collectAnswares() {
        m_strAnwares = '';
        var dem_so_cau_tra_loi = 0;
        var strIdCauHois = "";
        $("#" + strCtl + "divCheckBoxDapAn").find("input:checked").each(function (i, ob) {
            // roles.push($(ob).val());
            var name = $(ob).val();
            if (name.indexOf("vcauhoi_") >= 0) {
                var id = ob.id.replace('id_', '');
                //id_{0}_{1}
                $("#td_ket_qua_" + id).css({ "color": "blue" });
                var n = id.indexOf("_");
                var idCauHoi = id.substring(0, n);
                if ((strIdCauHois+";").indexOf(";" + idCauHoi+";") < 0)
                {
                    strIdCauHois += ";" + idCauHoi;
                    dem_so_cau_tra_loi++;
                }
                $("#tdMenuCauHoi_" + idCauHoi).css({ "color": "blue" });
                m_strAnwares = m_strAnwares + id + ";";
            }
        });
        $("#" + strCtl + "txtAnswares").val(m_strAnwares);
        //alert(m_strAnwares);
        $("#divTongSoCauDaLam").html(dem_so_cau_tra_loi);
        var so_cau_trua_lam = $("#" + strCtl + "divTongSoCau").html() - dem_so_cau_tra_loi;
        $("#divTongSoCauChuaDaLam").html(so_cau_trua_lam);
        return true;
    };
    $("input:checkbox").on('click', function () {
        // in the handler, 'this' refers to the box clicked on
        var $box = $(this);
        //nameCauHoi_{0}
        if ($box.is(":checked")) {
            // the name of the box is retrieved using the .attr() method
            // as it is assumed and expected to be immutable
            var group = "input:checkbox[name='" + $box.attr("name") + "']";
            // the checked state of the group/box on the other hand will change
            // and the current value is retrieved using .prop() method
            $(group).prop("checked", false);
            $box.prop("checked", true);

        } else {
            $box.prop("checked", false);
        }
    });

    function doPostBackAnswares() {
        if (checkOnbeforeunload == 0) {
            checkOnbeforeunload = 1;
            collectAnswares();
            $.post("/Handler/nuce.thichungchi/NopBai.aspx",
                { kithilophocsinhvien: iIDKiThiLopHocSinhVien, bailam: m_strAnwares, action: "chamdiem" },
                function (data) {
                    location.reload(true);
                });
        }
    };
    function doPostBackAnswares1() {
        if (checkOnbeforeunload == 0) {
            collectAnswares();
            $.post("/Handler/nuce.thichungchi/NopBai.aspx", { kithilophocsinhvien: iIDKiThiLopHocSinhVien, bailam: m_strAnwares, action: "nopbai" });
        }
    };
    function doPostBackAnswares2() {
        if (checkOnbeforeunload == 0) {
            collectAnswares();
            $.post("/Handler/nuce.thichungchi/NopBai.aspx", { kithilophocsinhvien: iIDKiThiLopHocSinhVien, bailam: m_strAnwares, action: "dangthi" });
        }
    };
    window.onbeforeunload = function (e) {
        
        var dialogText = 'Bạn muốn thoát khỏi trình duyệt ?';
        e.returnValue = dialogText;
        
        return doPostBackAnswares1();
    };
    $(window).unload(function () {
        return doPostBackAnswares1();
    });
</script>
<div id="divRunData" runat="server"></div>