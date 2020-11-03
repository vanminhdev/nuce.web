using System;
using DotNetNuke.Entities.Modules;
namespace nuce.web.commons
{
    public partial class QuanLySinhVienTrongLopHoc : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spLinkImport.InnerText = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "import", "mid/" + this.ModuleId.ToString() + "/lophocid/");
            }
        }
    }
}