var dichvuHelper = {
    ID: -1,
    Status: 1,
    Type: 4,
    Page: 1,
    RowCount: 50,
    Data: [],
    SameStudentRequest: {},
    selectedDataIndex: [],
    url: "/handler/nuce.ad.ctsv/",
    onSelectAll: function(data, selectedIndex = [], selectAllId = 'select-all') {
        const isChecked = $(`#${selectAllId}`).is(':checked');
        for(let i = 0; i < data.length; i++) {
            dichvuHelper.handleSelectedData(i, isChecked, selectedIndex);
            $(`#${i}-select`).prop({ checked: isChecked });
        }
        if(!isChecked) {
            selectedIndex = [];
        }
    },
    handleSelectedData: function(itemIndex = 0, isChecked, selectedIndex = []) {
        if (isChecked && !selectedIndex.includes(itemIndex)) {
            selectedIndex.push(itemIndex);
        } else if (!isChecked) {
            const index = selectedIndex.indexOf(itemIndex);
            selectedIndex.splice(index, 1);
        }
    },
    setValue: function(index = 0, field = '', data = [], sameStudentRequest = []) {
        const { StudentID, ID } = data[index];
        sameStudentRequest[StudentID].forEach(dataIndex => {
            const item = data[dataIndex];
            item[field] = $(`#${ID}-${field}`).val();
            $(`#${item.ID}-${field}`).val(item[field]);
        })
    },
    updateselectedDataIndex: function(clickIndex = 0, field = '', data = [], selectedIndex = []) {
        const isChecked = $(`#${clickIndex}-${field}`).is(':checked');
        dichvuHelper.handleSelectedData(clickIndex, isChecked, selectedIndex);
        const count = selectedIndex.length;
        const maxLength = data.length;
        if (count === 0) {
            $(`#select-all`).prop({checked: false, indeterminate: false});
            return;
        }
        if (count === maxLength) {
            $(`#select-all`).prop({checked: true, indeterminate: false});
            return;
        }
        $(`#select-all`).prop({
            indeterminate: true,
            checked: false
        });
    },
    updateFourthStatus: function(data, url, type, callback) {
        $('.loader').show();
        $.post(
            `${url}ad_ctsv_qldv_xacnhan_changestatus_selected.ashx?type=${type}`,
            data,
            function() {
                $('.loader').hide();
                $("#h_noidung_thongbao").html("Cập nhật thành công");
                $('#myModalThongBao').modal();
                callback();
            }
        ).fail(function(jqxhr, settings, ex) {
            console.log('ex: ', ex);
            $('#myModal').modal();
            $("#edit_header_ThongBao").show();
            $("#edit_header_ThongBao").html("Lỗi hệ thống");
        });
        return false;
    },
    fillData: function (id = 0, data = []) {
        $("#edit_header").html("Chuyển trạng thái");
        $("#edit_header_ThongBao").hide();
        $.each(data, function (i, item) {
            if (item.ID == id) {
                $("#PhanHoi").val(item.PhanHoi);
                $("#TrangThai").val(item.Status);
                $("#txtNgayBatDau").val(item.NgayHen_BatDau_Ngay);
                $("#txtGioBatDau").val(item.NgayHen_BatDau_Gio);
                $("#txtPhutBatDau").val(item.NgayHen_BatDau_Phut);

                $("#txtNgayKetThuc").val(item.NgayHen_KetThuc_Ngay);
                $("#txtGioKetThuc").val(item.NgayHen_KetThuc_Gio);
                $("#txtPhutKetThuc").val(item.NgayHen_KetThuc_Phut);

                let options = `<option value="3">Đã tiếp nhận và đang xử lý</option>
                    <option value="4">Đã xử lý và có lịch hẹn</option>
                    <option value="5">Từ chối dịch vụ</option>`;
                let status = item.Status;
                switch(item.Status) {
                    case 4:
                        options = `<option value="5">Từ chối dịch vụ</option>`;
                        status = 5;
                        break;
                    case 5:
                        options = `<option value="4">Đã xử lý và có lịch hẹn</option>`; 
                        status = 4;
                        break;
                    default:
                        break;
                };
                $(`#TrangThai`).html(options);
                console.log('before');
                dichvuHelper.showHideFormTrangThai(status);
            }
        });
    },
    showHideFormTrangThai: function (input) {
        $("#divNgayHen").hide();
        console.log('sdfsdf');
        if (input == 5) {
            $("#divPhanHoi").show();
            //$("#divNgayHen").hide();
        }
        else {
            $("#divPhanHoi").hide();
            //$("#divNgayHen").show();
        }
    },
    changeTrangThai: function () {
        dichvuHelper.showHideFormTrangThai($("#TrangThai").val());
    },
};