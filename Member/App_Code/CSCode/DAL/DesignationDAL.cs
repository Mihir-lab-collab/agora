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
/// Summary description for DesignationDAL
/// </summary>
public class DesignationDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

	public DesignationDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public List<DesignationBLL> GetDesignationDetails(string mode, int desigID)
    {
        List<DesignationBLL> objDesig = new List<DesignationBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Designation", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@DesigID", desigID);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                objDesig.Add(new DesignationBLL(
                     reader["SkillId"].ToString() == "" ? 0 : Convert.ToInt32(reader["SkillId"].ToString()),
                     reader["SkillDesc"].ToString() == "" ? "" : Convert.ToString(reader["SkillDesc"].ToString())                    

                    ));
            }
        }
        return objDesig;
    }

    public int SaveDesignation(string mode, DesignationBLL objDesig)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Designation", con);
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