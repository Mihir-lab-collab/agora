using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CategoryBLL
/// </summary>
public class CategoryBLL
{
    public int CategoryID { get; set; }
    public string Name { get; set; }
   
    public int InsertedBy { get; set; }
    public string InsertedIP { get; set; }
    public int ModifiedBy { get; set; }
    public string ModifiedIP { get; set; }
    public int SortOrder { get; set; }
    public string mode { get; set; }
    public double Quantity { get; set; }

	public CategoryBLL()
	{
		
	}
    public static List<CategoryBLL> BindCategory(string mode)
    {
        CategoryDAL objType = new CategoryDAL();
        return objType.BindCategory(mode);
    }
    public static List<CategoryBLL> BindCategory(string mode, string name)
    {
        CategoryDAL objType = new CategoryDAL();
        return objType.BindCategoryByName(mode, name);
    }
    public static int InsertCategory(CategoryBLL objSupdata)
    {
        CategoryDAL objIns = new CategoryDAL();
        return objIns.InsertCategory(objSupdata);
    }
    public static bool UpdateCategory(CategoryBLL objSupdata)
    {
        CategoryDAL objUpd = new CategoryDAL();
        return objUpd.UpdateCategory(objSupdata);
    }
    public static bool DeleteCategory(CategoryBLL objSupdata)
    {
        CategoryDAL objUpd = new CategoryDAL();
        return objUpd.DeleteCategory(objSupdata);
    }
}