using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceThi_CauHoi
    {
        public static DataTable get(int CauHoiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_get", CauHoiID).Tables[0];
        }
        public static DataTable getByMa(string Ma)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_getByMa", Ma).Tables[0];
        }
        public static DataTable getByBoCauHoi(int BoCauHoiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_getByBoCauHoi", BoCauHoiID).Tables[0];
        }
        public static DataTable getByBoCauHoi(int BoCauHoiID, string TuKhoa)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_getByBoCauHoi1", BoCauHoiID, TuKhoa).Tables[0];
        }
        public static DataTable getByBoCauHoiAndDoKho(int BoCauHoiID, int DoKhoID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_getByBoCauHoiAndDoKho", BoCauHoiID, DoKhoID).Tables[0];
        }
        public static int countByBoCauHoiAndDoKho(int BoCauHoiID, int DoKhoID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_CauHoi_countByBoCauHoiAndDoKho", BoCauHoiID, DoKhoID);
        }
        public static int countByBoCauHoiAndDoKho(int BoCauHoiID, int DoKhoID,int Level)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_CauHoi_countByBoCauHoiDoKhoAndLevel", BoCauHoiID, DoKhoID, Level);
        }
        public static DataTable getNameByBoCauHoi(int BoCauHoiID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_getNameByBoCauHoi", BoCauHoiID).Tables[0];
        }

        public static DataTable thongKeSoCauHoiTheoNguoiDungMonHoc(int NguoiDungMonHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_thongKeSoCauHoiTheoNguoiDungMonHoc", NguoiDungMonHocID).Tables[0];
        }

        public static DataTable getNameByBoCauHoiEx(int BoCauHoiID, int Type, int Level)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceThi_CauHoi_getNameByBoCauHoiEx", BoCauHoiID, Type, Level).Tables[0];
        }

        public static void update(int iID, int BoCauHoiID, int DoKhoID, string Ma, string NoiDung, string Image, string Media_Url, string Media_Setting
            , float Mark, float MarkFail, bool IsOption, int PartId, string Explain, int ParentQuestionId, int NoAnswareOnRow,
            DateTime DateExpired, int TimeVisibleAnswer, int InsertedUser, int UpdatedUser, DateTime InsertedDate, DateTime UpdatedDate
            , int Order, int Level, int Type)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_CauHoi_update", iID, BoCauHoiID, DoKhoID, Ma, NoiDung, Image, Media_Url,
                Media_Setting, Mark, MarkFail, IsOption, PartId, Explain, ParentQuestionId, NoAnswareOnRow,
                DateExpired, TimeVisibleAnswer, InsertedUser, UpdatedUser, InsertedDate, UpdatedDate,
                Order, Level, Type);
        }
        /*
      
  @Image nvarchar(250),
  @Media_Url nvarchar(250),
  @Media_Setting nvarchar(500),
  @Mark int,
  @MarkFail int,
  @IsOption int,
  @PartId int,
  @Explain nvarchar(max),
  @ParentQuestionId int,
  @NoAnswerOnRow int,
  @DateExpired datetime,
  @TimeVisibleAnswer int,
  @InsertedUser int,
  @UpdatedUser int,
  @InsertedDate Datetime,
  @UpdatedDate Datetime,
  @Order int,
  @Level int,
  @Type int,
  @Status int*/
        public static int insert(
   int BoCauHoiID, int DoKhoID, string Ma, string NoiDung, string GiaiThich, int Parent, string CheckImport, int Order, int Level, int Type, int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_CauHoi_insert1",
                BoCauHoiID, DoKhoID, Ma, NoiDung, GiaiThich, Parent, CheckImport,
                Order, Level, Type, Status);
        }

        public static int insert(int BoCauHoiID, int DoKhoID, string Ma, string NoiDung, string Image, string Media_Url, string Media_Setting
    , float Mark, float MarkFail, bool IsOption, int PartId, string Explain, int ParentQuestionId, int NoAnswareOnRow,
    DateTime DateExpired, int TimeVisibleAnswer, int InsertedUser, int UpdatedUser, DateTime InsertedDate, DateTime UpdatedDate
    , int Order, int Level, int Type, int Status)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceThi_CauHoi_insert", BoCauHoiID, DoKhoID, Ma, NoiDung, Image, Media_Url,
                Media_Setting, Mark, MarkFail, IsOption, PartId, Explain, ParentQuestionId, NoAnswareOnRow,
                DateExpired, TimeVisibleAnswer, InsertedUser, UpdatedUser, InsertedDate, UpdatedDate,
                Order, Level, Type, Status);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_CauHoi_update_status", iID, status);
        }
        public static void updateAllStatus(int BoCauHoiID, string TuKhoa, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_CauHoi_updateall_status", BoCauHoiID, TuKhoa, status);
        }
        public static void updateAllStatus(int BoCauHoiID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_CauHoi_updateall_status1", BoCauHoiID, status);
        }
        public static void updatePath(int iID, string Path)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceThi_CauHoi_update_path", iID, Path);
        }
    }
}
