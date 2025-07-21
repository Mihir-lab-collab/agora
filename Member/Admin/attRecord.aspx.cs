using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Admin_attRecord : System.Web.UI.Page
{

    string strEmailID;
    string strattDate;
    SqlConnection strConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ToString());
    string strResponse = "notdone";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["empID"] != null && Request.QueryString["attDate"] != null && Request.QueryString["ip"] != null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Add_Att";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empid", Request.QueryString["empID"]);
                cmd.Parameters.AddWithValue("@attdate", Request.QueryString["attDate"]);
                cmd.Parameters.AddWithValue("@ip", Request.QueryString["ip"]);
                strConn.Open();
                cmd.Connection = strConn;
                cmd.ExecuteNonQuery();
                strConn.Close();
                strResponse = "done";
            }
            catch (Exception ex){ 
		Response.Write(ex.Message.ToString());
}
        }
        Response.Write(strResponse);

    }
}