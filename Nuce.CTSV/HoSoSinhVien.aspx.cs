using Newtonsoft.Json;
using nuce.web.data;
using System;
using System.Data;
using System.Globalization;
using System.Net.Http;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class HoSoSinhVien : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string sql = "select * from AS_Academy_Student where Status<>4 and ID=" + m_SinhVien.SinhVienID;

                //DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                var res = await CustomizeHttp.SendRequest(Request, Response, HttpMethod.Get, $"{ApiModels.ApiEndPoint.GetFullStudent}/{m_SinhVien.MaSV}", "");
                ApiModels.FullStudentModel full = null;
                
                if (res.IsSuccessStatusCode)
                {
                    full = await CustomizeHttp.DeserializeAsync<ApiModels.FullStudentModel>(res.Content);
                }

                if (full != null && full.Student != null)
                {
                    ApiModels.StudentModel student = full.Student;

                    // Xử lý dữ liệu sinh viên
                    #region sinh vien
                    string NgaySinh = "1/1/1900";
                    if (student.DateOfBirth != null)
                    {
                        try
                        {
                            var tmpNgaySinh = DateTime.ParseExact(student.DateOfBirth, "dd/MM/yy", CultureInfo.InvariantCulture);
                            NgaySinh = tmpNgaySinh.ToString("dd/MM/yyyy");
                        }
                        catch (Exception)
                        {
                            NgaySinh = DateTime.Parse(student.DateOfBirth).ToString("dd/MM/yyyy");
                        }
                    }

                    string CMT_NgayCap = student.CmtNgayCap != null ? student.CmtNgayCap?.ToString("dd/MM/yyyy") : "";
                    string NgayVaoDoan = student.NgayVaoDoan != null ? student.NgayVaoDoan?.ToString("dd/MM/yyyy") : "";
                    string NgayVaoDang = student.NgayVaoDang != null ? student.NgayVaoDang?.ToString("dd/MM/yyyy") : "";


                    hovaten.InnerHtml = m_SinhVien.Ho.ToUpper();
                    masv.InnerHtml = m_SinhVien.MaSV;
                    namsinhsv.InnerHtml = NgaySinh;
                    noisinhsv.InnerHtml = (student.BirthPlace ?? "").Trim();
                    dantocsv.InnerHtml = (student.DanToc ?? "").Trim();
                    tongiaosv.InnerHtml = (student.TonGiao ?? "").Trim();
                    hktt_sonhasv.InnerHtml = (student.HkttSoNha ?? "").Trim();
                    hktt_phothonsv.InnerHtml = (student.HkttPho ?? "").Trim();
                    hktt_phuongxasv.InnerHtml = (student.HkttPhuong ?? "").Trim();
                    hktt_quanhuyensv.InnerHtml = (student.HkttQuan ?? "").Trim();
                    hktt_thanhphosv.InnerHtml = (student.HkttTinh ?? "").Trim();
                    socmnd.InnerHtml = (student.Cmt ?? "").Trim();
                    cmndngaycap.InnerHtml = CMT_NgayCap;
                    cmndnoicap.InnerHtml = (student.CmtNoiCap ?? "").Trim();
                    ngayvaodoansv.InnerHtml = NgayVaoDoan;
                    ngayvaodangsv.InnerHtml = NgayVaoDang;
                    namtotnghiepthptsv.InnerHtml = (student.NamTotNghiepPtth ?? "").Trim();
                    diemthithptqgsv.InnerHtml = (student.DiemThiPtth ?? "").Trim();
                    hktttkvsv.InnerHtml = (student.KhuVucHktt ?? "").Trim();
                    dtutsv.InnerHtml = (student.DoiTuongUuTien ?? "").Trim();
                    datunglamcanbodoan.InnerHtml = (student.DaTungLamCanBoDoan ?? false) ? "Có" : "Không";
                    datunglamcanbolop.InnerHtml = (student.DaTungLamCanBoLop ?? false) ? "Có" : "Không";
                    dathamgiadoituyenthihsg.InnerHtml = (student.DaThamGiaDoiTuyenThiHsg ?? false) ? "Có" : "Không";
                    //baotindiachi.InnerHtml = BaoTin_DiaChi;
                    baotindiachi.InnerHtml = (student.BaoTinDiaChi ?? "").Trim();
                    baotindiachinguoinhansv.InnerHtml = (student.BaoTinDiaChiNguoiNhan ?? "").Trim();
                    baotinhovatensv.InnerHtml = (student.BaoTinHoVaTen ?? "").Trim();
                    baotinsodienthoainguoinhansv.InnerHtml = (student.BaoTinSoDienThoai ?? "").Trim();
                    mail.InnerHtml = (student.Email ?? "").Trim();
                    sdt.InnerHtml = (student.Mobile ?? "").Trim();
                    coonoitrukhongsv.InnerHtml = (student.LaNoiTru ?? false) ? "Có" : "Không";
                    diachicuthesv.InnerHtml = (student.DiaChiCuThe ?? "").Trim();
                    baotindiachichuyenphatnhanh.InnerHtml = (student.BaoTinDiaChiNhanChuyenPhatNhanh ?? "").Trim();
                    //if (!File1.Trim().Equals(""))
                    //{
                    //    imgAnh.Src = File1;
                    //}
                    //else
                    //    imgAnh.Src = "/Data/images/noimage.png";
                    #endregion
                    #region qua trinh hoc tap
                    var qtht = full.QuaTrinhHoc;
                    if (qtht != null && qtht.Count > 0)
                    {
                        string strQTHT_thoigian = "<div class=\"col-12 fw-700\">Thời gian</div>";
                        string strQTHT_truong = "<div class=\"col-12 fw-700\">Trường</div>";

                        for (int i = 0; i < qtht.Count; i++)
                        {
                            string thoiGian = (qtht[i].ThoiGian ?? "").Trim();
                            string tenTruong = (qtht[i].TenTruong ?? "").Trim();
                            strQTHT_thoigian += $"<div class=\"col-12\">{thoiGian}</div>";
                            strQTHT_truong += $"<div class=\"col-12\">{tenTruong}</div>";
                        }

                        qtht_thoigian.InnerHtml = strQTHT_thoigian;
                        qtht_truong.InnerHtml = strQTHT_truong;
                    }
                    #endregion
                    #region thi hsg
                    var thiHsg = full.ThiHSG;
                    if (thiHsg != null && thiHsg.Count > 0)
                    {
                        string strThiHSG_Cap = "<div class=\"fw-700\">Cấp thi:</div>";
                        string strThiHSG_Mon = "<div class=\"fw-700\">Môn thi:</div>";
                        string strThiHSG_Giai = "<div class=\"fw-700\">Đạt giải:</div>";

                        for (int i = 0; i < thiHsg.Count; i++)
                        {
                            string capThi = thiHsg[i].CapThi ?? "";
                            string monThi = thiHsg[i].MonThi ?? "";
                            string datGiai = thiHsg[i].DatGiai ?? "";

                            strThiHSG_Cap += $"<div>{capThi}</div>";
                            strThiHSG_Mon += $"<div>{monThi}</div>";
                            strThiHSG_Giai += $"<div>{datGiai}</div>";
                        }
                        
                        hsg_capthi.InnerHtml = strThiHSG_Cap;
                        hsg_monthi.InnerHtml = strThiHSG_Mon;
                        hsg_giai.InnerHtml = strThiHSG_Giai;

                    }
                    #endregion
                    #region gia dinh
                    var giaDinh = full.GiaDinh;
                    if (giaDinh != null && giaDinh.Count > 0)
                    {
                        string strTTGD = "";
                        string strTemplate = "<div class=\"row mb-3\"><div class=\"col-12 col-md-5 fw-700\">[name]:</div><div class=\"col-12 col-md-7\">[value]</div></div>";
                        for (int i = 0; i < giaDinh.Count; i++)
                        {
                            var giaDinhRow = giaDinh[i];

                            string hoten = giaDinhRow.HoVaTen ?? "";
                            string moiQuanHe = giaDinhRow.MoiQuanHe ?? "";
                            string namsinh = giaDinhRow.NamSinh ?? "";
                            string quoctich = giaDinhRow.QuocTich ?? "";
                            string dantoc = giaDinhRow.DanToc ?? "";
                            string tongiao = giaDinhRow.TonGiao ?? "";
                            string nghenghiep = giaDinhRow.NgheNghiep ?? "";
                            string noicongtac = giaDinhRow.NoiCongTac ?? "";

                            strTTGD += strTemplate.Replace("[name]", $"Họ và tên {moiQuanHe}").Replace("[value]", hoten);
                            strTTGD += strTemplate.Replace("[name]", $"Năm sinh").Replace("[value]", namsinh);
                            strTTGD += strTemplate.Replace("[name]", $"Quốc tịch").Replace("[value]", quoctich);
                            strTTGD += strTemplate.Replace("[name]", $"Dân tộc").Replace("[value]", dantoc);
                            strTTGD += strTemplate.Replace("[name]", $"Tôn giáo").Replace("[value]", tongiao);
                            strTTGD += strTemplate.Replace("[name]", $"Nghề nghiệp").Replace("[value]", nghenghiep);
                            strTTGD += strTemplate.Replace("[name]", $"Nơi công tác").Replace("[value]", noicongtac);
                            if (i < giaDinh.Count - 1)
                            {
                                strTTGD += "<hr class=\"profile-line mt-2 mb-2\" />";
                            }
                        }

                        divGiaDinh.InnerHtml = strTTGD;
                    }
                    #endregion
                }
            }
        }
    }
}