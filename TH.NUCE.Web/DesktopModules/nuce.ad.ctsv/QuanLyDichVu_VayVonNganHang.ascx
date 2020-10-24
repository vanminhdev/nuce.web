<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuanLyDichVu_VayVonNganHang.ascx.cs" Inherits="nuce.ad.ctsv.QuanLyDichVu_VayVonNganHang" %>
<link id="bsdp-css" href="/bootstrap-datepicker/css/bootstrap-datepicker3.min.css" rel="stylesheet">
<script src="/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
<script src="/bootstrap-datepicker/locales/bootstrap-datepicker.uk.min.js" charset="UTF-8"></script>
<script src="/bootstrap-datepicker/locales/bootstrap-datepicker.vi.min.js" charset="UTF-8"></script>
<script src="/Resources/Shared/nuce/core.js" charset="UTF-8"></script>
<script src="/Resources/Shared/nuce/dichVuHelper.js" charset="UTF-8"></script>
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
    .table-input {
        width: 100%;
    }
    .status-symbol {
        width: 25px;
        height: 25px;
        border-radius: 50%;
        cursor: pointer;
    }
    .custom-bg-primary {
        background-color: #007bff;
    }
    .custom-bg-secondary {
        background-color: #6c757d;
    }
    .custom-bg-success {
        background-color: #28a745;
    }
    .custom-bg-danger {
        background-color: #dc3545;
    }
    .custom-bg-warning {
        background-color: #ffc107;
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
    .edit-container label {
        color: #8d8d8d;
    }
    .chip {
        display: inline-block;
        padding: 10px 25px;
        border-radius: 25px;
        margin-top: 5px;
        color: #fff;
    }
    .vertical-group-btn button {
        border-radius: 0;
    }
    .vertical-group-btn button:last-child {
        border-bottom-left-radius: 4px;
        border-bottom-right-radius: 4px;
    }
    .vertical-group-btn button:first-child {
        border-top-left-radius: 4px;
        border-top-right-radius: 4px;
    }
    .checkbox-select {
        cursor: pointer;
    }
</style>
<div class="row" style="padding-top: 5px;">
    <div class="col-sm-5">
        <input class="form-control" id="myInput" type="text" placeholder="Search..">
    </div>
    <div class="col-sm-3">
        <div class="form-group">
            <select class="form-control" id="slChonNgay" onchange="QuanLyDichVu_VayVonNganHang.changeNgayLayDuLieu(this);">
                <option value="7">Trong vòng 7 ngày</option>
                <option value="14">Trong vòng 14 ngày</option>
                <option value="21">Trong vòng 21 ngày</option>
                <option value="30">Trong vòng 30 ngày</option>
                <option value="60">Trong vòng 60 ngày</option>
                <option value="90">Trong vòng 90 ngày</option>
            </select>
        </div>
    </div>
    <div class="col-sm-4" style="text-align: right">
        <button type="button" class="btn btn-success" 
                onclick="QuanLyDichVu_VayVonNganHang.onClickConfirm(3)">Excel</button>
        <button type="button" class="btn btn-primary" 
                onclick="QuanLyDichVu_VayVonNganHang.onClickConfirm(1)">In</button>
        <button type="button" class="btn btn-info" 
                onclick="QuanLyDichVu_VayVonNganHang.onClickConfirm(2)">Chuyển trạng thái</button>
    </div>
</div>
<div>
    <div class="chip custom-bg-primary">
        Gửi yêu cầu lên nhà trường
    </div>
    <div class="chip custom-bg-secondary" style="color: #fff">
        Đã tiếp nhận và đang xử lý
    </div>
    <div class="chip custom-bg-warning">
        Đã xử lý và có lịch hẹn
    </div>
    <div class="chip custom-bg-danger">
        Từ chối dịch vụ
    </div>
    <div class="chip custom-bg-success">
        Hoàn thành
    </div>
</div>
<div class="table-responsive" style="margin-top: 5px;">
    <table class="table">
        <thead class="tbl-header">
            <tr>
                <th style="text-align: center; vertical-align: top;">
                    <input type="checkbox" class="checkbox-select"
                            id="select-all" onclick="QuanLyDichVu_VayVonNganHang.onSelectAll()" />
                </th>
                <th style="text-align: center; vertical-align: top;">TT</th>
                <th style="text-align: center; vertical-align: top;">MSSV</th>
                <th style="text-align: center; vertical-align: top;">Họ tên SV</th>
                <th style="text-align: center; vertical-align: top;">Nội dung</th>
                <th style="text-align: center; vertical-align: top;">Thời gian gửi yêu cầu</th>
                <th style="text-align: center; vertical-align: top;"></th>
            </tr>
        </thead>
        <tbody id="tbContent">
        </tbody>
    </table>
    <table class="table">
        <tr>
            <td style="text-align: right;">
                <ul class="pagination" id="ulPaging">
                    <li><a href="javascript:alert('hi hi');">1</a></li>
                    <li><a href="#">2</a></li>
                    <li><a href="#">3</a></li>
                    <li><a href="#">4</a></li>
                    <li><a href="#">5</a></li>
                </ul>
            </td>
        </tr>
    </table>
</div>
<span id="spThamSo" runat="server"></span>
<span id="spScriptInit" runat="server"></span>
<span id="spScript" runat="server"></span>
<!-- Modal them sua -->
<div id="myModal" class="modal fade" role="dialog">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" id="edit_header" style="text-align: center;">Modal Header</h4>
                <div id="edit_header_ThongBao" style="color: red;"></div>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="TrangThai">Trạng thái:</label>
                    <select class="form-control" id="TrangThai" onchange="dichvuHelper.changeTrangThai();">
                        <option value="3">Đã tiếp nhận và đang xử lý</option>
                        <option value="4">Đã xử lý và có lịch hẹn</option>
                        <option value="5">Từ chối dịch vụ</option>
                    </select>
                </div>

                <div style="padding-bottom: 0px;" id="divNgayHen">
                    <table class="table">
                        <tr>
                            <td colspan="6" style="font-weight: bold;">Thời gian hẹn sinh viên:</td>
                        </tr>
                        <tr>
                            <td colspan="6" style="font-weight: bold;">Ngày giờ bắt đầu:</td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; border: none; text-align: center; vertical-align: middle;">ngày (dd/mm/yyyy): </td>
                            <td style="width: 30%; border: none;">
                                <input type="text" class="form-control" id="txtNgayBatDau"></td>
                            <td style="font-weight: bold; width: 8%; border: none; text-align: center; vertical-align: middle;">Giờ:</td>
                            <td style="width: 12%; border: none;">
                                <input type="text" class="form-control" id="txtGioBatDau"></td>
                            <td style="font-weight: bold; width: 8%; border: none; text-align: center; vertical-align: middle;">Phút:</td>
                            <td style="width: 12%; border: none;">
                                <input type="text" class="form-control" id="txtPhutBatDau"></td>
                        </tr>
                        <tr>
                            <td colspan="6" style="font-weight: bold;">Ngày giờ kết thúc</td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; width: 30%; border: none; text-align: center; vertical-align: middle;">ngày (dd/mm/yyyy): </td>
                            <td style="width: 30%; border: none;">
                                <input type="text" class="form-control" id="txtNgayKetThuc"></td>
                            <td style="font-weight: bold; width: 8%; border: none; text-align: center; vertical-align: middle;">Giờ:</td>
                            <td style="width: 12%; border: none;">
                                <input type="text" class="form-control" id="txtGioKetThuc"></td>
                            <td style="font-weight: bold; width: 8%; border: none; text-align: center; vertical-align: middle;">Phút:</td>
                            <td style="width: 12%; border: none;">
                                <input type="text" class="form-control" id="txtPhutKetThuc"></td>
                        </tr>
                    </table>
                </div>
                <div class="form-group" id="divPhanHoi">
                    <label for="PhanHoi">Phản hồi:</label>
                    <textarea class="form-control" id="PhanHoi"></textarea>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                <button type="button" class="btn btn-default" id="btnCapNhat" onclick=" QuanLyDichVu_VayVonNganHang.update();">Cập nhật</button>
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
                <h4 class="modal-title" id="lblThongBaoXoa" style="text-align: center;">Bạn có chắc chắn muốn thực hiện ?</h4>
                <div id="edit_header_ThongBao1"></div>
            </div>
            <div class="modal-body" style="text-align: center;">
                <button type="button" class="btn btn-default" id="btnKhong" data-dismiss="modal">Không</button>
                <button type="button" class="btn btn-danger" id="btnXoa" onclick=" QuanLyDichVu_VayVonNganHang.delete();">Thực hiện</button>
            </div>
        </div>
    </div>
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
<!-- Modal confirm -->
<div id="myModalConfirm" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" style="text-align: center;">Xác nhận</h4>
            </div>
            <div class="modal-body" style="text-align: center;">
                <p id="h-confirm-content"></p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Thoát</button>
                <button id="btn-activate" type="button" class="btn btn-primary">Xác nhận</button>
            </div>
        </div>
    </div>
</div>
<div class="loader"></div>
<div style="display:none;">
    <asp:TextBox runat="server" ID="txtInputData"></asp:TextBox>
    <asp:Button runat="server" ID="btnExportData" OnClick="btnExportData_Click" />
    <asp:Button runat="server" ID="btnExcel" OnClick="btnExcel_Click" />
    <asp:Button runat="server" ID="btnPrint" OnClick="btnPrint_Click" />
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
    var QuanLyDichVu_VayVonNganHang = {
        ID: -1,
        Status: 1,
        Type: 6,
        Page: 1,
        RowCount: 50,
        Data: [],
        SameStudentRequest: {},
        selectedDataIndex: [],
        url: "/handler/nuce.ad.ctsv/",
        initData: function () {
        },
        ThuocDien: {
            1: 'Không miễn giảm',
            2: 'Giảm học phí',
            3: 'Miễn học phí',
        },
        ThuocDoiTuong: {
            1: 'Mồ côi',
            2: 'Không mồ côi',
        },
        onSelectAll: function() {
            dichvuHelper.onSelectAll(this.Data, this.selectedDataIndex, 'select-all');
        },
        setValue: function(index = 0, field = '') {
            dichvuHelper.setValue(index, field, this.Data, this.SameStudentRequest);
        },
        updateselectedDataIndex: function(clickIndex = 0, field = '') {
            dichvuHelper.updateselectedDataIndex(clickIndex, field, this.Data, this.selectedDataIndex);
        },
        afterConfirm: function(type) {
            $('#myModalConfirm').modal('hide');
            const data = [];
            QuanLyDichVu_VayVonNganHang.Data.forEach((item, index) => {
                if (QuanLyDichVu_VayVonNganHang.selectedDataIndex.includes(index)) {
                    data.push({
                        ID: item.ID,
                        HKTT_Tinh: item.HKTT_Tinh,
                        HKTT_Pho: item.HKTT_Pho,
                        HKTT_Phuong: item.HKTT_Phuong,
                        HKTT_Quan: item.HKTT_Quan,    
                        StudentID: item.StudentID,
                        Status: item.Status,
                    });
                }
            });
            const jsonData = JSON.stringify(data);
            $("#<%=txtInputData.ClientID%>").val(jsonData);
            switch(type) {
                    case 1:
                        __doPostBack('<%= btnExportData.UniqueID %>', '');
                        break;
                    case 2:
                        QuanLyDichVu_VayVonNganHang.updateFourthStatus(jsonData);
                        break;
                    case 3:
                        __doPostBack('<%= btnExcel.UniqueID %>', '');
                    default:
                        break;
            }
            return false;
        },
        onClickConfirm: function(type) {
            if (QuanLyDichVu_VayVonNganHang.selectedDataIndex.length > 0) {
                let content = '';
                switch(type) {
                    case 1:
                        content = 'Bạn có muốn xuất file word những yêu cầu đã chọn?';
                        break;
                    case 2:
                        content = 'Bạn có muốn chuyển tất cả các yêu cầu được chọn sang trạng thái ĐÃ XỬ LÝ VÀ CÓ LỊCH HẸN không?';
                        break;
                    case 3:
                        content = 'Bạn có muốn xuất Excel những yêu cầu đã chọn?';
                        break;
                    default:
                        break;
                }
                $('#btn-activate').click(function() { QuanLyDichVu_VayVonNganHang.afterConfirm(type); });
                $('#h-confirm-content').html(content);
                $('#myModalConfirm').modal('show');
                return;
            }
            $('#h_noidung_thongbao').html('Không có yêu cầu nào được chọn.');
            $('#myModalThongBao').modal('show');
        },
        updateFourthStatus: function(data) {
            dichvuHelper.updateFourthStatus(data, this.url, this.Type, this.bindData);
            return false;
        },
        bindData: function () {
            $('#tbContent').html("");
            $('#ulPaging').html("");
            var SPage = (QuanLyDichVu_VayVonNganHang.Page - 1) * QuanLyDichVu_VayVonNganHang.RowCount + 1;
            var EPage = QuanLyDichVu_VayVonNganHang.Page * QuanLyDichVu_VayVonNganHang.RowCount;
            //Lấy dữ liệu
            $.getJSON(QuanLyDichVu_VayVonNganHang.url + "ad_ctsv_qldv_xacnhan_get.ashx?Type=" + QuanLyDichVu_VayVonNganHang.Type + "&&search=" + $('#myInput').val() + "&&CDay=" + $('#slChonNgay').val() + "&&SPage="+SPage+"&&EPage="+EPage, function (data) {
                //alert(data);
                $(`#select-all`).prop({checked: false, indeterminate: false});
                QuanLyDichVu_VayVonNganHang.Data = data;
                QuanLyDichVu_VayVonNganHang.SameStudentRequest = {};
                QuanLyDichVu_VayVonNganHang.selectedDataIndex = [];
                var strHtml = "";
                var total = -1;
                $.each(data, function (i, item) {
                    total = item.Total;
                    if (item.StudentID in QuanLyDichVu_VayVonNganHang.SameStudentRequest) {
                        QuanLyDichVu_VayVonNganHang.SameStudentRequest[item.StudentID].push(i);
                    } else {
                        QuanLyDichVu_VayVonNganHang.SameStudentRequest[item.StudentID] = [i];
                    }
                    className = i % 2 === 0 ? 'tbl-even-row' : 'tbl-odd-row';
                    // row 1
                    strHtml += `<tr class="${className}">`;
                    strHtml += `<td rowspan="2" style="text-align: center; vertical-align: middle">`;
                    strHtml += `<input type="checkbox" id="${i}-select" 
                                    class="checkbox-select"
                                    onclick="QuanLyDichVu_VayVonNganHang.updateselectedDataIndex(${i}, 'select')">
                                </td>`;
                    strHtml += "<td rowspan=\"2\" style=\"text-align: center; vertical-align: middle;\">" + (i + SPage) + "</td>";
                    strHtml += "<td style=\"text-align: center; font-weight: bold; vertical-align: top;\">" + item.StudentCode + "</td>";
                    strHtml += "<td style=\"text-align: center; font-weight: bold; vertical-align: top;\">" + item.StudentName + "</td>";
                    strHtml += "<td style=\"text-align: left; vertical-align: top;\">";
                    strHtml += `<div class="row">`;
                    strHtml += `<div class="col-sm-11">`;
                    var strLydo = "<b>Thuộc diện:</b> " + QuanLyDichVu_VayVonNganHang.ThuocDien[item.ThuocDien] + "</br>";
                    strLydo += "<b>Thuộc đối tượng:</b> " + QuanLyDichVu_VayVonNganHang.ThuocDoiTuong[item.ThuocDoiTuong] + "</br>";
                    strHtml += `<div style="font-weight: bold; width: 300px;">${strLydo}</div>`;
                    if ([4, 6].includes(item.Status)) {
                        strHtml += `<div style="color:blue;">* Ngày hẹn: Từ  <b>${item.NgayHen_BatDau_Gio}</b> giờ - <b>${item.NgayHen_BatDau_Phut}</b> phút - Ngày <b>${item.NgayHen_BatDau_Ngay}</b></br>Đến <b>${item.NgayHen_KetThuc_Gio}</b> giờ - <b>${item.NgayHen_KetThuc_Phut}</b> phút - Ngày <b>${item.NgayHen_KetThuc_Ngay}</b></div>`;
                    } else if (item.Status === 5) {
                        strHtml += `<div style="color:red; max-width: 300px;">- Phản hồi: ${item.PhanHoi}</div>`;
                    }
                    strHtml += `</div>`;
                    strHtml += `<div class="col-sm-1">`;
                    bgColor = '';
                    title = '';
                    switch(item.Status) {
                        case 2:
                            bgColor = 'custom-bg-primary';
                            title = 'Gửi yêu cầu lên nhà trường';
                            break;
                        case 3:
                            bgColor = 'custom-bg-secondary';
                            title = 'Đã tiếp nhận và đang xử lý';
                            break;
                        case 4:
                            bgColor = 'custom-bg-warning';
                            title = 'Đã xử lý và có lịch hẹn';
                            break;
                        case 5:
                            bgColor = 'custom-bg-danger';
                            title = 'Từ chối dịch vụ';
                            break;
                        case 6:
                            bgColor = 'custom-bg-success';
                            title = 'Hoàn thành';
                            break;
                        default:
                            break;                
                    }
                    if (bgColor !== '') {
                        strHtml += `<div title="${title}" class="${bgColor} status-symbol"></div>`;
                    }
                    strHtml += `</div>`;
                    strHtml += `</div>`;
                    strHtml += `</td>`;
                    strHtml += "<td style=\"text-align: center; font-weight: bold; vertical-align: top;\">" + item.Ngay + "</td>";
                    strHtml += "<td rowspan=\"2\" style=\"text-align: center; vertical-align: middle;\">";
                    strHtml += `<div class="vertical-group-btn"><button type="button" style="display: block; width: 100%" class="btn btn-primary"  onclick="QuanLyDichVu_VayVonNganHang.print(${item.ID});">In</button>`;
                    strHtml += `<button type="button" style="display: block; width: 100%" class="btn btn-info" data-toggle="modal" data-target="#myModal" onclick="QuanLyDichVu_VayVonNganHang.fillData(${item.ID});">Chuyển trạng thái</button>`;
                    strHtml += `</td>`;
                    strHtml += `</div>`;
                    strHtml += "</tr>";
                    // row 2
                    strHtml += `<tr class="${className}">`;
                    strHtml += `<td colspan="4"><div>`;
                    strHtml += `<div class="row">`;
                    strHtml += `<div class="col-sm-3 form-group edit-container">`;
                    strHtml += `
                        <label for="Email">Phố</label>
                        <input type="text" class="form-control" id="${item.ID}-HKTT_Pho" onkeyup="QuanLyDichVu_VayVonNganHang.setValue(${i}, 'HKTT_Pho')" value="${item.HKTT_Pho || ''}"/>`;
                    strHtml += `</div>`;
                    strHtml += `<div class="col-sm-3 form-group edit-container">`;
                    strHtml += `
                        <label for="Email">Phường</label>
                        <input type="text" class="form-control" id="${item.ID}-HKTT_Phuong" onkeyup="QuanLyDichVu_VayVonNganHang.setValue(${i}, 'HKTT_Phuong')" value="${item.HKTT_Phuong || ''}"/>`;
                    strHtml += `</div>`;
                    strHtml += `<div class="col-sm-3 form-group edit-container">`;
                    strHtml += `
                        <label for="Email">Quận/Huyện</label>
                        <input type="text" class="form-control" id="${item.ID}-HKTT_Quan" onkeyup="QuanLyDichVu_VayVonNganHang.setValue(${i}, 'HKTT_Quan')" value="${item.HKTT_Quan || ''}"/>`;
                    strHtml += `</div>`;
                    strHtml += `<div class="col-sm-3 form-group edit-container">`;
                    strHtml += `
                        <label for="Email">Tỉnh/Thành phố</label>
                        <input type="text" class="form-control" id="${item.ID}-HKTT_Tinh" onkeyup="QuanLyDichVu_VayVonNganHang.setValue(${i}, 'HKTT_Tinh')" value="${item.HKTT_Tinh || ''}"/>`;
                    strHtml += `</div>`;
                    strHtml += `</div>`;
                    strHtml += `<div class="row">`;
                    
                    strHtml += `</div>`;            
                    strHtml += `</div></td>`;
                    strHtml += "</tr>";
                });
                $('#tbContent').html(strHtml);
                //Phân trang
                if (total > QuanLyDichVu_VayVonNganHang.RowCount) {
                    // <li><a href="javascript:alert('hi hi');">1</a></li>
                    //class="active"
                    var strHtmlPage = "";
                    var numPage = 1 + Math.floor((total - 1) / QuanLyDichVu_VayVonNganHang.RowCount);
                  
                    for (var i = 1; i <= numPage; i++) {
                        if (i != QuanLyDichVu_VayVonNganHang.Page)
                            strHtmlPage += "<li><a href=\"javascript:QuanLyDichVu_VayVonNganHang.changePage(" + i + ");\">" + i + "</a></li>";
                        else {
                            strHtmlPage += "<li class=\"active\"><a  href=\"#\">" + i + "</a></li>";
                        }
                    }
                    $('#ulPaging').html(strHtmlPage);
                }
            });
        },
        fillData: function(id) {
            //QuanLyDichVu_VayVonNganHang.ID = id;
            this.ID = id;
            dichvuHelper.fillData(id, this.Data);
        },
        //https://www.w3resource.com/javascript/form/javascript-date-validation.php
        //var from = $("#datepicker").val().split("-")
        // var f = new Date(from[2], from[1] - 1, from[0])
        update: function () {
            // Kiem tra ma trang
            var strPhanHoi = $("#PhanHoi").val().trim();
            var strStatus = $("#TrangThai").val();

            if (strStatus == -1) {
                if (($("#txtNgayBatDau").val() == "" || $("#txtNgayKetThuc").val() == "")) {
                    $("#edit_header_ThongBao").show();
                    $("#edit_header_ThongBao").html("Bạn hãy chọn thời gian hẹn");
                    return;
                }
                else {
                    if (convertNumberFromText($("#txtNgayBatDau").val()) >= convertNumberFromText($("#txtNgayKetThuc").val())) {
                        //$("#txtNgayKetThuc").focus();
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Bạn hãy chọn ngày hẹn KẾT THÚC lớn hơn ngày hẹn BẮT ĐẦU");
                        return;
                    }
                }
                if ($("#txtGioBatDau").val() == "") {
                    $("#txtGioBatDau").val("00");
                }
                else {
                    var hh = parseInt($("#txtGioBatDau").val());
                    if (isNaN(hh))
                        hh = 0;
                    if (hh < 0 || hh > 24)
                        $("#txtGioBatDau").val("00");
                    else if (hh < 10)
                        $("#txtGioBatDau").val("0" + hh);
                }
                if ($("#txtPhutBatDau").val() == "") {
                    $("#txtPhutBatDau").val("00");
                }
                else {
                    var mm = parseInt($("#txtPhutBatDau").val());
                    if (isNaN(mm))
                        mm = 0;
                    if (mm < 0 || mm > 60)
                        $("#txtPhutBatDau").val("00");
                    else if (mm < 10)
                        $("#txtPhutBatDau").val("0" + mm);
                }
                if ($("#txtGioKetThuc").val() == "") {
                    $("#txtGioKetThuc").val("00");
                }
                else {
                    var hh = parseInt($("#txtGioKetThuc").val());
                    if (isNaN(hh))
                        hh = 0;
                    if (hh < 0 || hh > 24)
                        $("#txtGioKetThuc").val("00");
                    else if (hh < 10)
                        $("#txtGioKetThuc").val("0" + hh);
                }
                if ($("#txtPhutKetThuc").val() == "") {
                    $("#txtPhutKetThuc").val("00");
                }
                else {
                    var mm = parseInt($("#txtPhutKetThuc").val());
                    if (isNaN(mm))
                        mm = 0;
                    if (mm < 0 || mm > 60)
                        $("#txtPhutKetThuc").val("00");
                    else if (mm < 10)
                        $("#txtPhutKetThuc").val("0" + mm);
                }
            }
            var strNgayBatDau = "01/01/1990 00:00";
            var strNgayKetThuc = "01/01/1990 00:00";
            if ($("#txtNgayBatDau").val() != "")
                strNgayBatDau = $("#txtNgayBatDau").val() + " " + $("#txtGioBatDau").val() + ":" + $("#txtPhutBatDau").val();
            if ($("#txtNgayKetThuc").val() != "")
                strNgayKetThuc = $("#txtNgayKetThuc").val() + " " + $("#txtGioKetThuc").val() + ":" + $("#txtPhutKetThuc").val();
            if (strStatus == null || strStatus == "") {
                $("#edit_header_ThongBao").show();
                $("#edit_header_ThongBao").html("Bạn hãy chọn trạng thái chuyển");
                return;
            }
            //alert(strNgayBatDau);
            $('.loader').show();
            $('#myModal').modal('hide')
            if (QuanLyDichVu_VayVonNganHang.ID > 0) {
                $.getJSON(this.url + "ad_ctsv_qldv_xacnhan_changestatus.ashx?Type=" + QuanLyDichVu_VayVonNganHang.Type + "&&ID=" + QuanLyDichVu_VayVonNganHang.ID + "&&STATUS=" + strStatus + "&&PhanHoi=" + strPhanHoi + "&&NgayBatDau=" + strNgayBatDau + "&&NgayKetThuc=" + strNgayKetThuc, function (data) {
                    $('.loader').hide();
                    if (data == 1) {
                        $("#h_noidung_thongbao").html("Cập nhật thành công");
                        $('#myModalThongBao').modal();
                        QuanLyDichVu_VayVonNganHang.bindData();
                    }
                    else {
                        $('#myModal').modal();
                        $("#edit_header_ThongBao").show();
                        $("#edit_header_ThongBao").html("Lỗi hệ thống");
                    }
                });
            }
        },
        print: function (input) {
            //window.location.href = '/tabid/' + tabid + '/default.aspx?act=print&&search=' + input;
            $('#myModalConfirm').modal('hide');
            let data = {};
            const selected = QuanLyDichVu_VayVonNganHang.Data.find(item => item.ID === input);
            if (typeof selected !== 'undefined') {
                data = {
                        ID: selected.ID,
                        HKTT_Tinh: selected.HKTT_Tinh,
                        HKTT_Pho: selected.HKTT_Pho,
                        HKTT_Phuong: selected.HKTT_Phuong,
                        HKTT_Quan: selected.HKTT_Quan,    
                        StudentID: selected.StudentID,
                        Status: selected.Status,
                };
            }
            $("#<%=txtInputData.ClientID%>").val(JSON.stringify(data));
            __doPostBack('<%= btnPrint.UniqueID %>', '');
            // window.open('/tabid/' + tabid + '/default.aspx?act=print&&search=' + input, '_blank');
        },
        changeNgayLayDuLieu: function (obj) {
            //alert($("#slChonNgay").val());
            QuanLyDichVu_VayVonNganHang.Page = 1;
            QuanLyDichVu_VayVonNganHang.bindData();
        },
        changePage: function (page) {
            //alert($("#slChonNgay").val());
            QuanLyDichVu_VayVonNganHang.Page = page;
            QuanLyDichVu_VayVonNganHang.bindData();
        }
    };
    QuanLyDichVu_VayVonNganHang.initData();
    QuanLyDichVu_VayVonNganHang.bindData();
</script>
<script>
    $(document).ready(function () {
        $(document).keypress(function (event) {

            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                QuanLyDichVu_VayVonNganHang.Page = 1;
                QuanLyDichVu_VayVonNganHang.bindData();
                event.preventDefault();
            }

        });
        var date_input_ngaybatdau = $('#txtNgayBatDau'); //our date input has the name "date"
        var date_input_ngayketthuc = $('#txtNgayKetThuc'); //our date input has the name "date"


        //var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
        var options = {
            format: 'dd/mm/yyyy',
            //    container: container,
            todayHighlight: true,
            autoclose: true,
        };
        date_input_ngaybatdau.datepicker(options);
        date_input_ngayketthuc.datepicker(options);
        
    });
</script>
<span id="spScriptUpdate" runat="server"></span>
