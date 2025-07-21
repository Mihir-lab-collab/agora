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
    /// <summary>
    /// Summary description for CustTaskManagerDAL
    /// </summary>
    public class BugsDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public BugsDAL()
        {

        }

        public List<Bugs> GetBugsByProjId(int projectid, string includeTerminated)
        {
            List<Bugs> CurBugsByProjId = new List<Bugs>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetBugsByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projid", projectid);
            cmd.Parameters.AddWithValue("@includeTerminated", includeTerminated);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CurBugsByProjId.Add(new Bugs(
                        Convert.ToInt32(reader["bug_id"]),
                        Convert.ToInt32(reader["moduleID"]),
                        Convert.ToInt32(reader["priority_id"].ToString()),
                        Convert.ToInt32(reader["status_id"].ToString()),
                        reader["bug_name"].ToString(),
                        reader["bug_desc"].ToString(),
                        reader["resolution"].ToString(),
                        reader["assigned_by"].ToString(),
                        reader["assigned_by_Name"].ToString(),
                        reader["ModuleName"].ToString(),
                        Convert.ToInt32(reader["assigned_to"].ToString()),
                        reader["date_assigned"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["date_assigned"].ToString()),
                        reader["date_resolved"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["date_resolved"].ToString()),
                        reader["bug_lastModified"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["bug_lastModified"].ToString()),
                        Convert.ToByte(reader["IsType"])
                        ));
                }
            }
            return CurBugsByProjId;
        }
        public List<Bugs> GetTotalReportBugsCountByProjId(int projectid,string mode)
        {
            List<Bugs> CurTotalReportBugsCountByProjId = new List<Bugs>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetTotalReportBugsCountByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projid", projectid);
            cmd.Parameters.AddWithValue("@mode", mode);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CurTotalReportBugsCountByProjId.Add(new Bugs(
                       0, Convert.ToInt32(reader["moduleID"]), 0, 0, "", "", "", "","","", 0, DateTime.Today, DateTime.Today, DateTime.Today,null));
                }
            }
            return CurTotalReportBugsCountByProjId;
        }
        public bool DeletebugById(Bugs objbugs)
        {
            bool updated = false;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_DeletebugByBugId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bug_Id", objbugs.bug_id);

            try
            {
                using (con)
                {
                    con.Open();
                    int output = cmd.ExecuteNonQuery();
                    if (output > 0)
                        updated = true;
                }
            }
            catch (Exception ex)
            { }
            return updated;
        }
        public int Insertbugs(Bugs objbugs)
        {
            int outputid = 0;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_Insertbugs", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@moduleID", objbugs.moduleID);
            cmd.Parameters.AddWithValue("@priority_id", objbugs.priority_id);
            cmd.Parameters.AddWithValue("@status_id", objbugs.status_id);
            cmd.Parameters.AddWithValue("@bug_name", objbugs.bug_name);
            cmd.Parameters.AddWithValue("@bug_desc", objbugs.bug_desc);
            cmd.Parameters.AddWithValue("@resolution", objbugs.resolution);
            cmd.Parameters.AddWithValue("@assigned_by", objbugs.assigned_by);
            cmd.Parameters.AddWithValue("@assigned_to", objbugs.assigned_to);
            cmd.Parameters.AddWithValue("@IsType", objbugs.istype);
            // cmd.Parameters.AddWithValue("@date_assigned", objbugs.date_assigned);            
            //cmd.Parameters.AddWithValue("@bug_lastmodified", objbugs.bug_lastModified);
            try
            {
                using (con)
                {
                    con.Open();
                    outputid = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            { }
            return outputid;
        }
        public bool UpdatebugByBugId(Bugs objbugs)
        {
            bool updated = false;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_UpdatebugByBugId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bug_Id", objbugs.bug_id);
            cmd.Parameters.AddWithValue("@statusId", objbugs.status_id);
            cmd.Parameters.AddWithValue("@priority_id", objbugs.priority_id);
            cmd.Parameters.AddWithValue("@Resolution", objbugs.resolution);
            cmd.Parameters.AddWithValue("@assignedTo", objbugs.assigned_to);        
            cmd.Parameters.AddWithValue("@IsType", objbugs.istype);
            //cmd.Parameters.AddWithValue("@bug_lastmodified", objbugs.bug_lastModified);
            try
            {
                using (con)
                {
                    con.Open();
                    int output = Convert.ToInt32(cmd.ExecuteScalar());
                    if (output == 1)
                        updated = true;
                }
            }
            catch (Exception ex)
            { }
            return updated;
        }
        public Bugs GetBugBybug_id(int bug_id)
        {
            Bugs CurBugBybug_id = new Bugs();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetBugBybug_id", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bug_id", bug_id);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CurBugBybug_id = new Bugs(
                        Convert.ToInt32(reader["bug_id"]),
                        Convert.ToInt32(reader["moduleID"]),
                        Convert.ToInt32(reader["priority_id"].ToString()),
                        Convert.ToInt32(reader["status_id"].ToString()),
                        reader["bug_name"].ToString(),
                        reader["bug_desc"].ToString(),
                        reader["resolution"].ToString(),
                        reader["assigned_by"].ToString(),
                        reader["assigned_by_Name"].ToString(),
                        reader["ModuleName"].ToString(),
                        Convert.ToInt32(reader["assigned_to"].ToString()),
                        reader["date_assigned"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["date_assigned"].ToString()),
                        reader["date_resolved"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["date_resolved"].ToString()),
                        reader["bug_lastModified"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["bug_lastModified"].ToString()),
                        Convert.ToByte(reader["IsType"])
                        );
                }
            }
            return CurBugBybug_id;
        }

        public string CustUserDetails(int UserId)
        {
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("GetNameById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserMasterID", UserId);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return reader["Name"].ToString();
                    }
                }
            }
            catch (Exception ex)
            { }
            return "";
        }
    }
}