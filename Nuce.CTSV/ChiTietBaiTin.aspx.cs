using System;
using System.Data;
using System.Web;
using System.Web.UI;

namespace Nuce.CTSV
{
    public partial class ChiTietBaiTin : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Request.QueryString["ID"]!=null)
                {
                    DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(nuce.web.data.Nuce_Common.ConnectionString, CommandType.Text, string.Format(@"SELECT *
  FROM [dbo].[AS_News_Items]
  where ID={0}", Request.QueryString["ID"])).Tables[0];
                    if(dt.Rows.Count>0)
                    {
                        div_title.InnerHtml = divThisArticle.InnerHtml = string.Format("{0} - {1:dd/MM/yyyy}",dt.Rows[0]["title"].ToString(),DateTime.Parse( dt.Rows[0]["update_datetime"].ToString()));
                        div_description.InnerHtml= string.Format("<i>{0}</i>", dt.Rows[0]["description"].ToString());
                        div_new_content.InnerHtml = string.Format("{0}", HttpUtility.HtmlDecode(dt.Rows[0]["new_content"].ToString()));
                    }
                }
            }
        }
    }
}