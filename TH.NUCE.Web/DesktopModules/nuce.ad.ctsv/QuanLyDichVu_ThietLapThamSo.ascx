<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyDichVu_ThietLapThamSo.ascx.cs" Inherits="nuce.ad.ctsv.QuanLyDichVu_ThietLapThamSo" %>
<link id="bsdp-css" href="/bootstrap-datepicker/css/bootstrap-datepicker3.min.css" rel="stylesheet">
<script src="/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
<script src="/bootstrap-datepicker/locales/bootstrap-datepicker.uk.min.js" charset="UTF-8"></script>
<script src="/bootstrap-datepicker/locales/bootstrap-datepicker.vi.min.js" charset="UTF-8"></script>
<script src="/Resources/Shared/nuce/core.js" charset="UTF-8"></script>
<style>
    .loader {
        display: none;
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(255,255,255,0.8) url(/images/loader.gif) center center no-repeat;
        z-index: 10000;
    }
    .tbl-header {
        background: #eee;
    }
    .tbl-odd-row {
        background: #eee;
    }
    .tbl-even-row {
        background: #fff;
    }
</style>
<div class="row" style="padding-top: 5px;">
    <div class="col-sm-4">
        <div class="form-group">
            <select class="form-control" id="slChonDichVu" onchange="QuanLyDichVu_ThietLapThamSo.changeDichVu(this);">
                
            </select>
        </div>
    </div>
    <div class="col-sm-8" style="text-align: right">
        <button class="btn btn-primary"
                onclick="return QuanLyDichVu_ThietLapThamSo.onSaveData();">Lưu</button>
    </div>
</div>
<div class="table-responsive" style="margin-top: 5px;">
    <table class="table">
        <thead class="tbl-header">
            <tr>
                <th style="text-align: center; vertical-align: top;">TT</th>
                <th style="text-align: center; vertical-align: top;">Tên tham số</th>
                <th style="text-align: center; vertical-align: top;">Giá trị</th>
            </tr>
        </thead>
        <tbody id="tbContent">
        </tbody>
    </table>
</div>
<div id="myModalThongBao" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="h_noidung_thongbao" style="text-align: center;"></h4>
            </div>
            <div class="modal-body" style="text-align: center;">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
            </div>
        </div>
    </div>
</div>
<div class="loader"></div>
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
    var QuanLyDichVu_ThietLapThamSo = {
        ID: -1,
        Status: 1,
        Type: 1,
        Page: 1,
        RowCount: 50,
        Data: [],
        url: "/handler/nuce.ad.ctsv/",
        bindData: function () {
            $('#tbContent').html("");
            //Lấy dữ liệu
            $.getJSON(QuanLyDichVu_ThietLapThamSo.url + "ad_ctsv_qldv_thamso_get.ashx?id=" + QuanLyDichVu_ThietLapThamSo.Type, function (data) {
                QuanLyDichVu_ThietLapThamSo.Data = data;
                let strHtml = "";
                $.each(data, function (i, item) {
                    strHtml += `<tr>`;
                    strHtml += `
                        <td style="text-align: center; vertical-align: middle">${i + 1}</td>
                        <td style="vertical-align: middle">${item.Name}</td>
                        <td style="vertical-align: middle">
                            <textarea class="form-control"
                                    rows="3" cols="55"
                                    onkeyup="QuanLyDichVu_ThietLapThamSo.setValue(this, ${i}, 'Value')"
                                    style="resize: none">${item.Value}</textarea>
                        </td>
                    `;
                    strHtml += `</tr>`;
                });
                $('#tbContent').html(strHtml);
            });
        },
        changeDichVu: function(context) {
            console.log(context.value);
            QuanLyDichVu_ThietLapThamSo.Type = context.value;
            QuanLyDichVu_ThietLapThamSo.bindData();
        },
        onSaveData: function() {
            const data = JSON.stringify(QuanLyDichVu_ThietLapThamSo.Data);
            $('.loader').show();
            $.post(
                `${this.url}ad_ctsv_qldv_thamso_update.ashx`,
                data,
                function() {
                    $('.loader').hide();
                    $("#h_noidung_thongbao").html("Cập nhật thành công");
                    $('#myModalThongBao').modal();
                    QuanLyDichVu_ThietLapThamSo.bindData();
                }
            ).fail(function(jqxhr, settings, ex) {
                console.log('ex: ', ex);
                $('#myModal').modal();
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Lỗi hệ thống");
            });
            return false;
        },
        setValue: function(context, index, field) {
            QuanLyDichVu_ThietLapThamSo.Data[index][field] = context.value;
        },
        getLoaiDichVu: function(callback) {
            $.getJSON(this.url + "ad_ctsv_qldv_loaidv_get.ashx", function (data) {
                $('#slChonDichVu').html('');
                strHtml = '';
                data.forEach(item => {
                    strHtml += `
                        <option value="${item.ID}">${item.Description}</option>
                    `;
                });
                $('#slChonDichVu').html(strHtml);
                QuanLyDichVu_ThietLapThamSo.Type = data.length > 0 ? data[0].ID : 0;
                callback();
            });
        },
        initData: function () {
            QuanLyDichVu_ThietLapThamSo.getLoaiDichVu(QuanLyDichVu_ThietLapThamSo.bindData);
        },
    };
    QuanLyDichVu_ThietLapThamSo.initData();
</script>