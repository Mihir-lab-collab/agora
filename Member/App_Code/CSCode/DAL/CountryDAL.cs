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
/// Summary description for CountryDAL
/// </summary>
public class CountryDAL
{
	private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
	public CountryDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public List<CountryBLL> GetCountryDetails(string mode, int countryID)
    {
        List<CountryBLL> objCountry = new List<CountryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("USP_Country", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@CountryID", countryID);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                objCountry.Add(new CountryBLL(
                     reader["CountryID"].ToString() == "" ? 0 : Convert.ToInt32(reader["CountryID"].ToString()),
                     reader["Name"].ToString() == "" ? "" : Convert.ToString(reader["Name"].ToString())

                    ));
            }
        }
        return objCountry;
    }

    public int SaveCountry(string mode, CountryBLL objCountry)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("USP_Country", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@CountryID", objCountry.CountryID);
        cmd.Parameters.AddWithValue("@Name", objCountry.Country);
        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

            }

        }
        catch (Exception ex)
        {
            outputid = 2; // added outputid =2 to know something went wrong
        }
        return outputid;
    }
    public int SaveCountry2(string mode, DesignationBLL objDesig)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("USP_Country", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@DesigID", objDesig.DesigID);
        cmd.Parameters.AddWithValue("@Designation", objDesig.Designation);
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
}