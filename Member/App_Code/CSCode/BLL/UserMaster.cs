using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.IO;

[Serializable()]
public class UserMaster
{
    public int EmployeeID;
    public Boolean IsAdmin = false;
    public Boolean IsModuleAdmin = false;
    public int SkillID;
    public string Name;
    public string EmailID;
    public string Address;
    public string Contact;
    public string JoiningDate;
    public string LeavingDate;
    public string ProbationPeriod;
    public string Notes;
    public string AccountNo;
    public string BDate;
    public string ADate;
    public string PreviousEmployer;
    public int Experince;
    public string LocationID;
    public string ProfileID;
    public int ProfileLocationID = 0;
    public Boolean IsActive = false;
    public string Message = "";
    public string UserType = "";
    public string CurrentAddress;
    public string empConfDate;
    public string Designation;
    public bool status;
    public string empForgotpwdLinkDate = null;
    public string empPassword;
    public bool onbaordingCompleted;
    public int Logintype;
    public bool IsRemoteEmployee;

    public UserMaster(int _EmployeeID, Boolean _IsAdmin, Boolean _IsModuleAdmin, int _SkillID,
        string _Name, string _Email, string _Address, string _Contact, string _JoiningDate, string _LeavingDate,
        string _ProbationPeriod, string _Notes, string _AccountNo, string _BDate, string _ADate,
        string _PreviousEmployer, int _Experince, string _LocationID, string _ProfileID, int _ProfileLocationID,
        Boolean _IsActive, string _Message, string _CurrentAddress, int logintype, bool _IsRemoteEmployee)
    {
        this.EmployeeID = _EmployeeID;
        this.IsAdmin = _IsAdmin;
        this.IsModuleAdmin = _IsModuleAdmin;
        this.SkillID = _SkillID;
        this.Name = _Name;
        this.EmailID = _Email;
        this.Address = _Address;
        this.Contact = _Contact;
        this.JoiningDate = _JoiningDate;
        this.LeavingDate = _LeavingDate;
        this.ProbationPeriod = _ProbationPeriod;
        this.Notes = _Notes;
        this.AccountNo = _AccountNo;
        this.BDate = _BDate;
        this.ADate = _ADate;
        this.PreviousEmployer = _PreviousEmployer;
        this.Experince = _Experince;
        this.LocationID = _LocationID;
        this.ProfileID = _ProfileID;
        this.ProfileLocationID = _ProfileLocationID;
        this.IsActive = _IsActive;
        this.Message = _Message;
        this.CurrentAddress = _CurrentAddress;
        Logintype = logintype;
        this.IsRemoteEmployee = _IsRemoteEmployee;
    }

    public UserMaster()
    {

    }

    public UserMaster(Boolean _IsActive, string _Message)
    {
        this.IsActive = _IsActive;
        this.Message = _Message;
    }



    public static void Login(int EmpID, string Password)
    {
        UserMasterDAL Obj = new UserMasterDAL();
        UserMaster ObjUser = Obj.Login(EmpID, Password);
       // HttpContext.Current.Session["SaveCredential"] = null;
        HttpContext.Current.Session["UserMasterSession"] = ObjUser;
    }
    public static void ModuleAdmin(Boolean IsAdmin)
    {
        UserMaster UD = UserMaster.UserMasterInfo();
        UD.IsModuleAdmin = IsAdmin;
        HttpContext.Current.Session["UserMasterSession"] = UD;
    }

    public static UserMaster UserMasterInfo()
    {
        UserMaster ObjUser = null;

        try
        {
            if (HttpContext.Current.Session["UserMasterSession"] == null)
            {
                LogOut();
            }
            else
            {
                ObjUser = HttpContext.Current.Session["UserMasterSession"] as UserMaster;
            }
        }
        catch (Exception e)
        {
            LogOut();
        }
        return ObjUser;
    }

    public void Access(UserMaster UM)
    {
        int Status = 0;
        string StrPage = HttpContext.Current.Request.FilePath;
        UserMasterDAL Obj = new UserMasterDAL();
        Status = Obj.Access(UM.EmployeeID, UM.ProfileID, StrPage);

        if (Status > 0)
        {
            if (Status == 2)
            {
                ModuleAdmin(true);
            }
        }
        else
        {
            this.Message = "Invalid access. (" + Status + ")";
            LogOut();
        }
    }

