using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace Customer.BLL
{

    public class CustUser
    {
        public int UserMasterID { get; set; }
        public int CustID { get; set; }
        public string Password { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime LastLogin { get; set; }
        public string LastLoginIP { get; set; }
        public string Status { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime ModifiedOn { get; set; }


        public CustUser()
        {

        }

        public CustUser(int _UserMasterID, int _CustID, string _Password, string _FName, string _LName, string _Email, string _ContactNo, bool _IsAdmin, DateTime _LastLogin, string _LastLoginIP, string _Status, DateTime _InsertedOn, DateTime _ModifiedOn)
        {
            this.UserMasterID = _UserMasterID;
            this.CustID = _CustID;
            this.Password = _Password;
            this.FName = _FName;
            this.LName = _LName;
            this.Email = _Email;
            this.ContactNo = _ContactNo;
            this.IsAdmin = _IsAdmin;
            this.LastLogin = _LastLogin;
            this.LastLoginIP = _LastLoginIP;
            this.Status = _Status;
            this.InsertedOn = _InsertedOn;
            this.ModifiedOn = _ModifiedOn;
        }
        public static int InsertCustomerUser(int userMasterID, int custId, string FName, string LName, string Email, string password, string Contactno, string IsAdmin, string CurLoginIP)
        {           
            CustUser curCustUser = new CustUser();
            curCustUser.UserMasterID = userMasterID;
            curCustUser.CustID = custId;
            curCustUser.FName = FName;
            curCustUser.LName = LName;
            curCustUser.Email = Email;
            curCustUser.Password = password;
            curCustUser.ContactNo = Contactno;
            if (IsAdmin == "true")
                curCustUser.IsAdmin = true;
            else
                curCustUser.IsAdmin = false;
            curCustUser.Status = "a";           
            curCustUser.LastLoginIP = CurLoginIP;
            CustUserDAL objCustUserDAL = new CustUserDAL();
            return objCustUserDAL.InsertCustomerUser(curCustUser);
        }

        public static int UpdateCustomerUser(int UserMasterID, string FName, string LName, string Email, string Contactno, string IsAdmin, string Isactive,string password)
        {
            CustUser curCustUser = new CustUser();
            curCustUser.UserMasterID = UserMasterID;
            curCustUser.FName = FName;
            curCustUser.LName = LName;
            curCustUser.Email = Email;
            curCustUser.Password = password;
            curCustUser.ContactNo = Contactno;
            if (IsAdmin == "true")
                curCustUser.IsAdmin = true;
            else
                curCustUser.IsAdmin = false;
            if (Isactive == "true")
                curCustUser.Status = "a";
            else
                curCustUser.Status = "i";
            curCustUser.ModifiedOn = DateTime.Now;
            CustUserDAL objCustUserDAL = new CustUserDAL();
            return objCustUserDAL.UpdateCustomerUser(curCustUser);
        }
        public static List<CustUser> GetAllFirstUserofAllCustomers()
        {
            CustUserDAL objCustUser = new CustUserDAL();
            return objCustUser.GetAllFirstUserofAllCustomers();
        }
        public static List<CustUser> GetAllCustUsersByCustID(int CustID)
        {
            CustUserDAL objCustUser = new CustUserDAL();
            return objCustUser.GetAllCustUsersByCustID(CustID);
        }

        public static List<CustUser> GetAllCustUsers()
        {
            CustUserDAL objCustUser = new CustUserDAL();
            return objCustUser.GetAllCustUsers();
        }
    }
}