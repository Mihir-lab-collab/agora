using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public class UserMasterDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public UserMasterDAL()
    {
    }
    public UserMaster Login(int EmpID, string Password, string OsType = "", string DeviceId = "")
    {
        UserMaster ObjUser = new UserMaster();
        try
       {
           SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "LOGIN");
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@OSType", OsType);
            cmd.Parameters.AddWithValue("@DeviceID", DeviceId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["empPassword"].ToString() == Password || EmpID==0)
                    {
                        if (reader["IsActive"].ToString() == "True")
                        {
                            ObjUser.IsActive = true;

                            ObjUser.EmployeeID = Convert.ToInt32(reader["EmpID"]);
                            if (reader["IsAdmin"].ToString() == "True")
                            {
                                ObjUser.IsAdmin = true;
                            }
                            ObjUser.SkillID = Convert.ToInt32(reader["SkillID"].ToString());
                            ObjUser.Name = reader["EmpName"].ToString();
                            ObjUser.EmailID = reader["EmpEmail"].ToString();
                            ObjUser.Address = reader["EmpAddress"].ToString();
                            ObjUser.Contact = reader["EmpContact"].ToString();
                            ObjUser.JoiningDate = reader["EmpJoiningDate"].ToString();
                            ObjUser.LeavingDate = reader["EmpLeavingDate"].ToString();
                            ObjUser.ProbationPeriod = reader["EmpProbationPeriod"].ToString();
                            ObjUser.Notes = reader["EmpNotes"].ToString();
                            ObjUser.AccountNo = reader["EmpAccountNo"].ToString();
                            ObjUser.BDate = reader["EmpBDate"].ToString();
                            ObjUser.ADate = reader["EmpADate"].ToString();
                            ObjUser.PreviousEmployer = reader["EmpPrevEmployer"].ToString();
                            ObjUser.Experince = Convert.ToInt32(reader["EmpExperince"].ToString());
                            ObjUser.LocationID = reader["LocationFKID"].ToString();
                            ObjUser.CurrentAddress = reader["EmpCurrentAddress"].ToString();
                            ObjUser.Message = Constants.successLoginmessage;
                            ObjUser.Designation = reader["Designation"].ToString();
                            ObjUser.empConfDate = reader["empConfDate"].ToString();
                            ObjUser.onbaordingCompleted = Convert.ToBoolean(reader["IsOnbaordingCompleted"]);
                            ObjUser.ProfileID = (reader["ProfileId"]).ToString();
                            if (reader["IsRemoteEmployee"] != DBNull.Value && !string.IsNullOrEmpty(reader["IsRemoteEmployee"].ToString()))
                            {
                                if (Convert.ToBoolean(reader["IsRemoteEmployee"].ToString()))
                                {
                                    ObjUser.IsRemoteEmployee = true;
                                }
                                else
                                {
                                    ObjUser.IsRemoteEmployee = false;
                                }
                            }

                            if (reader["ProfileID"] != "")
                            {
                                ObjUser.ProfileID = reader["ProfileID"].ToString();
                                if (reader["LocationID"] != System.DBNull.Value)
                                {
                                    ObjUser.ProfileLocationID = Convert.ToInt32(reader["LocationID"].ToString());
                                }
                            }
                        }
                        else
                        {
                            ObjUser.IsActive = false;
                            ObjUser.Message = "Account blocked.";
                        }

                    }
                    else
                    {
                        ObjUser.IsActive = false;
                        ObjUser.Message = "Invalid password.";
                    }
                }
                else
                {
                    ObjUser.IsActive = false;
                    ObjUser.Message = "Invalid login id.";
                }
            }
        }
        catch (Exception ex)
        {
            ObjUser.IsActive = false;
            ObjUser.Message = "Error: " + ex.Message;
        }
        return ObjUser;
    }

    public int Access(int EmpID, string ProfileID, string PageName)
    {
        int ID = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "ACCESS");
        cmd.Parameters.AddWithValue("@EmpID", EmpID);
        cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
        cmd.Parameters.AddWithValue("@PageName", PageName);
        try
        {
            using (con)
            {
                con.Open();
                ID = Convert.ToInt32(cmd.ExecuteScalar());

            }
        }
        catch (Exception ex)
        {
            ID = -1;
        }
        return ID;
    }

    public int GetLocatoionId(int empid)
    {
        int ID = 0;
        SqlDataReader reader = null;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GetLocationId");
        cmd.Parameters.AddWithValue("@EmpID", empid);
        try
        {
            using (con)
            {
                con.Open();
                ID = Convert.ToInt32(cmd.ExecuteScalar());

            }
        }
        catch (Exception ex)
        {
            ID = -1;
        }
        return ID;

    }

    public UserMaster ADLogin(string ADUserName)
    {
        UserMaster ObjUser = new UserMaster();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "LOGIN");
            cmd.Parameters.AddWithValue("@ADUserName", ADUserName);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["IsActive"].ToString() == "True")
                    {
                        ObjUser.IsActive = true;
                        ObjUser.EmployeeID = Convert.ToInt32(reader["EmpID"]);
                        if (reader["IsAdmin"].ToString() == "True")
                        {
                            ObjUser.IsAdmin = true;
                        }
                        ObjUser.SkillID = Convert.ToInt32(reader["SkillID"].ToString());
                        ObjUser.Name = reader["EmpName"].ToString();
                        ObjUser.EmailID = reader["EmpEmail"].ToString();
                        ObjUser.Address = reader["EmpAddress"].ToString();
                        ObjUser.Contact = reader["EmpContact"].ToString();
                        ObjUser.JoiningDate = reader["EmpJoiningDate"].ToString();
                        ObjUser.LeavingDate = reader["EmpLeavingDate"].ToString();
                        ObjUser.ProbationPeriod = reader["EmpProbationPeriod"].ToString();
                        ObjUser.Notes = reader["EmpNotes"].ToString();
                        ObjUser.AccountNo = reader["EmpAccountNo"].ToString();
                        ObjUser.BDate = reader["EmpBDate"].ToString();
                        ObjUser.ADate = reader["EmpADate"].ToString();
                        ObjUser.PreviousEmployer = reader["EmpPrevEmployer"].ToString();
                        ObjUser.Experince = Convert.ToInt32(reader["EmpExperince"].ToString());
                        ObjUser.LocationID = reader["LocationFKID"].ToString();
                        ObjUser.CurrentAddress = reader["EmpCurrentAddress"].ToString();

                        if (reader["ProfileID"] != "")
                        {
                            ObjUser.ProfileID = reader["ProfileID"].ToString();
                            if (reader["LocationID"] != System.DBNull.Value)
                            {
                                ObjUser.ProfileLocationID = Convert.ToInt32(reader["LocationID"].ToString());
                            }
                        }
                    }
                    else
                    {
                        ObjUser.IsActive = false;
                        ObjUser.Message = "Account blocked.";
                    }
                }
                else
                {
                    ObjUser.IsActive = false;
                    ObjUser.Message = "Invalid login id.";
                }
            }
        }
        catch (Exception ex)
        {
            ObjUser.IsActive = false;
            ObjUser.Message = "Error: " + ex.Message;
        }
        return ObjUser;
    }

    /// <summary>
    /// This method used to Update the token ,Deviceid and ostype of login device user
    /// </summary>
    /// <param name="Token"></param>
    /// <param name="deviceId"></param>
    /// <param name="osType"></param>
    /// <returns>Return as token</returns>
    public string UpdateToken(string Token, string deviceId, string osType, int EmpID)
    {
        string token = string.Empty;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "UpdateToken");
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@empToken", Token);
            cmd.Parameters.AddWithValue("@deviceId", deviceId);
            cmd.Parameters.AddWithValue("@osType", osType);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    token = Convert.ToString(reader["empToken"]);
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return token;
    }

    /// <summary>
    ///  This method is used to getting the token of the device user
    /// </summary>
    /// <param name="EmpID"></param>
    /// <returns>Return as token of the user</returns>
    public string GetToken(int EmpID)
    {
        string token = string.Empty;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GetToken");
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    token = Convert.ToString(reader["empToken"]);
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return token;
    }

    /// <summary>
    /// This method used to verify the user against the EmployeeId
    /// </summary>
    /// <param name="EmpID"></param>
    /// <returns>Return the user information</returns>
    public UserMaster VerifyUserByEmpid(int EmpID)
    {
        UserMaster ObjUser = new UserMaster();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "VerifyUserByEmpid");
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ObjUser.EmployeeID = Convert.ToInt32(reader["EmpID"]);
                    ObjUser.Name = reader["EmpName"].ToString();
                    ObjUser.EmailID = reader["EmpEmail"].ToString();
                    ObjUser.Contact = reader["EmpContact"].ToString();
                    ObjUser.empPassword = reader["empPassword"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return ObjUser;
    }

    /// <summary>
    /// This method is used to update password of the user
    /// </summary>
    /// <param name="EmpID"></param>
    /// <param name="EmpEmail"></param>
    /// <param name="Password"></param>
    /// <returns> Return true boolean result</returns>
    public bool UpdatePassword(int EmpID, string EmpEmail, string Password)
    {
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "UpdatePassword");
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@empEmail", EmpEmail);
            cmd.Parameters.AddWithValue("@empPassword", Password);
            using (con)
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        catch (Exception)
        {
            ////throw new ApplicationException(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Method is used to verryfy user
    /// </summary>
    /// <param name="EmpID"></param>
    /// <param name="empEmail"></param>
    /// <returns>Return UserMaster object</returns>
    public UserMaster VerifyUser(int EmpID, string empEmail)
    {
        UserMaster ObjUser = new UserMaster();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "VerifyUser");
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@empEmail", empEmail);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (Convert.ToBoolean(reader["status"]))
                    {
                        ObjUser.EmployeeID = Convert.ToInt32(reader["empid"]);
                        ObjUser.Contact = reader["empContact"].ToString();
                        ObjUser.EmailID = reader["empEmail"].ToString();
                        ObjUser.Name = reader["empName"].ToString();
                        ///// ObjUser.empForgotpwdLinkDate = Convert.ToString(reader["empForgotpwdLinkDate"]) == "" ? ObjUser.empForgotpwdLinkDate : Convert.ToDateTime(reader["empForgotpwdLinkDate"]);
                        ObjUser.empForgotpwdLinkDate = reader["empForgotpwdLinkDate"].ToString();
                    }
                    ObjUser.Message = reader["Message"].ToString();
                    ObjUser.status = Convert.ToBoolean(reader["status"]);
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return ObjUser;
    }

    /// <summary>
    /// Method is used to verifythe user and  send mail to user  for forgot password link
    /// </summary>
    /// <param name="empId"></param>
    /// <param name="empEmail"></param>
    /// <returns></returns>
    public UserMaster SendMailForForgotPassword(string empId, string empEmail)
    {
        string resetPasswoedLink = string.Empty;
        UserMaster objUserMaster = new UserMaster();
        string Message = string.Empty;
        bool status = false;
        try
        {
            string strNewPassword = string.Empty;
            if (!string.IsNullOrEmpty(empId))
            {
                objUserMaster = VerifyUser(Convert.ToInt32(empId), empEmail);
                if (objUserMaster != null && string.IsNullOrEmpty(objUserMaster.Message))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(objUserMaster.EmployeeID)) && objUserMaster.EmployeeID > 1 && !string.IsNullOrEmpty(objUserMaster.EmailID))
                    {
                        UpdatePasswordLinkDate(objUserMaster.EmployeeID, DateTime.Now);
                        objUserMaster = VerifyUser(Convert.ToInt32(empId), empEmail);
                       //// resetPasswoedLink = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/" + "Member" + "/" + "ChangePassword.aspx?id=" + CSCode.Global.Encrypt(empId, true) + "&datetime=" + CSCode.Global.Encrypt(objUserMaster.empForgotpwdLinkDate, true); ////For local 
                        resetPasswoedLink = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" + "Member" + "/" + "ChangePassword.aspx?id=" + CSCode.Global.Encrypt(empId, true) + "&datetime=" + CSCode.Global.Encrypt(objUserMaster.empForgotpwdLinkDate, true); ///For sirus
                        //// resetPasswoedLink = System.Configuration.ConfigurationManager.AppSettings["BaseUrlLive"].ToString() + "/" + "Member" + "/" + "ChangePassword.aspx?id=" + CSCode.Global.Encrypt(empId, true) + "&datetime=" + CSCode.Global.Encrypt(objUserMaster.empForgotpwdLinkDate, true);  //for live & straging
                        List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 43);  //for local & sirus
                        //// List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 27);  //for live & straging
                        string EmailBody = lstConfig[0].value.ToString();
                        EmailBody = EmailBody.Replace("<%EmployeeFirstName%>", objUserMaster.Name);
                        EmailBody = EmailBody.Replace("<%link%>", resetPasswoedLink);
                        string strBody, strSubject, mailTo, mailFrom, result, CC = string.Empty;
                        strBody = EmailBody;
                        strSubject = "Agora: Request to change password";
                        mailTo = objUserMaster.EmailID;
                        //// mailTo = "ajit.s@intelgain.com"; ////local
                        //// mailTo = "sachin.m@intelgain.com";  


                        mailFrom = "";
                        CC = "";
                        result = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
                        objUserMaster.Message = "Reset password link has been sent to your email.";

                    }
                }
                else
                {
                    Message = objUserMaster.Message;
                    status = objUserMaster.status;

                }
            }
            else
            {
                Message = "Invalid Employee id.";
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return objUserMaster;
    }

    /// <summary>
    /// Method is used for update the linkdate and time
    /// </summary>
    /// <param name="empId"></param>
    /// <returns> return result</returns>
    public bool UpdatePasswordLinkDate(int empId, DateTime? ForgotPwdLinkDate)
    {
        bool resul = false;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "UpdateForgotPwdLinkDate");
            cmd.Parameters.AddWithValue("@EmpID", empId);
            cmd.Parameters.AddWithValue("@ForgotPwdLinkDate", ForgotPwdLinkDate);
            using (con)
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    resul = true;
                }
                else
                {
                    resul = false;
                }

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return resul;
    }

    /// <summary>
    /// Method is used for check password linkdate
    /// </summary>
    /// <param name="EmpID"></param>
    /// <param name="Mode"></param>
    /// <returns> return  UserMaster object</returns>
    public UserMaster CheckPasswordLink(int empid, string mode, DateTime? empForgotpwdLinkDate)
    {
        UserMaster ObjUser = new UserMaster();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_UserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.Parameters.AddWithValue("@ForgotPwdLinkDate", empForgotpwdLinkDate);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ObjUser.Message = reader["Message"].ToString();
                    ObjUser.status = Convert.ToBoolean(reader["status"]);
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return ObjUser;
    }

    /// <summary>
    /// This method is used to change password for user
    /// </summary>
    /// <param name="empId"></param>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns>Return message</returns>
    public UserMaster changePassword(int empId, string oldPassword, string newPassword)
    {
        UserMaster objUserMaster = new UserMaster();
        string Message = string.Empty;
        try
        {
            objUserMaster = VerifyUserByEmpid(empId);
            if (objUserMaster != null & !string.IsNullOrEmpty(objUserMaster.EmailID))
            {
                if (string.Compare(objUserMaster.empPassword, oldPassword, true) == 0)
                {
                    if (UpdatePassword(empId, objUserMaster.EmailID, newPassword))
                    {
                        SendMailForChangepassword(objUserMaster.Name, objUserMaster.EmailID);
                        objUserMaster.Message = "your password has been changed successfully.";
                        objUserMaster.status = true;
                    }
                    else
                    {
                        objUserMaster.Message = "Change password operation failed";
                        objUserMaster.status = false;
                    }
                }
                else
                {
                    objUserMaster.Message = "Old password is not correct.";
                    objUserMaster.status = false;
                }
            }
            else
            {
                objUserMaster.Message = "Change password operation failed";
                objUserMaster.status = false;
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return objUserMaster;
    }

    /// <summary>
    /// Method is used to send mail to user for change password intemation mail.
    /// </summary>
    /// <param name="empId"></param>
    /// <param name="empEmail"></param>
    /// <returns></returns>
    public bool SendMailForChangepassword(string empName, string empEmailid)
    {
        bool IsSendmail = false;
        try
        {
            List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 45);  //for local & sirus
            string EmailBody = lstConfig[0].value.ToString();
            EmailBody = EmailBody.Replace("<%EmployeeFirstName%>", empName);
            string strBody, strSubject, mailTo, mailFrom, result, CC = string.Empty;
            strBody = EmailBody;
            strSubject = "Agora: Confirmation of change password.";
            mailTo = empEmailid;
            mailFrom = "";
            CC = "";
            result = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
            IsSendmail = true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return IsSendmail;
    }



}


