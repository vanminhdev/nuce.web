using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_KiThi_LopHoc_SinhVien
    {
        public static string getMoTa(int iID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<string>("NuceThi_KiThi_LopHoc_SinhVien_getMoTa", iID);
        }
        public static string getMatKhau(int iID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<string>("NuceThi_KiThi_LopHoc_SinhVien_getMatKhau", iID);
        }
        public static DataTable getBySinhVien(int SinhVienID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_SinhVien_getBySinhVien", SinhVienID).Tables[0];
        }
        public static DataTable get(int KiThi_LopHoc_SinhVienID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_SinhVien_get", KiThi_LopHoc_SinhVienID).Tables[0];
        }
        public static DataTable get()
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_SinhVien_get1").Tables[0];
        }
        public static DataTable get(string strID, string includeStatus)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_SinhVien_getBatch", strID, includeStatus).Tables[0];
        }
        public static DataTable getByKiThi_LopHoc(int KiThi_LopHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_SinhVien_getByKiThi_LopHoc", KiThi_LopHocID).Tables[0];
        }
        public static DataTable getByKiThi_LopHoc1(int KiThi_LopHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_LopHoc_SinhVien_getByKiThi_LopHoc1", KiThi_LopHocID).Tables[0];
        }
        public static void update(int iID, int KiThiID, int LopHocID, string PhongThi, string MoTa)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update", iID, KiThiID, LopHocID, PhongThi, MoTa);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_status", iID, status);
        }
        public static void updateStatus1(int iID, int status,string mota)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_status1", iID, status, mota);
        }
        public static void updateStatus2(int iID, int status, string baiLam,float diem)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_status2", iID, status, baiLam,diem);
        }
        //StrID được xếp nhau theo cách ,id,
        public static void updateBatchStatus(string strID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_batchstatus", strID, status);
        }
        //StrID được xếp nhau theo cách ,id,
        public static void updateBatchStatus1(string strID,string includeStatus, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_batchstatus1", strID, includeStatus, status);
        }
        public static void updateMoTa(int iID,  string mota)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_mota", iID, mota);
        }
        public static void updateMatKhau(int iID, string matkhau)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_matkhau", iID, matkhau);
        }
        public static void update_dethi(int iID, int DeThiID,string NoiDungDeThi,string DapAn,int TongThoiGianThi,int TongThoiGianConLai,string MaDe,string LogIP,int Status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_dethi", iID, DeThiID, NoiDungDeThi, DapAn, TongThoiGianThi, TongThoiGianConLai, MaDe,LogIP,Status);
        }
        public static void update_bailam(int iID, string BaiLam, DateTime NgayGioBatDau, DateTime NgayGioNopBai, int TongThoiGianConLai,  string LogIP, int Status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_bailam", iID, BaiLam, NgayGioBatDau,NgayGioNopBai, TongThoiGianConLai, LogIP, Status);
        }
        public static void update_bailam1(int iID, string BaiLam,float Diem, DateTime NgayGioBatDau, DateTime NgayGioNopBai, int TongThoiGianConLai, string LogIP, int Status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_update_bailam1", iID, BaiLam, Diem, NgayGioBatDau, NgayGioNopBai, TongThoiGianConLai, LogIP, Status);
        }
        public static int insert(int KiThiID, int LopHocID, string PhongThi, string MoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_KiThi_LopHoc_SinhVien_insert", KiThiID, LopHocID, PhongThi, MoTa);
        }
        public static void delete(int iID)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_delete", iID);
        }
    }
}
