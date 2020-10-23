using System;
namespace nuce.web.thi
{
    public partial class SuaGhiChuCuaSinhVienTrongKiThi : CoreModule
    {
        private int m_iKiThi_LopHocID;
        private int m_iKiThi_Lophoc_SinhVienID;
        private int m_iPreTabID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                m_iKiThi_LopHocID = -1;
                if (Request.QueryString["kithi_lophoc"] != null)
                {
                    m_iKiThi_LopHocID = int.Parse(Request.QueryString["kithi_lophoc"]);
                }
                m_iKiThi_Lophoc_SinhVienID = -1;
                if (Request.QueryString["kithi_lophoc_sinhvien"] != null)
                {
                    m_iKiThi_Lophoc_SinhVienID = int.Parse(Request.QueryString["kithi_lophoc_sinhvien"]);
                }
                m_iPreTabID = -1;
                if (Request.QueryString["pretabid"] != null)
                {
                    m_iPreTabID = int.Parse(Request.QueryString["pretabid"]);
                }
                txtID.Text = m_iKiThi_Lophoc_SinhVienID.ToString();
                txtKiThiLopHocID.Text = m_iKiThi_LopHocID.ToString();
                txtPreTabid.Text = m_iPreTabID.ToString();

                txtGhiChu.Text = data.dnn_NuceThi_KiThi_LopHoc_SinhVien.getMoTa(m_iKiThi_Lophoc_SinhVienID);
            }
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            string strMoTa = txtGhiChu.Text;
            int iID = int.Parse(txtID.Text);
            data.dnn_NuceThi_KiThi_LopHoc_SinhVien.updateMoTa(iID, strMoTa);
            returnMain();
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain();
        }
        private void returnMain()
        {
            m_iKiThi_LopHocID = int.Parse(txtKiThiLopHocID.Text);
            m_iPreTabID = int.Parse(txtPreTabid.Text);
            if (m_iPreTabID.Equals(-1))
                m_iPreTabID = this.TabId;
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?kithi_lophoc={1}", m_iPreTabID, m_iKiThi_LopHocID));
        }
    }
}