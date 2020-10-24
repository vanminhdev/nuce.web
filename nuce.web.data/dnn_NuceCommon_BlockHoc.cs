using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_BlockHoc
    {
        public static DataTable get(int BlockHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_BlockHoc_get", BlockHocID).Tables[0];
        }
        public static DataTable getNameActive()
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_BlockHoc_getNameActive").Tables[0];
        }
        public static DataTable getByNamHoc(int NamHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_BlockHoc_getByNamHoc", NamHocID).Tables[0];
        }
        public static DataTable getNameByNamHoc(int NamHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_BlockHoc_getNameByNamHoc", NamHocID).Tables[0];
        }
        public static void update(int iID, int NamHocID, string ten, string mota, DateTime ngaybatdau, DateTime ngayketthuc)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_BlockHoc_update", iID, NamHocID, ten, mota, ngaybatdau, ngayketthuc);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_BlockHoc_update_status", iID, status);
        }
        public static int insert(int NamHocID, string ten, string mota, DateTime ngaybatdau, DateTime ngayketthuc)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_BlockHoc_insert", NamHocID, ten, mota, ngaybatdau, ngayketthuc);
        }
    }
}
