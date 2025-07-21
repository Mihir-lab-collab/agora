using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LocationBLL
/// </summary>
public class LocationBLL
{
    public int LocationID { get; set; }
    public string Name { get; set; }
    //public string CityId { get; set; }
    public int CityId { get; set; }
    public string CityName { get; set; }
    public bool Biometric { get; set; }
    public string LegalName { get; set; }
    public string Address { get; set; }
    public string PhoneNo { get; set; }
    public string Fax { get; set; }
    public string Logo { get; set; }
    public string Bank { get; set; }
    public string BankAccount { get; set; }
    public string WireDetail { get; set; }
    public string mode { get; set; }
    public string Keyword { get; set; }
    public int InvoicePDFConfigID { get; set; }
    public LocationBLL()
    {
    }
    public LocationBLL(int _LocationID, string _Name, int _CityId, string _CityName, bool _Biometric, string _LegalName, string _Address, string _PhoneNo, string _Fax, string _Logo,
        string _Bank, string _BankAccount, string _WireDetail, string _mode, string _Keyword, int _InvoicePDFConfigID)
    {
        this.LocationID = _LocationID;
        this.Name = _Name;
        this.CityId = _CityId;
        this.CityName = CityName;
        this.Biometric = _Biometric;
        this.LegalName = _LegalName;
        this.Address = _Address;
        this.PhoneNo = _PhoneNo;
        this.Fax = _Fax;
        this.Logo = _Logo;
        this.Bank = _Bank;
        this.BankAccount = _BankAccount;
        this.WireDetail = _WireDetail;
        this.mode = _mode;
        this.Keyword = _Keyword;
        this.InvoicePDFConfigID = _InvoicePDFConfigID;
    }
    public static List<LocationBLL> BindLocation(string mode)
    {
        LocationDAL objType = new LocationDAL();
        return objType.BindLocation(mode, 0);
    }
    public static List<LocationBLL> BindLocation(string mode, int LocId)
    {
        LocationDAL objType = new LocationDAL();
        return objType.BindLocation(mode, LocId);
    }
    public static List<LocationBLL> BindCity(string mode)
    {
        LocationDAL objType = new LocationDAL();
        return objType.BindCity(mode);
    }
    public static int InsertLocation(LocationBLL objLocdata)
    {
        LocationDAL objIns = new LocationDAL();
        return objIns.InsertLocation(objLocdata);
    }
    public static bool UpdateLocation(LocationBLL objLocdata)
    {
        LocationDAL objIns = new LocationDAL();
        return objIns.UpdateLocation(objLocdata);
    }
    public static bool CheckExistsKeyword(int LocId, string keyword, string mode)
    {
        LocationDAL objIns = new LocationDAL();
        return objIns.CheckExistsKeyword(LocId, keyword, mode);
    }
}