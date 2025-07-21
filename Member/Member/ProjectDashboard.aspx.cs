using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Web.UI.HtmlControls;
using CommonFunctionLib;
using Customer.BLL;

public partial class Member_ProjectDashboard : Authentication
{
    //string arg = "";
    //int count = 0;
    //DBFunc objDBFunction = new DBFunc();
    UserMaster UM;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblProjectName.Text = string.Empty;
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(true, false, true, false);
        pnlTree.Style["display"] = "none";



        if (Convert.ToInt32(Session["ProjectId"]) != 0)
        {
            ShowProjectDetails();
            pnlTree.Style["display"] = "block";
        }

    }





    public void ShowProjectDetails()
    {


        CSCode.Global.BIReportProcess();

        int projectid = Convert.ToInt32(Session["ProjectId"]);
        string DevloperTeam = "";
        string projectname = Session["ProjectName"].ToString();
        int empid =0;
        int Projid=0;
        string EmpSPTimeBYProj = "";
      ///  string totalEmpSPTimeBYProj = "";
        string ProjectDurationDetails = "";
        string ProjectSpendTime = "";
      ////  string ProjectStatus = "";

        foreach (var item in EmployeeMaster.GetEmpDetailsByProjId(projectid).ToList())
        {
            //DevloperTeam = item.empName + "," + DevloperTeam;
            
            empid = item.empid;
            Projid =projectid;
            //IList<TimeSheet> lstTimesheet = TimeSheet.GetProjTotalSpendTime(Projid, empid);
            //foreach (var items in TimeSheet.GetProjTotalSpendTime(Projid, empid))
            //{

            //    if (!string.IsNullOrEmpty(items.ProjectTotalHrs))
            //    {
            //        EmpSPTimeBYProj = items.ProjectTotalHrs;
            //    }
            //    else
            //    {
            //        EmpSPTimeBYProj = "0";
            //    }
             
            //}
            DevloperTeam = item.empName + "&nbsp;&nbsp;&nbsp;&nbsp;" + "<br/>" + DevloperTeam;
        }

        //if (!string.IsNullOrEmpty(DevloperTeam))
        //{
        //    DevloperTeam = DevloperTeam.Remove(DevloperTeam.Length - 1);
        //}




        //projectMaster objpm=


        List<projectMaster> ObjProjectdetails = Customer.BLL.projectMaster.Projects(false, 0, null, projectid);

        //IList<TimeSheet> lstTimesheetByProj = TimeSheet.GetProjTotalSpendTime(Projid, empid = 0);
        //foreach (var items in TimeSheet.GetProjTotalSpendTime(Projid, empid))
        //{
        //    totalEmpSPTimeBYProj = items.ProjectTotalHrs;
        //}

        int PMID = 0;
        bool showOverHeads = false;
        bool Added = true;
        bool Initiated = true;
        bool InProgress = true;
        bool UnderUAT = true;
        bool OnHold = true;
        bool CompletedClosed = true;
        bool Cancelled = true;
        bool UnderWarranty = true;
        bool TNM = true;
        bool FixedCost = true;
        List<BITS> lsProjects = new List<BITS>();

        lsProjects = BITS.GetProjectDetails(PMID, showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty,TNM,FixedCost);

        foreach (var ProjItem in lsProjects)
        {
            if (ProjItem.ID == projectid)
           {

               ProjectDurationDetails = ProjItem.Duration;
               ProjectSpendTime = Convert.ToString(ProjItem.ActualHour);
               if (!string.IsNullOrEmpty(ProjectSpendTime))
               {
                   lblprojectSpendTimeFor.Text = ProjectSpendTime + " Hours";
               }
               else
               {
                   lblprojectSpendTimeFor.Text = "0 Hours";
               }
               lblEndDatefor.Text = string.Join(string.Empty, ProjectDurationDetails.Skip(15));
               lblProjStatusFor.Text = ProjItem.Status;

               lblProjBudgetedHoursFor.Text = Convert.ToString(ProjItem.BudgetedHour);
               lblProjUnapprovedHoursFor.Text = Convert.ToString(ProjItem.UnApprovedHours);
               lblProjBugCostFor.Text = Convert.ToString(ProjItem.BudgetedCost);
               lblActualCustFor.Text = Convert.ToString(ProjItem.ActualCost);
           }
        }


        foreach (var item in ObjProjectdetails)
        {
            lblProjectName.Text = item.projName;
            lblCustomernameFor.Text = customerMaster.GetCustomerByCustId(item.custId).custCompany.ToString();
            EmployeeMaster emp = EmployeeMaster.GetEmployeeDetails(item.projManager);
            lblProjectManagerFor.Text = Convert.ToString(emp.empName);
            lblDeveloperTeamFor.Text = DevloperTeam;
            if (!string.IsNullOrEmpty(item.projDuration))
            {
                lblProjectDurationFor.Text = Convert.ToString(item.projDuration) + " Month." + "</br>" + ProjectDurationDetails;
            }
            lblProjestStartdateFor.Text = item.projStartDate.ToString("dd-MMM-yyyy");
          
        }

    }
}


