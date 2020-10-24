using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Globalization;

namespace nuce.web.qlpm
{
    public partial class New_DangKiLich_For_GV : PortalModuleBase
    {
        //New 
        //Edit
        //Delete
        static string Type;
        static int ID;
        static int PhongHocID;
        static int CaHocID;
        static long NgayDangKi;
        static int Tuan;
        static DataTable dtPhongHoc;
        protected override void OnInit(EventArgs e)
        {
            Type = Request.QueryString["type"];
            PhongHocID = int.Parse(Request.QueryString["phong"]);
            Tuan = int.Parse(Request.QueryString["tuan"]);
            if (Type.Equals("new"))
            {
                divAnnouce.InnerHtml = "Thêm mới lịch";
                CaHocID = int.Parse(Request.QueryString["ca"]);
                NgayDangKi = long.Parse(Request.QueryString["ngay"]);
            }
            else
            {
                divAnnouce.InnerHtml = "Chỉnh sửa lịch";
                ID = int.Parse(Request.QueryString["id"]);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtPhongHoc = data.dnn_NuceCommon_PhongHoc.getByToaNha(3);
                ddlPhongHoc.DataTextField = "Ten";
                ddlPhongHoc.DataValueField = "PhongHocID";
                ddlPhongHoc.DataSource = dtPhongHoc;
                ddlPhongHoc.DataBind();
                ddlPhongHoc.SelectedValue = PhongHocID.ToString();
                if (Type.Equals("new"))
                {
                    txtCaHoc.Text = CaHocID.ToString();
                    txtNgay.Text = DateTime.FromFileTimeUtc(NgayDangKi).ToString("dd/MM/yyyy");
                    txtMaCB.Text = this.UserInfo.Username;
                    txtHoVaTenCB.Text = this.UserInfo.DisplayName;
                }
                else
                {
                    DataTable dtDangKi = data.dnn_NuceQLPM_LichPhongMay.get(ID);
                    txtMa.Text = dtDangKi.Rows[0]["MADK"].ToString();
                    txtMa.Enabled = false;

                    txtNgay.Text = DateTime.Parse(dtDangKi.Rows[0]["Ngay"].ToString()).ToString("dd/MM/yyyy");
                    txtMaCB.Text = dtDangKi.Rows[0]["MaCB"].ToString();
                    txtHoVaTenCB.Text = dtDangKi.Rows[0]["HoVaTenCB"].ToString();
                    txtLop.Text = dtDangKi.Rows[0]["Lop"].ToString();
                    txtMonHoc.Text = dtDangKi.Rows[0]["MonHoc"].ToString();
                    txtSoSinhVien.Text = dtDangKi.Rows[0]["SoSinhVien"].ToString();
                    txtMoTa.Text = dtDangKi.Rows[0]["MoTa"].ToString();
                    txtCaHoc.Text = dtDangKi.Rows[0]["CaHocID"].ToString();
                }
            }
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (txtLop.Text.Trim().Equals(""))
            {
                divAnnouce.InnerHtml = "Không được để trắng lớp";
                return;
            }
            if (txtMonHoc.Text.Trim().Equals(""))
            {
                divAnnouce.InnerHtml = "Không được để trắng môn học";
                return;
            }
            string strMaDK = txtMa.Text.Trim();
            if (strMaDK.Equals(""))
            {
                strMaDK = new Guid().ToString();
            }
            string strMaCB = txtMaCB.Text.Trim();
            string strHoVaTenCB = txtHoVaTenCB.Text.Trim();
            string strLop = txtLop.Text.Trim();
            string strMonHoc = txtMonHoc.Text.Trim();

            CultureInfo provider = CultureInfo.InvariantCulture;
            provider = new CultureInfo("fr-FR");
            string dtformat = "dd/MM/yyyy";
            DateTime Ngay = DateTime.ParseExact(txtNgay.Text, dtformat, provider);

            string strThu = "2";
            switch (Ngay.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    strThu = "2";
                    break;
                case DayOfWeek.Tuesday:
                    strThu = "3";
                    break;
                case DayOfWeek.Wednesday:
                    strThu = "4";
                    break;
                case DayOfWeek.Thursday:
                    strThu = "5";
                    break;
                case DayOfWeek.Friday:
                    strThu = "6";
                    break;
                case DayOfWeek.Saturday:
                    strThu = "7";
                    break;
                default: break;
            }
            int iCaHoc = int.Parse(txtCaHoc.Text);
            string strTietBatDau = "1";
            string strSoTiet = "3";
            int iBuoiHoc = 1;
            switch (iCaHoc)
            {
                case 2:
                    strTietBatDau = "4";
                    iBuoiHoc = 1;
                    break;
                case 3:
                    strTietBatDau = "7";
                    iBuoiHoc = 2;
                    break;
                case 4:
                    strTietBatDau = "10";
                    iBuoiHoc = 2;
                    break;
                case 5:
                    strTietBatDau = "13";
                    iBuoiHoc = 3;
                    break;
                default:
                    break;
            }
            int iSoSinhVien = 0;
            int.TryParse(txtSoSinhVien.Text.Trim(), out iSoSinhVien);
            string strMoTa = txtMoTa.Text;
            if (Type.Equals("new"))
            {
                if (data.dnn_NuceQLPM_LichPhongMay.Insert_DangKi(this.UserId, strMaDK, strMaCB, strHoVaTenCB, strLop, strMonHoc, strThu, strTietBatDau, strSoTiet
                     , "", PhongHocID, iBuoiHoc, "", Ngay, CaHocID, -1, -1, iSoSinhVien, "", strMoTa, "", Utils.synTypeGVLichHocTuDangKi, 1, chkCapNhatTheoMa.Checked).Equals(1))
                {
                    // ghi vao log
                    returnDS();
                }
                else
                {
                    divAnnouce.InnerHtml = "Bạn kiểm tra lại thông tin đã nhập";
                }
            }
            else
            {
                if (data.dnn_NuceQLPM_LichPhongMay.UpdateDangKi(this.UserId,ID, strMaDK, strMaCB, strHoVaTenCB, strLop, strMonHoc, strThu, strTietBatDau, strSoTiet
                    , "", PhongHocID, iBuoiHoc, "", Ngay, CaHocID, -1, -1, iSoSinhVien, "", strMoTa, "", Utils.synTypeGVLichHocTuDangKi, 1, chkCapNhatTheoMa.Checked).Equals(1))
                {
                    // ghi vao log
                    returnDS();
                }
                else
                {
                    divAnnouce.InnerHtml = "Bạn kiểm tra lại thông tin đã nhập";
                }
            }
        }
        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnDS();
        }
        private void returnDS()
        {
            Response.Redirect("/tabid/" + this.TabId + "/default.aspx?" + "phong=" + ddlPhongHoc.SelectedValue + "&&tuan=" + Tuan.ToString());
        }
    }
}