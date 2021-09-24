using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
