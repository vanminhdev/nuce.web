using System;
using System.Web;
namespace nuce.web.commons
{
    public class CoreHandlerCommonAdmin : IHttpHandler
    {
        protected DotNetNuke.Entities.Modules.ModuleInfo objModuleInfo;
        protected DotNetNuke.Entities.Users.UserInfo objUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            #region ViewPermission
            // Truyen vao tabid va mid de he thong kiem tra
            // Neu User dang truy cap co quyen thi he thong tra ra du lieu
            // Neu user khong co quyen he thong se thong bao not define
            try
            {
                DotNetNuke.Entities.Portals.PortalSettings portalSettings = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings();
                if (!context.Request.IsAuthenticated)
                {
                    WriteDataError(context, "NotAuthenticated");
                    return;
                }
                // get TabId
                int TabId = Utils.tabCheckCommon;

                // get ModuleId
                int ModuleId = Utils.moduleCheckCommon;

                objUserInfo = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo();
                DotNetNuke.Entities.Modules.ModuleController mc = new DotNetNuke.Entities.Modules.ModuleController();
                System.Collections.Hashtable settings = mc.GetModuleSettings(ModuleId);
                objModuleInfo = new DotNetNuke.Entities.Modules.ModuleController().GetModule(ModuleId, TabId);
                if (DotNetNuke.Security.Permissions.ModulePermissionController.CanViewModule(objModuleInfo))
                {
                    try
                    {
                        WriteData(context);
                        return;
                    }
                    catch (Exception ex)
                    {
                        WriteDataError(context, ex.Message);
                        return;
                    }
                }
                else
                {
                    WriteDataError(context, "NotPermission");
                    return;
                }
            }
            catch (Exception ex)
            {
                WriteDataError(context, ex.Message);
                return;
            }
            #endregion
        }
        public virtual void WriteData(HttpContext context)
        {
        }
        public virtual void WriteDataError(HttpContext context, string strData)
        {
            //context.Response.ContentType = "text/xml";
            context.Response.Write(strData);
            //context.Response.End();
            //context.Response.Write(strData);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}