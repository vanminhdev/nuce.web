using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_LopHoc
    {
        public static DataTable getByMonHoc(int monhocid)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopHoc_getByMonHoc", monhocid).Tables[0];
        }
        public static DataTable getByMonHoc(int monhocid,int UserID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopHoc_getByMonHocAndUser", monhocid,UserID).Tables[0];
        }
        public static DataTable getNameByStatus(int status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopHoc_getNameByStatus", status).Tables[0];
        }
        public static DataTable getNameByStatus(int status,int UserID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopHoc_getNameByStatusAndUser", status,UserID).Tables[0];
        }
        public static DataTable get(int LopHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopHoc_get", LopHocID).Tables[0];
        }
        public static DataTable get1(int LopHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopHoc_get1", LopHocID).Tables[0];
        }
        public static DataTable getByNguoiDungMonHocAndBlock(int NguoiDungMonHocID,int BlockID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopHoc_getByNguoiDungMonHocAndBlock", NguoiDungMonHocID, BlockID).Tables[0];
        }
        public static void update(int iID,int MonHocID,int UserID,int BlockID,string Ma,string txtTen,int SoChi,string txtMoTa,DateTime NgayBatDau,DateTime NgayKetThuc )
        {
            /*
            	@LopHocID int,
                @MonHocID int,
                @UserID int,
                @BlockID int,
                @Ma varchar(20),
                @Ten nvarchar(100),
                @SoChi int,
                @MoTa nvarchar(1000),
                @NgayBatDau datetime,
                @NgayKetThuc datetime*/
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_LopHoc_update", iID, MonHocID, UserID,BlockID, Ma, txtTen,SoChi, txtMoTa, NgayBatDau, NgayKetThuc);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_LopHoc_update_status", iID, status);
        }
        public static int insert(int MonHocID, int UserID, int BlockID, string Ma, string txtTen, int SoChi, string txtMoTa, DateTime NgayBatDau, DateTime NgayKetThuc)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_LopHoc_insert", MonHocID, UserID, BlockID, Ma, txtTen, SoChi, txtMoTa, NgayBatDau, NgayKetThuc);
        }

        public static int checkLopHocAndNguoiDung(int LopHocID, int UserID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_LopHoc_checkLopHocAndNguoiDung", LopHocID, UserID);
        }
    }
}
