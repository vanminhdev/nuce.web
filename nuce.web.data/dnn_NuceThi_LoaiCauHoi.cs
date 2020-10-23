using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_LoaiCauHoi
    {
        public static DataTable get(int ID)
        {
            IDataReader iDR=DotNetNuke.Data.DataProvider.Instance().ExecuteSQL(string.Format(@" SELECT [ID]
      ,[Name],[Description],[Status] FROM[dbo].[NuceThi_LoaiCauHoi] where (ID={0} or {0}=-1) and Status=1 Order by ID ", ID));
            DataTable dt=new DataTable(); ;
            dt.Load(iDR);
            return dt;
        }
    }
}
