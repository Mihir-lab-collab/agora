using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;

/// <summary>
/// Summary description for CustTaskManager
/// </summary>
/// 
namespace Customer.BLL
{
    public class Bugs
    {
        public int bug_id { get; set; }
        public int moduleID { get; set; }
        public int priority_id { get; set; }
        public int status_id { get; set; }
        public string bug_name { get; set; }
        public string bug_desc { get; set; }
        public string resolution { get; set; }
        public string assigned_by { get; set; }
        public string assigned_by_Name { get; set; }
        public string ModuleName { get; set; }
        public int assigned_to { get; set; }
        public DateTime date_assigned { get; set; }
        public DateTime date_resolved { get; set; }
        public DateTime bug_lastModified { get; set; }
        public byte? istype { get; set; }


        public Bugs()
        {

        }

        public Bugs(int _bug_id, int _moduleID, int _priority_id, int _status_id, string _bug_name, string _bug_desc, string _resolution,
                   string _assigned_by, string _assigned_by_Name, string _ModuleName, int _assigned_to, DateTime _date_assigned, DateTime _date_resolved, DateTime _bug_lastModified, byte? _istype)
        {
            this.bug_id = _bug_id;
            this.moduleID = _moduleID;
            this.priority_id = _priority_id;
            this.status_id = _status_id;
            this.bug_name = _bug_name;
            this.bug_desc = _bug_desc;
            this.resolution = _resolution;
            this.assigned_by = _assigned_by;
            this.assigned_by_Name = _assigned_by_Name;
            this.ModuleName = _ModuleName;
            this.assigned_to = _assigned_to;
            this.date_assigned = _date_assigned;
            this.date_resolved = _date_resolved;
            this.bug_lastModified = _bug_lastModified;
            this.istype = _istype;
        }
        public static List<Bugs> GetBugsByProjId(int ProjectId, string includeTerminated)
        {
            BugsDAL objBugsByProjId = new BugsDAL();
            return objBugsByProjId.GetBugsByProjId(ProjectId, includeTerminated);
        }

        public static List<Bugs> GetTotalReportBugsCountByProjId(int ProjectId,string mode)
        {
            BugsDAL objTotalReportBugsCountByProjId = new BugsDAL();
            return objTotalReportBugsCountByProjId.GetTotalReportBugsCountByProjId(ProjectId,mode);
        }

        public static int Insertbugs(int moduleID, int priority_id, int status_id, string bug_name, string bug_desc, string resolution, string assigned_by, int assigned_to, byte type)
        {
            Bugs curbug = new Bugs();
            curbug.moduleID = moduleID;
            curbug.priority_id = priority_id;
            curbug.status_id = status_id;
            curbug.bug_name = bug_name;
            curbug.bug_desc = bug_desc;
            curbug.resolution = resolution;
            curbug.assigned_by = assigned_by;
            curbug.assigned_to = assigned_to;
            curbug.date_assigned = DateTime.Now;
            curbug.istype = type;
            curbug.bug_lastModified = DateTime.Now;
            BugsDAL objbugs = new BugsDAL();
            return objbugs.Insertbugs(curbug);
        }
        public static bool DeletebugById(int bug_Id)
        {
            Bugs curbug = new Bugs();
            curbug.bug_id = bug_Id;

            BugsDAL objbugs = new BugsDAL();
            return objbugs.DeletebugById(curbug);
        }

        public static bool UpdatebugByBugId(int bug_Id, int priority_id, int statusId, int empId, string Resolution, byte IsType)
        {
            Bugs curbug = new Bugs();
            curbug.bug_id = bug_Id;
            curbug.priority_id = priority_id;
            curbug.status_id = statusId;
            curbug.resolution = Resolution;
            curbug.assigned_to = empId;         
            curbug.istype = IsType;
            //curbug.bug_lastModified = DateTime.Now;
            BugsDAL objbugs = new BugsDAL();
            return objbugs.UpdatebugByBugId(curbug);
        }
        public static Bugs GetBugBybug_id(int bug_id)
        {
            BugsDAL objBugsByProjId = new BugsDAL();
            return objBugsByProjId.GetBugBybug_id(bug_id);
        }

        public static string CustUserDetails(int UserId)
        {
            UsersDAL objCurUser = new UsersDAL();
            return objCurUser.CustUserDetails(UserId);
        }


    }
}


