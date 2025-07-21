using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.DAL;

namespace Customer.BLL
{
    /// <summary>
    /// Summary description for CustStatus
    /// </summary>
    public class bugStatuses
    {
        public int status_id { get; set; }
        public string status { get; set; }
        public Boolean statusAdmin { get; set; }
        public Boolean IsClient { get; set; }
        public int SortOrder { get; set; }
        public int TaskMailLevel { get; set; }

        public bugStatuses()
        {
        }

        public bugStatuses(int _status_id, string _status, bool _statusAdmin, bool _IsClient, int _SortOrder, int _TaskMailLevel)
        {
            this.status_id = _status_id;
            this.status = _status;
            this.statusAdmin = _statusAdmin;
            this.IsClient = _IsClient;
            this.SortOrder = _SortOrder;
            this.TaskMailLevel = _TaskMailLevel;
        }

        public static List<bugStatuses> GetAllbugStatuses()
        {
            bugStatusesDAL objCustProjects = new bugStatusesDAL();
            return objCustProjects.GetAllbugStatuses();
        }

        public static List<bugStatuses> GetbugStatusesByProjId(int ProjId,string mode)
        {
            bugStatusesDAL objbugStatusesByProjId = new bugStatusesDAL();
            return objbugStatusesByProjId.GetbugStatusesByProjId(ProjId,mode);
        }
    }

}
