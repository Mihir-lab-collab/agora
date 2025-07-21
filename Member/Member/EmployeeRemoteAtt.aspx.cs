using System;
using System.Data;
using System.Reflection.Emit;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_EmployeeRemoteAtt : Authentication
{
    private UserMaster UM;
    private EmpWFHBLL objEmpWFH;
    private DataSet ds;

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        objEmpWFH = new EmpWFHBLL();
        Session["IsRemoteEmployee"] = UM.IsRemoteEmployee;
        if (Session["IsRemoteEmployee"] == null || (bool)Session["IsRemoteEmployee"] == false)
        {
            Response.Redirect("Home.aspx");
            return;
        }
        if (!IsPostBack)
        {
            int EmpID = UM.EmployeeID;
            hdnEmpId.Value = Convert.ToString(EmpID);
            // Load attendance details for the current month
            LoadWFHAttendanceForMonth(DateTime.Now);
            // Update the button states based on leave status
            UpdateButtonStates(EmpID, DateTime.Today);
        }
    }

    private void LoadWFHAttendanceForMonth(DateTime month)
    {
        Label1.Text = month.ToString("MMMM yyyy");
        lblSelectedMonth.Text = " Remote Attendance for " + month.ToString("MMMM yyyy");
        int EmpID = UM.EmployeeID;

        string WFHFrom = new DateTime(month.Year, month.Month, 1).ToString("yyyy/MM/dd");
        string WFHTo = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month)).ToString("yyyy/MM/dd");

        ds = objEmpWFH.AppliedWFHFromTo(EmpID, WFHFrom, WFHTo);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dataTable = ds.Tables[0];
            DataTable reversedTable = dataTable.Clone(); // Create a structure clone of the table

            for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
            {
                reversedTable.ImportRow(dataTable.Rows[i]);
            }

            gridwfhattendancedetails.DataSource = reversedTable;
            gridwfhattendancedetails.DataBind();
        }
        else
        {
            gridwfhattendancedetails.DataSource = new DataTable();
            gridwfhattendancedetails.DataBind();
        }
    }

    //private void UpdateButtonStates(int empId, DateTime date)
    //{
    //    // Check if leave is approved
    //    bool isOnLeave = IsLeaveApproved(empId, date);
    //    btnStartDay.Enabled = !isOnLeave;
    //    btnEndDay.Enabled = !isOnLeave;

    //}
    private void UpdateButtonStates(int empId, DateTime date)
    {
        // Check if leave is approved
        bool isOnLeave = IsLeaveApproved(empId, date);

        // Check if the start day has already been clicked
        bool isStartDayClicked = IsStartDateAlreadyExist(empId, date);
        btnStartDay.Enabled = !isOnLeave && !isStartDayClicked;
        btnEndDay.Enabled = !isOnLeave && isStartDayClicked; // End day button should only be enabled if the start day is clicked
    }

    private bool IsLeaveApproved(int empId, DateTime date)
    {
        DataTable leaveDetails = null;
        try
        {
            leaveDetails = objEmpWFH.GetLeaveStatus(empId);
        }
        catch (Exception ex)
        {
            return false;
        }
        if (leaveDetails == null || leaveDetails.Rows.Count == 0)
        {
            return false; // No leave details found
        }
        foreach (DataRow row in leaveDetails.Rows)
        {
            if (row["LeaveStatus"] == DBNull.Value || row["leaveFrom"] == DBNull.Value || row["leaveTo"] == DBNull.Value)
            {
                continue; // Skip rows with missing data
            }
            string leaveStatus = row["LeaveStatus"].ToString();
            DateTime leaveFrom = Convert.ToDateTime(row["leaveFrom"]);
            DateTime leaveTo = Convert.ToDateTime(row["leaveTo"]);
            if (leaveStatus.Equals("a", StringComparison.OrdinalIgnoreCase) &&
                date >= leaveFrom && date <= leaveTo)
            {
                return true;
            }
        }

        return false;
    }
    private bool IsStartDateAlreadyExist(int empId, DateTime date)
    {
        try
        {        
            return objEmpWFH.GetAttendanceByDate(empId, date);
        }
        catch (Exception ex)
        {
            return false; 
        }
    }




    protected void PagerButtonClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        DateTime currentMonth = DateTime.Parse(Label1.Text);
        DateTime newMonth = btn.CommandArgument == "prev" ? currentMonth.AddMonths(-1) : currentMonth.AddMonths(1);
        LoadWFHAttendanceForMonth(newMonth);
    }

    protected void btnStartDay_Click(object sender, EventArgs e)
    {
        objEmpWFH.InsertRAAttendance(UM.EmployeeID);
        LoadWFHAttendanceForMonth(DateTime.Parse(Label1.Text));
        UpdateButtonStates(UM.EmployeeID, DateTime.Today);
    }

    protected void btnEndDay_Click(object sender, EventArgs e)
    {
        DateTime attDate = DateTime.Now.Date;
        objEmpWFH.UpdateRAAttendance(UM.EmployeeID, attDate);
        LoadWFHAttendanceForMonth(DateTime.Parse(Label1.Text));
        UpdateButtonStates(UM.EmployeeID, DateTime.Today);
    }


}
