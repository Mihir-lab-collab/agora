using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgoraBL.Models
{
    public class TimesheetTotalHours
    {
        public string EmpName { get; set; }
        public int EmpId { get; set; }
        public string ProjectName { get; set; }
        public int TotalHours { get; set; }
        public bool IsAccessible { get; set; }
    }
}