using DotNetNuke.Entities.Modules;
using nuce.web.data;
using System;
using System.Data;
using Nuce.CTSV.Model;
using System.Collections.Generic;

namespace nuce.ad.ctsv
{
    public partial class DanhSachDichVu : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Lấy danh sách dịch vụ
                string sql = "select -1 as ID, -1 as Status, -1 as Type ";
                foreach (LoaiDichVu t_LoaiDichVu in DataUtils.LoaiDichVuSinhViens.Values)
                {
                    sql += string.Format(" union select ID,Status,{0} as Type from {1} where status>1", t_LoaiDichVu.ID, t_LoaiDichVu.Param1);
                }
                DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Nuce_Common.ConnectionString, CommandType.Text, sql).Tables[0];
                string strHtml = "";
                int i = 0;
                int tongso = 0;
                int moigui = 0;
                int dangxuly = 0;
                int daxulyxong = 0;
                foreach (LoaiDichVu t_LoaiDichVu in DataUtils.LoaiDichVuSinhViens.Values)
                {
                    tongso = 0;
                    moigui = 0;
                    dangxuly = 0;
                    daxulyxong = 0;
                    getGiaTri(dt, t_LoaiDichVu.ID.ToString(), out tongso, out moigui, out dangxuly, out daxulyxong);
                    i++;
                    strHtml += "<tr>";
                    strHtml += string.Format("<td style=\"text-align:center; vertical-align:top;\">{0}</td>", i);
                    strHtml += string.Format("<td style=\"text-align:left; vertical-align:top;\"><a href='{1}'>{0}</a></td>", t_LoaiDichVu.Name, t_LoaiDichVu.Param2);
                    strHtml += string.Format("<td style=\"text-align:center; vertical-align:top;\">{0}</td>", tongso);
                    strHtml += string.Format("<td style=\"text-align:center; vertical-align:top;\">{0}</td>", moigui);
                    strHtml += string.Format("<td style=\"text-align:center; vertical-align:top;\">{0}</td>", dangxuly);
                    strHtml += string.Format("<td style=\"text-align:center; vertical-align:top;\">{0}</td>", daxulyxong);
                    strHtml += "</tr>";
                }
                tbContent.InnerHtml = strHtml;
            }
        }
        private void getGiaTri(DataTable Input,string type,out int tongso,out int moigui,out int dangxuly,out int daxulyxong)
        {
            tongso = 0;
            moigui = 0;
            dangxuly = 0;
            daxulyxong = 0;
            for(int i=0;i<Input.Rows.Count;i++)
            {
                if(Input.Rows[i]["Type"].ToString().Equals(type))
                {
                    tongso++;
                    int iStatus = int.Parse(Input.Rows[i]["Status"].ToString());
                    switch(iStatus)
                    {
                        case 2: moigui++;break;
                        case 4:
                        case 5:
                        case 6:
                            daxulyxong++; break;
                        case 3:dangxuly++;break;
                        default:break;
                    }
                }
            }
        }
    }
}