using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for InventoryBLL
/// </summary>
public class InventoryBLL
{
    public InventoryBLL()
    {
    }
    public int InsertedBy { get; set; }
    public string InsertedIP { get; set; }
    public int ModifiedBy { get; set; }
    public string ModifiedIP { get; set; }
    public string mode { get; set; }
 //   public double TotalPrice { get; set; }
    public int ItemInvoiceID { get; set; }
    public int ItemID { get; set; }
    public string InvoiceNo { get; set; }
    public int SupplierID { get; set; }
    public string Supplier { get; set; }
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public int BrandID { get; set; }
    public string BrandName { get; set; }
    public int AttributeID { get; set; }
    public string AttributeName { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime PurchaseDate { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public decimal VatPercentage { get; set; }
    public decimal STPercentage { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public string SerialNo { get; set; }
    public int ItemAttributeID { get; set; }
    public string Value { get; set; }
    public string DefaultValue { get; set; }
    public string Type { get; set; }
    public int SortOrder { get; set; }
    public decimal CGSTPercentage { get; set; } //Added By Nikhil Shetye on 12-10-2017 for GST implementation
    public decimal SGSTPercentage { get; set; } //Added By Nikhil Shetye on 12-10-2017 for GST implementation

    public static List<InventoryBLL> BindInvoiceDetails(string mode)
    {
        InventoryDAL objType = new InventoryDAL();
        return objType.BindInvoiceDetails(mode);
    }
    public static List<InventoryBLL> BindCatExpireQuantity(string mode)
    {
        InventoryDAL objType = new InventoryDAL();
        return objType.BindCatExpireQuantity(mode);
    }
    public static List<InventoryBLL> GetInvoiceIdByInvoiceNo(string mode,string InvoiceNo)
    {
        InventoryDAL objType = new InventoryDAL();
        return objType.GetInvoiceIdByInvoiceNo(mode, InvoiceNo);
    }
    public static List<InventoryBLL> BindLastPDP(string mode)
    {
        InventoryDAL objType = new InventoryDAL();
        return objType.BindLastPDP(mode);
    }
    public static int InsertItemInvoice(InventoryBLL objdata)
    {
        InventoryDAL objIns = new InventoryDAL();
        return objIns.InsertItemInvoice(objdata);
    }
    public static bool UpdateItemInvoice(InventoryBLL objdata)
    {
        InventoryDAL objUpd = new InventoryDAL();
        return objUpd.UpdateItemInvoice(objdata);
    }
    public static List<InventoryBLL> BindInvoiceDetailsById(string mode,int ItemId)
    {
        InventoryDAL objType = new InventoryDAL();
        return objType.BindInvoiceDetailsById(mode, ItemId);
    }
    public static List<InventoryBLL> BindInvoiceDetailsByCategory(string mode, int catId)
    {
        InventoryDAL objType = new InventoryDAL();
        return objType.BindInvoiceDetailsByCategory(mode, catId);
    }
    //InventoryItem
    public static List<InventoryBLL> BindItem(string mode, int ItemId)
    {
        InventoryDAL objType = new InventoryDAL();
        return objType.BindItem(mode, ItemId);
    }
    public static int InsertItem(InventoryBLL objdata)
    {
        InventoryDAL objIns = new InventoryDAL();
        return objIns.InsertItem(objdata);
    }
    public static bool UpdateItem(InventoryBLL objdata)
    {
        InventoryDAL objUpd = new InventoryDAL();
        return objUpd.UpdateItem(objdata);
    }

    //InventoryItemAttribute
    public static List<InventoryBLL> BindItemAttribute(string mode, int ItemId)
    {
        InventoryDAL objType = new InventoryDAL();
        return objType.BindItemAttribute(mode, ItemId);
    }
    public static List<InventoryBLL> BindItemAttribute(string mode, int ItemId, int AttrbuteId)
    {
        InventoryDAL objType = new InventoryDAL();
        return objType.BindItemAttribute(mode, ItemId, AttrbuteId);
    }
    public static int InsertItemAttribute(InventoryBLL objdata)
    {
        InventoryDAL objIns = new InventoryDAL();
        return objIns.InsertItemAttribute(objdata);
    }
    public static bool UpdateItemAttribute(InventoryBLL objdata)
    {
        InventoryDAL objUpd = new InventoryDAL();
        return objUpd.UpdateItemAttribute(objdata);
    }
}