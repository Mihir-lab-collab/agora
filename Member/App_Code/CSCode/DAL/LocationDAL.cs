using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for LocationDAL
/// </summary>
public class LocationDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    public LocationDAL()
    {
    }
    public List<LocationBLL> BindLocation(string mode, int? LocId)
    {
        List<LocationBLL> location = new List<LocationBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Location", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@LocationID", LocId);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                LocationBLL obj = new LocationBLL();
                obj.LocationID = Convert.ToInt32(Dr["LocationID"]);
                obj.Name = Convert.ToString(Dr["Name"]);
                if (Dr["CityFKID"] is DBNull)
                {
                    obj.CityId = 0;
                }
                else
                {
                    obj.CityId = Convert.ToInt32(Dr["CityFKID"]);
                }
                //obj.CityId = Convert.ToInt32(Dr["CityFKID"]);
                obj.CityName = Convert.ToString(Dr["CityName"]);
                obj.Biometric = Convert.ToBoolean(Dr["Biometric"]);
                obj.LegalName = Convert.ToString(Dr["LegalName"]);
                obj.Address = Convert.ToString(Dr["Address"]);
                obj.PhoneNo = Convert.ToString(Dr["Tel"]);
                //if (Dr["Fax"] is DBNull)
                //{
                //    obj.Fax = "";
                //}
                //else
                //{
                //    obj.Fax = Convert.ToString(Dr["Fax"]);
                //}
                obj.Fax = Convert.ToString(Dr["Fax"]);
                obj.Logo = Convert.ToString(Dr["Logo"]);
                obj.Bank = Convert.ToString(Dr["Bank"]);
                obj.BankAccount = Convert.ToString(Dr["BankAccount"]);
                obj.WireDetail = Convert.ToString(Dr["WireDetail"]);
                obj.Keyword = Convert.ToString(Dr["Keyword"]);
                if (Dr["InvoicePDFConfigID"] is DBNull)
                {
                    obj.InvoicePDFConfigID = 0;
                }
                else
                {
                    obj.InvoicePDFConfigID = Convert.ToInt32(Dr["InvoicePDFConfigID"]);
                }
                // obj.InvoicePDFConfigID = Convert.ToInt32(Dr["InvoicePDFConfigID"]); 
                location.Add(obj);
            }
        }
        return location;
    }
    public List<LocationBLL> BindCity(string mode)
    {
        List<LocationBLL> location = new List<LocationBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Location", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                LocationBLL obj = new LocationBLL();
                obj.CityId = Convert.ToInt32(Dr["cityid"]);
                obj.CityName = Convert.ToString(Dr["cityName"]);
                location.Add(obj);
            }
            //location.Add(new LocationBLL { CityId = -1, CityName = "Select City" });
        }
        return location;
    }
    public int InsertLocation(LocationBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Location", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@LocationID", objInsert.LocationID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
        if (objInsert.CityId == 0)
        {
            cmd.Parameters.AddWithValue("@CityFKID", null);
        }
        else
        {
            cmd.Parameters.AddWithValue("@CityFKID", objInsert.CityId);
        }

        cmd.Parameters.AddWithValue("@Biometric", objInsert.Biometric);
        cmd.Parameters.AddWithValue("@LegalName", objInsert.LegalName);
        cmd.Parameters.AddWithValue("@Address", objInsert.Address);
        cmd.Parameters.AddWithValue("@Tel", objInsert.PhoneNo);
        cmd.Parameters.AddWithValue("@Fax", objInsert.Fax);
        cmd.Parameters.AddWithValue("@Logo", objInsert.Logo);
        cmd.Parameters.AddWithValue("@Bank", objInsert.Bank);
        cmd.Parameters.AddWithValue("@BankAccount", objInsert.BankAccount);
        cmd.Parameters.AddWithValue("@WireDetail", objInsert.WireDetail);
        cmd.Parameters.AddWithValue("@Keyword", objInsert.Keyword);
        cmd.Parameters.AddWithValue("@InvoicePDFConfigID", objInsert.InvoicePDFConfigID);
        //cmd.Parameters.AddWithValue("@InvoicePDFConfigID", objInsert.InvoicePDFConfigID);
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
    public bool UpdateLocation(LocationBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Location", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@LocationID", objInsert.LocationID);
        cmd.Parameters.AddWithValue("@Name", objInsert.Name);
        if (objInsert.CityId == 0)
        {
            cmd.Parameters.AddWithValue("@CityFKID", null);
        }
        else
        {
            cmd.Parameters.AddWithValue("@CityFKID", objInsert.CityId);
        }

        cmd.Parameters.AddWithValue("@Biometric", objInsert.Biometric);
        cmd.Parameters.AddWithValue("@LegalName", objInsert.LegalName);
        cmd.Parameters.AddWithValue("@Address", objInsert.Address);
        cmd.Parameters.AddWithValue("@Tel", objInsert.PhoneNo);
        cmd.Parameters.AddWithValue("@Fax", objInsert.Fax);
        cmd.Parameters.AddWithValue("@Logo", objInsert.Logo);
        cmd.Parameters.AddWithValue("@Bank", objInsert.Bank);
        cmd.Parameters.AddWithValue("@BankAccount", objInsert.BankAccount);
        cmd.Parameters.AddWithValue("@WireDetail", objInsert.WireDetail);
        cmd.Parameters.AddWithValue("@Keyword", objInsert.Keyword);
        cmd.Parameters.AddWithValue("@InvoicePDFConfigID", objInsert.InvoicePDFConfigID);
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
    public bool CheckExistsKeyword(int LocId, string Keyword, string mode)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Location", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@LocationID", LocId);
        cmd.Parameters.AddWithValue("@Keyword", Keyword);
        cmd.Parameters.AddWithValue("@mode", mode);
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