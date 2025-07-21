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
    /// Summary description for taxmasterDAL
    /// </summary>
    public class taxmasterDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private string sqlquery;
        
        public taxmasterDAL()
        {
        }
        public List<taxmaster> Get_taxdetail()
        {
            List<taxmaster> Curtaxdetail = new List<taxmaster>();
            SqlConnection con = new SqlConnection(_strConnection);
            sqlquery = "select * from taxmaster";
            SqlCommand cmd = new SqlCommand(sqlquery, con);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Curtaxdetail.Add(new taxmaster(
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

        public int Inserttax(taxmaster objtax)
        {
            int outputid = 0;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_Inserttax", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@taxname", objtax.taxname);
            cmd.Parameters.AddWithValue("@taxdescript", objtax.taxdescription);
            cmd.Parameters.AddWithValue("@taxrate", objtax.tax_rate);
            cmd.Parameters.AddWithValue("@taxfrmdate", objtax.tax_Fromdate);
            cmd.Parameters.AddWithValue("@taxtodate", objtax.tax_todate);
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

        public bool UpdatetaxBytaxId(taxmaster objtax)
        {
            bool updated = false;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_UpdatetaxBytaxId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@tax_id", objtax.tax_id);
            cmd.Parameters.AddWithValue("@tax_rate", objtax.tax_rate);
            cmd.Parameters.AddWithValue("@tax_fromdate", objtax.tax_Fromdate);
            cmd.Parameters.AddWithValue("@tax_todate", objtax.tax_todate);
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

        public bool DeletetaxById(taxmaster objtax)
        {
            bool updated = false;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_DeletetaxBytaxId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@tax_Id", objtax.tax_id);

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


    }
}