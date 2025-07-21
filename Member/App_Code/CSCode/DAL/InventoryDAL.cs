using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for InventoryDAL
/// </summary>
public class InventoryDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public static string IPAddress
    {
        get
        {
            return Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        }
    }
    public InventoryDAL()
    {
    }
    //Inventory 
    public List<InventoryBLL> BindInvoiceDetails(string mode)
    {
        List<InventoryBLL> inventory = new List<InventoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_InvoiceDetails]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                InventoryBLL obj = new InventoryBLL();
                obj.ItemInvoiceID = Convert.ToInt32(Dr["ItemInvoiceID"]);
                obj.InvoiceNo = Convert.ToString(Dr["InvoiceNo"]);
                obj.Price = Convert.ToDouble(Dr["TotalPrice"]);
                obj.Supplier = Convert.ToString(Dr["Supplier"]);
                obj.PurchaseDate = Convert.ToDateTime(Dr["PurchaseDate"]);
                obj.Note = Convert.ToString(Dr["Note"]);
                inventory.Add(obj);
            }
        }
        return inventory;
    }
    public List<InventoryBLL> BindInvoiceDetailsById(string mode, int ItemId)
    {
        List<InventoryBLL> inventory = new List<InventoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_InvoiceDetails]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@ItemInvoiceID", ItemId);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                InventoryBLL obj = new InventoryBLL();
                obj.ItemInvoiceID = Dr["ItemInvoiceID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["ItemInvoiceID"]);
                obj.InvoiceNo = Dr["InvoiceNo"] == DBNull.Value ? "" : Convert.ToString(Dr["InvoiceNo"]);
                obj.PurchaseDate = Convert.ToDateTime(Dr["PurchaseDate"]);
                obj.Note = Dr["Note"] == DBNull.Value ? "" : Convert.ToString(Dr["Note"]);
                obj.ItemID = Dr["ItemID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["ItemID"]);

                obj.Price = Dr["Price"] == DBNull.Value ? 0.0 : Convert.ToDouble(Dr["Price"]);
                obj.Description = Dr["Description"] == DBNull.Value ? "" : Convert.ToString(Dr["Description"]);
                obj.Quantity = Dr["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["Quantity"]);
                obj.ExpiryDate = Dr["ExpiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Dr["ExpiryDate"]);


                obj.SerialNo = Dr["SerialNo"] == DBNull.Value ? "" : Convert.ToString(Dr["SerialNo"]); ;
                obj.SupplierID = Dr["SupplierID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["SupplierID"]);

                obj.Supplier = Dr["Supplier"] == DBNull.Value ? "" : Convert.ToString(Dr["Supplier"]);
                obj.CategoryID = Dr["CategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["CategoryID"]); ;
                obj.CategoryName = Dr["Category"] == DBNull.Value ? "" : Convert.ToString(Dr["Category"]);
                obj.BrandID = Dr["BrandID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["BrandID"]);

                obj.BrandName = Dr["Brand"] == DBNull.Value ? "" : Convert.ToString(Dr["Brand"]);
                obj.ItemAttributeID = Dr["ItemAttributeID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["ItemAttributeID"]);
                obj.Value = Dr["Value"] == DBNull.Value ? "" : Convert.ToString(Dr["Value"]);
                obj.AttributeID = Dr["AttributeID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["AttributeID"]);
                obj.AttributeName = Dr["AttributeName"] == DBNull.Value ? "" : Convert.ToString(Dr["AttributeName"]);
                obj.DefaultValue = Dr["DefaultValue"] == DBNull.Value ? "" : Convert.ToString(Dr["DefaultValue"]);
                obj.Type = Dr["Type"] == DBNull.Value ? "" : Convert.ToString(Dr["Type"]);
                obj.VatPercentage = Dr["VatPercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(Dr["VatPercentage"]);

                obj.STPercentage = Dr["STPercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(Dr["STPercentage"]);
                obj.CGSTPercentage= Dr["CGSTPercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(Dr["CGSTPercentage"]); //Added By Nikhil Shetye for GST implementation
                obj.SGSTPercentage = Dr["SGSTPercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(Dr["SGSTPercentage"]); //Added By Nikhil Shetye for GST implementation
                inventory.Add(obj);
            }
        }
        return inventory;
    }
    public List<InventoryBLL> BindInvoiceDetailsByCategory(string mode, int CatId)
    {
        List<InventoryBLL> inventory = new List<InventoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_InvoiceDetails]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@ItemInvoiceID", CatId);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                InventoryBLL obj = new InventoryBLL();
                obj.ItemInvoiceID = Convert.ToInt32(Dr["ItemInvoiceID"]);
                obj.InvoiceNo = Convert.ToString(Dr["InvoiceNo"]);
                obj.Price = Convert.ToDouble(Dr["Price"]);
                obj.Supplier = Convert.ToString(Dr["Supplier"]);
                obj.PurchaseDate = Convert.ToDateTime(Dr["PurchaseDate"]);
                obj.Note = Convert.ToString(Dr["Note"]);
                obj.ItemID = Convert.ToInt32(Dr["ItemID"]);
                obj.Description = Convert.ToString(Dr["Description"]);
                obj.Quantity = Convert.ToInt32(Dr["Quantity"]);
                obj.ExpiryDate = Convert.ToDateTime(Dr["ExpiryDate"]);
                obj.SerialNo = Convert.ToString(Dr["SerialNo"]);
                obj.SupplierID = Convert.ToInt32(Dr["SupplierID"]);
                obj.CategoryID = Convert.ToInt32(Dr["CategoryID"]);
                obj.CategoryName = Convert.ToString(Dr["Category"]);
                obj.BrandID = Convert.ToInt32(Dr["BrandID"]);
                obj.BrandName = Convert.ToString(Dr["Brand"]);
                inventory.Add(obj);
            }
        }
        return inventory;
    }
    public List<InventoryBLL> GetInvoiceIdByInvoiceNo(string mode, string InvoiceNo)
    {
        List<InventoryBLL> Invoiceid = new List<InventoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_InvoiceDetails]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@InvoiceNo", @InvoiceNo);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                InventoryBLL obj = new InventoryBLL();
                obj.ItemInvoiceID = Convert.ToInt32(Dr["ItemInvoiceID"]);

                Invoiceid.Add(obj);
            }
        }
        return Invoiceid;
    }
    public int InsertItemInvoice(InventoryBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_InvoiceDetails", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@SupplierID", objInsert.SupplierID);
        cmd.Parameters.AddWithValue("@InvoiceNo", objInsert.InvoiceNo);
        cmd.Parameters.AddWithValue("@ItemInvoiceID", objInsert.ItemInvoiceID);
        cmd.Parameters.AddWithValue("@PurchaseDate", objInsert.PurchaseDate);
        cmd.Parameters.AddWithValue("@Note", objInsert.Note);
        cmd.Parameters.AddWithValue("@VatPercentage", 0);  //Passed 0 By Nikhil Shetye on 12-10-2017 for GST implementation
        cmd.Parameters.AddWithValue("@STPercentage", 0);  //Passed 0 Nikhil Shetye on 12-10-2017 for GST implementation
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.InsertedBy);
        cmd.Parameters.AddWithValue("@InsertedIp", IPAddress);
        //Added By Nikhil Shetye on 12-10-2017 for GST implementation
        cmd.Parameters.AddWithValue("@CGSTPercentage", objInsert.CGSTPercentage); 
        cmd.Parameters.AddWithValue("@SGSTPercentage", objInsert.SGSTPercentage);


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
    public bool UpdateItemInvoice(InventoryBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_InvoiceDetails", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@SupplierID", objInsert.SupplierID);
        cmd.Parameters.AddWithValue("@InvoiceNo", objInsert.InvoiceNo);
        cmd.Parameters.AddWithValue("@ItemInvoiceID", objInsert.ItemInvoiceID);
        cmd.Parameters.AddWithValue("@PurchaseDate", objInsert.PurchaseDate);
        cmd.Parameters.AddWithValue("@Note", objInsert.Note);
        cmd.Parameters.AddWithValue("@VatPercentage", 0); //Passing 0 value By Nikhil Shetye on 12/10/2017 for GST implementation
        cmd.Parameters.AddWithValue("@STPercentage", 0); //Passing 0 value By Nikhil Shetye on 12/10/2017 for GST implementation
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.InsertedBy);
        cmd.Parameters.AddWithValue("@InsertedIp", IPAddress);
        //Added By Nikhil Shetye on 12/10/2017 for GST implementation
        cmd.Parameters.AddWithValue("@CGSTPercentage", objInsert.CGSTPercentage);
        cmd.Parameters.AddWithValue("@SGSTPercentage", objInsert.SGSTPercentage);

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
    public List<InventoryBLL> BindCatExpireQuantity(string mode)
    {
        List<InventoryBLL> category = new List<InventoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Inventory]", con);
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
                InventoryBLL obj = new InventoryBLL();
                obj.CategoryID = Convert.ToInt32(Dr["CategoryID"]);
                obj.Quantity = Convert.ToInt32(Dr["ExpiredQuantity"]);
                category.Add(obj);
            }
        }
        return category;
    }
    public List<InventoryBLL> BindLastPDP(string mode)
    {
        List<InventoryBLL> category = new List<InventoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Inventory]", con);
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
                InventoryBLL obj = new InventoryBLL();
                obj.CategoryID = Convert.ToInt32(Dr["CategoryID"]);
                obj.PurchaseDate = Convert.ToDateTime(Dr["DateOfPurchase"]);
                obj.Price = Convert.ToDouble(Dr["price"]);
                obj.ItemInvoiceID = Convert.ToInt32(Dr["ItemInvoiceID"]);

                category.Add(obj);
            }
        }
        return category;
    }

    //InventoryItem
    public List<InventoryBLL> BindItem(string mode, int ItemId)
    {
        List<InventoryBLL> Item = new List<InventoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_Item]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@_ItemID", ItemId);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                InventoryBLL obj = new InventoryBLL();
                obj.ItemID = Convert.ToInt32(Dr["ItemID"]);
                obj.BrandName = Convert.ToString(Dr["Brand"]);
                obj.Supplier = Convert.ToString(Dr["Supplier"]);
                obj.Price = Convert.ToDouble(Dr["Price"]);
                obj.Quantity = Convert.ToInt32(Dr["Quantity"]);
                obj.ExpiryDate = Convert.ToDateTime(Dr["ExpDate"]);
                obj.Description = Convert.ToString(Dr["Description"]);
                obj.SerialNo = Convert.ToString(Dr["SerialNo"]);
                obj.CategoryID = Convert.ToInt32(Dr["CategoryID"]);
                obj.CategoryName = Convert.ToString(Dr["CategoryName"]);
                obj.SupplierID = Convert.ToInt32(Dr["SupplierID"]);
                obj.BrandID = Convert.ToInt32(Dr["BrandID"]);
                obj.PurchaseDate = Convert.ToDateTime(Dr["PurchaseDate"]);
                Item.Add(obj);
            }
        }
        return Item;
    }
    public int InsertItem(InventoryBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Item", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@_ItemID", objInsert.ItemID);
        cmd.Parameters.AddWithValue("@BrandID", objInsert.BrandID);
        cmd.Parameters.AddWithValue("@CategoryID", objInsert.CategoryID);
        cmd.Parameters.AddWithValue("@ItemInvoiceID", objInsert.ItemInvoiceID);
        cmd.Parameters.AddWithValue("@Description", objInsert.Description);
        cmd.Parameters.AddWithValue("@Price", objInsert.Price);
        cmd.Parameters.AddWithValue("@ExpiryDate", objInsert.ExpiryDate);
        cmd.Parameters.AddWithValue("@Quantity", objInsert.Quantity);
        cmd.Parameters.AddWithValue("@SerialNo", objInsert.SerialNo);
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.InsertedBy);
        cmd.Parameters.AddWithValue("@InsertedIP", IPAddress);
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
    public bool UpdateItem(InventoryBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Item", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@_ItemID", objInsert.ItemID);
        cmd.Parameters.AddWithValue("@BrandID", objInsert.BrandID);
        cmd.Parameters.AddWithValue("@CategoryID", objInsert.CategoryID);
        cmd.Parameters.AddWithValue("@ItemInvoiceID", objInsert.ItemInvoiceID);
        cmd.Parameters.AddWithValue("@Description", objInsert.Description);
        cmd.Parameters.AddWithValue("@Price", objInsert.Price);
        cmd.Parameters.AddWithValue("@ExpiryDate", objInsert.ExpiryDate);
        cmd.Parameters.AddWithValue("@Quantity", objInsert.Quantity);
        cmd.Parameters.AddWithValue("@SerialNo", objInsert.SerialNo);
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.InsertedBy);
        cmd.Parameters.AddWithValue("@InsertedIP", IPAddress);
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

    //InventoryItemAttribute
    public List<InventoryBLL> BindItemAttribute(string mode, int ItemId, int AttributeId)
    {
        List<InventoryBLL> ItemAttribute = new List<InventoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_ItemAttribute]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@AttributeID", AttributeId);
        cmd.Parameters.AddWithValue("@ItemorItemAttID", ItemId);

        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                InventoryBLL obj = new InventoryBLL();
                obj.ItemAttributeID = Convert.ToInt32(Dr["ItemAttributeID"]);
                obj.Value = Convert.ToString(Dr["Value"]);
                obj.AttributeID = Convert.ToInt32(Dr["AttributeID"]);
                obj.ItemID = Convert.ToInt32(Dr["ItemID"]);
                ItemAttribute.Add(obj);
            }
        }
        return ItemAttribute;
    }
    public List<InventoryBLL> BindItemAttribute(string mode, int ItemId)
    {
        List<InventoryBLL> ItemAttribute = new List<InventoryBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("[SP_ItemAttribute]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@ItemorItemAttID", ItemId);

        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                InventoryBLL obj = new InventoryBLL();
                obj.ItemAttributeID = Convert.ToInt32(Dr["ItemAttributeID"]);
                obj.Value = Convert.ToString(Dr["Value"]);
                obj.AttributeID = Convert.ToInt32(Dr["AttributeID"]);
                obj.ItemID = Convert.ToInt32(Dr["ItemID"]);
                obj.AttributeName = Convert.ToString(Dr["AttributeName"]);
                obj.Type = Convert.ToString(Dr["Type"]);
                ItemAttribute.Add(obj);
            }
        }
        return ItemAttribute;
    }
    public int InsertItemAttribute(InventoryBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_ItemAttribute", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@ItemorItemAttID", objInsert.ItemID);
        cmd.Parameters.AddWithValue("@AttributeID", objInsert.AttributeID);
        cmd.Parameters.AddWithValue("@Value", objInsert.Value);
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.InsertedBy);
        cmd.Parameters.AddWithValue("@InsertedIP", IPAddress);
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
    public bool UpdateItemAttribute(InventoryBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ItemAttribute", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@ItemorItemAttID", objInsert.ItemID);
        cmd.Parameters.AddWithValue("@AttributeID", objInsert.AttributeID);
        cmd.Parameters.AddWithValue("@Value", objInsert.Value);
        cmd.Parameters.AddWithValue("@InsertedBy", objInsert.InsertedBy);
        cmd.Parameters.AddWithValue("@InsertedIP", IPAddress);
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