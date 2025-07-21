using AgoraBL.DAL;
using AgoraBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.BAL
{
    public class ProjectBAL
    {
        public static int InsertprojectMember(int projId, int empid,string mode)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            return objProjectDAL.InsertprojectMember(projId, empid,mode);
        }
        public static int DeleteprojectMemberByprojid(int projId, string mode,int empId=0)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            return objProjectDAL.DeleteprojectMemberByprojid(projId, mode,empId);
        }
        public List<Projects> GetProjectByRole(int empId, string mode)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            return objProjectDAL.GetProjectByRole(empId, mode);
        }
        public static List<ProjectMember> GetEmpDetailsByEntityName(string entityName, string mode)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            return objProjectDAL.GetEmpDetailsByEntityName(entityName, mode);
        }
        public static bool CheckAuth(int empId, string mode)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            return objProjectDAL.CheckAuth(empId, mode);
        }
        public static ProjectStackHolderDTO GetProjectStackHolder(int projId,string role,int newEmpId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            return objProjectDAL.GetProjectStackHolder(projId,role,newEmpId);
        }
        public static int UpdateProjectStackHolder(int projId, string role, int stackHolderId, int empId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            return objProjectDAL.UpdateProjectStackHolder(projId,role, stackHolderId, empId);
        }
    }
}
