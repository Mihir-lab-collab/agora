using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraBL.Models;


namespace AgoraBL.Models
{
    public class IncompleteTimesheet
    {
        public string EntityName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class ProvideTimesheetAccess
    {
        public int EmpId { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public bool IsSucess { get; set; }
    }
    public class IncompleteTimesheetGroupDTO
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string MSTeam { get; set; }
        public List<IncompleteTimesheetDTO> IncompleteTimesheets { get; set; }
    }

    public class IncompleteTimesheetDTO
    {
        public string Date { get; set; }
        public int IncompleteHours { get; set; }
        public string TimeAvailable { get; set; }
    }
    public class GetIncompleteTimesheetDTO
    {
        public List<IncompleteTimesheetGroupDTO> IncompleteTimesheetDetails { get; set; }
        public List<SendLeaveNotification.HRIdList> HRList { get; set; }
    }

}
