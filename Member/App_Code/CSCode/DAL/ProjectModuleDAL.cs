using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class ProjectModuleDAL
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);

    public ProjectModuleDAL()
    {
    }
       
    public List<ProjectModule> GetModules(int ProjID)
    {
        List<ProjectModule> curProjectModule = new List<ProjectModule>();
        SqlCommand cmd = new SqlCommand("SP_ProjectModule", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GETMODULE");
        cmd.Parameters.AddWithValue("@ProjID", ProjID);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curProjectModule.Add(new ProjectModule(
                    Convert.ToInt32(reader["ID"].ToString()),
                    reader["Name"].ToString())
                    );
            }
        }
        return curProjectModule;
    }

    public List<ProjectModule> GetProjectModulesByProjId(int ProjID)
    {
        List<ProjectModule> curProjectModule = new List<ProjectModule>();
        SqlCommand cmd = new SqlCommand("GetProjectModulesByProjId", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GETMODULE");
        cmd.Parameters.AddWithValue("@ProjID", ProjID);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curProjectModule.Add(new ProjectModule(
                        Convert.ToInt32(reader["ModuleID"].ToString()),
                        reader["ModuleName"].ToString()
                        ));
            }
        }
        return curProjectModule;
    }
}
