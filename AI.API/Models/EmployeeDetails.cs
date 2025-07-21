using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI.API.Models
{
    public class EmployeeDetails
    {
        public Demographic Demographic_Details { get; set; }
        public List<EngagementDetails> Engagement_Details { get; set; }
        public List<Timesheet> Timesheet_Details { get; set; }
        public List<Leave> Leave_Details { get; set; }
    }
}