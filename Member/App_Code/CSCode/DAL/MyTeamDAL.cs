using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using Customer.BLL;
using Customer.DAL;

/// <summary>
/// Summary description for MyTeamDAL
/// </summary>
namespace Customer.DAL
{

    public class MyTeamDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        public MyTeamDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public List<MyTeamBLL> GetMyTeam(int empId, string includeMyprojects)
        {
            List<MyTeamBLL> CurMyTeam = new List<MyTeamBLL>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetMyTeam", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpId", empId);
            cmd.Parameters.AddWithValue("@includeMyprojects", includeMyprojects);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CurMyTeam.Add(new MyTeamBLL(
                        reader["empId"].ToString() == "" ? 0 : Convert.ToInt32(reader["empId"]),
                        reader["empName"].ToString() == "" ? "" : Convert.ToString(reader["empName"].ToString()),
                        reader["designation"].ToString() == "" ? "" : Convert.ToString(reader["designation"].ToString()),
                        reader["primarySkill"].ToString() == "" ? "" : Convert.ToString(reader["primarySkill"].ToString()),

                        reader["SecondarySkill"].ToString() == "" ? "" : Convert.ToString(reader["SecondarySkill"].ToString().TrimStart(',').Trim()),
                        reader["empExperince"].ToString() == "" ? "" : Convert.ToString(reader["empExperince"].ToString()),
                        reader["projectsWorkingOn"].ToString() == "" ? "" : Convert.ToString(reader["projectsWorkingOn"].ToString())
                        
                       ));
                }
            }
            return CurMyTeam;
        }
    }
}