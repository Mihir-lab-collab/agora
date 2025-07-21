using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_BITS : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            if (UM == null)
                return;

            if (!(UM.IsAdmin || UM.IsModuleAdmin))
            {
                hdnAdmin.Value = Convert.ToString(UM.EmployeeID);
            }

        }

        //this.Form.Target = "_blank";
    }

    [System.Web.Services.WebMethod]
    public static string BindProjectDetails(int PMID, bool? showOverHeads, bool? Added, bool? Initiated, bool? InProgress, bool? UnderUAT, bool? OnHold, bool? CompletedClosed, bool? Cancelled, bool? UnderWarranty, bool? TNM, bool? FixedCost)
    {
        try
        {
            List<BITS> lsProjects = new List<BITS>();

            lsProjects = BITS.GetProjectDetails(PMID, showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);

            var data = from pr in lsProjects
                       select new
                       {
                           pr.ID,
                           pr.Name,
                           pr.PM,
                           pr.BA,
                           pr.AccManager,
                           pr.Duration,
                           pr.BudgetedHour,
                           pr.ActualHour,
                           pr.strUnApprovedHours,
                           pr.Status,
                           pr.BudgetedCost,
                           pr.ActualCost,
                           pr.ActualPayment,
                           pr.ProjectHealth_Effort,
                           pr.ProjectHealth_Cost,
                           pr.Reportdate,
                           //pr.PayReceivedIncreased,
                           //pr.ActualCostIncreased
                           DevelopmentTeam = String.Join(",", (from A in projectMember.GetProjectMembersNameByProjId(pr.ID).ToList() select A.empName).ToList()),//13-Dec-2022 User Story 125
                       };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception ex)
        {
            return null;
        }

    }

    [System.Web.Services.WebMethod]
    public static string GetTSBreakupDetails(int prjId)
    {
        try
        {
            List<BITS> lsProjects = new List<BITS>();

            lsProjects = BITS.GetTSBreakupDetails(prjId);

            var data = from pr in lsProjects
                       select new
                       {
                           pr.Module,
                           pr.ActualHour,
                           pr.Percentage_Effort,
                           pr.Cost,
                           pr.Reportdate
                       };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }

    }

    [System.Web.Services.WebMethod]
    public static string GetTimesheetDetailsMonthwise(int prjId)
    {
        try
        {
            List<BITS> lsProjects = new List<BITS>();

            lsProjects = BITS.GetTimesheetDetailsMonthwise(prjId);

            //list sorted by descending with order by TSyear
            var data = from pr in lsProjects
                       orderby pr.TSYear descending
                       select new
                       {
                           pr.Module,
                           pr.TSYear,
                           pr.TSMonth,
                           pr.ActualHour,
                           pr.Percentage_Effort,
                           //pr.UnApprovedHours,
                           pr.Cost,
                           pr.Reportdate
                       };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }

    }

    [System.Web.Services.WebMethod]
    public static string GetTimesheetDetailsWorkwise(int prjId, int year, int month)
    {
        try
        {
            List<BITS> lsProjects = new List<BITS>();

            lsProjects = BITS.GetTimesheetDetailsWorkwise(prjId, year, month);

            var data = from pr in lsProjects
                       select new
                       {
                           pr.Module,
                           pr.ActualHour,
                           pr.Percentage_Effort,
                           pr.Reportdate
                       };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }

    }

    //protected void imgRefresh_Click(object sender, ImageClickEventArgs e)
    //{
    //    Response.Redirect("http://localhost:51174//Crons/cron.aspx?m=BI");
    //}
}