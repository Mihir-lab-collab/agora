using AgoraBL.BAL;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_EmpWFH : Authentication
{
    EmpWFHBAL objEmpWFH;
    UserMaster UM;
    public string _WFHType, _WFHFrom, _WFHTo, _WFHReason, _WFHCnt = "";
    public string ddate, dMonth, ddate1;
    public DataSet ds = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            BindWFHBalance();
            Session["ForCheckCondition"] = Server.UrlEncode(System.DateTime.Now.ToString());
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

            BindEMPWFHDetails();
        }
    }
    private void BindWFHBalance()
    {
        DataTable dt = new DataTable();
        int EmpID = UM.EmployeeID;
        objEmpWFH = new EmpWFHBAL();
        dt = objEmpWFH.BindWFHBalance(EmpID);
        if (dt.Rows.Count > 0)
        {
            if (dt.Columns.Contains("Balance"))
            {
                object cellValue = dt.Rows[0]["Balance"];
                if (cellValue != DBNull.Value)
                {
                    string result = Convert.ToString(cellValue);
                    hdnBalance.Value = result;
                }
            }
        }
        gridWFH.DataSource = dt;
        gridWFH.DataBind();
    }

    private void BindEMPWFHDetails()
    {
        int EmpID = UM.EmployeeID;
        hdnEmpId.Value = Convert.ToString(EmpID);
        objEmpWFH = new EmpWFHBAL();
        string lStatus = ddlStatus.SelectedValue.ToString();
        ds = new DataSet();
        ds = objEmpWFH.GetWFHDetails(EmpID, lStatus, Convert.ToInt32(hdnYear.Value));
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string status = row["WFHStatus"].ToString(); // Assuming 'p' represents the column name

                // Replace values based on 'a' and 'r'
                if (status == "a")
                {
                    row["WFHStatus"] = "Approved";
                }
                else if (status == "r")
                {
                    row["WFHStatus"] = "Rejected";
                }
                else
                {
                    row["WFHStatus"] = "Pending";
                }
            }
            gridWFHDetails.DataSource = ds.Tables[0];
            gridWFHDetails.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            gridWFHDetails.DataSource = dt;
            gridWFHDetails.DataBind();
        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        ViewState["ForCheckCondition"] = Session["ForCheckCondition"];
    }
    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        if (Session["ForCheckCondition"].ToString() == ViewState["ForCheckCondition"].ToString())
        {
            if (!string.IsNullOrEmpty(hdnBalance.Value))
            {
                if (Convert.ToInt32(hdnBalance.Value)< Convert.ToInt32(hdnWFHCount.Value))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertInsufficientMessage", "alert('Insufficient WFH balance')", true);
                    return; 
                }
            }
            DataSet ds = new DataSet();
            objEmpWFH = new EmpWFHBAL();
            int EmpID = UM.EmployeeID;
            string WFHFrom = _WFHFrom = hdnFrom.Value.ToString();
            string WFHTo = _WFHTo = hdnTo.Value.ToString();
            string Reason = _WFHReason = hdnReason.Value.ToString();
            int WFHCount = string.IsNullOrEmpty(hdnWFHCount.Value.ToString()) ? 0 : Convert.ToInt32(hdnWFHCount.Value.ToString());
            ds = objEmpWFH.SaveWFH(EmpID, WFHFrom, WFHTo, Reason, WFHCount);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                SendMail(ds.Tables[0], WFHFrom, WFHTo, Reason);
            }
            Session["ForCheckCondition"] = Server.UrlEncode(System.DateTime.Now.ToString());
            BindEMPWFHDetails();
            BindWFHBalance();
        }
        else
        {
            BindEMPWFHDetails();
            BindWFHBalance();
        }
    }
    private void SendMail(DataTable dt, string strDateFrom, string strDateTo, string strReason)
    {

        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(strDateFrom) && !string.IsNullOrEmpty(strDateTo))
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
            string finalCC = string.Empty;
            if (CC.Length > 0)
            {
                var distinctCC = CC.Split(',').Select(email => email.Trim()).Distinct();
                finalCC = string.Join(", ", distinctCC);
            }

            strBody = dt.Rows[0]["empName"] + " " + "has applied for Work From Home " + "<b>From</b> " + ": " +
                Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy") + " " + "<b>TO</b> " + ": " +
                Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy") + " " + "<br> Reason for Work from Home is " + "<b>: </b>" + strReason;
            //strSubject = dt.Rows[0]["empName"].ToString() + " has applied for Work From Home.";
            strSubject = "Request to Work From Home";
            mailTo = ConfigurationManager.AppSettings["HREmail"];
            mailFrom = dt.Rows[0]["empEmail"].ToString();
            message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, finalCC, "");
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string CalculateWFH(string FromDate, string ToDate)
    {
        try
        {
            string days = "";
            int saturdays = 0;
            int sundays = 0;
            int final = 0;
            int holiday = 0;
            if (ToDate != string.Empty && FromDate != string.Empty)
            {
                DateTime dtf = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dtt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                List<Holiday> listHolidays = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);

                var fromDate = dtf;
                while (fromDate <= dtt)
                {
                    if (fromDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        saturdays++;
                    }
                    else if (fromDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        sundays++;
                    }

                    fromDate = fromDate.AddDays(1); // Move to the next day
                }
                if (listHolidays.Count > 0)
                {
                    foreach (var item in listHolidays)
                    {
                        var fromDateHoliday = dtf;
                        var date = Convert.ToDateTime(item.HolidayDate).ToString("dd/MM/yyyy").Replace('-', '/');
                        DateTime holidayDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        while (fromDateHoliday <= dtt)
                        {
                            if (holidayDate == fromDateHoliday)
                            {
                                if (holidayDate.DayOfWeek != DayOfWeek.Saturday)
                                {
                                    holiday++;
                                }
                                else if (holidayDate.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    holiday++;
                                }

                            }
                            fromDateHoliday = fromDateHoliday.AddDays(1);
                        }
                    }
                }
                final = saturdays + sundays+ holiday;
                TimeSpan ts = Convert.ToDateTime(dtf) - Convert.ToDateTime(dtt);
                int da = System.Math.Abs(Convert.ToInt16(ts.TotalDays)) + 1 - final;
                days = da.ToString();
            }
            return days;
        }
        catch (Exception e)
        {

            throw;
        }
    }
    protected void DeleteWFH(object sender, CommandEventArgs e)
    {
        int WFHId = Convert.ToInt32(e.CommandArgument.ToString());
        objEmpWFH = new EmpWFHBAL();
        int outId = objEmpWFH.DeleteWFHById(WFHId);
        if (outId > 0)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Work from home deleted succesfully')", true);

        BindEMPWFHDetails();
        Response.Redirect(Request.RawUrl);
    }
    protected void lbtnnext_Click(object sender, EventArgs e)
    {
        hdnCurrent.Value = DateAndTime.DateAdd("m", 1, DateTime.Parse(hdnCurrent.Value.ToString())).ToString();
        hdnYear.Value = Convert.ToString(Convert.ToInt32(hdnYear.Value) + 1);
        BindEMPWFHDetails();

    }
    protected void lbtnpre_Click(object sender, EventArgs e)
    {
        hdnCurrent.Value = DateAndTime.DateAdd(DateInterval.Year, -1, DateTime.Parse(hdnCurrent.Value.ToString())).ToString();
        hdnYear.Value = Convert.ToString(Convert.ToInt32(hdnYear.Value) - 1);
        lbtnnext.Visible = true;
        BindEMPWFHDetails();
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string IfExistsWFH(int EmpID, string FromDate, string ToDate)
    {
        try
        {
            bool output = EmpWFHBAL.IfExistsWFH("IFEXISTSWFH", EmpID, FromDate, ToDate);
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        objEmpWFH = new EmpWFHBAL();
        int EmpID = UM.EmployeeID;
        string lStatus = ddlStatus.SelectedValue.ToString();
        ds = new DataSet();
        ds = objEmpWFH.GetWFHDetails(EmpID, lStatus, Convert.ToInt32(hdnYear.Value));
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string status = row["WFHStatus"].ToString(); // Assuming 'p' represents the column name

                // Replace values based on 'a' and 'r'
                if (status == "a")
                {
                    row["WFHStatus"] = "Approved";
                }
                else if (status == "r")
                {
                    row["WFHStatus"] = "Rejected";
                }
                else
                {
                    row["WFHStatus"] = "Pending";
                }
            }
            gridWFHDetails.DataSource = ds.Tables[0];
            gridWFHDetails.DataBind();
        }
        else
        {
            gridWFHDetails.DataSource = null;
            gridWFHDetails.DataBind();
            gridWFHDetails.ShowHeaderWhenEmpty = true;
        }
    }
    //protected void gridWFHDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (string.Compare("EditWFH", e.CommandName) == 0)
    //    {
    //        // string empID = e.CommandArgument.ToString();
    //        divAddPopupForFillAttendance.Style["display"] = "";
    //        divAddPopupOverlayForFillAttendance.Attributes.Add("class", "k-overlay");
    //        gridwfhattendancedetails.DataSource = null;
    //        gridwfhattendancedetails.DataBind();
    //        gridwfhattendancedetails.ShowHeaderWhenEmpty = true;
    //    }

    //}

    protected void gridWFHDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string status = Convert.ToString(((GridView)sender).DataKeys[e.Row.RowIndex].Values[1]);
            if (string.Compare(status, "Approved", true) == 0)
            {
                e.Row.FindControl("lnkDeleteTask").Visible = false;
                e.Row.FindControl("lnkEditTask").Visible = true;
            }
            else if(string.Compare(status, "Rejected", true) == 0)
            {
                e.Row.FindControl("lnkDeleteTask").Visible = false;
                e.Row.FindControl("lnkEditTask").Visible = false;
            }
        }
    }

    protected void lnkEditTask_Command(object sender, CommandEventArgs e)
    {
        if (string.Compare("EditWFH", e.CommandName) == 0)
        {
            LinkButton lnkbtn = (LinkButton)sender;
            string commandArgument = lnkbtn.CommandArgument;

            // Split the commandArgument string to get individual values
            string[] empDetails = commandArgument.Split(',');

            // Access the individual values
            var currentDate = DateTime.Today; ;
            string empWFHId = empDetails[0];
            string WFHStatus = empDetails[1];
            string WFHFrom = Convert.ToDateTime(empDetails[2]).ToString("yyyy/MM/dd");
            string WFHTo = Convert.ToDateTime(empDetails[3]).ToString("yyyy/MM/dd");
            int EmpID = UM.EmployeeID;
            Session["WFHFrom"] = WFHFrom;
            Session["WFHTo"] = WFHTo;
            objEmpWFH = new EmpWFHBAL();
            ds = new DataSet();
            ds = objEmpWFH.AppliedWFHFromTo(EmpID, WFHFrom, WFHTo);
            divAddPopupForFillAttendance.Style["display"] = "";
            divAddPopupOverlayForFillAttendance.Attributes.Add("class", "k-overlay");
            if (currentDate >= Convert.ToDateTime(WFHFrom) && currentDate <= Convert.ToDateTime(WFHTo))
            {
                WFHAttendance.InnerText = "Fill Work From Home Attendance";
                divWFH.Visible = true;
                lblShowDT.Text = currentDate.ToString("dd-MMM-yyyy");
                btnStartDay.Enabled = true;
                btnEndDay.Enabled = true;
            }
            else
            {
                WFHAttendance.InnerText = "Work From Home Attendance List ";
                divWFH.Visible = false;
                lblShowDT.Text = "";
                btnStartDay.Enabled = false;
                btnEndDay.Enabled = false;
            }
        }

        if (ds.Tables[0].Rows.Count > 0)
        {

            gridwfhattendancedetails.ShowHeaderWhenEmpty = true;
            gridwfhattendancedetails.DataSource = ds.Tables[0];
            gridwfhattendancedetails.DataBind();

        }
        else
        {
            DataTable dt = new DataTable();
            gridwfhattendancedetails.DataSource = dt;
            gridwfhattendancedetails.DataBind();
            gridwfhattendancedetails.ShowHeaderWhenEmpty = true;
        }
    }



    protected void btnStartDay_Click(object sender, EventArgs e)
    {
        objEmpWFH = new EmpWFHBAL();
        int EmpID = UM.EmployeeID;
        string WFHFrom = Convert.ToDateTime(Session["WFHFrom"]).ToString("yyyy/MM/dd");
        string WFHTo = Convert.ToDateTime(Session["WFHTo"]).ToString("yyyy/MM/dd");
        if (!string.IsNullOrEmpty(EmpID.ToString()))
            objEmpWFH.InsertWFHAttendance(EmpID);
        ds = new DataSet();
        ds = objEmpWFH.AppliedWFHFromTo(EmpID, WFHFrom, WFHTo);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gridwfhattendancedetails.ShowHeaderWhenEmpty = true;
            gridwfhattendancedetails.DataSource = ds.Tables[0];
            gridwfhattendancedetails.DataBind();

        }
        else
        {
            DataTable dt = new DataTable();
            gridwfhattendancedetails.DataSource = dt;
            gridwfhattendancedetails.DataBind();
            gridwfhattendancedetails.ShowHeaderWhenEmpty = true;
        }

    }

    protected void btnEndDay_Click(object sender, EventArgs e)
    {
        objEmpWFH = new EmpWFHBAL();
        int EmpID = UM.EmployeeID;
        string WFHFrom = Convert.ToDateTime(Session["WFHFrom"]).ToString("yyyy/MM/dd");
        string WFHTo = Convert.ToDateTime(Session["WFHTo"]).ToString("yyyy/MM/dd");
        DateTime attDate = DateTime.Now.Date;
        DateTime attOutTime = DateTime.Now;
        if (!string.IsNullOrEmpty(EmpID.ToString()))
            objEmpWFH.UpdateWFHAttendance(EmpID, attOutTime, attDate);
        ds = new DataSet();
        ds = objEmpWFH.AppliedWFHFromTo(EmpID, WFHFrom, WFHTo);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gridwfhattendancedetails.ShowHeaderWhenEmpty = true;
            gridwfhattendancedetails.DataSource = ds.Tables[0];
            gridwfhattendancedetails.DataBind();

        }
        else
        {
            DataTable dt = new DataTable();
            gridwfhattendancedetails.DataSource = dt;
            gridwfhattendancedetails.DataBind();
            gridwfhattendancedetails.ShowHeaderWhenEmpty = true;
        }

    }

    protected void Imgbtn_Click(object sender, ImageClickEventArgs e)
    {
        divAddPopupForFillAttendance.Style["display"] = "none";
        divAddPopupOverlayForFillAttendance.Attributes.Remove("k-overlay");
        divAddPopupOverlayForFillAttendance.Attributes.Add("class", "k-overlayDisplaynone");

    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string CheckInTime(int EmpId)
    {
        try
        {
            bool output = EmpWFHBAL.CheckInTime("CheckInTime", EmpId);
            string isrecordexists = "false";
            if (output == true)
            {
                isrecordexists = "true";
            }
            return isrecordexists;
        }
        catch (Exception)
        {
            throw;
        }
    }
}