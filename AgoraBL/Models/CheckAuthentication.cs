using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgoraBL.Models
{
    public class CheckAuthentication
    {
        public bool CheckToken { get; set; }
        public int EmpId { get; set; }
        public int ProfileId { get; set; }
    }
}