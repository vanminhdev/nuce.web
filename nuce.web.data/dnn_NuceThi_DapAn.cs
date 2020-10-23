using System.ComponentModel;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_DapAn
    {
        public static DataTable get(int DapAnID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_DapAn_get", DapAnID).Tables[0];
        }
        public static DataTable getByCauHoi(int CauHoiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_DapAn_getByCauHoi", CauHoiID).Tables[0];
        }
        public static DataTable getByBoCauHoiAndDoKho(int BoCauHoiID, int DoKhoID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_DapAn_getByBoCauHoiAndDoKho", BoCauHoiID, DoKhoID).Tables[0];
        }
        public static void update(int iID, int CauHoiID, int SubQuestionId, string Content, bool IsCheck, string Matching,
            float Mark, float MarkFail, int Order)
        {
            DotNetNuke.Data.DataProvider.Instance()
                .ExecuteNonQuery("NuceThi_DapAn_update", iID, CauHoiID, SubQuestionId, Content, IsCheck, Matching
                , Mark, MarkFail, Order);
        }
        /*
        @CauHoiID int,
  @SubQuestionId int,
  @Content nvarchar(max),
  @IsCheck bit,
  @Matching nvarchar(max),
  @Mark float,
  @MarkFail float,
  @Order int,
  @Status int*/
        public static int insert(int CauHoiID, int SubQuestionId,string Content, bool IsCheck,string Matching,
            float Mark,float MarkFail,int Order,int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_DapAn_insert", CauHoiID, SubQuestionId, Content, IsCheck, Matching
                , Mark, MarkFail, Order, Status);
        }
    }
}
