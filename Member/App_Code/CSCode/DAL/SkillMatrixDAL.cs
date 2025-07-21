using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Web.Configuration;
using Customer.Model;
using System.Data.SqlTypes;
using System.Xml;

public class SkillMatrixDAL
{
    public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    DataTable dt = null;

	public SkillMatrixDAL()
	{
	}

    public DataTable GetSkills(SkillMatrixBLL objBLL)
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", objBLL.Mode);
        cmd.Parameters.AddWithValue("@SkillName", objBLL.SkillName);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }

    public List<KeyValueModel> GetSkill(int CategoryID)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GetSkill");
        cmd.Parameters.AddWithValue("@SkillCategoryID", CategoryID);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            List<KeyValueModel> list = new List<KeyValueModel>();

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new KeyValueModel()
                {
                    Key = reader["Skill"].ToString(),
                    Value = Convert.ToInt32(reader["SkillID"].ToString())
                });

            }
            return list;
        }
    }

    public List<KeyValueModel> GetCategory()
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GetCategory");
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            List<KeyValueModel> list = new List<KeyValueModel>();

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new KeyValueModel()
                {
                    Key = reader["Category"].ToString(),
                    Value = Convert.ToInt32(reader["SkillCategoryID"].ToString())
                });

            }
            return list;
        }
    }

    public int SaveSkill(SkillMatrixBLL objBLL)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", objBLL.Mode);
        cmd.Parameters.AddWithValue("@UserID", objBLL.UserID);
        cmd.Parameters.AddWithValue("@SkillCategoryID", objBLL.CategoryId);
        cmd.Parameters.AddWithValue("@SkillID", objBLL.SkillID);
        cmd.Parameters.AddWithValue("@SkillName", objBLL.SkillName);
       
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

    public int SaveCategory(SkillMatrixBLL objBLL)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", objBLL.Mode);
        cmd.Parameters.AddWithValue("@UserID", objBLL.UserID);
        cmd.Parameters.AddWithValue("@SkillCategoryID", objBLL.CategoryId);
        cmd.Parameters.AddWithValue("@CategoryName", objBLL.Category);

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

    public DataTable GetEmployeeSkill(SkillMatrixBLL objBLL)
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", objBLL.Mode);
        cmd.Parameters.AddWithValue("@EmpID", objBLL.EmpID);
        cmd.Parameters.AddWithValue("@SkillName", objBLL.SkillName);
        cmd.Parameters.AddWithValue("@ActiveSkill", objBLL.EmpCount);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }
    //-------------------- Employee skill Section
    public List<KeyValueModel> GetEmployee(string mode)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            List<KeyValueModel> list = new List<KeyValueModel>();

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new KeyValueModel()
                {
                    Key = reader["EmpName"].ToString(),
                    Value = Convert.ToInt32(reader["Empid"].ToString())
                });

            }
            return list;
        }
    }

    public string SaveEmployeeSkill(string strXML, SkillMatrixBLL objBLL)
    {        
        string outputid = "0";
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", objBLL.Mode);
        cmd.Parameters.AddWithValue("@UserID", objBLL.UserID);
        cmd.Parameters.AddWithValue("@EmpID", objBLL.EmpID);
        //cmd.Parameters.AddWithValue("@XmlData",strXML); 
        cmd.Parameters.Add("@XmlData", SqlDbType.VarChar, -1).Value = strXML;
        
        try
        {
            using (con)
            {
                outputid = cmd.ExecuteScalar().ToString(); //Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                outputid = "saved successfully";
            }
        }
        catch (Exception ex)
        {
            outputid = ex.ToString();
            return outputid;
        }
        
        return outputid.ToString();
    }

     public string SaveEmployeeSkill1(XmlDocument strXML, SkillMatrixBLL objBLL)
    {        
        string outputid = "0";
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", objBLL.Mode);
        cmd.Parameters.AddWithValue("@UserID", objBLL.UserID);
        cmd.Parameters.AddWithValue("@EmpID", objBLL.EmpID);
        cmd.Parameters.Add(
              new SqlParameter("@XMLFormat", SqlDbType.Xml)
              {
                  Value = new SqlXml(new XmlTextReader(strXML.InnerXml, XmlNodeType.Document, null))
              });

        try
        {
            using (con)
            {
                outputid = cmd.ExecuteScalar().ToString(); //Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                outputid = "saved successfully";
            }
        }
        catch (Exception ex)
        {
            outputid = ex.ToString();
            return outputid;
        }
        
        return outputid.ToString();
    }

    public DataTable GetEmployeeDetail(SkillMatrixBLL objBLL)
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", objBLL.Mode);
        cmd.Parameters.AddWithValue("@SkillID", objBLL.SkillID);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }

    public int CheckEmployeeSkillExists(int EmpID)
    {
        int SkillID;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "CheckSkill");
        cmd.Parameters.AddWithValue("@EmpID", EmpID);

        SkillID =Convert.ToInt32(cmd.ExecuteScalar());
        con.Close();
        return SkillID;
    }

    public string GetRemainderMsg()
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GetRemainderMsg");
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();
        return dt.Rows[0]["Value"].ToString();
    }

    public DataTable GetEmployeeSkillCount(string mode)
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }

    public DataTable GetHighlightSkill(string mode,int empID)
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empID);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }

    public DataTable GetSkillByID(SkillMatrixBLL objBLL)
    {
        dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SkillMatrix", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode",objBLL.Mode);
        cmd.Parameters.AddWithValue("@SkillID", objBLL.SkillID);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }
}