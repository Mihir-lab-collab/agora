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
using iTextSharp.text;
using ListItem = System.Web.UI.WebControls.ListItem;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

public partial class Member_TimesheetManage : Authentication
{
    string arg = "";
    int count = 0;

    UserMaster UM;
    public int projectId = 0;
    public int locationId = 0;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    private string sortExpression = string.Empty; // Add this line
    protected void Page_Load(object sender, EventArgs e)
    {

        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(true, true, true, true);
        if (!this.IsPostBack)
        {
            if (lblMonth.Text != "")
            {
                lblMonth.Text = DateTime.Parse(lblMonth.Text).ToString("MMM yyyy");
            }
            else
            {
                lblMonth.Text = DateTime.Today.ToString("MMM yyyy");
            }
            MemberBind();
            ModuleBind();
            hdnCheckMonthClick.Value = "NotClick";
        }

        if (this.Request.Params["__EVENTTARGET"] == null || (!this.Request.Params["__EVENTTARGET"].Contains("btnSearch")
        && !this.Request.Params["__EVENTTARGET"].Contains("btnApproved") && !this.Request.Params["__EVENTTARGET"].Contains("btnSubmit")))
        {
            if (projectId != Convert.ToInt32(Session["ProjectId"]) || locationId != Convert.ToInt32(Session["LocationID"]))
            {

                projectId = Convert.ToInt32(Session["ProjectId"]);
                locationId = Convert.ToInt32(Session["LocationID"]);
                if (string.Compare(hdnCheckMonthClick.Value, "NotClick", true) == 0)
                {
                    MemberBind();
                    ModuleBind();

                }
                hdnCheckMonthClick.Value = "NotClick";
            }
            TimeSheetBind("", "");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DateTime fromDate;
        DateTime toDate;

        if (DateTime.TryParse(txtStartDate.Text, out fromDate) && DateTime.TryParse(txtEndDate.Text, out toDate))
        {
            if (fromDate > toDate)
            {
                Page.RegisterStartupScript("", "<script type='text/javascript'>alert('From Date cannot be greater than To Date.');</script>");
                return;
            }

            // Log data to check if dates are correct
            System.Diagnostics.Debug.WriteLine("Searching from {fromDate} to {toDate}");

            TimeSheetBindByDateRange(fromDate, toDate);
        }
        else
        {
            Page.RegisterStartupScript("", "<script type='text/javascript'>alert('Please enter valid dates.');</script>");
        }
    }

    private void TimeSheetBindByDateRange(DateTime fromDate, DateTime toDate)
    {
        IList<TimeSheet> lstTimesheet = null;
        TimeSheet ts = new TimeSheet();
        if (string.Compare(hdnCheckConsolidateTS.Value, "Click", true) == 0)
        {
            if (Convert.ToInt32(Session["ProjectId"]) > 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowPopUpInActive();", true);

                if (Convert.ToInt32(Session["ProjectId"]) > 0)
                {
                    lstTimesheet = TimeSheet.GetConsolidatedTimesheetByDateRange(Convert.ToInt32(Session["ProjectId"]), fromDate, toDate);
                }

                if (lstTimesheet != null && lstTimesheet.Count > 0)
                {
                    Session["ExportTimesheet"] = lstTimesheet;
                    ts = ts.GetStatus(Convert.ToInt32(Session["ProjectId"]));
                    if (ts != null)
                    {
                        lblProjectStartDate.Visible = true;
                        lblProjectStartDate.Text = Convert.ToDateTime(ts.ProjectStartDate).ToString("dd MMM yyyy");

                        lblProjectStausDate.Visible = true;
                        lblProjectStausDate.Text = ts.ProjectStaus;
                    }

                    var totalHours = lstTimesheet.Sum(l => l.TotalHours);
                    divCT.Visible = true;
                    gridConsolidateTimesheet.Visible = true;
                    gridConsolidateTimesheet.DataSource = lstTimesheet;
                    gridConsolidateTimesheet.DataBind();

                    Label lblProjectTotalHrs = (Label)gridConsolidateTimesheet.FooterRow.FindControl("lblProjectTotalHrs");
                    lblProjectTotalHrs.Text = totalHours.ToString();
                }
                else
                {
                    gridConsolidateTimesheet.Visible = true;
                    gridConsolidateTimesheet.DataSource = new DataTable(); // Empty data table
                    gridConsolidateTimesheet.DataBind();
                }
            }
        }
    }


    protected void Search_Click(object sender, EventArgs e)
    {
        TimeSheetBind("", "");
    }
    //Added for US-968
    protected void btnConsolidateTimesheet_Click(object sender, EventArgs e)
    {
        IList<TimeSheet> lstTimesheet = null;
        IList<TimeSheet> GetTimesheet = null;
        TimeSheet ts = new TimeSheet();

        if (string.Compare(hdnCheckConsolidateTS.Value, "Click", true) == 0)
        {
            if (Convert.ToInt32(Session["ProjectId"]) > 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowPopUpInActive();", true);

                if (Convert.ToInt32(ddlMember.SelectedValue) > 0 || Convert.ToInt32(Session["ProjectId"]) > 0)
                {
                    lstTimesheet = TimeSheet.GetCT(Convert.ToInt32(Session["ProjectId"]), 0, 0, "GetCT");
                }
                if (lstTimesheet != null && lstTimesheet.Count > 0)
                {
                    Session["ExportTimesheet"] = lstTimesheet;
                    ts = ts.GetStatus(Convert.ToInt32(Session["ProjectId"]));
                    if (ts != null)
                    {
                        lblProjectStartDate.Visible = true;
                        lblProjectStartDate.Text = Convert.ToDateTime(ts.ProjectStartDate).ToString("dd MMM yyyy");

                        lblProjectStausDate.Visible = true;
                        lblProjectStausDate.Text = ts.ProjectStaus;
                        //if (Convert.ToInt32(ts.ProjectStaus) >= 0 && Convert.ToInt32(ts.ProjectStaus) < 100)
                        //{
                        //    lblProjectStausDate.Text = "( Project in progress) " + Convert.ToDateTime(ts.ProjectStausDate).ToString("dd MMM yyyy");
                        //}
                        //else
                        //{
                        //    lblProjectStausDate.Text = "(Project completed) " + Convert.ToDateTime(ts.ProjectStausDate).ToString("dd MMM yyyy");
                        //}

                    }
                    var totalHours = lstTimesheet.Sum(l => l.TotalHours);
                    divCT.Visible = true;
                    gridConsolidateTimesheet.Visible = true;
                    gridConsolidateTimesheet.DataSource = lstTimesheet;
                    gridConsolidateTimesheet.DataBind();
                    Label lblProjectTotalHrs = (Label)gridConsolidateTimesheet.FooterRow.FindControl("lblProjectTotalHrs");
                    lblProjectTotalHrs.Text = totalHours.ToString();

                }
                else
                {
                    DataTable dt = new DataTable();
                    gridConsolidateTimesheet.Visible = true;
                    gridConsolidateTimesheet.DataSource = dt;
                    gridConsolidateTimesheet.DataBind();
                }
                //if (Convert.ToInt32(ddlMember.SelectedValue) > 0 || Convert.ToInt32(Session["ProjectId"]) > 0)
                //{
                //    GetTimesheet = TimeSheet.GetCT(Convert.ToInt32(Session["ProjectId"]), Convert.ToInt32(DateTime.Now.Month),
                //        Convert.ToInt32(DateTime.Now.Year), "GetConsolidateTimeSheetByMonth");

                //}
                //if (GetTimesheet != null && GetTimesheet.Count > 0)
                //{
                //    var totalHours = GetTimesheet.Sum(l => l.TotalHours);
                //    tblCTMonth.Visible = true;
                //    lblCurrentMonth.Text = DateTime.Today.ToString("MMM yyyy");
                //    gridConsolidateTimesheetForOneMonth.Visible = true;
                //    gridConsolidateTimesheetForOneMonth.DataSource = GetTimesheet;
                //    gridConsolidateTimesheetForOneMonth.DataBind();
                //    Label lblMonthTotalHours = (Label)gridConsolidateTimesheetForOneMonth.FooterRow.FindControl("lblMonthTotalHours");
                //    lblMonthTotalHours.Text = totalHours.ToString();
                //}
                //else
                //{
                //    DataTable dt = new DataTable();
                //    tblCTMonth.Visible = true;
                //    lblCurrentMonth.Text = DateTime.Today.ToString("MMM yyyy");
                //    gridConsolidateTimesheetForOneMonth.Visible = true;
                //    gridConsolidateTimesheetForOneMonth.ShowHeaderWhenEmpty = true;
                //    gridConsolidateTimesheetForOneMonth.DataSource = dt;
                //    gridConsolidateTimesheetForOneMonth.DataBind();
                //}
                //hdnCheckConsolidateTS.Value = "NotClick";
            }
            else
            {
                hdnCheckConsolidateTS.Value = "NotClick";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select a project!')", true);
            }
        }

    }

    protected void prev_next_Click(object sender, EventArgs e)
    {
        IList<TimeSheet> lstTimesheet = null;
        TimeSheet ts = new TimeSheet();
        LinkButton btn = (LinkButton)sender;
        arg = btn.CommandArgument.ToString();
        switch (arg)
        {
            case "next":
                lblCurrentMonth.Text = DateTime.Parse(lblCurrentMonth.Text).AddMonths(1).ToString("MMM yyyy");
                break;

            case "prev":
                lblCurrentMonth.Text = DateTime.Parse(lblCurrentMonth.Text).AddMonths(-1).ToString("MMM yyyy");
                break;
        }
        BindConsolidateTimesheet(Convert.ToDateTime(lblCurrentMonth.Text).Month, Convert.ToDateTime(lblCurrentMonth.Text).Year,
        Convert.ToInt32(Session["ProjectId"]));

        if (!string.IsNullOrEmpty(Convert.ToString(Convert.ToInt32(Session["ProjectId"]))))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowPopUpInActive();", true);
        if (Convert.ToInt32(ddlMember.SelectedValue) > 0 || Convert.ToInt32(Session["ProjectId"]) > 0)
        {
            lstTimesheet = TimeSheet.GetCT(Convert.ToInt32(Session["ProjectId"]), 0, 0, "GetCT");
        }
        if (lstTimesheet != null && lstTimesheet.Count > 0)
        {
            ts = ts.GetStatus(Convert.ToInt32(Session["ProjectId"]));
            if (ts != null)
            {
                lblProjectStartDate.Visible = true;
                lblProjectStartDate.Text = Convert.ToDateTime(ts.ProjectStartDate).ToString("dd MMM yyyy");

                lblProjectStausDate.Visible = true;
                lblProjectStausDate.Text = ts.ProjectStaus;
                //if (Convert.ToInt32(ts.ProjectStaus) >= 0 && Convert.ToInt32(ts.ProjectStaus) < 100)
                //{
                //    lblProjectStausDate.Text = "( Project in progress) " + Convert.ToDateTime(ts.ProjectStausDate).ToString("dd MMM yyyy");
                //}
                //else
                //{
                //    lblProjectStausDate.Text = "(Project completed) " + Convert.ToDateTime(ts.ProjectStausDate).ToString("dd MMM yyyy");
                //}

            }
            var totalHours = lstTimesheet.Sum(l => l.TotalHours);
            divCT.Visible = true;
            gridConsolidateTimesheet.Visible = true;
            gridConsolidateTimesheet.DataSource = lstTimesheet;
            gridConsolidateTimesheet.DataBind();
            Label lblProjectTotalHrs = (Label)gridConsolidateTimesheet.FooterRow.FindControl("lblProjectTotalHrs");
            lblProjectTotalHrs.Text = totalHours.ToString();

        }
        else
        {
            DataTable dt = new DataTable();
            gridConsolidateTimesheet.Visible = true;
            gridConsolidateTimesheet.ShowHeaderWhenEmpty = true;
            gridConsolidateTimesheet.DataSource = dt;
            gridConsolidateTimesheet.DataBind();
        }
    }

    private void BindConsolidateTimesheet(int month, int year, int projId)
    {
        IList<TimeSheet> GetTimesheet = null;
        TimeSheet ts = new TimeSheet();
        if (!string.IsNullOrEmpty(Convert.ToString(Convert.ToInt32(projId))))
        {
            GetTimesheet = TimeSheet.GetCT(Convert.ToInt32(Session["ProjectId"]), month, year, "GetConsolidateTimeSheetByMonth");
        }
        if (GetTimesheet != null && GetTimesheet.Count > 0)
        {
            ts = ts.GetStatus(Convert.ToInt32(Session["ProjectId"]));
            if (ts != null)
            {
                lblProjectStartDate.Visible = true;
                lblProjectStartDate.Text = Convert.ToDateTime(ts.ProjectStartDate).ToString("dd MMM yyyy");

                lblProjectStausDate.Visible = true;
                lblProjectStausDate.Text = ts.ProjectStaus;
                //if (Convert.ToInt32(ts.ProjectStaus) >= 0 && Convert.ToInt32(ts.ProjectStaus) < 100)
                //{
                //    lblProjectStausDate.Text = "( Project in progress) " + Convert.ToDateTime(ts.ProjectStausDate).ToString("dd MMM yyyy");
                //}
                //else
                //{
                //    lblProjectStausDate.Text = "(Project completed) " + Convert.ToDateTime(ts.ProjectStausDate).ToString("dd MMM yyyy");
                //}

            }
            var totalHours = GetTimesheet.Sum(l => l.TotalHours);
            tblCTMonth.Visible = true;
            //gridConsolidateTimesheetForOneMonth.Visible = true;
            //gridConsolidateTimesheetForOneMonth.DataSource = GetTimesheet;
            //gridConsolidateTimesheetForOneMonth.DataBind();
            //Label lblMonthTotalHours = (Label)gridConsolidateTimesheetForOneMonth.FooterRow.FindControl("lblMonthTotalHours");
            //lblMonthTotalHours.Text = totalHours.ToString();
        }
        //else
        //{
        //    DataTable dt = new DataTable();
        //    tblCTMonth.Visible = true;
        //    gridConsolidateTimesheetForOneMonth.Visible = true;
        //    gridConsolidateTimesheetForOneMonth.ShowHeaderWhenEmpty = true;
        //    gridConsolidateTimesheetForOneMonth.DataSource = dt;
        //    gridConsolidateTimesheetForOneMonth.DataBind();
        //}
    }

    //end
    private void ModuleBind()
    {
        ddlselectModule.Items.Clear();
        ddlselectModule.Items.Add(new ListItem("All", "0"));

        ddlselectModule.DataSource = ProjectModule.GetModules(projectId);
        ddlselectModule.DataTextField = "Name";
        ddlselectModule.DataValueField = "ID";
        ddlselectModule.DataBind();

        ddlAddModule.Items.Clear();
        ddlAddModule.DataSource = ProjectModule.GetModules(projectId);
        ddlAddModule.DataTextField = "Name";
        ddlAddModule.DataValueField = "ID";
        ddlAddModule.DataBind();
        ddlAddModule.Items.Insert(0, new ListItem("Select", String.Empty));
        ddlAddModule.SelectedIndex = 0;

    }

    private void MemberBind()
    {
        ddlMember.Items.Clear();
        ddlMember.DataSource = null;
        ddlMember.DataBind();
        ddlMember.Items.Add(new ListItem("All", "0"));
        ddlMember.DataSource = TimeSheet.GetMember(locationId,
            projectId);
        ddlMember.DataTextField = "empname";
        ddlMember.DataValueField = "empid";
        ddlMember.DataBind();

        ddlAddMember.Items.Clear();
        ddlAddMember.DataSource = null;
        ddlAddMember.DataBind();
        ddlAddMember.DataSource = TimeSheet.GetMember(locationId,
          projectId);
        ddlAddMember.DataTextField = "empname";
        ddlAddMember.DataValueField = "empid";
        ddlAddMember.DataBind();
        ddlAddMember.Items.Insert(0, new ListItem("Select", String.Empty));
        ddlAddMember.SelectedIndex = 0;

    }

    protected void TimeSheetBind(string sortExp, string sortDir)
    {
        IList<TimeSheet> lstTimesheet = null;

        if (Convert.ToInt32(ddlMember.SelectedValue) > 0 || Convert.ToInt32(Session["ProjectId"]) > 0)
        {
            lstTimesheet = TimeSheet.GetManageTS(Convert.ToInt32(Session["ProjectId"]),
           Convert.ToInt32(ddlMember.SelectedValue), DateTime.Parse(lblMonth.Text).Month,
           DateTime.Parse(lblMonth.Text).Year, Convert.ToInt32(ddlselectModule.SelectedValue),
           locationId, ddlStatus.SelectedValue);
        }
        if (lstTimesheet != null && lstTimesheet.Count > 0)
        {
            int cnt = 0;

            divTotlalhrs.Visible = true;
            LblTotalHrs.Text = lstTimesheet[0].ProjectTotalHrs.ToString();
        }
        else
            divTotlalhrs.Visible = false;

        gridSearch.Visible = true;
        gridSearch.DataSource = lstTimesheet;
        gridSearch.DataBind();
    }
    public string sortOrder
    {
        get
        {
            if (ViewState["sortOrder"] == null)
            {
                ViewState["sortOrder"] = "asc";
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


    protected void gridSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btnDelete = (LinkButton)e.Row.FindControl("lnkDeleteTask");
            Label hrsValue = (Label)e.Row.Cells[5].FindControl("lblUpdatehours");
            if (e.Row.RowIndex == 0)
            {
                count = 0;
            }

            count = count + Convert.ToInt32(hrsValue.Text);

            for (int i = 0; i < 12; i++)
            {
                string variable = gridSearch.DataKeys[e.Row.RowIndex].Value.ToString();

                e.Row.Cells[i].Attributes.Add("onclick", "EditTS('" + variable + "','" + e.Row.RowIndex + "')");
                e.Row.Cells[i].Attributes["onmouseover"] = "this.style.cursor='pointer';";
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblHoursTotal = (Label)e.Row.Cells[5].FindControl("lblHoursTotal");
            lblHoursTotal.Text = Convert.ToString(count);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime Entrydate = Convert.ToDateTime(txtDate.Text);
        if (Entrydate >= DateTime.Now)
        {
            Page.RegisterStartupScript("", "<script type='text/javascript'>alert('Date should not be greater than currentdate');</script>");
            return;
        }


        ////Start by ajit for validate  past 3 days
        DateTime Fixdays = Convert.ToDateTime(DateTime.Now.AddDays(-3));
        int result = DateTime.Compare(Fixdays.Date, Entrydate.Date);
        bool isadmin = UM.IsAdmin;
        if (result > 0)
        {
            bool Approval = EmployeeTimesheetRequestBLL.CheckForApproval(UM.EmployeeID, DateTime.Now.Date);
            if (Approval == false)
            {
                if (UM.IsAdmin == false)
                {
                    Page.RegisterStartupScript("", "<script type='text/javascript'>alert('You are not allowed to fill timesheet for this date');</script>");
                    return;
                }

            }
        }
        ////End by ajit for validate  past 3 days

        int TSID;
        if (Int32.TryParse(hdntaskId.Value, out TSID))
        {
            TSID = Convert.ToInt32(hdntaskId.Value);
        }

        TimeSheet.Update(Convert.ToInt32(ddlAddModule.SelectedValue), Convert.ToInt16(ddlAddMember.SelectedValue), Entrydate, Convert.ToInt32(ddlHours.SelectedValue), txtDiscription.Text.Trim(), TSID);
        TimeSheetBind("", "");

        txtDate.Text = "";
        txtDiscription.Text = "";
        hdntaskId.Value = "";
    }

    protected void DeleteTask(object sender, CommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();
        TimeSheet.Delete(Convert.ToInt32(id));
        TimeSheetBind("", "");
        txtDate.Text = "";
        txtDiscription.Text = "";
    }

    protected void btnChecked_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in gridSearch.Rows)
        {
            string taskid = gvRow.Cells[0].Text;
            CheckBox chkSel = (CheckBox)gvRow.FindControl("chkItemChecked");
            Label lblTsId = (Label)gvRow.FindControl("lblTsId");
            if (chkSel.Checked)
            {
                TimeSheet.Approve(Convert.ToInt32(lblTsId.Text), true, Convert.ToInt32(UM.EmployeeID));
            }
            else
                TimeSheet.Approve(Convert.ToInt32(lblTsId.Text), false, Convert.ToInt32(UM.EmployeeID));

        }
        TimeSheetBind("", "");
    }

    protected void btnClick_Click(object sender, EventArgs e)
    {
        if (hdStockTypeMasterID.Value != "0" && !string.IsNullOrEmpty(hdStockTypeMasterID.Value))
        {
            if (btnClick.CommandName.ToString() == "edit")
            {
                GetTS(Convert.ToInt32(hdStockTypeMasterID.Value));
            }
        }
    }

    protected void gridSearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        TimeSheetBind(e.SortExpression, sortOrder);
    }

    protected void PagerButtonClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        arg = btn.CommandArgument.ToString();
        switch (arg)
        {
            case "next":
                lblMonth.Text = DateTime.Parse(lblMonth.Text).AddMonths(1).ToString("MMM yyyy");
                break;

            case "prev":
                lblMonth.Text = DateTime.Parse(lblMonth.Text).AddMonths(-1).ToString("MMM yyyy");
                break;
        }
        TimeSheetBind("", "");
    }

