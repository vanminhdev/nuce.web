using nuce.web.data;
using Nuce.CTSV;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace nuce.ctsv
{
    public partial class CapNhatMaYeuCauDichVu : Page
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
                    if (((Request.QueryString["ID"] != null) && (Request.QueryString["Ma"] != null)&& (Request.QueryString["Type"] != null)) ||
                        ((Request.Form["ID"] != null) && (Request.Form["Ma"] != null) && (Request.Form["Type"] != null)))
                    {
                        nuce.web.model.SinhVien m_SinhVien = (nuce.web.model.SinhVien)Session[Utils.session_sinhvien];
                        int iID=-1;
                        string Ma = "";
                        int Type = -1;
                        if (Request.QueryString["ID"] != null)
                        {
                            iID = int.Parse(Request.QueryString["ID"]);
                            Ma = Request.QueryString["Ma"];
                            Type = int.Parse(Request.QueryString["Type"]);
                        }
                        else
                        {
                            iID = int.Parse(Request.Form["ID"]);
                            Ma = Request.Form["Ma"];
                            Type = int.Parse(Request.Form["Type"]);
                        }
                        // Cap nhat
                        string sql = "";
                        
                        if(nuce.web.data.DataUtils.LoaiDichVuSinhViens.ContainsKey(Type))
                        {
                            sql = string.Format(@"if exists(select 1 from [dbo].[{0}]
                                                    where MaXacNhan=@Param0 and ID=@Param1 and Status=1)
                                                    Begin
                                                        update [dbo].[{0}] set Status=2,NgayGui=getdate(),LastModifiedTime=getdate(),LastModifiedBy=@Param2 where ID=@Param1;
                                                        select 1;
                                                    END
                                                    ELSE
                                                    BEGIN
                                                        select -1;
                                                    END", nuce.web.data.DataUtils.LoaiDichVuSinhViens[Type].Param1);
                        }
                       
                        SqlParameter[] sqlParams = new SqlParameter[3];
                        sqlParams[0] = new SqlParameter("@Param0", Ma);
                        sqlParams[1] = new SqlParameter("@Param1",iID);
                        sqlParams[2] = new SqlParameter("@Param2", m_SinhVien.SinhVienID);
                        strData =(string)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(Nuce_Common.ConnectionString, CommandType.Text, sql, sqlParams).ToString();
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