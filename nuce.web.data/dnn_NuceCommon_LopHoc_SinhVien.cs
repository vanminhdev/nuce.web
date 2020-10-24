using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_LopHoc_SinhVien
    {
        public static DataTable getByLopHoc(int LopHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopHoc_SinhVien_getByLopHoc", LopHocID).Tables[0];
        }
        public static DataTable getByLopHoc(int LopHocID,int UserID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopHoc_SinhVien_getByLopHocAndUser", LopHocID, UserID).Tables[0];
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_LopHoc_SinhVien_update_status", iID, status);
        }
        public static int insert(int LopHocID, string MaSV, string GhiChu,int Order)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_LopHoc_SinhVien_insert", LopHocID, MaSV, GhiChu, Order);
        }

        public static int insert(int LopHocID, string MaSV,string Ho,string Ten, string GhiChu, int Order,string MaLopQL)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_LopHoc_SinhVien_insert1", LopHocID, MaSV, Ho,Ten, GhiChu, Order, MaLopQL);
        }
    }
}
