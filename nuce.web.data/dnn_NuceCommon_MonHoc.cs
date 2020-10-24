using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_MonHoc
    {
        public static DataTable get(int MonHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_MonHoc_get", MonHocID).Tables[0];
        }
        public static DataTable getName(int MonHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_MonHoc_getName", MonHocID).Tables[0];
        }
        public static DataTable getByBoMon(int BoMonID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_MonHoc_getByBoMon", BoMonID).Tables[0];
        }
        public static DataTable getNameByBoMon(int BoMonID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_MonHoc_getNameByBoMon", BoMonID).Tables[0];
        }
        public static DataTable getByNguoiDung(int UserID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_MonHoc_getByNguoiDung", UserID).Tables[0];
        }
        public static void update(int iID,int BoMonID,string txtMa,string txtTen,string txtTenTiengAnh,int iTinChi,string txtMoTa)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_MonHoc_update", iID, BoMonID, txtMa, txtTen, txtTenTiengAnh, iTinChi, txtMoTa);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_MonHoc_update_status", iID, status);
        }
        public static int insert(int BoMonID, string txtMa, string txtTen, string txtTenTiengAnh, int iTinChi, string txtMoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_MonHoc_insert", BoMonID, txtMa, txtTen, txtTenTiengAnh, iTinChi, txtMoTa);
        }
        public static int insert(int MonHocID,int BoMonID,string MaBoMon, string txtMa, string txtTen, string txtTenTiengAnh, int iTinChi, string txtMoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_MonHoc_insertSyn", MonHocID, BoMonID, MaBoMon, txtMa, txtTen, txtTenTiengAnh, iTinChi, txtMoTa);
        }
        public static int checkNguoiDungAndMonHoc(int UserID, int MonHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_MonHoc_checkNguoiDungAndMonHoc", UserID, MonHocID);
        }
    }
}
