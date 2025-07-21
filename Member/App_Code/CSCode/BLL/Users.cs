using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.BLL
{
    /// <summary>
    /// Summary description for CustUsers
    /// </summary>
    public class Users
    {
        public int UserMasterID { get; set; }
        public int CustID { get; set; }
        public string Password { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime LastLogin { get; set; }
        public string LastLoginIP { get; set; }
        public string Status { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ContactNo { get; set; }

        public Users()
        {

        }

        public Users(int _UserMasterID, int _CustID, string _Password, string _FName, string _LName, string _Email,string _ContactNo, bool _IsAdmin, DateTime _LastLogin, string _LastLoginIP, string _Status, DateTime _InsertedOn, DateTime _ModifiedOn)
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

        public static Users login(string UserId, string Password)
        {
            UsersDAL objCurUser = new UsersDAL();
            return objCurUser.login(UserId, Password);
        }

        public static string CustUserDetails(int UserId)
        {
            UsersDAL objCurUser = new UsersDAL();
            return objCurUser.CustUserDetails(UserId);
        }

        public static bool UpdateUserProfile(string FName, string LName, string Email, string Password, string UserMasterID)
        {
            UsersDAL objCustProfile = new UsersDAL();
            return objCustProfile.UpdateUserProfile(FName, LName, Email, Password, UserMasterID);
        }

        public static bool UpdateUserProfileAll(Users _Users)
        {
            UsersDAL objCustProfile = new UsersDAL();
            return objCustProfile.UpdateUserProfileAll(_Users);
        }

        public static bool InsertUserProfile(Users _Users)
        {
            UsersDAL objCustProfile = new UsersDAL();
            return objCustProfile.InsertUserProfile(_Users);
        }

        /// <summary>
        /// Creation Date on :25-07-2013
        /// Created By       :Ram
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public static List<Users> GetAllCustUsersByUserMasterID(int UserMasterID)
        {
            UsersDAL objCustProfile = new UsersDAL();
            return objCustProfile.GetAllCustUsersByUserMasterID(UserMasterID);
        }

        public static List<Users> GetAllCustUsersByCustID(int CustID)
        {
            UsersDAL objCustProfile = new UsersDAL();
            return objCustProfile.GetAllCustUsersByCustID(CustID);
        }


        public static List<Users> GetAllCustUsers()
        {
            UsersDAL objCurUser = new UsersDAL();
            return objCurUser.GetAllCustUsers();
        }
    }
}