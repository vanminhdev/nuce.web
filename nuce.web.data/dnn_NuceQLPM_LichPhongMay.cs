using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLPM_LichPhongMay
    {
        public static void Insert(string MADK,string MACB,string HoVaTen,string Lop,string MonHoc,string Thu,
            string TietBD,string SoTiet,string MaPH,int PhongHocID,int BuoiHoc,string TuanHoc,
            DateTime Ngay,int CahocID,int HocKyID,int NamHocID,int SoSinhVien,string TTThemCB,
            string MoTa,string GhiChu,int Type,int Status)
        {
             DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_LichPhongMay_insert", 
                MADK,MACB,HoVaTen, Lop, MonHoc,Thu, TietBD,SoTiet,MaPH,PhongHocID,BuoiHoc,TuanHoc
                ,Ngay,CahocID,HocKyID,NamHocID,SoSinhVien,TTThemCB,MoTa,GhiChu,Type,Status);
        }
        public static int Insert_DangKi(int UserId,string MADK, string MACB, string HoVaTen, string Lop, string MonHoc, string Thu,
    string TietBD, string SoTiet, string MaPH, int PhongHocID, int BuoiHoc, string TuanHoc,
    DateTime Ngay, int CahocID, int HocKyID, int NamHocID, int SoSinhVien, string TTThemCB,
    string MoTa, string GhiChu, int Type, int Status,bool CheckCapNhat)
        {
           return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLPM_LichPhongMay_insertDangKi", UserId,
               MADK, MACB, HoVaTen, Lop, MonHoc, Thu, TietBD, SoTiet, MaPH, PhongHocID, BuoiHoc, TuanHoc
               , Ngay, CahocID, HocKyID, NamHocID, SoSinhVien, TTThemCB, MoTa, GhiChu, Type, Status, CheckCapNhat);
        }
        public static int UpdateDangKi(int UserID,int ID,string MADK, string MACB, string HoVaTen, string Lop, string MonHoc, string Thu,
 string TietBD, string SoTiet, string MaPH, int PhongHocID, int BuoiHoc, string TuanHoc,
 DateTime Ngay, int CahocID, int HocKyID, int NamHocID, int SoSinhVien, string TTThemCB,
 string MoTa, string GhiChu, int Type, int Status, bool CheckCapNhat)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLPM_LichPhongMay_UpdateDangKi", UserID,ID,
                MADK, MACB, HoVaTen, Lop, MonHoc, Thu, TietBD, SoTiet, MaPH, PhongHocID, BuoiHoc, TuanHoc
                , Ngay, CahocID, HocKyID, NamHocID, SoSinhVien, TTThemCB, MoTa, GhiChu, Type, Status, CheckCapNhat);
        }
        public static int UpdateStatus(int UserID, int ID,string GhiChu, int Type, int Status, bool CheckCapNhat)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLPM_LichPhongMay_UpdateStatus", UserID, ID,GhiChu, Type, Status, CheckCapNhat);
        }
        public static DataTable get(DateTime NgayBatDau, DateTime NgayKetThuc,int Type, int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_LichPhongMay_get", NgayBatDau, NgayKetThuc, Type, Status).Tables[0];
        }
        public static DataTable get(DateTime NgayBatDau, DateTime NgayKetThuc)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_LichPhongMay_getAll", NgayBatDau, NgayKetThuc).Tables[0];
        }
        public static DataTable getByUpdatedDate(DateTime NgayBatDau, DateTime NgayKetThuc,int PhongID, int Type, int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_LichPhongMay_getUpdateDate", NgayBatDau, NgayKetThuc, PhongID, Type, Status).Tables[0];
        }
        public static DataTable getByUpdatedDate(DateTime NgayBatDau, DateTime NgayKetThuc, int PhongID, int Type, int Status,int UserID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_LichPhongMay_getUpdateDateByUser", NgayBatDau, NgayKetThuc, PhongID, Type, Status, UserID).Tables[0];
        }
        public static DataTable get(int ID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_LichPhongMay_getByID", ID).Tables[0];
        }
        public static int Discard(int ID, string NoiDung, int UserID, string UserName)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLPM_LichPhongMay_Discard",ID, NoiDung, UserID,DateTime.Now, UserName);
        }
        public static int Approve(int ID, string NoiDung, int UserID, string UserName)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLPM_LichPhongMay_Approve", ID, NoiDung, UserID, DateTime.Now, UserName);
        }
    }
}
