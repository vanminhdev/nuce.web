using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLPM_TuanHienTai
    {
        public static DataTable getByTypeAndPhongHoc(int type,int phonghocid,int status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_TuanHienTai_getByTypeAndPhongHoc", type,phonghocid,status).Tables[0];
        }
        public static DataTable getByType(int tuanhientai, int type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_TuanHienTai_getByType", tuanhientai, type).Tables[0];
        }
        public static DataTable getByType1(int tuanhientai, int type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_TuanHienTai_getByType1", tuanhientai, type).Tables[0];
        }
        public static int taongaytuanketiep(DateTime ngay,string tentuan)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLPM_TuanHienTai_taongaytuanketiep", ngay,tentuan);
        }
        public static int chuyentuanhientai()
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLPM_TuanHienTai_chuyentuanhientai");
        }
        public static void chottuanhientai()
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_TuanHienTai_chottuanhientai");
        }
        public static void update(int tuanhientai,int lophocid,string ghichu)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_TuanHienTai_update",tuanhientai,lophocid,ghichu);
        }

        public static void update1(int tuanhientai,string ten,DateTime ngay,int phonghocid, int cahocid, int lophocid, string ghichu)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_TuanHienTai_update1", tuanhientai, ten,ngay, phonghocid, cahocid, lophocid, ghichu);
        }
        public static void insert( string ten, DateTime ngay, int phonghocid, int cahocid, int lophocid, string ghichu)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_TuanHienTai_insert",  ten, ngay, phonghocid, cahocid, lophocid, ghichu);
        }
        public static void updateten(int type, int status, string tentuan)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_TuanHienTai_updateten", type, status, tentuan);
        }
        public static void updateStatus(int type, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_TuanHienTai_update_status", type, status);
        }
    }
}
