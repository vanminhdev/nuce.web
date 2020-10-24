using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;
using System.Text;
using ClosedXML.Excel;
using System.Collections.Generic;

namespace nuce.web.ks.analytics
{
    public partial class StatisticsNH : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            DataSet ds = nuce.web.data.dnn_Nuce_KS_DiemThi1.Statistics_NamHoc();

            DataTable dt1 = ds.Tables[3];
            string strDiv1 = "<table border='1' width='50%' style='padding: 5px;'><tr>";
            strDiv1 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Năm Học");
            strDiv1 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Standard deviation của ĐQT");
            strDiv1 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Điểm trung bình DQT");
            strDiv1 += "</tr>";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                strDiv1 += "<tr>";
                strDiv1 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", dt1.Rows[i]["S_NAMHOC"].ToString());
                strDiv1 += string.Format("<td style='text-align:center;'>{0:N2}</td>", float.Parse(dt1.Rows[i]["DQT_SD"].ToString()));
                strDiv1 += string.Format("<td style='text-align:center;'>{0:N2}</td>", float.Parse(dt1.Rows[i]["DQT_TBC"].ToString()));
                

                strDiv1 += "</tr>";
            }
            strDiv1 += "</table>";
            div1.InnerHtml = strDiv1;
            DataTable dt2 = ds.Tables[0];
            string strDiv2 = "<table border='1' width='100%' style='padding: 5px;'><tr>";
            strDiv2 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Năm Học");
            strDiv2 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Số sinh viên");
            strDiv2 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Điểm trung bình DQT");
            strDiv2 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", ">2 (Số lượng - %)");
            strDiv2 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", ">3 (Số lượng - %)");
            strDiv2 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", ">4 (Số lượng - %)");
            strDiv2 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", ">5 (Số lượng - %)");
            strDiv2 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", ">6 (Số lượng - %)");
            strDiv2 += "</tr>";
            int iSoSV = 0;
            int d2 = 0;
            int d3= 0;
            int d4 = 0;
            int d5 = 0;
            int d6 = 0;
            for (int i=0;i<dt2.Rows.Count;i++)
            {
                strDiv2 += "<tr>";
                strDiv2 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", dt2.Rows[i]["S_NAMHOC"].ToString());
                iSoSV = int.Parse(dt2.Rows[i]["sosv"].ToString());

                d2 = int.Parse(dt2.Rows[i]["d2"].ToString());
                d3 = int.Parse(dt2.Rows[i]["d3"].ToString());
                d4 = int.Parse(dt2.Rows[i]["d4"].ToString());
                d5 = int.Parse(dt2.Rows[i]["d5"].ToString());
                d6 = int.Parse(dt2.Rows[i]["d6"].ToString());

                strDiv2 += string.Format("<td style='text-align:center;'>{0}</td>", iSoSV);
                strDiv2 += string.Format("<td style='text-align:center;'>{0:N2}</td>", float.Parse(dt2.Rows[i]["DQT_TBC"].ToString()));
                strDiv2 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%)</td>", d2,100*(float)d2/ iSoSV);
                strDiv2 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%)</td>", d3, 100 * (float)d3 / iSoSV);
                strDiv2 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%)</td>", d4, 100 * (float)d4 / iSoSV);
                strDiv2 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%)</td>", d5, 100 * (float)d5 / iSoSV);
                strDiv2 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%)</td>", d6, 100 * (float)d6 / iSoSV);

                strDiv2 += "</tr>";
            }
            strDiv2 += "</table>";
            div2.InnerHtml = strDiv2;
            DataTable dt3 = ds.Tables[1];
            string strDiv3 = "<table border='1' width='100%' style='padding: 5px;'><tr>";
            strDiv3 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Năm Học");
            strDiv3 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Số lớp môn học");
            strDiv3 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "<3 (Số lượng - %)");
            strDiv3 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "<4 (Số lượng - %)");
            strDiv3 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "<5 (Số lượng - %)");
            strDiv3 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "<6 (Số lượng - %)");
            strDiv3 += "</tr>";
            int iSoLopMonHoc = 0;
            int sl3 = 0;
            int sl4 = 0;
            int sl5 = 0;
            int sl6 = 0;
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                strDiv3 += "<tr>";
                strDiv3 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", dt3.Rows[i]["S_NAMHOC"].ToString());
                iSoLopMonHoc = int.Parse(dt3.Rows[i]["SoLop"].ToString());

                sl3 = int.Parse(dt3.Rows[i]["sl3"].ToString());
                sl4 = int.Parse(dt3.Rows[i]["sl4"].ToString());
                sl5 = int.Parse(dt3.Rows[i]["sl5"].ToString());
                sl6 = int.Parse(dt3.Rows[i]["sl6"].ToString());

                strDiv3 += string.Format("<td style='text-align:center;'>{0}</td>", iSoLopMonHoc);
                strDiv3 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%)</td>", sl3, 100 * (float)sl3 / iSoLopMonHoc);
                strDiv3 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%)</td>", sl4, 100 * (float)sl4 / iSoLopMonHoc);
                strDiv3 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%)</td>", sl5, 100 * (float)sl5 / iSoLopMonHoc);
                strDiv3 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%)</td>", sl6, 100 * (float)sl6 / iSoLopMonHoc);

                strDiv3 += "</tr>";
            }
            strDiv3 += "</table>";
            div3.InnerHtml = strDiv3;
            DataTable dt4 = ds.Tables[2];
            string strDiv4 = "<table border='1' width='100%' style='padding: 5px;'><tr>";
            strDiv4 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Năm Học");
            strDiv4 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "Số lớp môn học");
            strDiv4 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "<1 (Số lượng - %) - điểm trung bình");
            strDiv4 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "<0.75 (Số lượng - %) - điểm trung bình");
            strDiv4 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "<0.5 (Số lượng - %) - điểm trung bình");
            strDiv4 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", "<0.25 (Số lượng - %) - điểm trung bình");
            strDiv4 += "</tr>";
            iSoLopMonHoc = 0;
            int dsl1 = 0;
            int dsl75 = 0;
            int dsl5 = 0;
            int dsl25 = 0;
            for (int i = 0; i < dt4.Rows.Count; i++)
            {
                strDiv4 += "<tr>";
                strDiv4 += string.Format("<td style='text-align:center;font-weight:bold;'>{0}</td>", dt4.Rows[i]["S_NAMHOC"].ToString());
                iSoLopMonHoc = int.Parse(dt4.Rows[i]["SoLop"].ToString());

                dsl1 = int.Parse(dt4.Rows[i]["sl1"].ToString());
                dsl75 = int.Parse(dt4.Rows[i]["sl75"].ToString());
                dsl5 = int.Parse(dt4.Rows[i]["sl5"].ToString());
                dsl25 = int.Parse(dt4.Rows[i]["sl25"].ToString());

                strDiv4 += string.Format("<td style='text-align:center;'>{0}</td>", iSoLopMonHoc);
                strDiv4 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%) --- <span style='color:red;'>{2:N2}</span></td>", dsl1, 100 * (float)dsl1 / iSoLopMonHoc,float.Parse(dt4.Rows[i]["tb1"].ToString()));
                strDiv4 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%) --- <span style='color:red;'>{2:N2}</span></td>", dsl75, 100 * (float)dsl75 / iSoLopMonHoc, float.Parse(dt4.Rows[i]["tb75"].ToString()));
                strDiv4 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%) --- <span style='color:red;'>{2:N2}</span></td>", dsl5, 100 * (float)dsl5 / iSoLopMonHoc, float.Parse(dt4.Rows[i]["tb5"].ToString()));
                strDiv4 += string.Format("<td style='text-align:center;'>{0} ({1:N2}%) --- <span style='color:red;'>{2:N2}</span></td>", dsl25, 100 * (float)dsl25 / iSoLopMonHoc, float.Parse(dt4.Rows[i]["tb25"].ToString()));

                strDiv4 += "</tr>";
            }
            strDiv4 += "</table>";
            div4.InnerHtml = strDiv4;
        }

    }
}