using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for projectReports
/// </summary>
public class projectReportsDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    public projectReportsDAL()
    {
       
    }

    public List<projectReports> getReports(int mode, int empid, DateTime FromDate, DateTime ToDate)
    {
        List<projectReports> pr = new List<projectReports>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_GetProjectReports1", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@empid", empid);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@FromDate", FromDate);
        cmd.Parameters.AddWithValue("@ToDate", ToDate);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pr.Add(new projectReports(
                    Convert.ToInt32(reader["projId"].ToString()),
                     reader["projName"].ToString() == "" ? "" : Convert.ToString(reader["projName"].ToString()),
                      reader["reportDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["reportDate"].ToString()),
                     reader["reportSubject"].ToString() == "" ? "" : Convert.ToString(reader["reportSubject"].ToString()),
                     reader["reportLastModified"].ToString() == "" ? "" : Convert.ToString(reader["reportLastModified"].ToString()),
                     Convert.ToInt32(reader["reportEmpId"].ToString()),
                     Convert.ToInt32(reader["reportId"].ToString()),
                     reader["reportDesc"].ToString() == "" ? "" : Convert.ToString(reader["reportDesc"].ToString()),
                     reader["empName"].ToString() == "" ? "" : Convert.ToString(reader["empName"].ToString())
                     )
                    );
            }
        }
        return pr;
    }

    public List<projectReports> getReportsData(int projid)
    {
        List<projectReports> pr = new List<projectReports>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_getReportData", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@projectid", projid);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pr.Add(new projectReports(
                     reader["custName"].ToString() == "" ? "" : Convert.ToString(reader["custName"].ToString()),
                     reader["custEmail"].ToString() == "" ? "" : Convert.ToString(reader["custEmail"].ToString())
                     )
                    );
            }
        }
        return pr;
    }

    public int InsertData(projectReports objdata)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_ManageReports", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProjectID", objdata.projID);
        cmd.Parameters.AddWithValue("@ReportTitle", objdata.reportTitle);
        cmd.Parameters.AddWithValue("@Description", objdata.Description);
        cmd.Parameters.AddWithValue("@ReportDate", objdata.reportDate);
        cmd.Parameters.AddWithValue("@ReportEmpId", objdata.ReportEmpID);
        cmd.Parameters.AddWithValue("@mode", objdata.mode1);
        int outputid = 0;
        using (con)
        {
            con.Open();
            outputid = Convert.ToInt32(cmd.ExecuteScalar());
        }
        return outputid;
    }


    public bool UpdateReports(projectReports objUpdate)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_ManageReports", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objUpdate.mode1);
        cmd.Parameters.AddWithValue("@ProjectID", objUpdate.projID);
        cmd.Parameters.AddWithValue("@ReportTitle", objUpdate.reportTitle);
        cmd.Parameters.AddWithValue("@Description", objUpdate.Description);
        cmd.Parameters.AddWithValue("@ReportDate", objUpdate.reportDate);
        cmd.Parameters.AddWithValue("@ReportEmpId", objUpdate.ReportEmpID);
        cmd.Parameters.AddWithValue("@reportId", objUpdate.reportId);

        try
        {
            using (con)
            {
                con.Open();
                int output = Convert.ToInt32(cmd.ExecuteNonQuery());
                if (output == 1)
                    updated = true;
                else
                    updated = false;
            }
        }
        catch (Exception ex)
        { }
        return updated;
    }

    public List<projectReports> GetReportAttachments(int reportId)
    {
        List<projectReports> getAttachment = new List<projectReports>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetReportAttachments", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@reportId", reportId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    getAttachment.Add(new projectReports(
                        Convert.ToInt32(reader["attachmentId"]),
                        Convert.ToInt32(reader["reportId"]),
                        reader["attachmentFile"].ToString(),
                        reader["attachmentDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["attachmentDate"].ToString())
                        ));
                }
            }
        }
        catch (Exception)
        { }
        return getAttachment;
    }

    public bool InsertAttachments(projectReports objAttachments)
    {
        bool inserted = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_InsertAttachments", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@reportId", objAttachments.reportId);
        cmd.Parameters.AddWithValue("@attachmentFile", objAttachments.attachmentFile);
        cmd.Parameters.AddWithValue("@attachmentDate", objAttachments.attachmentDate);
        try
        {
            using (con)
            {
                con.Open();
                int output = Convert.ToInt32(cmd.ExecuteScalar());
                if (output == 1)
                    inserted = true;
            }
        }
        catch (Exception)
        { }
        return inserted;
    }

    public DataTable bindProjectDropdown(int empid)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_GetProjects", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@EmpID", empid);

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);

        return dt;
    }

}