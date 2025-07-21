using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI.API.Models
{
    public class LeaveDetails
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Type { get; set; }
        public int Total { get; set; }
        public int Total_Accural { get; set; }
        public int Consumed { get; set; }
        public int Balance { get; set; }
        public int CarryFwdPL { get; set; }
        public int AccCurrYrPL { get; set; }
    }
}