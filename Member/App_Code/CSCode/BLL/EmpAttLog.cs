using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmpAttLog
/// </summary>
public class EmpAttLog
{
    public int EmpID { get; set; }
    public string PunchTime { get; set; }
    public string IP { get; set; }
    public string Mode { get; set; }
    public string InsertedOn { get; set; }
    public DateTime attDate { get; set; }
    public string attInTime { get; set; }
    public string attOutTime { get; set; }
    public string attStatus { get; set; }
    public string timesheethours { get; set; }
    public string workinghours { get; set; }
    public string brktimehours { get; set; }
    public string attendanceDate { get; set; }
    public string description { get; set; }

    public string Comment { get; set; }

    public EmpAttLog()
    {
 
    }
    public EmpAttLog(int EmpId, string PunchTime, string IP, string Mode, string InsertedOn)
    {
        this.EmpID = EmpId;
        this.PunchTime = PunchTime;
        this.IP = IP;
        this.Mode = Mode;
        this.InsertedOn = InsertedOn;
    }
    public EmpAttLog(DateTime attDate, string attInTime, string attOutTime, string attStatus, 
        string timesheethours, string workinghours,string brktimehours,string Comment)
    {
        this.attDate = attDate;
        this.attInTime = attInTime;
        this.attOutTime = attOutTime;
        this.attStatus = attStatus;
        this.timesheethours = timesheethours;
        this.workinghours = workinghours;
        this.brktimehours = brktimehours;
        this.Comment = Comment;
    }
    public static List<EmpAttLog> getEmpAttLogData(int empid, DateTime FromDate, string Mode)
    {
        return AttendanceDAL.getEmpAttLog(empid, FromDate, Mode);
    }
    public static List<EmpAttLog> GetCalendarAtt(string mode,int empid,string startDate,string endDate)
    {
        return AttendanceDAL.GetCalendarAtts(mode, empid, startDate, endDate);
    }
   
}