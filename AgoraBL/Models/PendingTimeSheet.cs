using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Models
{
    public class  PendingTimeSheet
    {
        public PendingTimeSheet()
        {
        }
        public PendingTimeSheet(int _EmpId, string _EmpName, string _EmailId, string _PendingTSDate,string _TimeAvailable,string _TimeReported, string _comment)
        {
            EmpId = _EmpId;
            EmpName = _EmpName;
            EmailID = _EmailId;
            PendingTSDate = _PendingTSDate;
            TimeAvailable = _TimeAvailable;
            TimeReported = _TimeReported;
            Comment = _comment;
        }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmailID { get; set; }
        public string TimeAvailable { get; set; }
        public string PendingTSDate { get; set; }
        public string TimeReported { get; set; }
        public string Comment { get; set; }

        public static implicit operator List<object>(PendingTimeSheet v)
        {
            throw new NotImplementedException();
        }
    }
}
