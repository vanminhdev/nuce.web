using System;
using System.Data;
using DotNetNuke.Entities.Modules;

namespace nuce.web.qlpm
{
    public partial class New_HuyDangKiLich : PortalModuleBase
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
            PhongHocID = int.Parse(Request.QueryString["phong"]);
            Tuan = int.Parse(Request.QueryString["tuan"]);
            ID = int.Parse(Request.QueryString["id"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
            }
        }

        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnDS();
        }
        private void returnDS()
        {
            Response.Redirect("/tabid/" + this.TabId + "/default.aspx?" + "phong=" + PhongHocID.ToString() + "&&tuan=" + Tuan.ToString());
        }

        protected void btnHuyLich_Click(object sender, EventArgs e)
        {
            string strMoTa = txtMoTa.Text;
            if (data.dnn_NuceQLPM_LichPhongMay.UpdateStatus(this.UserId, ID, strMoTa, Utils.synTypeLichHocThayDoi, 4, chkCapNhatTheoMa.Checked).Equals(1))
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
}