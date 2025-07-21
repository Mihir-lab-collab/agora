using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for SupplierDAL
/// </summary>
public class SupplierDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public static string IPAddress
    {
        get
        {
            return Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        }
    }
    public SupplierDAL()
    {
    }
    public List<SupplierBLL> BindSupplier(string mode)
    {
        List<SupplierBLL> supplier = new List<SupplierBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Supplier]", con);
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
                SupplierBLL obj = new SupplierBLL();
                obj.SupplierID = Convert.ToInt32(Dr["SupplierID"]);
                obj.Name = Convert.ToString(Dr["Name"]);
                // obj.CityId = Convert.ToInt32(Dr["City"]);
                obj.CityName = Convert.ToString(Dr["City"]);
                obj.State = Convert.ToString(Dr["State"]);
                obj.Country = Convert.ToString(Dr["Country"]);
                obj.CountryID = Convert.ToInt32(Dr["CountryID"]);
                obj.Address = Convert.ToString(Dr["Address"]);
                obj.Email = Convert.ToString(Dr["Email"]);
                obj.Mobile = Convert.ToString(Dr["Mobile"]);
                obj.SortOrder = Convert.ToInt32(Dr["SortOrder"]);

                supplier.Add(obj);
            }
        }
        return supplier;
    }
    public List<SupplierBLL> BindSupplierPrefix(string mode, string Name)
    {
        List<SupplierBLL> supplier = new List<SupplierBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Supplier]", con);
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
                SupplierBLL obj = new SupplierBLL();
                obj.SupplierID = Convert.ToInt32(Dr["SupplierID"]);
                obj.Name = Convert.ToString(Dr["Name"]);

                supplier.Add(obj);
            }
        }
        return supplier;
    }
    public List<SupplierBLL> BindCountry(string mode)
    {
        List<SupplierBLL> supplier = new List<SupplierBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Supplier]", con);
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
                SupplierBLL obj = new SupplierBLL();
                obj.Country = Convert.ToString(Dr["Country"]);
                obj.CountryID = Convert.ToInt32(Dr["CountryID"]);
                supplier.Add(obj);
            }
        }
        return supplier;
    }
    public int InsertSupplier(SupplierBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Supplier", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@SupplierID", objInsert.SupplierID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
        cmd.Parameters.AddWithValue("@City", objInsert.CityName);
        cmd.Parameters.AddWithValue("@State", objInsert.State);
        cmd.Parameters.AddWithValue("@Country", objInsert.Country);
        cmd.Parameters.AddWithValue("@Email", objInsert.Email);
        cmd.Parameters.AddWithValue("@Mobile", objInsert.Mobile);
        cmd.Parameters.AddWithValue("@Address", objInsert.Address);
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
    public bool UpdateSupplier(SupplierBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Supplier", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@SupplierID", objInsert.SupplierID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
        cmd.Parameters.AddWithValue("@City", objInsert.CityName);
        cmd.Parameters.AddWithValue("@State", objInsert.State);
        cmd.Parameters.AddWithValue("@Country", objInsert.Country);
        cmd.Parameters.AddWithValue("@Email", objInsert.Email);
        cmd.Parameters.AddWithValue("@Mobile", objInsert.Mobile);
        cmd.Parameters.AddWithValue("@Address", objInsert.Address);
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.ModifiedBy);
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
}