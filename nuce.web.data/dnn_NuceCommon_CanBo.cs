using System.Data;
namespace nuce.web.data
{
    public static class dnn_NuceCommon_CanBo
    {
        public static DataTable get(int CanBoID)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("NuceCommon_CanBo_get", CanBoID).Tables[0];
        }
        public static int insert(int CanBoID,string MaCB, string TenCB,string MaBM, string CanBoTG, string EmailCB1,
            string EmailCB2, string TelCB1,string TelCB2,string NgaySinh,string ngsinhcb,int isNhanVien)
        {
            return DotNetNuke.Data.DataProvider.Instance().ExecuteScalar<int>("NuceCommon_CanBo_insertSyn", CanBoID, MaCB,
                TenCB, MaBM, CanBoTG, EmailCB1, EmailCB2, TelCB1, TelCB2, NgaySinh, ngsinhcb, isNhanVien);
        }
    }
}
