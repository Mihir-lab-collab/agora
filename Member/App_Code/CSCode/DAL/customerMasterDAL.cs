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
    /// Summary description for customerMasterDAL
    /// </summary>
    public class customerMasterDAL
    {
        public customerMasterDAL()
        {
        }
        public int InsertCustomer(customerMaster objcustomerMaster)
        {
            int outputid = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_InsertCustomer", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@custName", objcustomerMaster.custName);
            cmd.Parameters.AddWithValue("@custCompany", objcustomerMaster.custCompany);
            cmd.Parameters.AddWithValue("@custEmail", objcustomerMaster.custEmail);
            cmd.Parameters.AddWithValue("@custAddress", objcustomerMaster.custAddress);
            cmd.Parameters.AddWithValue("@custRegDate", objcustomerMaster.custRegDate);
            cmd.Parameters.AddWithValue("@custNotes", objcustomerMaster.custNotes);
            cmd.Parameters.AddWithValue("@custStatus", objcustomerMaster.custStatus);
            cmd.Parameters.AddWithValue("@TaskMailLevel", objcustomerMaster.TaskMailLevel);
            cmd.Parameters.AddWithValue("@InsertedOn", objcustomerMaster.InsertedOn);
            cmd.Parameters.AddWithValue("@ModifiedOn", objcustomerMaster.ModifiedOn);
            cmd.Parameters.AddWithValue("@EmailCC", objcustomerMaster.custEmailCC);
            cmd.Parameters.AddWithValue("@ShowAllTask", objcustomerMaster.ShowAllTask);
            cmd.Parameters.AddWithValue("@CountryName",objcustomerMaster.CountryName);
		    cmd.Parameters.AddWithValue("@StateName",objcustomerMaster.StateName);
		    cmd.Parameters.AddWithValue("@CityName",objcustomerMaster.CityName);
            cmd.Parameters.AddWithValue("@GSTIN", objcustomerMaster.GSTIN);

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
        public int UpdateCustomer(customerMaster objcustomerMaster)
        {
            int outputid = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateCustomer", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@custId", objcustomerMaster.custId);
            cmd.Parameters.AddWithValue("@custName", objcustomerMaster.custName);
            cmd.Parameters.AddWithValue("@custCompany", objcustomerMaster.custCompany);
            cmd.Parameters.AddWithValue("@custEmail", objcustomerMaster.custEmail);
            cmd.Parameters.AddWithValue("@custAddress", objcustomerMaster.custAddress);
            cmd.Parameters.AddWithValue("@custNotes", objcustomerMaster.custNotes);
            cmd.Parameters.AddWithValue("@ModifiedOn", objcustomerMaster.ModifiedOn);
            cmd.Parameters.AddWithValue("@EmailCC", objcustomerMaster.custEmailCC);
            cmd.Parameters.AddWithValue("@ShowAllTask", objcustomerMaster.ShowAllTask);
            cmd.Parameters.AddWithValue("@CountryName", objcustomerMaster.CountryName);
            cmd.Parameters.AddWithValue("@StateName", objcustomerMaster.Name);
            cmd.Parameters.AddWithValue("@CityName", objcustomerMaster.CityName);
            cmd.Parameters.AddWithValue("@GSTIN", objcustomerMaster.GSTIN);
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
        public customerMaster GetCustomerByCustomerName(string CompanyName)
        {

            customerMaster objGetCustomerByCustomerName = new customerMaster();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Sp_GetCustomerByCustomerName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyName", CompanyName);
                SqlDataReader reader = null;
                Nullable<DateTime> dt = null;
                //Nullable<bool> ismis = null;

                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        objGetCustomerByCustomerName = new customerMaster(
                        Convert.ToInt32(reader["custId"]),
                        reader["custName"].ToString(),
                        reader["custCompany"].ToString(),
                        reader["custAddress"].ToString(),
                        reader["custRegDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["custRegDate"].ToString()),
                        reader["custNotes"].ToString(),
                        Convert.ToBoolean(reader["custStatus"].ToString()),

                        reader["custEmail"].ToString(),
                            //string.IsNullOrEmpty(Convert.ToString(reader["lastLogin"])) ? dt : Convert.ToDateTime(reader["lastLogin"]),
                        Convert.ToInt32(reader["TaskMailLevel"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                        reader["EmailCC"].ToString(),
                         Convert.ToBoolean(reader["ShowALLTask"].ToString())
                        );

                    }
                }
            }
            catch (Exception ex)
            { }
            return objGetCustomerByCustomerName;
        }
        public customerMaster GetCustomerByCustId(int CustId)
        {

            customerMaster objGetCustomerByCustomerName = new customerMaster();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Sp_GetCustomerByCustId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustId", CustId);
                SqlDataReader reader = null;
                Nullable<DateTime> dt = null;

                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        objGetCustomerByCustomerName = new customerMaster(
                        Convert.ToInt32(reader["custId"]),
                        reader["custName"].ToString(),
                        reader["custCompany"].ToString(),
                        reader["custAddress"].ToString(),
                        reader["custRegDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["custRegDate"].ToString()),
                        reader["custNotes"].ToString(),
                        Convert.ToBoolean(reader["custStatus"].ToString()),
                        reader["custEmail"].ToString(),
                            //string.IsNullOrEmpty(Convert.ToString(reader["lastLogin"])) ? dt : Convert.ToDateTime(reader["lastLogin"]),
                        Convert.ToInt32(reader["TaskMailLevel"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                        reader["EmailCC"].ToString(),
                         Convert.ToBoolean(reader["ShowALLTask"].ToString())
                        );

                    }
                }
            }
            catch (Exception ex)
            { }
            return objGetCustomerByCustomerName;
        }
        public List<customerMaster> GetAllCustomers()
        {
            List<customerMaster> curcustomerMaster = new List<customerMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetAllCustomers", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            Nullable<DateTime> dt = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curcustomerMaster.Add(new customerMaster(
                        Convert.ToInt32(reader["custId"]),
                        reader["custName"].ToString(),
                        reader["custCompany"].ToString(),
                        reader["custAddress"].ToString(),
                        reader["custRegDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["custRegDate"].ToString()),
                        reader["custNotes"].ToString(),
                        Convert.ToBoolean(reader["custStatus"].ToString()),
                        reader["custEmail"].ToString(),
                        //string.IsNullOrEmpty(Convert.ToString(reader["lastLogin"])) ? dt : Convert.ToDateTime(reader["lastLogin"]),                       
                        Convert.ToInt32(reader["TaskMailLevel"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                        reader["EmailCC"].ToString(),
                         Convert.ToBoolean(reader["ShowALLTask"].ToString()),
                         reader["CountryName"].ToString(),
                         reader["StateName"].ToString(),
                         reader["CityName"].ToString(),
                         reader["GSTIN"].ToString()
                        ));
                }
            }
            return curcustomerMaster;
        }

        public List<customerMaster> GetAllCountry()
        {
            List<customerMaster> curGetAllCountry = new List<customerMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("USP_GetAllCountry", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curGetAllCountry.Add(new customerMaster(
                         Convert.ToInt32(reader["CountryId"]),
                        reader["Name"].ToString()
                      ));
                }
            }
            return curGetAllCountry;
        }

        public List<customerMaster> GetAllState()
        {
            List<customerMaster> curGetAllState = new List<customerMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("USP_GetAllState", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curGetAllState.Add(new customerMaster(
                         Convert.ToInt32(reader["CountryId"]),
                        reader["StateName"].ToString(),
                        Convert.ToInt32(reader["StateID"])
                      ));
                }
            }
            return curGetAllState;
        }


        public List<customerMaster> GetAllCity()
        {
            List<customerMaster> curGetAllCity = new List<customerMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("USP_GetAllCity", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curGetAllCity.Add(new customerMaster(
                         Convert.ToInt32(reader["CountryFKId"]),
                        reader["CityName"].ToString(),
                        Convert.ToInt32(reader["StateID"]),
                        Convert.ToInt32(reader["CityID"])

                      ));
                }
            }
            return curGetAllCity;
        }
    }
}