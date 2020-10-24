using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;

namespace nuce.web.thi
{
    public partial class DanhSachDeTrongBoDe : CoreModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["NguoiDung_MonHocid"] != null && Request.QueryString["NguoiDung_MonHocid"] != "")
                {
                    divAnnouce.InnerHtml = "";
                    int NguoiDung_MonHocID = -1;
                    if (int.TryParse(Request.QueryString["NguoiDung_MonHocid"].ToString().Trim(), out NguoiDung_MonHocID))
                    {
                        if (Request.QueryString["bodeid"] != null && Request.QueryString["bodeid"] != "")
                        {
                            int BoDeID = -1;
                            if (int.TryParse(Request.QueryString["bodeid"].ToString().Trim(), out BoDeID))
                            {
                                DataTable dtMaDe = data.dnn_NuceThi_DeThi.getMa(BoDeID);
                                ddlMaDe.DataSource = dtMaDe;
                                ddlMaDe.DataTextField = "Ma";
                                ddlMaDe.DataValueField = "DeThiID";
                                ddlMaDe.DataBind();
                                if (dtMaDe.Rows.Count > 0)
                                {
                                    string strDeThiID = dtMaDe.Rows[0]["DeThiID"].ToString();
                                    ddlMaDe.SelectedValue = strDeThiID;
                                    divContent.InnerHtml= UtilsDisplayDe.displayDe(strDeThiID, ddlMaDe.SelectedItem.Text, this.PortalId);
                                }
                            }
                            else
                            {
                                Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, NguoiDung_MonHocID));
                            }
                        }
                        else
                        {
                            Response.Redirect(string.Format("/tabid/{0}/default.aspx?NguoiDung_MonHocid={1}", this.TabId, NguoiDung_MonHocID));
                        }
                    }
                    else
                    {
                        Response.Redirect(string.Format("/tabid/{0}/default.aspx", this.TabId));
                    }
                }
                else
                {
                    Response.Redirect(string.Format("/tabid/{0}/default.aspx", this.TabId));
                }
            }
        }
        private void returnMain(string NguoiDung_MonHocID)
        {
            //Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "DanhSachCauHoi", "mid/" + this.ModuleId.ToString()));
            Response.Redirect(Globals.NavigateURL(this.TabId));
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            divContent.InnerHtml = UtilsDisplayDe.displayDe(ddlMaDe.SelectedValue, ddlMaDe.SelectedItem.Text, this.PortalId);
            //displayDe(ddlMaDe.SelectedValue);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(this.TabId));
        }
    }
}