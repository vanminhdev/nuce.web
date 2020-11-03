using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_Nuce_KS_SinhVienRaTruong_BaiKhaoSat_SinhVien
    {
        public static DataTable get(int ID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_SinhVienRaTruong_SinhVien_get", ID).Tables[0];
        }
        public static DataTable getBySv(int SvID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_SinhVienRaTruong_BaiKhaoSat_SinhVien_GetBySV", SvID).Tables[0];
        }
        public static DataTable getByStatus(int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_SinhVienRaTruong_BaiKhaoSat_SinhVien_GetByStatus", Status).Tables[0];
        }
        public static void update_bailam1(int iID, string BaiLam, DateTime NgayGioNopBai, string LogIP, int Status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("Nuce_KS_SinhVienRaTruong_BaiKhaoSat_SinhVien_update_bailam1", iID, BaiLam, NgayGioNopBai, LogIP, Status);
        }
    }
}
