using nuce.web.data;
using System;
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
    public class ad_tcc_updatethisinhtrongkithi : CoreHandlerCommonAdminThiChungChi
    {
        public override void WriteData(HttpContext context)
        {
            string strID = context.Request["ID"].ToString();
            string strType = context.Request["Type"].ToString();
            string strSinhViens = context.Request["SinhVien"].ToString();
            context.Response.ContentType = "text/plain";
            string strInsert = "";
            string[] strSplits = strSinhViens.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string strTemp in strSplits)
            {
                if (strType.Equals("1"))
                {
                    strInsert += string.Format(@"if(not exists(select 1 from [dbo].[NuceThi_KiThi_LopHoc_SinhVien] where KiThi_LopHocID={0} AND SinhVienID={1}))
                Begin
                     INSERT INTO [dbo].[NuceThi_KiThi_LopHoc_SinhVien]
                           ([KiThi_LopHocID],[SinhVienID],[DeThiID],[NoiDungDeThi],[DapAn],[BaiLam],[NgayGioBatDau]
                           ,[NgayGioNopBai],[TongThoiGianThi],[TongThoiGianConLai],[Diem],[MoTa],[Type],[Status]
                           ,[MaDe],[LogIP])
                     VALUES
                           ({0},{1},-1,'','','',GETDate(),GETDate()
                           ,-1,-1,-1,''
                           ,1,1,'','');
                END
                ELSE
                BEGIN
                    UPDATE [dbo].[NuceThi_KiThi_LopHoc_SinhVien] set Type=1 where KiThi_LopHocID={0} and SinhVienID={1};
                END;", strID, strTemp);
                }
                else
                {
                    strInsert +=string.Format(@"  UPDATE [dbo].[NuceThi_KiThi_LopHoc_SinhVien] set Type=4 where KiThi_LopHocID={0} and SinhVienID={1};", strID, strTemp);
                }
            }
            int iReturn = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Nuce_ThiChungChi.ConnectionString, CommandType.Text, strInsert);
            context.Response.Write(iReturn.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering i
        }
    }
}
