using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_CauHoi_ThongKe
    {
        public static DataTable get(int CauHoi_ThongKeID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_ThongKe_get", CauHoi_ThongKeID).Tables[0];
        }
        public static DataTable getByBoCauHoi(int BoCauHoiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_ThongKe_getByBoCauHoi", BoCauHoiID).Tables[0];
        }
        public static void update(int iCauHoi_ThongKeID, bool IsCheck)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_CauHoi_ThongKe_update", iCauHoi_ThongKeID, IsCheck);
        }
        public static int insert(int CauHoiID, int KiThi_LopHoc_SinhVienID, bool IsCheck)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_CauHoi_ThongKe_insert", CauHoiID, KiThi_LopHoc_SinhVienID, IsCheck);
        }
    }
}
