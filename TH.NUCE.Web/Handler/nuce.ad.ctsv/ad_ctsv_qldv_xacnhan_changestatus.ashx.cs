using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.Services;
using nuce.web.tienich;
using nuce.web.data;

namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ad_ctsv_qldv_xacnhan_changestatus : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
            string strID = context.Request["ID"].ToString();
            string strStatus = context.Request["STATUS"].ToString();
            string strPhanHoi = context.Request["PhanHoi"].ToString();
            string strNgayBatDau = context.Request["NgayBatDau"].ToString();
            string strNgayKetThuc = context.Request["NgayKetThuc"].ToString();
            string strType = "1";
            try
            {
                strType = context.Request["Type"].ToString();
            }
            catch
            {
                strType = "1";
            }

            string dtReturnRowCount = nuce.web.data.Nuce_CTSV.UpdateRequestStatus(strID, strStatus, strPhanHoi, strNgayBatDau, strNgayKetThuc, strType);
            context.Response.ContentType = "text/plain";
            context.Response.Write(dtReturnRowCount);
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
