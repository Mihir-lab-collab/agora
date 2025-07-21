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

    public class projectdetails
    {
        public int projId { get; set; }
        public int empid { get; set; }
        public string custName { get; set; }


        public projectdetails()
        {

        }
        //public projectdetails(int _projId, int _empid)
        //{
        //    this.projId = _projId;
        //    this.empid = _empid;
        //}
        //public static int InsertprojectMember(int projId, int empid)
        //{
        //    projectMember curprojectMember = new projectMember();
        //    curprojectMember.projId = projId;
        //    curprojectMember.empid = empid;
        //    projectMemberDAL objprojectMember = new projectMemberDAL();
        //    return objprojectMember.InsertprojectMember(curprojectMember);
        //}

        //public static int DeleteprojectMemberByprojid(int projId)
        //{
        //    projectMemberDAL objprojectMember = new projectMemberDAL();
        //    return objprojectMember.DeleteprojectMemberByprojid(projId);
        //}
        //public static List<projectMember> GetProjectMembersByProjId(int projId)
        //{
        //    projectMemberDAL objprojectMember = new projectMemberDAL();
        //    return objprojectMember.GetProjectMembersByProjId(projId);
        //}

    }
}