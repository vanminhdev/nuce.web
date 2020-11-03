using System;
using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_Nuce_KS_SinhVienSapRaTruong_BaiKhaoSat_SinhVien1
    {
        public static DataTable get(int ID)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_SinhVien_get");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, ID).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getBySv(int SvID)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_BaiKhaoSat_SinhVien_GetBySV");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, SvID).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataTable getByStatus(int Status,int Type)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_BaiKhaoSat_SinhVien_GetByStatus");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, Status, Type).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static void update_bailam1(int iID, string BaiLam,  DateTime NgayGioNopBai, string LogIP, int Status)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return ;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_BaiKhaoSat_SinhVien_update_bailam1");
                     Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, iID, BaiLam, NgayGioNopBai, LogIP, Status);

                }
                catch
                {
                    return ;
                }
            }
        }
        public static void update_Type(int iID, int Type)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_BaiKhaoSat_SinhVien_update_type");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, iID, Type);

                }
                catch
                {
                    return;
                }
            }
        }
        public static void SinhVien_THANGHN_insert(int Value, string Desc)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_SinhVien_THANGHN_insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, Value,Desc);

                }
                catch
                {
                    return;
                }
            }
        }

        public static void SinhVien_THANGHN_insert1(int Value, string Desc)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienSapRaTruong_SinhVien_THANGHN_insert1");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, Value, Desc);

                }
                catch
                {
                    return;
                }
            }
        }
    }
}
