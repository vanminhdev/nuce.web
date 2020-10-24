using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_BoDe_Phan
    {
        public static DataTable get(int BoDe_PhanID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_Phan_get", BoDe_PhanID).Tables[0];
        }
        public static DataTable getByBoDe(int BoDeID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_BoDe_Phan_getByBoDe", BoDeID).Tables[0];
        }
        /*
         * 	@BoDe_PhanID int,
	        @BoDeID int,
	        @Name varchar(50),
	        @MoTa nvarchar(500),
	        @Order int,
	        @Type int
         */
        public static void update(int iID, int BoDeID, string Name, string MoTa, int Order,int Type)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_BoDe_Phan_update", iID, BoDeID, Name, MoTa, Order, Type);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_BoDe_Phan_update_status", iID, status);
        }
        public static int insert(int BoDeID, string Name, string MoTa, int Order, int Type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_BoDe_Phan_insert", BoDeID, Name, MoTa, Order, Type);
        }
    }
}
