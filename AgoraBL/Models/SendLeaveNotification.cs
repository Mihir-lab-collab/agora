using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Models
{
    public class SendLeaveNotification
    {
        public class EmpLeaveDetail
        {
            public string EmpName { get; set; }
            public string EmpID { get; set; }
            public List<ChannelList> ChannelList { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string LeaveType { get; set; }
            public string Reason { get; set; }
            public int NoOfDays { get; set; }
            public int EmpLeaveId { get; set; }
            public string EmpLeaveStatus { get; set; }
            public string UpdateReason { get; set; }
            public string UpdateDate { get; set; }
            public string UpdatedBy { get; set; }
        }

        public class HRIdList
        {
            public string HRName { get; set; }
            public List<ChannelList> ChannelList { get; set; }
            public string EmpId { get; set; }
        }

        public class SendLeaveNotificationDto
        {
            public EmpLeaveDetail EmpLeaveDetail { get; set; }
            public List<HRIdList> HRIdList { get; set; }
            public List<StakeHolderInfo> StakeHolders { get; set; }
        }
        public class ChannelList
        {
            public string ChannelName { get; set; }
            public string ChannelUniqueId { get; set; }
            public string ChannelNotificationUniqueId { get; set; }
            public string EmpId { get; set; }
        }
        public class StakeHolderInfo
        {
            public string RoleName { get; set; }          
            public string StakeHolderName { get; set; }   
            public string EmpId { get; set; }
            public List<ChannelList> ChannelList { get; set; }
        }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
