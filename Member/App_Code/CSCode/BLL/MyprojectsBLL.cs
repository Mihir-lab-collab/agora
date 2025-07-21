using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;


namespace Customer.BLL
{
    public class MyprojectsBLL
    {


        public int projId { get; set; }
        public int custId { get; set; }
        public string projName { get; set; }
        public string projDesc { get; set; }
        public int projManager { get; set; }
        public DateTime projStartDate { get; set; }
        public float projDuration { get; set; }
        public string  projActComp { get; set; }
        public int currID { get; set; }
        public int currExRate { get; set; }
        public int projCost { get; set; }
        public DateTime projProcMonth { get; set; }
        public int noOfPayments { get; set; }
        public DateTime lastPaymentMonth { get; set; }
        public string codeRevTeam { get; set; }
        public string codeDevTeam { get; set; }
        public Boolean allowTSEmployee { get; set; }
        public DateTime InsertedOn { get; set; }
        public string OtherEmailId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Boolean AlertCC { get; set; }
        public string projStatusTDesc { get; set; }
        public int projStatusId { get; set; }
        public string projStatus { get; set; }
        public int projStatusTId { get; set; }
        public DateTime proStatusDate { get; set; }
        public string custPassword { get; set; }
        public string custName { get; set; }
        public string custCompany { get; set; }
        public string custAddress { get; set; }
        public string custEmail { get; set; }
        public DateTime custRegDate { get; set; }
        public string custNotes { get; set; }
        public Boolean custStatus { get; set; }
        public DateTime lastLogin { get; set; }
        public Boolean allowTSCust { get; set; }
        public Boolean allowInvoice { get; set; }
        public Boolean allowTaskManager { get; set; }
        public Boolean allowComplaints { get; set; }
        public Boolean allowFeedBack { get; set; }
        public Boolean allowProjectReport { get; set; }
        public Boolean allowChangeRequest { get; set; }
        public Boolean allowEmpTimesheetEntry { get; set; }
        public Boolean TaskMailLevel { get; set; }
        public int empid { get; set; }
        public int ProfileID { get; set; }
        public string empPassword { get; set; }
        public int skilled { get; set; }
        public string empName { get; set; }
        public int empContact { get; set; }
        public DateTime empJoiningDate { get; set; }
        public DateTime empLeavingDate { get; set; }
        public int empProbationPeriod { get; set; }
        public string empNotes { get; set; }
        public string empEmail { get; set; }
        public Boolean empTester { get; set; }
        public int empAccountNo { get; set; }
        public DateTime empBDate { get; set; }
        public DateTime empADate { get; set; }
        public string empPrevEmployer { get; set; }
        public int empExperience { get; set; }
        public Boolean IsSuperAdmin { get; set; }
        public DateTime projExpComp { get; set; }
        public string ExpCompleted { get; set; }
        public string overallrating{get;set;}
        public string coderevname { get; set; }
        public string codedevname { get; set; }
        public int projStatusActive { get; set; }
        public string projRemark { get; set; }
        public string AccountManager { get; set; }
      
        public MyprojectsBLL()
        {  
        
        }
        public MyprojectsBLL(int projId, int custId, string projName, string projDesc, int projManager, DateTime projStartDate, string custName, string empName, DateTime projExpComp, string projActComp, string projStatusTDesc, string ExpCompleted, string codeDevTeam, string codeRevTeam, string projStatus, int projStatusActive, int projStatusTId)
        {
            this.projId = projId;
            this.custId = custId;
            this.projName = projName;
            this.projDesc = projDesc;
            this.projManager = projManager;
            this.projStartDate = projStartDate;
            this.custName = custName;
            this.empName = empName;
            this.projExpComp = projExpComp;
            this.projActComp = projActComp;
            this.projStatusTDesc = projStatusTDesc;
            this.ExpCompleted = ExpCompleted;
            this.codeDevTeam = codeDevTeam;
            this.codeRevTeam = codeRevTeam;
            this.projStatus = projStatus;
            this.projStatusTId = projStatusTId;
            this.projStatusActive = projStatusActive;

        }
       
        public MyprojectsBLL(int projId, int custId, string projName, string projDesc, int projManager, DateTime projStartDate, string custName, string empName,string accountManager, DateTime projExpComp, string projActComp,string projRemark, string projStatusTDesc, string ExpCompleted, string codeDevTeam, string codeRevTeam, string projStatus, int projStatusActive, int projStatusTId)
        {
            this.projId = projId;
            this.custId = custId;
            this.projName = projName;
            this.projDesc = projDesc;
            this.projManager = projManager;
            this.projStartDate = projStartDate;
            this.custName = custName;
            this.empName = empName;
            this.AccountManager = accountManager;
            this.projExpComp = projExpComp;
            this.projActComp = projActComp;
            this.projRemark = projRemark;
            this.projStatusTDesc = projStatusTDesc;
            this.ExpCompleted = ExpCompleted;
            this.codeDevTeam = codeDevTeam;
            this.codeRevTeam = codeRevTeam;
            this.projStatus = projStatus;
            this.projStatusTId = projStatusTId;
            this.projStatusActive = projStatusActive;

        }

        public MyprojectsBLL(string coderevname)
        {
            this.coderevname = coderevname;
        }

        //public static MyprojectsBLL GetOverallratingByPRojId(int projId)
        //{
        //    MyprojectsDAL objoverdetailDtl = new MyprojectsDAL();
        //    return objoverdetailDtl.GetOverallratingByPRojId(projId);

        //}



        public static List<MyprojectsBLL> GetMyProjId(int empid, Boolean IsAdmin,string include="")
        {
            MyprojectsDAL objBugsByProjId = new MyprojectsDAL();
            return objBugsByProjId.GetMyProjId(empid, IsAdmin,include);
        }

        public static List<MyprojectsBLL> GetProjectStatusDetail(int ProjId)
        {
            MyprojectsDAL objBugsByProjId = new MyprojectsDAL();
            return objBugsByProjId.GetProjStatusDetails(ProjId);
        }

        public static List<MyprojectsBLL> GetMycoderevteam(string empid)
        {
            MyprojectsDAL objMycoderevteam = new MyprojectsDAL();
            return objMycoderevteam.GetMycoderevteam(empid);
        }


    }
}