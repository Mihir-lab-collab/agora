using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Web.UI.HtmlControls;
using CommonFunctionLib;

public partial class Member_ManageTimesheet : Authentication
{
    int monthNumber, year;
    string arg = "";
    int count = 0;
    int count1 = 0;
    SqlDataAdapter da;
    DataSet ds;
    SqlConnection con;
    clsCommon objCommon = new clsCommon();
    public int intMonth = DateTime.Now.Month;
    public int intYear = DateTime.Now.Year;
    public DateTime currentDate = DateTime.Now;
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Form.Attributes.Add("enctype", "multipart/form-data");
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            UM = UserMaster.UserMasterInfo();
            if (UM == null)
                return;

            currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            Session.Add("CurrentAttandenceDate", currentDate);
            intMonth = currentDate.Month;
            intYear = currentDate.Year;
            lblMonth.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
           
            hdLocationId.Value = objCommon.GetLocationAcess(UM.ProfileID).ToString();//userDetail.ProfileID).ToString();
            BindLocation();
            divLocation.Visible = true;
            gridSearch.Visible = true;
            trprojmember.Visible = true;
            tdmember.Visible = true;
            BindAllProjects();
            BindSearchDropdownModules();
            AdminAddMember();

            bindEmployeProject();
            bindEmpModulebyProjId();
            bindProjMemberList();
            btnApproved.Visible = true;
            tdApprove.Visible = true;
          
