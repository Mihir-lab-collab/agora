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
    public class paymentMasterDAL
    {
        public paymentMasterDAL()
        {
        }

        public List<paymentMaster> GetAllpaymentMaster()
        {
            List<paymentMaster> curGetAllpaymentMaster = new List<paymentMaster>();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetAllpaymentMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;          
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curGetAllpaymentMaster.Add(new paymentMaster(
                        Convert.ToInt32(reader["payId"]),
                        Convert.ToInt32(reader["payProjId"]),
                        reader["payDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["payDate"].ToString()),
                        reader["payAmount"].ToString() == "" ? 0 : Convert.ToDecimal(reader["payAmount"].ToString()),
                        reader["payExRate"].ToString() == "" ? 0 : Convert.ToDecimal(reader["payExRate"].ToString()),
                        reader["payConfirmedDate"].ToString() == "" || reader["payConfirmedDate"].ToString() == "01-01-1900 00:00:00" ? DateTime.MinValue : Convert.ToDateTime(reader["payConfirmedDate"].ToString()),
                        reader["payComment"].ToString(),
                        Convert.ToDecimal(reader["payTransCharge"].ToString()),
                        reader["paymentType"].ToString() == "" ? 0 : Convert.ToInt32(reader["paymentType"].ToString()),
                        reader["PaymentMode"].ToString(),
                        reader["InvoiceSendOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InvoiceSendOn"].ToString()),
                        reader["crId"].ToString() == "" ? 0 : Convert.ToInt32(reader["crId"].ToString()),
                        reader["CreatedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedOn"].ToString()),
                        reader["IsMiscellaneous"].ToString() == "" ? false : Convert.ToBoolean(reader["IsMiscellaneous"].ToString())
                        ));
                }
            }
            return curGetAllpaymentMaster;
        }
        public List<paymentMaster> GetAllpaymentMasterByProjId(int projId)
        {
            List<paymentMaster> curAllpaymentMasterByProjId = new List<paymentMaster>();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetAllpaymentMasterByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@payProjId", projId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curAllpaymentMasterByProjId.Add(new paymentMaster(
                        Convert.ToInt32(reader["payId"]),
                        Convert.ToInt32(reader["payProjId"]),
                        reader["payDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["payDate"].ToString()),
                        reader["payAmount"].ToString() == "" ? 0 : Convert.ToDecimal(reader["payAmount"].ToString()),
                        reader["payExRate"].ToString() == "" ? 0 : Convert.ToDecimal(reader["payExRate"].ToString()),
                        reader["payConfirmedDate"].ToString() == "" || reader["payConfirmedDate"].ToString() == "01-01-1900 00:00:00" ? DateTime.MinValue : Convert.ToDateTime(reader["payConfirmedDate"].ToString()),
                        reader["payComment"].ToString(),
                        Convert.ToDecimal(reader["payTransCharge"].ToString()),
                        reader["paymentType"].ToString() == "" ? 0 : Convert.ToInt32(reader["paymentType"].ToString()),
                        reader["PaymentMode"].ToString(),
                        reader["InvoiceSendOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InvoiceSendOn"].ToString()),
                        reader["crId"].ToString() == "" ? 0 : Convert.ToInt32(reader["crId"].ToString()),
                        reader["CreatedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedOn"].ToString()),
                        reader["IsMiscellaneous"].ToString() == "" ? false : Convert.ToBoolean(reader["IsMiscellaneous"].ToString()) 
                        ));
                }
            }
            return curAllpaymentMasterByProjId;
        }
    }
}