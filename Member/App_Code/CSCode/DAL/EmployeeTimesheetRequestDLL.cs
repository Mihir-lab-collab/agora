using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Web.Configuration;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for EmployeeTimesheetRequestDLL
/// </summary>
public class EmployeeTimesheetRequestDLL
{

    public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    DataTable dt = null;
    public EmployeeTimesheetRequestDLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    // Code by AP starts

    public int Save(EmployeeTimesheetRequestBLL objEmployeeTimesheetRequest)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("Sp_EmployeeTimeSheetRequest", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "SAVE");
        cmd.Parameters.AddWithValue("@EmpId", Convert.ToInt32(objEmployeeTimesheetRequest.EmpId));
        cmd.Parameters.AddWithValue("@RequestDate", objEmployeeTimesheetRequest.Requestdate);
        cmd.Parameters.AddWithValue("@Descripation", objEmployeeTimesheetRequest.Descripation);
        cmd.Parameters.AddWithValue("@InsertedBy", objEmployeeTimesheetRequest.InsertedBy);

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

    //Code by AP ends





    //public int Save(EmployeeTimesheetRequestBLL objEmployeeTimesheetRequest)
    //{
    //    int outputid = 0;
    //    SqlConnection con = new SqlConnection(_strConnection);
    //    con.Open();
    //    SqlCommand cmd = new SqlCommand("Sp_EmployeeTimeSheetRequest", con);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    cmd.Parameters.AddWithValue("@Mode","SAVE");
    //    cmd.Parameters.AddWithValue("@EmpId", objEmployeeTimesheetRequest.EmpId);
    //    cmd.Parameters.AddWithValue("@RequestDate", objEmployeeTimesheetRequest.Requestdate);
    //    cmd.Parameters.AddWithValue("@Descripation", objEmployeeTimesheetRequest.Descripation);
    //    cmd.Parameters.AddWithValue("@InsertedBy", objEmployeeTimesheetRequest.InsertedBy);
    //   // cmd.Parameters.AddWithValue("@Time", objCIPBLL.Time);
    //    try
    //    {
    //        using (con)
    //        {
    //            outputid = Convert.ToInt32(cmd.ExecuteScalar());
    //            con.Close();
    //        }
    //    }
    //    catch (Exception ex)
    //    { }
    //    return outputid;
    //}





    public DataTable GetAllEmployeeTimesheetRequest()
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("Sp_EmployeeTimeSheetRequest", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GETALL");
        //cmd.Parameters.AddWithValue("@KEID", objCIPBLL.KEID);
        //cmd.Parameters.AddWithValue("@LocationID", objCIPBLL.LocationId);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }

    public bool CheckforApproval(int EmpId, DateTime reqestdate)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("Sp_EmployeeTimeSheetRequest", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GET");
        cmd.Parameters.AddWithValue("@EmpId", EmpId);
        cmd.Parameters.AddWithValue("@RequestDate", reqestdate);
        int count = (int)cmd.ExecuteScalar();
        bool check;
        if (count > 0)
        {
            check = true;
        }
        else
        {
            check = false;
        }
        return check;
    }
}