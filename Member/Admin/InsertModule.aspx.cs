using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

public partial class Admin_InsertModule : Authentication
{
    string prjid = "";
    public string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {

   
        insertModule();


    }

    protected void insertModule()
    {
        string moduleid = "";
        string moduleid1 = "";
        string moduleid2 = "";
        string prosql = "";
        SqlConnection conn = null;
        SqlCommand Cmd1 = null;
        DataSet ds;
        SqlDataAdapter da;
        conn = new SqlConnection(connectionstring);
        conn.Open();
        SqlDataReader Rdr = default(SqlDataReader);

        prosql = "SELECT MAX(moduleId) as moduleId from projectModuleMaster";
        Cmd1 = new SqlCommand(prosql, conn);
        Rdr = Cmd1.ExecuteReader();
        if (Rdr.Read())
        {
            if (!String.IsNullOrEmpty(Rdr["moduleId"].ToString()))
                moduleid = Rdr["moduleId"].ToString();
            else
                moduleid = "0";
        }

        Rdr.Close();
        Cmd1.Dispose();


        prosql = "SELECT DISTINCT projId FROM projectModuleMaster";
        da = new SqlDataAdapter(prosql, conn);
        ds = new DataSet();
        da.Fill(ds);
        int count = ds.Tables[0].Rows.Count;
        
        for (int i = 0; i < count; i++)
        {
            prosql = "SELECT COUNT(*) FROM projectModuleMaster WHERE projId=" + ds.Tables[0].Rows[i]["projId"]+
                     "AND moduleName='Database Design'";
            //Response.Write(ds.Tables[0].Rows[i]["projId"]);
            //Response.End();
            Cmd1 = new SqlCommand(prosql, conn);
            //conn.Open();
            int count2 = Convert.ToInt32(Cmd1.ExecuteScalar());
            //conn.Close();
            //Response.Write(count2.ToString());
            //Response.End();
           
            Cmd1.Dispose();
            Rdr.Close();
           

            if (count2 == 0)
            {
                moduleid1 = moduleid + 1;
                moduleid2 = moduleid + 2;
                moduleid = moduleid2;

                prosql = "INSERT INTO projectModuleMaster(projId,moduleName,moduleRefId)" +
                         "VALUES(" + Convert.ToInt32(ds.Tables[0].Rows[i]["projId"].ToString()) + ",'Database Design'," + 2 + ")";
                Cmd1 = new SqlCommand(prosql, conn);
                Rdr = Cmd1.ExecuteReader();
                Cmd1.Dispose();
                Rdr.Close();

                prosql = "UPDATE projectModuleMaster SET modulerefid=" + moduleid1 +
                         " WHERE moduleid=" + moduleid1;

                Response.Write("Inserted successfully for projectModule=" + "Database Design and ProjID=" + ds.Tables[0].Rows[i]["projId"].ToString());
               
                Cmd1 = new SqlCommand(prosql, conn);
                Rdr = Cmd1.ExecuteReader();
                Cmd1.Dispose();
                Rdr.Close();

                prosql = "INSERT INTO projectModuleMaster(projId,moduleName,moduleRefId)" +
                         "VALUES(" + Convert.ToInt32(ds.Tables[0].Rows[i]["projId"].ToString()) + ",'Web Design'," + 3 + ")";
                Cmd1 = new SqlCommand(prosql, conn);
                Rdr = Cmd1.ExecuteReader();
                Cmd1.Dispose();
                Rdr.Close();

                prosql = "UPDATE projectModuleMaster SET modulerefid=" + moduleid2 +
                         "WHERE moduleid=" + moduleid2;

                Response.Write("Inserted successfully for projectModule=" + "Web Design and ProjID=" + ds.Tables[0].Rows[i]["projId"].ToString());


                Cmd1 = new SqlCommand(prosql, conn);
                Rdr = Cmd1.ExecuteReader();
                Cmd1.Dispose();
                Rdr.Close();

            }
           

        }
        conn.Close();
        Response.Write("Inserted successfully");
    }
}
