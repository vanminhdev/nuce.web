using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using nuce.web.data;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ad_ctsv_qldv_thamso_update : CoreHandlerCommonAdminCTSV
    {
        public override void WriteData(HttpContext context)
        {
            string body = new StreamReader(context.Request.InputStream).ReadToEnd();
            var paramData = JsonConvert.DeserializeObject<List<Parameter>>(body);
            context.Response.ContentType = "text/plain";
            string sql = "";
            foreach (var item in paramData)
            {
                sql += $@"UPDATE dbo.AS_Academy_Student_SV_ThietLapThamSoDichVu
                         SET Value = N'{item.Value.Trim()}'
                         WHERE ID = {item.ID}; ";    
            }
            SqlHelper.ExecuteNonQuery(Nuce_Common.ConnectionString, CommandType.Text, sql);
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write("");
        }

        public class Parameter
        {
            public int ID { get; set; }
            public int DichVuID { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }

    }
}
