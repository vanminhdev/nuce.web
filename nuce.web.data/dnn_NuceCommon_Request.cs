using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_Request
    {
        public static DataTable getByCodeAndPartner(string Code,string sPartner)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_Request_getByCodeAndPartner", Code, sPartner).Tables[0];
        }
        public static DataTable getByCodeAndStatus(string Code, string sStatus)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_Request_getByCodeAndStatus", Code, sStatus).Tables[0];
        }
        public static int getStatusByCodeAndPartnerID(string Code, int PartnerID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_Request_getStatusByCodeAndPartnerID", Code, PartnerID);
        }
        public static void update(int iID, string ten, string code, string mota, int partner, string data, int insertedUser, int updatedUser,
            DateTime insertedDate, DateTime updatedDate, int type)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_Request_update", iID, ten, code, mota, partner, data, insertedUser
                , insertedDate, updatedDate,type);
        }
        public static void update1(int iID, string ten, string code, string mota, int partner, string data,string dataInput,string dataOutPut, int insertedUser, int updatedUser,
    DateTime insertedDate, DateTime updatedDate, int type)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_Request_update1", iID, ten, code, mota, partner, data,dataInput,dataOutPut, insertedUser
                , insertedDate, updatedDate, type);
        }
        public static void updateStatus(int iID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_Request_update_status", iID, status);
        }
        public static void updateStatus1(string Code, int PartnerID, int status)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceCommon_Request_update_status1", Code, PartnerID, status);
        }
        public static int insert(string ten, string code, string mota, int partner, string data, int insertedUser, int updatedUser,
            DateTime insertedDate, DateTime updatedDate, int type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_Request_insert", ten, code, mota, partner, data, insertedUser, updatedUser
                , insertedDate, updatedDate, type);
        }
        public static int insert1(string ten, string code, string mota, int partner, string data,string dataInput,string dataOutput, int insertedUser, int updatedUser,
    DateTime insertedDate, DateTime updatedDate, int type)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_Request_insert1", ten, code, mota, partner, data,dataInput,dataOutput, insertedUser, updatedUser
                , insertedDate, updatedDate, type);
        }
    }
}
