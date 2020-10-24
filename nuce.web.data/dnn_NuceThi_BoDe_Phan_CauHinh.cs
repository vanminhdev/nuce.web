using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_BoDe_Phan_CauHinh
    {
        //tinhToanSoCauHoi
        public static DataTable tinhToanSoCauHoi(int BoDeID,int BoCauHoiID,int DoKho,int iBoDePhanCauHinh)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_Phan_CauHinh_tinhToanSoCauHoi", BoDeID, BoCauHoiID, DoKho, iBoDePhanCauHinh).Tables[0];
        }
        public static DataTable get(int BoDe_Phan_CauHinhID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_Phan_CauHinh_get", BoDe_Phan_CauHinhID).Tables[0];
        }
        public static DataTable getByBoDe_Phan(int ByBoDe_PhanID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_Phan_CauHinh_getByBoDe_Phan", ByBoDe_PhanID).Tables[0];
        }
        /*
         * 		@BoDe_Phan_CauHinhID int,
	            @BoDe_PhanID int,
	            @BoCauHoiID int,
	            @DoKhoID int,
	            @SoCauHoi int,
	            @Type int
         */
        public static void update(int iID, int BoDe_PhanID, int BoCauHoiID, int DoKhoID,
            int SoCauHoi, int Type)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_BoDe_Phan_CauHinh_update", iID, BoDe_PhanID, BoCauHoiID, DoKhoID
                , SoCauHoi, Type);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_BoDe_Phan_CauHinh_update_status", iID, status);
        }
        public static int insert(int BoDe_PhanID, int BoCauHoiID, int DoKhoID,
            int SoCauHoi, int Type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_BoDe_Phan_CauHinh_insert", BoDe_PhanID, BoCauHoiID, DoKhoID
                , SoCauHoi, Type);
        }
    }
}
