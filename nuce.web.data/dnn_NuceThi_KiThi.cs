using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_KiThi
    {
        public static DataTable get(int KiThiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_get", KiThiID).Tables[0];
        }
        public static DataTable getName(int KiThiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_getName", KiThiID).Tables[0];
        }
        public static DataTable getByNguoiDungMonHocAndBlockHoc(int NguoiDungMonHocID,int BlockHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_KiThi_getByNguoiDungMonHocAndBlockHoc", NguoiDungMonHocID,BlockHocID).Tables[0];
        }
        public static void update(int iID,int MonHocID,int BlockHocID, int BoDeID, string ten, string MoTa)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_update", iID, MonHocID, BlockHocID, BoDeID,ten,MoTa);
        }
        public static void update1(int iID, int MonHocID, int BlockHocID, int BoDeID, string ten, string MoTa,int Type)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_update1", iID, MonHocID, BlockHocID, BoDeID, ten, MoTa,Type);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_update_status", iID, status);
        }
        public static int insert(int MonHocID, int BlockHocID, int BoDeID, string ten, string MoTa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_KiThi_insert", MonHocID, BlockHocID, BoDeID, ten, MoTa);
        }
        public static int insert1(int MonHocID, int BlockHocID, int BoDeID, string ten, string MoTa,int Type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_KiThi_insert1", MonHocID, BlockHocID, BoDeID, ten, MoTa, Type);
        }
    }
}
