using Customer.BLL;
using Customer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_ProformaInvoices : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        hdninsertedby.Value = UM.EmployeeID.ToString();

        Admin MasterPage = (Admin)Page.Master;

        MasterPage.MasterInit(true, true, true, true);
        if (!this.IsPostBack)
        {
            if (Request.QueryString["p"] != null && Convert.ToString(Request.QueryString["p"]) != String.Empty)
            {
                hdnProjID.Value = Convert.ToString(Request.QueryString["p"]);
            }
            else
            {
                hdnProjID.Value = HttpContext.Current.Session["ProjectID"].ToString();
                hdnRequest.Value = "1";
            }
        }

        hdnLocationId.Value = HttpContext.Current.Session["LocationID"].ToString();
        hdnProjectName.Value = HttpContext.Current.Session["ProjectName"].ToString();
        hdnProjID.Value = HttpContext.Current.Session["ProjectID"].ToString();

        List<ProformaInvoiceBLL> lstCode = ProformaInvoiceBLL.BindCode("GetCode");
        SacHsnCode.DataSource = lstCode;
        SacHsnCode.DataValueField = "CodeId";
        SacHsnCode.DataTextField = "Code";
        SacHsnCode.DataBind();
        SacHsnCode.Items.Insert(0, new ListItem("--Select Code--", "0"));

        List<ProformaInvoiceBLL> lstCode1 = ProformaInvoiceBLL.BindCode("GetCode");
        ProjectSacHsnCode.DataSource = lstCode1;
        ProjectSacHsnCode.DataValueField = "CodeId";
        ProjectSacHsnCode.DataTextField = "Code";
        ProjectSacHsnCode.DataBind();
        ProjectSacHsnCode.Items.Insert(0, new ListItem("--Select Code--", "0"));

    }

    [System.Web.Services.WebMethod]
    public static String GetProformaInvoices(string status)
    {
        try
        {
            ProformaInvoiceBLL objProjectInvoice = new ProformaInvoiceBLL();
            List<ProformaInvoiceBLL> InvoiceList = objProjectInvoice.GetProformaInvoices(Convert.ToInt32(HttpContext.Current.Session["ProjectID"]), Convert.ToInt32(HttpContext.Current.Session["LocationID"]), status);
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
    public static void InsertProInvoice(string JSONData)
    {
        ProformaInvoiceBLL header = new ProformaInvoiceBLL();
        header = JsonConvert.DeserializeObject<ProformaInvoiceBLL>(JSONData);

        header.InsertProInvoice(header);

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<mileStoneModel> GetProInvoiceMile(string projid)
    {
        ProformaInvoiceBLL objInvoice = new ProformaInvoiceBLL();
        List<mileStoneModel> lstgetInvoiceMile = new List<mileStoneModel>();
        lstgetInvoiceMile = objInvoice.GetProformaInvoiceMile(projid);


        return lstgetInvoiceMile;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static bool IfExistsInvoiceNo(string InvoiceNo, int ProformaInvoiceID, int ProjID, int LocationID)
    {
        ProInvoiceModel invModel = new ProInvoiceModel();
        return invModel.IfExistsInvoiceNo(InvoiceNo, ProformaInvoiceID, ProjID, LocationID);

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void DeleteInvoiceMile(string JSONData)
    {
        int ProformaInvoiceDetailID = 0;
        if (JSONData == "undefined")
            ProformaInvoiceDetailID = 0;
        else
            ProformaInvoiceDetailID = Convert.ToInt32(JSONData);
        ProInvoiceModel.DeleteInvoiceMilestone(ProformaInvoiceDetailID);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static ProformaInvoiceBLL GetInvoiceForEdit(string pInvId, string projid)
    {
        ProformaInvoiceBLL objInvoice = new ProformaInvoiceBLL();
        objInvoice = objInvoice.GetInvoiceForEdit(pInvId, projid);

        return objInvoice;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static ProformaInvoiceBLL GetDetailsForTaxInvoice(string pInvId, string projid)
    {
        ProformaInvoiceBLL objTaxInvoice = new ProformaInvoiceBLL();
        objTaxInvoice = objTaxInvoice.GetDetailsForTaxInvoice(pInvId, projid);

        return objTaxInvoice;
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void PostTaxInvoice(string JSONData)
    {
        ProformaInvoiceBLL header = new ProformaInvoiceBLL();
        header = JsonConvert.DeserializeObject<ProformaInvoiceBLL>(JSONData);

        header.PostTaxInvoice(header);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<mileStoneModel> GetInvoiceMile(string projid)
    {
        ProformaInvoiceBLL objInvoice = new ProformaInvoiceBLL();
        List<mileStoneModel> lstgetInvoiceMile = new List<mileStoneModel>();
        lstgetInvoiceMile = objInvoice.GetProformaInvoiceMile(projid);

        return lstgetInvoiceMile;
    }

    [System.Web.Services.WebMethod]
    public static String GetInvoiceStatus(string pInvId)
    {
        try
        {
            ProformaInvoiceBLL objProformaInvoice = new ProformaInvoiceBLL();
            List<ProformaInvoiceBLL> InvoiceList = objProformaInvoice.GetInvoicesStatus(Convert.ToInt32(pInvId));
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string str = jss.Serialize(InvoiceList);

            return str;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public List<KeyValueModel> getProjects()
    {
        return mileStone.GetProjects();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<mileStoneModel> GetMileStoneDetails(string JSONData)
    {
        ProformaInvoiceBLL objInvoice = new ProformaInvoiceBLL();
        mileStoneModel msmodel = new mileStoneModel();
        List<mileStoneModel> lstmodel = new List<mileStoneModel>();
        msmodel = JsonConvert.DeserializeObject<mileStoneModel>(JSONData);
        lstmodel = objInvoice.GetMileStoneDetails(msmodel);

        return lstmodel;
    }

    [System.Web.Services.WebMethod]
    public static string VoidProformaInvoice(string pInvId)
    {
        int sStatus = ProformaInvoiceBLL.VoidInvoice(pInvId);

        return sStatus.ToString();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void updateinvoice(string JSONData)
    {
        ProformaInvoiceBLL header = new ProformaInvoiceBLL();
        header = JsonConvert.DeserializeObject<ProformaInvoiceBLL>(JSONData);

        header.updateinvoice(header);
    }

    [System.Web.Services.WebMethod]
    public static string SetProjIdForMilestone(string projid, string currExRate)
    {
        Page objp = new Page();
        objp.Session["ProjectID"] = projid;
        objp.Session["CurrExRate"] = currExRate;
        return projid;
    }

    // PDF:  
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GeneratePDF(string pinvoiceid, string InvoiceNo, string InvoiceProjectName)
    {
        string strPDF = "";

        Page objp = new Page();
        objp.Session["ProformaInvoiceID"] = pinvoiceid;
        objp.Session["InvoiceNo"] = InvoiceNo;
        objp.Session["ProformaInvoiceProjectName"] = InvoiceProjectName;
        return strPDF;


    }

    // Mail:  
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetMailInfo(string pinvoiceid)
    {

        try
        {
            List<ProformaInvoiceBLL> lstProformaInvoice = ProformaInvoiceBLL.GetmailInfo(Convert.ToInt32(pinvoiceid));

            var data = from curPrj in lstProformaInvoice
                       select new
                       {
                           curPrj.custName,
                           curPrj.projName,
                           curPrj.custAddress, // <-- it has emailid
                           curPrj.InvoiceNo,
                           curPrj.Comment,
                           curPrj.Amount,  // <-- it has file attachment path.
                           curPrj.DueDate, // <-- it has filename to display.
                           curPrj.custCompany // <-- it has email cc 
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
    public static string SendMail(string To, string Cc, string Bcc, string Subject, string MsgBody, string InvoiceNo, int pInvId)
    {
        string sReturn = "Success";
        string FromMailID = "accounts@intelegain.com";
        try
        {
            if (To != "" && MsgBody != "")
            {
                string TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\INVOICE\";
                string strFileName = TargetDir + InvoiceNo.Replace('/', '-') + ".pdf";
                FileInfo objInfo = new FileInfo(strFileName);
                if (objInfo.Exists)
                {
                }

                sReturn = CSCode.Global.SendMailWithAttachments(MsgBody, Subject, To, FromMailID, true, Cc, Bcc, strFileName);

                ProformaInvoiceBLL objBLL = new ProformaInvoiceBLL();
                objBLL.UpdateMailSentDate(pInvId);
            }
        }
        catch (Exception ex)
        {

        }

        return sReturn;
    }

    //GST

    [System.Web.Services.WebMethod]
    public static string GetLocationDetails(int projID, int locationId)
    {

        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(ProformaInvoiceBLL.GetLocationDetails(projID, locationId).ToList());



    }

    [System.Web.Services.WebMethod]
    public static string GetGSTPercent(int CodeID)
    {

        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(ProformaInvoiceBLL.GetGSTPercent(CodeID).ToList());



    }

}