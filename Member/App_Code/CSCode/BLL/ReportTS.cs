using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ReportTS
/// </summary>
public class ReportTS
{
    public int EmpId { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    public string TimeAvailable { get; set; }
    public string TimeReported { get; set; }


	public ReportTS()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public ReportTS(int EmpId, string Name, string Date, string TimeAvailable, string TimeReported)
    {
        this.EmpId = EmpId;
        this.Name = Name;
        this.Date = Date;
        this.TimeAvailable = TimeAvailable;
        this.TimeReported = TimeReported;
    }

    public static List<ReportTS> GetReportTS(int empid, DateTime FromDate, int day, int LocationID)
    {
        ReportTSDAL objreport = new ReportTSDAL();
        return objreport.getReports(empid, FromDate, day, LocationID);
                
    }
}