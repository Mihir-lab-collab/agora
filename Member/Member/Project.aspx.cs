using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectType.BLL;
using Newtonsoft.Json;

public partial class Customer_Project : System.Web.UI.Page
{
    UserMaster UM;
    public string empID;

    [System.Web.Services.WebMethod]
    public static String BindCustomersDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(customerMaster.GetAllCustomers());
    }

    [System.Web.Services.WebMethod]
    public static String BindProjectManagerDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(EmployeeMaster.GetAllEmployees());
    }

    [System.Web.Services.WebMethod]
    public static String BindProjectStatusDropDown()
    {
        string mode = "Status";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(Projects.ProjectStatus(mode));
    }

    [System.Web.Services.WebMethod]
    public static string SetProjIdForInvoice(string projid, string projname)
    {
        Page objp = new Page();
        objp.Session["ProjectID"] = projid;
        objp.Session["ProjectName"] = projname;
        return projid;
    }

    [System.Web.Services.WebMethod]
    public static string SetProjIdForPayment(string projid, string projname)
    {
        Page objp = new Page();
        objp.Session["ProjectID"] = projid;
        objp.Session["ProjectName"] = projname;
        return projid;
    }

    [System.Web.Services.WebMethod]
    public static string SetProjIdForMilestone(string projid, string currExRate)
    {
        Page objp = new Page();
        objp.Session["ProjectID"] = projid;
        objp.Session["CurrExRate"] = currExRate;
        return projid;
    }

    [System.Web.Services.WebMethod]
    public static String GetModulesForProjects()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(Modules.GetModulesForProjects());
    }

    [System.Web.Services.WebMethod]
    public static String GetAllcurrencyMaster()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(currencyMaster.GetAllcurrencyMaster());
    }

    //Added by trupti
    [System.Web.Services.WebMethod]
    public static String getProjectType()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(ProjectTypeMaster.GetAllProjectType());
    }

    [System.Web.Services.WebMethod]
    public static String ProjectList(int CustId, bool isChecked, string status)
    {
        if (status == "undefined")
        {
            status = "";
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        List<projectMaster> AllProjects = projectMaster.Projects(isChecked, CustId, status)
                                                .OrderByDescending(p => p.projId)
                                                .ToList();

        var data = from curproject in AllProjects
                   orderby curproject.projId descending
                   select new
                   {
                       curproject.projId,
                       curproject.custId,
                      // CompanyName = customerMaster.GetCustomerByCustId(curproject.custId).custCompany.ToString(),
                       curproject.CustComapny,
                       curproject.projName,
                       curproject.projManager,
                       curproject.AccountMgr,
                       curproject.projDesc,
                       curproject.currID,
                       DevelopmentTeam = String.Join(",", (from A in projectMember.GetProjectMembersByProjId(curproject.projId).ToList()
                                                           select A.empid).ToList()),
                       AppraisalAuthorityMembers = String.Join(",", (from A in projectMember.GetProjectAppraisalAuthorityMemberByProjId(curproject.projId).ToList()
                                                                     select A.empid).ToList()),
                       curproject.OtherEmailId,
                       //currSymbol = currencyMaster.GetcurrencyMasterBycurrId(curproject.currID).currSymbol,
                       curproject.CurrSymbol,
                       curproject.projCost,
                       curproject.projDuration,
                       curproject.projStartDate,
                       projectDuration = curproject.projDuration,
                       curproject.Status,
                       curproject.currExRate,
                       curproject.RevisedBudget,
                       curproject.CreditAmount,
                       curproject.InHouse,
                       curproject.OnGoing,
                       curproject.projReportDate,
                       curproject.isSendEmail,
                       curproject.ProjectType,
                       curproject.InitialProjectCost,
                       curproject.IsTracked,
                       curproject.BA,
                       // ProjectTypeNsame = ProjectTypeMaster.GetprojectTypeByprojTypeId(2).ProjectType,


                   };
        return JsonConvert.SerializeObject(data);
       // return jss.Serialize(data.FirstOrDefault());
    }

    [System.Web.Services.WebMethod]
    public static string SelectedProject(int projId)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        List<projectMaster> AllProjects = projectMaster.Projects(true, 0, "").Where(x => x.projId == projId).ToList();

        //  projectMaster selectedproj = projectMaster.Projects(true, 0, "").Where(x => x.projId == projId).FirstOrDefault();

        var data = from curproject in AllProjects
                   orderby curproject.projId descending
                   select new
                   {
                       curproject.projId,
                       curproject.custId,
                       CompanyName = customerMaster.GetCustomerByCustId(curproject.custId).custCompany.ToString(),
                       curproject.projName,
                       curproject.projManager,
                       curproject.AccountMgr,
                       curproject.projDesc,
                       curproject.currID,
                       DevelopmentTeam = String.Join(",", (from A in projectMember.GetProjectMembersByProjId(curproject.projId).ToList()
                                                           select A.empid).ToList()),
                       AppraisalAuthorityMembers = String.Join(",", (from A in projectMember.GetProjectAppraisalAuthorityMemberByProjId(curproject.projId).ToList()
                                                                     select A.empid).ToList()),
                       curproject.OtherEmailId,
                       currSymbol = currencyMaster.GetcurrencyMasterBycurrId(curproject.currID).currSymbol,
                       curproject.projCost,
                       curproject.projDuration,
                       projStartDate = curproject.projStartDate.ToString(),
                       projectDuration = curproject.projDuration,
                       curproject.Status,
                       curproject.currExRate,
                       curproject.RevisedBudget,
                       curproject.CreditAmount,
                       curproject.InHouse,
                       curproject.OnGoing,
                       curproject.isSendEmail,
                       curproject.ProjectType,
                       curproject.InitialProjectCost,
                       curproject.IsTracked,
                     //  ProjectType = ProjectTypeMaster.GetprojectTypeByprojTypeId(2).ProjectType
                   };
        return jss.Serialize(data.FirstOrDefault());
    }

    [System.Web.Services.WebMethod]
    public static String UpdateProjectByProjId(string projId, string Customer, string Title, string ProjectDescription, string Projmanager, string StartDate, string projectDuration,
             string ActualCompletion, string PaymentCurrency, string ExchangeRate, string ProjectCost,
             string CodeReviewTeam, string otherEmailIds, string DevelopmentTeam, string Modules, string AppraisalAuthorityMembers, int isSendEmail, int IsTracked)
    {
        string statusmsg = "Update Failed";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(statusmsg);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        empID = UM.EmployeeID.ToString();
        hdnEmpId.Value = empID;
        if (!IsPostBack)
        {
            string projectListJson = ProjectList(0, false, "2");
        }
    }



    protected void btnAddProjects_Click(object sender, EventArgs e)
    {
        try
        {
            int projectId = hdnProjectId.Value != "" ? Convert.ToInt32(hdnProjectId.Value) : 0;
            bool checkEditMode;
            if (projectId == 0)
            {
                checkEditMode = false;
            }
            else
            {
                checkEditMode = true;
            }
            string Title = hftxtProjectTile.Value.Replace("'", "''");
            int Customer = Convert.ToInt32(hftxtCustomer.Value.Replace("'", "''"));
            int Projmanager = Convert.ToInt32(hftxtProjectManager.Value.Replace("'", "''"));
            int BA = Convert.ToInt32(hftxtBA.Value.Replace("'", "''"));
            int AccountMgr = Convert.ToInt32(hftxtAccountMgr.Value.Replace("'", "''"));
            string AppraisalAuth = hftxtAppraisalAuthority.Value.Replace("'", "''");
            string ProjectDescription = hftxtProjectDescription.Value.Replace("'", "''");
            int PaymentCurrency = Convert.ToInt32(hftxtPaymentCurrency.Value.Replace("'", "''"));
            decimal ExchangeRate = hftxtExchangeRate.Value.Replace("'", "''") != "" ? Convert.ToDecimal(hftxtExchangeRate.Value.Replace("'", "''")) : 1;
            string DevelopmentTeam = hftxtDevelopmentTeam.Value.Replace("'", "''");
            DateTime StartDate = Convert.ToDateTime(hfStartDate.Value);
            string otherEmailIds = hftxtotherEmailIds.Value.Replace("'", "''");
           // DateTime Reportdate = Convert.ToDateTime(hfStartDate.Value);
            DateTime Reportdate = Convert.ToDateTime(hdnReportDate.Value);
            int inHouse = Convert.ToInt16(chkInHouse.Checked);
            int onGoing = Convert.ToInt16(chkOnGoing.Checked);
            string ProjectType = hftxtProjectType.Value.Replace("'", "''");
            decimal InitialProjectCost = 0;
            if (hftxtInitialProjectCost.Value != "")
            {
                InitialProjectCost = Convert.ToDecimal(hftxtInitialProjectCost.Value.Replace("'", "''"));
            }

            string statusmsg = "Save Failed";

            string mailconfirmation = hdnSendMail.Value;
            int isSendEmail = 0;
            if (chkTSEmail.Checked == true)
            {
                isSendEmail = 1;
            }
            else
            {
                isSendEmail = 0;
            }
            int IsTracked = 0;
            if (chkIsTracked.Checked == true)
            {
                IsTracked = 1;
            }
            else
            {
                IsTracked = 0;
            }
            int newProjectId;
            string emailData;
            string mailTo;
            string mailCC;
            projectMaster.UpdateProject(projectId, Customer, Title, ProjectDescription, Projmanager,AccountMgr,
            StartDate, "0", null, PaymentCurrency, ExchangeRate, 0, otherEmailIds, inHouse, onGoing, Reportdate, mailconfirmation, out newProjectId, out emailData,out mailTo,out mailCC, isSendEmail,ProjectType,InitialProjectCost,IsTracked,BA);

            int projectid = newProjectId;
            string MsgBody = emailData;
            if (projectid != 0)
            {
                int modulemasterid = projectModuleMaster.InsertprojectModuleMasterByProjId(projectid);

                string[] teamMembers = DevelopmentTeam.Split(',');
                projectMember.DeleteprojectMemberByprojid(projectid);
                for (int i = 0; i <= teamMembers.Count() - 1; i++)
                {
                    if (teamMembers[i].Trim() != "")
                    {
                        try
                        {
                            projectMember.InsertprojectMember(projectid, Convert.ToInt32(teamMembers[i].ToString()));
                        }
                        catch { }
                    }
                }

                string[] AppraisalAuths = AppraisalAuth.Split(',');
                projectMember.DeleteprojectAppraisalAuthorityMemberByprojid(projectid);
                for (int i = 0; i <= AppraisalAuths.Count() - 1; i++)
                {
                    if (AppraisalAuths[i].Trim() != "")
                    {
                        try
                        {
                            projectMember.InsertprojectAppraisalAuthorityMember(projectid, Convert.ToInt32(AppraisalAuths[i].ToString()));
                        }
                        catch { }
                    }
                }

                statusmsg = "Saved Successfully";

                string curUrl = projectid.ToString();
                if (checkEditMode == false)
                {
                    SetProjIdForMilestone(projectid.ToString(), ExchangeRate.ToString());

                    if (mailconfirmation == "true" && MsgBody != null)
                    {
                        txtTo.Text = mailTo;
                        txtCc.Text = mailCC;
                        txtSubject.Text = "Welcome to Intelegain !";
                        txtEmail.InnerText = MsgBody;
                        hdnshowHidediv.Value = "true";
                    }
                    else
                    {
                        Response.Redirect("/Member/Milestone.aspx");
                    }
                }
                else
                {
                    messageBox(statusmsg);
                }

               
            }
        }
        catch (Exception ex)
        {
            messageBox("Save Failed." + ex.Message);
        }
    }

    private void messageBox(string message)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('" + message + "');", true);
    }

    private void messageBoxandSamepage(string message, string Url)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgboxsamepage", "alert('" + message + "');OpenPopupinEditMode('" + Url + "');", true);
    }
    protected void btnAddMilstone_Click(object sender, EventArgs e)
    {
        int projectId = hdnProjectId.Value != "" ? Convert.ToInt32(hdnProjectId.Value) : 0;
        decimal ExchangeRate = hftxtExchangeRate.Value.Replace("'", "''") != "" ? Convert.ToDecimal(hftxtExchangeRate.Value.Replace("'", "''")) : 1;
        SetProjIdForMilestone(projectId.ToString(), ExchangeRate.ToString());
        Response.Redirect("/Member/Milestone.aspx");
    }


    [System.Web.Services.WebMethod]
    public static String BindAccountMgrDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(EmployeeMaster.GetAllAccountMgr());
    }

    [System.Web.Services.WebMethod]
    public static String BindAppraisalAuthorityDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(EmployeeMaster.GetAllAppraisalAuth());
    }

   
    [System.Web.Services.WebMethod]
    public static String SendMail(string mailTo,string Cc,string Bcc,string strSubject,string strMsgBody)
    {
        bool status = projectMaster.sendProjectWelcomeEmail(mailTo, Cc, Bcc, strSubject, strMsgBody);
        string message = string.Empty;
        if(status)
        {
            message = "Mail Sent";
        }

        return message;
    }
}
