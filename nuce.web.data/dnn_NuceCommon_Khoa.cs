using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_Khoa
    {
        public static DataTable get(int khoaID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_Khoa_get", khoaID).Tables[0];
        }
        public static DataTable getName(int khoaID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_Khoa_getName", khoaID).Tables[0];
        }
        public static void update(int iID,int truongID,string txtMa,string txtTen,string txtTenTiengAnh,string txtDiaChi,string txtMoTa)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_Khoa_update", iID,truongID, txtMa, txtTen, txtTenTiengAnh, txtDiaChi, txtMoTa);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_Khoa_update_status", iID, status);
        }
        public static int insert(int truongID, string txtMa, string txtTen, string txtTenTiengAnh, string txtDiaChi, string txtMoTa)
        {
           return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_Khoa_insert", truongID, txtMa, txtTen, txtTenTiengAnh, txtDiaChi, txtMoTa);
        }
        public static int insert(int KhoaID,int truongID, string txtMa, string txtTen, string txtTenTiengAnh, string txtDiaChi, string txtMoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_Khoa_insertSyn", KhoaID, truongID, txtMa, txtTen, txtTenTiengAnh, txtDiaChi, txtMoTa);
        }
    }
}
