using System;

namespace nuce.web.sinhvien
{
    public partial class DoiMatKhau : CoreModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["dieukhien"] != null)
                {
                    if (Request.QueryString["dieukhien"].ToString().Equals("logout"))
                    {
                        Session.Remove(Utils.session_sinhvien);
                        Session.Remove(Utils.session_kithi_lophoc_sinhvien);
                        Session.Abandon();
                        Response.Redirect(string.Format("/tabid/{0}/default.aspx", Utils.tab_login_sinhvien));
                    }
                }
            }
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            string strMauKhauCu = txtMauKhauOld.Text.Trim();
            string strMatKhauMoi = txtMauKhauNew.Text.Trim();
            string strMatKhauMoiNhapLai = txtMauKhauNewRepeat.Text.Trim();
            if (strMauKhauCu.Equals(""))
            {
                tdAnnounce.InnerHtml = "Không được để mật khẩu cũ trắng";
                return;
            }
            if (strMatKhauMoi.Equals(""))
            {
                tdAnnounce.InnerHtml = "Không được để mật khẩu mới trắng";
                return;
            }
            if (!strMatKhauMoi.Equals(strMatKhauMoiNhapLai))
            {
                tdAnnounce.InnerHtml = "Nhập lại mật khẩu sai";
                return;
            }
            if (data.dnn_NuceCommon_SinhVien.doimatkhau(this.m_SinhVien.SinhVienID, strMauKhauCu, strMatKhauMoi))
            {
                tdAnnounce.InnerHtml = "Đổi mật khẩu thành công";
            }
            else
            {
                tdAnnounce.InnerHtml = "Đổi mật khẩu không thành công";
            }
        }
    }
}