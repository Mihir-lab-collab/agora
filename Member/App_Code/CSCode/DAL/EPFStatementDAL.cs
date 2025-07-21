using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Customer.BLL;

using System.IO;
using System.Web.UI;


/// <summary>
/// Summary description for EPFStatementDAL
/// </summary>
public class EPFStatementDAL 
{
    private static string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    
    public EPFStatementDAL()
    {

    }

    public DataSet GetEpfStatement(int Month, int Year)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlDataAdapter SDA = new SqlDataAdapter();
        DataSet DS = new DataSet();
       // EPFStatement objEpf = new EPFStatement();
        
        List<EPFStatement> objEPFStatementList = new List<EPFStatement>();
        try
        {
           
            SqlCommand cmd = new SqlCommand("USP_EPFStatement", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@month", Month);
            cmd.Parameters.AddWithValue("@year", Year);
            
            using (con)
            {
                con.Open();
                SDA = new SqlDataAdapter(cmd);
                SDA.Fill(DS);
            }
        }
        catch (Exception ex)
        {
            //ex.WriteErrorLog();
        }
        return DS;
    }

    
}