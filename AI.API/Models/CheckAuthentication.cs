using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI.API.Models
{
    public class CheckAuthentication
    {
        public bool CheckToken { get; set; }
        public int EmpId { get; set; }
        public int ProfileId { get; set; }
    }
}