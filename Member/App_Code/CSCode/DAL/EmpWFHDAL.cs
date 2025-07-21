using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

/// <summary>
/// Summary description for EmpWFHDAL
/// </summary>
public class EmpWFHDAL
{
    DataSet ds = null;
    DataTable dt = null;
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public EmpWFHDAL()
    {

    }
    public DataSet GetWFHList(string mode, int empid, string lStatus, int year)//string leaveMonth )
    {
        SqlConnection con = new SqlConnection(_strConnection);

        SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
        // cmd.CommandTimeout = 0;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empid);
        cmd.Parameters.AddWithValue("@Status", lStatus);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.CommandType = CommandType.StoredProcedure;
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        ds = new DataSet();
        da.Fill(ds);
        con.Close();
        return ds;
    }
    public DataTable SaveWFH(string mode, int empid, EmpWFHBLL obj)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empid);
        cmd.Parameters.AddWithValue("@Status", "0");
        cmd.Parameters.AddWithValue("@Month", "");
        cmd.Parameters.AddWithValue("@WFHFrom", DateTime.ParseExact(obj.WFHFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@WFHTo", DateTime.ParseExact(obj.WFHTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@WFHDesc", obj.WFHDesc);
        cmd.Parameters.AddWithValue("@WFHCount", obj.WFHCount);
        cmd.CommandType = CommandType.StoredProcedure;

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public int DeleteWFH(string mode, int empWFHID)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", 0);
        cmd.Parameters.AddWithValue("@Status", "0");
        cmd.Parameters.AddWithValue("@Month", "");
        cmd.Parameters.AddWithValue("@WFHFrom", "");
        cmd.Parameters.AddWithValue("@WFHTo", "");
        cmd.Parameters.AddWithValue("@WFHDesc", "");
        cmd.Parameters.AddWithValue("@empWFHId", empWFHID);
        cmd.CommandType = CommandType.StoredProcedure;
        int ireturn = cmd.ExecuteNonQuery();
        con.Close();
        return ireturn;
    }

    public bool IfExistsWFH(string mode, int empid, string WFHFrom, string WFHTo)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empid);
        cmd.Parameters.AddWithValue("@WFHFrom", DateTime.ParseExact(WFHFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@WFHTo", DateTime.ParseExact(WFHTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
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
    public DataSet AppliedWFHFromTo(string mode, int empid, string From, string To)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        List<EmpWFHBLL> objEmpWFHBLL = new List<EmpWFHBLL>();
        SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@empid", empid);
        cmd.Parameters.AddWithValue("@WFHFrom", From);
        cmd.Parameters.AddWithValue("@WFHTo", To);
        cmd.CommandType = CommandType.StoredProcedure;
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        ds = new DataSet();
        da.Fill(ds);
        con.Close();
        return ds;
    }
    public int InsertWFHAttendance(string mode, int empId)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empId);
        cmd.CommandType = CommandType.StoredProcedure;
        int ireturn = cmd.ExecuteNonQuery();
        con.Close();
        return ireturn;
    }

    public int InsertRAAttendance(string mode, int empId)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpRemoteAttendance", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empId);
        cmd.CommandType = CommandType.StoredProcedure;
        int ireturn = cmd.ExecuteNonQuery();
        con.Close();
        return ireturn;
    }

    public int UpdateRAAttendance(string mode, int empId, DateTime attDate)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpRemoteAttendance", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empId);
        cmd.Parameters.AddWithValue("@attDate", attDate);
        cmd.CommandType = CommandType.StoredProcedure;
        int ireturn = cmd.ExecuteNonQuery();
        con.Close();
        return ireturn;
    }
    public int CheckAttendanceExistence(int empId, DateTime attDate)
    {
        int recordCount = 0; // Declare variable outside
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            using (SqlCommand cmd = new SqlCommand("SP_EmpRemoteAttendance", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "IsStartDayClicked");
                cmd.Parameters.AddWithValue("@EmpID", empId);
                cmd.Parameters.AddWithValue("@attDate", attDate);
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null) 
                {
                    int.TryParse(result.ToString(), out recordCount); 
                }
            }
        }

        return recordCount; 
    }




    public int UpdateWFHAttendance(string mode, int empId, DateTime attOutTime, DateTime attDate)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empId);
        cmd.Parameters.AddWithValue("@attOutTime", attOutTime);
        cmd.Parameters.AddWithValue("@attDate", attDate);
        cmd.CommandType = CommandType.StoredProcedure;
        int ireturn = cmd.ExecuteNonQuery();
        con.Close();
        return ireturn;
    }
    public bool CheckInTime(string mode, int empid)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empid);
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
    public DataTable BindWFHBalance(int empid)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
        cmd.Parameters.AddWithValue("@Mode", "WFHBalance");
        cmd.Parameters.AddWithValue("@EmpID", empid);
        cmd.CommandType = CommandType.StoredProcedure;
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }
    public DataTable GetLeaveStatus(int empid)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpRemoteAttendance", con);
        cmd.Parameters.AddWithValue("@Mode", "GetLeaveStatus");
        cmd.Parameters.AddWithValue("@EmpID", empid);
        cmd.CommandType = CommandType.StoredProcedure;
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }


}