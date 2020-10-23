using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLHPM_CaHoc_DotTraoDoi
    {
        //
        public static DataTable getByCaLopHoc(int ID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_CaHoc_DotTraoDoi_GetByCaLopHoc", ID).Tables[0];
        }
        public static DataTable get(int ID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_CaHoc_DotTraoDoi_Get", ID).Tables[0];
        }
        public static DataTable getByCa(int CaHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_CaHoc_DotTraoDoi_GetByCa", CaHocID).Tables[0];
        }
        public static DataTable getName(int iCaHoc_DotTraoDoiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_CaHoc_DotTraoDoi_getName", iCaHoc_DotTraoDoiID).Tables[0];
        }
        public static void update(int iID, int CaHocID, string txtTen, int GioBatDau, int PhutBatDau, int GioKetThuc, int PhutKetThuc, string FileChoPhep,int DungLuong, string MoTa, int Type, int Status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLHPM_CaHoc_DotTraoDoi_update", iID, CaHocID, txtTen, GioBatDau, PhutBatDau, GioKetThuc, PhutKetThuc, FileChoPhep, DungLuong, MoTa, Type, Status);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLHPM_CaHoc_DotTraoDoi_update_status", iID, status);
        }
        public static int insert(int CaHocID, string txtTen, int GioBatDau,int PhutBatDau,int GioKetThuc,int PhutKetThuc, string FileChoPhep,int DungLuong,string MoTa,int Type,int Status)
        {
           return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLHPM_CaHoc_DotTraoDoi_insert", CaHocID, txtTen, GioBatDau, PhutBatDau, GioKetThuc, PhutKetThuc,FileChoPhep, DungLuong,MoTa, Type,Status);
        }
    }
}
