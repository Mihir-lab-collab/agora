using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Report
/// </summary>
public class Report
{
    public int reportId { get; set; }
    public string name { get; set; }
   // public int numberOfField { get; set; }
    public string field1 { get; set; }
    public string field2 { get; set; }
    public string field3 { get; set; }
    public string field4 { get; set; }
    public string field5 { get; set; }


    public string query { get; set; }
 
    public int insertedBy { get; set; }
    public string insertedIP { get; set; }
    public string mode { get; set; }
    public string insertedOn { get; set; }
    public string Description { get; set; }
    public string MgmtReport { get; set; }
    public string chartType { get; set; }

    public Report(int reportId, string name, int numberOfField, string field1, string field2, string field3, string field4, string field5, string query, string insertedOn, string Description, string MgmtReport, string chartType)
 // int numberOfField,
    {
        this.reportId = reportId;
        this.name = name;
       // this.numberOfField = numberOfField;
        this.field1 = field1;
        this.field2 = field2;
        this.field3 = field3;
        this.field4 = field4;
        this.field5 = field5;
        this.query = query;
        // this.showChart = showChart;
        this.insertedOn = insertedOn;
        this.Description = Description;
        this.MgmtReport = MgmtReport;
        this.chartType = chartType;
      
    }
    public static List<Report> GetReportList(string mode, int reportId)
    {
        ReportDAL objReportDAL = new ReportDAL();
        return objReportDAL.GetReportList(mode, reportId);
    }

    public static string GetRecord(string query)
    {
        ReportDAL objreportDAL = new ReportDAL();
        return objreportDAL.GetRecordDs(query);
    }

    public static DataTable GetRecordDT(string query)
    {
        ReportDAL objreportDAL = new ReportDAL();
        return objreportDAL.GetRecordInTable(query);
    }


    public static int DeleteReport(string mode, int reportId)
    {
        ReportDAL objreportDAL = new ReportDAL();
        return objreportDAL.DeleteReport(mode, reportId);
    }

    public static int SaveReport(int reportId, string name, string field1, string field2, string field3, string field4, string field5, string query, int insertedBy, string insertedIP, string mode, string Description, string MgmtReport, string chartType)
    // int numberOfField, string type1, string type2, string type3, string type4, string type5, string data1, string data2, string data3, string data4, string data5,
    {
        Report objreport = new Report();
        objreport.reportId = reportId;
        objreport.name = name;
       // objreport.numberOfField = numberOfField;

        objreport.field1 = field1;
        objreport.field2 = field2;
        objreport.field3 = field3;
        objreport.field4 = field4;
        objreport.field5 = field5;

    
        objreport.query = query;
        //objreport.showChart = showChart;
        objreport.insertedBy = insertedBy;
        objreport.insertedIP = insertedIP;
        objreport.mode = mode;
        objreport.Description = Description;
        objreport.MgmtReport = MgmtReport;
        objreport.chartType = chartType;
        ReportDAL objreportDAL = new ReportDAL();
        return objreportDAL.SaveReport(mode, objreport);
    }

    public Report()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static List<clsKeyValuePair> GetRecordsForDD(string query)
    {
       
        List<clsKeyValuePair> list = new List<clsKeyValuePair>();

        ReportDAL objreportDAL = new ReportDAL();
        DataTable dt = objreportDAL.GetRecordInTable(query);
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Columns.Count > 1)
            {
                foreach (DataRow item in dt.Rows)
                {
                    clsKeyValuePair obj = new clsKeyValuePair();
                    obj.ID = Convert.ToString(item[0]);
                    obj.Name = Convert.ToString(item[1]);
                    //obj.Name = Convert.ToString(item["Name"]);
                    list.Add(obj);
                }
            }
            else
            {
                foreach (DataRow item in dt.Rows)
                {
                    clsKeyValuePair obj = new clsKeyValuePair();
                    obj.ID = Convert.ToString(item[0]);
                    obj.Name = Convert.ToString(item[0]);
                    list.Add(obj);
                }
            }
        }
        return list;
    }
}

public class clsKeyValuePair
{
    public string ID { get; set; }
    public string Name { get; set; }
}