using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Customer.BLL;
using System.Data.SqlClient;
using System.Data;
/// <summary>
/// Summary description for CustProfileDAL
/// </summary>
/// 
namespace Customer.DAL
{
    public class ProfileDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public ProfileDAL()
        {
           
        }
        public Profile GetCustomerDetails(string CustId)
        {
            Profile custDetailsObj = new Profile();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("sp_GetCustProfileDtl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustId", CustId);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            custDetailsObj =  new Profile(reader["custName"].ToString(), reader["custCompany"].ToString(), reader["custAddress"].ToString(), reader["custEmail"].ToString(), reader["custPassword"].ToString(), Convert.ToInt32(reader["custId"]).ToString());                        
                        }
                        //custProfile = new CustProfile(Convert.ToInt32(reader["custId"]), reader["custName"].ToString(), reader["custCompany"].ToString(), reader["custAddress"].ToString(), reader["custEmail"].ToString(), reader["custPassword"].ToString());
                    }
                }
            }
            catch (Exception ex)
            { }
            return custDetailsObj;
        }
        
        public bool UpdateCustomerProfile(string CustName,string Company, string CustAddress, string CustEmail, string Password,string CustId)
        {
            Profile custProfile = new Profile();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("sp_UpdateCustProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustName", CustName);
                cmd.Parameters.AddWithValue("@Company", Company);
                cmd.Parameters.AddWithValue("@CustAddress", CustAddress);
                cmd.Parameters.AddWithValue("@CustEmail", CustEmail);
                cmd.Parameters.AddWithValue("@CustPassword", Password);
                cmd.Parameters.AddWithValue("@CustId", Convert.ToInt32(CustId));
                //SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                  int i = cmd.ExecuteNonQuery();
                  if (i > 0)
                  {
                      return true;
                  }
                  else
                  {
                      return false;
                  }
                    
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            //return curUser;
        }

    }
}