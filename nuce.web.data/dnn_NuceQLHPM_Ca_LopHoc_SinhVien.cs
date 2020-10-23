using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLHPM_Ca_LopHoc_SinhVien
    {
        public static DataTable getBySinhVien(int SinhVienID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_SinhVien_getBySinhVien", SinhVienID).Tables[0];
        }
        public static int xacthucmac(int ID, int CaLopHocID,int SinhVienID, string Mac)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_KiThi_LopHoc_SinhVien_XacThucMac", ID,CaLopHocID,SinhVienID,Mac);
        }
        public static void HuyXacThucMac(int iID)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_KiThi_LopHoc_SinhVien_HuyXacThucMac", iID);
        }
        public static DataTable get(int ID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_SinhVien_get", ID).Tables[0];
        }
        public static DataTable getMay(int Ca_LopHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_SinhVien_getMay", Ca_LopHocID).Tables[0];
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLHPM_Ca_LopHoc_SinhVien_update_status", iID, status);
        }
        public static DataTable getByCaLopHoc(int iID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLHPM_Ca_LopHoc_SinhVien_getByCaLopHoc", iID).Tables[0];
        }
        public static int insert(int CaLopHocID, string MaSV , string Mac, int Type,int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLHPM_Ca_LopHoc_SinhVien_insert", CaLopHocID, MaSV, Mac, Type,Status);
        }
        public static int update(int ID,int CaLopHocID, string MaSV, string Mac, int Type, int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceQLHPM_Ca_LopHoc_SinhVien_update",ID, CaLopHocID, MaSV, Mac, Type, Status);
        }
    }
}
