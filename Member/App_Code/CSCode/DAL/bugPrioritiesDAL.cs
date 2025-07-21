using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Customer.BLL;
using System.Web.Configuration;

namespace Customer.DAL
{
    public class bugPrioritiesDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public bugPrioritiesDAL()
        {
        }
        public List<bugPriorities> GetAllbugPriorities()
        {
            List<bugPriorities> CurbugPriorities = new List<bugPriorities>();

            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetAllbugPriorities", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CurbugPriorities.Add(new bugPriorities(
                        Convert.ToInt32(reader["priority_id"]),
                        reader["priority_desc"].ToString()
                        ));
                }
            }
            return CurbugPriorities;
        }

        public List<bugPriorities> GetbugPrioritiesByProjId(int projId,string mode)
        {
            List<bugPriorities> CurbugPrioritiesByProjId = new List<bugPriorities>();

            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetbugPrioritiesCountByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projid", projId);
            cmd.Parameters.AddWithValue("@mode", mode);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CurbugPrioritiesByProjId.Add(new bugPriorities(Convert.ToInt32(reader["priority_id"]), reader["priority_desc"].ToString()));
                }
            }
            return CurbugPrioritiesByProjId;
        }
    }
}