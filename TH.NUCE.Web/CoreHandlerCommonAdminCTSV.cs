using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;

namespace nuce.web.commons
{
    public class CoreHandlerCommonAdminCTSV : IHttpHandler
    {
        protected DotNetNuke.Entities.Modules.ModuleInfo objModuleInfo;
        protected DotNetNuke.Entities.Users.UserInfo objUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            #region ViewPermission
            try
            {
                if (!context.Request.IsAuthenticated)
                {
                    WriteDataError(context, "NotAuthenticated");
                    return;
                }

                objUserInfo = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo();
                string Role = "CTSV";
                if (checkRole(objUserInfo.Roles,Role))
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
        public bool checkRole(string [] Roles,string Role)
        {
            foreach(string temp in Roles)
            {
                if (temp.Equals(Role))
                    return true;
            }
            return false;
        }
        public static string DataTableToJSONWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
    }
}