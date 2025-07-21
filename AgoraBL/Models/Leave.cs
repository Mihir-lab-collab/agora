using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgoraBL.Models
{
    public class Leave
    {
        public int EmpId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveFrom { get; set; }
        public string LeaveTo { get; set; }
        public string LeaveDescription { get; set; }
        public string LeaveStatus { get; set; }
        public string SanctionDate { get; set; }
        public string SanctionBy { get; set; }
        public string EmpName { get; set; }
        public string LeaveComment { get; set; }
        public int LeaveCount { get; set; }
        public string EmpLeaveId { get; set; }
    }
}