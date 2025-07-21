using AgoraBL.BAL;
using AgoraBL.Models;
using CSCode;
using dwtDAL;
using System;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;

public partial class Member_Login : System.Web.UI.Page
{
    private void Page_Load(System.Object sender, System.EventArgs e)

    {
        txtPassword.Attributes["type"] = "password";
        if (!IsPostBack)
        {

            //Added by shubham Alekar//22/12/22-- remember me password//
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                var decryptpassword = Request.Cookies["Password"].Value.ToString();
                txtPassword.Attributes["type"] = "password";
                txtEmpId.Value = Request.Cookies["UserName"].Value.ToString();
                //txtEmpId.Attributes["value"] = Request.Cookies["UserName"].Value.ToString();
                txtPassword.Value = Global.Decrypt(decryptpassword, true);

            }
            if (Request.Cookies["Ad"] != null)
            {
                rblUserType.SelectedValue = Request.Cookies["Ad"].Value;
            }
            Session["islogout"] = string.Empty;
            tblChangepassword.Visible = false;
            if (Session["Message"] != null)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ChangePassword", "<script>alert('" + Session["Message"].ToString() + "');</script>", false);
            }
            Session["Message"] = null;
            if (Request.QueryString["Exp"] != null)
            {
                var message = Request.QueryString["Exp"];
                if (string.Compare(message, "true", true) == 0)
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.RemoveAll();
                    HttpContext.Current.Session.Abandon();
                    lblMessage.Visible = true;
                    lblMessage.Text = "Your session has expired due to inactivity.";
                }

            }

        }
    }
    public void doLogin(object obj, EventArgs eArg)
    {
        if (IsValid)
        {
            Login();
        }
        else
        {
            lblStatus.Visible = true;
            if (txtEmpId.Value.Trim() == "" && txtPassword.Value.Trim() == "")
            {
                lblStatus.Text = "* Mandatory fields. Enter your employee ID and password.";
            }
            else if (txtEmpId.Value.Trim() == "" && txtPassword.Value.Trim() != "")
            {
                lblStatus.Text = "* Mandatory fields. Enter your employee ID.";
            }
            else if (txtEmpId.Value.Trim() == "" && txtPassword.Value.Trim() == "")
            {
                lblStatus.Text = "* Mandatory fields. Enter your valid password.";
            }
        }
    }

    private void Login()
    {
        string EmpID = txtEmpId.Value;
        string Password = txtPassword.Value;
        int loginType = Convert.ToInt32(rblUserType.SelectedValue); // Assume "1" is AD login, else regular login.
        string strError = string.Empty;
        string dominName = Convert.ToString(ConfigurationSettings.AppSettings["DirectoryDomain"]);
        string adPath = Convert.ToString(ConfigurationSettings.AppSettings["DirectoryPath"]);
        ClsLogin loginResult = null; // Initialize variable to store login result


        if (loginType == 2) // Regular login
        {
            loginResult = EmployeeMasterBAL.EmployeeLogin(EmpID, EncryptPassword(Password), (AgoraBL.Models.LoginType)loginType);
        }
        else // AD login
        if (!String.IsNullOrEmpty(dominName) && !String.IsNullOrEmpty(adPath))
        {
            if (true == AuthenticateUser(dominName, EmpID, Password, adPath, out strError))
            {
                loginResult = EmployeeMasterBAL.EmployeeLogin(EmpID, Password, (AgoraBL.Models.LoginType)loginType);
            }
            else
            {
                loginResult.Message = !string.IsNullOrEmpty(strError) && strError.Replace("\r\n", "") == "The server is not operational."
                    ? "AD server is not available."
                      : "Invalid username or password!";

                loginResult.IsActive = false;

            }
        }

        if (loginResult != null && loginResult.IsActive)
        {
            UserMaster ObjUser = new UserMaster
            {
                EmployeeID = Convert.ToInt32(loginResult.EmpId),
                IsActive = loginResult.IsActive,
                Message = loginResult.Message,
                ProfileID = loginResult.ProfileID,
                onbaordingCompleted = loginResult.onbaordingCompleted,
                status = loginResult.status,
                IsAdmin = loginResult.IsAdmin,
                IsModuleAdmin = loginResult.IsModuleAdmin,
                SkillID = loginResult.SkillID,
                Name = loginResult.Name,
                EmailID = loginResult.EmailID,
                Address = loginResult.Address,
                Contact = loginResult.Contact,
                JoiningDate = loginResult.JoiningDate,
                LeavingDate = loginResult.LeavingDate,
                ProbationPeriod = loginResult.ProbationPeriod,
                Notes = loginResult.Notes,
                AccountNo = loginResult.AccountNo,
                BDate = loginResult.BDate,
                ADate = loginResult.ADate,
                PreviousEmployer = loginResult.PreviousEmployer,
                Experince = loginResult.Experince,
                LocationID = loginResult.LocationID,
                ProfileLocationID = loginResult.ProfileLocationID,
                UserType = loginResult.UserType,
                CurrentAddress = loginResult.CurrentAddress,
                empConfDate = loginResult.empConfDate,
                Designation = loginResult.Designation,
                empForgotpwdLinkDate = loginResult.empForgotpwdLinkDate,
                empPassword = loginResult.empPassword,
                IsRemoteEmployee = loginResult.IsRemoteEmployee

            };
            HttpContext.Current.Session["UserMasterSession"] = ObjUser;
            Session["EmpID"] = ObjUser.EmployeeID;

            //  added by satya on 20 nov 2015

            if (ObjUser.IsActive)
            {
                //HttpCookie cookie = cookie = new HttpCookie(EmpID);
                //DateTime dt = DateTime.Now;
                if (chkRemeber.Checked == true)
                {
                    //cookie.Expires = dt.AddDays(1);
                    Response.Cookies.Add(new HttpCookie("UserName", txtEmpId.Value.Trim()));
                    Response.Cookies.Add(new HttpCookie("Password", Global.Encrypt(txtPassword.Value.Trim(), true)));
                    Response.Cookies.Add(new HttpCookie("Ad", rblUserType.SelectedValue));
                    //expire duration 
                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["Ad"].Expires = DateTime.Now.AddDays(30);

                    //Response.Cookies["UserName"].Value = txtEmpId.Value.Trim();
                    //Response.Cookies["Password"].Value = Global.Encrypt(txtPassword.Value.Trim(), true);
                    //Response.Cookies["Ad"].Value = rblUserType.SelectedValue;


                }
                HttpCookie returnCookie = null;//Request.Cookies["returnUrl"];
                if (returnCookie != null || Convert.ToString(returnCookie) != "")
                {
                    Response.Redirect(returnCookie.Value);
                }
                else
                {
                    //if (chkRemeber.Checked)
                    //{
                    //    HttpCookie cookiee = cookie = new HttpCookie(EmpID);
                    //    cookie.Values.Add("EmpID", txtEmpId.Value);
                    //    cookie.Values.Add("Password", Global.Encrypt(txtPassword.Value, true));
                    //    string value = rblUserType.SelectedItem.Value.ToString();
                    //    cookie.Values.Add("Ad", value);

                    //    DateTime dt2 = DateTime.Now;

                    //    Response.Cookies.Add(cookie);
                    //}
                    UserSession wls = new UserSession();
                    wls.CreateUserSessions(ObjUser);
                    string checkonbording = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CheckOnboarding"]);
                    if (string.Compare(ObjUser.Message, "Login success", true) == 0 || string.Compare(ObjUser.Message, "", true) == 0)
                    {
                        if (!ObjUser.onbaordingCompleted)
                        {
                            ObjUser.ProfileID = string.IsNullOrEmpty(ObjUser.ProfileID) ? "0" : ObjUser.ProfileID;
                            if (ObjUser.EmployeeID > Convert.ToInt32(checkonbording) && Convert.ToInt32(ObjUser.ProfileID) != 23)
                            {
                                string encryptedID = Encrypt(ObjUser.EmployeeID.ToString());
                                string url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["OnboardingURL"]) + "=" + encryptedID;
                                Response.Redirect(url, true);
                            }
                            else
                            {
                                Response.Redirect("Home.aspx", false);
                            }

                        }
                        else
                        {
                            Response.Redirect("Home.aspx", false);
                        }
                    }
                }
            }
            else
            {
                lblStatus.Visible = true;
                lblStatus.Text = ObjUser.Message;
            }
        }
        else
        {
            lblStatus.Visible = true;
            lblStatus.Text = loginResult.Message;
        }
    }
    public static string Encrypt(string clearText)
    {
        byte[] salt = new byte[2];
        string password = "sKnc46B3$D68a#4e8F@aB7v^2cQd3cEb47b9";
        //string password = "12345";
        if (!string.IsNullOrEmpty(clearText))
        {
            byte[] _clearBytes = Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes _pdb = new PasswordDeriveBytes(password, salt);
            MemoryStream _ms = new MemoryStream();
            Rijndael _alg = Rijndael.Create();
            _alg.Key = _pdb.GetBytes(32);
            _alg.IV = _pdb.GetBytes(16);
            CryptoStream cs = new CryptoStream(_ms, _alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(_clearBytes, 0, _clearBytes.Length);
            cs.Close();
            byte[] _EncryptedData = _ms.ToArray();
            return Convert.ToBase64String(_EncryptedData);
        }
        else
        {
            return string.Empty;
        }
    }

    private string EncryptPassword(string strPassword)
    {
        string haskey = ConfigurationManager.AppSettings["hashKey"].ToString();
        byte tempbyte = byte.Parse("123");
        byte[] bytearr = new byte[2];
        bytearr[0] = tempbyte;

        return SessionHelper.ComputeHash(txtPassword.Value, haskey, bytearr);
    }

    protected void lnkforgotPassword_Click(object sender, EventArgs e)
    {
        //// SendMail(txtEmpId.Value);
        Response.Redirect("/Member/Forgotpassword.aspx");
    }

    private void SendMail(string empid)
    {
        if (txtEmpId.Value.ToString().Trim() != "")
        {
            clsCommon open = new clsCommon();
            // Check if authorized user
            int EmpID = 0;
            try
            {
                EmpID = Convert.ToInt16(empid);
            }
            catch (Exception)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Not valid user id";
            }

            DataTable dtUserDetails = open.VerifyUser(EmpID);

            if (dtUserDetails.Rows.Count > 0)
            {
                MailMessage msg = new MailMessage();
                MailAddress msgFrom = new MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"),
                    ConfigurationManager.AppSettings.Get("compName"));
                msg.From = msgFrom;

                //  MailAddress msgBCC = new MailAddress(ConfigurationManager.AppSettings.Get("quotationEmailID"));
                foreach (string emailAddress in dtUserDetails.Rows[0]["empEmail"].ToString().Trim().Split(','))
                {
                    msg.To.Add(emailAddress);
                }

                //MailAddress msgCC = new MailAddress(ConfigurationManager.AppSettings.Get("ToBeautifulPecificAdmin"));
                //msg.CC.Add(msgCC);

                msg.IsBodyHtml = true;
                msg.Subject = "Agora - Request For Password";

                msg.Body = CreateMsgBody(dtUserDetails.Rows[0]["empName"].ToString(), dtUserDetails.Rows[0]["empEmail"].ToString(), EmpID, false).ToString();

                SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings.Get("SMTP"));
                mailClient.UseDefaultCredentials = true;
                mailClient.Port = int.Parse(ConfigurationManager.AppSettings.Get("Port"));
                mailClient.EnableSsl = false;



                //SmtpClient mailClient = new SmtpClient();
                //mailClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings.Get("SMTP"));
                //mailClient.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Port"));
                //mailClient.Credentials = new System.Net.NetworkCredential("smtp@intelgain.com", "intelgain");
                //mailClient.EnableSsl = false;

                try
                {
                    mailClient.Send(msg);
                    lblStatus.Visible = true;
                    lblStatus.Text = "Your login details have been send to your mail.";
                    //   success = true;
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = ex.Message;
                }
            }
        }
    }

    private void SendChangepassMail(string empid)
    {
        if (txtEmpId.Value.ToString().Trim() != "")
        {
            clsCommon open = new clsCommon();
            // Check if authorized user
            int EmpID = 0;
            try
            {
                EmpID = Convert.ToInt16(empid);
            }
            catch (Exception)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Not valid user id";
            }

            DataTable dtUserDetails = open.VerifyUser(EmpID);

            if (dtUserDetails.Rows.Count > 0)
            {
                MailMessage msg = new MailMessage();
                MailAddress msgFrom = new MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"),
                    ConfigurationManager.AppSettings.Get("compName"));
                msg.From = msgFrom;

                //  MailAddress msgBCC = new MailAddress(ConfigurationManager.AppSettings.Get("quotationEmailID"));
                foreach (string emailAddress in dtUserDetails.Rows[0]["empEmail"].ToString().Trim().Split(','))
                {
                    msg.To.Add(emailAddress);
                }

                //MailAddress msgCC = new MailAddress(ConfigurationManager.AppSettings.Get("ToBeautifulPecificAdmin"));
                //msg.CC.Add(msgCC);

                msg.IsBodyHtml = true;
                msg.Subject = "Agora - Password Changed";

                msg.Body = CreateMsgBody(dtUserDetails.Rows[0]["empName"].ToString(), dtUserDetails.Rows[0]["empEmail"].ToString(), EmpID, true).ToString();

                SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings.Get("SMTP"));
                mailClient.UseDefaultCredentials = true;

                mailClient.Port = int.Parse(ConfigurationManager.AppSettings.Get("Port"));
                mailClient.EnableSsl = false;

                //SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings.Get("SmtpHost"));

                //mailClient.Port = int.Parse(ConfigurationManager.AppSettings.Get("SmtpPort"));
                //mailClient.Credentials = new System.Net.NetworkCredential("smtp@intelgain.com", "intelgain");
                //mailClient.EnableSsl = false;

                try
                {
                    mailClient.Send(msg);
                    lblStatus.Visible = true;
                    lblStatus.Text = "Your login details have been send to your mail.";
                    //   success = true;
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = ex.Message;
                }
            }
        }
    }

    protected StringBuilder CreateMsgBody(string strUserName, string strEmail, int intEmpID, bool isChangePassword)
    {
        StringBuilder mailBody = new StringBuilder();
        mailBody.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title></title></head><body>");
        mailBody.Append("<table  cellspacing=\"0\" border=\"0\" style=\"background-color: #FFCC00;border-width:1px;font-family:Gill Sans MT;font-size:12px;border-style:double; border-color:#FF9900;\">");
        mailBody.Append("<tr style=\"font-weight:bold;\"><td colspan=\"2\" style=\"padding-left:5px;padding-top:5px\">");
        //Add the header to the email body
        mailBody.Append("Dear " + strUserName + ",");
        mailBody.Append("<br/><br/>We are pleased to send your new password on your request. Please don't forward this mail to any email id for security purpose of your account.<br/><br/>");
        mailBody.Append("</td></tr>");
        Random r = new Random();
        int intRandom = r.Next();

        //string   strNewPassword = strUserName.Trim().Substring(0, 3) + intRandom.ToString().Substring(0, 4);
        string strNewPassword = string.Empty;
        if (!isChangePassword)
        {
            strNewPassword = strUserName.Trim().Substring(0, 3) + intRandom.ToString().Substring(0, 4);
        }
        else
        {
            strNewPassword = txtConfirmPassword.Value.Trim();
        }
        mailBody.Append("<tr>");
        mailBody.Append(string.Format("<td style=\"padding-left:5px;font-weight:bold\" colspan=\"1\">&nbsp;{0}</td>", "User ID:"));
        mailBody.Append(string.Format("<td style=\"width:90%;font-weight:bold\" colspan=\"1\">{0}</td>", intEmpID.ToString()));
        mailBody.Append("</tr>");

        mailBody.Append("<tr>");
        mailBody.Append(string.Format("<td style=\"padding-left:5px;font-weight:bold\" colspan=\"1\">&nbsp;{0}</td>", "Password:"));

        mailBody.Append(string.Format("<td style=\"font-weight:bold\" colspan=\"1\">{0}</td>", strNewPassword));

        mailBody.Append("</tr>");
        mailBody.Append("</table>");

        mailBody.Append("<br/> Regards,");
        mailBody.Append("<br/> Intelgain Pvt Ltd.");

        mailBody.Append("<br />");
        mailBody.Append("</body></html>");
        string strEncrptPassword = string.Empty;
        if (!isChangePassword)
        {
            strEncrptPassword = CreatrePassword(strNewPassword);
        }
        else
        {
            strEncrptPassword = CreatrePassword(txtConfirmPassword.Value.Trim());
        }

        clsCommon open = new clsCommon();
        open.UpdatePassword(strEmail, intEmpID, strEncrptPassword);

        return mailBody;
    }

    private string CreatrePassword(string strPassword)
    {
        string comppwd = null;
        string haskey = ConfigurationManager.AppSettings["hashKey"].ToString();
        byte tempbyte = byte.Parse("123");
        byte[] bytearr = new byte[2];
        bytearr[0] = tempbyte;
        return comppwd = SessionHelper.ComputeHash(strPassword, haskey, bytearr);
    }

    protected void btnchangepass_Click(object sender, EventArgs e)
    {
        try
        {

            SendChangepassMail(txtChnageUserid.Value);
            Response.Redirect(ConfigurationManager.AppSettings["SiteRoot"].ToString() + "Home.aspx");

        }
        catch (Exception)
        {

        }

    }

    public bool AuthenticateUser(string domain, string username, string password, string LdapPath, out string Errmsg)
    {
        Errmsg = string.Empty;
        using (HostingEnvironment.Impersonate())
        {
            DirectoryEntry entry = new DirectoryEntry(LdapPath, username, password, System.DirectoryServices.AuthenticationTypes.Secure);
            try
            {
                // Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(&(objectClass=top)(objectClass=person)(objectClass=organizationalPerson)(objectClass=user))";
                search.PropertyNamesOnly = false;
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                // Update the new path to the user in the directory
                LdapPath = result.Path;
                string _filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                Errmsg = ex.Message;
                return false;
            }
            return true;
        }
    }

}
