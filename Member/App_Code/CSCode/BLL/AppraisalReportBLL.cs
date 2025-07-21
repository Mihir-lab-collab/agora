using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

/// <summary>
/// Summary description for AppraisalReportBLL
/// </summary>
/// 

public class AppraisalReportBLL
{  
    public AppraisalReportBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public AppraisalReportBLL(int _Id, string _ProjName, int _ProjectMemberId, string _empName, int _AppraisalTransactionId, int _ManagerValue)
    {
        this.Id = _Id;
        this.ProjName = _ProjName;
        this.ProjectMemberId = _ProjectMemberId;
        this.empName = _empName;
        this.AppraisalTransactionId = _AppraisalTransactionId;
        this.ManagerValue = _ManagerValue;

    }

    public AppraisalReportBLL(int _Id, string _ProjName, int _projectmemberId, string _empName)
    {
        this.Id = _Id;
        this.ProjName = _ProjName;
        this.ProjectMemberId = _projectmemberId;
        this.empName = _empName;
    }

    public AppraisalReportBLL(int _projectmemberId, string _empName)
    {
        this.ProjectMemberId = _projectmemberId;
        this.empName = _empName;
    }

    public AppraisalReportBLL(int _Id, string _ProjName, string _empName, string _Status, string _Quarter)
    {
        this.Id = _Id;
        this.ProjName = _ProjName;
        this.empName = _empName;
        this.Status = _Status;
        this.Quarter = _Quarter;
    }

    public AppraisalReportBLL(int _EmployeeAppraisalId, int _Id, int _empid, int _StatusId, string _ProjName, string _empName, string _Status, string _Quarter, int _projId, string _ModifiedOn, string _EmpSelfAppraisaDate, string _EmpFinalAppraiseDate, DateTime _QuarterStartDate, int _projectmemberId,string _ReportingManager)
    {
        this.EmployeeAppraisalId = _EmployeeAppraisalId;
        this.Id = _Id;
        this.empid = _empid;
        this.StatusId = _StatusId;
        this.ProjName = _ProjName;
        this.empName = _empName;
        this.Status = _Status;
        this.Quarter = _Quarter;
        this.projId = _projId;
        this.ModifiedOn = _ModifiedOn;
        this.EmpSelfAppraisaDate = _EmpSelfAppraisaDate;
        this.EmpFinalAppraiseDate = _EmpFinalAppraiseDate;
        this.QuarterStartDate = _QuarterStartDate;
        this.ProjectMemberId = _projectmemberId;
        this.ReportingManager = _ReportingManager;
    }

    public AppraisalReportBLL(string _Quarter)
    {
        this.Quarter = _Quarter;
    }

    public AppraisalReportBLL(int _Id, int _projId, string _ProjName)
    {
        this.Id = _Id;
        this.projId = _projId;
        this.ProjName = _ProjName;
        //  this.Quarter = _Quarter;
    }

    //public AppraisalReportBLL(int _Id, string _ProjName)
    //{
    //    this.Id = _Id;
    //    this.ProjName = _ProjName;
    //}


    public int Id { get; set; }

    public int KRAID { get; set; }

    public int Value { get; set; }

    public int StatusId { get; set; }

    public int empid { get; set; }

    public int ProjectCounts { get; set; }

    public int AppraisalId { get; set; }

    public DateTime InsertedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public DateTime InsertedOn { get; set; }

    public string ModifiedOn { get; set; }

    public string EmpSelfAppraisaDate { get; set; }

    public string EmpFinalAppraiseDate { get; set; }

    public string CURRQuarterStartDate { get; set; }

    public string QuarterStartDates { get; set; }

    public DateTime QuarterStartDate { get; set; }

    public string SelfAppQuarterDate { get; set; }

    public string KRANames { get; set; }

    public int AppraisalTransactionId { get; set; }

    public int ManagerValue { get; set; }

    public int EmployeeAppraisalId { get; set; }

    public int ProfileId { get; set; }

    public int ProjectMemberId { get; set; }

    public string Name { get; set; }

    public string Status { get; set; }

    public string Quarter { get; set; }

    //public string projName { get; set; }

    public int projId { get; set; }

    public string Comments { get; set; }

    public int AppraiseBy { get; set; }

    public string ProjName { get; set; }

    public string AuthorityName { get; set; }

    public string empName { get; set; }

    public int ProjectId { get; set; }

    public int EmployeeRatings { get; set; }

    public int ManagerRatings { get; set; }

    public int EmpAprId { get; set; }

    public DateTime TransQuarterStartDate { get; set; }

    public string ReportingManager { get; set; }
    //public string QuarterStartDate { get; set; }
    //public string InsertedOn { get; set; }

    public DataSet CheckEmpAppraisal(int empid, string employee)
    {
        AppraisalReportDAL objappraisalDAL = new AppraisalReportDAL();
        return objappraisalDAL.CheckEmpAppraisal(empid, employee);
    }


    public static List<AppraisalReportBLL> GetManagerApprsial(int aprId)
    {
        AppraisalReportDAL objappraisalDAL = new AppraisalReportDAL();
        return objappraisalDAL.GetManagerAppraisal(aprId);
    }

    public static List<AppraisalReportBLL> GetQuarter(int Counter)
    {
        AppraisalReportDAL objqtrApr = new AppraisalReportDAL();

        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        List<AppraisalReportBLL> emp = objqtrApr.GetQuarter(Counter);

        DataTable table = conveter.ToDataTable(emp);

        return emp;

    }

