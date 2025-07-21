using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using CSCode;
using System.Data.SqlClient;
using System.Configuration;


public partial class Member_Attendance : Authentication
{
   
   
    
    string arg = "";
    public int intMonth = DateTime.Now.Month;
    public int intYear = DateTime.Now.Year;
    public DateTime currentDate = DateTime.Now;
    public DateTime currentDateHR = DateTime.Now;
    public int intMonthHR = DateTime.Now.Month;
    public int intYearHR = DateTime.Now.Year;
    UserMaster UM;

    protected void Page_Load(object sender, EventArgs e)
    {

       


        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true);
        
        if (!IsPostBack)
        {
            currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            currentDateHR = currentDate;
            Session.Add("CurrentAttDate", currentDate);
            Session.Add("CurrentAttDateHR", currentDate);
            intMonth = currentDate.Month;
            intYear = currentDate.Year;
            hdnMonthNo.Value = intMonth.ToString();
            hdnYear.Value = intYear.ToString();
            hdnMonthNoHR.Value = currentDateHR.Month.ToString();
            hdnYearHR.Value = currentDateHR.Year.ToString();
            lblMonth.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
            BindStatus();

            
            Session.Add("CurrentAttandenceDate", currentDate);
            intMonth = currentDate.Month;
            lblMonthYear.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
            BindGrid();
        }
        lblLocation.Text = Session["LocationName"].ToString ();

        
        
    }

    [System.Web.Services.WebMethod]
    public static String BindAttLog(int EmpId, string Date)
    {
        try
        {
            List<EmpAttLog> lstEmpAttLog = new List<EmpAttLog>();
            lstEmpAttLog = AttendanceDAL.getEmpAttLog(EmpId, Convert.ToDateTime(Date), "Log");
            var data = from pr in lstEmpAttLog
                       select new
                       {
                           pr.EmpID,
                           pr.PunchTime,
                           pr.IP,
                           pr.Mode,
                           pr.InsertedOn
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception)
        {
            return null;
        }
    }

     private void BindStatus()
    {
        DataTable dtEmployeeLocation = new DataTable();
        dtEmployeeLocation = Attendance.getEmpStatus();
        ddlStatus.DataSource = dtEmployeeLocation;
        ddlStatus.DataTextField = "statusDesc";
        ddlStatus.DataValueField = "statusid";
        ddlStatus.DataBind();
    }



    protected void PagerButtonClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        arg = btn.CommandArgument.ToString();
        switch (arg)
        {
            case "next":
                currentDate = ((DateTime)Session["CurrentAttDate"]).AddMonths(1);
                BindAttendanceOnPagerButtonClick(currentDate);
                break;
            case "prev":
                currentDate = ((DateTime)Session["CurrentAttDate"]).AddMonths(-1);
                BindAttendanceOnPagerButtonClick(currentDate);
                break;
        }
    }

    public void BindAttendanceOnPagerButtonClick(DateTime currentDate)
    {
        intMonth = currentDate.Month;
        hdnMonthNo.Value = intMonth.ToString();
        intYear = currentDate.Year;
        hdnYear.Value = intYear.ToString();
        hdnMonthNoHR.Value = hdnMonthNo.Value;
        hdnYearHR.Value = hdnYear.Value;
        Session["CurrentAttDate"] = currentDate;
        Session["CurrentAttDateHR"] = currentDate;
        lblMonth.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
        BindGrid();
    }

    protected void PagerHButtonClick(object sender, EventArgs e)
    {
        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "openLoading();", true);
        LinkButton btn = (LinkButton)sender;
        arg = btn.CommandArgument.ToString();
        switch (arg)
        {
            case "nextH":
                currentDateHR = ((DateTime)Session["CurrentAttDateHR"]).AddMonths(1);
                BindHoursReportOnPagerButtonClick(currentDateHR);
                break;
            case "prevH":
                currentDateHR = ((DateTime)Session["CurrentAttDateHR"]).AddMonths(-1);
                BindHoursReportOnPagerButtonClick(currentDateHR);
                break;
        }
        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "closeLoading();", true);
    }

    public void BindHoursReportOnPagerButtonClick(DateTime Currentdate)
    {
        intMonthHR = Currentdate.Month;
        hdnMonthNoHR.Value = intMonthHR.ToString();
        intYearHR = Currentdate.Year;
        hdnYearHR.Value = intYearHR.ToString();
        Session["CurrentAttDateHR"] = Currentdate;
        BindHoursReport(intMonthHR, intYearHR, Currentdate);
    }

 




    public void BindGrid()
    {
        grdatt.DataSource = null;
        grdatt.DataBind();
        int grdtotaldays = 0;
        int months = Convert.ToInt32(hdnMonthNo.Value);
        int years = Convert.ToInt32(hdnYear.Value);

        int totaldays = DateTime.DaysInMonth(Convert.ToInt32(years), Convert.ToInt32(months));

        DataSet Ds = new DataSet();
        Ds = Attendance.GetRecord(totaldays, months, years,  Convert.ToInt32(Session["LocationID"]), Convert.ToInt32(hdEmpid.Value), DateTime.Now, DateTime.Now, hdnFilter.Value, "Attendance");
        grdtotaldays = totaldays;
        totaldays = totaldays + 1;
        if (totaldays <= 31)
        {
            for (int i = totaldays; i <= 31; i++)
            {
                Ds.Tables[0].Columns.Add("aa" + i.ToString());
            }
        }


        if (Ds.Tables[0].Rows.Count > 0)
        {
            grdatt.DataSource = Ds.Tables[0];
            grdatt.DataBind();
            DivGridv.Visible = false;


            for (int i = 0; i <= grdtotaldays; i++)
            {
                grdatt.Columns[i + 1].Visible = true;
            }

            grdtotaldays = grdtotaldays + 1;
            if (grdtotaldays <= 10)
            {
                for (int i = grdtotaldays; i <= 10; i++)
                {
                    grdatt.Columns[i + 1].Visible = false;
                }
            }

            grdatt.Columns[0].Visible = false;

            findholidayofmonth();
            BindHoursReport(months, years, currentDate);
        }
        else
        {
            DivGridv.Visible = true;
        }
    }

    public void findholidayofmonth()
    {
        int x = 0;
        int y = 0;
        int t = 0;
        ArrayList arrholiday = new ArrayList();
        Color bccolor = Color.FromArgb(197, 213, 174);
        Color bccolorwhite = Color.FromArgb(255, 255, 255);
        int months = Convert.ToInt32(hdnMonthNo.Value);
        int years = Convert.ToInt32(hdnYear.Value);
        int totaldays = DateTime.DaysInMonth(Convert.ToInt32(years), Convert.ToInt32(months));
        for (x = 1; x <= totaldays; x++)
        {
            System.DateTime d = new System.DateTime(Convert.ToInt32(years), Convert.ToInt32(months), x);
            if ((d.ToString("dddd").ToUpper() == "SATURDAY"))
            {
                if ((y != 1))
                {
                    x = x + 1;
                    if (x <= totaldays)
                    {
                        arrholiday.Add(x.ToString());
                    }
                    y = 1;
                }
                else
                {
                    t = x + 1;
                    arrholiday.Add(t.ToString());
                    arrholiday.Add(x.ToString());
                    y = 0;
                }
            }
            else
            {
                if ((d.ToString("dddd").ToUpper() == "SUNDAY"))
                {
                    arrholiday.Add(x.ToString());
                }
            }
        }

        DataSet ds = Attendance.GetRecord(0, months, years, 0, Convert.ToInt32(hdEmpid.Value), DateTime.Now, DateTime.Now, hdnFilter.Value, "Getholiday");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                arrholiday.Add(Convert.ToString(ds.Tables[0].Rows[i][0]));
            }
        }

        for (int i = 0; i < totaldays; i++)
        {
            grdatt.Columns[Convert.ToInt32(i + 1)].ItemStyle.BackColor = bccolorwhite;
        }

        for (int i = 0; i < arrholiday.Count; i++)
        {
            int j = 0;
            j = Convert.ToInt32(arrholiday[i].ToString());
            grdatt.Columns[Convert.ToInt32(j + 1)].ItemStyle.BackColor = bccolor;
           
        }

    }


    
    protected void grdatt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gvr = e.Row;
        int j = 0;
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.TableSection = TableRowSection.TableHeader;
        if ((gvr.RowType == DataControlRowType.DataRow))
        {
            Label l1 = (Label)e.Row.Cells[2].FindControl("Label1");
            LinkButton l2 = (LinkButton)e.Row.Cells[2].FindControl("Label2");

            string EmpName = "";
            string[] words = l2.Text.Split('(');
            EmpName = words[0];

            for (j = 1; j <= 31; j++)
            {
                int months = Convert.ToInt32(hdnMonthNo.Value);
                int years = Convert.ToInt32(hdnYear.Value);
                int totaldays = DateTime.DaysInMonth(Convert.ToInt32(years), Convert.ToInt32(months));
                string strdate = grdatt.Columns[j + 1].HeaderText.ToString() + "/" + months + "/" + Convert.ToString(years);
                strdate = strdate.Replace("Date ", "");
                string strUpdate = "update";
                CheckBox chk3 = (CheckBox)e.Row.Cells[j + 1].FindControl("chk" + Convert.ToString(j + 2));
                HiddenField hdn3 = (HiddenField)e.Row.Cells[j + 1].FindControl("Hdn" + Convert.ToString(j + 2));
                
                Label lbl3 = (Label)e.Row.Cells[j + 1].FindControl("Label" + Convert.ToString(j + 2));
                Label lblH3 = (Label)e.Row.Cells[j + 1].FindControl("LabelH" + Convert.ToString(j + 2));

                if (hdn3.Value.Contains("-"))
                {
                    string[] parts = hdn3.Value.Split('=');
                    //lblH3.Text = Convert.ToDecimal(parts[1]).ToString("00.00");
                    lblH3.Text = Convert.ToString(parts[1]).ToString();
                    lbl3.Text = parts[0].Replace(" ", "");
                }
                else
                {
                    lblH3.Visible = false;
                    lbl3.Text = hdn3.Value;
                }

                if ((hdn3.Value.ToString() != "A"))
                {
                    if ((hdn3.Value.EndsWith("L") == true || hdn3.Value.EndsWith("O") == true))
                    {
                        lbl3.BackColor = Color.FromArgb(136, 233, 121);
                    }
                    chk3.Attributes.Add("onClick", "doOpen(this, '" + l1.Text + "','" + strdate + "','" + EmpName + "','" + strUpdate + "')");
                    chk3.Visible = false;
                }
                else
                {
                    chk3.Attributes.Add("onClick", "doOpen(this,'" + l1.Text + "','" + strdate + "','" + EmpName + "','')");
                    lbl3.Visible = false;
                    lblH3.Visible = false;
                }

                lbl3.Attributes.Add("style", "cursor:pointer;");
                lbl3.Attributes.Add("onClick", "doOpen(this,'" + l1.Text + "','" + strdate + "','" + EmpName + "','" + strUpdate + "')");
                lblH3.Attributes.Add("style", "cursor:pointer; font-weight:bold; font-size:15px;");
                lblH3.Attributes.Add("onClick", "doOpen(this,'" + l1.Text + "','" + strdate + "','" + EmpName + "','" + strUpdate + "')");

              

            }
        }
    }

    public void BindHoursReport(int months, int years, DateTime date)
    {
        DataTable grdTable = new DataTable();
        int totaldays = DateTime.DaysInMonth(Convert.ToInt32(years), Convert.ToInt32(months));
        lblMonthHR.Text = String.Format("{0:MMMM}", date) + " " + Convert.ToDateTime(date).Year.ToString();
        DataSet dsWeekDate = new DataSet();
        dsWeekDate = Attendance.GetRecord(totaldays, months, years, 0, Convert.ToInt32(hdEmpid.Value), DateTime.Now, DateTime.Now, hdnFilter.Value, "GetHoursReport");
        if ((dsWeekDate.Tables.Count > 0))
        {
            DataColumn grdColName = new DataColumn("Name");
            grdTable.Columns.Add(grdColName);
            if ((dsWeekDate.Tables[0].Rows.Count > 0))
            {
                int col = 1;
                foreach (DataRow dr in dsWeekDate.Tables[0].Rows)
                {
                    DataColumn grdCol = new DataColumn(Convert.ToDateTime(dr[0].ToString()).ToString("dd-MMM") + " To " + Convert.ToDateTime(dr[1].ToString()).ToString("dd-MMM"));
                    col = col + 1;
                    grdTable.Columns.Add(grdCol);
                }
            }
        }

        DataTable dtEmpTbl = new DataTable();
        dtEmpTbl = Global.GetEmployees(Convert.ToInt32(Session["LocationID"]), true);
        DataView dv = dtEmpTbl.DefaultView;
        if (hdnFilter.Value.Trim() != "Nofilter")
            dv.RowFilter = "empName LIKE '" + hdnFilter.Value.Trim() + "%'";
        dv.Sort = "EmpID asc";
        DataTable dtEmp = dv.ToTable();

        if ((dtEmp.Rows.Count > 0))
        {//EMP LOOP
            foreach (DataRow drEmp in dtEmp.Rows)
            {
                int i = 0;
                DataRow gdRow = grdTable.NewRow();
                gdRow[0] = drEmp[1].ToString();
                // WEEKDATE LOOP
                foreach (DataRow dr in dsWeekDate.Tables[0].Rows)
                {
                    i = i + 1;
                    gdRow[i] = GetWeekHours(Convert.ToInt32(drEmp[0].ToString()), Convert.ToDateTime(dr[0].ToString()), Convert.ToDateTime(dr[1].ToString()));
                }
                grdTable.Rows.Add(gdRow);
            }
        }
        grdReport.DataSource = grdTable;
        grdReport.DataBind();

        int tempVar = 0;
        double tempVar2 = 0;
        int colCount = grdTable.Columns.Count;
        foreach (GridViewRow gridRow in grdReport.Rows)
        {
            for (tempVar = 1; tempVar <= colCount - 1; tempVar++)
            {
                tempVar2 = tempVar % 2;
                try
                {
                    if (tempVar2 != 0)
                    {
                        if (Convert.ToDouble(gridRow.Cells[tempVar].Text) < 50.0)
                        {
                            gridRow.Cells[tempVar].BackColor = Color.Yellow;
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(gridRow.Cells[tempVar].Text) < 45.0)
                        {
                            gridRow.Cells[tempVar].BackColor = Color.Yellow;
                        }
                    }
                }
                catch (Exception ex)
                {
                    gridRow.Cells[tempVar].Text = "0.0";
                    gridRow.Cells[tempVar].BackColor = Color.Yellow;
                }
            }
        }
        UpdatePanel1.Update();
    }

    public string GetWeekHours(int empID, DateTime WkStartDate, DateTime WkEndDate)
    {
        double TotalHr = 0.0;
        double weekHours = 0.0;
        double leaveHr = 0.0;
        weekHours = Attendance.GetValue(0, 0, 0, 0, empID, WkStartDate, WkEndDate, hdnFilter.Value, "GetWeekHours");
        leaveHr = Attendance.GetValue(0, 0, 0, 0, empID, WkStartDate, WkEndDate, hdnFilter.Value, "GetLeaveHours");
        TotalHr = weekHours + leaveHr;
        return Convert.ToString(TotalHr);
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        string[] words = txtFromDate.Text.Split('/');
        string _fromdate = "";
        _fromdate = words[1] + "/" + words[0] + "/" + words[2];

        string[] wordsto = txtToDate.Text.Split('/');
        string _todate = "";
        _todate = wordsto[1] + "/" + wordsto[0] + "/" + wordsto[2];


        DateTime toDate = new DateTime();
        DateTime fromDate = new DateTime();
        toDate = Convert.ToDateTime(_todate);

        fromDate = Convert.ToDateTime(_fromdate);
        string strUpdate = "";
        strUpdate = hdStatusUpdate.Value;
        int Eid = 0;
        Eid = Convert.ToInt32(hdEmpid.Value);
        while (fromDate <= toDate)
        {
            Attendance objatt = new Attendance();
            txtToDate.Text = fromDate.ToString("MM/dd/yyyy");
            txtToDate.Text = txtToDate.Text + " " + hdnTime.Value + "AM";
           // int empId = Eid;
            objatt.empId = Eid;
            objatt.attDate = Convert.ToDateTime(txtToDate.Text);
           
            objatt.attStatus = ddlStatus.SelectedValue;
            objatt.attInTime = Convert.ToDateTime(txtToDate.Text);
            objatt.attOutTime = Convert.ToDateTime(txtToDate.Text).AddHours(9);
            objatt.attComment = "";
            objatt.attIP = Global.GetLocalIPAddress();
            objatt.adminID = UM.EmployeeID;//Convert.ToInt32(Session["CurrentEmpID"].ToString());


            if (strUpdate == "")
            {
                objatt.mode = "Insert";
            }
            else
            {
                objatt.mode = "update";
            }
            Attendance.AddUpdateEmpAtt(objatt);
            fromDate = fromDate.AddDays(1);
        }
        BindGrid();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        hdnFilter.Value = txtfilter.Text.Trim();
        BindGrid();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        hdnFilter.Value = "Nofilter";
        txtfilter.Text = string.Empty;
        BindGrid();

    }
    protected void btnNodataFound_Click(object sender, EventArgs e)
    {
        hdnFilter.Value = "Nofilter";
        txtfilter.Text = string.Empty;
        BindGrid();

    }

    protected void lbtnpre_Click(object sender, EventArgs e)
    {
        currentDate = ((DateTime)Session["CurrentAttandenceDate"]).AddMonths(-1);
        intMonth = currentDate.Month;
        Session["CurrentAttandenceDate"] = currentDate;
        lblMonthYear.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
        DataCalendar.DataBind();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "fn", "HideLoading();", true);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowCalendarDiv();", true);
    }
    protected void lbtnnext_Click(object sender, EventArgs e)
    {
        currentDate = ((DateTime)Session["CurrentAttandenceDate"]).AddMonths(1);
        intMonth = currentDate.Month;
        Session["CurrentAttandenceDate"] = currentDate;
        lblMonthYear.Text = Microsoft.VisualBasic.DateAndTime.MonthName(intMonth).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(currentDate);
        DataCalendar.DataBind();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "fn", "HideLoading();", true);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowCalendarDiv();", true);
    }


    protected void lnk_EmpName_Click(object sender, EventArgs e)
        {
        UM = UserMaster.UserMasterInfo();
        LinkButton lb = (LinkButton)sender;

        GridViewRow row = (GridViewRow)lb.NamingContainer;
        int i = Convert.ToInt32(row.RowIndex);

        Label empid = (Label)row.FindControl("Label1");
        int id =Convert.ToInt32(empid.Text);
        Session["CurrentEmpID"] = id;


        lbl_empname.Text = lb.Text;
        int locationfk= Attendance.getLocationFkbyEmpId(Convert.ToInt32(id), "getEmpLocationFk");
        //int locationId = Convert.ToInt32(loc.Text); 
        Session["CurrentEmpLocationFKID"] = locationfk;
        DataCalendar.DataBind();
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowCalendarDiv();", true);

        
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
                objatt.empId = Convert.ToInt32(Session["CurrentEmpID"].ToString());// UM.EmployeeID;
                objatt.attDate = dtNow;
                objatt.attStatus = strStatus;
                objatt.attComment = "";
                objatt.adminID = UM.EmployeeID; //Convert.ToInt32(Session["CurrentEmpID"].ToString());// 

                int empid = new Attendance().CheckExistsEmp("CheckExistsEmp", Convert.ToInt32(Session["CurrentEmpID"].ToString()),/* UM.EmployeeID*/ dtNow.ToShortDateString());
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
    protected void MyCalendar_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["startdate"] = Session["CurrentAttandenceDate"];
        e.InputParameters["intUserID"] = Session["CurrentEmpID"];//UM.EmployeeID;
        e.InputParameters["intUserLocation"] = Session["CurrentEmpLocationFKID"];//UM.LocationID;
    }
}







