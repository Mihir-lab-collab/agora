using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI.API.Models
{
    public class EmployeeDetailsHR
    {
        public List<Demographic> Demographic_Details_HR { get; set; }
        public List<EngagementDetails> Engagement_Details_HR { get; set; }
        public List<Timesheet> Timesheet_Details_HR { get; set; }
        public List<Leave> Leave_Details_HR { get; set; }
    }
}