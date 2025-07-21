using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LeaveBalanceBLL
/// </summary>
public class LeaveBalanceBLL
{
    public int EmpID { get; set; }
    public string EmpName { get; set; }
    public string LeaveSummary { get; set; }
    public LeaveBalanceBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public LeaveBalanceBLL(int EmpID, string EmpName, string LeaveSummary)
    {
        this.EmpID = EmpID;
        this.EmpName = EmpName;
        this.LeaveSummary = LeaveSummary;
    }
    public static List<LeaveBalanceBLL> GetLeaveBalance(string mode)
    {
        LeaveBalanceDAL objLeaveBalanceDAL = new LeaveBalanceDAL();
        return objLeaveBalanceDAL.GetLeaveBalance(mode);
    }
    public static DataTable GetLeaveDetails(int empId, string mode)
    {
        DataTable dt = new DataTable();
        LeaveBalanceDAL objLeaveBalanceDAL = new LeaveBalanceDAL();
        return objLeaveBalanceDAL.GetLeaveDetails(empId, mode);
    }
}