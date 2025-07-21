using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI.API.Models
{
    public class Timesheet
    {
        public string ProjName { get; set; }
        public string TsHour { get; set; }
        public string TsEntryDate { get; set; }
        public string TsComment { get; set; }
        public string EmpName { get; set; }
        public int EmpId { get; set; }
    }
}