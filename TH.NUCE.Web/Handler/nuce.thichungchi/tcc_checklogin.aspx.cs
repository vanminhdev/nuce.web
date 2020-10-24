using nuce.web.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace nuce.web.sinhvien
{
    public partial class tcc_checklogin : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                string strMa =Request.QueryString["ma"].ToString();
                string strPass = Request.QueryString["pass"].ToString();
                string sql = string.Format(@"select * from [dbo].[NUCE_ThiChungChi_NguoiThi] where Ma=@Param0 and MatKhau=@Param1");
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = new SqlParameter("@Param0", strMa);
                sqlParams[1] = new SqlParameter("@Param1", strPass);
                DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_ThiChungChi.ConnectionString, CommandType.Text, sql,sqlParams).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    model.SinhVien SinhVien = new model.SinhVien();
                    SinhVien.Ho = dt.Rows[0]["Ho"].ToString();
                    SinhVien.Ten = dt.Rows[0]["Ten"].ToString();
                    SinhVien.MaSV = dt.Rows[0]["Ma"].ToString();
                    SinhVien.TrangThai = int.Parse(dt.Rows[0]["Status"].ToString());
                    SinhVien.SinhVienID = int.Parse(dt.Rows[0]["ID"].ToString());
                    SinhVien.CMT= dt.Rows[0]["CMT"].ToString();
                    Session[Utils.session_sinhvien] = SinhVien;
                    #region KiThiLopHocSinhVien
                    DataTable dtKiThiLopHocSinhVien = data.dnn_NuceThi_KiThi_LopHoc_SinhVien.getBySinhVien(SinhVien.SinhVienID);
                    if (dtKiThiLopHocSinhVien.Rows.Count > 0)
                    {
                        int iLenghKiThiLopHocSinhVien = dtKiThiLopHocSinhVien.Rows.Count;
                        Dictionary<int, model.KiThiLopHocSinhVien> KiThiLopHocSinhViens = new Dictionary<int, model.KiThiLopHocSinhVien>();
                        for (int i = 0; i < iLenghKiThiLopHocSinhVien; i++)
                        {
                            model.KiThiLopHocSinhVien KiThiLopHocSinhVien = new model.KiThiLopHocSinhVien();
                            KiThiLopHocSinhVien.BoDeID = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["BoDeID"].ToString());
                            KiThiLopHocSinhVien.DeThiID = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["DeThiID"].ToString());
                            KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["KiThi_LopHoc_SinhVienID"].ToString());
                            KiThiLopHocSinhVien.Status = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["Status"].ToString());
                            KiThiLopHocSinhVien.StatusKiThi = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["StatusKiThi"].ToString());
                            //KiThiLopHocSinhVien.LoaiKiThi = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["LoaiKiThi"].ToString());
                            KiThiLopHocSinhVien.TenBlockHoc = dtKiThiLopHocSinhVien.Rows[i]["PhongThi"].ToString();
                            KiThiLopHocSinhVien.TenKiThi = dtKiThiLopHocSinhVien.Rows[i]["TenKiThi"].ToString();
                            KiThiLopHocSinhVien.TenMonHoc = dtKiThiLopHocSinhVien.Rows[i]["TenMonHoc"].ToString();
                            KiThiLopHocSinhVien.NoiDungDeThi = dtKiThiLopHocSinhVien.Rows[i]["NoiDungDeThi"].ToString();
                            KiThiLopHocSinhVien.DapAn = dtKiThiLopHocSinhVien.Rows[i]["DapAn"].ToString();
                            KiThiLopHocSinhVien.Diem = float.Parse(dtKiThiLopHocSinhVien.Rows[i]["Diem"].ToString());
                            KiThiLopHocSinhVien.BaiLam = dtKiThiLopHocSinhVien.Rows[i]["BaiLam"].ToString();
                            KiThiLopHocSinhVien.MaDe = dtKiThiLopHocSinhVien.Rows[i].IsNull("MaDe") ? "" : dtKiThiLopHocSinhVien.Rows[i]["MaDe"].ToString();
                            KiThiLopHocSinhVien.NgayGioBatDau = dtKiThiLopHocSinhVien.Rows[i].IsNull("NgayGioBatDau") ? DateTime.Now : DateTime.Parse(dtKiThiLopHocSinhVien.Rows[i]["NgayGioBatDau"].ToString());
                            if (KiThiLopHocSinhVien.Status.Equals(5) || KiThiLopHocSinhVien.Status.Equals(4))
                            {
                                KiThiLopHocSinhVien.TongThoiGianConLai = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["TongThoiGianConLai"].ToString());
                                KiThiLopHocSinhVien.TongThoiGianThi = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["ThoiGianThi"].ToString());
                                if (KiThiLopHocSinhVien.Status.Equals(4))
                                    KiThiLopHocSinhVien.Mota = string.Format("<div style='text-align: center;font-weight: bold;font-size: 20px;color: red;padding-top: 20px;'>Bài thi được {0:N2} điểm</div>", float.Parse(dtKiThiLopHocSinhVien.Rows[i]["Diem"].ToString()));
                                //KiThiLopHocSinhVien.Mota = string.Format("Bài thi được {0:N2} điểm", float.Parse(dtKiThiLopHocSinhVien.Rows[i]["Diem"].ToString()));
                            }
                            else
                            {
                                KiThiLopHocSinhVien.TongThoiGianConLai = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["TongThoiGianConLai"].ToString());
                                KiThiLopHocSinhVien.TongThoiGianThi = int.Parse(dtKiThiLopHocSinhVien.Rows[i]["ThoiGianThi"].ToString());
                            }
                            KiThiLopHocSinhViens.Add(KiThiLopHocSinhVien.KiThi_LopHoc_SinhVien, KiThiLopHocSinhVien);
                        }
                        Session[Utils.session_kithi_lophoc_sinhvien] = KiThiLopHocSinhViens;
                    }
                    #endregion
                    strData = "1";
                }
            }
            catch (Exception ex)
            {
                strData = ex.Message;
            }
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write(strData);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}