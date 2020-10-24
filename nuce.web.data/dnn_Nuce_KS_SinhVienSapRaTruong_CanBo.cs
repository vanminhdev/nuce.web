using System;
using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_Nuce_KS_SinhVienSapRaTruong_CanBo
    {
        public static DataTable checkLogin(string tendangnhap, string psw)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_CanBo_checkLogin");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, tendangnhap, psw).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable SearchSV(string MaSV)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_SearchSV");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, MaSV).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable SearchSVByCanBo(string MaSV,string MaCB)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_SearchSVByCanBo");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, MaSV, MaCB).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static int DoiMatKhau(string maCB, string OldPsw, string NewPsw)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_CanBo_DoiMatKhau");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, maCB, OldPsw, NewPsw);

                }
                catch
                {
                    return -1;
                }
            }
        }

        #region cuusinhvien
        //danh sach sinh vien lam bai theo khoa
        public static DataTable CuuSinhVienSearchSVByMAKhoa(string MAKhoa)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"select a.ex_masv,a.tensinhvien,a.lopqd, b.* from [dbo].[dnn_Nuce_KS_SinhVienRaTruong_SinhVien] a inner join 
(select SinhVienID,LogIP,NgayGioNopBai from [dbo].[dnn_Nuce_KS_SinhVienRaTruong_BaiKhaoSat_SinhVien]
where Status=3 and [SinhVienRaTruong_BaiKhaoSatID]=3) b on a.ID=b.SinhVienID
inner join [dbo].[dnn_Nuce_KS_SinhVienRaTruong_SV_2018] c on a.ex_masv=c.masv
where c.MAKHOA='{0}' order by NgayGioNopBai desc", MAKhoa);
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection,CommandType.Text, strSql).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable SinhVienRaTruong_GetTK()
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, CommandType.StoredProcedure, "dnn_Nuce_KS_SinhVienRaTruong_GetTK").Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
