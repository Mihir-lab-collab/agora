using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

public class TimeSheet
{


    public int ProjID { get; set; }
    public string ProjName { get; set; }
    public int ModuleID { get; set; }
    public string ModuleName { get; set; }
    public int EmpID { get; set; }
    public string EmpName { get; set; }
    public int TSID { get; set; }
    public DateTime TSDate { get; set; }
    public string TSDisplayDate { get; set; }
    public double TSHour { get; set; }
    public string TSComment { get; set; }
    public Boolean IsApproved { get; set; }
    public int EmpApproveID { get; set; }
    public string Role { get; set; }
    public string AttHour { get; set; }
    public string AttTSHour { get; set; }
    public string ApprovedBy { get; set; }
    public string ApprovedOn { get; set; }
    public DateTime TSEntryDate { get; set; }
    public string ProjectTotalHrs { get; set; }
    public Boolean TsVerified { get; set; }
    public string AccountManagerEmail { get; set; }
    public string ProjectManagerEmail { get; set; }
    public string ManagerName { get; set; }
    public Boolean IsSendMail { get; set; }
    public bool IsSaturday { get; set; }
    public bool IsSunday { get; set; }
    public bool IsHoliday { get; set; }
    public bool IsLeave { get; set; }
    public string TsVerifiedBy { get; set; }
    public string TsVerifiedOn { get; set; }

    // added for US-968-consildate Timesheet
    public int TotalHours { get; set; } // added by atif for consolidate timesheet hours
    public string Designation { get; set; } //added by atif for consolidate timesheet Designation
    public int ProjectTotalHours { get; set; } //added by atif for consolidate timesheet ProjectTotalHours.
    public string Module { get; set; }
    public string ProjectStartDate { get; set; }
    public string ProjectStausDate { get; set; }
    public string ProjectStaus { get; set; }
    //end
    public TimeSheet()
    {
    }

    public TimeSheet(int _ProjID, string _ProjName)
    {
        this.ProjID = _ProjID;
        this.ProjName = _ProjName;
    }

    public TimeSheet(int _ProjID, int _EmpID, string _EmpName)
    {
        this.EmpID = _EmpID;
        this.EmpName = _EmpName;
    }

    public TimeSheet(int _TSID, int _ProjID, string _ProjName, int _ModuleID, string _ModuleName, int _EmpID, string _EmpName,
        DateTime _TSDate,string __TSDisplayDate, int _TSHour, string _TSComment, Boolean _IsApproved, DateTime _TSEntryDate,
        string _ApprovedBy, string _ApprovedOn, string _ProjectTotalHrs, Boolean _IsSendMail,
        bool _IsSaturday, bool _IsSunday, bool _IsHoliday, bool _IsLeave)
    {
        this.TSID = _TSID;
        this.ProjID = _ProjID;
        this.ProjName = _ProjName;
        this.ModuleID = _ModuleID;
        this.ModuleName = _ModuleName;
        this.EmpID = _EmpID;
        this.EmpName = _EmpName;
        this.TSDate = _TSDate;
        this.TSDisplayDate = __TSDisplayDate;
        this.TSHour = _TSHour;
        this.TSComment = _TSComment;
        this.TSEntryDate = _TSEntryDate;
        this.ApprovedBy = _ApprovedBy;
        this.ApprovedOn = _ApprovedOn;
        this.IsApproved = _IsApproved;
        this.ProjectTotalHrs = _ProjectTotalHrs;
        this.IsSendMail = _IsSendMail;
        this.IsSaturday = _IsSaturday;
        this.IsSunday = _IsSunday;
        this.IsHoliday = _IsHoliday;
        this.IsLeave = _IsLeave;
    }

