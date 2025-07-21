using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrandBLL
/// </summary>
public class BrandBLL
{
    public int BrandID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int InsertedBy { get; set; }
    public string InsertedIP { get; set; }
    public int ModifiedBy { get; set; }
    public string ModifiedIP { get; set; }
    public int SortOrder { get; set; }
    public string mode { get; set; }
 
	public BrandBLL()
	{
		
	}
    public static List<BrandBLL> BindBrand(string mode)
    {
        BrandDAL objType = new BrandDAL();
        return objType.BindBrand(mode);
    }
    public static List<BrandBLL> BindBrandPrefix(string mode,string Name)
    {
        BrandDAL objType = new BrandDAL();
        return objType.BindBrandPrefix(mode, Name);
    }
    public static int InsertBrand(BrandBLL objSupdata)
    {
        BrandDAL objIns = new BrandDAL();
        return objIns.InsertBrand(objSupdata);
    }
    public static bool UpdateBrand(BrandBLL objSupdata)
    {
        BrandDAL objUpd = new BrandDAL();
        return objUpd.UpdateBrand(objSupdata);
    }
    public static bool DeleteBrand(BrandBLL objSupdata)
    {
        BrandDAL objUpd = new BrandDAL();
        return objUpd.DeleteBrand(objSupdata);
    }
}