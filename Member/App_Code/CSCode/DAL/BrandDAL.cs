using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for BrandDAL
/// </summary>
public class BrandDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public static string IPAddress
    {
        get
        {
            return Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        }
    }
	public BrandDAL()
	{
		
	}
    public List<BrandBLL> BindBrand(string mode)
    {
        List<BrandBLL> brand = new List<BrandBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Brand]", con);
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
                BrandBLL obj = new BrandBLL();
                obj.BrandID = Convert.ToInt32(Dr["BrandID"]);
                obj.Name = Convert.ToString(Dr["Name"]);
                obj.Description = Convert.ToString(Dr["Description"]);
                brand.Add(obj);
            }
        }
        return brand;
    }
    public List<BrandBLL> BindBrandPrefix(string mode,string Name)
    {
        List<BrandBLL> brand = new List<BrandBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Brand]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@Name", Name);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                BrandBLL obj = new BrandBLL();
                obj.BrandID = Convert.ToInt32(Dr["BrandID"]);
                obj.Name = Convert.ToString(Dr["Name"]);
              
                brand.Add(obj);
            }
        }
        return brand;
    }
    public int InsertBrand(BrandBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Brand", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@BrandID", objInsert.BrandID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
        cmd.Parameters.AddWithValue("@Description", objInsert.Description);
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
    public bool UpdateBrand(BrandBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Brand", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@BrandID", objInsert.BrandID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
        cmd.Parameters.AddWithValue("@Description", objInsert.Description);
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
    public bool DeleteBrand(BrandBLL objDelete)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Brand", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objDelete.mode);
        cmd.Parameters.AddWithValue("@BrandID", objDelete.BrandID);

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