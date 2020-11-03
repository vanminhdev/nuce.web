using System;
using System.Data;
using System.Data.SqlClient;

namespace nuce.web.data
{
    public static class dnn_Nuce_KS_SinhVienRaTruong_ReportTotal1
    {
        /*	@SinhVienRaTruong_BaiKhaoSat_SinhVienID int,
	@SinhVienRaTruong_BaiKhaoSatID int,
	@SinhVienID int,
	@QuestionType int,
	@QuestionId int,
	@AnswerId int,
	@Total int ,
	@Value nvarchar(max)*/
        public static void insert(int SinhVienRaTruong_BaiKhaoSat_SinhVienID, int SinhVienRaTruong_BaiKhaoSatID,
            int SinhVienID,string QuestionType,int QuestionID,int AnswerId,int Total,string Value)
        {
            using (SqlConnection objConnection = Nuce_Common.GetConnection())
            {
                if (objConnection == null)
                    return;
                try
                {
                    //Execute select command
                    string strSql = string.Format(@"dnn_Nuce_KS_SinhVienRaTruong_ReportTotal_insert");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(objConnection, strSql, SinhVienRaTruong_BaiKhaoSat_SinhVienID, SinhVienRaTruong_BaiKhaoSatID
                        , SinhVienID, QuestionType, QuestionID, AnswerId, Total, Value);

                }
                catch(Exception ex)
                {
                    return;
                }
            }
        }
    }
}
