using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for EmployeeAttendanceDAL
/// </summary>
public class AttendanceDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public AttendanceDAL()
    {
    }

    public DataSet GetRecordDs(int days, int months, int years, int locationID, int empId, DateTime startDate, DateTime endDate, string filter, string mode)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Attendance", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@days", days);
        cmd.Parameters.AddWithValue("@months", months);
        cmd.Parameters.AddWithValue("@years", years);
        cmd.Parameters.AddWithValue("@Location", locationID);
        cmd.Parameters.AddWithValue("@empid", empId);
        cmd.Parameters.AddWithValue("@StartDate", startDate);
        cmd.Parameters.AddWithValue("@EndDate", endDate);
        cmd.Parameters.AddWithValue("@Filter", filter);
        cmd.Parameters.AddWithValue("@Mode", mode);
        DataSet Ds = new DataSet();

        SqlDataAdapter dataAdapt = new SqlDataAdapter();
        dataAdapt.SelectCommand = cmd;
        dataAdapt.Fill(Ds);
        return Ds;
    }

    public double GetValue(int days, int months, int years, int locationID, int empId, DateTime startDate, DateTime endDate, string filter, string mode)
    {
        double wkHours = 0.0;
        object objHrs;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Attendance", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@days", days);
        cmd.Parameters.AddWithValue("@months", months);
        cmd.Parameters.AddWithValue("@years", years);
        cmd.Parameters.AddWithValue("@Location", locationID);
        cmd.Parameters.AddWithValue("@empid", empId);
        cmd.Parameters.AddWithValue("@StartDate", startDate);
        cmd.Parameters.AddWithValue("@EndDate", endDate);
        cmd.Parameters.AddWithValue("@Filter", filter);
        cmd.Parameters.AddWithValue("@Mode", mode);
        try
        {
            using (con)
            {
                objHrs = cmd.ExecuteScalar();
                wkHours = Convert.ToDouble(objHrs);
                con.Close();
            }
        }
        catch (Exception ex)
        { }
        return wkHours;
    }



    public int getlocationFkEmpId(int empId, string mode)
    {
        int locationfk = 0;
        object objlocationFkId;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Attendance", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@empid", empId);
        cmd.Parameters.AddWithValue("@Mode", mode);
        try
        {
            using (con)
            {
                objlocationFkId = cmd.ExecuteScalar();

                locationfk = Convert.ToInt32(objlocationFkId);
                con.Close();
            }
        }
        catch (Exception ex)
        { }
        finally
        {
            if (con.State == ConnectionState.Open)
            { con.Close(); }

            con.Dispose();
        }
        return locationfk;
    }

    public static List<EmpAttLog> getEmpAttLog(int empid, DateTime FromDate, string Mode)
    {
        string _strConn = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        List<EmpAttLog> pr = new List<EmpAttLog>();
        SqlConnection con = new SqlConnection(_strConn);
        SqlCommand cmd = new SqlCommand("SP_Attendance", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@days", 0);
        cmd.Parameters.AddWithValue("@months", 0);
        cmd.Parameters.AddWithValue("@years", 0);
        cmd.Parameters.AddWithValue("@Location", 0);
        cmd.Parameters.AddWithValue("@empid", empid);
        cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(FromDate));
        cmd.Parameters.AddWithValue("@EndDate", DateTime.Now);
        cmd.Parameters.AddWithValue("@Mode", Mode);

        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pr.Add(new EmpAttLog(Convert.ToInt32(reader["EmpID"].ToString()),
                     reader["PunchTime"].ToString() == "" ? "" : Convert.ToString(reader["PunchTime"].ToString()),
                      reader["IP"].ToString() == "" ? "" : Convert.ToString(reader["IP"].ToString()),
                       reader["Mode"].ToString() == "" ? "" : Convert.ToString(reader["Mode"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? "" : Convert.ToString(reader["InsertedOn"].ToString()))
                    );
            }
        }
        return pr;
    }

    public void AddUpdateEmpAtt(int empId, DateTime attDate, string attStatus, DateTime attInTime, DateTime attOutTime, string attComment, string attIP, int adminID, string mode)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_UpdateAttendance", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@empId", empId);
        cmd.Parameters.AddWithValue("@attDate", attDate);
        cmd.Parameters.AddWithValue("@attStatus", attStatus);
        cmd.Parameters.AddWithValue("@attInTime", attInTime);
        cmd.Parameters.AddWithValue("@attOutTime", attOutTime);
        cmd.Parameters.AddWithValue("@attComment", attComment);
        cmd.Parameters.AddWithValue("@attIP", attIP);
        cmd.Parameters.AddWithValue("@adminID", adminID);
        cmd.Parameters.AddWithValue("@mode", mode);
        try
        {
            using (con)
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        { }
    }

    public DataTable getEmpStatus()
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_GetEmpStatus", con);
        cmd.CommandType = CommandType.StoredProcedure;
        DataSet Ds = new DataSet();
        SqlDataAdapter dataAdapt = new SqlDataAdapter();
        dataAdapt.SelectCommand = cmd;
        dataAdapt.Fill(Ds);
        return Ds.Tables[0];

    }

    public static List<EmpAttLog> GetCalendarAtts(string mode, int empid, string startDate, string endDate)
    {
        string _strConn = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        List<EmpAttLog> attribute = new List<EmpAttLog>();
        SqlConnection con = new SqlConnection(_strConn);
        SqlCommand cmd = new SqlCommand("[SP_Attendance]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@empid", empid);
        cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(startDate));
        cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(endDate));
        cmd.Parameters.AddWithValue("@Mode", mode);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                attribute.Add(
                new EmpAttLog(
                Convert.ToDateTime(Dr["attDate"].ToString()),
                Convert.ToString(Dr["attInTime"].ToString()),
                Convert.ToString(Dr["attOutTime"].ToString()),
                Convert.ToString(Dr["attStatus"].ToString()),
                Convert.ToString(Dr["timesheethours"].ToString()),////For mobile team
                Convert.ToString(Dr["workinghours"].ToString()), ///For mobile team
                Convert.ToString(Dr["brktimehours"].ToString()),
                Convert.ToString(Dr["attComment"].ToString())
                   ));
            }
        }
        return attribute;
    }

    public void AddUpdateEmpAtt(Attendance objData)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Attendance", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@empId", objData.empId);
        cmd.Parameters.AddWithValue("@attributeDate", objData.attDate);
        cmd.Parameters.AddWithValue("@attStatus", objData.attStatus);
        cmd.Parameters.AddWithValue("@InTime", objData.attInTime);
        cmd.Parameters.AddWithValue("@OutTime", objData.attOutTime);
        cmd.Parameters.AddWithValue("@attComment", objData.attComment);
        cmd.Parameters.AddWithValue("@attributeIP", objData.attIP);
        cmd.Parameters.AddWithValue("@adminID", objData.adminID);
        cmd.Parameters.AddWithValue("@mode", objData.mode);
        try
        {
            using (con)
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        { }
    }

    public int CheckExistsEmp(string mode, int empid, string attDate)
    {

        int id = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("[SP_Attendance]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@empid", empid);
        cmd.Parameters.AddWithValue("@attributeDate", Convert.ToDateTime(attDate));
        cmd.Parameters.AddWithValue("@Mode", mode);

        using (con)
        {
            id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }

        return id;
    }
}