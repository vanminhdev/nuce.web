using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_ThangDiem
    {
        public static DataTable get(int ThangDiemID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_ThangDiem_get", ThangDiemID).Tables[0];
        }
        public static DataTable getName(int ThangDiemID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_ThangDiem_getName", ThangDiemID).Tables[0];
        }
        public static void update(int iID,string txtTen,string txtMoTa)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_ThangDiem_update", iID,txtTen, txtMoTa);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_ThangDiem_update_status", iID, status);
        }
        public static int insert(string txtTen, string txtMoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_ThangDiem_insert", txtTen, txtMoTa);
        }
    }
}
