using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.BLL
{
    public class customerMaster
    {

        public int custId { get; set; }
        public string custName { get; set; }        
        public string custCompany { get; set; }
        public string custAddress { get; set; }
        public DateTime custRegDate { get; set; }
        public string custNotes { get; set; }
        public bool custStatus { get; set; }
        public DateTime? lastLogin { get; set; }
        public int TaskMailLevel { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public string custEmail { get; set; }
        public string custEmailCC { get; set; }
        public bool ShowAllTask { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string GSTIN { get; set; }

        public customerMaster()
        {

        }
        public customerMaster(int _custId, string _custName, string _custCompany, string _custAddress, DateTime _custRegDate, string _custNotes, 
                              bool _custStatus, string _custEmail, int _TaskMailLevel,
                              DateTime _InsertedOn, DateTime _ModifiedOn, string _custEmailCC,
                              bool _ShowAllTask)
        {
            this.custId = _custId;
            this.custName = _custName;
            this.custCompany = _custCompany;
            this.custAddress = _custAddress;
            this.custRegDate = _custRegDate;
            this.custNotes = _custNotes;
            this.custStatus = _custStatus;
            //this.lastLogin = _lastLogin;
            this.custEmail = _custEmail;
            this.TaskMailLevel = _TaskMailLevel;
            this.InsertedOn = _InsertedOn;
            this.ModifiedOn = _ModifiedOn;
            this.custEmailCC = _custEmailCC;
            this.ShowAllTask = _ShowAllTask;
        
        }

        public customerMaster(int _custId, string _custName, string _custCompany, string _custAddress, DateTime _custRegDate, string _custNotes,
                              bool _custStatus, string _custEmail, int _TaskMailLevel,
                              DateTime _InsertedOn, DateTime _ModifiedOn, string _custEmailCC,
                              bool _ShowAllTask,string _CountryName, string _Name, string _CityName, string _GSTIN)
        {
            this.custId = _custId;
            this.custName = _custName;
            this.custCompany = _custCompany;
            this.custAddress = _custAddress;
            this.custRegDate = _custRegDate;
            this.custNotes = _custNotes;
            this.custStatus = _custStatus;
           this.custEmail = _custEmail;
            this.TaskMailLevel = _TaskMailLevel;
            this.InsertedOn = _InsertedOn;
            this.ModifiedOn = _ModifiedOn;
            this.custEmailCC = _custEmailCC;
            this.ShowAllTask = _ShowAllTask;
           
            this.GSTIN = _GSTIN;
            this.CityName = _CityName;
            this.CountryName = _CountryName;
            this.StateName=_Name;
        }

        public static int InsertCustomer(string Name, string Company, string custEmail, string Address, string Notes, string custEmailCC, bool ShowAllTask,string CountryName, string StateName, string CityName, string GSTIN)
        {
            customerMaster curCustomermaster = new customerMaster();
            curCustomermaster.custId = 0;
            curCustomermaster.custName = Name;
            curCustomermaster.custCompany = Company;
            curCustomermaster.custEmail = custEmail;
            curCustomermaster.custEmailCC = custEmailCC;
            curCustomermaster.custAddress = Address;
            curCustomermaster.custRegDate = DateTime.Today;
            curCustomermaster.custNotes = Notes;
            curCustomermaster.custStatus = true;
            curCustomermaster.ShowAllTask = ShowAllTask;
            curCustomermaster.CountryName = CountryName;
            curCustomermaster.StateName = StateName;
            curCustomermaster.CityName = CityName;
            curCustomermaster.GSTIN = GSTIN;
            curCustomermaster.TaskMailLevel = 0;
            curCustomermaster.InsertedOn = DateTime.Now;
            curCustomermaster.ModifiedOn = DateTime.Now;
            customerMasterDAL objcustomerMasterDAL = new customerMasterDAL();
            return objcustomerMasterDAL.InsertCustomer(curCustomermaster);
        }

        public static int UpdateCustomer(int custId, string Name, string Company, string custEmail, string Address, string Notes, string custEmailCC, string ShowAllTask, string CountryName, string StateName, string CityName, string GSTIN)
        {
            customerMaster curCustomermaster = new customerMaster();
            curCustomermaster.custId = custId;
            curCustomermaster.custName = Name;
            curCustomermaster.custCompany = Company;
            curCustomermaster.custEmail = custEmail;
            curCustomermaster.custEmailCC = custEmailCC;
            curCustomermaster.custAddress = Address;           
            curCustomermaster.custNotes = Notes;
            curCustomermaster.ModifiedOn = DateTime.Now;
            curCustomermaster.ShowAllTask = Convert.ToBoolean(ShowAllTask);
            curCustomermaster.CountryName = CountryName;

            if (CountryName == "India")
            {
                curCustomermaster.Name = StateName;
                curCustomermaster.CityName = CityName;
            }
            else

            {
                curCustomermaster.Name = "";
                curCustomermaster.CityName = "";
            }
            curCustomermaster.GSTIN = GSTIN;
            customerMasterDAL objcustomerMasterDAL = new customerMasterDAL();
            return objcustomerMasterDAL.UpdateCustomer(curCustomermaster);
        }
        
        public static customerMaster GetCustomerByCustomerName(string CustomerName)
        {
            customerMasterDAL objcustomerMaster = new customerMasterDAL();
            return objcustomerMaster.GetCustomerByCustomerName(CustomerName);
        }
        
        public static customerMaster GetCustomerByCustId(int CustId)
        {
            customerMasterDAL objcustomerMaster = new customerMasterDAL();
            return objcustomerMaster.GetCustomerByCustId(CustId);
        }
        
        public static List<customerMaster> GetAllCustomers()
        {
            customerMasterDAL objcustomerMaster = new customerMasterDAL();
            return objcustomerMaster.GetAllCustomers();
        }


        public customerMaster(int _CountryId, string _Name)
        {
            this.CountryId = _CountryId;
            this.Name = _Name;
          
           
        }


        public static List<customerMaster> GetAllCountry()
        {
            customerMasterDAL objcountryMaster = new customerMasterDAL();
            return objcountryMaster.GetAllCountry();
        }

        public customerMaster(int _CountryId, string _StateName,int _StateID)
        {
            this.CountryId = _CountryId;
            this.StateName = _StateName;
            this.StateId = _StateID;


        }


        public static List<customerMaster> GetAllState()
        {
            customerMasterDAL objStateMaster = new customerMasterDAL();
            return objStateMaster.GetAllState();
        }

        public customerMaster(int _CountryId, string _CityName, int _StateID,int _CityId)
        {
            this.CountryId = _CountryId;
            this.CityName = _CityName;
            this.StateId = _StateID;
            this.CityId = _CityId;


        }


        public static List<customerMaster> GetAllCity()
        {
            customerMasterDAL objCityMaster = new customerMasterDAL();
            return objCityMaster.GetAllCity();
        }
    }
}