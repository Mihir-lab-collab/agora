using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;

/// <summary>
/// Summary description for MyTeamBLL
/// </summary>
namespace Customer.BLL
{

    public class MyTeamBLL
    {
        public MyTeamBLL()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        public MyTeamBLL(int empId, string empName, string designation, string primarySkill, string SecondarySkill, string experience, string projectsWorkingOn)
        {
            this.empId = empId;
            this.empName = empName;
            this.designation = designation;
            this.primarySkill = primarySkill;
            this.SecondarySkill = SecondarySkill;
            this.experience = experience;
            this.projectsWorkingOn = projectsWorkingOn;
            

        }
        public static List<MyTeamBLL> GetMyTeam(int empId,string includeProjectsCompleted)
        {
            MyTeamDAL objMyTeam = new MyTeamDAL();
            return objMyTeam.GetMyTeam(empId,includeProjectsCompleted);
        }

        public int empId { get; set; }

        public string primarySkill { get; set; }
        public string SecondarySkill { get; set; }

        public string empName { get; set; }

        public string designation { get; set; }

        public string experience { get; set; }

        public string projectsWorkingOn { get; set; }

        
    }
}