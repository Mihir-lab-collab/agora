using AgoraBL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Models
{
    public class clsTimeSheetEmail
    {
        public int ProjID { get; set; }
        public int UserID { get; set; }
        public string ProjName { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public int TSID { get; set; }
        public DateTime TSDate { get; set; }
        public double TSHour { get; set; }
        public string TSComment { get; set; }
        public string WBSName { get; set; }
        public Boolean IsWBS { get; set; }
        public int intWBSID { get; set; }

        public string AttHour { get; set; }
        public string AttTSHour { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime TSEntryDate { get; set; }
        public string AccountManagerEmail { get; set; }
        public string ProjectManagerEmail { get; set; }
        public string ManagerName { get; set; }

    }
}