    public static List<AppraisalReportBLL> GetAppraisalData(int empId, string Mode, int Counter)
    {
        AppraisalReportDAL objempApr = new AppraisalReportDAL();

        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        List<AppraisalReportBLL> emp = objempApr.GetAppraisalData(empId, Mode, Counter);

        DataTable table = conveter.ToDataTable(emp);

        return emp;
    }


    public static int InsertAllTransaction(string Mode, int AppraisalID, int ProjectId, int EmpId, int InitiatedBy, string ProfileId, string KRAId, DateTime QuarterDate)
    {
        AppraisalReportDAL objempApr = new AppraisalReportDAL();

        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        //List<AppraisalReportBLL> emp = objempApr.InsertAllTransaction(Mode, AppraisalID, ProjectId, EmpId, InitiatedBy, ProfileId, KRAId, QuarterDate);
        int test = objempApr.InsertAllTransaction(Mode, AppraisalID, ProjectId, EmpId, InitiatedBy, ProfileId, KRAId, QuarterDate);

        //DataTable table = conveter.ToDataTable(emp);

        return test;

    }

    public static List<AppraisalReportBLL> GetEmpAppraisalReport(int empid, int projId, DateTime CurrentQuarterDate)
    {
        AppraisalReportDAL objempApr = new AppraisalReportDAL();

        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        List<AppraisalReportBLL> emp = objempApr.GetEmpAppraisalReport(empid, projId, CurrentQuarterDate);

        DataTable table = conveter.ToDataTable(emp);

        return emp;
    }

    public static List<AppraisalReportBLL> GetReportingManager(string mode,int projId)
    {
        AppraisalReportDAL objempApr = new AppraisalReportDAL();

        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        List<AppraisalReportBLL> emp = objempApr.GetReportingManager(mode, projId);

        DataTable table = conveter.ToDataTable(emp);

        return emp;
    }

    public void SaveEmpMgrApr(List<AppraisalReportBLL> GridData, string Comments, int AppraiseById)
    {
        AppraisalReportDAL objappraisalDAL = new AppraisalReportDAL();
        try
        {
            objappraisalDAL.SaveEmpManagerReview(GridData, Comments, AppraiseById);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public static List<AppraisalReportBLL> GetSelfAppraisal(string SubMode, int EmpId, int ProjId, DateTime SelfAppraiseDate)
    {
        AppraisalReportDAL objappraisalDAL = new AppraisalReportDAL();
        return objappraisalDAL.GetSelfpAppraisal(SubMode, EmpId, ProjId, SelfAppraiseDate);
    }


    public void Saveempappr(List<AppraisalReportBLL> GridData, int ProjectId)
    {
        AppraisalReportDAL objappraisalDAL = new AppraisalReportDAL();
        try
        {
            objappraisalDAL.SaveEmpApprReview(GridData, ProjectId);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public static List<AppraisalReportBLL> GetAllProjects(int empId)
    {
        AppraisalReportDAL objempApr = new AppraisalReportDAL();
        List<AppraisalReportBLL> emp = objempApr.GetAllProjects(empId);
        return emp;

    }

    public static List<AppraisalReportBLL> GetEmpSelfAppraisalReport(int empid, int projId, int AppraiseBy, DateTime CurrentQuarterDate)
    {
        AppraisalReportDAL objempApr = new AppraisalReportDAL();

        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        List<AppraisalReportBLL> emp = objempApr.GetEmpSelfAppraisalReport(empid, projId, AppraiseBy, CurrentQuarterDate);

        DataTable table = conveter.ToDataTable(emp);

        return emp;
    }

    public static List<AppraisalReportBLL> GetFinalAppraisalReport(string Mode, int Counter)
    {
        AppraisalReportDAL objAprRpt = new AppraisalReportDAL();
        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        List<AppraisalReportBLL> emp = objAprRpt.GetFinalAppraisalReport(Mode, Counter);

        DataTable table = conveter.ToDataTable(emp);

        return emp;
    }

    public static List<AppraisalReportBLL> GetEmpolyeeAppraisalProject(int EmpId, int Counter)
    {
        AppraisalReportDAL objAprRpt = new AppraisalReportDAL();
        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        List<AppraisalReportBLL> emp = objAprRpt.GetEmployeeAppraisalProjects(EmpId, Counter);

        DataTable table = conveter.ToDataTable(emp);
        return emp;
    }

    public static List<AppraisalReportBLL> GetManagerComments(int EmpId, int ProjectId)
    {
        AppraisalReportDAL objAprRpt = new AppraisalReportDAL();
        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        List<AppraisalReportBLL> emp = objAprRpt.GetManagerComments(EmpId, ProjectId);

        DataTable table = conveter.ToDataTable(emp);
        return emp;
    }

    public static DataTable GetAppraisalManagerRatingsReport(int EmpId, int ProjectId, int Counter)
    {
        AppraisalReportDAL objAprRpt = new AppraisalReportDAL();
        ListtoDataTableConverter conveter = new ListtoDataTableConverter();
        var dt = objAprRpt.GetAppraisalManagerRatingsReport(EmpId,ProjectId, Counter);
        return dt;
    }

}

public class ListtoDataTableConverter
{
    public DataTable ToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);
        //Get all the properties
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Setting column names as Property names
            dataTable.Columns.Add(prop.Name);
        }
        foreach (T item in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                //inserting property values to datatable rows
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }
        //put a breakpoint here and check datatable
        return dataTable;
    }
}