using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HolidayWorking
/// </summary>
public class HolidayWorking
{
    public int Id { get; set; }
    public int EmpId { get; set; }
    public string EmpName { get; set; }
    public int ProjId { get; set; }
    public string HolidayDate { get; set; }
    public int ExpectedHours { get; set; }
    public string UserReason { get; set; }
    public string mode { get; set; }
    public string ProjectName { get; set; }
    public string UserEntryDate { get; set; }  
    public int Status { get; set; }
    public string AdminComment { get; set; }
    public string AdminCanReason { get; set; }
    public string Statusflag { get; set; }
    public string empEmail { get; set; }
    public string projName { get; set; }
    public int EntryBy { get; set; }
    public string EntryByName { get; set; }
    
	public HolidayWorking()
	{
	}

    public HolidayWorking(int Id, int EmpId, string HolidayDate, int ProjId, int ExpectedHours, string UserReason,string ProjectName,
                          string UserEntryDate, int Status, string AdminComment, string AdminCanReason, string Statusflag)
    {                                                
        this.Id = Id;
        this.EmpId = EmpId;
        this.HolidayDate = HolidayDate;
        this.ProjId = ProjId;
        this.ExpectedHours = ExpectedHours;
        this.UserReason = UserReason;
        this.ProjectName = ProjectName;
        this.UserEntryDate = UserEntryDate;
        this.Status = Status;
        this.AdminComment = AdminComment;
        this.AdminCanReason = AdminCanReason;
        this.Statusflag = Statusflag;
    }

    public HolidayWorking(int EmpId, string empEmail, string ProjName)
    {
        // TODO: Complete member initialization
        this.EmpId = EmpId;
        this.empEmail = empEmail;
        this.ProjectName = ProjName;
    }

    public HolidayWorking(string empName, int projId, string projName,int empid,string HolidayDate)
    {
        // TODO: Complete member initialization
        this.EmpName = empName;
        this.ProjId = projId;
        this.ProjectName = projName;
        this.EmpId = empid;
        this.HolidayDate = HolidayDate;     
    }

    public HolidayWorking(int coID, int EmpId, string Name, string HolidayDate, string Admincomment, string UserEntryDate,int EntryBy,string EntryByName)
    {
        // TODO: Complete member initialization
        this.Id = coID;
        this.EmpId = EmpId;
        this.EmpName = Name;
        this.HolidayDate = HolidayDate;
        this.AdminComment = Admincomment;
        this.UserEntryDate = UserEntryDate;
        this.EntryBy = EntryBy;
        this.EntryByName = EntryByName;
    }

    public HolidayWorking(string empName, int Id, string HolidayDate, int EmpId, int PrjId, int ExpectedHours, string UserReason,string AdminReason,string AdminCancelReason,string projName)
    {
        // TODO: Complete member initialization
        this.EmpName = empName;
        this.Id = Id;
        this.HolidayDate = HolidayDate;
        this.EmpId = EmpId;
        this.ExpectedHours = ExpectedHours;
        this.UserReason = UserReason;
        this.AdminComment=AdminReason;
        this.AdminCanReason = AdminCancelReason;
        this.projName = projName;
    }

    public static List<HolidayWorking> GetHolidayWorkingDetails(string mode, int EmpId,int ProjectId)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        return objHolidayW.GetHolidayWorkingDetails(null, mode, EmpId, null, ProjectId, null, null);
    }

    public static List<HolidayWorking> GetHolidayDate()
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        return objHolidayW.GetHolidayDate();
        
    }

    public static string SaveHolidayWorking(string mode, int Id, int EmpID, int ProjId, string HolidayDate, int ExpectedHours, string UserReason)
    {
        string output = "";
        HolidayWorking objHoliday = new HolidayWorking();
        objHoliday.Id = Id;
        objHoliday.EmpId = EmpID;
        objHoliday.ProjId = ProjId;
        objHoliday.HolidayDate = HolidayDate;
        objHoliday.ExpectedHours = ExpectedHours;
        objHoliday.UserReason = UserReason;
        HolidayWorkingDAL objholidayDAL = new HolidayWorkingDAL();
        output= objholidayDAL.SaveHolidayWorking(mode, objHoliday);
        return output;
    }

    public static void CancelHolidayWorking(string mode, int EmpID, string HolidayDate, int ProjId)
    {
        HolidayWorking objHoliday = new HolidayWorking();
        objHoliday.EmpId = EmpID;
        objHoliday.HolidayDate = HolidayDate;
        objHoliday.ProjId = ProjId;
        HolidayWorkingDAL objCancel = new HolidayWorkingDAL();
        objCancel.CancelHolidayWorking(mode, objHoliday);
    }

    public static List<HolidayWorking> GetPmDetailsByProjid(int ProjectId)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        return objHolidayW.GetPmDetailsByProjid(ProjectId);
    }

    public static List<HolidayWorking> GetCompOffDetails(int HolidayLeaveId)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        return objHolidayW.GetCompOffDetails(HolidayLeaveId);
    }

    public static List<HolidayWorking> GetHolidayWorkingData(int Empid, int Status, string HolidayStartDate, string HolidayEndDate,int LocationID)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        return objHolidayW.GetHolidayWorkingData(Empid, Status, HolidayStartDate, HolidayEndDate, LocationID);
    }

    public static void AddToCompOff(int Empid, int HolidayLeaveID,int Status)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        objHolidayW.AddToCompOff(Empid,HolidayLeaveID,Status);
    }

    public static void CreateCompOff(int Empid, string CompOffDate, string Comment, int EntryBy)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        objHolidayW.CreateCompOff(Empid,CompOffDate,Comment,EntryBy);
    }


    public static void HolidayLeave(string mode,int Empid, string Comment, int HolidayLeaveID,string CompOffDate)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        objHolidayW.HolidayLeave(mode,Empid, Comment, HolidayLeaveID, CompOffDate);
    }


     public static String checkCompOffExists(int Empid,string CompOffDate)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        return objHolidayW.checkCompOffExists(Empid, CompOffDate);
    }

    public static void CreateCompof(int Empid, string CompOffDate,int Comment, int EntryBy)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        objHolidayW.CreateCompof(Empid, CompOffDate,Comment, EntryBy);
    }
    
   public static int GetLocationAcess(int ProfileId)
   {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        return objHolidayW.GetLocationAcess(ProfileId);
   }
   public static string GetLocationName(int LocationId)
   {
       HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
       return objHolidayW.GetLocationName(LocationId);
   }

   public static List<HolidayWorking> GetCompOffReportData(int empID, int LocationID)
    {
        HolidayWorkingDAL objHolidayW = new HolidayWorkingDAL();
        return objHolidayW.GetCompOffReportData(empID,LocationID);
    }
   
}