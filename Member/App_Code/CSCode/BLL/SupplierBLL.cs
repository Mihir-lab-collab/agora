using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SupplierBLL
/// </summary>
public class SupplierBLL
{
    public int SupplierID { get; set; }
    public string Name { get; set; }
    public int CityId { get; set; }
    public string CityName { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public int CountryID { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public int InsertedBy { get; set; }
    public string InsertedIP { get; set; }
    public int ModifiedBy { get; set; }
    public string ModifiedIP { get; set; }
    public int SortOrder { get; set; }
    public string mode { get; set; }

	public SupplierBLL()
	{
	}

    public SupplierBLL(int _SupplierID, string _Name, int _CityId, string _CityName, string _State, string _Country, string _Address, string _Email, string _Mobile, int _InsertedBy,
        string _InsertedIP, int _ModifiedBy, string _ModifiedIP, int _SortOrder, string _mode)
        {
            this.SupplierID = _SupplierID;
            this.Name = _Name;
            this.CityId = _CityId;
            this.CityName = _CityName;
            this.State = _State;
            this.Country = _Country;
            this.Address = _Address;
            this.Email= _Email;
            this.Mobile = _Mobile;
            this.InsertedBy = _InsertedBy;
            this.InsertedIP = InsertedIP;
            this.ModifiedBy = _ModifiedBy;
            this.ModifiedIP = _ModifiedIP;
            this.SortOrder = _SortOrder;
            this.mode = _mode;
        }
    public static List<SupplierBLL> BindSupplier(string mode)
    {
        SupplierDAL objType = new SupplierDAL();
        return objType.BindSupplier(mode);
    }
    public static List<SupplierBLL> BindSupplierPrefix(string mode,string Name)
    {
        SupplierDAL objType = new SupplierDAL();
        return objType.BindSupplierPrefix(mode, Name);
    }
    public static int InsertSupplier(SupplierBLL objSupdata)
    {
        SupplierDAL objIns = new SupplierDAL();
        return objIns.InsertSupplier(objSupdata);
    }
    public static bool UpdateSupplier(SupplierBLL objSupdata)
    {
        SupplierDAL objUpd = new SupplierDAL();
        return objUpd.UpdateSupplier(objSupdata);
    }
    public static List<SupplierBLL> BindCountry(string mode)
    {
        SupplierDAL objType = new SupplierDAL();
        return objType.BindCountry(mode);
    }
}