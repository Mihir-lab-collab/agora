using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgoraBL.Models
{
    public class ConsolidateLeaves
    {
        public List<Leave> Leave { get; set; }
        public List<LeaveDetails> Leave_Details { get; set; }
    }
}