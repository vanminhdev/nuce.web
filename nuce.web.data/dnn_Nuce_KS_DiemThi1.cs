using System;
using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_Nuce_KS_DiemThi1
    {
        public static void OutputText_Batch_Statistics()
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return ;

                using (SqlCommand command = objConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300; 
                    command.CommandText = "dnn_Nuce_KS_OutputText_Batch_Statistics";

                    command.ExecuteNonQuery();
                }
            }
        }

        public static DataSet Statistics_NamHoc()
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_NUCE_KS_DiemThi_LMH_Statistics_NamHoc");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql);

                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataSet getByMH(string code)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_NUCE_KS_DiemThi_LMH_StatisticsMH");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, code);

                }
                catch
                {
                    return null;
                }
            }
        }
        public static int insert(string masv, string mamh, string manh, string diem1, string diem2, string diem3, float fdiem1, float fdiem2, float fdiem3, string namhoc)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_13_14_Insert");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh, diem1, diem2, diem3, fdiem1, fdiem2, fdiem3, namhoc);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int insert34(string masv, string mamh, string manh, string diem1, string diem2, string diem3, float fdiem1, float fdiem2, float fdiem3, string namhoc)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_13_14_Insert");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh, diem1, diem2, diem3, fdiem1, fdiem2, fdiem3, namhoc);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int insert45(string masv, string mamh, string manh, string diem1, string diem2, string diem3, float fdiem1, float fdiem2, float fdiem3, string namhoc)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_14_15_Insert");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh, diem1, diem2, diem3, fdiem1, fdiem2, fdiem3, namhoc);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int insert56(string masv, string mamh, string manh, string diem1, string diem2, string diem3, float fdiem1, float fdiem2, float fdiem3, string namhoc)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_15_16_Insert");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh, diem1, diem2, diem3, fdiem1, fdiem2, fdiem3, namhoc);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int insert67(string masv, string mamh, string manh, string diem1, string diem2, string diem3, float fdiem1, float fdiem2, float fdiem3, string namhoc)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_16_17_Insert");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh, diem1, diem2, diem3, fdiem1, fdiem2, fdiem3, namhoc);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int insert78(string masv, string mamh, string manh, string diem1, string diem2, string diem3, float fdiem1, float fdiem2, float fdiem3, string namhoc)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_17_18_Insert");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh, diem1, diem2, diem3, fdiem1, fdiem2, fdiem3, namhoc);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static DataTable Check34()
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_13_14_Check");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql).Tables[0];

                }
                catch
                {
                    return null;
                }
            }
        }
        public static int Check45(string masv, string mamh, string manh)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_14_15_Check");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int Check56(string masv, string mamh, string manh)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_15_16_Check");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int Check67(string masv, string mamh, string manh)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_16_17_Check");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh);

                }
                catch
                {
                    return -1;
                }
            }
        }
        public static int Check78(string masv, string mamh, string manh)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return -1;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_DiemThi_17_18_Check");
                    return (int)Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(objConnection, strSql, masv, mamh, manh);

                }
                catch
                {
                    return -1;
                }
            }
        }
    }
}
