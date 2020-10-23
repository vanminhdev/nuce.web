using System;
using System.Data;
using DotNetNuke.Entities.Modules;
namespace nuce.web.qlpm
{
    public partial class SuaDangKiLopHoc : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["tuanhientaiid"] != null && Request.QueryString["tuanhientaiid"] != "" && Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                {
                    try
                    {
                        //Kiểm tra xem có chuyển điểm danh

                        DataTable dtLopHienTai = data.dnn_NuceQLPM_TuanHienTai.getByType(int.Parse(Request.QueryString["tuanhientaiid"].Trim()), int.Parse(Request.QueryString["type"].Trim()));
                        if(dtLopHienTai.Rows.Count>0)
                        { 
                             DataTable dtTableLopHoc = data.dnn_NuceCommon_LopHoc.get1(-1);
                            ddlLopHoc.DataTextField = "Ten";
                            ddlLopHoc.DataValueField = "LopHocID";
                            ddlLopHoc.DataSource = dtTableLopHoc;
                            ddlLopHoc.DataBind();
                            ddlLopHoc.SelectedValue = dtLopHienTai.Rows[0]["LopHocID"].ToString();
                            txtGhiChu.Text = dtLopHienTai.Rows[0]["GhiChu"].ToString();
                            txtID.Text = dtLopHienTai.Rows[0]["TuanHienTaiID"].ToString();
                            txtType.Text = dtLopHienTai.Rows[0]["Type"].ToString();
                        }
                        else
                        {
                            divAnnounce.InnerText = "Tuần hiện tại không thỏa mãn";
                        }
                    }
                    catch (Exception ex)
                    {
                        divAnnounce.InnerText = ex.ToString();
                    }
                   
                }
                else
                {
                    divAnnounce.InnerText = "Chưa chọn tuần hiện tại";
                }

            }
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            int iTuanHienTai = int.Parse(txtID.Text);
            int iType = int.Parse(txtType.Text);
            int iLopHoc = int.Parse(ddlLopHoc.SelectedValue);
            string strGhiChu = txtGhiChu.Text;
            data.dnn_NuceQLPM_TuanHienTai.update(iTuanHienTai, iLopHoc, strGhiChu);
            divAnnounce.InnerText = "Cập nhật thành công";
            if (Request.QueryString["phonghocid"] != null && Request.QueryString["phonghocid"] != "" && Request.QueryString["tab"] != null && Request.QueryString["tab"] != "")
            {
                Response.Redirect(string.Format("/tabid/{0}/default.aspx?phonghocid={1}", Request.QueryString["tab"], Request.QueryString["phonghocid"]));
            }
        }

        protected void btnHuy_Click(object sender, EventArgs e)
        {
            int iTuanHienTai = int.Parse(txtID.Text);
            int iLopHoc = -1;
            string strGhiChu = txtGhiChu.Text;
            data.dnn_NuceQLPM_TuanHienTai.update(iTuanHienTai, iLopHoc, strGhiChu);
            divAnnounce.InnerText = "Hủy đăng kí thành công";
            if (Request.QueryString["phonghocid"] != null && Request.QueryString["phonghocid"] != "" && Request.QueryString["tab"] != null && Request.QueryString["tab"] != "")
            {
                Response.Redirect(string.Format("/tabid/{0}/default.aspx?phonghocid={1}", Request.QueryString["tab"], Request.QueryString["phonghocid"]));
            }
        }

        protected void btnChuyenDiemDanh_Click(object sender, EventArgs e)
        {
          
        }
    }
}