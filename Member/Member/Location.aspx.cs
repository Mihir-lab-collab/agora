using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_Location : Authentication
{
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();

        if (!IsPostBack)
        {
        }
    }

    [System.Web.Services.WebMethod]
    public static bool CheckExistsKeyword(int LocationId, string keyword)
    {
        var data = LocationBLL.CheckExistsKeyword(LocationId, keyword, "CheckKeywordExists");
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return data;
    }


    [System.Web.Services.WebMethod]
    public static String FillCityMaster()
    {
        try
        {
            List<LocationBLL> lstCity = LocationBLL.BindCity("GetCity");

            var data = (from ItemCity in lstCity

                        select new
                        {
                            ItemCity.CityId,
                            ItemCity.CityName
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    [System.Web.Services.WebMethod]
    public static String BindLocation()
    {
        try
        {
            List<LocationBLL> lstLoc = LocationBLL.BindLocation("GetLocation");
            var data = from lstLocItem in lstLoc
                       select new
                       {
                           lstLocItem.LocationID,
                           lstLocItem.Name,
                           lstLocItem.CityId,
                           lstLocItem.CityName,
                           lstLocItem.Biometric,
                           lstLocItem.LegalName,
                           lstLocItem.Address,
                           lstLocItem.PhoneNo,
                           lstLocItem.Fax,
                           lstLocItem.Logo,
                           lstLocItem.Bank,
                           lstLocItem.BankAccount,
                           lstLocItem.WireDetail,
                           lstLocItem.Keyword,
                           lstLocItem.InvoicePDFConfigID,
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    protected void lnkSaveLocation_Click(object sender, EventArgs e)
    {
        string output = "Insert Failed";

        LocationBLL locIns = new LocationBLL();
        locIns.LocationID = 0;
        locIns.Name = hdnLocName.Value;
        locIns.CityId = Convert.ToInt32(hdnCityId.Value);
        //if(locIns.CityId == 0)
        //{
        //    locIns.CityId = null;
        //}
        locIns.Biometric = Convert.ToBoolean(hdnChkBiometric.Value);
        locIns.Logo = hdnLogo.Value;
        locIns.LegalName = hdnCompanyName.Value;
        locIns.Address = hdnCompanyAdd.Value;
        locIns.PhoneNo = hdnPhoneNo.Value;
        locIns.Fax = hdnFax.Value;
        locIns.Bank = hdnBank.Value;
        locIns.BankAccount = hdnBankAccount.Value;
        locIns.WireDetail = hdnWireDetails.Value;
        locIns.Keyword = Convert.ToString(hdnKeyword.Value);
        locIns.InvoicePDFConfigID = Convert.ToInt32(hdnInvoicePDFConfigID.Value);
        locIns.mode = "Insert";
        //locIns.InvoicePDFConfigID = Convert.ToInt32(hdnPDFConfigID.Value);

        int id = LocationBLL.InsertLocation(locIns);
        if (id != 0)
        {
            output = "Inserted successfully.";
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + output + "')", true);
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string UpdateLocation(int LocationID, string LocationName, int CityID, bool Biometric, string CompanyName, string CompanyAddress, string PhoneNo, string Fax, string Logo, string Bank,
        string BankAccount, string WireDetails, string keyword, int InvoicePDFConfigID)
    {
        string output = "Update Failed";
        try
        {
            LocationBLL updateLoc = new LocationBLL();
            updateLoc.LocationID = LocationID;
            updateLoc.Name = LocationName;
            updateLoc.CityId = CityID;
            updateLoc.Biometric = Biometric;
            updateLoc.LegalName = CompanyName;
            updateLoc.Address = CompanyAddress;
            updateLoc.PhoneNo = PhoneNo;
            updateLoc.Fax = Fax;
            updateLoc.Keyword = keyword;
            updateLoc.Logo = Logo;
            updateLoc.Bank = Bank;
            updateLoc.BankAccount = BankAccount;
            updateLoc.WireDetail = WireDetails;
            updateLoc.InvoicePDFConfigID = InvoicePDFConfigID;
            updateLoc.mode = "Update";

            bool isupdated = LocationBLL.UpdateLocation(updateLoc);
            if (isupdated == true)
            {
                output = "updated successfully.";
            }
        }
        catch (Exception ex)
        {

        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }
}