    protected void btnMonthlyTS_Click(object sender, EventArgs e)
    {

        Response.Redirect("TimesheetReport.aspx");
    }

    public void GetTS(int id)
    {
        if (projectId == 0)
        {
            Page.RegisterStartupScript("", "<script type='text/javascript'>alert('Select the respective project to edit timesheet.');</script>");
        }
        else
        {
            IList<TimeSheet> lstTimesheet = TimeSheet.GetTS(id);

            if (lstTimesheet.Count > 0)
            {
                txtDate.Text = lstTimesheet[0].TSDate.ToString("dd-MMM-yyyy");
                txtDiscription.Text = lstTimesheet[0].TSComment.ToString();
                hdntaskId.Value = lstTimesheet[0].TSID.ToString();

                if (ddlAddModule.Items.Contains(new ListItem("102")))
                {
                    ddlAddModule.SelectedValue = lstTimesheet[0].ModuleID.ToString();
                }
                if (ddlHours.Items.Contains(new ListItem(lstTimesheet[0].TSHour.ToString())))
                {
                    ddlHours.SelectedValue = lstTimesheet[0].TSHour.ToString();
                }
                // if (ddlAddMember.Items.Contains(new ListItem(lstTimesheet[0].EmpID.ToString())))
                // if (ddlAddMember.Items.Contains(lstTimesheet[0].EmpID.ToString()))
                //{
                ddlAddMember.SelectedValue = lstTimesheet[0].EmpID.ToString();
                // }
            }
        }
    }

