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
using AgoraBL.BAL;
using AjaxControlToolkit;

public partial class Member_Timesheet : Authentication
{
    string arg = "";
    int count = 0;
    DBFunc objDBFunction = new DBFunc();
    UserMaster UM;

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(true, false, true, false);
        txtDate.ReadOnly = false;
        CalendarExtender5.Enabled = true;
        if (lblMonth.Text != "")
        {
            lblMonth.Text = DateTime.Parse(lblMonth.Text).ToString("MMM yyyy");
        }
        else
        {
            lblMonth.Text = DateTime.Today.ToString("MMM yyyy");
        }
        bindgridSearchByProject("", "");
        if (hdProjID.Value != Session["ProjectID"].ToString())
        {
            AddModules();
        }
        hdProjID.Value = Session["ProjectID"].ToString();
    }

    #region Sort Order Function

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
    #endregion




    protected bool IsUdateEnabled()
    {
        bool status = Convert.ToBoolean(Eval("IsApproved"));
        if (status == true)
            return true;
        return false;

    }

    protected void gridSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DateTime tsDate = (DateTime)DataBinder.Eval(e.Row.DataItem, "TSDate");
            double tsHour = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "TSHour"));
            bool isSaturday = (bool)DataBinder.Eval(e.Row.DataItem, "IsSaturday");
            bool isSunday = (bool)DataBinder.Eval(e.Row.DataItem, "IsSunday");
            bool isLeave = (bool)DataBinder.Eval(e.Row.DataItem, "IsLeave");
            bool isHoliday = (bool)DataBinder.Eval(e.Row.DataItem, "IsHoliday");
            LinkButton btnDelete = (LinkButton)e.Row.FindControl("lnkDeleteTask");
            Label hrsValue = (Label)e.Row.Cells[5].FindControl("lblUpdatehours");
            bool status = (bool)DataBinder.Eval(e.Row.DataItem, "IsApproved");

            object tsEntryDateObj = DataBinder.Eval(e.Row.DataItem, "tsEntryDate");

            if (isSaturday || isSunday)
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFF8DC"); // light beige
            }
            if (isLeave)
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6F9FF"); // light blue
            }
            if (isHoliday)
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FDEBD0"); // light orange
            }

            if (tsHour == 0)
            {
                btnDelete.Visible = false;
                e.Row.Cells[10].Text = "";
            }
            else
            {
                btnDelete.Visible = true;
                if (tsEntryDateObj != DBNull.Value)
                {
                    e.Row.Cells[10].Text = Convert.ToDateTime(tsEntryDateObj).ToString("dd-MMM-yyyy");
                }
            }

            if (e.Row.RowIndex == 0)
            {
                count = 0;
            }
            if (!string.IsNullOrWhiteSpace(hrsValue.Text))
            {
                count = count + Convert.ToInt32(hrsValue.Text);
            }

            if (btnDelete.Enabled)
            {
                for (int i = 0; i < 11; i++)
                {
                    string variable = gridSearch.DataKeys[e.Row.RowIndex].Value.ToString();
                    e.Row.Cells[i].Attributes.Add("onclick", "DoEdit('" + variable + "','" + e.Row.RowIndex + "')");
                    e.Row.Cells[i].Attributes["onmouseover"] = "this.style.cursor='pointer';";
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblHoursTotal = (Label)e.Row.Cells[5].FindControl("lblHoursTotal");
            lblHoursTotal.Text = Convert.ToString(count);
        }
    }

    public void AddModules()
    {
        List<ProjectModule> modules = ProjectModule.GetModules(Convert.ToInt16(Session["ProjectID"].ToString())).ToList();
        foreach (ProjectModule item in modules)
        {
            item.Name = item.Name + "[" + Convert.ToString(item.ID) + "]";
        }
        ddlModule.Items.Clear();
        if (modules.Count == 0)
        {
            ddlModule.Items.Add(new ListItem("Select", "0"));
        }
        ddlModule.DataSource = modules;
        ddlModule.DataTextField = "Name";
        ddlModule.DataValueField = "ID";
        ddlModule.DataBind();
    }

    protected void UpdateTask(object sender, CommandEventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        if (btn.CommandName.ToString() == "Update")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                Label nametxt = (Label)row.FindControl("lblUpdatehours");
                Label description = (Label)row.FindControl("lblUpdateCommentName");
                Label ModuleName = (Label)row.FindControl("lblUpdateModuleName");
                Label moduleId = (Label)row.FindControl("lblModuleId");

                Label projMember = (Label)row.FindControl("lbldropProjectMember");
                Label projMember1 = (Label)row.FindControl("lblmemId");

                if (!string.IsNullOrEmpty(description.Text))
                    txtDiscription.Text = description.Text;

                txtDate.Text = row.Cells[3].Text;

                if (!string.IsNullOrEmpty(moduleId.Text))
                    ddlModule.SelectedValue = moduleId.Text;
                if (!string.IsNullOrEmpty(nametxt.Text))
                    ddlHours.SelectedValue = nametxt.Text;
                hdntaskId.Value = e.CommandArgument.ToString();

            }

        }
        bindgridSearchByProject("", "");
    }

    protected void DeleteTask(object sender, CommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument.ToString());
        EmpTimesheetBAL.Delete(id);
        bindgridSearchByProject("", "");
        txtDate.Text = "";
        txtDiscription.Text = "";
        Response.Redirect(Request.RawUrl);
    }


    protected void grdTimesheet_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label hrsValue = (Label)e.Row.Cells[2].FindControl("lblHours");
            Label taskID = (Label)e.Row.FindControl("lblTsId");
            count = count + Convert.ToInt32(hrsValue.Text);
            Label upstatus = (Label)e.Row.FindControl("lblStatus");
            e.Row.Cells[6].Visible = false;
            if (upstatus.Text == "True")
            {
                e.Row.BackColor = System.Drawing.Color.LightYellow;

            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    string variable = gridSearch.DataKeys[e.Row.RowIndex].Value.ToString();

                    e.Row.Cells[i].Attributes.Add("onclick", "OnEditProductSupplyMaster('" + variable + "','" + e.Row.RowIndex + "')");
                    e.Row.Cells[i].Attributes["onmouseover"] = "this.style.cursor='pointer';";
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblHoursTotal = (Label)e.Row.Cells[3].FindControl("lblHoursTotal1");
            lblHoursTotal.Text = Convert.ToString(count);
            e.Row.Cells[6].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[6].Visible = false;
        }
    }

    protected void btnClick_Click(object sender, EventArgs e)
    {
        if (hdStockTypeMasterID.Value != "0" && !string.IsNullOrEmpty(hdStockTypeMasterID.Value))
        {
            if (btnClick.CommandName.ToString() == "edit")
            {
                txtDate.ReadOnly = true;

                CalendarExtender5.Enabled = false;
                FillUserDetails(Convert.ToInt32(hdStockTypeMasterID.Value));
            }
        }
    }

    protected void grdTimesheet_Sorting(object sender, GridViewSortEventArgs e)
    {
        bindgridSearchByProject(e.SortExpression, sortOrder);
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

        bindgridSearchByProject("", "");
        txtDiscription.Text = "";
        txtDate.Text = "";
    }

    protected void bindgridSearchByProject(string sortExp, string sortDir)
    {        
        IList<TimeSheet> lstTimesheet = TimeSheet.GetTS(Convert.ToInt32(Session["ProjectID"].ToString()),
            UM.EmployeeID, DateTime.Parse(lblMonth.Text).Month, DateTime.Parse(lblMonth.Text).Year,
            0, 0, "");
        gridSearch.Visible = true;
        gridSearch.DataSource = lstTimesheet;
        gridSearch.DataBind();

    }

    protected void gridSearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        bindgridSearchByProject(e.SortExpression, sortOrder);
    }

    protected void gridSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        bindgridSearchByProject("", "");
    }

    #region Fill User Details
    public void FillUserDetails(int id)
    {
        if (Session["ProjectID"].ToString() == "0")
        {
            Page.RegisterStartupScript("", "<script type='text/javascript'>alert('Select the respective project to edit timesheet.');</script>");
        }
        else
        {
            IList<TimeSheet> lstTimesheet = TimeSheet.GetTS(id);

            if (lstTimesheet.Count > 0)
            {
                txtDate.Text = lstTimesheet[0].TSDate.ToString("dd-MMM-yyyy");
                ddlModule.SelectedValue = lstTimesheet[0].ModuleID.ToString();
                ddlHours.SelectedValue = lstTimesheet[0].TSHour.ToString();
                txtDiscription.Text = lstTimesheet[0].TSComment.ToString();
                hdntaskId.Value = lstTimesheet[0].TSID.ToString();
                // hdnIsSendMail.Value = lstTimesheet[0].IsSendMail.ToString();

            }
        }
    }

    #endregion

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        DateTime Entrydate = Convert.ToDateTime(txtDate.Text);
        if (Entrydate >= DateTime.Now)
        {
            Page.RegisterStartupScript("", "<script type='text/javascript'>alert('date should not be greater than currentdate');</script>");
            return;
        }


        int c = 0;

        //for (int i = 1; i <= 3; i++)
        //{
        //    //DateTime day = Convert.ToDateTime(DateTime.Now.AddDays(-i));
        //    //string datetocompare = day.ToString("yyyyMMdd");
        //    //TimeSheet ts = new TimeSheet();

        //    //List<Holiday> listHolidays = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);
        //    //var isHoliday = listHolidays.FirstOrDefault(x => x.HolidayDay == datetocompare);

        //    //string DayName = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(day.DayOfWeek);
        //    //List<EmpSaturdayHoliday> empSaturdayHoliday = ts.GetEmpSaturdayHoliday();

        //    //if (isHoliday != null)
        //    //{
        //    //    c = c + 1;
        //    //}
        //    //else if (DayName == "Sunday")
        //    //{
        //    //    c = c + 1;
        //    //    var isSaturday = empSaturdayHoliday.FirstOrDefault(x => x.WeekDate == Convert.ToDateTime(DateTime.Now.AddDays(-(i + 1))).ToString("yyyyMMdd"));
        //    //    if (isSaturday != null)
        //    //    {
        //    //        c = c + 1;
        //    //    }
        //    //}
        //}

        DateTime Fixdays;
        if (c == 0)
        {
            Fixdays = Convert.ToDateTime(DateTime.Now.AddDays(-3));
        }
        else
        {
            Fixdays = Convert.ToDateTime(DateTime.Now.AddDays(-(3 + c)));
        }

        int result = DateTime.Compare(Fixdays.Date, Entrydate.Date);

        if (result > 0)
        {
            bool Approval = AgoraBL.BAL.EmpTimesheetBAL.CheckForApproval(UM.EmployeeID, DateTime.Now.Date);
            if (Approval == false)
            {
                Page.RegisterStartupScript("", "<script type='text/javascript'>alert('You are not allowed to fill timesheet for this date');</script>");
                txtDate.Text = "";
                txtDiscription.Text = "";
                hdntaskId.Value = "";
                hdnIsSendMail.Value = "";
                ddlHours.SelectedValue = "8";
                return;
            }
        }


        int TSID;
        int ProjectId = Convert.ToInt32(Session["ProjectId"].ToString());

        // IList<TimeSheet> lstTimesheet = TimeSheet.GetIsSendMail(ProjectId);

        if (Int32.TryParse(hdntaskId.Value, out TSID))
        {
            TSID = Convert.ToInt32(hdntaskId.Value);
        }
        string isSendMail = Session["IsSendMail"].ToString();
        EmpTimesheetBAL.Update(Convert.ToInt32(ddlModule.SelectedValue), UM.EmployeeID, Entrydate, Convert.ToInt32(ddlHours.SelectedValue),
            txtDiscription.Text.Trim(), TSID);
        if (isSendMail == "True" || isSendMail == "true")
        {
            EmpTimesheetBAL objEmpTimesheetBAL = new EmpTimesheetBAL();
            clsTimeSheetEmail objTSEmail = new clsTimeSheetEmail();
            objTSEmail.IsWBS = false;
            objTSEmail.StartDate = Convert.ToDateTime(txtDate.Text);
            objTSEmail.ModuleName = ddlModule.SelectedItem.Text;
            objTSEmail.AttTSHour = ddlHours.SelectedValue.ToString();
            objTSEmail.TSComment = txtDiscription.Text;
            objTSEmail.ProjID = Convert.ToInt32(Session["ProjectId"].ToString());
            objTSEmail.ProjName = Session["ProjectName"].ToString();
            //objTSEmail.SendMail(TSID);
            objEmpTimesheetBAL.SendMail(objTSEmail.IsWBS, objTSEmail.StartDate, objTSEmail.ModuleName, objTSEmail.AttTSHour, objTSEmail.TSComment,
                objTSEmail.ProjID, objTSEmail.ProjName, UM.EmployeeID, UM.Name, TSID);
        }
        bindgridSearchByProject("", "");

        txtDate.Text = "";
        txtDiscription.Text = "";
        hdntaskId.Value = "";
        //hdnIsSendMail.Value = "";
        Response.Redirect("Timesheet.aspx");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtDate.Text = "";
        txtDiscription.Text = "";
        hdntaskId.Value = "";
        ddlHours.SelectedValue = "8";

    }


}