    public TimeSheet(string _EmpName, string _Role, int _TSHour, int _EmpID)
    {
        this.EmpName = _EmpName;
        this.Role = _Role;
        this.TSHour = _TSHour;
        this.EmpID = _EmpID;
    }
    public TimeSheet(int _tsId, int _moduleId, int _empid, DateTime _tsDate, int _tsHour, string _tsComment, DateTime _tsEntryDate, Boolean _tsVerified)
    {
        this.TSID = _tsId;
        this.ModuleID = _moduleId;
        this.EmpID = _empid;
        this.TSDate = _tsDate;
        this.TSHour = _tsHour;
        this.TSComment = _tsComment;
        this.TSEntryDate = _tsEntryDate;
        this.TsVerified = _tsVerified;

    }

    // added by vw
    public TimeSheet(int _tsId, int _moduleId, int _empid, DateTime _tsDate, int _tsHour, string _tsComment, DateTime _tsEntryDate, Boolean _tsVerified, string _tsVerifiedBy, string _tsVerifiedOn, int _projId, string _projName, string _moduleName)
    {
        this.TSID = _tsId;
        this.ModuleID = _moduleId;
        this.EmpID = _empid;
        this.TSDate = _tsDate;
        this.TSHour = _tsHour;
        this.TSComment = _tsComment;
        this.TSEntryDate = _tsEntryDate;
        this.TsVerified = _tsVerified;
        this.TsVerifiedBy = _tsVerifiedBy;
        this.TsVerifiedOn = _tsVerifiedOn;
        this.ProjID = _projId;
        this.ProjName = _projName;
        this.ModuleName = _moduleName;

    }

    // Trupti Dandekar
    public static List<TimeSheet> getall(string mode)
    {
        TimeSheetDAL objtype = new TimeSheetDAL();
        return objtype.GetAllTimeSheetData(mode);
    }

    // Vishal w
    public static MultiResultUserDetails getMultiResultUserDetails(string mode, int Month, int Year, int Empid, int TSID, DateTime TSDate)
    {
        TimeSheetDAL objtype = new TimeSheetDAL();
        return objtype.GetMultiResultUserDetails(mode, Month, Year, Empid, TSID, TSDate);
    }

    public TimeSheet(int _EmpID, string _EmpName, DateTime _TSDate, string _AttHour, string _AttTSHour)
    {
        this.EmpID = _EmpID;
        this.EmpName = _EmpName;
        this.TSDate = _TSDate;
        this.AttHour = _AttHour;
        this.AttTSHour = _AttTSHour;
    }

    public static IList<TimeSheet> GetTS(int TSID)
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.GetTS(TSID, 0, 0, 0, 0, 0, 0, "");
    }
    // added for US-968-Consolidate Timesheet

    public static IList<TimeSheet> GetCT(int projId, int month, int year, string mode)
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.GetConsolidateTimesheet(projId, month, year, mode);

    }
    public TimeSheet GetStatus(int projId)
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        TimeSheet ts = new TimeSheet();
        return ts = objTS.GetStatus(projId);
    }
    //end
    public static IList<TimeSheet> GetEmailDetails(int ProjID)
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.GetEmailDetailsByProjID(ProjID);
    }
    public static IList<TimeSheet> GetManageTS(int ProjID = 0, int EmpID = 0, int Month = 0, int Year = 0, int ModuleID = 0, int LocationID = 0, string IsApproved = "")
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.GetManageTS(0, ProjID, EmpID, Month, Year, ModuleID, LocationID, IsApproved);
    }
    public static IList<TimeSheet> GetTS(int ProjID = 0, int EmpID = 0, int Month = 0, int Year = 0, int ModuleID = 0, int LocationID = 0, string IsApproved = "")
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.GetTS(0, ProjID, EmpID, Month, Year, ModuleID, LocationID, IsApproved);
    }

    public static IList<TimeSheet> GetTSReport(int Month, int Year, int LocationID)
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.GetTSReport(Month, Year, LocationID);
    }

    public static List<TimeSheet> GetIncomepleteTS(int Month, int Year, int LocationID, int EmpID)
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.GetIncomepleteTS(Month, Year, LocationID, EmpID);

    }

    public static IList<TimeSheet> GetMember(int LocationId, int ProjectID)
    {
        TimeSheetDAL ObjMember = new TimeSheetDAL();
        return ObjMember.GetMember(LocationId, ProjectID);
    }

    public static Boolean Approve(int TSID, Boolean IsApproved, int EmpID)
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.Approve(TSID, IsApproved, EmpID);
    }

    public static Boolean Delete(int TSID)
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.Delete(TSID);
    }

    public static Boolean Update(int ModuleID, int EmpID, DateTime TSDate, int TSHour, string TSComment, int TSID)
    {
        TimeSheetDAL objTime = new TimeSheetDAL();
        return objTime.Update(ModuleID, EmpID, TSDate, TSHour, TSComment, TSID);
    }

    public List<EmpSaturdayHoliday> GetEmpSaturdayHoliday()
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.GetEmpSaturdayHoliday();
    }
    public static List<TimeSheet> GetConsolidatedTimesheetByDateRange(int projId, DateTime fromDate, DateTime toDate)
    {
        TimeSheetDAL objTS = new TimeSheetDAL();
        return objTS.GetConsolidatedTimesheetByDateRange(projId, fromDate, toDate);
    }

}

