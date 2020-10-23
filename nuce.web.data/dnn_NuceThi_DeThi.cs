using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_DeThi
    {
        public static DataTable get(int DeThiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_DeThi_get", DeThiID).Tables[0];
        }
        public static DataTable getRandomDeByBoDe(int BoDeID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_DeThi_getRandomDeByBoDe", BoDeID).Tables[0];
        }
        public static DataTable getMa(int BoDeID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_DeThi_getMa", BoDeID).Tables[0];
        }
        public static void deleteByBoDe(int BoDeID)
        {
             DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_DeThi_deleteByBoDe", BoDeID);
        }
        public static DataTable getByBoDe(int BoDeID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_DeThi_getByBoDe", BoDeID).Tables[0];
        }
        public static void update(int iID,int BoDeID,string Ma,string NoiDungDeThi,string DapAn,int Type)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_DeThi_update", iID, BoDeID, Ma, NoiDungDeThi, DapAn, Type);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_DeThi_update_status", iID, status);
        }
        public static int insert(int BoDeID, string Ma, string NoiDungDeThi, string DapAn, int Type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_DeThi_insert", BoDeID, Ma, NoiDungDeThi, DapAn, Type);
        }
    }
}
