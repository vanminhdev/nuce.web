using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using Newtonsoft.Json;
namespace nuce.web.qlpm
{
    public partial class DiemDanh : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strInfo = "";
                if (Request.QueryString["tuanhientaiid"] != null && Request.QueryString["tuanhientaiid"] != "")
                {
                    try
                    {
                        txtID.Text = Request.QueryString["tuanhientaiid"];
                        //Kiểm tra xem có chuyển điểm danh
                        int iCheckStatus = data.dnn_NuceQLPM_SinhVien_DiemDanh.checkstatus(int.Parse(Request.QueryString["tuanhientaiid"].Trim()));
                        if (Request.QueryString["lophoc"] != null && Request.QueryString["lophoc"] != "")
                        {
                            strInfo = Request.QueryString["lophoc"] +" ";
                        }

                        switch (iCheckStatus)
                        {
                            case 1:
                                divTaoDiemDanh.Visible = false;
                                divC.Visible = false;
                                divAnnounce.InnerText = strInfo+"Ở trạng thái đã điểm danh xong";
                                bindataH();
                                break;
                                ;
                            case 2:
                                divTaoDiemDanh.Visible = false;
                                divC.Visible = true;
                                divQuayLai.Visible = false;
                                divAnnounce.InnerText = strInfo+ "Ở trạng thái đang điểm danh";
                                bindataC();
                                break;
                            default:
                                divTaoDiemDanh.Visible = true;
                                divQuayLai.Visible = false;
                                divC.Visible = false;
                                divAnnounce.InnerText = strInfo+ "Ở trạng thái chưa tạo điểm danh";
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        divAnnounce.InnerText =strInfo+ ex.ToString();
                    }
                }
                else
                {
                    divTaoDiemDanh.Visible = false;
                    divC.Visible = false;
                    divAnnounce.InnerText =strInfo+ "Chưa chọn tuần hiện tại";
                }

            }
        }
        protected void bindataC()
        {
            int iTuanHienTai = int.Parse(txtID.Text.Trim());
            // Lay du lieu diem danh de hien thi
            DataTable dtTable = data.dnn_NuceQLPM_SinhVien_DiemDanh.GetDataByTuanHienTai(iTuanHienTai);
            string strData = "<table border='1' style='width: 100%;padding: 3px; margin: 3px;'>";
            strData = strData + "<tr style='font-weight: bold;text-align: center;'>";
            strData = strData + string.Format("<td>{0}</td>", "");
            strData = strData + string.Format("<td>{0}</td>", "");
            strData = strData + string.Format("<td>{0}</td>", "Mã SV");
            strData = strData + string.Format("<td>{0}</td>", "Họ và Tên");
            strData = strData + string.Format("<td>{0}</td>", "Mã lớp QL");
            strData = strData + string.Format("<td>{0}</td>", "Điểm danh");
            strData = strData + string.Format("<td>{0}</td>", "Điểm danh tự động");
            strData = strData + string.Format("<td>{0}</td>", "Ghi chú");
            strData = strData + "</tr>";
            int iCount = 0;
            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                iCount++;
                strData = strData + "<tr>";
                strData = strData + string.Format("<td>{0}</td>", iCount);
                strData = strData + string.Format("<td> <input type='checkbox' name='namekiemtradanhdau' value='value_{0}' id='{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());
                strData = strData + string.Format("<td>{0}<span style='display: none;' id='spsvid_{1}'>{2}</span></td>", dtTable.Rows[i]["MaSV"].ToString(), dtTable.Rows[i]["DiemDanhNgayID"].ToString(), dtTable.Rows[i]["SinhVienID"].ToString());
                strData = strData + string.Format("<td>{0}</td>", dtTable.Rows[i]["HoVaTen"].ToString());
                strData = strData + string.Format("<td>{0}</td>", dtTable.Rows[i]["LopQuanLy"].ToString());
                if (bool.Parse(dtTable.Rows[i]["IsDiemDanh"].ToString()))
                    strData = strData + string.Format("<td> <input type='checkbox' checked='' name='namediemdanh' value='valuediemdanh_{0}' id='iddiemdanh_{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());
                else
                    strData = strData + string.Format("<td> <input type='checkbox' name='namediemdanh' value='valuediemdanh_{0}' id='iddiemdanh_{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());

                if (bool.Parse(dtTable.Rows[i]["IsDiemDanhTuDong"].ToString()))
                    strData = strData + string.Format("<td> <input type='checkbox' checked='' disabled='disabled' name='namediemdanhtudong' value='valuediemdanhtudong_{0}' id='iddiemdanhtudong_{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());
                else
                    strData = strData + string.Format("<td> <input type='checkbox' disabled='disabled' name='namediemdanhtudong' value='valuediemdanhtudong_{0}' id='iddiemdanhtudong_{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());

                strData = strData + string.Format("<td><input style='width:100%' type='text' id='idghichu{0}' value='{1}'/></td>",dtTable.Rows[i]["DiemDanhNgayID"].ToString(), dtTable.Rows[i]["GhiChu"].ToString());
                strData = strData + "</tr>";
            }
            strData = strData +
          string.Format("<tr><td colspan='8'>Có tất cả {0} sinh viên trong danh sách</td></tr>",iCount);
            strData = strData + "</table>";
            divContentC.InnerHtml = strData;
            int iStatus = data.dnn_NuceCommon_Request.getStatusByCodeAndPartnerID("DIEM_DANH", iTuanHienTai);
            switch (iStatus)
            {
                case -1:
                    btnGuiYeuCau.Text = "(Chưa gửi tự động) gửi";
                    break;
                case 3:
                    btnGuiYeuCau.Text = "(Đã xủ lý tự động) gửi lại";
                    break;
                case 5:
                    btnGuiYeuCau.Text = "(Chờ gửi tự động) gửi lại";
                    break;
                default:
                    btnGuiYeuCau.Text = "Đã gửi tự động";
                    btnGuiYeuCau.Visible = false;
                    divAnnounce.InnerText = divAnnounce.InnerText + " (Đã gửi tự động chờ xử lý)";
                    break;
            }
            txtRequestStatus.Text = iStatus.ToString();
        }
        protected void bindataH()
        {
            int iTuanHienTai = int.Parse(txtID.Text.Trim());
            // Lay du lieu diem danh de hien thi
            DataTable dtTable = data.dnn_NuceQLPM_SinhVien_DiemDanh.GetDataHByTuanHienTai(iTuanHienTai);
            string strData = "<table border='1' style='width: 100%;padding: 3px; margin: 3px;'>";
            strData = strData + "<tr style='font-weight: bold;text-align: center;'>";
            //strData = strData + string.Format("<td>{0}</td>", "");
            strData = strData + string.Format("<td>{0}</td>", "");
            strData = strData + string.Format("<td>{0}</td>", "Mã SV");
            strData = strData + string.Format("<td>{0}</td>", "Họ và Tên");
            strData = strData + string.Format("<td>{0}</td>", "Mã lớp QL");
            strData = strData + string.Format("<td>{0}</td>", "Điểm danh");
            strData = strData + string.Format("<td>{0}</td>", "Điểm danh tự động");
            strData = strData + string.Format("<td>{0}</td>", "Ghi chú");
            strData = strData + "</tr>";


            /*
             * xu ly thong ke diem danh
             */
            int iSoDiemDanhTuDong = 0;
            int iSoDiemDanh = 0;
            int iCount = 0;
            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                iCount++;
                strData = strData + "<tr>";
               // strData = strData + string.Format("<td> <input type='checkbox' name='namekiemtradanhdau' value='value_{0}' id='{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());
                strData = strData + string.Format("<td>{0}</td>", iCount);
                strData = strData + string.Format("<td>{0}</td>", dtTable.Rows[i]["MaSV"].ToString());
                strData = strData + string.Format("<td>{0}</td>", dtTable.Rows[i]["HoVaTen"].ToString());
                strData = strData + string.Format("<td>{0}</td>", dtTable.Rows[i]["LopQuanLy"].ToString());
                if (bool.Parse(dtTable.Rows[i]["IsDiemDanh"].ToString()))
                {
                    strData = strData + string.Format("<td> <input type='checkbox' disabled='disabled' checked='' name='namediemdanh' value='valuediemdanh_{0}' id='iddiemdanh_{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());
                    iSoDiemDanh++;
                }
                else
                    strData = strData + string.Format("<td> <input type='checkbox' disabled='disabled' name='namediemdanh' value='valuediemdanh_{0}' id='iddiemdanh_{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());

                if (bool.Parse(dtTable.Rows[i]["IsDiemDanhTuDong"].ToString()))
                {
                    strData = strData + string.Format("<td> <input type='checkbox' checked='' disabled='disabled' name='namediemdanhtudong' value='valuediemdanhtudong_{0}' id='iddiemdanhtudong_{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());
                    iSoDiemDanhTuDong++;
                }
                else
                    strData = strData + string.Format("<td> <input type='checkbox' disabled='disabled' name='namediemdanhtudong' value='valuediemdanhtudong_{0}' id='iddiemdanhtudong_{0}'></td>", dtTable.Rows[i]["DiemDanhNgayID"].ToString());

                strData = strData + string.Format("<td>{0}</td>", dtTable.Rows[i]["GhiChu"].ToString());
                strData = strData + "</tr>";
            }
            strData = strData +
                      string.Format("<tr><td colspan='7'>Có tất cả {0} sinh viên điểm danh trên tổng số {1} sinh viên cần điểm danh</td></tr>",
                          iSoDiemDanh, dtTable.Rows.Count);
            strData = strData +
          string.Format("<tr><td colspan='7'>Có tất cả {0} sinh viên điểm danh tự động trên tổng số {1} sinh viên cần điểm danh</td></tr>",
              iSoDiemDanhTuDong, iCount);
            strData = strData + string.Format("<tr><td colspan='7'><a href='/ExportExcel.aspx?type=2&&tuanhientaiid={0}' target='_blank'>Tải file</a></td></tr>", iTuanHienTai);
            strData = strData + "</table>";
            divContentH.InnerHtml = strData;
        }
        protected void txtTaoDiemDanh_Click(object sender, EventArgs e)
        {
            int iTuanHienTai = int.Parse(txtID.Text.Trim());
            data.dnn_NuceQLPM_SinhVien_DiemDanh.taodiemdanh(iTuanHienTai);
            Response.Redirect(Request.RawUrl);
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            int iTuanHienTai = int.Parse(txtID.Text.Trim());
            string strData = txtData.Text;
            string strReturnData = "";
            string[] strSplit1 = strData.Split(new string[]{"|||"}, StringSplitOptions.RemoveEmptyEntries);
            if (strSplit1.Length > 0)
            {
                for (int i = 0; i < strSplit1.Length; i++)
                {
                    string[] strSplit2 = strSplit1[i].Split(new string[] { "$$$" }, StringSplitOptions.RemoveEmptyEntries);
                    int iID = int.Parse(strSplit2[0]);
                    bool iCheck = bool.Parse(strSplit2[1]);
                    string strGhiChu = strSplit2[2];
                    int iSvId=int.Parse(strSplit2[3]);
                    //Goi ham cap nhat
                    data.dnn_NuceQLPM_SinhVien_DiemDanh.update(iID,iTuanHienTai , iSvId, iCheck, strGhiChu);
                    strReturnData = strReturnData + strSplit2[0]+",";
                }
            }
            else
            {
                divAnnounce.InnerText = "Không có dữ liệu";
            }
            bindataC();
            txtData.Text = strReturnData;
        }

        protected void btnCapNhatTatCa_Click(object sender, EventArgs e)
        {
            //Goi ham cap nhat
            int iTuanHienTai = int.Parse(txtID.Text.Trim());
            string strData = txtData.Text;
            string strReturnData = "";
            string[] strSplit1 = strData.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            if (strSplit1.Length > 0)
            {
                for (int i = 0; i < strSplit1.Length; i++)
                {
                    string[] strSplit2 = strSplit1[i].Split(new string[] { "$$$" }, StringSplitOptions.RemoveEmptyEntries);
                    int iID = int.Parse(strSplit2[0]);
                    bool iCheck = bool.Parse(strSplit2[1]);
                    string strGhiChu = strSplit2[2];
                    int iSvId = int.Parse(strSplit2[3]);
                    //Goi ham cap nhat
                    data.dnn_NuceQLPM_SinhVien_DiemDanh.update(iID,iTuanHienTai, iSvId, iCheck, strGhiChu);
                    strReturnData = strReturnData + strSplit2[0] + ",";
                }
            }
            else
            {
                divAnnounce.InnerText = "Không có dữ liệu";
            }
            bindataC();
            txtData.Text = strReturnData;
        }

        protected void btnCapNhatTuDong_Click(object sender, EventArgs e)
        {
            int iTuanHienTai = int.Parse(txtID.Text.Trim());
            string strData = txtData.Text;
            string strReturnData = "";
            string[] strSplit1 = strData.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            if (strSplit1.Length > 0)
            {
                for (int i = 0; i < strSplit1.Length; i++)
                {
                    string[] strSplit2 = strSplit1[i].Split(new string[] { "$$$" }, StringSplitOptions.RemoveEmptyEntries);
                    int iID = int.Parse(strSplit2[0]);
                    bool iCheck = bool.Parse(strSplit2[1]);
                    string strGhiChu = strSplit2[2];
                    int iSvId = int.Parse(strSplit2[3]);
                    data.dnn_NuceQLPM_SinhVien_DiemDanh.update(iID,iTuanHienTai, iSvId, iCheck, strGhiChu);
                    //Goi ham cap nhat tu dong
                    strReturnData = strReturnData + strSplit2[0] + ",";
                }
            }
            else
            {
                divAnnounce.InnerText = "Không có dữ liệu";
            }
            bindataC();
            txtData.Text = strReturnData;
        }

        protected void btnCapNhatTuDongTatCa_Click(object sender, EventArgs e)
        {
            int iTuanHienTai = int.Parse(txtID.Text.Trim());
            data.dnn_NuceQLPM_SinhVien_DiemDanh.updateautoall(iTuanHienTai);
            bindataC();
        }

        protected void btnChotDuLieu_Click(object sender, EventArgs e)
        {
            int iTuanHienTai = int.Parse(txtID.Text.Trim());
            data.dnn_NuceQLPM_SinhVien_DiemDanh.chotdulieu(iTuanHienTai);
            Response.Redirect(Request.RawUrl);
        }

        protected void btnGuiYeuCau_Click(object sender, EventArgs e)
        {
            //Quá trình biến đổi trạng thái của một request sẽ là:

            //- 1: Trạng thái bắt đầu
            //- 2: Trạng thái đang bắt đầu xử lý (đã gửi tin nhắn đến thiết bị nhưng chưa có phản hồi)
            //- 3: Trạng thái đã xử lý xong
            //- 4: Trạng thái xóa
            //- 5: Trạng thái xếp hàng chờ đợi để được bắt đầu
            //- 6: Trạng thái đã gửi tin nhắn đến thiết bị chờ tín hiệu kết thúc

            int iStatus = int.Parse(txtRequestStatus.Text.Trim());
            int iTuanHienTai = int.Parse(txtID.Text.Trim());
            string text = "";
            DataTable tblTuanHienTai = data.dnn_NuceQLPM_TuanHienTai.getByType(iTuanHienTai, 2);
            switch (iStatus)
            {
                case -1:
                    //
                    if (checkDataRequest(tblTuanHienTai, out text))
                    {
                        if (tblTuanHienTai.Rows.Count > 0)
                        {
                            string JSONString = string.Empty;
                            JSONString = JsonConvert.SerializeObject(tblTuanHienTai);
                            // Lay du lieu can diem danh 
                            DataTable tblDSDiemDanh =
                                data.dnn_NuceQLPM_SinhVien_DiemDanh.GetDataUsingRequestByTuanHienTai(iTuanHienTai);
                            string JSONStringDSDiemDanh = string.Empty;
                            JSONStringDSDiemDanh = JsonConvert.SerializeObject(tblDSDiemDanh);
                            /*
                            JSONStringDSDiemDanh = "[";
                            //
                            for (int i = 0; i < tblDSDiemDanh.Rows.Count; i++)
                            {
                                JSONStringDSDiemDanh = JSONStringDSDiemDanh + "{";
                                JSONStringDSDiemDanh = JSONStringDSDiemDanh + string.Format("\"SinhVienID\":{0},\"Vantay\":{1}", tblDSDiemDanh.Rows[i]["SinhVienID"].ToString(), tblDSDiemDanh.Rows[i]["Vantay"].ToString());
                                JSONStringDSDiemDanh = JSONStringDSDiemDanh + "},";
                            }
                            JSONStringDSDiemDanh = JSONStringDSDiemDanh.Substring(0, JSONStringDSDiemDanh.Length - 1);
                            JSONStringDSDiemDanh = JSONStringDSDiemDanh + "]";*/
                            data.dnn_NuceCommon_Request.insert1("Điểm danh tự động", Utils.code_diem_danh, "Điểm danh tự động",
                                iTuanHienTai, JSONString, JSONStringDSDiemDanh, "", this.UserId, this.UserId, DateTime.Now, DateTime.Now, 1);
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            divAnnounce.InnerText = divAnnounce.InnerText + " (Không tồn tại dữ liệu ở tuần hiện tại !!!)";
                        }
                    }
                    else
                        divAnnounce.InnerText = divAnnounce.InnerText + "Không thực hiện được vì: " + text;
                    break;
                case 3:
                case 5:
                    if (checkDataRequest(tblTuanHienTai, out text))
                    {
                        data.dnn_NuceCommon_Request.updateStatus1(Utils.code_diem_danh, iTuanHienTai, 1);
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                        divAnnounce.InnerText = divAnnounce.InnerText + "Không thực hiện được vì: " + text;
                    break;
                default:
                    btnGuiYeuCau.Visible = false;
                    break;
            }
        }

        private bool checkDataRequest(DataTable tblTuanHienTai,out string text)
        {
            text = "";
            /*Kiểm tra tuần hiện tại
            Lấy các dữ liệu cần kiểm tra
             *1. Phòng nào ID, Lớp học nào ID, ca nào ID, ngày nào
             *2. Lấy tất cả các request đang ở trạng thái =1,2,6 có code là DIEM_DANH
                
             * 
            */
            try
            {
                DataTable dtRequest = data.dnn_NuceCommon_Request.getByCodeAndStatus(Utils.code_diem_danh, ",1,2,6");
                for (int i = 0; i < dtRequest.Rows.Count; i++)
                {
                    string strData = dtRequest.Rows[i]["Data"].ToString();
                    DataTable dtDeserialize = JsonConvert.DeserializeObject<DataTable>(strData);
                    // Kiểm tra xem có cùng phòng
                    if (tblTuanHienTai.Rows[0]["PhongHocID"].ToString() == dtDeserialize.Rows[0]["PhongHocID"].ToString())
                    {
                        text = "Đã tồn tại phòng học đang xử lý";
                        return false;
                    }
                    // Kiểm tra xem có cùng ngày, ca học
                    if (tblTuanHienTai.Rows[0]["Ngay"].ToString() == dtDeserialize.Rows[0]["Ngay"].ToString()&&
                        tblTuanHienTai.Rows[0]["CaHocID"].ToString() == dtDeserialize.Rows[0]["CaHocID"].ToString()
                        && tblTuanHienTai.Rows[0]["LopHocID"].ToString() == dtDeserialize.Rows[0]["LopHocID"].ToString())
                    {
                        text = "Đã tồn tại lớp học có cùng ngày và ca học";
                        return false;
                    }
                    if(DateTime.Parse(tblTuanHienTai.Rows[0]["Ngay"].ToString())>DateTime.Now)
                    {
                        text = "Đã quá ngày xử lý";
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                text = ex.ToString();
                return false;
            }
            return true;
        }
    }
}