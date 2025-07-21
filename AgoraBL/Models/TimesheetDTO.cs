using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Models
{
    public class TimesheetDTO
    {
        public int ProjID { get; set; }
        public string ProjName { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public int TSID { get; set; }
        public DateTime TSDate { get; set; }
        public double TSHour { get; set; }
        public string TSComment { get; set; }
        public Boolean IsApproved { get; set; }
        public int EmpApproveID { get; set; }
        public string Role { get; set; }
        public string AttHour { get; set; }
        public string AttTSHour { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public DateTime TSEntryDate { get; set; }
        public string ProjectTotalHrs { get; set; }
        public Boolean TsVerified { get; set; }
        public string AccountManagerEmail { get; set; }
        public string ProjectManagerEmail { get; set; }
        public string BAName { get; set; }
        public string BaEmail { get; set; }
        public string ManagerName { get; set; }
        public Boolean IsSendMail { get; set; }

        public string TsVerifiedBy { get; set; }
        public string TsVerifiedOn { get; set; }
        public int TotalHours { get; set; }
        public string Designation { get; set; }
        public int ProjectTotalHours { get; set; }
        public string Module { get; set; }
        public string ProjectStartDate { get; set; }
        public string ProjectStausDate { get; set; }
        public string ProjectStaus { get; set; }
        public int SkillId { get; set; }
        public string Description { get; set; }
        public bool IsSucess { get; set; }
        public string Message { get; set; }
    }
   public class AddTimesheet
   {
        public int EmpID { get; set; }
        public int SkillId { get; set; }
        public List<TimesheetDTO> lstTimesheet { get; set; }
   }
}
