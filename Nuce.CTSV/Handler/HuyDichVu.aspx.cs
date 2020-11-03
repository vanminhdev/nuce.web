using nuce.web.data;
using Nuce.CTSV;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace nuce.ctsv
{
    public partial class HuyDichVu : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strData = "-1";
            try
            {
                if (Session[Utils.session_sinhvien] == null)
                {
                    strData = "NotAuthenticated";
                }
                else
                {
                    nuce.web.model.SinhVien m_SinhVien = (nuce.web.model.SinhVien)Session[Utils.session_sinhvien];
                    if (((Request.QueryString["ID"] != null) && (Request.QueryString["Type"] != null)) ||
                        ((Request.Form["ID"] != null) && (Request.Form["Type"] != null)))
                    {
                        int iID = -1;
                        string Ma = "";
                        int Type = -1;
                        if (Request.QueryString["ID"] != null)
                        {
                            iID = int.Parse(Request.QueryString["ID"]);
                            Type = int.Parse(Request.QueryString["Type"]);
                        }
                        else
                        {
                            iID = int.Parse(Request.Form["ID"]);
                            Type = int.Parse(Request.Form["Type"]);
                        }
                        // Cap nhat
                        string sql = "";
                        if (nuce.web.data.DataUtils.LoaiDichVuSinhViens.ContainsKey(Type))
                        {
                            sql = string.Format(@" update [dbo].[{0}] set Deleted=1,DeletedBy=@Param0,DeletedTime=getdate() where ID=@Param1 and Status=1;
                                                        select 1;
                                                    ", nuce.web.data.DataUtils.LoaiDichVuSinhViens[Type].Param1);
                        }
                      
                        SqlParameter[] sqlParams = new SqlParameter[2];
                        sqlParams[0] = new SqlParameter("@Param0", m_SinhVien.SinhVienID);
                        sqlParams[1] = new SqlParameter("@Param1", iID);
                        strData = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(Nuce_Common.ConnectionString, CommandType.Text, sql, sqlParams).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                strData = ex.Message;
            }
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write(strData);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}