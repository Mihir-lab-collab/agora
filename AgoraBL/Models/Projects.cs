using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Models
{
    public class Projects
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public string ProjectStartDate { get; set; }
        public bool IsInhouse { get; set; }
        public int ProjStatus { get; set; }
        public bool IsTracked { get; set; }
        public string Status { get; set; }
        public PM PM { get; set; }
        public BA BA { get; set; }
        public AM AM { get; set; }
        //public List<ProjectMember> ProjectMemberList { get; set; }
        public string EmployeeDetails { get; set; }
        public bool IsAccessible { get; set; }
    }
    public class ProjectMember
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }
    public class PM
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }
    public class BA
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }
    public class AM
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }
}
