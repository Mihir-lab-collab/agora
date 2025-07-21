using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for AttributeDAL
/// </summary>
public class AttributeDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public static string IPAddress
    {
        get
        {
            return Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        }
    }
	public AttributeDAL()
	{
		
	}
    public List<AttributeBLL> BindAttribute(string mode,int? catID,int? AttrId,string AttrName =null)
    {
        List<AttributeBLL> attribute = new List<AttributeBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Attribute]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@CategoryID", catID);
        cmd.Parameters.AddWithValue("@AttributeID", AttrId);
        cmd.Parameters.AddWithValue("@Name", AttrName);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                AttributeBLL obj = new AttributeBLL();
                obj.AttributeID = Convert.ToInt32(Dr["AttributeID"]);
                obj.CategoryID = Convert.ToInt32(Dr["CategoryID"]);
                obj.Name = Convert.ToString(Dr["Name"]);
                obj.DefaultValue = Convert.ToString(Dr["DefaultValue"]);
                obj.Type = Convert.ToString(Dr["Type"]);
                obj.SortOrder = Convert.ToInt32(Dr["SortOrder"]);

                attribute.Add(obj);
            }
        }
        return attribute;
    }
    public List<AttributeBLL> BindCatAttribute(string mode, int? ID)
    {
        List<AttributeBLL> attribute = new List<AttributeBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Attribute]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@AttributeID", ID);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                AttributeBLL obj = new AttributeBLL();
                obj.AttributeID = Convert.ToInt32(Dr["CategoryAttributeID"]);
                obj.AttributeID = Convert.ToInt32(Dr["AttributeID"]);
                obj.Value = Convert.ToString(Dr["Value"]);
                obj.SortOrder = Convert.ToInt32(Dr["SortOrder"]);

                attribute.Add(obj);
            }
        }
        return attribute;
    }
    public int InsertAttribute(AttributeBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Attribute", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@CategoryID", objInsert.CategoryID);
        cmd.Parameters.AddWithValue("@AttributeID", objInsert.AttributeID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
        cmd.Parameters.AddWithValue("@DefaultValue", objInsert.DefaultValue);
        cmd.Parameters.AddWithValue("@Type", objInsert.Type);
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.InsertedBy);
        cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
        cmd.Parameters.AddWithValue("@SortOrder", objInsert.SortOrder);

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
    public bool UpdateAttribute(AttributeBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Attribute", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@CategoryID", objInsert.CategoryID);
        cmd.Parameters.AddWithValue("@AttributeID", objInsert.AttributeID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
        cmd.Parameters.AddWithValue("@DefaultValue", objInsert.DefaultValue);
        cmd.Parameters.AddWithValue("@Type", objInsert.Type);
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.InsertedBy);
        cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
        cmd.Parameters.AddWithValue("@SortOrder", objInsert.SortOrder);

        try
        {
            using (con)
            {
                con.Open();
                int output = Convert.ToInt32(cmd.ExecuteScalar());
                if (output == 1)
                    updated = true;
            }
        }
        catch (Exception ex)
        { }
        return updated;
    }
    public int InsertCategoryAttribute(AttributeBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Attribute", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@AttributeID", objInsert.AttributeID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.InsertedBy);
        cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
        cmd.Parameters.AddWithValue("@SortOrder", objInsert.SortOrder);

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
}