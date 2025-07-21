using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.DAL;

namespace Customer.BLL
{
   
    public class bugPriorities
    {
        public int priority_id { get; set; }
        public string priority_desc { get; set; }

        public bugPriorities()
        {
        }
        public bugPriorities(int _priority_id, string _priority_desc)
        {
            this.priority_id = _priority_id;
            this.priority_desc = _priority_desc;
        }

        public static List<bugPriorities> GetAllbugPriorities()
        {
            bugPrioritiesDAL objbugPriorities = new bugPrioritiesDAL();
            return objbugPriorities.GetAllbugPriorities();
        }

        public static List<bugPriorities> GetbugPrioritiesByProjId(int ProjId,string mode)
        {
            bugPrioritiesDAL objbugPrioritiesByProjId = new bugPrioritiesDAL();
            return objbugPrioritiesByProjId.GetbugPrioritiesByProjId(ProjId,mode);
        }
    }
}