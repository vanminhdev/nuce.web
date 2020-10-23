using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;

namespace nuce.web.ks.analytics
{
    public partial class SearchByMH : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtKeyword.Text = "010211";
                LoadGridData();
            }
        }
        private void LoadGridData()
        {
            //I am adding dummy data here. You should bring data from your repository.
            string strRemoveDau = txtKeyword.Text.Trim();
            DataSet ds = nuce.web.data.dnn_Nuce_KS_DiemThi1.getByMH(strRemoveDau);
            
            DataTable dt = ds.Tables[0];
            if (dt == null || dt.Rows.Count < 1)
            {
                divSummary.InnerHtml = "Không có dữ liệu";
                grdData.DataSource = dt;
                grdData.DataBind();
                ViewState["dirState"] = dt;
                ViewState["sortdr"] = "Asc";
            }
            else
            {
                DataTable dt1 = ds.Tables[1];

                int iTotalLop = dt.Rows.Count;
                int iTotalDTB4 = int.Parse(ds.Tables[2].Rows[0][0].ToString());
                int iTotalDTB5 = int.Parse(ds.Tables[3].Rows[0][0].ToString());
                int iTotalDTB6 = int.Parse(ds.Tables[4].Rows[0][0].ToString());

                int iTotalSD1 = int.Parse(ds.Tables[5].Rows[0][0].ToString());
                int iTotalSD75 = int.Parse(ds.Tables[6].Rows[0][0].ToString());
                int iTotalSD5 = int.Parse(ds.Tables[7].Rows[0][0].ToString());
                int iTotalSD25 = int.Parse(ds.Tables[8].Rows[0][0].ToString());

                grdData.DataSource = dt;
                grdData.DataBind();
                ViewState["dirState"] = dt;
                ViewState["sortdr"] = "Asc";
                string strSummary = string.Format("<div style='text - align: left; padding: 2px;'>Môn học: <span style='color:red;'>{0}</span> có tất cả: <span style='color:red;'>{1}</span> lớp môn học", ds.Tables[9].Rows[0]["Name"].ToString(), iTotalLop);
                strSummary += string.Format(". Tổng số sinh viên: <span style='color:red;'>{0}</span></div>", dt1.Rows[0]["sosv"].ToString());
                strSummary += string.Format("<div style='text - align: left; padding: 2px;'>Tỷ lệ tỷ lệ các điểm (ĐQT - ĐKT) >2: <span style='color:red;'>{0:N2}%</span>", dt1.Rows[0]["d2"].ToString());
                strSummary += string.Format("; >3: <span style='color:red;'>{0:N2}%</span>", float.Parse(dt1.Rows[0]["d3"].ToString()));
                strSummary += string.Format("; >4: <span style='color:red;'>{0:N2}%</span>", float.Parse(dt1.Rows[0]["d4"].ToString()));
                strSummary += string.Format("; >5: <span style='color:red;'>{0:N2}%</span>", float.Parse(dt1.Rows[0]["d5"].ToString()));
                strSummary += string.Format("; >6: <span style='color:red;'>{0:N2}%</span></div>", float.Parse(dt1.Rows[0]["d6"].ToString()));

                strSummary += string.Format("<div style='text - align: left; padding: 2px;'>Số lớp môn học có điểm DQT trung bình <4: <span style='color:red;'>{0} ({1:N2}%)</span>", iTotalDTB4, 100 * (float)iTotalDTB4 / iTotalLop);
                strSummary += string.Format("; <5: <span style='color:red;'>{0} ({1:N2}%)</span>", iTotalDTB5, 100 * (float)iTotalDTB5 / iTotalLop);
                strSummary += string.Format("; <6: <span style='color:red;'>{0} ({1:N2}%)</span></div>", iTotalDTB6, 100 * (float)iTotalDTB6 / iTotalLop);

                strSummary += string.Format("<div style='text - align: left; padding: 2px;'>Số lớp môn học có độ lệch trung bình (standard deviation) của ĐQT<1: <span style='color:red;'>{0} ({1:N2}%)</span>", iTotalSD1, 100 * (float)iTotalSD1 / iTotalLop);
                strSummary += string.Format("; <0.75: <span style='color:red;'>{0} ({1:N2}%)</span>", iTotalSD75, 100 * (float)iTotalSD75 / iTotalLop);
                strSummary += string.Format("; <0.5: <span style='color:red;'>{0} ({1:N2}%)</span>", iTotalSD5, 100 * (float)iTotalSD5 / iTotalLop);
                strSummary += string.Format("; <0.25: <span style='color:red;'>{0} ({1:N2}%)</span></div>", iTotalSD25, 100 * (float)iTotalSD25 / iTotalLop);

                divSummary.InnerHtml = strSummary;
            }
        }
        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdData.PageIndex = e.NewPageIndex;
            DataTable dtrslt = (DataTable)ViewState["dirState"];
            grdData.DataSource = dtrslt;
            grdData.DataBind();
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadGridData();
        }
        protected void grdData_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtrslt = (DataTable)ViewState["dirState"];
            if (dtrslt.Rows.Count > 0)
            {
                if (Convert.ToString(ViewState["sortdr"]) == "Asc")
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Desc";
                    ViewState["sortdr"] = "Desc";
                }
                else
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Asc";
                    ViewState["sortdr"] = "Asc";
                }
                grdData.DataSource = dtrslt;
                grdData.DataBind();
            }

        }
    }
}