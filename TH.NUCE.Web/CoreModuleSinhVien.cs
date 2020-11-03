using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Log.EventLog;
using System;
using System.Collections.Generic;
using System.Data;

namespace nuce.web.sinhvien
{
    public class CoreModule : PortalModuleBase
    {
        public model.SinhVien m_SinhVien;
        public Dictionary<int,model.KiThiLopHocSinhVien> m_KiThiLopHocSinhViens;
        public Dictionary<int, model.CaLopHocSinhVien> m_CaLopHocSinhViens;
        protected override void OnInit(EventArgs e)
        {
            m_KiThiLopHocSinhViens = new Dictionary<int, model.KiThiLopHocSinhVien>();
            m_CaLopHocSinhViens= new Dictionary<int, model.CaLopHocSinhVien>();
            if (Session[Utils.session_sinhvien] == null)
            {
                //Chuyển đến trang đăng nhập
                Response.Redirect(string.Format("/tabid/{0}/default.aspx", Utils.tab_login_sinhvien));
            }
            m_SinhVien = (model.SinhVien)Session[Utils.session_sinhvien];
            if (Session[Utils.session_kithi_lophoc_sinhvien] != null)
                m_KiThiLopHocSinhViens = (Dictionary<int, model.KiThiLopHocSinhVien>)Session[Utils.session_kithi_lophoc_sinhvien];
            if (Session[Utils.session_ca_lophoc_sinhvien] != null)
                m_CaLopHocSinhViens = (Dictionary<int, model.CaLopHocSinhVien>)Session[Utils.session_ca_lophoc_sinhvien];
            base.OnInit(e);
        }
        public void writeLog(string type,string log)
        {
            EventLogController eventLog = new EventLogController();
            DotNetNuke.Services.Log.EventLog.LogInfo logInfo = new LogInfo();
            logInfo.LogUserID = UserId;
           
            logInfo.LogPortalID = PortalSettings.PortalId;
            logInfo.LogTypeKey = EventLogController.EventLogType.ADMIN_ALERT.ToString();
            logInfo.AddProperty("tabid=", this.TabId.ToString());
            logInfo.AddProperty("moduleid=", this.ModuleId.ToString());
            logInfo.AddProperty("Loai=", type);
            logInfo.AddProperty("ThongTin=", log);
            eventLog.AddLog(logInfo);
        }
    }
}