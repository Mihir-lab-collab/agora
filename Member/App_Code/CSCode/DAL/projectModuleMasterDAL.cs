using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Customer.DAL
{
    public class projectModuleMasterDAL
    {
        public projectModuleMasterDAL()
        {
        }
        public projectModuleMaster GetprojectModuleMasterByModuleId(int moduleId)
        {
            projectModuleMaster objGetprojectModuleMasterByModuleId = new projectModuleMaster();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_GetprojectModuleMasterByModuleId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@moduleId", moduleId);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            objGetprojectModuleMasterByModuleId = new projectModuleMaster(
                            Convert.ToInt32(reader["moduleId"]),
                            Convert.ToInt32(reader["projId"]),
                            Convert.ToInt32(reader["moduleRefId"]),
                            reader["moduleName"].ToString(),
                            reader["moduleDescription"].ToString(),
                            reader["moduleDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["moduleDate"].ToString()),
                            reader["moduleEstimate"].ToString() == "" ? 0 : Convert.ToInt32(reader["moduleEstimate"].ToString()),
                            reader["InsertedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                            reader["ModifiedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"].ToString())
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return objGetprojectModuleMasterByModuleId;
        }

        public List<projectModuleMaster> GetprojectModuleMasterByProjectId(int projectId)
        {
           List<projectModuleMaster> objGetprojectModuleMasterByProjectId = new List<projectModuleMaster>();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_GetprojectModuleMasterByProjectId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjId", projectId);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                           objGetprojectModuleMasterByProjectId.Add(new projectModuleMaster(
                           Convert.ToInt32(reader["moduleId"]),
                           0,
                           0,
                           reader["moduleName"].ToString(),
                           "",
                           DateTime.Today,
                           0,
                           DateTime.Today,
                           DateTime.Today
                           ));
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return objGetprojectModuleMasterByProjectId;
        }

        public int InsertprojectModuleMasterByProjId(int projId)
        {
            int moduleid = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_InsertprojectModuleMasterByProjId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjId", projId);
                using (con)
                {
                    con.Open();
                    moduleid = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {

            }
            return moduleid;
        }

    }
}