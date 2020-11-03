using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_ToaNha
    {
        public static DataTable get(int ToaNhaID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_ToaNha_get", ToaNhaID).Tables[0];
        }
    }
}
