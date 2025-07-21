using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

public class ReportDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    public string GetRecordDs(string query)
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
        }
    }

    public DataTable GetRecordInTable(string query)
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }
        return dt;
    }

    public List<Report> GetReportList(string mode, int reportId)
    {
        List<Report> lstReport = new List<Report>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Report", con);
        cmd.Parameters.AddWithValue("@reportId", reportId);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Report obj = new Report();
                obj.reportId = Convert.ToInt32(reader["reportId"]);
                obj.name = Convert.ToString(reader["name"]);
              //  obj.numberOfField = Convert.ToInt32(reader["numberOfField"]);
                obj.query = Convert.ToString(reader["query"]);
                obj.insertedOn = Convert.ToString(reader["insertedOn"]);
                obj.Description = Convert.ToString(reader["Description"]);
                obj.MgmtReport = Convert.ToString(reader["ManagementReport"]);
                obj.field1 = Convert.ToString(reader["field1"]);
                obj.field2 = Convert.ToString(reader["field2"]);
                obj.field3 = Convert.ToString(reader["field3"]);
                obj.field4 = Convert.ToString(reader["field4"]);
                obj.field5 = Convert.ToString(reader["field5"]);
                obj.chartType = Convert.ToString(reader["chartType"]);
                lstReport.Add(obj);
            }
        }
        return lstReport;
    }

    public int DeleteReport(string mode, int reportId)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Report", con);
        cmd.Parameters.AddWithValue("@reportId", reportId);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.CommandType = CommandType.StoredProcedure;
        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception ex)
        { }
        return outputid;
    }

    public int SaveReport(string mode, Report objReport)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();

        SqlCommand cmd = new SqlCommand("SP_Report", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@reportId", objReport.reportId);
        cmd.Parameters.AddWithValue("@name", objReport.name);
       // cmd.Parameters.AddWithValue("@numberOfField", objReport.numberOfField);
        cmd.Parameters.AddWithValue("@field1", objReport.field1);
        cmd.Parameters.AddWithValue("@field2", objReport.field2);
        cmd.Parameters.AddWithValue("@field3", objReport.field3);
        cmd.Parameters.AddWithValue("@field4", objReport.field4);
        cmd.Parameters.AddWithValue("@field5", objReport.field5);
        cmd.Parameters.AddWithValue("@query", objReport.query);
        cmd.Parameters.AddWithValue("@insertedBy", objReport.insertedBy);
        cmd.Parameters.AddWithValue("@insertedIP", objReport.insertedIP);
        cmd.Parameters.AddWithValue("@mode", objReport.mode);
        cmd.Parameters.AddWithValue("@Description", objReport.Description);
        cmd.Parameters.AddWithValue("@MgmtReport", objReport.MgmtReport);
        cmd.Parameters.AddWithValue("@chartType", objReport.chartType);

        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception ex)
        { }
        return outputid;
    }

    public ReportDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}