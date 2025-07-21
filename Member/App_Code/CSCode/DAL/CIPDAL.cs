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
/// Summary description for CIPDAL
/// </summary>
public class CIPDAL
{
    public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    DataTable dt = null;
	public CIPDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int Save(CIPBLL objCIPBLL)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_KnowledgeEvents", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode",objCIPBLL.Mode);
        cmd.Parameters.AddWithValue("@KEID", objCIPBLL.KEID);
        cmd.Parameters.AddWithValue("@LocationID", objCIPBLL.LocationId);
        cmd.Parameters.AddWithValue("@EventDate", DateTime.ParseExact(objCIPBLL.EventDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@EventDesc", objCIPBLL.Description);
        cmd.Parameters.AddWithValue("@Time", objCIPBLL.Time);
        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception ex)
        { }
        return outputid;
    }

    public DataTable GetEvents(CIPBLL objCIPBLL)
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_KnowledgeEvents", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", objCIPBLL.Mode);
        cmd.Parameters.AddWithValue("@KEID", objCIPBLL.KEID);
        cmd.Parameters.AddWithValue("@LocationID", objCIPBLL.LocationId);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }

    public void Delete(CIPBLL objCIPBLL)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_KnowledgeEvents", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", objCIPBLL.Mode);
        cmd.Parameters.AddWithValue("@KEID", objCIPBLL.KEID);
        
        try
        {
            using (con)
            {
                cmd.ExecuteScalar();
                con.Close();
            }
        }
        catch (Exception ex)
        { }
    }

    public DataTable GetMailInfo(string mode)
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_KnowledgeEvents", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }
}