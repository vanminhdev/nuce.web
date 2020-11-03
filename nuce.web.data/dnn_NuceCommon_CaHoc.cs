using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_CaHoc
    {
        public static DataTable get(int CaHocID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_CaHoc_get", CaHocID).Tables[0];
        }
    }
}
