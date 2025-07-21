using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AttributeBLL
/// </summary>
public class AttributeBLL
{
    public int AttributeID { get; set; }
    public int CategoryID { get; set; }
    public string Name { get; set; }
    public string DefaultValue { get; set; }
    public string Type { get; set; }
    public int InsertedBy { get; set; }
    public string InsertedIP { get; set; }
    public int ModifiedBy { get; set; }
    public string ModifiedIP { get; set; }
    public int SortOrder { get; set; }
    public string mode { get; set; }
    public int CategoryAttributeID { get; set; }
    public string Value { get; set; }

	public AttributeBLL()
	{
		
	}
    public static List<AttributeBLL> BindAttribute(string mode,int ID)
    {
        AttributeDAL objType = new AttributeDAL();
        return objType.BindAttribute(mode,ID,0);
    }
    public static List<AttributeBLL> BindAttribute(string mode, int ID,int AttrId,string AttrName =null)
    {
        AttributeDAL objType = new AttributeDAL();
        return objType.BindAttribute(mode, ID, AttrId, AttrName);
    }
    public static int InsertAttribute(AttributeBLL objdata)
    {
        AttributeDAL objIns = new AttributeDAL();
        return objIns.InsertAttribute(objdata);
    }
    public static bool UpdateAttribute(AttributeBLL objdata)
    {
        AttributeDAL objUpd = new AttributeDAL();
        return objUpd.UpdateAttribute(objdata);
    }

    //Category Attribute
    public static List<AttributeBLL> BindCatAttribute(string mode, int? ID)
    {
        AttributeDAL objType = new AttributeDAL();
        return objType.BindCatAttribute(mode, ID);
    }
    public static int InsertCategoryAttribute(AttributeBLL objData)
    {
        AttributeDAL objIns = new AttributeDAL();
        return objIns.InsertCategoryAttribute(objData);
    }
}