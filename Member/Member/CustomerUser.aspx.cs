using CSCode;
using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Customer_CustomerUser : Authentication
{
    public string CurCustid = "0";
    public string CurCompanyName = "0";
    UserMaster UM;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["Customer"] != null)
            {
                CurCustid = Request.QueryString["Customer"].ToString();
                try
                {
                    CurCompanyName = customerMaster.GetCustomerByCustId(Convert.ToInt32(CurCustid)).custCompany.ToString();
                    lblCusomerUser.Text = lblCusomerUser.Text + CurCompanyName;
                    hfCompanyName.Value = CurCompanyName;
                }
                catch (Exception ex)
                { }
            }
        }

    }
   
    protected void btnAddCustomerUser_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            Page.Validate();
        string CustId = hfCustID.Value.Replace("'", "''");
        string FName = hftxtFirstName.Value.Replace("'", "''");
        string LName = hftxtLastName.Value.Replace("'", "''");
        string Email = hftxtEmail.Value.Replace("'", "''");
        string Contactno = hftxtContact.Value.Replace("'", "''");
        string IsAdmin = hfchkIsAdmin.Value.Replace("'", "''");
        string SendMail = hfsendmail.Value.Replace("'", "''");
        string Name = FName + " " + LName;

        if (CustId == "" || CustId == "0")
        {
            messageBox("Sorry User Could not created.");
            return;
        }

        string password = Global.RandomPassword(6);
        string encryptPwd = Global.CreatePassword(password);
        UM = UserMaster.UserMasterInfo(); 
        string CurrentIP = LocalIPAddress();
        int createduserid = CustUser.InsertCustomerUser(Convert.ToInt32(UM.EmployeeID), Convert.ToInt32(CustId), FName, LName, Email, encryptPwd, Contactno, IsAdmin, CurrentIP);
        if (createduserid != 0)
        {
            string statusmsg = "Insert Sucesses";
            if (SendMail.ToLower() == "true")
            {
                statusmsg = statusmsg + ". " + Global.SendmailUser(Email, hfCompanyName.Value, Name, Contactno, createduserid.ToString(), password);
            }
            string curUrl = Request.RawUrl;
            messageBoxandSamepage(statusmsg, curUrl);
        }
        else
        {
            messageBox("Sorry User Could not created.");
        }
    }

    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
        
    private void messageBox(string message)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('" + message + "');", true);
    }
    private void messageBoxandSamepage(string message, string Url)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgboxsamepage", "alert('" + message + "');window.location='" + Url + "'; ", true);
    }
    public static string EncryptMD5(string text)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(text, "MD5");
    }
    public string SendmailUser(string toEmailid, string ComapanyName, string Name, string ContactNo, string Login, string Password)
    {
        string StatusStr = "Error in sendin email.";
        try
        {
            System.IO.StreamReader DynamicFileReader = null;
            string fileContent = null;
            DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"../MailTemplates/NewCustomerUser.htm"));

            fileContent = DynamicFileReader.ReadToEnd();
            fileContent = fileContent.Replace("{DateToday}", DateTime.Today.ToString("dd/MM/yyyy"));
            fileContent = fileContent.Replace("{ComapanyName}", ComapanyName);
            fileContent = fileContent.Replace("{Name}", Name);
            fileContent = fileContent.Replace("{Contactno}", ContactNo);
            fileContent = fileContent.Replace("{Login}", Login);
            fileContent = fileContent.Replace("{Password}", Password);
            CSCode.Global.SendMail(fileContent, "New User Created ", ConfigurationManager.AppSettings.Get("fromEmail"), Convert.ToString(toEmailid), false, "", "");
            StatusStr = "";
        }
        catch (Exception ex)
        {
            StatusStr = "Error in sending email. " + ex.Message ;
        }
        return StatusStr;
    }
}