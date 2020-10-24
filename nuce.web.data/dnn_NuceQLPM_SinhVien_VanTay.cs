using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLPM_SinhVien_VanTay
    {
        public static DataTable search(DateTime tungay, DateTime denngay)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_SinhVien_VanTay_get", tungay, denngay).Tables[0];
        }
        public static DataTable search(DateTime tungay, DateTime denngay,int Status,string MaSV)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_SinhVien_VanTay_get1", tungay, denngay, Status, MaSV).Tables[0];
        }
        public static void updateStatus(string MaSv, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_SinhVien_VanTay_update_status", MaSv, status);
        }
        public static void updateMaSV(string MaSV, string newMaSV)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_SinhVien_VanTay_update_MaSV", MaSV, newMaSV);
        }
        public static void delete(string MaSV)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_SinhVien_VanTay_delete", MaSV);
        }
    }
}
