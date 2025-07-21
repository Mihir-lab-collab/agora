using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.BLL
{

    public class projectModuleMaster
    {

        public int moduleId { get; set; }
        public int projId { get; set; }
        public int moduleRefId { get; set; }
        public string moduleName { get; set; }
        public string moduleDescription { get; set; }
        public DateTime moduleDate { get; set; }
        public int moduleEstimate { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime ModifiedOn { get; set; }


        public projectModuleMaster()
        {

        }
        public projectModuleMaster(int _moduleId, int _projId, int _moduleRefId, string _moduleName, string _moduleDescription, DateTime _moduleDate, int _moduleEstimate, DateTime _InsertedOn, DateTime _ModifiedOn)
        {
            this.moduleId = _moduleId;
            this.projId = _projId;
            this.moduleRefId = _moduleRefId;
            this.moduleName = _moduleName;
            this.moduleDescription = _moduleDescription;
            this.moduleDate = _moduleDate;
            this.moduleEstimate = _moduleEstimate;
            this.InsertedOn = _InsertedOn;
            this.ModifiedOn = _ModifiedOn;
        }
        public static projectModuleMaster GetprojectModuleMasterByModuleId(int moduleId)
        {
            projectModuleMasterDAL objGetprojectModuleMasterByModuleId = new projectModuleMasterDAL();
            return objGetprojectModuleMasterByModuleId.GetprojectModuleMasterByModuleId(moduleId);
        }

        public static List<projectModuleMaster> GetprojectModuleMasterByProjectId(int ProjId)
        {
            projectModuleMasterDAL objGetprojectModuleMasterByProjectId = new projectModuleMasterDAL();
            return objGetprojectModuleMasterByProjectId.GetprojectModuleMasterByProjectId(ProjId);
        }

        public static int InsertprojectModuleMasterByProjId(int ProjId)
        {
            projectModuleMasterDAL objprojectModuleMaster = new projectModuleMasterDAL();
            return objprojectModuleMaster.InsertprojectModuleMasterByProjId(ProjId);
        }


    }
}