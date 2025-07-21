using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.BLL
{
    /// <summary>
    /// Summary description for CustUsers
    /// </summary>
    public class Projects
    {
        public int projId { get; set; }
        public int custId { get; set; }
        public string projName { get; set; }
        public string projDesc { get; set; }
        public int projManager { get; set; }
        public DateTime projStartDate { get; set; }
        public decimal projDuration { get; set; }
        public DateTime projActComp { get; set; }
        public Int16 currID { get; set; }
        public decimal currExRate { get; set; }
        public decimal projCost { get; set; }
        public DateTime projProcMonth { get; set; }
        public Int16 noOfPayments { get; set; }
        public DateTime lastPaymentMonth { get; set; }
        public string codeRevTeam { get; set; }
        public bool allowTSEmployee { get; set; }
        public DateTime InsertedOn { get; set; }
        public string OtherEmailId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int projStatusId { get; set; }
        public Boolean IsSendMail { get; set; }

        public Projects()
        {

        }
        public Projects(int _projId, int _custId, string _projName, string _projDesc, int _projManager, DateTime _projStartDate, decimal _projDuration, DateTime _projActComp, Int16 _currID, decimal _currExRate, decimal _projCost, DateTime _projProcMonth, Int16 _noOfPayments, DateTime _lastPaymentMonth, string _codeRevTeam, bool _allowTSEmployee, DateTime _InsertedOn, string _OtherEmailId, DateTime _ModifiedOn, Boolean _IsSendMail)
        {
            this.projId = _projId;
            this.custId = _custId;
            this.projName = _projName;
            this.projDesc = _projDesc;
            this.projManager = _projManager;
            this.projStartDate = _projStartDate;
            this.projDuration = _projDuration;
            this.projActComp = _projActComp;
            this.currID = _currID;
            this.currExRate = _currExRate;
            this.projCost = _projCost;
            this.projProcMonth = _projProcMonth;
            this.noOfPayments = _noOfPayments;
            this.lastPaymentMonth = _lastPaymentMonth;
            this.codeRevTeam = _codeRevTeam;
            this.allowTSEmployee = _allowTSEmployee;
            this.InsertedOn = _InsertedOn;
            this.OtherEmailId = _OtherEmailId;
            this.ModifiedOn = _ModifiedOn;
            this.IsSendMail = _IsSendMail;
        }
        public static List<Projects> CustomerProjectList(int CustomerId)
        {
            ProjectsDAL objCustProjects = new ProjectsDAL();
            return objCustProjects.CustomerProjectList(CustomerId);
        }
        public static List<Projects> GetProjectList(int EmpID, int isAdmin)
        {
            ProjectsDAL objCustProjects = new ProjectsDAL();
            return objCustProjects.GetProjectList(EmpID, isAdmin);
        }
        public static List<Projects> CustomerUserProjectList(int CustomerId, int UserId)
        {
            ProjectsDAL objCustProjects = new ProjectsDAL();
            return objCustProjects.CustomerUserProjectList(CustomerId, UserId);
        }
        public static Projects GetCustomerProjectbyProjId(int projId)
        {
            ProjectsDAL objCustProjects = new ProjectsDAL();
            return objCustProjects.GetCustomerProjectbyProjId(projId);
        }

        public static List<Projects> GetProjectsAssignedToUser(int UserMasterID)
        {
            ProjectsDAL objCustProjects = new ProjectsDAL();
            return objCustProjects.GetProjectsAssignedToUser(UserMasterID);
        }

        public Projects(int projStatusId, string projDesc)
        {
            this.projStatusId = projStatusId;
            this.projDesc = projDesc;
        }

        public static List<Projects> ProjectStatus(string mode)
        {
            ProjectsDAL obj = new ProjectsDAL();
            return obj.ProjectStatus(mode);
        }
    }
}