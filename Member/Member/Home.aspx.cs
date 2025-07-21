using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using Customer.BLL;
using System.Text;
using System.Web.UI.HtmlControls;
using Customer.DAL;
using CSCode; // need to comment
 
public partial class _Default : Authentication
{
    clsCommon objCommon = new clsCommon();
    DataTable dTable = new DataTable();
    public int intMonth = DateTime.Now.Month;
    public DateTime currentDate = DateTime.Now;
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, false, false, false, true);
        if (UM.IsRemoteEmployee)
        {
            btnGoToEmployeeRemoteAtt.Visible = true;
        }
        else
        {
            btnGoToEmployeeRemoteAtt.Visible = false; ;
        }

        if (!IsPostBack)
        {

            // DailyReport(); //for testing by trupti Consolidated report
            Control accordionProjectHealthAlert = accordionHome.FindControl("AccordionProjectHealthAlert");
            Control accordionPendingProjectStatusAlert = accordionHome.FindControl("AccordionPendingProjectStatusAlert");
            accordionProjectHealthAlert.Visible = false;
            accordionPendingProjectStatusAlert.Visible = false;

            //--------------vw  Dynamic file Path Set Start --------------------
            int fileCount;
            List<UploadPolicyBLL> ListFile = UploadPolicyBLL.GetHrPolicyFilesDetails("GET");
            fileCount = ListFile.Count;
            if (fileCount > 0)
            {
                fileCount = ListFile.Count - 1;
                for (int i = 0; i <= fileCount; i++)
                {
                    //if (ListFile[i].FileName == "Hr_Manual.pdf")
                    if (ListFile[i].FileName.StartsWith("HR_Policy", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lblHr_Manual.Text = Convert.ToDateTime(ListFile[i].CreateDate).ToString("dd-MMM-yyyy");
                        hdn_Hrmanual.Value = ListFile[i].DisplayFileURL.ToString();                        
                    }
                    //else if (ListFile[i].FileName == "Hr_Medical.pdf")
                    if (ListFile[i].FileName.StartsWith("Mediclaim_Policy", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lblHr_Mediclame.Text = Convert.ToDateTime(ListFile[i].CreateDate).ToString("dd-MMM-yyyy");
                        hdn_Mediclaim.Value = ListFile[i].DisplayFileURL.ToString();                       

                    }
                    else if (ListFile[i].FileName.StartsWith("ASH_Policy", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lbl_AntiASHP.Text = Convert.ToDateTime(ListFile[i].CreateDate).ToString("dd-MMM-yyyy");
                        hdn_ASHP.Value = ListFile[i].DisplayFileURL.ToString();                        
                    }
                    else
                    {
                    }
                   
                }
            }
            //--------------vw  Dynamic file Path Set End --------------------

            FileDownload.HRef = "DownloadFile.aspx?Filename=" + objCommon.HRManualFileName() + "&Folder=" + objCommon.GetEncriptFileName("~/ManualsDocument/");
            A1.HRef = "DownloadFile.aspx?Filename=" + objCommon.MediclaimFileName() + "&Folder=" + objCommon.GetEncriptFileName("~/ManualsDocument/");
            A2.HRef = "DownloadFile.aspx?Filename=" + objCommon.AntiHarasmentFileName() + "&Folder=" + objCommon.GetEncriptFileName("~/ManualsDocument/");
            string status = projectMaster.checkManagerforProject("checkManagerforProject", UM.EmployeeID.ToString());

            List<TimeSheet> lstTS = TimeSheet.GetIncomepleteTS(DateTime.Now.Month, DateTime.Now.Year, 0, UM.EmployeeID);
            if (lstTS.Count > 0)
            {
                TSGrid.DataSource = lstTS;
                TSGrid.DataBind();
            }
            else
            {
                TSGrid.DataSource = null;
                TSGrid.DataBind();
                divAlert.Visible = false;
            }

            if (status == "True" || UM.IsAdmin)
            {
                ProjectHealthAlert.Text = "Project Health Alert";
                List<ProjectHourDetails> ProjectHourlist = ProjectHourDetails.GetDetails(Convert.ToInt32(UM.EmployeeID));
                List<ProjecEstDetails> ProjectEstList = ProjecEstDetails.GetEstDetails();

                if (ProjectHourlist.Count > 0)
                {

                    gvParentGrid.DataSource = ProjectHourlist;
                    gvParentGrid.DataBind();
                    accordionProjectHealthAlert.Visible = true;
                }
                else
                {
                    gvParentGrid.DataSource = null;
                    gvParentGrid.DataBind();
                    accordionProjectHealthAlert.Visible = false;
                }
            }

            if (status == "True" || UM.IsAdmin)
            {
                ProjectStatusAlert.Text = "Pending Project Status";
                List<projectMaster> lstIncompleterStatus = projectMaster.GetInCompleteProjectsStatus("GetIncompleStatus", UM.EmployeeID.ToString());
                if (lstIncompleterStatus.Count > 0)
                {

                    IncompeteStatus.DataSource = lstIncompleterStatus;
                    IncompeteStatus.DataBind();
                    accordionPendingProjectStatusAlert.Visible = true;

                }
                else
                {
                    IncompeteStatus.DataSource = null;
                    IncompeteStatus.DataBind();
                    accordionPendingProjectStatusAlert.Visible = false;
                }
            }

            if (accordionProjectHealthAlert.Visible == true && accordionPendingProjectStatusAlert.Visible == true)
            {
                accordionHome.SelectedIndex = 3;
            }

            if (accordionProjectHealthAlert.Visible == true && accordionPendingProjectStatusAlert.Visible == false)
            {
                accordionHome.SelectedIndex = 2;
            }

            if (accordionProjectHealthAlert.Visible == false && accordionPendingProjectStatusAlert.Visible == true)
            {
                accordionHome.SelectedIndex = 2;
            }

            if (accordionProjectHealthAlert.Visible == false && accordionPendingProjectStatusAlert.Visible == false)
            {
                accordionHome.SelectedIndex = 1;
            }
            //Added by shubham Alekar = 20/12/2022
            DateTime Today = DateTime.Now;
            //lblHolidays.Text = "Holidays " + (Today.Month >= 4 ? Today.ToString("yyyy") + "-" + Today.AddYears(1).ToString("yy") : Today.AddYears(-2).ToString("yyyy") + "-" + Today.ToString("yy"));
            //List<Holiday> listHolidays = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);
            //if (listHolidays != null)
            
            lblHolidays.Text = " Holidays "   +  (Today.ToString("yyyy")+  "-" +  Today.AddYears(1).ToString("yy"));
            List<Holiday> listHolidays = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);
             if (listHolidays != null)
            {
                if (listHolidays.Count() > 0)
                {
                    gvHolidayView.DataSource = listHolidays;
                    gvHolidayView.DataBind();
                }
                else
                {
                    gvHolidayView.DataSource = null;
                    gvHolidayView.DataBind();
                }
            }



            // CIP Sessions
            lblEvents.Text = "CIP Sessions (Continuous Improvement Program) " + (Today.ToString("yyyy") + "-" + Today.AddYears(1).ToString("yy"));
            List<CIPBLL> listEvents = CIPBLL.GetEvents("SELECT_YEARLY", 0, 0);
            if (listEvents != null)
            {
                if (listEvents.Count() > 0)
                {
                    grdCIP.DataSource = listEvents;
                    grdCIP.DataBind();
                }
                else
                {
                    grdCIP.DataSource = null;
                    grdCIP.DataBind();
                }
            }

            //news
            List<Notice> listNotice = Notice.SelectNotice("SelectNotice");
            if (listNotice != null)
            {
                if (listNotice.Count() > 0)
                {
                    gvNews.DataSource = listNotice;
                    gvNews.DataBind();
                }
                else
                {
                    gvNews.DataSource = null;
                    gvNews.DataBind();
                }
            }

            //upcoming occasions
            dTable = null;
            dTable = CSCode.Global.Events(UM.LocationID);
            if (dTable != null)
            {
                if (dTable.Rows.Count > 0)
                {
                    grdupcoming.DataSource = dTable;
                    grdupcoming.DataBind();
                }
                else
                {
                    grdupcoming.DataSource = null;
                    grdupcoming.DataBind();
                }
            }

            //events
            List<EmployeeMaster> lstEvents = EmployeeMaster.SelectEvents("Events", Convert.ToInt32(UM.LocationID), 0).ToList();
            if (lstEvents.Count() > 0)
            {
                Bday.Visible = true;
                foreach (var evt in lstEvents)
                {
                    string body = "<div style='font-size: 18px; margin-top: 10px;float:Left; font-weight:bold;'>" +
                    "Happy " + evt.Event + ": <span <span style='color:#E69503;' >" + evt.empName + "</span> </div><br/><br/>";
                    Bday.InnerHtml = Bday.InnerHtml + body;
                }
                divBirthday.Visible = true;
            }
            else
            {
                divBirthday.Visible = false;
                Bday.Visible = false;
            }

            AppraisalReportBLL objappraisal = new AppraisalReportBLL();
            DataSet IsEmpAppraisal = objappraisal.CheckEmpAppraisal(UM.EmployeeID, "Employee");

            Session["empId"] = UM.EmployeeID;

            if (IsEmpAppraisal.Tables[0].Rows.Count > 0)
            {
                AppDiv.Visible = true;
                divEmpAppraisal.Visible = true;
            }
            else
            {
                divEmpAppraisal.Visible = false;
                AppDiv.Visible = false;
            }

            DataSet IsAppraisal = objappraisal.CheckEmpAppraisal(UM.EmployeeID, "Manager");

            if (IsAppraisal.Tables[0].Rows.Count > 0 && IsAppraisal.Tables[1].Rows.Count > 0)
            {
                AppDiv.Visible = true;
                divAppraisal.Visible = true;
            }
            else
            {
                divAppraisal.Visible = false;
                AppDiv.Visible = false;

            }

            currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            Session.Add("CurrentAttandenceDate", currentDate);
            intMonth = currentDate.Month;
            lblMonthYear.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);

            lblApkVersion.Text = System.Configuration.ConfigurationManager.AppSettings.Get("Android_apk_version");
            //lblApkName.Text = System.Configuration.ConfigurationManager.AppSettings.Get("Android_apk_filename_stagging").Replace(".apk", "");
        }
    }

    protected void gvUserInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView gv = (GridView)e.Row.FindControl("gvChildGrid");
            int PrjectId = Convert.ToInt32(e.Row.Cells[1].Text);
            List<ProjecEstDetails> ProjectEstList = ProjecEstDetails.GetEstDetails();
            List<ProjecEstDetails> ProjectList = ProjectEstList.Where(x => x.ProjID == PrjectId).ToList();
            if (ProjectList.Count != 0)
            {
                gv.DataSource = ProjectList;
                gv.DataBind();
            }
            else
            {
                gv.DataSource = null;
                gv.DataBind();
                Label lblEmptyMessage = gv.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                lblEmptyMessage.Text = "Currently there are no records in system.";
            }


            HtmlGenericControl PM = (HtmlGenericControl)e.Row.FindControl("PM");
            HtmlGenericControl AM = (HtmlGenericControl)e.Row.FindControl("AM");
            HtmlGenericControl BA = (HtmlGenericControl)e.Row.FindControl("BA");
            PM.InnerHtml = "Project Manager:";
            AM.InnerHtml = "Account Manager:";
            BA.InnerHtml = "Business Analyst:";

            HtmlGenericControl PMName = (HtmlGenericControl)e.Row.FindControl("PMName");
            HtmlGenericControl AMName = (HtmlGenericControl)e.Row.FindControl("AMName");
            HtmlGenericControl BAName = (HtmlGenericControl)e.Row.FindControl("BAName");
            PMName.InnerHtml = ((Customer.BLL.ProjectHourDetails)(e.Row.DataItem)).ProjectManager;
            AMName.InnerHtml = ((Customer.BLL.ProjectHourDetails)(e.Row.DataItem)).AccountManager;
            BAName.InnerHtml = ((Customer.BLL.ProjectHourDetails)(e.Row.DataItem)).BusinessAnalyst;

        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            currentDate = ((DateTime)Session["CurrentAttandenceDate"]);
            intMonth = currentDate.Month;
            Panel CurrentPanel = (Panel)((LinkButton)sender).Parent;

            if (CurrentPanel != null)
            {
                TextBox txtCheckInHour = (TextBox)CurrentPanel.FindControl("txtCheckInHour");
                TextBox txtCheckInMinute = (TextBox)CurrentPanel.FindControl("txtCheckInMinute");
                TextBox txtCheckOutHour = (TextBox)CurrentPanel.FindControl("txtCheckOutHour");
                TextBox txtCheckOutMinute = (TextBox)CurrentPanel.FindControl("txtCheckOutMinute");
                string strStatus = "p";
                string CheckINTime = string.Empty;
                string CheckOUTTime = string.Empty;
                string workingHours = string.Empty;
                DateTime dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                DateTime dtCheckIN = new DateTime();
                DateTime dtCheckOut = new DateTime();
                if (txtCheckInHour.Text.Trim() != "" && txtCheckInMinute.Text.Trim() != "")
                {
                    CheckINTime = (txtCheckInHour.Text.Trim().Length > 1 ? txtCheckInHour.Text.Trim() : "0" + txtCheckInHour.Text.Trim()) + "." + (txtCheckInMinute.Text.Trim().Length > 1 ? txtCheckInMinute.Text.Trim() : "0" + txtCheckInMinute.Text.Trim());
                    dtCheckIN = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt16(txtCheckInHour.Text.Trim()), Convert.ToInt16(txtCheckInMinute.Text.Trim()), 0);
                }

                if (txtCheckOutHour.Text.Trim() != "" && txtCheckOutMinute.Text.Trim() != "")
                {
                    CheckOUTTime = (txtCheckOutHour.Text.Trim().Length > 1 ? txtCheckOutHour.Text.Trim() : "0" + txtCheckOutHour.Text.Trim()) + "." + (txtCheckOutMinute.Text.Trim().Length > 1 ? txtCheckOutMinute.Text.Trim() : "0" + txtCheckOutMinute.Text.Trim());
                    dtCheckOut = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt16(txtCheckOutHour.Text.Trim()), Convert.ToInt16(txtCheckOutMinute.Text.Trim()), 0);
                }

                Attendance objatt = new Attendance();
                objatt.empId = UM.EmployeeID;
                objatt.attDate = dtNow;
                objatt.attStatus = strStatus;
                objatt.attComment = "";
                objatt.adminID = UM.EmployeeID;

                int empid = new Attendance().CheckExistsEmp("CheckExistsEmp", UM.EmployeeID, dtNow.ToShortDateString());
                if (empid == 0)
                {
                    objatt.mode = "Insert";
                }
                else
                {
                    objatt.mode = "update";
                }

                if (!string.IsNullOrEmpty(CheckINTime) && !string.IsNullOrEmpty(CheckOUTTime))
                {
                    objatt.attInTime = dtCheckIN;
                    objatt.attOutTime = dtCheckOut;
                }
                else if (!string.IsNullOrEmpty(CheckINTime) && string.IsNullOrEmpty(CheckOUTTime))
                {
                    objatt.attInTime = dtCheckIN;
                    objatt.attOutTime = dtCheckIN;
                }

                Attendance.AddUpdateEmpAtt(objatt);
                lblMonthYear.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
                DataCalendar.DataBind();
            }
        }
    }
    protected void lbtnpre_Click(object sender, EventArgs e)
    {
        currentDate = ((DateTime)Session["CurrentAttandenceDate"]).AddMonths(-1);
        intMonth = currentDate.Month;
        Session["CurrentAttandenceDate"] = currentDate;
        lblMonthYear.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
        DataCalendar.DataBind();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "fn", "HideLoading();", true);
    }
    protected void lbtnnext_Click(object sender, EventArgs e)
    {
        currentDate = ((DateTime)Session["CurrentAttandenceDate"]).AddMonths(1);
        intMonth = currentDate.Month;
        Session["CurrentAttandenceDate"] = currentDate;
        lblMonthYear.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
        DataCalendar.DataBind();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "fn", "HideLoading();", true);
    }
    protected void MyCalendar_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["startdate"] = Session["CurrentAttandenceDate"];
        e.InputParameters["intUserID"] = UM.EmployeeID;
        e.InputParameters["intUserLocation"] = UM.LocationID;
    }
    //fo Testing then need to delete following


    //Start by shubh
    [System.Web.Services.WebMethod]
    public static String Get_ReviewList(string Mode)
    {
        try
        {
            string review = "";
            List<EmpQuickReviewBLL> lstforInit = EmpQuickReviewBLL.Get_ReviewListByEmpCode(UM.EmployeeID, Mode);
            if (lstforInit != null && lstforInit.Count > 0)
            {
                for (int i = 0; i < lstforInit.Count; i++)
                {
                    if (lstforInit[i].AcceptedStatus == "Pending")
                    {
                        review = review + " " + lstforInit[i].ReviewText;
                    }
                }
            }
            var data = review;
            return (data);
        }
        catch (Exception ex)
        {
            string err = ex.Message;
            return null;
        }
    }
    protected void Btn_AcceptReview_Click(object sender, EventArgs e)
    {
        EmpQuickReviewBLL obj = new EmpQuickReviewBLL();
        obj.Update_AcceptedReviews("Update_AcceptedReviews", UM.EmployeeID);
        obj.Send_ReviewAcceptanceMailToHR(UM.EmployeeID);
    }
    //End By Shubh

    protected void btnGoToEmployeeRemoteAtt_Click(object sender, EventArgs e)
    {
        // Redirect to EmployeeRemoteAtt.aspx page
        Response.Redirect("EmployeeRemoteAtt.aspx");
    }
}