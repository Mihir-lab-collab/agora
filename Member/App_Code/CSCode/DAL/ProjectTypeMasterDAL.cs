using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectType.BLL;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProjectType.DAL
{
    public class ProjectTypeMasterDAL
    {
        public ProjectTypeMasterDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public ProjectTypeMaster GetprojectTypeByprojTypeId(int ProjTypeId)
        {

            ProjectTypeMaster objprojectTypeMaster = new ProjectTypeMaster();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_GetProjectTypebyProjId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjTypeID", ProjTypeId);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        objprojectTypeMaster = new ProjectTypeMaster(
                        Convert.ToInt32(reader["ProjTypeID"]),
                        reader["ProjectType"].ToString()
                       
                        );

                    }
                }
            }
            catch (Exception ex)
            { }
            return objprojectTypeMaster;
        }


        public List<ProjectTypeMaster> getProjectType()
        {
            List<ProjectTypeMaster> getAllProjectType = new List<ProjectTypeMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetProjectType ", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    getAllProjectType.Add(new ProjectTypeMaster(
                         Convert.ToInt32(reader["ProjTypeID"]),
                        reader["ProjectType"].ToString()
                        
                        ));
                }
            }
            return getAllProjectType;
        }
    }
}