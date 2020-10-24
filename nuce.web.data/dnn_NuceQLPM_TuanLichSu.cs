using System;
using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceQLPM_TuanLichSu
    {
        public static DataTable search(DateTime tungay,DateTime denngay,int phonghocid)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceQLPM_TuanLichSu_Search", tungay, denngay, phonghocid).Tables[0];
        }
       
    }
}
