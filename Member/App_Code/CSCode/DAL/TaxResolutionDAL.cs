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

    public class TaxResolutionDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private string sqlquery;

        public TaxResolutionDAL()
        {

        }
        public List<TaxResolution> GetAlltaxResolutionBytaxId(int tax_id)
        {
            List<TaxResolution> Curtaxdetail = new List<TaxResolution>();
            SqlConnection con = new SqlConnection(_strConnection);
            sqlquery = "select * from taxmaster where tax_id=" + tax_id;
            SqlCommand cmd = new SqlCommand(sqlquery, con);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Curtaxdetail.Add(new TaxResolution(
                        reader["tax_id"].ToString() == "" || reader["tax_id"] == null ? 0 : Convert.ToInt32(reader["tax_id"]),
                        reader["tax_parentid"].ToString() == "" || reader["tax_parentid"] == null ? 0 : Convert.ToInt32(reader["tax_parentid"]),
                        reader["taxname"].ToString() == "" || reader["taxname"] == null ? "" : Convert.ToString(reader["taxname"].ToString()),
                        reader["tax_description"].ToString() == "" || reader["tax_description"] == null ? "" : Convert.ToString(reader["tax_description"].ToString()),
                        reader["tax_rate"].ToString() == "" || reader["tax_rate"] == null ? 0 : Convert.ToDecimal(reader["tax_rate"]),
                        reader["tax_fromdate"].ToString() == "" || reader["tax_fromdate"] == null ? DateTime.Today : Convert.ToDateTime(reader["tax_fromdate"].ToString()),
                        reader["tax_todate"].ToString() == "" || reader["tax_todate"] == null ? DateTime.Today : Convert.ToDateTime(reader["tax_todate"].ToString())
                        ));
                }
            }
            return Curtaxdetail;
        }





    }
}