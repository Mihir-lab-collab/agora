using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Customer.BLL;
namespace Customer.DAL
{
    public class bugStatusesDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public bugStatusesDAL()
        {
        }
        public List<bugStatuses> GetAllbugStatuses()
        {
           
            List<bugStatuses> CurbugStatuses = new List<bugStatuses>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetAllbugStatuses", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CurbugStatuses.Add(new bugStatuses(
                        Convert.ToInt32(reader["status_id"]),
                        reader["status"].ToString(),
                        Convert.ToBoolean(reader["statusAdmin"].ToString()),
                        Convert.ToBoolean(reader["IsClient"].ToString()),
                        Convert.ToInt32(reader["SortOrder"]),
                        Convert.ToInt32(reader["TaskMailLevel"].ToString())
                        ));
                }
            }
            return CurbugStatuses;
        }
        public List<bugStatuses> GetbugStatusesByProjId(int ProjId,string mode)
        {
            List<bugStatuses> CurbugStatusesByProjId = new List<bugStatuses>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetbugStatusesByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projid", ProjId);
            cmd.Parameters.AddWithValue("@mode",mode);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CurbugStatusesByProjId.Add(new bugStatuses(
                        Convert.ToInt32(reader["status_id"]), reader["status"].ToString(), false, false, 0, 0));
                }
            }
            return CurbugStatusesByProjId;
        }
    }
}