using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for ReportTSDAL
/// </summary>
public class ReportTSDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
	public ReportTSDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public List<ReportTS> getReports(int empid, DateTime FromDate, int Day, int LocationID)
    {
        List<ReportTS> pr = new List<ReportTS>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_reportTS", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ID", empid);
        cmd.Parameters.AddWithValue("@Days", Day);
        cmd.Parameters.AddWithValue("@FromDate", FromDate);
        cmd.Parameters.AddWithValue("@LocationID", LocationID);
       
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pr.Add(new ReportTS(
                    Convert.ToInt32(reader["EmpID"].ToString()),
                     reader["EMPName"].ToString() == "" ? "" : Convert.ToString(reader["EMPName"].ToString()),
                      reader["Date"].ToString() == "" ? "" : Convert.ToString(reader["Date"].ToString()),
                       reader["TimeAvailable"].ToString() == "" ? "" : Convert.ToString(reader["TimeAvailable"].ToString()),
                        reader["TimeReported"].ToString() == "" ? "" : Convert.ToString(reader["TimeReported"].ToString()))
                    );
            }
        }
        return pr;
    }
}