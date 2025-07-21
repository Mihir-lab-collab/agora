using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for TechnicalSkillsDAL
/// </summary>
public class TechnicalSkillsDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
	public TechnicalSkillsDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public List<TechicalSkills> GetSecondarySkills()
    {
        List<TechicalSkills> curSecSkills = new List<TechicalSkills>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_TechnicalSkills", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curSecSkills.Add(new TechicalSkills(
                        Convert.ToInt32(reader["techId"]),
                        reader["skillDesc"].ToString()
                        //Convert.ToInt32(reader["SkillId"]),
                        //reader["Name"].ToString()

                        ));
                }
            }
        }
        catch (Exception ex)
        { throw new ApplicationException(ex.Message); }
        return curSecSkills;
    }

}