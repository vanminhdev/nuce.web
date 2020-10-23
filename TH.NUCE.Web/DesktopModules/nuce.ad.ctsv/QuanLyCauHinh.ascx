<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyCauHinh.ascx.cs" Inherits="nuce.ad.ctsv.QuanLyCauHinh" %>
<div class="row" style="padding-top: 5px; text-align: center; font-weight: bold;" runat="server" id="divSinhVien">
</div>
<div class="table-responsive">
    <table class="table">
        <thead>
            <tr>
                <th style="text-align: center; vertical-align: top;">Mô tả</th>
                <th style="text-align: center; vertical-align: top;">Trạng thái hiện tại</th>
                <th style="text-align: center; vertical-align: top;"></th>
            </tr>
        </thead>
        <tbody id="tbContent">
        </tbody>
    </table>
</div>
<span id="spThamSo" runat="server"></span>
<!-- Modal xoa -->
<div id="myModal1" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="lblThongBaoXoa" style="text-align:center;">Bạn có chắc chắn muốn thực hiện ?</h4>
                <div id="edit_header_ThongBao1"></div>
            </div>
            <div class="modal-body" style="text-align: center;">
                <button type="button" class="btn btn-default" id="btnKhong" data-dismiss="modal">Không</button>
                <button type="button" class="btn btn-danger" id="btnXoa" onclick=" QuanLyCauHinh.ChangeStatus();">Thực hiện</button>
            </div>
        </div>
    </div>
</div>

<script>
    (function ($) {
        $.fn.serializeAny = function () {
            var ret = [];
            $.each($(this).find(':input'), function () {
                if (this.name != "")
                    ret.push(encodeURIComponent(this.name) + "=" + encodeURIComponent($(this).val()));
            });

            return ret.join("&").replace(/%20/g, "+");
        }
    })(jQuery);
    var QuanLyCauHinh = {
        ID: -1,
        Status: 1,
        Data: [],
        url: "/handler/nuce.ad.ctsv/",
        initData: function () {
        },
        bindData: function () {
            $('#tbContent').html("");
            $.getJSON(this.url + "ad_ctsv_qlch_get.ashx", function (data) {
                //alert(data);
                QuanLyCauHinh.Data = data;
                var strHtml = "";
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td style='text-align: center; vertical-align: top;'>" + item.Description + "</td>";
                    if (item.Enabled)
                        {
                        strHtml += "<td style='text-align: center; color:blue; '>Đang hoạt động</td>";
                        QuanLyCauHinh.Status = 0;
                    }
                    else
                        {
                        strHtml += "<td style='text-align: center;color:red; '>Đang không hoạt động</td>";
                        QuanLyCauHinh.Status = 1;
                    }
                    strHtml += "<td style='text-align: center; vertical-align: top;'><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"QuanLyCauHinh.initChangeStatus(" + item.ID + "," + QuanLyCauHinh.Status + ",'Bạn có chắc chắn muốn chuyển trạng thái ?');\">Chuyển trạng thái</button></td>";
                    strHtml += "</tr>";

                });
                $('#tbContent').html(strHtml);
            });
        },
        initChangeStatus: function (id, status, message) {
            QuanLyCauHinh.ID = id;
            $("#lblThongBaoXoa").html(message);
            $("#edit_header_ThongBao1").hide();
            $('#btnXoa').show();
            //$("#btnXoa").on("click", QuanLyCauHinh.delete);
        },
        ChangeStatus: function () {
            //Update
            $.getJSON(this.url + "ad_ctsv_qlch_changestatus.ashx?ID=" + QuanLyCauHinh.ID + "&&STATUS=" + QuanLyCauHinh.Status, function (data) {
                if (data == 1) {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Thực hiện thành công");
                    $('#btnKhong').text("Thoát");
                    $('#btnXoa').hide();
                    QuanLyCauHinh.bindData();
                }
                else {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Lỗi hệ thống");
                }
            });
        }
    };
    QuanLyCauHinh.initData();
    QuanLyCauHinh.bindData();
</script>
