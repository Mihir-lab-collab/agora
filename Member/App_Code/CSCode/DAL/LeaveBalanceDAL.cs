using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for LeaveBalanceDAL
/// </summary>
public class LeaveBalanceDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public LeaveBalanceDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public List<LeaveBalanceBLL> GetLeaveBalance(string mode)
    {
        List<LeaveBalanceBLL> objLeaveBalanceBLL = new List<LeaveBalanceBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_GenerateLeaveSummary", con);
        cmd.Parameters.AddWithValue("@Mode", mode);        
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                objLeaveBalanceBLL.Add(new LeaveBalanceBLL(
                     reader["EmpID"].ToString() == "" ? 0 : Convert.ToInt32(reader["EmpID"].ToString()),
                     reader["EmpName"].ToString() == "" ? "" : Convert.ToString(reader["EmpName"].ToString()),
                     reader["LeaveSummary"].ToString() == "" ? "" : Convert.ToString(reader["LeaveSummary"].ToString())

                    ));
            }
        }
        return objLeaveBalanceBLL;
    }
    public DataTable GetLeaveDetails(int empId, string mode)
    {
        DataTable dt = new DataTable();

        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            using (SqlCommand cmd = new SqlCommand("sp_GenerateLeaveSummary", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", empId);
                cmd.Parameters.AddWithValue("@Mode", mode);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }

}