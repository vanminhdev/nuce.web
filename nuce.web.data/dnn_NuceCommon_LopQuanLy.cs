using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_LopQuanLy
    {
        public static DataTable get(int LopQuanLyID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopQuanLy_get", LopQuanLyID).Tables[0];
        }
        public static DataTable getByKhoa(int KhoaID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopQuanLy_getByKhoa", KhoaID).Tables[0];
        }
        public static int getIDByMa(string Ma)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_LopQuanLy_getIDByMa", Ma);
            
        }
        public static DataTable getNameByKhoa(int KhoaID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_LopQuanLy_getNameByKhoa", KhoaID).Tables[0];
        }
        public static void update(int iID,int KhoaID,int NamHocID,string Ma,string txtTen,string txtMoTa,DateTime NgayBatDau,DateTime NgayKetThuc )
        {
            /*
            	@KhoaID int,
	            @NamHocID int,
	            @Ma varchar(20),
	            @Ten nvarchar(100),
	            @MoTa nvarchar(500),
	            @NgayBatDau datetime,
	            @NgayKetThuc datetime*/
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_LopQuanLy_update", iID, KhoaID, NamHocID, Ma, txtTen, txtMoTa, NgayBatDau, NgayKetThuc);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_LopQuanLy_update_status", iID, status);
        }
        public static void updateKhoa(int iID, int KhoaID)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_LopQuanLy_update_khoa", iID, KhoaID);
        }
        public static int insert(int KhoaID, int NamHocID, string Ma, string txtTen, string txtMoTa, DateTime NgayBatDau, DateTime NgayKetThuc)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_LopQuanLy_insert", KhoaID, NamHocID, Ma, txtTen, txtMoTa, NgayBatDau, NgayKetThuc);
        }
    }
}
