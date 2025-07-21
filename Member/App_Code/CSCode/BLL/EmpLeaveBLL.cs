using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;
using nsMobileAPI;
using System.Configuration;
using AgoraBL.BAL;
/// <summary>
/// Summary description for EmpLeaveBLL
/// </summary>
public class EmpLeaveBLL
{
    public int ID { get; set; }
    public string LeaveType { get; set; }
    public string LeaveFrom { get; set; }
    public string LeaveTo { get; set; }
    public string LeaveDesc { get; set; }
    public string LeaveStatus { get; set; }
    public string AdminComments { get; set; }

    /// <summary>
    /// Add some extra property for Mobile Api
    /// </summary>  
    //// public ResponseData ObjresponseData { get; set; }  

    public int TotalCL { get; set; }
    public int TotalSL { get; set; }
    public int TotalPL { get; set; }
    public int TotalCO { get; set; }

    public int ConsumedCL { get; set; }
    public int ConsumedSL { get; set; }
    public int ConsumedPL { get; set; }
    public int ConsumedCO { get; set; }

    public int BalanceCL { get; set; }
    public int BalanceSL { get; set; }
    public int BalancePL { get; set; }
    public int BalanceCO { get; set; }

    public int TotalCLTillDate { get; set; }
    public int TotalSLTillDate { get; set; }
    public int TotalPLTillDate { get; set; }
    public int TotalCOTillDate { get; set; }

    public EmpLeaveBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public EmpLeaveBLL(string LeaveType, string LeaveFrom, string LeaveTo, string LeaveDesc)
    {
        this.LeaveType = LeaveType;
        this.LeaveFrom = LeaveFrom;
        this.LeaveTo = LeaveTo;
        this.LeaveDesc = LeaveDesc;
    }
    public DataSet GetLeave(int empid)
    {
        return GetLeave("LEAVE", empid);
    }

    public DataSet GetLeaveDetails(int empid, string leaveType = "0", string lStatus = "0", int year = 0)// string LeaveMonth = "")
    {
        return GetLeave("LEAVEDETAILS", empid, leaveType, lStatus, year);//LeaveMonth);
    }

    private DataSet GetLeave(string mode, int empid, string leaveType = "0", string lStatus = "0", int year = 0)//string LeaveMonth="")
    {
        EmpLeaveDAL objLeaveDAL = new EmpLeaveDAL();
        return objLeaveDAL.GetLeave(mode, empid, leaveType, lStatus, year);//LeaveMonth);
    }

    public DataSet SaveLeave(int EmpID, string leaveType, string leaveFrom, string leaveTo, string Reason,out bool IsEmailSent)
    {
        //EmpLeaveBLL obj = new EmpLeaveBLL();
        //obj.LeaveType = leaveType;
        //obj.LeaveFrom = leaveFrom;
        //obj.LeaveTo = leaveTo;
        //obj.LeaveDesc = Reason;

        //EmpLeaveDAL objLeaveDAL = new EmpLeaveDAL();
        //DataSet ds = new DataSet();
        //return ds = objLeaveDAL.SaveLeave("SAVE", EmpID, obj);
        EmployeeMasterBAL ObjEmpLeaveBLL = new EmployeeMasterBAL();
        DataSet dsEmpLeave = new DataSet();
        dsEmpLeave = ObjEmpLeaveBLL.SaveLeave(EmpID, leaveType, leaveFrom, leaveTo, Reason, EmpName: string.Empty, IsEmailSent:out IsEmailSent);
        return dsEmpLeave;
    }

    public int DeleteLeave(int empLeaveID)
    {
        //EmpLeaveDAL objLeaveDAL = new EmpLeaveDAL();
        //return objLeaveDAL.DeleteLeave("DELETE", empLeaveID);
        EmployeeMasterBAL ObjEmpLeaveBLL = new EmployeeMasterBAL();
        return ObjEmpLeaveBLL.DeleteLeave(empLeaveID);
    }

    public static bool IfExistsLeave(string mode, int empid, string leaveFrom, string leaveTo)
    {
        EmpLeaveDAL objLeaveDAL = new EmpLeaveDAL();
        return objLeaveDAL.IfExistsLeave(mode, empid, leaveFrom, leaveTo);
    }

    public DataTable GetPmDetails(string mode, string empid, bool IsAdmin)
    {
        EmpLeaveDAL objLeaveDAL = new EmpLeaveDAL();
        DataTable dt = new DataTable();
        return dt = objLeaveDAL.GetPmDetails(mode, empid, IsAdmin);
    }


    /// <summary>
    /// method is usedsend mail to employee for epplyed leave
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="strDateFrom"></param>
    /// <param name="strDateTo"></param>
    /// <param name="strReason"></param>
    public void SendMail(DataTable dt, string strDateFrom, string strDateTo, string strReason)
    {
        string[] DateFrom = strDateFrom.Split('/');
        string New_FromDt = DateFrom[1] + '/' + DateFrom[0] + '/' + DateFrom[2];
        DateTime dtFrom = Convert.ToDateTime(New_FromDt.ToString());

        string[] DateTo = strDateTo.Split('/');
        string New_ToDt = DateTo[1] + '/' + DateTo[0] + '/' + DateTo[2];
        DateTime dtTo = Convert.ToDateTime(New_ToDt.ToString());

        string strBody, strSubject, mailTo, mailFrom, message, CC = "";
        List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 44);
        strBody = lstConfig[0].value.ToString();
        strBody = strBody.Replace("{UserNmae}", Convert.ToString(dt.Rows[0]["empName"]));
        strBody = strBody.Replace("{Fromdate}", Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy"));
        strBody = strBody.Replace("{Todate}", Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy"));
        strBody = strBody.Replace("{Reason}", strReason);
        strSubject = Convert.ToString(dt.Rows[0]["empName"]) + " has applied for leave.";
        //mailTo = "apurva.p@intelgain.com"; ///for local
        mailTo = ConfigurationManager.AppSettings.Get("HREmail").ToString();
        mailFrom =CC = dt.Rows[0]["empEmail"].ToString();
        message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
    }
}