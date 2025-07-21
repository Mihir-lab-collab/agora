using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agora.Onboarding.Models
{
    public class LogExceptions
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string LongDateTime { get; set; }
        public string Message { get; set; }
        public string QueryString { get; set; }
        public string TargetSite { get; set; }
        public string StackTrace { get; set; }
        public string ServerName { get; set; }
        public string RequestURL { get; set; }
        public string UserAgent { get; set; }
        public string UserIP { get; set; }
        public string UserName { get; set; }
        public string Mode { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }
}