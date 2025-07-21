using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Customer.DAL
{
    public class bugsResolutionDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public bugsResolutionDAL()
        {

        }
        public List<bugsResolution> GetAllbugsResolutionByBugId(int BugId)
        {
            List<bugsResolution> CurGetAllbugsResolutionByBugId = new List<bugsResolution>();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("sp_GetAllbugsResolutionByBugId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BugId", BugId);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CurGetAllbugsResolutionByBugId.Add(new bugsResolution(
                            Convert.ToInt32(reader["bugsResolutionId"]),
                            Convert.ToInt32(reader["bug_id"]),
                            Convert.ToInt32(reader["status_id"]),
                            Convert.ToInt32(reader["priority_id"]),
                            reader["resolution"].ToString(),
                            Convert.ToInt32(reader["resolutionBy"]),
                            reader["resolutionDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["resolutionDate"].ToString()),
                            reader["insertedIp"].ToString()
                            ));
                    }
                }
            }
            catch (Exception ex)
            { }
            return CurGetAllbugsResolutionByBugId;
        }
        public int InsertbugsResolution(bugsResolution objbugsResolution)
        {
            int inserted = 0;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_InsertbugsResolution", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bug_id", objbugsResolution.bug_id);
            cmd.Parameters.AddWithValue("@status_id", objbugsResolution.status_id);
            cmd.Parameters.AddWithValue("@priority_id", objbugsResolution.priority_id);
            cmd.Parameters.AddWithValue("@resolution", objbugsResolution.resolution);
            cmd.Parameters.AddWithValue("@resolutionBy", objbugsResolution.resolutionBy);
            cmd.Parameters.AddWithValue("@resolutionDate", objbugsResolution.resolutionDate);
            cmd.Parameters.AddWithValue("@insertedIp", objbugsResolution.insertedIp);
            try
            {
                using (con)
                {
                    con.Open();
                    inserted = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            { }
            return inserted;
        }
    }

}