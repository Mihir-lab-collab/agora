using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI.API.Models
{
    public class PendingTimesheet
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public int EmpId { get; set; }
    }
}