    protected void btnExcelSave_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        List<TimeSheet> lstTimeSheet = (List<TimeSheet>)Session["ExportTimesheet"];
        dt = GetTable(lstTimeSheet);
        if (dt.Rows.Count>0)
        {
            ExportToExcel(dt);
        }
        else
        {

        }

    }
    //public void ExportToExcel(DataTable dt)
    //{
    //    string fileName = "EmployeeConsolidateTimesheet.xlsx";

    //    DataGrid dg = new DataGrid();
    //    dg.AllowPaging = false;
    //    dg.DataSource = dt;
    //    dg.DataBind();

    //    System.Web.HttpContext.Current.Response.Clear();
    //    System.Web.HttpContext.Current.Response.Buffer = true;
    //    System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
    //    System.Web.HttpContext.Current.Response.Charset = "";
    //    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

    //    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
    //    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
    //    System.Web.UI.HtmlTextWriter htmlTextWriter =
    //      new System.Web.UI.HtmlTextWriter(stringWriter);
    //    dg.RenderControl(new HtmlTextWriter(Response.Output));
    //    Response.Flush();
    //    Response.End();
    //}
    public void ExportToExcel(DataTable dt)
    {
        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string fileName = "EmployeeConsolidateTimesheet.xlsx";
        HttpResponse response = HttpContext.Current.Response;

        response.Clear();
        response.Buffer = true;
        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        response.AddHeader("content-disposition", "attachment;filename=" + fileName);

        using (ExcelPackage package = new ExcelPackage())
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Add the headers
            for (int col = 1; col <= dt.Columns.Count; col++)
            {
                worksheet.Cells[1, col].Value = dt.Columns[col - 1].ColumnName;
            }

            // Add the data rows
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = dt.Rows[row][col];
                }
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                package.SaveAs(memoryStream);
                memoryStream.WriteTo(response.OutputStream);
            }
        }

        response.Flush();
        response.End();
    }




    private DataTable GetTable(List<TimeSheet> lstTimeSheet)
    {
        DataTable table = new DataTable();
        table.Columns.Add("Employee Name", typeof(string));
        table.Columns.Add("Hours", typeof(string));
        table.Columns.Add("Designation", typeof(string));
        table.Columns.Add("Module", typeof(string));

        for (int i = 0; i < lstTimeSheet.Count; i++)
        {
            table.Rows.Add(lstTimeSheet[i].EmpName, lstTimeSheet[i].TotalHours, lstTimeSheet[i].Designation, lstTimeSheet[i].Module);
        }
        var totalHours = lstTimeSheet.Sum(x => x.TotalHours);
        // table.Columns["Employee Name"].ExtendedProperties["Total Hours"] = "Total Hours";
        //table.Columns["Hours"].ExtendedProperties["Footer"] = totalHours;
        DataRow newRow = table.NewRow();
        newRow["Employee Name"] = "Total Hours";
        newRow["Hours"] = totalHours;
        table.Rows.Add(newRow);
        return table;
    }
    //[System.Web.Services.WebMethod]
    //public static String BindCT()
    //{
    //    IList<TimeSheet> lstTimesheet = null;
    //   // IList<TimeSheet> GetTimesheet = null;
    //    if (projId > 0)
    //    {
    //        lstTimesheet = TimeSheet.GetCT(projId, 0, 0, "GetCT");
    //    }
    //    lstTimesheet = TimeSheet.GetCT(382, 0, 0, "GetCT");
    //    var totalHours = lstTimesheet.Sum(l => l.TotalHours);
    //    var data = from ct in lstTimesheet
    //               orderby ct.Designation descending
    //               select new
    //               {
    //                   ct.Designation,
    //                   ct.EmpName,
    //                   ct.TotalHours,
    //               };
    //    JavaScriptSerializer jss = new JavaScriptSerializer();
    //    return jss.Serialize(data);
    //}

    protected void gridConsolidateTimesheet_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (!string.IsNullOrEmpty(Convert.ToString(Convert.ToInt32(Session["ProjectId"]))))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowPopUpInActive();", true);
    }
    protected void gridConsolidateTimesheet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridConsolidateTimesheet.PageIndex = e.NewPageIndex;
        if (!string.IsNullOrEmpty(Convert.ToString(Convert.ToInt32(Session["ProjectId"]))))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowPopUpInActive();", true);

        List<TimeSheet> lstTimeSheet = (List<TimeSheet>)Session["ExportTimesheet"];
        gridConsolidateTimesheet.DataSource = lstTimeSheet;
        gridConsolidateTimesheet.DataBind();
        var totalHours = lstTimeSheet.Sum(l => l.TotalHours);
        Label lblProjectTotalHrs = (Label)gridConsolidateTimesheet.FooterRow.FindControl("lblProjectTotalHrs");
        lblProjectTotalHrs.Text = totalHours.ToString();
    }
    //protected void btnExcelSaved_Click(object sender, EventArgs e)
    //{
    //    TimeSheetBind("", "");
    //    gridSearch.DataBind();

    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", "attachment;filename=TimesheetExport.xls");
    //    Response.Charset = "";
    //    Response.ContentType = "application/vnd.ms-excel";
    //    StringBuilder sb = new StringBuilder();

    //    sb.Append("<table border='1' style='border-collapse:collapse;'>");
    //    sb.Append("<tr style='font-weight:bold; background-color:#f2f2f2;'>");
    //    sb.Append("<th>Date</th>");
    //    sb.Append("<th>Project Name</th>");
    //    sb.Append("<th>Module Name</th>");
    //    sb.Append("<th>Project Member</th>");
    //    sb.Append("<th>Hour</th>");
    //    sb.Append("<th>Comment</th>");
    //    sb.Append("<th>Entry Date</th>");
    //    sb.Append("</tr>");

    //    double totalHours = 0;

    //    foreach (GridViewRow row in gridSearch.Rows)
    //    {
    //        sb.Append("<td>" + row.Cells[3].Text + "</td>");

    //        sb.Append("<td>" + row.Cells[5].Text + "</td>");

    //        Label lblModuleName = row.FindControl("lblUpdateModuleName") as Label;
    //        sb.Append("<td>" + (lblModuleName != null ? lblModuleName.Text : "") + "</td>");

    //        Label lblProjectMember = row.FindControl("lbldropProjectMember") as Label;
    //        sb.Append("<td>" + (lblProjectMember != null ? lblProjectMember.Text : "") + "</td>");

    //        Label lblHours = row.FindControl("lblUpdatehours") as Label;
    //        double hour = 0;
    //        if (lblHours != null && double.TryParse(lblHours.Text, out hour))
    //        {
    //            totalHours += hour;
    //        }
    //        sb.Append("<td>" + hour.ToString("0.##") + "</td>");

    //        Label lblComment = row.FindControl("lblUpdateCommentName") as Label;
    //        sb.Append("<td>" + (lblComment != null ? lblComment.Text.Replace("\r\n", "<br>") : "") + "</td>");

    //        Label lblEntryDate = row.FindControl("lblEntryDate") as Label;
    //        sb.Append("<td>" + (lblEntryDate != null ? lblEntryDate.Text : "") + "</td>");
    //        sb.Append("</tr>");
    //    }

    //    // Footer Row for Total Hours
    //    sb.Append("<tr>");
    //    sb.Append("<td colspan='4'></td>"); // Skip to Hour column
    //    sb.Append("<td style='font-weight:bold; background-color:#007bff; color:white;'>Total Hours</td>");
    //    sb.Append("<td colspan='2' style='font-weight:bold; background-color:#007bff; color:white; text-align:left;'>" + totalHours.ToString("0.##") + "</td>");
    //    sb.Append("</tr>");

    //    sb.Append("</table>");
    //    Response.Output.Write(sb.ToString());
    //    Response.Flush();
    //    Response.End();
    //}
    protected void btnExcelSaved_Click(object sender, EventArgs e)
    {
        // Bind your data
        TimeSheetBind("", "");
        gridSearch.DataBind();

        using (ExcelPackage package = new ExcelPackage())
        {
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("TimesheetExport");

            // Add header row
            ws.Cells[1, 1].Value = "Date";
            ws.Cells[1, 2].Value = "Project Name";
            ws.Cells[1, 3].Value = "Module Name";
            ws.Cells[1, 4].Value = "Project Member";
            ws.Cells[1, 5].Value = "Hour";
            ws.Cells[1, 6].Value = "Comment";
            ws.Cells[1, 7].Value = "Entry Date";

            // Optional: format header row
            using (var range = ws.Cells[1, 1, 1, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            int rowNumber = 2;
            double totalHours = 0;

            foreach (GridViewRow row in gridSearch.Rows)
            {
                ws.Cells[rowNumber, 1].Value = row.Cells[3].Text;
                ws.Cells[rowNumber, 2].Value = row.Cells[5].Text;

                Label lblModuleName = row.FindControl("lblUpdateModuleName") as Label;
                ws.Cells[rowNumber, 3].Value = lblModuleName != null ? lblModuleName.Text : "";

                Label lblProjectMember = row.FindControl("lbldropProjectMember") as Label;
                ws.Cells[rowNumber, 4].Value = lblProjectMember != null ? lblProjectMember.Text : "";

                Label lblHours = row.FindControl("lblUpdatehours") as Label;
                double hour = 0;
                if (lblHours != null && double.TryParse(lblHours.Text, out hour))
                {
                    totalHours += hour;
                }
                ws.Cells[rowNumber, 5].Value = hour;

                Label lblComment = row.FindControl("lblUpdateCommentName") as Label;
                ws.Cells[rowNumber, 6].Value = lblComment != null ? lblComment.Text : "";

                Label lblEntryDate = row.FindControl("lblEntryDate") as Label;
                ws.Cells[rowNumber, 7].Value = lblEntryDate != null ? lblEntryDate.Text : "";

                rowNumber++;
            }

            // Footer Row for Total Hours
            ws.Cells[rowNumber, 4].Value = "Total Hours";
            ws.Cells[rowNumber, 4].Style.Font.Bold = true;
            ws.Cells[rowNumber, 5].Value = totalHours;
            ws.Cells[rowNumber, 5].Style.Font.Bold = true;

            // Auto-fit columns
            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            // Return the file to the browser
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=TimesheetExport.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.Flush();
            Response.End();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
    }
}