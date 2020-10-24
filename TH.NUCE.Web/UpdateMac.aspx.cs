using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;

namespace nuce.web
{
    public partial class UpdateMac : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable tblTable = data.dnn_NuceCommon_May1.getName(-1);

                ddlMay.DataSource = tblTable;
                ddlMay.DataTextField = "Ma1";
                ddlMay.DataValueField = "MayID";
                ddlMay.DataBind();
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int iID = int.Parse(ddlMay.SelectedValue.ToString());
            data.dnn_NuceCommon_May1.updateMac(iID, txtMac.Text.Trim());
            tdMacSaoChep.InnerHtml = string.Format("Cập nhật thành công  {0}", txtMac.Text);
        }
    }
}