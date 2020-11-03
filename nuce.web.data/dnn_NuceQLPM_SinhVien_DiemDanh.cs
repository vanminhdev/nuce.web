using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLPM_SinhVien_DiemDanh
    {
        public static int checkstatus(int tuanhientaiid)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLPM_DiemDanh_CheckStatus", tuanhientaiid);
        }
        public static void taodiemdanh(int tuanhientaiid)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_DiemDanh_TaoDiemDanh", tuanhientaiid);
        }
        public static DataTable GetDataByTuanHienTai(int tuanhientaiid)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_DiemDanh_GetDataByTuanHienTai", tuanhientaiid).Tables[0];
        }
        public static DataTable GetDataUsingRequestByTuanHienTai(int tuanhientaiid)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_DiemDanh_GetDataUsingRequestByTuanHienTai", tuanhientaiid).Tables[0];
        }
        public static DataTable GetDataHByTuanHienTai(int tuanhientaiid)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_DiemDanh_GetDataHByTuanHienTai", tuanhientaiid).Tables[0];
        }
        public static void update(int diemdanhngayid,int tuanhientaiid,int sinhvienid,bool isdiemdanh,string ghichu)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_DiemDanh_update", diemdanhngayid, tuanhientaiid, sinhvienid, isdiemdanh, ghichu);
        }
        public static void updateautoall(int tuanhientaiid)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_DiemDanh_updateautoall", tuanhientaiid);
        }
        public static void chotdulieu(int tuanhientaiid)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_DiemDanh_chotdulieu", tuanhientaiid);
        }
    }
}
