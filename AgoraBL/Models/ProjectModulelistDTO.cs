using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Models
{
    public class ProjectModulelistDTO
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<Module> Modules { get; set; }

        public ProjectModulelistDTO()
        {
            Modules = new List<Module>(); // Initialize to avoid null reference issues
        }
    }

    public class Module
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
    }
}

