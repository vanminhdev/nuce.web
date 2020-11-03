using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_NamHoc
    {
        public static DataTable get(int NamHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_NamHoc_get", NamHocID).Tables[0];
        }
        public static DataTable getByStatus(int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_NamHoc_get_byStatus", Status).Tables[0];
        }
        public static DataTable getName(int NamHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_NamHoc_getName", NamHocID).Tables[0];
        }
        public static void update(int iID, string ten, string mota, DateTime ngaybatdau, DateTime ngayketthuc)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_NamHoc_update", iID, ten, mota, ngaybatdau, ngayketthuc);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_NamHoc_update_status", iID, status);
        }
        public static int insert(string ten, string mota, DateTime ngaybatdau, DateTime ngayketthuc)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_NamHoc_insert", ten, mota, ngaybatdau, ngayketthuc);
        }
    }
}
