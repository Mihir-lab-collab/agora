using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CustProfile
/// </summary>
/// 
namespace Customer.BLL
{
    public class Profile
    {
        public Profile()
        {
        }
        public string custId { get; set;}
        public string custName { get; set; }
        public string custCompany { get; set; }
        public string custAddress { get; set; }
        public string custEmail { get; set; }
        public string custPassword { get; set; }

        public Profile(string _CustName, string _Company, string _CustAddress, string _CustEmail, string _Password, string _CustId)
        {
            this.custName = _CustName;
            this.custCompany = _Company;
            this.custAddress = _CustAddress;
            this.custEmail = _CustEmail;
            this.custPassword = _Password;
            this.custId = _CustId;
           
        }
        public static Profile GetCustomerDetails(string custID)
        {
            ProfileDAL objCustProfile = new ProfileDAL();
            return objCustProfile.GetCustomerDetails(custID);
        }
        public static bool UpdateCustomerProfile(string CustName,string Company, string CustAddress, string CustEmail, string Password,string CustId)
        {
            ProfileDAL objCustProfile = new ProfileDAL();
            return objCustProfile.UpdateCustomerProfile(CustName, Company, CustAddress, CustEmail, Password, CustId);
        }

    }

}