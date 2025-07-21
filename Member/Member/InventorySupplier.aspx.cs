using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_InventorySupplier : Authentication
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
    }
    [System.Web.Services.WebMethod]
    public static String BindSupplier()
    {
        try
        {
            List<SupplierBLL> lstSup = SupplierBLL.BindSupplier("Select");
            var data = from lstSupItem in lstSup
                       select new
                       {
                           lstSupItem.SupplierID,
                           lstSupItem.Name,
                           lstSupItem.CityName,
                           lstSupItem.State,
                           lstSupItem.Country,
                           lstSupItem.CountryID,
                           lstSupItem.Address,
                           lstSupItem.Mobile,
                           lstSupItem.Email,
                           lstSupItem.SortOrder,
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    protected void lnkSaveSupplier_Click(object sender, EventArgs e)
    {
        string output = "Insert Failed";

        SupplierBLL SupIns = new SupplierBLL();
        SupIns.SupplierID = 0;
        SupIns.Name = hdnName.Value;
        SupIns.CityName = hdnCity.Value;
        SupIns.State = hdnState.Value;
        SupIns.Country = hdnCountry.Value;
        SupIns.Address = hdnAddress.Value;
        SupIns.Mobile = hdnMobile.Value;

        if (!string.IsNullOrEmpty(hdnSortOrder.Value))
            SupIns.SortOrder = Convert.ToInt32(hdnSortOrder.Value);
        SupIns.Email = hdnEmail.Value;
        SupIns.mode = "Insert";
        SupIns.InsertedBy = UM.EmployeeID;
        int id = SupplierBLL.InsertSupplier(SupIns);
        if (id != 0)
        {
            output = "Inserted successfully.";
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + output + "')", true);
    }
    [System.Web.Services.WebMethod(EnableSession = true)]

    public static string UpdateSupplier(int SupplierID, string Name, string City, string State, string Country, string Address, string Mobile, string Email, int SortOrder)
    {
        string output = "Update Failed";
        try
        {
            SupplierBLL updateSup = new SupplierBLL();
            updateSup.SupplierID = SupplierID;
            updateSup.Name = Name;
            updateSup.CityName = City;
            updateSup.State = State;
            updateSup.Country = Country;
            updateSup.Address = Address;
            updateSup.Mobile = Mobile;
            updateSup.Email = Email;
            updateSup.SortOrder = SortOrder;
            updateSup.ModifiedBy = UM.EmployeeID;
            updateSup.mode = "Update";

            bool isupdated = SupplierBLL.UpdateSupplier(updateSup);
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
    [System.Web.Services.WebMethod]
    public static String FillCountryMaster()
    {
        try
        {
            List<SupplierBLL> lstCountry = SupplierBLL.BindCountry("GetCountry");
            var data = (from Item in lstCountry
                        select new
                        {
                            Item.CountryID,
                            Item.Country
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}