<%@ control language="C#" autoeventwireup="true" codebehind="ItemsMgmt.ascx.cs" inherits="nuce.ad.news.ItemsMgmt" %>
<%@ register tagprefix="dnn" tagname="TextEditor" src="../../controls/TextEditor.ascx" %>
<%@ register tagprefix="dnn" tagname="URLControl" src="../../controls/URLControl.ascx" %>
<span id="spThongBao" runat="server"></span>
<span id="spThamSo" runat="server"></span>
<div class="row" style="padding-top: 5px;">
    <div class="col-sm-8">
        <input class="form-control" id="myInput" type="text" placeholder="Search..">
    </div>
    <div class="col-sm-4">
        <select class="form-control" id="slDanhMucSearch" style="width: 200px;" onchange='ItemsMgmt.changeSlDanhMucSearch(this.value)'>
            <option>1</option>
            <option>2</option>
            <option>3</option>
            <option>4</option>
        </select>
    </div>
</div>

<div class="table-responsive">
    <table class="table">
        <thead>
            <tr>
                <th>#</th>
                <th>Tiêu đề</th>
                <th>Danh mục</th>
                <th>Ngày cập nhật</th>
                <th style="width: 5%;"></th>
             <%--   <th style="width: 5%;"></th>--%>
                <th style="width: 5%;"></th>
            </tr>
        </thead>
        <tbody id="tbContent">
        </tbody>
    </table>
</div>

<!-- Modal them sua -->
<div id="myModal" class="modal fade" role="dialog">
    <div class="modal-dialog" style="width: 760px;">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="edit_header" style="text-align:center;">Modal Header</h4>
                <div id="edit_header_ThongBao" style="color: red;"></div>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="ddlDanhMuc">Danh mục:</label>
                    <asp:DropDownList ID="ddlDanhMuc" class="form-control" runat="server"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="txtTieuDe">Tiêu đề:</label>
                    <asp:TextBox ID="txtTieuDe" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtTieuDe">Ảnh:</label>
                    <img class="img-rounded" runat="server" id="imgAnh" width="200">
                    <asp:FileUpload ID="FileUploadControl" runat="server" />
                </div>
                <div class="form-group">
                    <label for="txtMoTa">Mô tả:</label>
                    <asp:TextBox ID="txtMoTa" runat="server" TextMode="MultiLine" class="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtNoiDung">Nội dung:</label>
                    <dnn:texteditor id="txtNoiDung" runat="server" mode="Rich" width="100%" height="250"></dnn:texteditor>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                <asp:Button class="btn btn-default" ID="btnCapNhat" runat="server" Text="Cập nhật" OnClick="btnCapNhat_Click" />
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
                <button type="button" class="btn btn-danger" id="btnXoa" onclick=" ItemsMgmt.delete();">Xóa</button>
            </div>
        </div>
    </div>
</div>
<!-- Modal xoa -->
<div style="display: none;">
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
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
    var ItemsMgmt = {
        ID: -1,
        Data: [],
        DataDanhMuc: [],
        url: "/handler/nuce.ad.news/",
        init: function () {
            $.getJSON(this.url + "ad_news_getcats.ashx", function (data) {
                ItemsMgmt.DataDanhMuc = data;
                var strHtml = "";
                var strHtmlSearch = "<option value=\"-1\">Lựa chọn Danh mục</option>";
                $.each(data, function (i, item) {
                    strHtml += "<option value=\"" + item.ID + "\">" + item.Display + "</option>";
                    strHtmlSearch += "<option value=\"" + item.ID + "\">" + item.Display + "</option>";
                });
                //$("#" + strCtl + "slDanhMuc").html(strHtml);
                $("#slDanhMucSearch").html(strHtmlSearch);
            });
            ItemsMgmt.bindData('', '-1');
        },
        changeSlDanhMucSearch: function (giatri) {
            ItemsMgmt.bindData($('#myInput').val(), giatri);
        },
        bindData1: function () {
            ItemsMgmt.bindData($('#myInput').val(), $('#slDanhMucSearch').val());
        },
        bindData: function (text, giatridanhmuc) {
            $('#tbContent').html("");
            $.getJSON(this.url + "ad_news_getItems.ashx?search=" + text + "&&danhmuc=" + giatridanhmuc + "&&nocache='" + (new Date()).getTime(), function (data) {
                //alert(data);
                ItemsMgmt.Data = data;
                var strHtml = "";
                var count = 1;
                $.each(data, function (i, item) {
                    strHtml += "<tr>";
                    strHtml += "<td>" + count + "</td>";
                    strHtml += "<td>" + item.title + "</td>";
                    strHtml += "<td>" + item.DanhMuc + "</td>";
                    strHtml += "<td>" + item.NgayCapNhat + "</td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"ItemsMgmt.update(" + item.ID + ");\">Sửa</button></td>";
                    //strHtml += "<td><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"ItemsMgmt.add();\">Thêm mới</button></td>";
                    strHtml += "<td><button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal1\" onclick=\"ItemsMgmt.initDelete(" + item.ID + ");\">Xóa</button></td>";
                    strHtml += "</tr>";
                    count++;
                });
                strHtml += "<tr>";
                strHtml += "<td colspan='6' style='text-align:right;'><button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"ItemsMgmt.add();\">Thêm mới</button></td>";
                strHtml += "</tr>";
                $('#tbContent').html(strHtml);
            });
        },
        add: function () {
            ItemsMgmt.ID = -1;
            $("#edit_header").html("Thêm mới");
            $("#edit_header_ThongBao").hide();
            $("#" + strCtl + "txtTieuDe").val("");
            $("#" + strCtl + "txtMoTa").val("");
            $("#" + strCtl + "txtNoiDung_txtNoiDung").val("");
            $("#" + strCtl + "imgAnh").attr("src", "");
            $("#" + strCtl + "txtID").val("-1");

        },
        update: function (id) {
            window.location.href = link_root + "?ID=" + id;
        }
        ,
        initDelete: function (id) {
            ItemsMgmt.ID = id;
            $("#edit_header_ThongBao1").hide();
            $('#btnXoa').show();
        },
        delete: function () {
            //Update
            $.getJSON(this.url + "ad_news_deleteItems.ashx?ID=" + ItemsMgmt.ID, function (data) {
                if (data == 1) {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Xóa thành công");
                    $('#btnKhong').text("Thoát");
                    $('#btnXoa').hide();
                    ItemsMgmt.bindData1();
                }
                else {
                    $("#edit_header_ThongBao1").show();
                    $("#edit_header_ThongBao1").html("Lỗi hệ thống");
                }
            });
        },
        message: function (id, header, message) {
            $('#' + id).modal('show');
            $("#edit_header").html(header);
            $("#edit_header_ThongBao").show();
            $("#edit_header_ThongBao").html(message);
        },
        upload: function () {
            var url = $("#" + strCtl + "txtLinkUpload").val() + "/" + $('#slDanhMucSearch').val();
            window.location.href = url;
        }
    };
    ItemsMgmt.init();

</script>
<span id="spScript" runat="server"></span>
<script>
    $(document).ready(function () {
        $(document).keypress(function (event) {

            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                var search = $('#myInput').val();
                ItemsMgmt.bindData1();
                event.preventDefault();
            }

        });
    });
</script>

