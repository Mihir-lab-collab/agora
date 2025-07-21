using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.BLL
{

    public class bugsResolution
    {
        public int bugsResolutionId { get; set; }
        public int bug_id { get; set; }
        public int status_id { get; set; }
        public int priority_id { get; set; }
        public string resolution { get; set; }
        public int resolutionBy { get; set; }
        public DateTime resolutionDate { get; set; }
        public string insertedIp { get; set; }

        public bugsResolution()
        {
        }
        public bugsResolution(int _bugsResolutionId, int _bug_id, int _status_id, int _priority_id, string _resolution, int _resolutionBy, DateTime _resolutionDate, string _insertedIp)
        {
            this.bugsResolutionId = _bugsResolutionId;
            this.bug_id = _bug_id;
            this.status_id = _status_id;
            this.priority_id = _priority_id;
            this.resolution = _resolution;
            this.resolutionBy = _resolutionBy;
            this.resolutionDate = _resolutionDate;
            this.insertedIp = _insertedIp;
        }
        public static List<bugsResolution> GetAllbugsResolutionByBugId(int BugId)
        {
            bugsResolutionDAL objbugsResolutionByBugId = new bugsResolutionDAL();
            return objbugsResolutionByBugId.GetAllbugsResolutionByBugId(BugId);
        }
        public static int InsertbugsResolution(int bug_id, int status_id, int priority_id, string resolution, int resolutionBy, string insertedIp)
        {
            bugsResolution curbugsResolution = new bugsResolution();
            curbugsResolution.bug_id = bug_id;
            curbugsResolution.status_id = status_id;
            curbugsResolution.priority_id = priority_id;
            curbugsResolution.resolution = resolution;
            curbugsResolution.resolutionBy = resolutionBy;
            curbugsResolution.resolutionDate = DateTime.Now;
            curbugsResolution.insertedIp = insertedIp;
            bugsResolutionDAL objbugsResolutionByBugId = new bugsResolutionDAL();
            return objbugsResolutionByBugId.InsertbugsResolution(curbugsResolution);
        }
    }
}