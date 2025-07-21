using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Configuration;
using System.IO;
using Customer.Model;
using System.Web.Services;

//using WebSupergoo.ABCpdf6;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using ICSharpCode.SharpZipLib;
using Newtonsoft.Json;

//using Pdfizer;

public partial class Member_Invoices : Authentication
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

            if (Convert.ToString(Request.QueryString["status"]) != null)
                hdnStatus.Value = Convert.ToString(Request.QueryString["status"]);

        }
        else
            hdnStatus.Value = "";

        hdnLocationId.Value = HttpContext.Current.Session["LocationID"].ToString();
        hdnProjectName.Value = HttpContext.Current.Session["ProjectName"].ToString();
        hdnProjID.Value = HttpContext.Current.Session["ProjectID"].ToString();

        List<ProformaInvoiceBLL> lstCode = ProformaInvoiceBLL.BindCode("GetCode");
        ProjectSacHsnCode.DataSource = lstCode;
        ProjectSacHsnCode.DataValueField = "CodeId";
        ProjectSacHsnCode.DataTextField = "Code";
        ProjectSacHsnCode.DataBind();
        ProjectSacHsnCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Code--", "0"));

    }



    [System.Web.Services.WebMethod]
    public static String GetProjectInvoices(string status)
    {
        try
        {
            NewInvoice objProjectInvoice = new NewInvoice();
            List<NewInvoice> InvoiceList = objProjectInvoice.GetProjectInvoices(Convert.ToInt32(HttpContext.Current.Session["ProjectID"]),
                                                                                Convert.ToInt32(HttpContext.Current.Session["LocationID"]),
                                                                                status);
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            // string str = jss.Serialize(InvoiceList);
            string str=JsonConvert.SerializeObject(InvoiceList);

            return str;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String GetInvoiceStatus(string pInvId)
    {
        try
        {
            NewInvoice objProjectInvoice = new NewInvoice();
            List<NewInvoice> InvoiceList = objProjectInvoice.GetInvoicesStatus(Convert.ToInt32(pInvId));
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string str = jss.Serialize(InvoiceList);

            return str;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    protected void ddlProj_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ddlProj_SelectedIndexChanged", "BindAllDateCalender();", true);
    }
    public List<KeyValueModel> getProjects()
    {
        return mileStone.GetAllProjects();
    }

    protected void grdMileStone_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        // grdMileStone.EditIndex = -1;
    }
    protected void grdMileStone_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void grdMileStone_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }
    protected void grdMileStone_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    protected void grdMileStone_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddNew"))
        {

            mileStone objProjMileStone = new mileStone();
            objProjMileStone.mode = "InsertProjectMileStone";
        }
    }
    protected void grdMileStone_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtrate = (TextBox)e.Row.FindControl("txtRate");
            TextBox txtqut = (TextBox)e.Row.FindControl("txtQuantity");
            Label lblAmount = (Label)e.Row.FindControl("lblAmount");
            txtqut.Attributes.Add("onkeyup", "CalcSellPrice2('" + txtrate.ClientID + "', '" + txtqut.ClientID + "','" + lblAmount.ClientID + "')");
        }
    }

    protected void EmptyGridview(GridView gv, DataSet ds)
    {
        ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
        gv.DataSource = ds;
        gv.DataBind();
        int totalcolums = gv.Rows[0].Cells.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = totalcolums;
        gv.Rows[0].Cells[0].Text = "No Data Found";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "closeAddPopUP();", true);

    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetMailInfo(string pinvoiceid, string Inv_Delay)
    {

        try
        {
            List<NewInvoice> lstProjecInvoice = NewInvoice.GetmailInfo(Convert.ToInt32(pinvoiceid), Inv_Delay);

            var data = from curPrj in lstProjecInvoice
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

    [System.Web.Services.WebMethod]
    public static string SetProjIdForMilestone(string projid, string currExRate)
    {
        Page objp = new Page();
        objp.Session["ProjectID"] = projid;
        objp.Session["CurrExRate"] = currExRate;
        return projid;
    }

    [System.Web.Services.WebMethod]
    public static string VoidInvoice(string pInvId)
    {
        int sStatus = NewInvoice.VoidInvoice(pInvId);

        return sStatus.ToString();
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
                string strInvoicePath = HttpContext.Current.Server.MapPath(@"\Admin\Invoice\");
                string strFileName = TargetDir + InvoiceNo.Replace('/', '-') + ".pdf";
                FileInfo objInfo = new FileInfo(strFileName);
                if (objInfo.Exists)
                {
                    // dvAttachnemt.InnerHtml = "<a href='#' onclick='javascript:window.open(\"http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/admin/Invoice/" + strFileName + "\")'>" + strFileName + "</a>";
                }

                sReturn = CSCode.Global.SendMailWithAttachments(MsgBody, Subject, To, FromMailID, true, Cc, Bcc, strFileName);
                //sReturn = CSCode.Global.SendMail(MsgBody, Subject, To, FromMailID, true, Cc, Bcc);
                NewInvoice objBLL = new NewInvoice();
                objBLL.UpdateMailSentDate(pInvId);

            }
        }
        catch (Exception ex)
        {
            sReturn = "Failed";
        }

        return sReturn;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GeneratePDF(string pinvoiceid, string InvoiceNo, string InvoiceProjectName)
    {
        string strPDF = "";

        Page objp = new Page();
        objp.Session["ProjectInvoiceID"] = pinvoiceid;
        objp.Session["InvoiceNo"] = InvoiceNo;
        objp.Session["InvoiceProjectName"] = InvoiceProjectName;


        return strPDF;


    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static NewInvoice GetInvoiceForEdit(string pInvId)
    {
        NewInvoice objInvoice = new NewInvoice();
        objInvoice = objInvoice.GetInvoiceForEdit(pInvId);

        return objInvoice;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<mileStoneModel> GetInvoiceMile(string projid)
    {
        NewInvoice objInvoice = new NewInvoice();
        List<mileStoneModel> lstgetInvoiceMile = new List<mileStoneModel>();
        lstgetInvoiceMile = objInvoice.GetInvoiceMile(projid);

        return lstgetInvoiceMile;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<KeyValueModel> GetProjects()
    {
        NewInvoice objInvoice = new NewInvoice();

        return objInvoice.GetProjects();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<mileStoneModel> GetMileStoneDetails(string JSONData)
    {
        NewInvoice objInvoice = new NewInvoice();
        mileStoneModel msmodel = new mileStoneModel();
        List<mileStoneModel> lstmodel = new List<mileStoneModel>();
        msmodel = JsonConvert.DeserializeObject<mileStoneModel>(JSONData);
        lstmodel = objInvoice.GetMileStoneDetails(msmodel);

        return lstmodel;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static bool IfExistsInvoiceNo(string InvoiceNo, int ProjectInvoiceID, int ProjID, int LocationID)
    {
        NewInvoiceModel invModel = new NewInvoiceModel();
        return invModel.IfExistsInvoiceNo(InvoiceNo, ProjectInvoiceID, ProjID, LocationID);
    }

    ///////////////////post

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void updateinvoice(string JSONData)
    {
        NewInvoice header = new NewInvoice();
        header = JsonConvert.DeserializeObject<NewInvoice>(JSONData);

        header.updateinvoice(header);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void PostInvoice(string JSONData)
    {
        NewInvoice header = new NewInvoice();
        header = JsonConvert.DeserializeObject<NewInvoice>(JSONData);

        header.PostInvoice(header);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void DeleteInvoiceMile(string JSONData)
    {
        int ProjectInvoiceDetailID = 0;
        if (JSONData == "undefined")
            ProjectInvoiceDetailID = 0;
        else
            ProjectInvoiceDetailID = Convert.ToInt32(JSONData);
        NewInvoiceModel.DeleteInvoiceMilestone(ProjectInvoiceDetailID);
    }

    [System.Web.Services.WebMethod]
    public static String Get_InvoiceReminderDetails(string ProjectInvoiceID)
    {
        try
        {
            PrjInvcReminderDtls objProjectInvoice = new PrjInvcReminderDtls();
            string str = objProjectInvoice.Get_InvoiceReminderDetails(Convert.ToInt32(ProjectInvoiceID));
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //string str = jss.Serialize(InvoiceList);
            return str;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static void Upload(HttpPostedFileBase[] attachment, string formData)
    {
        //for (int i = 0; i < Request.Files.Count; i++)
        //{
        //    var file = Request.Files[i];
        //    var fileName = Path.GetFileName(file.FileName);
        //    var path = Path.Combine(Server.MapPath("~/[Your_Folder_Name]/"), fileName);

        //    file.SaveAs(path);
        //}
    }
    [System.Web.Services.WebMethod]
    public static string submitApplication(HttpPostedFileBase[] attachment)
    {
        for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
        {
            var file = HttpContext.Current.Request.Files[i];
            var fileName = Path.GetFileName(file.FileName);
            //var path = Path.Combine(Server.MapPath("~/[Your_Folder_Name]/"), fileName);
            //file.SaveAs(path);
        }


        string fil = "";
        foreach (HttpPostedFileBase file in attachment)
        {
            /*Geting the file name*/
            string filename = System.IO.Path.GetFileName(file.FileName);
            fil += filename;
            /*Saving the file in server folder*/
            //file.SaveAs(Server.MapPath("~/Images/" + filename));
            string filepathtosave = "Images/" + filename;
            /*HERE WILL BE YOUR CODE TO SAVE THE FILE DETAIL IN DATA BASE*/
        }
        return "";
        //return this.Json(fil, JsonRequestBehavior.AllowGet);
    }
    public class p_sendmail
    {
        public string To { set; get; }
        public string Cc { set; get; }
        public string Bcc { set; get; }
        public string Subject { set; get; }
        public string editor { set; get; }
        public string MsgBody { set; get; }
        public int pInvId { set; get; }
        public string InvoiceNo { set; get; }
        public HttpPostedFileBase File { set; get; }
    }
}

