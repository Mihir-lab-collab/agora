using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;

/// <summary>
/// Summary description for KRADAL
/// </summary>
public class KRADAL
{
    public KRADAL()
    {
    }

    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    
    public int SaveKRA(string mode, KRABLL objKRA)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_KRA", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@KRAID", objKRA.KRAID);
        cmd.Parameters.AddWithValue("@KRANames", objKRA.KRANames);
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

    public List<KRABLL> GetkAR(string mode, int KRAID)
    {
        List<KRABLL> objKRA = new List<KRABLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_KRA", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@KRAID", KRAID);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                objKRA.Add(new KRABLL(
                     reader["KRAID"].ToString() == "" ? 0 : Convert.ToInt32(reader["KRAID"].ToString()),
                     reader["KRANames"].ToString() == "" ? "" : Convert.ToString(reader["KRANames"].ToString())

                    ));
            }
        }
        return objKRA;
    }

    public int InsertUpdateKRAProfile(KRABLL objKRAProfile)
    {
        int ProfileId = 0;
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_GetKRAProfileDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "Save");
            cmd.Parameters.AddWithValue("@ProfileId", objKRAProfile.ProfileId);
            cmd.Parameters.AddWithValue("@KRAID", objKRAProfile.KRAID);
            using (con)
            {
                con.Open();
                ProfileId = Convert.ToInt32(cmd.ExecuteScalar());

            }
        }
        catch (Exception ex)
        {

        }
        return ProfileId;
    }

    public int DeleteKRAProfile(KRABLL objKRAProfile)
    {
        int ProfileId = 0;
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_GetKRAProfileDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "Delete");
            cmd.Parameters.AddWithValue("@ProfileId", objKRAProfile.ProfileId);
            using (con)
            {
                con.Open();
                ProfileId = Convert.ToInt32(cmd.ExecuteScalar());

            }
        }
        catch (Exception ex)
        {

        }
        return ProfileId;
    }

}