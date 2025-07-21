using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// Summary description for clsTimeSheetEmail
/// </summary>
public class clsTimeSheetEmail
{
    public int ProjID { get; set; }
    public int UserID { get; set; }
    public string ProjName { get; set; }
    public int ModuleID { get; set; }
    public string ModuleName { get; set; }
    public int EmpID { get; set; }
    public string EmpName { get; set; }
    public int TSID { get; set; }
    public DateTime TSDate { get; set; }
    public double TSHour { get; set; }
    public string TSComment { get; set; }
    public string WBSName { get; set; }
    public Boolean IsWBS { get; set; }
    public int intWBSID { get; set; }
   
    public string AttHour { get; set; }
    public string AttTSHour { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime TSEntryDate { get; set; }
    //public string ProjectTotalHrs { get; set; }
    public string AccountManagerEmail { get; set; }
    public string ProjectManagerEmail { get; set; }
    public string ManagerName { get; set; }

    
    public clsTimeSheetEmail()
	{
		//
		// TODO: Add constructor logic here
		//

	}

    public static IList<clsTimeSheetEmail> getWBSID()
    {
        
        TimeSheetListDAL objTS = new TimeSheetListDAL();
        return objTS.getWBSID();
    }


    public void SendMail(int intID)//(int intWBSID, string empName, int intWBS)
    {
        UserMaster UM;
        UM = UserMaster.UserMasterInfo();
        int projectID = Convert.ToInt32(ProjID);//Convert.ToInt32(Session["ProjectId"].ToString());
        IList<TimeSheet> lstTimesheet = TimeSheet.GetEmailDetails(projectID);

        string PM_Name, PM_Email, AccEmail = "";
        if (lstTimesheet.Count > 0)
        {
            PM_Name = lstTimesheet[0].ManagerName.ToString();
            PM_Email = lstTimesheet[0].ProjectManagerEmail.ToString();
            AccEmail = lstTimesheet[0].AccountManagerEmail.ToString();
        }
        else
        {
            PM_Name = "";
            PM_Email = "";
            AccEmail = "";
        }

        string projectName = ProjName;//Session["ProjectName"].ToString();
        string strBody, strSubject, mailTo, mailFrom, message, CC = "";
        if (projectName == "Intelegain Internal")
        {
            mailTo = AccEmail;
        }
        else
        {
            mailTo = PM_Email;
        }
        if (IsWBS == true)
        {
            string ddlModule = ModuleName;//Convert.ToString(Request.Form["ctl00$ContentPlaceHolder1$hdnModuleName"]);
            int WBSID = intID;
            string module = ddlModule;
            string Comment = TSComment;//Convert.ToString(txtComment.Value).Trim();
            if (WBSID == 0)
            {
                IList<clsTimeSheetEmail> clsTimeSheetEmails = clsTimeSheetEmail.getWBSID();
                if (clsTimeSheetEmails.Count > 0)
                {
                    WBSID = clsTimeSheetEmails[0].intWBSID;
                }

                strSubject = "TimeSheet created of [" + StartDate.ToString("dd/MM/yyyy") + "] for " + projectName + " by " + UM.Name;
            }
            else
            {
                strSubject = "TimeSheet updated of [" + StartDate.ToString("dd/MM/yyyy") + "] for " + projectName + " by " + UM.Name;
                // strSubject = "TimeSheet updated of [" + Entrydate.ToString("dd/MM/yyyy") + "] for " + projectName + " by " + UM.Name;
            }

            strBody = @"<table><tr><td><b><u>Time Sheet Details<u><b></td></tr><tr><td><b>Name of Employee: </b>" + EmpName + "</td></tr><tr><td><b>Project Name: </b>" + projectName + "</td></tr><tr><td><b>WBS ID </b>" + WBSID + "</td></tr><tr><td><b>WBS Name: </b>" + WBSName + " </td></tr><tr><td><b>Start Date: </b>" + StartDate + " </td></tr><tr><td><b>End Date: </b>" + EndDate + " </td></tr><tr><td><b>Module: </b>" + ddlModule + " </td></tr><tr><td><b>Comment: </b>" + Comment + " </td></tr></table>";
            string dt = DateTime.Now.ToString("dd MMM yyyy");

            //mailTo = ConfigurationManager.AppSettings.Get("HREmail").ToString();
            mailFrom = CC = "";
            message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
        }
        else
        {
            DateTime Entrydate = StartDate;
           TSID=intID;
            if (TSID == 0)
            {
                strSubject = "TimeSheet created of [" + Entrydate.ToString("dd/MM/yyyy") + "] for " + projectName + " by " + UM.Name;
            }
            else
            {
                strSubject = "TimeSheet updated of [" + Entrydate.ToString("dd/MM/yyyy") + "] for " + projectName + " by " + UM.Name;
            }

            //mailTo = ConfigurationManager.AppSettings.Get("HREmail").ToString();
            mailFrom = CC = "";
        //    string strBody, strSubject, mailTo, mailFrom, message, CC = "";
            strBody = @"<table><tr><td><b><u>Time Sheet Details<u><b></td></tr><tr><td><b>Name of Employee: </b>"+UM.Name+"</td></tr><tr><td><b>Project Name: </b>" + projectName+"</td></tr><tr><td><b>TimeSheet Date: </b>" + Entrydate.ToString("dd/MMM/yyyy") + " </td></tr><tr><td><b>Module: </b>" + ModuleName + " </td></tr><tr><td><b>Hours: </b>" + AttTSHour + "</td></tr><tr><td><b>Description: </b>" + TSComment+ " </td></tr></table>";

            message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");

        }



    }
}