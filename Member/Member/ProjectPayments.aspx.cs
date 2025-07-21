using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Customer.BLL;
using Customer.DAL;
using dwtDAL;
using System.Configuration;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using Customer.Model;

public partial class Member_ProjectPayments : Authentication
{
    //private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    UserMaster UM;
       
    [System.Web.Services.WebMethod]
    public static String BindInvoices(int ProjID, string FromDate,string ToDate)
    {
        DateTime frm = DateTime.ParseExact(FromDate, "d/M/yyyy", CultureInfo.InvariantCulture);
        FromDate = frm.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        DateTime to = DateTime.ParseExact(ToDate, "d/M/yyyy", CultureInfo.InvariantCulture);
        ToDate = to.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);        
        try
        {   
            
            GetInvoice objInvoice = new GetInvoice();
            List<GetInvoice> InvoiceList = objInvoice.GetInvoices(ProjID, FromDate, ToDate);
           
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string str = jss.Serialize(InvoiceList);
            return str;

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetMailInfo(int PaymentID)
    {
                
        try
        {
            List<ProjecInvoiceBLL> lstProjecInvoice = ProjecInvoiceBLL.GetmailInfo(PaymentID);

            var data = from curPrj in lstProjecInvoice
                       select new
                       {
                           curPrj.customerName,
                           curPrj.customerAddress,
                           curPrj.ProjectName,
                           curPrj.Description
                       };
            
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string str = jss.Serialize(data);
            return str;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string SendMail(string To, string Cc, string Bcc, string Subject, string MsgBody, int PaymentID)
    {        
        string sReturn="Success";
        string FromMailID = "accounts@intelgain.com";
        try
        {
            if (To != "" && Bcc != "" && MsgBody != "")
            {
                //sReturn = CSCode.Global.SendMail(MsgBody, Subject, "elumalai.n@intelgain.com", FromMailID, true, "elumalai.n@intelgain.com", "elumalai.n@intelgain.com");
                //sReturn = CSCode.Global.SendMail(MsgBody, "test", "elumalai.n@intelgain.com");

                sReturn = CSCode.Global.SendMail(MsgBody, Subject, To, FromMailID, true, Cc,Bcc);
                ProjecInvoiceBLL objBLL = new ProjecInvoiceBLL();
                objBLL.UpdateMailSentDate(PaymentID);
            }
        }
        catch(Exception ex)
        {

        }

        return sReturn;
    }

    //-------------------------Get
    #region GET
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static ProjecInvoiceBLL GetInvoiceForPayment(string prjID)
    {
        ProjecInvoiceBLL objInvoice = new ProjecInvoiceBLL();
        ProjectInvoiceDAL objPrjInvoice = new ProjectInvoiceDAL();
    DataSet ds = objPrjInvoice.GetInvoicePaymentDetails(prjID, "GETPAYMENT");

        objInvoice = objInvoice.BindPayment(ds, "GET");

        return objInvoice;

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static ProjecInvoiceBLL GetInvoiceForEditPayment(string paymentID)
    {
        ProjecInvoiceBLL objInvoice = new ProjecInvoiceBLL();
        ProjectInvoiceDAL objPrjInvoice = new ProjectInvoiceDAL();

        //DataSet ds = new InvoicePayment().GetInvoicePaymentDetails(prjID, "GETPAYMENT");
        DataSet ds = objPrjInvoice.GetInvoicePaymentDetails(paymentID, "GETEDITPAYMENT");

        objInvoice = objInvoice.BindPayment(ds, "EDIT");

        return objInvoice;

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<KeyValueModel> getCurrency()
    {
        return ProjecInvoiceBLL.GetCurrency();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<KeyValueModel> getProjects()
    {
        return mileStone.GetAllProjects();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<KeyValueModel> GetPaymentType()
    {
        return ProjecInvoiceBLL.GetPaymentType();
    }
    #endregion

    #region POST
    //--------------------------Post

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static int PostPaymentTypes(string JSONData)
    {
        ProjecInvoiceBLL header = new ProjecInvoiceBLL();
        header = JsonConvert.DeserializeObject<ProjecInvoiceBLL>(JSONData);
        int result1 = new ProjecInvoiceBLL().SavePaymentType(header.PTypes);
        return result1;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void PostInvoicePayments(string JSONData)
    {
        ProjecInvoiceBLL header = new ProjecInvoiceBLL();
        header = JsonConvert.DeserializeObject<ProjecInvoiceBLL>(JSONData);

        header.PostInvoicePayments(header);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void DeletePayment(int PaymentID)
    {
        ProjecInvoiceBLL obj = new ProjecInvoiceBLL();

        obj.DeletePayment(PaymentID);
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(true, false, true);
        hdninsertedby.Value = UM.EmployeeID.ToString();
        hdnProjID.Value = Session["ProjectID"].ToString();

        if (Convert.ToInt32(HttpContext.Current.Session["ProjectID"].ToString()) == 0)
            btnAddpayments.Visible = false;
        else
            btnAddpayments.Visible = true;
    }
}