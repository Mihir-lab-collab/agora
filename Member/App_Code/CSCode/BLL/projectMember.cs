using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace Customer.BLL
{

    public class projectMember
    {
        public int projId { get; set; }
        public int empid { get; set; }
       public string projectName { get; set; }
        public string empName { get; set; }
        public projectMember()
        {

        }
        public projectMember(int _projId, int _empid)
        {
            this.projId = _projId;
            this.empid = _empid;
        }
        public projectMember(int _empid, string _projectName,string _empName )
        {
            
            this.empid = _empid;
            this.projectName = _projectName;
            this.empName = _empName;
        }

        public static int InsertprojectMember(int projId, int empid)
        {
            projectMember curprojectMember = new projectMember();
            curprojectMember.projId = projId;
            curprojectMember.empid = empid;
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.InsertprojectMember(curprojectMember);
        }

        public static int DeleteprojectMemberByprojid(int projId)
        {
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.DeleteprojectMemberByprojid(projId);
        }
        public static List<projectMember> GetProjectMembersByProjId(int projId)
        {
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.GetProjectMembersByProjId(projId);
        }

        //Added By Trupti on 19 Jully 2018
        public static List<projectMember> GetProjectDetailsByEmpId(int intEmpID)
        {
       
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.GetProjectIdByEmpId(intEmpID);
        }
            //
        public static int InsertUpdateProjectMem(int projId, int empid)
        {
            projectMember curprojectMember = new projectMember();
            curprojectMember.projId = projId;
            curprojectMember.empid = empid;
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.InsertUpdateProjectMem(curprojectMember);
        }


        public static int DeleteprojectMember(int projId)
        {
            projectMember curprojectMember = new projectMember();
            curprojectMember.projId = projId;
            // curprojectMember.empid = empid;
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.DeleteprojectMember(curprojectMember);
        }

        

            public static int DeleteTeamMemberFromAllProjects(int empId)
        {
            projectMember curprojectMember = new projectMember();
            curprojectMember.empid = empId;
            // curprojectMember.empid = empid;
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.DeleteprojectMemberbyEmpId(curprojectMember);
        }


        public static int DeleteprojectAppraisalAuthorityMemberByprojid(int projId)
        {
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.DeleteprojectAppraisalAuthorityMemberByprojid(projId);
        }

        public static int InsertprojectAppraisalAuthorityMember(int projId, int empid)
        {
            projectMember curprojectMember = new projectMember();
            curprojectMember.projId = projId;
            curprojectMember.empid = empid;
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.InsertprojectAppraisalAuthorityMember(curprojectMember);
        }

        public static List<projectMember> GetProjectAppraisalAuthorityMemberByProjId(int projId)
        {
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.GetProjectAppraisalAuthorityMemberByProjId(projId);
        }

        public projectMember(int _projId, string _empName)
        {
            this.projId = _projId;
            this.empName = _empName;
        }
        public static List<projectMember> GetProjectMembersNameByProjId(int projId)
        {
            projectMemberDAL objprojectMember = new projectMemberDAL();
            return objprojectMember.GetProjectMembersNameByProjId(projId);
        }
    }
}