using System.Web;
namespace nuce.web.commons
{
    public class CoreHandlerCommon : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            WriteData(context);
        }

        public virtual void WriteData(HttpContext context)
        {
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