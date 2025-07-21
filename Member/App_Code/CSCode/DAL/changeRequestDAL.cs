using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Customer.DAL
{

    public class changeRequestDAL
    {
        public changeRequestDAL()
        {
        }

        public List<changeRequest> GetAllchangeRequestByProjId(int ProjId)
        {
            List<changeRequest> curGetAllchangeRequestByProjId = new List<changeRequest>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetAllchangeRequestByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@chgProjId", ProjId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curGetAllchangeRequestByProjId.Add(new changeRequest(
                         Convert.ToInt32(reader["chgid"]),
                        Convert.ToInt32(reader["chgProjId"]),
                         reader["chgEstCost"].ToString() == "" ? 0 : Convert.ToDecimal(reader["chgEstCost"].ToString())
                        ));
                }
            }
            return curGetAllchangeRequestByProjId;
        }
    }
}