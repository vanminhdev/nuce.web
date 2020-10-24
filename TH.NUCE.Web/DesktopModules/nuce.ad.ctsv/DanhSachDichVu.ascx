<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DanhSachDichVu.ascx.cs" Inherits="nuce.ad.ctsv.DanhSachDichVu" %>
<div class="row" style="padding-top: 5px; text-align: center; font-weight: bold;" runat="server" id="divSinhVien">
</div>
<div class="table-responsive">
    <table class="table">
        <thead>
            <tr>
                <th style="text-align: center; vertical-align: top;">STT</th>
                <th style="text-align: center; vertical-align: top;">Dịch vụ</th>
                <th style="text-align: center; vertical-align: top;">Tổng số</th>
                <th style="text-align: center; vertical-align: top;">Mới gửi</th>
                <th style="text-align: center; vertical-align: top;">Đang xử lý</th>
                <th style="text-align: center; vertical-align: top;">Đã xử lý xong</th>
            </tr>
        </thead>
        <tbody id="tbContent" runat="server">
        </tbody>
    </table>
</div>
<span id="spThamSo" runat="server"></span>
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
</script>
