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
/// Summary description for ProjectOverallratingDAL
/// </summary>
public class ProjectOverallratingDAL
{

    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    private string sqlquery;
    private int numrow;
    private decimal codingconvention, fileStructure, codeOptimization, codeDocumentation, dbStructure;



	public ProjectOverallratingDAL()
	{
	
	}

    public ProjectOverallrating GetOverallratingByPRojId(int projId)
    {
        ProjectOverallrating objGetoveralldetailbyprojid = new ProjectOverallrating();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            sqlquery = "select  count(codeRevId) as numRow,sum(floor(codingConventions/5.0*100.0))as  codingConventions, sum(floor(fileStructure/5.0*100.0)) as  fileStructure,sum(floor(codeOptimization/5.0*100.0)) as codeOptimization,sum(floor(codeDocumentation/5.0*100.0)) as codeDocumentation,sum(floor(dbStructure/5.0*100.0)) as dbStructure from tblcodeRevReport where  projectId=" + projId;
            SqlCommand cmd = new SqlCommand(sqlquery, con);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    numrow = reader["numRow"].ToString() == "" ? 0 : Convert.ToInt32(reader["numRow"]);
                    codingconvention = reader["codingConventions"].ToString() == "" ? 0 : Convert.ToDecimal(reader["codingConventions"]);
                    fileStructure = reader["filestructure"].ToString() == "" ? 0 : Convert.ToDecimal(reader["filestructure"]);
                    codeOptimization=reader["codeOptimization"].ToString() == "" ? 0 : Convert.ToDecimal(reader["codeOptimization"]);
                    codeDocumentation = reader["codeDocumentation"].ToString() == "" ? 0 : Convert.ToDecimal(reader["codeDocumentation"]);
                    dbStructure = reader["dbStructure"].ToString() == "" ? 0 : Convert.ToDecimal(reader["dbStructure"]);
                    objGetoveralldetailbyprojid = new ProjectOverallrating(
                    reader["numRow"].ToString() == "0" ? "---" : Convert.ToDecimal(codingconvention/numrow).ToString() + Convert.ToDecimal(fileStructure/numrow).ToString() + Convert.ToDecimal(codeOptimization/numrow).ToString() + Convert.ToDecimal(codeDocumentation/numrow).ToString() + Convert.ToDecimal(dbStructure/numrow).ToString()
                    );


                }

            }
        }

        catch (Exception ex)
        { }
        return objGetoveralldetailbyprojid;

    }
}