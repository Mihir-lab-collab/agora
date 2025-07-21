using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Customer.BLL;
using System.Web.Configuration;

/// <summary>
/// Summary description for EmployeeLWD_DAL
/// </summary>
public class EmployeeLWD_DAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public EmployeeLWD_DAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public List<EmployeeLWD_DLL> GetEmployeeLWDDetails(string mode)
    {

        List<EmployeeLWD_DLL> lstEmployee = new List<EmployeeLWD_DLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);

        SqlDataReader reader = null;
        Nullable<DateTime> dt = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EmployeeLWD_DLL obj = new EmployeeLWD_DLL();
                obj.empid = Convert.ToInt32(reader["empid"]);
                obj.empName = Convert.ToString(reader["empName"]);
                obj.empExpectedLWD = string.IsNullOrEmpty(Convert.ToString(reader["empExpectedLWD"])) ? dt : Convert.ToDateTime(reader["empExpectedLWD"]);
                obj.daysPending = Convert.ToString(reader["daysPending"]);
                lstEmployee.Add(obj);
            }
        }
        return lstEmployee;
    }
}