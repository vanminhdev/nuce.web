using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_DoKho
    {
        public static DataTable get(int DoKhoID)
        {
            IDataReader iDR=DotNetNuke.Data.DataProvider.Instance().ExecuteSQL(string.Format(" select DoKhoID, Ten from [dbo].[NuceThi_DoKho] where (DoKhoID={0} or {0}=-1) ",DoKhoID));
            DataTable dt=new DataTable(); ;
            dt.Load(iDR);
            return dt;
        }
    }
}
