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
public class ProjEmpnameDAL
{

    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    String[] arr_string;
    private string sqlquery, coderevname;

	public ProjEmpnameDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public ProjEmpname GetProjempname(string coderevteam)
    {
        ProjEmpname CurMycodeteam = new ProjEmpname();
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
                        { coderevname = rdr["empname"].ToString(); }
                        else { coderevname += ", " + rdr["empname"].ToString(); }

                    }
                }

            }

            CurMycodeteam=new ProjEmpname(
                  coderevname
                  );
        }
        return CurMycodeteam;

    }

    public ProjEmpname GetProjdevname(string codedevteam)
    {
        ProjEmpname CurMycodeteam = new ProjEmpname();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd;

        using (con)
        {
            con.Open();
            arr_string = codedevteam.Split(',');
            for (int i = 0; i < arr_string.Length; i++)
            {
                sqlquery = "select empname  from employeemaster where  empid ='" + arr_string[i].ToString() + "'";
                cmd = new SqlCommand(sqlquery, con);

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    //rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (codedevteam == "" || coderevname == null)
                        { codedevteam = rdr["empname"].ToString(); }
                        else { codedevteam += ", " + rdr["empname"].ToString(); }

                    }
                }

            }

            CurMycodeteam = new ProjEmpname(
                  codedevteam
                  );
        }
        return CurMycodeteam;

    }

//    public ProjEmpname GetDevID(int Devid)
//    {
//        string devname= String.Empty;
//        ProjEmpname CurMycodeteam = new ProjEmpname();
//        SqlConnection con = new SqlConnection(_strConnection);
//        SqlCommand cmd;

//        using (con)
//        {
//            con.Open();
//                sqlquery = @"SELECT  projId,STUFF((SELECT ', ' + CAST(empId AS VARCHAR(10)) [text()] FROM projectmember  WHERE projId = t.projId FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)'),1,2,' ')devid
//FROM projectmember t where projId=='" + Devid + "' GROUP BY projId ";
//                cmd = new SqlCommand(sqlquery, con);

//                using (SqlDataReader rdr = cmd.ExecuteReader())
//                {
//                    //rdr = cmd.ExecuteReader();
//                    while (rdr.Read())
//                    {
//                        devname = rdr["devid"].ToString();

//                    }
//                }

//            }


//            CurMycodeteam = new ProjEmpname(
//                  devname
//                  );
//         return CurMycodeteam;
//        }
       

    

}