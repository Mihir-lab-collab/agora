using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for projectReports
/// </summary>
public class projectReports
{

    public int mode { get; set; }
    public string mode1 { get; set; }
    public int projID { get; set; }
    public string projectName { get; set; }
    public string reportTitle { get; set; }
    public string lastModified { get; set; }
    public string ReportedBy { get; set; }
    public DateTime reportDate { get; set; }
    public string custName { get; set; }
    public string Description { get; set; }
    public int ReportEmpID { get; set; }
    public string custEmail { get; set; }
    public int reportId { get; set; }
    public int attachmentId { get; set; }
    public string attachmentFile { get; set; }
    public DateTime attachmentDate { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public projectReports()
    {
       
    }

    public projectReports(int projID, string projectname, DateTime reportdate, string reportTitle, string lastmodified, int reportEmpId, int reportId, string reportDescription, string reportedby)
    {
        this.projID = projID;
        this.projectName = projectname;
        this.reportDate = reportdate;
        this.reportTitle = reportTitle;
        this.lastModified = lastmodified;
        this.ReportEmpID = reportEmpId;
        this.reportId = reportId;
        this.Description = reportDescription;
        this.ReportedBy = reportedby;
    }

    public projectReports(string custName, string custEmail)
    {
        this.custName = custName;
        this.custEmail = custEmail;
    }

    public projectReports(int attachmentId, int reportId, string attachmentFile, DateTime attachmentDate)
    {
        this.attachmentId = attachmentId;
        this.reportId = reportId;
        this.attachmentFile = attachmentFile;
        this.attachmentDate = attachmentDate;
    }

    public static List<projectReports> getProjectReports(int mode, int empid, DateTime FromDate, DateTime ToDate)
    {
        projectReportsDAL objreport = new projectReportsDAL();
        return objreport.getReports(mode, empid, FromDate, ToDate);
    }


    public static List<projectReports> getReportsData(int projid)
    {
        projectReportsDAL objreport = new projectReportsDAL();
        return objreport.getReportsData(projid);
    }

    public static DataTable bindProjectDropdown(int empid)
    {
        projectReportsDAL objreport = new projectReportsDAL();
        return objreport.bindProjectDropdown(empid);
    }

    public int Insertdata(int ProjectID, string ReportTitle, string Description, DateTime ReportDate, int ReportEmpId, string mode)
    {
        projectReports objInsert = new projectReports();
        objInsert.projID = ProjectID;
        objInsert.reportTitle = ReportTitle;
        objInsert.Description = Description;
        objInsert.reportDate = ReportDate;
        objInsert.ReportEmpID = ReportEmpId;
        objInsert.mode1 = mode;
        projectReportsDAL objInsertInto = new projectReportsDAL();
        return objInsertInto.InsertData(objInsert);
    }

    public static bool updateReports(string mode, int projId, string ReportTitle, string Description, DateTime ReportDate, int ReportEmpId, string lastmodified, int reportId)
    {
        projectReports objInsert = new projectReports();
        objInsert.mode1 = mode;
        objInsert.projID = projId;
        objInsert.reportTitle = ReportTitle;
        objInsert.Description = Description;
        objInsert.reportDate = ReportDate;
        objInsert.ReportEmpID = ReportEmpId;
        objInsert.lastModified = lastmodified;
        objInsert.reportId = reportId;
        projectReportsDAL objInsertInto = new projectReportsDAL();
        return objInsertInto.UpdateReports(objInsert);
    }

    public static List<projectReports> GetReportAttachments(int reportId)
    {
        projectReportsDAL objAttachments = new projectReportsDAL();
        return objAttachments.GetReportAttachments(reportId);
    }

    public static bool InsertAttachments(int reportId, string attachmentFile)
    {
        projectReports Attachments = new projectReports();
        Attachments.reportId = reportId;
        Attachments.attachmentFile = attachmentFile;
        Attachments.attachmentDate = DateTime.Now;
        projectReportsDAL objAttachments = new projectReportsDAL();
        return objAttachments.InsertAttachments(Attachments);
    }

}