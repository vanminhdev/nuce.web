using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_MucDiem
    {
        public static DataTable get(int MucDiemID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_MucDiem_get", MucDiemID).Tables[0];
        }
        public static DataTable getByThangDiem(int ThangDiemID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_MucDiem_getByThangDiem", ThangDiemID).Tables[0];
        }
        public static void update(int iID,int ThangDiemID,int DoKho,float Diem)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_MucDiem_update", iID, ThangDiemID, DoKho, Diem);
        }
        public static void delete(int iID)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_MucDiem_delete", iID);
        }
        public static int insert(int ThangDiemID, int DoKho, float Diem)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_MucDiem_insert", ThangDiemID, DoKho, Diem);
        }
    }
}
