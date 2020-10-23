using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLHPM_CaHoc_Items
    {
        public static DataTable getGetByCaHocDotTraoDoi(int CaHoc_DotTraoDoiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_CaHoc_Items_getGetByCaHocDotTraoDoi", CaHoc_DotTraoDoiID).Tables[0];
        }
        //
        public static DataTable getGetByCaLopHocSinhVien(int ID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_CaHoc_Items_GetByCaLopHocSinhVien", ID).Tables[0];
        }
        public static void updateStatus(int Id, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLHPM_CaHoc_Items_UpdateStatus", Id, status);
        }
        public static void insert(int CaHoc_DotTraoDoiID, int Ca_LopHoc_SinhVienID, string NoiDung,string Link,string TenFileGoc,string TenFileMoi,string MoRongFile,string LoaiFile,int FileDungLuong)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLHPM_CaHoc_Items_Insert", CaHoc_DotTraoDoiID, Ca_LopHoc_SinhVienID,NoiDung,Link, TenFileGoc,TenFileMoi,MoRongFile,LoaiFile,FileDungLuong);
        }
    }
}
