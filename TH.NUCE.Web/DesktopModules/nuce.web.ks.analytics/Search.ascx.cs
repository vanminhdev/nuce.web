using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;
using System.Text;
using ClosedXML.Excel;
using System.Collections.Generic;

namespace nuce.web.ks.analytics
{
    public partial class Search : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtKeyword.Text = "Phuong phap";
                LoadGridData();
            }
        }
        private void LoadGridData()
        {
            //I am adding dummy data here. You should bring data from your repository.
            string strRemoveDau = Utils.RemoveUnicode(txtKeyword.Text.Trim()).Replace("  ", " ").Replace(" ", "-");
            strRemoveDau = strRemoveDau.Trim();
            strRemoveDau = strRemoveDau.ToUpper();
            //DataTable dt=DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_OutputText_Search", strRemoveDau).Tables[0];
            DataTable dt = DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_OutputText_Search1", strRemoveDau,ddlCauHoi.SelectedValue).Tables[0];
            grdData.DataSource = dt;
            grdData.DataBind();
            divSummary.InnerHtml = string.Format("Có tất cả {0} kết quả được tìm thấy",dt.Rows.Count);
        }
        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdData.PageIndex = e.NewPageIndex;
            LoadGridData();
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadGridData();
        }

    }
}