public class EmpSaturdayHoliday
{
    public string WeekDate { get; set; }
    public string WeekDayName { get; set; }
    public string WeekNumber { get; set; }

}
public class EmployeeProjectsNames
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public string EmployeeName { get; set; }

    public EmployeeProjectsNames(int _ProjectId, string _ProjectName, string _EmployeeName)
    {
        this.ProjectId = _ProjectId;
        this.ProjectName = _ProjectName;
        this.EmployeeName = _EmployeeName;
    }
}
public class EmployeeProjectModule
{
    public int ProjectId { get; set; }
    public string ModuleName { get; set; }
    public string ProjectName { get; set; }
    public int moduleId { get; set; }

    public EmployeeProjectModule(int _ProjectId, string _ModuleName, string _ProjectName, int _moduleId)
    {
        this.ProjectId = _ProjectId;
        this.ModuleName = _ModuleName;
        this.ProjectName = _ProjectName;
        this.moduleId = _moduleId;
    }
}
public class EmployeePreviousRequestDates
{
    public int EmpId { get; set; }
    public string RequestDate { get; set; }
    public string InsertedBy { get; set; }
    public string insertedOn { get; set; }

    public EmployeePreviousRequestDates(int _EmpId, string _RequestDate, string _InsertedBy, string _insertedOn)
    {
        this.EmpId = _EmpId;
        this.RequestDate = _RequestDate;
        this.InsertedBy = _InsertedBy;
        this.insertedOn = _insertedOn;
    }
}

public class EmployeeIncompleteTimesheet
{
    public int EmpID { get; set; }
    public string EMPName { get; set; }
    public string Date { get; set; }
    public string TimeAvailable { get; set; }
    public string TimeReported { get; set; }

    public EmployeeIncompleteTimesheet(int _EmpID, string _EMPName, string _Date, string _TimeAvailable, string _TimeReported)
    {
        this.EmpID = _EmpID;
        this.EMPName = _EMPName;
        this.Date = _Date;
        this.TimeAvailable = _TimeAvailable;
        this.TimeReported = _TimeReported;
    }
}

public class EmployeeTimesheetFilledHrs
{
    public int FilledTotHrs { get; set; }
    public EmployeeTimesheetFilledHrs(int _FilledTotHrs)
    {
        this.FilledTotHrs = _FilledTotHrs;
    }
}
public class MultiResultUserDetails
{
    public List<TimeSheet> TimeSheet { get; set; }
    public List<EmployeeProjectsNames> EmployeeProjectsNames { get; set; }
    public List<EmployeeProjectModule> EmployeeProjectModule { get; set; }
    public List<EmployeePreviousRequestDates> EmployeePreviousRequestDates { get; set; }
    public List<EmployeeIncompleteTimesheet> EmployeeIncompleteTimesheet { get; set; }
    public List<EmployeeTimesheetFilledHrs> EmployeeTimesheetFilledHrs { get; set; }
    //public EmployeeTimesheetFilledHrs TotFilledTotHrs { get; set; }
    public string Alloweddate { get; set; }


}
