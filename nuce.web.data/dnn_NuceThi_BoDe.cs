using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_BoDe
    {
        public static DataTable get(int BoDeID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_get", BoDeID).Tables[0];
        }
        public static DataTable thongKeSoCauHoiDaDung(int BoDeID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_thongKeSoCauHoiDaDung", BoDeID).Tables[0];
        }
        public static DataTable nhanban(int BoDeID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_nhanban", BoDeID).Tables[0];
        }
        public static DataTable getByMaAndUser(int UserID, string Ma)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_getByMaAndUser", UserID,Ma).Tables[0];
        }
        public static DataTable getByNguoiDung_MonHoc(int NguoiDung_MonHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_getByNguoiDung_MonHoc", NguoiDung_MonHocID).Tables[0];
        }
        public static DataTable getNameBoDeDungThiByNguoiDung_MonHoc(int NguoiDung_MonHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_getNameBoDeDungThiByNguoiDung_MonHoc", NguoiDung_MonHocID).Tables[0];
        }
        /*
         * 	@BoDeID int,
	        @NguoiDung_MonHocID int,
	        @Ma varchar(20),
	        @Ten nvarchar(150),
	        @MoTa nvarchar(500),
	        @ThoiGianThi int,
	        @Type int
         */
        public static void update(int iID, int NguoiDung_MonHocID, string txtMa, string txtTen,string Mota,int ThoiGianThi,int SoDe,int Type)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_BoDe_update", iID, NguoiDung_MonHocID, txtMa, txtTen, Mota, ThoiGianThi,SoDe, Type);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_BoDe_update_status", iID, status);
        }
        public static void update_diemtoida(int iID, float diem)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_BoDe_update_diemtoida", iID, diem);
        }
        public static int insert(int NguoiDung_MonHocID, string txtMa, string txtTen, string Mota, int ThoiGianThi,int SoDe, int Type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_BoDe_insert", NguoiDung_MonHocID, txtMa, txtTen, Mota, ThoiGianThi,SoDe, Type);
        }
        public static int getStatus(int BoDeID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_BoDe_getStatus", BoDeID);
        }
    }
}
