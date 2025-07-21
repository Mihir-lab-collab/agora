using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class Member_TimesheetReport : Authentication
{
    string arg = "";
    clsCommon objCommon = new clsCommon();
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo () ;
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true, false, true); 

        if (lblMonth.Text != "")
        {
            lblMonth.Text = DateTime.Parse(lblMonth.Text).ToString("MMMM yyyy");
            Session["Timesheet"] = lblMonth.Text.ToString();
        }
        else
        {
            lblMonth.Text = DateTime.Today.ToString("MMMM yyyy");
            Session["Timesheet"] = lblMonth.Text.ToString();
        }

        Timesheet("", "");
    }

    protected void PagerButtonClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        arg = btn.CommandArgument.ToString();
        switch (arg)
        {
            case "next":
                lblMonth.Text = DateTime.Parse(lblMonth.Text).AddMonths(1).ToString("MMMM yyyy");
                Session["Timesheet"] = lblMonth.Text.ToString();
                break;

            case "prev":
                lblMonth.Text = DateTime.Parse(lblMonth.Text).AddMonths(-1).ToString("MMMM yyyy");
                Session["Timesheet"] = lblMonth.Text.ToString();
                break;
        }
   
        Timesheet("", "");
    }

    private void Timesheet(string sortExp, string sortDir)
    {
        grdTimesheet.DataSource = TimeSheet.GetTSReport(DateTime.Parse(lblMonth.Text).Month,
        DateTime.Parse(lblMonth.Text).Year, Convert.ToInt32(Session["LocationID"])); ;
        grdTimesheet.DataBind();
    }

    protected void grdTimesheet_Sorting(object sender, GridViewSortEventArgs e)
    {
        Timesheet(e.SortExpression, sortOrder);
    }

    protected void grdTimesheet_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTimesheet.PageIndex = e.NewPageIndex;
        Timesheet("", "");
    }

    public string sortOrder
    {
        get
        {
            if (ViewState["sortOrder"] == null)
            {
                ViewState["sortOrder"] = " asc";
            }

            else if (ViewState["sortOrder"].ToString() == "desc")
            {
                ViewState["sortOrder"] = "asc";
            }
            else
            {
                ViewState["sortOrder"] = "desc";
            }

            return ViewState["sortOrder"].ToString();
        }
        set
        {
            ViewState["sortOrder"] = value;
        }
    }
}