            BindAllProjects();
            bindgridSearchByProject("", "");
        }
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

    public void CheckValidUser()
    {
        if (UM == null)
        {
            Session.Abandon();
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["SiteRoot"] + "Login.aspx");
        }
        else
        {
            string pageName = System.IO.Path.GetFileName(Request.PhysicalPath).Replace(".aspx", "");
            if (UM.IsAdmin != true)//userDetail
            {
                int isvalid = CSCode.Global.validUser(UM.ProfileID, UM.UserType, pageName);
                if (isvalid == 0)
                {
                    Session.Abandon();
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["SiteRoot"] + "Login.aspx");
                }
            }
        }
    }

    protected bool IsUdateEnabled()
    {
        bool status = Convert.ToBoolean(Eval("tsVerified"));
        if (status == true)
            return true;
        return false;
    }

    protected void gridSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //don't defined count=0 inside function
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //LinkButton btnUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
            LinkButton btnDelete = (LinkButton)e.Row.FindControl("lnkDeleteTask");
            Label hrsValue = (Label)e.Row.Cells[5].FindControl("lblUpdatehours");
            if (((CheckBox)e.Row.FindControl("chkItemChecked")).Checked == true)
            {
                //btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                count = count + Convert.ToInt32(hrsValue.Text);
            }
            else
            {
                //btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                count = count + Convert.ToInt32(hrsValue.Text);
                for (int i = 0; i < 12; i++)
                {
                    string variable = gridSearch.DataKeys[e.Row.RowIndex].Value.ToString();
                    e.Row.Cells[i].Attributes.Add("onclick", "OnEditProductSupplyMaster('" + variable + "','" + e.Row.RowIndex + "')");
                    e.Row.Cells[i].Attributes["onmouseover"] = "this.style.cursor='pointer';";
                }
            }
        }
        //not displayed in design
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblHoursTotal = (Label)e.Row.Cells[5].FindControl("lblHoursTotal");
            lblHoursTotal.Text = Convert.ToString(count);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime srtdate = Convert.ToDateTime(txtDate.Text);
        if (srtdate >= DateTime.Now)
        {
            Page.RegisterStartupScript("", "<script type='text/javascript'>alert('date should not be greater than currentdate');</script>");
            return;
        }
        int TaskId, EmployeeId, hours, ModuleId = 0;
        hours = Convert.ToInt16(ddlHours.SelectedValue);
        if (string.IsNullOrEmpty(ddlModule.SelectedValue))
            ModuleId = 0;
        else
            ModuleId = Convert.ToInt32(ddlModule.SelectedValue);

        string Comment = txtDiscription.Text.Trim();
        if (Comment.Contains("'"))
            Comment = Comment.Replace("'", "''");
        else if (Comment.Contains("\n"))
            Comment = Comment.Replace("\n", "<br/>");

        DateTime Entrydate;
        string InputDate = txtDate.Text;
        Entrydate = Convert.ToDateTime(InputDate);
        EmployeeId = Convert.ToInt16(ddlAdminSelectMember.SelectedValue);

        if (hdntaskId.Value == "")
            TaskId = projectTimeSheet.InsertTimesheetList(ModuleId, EmployeeId, Entrydate, hours, Comment, 0, "INSERT");
        else
            TaskId = projectTimeSheet.InsertTimesheetList(ModuleId, EmployeeId, Entrydate, hours, Comment, Convert.ToInt32(hdntaskId.Value), "EDIT");

        if (ddlProj.SelectedValue != "-1")
        {
            if (UM.IsAdmin == true)
            {
                if (string.IsNullOrEmpty(Request.QueryString["p"]))
                {
                    BindgridTimesheet("", "");
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["p"]))
                {
                    grdTimesheet.Visible = false;
                    bindgridSearchByProject("", "");
                }
            }
            else if (UM.IsAdmin != true)
            {
                BindgridTimesheet("", "");
            }
        }
        else if (UM.IsAdmin == true && ddlempProjMember.SelectedValue != "0")
        {
            if (!string.IsNullOrEmpty(Request.QueryString["p"]))
            {
                grdTimesheet.Visible = false;
                bindgridSearchByEmployee("", "");
            }
        }

        txtDate.Text = "";
        txtDiscription.Text = "";
        hdntaskId.Value = "";
        bindgridSearchByProject("", ""); //Added by Pravin on 2 Jul 2014 to bind Grid after adding entry by PM
    }

    //'sbtnSubmit member
    private void AdminAddMember()
    {
        if (ddlProj.SelectedValue != "-1")
        {
            ddlAdminSelectMember.DataSource = TimesheetProjMember.AdminProjMemberList("AdminAddMemberInTimesheet", Convert.ToInt16(ddlProj.SelectedValue));
            ddlAdminSelectMember.DataTextField = "empName";
            ddlAdminSelectMember.DataValueField = "empid";
            ddlAdminSelectMember.DataBind();
        }
        else
        {
            ddlAdminSelectMember.DataSource = TimesheetProjMember.AdminProjMemberList("AdminAddMemberInTimesheet", -1);
            ddlAdminSelectMember.DataTextField = "empName";
            ddlAdminSelectMember.DataValueField = "empid";
            ddlAdminSelectMember.DataBind();
        }
    }
    //Add module for submit
    public void AddModules()
    {
        List<TimesheetModule> modules = TimesheetModule.EmpModule(Convert.ToInt16(ddlProj.SelectedValue), "AddModule").ToList();
        ddlModule.Items.Clear();
        if (modules.Count == 0)
        {
            ddlModule.Items.Add(new ListItem("Select", "0"));
        }
        ddlModule.DataSource = modules;
        ddlModule.DataTextField = "ModuleName";
        ddlModule.DataValueField = "moduleId";
        ddlModule.DataBind();
        //module add karne ke time project member add karvana
    }

    protected void grdTimesheet_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTimesheet.PageIndex = e.NewPageIndex;
        BindgridTimesheet("", "");
    }

    private void BindAllProjects()
    {
        ddlProj.Items.Clear();
        ddlProj.Items.Add(new ListItem("Select", "-1"));
        ddlProj.DataSource = TimeSheetList.GetAllProject();
        ddlProj.DataTextField = "project_Name";
        ddlProj.DataValueField = "project_Id";
        ddlProj.DataBind();
    }

    private void BindSearchDropdownModules()
    {
        ddlselectModule.Items.Clear();
        ddlselectModule.Items.Add(new ListItem("All", "0"));
        if (ddlProj.SelectedValue != "-1" || ddlselectModule.SelectedValue != "")
        {
            ddlselectModule.DataSource = TimesheetModule.EmpModule(Convert.ToInt16(ddlProj.SelectedValue), "FindModuleNameByProjectID");
            ddlselectModule.DataTextField = "moduleName";
            ddlselectModule.DataValueField = "moduleId";
            ddlselectModule.DataBind();
        }
    }

    private void BindProjMember()
    {
        ddlProjMember.Items.Clear();
        ddlProjMember.Items.Add(new ListItem("All", "0"));
        //search member
        //when we select project module wont be -1 it wd All=0 index
        //ddlProjMember.Items.Clear();
        //ddlProjMember.Items.Add(new ListItem("All", " 0"));
        //else give error in page load

        //Added by Pravin on 30 Jun 2014- starts here
        DateTime thisDate = Convert.ToDateTime(lblMonth.Text);
        intMonth = thisDate.Month;
        intYear = thisDate.Year;
        //Added by Pravin on 30 Jun 2014- ends here

        if (ddlProj.SelectedValue != "-1")
        {
            ddlProjMember.DataSource = TimesheetProjMember.GetAllTimesheetProjMember(Convert.ToInt16(ddlProj.SelectedValue), Convert.ToInt16(ddlselectModule.SelectedValue), intMonth, intYear);
            ddlProjMember.DataTextField = "empName";
            ddlProjMember.DataValueField = "empid";
            ddlProjMember.DataBind();
        }
    }

    private void bindProjMemberList()
    {
        setProjControl();
        ddlempProjMember.Items.Clear();
        ddlempProjMember.Items.Add(new ListItem("Select", "0"));
        ddlempProjMember.DataSource = TimesheetProjMember.PrjmemberByLocId(Convert.ToInt32(hdLocationId.Value));
        ddlempProjMember.DataTextField = "empname";
        ddlempProjMember.DataValueField = "empid";
        ddlempProjMember.DataBind();
        //if(ddlempProjMember.Items.Count > 1)
        setEmpControl();
        setempProjControl();
    }

    private void bindEmployeProject()
    {
        ddlEmpProj.Items.Clear();
        ddlEmpProj.Items.Add(new ListItem("All", "-1"));
        //for project all would be -1
        if (ddlempProjMember.SelectedValue != "0" && ddlempProjMember.SelectedValue != "")
            //select 0 inside project member assigned   
            ddlEmpProj.DataSource = TimesheetEmpProj.GetEmpProject(Convert.ToInt16(ddlempProjMember.SelectedValue), intMonth, intYear);
        ddlEmpProj.DataTextField = "projName";
        ddlEmpProj.DataValueField = "projId";
        ddlEmpProj.DataBind();
    }

    private void bindEmpModulebyProjId()
    {
        setProjControl();
        //THIS MODULE =PROJECT mEMBER+PROJiD
        ddlEmpModule.Items.Clear();
        ddlEmpModule.Items.Add(new ListItem("All", "0"));
        if (ddlempProjMember.SelectedIndex != 0 && ddlempProjMember.SelectedValue != "0")
        {
            if (ddlEmpProj.SelectedValue != "" && ddlEmpProj.SelectedValue != "0")
            {
                ddlEmpModule.DataSource = TimesheetModule.EmpModule(Convert.ToInt16(ddlEmpProj.SelectedValue), "FindModuleNameByProjectID");
                ddlEmpModule.DataTextField = "moduleName";
                ddlEmpModule.DataValueField = "moduleId";
                ddlEmpModule.DataBind();
            }
        }
    }

    protected void ddlProj_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        if (ddlProj.SelectedValue != "-1")
        {
            AdminAddMember();
            AddModules();
            BindSearchDropdownModules();
            BindProjMember();
            BindgridTimesheet("", "");
            bindgridSearchByProject("", "");
            //setEmpControl();
            //bindProjMemberList();
            //ddlempProjMember.SelectedIndex = 0;
            ddlProjMember.SelectedIndex = 0;
        }
     
        txtDate.Text = "";
        txtDiscription.Text = "";
    }

    protected void ddlProjMember_SelectedIndexChanged(object sender, EventArgs e)
    {
        setProjControl();
        bindEmployeProject();
    }

    protected void ddlEmpProj_SelectedIndexChanged(object sender, EventArgs e)
    {
        setProjControl();
        bindgridSearchByEmployee("", "");
        bindEmpModulebyProjId();
    }

    protected void ddlselectModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindProjMember();
        bindgridSearchByProject("", ""); // Added by Pravin on 2 Jul 2014
    }

    private void BindgridTimesheet(string sortExp, string sortDir)
    {
        UM = UserMaster.UserMasterInfo();
        int EmployeeId = UM.EmployeeID;

        string projId = ddlProj.SelectedValue;
        SqlDataAdapter da;
        DataSet ds;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("UpdateTimesheet", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProjId ", ddlProj.SelectedValue);
        cmd.Parameters.AddWithValue("@EmpId ", EmployeeId);
        cmd.Parameters.AddWithValue("@Month ", hdnMonthNo.Value);
        cmd.Parameters.AddWithValue("@Year ", hdnYear.Value);
        cmd.Parameters.AddWithValue("@arg", arg);
        cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
        da = new SqlDataAdapter(cmd);
        ds = new DataSet();
        da.Fill(ds);
        DataView myDataView = new DataView();
        myDataView = ds.Tables[0].DefaultView;

        if (sortExp != string.Empty)
        {
            myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }

        grdTimesheet.DataSource = myDataView;
        grdTimesheet.DataBind();
        //grdTimesheet.ShowFooter = true;
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
                //Label projMember2 = (Label)row.FindControl("lbldropProjectMember");

                if (!string.IsNullOrEmpty(description.Text))
                    txtDiscription.Text = description.Text;

                txtDate.Text = row.Cells[3].Text;

                if (!string.IsNullOrEmpty(projMember.Text))
                    ddlAdminSelectMember.SelectedItem.Text = projMember.Text;
                if (!string.IsNullOrEmpty(moduleId.Text))
                    ddlModule.SelectedValue = moduleId.Text;
                if (!string.IsNullOrEmpty(nametxt.Text))
                    ddlHours.SelectedValue = nametxt.Text;
                hdntaskId.Value = e.CommandArgument.ToString();
            }
        }
        //bindgridSearchByProject("", "");
    }

    protected void gridSearch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void DeleteTask(object sender, CommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();
        projectTimeSheet.DeleteTimesheet(Convert.ToInt32(id));
        bindgridSearchByProject("", "");
        txtDate.Text = "";
        txtDiscription.Text = "";
    }

    private void bindgridSearchByEmployee(string sortExp, string sortDir)
    {
        if (string.IsNullOrEmpty(hdnMonthNo.Value))
        {
            hdnMonthNo.Value = intMonth.ToString();
        }
        if (string.IsNullOrEmpty(hdnYear.Value))
        {
            hdnYear.Value = intYear.ToString();
        }

        string EmpprojMemberId = ddlempProjMember.SelectedValue;
        string EmpProjId = ddlEmpProj.SelectedValue;

        int ModuleId;
        if (ddlEmpModule.SelectedValue != "0" && ddlEmpModule.SelectedIndex != -1 && ddlEmpModule.SelectedValue != "")
            ModuleId = Convert.ToInt32(ddlEmpModule.SelectedValue);
        else
            ModuleId = 0;

        string EmpStatus = ddlEmpStatus.SelectedValue;
        SqlDataAdapter da;
        DataSet ds;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("sp_TimeSheetsearch", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@EmpProjectId", Convert.ToInt16(EmpProjId));
        cmd.Parameters.AddWithValue("@ProjMemberId", Convert.ToInt16(EmpprojMemberId));
        cmd.Parameters.AddWithValue("@prjmonth", hdnMonthNo.Value);
        cmd.Parameters.AddWithValue("@Projyear", hdnYear.Value);
        cmd.Parameters.AddWithValue("@EmpModuleId", Convert.ToInt32(ddlEmpModule.SelectedValue));
        cmd.Parameters.AddWithValue("@Status", EmpStatus);
        da = new SqlDataAdapter(cmd);
        ds = new DataSet();
        da.Fill(ds);
        DataView myDataView = new DataView();
        myDataView = ds.Tables[0].DefaultView;

        if (sortExp != string.Empty)
        {
            myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        //  if (IsPostBack)
        // {
        gridSearch.DataSource = myDataView;
        gridSearch.DataBind();
        //}
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
                //projectTimeSheet.UpdateTaskApprove(14939);
                projectTimeSheet.UpdateTaskApprove(Convert.ToInt32(lblTsId.Text));
            }
            else
                if (!chkSel.Checked)
                {
                    projectTimeSheet.UpdateTaskUnApprove(Convert.ToInt32(lblTsId.Text));
                }
        }
        bindgridSearchByProject("", "");
    }

    protected void grdTimesheet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //don't defined count=0 inside function
        //  DataRowView dtview = e.Row.DataItem as DataRowView;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //int i = 0;
            //e.Row.Cells[2].Attributes.Add("onclick", "OnEditProductSupplyMaster('" + grdTimesheet.DataKeys[2].Value.ToString() + "','" + e.Row.RowIndex + "')");
            //while 1< grdTimesheet.Rows.Count;
            Label hrsValue = (Label)e.Row.Cells[2].FindControl("lblHours");
            Label taskID = (Label)grdTimesheet.FindControl("lblTsId");
            count1 = count1 + Convert.ToInt32(hrsValue.Text);
            //string status = e.Row.Cells[0].Text;
            Label upstatus = (Label)e.Row.FindControl("lblStatus");
            e.Row.Cells[6].Visible = false;
            //Label upstatus = (Label)e.Row.FindControl("lblStatus");
            // LinkButton updateGrid = (LinkButton)e.Row.FindControl("btnEdit");
            if (upstatus.Text == "True")
            {
                // updateGrid.Enabled = false;
                //e.Row.Attributes.Add("style", "font-weight:bold;color:blue");
                e.Row.BackColor = System.Drawing.Color.LightYellow;
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    string variable = grdTimesheet.DataKeys[e.Row.RowIndex].Value.ToString();
                    e.Row.Cells[i].Attributes.Add("onclick", "OnEditProductSupplyMaster('" + variable + "','" + e.Row.RowIndex + "')");
                    e.Row.Cells[i].Attributes["onmouseover"] = "this.style.cursor='pointer';";
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblHoursTotal = (Label)e.Row.Cells[3].FindControl("lblHoursTotal1");
            lblHoursTotal.Text = Convert.ToString(count1);
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
                FillUserDetails(Convert.ToInt32(hdStockTypeMasterID.Value));
            }
        }
    }

    protected void grdTimesheet_Sorting(object sender, GridViewSortEventArgs e)
    {
        BindgridTimesheet(e.SortExpression, sortOrder);
    }

    protected void PagerButtonClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        arg = btn.CommandArgument.ToString();
        switch (arg)
        {
            case "next":
                currentDate = ((DateTime)Session["CurrentAttandenceDate"]).AddMonths(1);
                intMonth = currentDate.Month;
                hdnMonthNo.Value = intMonth.ToString();
                intYear = currentDate.Year;
                hdnYear.Value = intYear.ToString();
                Session["CurrentAttandenceDate"] = currentDate;
                lblMonth.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
                break;

            case "prev":
                currentDate = ((DateTime)Session["CurrentAttandenceDate"]).AddMonths(-1);
                intMonth = currentDate.Month;
                hdnMonthNo.Value = intMonth.ToString();
                intYear = currentDate.Year;
                hdnYear.Value = intYear.ToString();
                Session["CurrentAttandenceDate"] = currentDate;
                lblMonth.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
                //BindProjMember(); //Pravin
                break;

        }

        UM = UserMaster.UserMasterInfo();
        if (ddlProj.SelectedValue != "-1")
        {
            if (UM.IsAdmin == true)
            {
                if (string.IsNullOrEmpty(Request.QueryString["p"]))
                {
                    BindgridTimesheet("", "");
                }
                else
                {
                    grdTimesheet.Visible = false;
                    BindSearchDropdownModules();
                    BindProjMember();
                    bindgridSearchByProject("", "");
                }
            }
            else if (UM.IsAdmin != true)
            {
                BindgridTimesheet("", "");
            }
        }
        else if (UM.IsAdmin == true && ddlempProjMember.SelectedValue != "0")
        {
            if (!string.IsNullOrEmpty(Request.QueryString["p"]))
            {
                grdTimesheet.Visible = false;
                bindEmployeProject();
                bindEmpModulebyProjId();
                bindgridSearchByEmployee("", "");
            }
        }
        txtDiscription.Text = "";
        txtDate.Text = "";
    }

    protected void btnempsearch_Click(object sender, EventArgs e)
    {
        setProjControl();
        pnlEmployeeGridView.Visible = false;
        grdTimesheet.Visible = false;
        gridSearch.Visible = true;
        bindgridSearchByEmployee("", "");
    }

    protected void EditData(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        if (btn.CommandName.ToString() == "View")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                Label nametxt = (Label)row.FindControl("lblHours");
                Label description = (Label)row.FindControl("lblCommentName");
                Label ModuleName = (Label)row.FindControl("lblModuleName");
                Label ModuleId = (Label)row.FindControl("prjmoduleId");
                Label taskID = (Label)grdTimesheet.FindControl("lblTsId");
                             
                string Date = row.Cells[0].Text;
                ddlHours.SelectedValue = nametxt.Text;
                txtDiscription.Text = description.Text;
                txtDate.Text = Date;
                if (!string.IsNullOrEmpty(ModuleId.Text))
                {
                    ddlModule.SelectedValue = ModuleId.Text;
                }
                hdntaskId.Value = taskID.Text;
            }
        }
        BindgridTimesheet("", "");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("MonthlyTimesheetReport.aspx");
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        pnlEmployeeGridView.Visible = false;
        grdTimesheet.Visible = false;
        bindgridSearchByProject("", "");
    }

    protected void bindgridSearchByProject(string sortExp, string sortDir)
    {
        if (string.IsNullOrEmpty(hdnMonthNo.Value))
        {
            hdnMonthNo.Value = intMonth.ToString();
        }
        if (string.IsNullOrEmpty(hdnYear.Value))
        {
            hdnYear.Value = intYear.ToString();
        }

        gridSearch.Visible = true;
        string ProjMember;
        int ModuleId;
        string status;
        if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
        {
            status = ddlStatus.SelectedValue;
        }
        else
        {
            status = " ";
        }
        string projId = ddlProj.SelectedValue;
        arg = string.Empty;
        if (ddlProjMember.SelectedValue != "0" && ddlProjMember.SelectedIndex != -1 && ddlProjMember.SelectedValue != "")
            ProjMember = ddlProjMember.SelectedValue;
        else
            ProjMember = "0";
        if (ddlselectModule.SelectedValue != "0" && ddlselectModule.SelectedIndex != -1 && ddlselectModule.SelectedValue != "")
            ModuleId = Convert.ToInt32(ddlselectModule.SelectedValue);
        else
            ModuleId = 0;
        SqlDataAdapter da;
        DataSet ds;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("sp_TimeSheetsearchByProject", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProjId", Convert.ToInt32(ddlProj.SelectedValue));
        cmd.Parameters.AddWithValue("@Month", hdnMonthNo.Value);
        cmd.Parameters.AddWithValue("@Year", hdnYear.Value);
        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@PrjMember", Convert.ToInt32(ProjMember));
        cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
        da = new SqlDataAdapter(cmd);
        ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables.Count > 0)
        {
            DataView myDataView = new DataView();
            myDataView = ds.Tables[0].DefaultView;
            if (sortExp != string.Empty)
            {
                myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
            }
            gridSearch.DataSource = myDataView;
            gridSearch.DataBind();
        }
    }

    protected void gridSearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (ddlProj.SelectedValue != "-1")
        {
            bindgridSearchByProject(e.SortExpression, sortOrder);
        }
        if (ddlempProjMember.SelectedValue != "0")
        {
            bindgridSearchByEmployee(e.SortExpression, sortOrder);
        }
    }

    protected void gridSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTimesheet.PageIndex = e.NewPageIndex;
        if (ddlProj.SelectedValue != "-1")
        {
            bindgridSearchByProject("", "");
        }
        if (ddlEmpProj.SelectedValue != "-1")
        {
            bindgridSearchByEmployee("", "");
        }
    }

    private void setProjControl()
    {
        ddlProj.SelectedValue = "-1";
        ddlProjMember.Items.Clear();
        ddlProjMember.Items.Add(new ListItem("All", "0"));
        ddlProjMember.SelectedIndex = 0;

        ddlselectModule.Items.Clear();
        ddlselectModule.Items.Add(new ListItem("All", "0"));
        ddlselectModule.SelectedIndex = 0;

        ddlStatus.SelectedIndex = 0;

        txtDate.Text = "";
        txtDiscription.Text = "";
    }

    private void setEmpControl()
    {
        ddlempProjMember.SelectedValue = "0";
        ddlEmpProj.Items.Clear();
        ddlEmpProj.Items.Add(new ListItem("All", "0"));
        ddlEmpProj.SelectedIndex = 0;
    }

    private void setempProjControl()
    {
        if (ddlempProjMember.Items.Count > 1)
        {
            ddlempProjMember.Items.Add(new ListItem("All", "0"));
            ddlempProjMember.SelectedIndex = 0;
            ddlEmpStatus.SelectedIndex = 0;
        }
    }

    #region Fill User Details
    public void FillUserDetails(int id)
    {
        DBFunc objDBFunction = new DBFunc();
        DataTable dt = new DataTable();

        try
        {
            string strSql = string.Empty;
            string DocName = string.Empty;
            strSql = string.Empty;
            hdntaskId.Value = id.ToString();
            strSql = "SELECT * FROM projectTimeSheet WHERE tsId =" + id;
            dt = objDBFunction.ExecuteSQLRtnDT(strSql);
            if (dt.Rows.Count > 0)
            {
                txtDate.Text = Convert.ToDateTime(Convert.ToString(dt.Rows[0]["tsDate"].ToString())).Date.ToString("dd-MMM-yyyy");
                ddlModule.SelectedValue = !string.IsNullOrEmpty(dt.Rows[0]["moduleId"].ToString()) ? dt.Rows[0]["moduleId"].ToString() : "0";
                ddlHours.SelectedValue = dt.Rows[0]["tsHour"].ToString();
                txtDiscription.Text = !string.IsNullOrEmpty(dt.Rows[0]["tsComment"].ToString()) ? dt.Rows[0]["tsComment"].ToString().Replace("<br/>", "\n") : "";
                //UserDetails userDetail = HttpContext.Current.Session["DynoEmpSessionObject"] as UserDetails;
               // UM = HttpContext.Current.Session["UserMasterSession"] as UserMaster;
                if (UM.IsAdmin == true)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["p"]))
                    {
                        ddlAdminSelectMember.SelectedValue = !string.IsNullOrEmpty(dt.Rows[0]["empName"].ToString()) ? dt.Rows[0]["empName"].ToString() : "0";
                    }
                }
            }
            dt.Clear();
            dt.Dispose();
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    protected void dlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdLocationId.Value = dlLocation.SelectedValue;
        bindProjMemberList();
    }

    private void BindLocation()
    {
        DataTable dtEmployeeLocation = new DataTable();
        dtEmployeeLocation = objCommon.EmployeeLocationList();
        dlLocation.DataSource = dtEmployeeLocation;
        dlLocation.DataTextField = "Name";
        dlLocation.DataValueField = "LocationID";
        dlLocation.DataBind();
        dlLocation.SelectedValue = "10";
        hdLocationId.Value = dlLocation.SelectedValue;
        dlLocation.Visible = true;
    }

    protected bool IsAutopostback()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["p"]))
            return true;
        else return false;
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["p"]))
        {
            if (ddlProj.SelectedValue != "-1")
            {
                BindgridTimesheet("", "");
            }
        }
    }

    protected void ddlProjMember_SelectedIndexChanged1(object sender, EventArgs e)
    {
        bindgridSearchByProject("", "");
    }
}