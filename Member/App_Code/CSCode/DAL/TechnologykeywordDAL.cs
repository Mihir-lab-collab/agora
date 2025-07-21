using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
/// <summary>
/// Summary description for TechnologykeywordDAL
/// </summary>
public class TechnologykeywordDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public  List<TechnologykeywordBLL> GetAllTechnologyKeyword(string mode,string techname)
    {
        List<TechnologykeywordBLL> getalltechnology = new List<TechnologykeywordBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_TechnologyKeyword", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@techName", techname);
                cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getalltechnology.Add(new TechnologykeywordBLL(
                    Convert.ToInt32(reader["techId"]),
                    reader["techName"].ToString(),
                    reader["subTechName"].ToString()
                ));
            }
            con.Close();
        }
        return getalltechnology;

    }

    public void AddNewTechnology(TechnologykeywordBLL objtech)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_TechnologyKeyword", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@mode", "Insert");
        cmd.Parameters.AddWithValue("@techId", objtech.techId);
        cmd.Parameters.AddWithValue("@techName", objtech.techName);
        //cmd.Parameters.AddWithValue("@subTechName", objtech.subTechName);
        try
        {
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        { }
    }

    public void UpdateTechnology(TechnologykeywordBLL objtech)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_TechnologyKeyword", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@mode", "Update");
        cmd.Parameters.AddWithValue("@techId", objtech.techId);
        cmd.Parameters.AddWithValue("@techName", objtech.techName);
     //   cmd.Parameters.AddWithValue("@subTechName", objtech.subTechName);
        try
        {
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        { }
    }

    public void DeleteTechnology(TechnologykeywordBLL objtech)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_TechnologyKeyword", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@mode", "delete");
        cmd.Parameters.AddWithValue("@techId", objtech.techId);
        try
        {
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        { }
    }
	
}