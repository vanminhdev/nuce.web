using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLHPM_Ca_LopHoc
    {
        public static DataTable getSinhVien(int ID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_getSinhVien", ID).Tables[0];
        }
        public static DataTable getMayNotUseByPhong(int ID,int PhongID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_getMayNotUseByPhong", ID, PhongID).Tables[0];
        }
        public static DataTable getMayUseByPhong(int ID,int PhongID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_getMayUseByPhong", ID, PhongID).Tables[0];
        }
        public static DataTable get(int ID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_get", ID).Tables[0];
        }
        public static DataTable getNotInByCaHoc(int CaHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_getNotInByCaHoc", CaHocID).Tables[0];
        }
        public static DataTable getByCaHoc(int CaHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_getByCaHoc", CaHocID).Tables[0];
        }
        public static int insert(int KiThiID, int LopHocID, string PhongThi, string MoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLHPM_Ca_LopHoc_insert", KiThiID, LopHocID, PhongThi, MoTa);
        }
        public static void delete(int iID)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLHPM_Ca_LopHoc_delete", iID);
        }
    }
}
