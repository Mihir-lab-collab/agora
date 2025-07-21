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
    /// <summary>
    /// Summary description for CustUserDAL
    /// </summary>
    public class CustUserDAL
    {
        public CustUserDAL()
        {
        }
        public int InsertCustomerUser(CustUser objCustUser)
        {
            int userid = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_InsertUserProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserMasterID", objCustUser.UserMasterID);
                cmd.Parameters.AddWithValue("@CustId", objCustUser.CustID);               
                cmd.Parameters.AddWithValue("@FName", objCustUser.FName);
                cmd.Parameters.AddWithValue("@LName", objCustUser.LName);
                cmd.Parameters.AddWithValue("@Email", objCustUser.Email);
                cmd.Parameters.AddWithValue("@Password", objCustUser.Password);
                cmd.Parameters.AddWithValue("@ContactNo", objCustUser.ContactNo);
                cmd.Parameters.AddWithValue("@isAdmin", objCustUser.IsAdmin);
                cmd.Parameters.AddWithValue("@Status", objCustUser.Status);
                cmd.Parameters.AddWithValue("@InsertedOn", DateTime.Now);
                cmd.Parameters.AddWithValue("@CurLoginIP", objCustUser.LastLoginIP);

                using (con)
                {
                    con.Open();
                    userid = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {

            }
            return userid;
        }
        public int UpdateCustomerUser(CustUser objCustUser)
        {
            int userid = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_UpdateCustomerUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserMasterID", objCustUser.UserMasterID);
                cmd.Parameters.AddWithValue("@FName", objCustUser.FName);
                cmd.Parameters.AddWithValue("@LName", objCustUser.LName);
                cmd.Parameters.AddWithValue("@Email", objCustUser.Email);
                cmd.Parameters.AddWithValue("@ContactNo", objCustUser.ContactNo);
                cmd.Parameters.AddWithValue("@IsAdmin", objCustUser.IsAdmin);
                cmd.Parameters.AddWithValue("@Status", objCustUser.Status);
                cmd.Parameters.AddWithValue("@Password", objCustUser.Password);
                cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);

                using (con)
                {
                    con.Open();
                    userid = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {

            }
            return userid;
        }
        public List<CustUser> GetAllFirstUserofAllCustomers()
        {
            List<CustUser> curGetAllFirstUserofAllCustomers = new List<CustUser>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_curGetAllFirstUserofAllCustomers", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curGetAllFirstUserofAllCustomers.Add(new CustUser(
                        Convert.ToInt32(reader["UserMasterID"]),
                        Convert.ToInt32(reader["CustID"]),
                        reader["Password"].ToString(),
                        reader["FName"].ToString(),
                        reader["LName"].ToString(),
                        reader["Email"].ToString(),
                        reader["ContactNo"].ToString(),
                        Convert.ToBoolean(reader["IsAdmin"]),
                        reader["LastLogin"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["LastLogin"].ToString()),
                        reader["LastLoginIP"].ToString(),
                        reader["Status"].ToString(),
                        reader["InsertedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedOn"].ToString())
                        ));
                }
            }
            return curGetAllFirstUserofAllCustomers;
        }
        public List<CustUser> GetAllCustUsersByCustID(int CustID)
        {
            List<CustUser> curcustusers = new List<CustUser>();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_GetAllCustUsersByCustID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustID", Convert.ToInt32(CustID));
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        curcustusers.Add(new CustUser(Convert.ToInt16(reader["UserMasterID"]),
                            Convert.ToInt16(reader["CustID"]),
                            Convert.ToString(reader["Password"]),
                            Convert.ToString(reader["FName"]),
                            Convert.ToString(reader["LName"]),
                            Convert.ToString(reader["Email"]),
                            Convert.ToString(reader["ContactNo"]),
                            Convert.ToBoolean(reader["IsAdmin"]),
                            Convert.ToString(reader["LastLogin"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["LastLogin"]),
                            Convert.ToString(reader["LastLoginIP"]),
                            Convert.ToString(reader["Status"]),
                             Convert.ToString(reader["InsertedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"]),
                            Convert.ToString(reader["ModifiedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"])));
                    }
                }
            }
            catch (Exception ex)
            { }

            return curcustusers;
        }
        public List<CustUser> GetAllCustUsers()
        {
            List<CustUser> curcustusers = new List<CustUser>();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_GetAllCustUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        curcustusers.Add(new CustUser(Convert.ToInt16(reader["UserMasterID"]),
                            Convert.ToInt16(reader["CustID"]),
                            Convert.ToString(reader["Password"]),
                            Convert.ToString(reader["FName"]),
                            Convert.ToString(reader["LName"]),
                            Convert.ToString(reader["Email"]),
                            Convert.ToString(reader["ContactNo"]),
                            Convert.ToBoolean(reader["IsAdmin"]),
                            Convert.ToString(reader["LastLogin"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["LastLogin"]),
                            Convert.ToString(reader["LastLoginIP"]),
                            Convert.ToString(reader["Status"]),
                             Convert.ToString(reader["InsertedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"]),
                            Convert.ToString(reader["ModifiedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"])));
                    }
                }
            }
            catch (Exception ex)
            { }

            return curcustusers;
        }
    }
}