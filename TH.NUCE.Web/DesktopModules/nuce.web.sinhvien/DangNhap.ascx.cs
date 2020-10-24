using DotNetNuke.Entities.Modules;
using System;
using System.Collections.Generic;
using System.Data;

namespace nuce.web.sinhvien
{
    public partial class DangNhap : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                tdAnnounce.InnerHtml = "SINH VIÊN - ĐĂNG NHẬP";
            }
        }

        protected void btnDangNhap_Click(object sender, EventArgs e)
        {
            string strTen = txtTen.Text.Trim();
            string strMatKhau = txtMauKhau.Text.Trim();
            if (strTen.Equals(""))
            {
                tdAnnounce.InnerHtml = "Không được để tên trắng";
                return;
            }
            if (strMatKhau.Equals(""))
            {
                tdAnnounce.InnerHtml = "Không được mật khẩu trắng";
                return;
            }
            DataTable dtData = data.dnn_NuceCommon_SinhVien.dangnhap(strTen, strMatKhau);
            if (dtData.Rows.Count > 0)
            {
                model.SinhVien SinhVien = new model.SinhVien();
                SinhVien.Ho = dtData.Rows[0]["Ho"].ToString();
                SinhVien.Ten = dtData.Rows[0]["Ten"].ToString();
                SinhVien.MaSV = dtData.Rows[0]["MaSV"].ToString();
                SinhVien.TrangThai = int.Parse(dtData.Rows[0]["Status"].ToString());
                SinhVien.SinhVienID = int.Parse(dtData.Rows[0]["SinhVienID"].ToString());
                Session[Utils.session_sinhvien] = SinhVien;

                #region KiThiLopHocSinhVien
                DataTable dtKiThiLopHocSinhVien = data.dnn_NuceThi_KiThi_LopHoc_SinhVien.getBySinhVien(SinhVien.SinhVienID);
                if(dtKiThiLopHocSinhVien.Rows.Count>0)
                {
                    int iLenghKiThiLopHocSinhVien = dtKiThiLopHocSinhVien.Rows.Count;
                    Dictionary<int, model.KiThiLopHocSinhVien> KiThiLopHocSinhViens = new Dictionary<int, model.KiThiLopHocSinhVien>();
                    for (int i=0;i<iLenghKiThiLopHocSinhVien;i++)
                    {
                        model.KiThiLopHocSinhVien KiThiLopHocSinhVien = new model.KiThiLopHocSinhVien();
                        KiThiLopHocSinhVien.BoDeID = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["BoDeID"].ToString());
                        KiThiLopHocSinhVien.DeThiID= int.Parse(dtKiThiLopHocSinhVien.Rows[i]["DeThiID"].ToString());
                        KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien= int.Parse(dtKiThiLopHocSinhVien.Rows[i]["KiThi_LopHoc_SinhVienID"].ToString());
                        KiThiLopHocSinhVien.Status= int.Parse(dtKiThiLopHocSinhVien.Rows[i]["Status"].ToString());
                        KiThiLopHocSinhVien.LoaiKiThi = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["LoaiKiThi"].ToString());
                        KiThiLopHocSinhVien.TenBlockHoc = dtKiThiLopHocSinhVien.Rows[i]["TenBlockHoc"].ToString();
                        KiThiLopHocSinhVien.TenKiThi = dtKiThiLopHocSinhVien.Rows[i]["TenKiThi"].ToString();
                        KiThiLopHocSinhVien.TenMonHoc= dtKiThiLopHocSinhVien.Rows[i]["TenMonHoc"].ToString();
                        KiThiLopHocSinhVien.NoiDungDeThi= dtKiThiLopHocSinhVien.Rows[i]["NoiDungDeThi"].ToString();
                        KiThiLopHocSinhVien.DapAn= dtKiThiLopHocSinhVien.Rows[i]["DapAn"].ToString();
                        KiThiLopHocSinhVien.Diem = float.Parse( dtKiThiLopHocSinhVien.Rows[i]["Diem"].ToString());
                        KiThiLopHocSinhVien.BaiLam= dtKiThiLopHocSinhVien.Rows[i]["BaiLam"].ToString();
                        KiThiLopHocSinhVien.MaDe = dtKiThiLopHocSinhVien.Rows[i].IsNull("MaDe") ? "" : dtKiThiLopHocSinhVien.Rows[i]["MaDe"].ToString();
                        KiThiLopHocSinhVien.NgayGioBatDau = dtKiThiLopHocSinhVien.Rows[i].IsNull("NgayGioBatDau") ? DateTime.Now : DateTime.Parse(dtKiThiLopHocSinhVien.Rows[i]["NgayGioBatDau"].ToString());
                        if (KiThiLopHocSinhVien.Status.Equals(5)|| KiThiLopHocSinhVien.Status.Equals(4))
                        {
                            KiThiLopHocSinhVien.TongThoiGianConLai = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["TongThoiGianConLai"].ToString());
                            KiThiLopHocSinhVien.TongThoiGianThi = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["TongThoiGianThi"].ToString());
                            if(KiThiLopHocSinhVien.Status.Equals(4))
                                KiThiLopHocSinhVien.Mota=string.Format("<div style='width: 80%;text-align: center;font-weight: bold;font-size: 20px;color: red;padding-top: 20px;'>Bài thi được {0:N2} điểm</div>", float.Parse(dtKiThiLopHocSinhVien.Rows[i]["Diem"].ToString()));
                            //KiThiLopHocSinhVien.Mota = string.Format("Bài thi được {0:N2} điểm", float.Parse(dtKiThiLopHocSinhVien.Rows[i]["Diem"].ToString()));
                        }
                        else
                        {
                            KiThiLopHocSinhVien.TongThoiGianConLai = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["ThoiGianThi"].ToString())*60;
                            KiThiLopHocSinhVien.TongThoiGianThi = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["ThoiGianThi"].ToString());
                        }
                        KiThiLopHocSinhViens.Add(KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien,KiThiLopHocSinhVien);
                    }
                    Session[Utils.session_kithi_lophoc_sinhvien] = KiThiLopHocSinhViens;
                }
                #endregion
                #region CaLopHocSinhVien
                DataTable dtCaLopHocSinhVien = data.dnn_NuceQLHPM_Ca_LopHoc_SinhVien.getBySinhVien(SinhVien.SinhVienID);
                if (dtCaLopHocSinhVien.Rows.Count > 0)
                {
                    int iLenghCaLopHocSinhVien = dtCaLopHocSinhVien.Rows.Count;
                    Dictionary<int, model.CaLopHocSinhVien> CaLopHocSinhViens = new Dictionary<int, model.CaLopHocSinhVien>();
                    for (int i = 0; i < iLenghCaLopHocSinhVien; i++)
                    {
                        model.CaLopHocSinhVien CaLopHocSinhVien = new model.CaLopHocSinhVien();
                        CaLopHocSinhVien.Ca_LopHoc_SinhVienID = int.Parse(dtCaLopHocSinhVien.Rows[i]["Ca_LopHoc_SinhVienID"].ToString());
                        CaLopHocSinhVien.Ca_LopHocID = int.Parse(dtCaLopHocSinhVien.Rows[i]["Ca_LopHocID"].ToString());
                        CaLopHocSinhVien.SinhVienID = int.Parse(dtCaLopHocSinhVien.Rows[i]["SinhVienID"].ToString());
                        CaLopHocSinhVien.Mac = dtCaLopHocSinhVien.Rows[i]["MacAddress"].ToString();
                        CaLopHocSinhVien.Type = 1;
                        CaLopHocSinhVien.Status = 1;
                        CaLopHocSinhVien.TenMonHoc = dtCaLopHocSinhVien.Rows[i]["TenMonHoc"].ToString();
                        CaLopHocSinhVien.MaMonHoc = dtCaLopHocSinhVien.Rows[i]["MaMonHoc"].ToString();
                        CaLopHocSinhVien.TenCa = dtCaLopHocSinhVien.Rows[i]["TenCa"].ToString();
                        CaLopHocSinhVien.Ngay = DateTime.Parse(dtCaLopHocSinhVien.Rows[i]["Ngay"].ToString());
                        CaLopHocSinhVien.GioBatDau = int.Parse(dtCaLopHocSinhVien.Rows[i]["GioBatDau"].ToString());
                        CaLopHocSinhVien.GioKetThuc = int.Parse(dtCaLopHocSinhVien.Rows[i]["GioKetThuc"].ToString());
                        CaLopHocSinhVien.PhutBatDau = int.Parse(dtCaLopHocSinhVien.Rows[i]["PhutBatDau"].ToString());
                        CaLopHocSinhVien.PhutKetThuc = int.Parse(dtCaLopHocSinhVien.Rows[i]["PhutKetThuc"].ToString());
                        CaLopHocSinhViens.Add(CaLopHocSinhVien.Ca_LopHoc_SinhVienID, CaLopHocSinhVien);
                    }
                    Session[Utils.session_ca_lophoc_sinhvien] = CaLopHocSinhViens;
                }
                #endregion
                Response.Redirect(string.Format("/tabid/{0}/default.aspx", Utils.tab_trangchu_sinhvien));
                tdAnnounce.InnerHtml = "Đăng nhập thành công";
            }
            else
                tdAnnounce.InnerHtml = "Đăng nhập thất bại";
        }
    }
}