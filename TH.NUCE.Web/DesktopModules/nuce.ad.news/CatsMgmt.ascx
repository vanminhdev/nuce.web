<%@ control language="C#" autoeventwireup="true" codebehind="CatsMgmt.ascx.cs" inherits="nuce.ad.news.CatsMgmt" %>
<div class="table-responsive">
    <table class="table">
        <thead>
            <tr>
                <th>ID</th>
                <th>Tên</th>
                <th>Tên hiển thị</th>
                <th>Thứ tự</th>
                <th style="width: 5%;"></th>
             <%--   <th style="width: 5%;"></th>--%>
                <th style="width: 5%; "></th>
            </tr>
        </thead>
        <tbody id="tbContent">
         
        </tbody>
    </table>
</div>

<!-- Modal them sua -->
<div id="myModal" class="modal fade" role="dialog">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="edit_header" style="text-align:center;">Modal Header</h4>
                <div id="edit_header_ThongBao" style="color: red;"></div>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="Ten">Tên:</label>
                    <input type="text" class="form-control" id="Ten">
                </div>
                <div class="form-group">
                    <label for="Ten">Tên hiển thị:</label>
                    <input type="text" class="form-control" id="TenHienThi">
                </div>
                <div class="form-group">
                    <label for="MoTa">Mô tả:</label>
                    <input type="text" class="form-control" id="MoTa">
                </div>
                <div class="form-group">
                    <label for="MoTa">Thứ tự:</label>
                    <input type="text" class="form-control" id="ThuTu">
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                <button type="button" class="btn btn-default" id="btnCapNhat" onclick="CatsMgmt.update();">Cập nhật</button>
            </div>
        </div>

    </div>
</div>
<!-- Modal xoa -->
<div id="myModal1" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" style="text-align:center;">Bạn có chắc chắn muốn xóa ?</h4>
                <div id="edit_header_ThongBao1"></div>
            </div>
            <div class="modal-body" style="text-align: center;">
                <button type="button" class="btn btn-default" id="btnKhong" data-dismiss="modal">Không</button>
                <button type="button" class="btn btn-danger" id="btnXoa" onclick="CatsMgmt.delete();">Xóa</button>
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
    var CatsMgmt = {
        ID: -1,
        Data: [],
        url: "/handler/nuce.ad.news/",
        bindData: function () {
            $('#tbContent').html("");
            $.getJSON(this.url + "ad_news_getcats.ashx", function (data) {
                //alert(data);
                CatsMgmt.Data = data;
                var strHtml = "";
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td>" + item.ID + "</td>";
                    strHtml += "<td>" + item.Name + "</td>";
                    strHtml += "<td>" + item.Display + "</td>";
                    strHtml += "<td>" + item.Count + "</td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"CatsMgmt.fillData(" + item.ID + ");\">Sửa</button></td>";
                    //strHtml += "<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"CatsMgmt.add();\">Thêm mới</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"CatsMgmt.initDelete(" + item.ID + ");\">Xóa</button></td>";
                    strHtml += "</tr>";

                });
                strHtml += "<tr>";
                strHtml += "<td colspan='6' style='text-align:right;'><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"CatsMgmt.add();\">Thêm mới</button></td>";
                strHtml += "</tr>";
                $('#tbContent').html(strHtml);
            });
        },
        add: function () {
            CatsMgmt.ID = -1;
            $("#edit_header").html("Thêm mới");
            $("#edit_header_ThongBao").hide();
            $("#TenHienThi").val("");
            $("#Ten").val("");
            $("#MoTa").val("");
            $("#ThuTu").val("1000");
        },
        fillData: function (id) {
            CatsMgmt.ID = id;
            $("#edit_header").html("Sửa thông tin");
            $("#edit_header_ThongBao").hide();

            $.getJSON(this.url + "ad_news_getchitietcats.ashx?ID=" + id, function (data) {
                //alert(data[0].ID);
                $.each(data, function (i, item) {
                    $("#TenHienThi").val(item.Display);
                    $("#Ten").val(item.Name);
                    $("#MoTa").val(item.Des);
                    $("#ThuTu").val(item.Count);
                });
            });

        },
        update: function () {
            // Kiem tra ma trang
            strTenHienThi = $("#TenHienThi").val().trim();
            strTen = $("#Ten").val().trim();
            strMoTa = $("#MoTa").val().trim();
            strCount = $("#ThuTu").val().trim();
            if (strTenHienThi == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Không được để tên hiển thị trắng");
                return;
            }
            if (strTen == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Không được để tên trắng");
                return;
            }
            //Kiem tra co trung ma
            kiemtra = 0;
            $.each(this.Data, function (i, item) {
                if (item.Name == strTen && (item.ID != CatsMgmt.ID)) {

                    kiemtra = 1;
                    return;
                }
            });
            if (kiemtra == 1) {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Không được để trùng tên: " + strTen);
                return;
            }
            if (CatsMgmt.ID > 0) {
                //Update
                $.getJSON(this.url + "ad_news_updatecats.ashx?ID=" + CatsMgmt.ID + "&&Name=" + strTen + "&&Display=" + strTenHienThi + "&&Des=" + strMoTa + "&&Count=" + strCount + "&&Type=1", function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        CatsMgmt.bindData();
                    }
                    else {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }
            else {
                $.getJSON(this.url + "ad_news_insertcats.ashx?ID=" + CatsMgmt.ID + "&&Name=" + strTen + "&&Display=" + strTenHienThi + "&&Des=" + strMoTa + "&&Count=" + strCount + "&&Type=1", function (data) {
                    if (data == 1) {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Cập nhật thành công");
                        CatsMgmt.bindData();
                    }
                    else {
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }

            //alert(this.Data.length);
        },
        initDelete: function (id) {
            CatsMgmt.ID = id;
            $("#edit_header_ThongBao1").hide();
            $('#btnXoa').show();
        },
        delete: function () {
            //Update
            $.getJSON(this.url + "ad_tcc_deletemonthi.ashx?ID=" + CatsMgmt.ID, function (data) {
                if (data == 1) {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Xóa thành công");
                    $('#btnKhong').text("Thoát");
                    $('#btnXoa').hide();
                    CatsMgmt.bindData();
                }
                else {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Lỗi hệ thống");
                }
            });
        }
    };
    CatsMgmt.bindData();
</script>
