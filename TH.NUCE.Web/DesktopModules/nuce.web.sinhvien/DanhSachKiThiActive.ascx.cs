using System;
namespace nuce.web.sinhvien
{
    public partial class DanhSachKiThiActive : CoreModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Thi

                string strHtml = "<table border='1px' style='width: 100%;'>";
                strHtml += "<tr>";
                strHtml += "<td style='width: 50px;text-align: center;font-weight: bold;'>STT</td>";
                strHtml += "<td style='width: 50%;text-align: center;font-weight: bold;'>Tên</td>";
                strHtml += "<td style='width: 12%;text-align: center;font-weight: bold;'>Thời gian thi</td>";
                strHtml += "<td style='width: 12%;text-align: center;font-weight: bold;'>Thời gian thi còn lại</td>";
                strHtml += "<td style='width: 12%;text-align: center;font-weight: bold;'>Trạng thái</td>";
                strHtml += "<td style='text-align: center;font-weight: bold;'></td>";
                strHtml += "</tr>";
                int i = 0;
                foreach (var data in m_KiThiLopHocSinhViens)
                {
                    model.KiThiLopHocSinhVien KiThiLopHocSinhVien = data.Value;
                    i++;
                    strHtml += "<tr>";
                    strHtml += string.Format("<td style='width: 50px;text-align: center;font-weight: bold;'>{0}</td>", i);
                    strHtml += string.Format("<td style='width: 50%;'>{0} (Môn: {1};Block học: {2})</td>", KiThiLopHocSinhVien.TenKiThi, KiThiLopHocSinhVien.TenMonHoc, KiThiLopHocSinhVien.TenBlockHoc) ;
                    strHtml += string.Format("<td style='width: 12%;text-align: center;'>{0} phút</td>", KiThiLopHocSinhVien.TongThoiGianThi);
                    strHtml += string.Format("<td style='width: 12%;text-align: center;'>{0} phút {1} giây</td>", KiThiLopHocSinhVien.TongThoiGianConLai/60, KiThiLopHocSinhVien.TongThoiGianConLai%60);
                    switch(KiThiLopHocSinhVien.Status)
                    {
                        case 2: strHtml += string.Format("<td style='width: 10%;text-align: center;'>{0}</td>", "Chuẩn bị thi");
                            strHtml += string.Format("<td style='text-align: center;'><a href='/tabid/{0}/default.aspx?kithilophocsinhvien={1}'>Vào thi</a></td>", 1119, KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien);
                            break;
                        case 3:
                            strHtml += string.Format("<td style='width: 10%;text-align: center;'>{0}</td>", "Đang thi");
                            strHtml += string.Format("<td style='text-align: center;'><a href='/tabid/{0}/default.aspx?kithilophocsinhvien={1}'>Vào thi</a></td>", 1119, KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien);
                            break;
                        case 4:
                            strHtml += string.Format("<td style='width: 10%;text-align: center;'>{0}</td>", "Đã thi xong");
                            strHtml += string.Format("<td style='text-align: center; color:red;'>Điểm: {0:N2}</td>", KiThiLopHocSinhVien.Diem);
                            break;
                        case 5:
                            strHtml += string.Format("<td style='width: 10%;text-align: center;'>{0}</td>", "Chuẩn bị thi tiếp");
                            strHtml += string.Format("<td style='text-align: center;'><a href='/tabid/{0}/default.aspx?kithilophocsinhvien={1}'>Vào thi</a></td>", 1119, KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien);
                            break;
                        case 6:
                            strHtml += string.Format("<td style='width: 10%;text-align: center;'>{0}</td>", "Chuẩn bị thi lại");
                            strHtml += string.Format("<td style='text-align: center;'><a href='/tabid/{0}/default.aspx?kithilophocsinhvien={1}'>Vào thi</a></td>", 1119, KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien);
                            break;
                        default:break;
                    }
                    
                    strHtml += "</tr>";
                }
                strHtml += "</table>";
                #endregion
                #region Hoc
                strHtml += "<table border='1px' style='width: 100%;'>";
                strHtml += "<tr>";
                strHtml += "<td style='width: 50px;text-align: center;font-weight: bold;'>STT</td>";
                strHtml += "<td style='width: 20%;text-align: center;font-weight: bold;'>Tên môn học</td>";
                strHtml += "<td style='width: 20%;text-align: center;font-weight: bold;'>Tên Ca</td>";
                strHtml += "<td style='width: 30%;text-align: center;font-weight: bold;'>Thời gian</td>";
                strHtml += "<td style='width: 10%;text-align: center;font-weight: bold;'>Trạng thái</td>";
                strHtml += "<td style='text-align: center;font-weight: bold;'></td>";
                strHtml += "<td style='text-align: center;font-weight: bold;'></td>";
                strHtml += "</tr>";
                i = 0;
                foreach (var data in m_CaLopHocSinhViens)
                {
                    model.CaLopHocSinhVien CaLopHocSinhVien = data.Value;
                    i++;
                    strHtml += "<tr>";
                    strHtml += string.Format("<td style='width: 50px;text-align: center;font-weight: bold;'>{0}</td>", i);
                    strHtml += string.Format("<td style='width: 20%;text-align: center;'>{0}</td>", CaLopHocSinhVien.TenMonHoc);
                    strHtml += string.Format("<td style='width: 20%;text-align: center;'>{0}</td>", CaLopHocSinhVien.TenCa);
                    strHtml += string.Format("<td style='width: 30%;text-align: center;'>Ngày {0} (từ {1} giờ {2} phút đến {3} giờ {4} phút)</td>", CaLopHocSinhVien.Ngay.ToString("dd/MM/yyyy"), CaLopHocSinhVien.GioBatDau, CaLopHocSinhVien.PhutBatDau, CaLopHocSinhVien.GioKetThuc, CaLopHocSinhVien.PhutKetThuc);
                    switch (CaLopHocSinhVien.Status)
                    {
                        case 1:
                            strHtml += string.Format("<td style='width: 10%;text-align: center;'>{0}</td>", "Chưa xác thực");
                            strHtml += string.Format("<td style='text-align: center;'><a href='javascript:xacthuc({0});'>Xác thực ngay</a></td>", CaLopHocSinhVien.Ca_LopHoc_SinhVienID);
                            strHtml += string.Format("<td style='text-align: center;'>---</td>");
                            break;
                        case 2:
                            strHtml += string.Format("<td style='width: 10%;text-align: center;'>{0}</td>", "Xác thực");
                            strHtml += string.Format("<td style='text-align: center;'><a href='javascript:huyxacthuc({0});'>Huỷ xác thực ngay</a></td>", CaLopHocSinhVien.Ca_LopHoc_SinhVienID);
                            strHtml += string.Format("<td style='text-align: center;'><a href='/tabid/2129/default.aspx?calophoc={0}&&calophocsinhvien={1}'>Vào học</a></td>", CaLopHocSinhVien.Ca_LopHocID, CaLopHocSinhVien.Ca_LopHoc_SinhVienID);
                            break;
                        default: break;
                    }

                    strHtml += "</tr>";
                }
                strHtml += "</table>";
                #endregion
                divContent.InnerHtml = strHtml;
            }
      
        }
    }
}