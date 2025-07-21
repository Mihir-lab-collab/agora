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

    public class currencyMasterDAL
    {
        public currencyMasterDAL()
        {
        }
        public currencyMaster GetcurrencyMasterBycurrId(int currId)
        {

            currencyMaster objGetcurrencyMasterBycurrId = new currencyMaster();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Sp_GetcurrencyMasterBycurrId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@currId", currId);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        objGetcurrencyMasterBycurrId = new currencyMaster(
                        Convert.ToInt32(reader["currId"]),
                        reader["currName"].ToString(),
                        reader["currSymbol"].ToString()
                        );

                    }
                }
            }
            catch (Exception ex)
            { }
            return objGetcurrencyMasterBycurrId;
        }
        public List<currencyMaster> GetAllcurrencyMaster()
        {
            List<currencyMaster> curGetAllcurrencyMaster = new List<currencyMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetAllcurrencyMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curGetAllcurrencyMaster.Add(new currencyMaster(
                         Convert.ToInt32(reader["currId"]),
                        reader["currName"].ToString(),
                        reader["currSymbol"].ToString()
                        ));
                }
            }
            return curGetAllcurrencyMaster;
        }
    }
}