using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Net.Mail;
using System.IO; 

public partial class Admin_Invoice_Mail : Authentication
{
    string payID;
    string strProjectID;
    string strFileName;
    public string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        payID = Convert.ToString(Request.QueryString["invID"]);
        strProjectID = Convert.ToString(Request.QueryString["projectID"]);
        strFileName = strProjectID + "_" + payID + ".pdf";
        if (!IsPostBack)
            getEmail();
    }

    protected void getEmail()
    {
        SqlConnection conn;
        SqlDataAdapter da;
        DataSet ds;
        string strEmail = "";
        string[] arrCustName;
        conn = new SqlConnection(connectionstring);
        string strSql = " SELECT customerMaster.CustName,customerMaster.CustEmail,projectMaster.ProjName " +
                        " FROM paymentMaster " +
                        " INNER JOIN projectMaster " +
                        " ON paymentMaster.PayProjID = projectMaster.ProjID " +
                        " INNER JOIN customerMaster " +
                        " ON customerMaster.CustID = projectMaster.CustID " +
                        " WHERE PayID = " + payID;
     
        da = new SqlDataAdapter(strSql, conn);
        ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            arrCustName = Convert.ToString(ds.Tables[0].Rows[0]["custName"]).Split(' ');
            lblTo.Text = ds.Tables[0].Rows[0]["custEmail"].ToString();
            strEmail = "Dear " + arrCustName[0] + ", \n\n";
            strEmail = strEmail + "Attached is the invoice for " + ds.Tables[0].Rows[0]["projName"].ToString() + ". Kindly acknowledge receipt of this  mail.\n\n";
            strEmail = strEmail + "Wire details are below; \n\n";
            strEmail = strEmail + "Beneficiary Account Name: Intelegain Technologies Pvt. Ltd. \n";
            strEmail = strEmail + "Beneficiary Account Number: 0990317119  \n";
            strEmail = strEmail + "Receiving Bank Name: Citibank N.A   \n\n";
            strEmail = strEmail + "Receiving Bank Swiftbic: CITIINBX    \n\n";
            strEmail = strEmail + "IFSC Code: CITI0100000  \n";
            strEmail = strEmail + "ABA Routing no: 021000089   \n\n";
            strEmail = strEmail + "Receiving Bank Address: \n";
            strEmail = strEmail + "Citibank N.A  \n";
            strEmail = strEmail + "Mahatma Phule Bhavan  \n";
            strEmail = strEmail + "Palm Beach Road Junction \n";
            strEmail = strEmail + "Sector-17, Vashi, New Mumbai – 400705, India  \n\n";
            strEmail = strEmail + "Sincerely,  \n";
            strEmail = strEmail + "Accounts Team  \n";
            strEmail = strEmail + "Intelegain Technologies  \n\n";
            
            txtEmail.Text = strEmail;
            txtSubject.Text = "Invoice #" + payID + " for Project " + ds.Tables[0].Rows[0]["projName"].ToString();

            FileInfo objInfo = new FileInfo(Server.MapPath(@"\admin\invoice\") + strFileName);
            if (objInfo.Exists)
            {
                dvAttachnemt.InnerHtml = "<a href='#' onclick='javascript:window.open(\"http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() +"/admin/Invoice/" + strFileName + "\")'>" + strFileName + "</a>"; 
               
            }
        }
        ds.Dispose();
    }
   
    protected void btnSendInvoice_Click(object sender, EventArgs e)
    {
        string strEmailID = lblTo.Text;
        string strBCC = txtBcc.Text;
        string strCC = txtCc.Text;
        SendMailWithBCC("accounts@intelegain.com", strEmailID, strBCC, strCC, txtSubject.Text, txtEmail.Text.Replace("\n", "<br/>"));
        string strScript = "";
        strScript = "<script language='javascript'>alert('Mail Has Been Sent Successfully!');window.opener.location.reload(true);self.close();</script>";
        /*done by sonal*/
        SqlConnection conn;

        conn = new SqlConnection(connectionstring);
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        string strSql = "Update PaymentMaster set InvoiceSendOn = '" + DateTime.Now + "' WHERE PayID = " + payID;
        cmd.CommandText = strSql;
        cmd.ExecuteNonQuery();
        conn.Close();
        Page.RegisterStartupScript("alertBox", strScript);
    }


    public void SendMailWithBCC(string from, string strToCsvEmail, string strCSVBCCEmail, string strCSVCCEmail, string subject, string message)
    {
        string _strSMTP = System.Configuration.ConfigurationManager.AppSettings["SMTP"].ToString();
        int _intPORT = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"].ToString());

        MailMessage _mailMsg = new MailMessage();
       
        if (strToCsvEmail != "")
        {
            string[] _strArrToEmail;
            strToCsvEmail = strToCsvEmail.Replace(";", ","); 
            _strArrToEmail = strToCsvEmail.Split(',');
            foreach (object _objToEmailID in _strArrToEmail)
                _mailMsg.To.Add(new MailAddress(_objToEmailID.ToString()));
        }

        if (strCSVBCCEmail != "")
        {
            string[] _strArrBCCMail;
            strCSVBCCEmail = strCSVBCCEmail.Replace(";", ","); 
            _strArrBCCMail = strCSVBCCEmail.Split(',');
            foreach (object objBCCEmailID in _strArrBCCMail)
                _mailMsg.Bcc.Add(new MailAddress(objBCCEmailID.ToString()));
        }
        if (strCSVCCEmail != "")
        {
            string[] _strArrCCMail;
            strCSVCCEmail = strCSVCCEmail.Replace(";", ",");
            _strArrCCMail = strCSVCCEmail.Split(',');
            foreach (object objCCEmailID in _strArrCCMail)
                _mailMsg.CC.Add(new MailAddress(objCCEmailID.ToString()));
        }
        _mailMsg.From = new MailAddress(from);
        _mailMsg.Subject = subject;
        _mailMsg.Body = message;
        _mailMsg.IsBodyHtml = true;

        if (dvAttachnemt.InnerHtml != "")
        {
            Attachment attDoc = new Attachment(Server.MapPath(@"\admin\invoice\") + strFileName);
            _mailMsg.Attachments.Add(attDoc);
        }
        SmtpClient smtpClient = new SmtpClient(_strSMTP);
        smtpClient.Port = _intPORT;
        smtpClient.Send(_mailMsg);
        _mailMsg.Dispose();
    }
}

