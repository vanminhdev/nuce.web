using System;
using System.Data;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;
using System.Text;
using ClosedXML.Excel;
using System.Collections.Generic;

namespace nuce.web.ks.analytics
{
    public partial class SynthesizedResults : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            DataSet ds = DotNetNuke.Data.DataProvider.Instance().ExecuteDataSet("Nuce_KS_OutputText_Statistics");
            DataTable dtSummaries = ds.Tables[0];
            DataTable dtGroup = ds.Tables[1];
            DataTable dtKeywords = ds.Tables[2];

            int iTotal = int.Parse(dtSummaries.Rows[0]["total"].ToString());
            int iTotal1 = int.Parse(dtSummaries.Rows[0]["total1"].ToString());
            int iTotal2 = int.Parse(dtSummaries.Rows[0]["total2"].ToString());
            int iTotal3 = int.Parse(dtSummaries.Rows[0]["total3"].ToString());
            string strSummaries = "";
            strSummaries += string.Format("<div style='text - align: left; padding: 5px;'>Số lượng phản hồi tổng cộng: {0}</div>", iTotal);
            strSummaries += string.Format("<div style='text - align: left; padding: 5px;'>Số lượng phản hồi tổng cộng loại câu hỏi 1: {0}</div>", iTotal1);
            strSummaries += string.Format("<div style='text - align: left; padding: 5px;'>Số lượng phản hồi tổng cộng loại câu hỏi 2: {0}</div>", iTotal2);
            strSummaries += string.Format("<div style='text - align: left; padding: 5px;'>Số lượng phản hồi tổng cộng loại câu hỏi 3: {0}</div>", iTotal3);
            divSummary.InnerHtml = strSummaries;
            string strContent = "<table border='1' style='padding: 5px;'><tr><td colspan='8' style='text-align:center;font-weight:bold;'>Bảng thống kê tổng hợp theo Group</td></tr>";
                strContent += "<tr style='text-align:center;font-weight:bold;'><td rowspan='2' width='5%' style='padding-left:3px;'>STT</td>";
                strContent += "<td rowspan='2'>Group</td>";
                strContent += "<td width='30%' colspan='3'>Câu hỏi 2</td>";
                strContent += "<td width='30%' colspan='3'>Câu hỏi 3</td></tr>";
                strContent += "<tr><td  width='5%'>Số lượng</td><td  width='5%'>Tỷ lệ (%)</td><td>Chi tiết</td><td  width='5%'>Số lượng</td><td  width='5%'>Tỷ lệ (%)</td><td>Chi tiết</td></tr>";
            int iGroupID = -1;
            int iValue2 = -1;
            int iValue3 = -1;
            double fValue2 = -1;
            double fValue3 = -1;
            string sValue2 = "";
            string sValue3 = "";
            for (int i=0;i<dtGroup.Rows.Count;i++)
            {
                iGroupID = int.Parse(dtGroup.Rows[i]["ID"].ToString());
                strContent += string.Format("<tr><td  style='text-align:center;vertical-align: top;'>{0}</td>", i+1);
                strContent += string.Format("<td style='padding-left:2px;vertical-align: top;'>{0}</td>", dtGroup.Rows[i]["Name"].ToString());
                iValue2 = getValue(dtKeywords, iGroupID, 2,out sValue2);
                fValue2 = 100*iValue2 / iTotal2;
                iValue3 = getValue(dtKeywords, iGroupID, 3,out sValue3);
                fValue3 = 100*iValue3 / iTotal3;
                strContent += string.Format("<td style='text-align:right;padding-right:2px;vertical-align: top;'>{0}</td>", iValue2);
                strContent += string.Format("<td style='text-align:right;padding-right:2px;vertical-align: top;'>{0:N1}%</td>", fValue2);
                strContent += string.Format("<td style='text-align:left;padding-left:2px;vertical-align: top;'>{0}</td>", sValue2);

                strContent += string.Format("<td style='text-align:right;padding-right:2px;vertical-align: top;'>{0}</td>", iValue3);
                strContent += string.Format("<td style='text-align:right;padding-right:2px;vertical-align: top;'>{0:N1}%</td>", fValue3);
                strContent += string.Format("<td style='text-align:left;padding-left:2px;vertical-align: top;'>{0}</td></tr>", sValue3);
            }

            strContent += "</table>";

            divContent.InnerHtml = strContent;
        }
        private int getValue(DataTable dt,int GroupID,int CauHoi,out string sValue)
        {
            sValue = "";
            int iValue = -1;
            int iReturn = 0;
            DataRow[] drs = dt.Select(string.Format("GroupID={0} and Type={1} and Status=1", GroupID, CauHoi));
            for(int i=0;i<drs.Length;i++)
            {
                iValue= int.Parse(drs[i]["Value"].ToString());
                iReturn += iValue;
                //Description
                sValue += string.Format("{0} <span style='color:red;'>({1});</span> ", drs[i]["Description"].ToString().Trim(), iValue);
            }
            return iReturn;
        }
        protected void btnSynthesized_Click(object sender, EventArgs e)
        {
            nuce.web.data.dnn_Nuce_KS_DiemThi1.OutputText_Batch_Statistics();
            Response.Redirect(Request.RawUrl);
            //DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("Nuce_KS_OutputText_Batch_Statistics");
            //LoadData();
        }

    }
}