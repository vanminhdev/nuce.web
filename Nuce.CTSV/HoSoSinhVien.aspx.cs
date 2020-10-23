using nuce.web.data;
using System;
using System.Data;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class HoSoSinhVien : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = "select * from AS_Academy_Student where Status<>4 and ID=" + m_SinhVien.SinhVienID;
                DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    // Xử lý dữ liệu sinh viên
                    string Email = dt.Rows[0].IsNull("Email") ? "" : dt.Rows[0]["Email"].ToString();
                    string Mobile = dt.Rows[0].IsNull("Mobile") ? "" : dt.Rows[0]["Mobile"].ToString();
                    string NgaySinh = dt.Rows[0].IsNull("NgaySinh") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgaySinh"].ToString()).ToString("dd/MM/yyyy");
                    string BirthPlace = dt.Rows[0].IsNull("BirthPlace") ? "" : dt.Rows[0]["BirthPlace"].ToString();
                    string DanToc = dt.Rows[0].IsNull("DanToc") ? "" : dt.Rows[0]["DanToc"].ToString();
                    string TonGiao = dt.Rows[0].IsNull("TonGiao") ? "" : dt.Rows[0]["TonGiao"].ToString();
                    string HKTT_SoNha = dt.Rows[0].IsNull("HKTT_SoNha") ? "" : dt.Rows[0]["HKTT_SoNha"].ToString();
                    string HKTT_Pho = dt.Rows[0].IsNull("HKTT_Pho") ? "" : dt.Rows[0]["HKTT_Pho"].ToString();
                    string HKTT_Phuong = dt.Rows[0].IsNull("HKTT_Phuong") ? "" : dt.Rows[0]["HKTT_Phuong"].ToString();
                    string HKTT_Quan = dt.Rows[0].IsNull("HKTT_Quan") ? "" : dt.Rows[0]["HKTT_Quan"].ToString();
                    string HKTT_Tinh = dt.Rows[0].IsNull("HKTT_Tinh") ? "" : dt.Rows[0]["HKTT_Tinh"].ToString();
                    string CMT = dt.Rows[0].IsNull("CMT") ? "" : dt.Rows[0]["CMT"].ToString();
                    string CMT_NoiCap = dt.Rows[0].IsNull("CMT_NoiCap") ? "" : dt.Rows[0]["CMT_NoiCap"].ToString();
                    string CMT_NgayCap = dt.Rows[0].IsNull("CMT_NgayCap") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["CMT_NgayCap"].ToString()).ToString("dd/MM/yyyy");
                    string NamTotNghiepPTTH = dt.Rows[0].IsNull("NamTotNghiepPTTH") ? "" : dt.Rows[0]["NamTotNghiepPTTH"].ToString();
                    string NgayVaoDoan = dt.Rows[0].IsNull("NgayVaoDoan") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgayVaoDoan"].ToString()).ToString("dd/MM/yyyy");
                    string NgayVaoDang = dt.Rows[0].IsNull("NgayVaoDang") ? "1/1/1900" : DateTime.Parse(dt.Rows[0]["NgayVaoDang"].ToString()).ToString("dd/MM/yyyy");
                    string DiemThiPTTH = dt.Rows[0].IsNull("DiemThiPTTH") ? "" : dt.Rows[0]["DiemThiPTTH"].ToString();
                    string KhuVucHKTT = dt.Rows[0].IsNull("KhuVucHKTT") ? "" : dt.Rows[0]["KhuVucHKTT"].ToString();
                    string DoiTuongUuTien = dt.Rows[0].IsNull("DoiTuongUuTien") ? "" : dt.Rows[0]["DoiTuongUuTien"].ToString();
                    bool DaTungLamCanBoLop = dt.Rows[0].IsNull("DaTungLamCanBoLop") ? false : bool.Parse(dt.Rows[0]["DaTungLamCanBoLop"].ToString());
                    bool DaTungLamCanBoDoan = dt.Rows[0].IsNull("DaTungLamCanBoLop") ? false : bool.Parse(dt.Rows[0]["DaTungLamCanBoDoan"].ToString());
                    bool DaThamGiaDoiTuyenThiHSG = dt.Rows[0].IsNull("DaThamGiaDoiTuyenThiHSG") ? false : bool.Parse(dt.Rows[0]["DaThamGiaDoiTuyenThiHSG"].ToString());
                    string BaoTin_DiaChi = dt.Rows[0].IsNull("BaoTin_DiaChi") ? "" : dt.Rows[0]["BaoTin_DiaChi"].ToString();
                    string BaoTin_HoVaTen = dt.Rows[0].IsNull("BaoTin_HoVaTen") ? "" : dt.Rows[0]["BaoTin_HoVaTen"].ToString();
                    string BaoTin_DiaChiNguoiNhan = dt.Rows[0].IsNull("BaoTin_DiaChiNguoiNhan") ? "" : dt.Rows[0]["BaoTin_DiaChiNguoiNhan"].ToString();
                    string BaoTin_SoDienThoai = dt.Rows[0].IsNull("BaoTin_SoDienThoai") ? "" : dt.Rows[0]["BaoTin_SoDienThoai"].ToString();
                    string BaoTin_Email = dt.Rows[0].IsNull("BaoTin_Email") ? "" : dt.Rows[0]["BaoTin_Email"].ToString();
                    bool LaNoiTru = dt.Rows[0].IsNull("LaNoiTru") ? false : bool.Parse(dt.Rows[0]["LaNoiTru"].ToString());
                    string DiaChiCuThe = dt.Rows[0].IsNull("DiaChiCuThe") ? "" : dt.Rows[0]["DiaChiCuThe"].ToString();

                    string File1 = dt.Rows[0].IsNull("File1") ? "" : dt.Rows[0]["File1"].ToString();
                    string File2 = dt.Rows[0].IsNull("File2") ? "" : dt.Rows[0]["File2"].ToString();

                    hovaten.InnerHtml = m_SinhVien.Ho.ToUpper();
                    masv.InnerHtml = m_SinhVien.MaSV;
                    namsinhsv.InnerHtml = NgaySinh;
                    noisinhsv.InnerHtml = BirthPlace;
                    dantocsv.InnerHtml = DanToc;
                    dantocsv.InnerHtml = TonGiao;
                    hktt_sonhasv.InnerHtml = HKTT_SoNha;
                    hktt_phothonsv.InnerHtml = HKTT_Pho;
                    hktt_phuongxasv.InnerHtml = HKTT_Phuong;
                    hktt_quanhuyensv.InnerHtml = HKTT_Quan;
                    hktt_thanhphosv.InnerHtml = HKTT_Tinh;
                    socmnd.InnerHtml = CMT;
                    cmndngaycap.InnerHtml = CMT_NgayCap;
                    cmndnoicap.InnerHtml = CMT_NoiCap;
                    ngayvaodoansv.InnerHtml = NgayVaoDoan;
                    ngayvaodangsv.InnerHtml = NgayVaoDang;
                    namtotnghiepthptsv.InnerHtml = NamTotNghiepPTTH;
                    diemthithptqgsv.InnerHtml = DiemThiPTTH;
                    hktttkvsv.InnerHtml = KhuVucHKTT;
                    dtutsv.InnerHtml = DoiTuongUuTien;
                    datunglamcanbodoan.InnerHtml = DaTungLamCanBoDoan ? "Có" : "Không";
                    datunglamcanbolop.InnerHtml = DaTungLamCanBoLop ? "Có" : "Không";
                    dathamgiadoituyenthihsg.InnerHtml = DaThamGiaDoiTuyenThiHSG ? "Có" : "Không";
                    //baotindiachi.InnerHtml = BaoTin_DiaChi;
                    baotindiachinguoinhansv.InnerHtml = BaoTin_DiaChiNguoiNhan;
                    baotinhovatensv.InnerHtml = BaoTin_HoVaTen;
                    baotinsodienthoainguoinhansv.InnerHtml = BaoTin_SoDienThoai;
                    mail.InnerHtml = Email;
                    sdt.InnerHtml = Mobile;
                    coonoitrukhongsv.InnerHtml= LaNoiTru ? "Có" : "Không";
                    diachicuthesv.InnerHtml = DiaChiCuThe;
                    //if (!File1.Trim().Equals(""))
                    //{
                    //    imgAnh.Src = File1;
                    //}
                    //else
                    //    imgAnh.Src = "/Data/images/noimage.png";

                    sql = "select * from AS_Academy_Student_QuaTrinhHocTap where StudentID=" + m_SinhVien.SinhVienID +" order by Count";
                    DataTable dtQTHT = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                    string strQTHT_thoigian = "<div class=\"col-12 fw-700\">Thời gian</div>";
                    string strQTHT_truong = "<div class=\"col-12 fw-700\">Trường</div>";
                    if (dtQTHT.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtQTHT.Rows.Count; i++)
                        {
                            string thoiGian = Nuce_CTSV.firstOrDefault(dtQTHT.Rows, i, "ThoiGian");
                            string tenTruong = Nuce_CTSV.firstOrDefault(dtQTHT.Rows, i, "TenTruong");
                            strQTHT_thoigian += $"<div class=\"col-12\">{thoiGian}</div>";
                            strQTHT_truong += $"<div class=\"col-12\">{tenTruong}</div>";
                        }
                    }
                    qtht_thoigian.InnerHtml = strQTHT_thoigian;
                    qtht_truong.InnerHtml = strQTHT_truong;

                    sql = "select * from AS_Academy_Student_ThiHSG where StudentID=" + m_SinhVien.SinhVienID + " order by Count";
                    DataTable dtThiHSG = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                    string strThiHSG_Cap = "<div class=\"fw-700\">Cấp thi:</div>";
                    string strThiHSG_Mon = "<div class=\"fw-700\">Môn thi:</div>";
                    string strThiHSG_Giai = "<div class=\"fw-700\">Đạt giải:</div>";
                    if (dtThiHSG.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtThiHSG.Rows.Count; i++)
                        {
                            string capThi = Nuce_CTSV.firstOrDefault(dtThiHSG.Rows, i, "CapThi");
                            string monThi = Nuce_CTSV.firstOrDefault(dtThiHSG.Rows, i, "MonThi");
                            string datGiai = Nuce_CTSV.firstOrDefault(dtThiHSG.Rows, i, "DatGiai");

                            strThiHSG_Cap += $"<div>{capThi}</div>";
                            strThiHSG_Mon += $"<div>{monThi}</div>";
                            strThiHSG_Giai += $"<div>{datGiai}</div>";
                        }
                    }
                    hsg_capthi.InnerHtml = strThiHSG_Cap;
                    hsg_monthi.InnerHtml = strThiHSG_Mon;
                    hsg_giai.InnerHtml = strThiHSG_Giai;

                    sql = "select * from AS_Academy_Student_GiaDinh where StudentID=" + m_SinhVien.SinhVienID + " order by Count";
                    DataTable dtTTGD = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                    if (dtTTGD.Rows.Count > 0)
                    {
                        string strTTGD = "";
                        string strTemplate = "<div class=\"row mb-3\"><div class=\"col-12 col-md-5 fw-700\">[name]:</div><div class=\"col-12 col-md-7\">[value]</div></div>";
                        for (int i = 0; i < dtTTGD.Rows.Count; i++)
                        {
                            string hoten = Nuce_CTSV.firstOrDefault(dtTTGD.Rows, i, "HoVaTen");
                            string moiQuanHe = Nuce_CTSV.firstOrDefault(dtTTGD.Rows, i, "MoiQuanHe");
                            string namsinh = Nuce_CTSV.firstOrDefault(dtTTGD.Rows, i, "NamSinh");
                            string quoctich = Nuce_CTSV.firstOrDefault(dtTTGD.Rows, i, "QuocTich");
                            string dantoc = Nuce_CTSV.firstOrDefault(dtTTGD.Rows, i, "DanToc");
                            string tongiao = Nuce_CTSV.firstOrDefault(dtTTGD.Rows, i, "TonGiao");
                            string nghenghiep = Nuce_CTSV.firstOrDefault(dtTTGD.Rows, i, "NgheNghiep");
                            string noicongtac = Nuce_CTSV.firstOrDefault(dtTTGD.Rows, i, "NoiCongTac");

                            strTTGD += strTemplate.Replace("[name]", $"Họ và tên {moiQuanHe}").Replace("[value]", hoten);
                            strTTGD += strTemplate.Replace("[name]", $"Năm sinh").Replace("[value]", namsinh);
                            strTTGD += strTemplate.Replace("[name]", $"Quốc tịch").Replace("[value]", quoctich);
                            strTTGD += strTemplate.Replace("[name]", $"Dân tộc").Replace("[value]", dantoc);
                            strTTGD += strTemplate.Replace("[name]", $"Tôn giáo").Replace("[value]", tongiao);
                            strTTGD += strTemplate.Replace("[name]", $"Nghề nghiệp").Replace("[value]", nghenghiep);
                            strTTGD += strTemplate.Replace("[name]", $"Nơi công tác").Replace("[value]", noicongtac);
                            if (i < dtTTGD.Rows.Count - 1)
                            {
                                strTTGD += "<hr class=\"profile-line mt-2 mb-2\" />";
                            }
                        }

                        divGiaDinh.InnerHtml = strTTGD;
                    }
                }
            }
        }
    }
}