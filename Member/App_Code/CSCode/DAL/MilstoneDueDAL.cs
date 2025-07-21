using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Web.Configuration;

/// <summary>
/// Summary description for MilstoneDueDAL
/// </summary>
public class MilstoneDueDAL
{
    public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    DataTable dt = null;
	public MilstoneDueDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

public DataTable GetMilestoneDue(string mode)
{
    SqlConnection con = new SqlConnection(_strConnection);
    SqlCommand cmd = new SqlCommand("SP_MilestoneDue", con);
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@Mode", mode);
    using (con)
    {
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }
}

}