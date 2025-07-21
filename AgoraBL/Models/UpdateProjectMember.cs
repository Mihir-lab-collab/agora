using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Models
{
    public class UpdateProjectStackHolder
    {
        public int ProjId { get; set; }
        public int EmpId { get; set; }
        public string Role { get; set; }

    }
    public class ProjectStackHolderDTO
    {
        public int ProjId { get; set; }
        public string ProjName { get; set; }
        public int OldEmpId { get; set; }
        public string OldEmpName { get; set; }
        public int NewEmpId { get; set; }
        public string NewEmpName { get; set; }
        public string Role { get; set; }
        public bool IsSucess { get; set; }
        public string Message { get; set; }

    }
}
