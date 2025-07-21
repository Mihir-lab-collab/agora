using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace AgoraBL.Models
{
    public class ConfigDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        public ConfigDAL()
        {
        }

        public List<ConfigBLL> GetConfigDetails(string mode, int configID)
        {
            List<ConfigBLL> curConfig = new List<ConfigBLL>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_CONFIG", con);
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@ConfigID", configID);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curConfig.Add(new ConfigBLL(
                         reader["configID"].ToString() == "" ? 0 : Convert.ToInt32(reader["ConfigID"].ToString()),
                         reader["category"].ToString() == "" ? "" : Convert.ToString(reader["Category"].ToString()),
                        reader["Name"].ToString() == "" ? "" : Convert.ToString(reader["Name"].ToString()),
                        reader["value"].ToString() == "" ? "" : Convert.ToString(reader["Value"].ToString()),
                        reader["value1"].ToString() == "" ? "" : Convert.ToString(reader["Value1"].ToString()),
                         reader["comment"].ToString() == "" ? "" : Convert.ToString(reader["Comment"].ToString()),
                         reader["modifiedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                        reader["modifiedBy"].ToString() == "" ? 0 : Convert.ToInt32(reader["ModifiedBy"].ToString())

                        ));
                }
            }
            return curConfig;
        }

        public int InsertConfig(ConfigBLL objInsert)
        {
            int outputid = 0;
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_CONFIG", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", objInsert.mode);
            cmd.Parameters.AddWithValue("@ConfigID", objInsert.configID);
            cmd.Parameters.AddWithValue("@Category", objInsert.category);
            cmd.Parameters.AddWithValue("@Name", objInsert.name);
            cmd.Parameters.AddWithValue("@Value", objInsert.value);
            cmd.Parameters.AddWithValue("@Value1", objInsert.value1);
            cmd.Parameters.AddWithValue("@Comment", objInsert.comment);
            cmd.Parameters.AddWithValue("@UserID", objInsert.insertedBy);
            try
            {
                using (con)
                {
                    outputid = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();

                }

            }
            catch (Exception ex)
            { }
            return outputid;

        }

        public bool UpdateConfig(ConfigBLL objUpdate)
        {
            bool updated = false;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_CONFIG", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", objUpdate.mode);
            cmd.Parameters.AddWithValue("@ConfigID", objUpdate.configID);
            cmd.Parameters.AddWithValue("@Category", objUpdate.category);
            cmd.Parameters.AddWithValue("@Name", objUpdate.name);
            cmd.Parameters.AddWithValue("@Value", objUpdate.value);
            cmd.Parameters.AddWithValue("@Value1", objUpdate.value1);
            cmd.Parameters.AddWithValue("@Comment", objUpdate.comment);
            cmd.Parameters.AddWithValue("@UserID", objUpdate.modifiedBy);


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

        public List<ConfigBLL> GetDefaultID(string mode)
        {
            List<ConfigBLL> curConfig = new List<ConfigBLL>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_CONFIG", con);
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curConfig.Add(new ConfigBLL(
                         reader["ConfigID"].ToString() == "" ? 0 : Convert.ToInt32(reader["ConfigID"].ToString())
                        ));
                }
            }
            return curConfig;
        }

    }
}
