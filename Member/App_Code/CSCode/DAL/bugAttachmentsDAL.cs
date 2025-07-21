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
    public class bugAttachmentsDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public bugAttachmentsDAL()
        {
        }
        public List<bugAttachments> GetAllbugAttachmentsByBugId(int BugId)
        {
            List<bugAttachments> CurbugAttachmentsByBugId = new List<bugAttachments>();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("sp_GetAllbugAttachmentsByBugId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BugId", BugId);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CurbugAttachmentsByBugId.Add(new bugAttachments(
                            Convert.ToInt32(reader["bugFileId"]),
                            Convert.ToInt32(reader["bug_Id"]),
                            reader["bugsResolutionId"].ToString() == "" ? 0 : Convert.ToInt32(reader["bugsResolutionId"]),
                            reader["bugFilePath"].ToString(),
                            reader["bugFileDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["bugFileDate"].ToString())
                            ));
                    }
                }
            }
            catch (Exception ex)
            { }
            return CurbugAttachmentsByBugId;
        }

        public bool DeletebugAttachmentsByFileId(int bugFileId)
        {
            bool deleted = false;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_DeletebugAttachmentsByFileId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bugFileId", bugFileId);
            try
            {
                using (con)
                {
                    con.Open();
                    int isdeleted = Convert.ToInt32(cmd.ExecuteScalar());
                    if (isdeleted == 1)
                        deleted = true;
                }
            }
            catch (Exception ex)
            { }
            return deleted;
        }

        public bool InsertbugAttachments(bugAttachments objbugAttachments)
        {
            bool inserted = false;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_InsertbugAttachments", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bug_Id", objbugAttachments.bug_Id);
            cmd.Parameters.AddWithValue("@bugsResolutionId", objbugAttachments.bugsResolutionId);
            cmd.Parameters.AddWithValue("@bugFilePath", objbugAttachments.bugFilePath);
            cmd.Parameters.AddWithValue("@bugFileDate", objbugAttachments.bugFileDate);
            try
            {
                using (con)
                {
                    con.Open();
                    int output = Convert.ToInt32(cmd.ExecuteScalar());
                    if (output == 1)
                        inserted = true;
                }
            }
            catch (Exception ex)
            { }
            return inserted;
        }
    }
}