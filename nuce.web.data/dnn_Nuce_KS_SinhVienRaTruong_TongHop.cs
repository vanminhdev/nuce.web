using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_Nuce_KS_SinhVienRaTruong_TongHop
    {
        public static DataTable getXuatDuLieuSinhVienSauKhaoSatByMaCNganh(string maCnganh)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_XuatDuLieuSinhVienSauKhaoSatByMaCNganh");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql, maCnganh).Tables[0];
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DataSet TSCNganh_GetData()
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return null;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_TSCNganh_GetData");
                    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(objConnection, strSql);

                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
