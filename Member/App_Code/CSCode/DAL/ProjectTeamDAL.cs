using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectTeamDAL
/// </summary>
public class ProjectTeamDAL
{
	public ProjectTeamDAL()
	{
		
	}

    internal List<ProjectTeam> Projects()
    {
        List<ProjectTeam> objcurrProject = new List<ProjectTeam>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_ProjectTeam", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Mode", "SELECT");
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                objcurrProject.Add(new ProjectTeam(
                   Convert.ToString(reader["projName"]),
                   Convert.ToInt32(Convert.ToString(reader["projId"])),
                   Convert.ToInt32(Convert.ToString(reader["empId"])),
                    Convert.ToString(reader["empName"]),
                    Convert.ToInt32(Convert.ToString(reader["Discount"])),
                    //Convert.ToString(Convert.ToDateTime(reader["ModifiedOn"]).ToString("dd-MMM-yyyy")),
                   Convert.ToString(reader["ModifiedOn"]) == "" ? DateTime.MinValue : Convert.ToDateTime(Convert.ToDateTime(reader["ModifiedOn"]).ToString("dd-MMM-yyyy")),
                     Convert.ToInt32(Convert.ToString(reader["MemberId"]))
                    
                    // Convert.ToInt16(reader["IsActive"])
                   ));
            }
        }
        return objcurrProject;
    }


    internal string UpdateProjectTeam(ProjectTeam objteam)
    {
        string message = string.Empty;
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_ProjectTeam", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", objteam.Mode);
            cmd.Parameters.AddWithValue("@projId", objteam.projId);
            cmd.Parameters.AddWithValue("@empId", objteam.empId);
            cmd.Parameters.AddWithValue("@Discount", objteam.Discount);
            cmd.Parameters.AddWithValue("@IsActive", objteam.Isactive);
            cmd.Parameters.AddWithValue("@ModifiedOn", objteam.ModifiedOn);
            cmd.Parameters.AddWithValue("@MemberId", objteam.MemberId);
            using (con)
            {
                con.Open();
               // cmd.ExecuteScalar();
                
                message = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception Ex)
        {

        }
        return message;
    }

    public List<ProjectTeam> GetEmployees()
    {
        List<ProjectTeam> objcurrProject = new List<ProjectTeam>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_ProjectTeam", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GetEmployee");
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                objcurrProject.Add(new ProjectTeam(
                    Convert.ToInt32(reader["empid"]),
                    reader["empName"].ToString()
                    //reader["empAddress"].ToString(),
                    //reader["InsertedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                    //reader["ModifiedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"].ToString())
                    ));
            }
        }
        return objcurrProject;
    }

    internal List<ProjectTeam> GetProjectListById(string mode, int _projId)
    {
        List<ProjectTeam> objcurrProject = new List<ProjectTeam>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_ProjectTeam", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@projId", _projId);
        cmd.Parameters.AddWithValue("@Mode", mode);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                objcurrProject.Add(new ProjectTeam(
                   Convert.ToString(reader["projName"]),
                   Convert.ToInt32(Convert.ToString(reader["projId"])),
                   Convert.ToInt32(Convert.ToString(reader["empId"])),
                    Convert.ToString(reader["empName"]),
                    Convert.ToInt32(Convert.ToString(reader["Discount"])),
                    Convert.ToString(reader["ModifiedOn"]) == "" ? DateTime.MinValue : Convert.ToDateTime(Convert.ToString(reader["ModifiedOn"])),
                     Convert.ToInt32(Convert.ToString(reader["MemberId"]))

                    // Convert.ToInt16(reader["IsActive"])
                   ));
            }
        }
        return objcurrProject;
    }
}