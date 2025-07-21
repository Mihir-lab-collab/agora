using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CountryBLL
/// </summary>
public class CountryBLL
{
	public int CountryID { get; set; }
	public string Country { get; set; }
	//public int SecurityLevel { get; set;}
	public CountryBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}
	public CountryBLL(int CountryID, string Country)
	{
		this.CountryID = CountryID;
		this.Country = Country;
	}
	public static List<CountryBLL> GetCountry(string mode)
	{
		return GetCountry(mode, 0);
	}

	private static List<CountryBLL> GetCountry(string mode, int CountryID)
	{
		CountryDAL objCountryDAL = new CountryDAL();
		return objCountryDAL.GetCountryDetails(mode,CountryID);		
	}
    public static int SaveCountry(string mode, int countryID, string country)
    {
        CountryBLL objCountry = new CountryBLL();
        objCountry.CountryID = countryID;
        objCountry.Country = country;
        CountryDAL objCountryDAL = new CountryDAL();
        return objCountryDAL.SaveCountry(mode, objCountry);
    }
}