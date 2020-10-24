using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_SinhVien
    {
        public static DataTable get(int SinhVienID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_SinhVien_get", SinhVienID).Tables[0];
        }
        public static DataTable dangnhap(string MaSV,string MatKhau)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_SinhVien_dangnhap", MaSV, MatKhau).Tables[0];
        }
        public static DataTable getByLopQuanLy(int LopQuanLyID,int Type,int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_SinhVien_getByLopQuanLy", LopQuanLyID,Type,Status).Tables[0];
        }
        public static DataTable getByMaLopQuanLy(string Ma, int Type, int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_SinhVien_getByMaLopQuanLy", Ma, Type, Status).Tables[0];
        }
        public static void update(int iID,int LopQuanLyID,string Ma,string txtTen,string Ho,string QueQuan,DateTime NgayThangNamSinh,string txtMoTa,DateTime NgayBatDau,DateTime NgayKetThuc,int Order )
        {
            /*
	            @SinhVienID int,
	            @LopQuanLyID int,
	            @MaSV varchar(20),
	            @Ten nvarchar(100),
	            @QueQuan nvarchar(200),
	            @NgayThangNamSinh datetime,
	            @MoTa nvarchar(500),
	            @NgayBatDau datetime,
	            @NgayKetThuc datetime*/
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_SinhVien_update", iID, LopQuanLyID, Ma, txtTen,Ho, QueQuan, NgayThangNamSinh,txtMoTa, NgayBatDau, NgayKetThuc, Order);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_SinhVien_update_status", iID, status);
        }
        public static void updateMatKhau(int iID, string matkhau)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_SinhVien_update_matkhau", iID, matkhau);
        }
        public static bool doimatkhau(int iID, string matkhau,string matkhaumoi)
        {
           return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<bool>("NuceCommon_SinhVien_doimatkhau", iID, matkhau, matkhaumoi);
        }
        public static int insert(int LopQuanLyID, string Ma, string txtTen,string Ho, string QueQuan, DateTime NgayThangNamSinh, string txtMoTa, DateTime NgayBatDau, DateTime NgayKetThuc,int Order)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_SinhVien_insert", LopQuanLyID, Ma, txtTen,Ho, QueQuan, NgayThangNamSinh, txtMoTa, NgayBatDau, NgayKetThuc, Order);
        }
    }
}
