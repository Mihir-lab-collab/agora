using CSCode;
using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// Summary description for Customerservice
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Customerservice : System.Web.Services.WebService
{

    public Customerservice()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    [System.Web.Services.WebMethod]
    public String CheckCompanyExists(string ComapnyName)
    {
        string isvalid = "valid";
        customerMaster objcustomerMaster = customerMaster.GetCustomerByCustomerName(ComapnyName);
        if (objcustomerMaster.custCompany != null)
            isvalid = "not valid";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(isvalid);
    }

    [System.Web.Services.WebMethod]
    public String GetAllCustomerDetailsold()
    {

        List<customerMaster> lstcustomerMaster = customerMaster.GetAllCustomers();
        List<CustUser> lstCustUser = CustUser.GetAllFirstUserofAllCustomers();

        //var data = from objcustomerMaster in lstcustomerMaster
        //           join objCustUser in lstCustUser
        //           on objcustomerMaster.custId equals objCustUser.CustID
        //           orderby objcustomerMaster.custId descending
        var data = from objcustomerMaster in lstcustomerMaster
                   join objCustUser in lstCustUser on objcustomerMaster.custId equals objCustUser.CustID into allcustomers
                   from objCustUser in allcustomers.DefaultIfEmpty()
                   orderby objcustomerMaster.custId descending
                   select new
                   {

                       objcustomerMaster.custId,
                       objcustomerMaster.custCompany,
                       objcustomerMaster.custAddress,
                       objcustomerMaster.custRegDate,
                       objcustomerMaster.custNotes,
                       objcustomerMaster.custStatus,
                       objcustomerMaster.TaskMailLevel,
                       objcustomerMaster.InsertedOn,
                       objcustomerMaster.ModifiedOn,
                       CustUserID = objCustUser == null ? 0 : objCustUser.UserMasterID,
                       Password = objCustUser == null ? "" : objCustUser.Password,
                       FName = objCustUser == null ? "" : objCustUser.FName,
                       LName = objCustUser == null ? "" : objCustUser.LName,
                       Email = objCustUser == null ? "" : objCustUser.Email,
                       ContactNo = objCustUser == null ? "" : objCustUser.ContactNo,
                       IsAdmin = objCustUser == null ? true : objCustUser.IsAdmin,
                       LastLogin = objCustUser == null ? DateTime.MinValue : objCustUser.LastLogin,
                       LastLoginIP = objCustUser == null ? "" : objCustUser.LastLoginIP,
                       Status = objCustUser == null ? "" : objCustUser.Status,
                       Name = objCustUser == null ? "" : objCustUser.FName + " " + objCustUser.LName
                   };

        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(data);
    }

    [System.Web.Services.WebMethod]
    public String GetAllCustomerDetailsNew()
    {

        List<customerMaster> lstcustomerMaster = customerMaster.GetAllCustomers();

        var data = from objcustomerMaster in lstcustomerMaster
                   orderby objcustomerMaster.custId descending
                   select new
                   {
                       objcustomerMaster.custId,
                       objcustomerMaster.custName,
                       objcustomerMaster.custCompany,
                       objcustomerMaster.custAddress,
                       objcustomerMaster.custRegDate,
                       objcustomerMaster.custNotes,
                       objcustomerMaster.custStatus,
                       objcustomerMaster.lastLogin,
                       objcustomerMaster.TaskMailLevel,
                       objcustomerMaster.InsertedOn,
                       objcustomerMaster.ModifiedOn,
                       objcustomerMaster.custEmail,
                       objcustomerMaster.custEmailCC,
                       objcustomerMaster.ShowAllTask,
                       objcustomerMaster.CountryName,
                       objcustomerMaster.StateName,
                       objcustomerMaster.CityName,
                       objcustomerMaster.GSTIN
                   };
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(data);
    }

    [System.Web.Services.WebMethod]
    public String CheckUpdatedCompanyNotExists(string custId, string ComapnyName)
    {
        string isvalid = "valid";
        customerMaster objcustomerMaster = customerMaster.GetCustomerByCustomerName(ComapnyName);
        if (objcustomerMaster.custCompany != null)
        {
            if (objcustomerMaster.custId != Convert.ToInt32(custId))
                isvalid = "not valid";
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(isvalid);
    }

    [System.Web.Services.WebMethod]
    public String UpdateCustomer(string custId, string custName, string custCompany, string custEmail, string custAddress, string custNotes, string custEmailCC,string ShowAllTask,string CountryName, string StateName, string CityName,string GSTIN)
    {
        string statusmsg = "Update Sucesses";
        int isupadted = customerMaster.UpdateCustomer(Convert.ToInt32(custId), custName, custCompany, custEmail, custAddress, custNotes, custEmailCC, ShowAllTask, CountryName, StateName, CityName, GSTIN);
        if (isupadted == 0)
            statusmsg = "Update Failed";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(statusmsg);
    }

    [System.Web.Services.WebMethod]
    public String GetAllCustUsersByCustID(int CustId)
    {
        List<CustUser> lstCustUser = CustUser.GetAllCustUsersByCustID(CustId);
        var data = from objCustUser in lstCustUser
                   orderby objCustUser.UserMasterID descending
                   select new
                   {
                       objCustUser.UserMasterID,
                       objCustUser.CustID,
                       objCustUser.Password,
                       objCustUser.FName,
                       objCustUser.LName,
                       objCustUser.Email,
                       ContactNo = objCustUser.ContactNo.Trim(),
                       objCustUser.IsAdmin,
                       objCustUser.Status,
                       objCustUser.InsertedOn,
                       objCustUser.ModifiedOn,
                       Name = objCustUser.FName + " " + objCustUser.LName,

                   };
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(data);
    }

    [System.Web.Services.WebMethod]
    public String UpdateCustomerUser(string UserMasterID, string CustId, string FName, string LName, string Email, string Contactno, string IsAdmin, string Isactive, string sendmail, string password)
    {
        string statusmsg = "Update Sucesses";
        string encryptPwd = "";

        if (sendmail.ToLower() == "true")
        {
            password = Global.RandomPassword(6);
            encryptPwd = Global.CreatePassword(password);
        }

        int isupadted = CustUser.UpdateCustomerUser(Convert.ToInt32(UserMasterID), FName, LName, Email, Contactno, IsAdmin, Isactive,encryptPwd);
        if (isupadted != 0)
        {
            string Name = FName + " " + LName;
            if (sendmail.ToLower() == "true")
            {
                string CompanyName = customerMaster.GetCustomerByCustId(Convert.ToInt32(CustId)).custCompany.ToString();
                statusmsg = statusmsg + " and " + Global.SendmailUser(Email, CompanyName, Name, Contactno, UserMasterID, password);
            }


        }
        else
            statusmsg = "Update Failed";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(statusmsg);
    }

}
