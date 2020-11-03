using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLPM_Log_Syn
    {
        public static DataTable getByType(int Type,int Status)
        {
          return  DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_Log_Syn_getByType", Type, Status).Tables[0];
        }
        public static void Insert(string KeyCheck, int Status, int Type, string Description)
        {
            DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("NuceQLPM_Log_Syn_insert",
               KeyCheck, Status, Type, Description);
        }
    }
}
