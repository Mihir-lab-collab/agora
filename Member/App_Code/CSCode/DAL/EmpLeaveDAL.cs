using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

/// <summary>
/// Summary description for EmpLeaveDAL
/// </summary>
public class EmpLeaveDAL
{
    DataSet ds = null;
    DataTable dt = null;
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public EmpLeaveDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public DataSet GetLeave(string mode, int empid, string leaveType, string lStatus, int year)//string leaveMonth )
    {
        SqlConnection con = new SqlConnection(_strConnection);

        SqlCommand cmd = new SqlCommand("SP_EmpLeave", con);
        // cmd.CommandTimeout = 0;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empid);
        cmd.Parameters.AddWithValue("@Status", lStatus);
        cmd.Parameters.AddWithValue("@Type", leaveType);
        //cmd.Parameters.AddWithValue("@Month", leaveMonth);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.CommandType = CommandType.StoredProcedure;
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        ds = new DataSet();
        da.Fill(ds);
        con.Close();
        return ds;
    }
    public DataSet SaveLeave(string mode, int empid, EmpLeaveBLL obj)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpLeave", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empid);
        cmd.Parameters.AddWithValue("@Status", "0");
        cmd.Parameters.AddWithValue("@Type", obj.LeaveType);
        cmd.Parameters.AddWithValue("@Month", "");
        cmd.Parameters.AddWithValue("@LeaveFrom", DateTime.ParseExact(obj.LeaveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@LeaveTo", DateTime.ParseExact(obj.LeaveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@LeaveDesc", obj.LeaveDesc);
        cmd.CommandType = CommandType.StoredProcedure;

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        con.Close();
        return ds;
    }

    public int DeleteLeave(string mode, int empLeaveID)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpLeave", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", 0);
        cmd.Parameters.AddWithValue("@Status", "0");
        cmd.Parameters.AddWithValue("@Type", "0");
        cmd.Parameters.AddWithValue("@Month", "");
        cmd.Parameters.AddWithValue("@LeaveFrom", "");
        cmd.Parameters.AddWithValue("@LeaveTo", "");
        cmd.Parameters.AddWithValue("@LeaveDesc", "");
        cmd.Parameters.AddWithValue("@EmpLeaveID", empLeaveID);
        cmd.CommandType = CommandType.StoredProcedure;
        int ireturn = cmd.ExecuteNonQuery();
        con.Close();
        return ireturn;
    }

    public bool IfExistsLeave(string mode, int empid, string leaveFrom, string leaveTo)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpLeave", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empid);
        cmd.Parameters.AddWithValue("@LeaveFrom", DateTime.ParseExact(leaveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@LeaveTo", DateTime.ParseExact(leaveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader dr;
        con.Open();
        dr = cmd.ExecuteReader();
        if (dr.HasRows)
            return true;
        else
            return false;
        con.Close();

    }

    public DataTable GetPmDetails(string mode, string empid , bool IsAdmin)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                SqlCommand cmd = new SqlCommand("Sp_GetpmDetails", con);
                // cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@Empid", empid);
                cmd.Parameters.AddWithValue("@isAdmin", IsAdmin);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
               // ds = new DataSet();
                da.Fill(dt);
                con.Close();
                return dt;
            }
        }
        catch (Exception ex)
        {
            return dt;
        }
    }

}