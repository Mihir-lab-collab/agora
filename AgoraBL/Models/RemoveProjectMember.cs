using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Models
{
    public class RemoveProjectMember
    {
        public int ProjId { get; set; }
        public string EmpId { get; set; }
        public string Mode { get; set; }
    }

}