    public static void LogOut()
    {
        try
        {

            string value = HttpContext.Current.Session["islogout"] as string;

            if (String.IsNullOrEmpty(value))
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("returnUrl", HttpContext.Current.Request.UrlReferrer.ToString()));
            }
            if (HttpContext.Current.Request.Cookies["returnUrl"] != null)
            {
                HttpContext.Current.Response.Cookies["returnUrl"].Expires = DateTime.Now.AddDays(-1);
            }
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Redirect("Login.aspx");
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Redirect("Login.aspx");
        }
    }

    public static int GetLocatoionId(int empid)
    {
        UserMasterDAL Obj = new UserMasterDAL();
        return Obj.GetLocatoionId(empid);

    }

    public static void ADLogin(string ADUserName)
    {
        UserMasterDAL Obj = new UserMasterDAL();
        UserMaster ObjUser = Obj.ADLogin(ADUserName);
        HttpContext.Current.Session["UserMasterSession"] = ObjUser;
    }

    /// <CreatedBy>Ajit Kumar Sa</CreatedBy>
    /// <Createdon>Dt:02-Sep-2016</Createdon>
    /// <summary> Method used to validate login and  return Employee data for Api </summary>
    /// <param name="EmpID">Employee ID</param>
    /// <param name="Password">Employee Password</param>
    /// <returns>User master object</returns>
    public static UserMaster DeviceLogin(int EmpID, string Password, string OsType="", string DeviceId="")
    {
        UserMaster ObjUser;
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            ObjUser = Obj.Login(EmpID, Password, OsType, DeviceId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return ObjUser;

    }

    /// <CreatedBy>Ajit Kumar Sa</CreatedBy>
    /// <Createdon>Dt:02-Sep-2016</Createdon>
    /// <summary>
    /// Method used to after sucessfull login update token,deviceId,osType of the login user in database.
    /// </summary>
    /// <param name="Empid"></param>
    /// <param name="Token"></param>
    /// <returns>Return updated token</returns>
    public static string UpdateToken(string Token, string deviceId, string osType, int EmpID)
    {
        string token = string.Empty;
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            token = Obj.UpdateToken(Token, deviceId, osType, EmpID);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return token;

    }


    /// <CreatedBy>Ajit Kumar Sa</CreatedBy>
    /// <Createdon>Dt:02-Sep-2016</Createdon>
    /// <summary>
    /// Method used to get the token against the user
    /// </summary>
    /// <param name="EmpID"></param>
    /// <returns>Return token</returns>
    public static string GetToken(int EmpID)
    {
        string token = string.Empty;
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            token = Obj.GetToken(EmpID);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return token;
    }


    /// <CreatedBy>Ajit Kumar Sa</CreatedBy>
    /// <Createdon>Dt:08-Sep-2016</Createdon>
    /// <summary>
    /// Method used to verify the user against the employeeId
    /// </summary>
    /// <param name="EmpId"></param>
    /// <returns> Return user information</returns>
    public static UserMaster VerifyUserByEmpid(int EmpId)
    {
        UserMaster ObjUser;
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            ObjUser = Obj.VerifyUserByEmpid(EmpId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return ObjUser;

    }

    /// <CreatedBy>Ajit Kumar Sa</CreatedBy>
    /// <Createdon>Dt:08-Sep-2016</Createdon>
    /// <summary>
    /// This method used to Update password of the user;
    /// </summary>
    /// <param name="EmpId"></param>
    /// <param name="EmailId"></param>
    /// <param name="Password"></param>
    /// <returns>Return as boolean result</returns>
    public static bool UpdatePassword(int EmpId, string EmailId, string Password)
    {
        bool result = false;
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            result = Obj.UpdatePassword(EmpId, EmailId, Password);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return result;
    }

    /// <CreatedBy>Ajit Kumar Sa</CreatedBy>
    /// <Createdon>Dt:08-Sep-2016</Createdon>
    /// <summary>
    /// This method is used for generate a password ,Update the new password in database & send the new password to the user mailId.
    /// </summary>
    /// <param name="empid"></param>
    public static bool SendMail(string empid, string newPassword, bool isChangePassword)
    {
        bool result = false;
        UserMaster objUserMaster = new UserMaster();
        try
        {
            string strNewPassword = string.Empty;
            if (!string.IsNullOrEmpty(empid))
            {
                objUserMaster = UserMaster.VerifyUserByEmpid(Convert.ToInt16(empid));
                if (!string.IsNullOrEmpty(Convert.ToString(objUserMaster.EmployeeID)) && objUserMaster.EmployeeID > 1 && !string.IsNullOrEmpty(objUserMaster.EmailID))
                {
                    Random r = new Random();
                    int intRandom = r.Next();
                    if (!isChangePassword)
                    {
                        strNewPassword = objUserMaster.Name.Trim().Substring(0, 3) + intRandom.ToString().Substring(0, 4);
                    }
                    else
                    {
                        strNewPassword = newPassword.Trim();
                    }
                    UserMaster.UpdatePassword(objUserMaster.EmployeeID, objUserMaster.EmailID, CSCode.Global.CreatePassword(strNewPassword));
                    List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 43);
                    string EmailBody = lstConfig[0].value.ToString();
                    EmailBody = EmailBody.Replace("{UserNmae}", objUserMaster.Name);
                    EmailBody = EmailBody.Replace("{EmpId}", Convert.ToString(objUserMaster.EmployeeID));
                    EmailBody = EmailBody.Replace("{Password}", strNewPassword);
                    string strBody, strSubject, mailTo, mailFrom, message, CC = string.Empty;
                    strBody = EmailBody;
                    if (!isChangePassword)
                    {
                        strSubject = "Agora - Request For Password";
                    }
                    else
                    {
                        strSubject = "Agora - Password Changed";
                    }
                    //// mailTo = objUserMaster.EmailID;
                    mailTo = "ajit.s@intelgain.com";
                    mailFrom = "";
                    CC = "";
                    message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return result;
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="empId"></param>
    /// <param name="empEmail"></param>
    /// <returns></returns>
    public static UserMaster SendMailForForgotPassword(string empId, string empEmail)
    {
        string message = string.Empty;
        UserMaster objusermaster = new UserMaster();
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            objusermaster = Obj.SendMailForForgotPassword(empId, empEmail);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return objusermaster;
    }

    /// <summary>
    /// This method is used to check paaaword link of employee
    /// </summary>
    /// <param name="empid"></param>
    /// <param name="mode"></param>
    /// <returns>return UserMaster </returns>
    public static UserMaster CheckPasswordLink(int empid, string mode, DateTime? empForgotpwdLinkDate)
    {
        UserMaster ObjUser;
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            ObjUser = Obj.CheckPasswordLink(empid, mode, empForgotpwdLinkDate);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return ObjUser;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="empId"></param>
    /// <param name="ForgotPwdLinkDate"></param>
    /// <returns></returns>
    public bool UpdatePasswordLinkDate(int empId, DateTime? ForgotPwdLinkDate)
    {
        bool result = false;
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            result = Obj.UpdatePasswordLinkDate(empId, ForgotPwdLinkDate);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return result;
    }



    /// <summary>
    ///  Method is used for change password
    /// </summary>
    /// <param name="empId"></param>
    /// <param name="empEmail"></param>
    /// <returns>Return message</returns>
    public static UserMaster changePassword(int empId, string oldPassword, string newPassword)
    {
        string message = string.Empty;
        UserMaster objUsermaster = new UserMaster();
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            objUsermaster = Obj.changePassword(empId, oldPassword, newPassword);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return objUsermaster;
    }

    public bool SendMailForChangepassword(string empName, string empEmailid)
    {
        bool isMailSend = false;
        try
        {
            UserMasterDAL Obj = new UserMasterDAL();
            isMailSend = Obj.SendMailForChangepassword(empName, empEmailid);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return isMailSend;
    }

}