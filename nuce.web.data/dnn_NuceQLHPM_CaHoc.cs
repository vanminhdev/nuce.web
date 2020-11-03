using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLHPM_CaHoc
    {
        public static DataTable get(int id)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_CaHoc_get", id).Tables[0];

        }
        public static DataTable getName(int id)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_CaHoc_getName", id).Tables[0];

        }
        
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLHPM_CaHoc_update_status", iID, status);
        }
        public static DataTable getByNguoiDungMonHocAndBlockHoc(int NguoiDungMonHocID, int BlockHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_CaHoc_getByNguoiDungMonHocAndBlockHoc", NguoiDungMonHocID, BlockHocID).Tables[0];
        }
        public static int insert(int NguoiDungMonHocID, int BlockHocID,  string ten, string MoTa, DateTime Ngay,int GioBatDau,int PhutBatDau,int GioKetThuc,int PhutKetThuc, int Type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLHPM_CaHoc_insert", NguoiDungMonHocID, BlockHocID, ten, MoTa,  Ngay, GioBatDau,PhutBatDau,GioKetThuc,PhutKetThuc, Type);
        }
        public static void update(int iID, int NguoiDungMonHocID, int BlockHocID, string ten, string MoTa, DateTime Ngay,int GioBatDau, int PhutBatDau, int GioKetThuc, int PhutKetThuc, int Type)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLHPM_CaHoc_update", iID, NguoiDungMonHocID, BlockHocID, ten, MoTa,Ngay, GioBatDau, PhutBatDau, GioKetThuc, PhutKetThuc, Type);
        }
    }
}
