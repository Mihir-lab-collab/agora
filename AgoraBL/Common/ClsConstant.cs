using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Common
{
    public class ClsConstant
    {
        public class EmpLeaveStatus
        {
            public const string Pending = "Pending";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
            public const string Deleted = "Deleted";
        }
        public class EmpDBLeaveStatus
        {
            public const string Pending = "p";
            public const string Approved = "a";
            public const string Rejected = "r";
            public const string Deleted = "d";
        }
        public class EmpWFHStatus
        {
            public const string Pending = "Pending";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
            public const string Deleted = "Deleted";
        }
        public class EmpDBWFHStatus
        {
            public const string Pending = "p";
            public const string Approved = "a";
            public const string Rejected = "r";
            public const string Deleted = "d";
        }
    }
}
