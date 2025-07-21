using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI;
/// <summary>
/// Summary description for ProfileModuleDAL
/// </summary>
public class ProfileModuleDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    public ProfileModuleDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int UpdateProfile(string Name, bool IsAdmin, int LocationID, int ProfileID, int reportingmanagerID)
    {
        DataTable dtPrj = null;
        SqlConnection con = new SqlConnection(_strConnection);
        try
        {
            using (con)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ProfileModule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "UpdateProfile");
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@IsAdmin", IsAdmin);
                cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                cmd.Parameters.AddWithValue("@ReportingManager", reportingmanagerID);
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    dtPrj = new DataTable();
                    sqlAdapter.Fill(dtPrj);
                }
            }
        }
        catch (SqlException ex)
        {
            if (ex.Message.Contains("Violation of UNIQUE KEY constraint 'IX_Name'. Cannot insert duplicate key in object 'dbo.Profile'."))
            {
                return 0;

            }
            return 1;
        }
        return 2;



    }
    public int InsertProfile(string Name, bool IsAdmin, int LocationID, int reportingmanagerID)
    {
        DataTable dtPrj = null;
        SqlConnection con = new SqlConnection(_strConnection);
        try
        {
            using (con)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ProfileModule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "InsertProfile");
                cmd.Parameters.AddWithValue("@Name", Name);
                if (LocationID == 0)
                    cmd.Parameters.AddWithValue("@LocationID", null);
                else
                    cmd.Parameters.AddWithValue("@LocationID", LocationID);

                if (reportingmanagerID == 0)
                    cmd.Parameters.AddWithValue("@ReportingManager", null);
                else
                    cmd.Parameters.AddWithValue("@ReportingManager", reportingmanagerID);
                cmd.Parameters.AddWithValue("@IsAdmin", IsAdmin);
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    dtPrj = new DataTable();
                    sqlAdapter.Fill(dtPrj);
                    con.Close();
                }
            }
        }
        catch (SqlException ex)
        {
            // string output = "Error";
            if (ex.Message.Contains("Violation of UNIQUE KEY constraint 'IX_Name'. Cannot insert duplicate key in object 'dbo.Profile'."))
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Duplicate Record');", true);
                return 0;

                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Insert is successfull')", true);
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"alert","alert('" + output + "')", true);

            }
            return 1;
        }
        return 2;
    }
    public DataTable getLocations()
    {
        DataTable dtPrj = null;
        SqlConnection con = new SqlConnection(_strConnection);
        using (con)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_ProfileModule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "AllLocations");
            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
            {
                dtPrj = new DataTable();
                sqlAdapter.Fill(dtPrj);
            }
        }
        return dtPrj;
    }

    public DataTable getReportingManagers()
    {
        DataTable dtPrj = null;
        SqlConnection con = new SqlConnection(_strConnection);
        using (con)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_GetAllEmployees", con);
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
            {
                dtPrj = new DataTable();
                sqlAdapter.Fill(dtPrj);
            }
        }
        return dtPrj;
    }

    public DataTable getProfile(string ProfileID)
    {
        DataTable dtPrj = null;
        SqlConnection con = new SqlConnection(_strConnection);
        using (con)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_ProfileModule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "AllProfiles");
            cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
            cmd.Parameters.AddWithValue("@ModuleID", 0);
            cmd.Parameters.AddWithValue("@IsAdmin", 0);
            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
            {
                dtPrj = new DataTable();
                sqlAdapter.Fill(dtPrj);
            }
        }
        return dtPrj;
    }



    public DataTable getParentModules(string ProfileID)
    {
        DataTable dtPrj = null;
        SqlConnection con = new SqlConnection(_strConnection);
        using (con)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_ProfileModule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "ParentModules");
            cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
            cmd.Parameters.AddWithValue("@ModuleID", 0);
            cmd.Parameters.AddWithValue("@IsAdmin", 0);
            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
            {
                dtPrj = new DataTable();
                sqlAdapter.Fill(dtPrj);
            }
        }
        return dtPrj;
    }


    public DataTable getModules(string ProfileID, string ModuleID)
    {
        DataTable dtPrj = null;
        SqlConnection con = new SqlConnection(_strConnection);
        using (con)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_ProfileModule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "Modules");
            cmd.Parameters.AddWithValue("@ProfileID", Convert.ToInt32(ProfileID));
            cmd.Parameters.AddWithValue("@ModuleID", Convert.ToInt32(ModuleID));
            cmd.Parameters.AddWithValue("@IsAdmin", 0);
            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
            {
                dtPrj = new DataTable();
                sqlAdapter.Fill(dtPrj);
            }
        }
        return dtPrj;
    }
}