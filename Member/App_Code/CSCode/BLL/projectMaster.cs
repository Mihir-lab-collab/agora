using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Customer.BLL
{
    public class projectMaster
    {
        private string p;

        public int projId { get; set; }
        public int custId { get; set; }
        public string projName { get; set; }
        public string projDesc { get; set; }
        public int projManager { get; set; }
        public int AccountMgr { get; set; }
        public DateTime projStartDate { get; set; }
        public string projDuration { get; set; }
        public DateTime? projActComp { get; set; }
        public int currID { get; set; }
        public decimal currExRate { get; set; }
        public decimal projCost { get; set; }
        public string OtherEmailId { get; set; }
        public string Status { get; set; }
        public decimal RevisedBudget { get; set; }
        public decimal CreditAmount { get; set; }
        public int InHouse { get; set; }
        public int OnGoing { get; set; }
        public DateTime projReportDate { get; set; }
        public decimal TotalInvoiced { get; set; }
        public decimal TotalRecieved { get; set; }
        public customerMaster customer { get; set; }
        public EmployeeMaster accountManager { get; set; }
        public DateTime? ProjectStautusDate { get; set; }
        public int isSendEmail { get; set; }
        public string ProjectType { get; set; }
        public Decimal InitialProjectCost { get; set; }
        public int IsTracked { get; set; }
        public int BA { get; set; }
        public string CurrSymbol { get; set; }
        public string CustComapny { get; set; }
        public projectMaster()
        {
        }

        public projectMaster(int _projId, int _custId, string _projName, string _projDesc, int _projManager, int _AccountMgr, DateTime _projStartDate,
            string _projDuration, DateTime? _projActComp, Int16 _currID, decimal _currExRate, decimal _projCost, string _OtherEmailId, string _Status, decimal _RevisedBudget, decimal _CreditAmount, int _inHouse, int _onGoing, DateTime _projReportDate, decimal _totalInvoiced, decimal _totalRecieved, int _isSendEmail, string _ProjectType, Decimal _InitialProjectCost,int _isTracked,int _BA,string _currSymbol,string _custCompany)
        {
            this.projId = _projId;
            this.custId = _custId;
            this.projName = _projName;
            this.projDesc = _projDesc;
            this.projManager = _projManager;
            this.AccountMgr = _AccountMgr;
            this.projStartDate = _projStartDate;
            this.projDuration = _projDuration;
            this.projActComp = _projActComp;
            this.currID = _currID;
            this.currExRate = _currExRate;
            this.projCost = _projCost;
            this.OtherEmailId = _OtherEmailId;
            this.Status = _Status;
            this.RevisedBudget = _RevisedBudget;
            this.CreditAmount = _CreditAmount;
            this.InHouse = _inHouse;
            this.OnGoing = _onGoing;
            this.projReportDate = _projReportDate;
            this.TotalInvoiced = _totalInvoiced;
            this.TotalRecieved = _totalRecieved;
            this.isSendEmail = _isSendEmail;
            this.ProjectType = _ProjectType;
            this.InitialProjectCost = _InitialProjectCost;
            this.IsTracked = _isTracked;
            this.BA = _BA;
            this.CurrSymbol = _currSymbol;
            this.CustComapny = _custCompany;
               
        }

        public projectMaster(int _projId, string _projName, DateTime? _proStatusDate)
        {
            this.projId = _projId;
            this.projName = _projName;
            this.ProjectStautusDate = _proStatusDate;

        }
        public static List<projectMaster> Projects(bool All = false, int CustID = 0, string status = "", int ProjID = 0)
        {
            projectMasterDAL objCustProjects = new projectMasterDAL();
            return objCustProjects.Projects(All, CustID, ProjID, status);
        }

        public static void UpdateProject(int projId, int custId, string projName, string projDesc, int projManager, int AccountMgr, DateTime projStartDate, string projDuration,
            DateTime? projActComp, int currID, decimal currExRate, decimal projCost, string OtherEmailId, int inHouse, int onGoing, DateTime projrptDate, string mailconfirmation, out int newProjectId, out string emailData, out string mailTo, out string mailCC, int isSendEmail, string ProjectType, decimal InitialProjectCost, int IsTracked,int BA)
        {
            int ProjID;
            projectMaster curprojectMaster = new projectMaster();
            curprojectMaster.projId = projId;
            curprojectMaster.custId = custId;
            curprojectMaster.projName = projName;
            curprojectMaster.projDesc = projDesc;
            curprojectMaster.projManager = projManager;
            curprojectMaster.AccountMgr = AccountMgr;
            curprojectMaster.projStartDate = projStartDate;
            curprojectMaster.projDuration = projDuration;
            curprojectMaster.projActComp = projActComp;
            curprojectMaster.currID = currID;
            curprojectMaster.currExRate = currExRate;
            curprojectMaster.projCost = projCost;
            curprojectMaster.OtherEmailId = OtherEmailId;
            curprojectMaster.InHouse = inHouse;
            curprojectMaster.OnGoing = onGoing;
            curprojectMaster.projReportDate = projrptDate;
            curprojectMaster.isSendEmail = isSendEmail;
            curprojectMaster.ProjectType = ProjectType;
            curprojectMaster.InitialProjectCost = InitialProjectCost;
            curprojectMaster.IsTracked = IsTracked;
            curprojectMaster.BA = BA;
            projectMasterDAL objprojectMaster = new projectMasterDAL();
            ProjID = objprojectMaster.UpdateProject(curprojectMaster);
            string MsgBody = string.Empty;
            string sendMailTo = string.Empty;
            string sendMailCc = string.Empty;  
            projectMaster p = new projectMaster();


            if (ProjID != 0)
            {
                if (mailconfirmation == "true")
                {
                    customerMaster cust = customerMaster.GetCustomerByCustId(custId);
                    p.customer = cust;
                    EmployeeMaster emp = EmployeeMaster.GetEmpDetailsByEmployeeId(AccountMgr);
                    p.accountManager = emp;
                    MsgBody = sendProjectWelcomeEmail(p);
                    sendMailTo = p.customer.custEmail;
                    sendMailCc = p.accountManager.empEmail;
                }

            }

            newProjectId = ProjID;
            emailData = MsgBody;
            mailTo = sendMailTo;
            mailCC = sendMailCc;
        }

        public static bool GetAlertCCProjId(int projId)
        {
            projectMasterDAL objGetAlertProjId = new projectMasterDAL();
            return objGetAlertProjId.GetAlertCC(projId);
        }

        public static List<projectMaster> GetProjectsTitles()
        {
            projectMasterDAL objCustProjects = new projectMasterDAL();
            return objCustProjects.GetProjectTitle();
        }

        public projectMaster(string _ProjectTitle, int _projId)
        {
            this.projName = _ProjectTitle;
            this.projId = _projId;
        }

        private static string sendProjectWelcomeEmail(projectMaster proj)
        {
            projectMasterDAL objprojectMaster = new projectMasterDAL();
            string emailData = objprojectMaster.GetConfigValue();

            string mystr = string.Empty;
            List<string> myListOfTokens = new List<string>();

            var pattern = @"(\{(?:.*?)\})";
            foreach (var m in System.Text.RegularExpressions.Regex.Split(emailData, pattern))
            {
                if (m.Substring(0, 1) == "{" && m.Substring(m.Length - 1) == "}")
                {
                    mystr = string.Join(string.Empty, m.Skip(9));
                    mystr = mystr.Remove(mystr.Length - 1);
                    myListOfTokens.Add(mystr);
                }
            }

            string replaceValue = string.Empty;
            foreach (string token in myListOfTokens)
            {
                if (token.Contains("cust"))
                {
                    replaceValue = CSCode.Global.GetTemplatePropertyValue(proj.customer, token);

                }

                if (token.Contains("emp"))
                {
                    replaceValue = CSCode.Global.GetTemplatePropertyValue(proj.accountManager, token);

                }

                emailData = emailData.Replace("{project." + token + "}", replaceValue);
            }
            return emailData;
        }

        public static bool sendProjectWelcomeEmail(string mailTo, string Cc, string Bcc, string strSubject, string strMsgBody)
        {
            bool IsSendmail = false;
            if (strMsgBody != "")
            {
                string result = string.Empty;
                string mailFrom = "";
                result = CSCode.Global.SendMail(strMsgBody, strSubject, mailTo, mailFrom, true, Cc, "");
                IsSendmail = true;
            }
            return IsSendmail;
        }

        public static List<projectMaster> GetInCompleteProjectsStatus(string mode, string empID)
        {
            projectMasterDAL objCustProjects = new projectMasterDAL();
            return objCustProjects.GetInCompleteProjectsStatus(mode, empID);
        }

        public static string checkManagerforProject(string mode, string empID)
        {
            projectMasterDAL objCustProjects = new projectMasterDAL();
            return objCustProjects.checkManagerforProject(mode, empID);
        }
        public  List<ProjectStatusDetails> GetProjectStatusDetails()
        {
            projectMasterDAL objCustProjects = new projectMasterDAL();
            return objCustProjects.GetProjectStatusDetails();
        }
        
        
    }
   
    public class ProjectHourDetails
    {
        public int ProjID { get; set; }
        public string projName { get; set; }
        public int EstHours { get; set; }
        public string ActualHours { get; set; }
        public string ProjectManager { get; set; }
        public string AccountManager { get; set; }
        public string BusinessAnalyst { get; set; }
        public static List<ProjectHourDetails> GetDetails(int EmpID)
        {
            projectMasterDAL obj = new projectMasterDAL();
            return obj.GetDetails(EmpID);
        }
    }
    public class ProjectStatusDetails
    {
        public int projStatusTId { get; set; }
        public string projStatusTDesc { get; set; }
    }
    public class ProjecEstDetails
    {
        public int ProjID { get; set; }
        public int ProjectMileStoneID { get; set; }
        public string Name { get; set; }
        public string DueDate { get; set; }
        public string DeliveryDate { get; set; }
        public string EstHours { get; set; }
        public string ActualHours { get; set; }
        public string ComputedMSHours { get; set; }

        public static List<ProjecEstDetails> GetEstDetails()
        {
            projectMasterDAL obj = new projectMasterDAL();
            return obj.GetEstDetails();
        }
    }
}