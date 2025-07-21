using Agora.Onboarding.DAL;
using Agora.Onboarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Agora.Onboarding.filter
{
    public class CustomException : ActionFilterAttribute/*, IExceptionFilter*/
    {
        //public void OnException(ExceptionContext filterContext)
        //{
        //    LogExceptions logExceptions = new LogExceptions()
        //    {
        //        Message = filterContext.Exception.Message,
        //        StackTrace = filterContext.Exception.StackTrace,
        //        LongDateTime = DateTime.Now.ToString(),
        //        TargetSite = filterContext.Exception.TargetSite.ToString(),
        //        Source = filterContext.Exception.Source,
        //        ControllerName = filterContext.RouteData.Values["controller"].ToString(),
        //        ActionName = filterContext.RouteData.Values["action"].ToString()
        //    };
        //    HttpContext.Current.Session["GlobalErrorMessage"] =logExceptions.StackTrace+ ";" + logExceptions.Message; ;
        //    DbContext.LogErrorToDB(logExceptions);
        //}
    }
}