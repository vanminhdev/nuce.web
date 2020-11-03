using nuce.web.data;
using System.Data;
using System.Web;
using System.Web.Services;
namespace nuce.web.commons
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ad_tcc_getnguoithitrongkithi : CoreHandlerCommonAdminThiChungChi
    {
        public override void WriteData(HttpContext context)
        {
           // DataSet ds = data.dnn_NuceCommon_Khoa.getName(-1).DataSet;
            context.Response.ContentType = "text/plain";
            string search = context.Request["search"].ToString();
            string strKiThiID= context.Request["ID"].ToString();
            string strType= context.Request["Type"].ToString();
            string strDanhMuc= context.Request["DanhMuc"].ToString();
            string sql = "";
            if(strType.Equals("1"))
            { 
             sql = string.Format(@"select top 20 *,CONVERT(VARCHAR(10), NgaySinh, 103) as NgaySinhVN from [dbo].[NUCE_ThiChungChi_NguoiThi] 
            where (DanhMucID ={2} or {2}=-1) and ID in (select [SinhVienID] from [dbo].[NuceThi_KiThi_LopHoc_SinhVien] where [KiThi_LopHocID]={1} and Type=1) 
                  and (CMT like N'%{0}%' or Ma like N'%{0}%' or Ten like N'%{0}%' or Ho like N'%{0}%') 
                  and  Status<>4 order by Ten,Ho, [UpdatedDate] desc", search,strKiThiID,strDanhMuc);
            }
            else
            {
                sql = string.Format(@"select KiThi_LopHoc_SinhVienID, KiThi_LopHocID, SinhVienID, DeThiID,  NgayGioBatDau, NgayGioNopBai, TongThoiGianThi, TongThoiGianConLai, Diem, a.Type, a.Status, MaDe, LogIP,b.Ho,b.Ten,b.Ma from [dbo].[NuceThi_KiThi_LopHoc_SinhVien] a inner join [dbo].[NUCE_ThiChungChi_NguoiThi] b
on a.SinhVienID=b.ID where KiThi_LopHocID={0} and a.Type=1 order by Ten,Ho", strKiThiID);
            }
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_ThiChungChi.ConnectionString, CommandType.Text, sql).Tables[0];
            context.Response.Write(DataTableToJSONWithJavaScriptSerializer(dt));
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
