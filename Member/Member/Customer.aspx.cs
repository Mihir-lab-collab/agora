using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using Customer.BLL;
using System.Web.Security;

public partial class Customer_Customer : Authentication
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
   
    [System.Web.Services.WebMethod]
    public static String CheckEmailExists(string EmailId)
    {
        string isvalid = "not valid";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(isvalid);
    }
    protected void btnAddCustomer_Click(object sender, EventArgs e)
    {
        string Name = hftxtCustomerName.Value.Replace("'", "''");
        string Company = hftxtCustomerCompany.Value.Replace("'", "''");
        string Address = hftxtAddress.Value.Replace("'", "''");
        string Notes = hftxtNotes.Value.Replace("'", "''");
        string Email = hftxtCustEmail.Value.Replace("'", "''");
        string EmailCC = hftxtCustEmailCC.Value.Replace("'", "''");
        string ShowAllTask = hfIsShowAllTask.Value.Replace("'", "''");
        string CountryName = hfddlCountryId.Value.Replace("'", "''");
        string StateName = hfddlStateId.Value.Replace("'", "''");
        string CityName = hfddlCityId.Value.Replace("'", "''");
        string GSTIN = hftxtGSTIN.Value.Replace("'", "''");
        int custId = customerMaster.InsertCustomer(Name, Company, Email, Address, Notes, EmailCC, Convert.ToBoolean(ShowAllTask),CountryName,StateName,CityName,GSTIN);
        if (custId != 0)
        {

            Response.Redirect("Customer.aspx");
        }
        else
        {
            messageBox("Sorry Customer Could not created.");
        }

    }
    private void messageBox(string message)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('" + message + "');", true);
    }

    [System.Web.Services.WebMethod]
    public static String GetAllCountry()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(customerMaster.GetAllCountry());
    }

    [System.Web.Services.WebMethod]
    public static String GetAllState()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(customerMaster.GetAllState());
    }

    [System.Web.Services.WebMethod]
    public static String GetAllCity()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(customerMaster.GetAllCity());
    }
}