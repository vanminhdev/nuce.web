using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.survey.student.Attributes.ActionFilter
{
    public class GraduateActionFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (session["masv"] != null)
            {
                return;
            } 
            else
            {
                filterContext.Result = new RedirectResult("/account/logingraduate");
            }
        }
    }
}