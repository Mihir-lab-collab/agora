using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Web.Configuration;

/// <summary>
/// Summary description for EmpLeaveApproval
/// </summary>
public class EmpLeaveApprovalDAL
{
    public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    DataTable dt = null;
    public EmpLeaveApprovalDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataTable GetEmpLeaves(string mode, EmpLeaveApprovalBLL objBLL, int loginID, int includeArchive)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpLeaveApproval", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@LoginID", loginID);
        cmd.Parameters.AddWithValue("@LocationID", objBLL.LocationID);
        cmd.Parameters.AddWithValue("@Name", objBLL.EmpName);
        cmd.Parameters.AddWithValue("@Status", objBLL.LeaveStatus);
        cmd.Parameters.AddWithValue("@StartDate", objBLL.LeaveFrom);
        cmd.Parameters.AddWithValue("@EndDate", objBLL.LeaveTo);
        cmd.Parameters.AddWithValue("@isArchive", includeArchive);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    public DataTable GtLeaveType(string mode)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpLeaveApproval", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    public string GetProfile(string mode, string empID)
    {
        string sReturn = "";
        SqlConnection con = new SqlConnection(_strConnection);
       // con.Open();
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmployeeID", Convert.ToInt32(empID)); 
        try
        {
            using (con)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    if(dt.Rows[0]["Result"].ToString()=="1")
                    {
                        sReturn = "True";
                    }
                    else
                    {
                        sReturn = "False";
                    }
                }
               // sReturn= "True";

              // sReturn = cmd.ExecuteNonQuery().ToString();
               // int output1 = cmd.ExecuteNonQuery();
               //int output = Convert.ToInt32(cmd.ExecuteNonQuery());
               //if (output == 1)
               //    sReturn = "True";
               //else
               //    sReturn = "False";
               //con.Close();
            }
        }
        catch (Exception ex)
        { }
        return sReturn;
    }

    public string UpdateEmpLeaveStatus(string mode, string ip, EmpLeaveApprovalBLL objBLL)
    {
        
        string sReturn = "";
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpLeaveApproval", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", objBLL.EmpID);
        cmd.Parameters.AddWithValue("@EmpLeaveID", objBLL.EmpLeaveID);
        cmd.Parameters.AddWithValue("@StartDate",objBLL.LeaveFrom); //, DateTime.ParseExact(objBLL.LeaveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@EndDate", objBLL.LeaveTo);    // DateTime.ParseExact(objBLL.LeaveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@Status", objBLL.LeaveStatus);
        cmd.Parameters.AddWithValue("@LeaveType", objBLL.LeaveType);
        cmd.Parameters.AddWithValue("@leaveComment", objBLL.AdminComment);
        cmd.Parameters.AddWithValue("@leaveSanctionBy", objBLL.LeaveSanctionBy);
        cmd.Parameters.AddWithValue("@ip", ip);
        cmd.Parameters.AddWithValue("@LoginID", Convert.ToInt32(objBLL.LeaveSanctionBy));
        try
        {
            using (con)
            {
                sReturn = cmd.ExecuteNonQuery().ToString();
                con.Close();
            }
        }
        catch (Exception ex)
        { }
        return sReturn;
    }
}