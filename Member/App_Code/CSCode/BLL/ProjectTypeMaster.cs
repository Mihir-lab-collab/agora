using ProjectType.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ProjectType.BLL
{
    public class ProjectTypeMaster
    {
        
        public int ProjTypeID { get; set; }
        public string ProjectType { get; set; }

        public ProjectTypeMaster()
        {

        }
    
        public ProjectTypeMaster(int _projTypeID, string _ProjectType)
         {
           this.ProjTypeID = _projTypeID;
          this.ProjectType = _ProjectType;
         }
        public static ProjectTypeMaster GetprojectTypeByprojTypeId(int ProjTypeID)
        {
            ProjectTypeMasterDAL objProjectTypeMaster = new ProjectTypeMasterDAL();
            return objProjectTypeMaster.GetprojectTypeByprojTypeId(ProjTypeID);
        }
        public static List<ProjectTypeMaster> GetAllProjectType()
        {
            ProjectTypeMasterDAL objProjectTypeMaster = new ProjectTypeMasterDAL();
            return objProjectTypeMaster.getProjectType();
        }
    }
}


    
