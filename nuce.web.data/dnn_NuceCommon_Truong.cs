using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_Truong
    {
        public static DataTable get(int truongID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_Truong_get", truongID).Tables[0];
        }

        public static void update(int iID,string txtMa,string txtTen,string txtTenTiengAnh,string txtDiaChi,string txtMoTa)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_Truong_update", iID, txtMa, txtTen, txtTenTiengAnh, txtDiaChi, txtMoTa);
        }
    }
}
