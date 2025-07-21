using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agora.Onboarding.filter
{

    public class SessionTimeout : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext _context = HttpContext.Current;
            if (HttpContext.Current.Session["onboradObj"] == null || HttpContext.Current.Session["getObjById"] == null)
            {
                filterContext.Result = new RedirectResult("CustomError/index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}