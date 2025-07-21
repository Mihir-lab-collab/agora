using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI.API.Models
{
    public class TimesheetTotalHours
    {
        public string EmpName { get; set; }
        public int EmpId { get; set; }
        public int TotalHours { get; set; }
    }
}