using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Collections;
using AgoraBL.BAL;
using System.Threading.Tasks;

public partial class Member_EmpLeave : Authentication
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public DataSet ds = null;
    EmpLeaveBLL objEmpLeave = null;
    public string ddate, dMonth, ddate1;
    DateTime date1;
    int intmonth, strDate;
    UserMaster UM;
    public int CLBal, SLBal, PLBal, COBal = 0;
    public string _leaveType, _leaveFrom, _leaveTo, _leaveReason, _leaveCnt = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            hfMSTeamURL.Value = System.Configuration.ConfigurationManager.AppSettings["MSTeamURL"];
            // Assign the Session["update"] with unique value
            Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());

            int EmpID = UM.EmployeeID;
            objEmpLeave = new EmpLeaveBLL();
            ds = objEmpLeave.GetLeave(EmpID);
            if (ds.Tables[2].Rows.Count > 0)
            {
                //ddlleaveType.DataSource = ds.Tables[2];
                //ddlleaveType.DataTextField = "statusDesc";
                //ddlleaveType.DataValueField = "statusid";
                //ddlleaveType.DataBind();
                //ddlleaveType.Items.Add(new ListItem("Select", "0", true));


                ddlleaveType.SelectedIndex = 0;

                ddlAddLeaveType.DataSource = ds.Tables[2];
                ddlAddLeaveType.DataTextField = "statusDesc";
                ddlAddLeaveType.DataValueField = "statusid";
                ddlAddLeaveType.DataBind();

            }
            BindLeave();
            BindLeaveDetails();

            int intOffMonthset = 0;//-1
            if (DateTime.Now.Day < 10)
                intOffMonthset = -1;//-2

            lbtnnext.Visible = true;
            hdnCurrent.Value = DateAndTime.DateAdd("m", intOffMonthset, DateAndTime.Now).ToString();
            ddate = hdnCurrent.Value.ToString();

            if (DateTime.Now.Month < 4)
            {
                hdnYear.Value = Convert.ToString(Convert.ToInt32((Convert.ToDateTime(hdnCurrent.Value).Year) - 1));
            }
            else
            {
                hdnYear.Value = Convert.ToString((Convert.ToDateTime(hdnCurrent.Value).Year));
            }

            BindLeaveDetails();
        }

    }

    private void BindLeaveDetails()
    {
        int EmpID = UM.EmployeeID;
        hdnEmpId.Value = Convert.ToString(EmpID);
        objEmpLeave = new EmpLeaveBLL();
        string leaveType = ddlleaveType.SelectedValue.ToString();
        string lStatus = ddlStatus.SelectedValue.ToString();
        string Leavemonth = hdnCurrent.Value.ToString();
        ds = new DataSet();
        ds = objEmpLeave.GetLeaveDetails(EmpID, leaveType, lStatus, Convert.ToInt32(hdnYear.Value));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gridLeaveDetails.DataSource = ds.Tables[0];
            gridLeaveDetails.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            gridLeaveDetails.DataSource = dt;
            gridLeaveDetails.DataBind();
        }
    }
    
    private void BindLeave()
    {
        int EmpID = UM.EmployeeID;
        string currStartDate = string.Empty;
        string currEndDate = string.Empty;

        if (DateTime.Now.Month < 4)
        {
            currStartDate = "1-Apr-" + (DateTime.Now.Year - 1).ToString();
            currEndDate = "31-Mar-" + (DateTime.Now.Year).ToString();
            lblCurYear.Text = "Leave Details (" + currStartDate + "  To  " + currEndDate + ")";
        }
        else
        {
            currStartDate = "1-Apr-" + (DateTime.Now.Year).ToString();
            currEndDate = "31-Mar-" + (DateTime.Now.Year + 1).ToString();
            lblCurYear.Text = "Leave Details (" + currStartDate + "  To  " + currEndDate + ")";
        }

        objEmpLeave = new EmpLeaveBLL();
        ds = objEmpLeave.GetLeave(EmpID);
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblJDate.Text = ds.Tables[0].Rows[0]["JoinDate"].ToString();
                lblCDate.Text = ds.Tables[0].Rows[0]["CoDate"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                gridLeave.DataSource = ds.Tables[1];
                gridLeave.DataBind();

                DataTable dtLeavesDetails = ds.Tables[1];
                if(dtLeavesDetails  != null && dtLeavesDetails.Rows.Count > 0)
                {
                    
                        //For CL
                         hdnCL_Bal.Value = Convert.ToString(dtLeavesDetails.Rows[0]["Balance"]);
                         CLBal = !string.IsNullOrEmpty(Convert.ToString(dtLeavesDetails.Rows[0]["Balance"])) ? Convert.ToInt32(Convert.ToString(dtLeavesDetails.Rows[0]["Balance"])) : 0;

                        //For SL
                        hdnSL_Bal.Value = Convert.ToString(dtLeavesDetails.Rows[1]["Balance"]);
                        SLBal = !string.IsNullOrEmpty(Convert.ToString(dtLeavesDetails.Rows[1]["Balance"])) ? Convert.ToInt32(Convert.ToString(dtLeavesDetails.Rows[1]["Balance"])) : 0;

                        //For PL
                        hdnPL_Bal.Value = Convert.ToString(dtLeavesDetails.Rows[2]["Balance"]);
                        PLBal = !string.IsNullOrEmpty(Convert.ToString(dtLeavesDetails.Rows[2]["Balance"])) ? Convert.ToInt32(Convert.ToString(dtLeavesDetails.Rows[2]["Balance"])) : 0;

                        //For CO
                        hdnCO_Bal.Value = Convert.ToString(dtLeavesDetails.Rows[3]["Balance"]);
                        COBal = !string.IsNullOrEmpty(Convert.ToString(dtLeavesDetails.Rows[3]["Balance"])) ? Convert.ToInt32(Convert.ToString(dtLeavesDetails.Rows[3]["Balance"])) : 0;
                    
                }
            }
            if (ds.Tables[4].Rows.Count > 0 && Convert.ToInt32(ds.Tables[4].Rows[0]["empProbationPeriod"]) != 0)
            {
                ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("CL"));
                ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("SL"));
                ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("PL"));
            }
            else
            {
                if (CLBal <= 0)
                    ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("CL"));
                if (SLBal <= 0)
                    ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("SL"));
                if (PLBal <= 0)
                    ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("PL"));
                if (COBal <= 0)
                    ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("CO"));
            }
            //if (ds.Tables[2].Rows.Count > 0)
            //{
            //    //ddlleaveType.DataSource = ds.Tables[2];
            //    //ddlleaveType.DataTextField = "statusDesc";
            //    //ddlleaveType.DataValueField = "statusid";
            //    //ddlleaveType.DataBind();
            //    //ddlleaveType.Items.Add(new ListItem("Select", "0", true));


            //    ddlleaveType.SelectedIndex = 0;

            //    ddlAddLeaveType.DataSource = ds.Tables[2];
            //    ddlAddLeaveType.DataTextField = "statusDesc";
            //    ddlAddLeaveType.DataValueField = "statusid";
            //    ddlAddLeaveType.DataBind();

            //}
            //Commented by vishal waghmare for Bug 969
            //if (ds.Tables[3].Rows.Count > 0)
            //{

            //    string[] lvbalance = ds.Tables[3].Rows[0]["LeaveBalance"].ToString().Split(',');

            //    for (int i = 0; i < lvbalance.Count(); i++)
            //    {
            //        string[] lvtype = lvbalance[i].Split('-');
            //        if (lvtype.Count() > 0)
            //        {
            //            if (lvtype[0] == "CL")
            //            {
            //                CLBal = Convert.ToInt32(lvbalance[i].Substring(lvbalance[i].IndexOf('-') + 1));
            //                hdnCL_Bal.Value = lvbalance[i].Substring(lvbalance[i].IndexOf('-') + 1);
            //            }
            //            else if (lvtype[0] == "SL")
            //            {
            //                SLBal = Convert.ToInt32(lvbalance[i].Substring(lvbalance[i].IndexOf('-') + 1));
            //                hdnSL_Bal.Value = lvbalance[i].Substring(lvbalance[i].IndexOf('-') + 1);
            //            }
            //            else if (lvtype[0] == "PL")
            //            {
            //                PLBal = Convert.ToInt32(lvbalance[i].Substring(lvbalance[i].IndexOf('-') + 1));
            //                hdnPL_Bal.Value = lvbalance[i].Substring(lvbalance[i].IndexOf('-') + 1);
            //            }
            //            else if (lvtype[0] == "CO")
            //            {
            //                COBal = Convert.ToInt32(lvbalance[i].Substring(lvbalance[i].IndexOf('-') + 1));
            //                hdnCO_Bal.Value = lvbalance[i].Substring(lvbalance[i].IndexOf('-') + 1);
            //            }
            //        }
            //    }
            //}
            //if (ds.Tables[4].Rows.Count > 0 && Convert.ToInt32(ds.Tables[4].Rows[0]["empProbationPeriod"]) != 0)
            //{
            //    ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("CL"));
            //    ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("SL"));
            //    ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("PL"));
            //}
            //else
            //{
            //    if (CLBal <= 0)
            //        ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("CL"));
            //    if (SLBal <= 0)
            //        ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("SL"));
            //    if (PLBal <= 0)
            //        ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("PL"));
            //    if (COBal <= 0)
            //        ddlAddLeaveType.Items.Remove(ddlAddLeaveType.Items.FindByValue("CO"));
            //}

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        objEmpLeave = new EmpLeaveBLL();
        int EmpID = UM.EmployeeID;
        string leaveType = ddlleaveType.SelectedValue.ToString();
        string lStatus = ddlStatus.SelectedValue.ToString();
        string Leavemonth = hdnCurrent.Value.ToString();
        ds = new DataSet();
        //ds = objEmpLeave.GetLeaveDetails(EmpID, leaveType, lStatus, Leavemonth);
        ds = objEmpLeave.GetLeaveDetails(EmpID, leaveType, lStatus, Convert.ToInt32(hdnYear.Value));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gridLeaveDetails.DataSource = ds.Tables[0];
            gridLeaveDetails.DataBind();
        }
        else
        {
            gridLeaveDetails.DataSource = null;
            gridLeaveDetails.DataBind();
        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        ViewState["update"] = Session["update"];
    }
    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        // If page not Refreshed
        if (Session["update"].ToString() == ViewState["update"].ToString())
        {            
            DataSet ds = new DataSet();
            objEmpLeave = new EmpLeaveBLL();
            int EmpID = UM.EmployeeID;
            string leaveType = _leaveType = ddlAddLeaveType.SelectedValue.ToString();
            string leaveFrom = _leaveFrom = hdnFrom.Value.ToString();
            string leaveTo = _leaveTo = hdnTo.Value.ToString();
            string Reason = _leaveReason = hdnReason.Value.ToString();
            _leaveCnt = lblLeaveCount.Text;
            int CLBal = Convert.ToInt32(hdnCL_Bal.Value);
            int SLBal = Convert.ToInt32(hdnSL_Bal.Value);
            int PLBal = Convert.ToInt32(hdnPL_Bal.Value);
            int COBal = Convert.ToInt32(hdnCO_Bal.Value);
            int LeaveDays = Convert.ToInt32(CalulateLeave(leaveFrom, leaveTo));
            int LeaveStatus = 0;
            if (string.Compare(leaveType, "CL", true) == 0)
            {
                if (CLBal < LeaveDays)
                {
                    LeaveStatus = -1;
                }
                else
                {
                    bool IsEmailSent = false;
                    ds = objEmpLeave.SaveLeave(EmpID, leaveType, leaveFrom, leaveTo, Reason,out IsEmailSent);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        LeaveStatus = 1;
                        if (!IsEmailSent)
                        {
                            SendMail(ds.Tables[0], leaveFrom, leaveTo, Reason);
                        }
                    }
                    else
                    {
                        LeaveStatus = 0;
                    }
                    BindLeaveDetails();
                }
            }
            else if (string.Compare(leaveType, "SL", true) == 0)
            {
                if (SLBal < LeaveDays)
                {
                    LeaveStatus = -1;
                }
                else
                {
                    bool IsEmailSent = false; 
                    ds = objEmpLeave.SaveLeave(EmpID, leaveType, leaveFrom, leaveTo, Reason,out IsEmailSent);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        LeaveStatus = 1;
                        if (!IsEmailSent)
                        {
                            SendMail(ds.Tables[0], leaveFrom, leaveTo, Reason);
                        }
                    }
                    else
                    {
                        LeaveStatus = 0;
                    }
                    BindLeaveDetails();
                }
            }

            else if (string.Compare(leaveType, "PL", true) == 0)
            {
                if (PLBal < LeaveDays)
                {
                    LeaveStatus = -1;
                }
                else
                {
                    bool IsEmailSent = false; 
                    ds = objEmpLeave.SaveLeave(EmpID, leaveType, leaveFrom, leaveTo, Reason,out IsEmailSent);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        LeaveStatus = 1;
                        if (!IsEmailSent)
                        {
                            SendMail(ds.Tables[0], leaveFrom, leaveTo, Reason);
                        }
                    }
                    else
                    {
                        LeaveStatus = 0;
                    }
                    BindLeaveDetails();
                }
            }
            else if (string.Compare(leaveType, "CO", true) == 0)
            {
                if (COBal < LeaveDays)
                {
                    LeaveStatus = -1;
                }
                else
                {
                    bool IsEmailSent = false; 
                    ds = objEmpLeave.SaveLeave(EmpID, leaveType, leaveFrom, leaveTo, Reason, out IsEmailSent);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        LeaveStatus = 1;
                        if (!IsEmailSent)
                        {
                            SendMail(ds.Tables[0], leaveFrom, leaveTo, Reason);
                        }
                    }
                    else
                    {
                        LeaveStatus = 0;
                    }
                    BindLeaveDetails();
                }
            }
            else if (string.Compare(leaveType, "WL", true) == 0)
            {
                bool IsEmailSent = false; 
                ds = objEmpLeave.SaveLeave(EmpID, leaveType, leaveFrom, leaveTo, Reason, out IsEmailSent);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    LeaveStatus = 1;
                    if (!IsEmailSent)
                    {
                        SendMail(ds.Tables[0], leaveFrom, leaveTo, Reason);
                    }
                }
                else
                {
                    LeaveStatus = 0;
                }
                BindLeaveDetails();
            }
            BindLeave();//Added by trupti
            if (LeaveStatus == -1)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertInsufficientMessage", "alert('Insufficient leave balance')", true);
            else if (LeaveStatus == 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertFailedMessage", "alert('Leave not saved')", true);
            else if (LeaveStatus == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertSuccessMessage", "alert('Leave saved succesfully')", true);
                //EmployeeMasterBAL objEmployeeMasterBAL = new EmployeeMasterBAL();
                //objEmployeeMasterBAL.SendLeaveNotificationToHrThroughAI(EmpID, leaveFrom, leaveType, Reason, leaveTo, ds,UM.Name);
            }

            // After the event/ method, again update the session 
            Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());
        }
        else // If Page Refreshed
        {
            BindLeaveDetails();
            BindLeave();            
        }

    }    
    private void SendMail(DataTable dt, string strDateFrom, string strDateTo, string strReason)
    {
        string[] DateFrom = strDateFrom.Split('/');
        string New_FromDt = DateFrom[1] + '/' + DateFrom[0] + '/' + DateFrom[2];
        DateTime dtFrom = Convert.ToDateTime(New_FromDt.ToString());

        string[] DateTo = strDateTo.Split('/');
        string New_ToDt = DateTo[1] + '/' + DateTo[0] + '/' + DateTo[2];
        DateTime dtTo = Convert.ToDateTime(New_ToDt.ToString());

        string strBody, strSubject, mailTo, mailFrom, message, CC = "";

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            // this if conditions applay to avoid duplicate emailId under CC in mail
            if (CC.Contains(dt.Rows[i]["ProjMangerEmail"].ToString()))
            {

            }
            else
            {
                CC = CC + dt.Rows[i]["ProjMangerEmail"].ToString();
                CC += (i < dt.Rows.Count) ? ";" : string.Empty;
            }
            if (CC.Contains(dt.Rows[i]["BAEmail"].ToString()))
            {
            }
            else
            {
                CC = CC + dt.Rows[i]["BAEmail"].ToString();
                CC += (i < dt.Rows.Count) ? ";" : string.Empty;
            }

        }
        if (CC.Contains(dt.Rows[0]["empEmail"].ToString()))
        {
        }
        else
        {
            CC += dt.Rows[0]["empEmail"].ToString();
        }

        strBody = dt.Rows[0]["empName"] + " " + "has applied for leave " + "<b>From</b>" + ":" + Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy") + " " + "<b>TO</b>" + ":" + Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy") + " " + "<br> Reason for Leave Is " + "<b>:</b>" + strReason;
        strSubject = dt.Rows[0]["empName"].ToString() + " has applied for leave.";        
        mailTo = "hr@intelgain.com";
        mailFrom = dt.Rows[0]["empEmail"].ToString();
        message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
    }

    //private void SendMail(DataTable dt, string strDateFrom, string strDateTo, string strReason)
    //{
    //    string[] DateFrom = strDateFrom.Split('/');
    //    string New_FromDt = DateFrom[1] + '/' + DateFrom[0] + '/' + DateFrom[2];
    //    DateTime dtFrom = Convert.ToDateTime(New_FromDt.ToString());

    //    string[] DateTo = strDateTo.Split('/');
    //    string New_ToDt = DateTo[1] + '/' + DateTo[0] + '/' + DateTo[2];
    //    DateTime dtTo = Convert.ToDateTime(New_ToDt.ToString());

    //    string strBody, strSubject, mailTo, mailFrom, message, CC = "";
    //    strBody = dt.Rows[0]["empName"] + " " + "has applied for leave " + "<b>From</b>" + ":" + Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy") + " " + "<b>TO</b>" + ":" + Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy") + " " + "<br> Reason for Leave Is" + "<b>:</b>" + strReason;
    //    strSubject = dt.Rows[0]["empName"].ToString() + " has applied for leave.";
    //    mailTo = "hr@intelgain.com";
    //    mailFrom = CC = dt.Rows[0]["empEmail"].ToString();
    //    message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
    //}

    protected void DeleteLeave(object sender, CommandEventArgs e)
    {
        int empLeaveID = Convert.ToInt32(e.CommandArgument.ToString());
        objEmpLeave = new EmpLeaveBLL();
        int outId = objEmpLeave.DeleteLeave(empLeaveID);
        if (outId > 0)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Leave deleted succesfully')", true);
        
        BindLeaveDetails();
        BindLeave();// added  for user story 215 20-dec-2021
        // BindLeave(); //Added by trupti
        Response.Redirect(Request.RawUrl);
    }
    protected void lbtnpre_Click(object sender, EventArgs e)
    {
        hdnCurrent.Value = DateAndTime.DateAdd(DateInterval.Year, -1, DateTime.Parse(hdnCurrent.Value.ToString())).ToString();
        hdnYear.Value = Convert.ToString(Convert.ToInt32(hdnYear.Value) - 1);
        lbtnnext.Visible = true;
        BindLeaveDetails();
    }

    private void calculateDate()
    {
        ddate = hdnCurrent.Value.ToString();
        dMonth = DateAndTime.Month(DateTime.Parse(ddate)).ToString();
        intmonth = Convert.ToInt32(dMonth);
        dynamic intYear = Convert.ToInt32(DateAndTime.Year(DateTime.Parse(ddate)).ToString());
        ddate1 = ddate;
        ddate = DateAndTime.Year(DateTime.Parse(ddate1).AddYears(-1)) + "-" + DateAndTime.Year(DateTime.Parse(ddate1));

    }
    protected void lbtnnext_Click(object sender, EventArgs e)
    {
        hdnCurrent.Value = DateAndTime.DateAdd("m", 1, DateTime.Parse(hdnCurrent.Value.ToString())).ToString();
        hdnYear.Value = Convert.ToString(Convert.ToInt32(hdnYear.Value) + 1);
        BindLeaveDetails();

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string CalulateLeave(string FromDate, string ToDate)
    {
        string days = "";

        if (ToDate != string.Empty && FromDate != string.Empty)
        {
            DateTime dtf = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dtt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            TimeSpan ts = Convert.ToDateTime(dtf) - Convert.ToDateTime(dtt);
            int da = System.Math.Abs(Convert.ToInt16(ts.TotalDays)) + 1;
            days = da.ToString();
        }
        return days;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string IfExistsLeave(int EmpID, string FromDate, string ToDate)
    {
        try
        {
            bool output = EmpLeaveBLL.IfExistsLeave("IFEXISTSLEAVE", EmpID, FromDate, ToDate);
            string isrecordexists = "false";
            if (output == true)
            {
                isrecordexists = "true";
            }
            return isrecordexists;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    protected void gridLeave_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl = (Label)e.Row.FindControl("lbleTotalcnt");
            if (e.Row.RowIndex == 2)
            {
                lbl.ToolTip = "Carry Forwarded PL : " + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CarryFwdPL")) + "  Current Year PL Till Date: " + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "AccCurrYrPL"));
            }
            else
            {
                lbl.ToolTip = "";
            }
        }

    }

}