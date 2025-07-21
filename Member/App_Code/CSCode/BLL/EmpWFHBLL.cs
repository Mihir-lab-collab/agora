using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for EmpWFHBLL
/// </summary>
public class EmpWFHBLL
{
    public int ID { get; set; }
    public string WFHFrom { get; set; }
    public string WFHTo { get; set; }
    public string WFHDesc { get; set; }
    public string WFHStatus { get; set; }
    public string AdminComments { get; set; }
    public int WFHCount { get; set; }
    public DataTable SaveWFH(int EmpID, string WFHFrom, string WFHTo, string Reason, int WFHCount)
    {
        EmpWFHBLL obj = new EmpWFHBLL();
        obj.WFHFrom = WFHFrom;
        obj.WFHTo = WFHTo;
        obj.WFHDesc = Reason;
        obj.WFHCount = WFHCount;
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        DataTable dt = new DataTable();
        return dt = objWFHDAL.SaveWFH("SAVE", EmpID, obj);
    }

    public int DeleteWFHById(int empLeaveID)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.DeleteWFH("DELETE", empLeaveID);
    }

    public static bool IfExistsWFH(string mode, int empid, string WFHFrom, string WFHTo)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.IfExistsWFH(mode, empid, WFHFrom, WFHTo);
    }

    public DataSet GetWFHDetails(int empid, string lStatus = "0", int year = 0)
    {
        if (lStatus == "0")
            return GetWFHList("WFHDETAILS", empid, lStatus, year);
        return GetWFHList("GetWFHStatus", empid, lStatus, year);

    }
    public DataSet AppliedWFHFromTo(int empid, string from, string to)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        from = from.Replace("-", "/").Replace("00:00:00", "").Trim();
        to = to.Replace("-", "/").Replace("00:00:00", "").Trim();
        return objWFHDAL.AppliedWFHFromTo("AppliedWFHFromTo", empid, from, to);
    }
    private DataSet GetWFHList(string mode, int empid, string lStatus = "0", int year = 0)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.GetWFHList(mode, empid, lStatus, year);//LeaveMonth);
    }
    public int InsertWFHAttendance(int empId)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.InsertWFHAttendance("InsertAttendance", empId);
    }

    public int InsertRAAttendance(int empId)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.InsertRAAttendance("InsertAttendance", empId);
    }

    public int UpdateRAAttendance(int empId, DateTime attDate)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.UpdateRAAttendance("UpdateAttendance", empId, attDate);
    }

    public bool GetAttendanceByDate(int empId, DateTime attDate)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        int count = objWFHDAL.CheckAttendanceExistence(empId, attDate);
        return count > 0;
    }



    public int UpdateWFHAttendance(int empId, DateTime attOutTime, DateTime attDate)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.UpdateWFHAttendance("UpdateAttendance", empId, attOutTime, attDate);
    }

    public void SendMail(DataTable dt, string strDateFrom, string strDateTo, string strReason)
    {
        string[] DateFrom = strDateFrom.Split('/');
        string New_FromDt = DateFrom[1] + '/' + DateFrom[0] + '/' + DateFrom[2];
        DateTime dtFrom = Convert.ToDateTime(New_FromDt.ToString());

        string[] DateTo = strDateTo.Split('/');
        string New_ToDt = DateTo[1] + '/' + DateTo[0] + '/' + DateTo[2];
        DateTime dtTo = Convert.ToDateTime(New_ToDt.ToString());

        string strBody, strSubject, mailTo, mailFrom, message, CC = "";
        List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 53);
        strBody = lstConfig[0].value.ToString();
        strBody = strBody.Replace("{UserNmae}", Convert.ToString(dt.Rows[0]["empName"]));
        strBody = strBody.Replace("{Fromdate}", Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy"));
        strBody = strBody.Replace("{Todate}", Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy"));
        strBody = strBody.Replace("{Reason}", strReason);
        strSubject = Convert.ToString(dt.Rows[0]["empName"]) + " has applied for Work From Home.";
        //mailTo = "apurva.p@intelgain.com"; ///for local
        mailTo = ConfigurationManager.AppSettings.Get("HREmail").ToString();
        mailFrom = CC = dt.Rows[0]["empEmail"].ToString();
        message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
    }
    public static bool CheckInTime(string mode, int empid)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.CheckInTime(mode, empid);
    }
    public DataTable BindWFHBalance(int empid)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.BindWFHBalance(empid);
    }
    public DataTable GetLeaveStatus(int empId)
    {
        EmpWFHDAL objWFHDAL = new EmpWFHDAL();
        return objWFHDAL.GetLeaveStatus(empId);
    }

}