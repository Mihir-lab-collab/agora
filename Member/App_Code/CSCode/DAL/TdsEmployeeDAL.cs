using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for TdsEmployeeDAL
/// </summary>
public class TdsEmployeeDAL
{
    private static string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    SqlConnection con = new SqlConnection(_strConnection);
    DataTable dt = new DataTable();
    SqlDataAdapter SDA = new SqlDataAdapter();
    DataSet DS = new DataSet();
   

    public DataTable GetEmpTds(string EmpId,string Year)
    {

        try
        {
            SqlCommand cmd = new SqlCommand("USP_TDSEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "AddEmployeeTds");
            cmd.Parameters.AddWithValue("@EmpId", EmpId);
            cmd.Parameters.AddWithValue("@Year", Year);
            using (con)
            {
                con.Open();
                dt.Load(cmd.ExecuteReader());
                con.Close();
            }

        }
        catch (Exception Ex)
        {

            con.Close();
        }

        return dt;
    }

    public void SaveTDS(List<TdsEmployeeBLL> GridData, string EmpID, string Year, string FatherName, string PanNo, int InsertedBy, bool IsRegime, bool IsDeclaimer)
    {
        try
        {
            if (GridData.Count > 0)
            {
                using (con)
                {
                    con.Open();
                   
                    foreach (var item in GridData)
                    {
                       // if (item.Amount!=0)
                       // {
                            SqlCommand cmd = new SqlCommand("USP_TDSEmployee", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Mode","InsertTdsEmployee" );
                            cmd.Parameters.AddWithValue("@EmpId", EmpID);
                            cmd.Parameters.AddWithValue("@Year", Year);
                            cmd.Parameters.AddWithValue("@TdsType", item.Id);
                            cmd.Parameters.AddWithValue("@Amount", item.Amount);
                            cmd.Parameters.AddWithValue("@Comment", item.Comment);
                            cmd.Parameters.AddWithValue("@FatherName", FatherName);
                            cmd.Parameters.AddWithValue("@PanNo", PanNo);
                            cmd.Parameters.AddWithValue("@InsertedBy", InsertedBy);
                            cmd.Parameters.AddWithValue("@IsRegime", IsRegime);
                            cmd.Parameters.AddWithValue("@IsDeclaimer", IsDeclaimer);
                            cmd.ExecuteNonQuery();
                        //}
                    }
                   
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public  DataTable CheckExistingTdsEmployee(string EmpID, string Year)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand cmd = new SqlCommand("USP_TDSEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GetTdsEmployee");
            cmd.Parameters.AddWithValue("@EmpId", EmpID);
            cmd.Parameters.AddWithValue("@Year", Year);
            using (con)
            {
                con.Open();
                dt.Load(cmd.ExecuteReader());
                con.Close();
            }
        }
        catch (Exception Ex)
        {
            con.Close();
        }
        return dt;
    }

    public DataTable GetEmplist(string Year)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand cmd = new SqlCommand("USP_TDSEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GetCurrentYearEmployee");
            cmd.Parameters.AddWithValue("@Year", Year);
            using (con)
            {
                con.Open();
                dt.Load(cmd.ExecuteReader());
                con.Close();
            }
        }
        catch (Exception Ex)
        {
            con.Close();
        }
        return dt;
    }

   
}