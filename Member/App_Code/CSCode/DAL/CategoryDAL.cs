using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for CategoryDAL
/// </summary>
public class CategoryDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public static string IPAddress
    {
        get
        {
            return Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        }
    }
    public CategoryDAL()
    {

    }
    public List<CategoryBLL> BindCategory(string mode)
    {
        List<CategoryBLL> category = new List<CategoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Category]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        //cmd.Parameters.AddWithValue("@SupplierID", SupId);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                CategoryBLL obj = new CategoryBLL();
                obj.CategoryID = Convert.ToInt32(Dr["CategoryID"]);
                obj.Name = Convert.ToString(Dr["Name"]);
                obj.Quantity = Convert.ToDouble(Dr["Quantity"]);
                category.Add(obj);
            }
        }
        return category;
    }
    public List<CategoryBLL> BindCategoryByName(string mode, string name)
    {
        List<CategoryBLL> category = new List<CategoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Category]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@Name", name);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                CategoryBLL obj = new CategoryBLL();
                obj.CategoryID = Convert.ToInt32(Dr["CategoryID"]);
                category.Add(obj);
            }
        }
        return category;
    }
    public int InsertCategory(CategoryBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Category", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@ID", objInsert.CategoryID);
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
    public bool UpdateCategory(CategoryBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Category", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@ID", objInsert.CategoryID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
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
    public bool DeleteCategory(CategoryBLL objDelete)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Category", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objDelete.mode);
        cmd.Parameters.AddWithValue("@ID", objDelete.CategoryID);

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

}
