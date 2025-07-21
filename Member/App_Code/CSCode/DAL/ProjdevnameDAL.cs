using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for ProjEmpnameDAL
/// </summary>
public class ProjdevnameDAL
{

    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    String[] arr_string;
    private string sqlquery, coderevname;

    public ProjdevnameDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public Projdevname GetProjempname(string coderevteam)
    {
        Projdevname CurMycodeteam = new Projdevname();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd;
       
        using (con)
        {
            con.Open();
            arr_string = coderevteam.Split(',');
            for (int i = 0; i < arr_string.Length; i++)
            {
                sqlquery = "select empname  from employeemaster where  empid ='" + arr_string[i].ToString() +"'";
                cmd = new SqlCommand(sqlquery, con);
               
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    //rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (coderevname == "" || coderevname == null)
                        { coderevname = "- "+ rdr["empname"].ToString()+"\n"; }
                        else { coderevname += "- " + rdr["empname"].ToString() + "\n"; }

                    }
                }

            }

            CurMycodeteam = new Projdevname(
                  coderevname
                  );
        }
        return CurMycodeteam;

    }
    

}