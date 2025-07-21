using System;
using System.Web;
using System.Web.UI;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Data;
using Customer.BLL;
using System.Configuration;
public partial class Member_GeneratePDF : Authentication
{
     string TargetDir;
    protected void Page_Load(object sender, EventArgs e)
    {
       TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\INVOICE\";
        if (!String.IsNullOrEmpty(Convert.ToString(Session["ProjectInvoiceID"])))
        {
            if (!Directory.Exists(TargetDir))
            {
                Directory.CreateDirectory(TargetDir);
            }

            BindPDFData();

        }
        else if (!String.IsNullOrEmpty(Convert.ToString(Session["ProformaInvoiceID"])))
        {
            if (!Directory.Exists(TargetDir))
            {
                Directory.CreateDirectory(TargetDir);
            }

            BindProInvData();

        }
    }
    
    private void BindPDFData()
    {
        DataSet ds = new DataSet();
        NewInvoice objInvoice = new NewInvoice();
        int LocationID = Convert.ToInt32(Request.QueryString["LocationID"].ToString());
        ds = objInvoice.GeneratePDF(Convert.ToInt32(HttpContext.Current.Session["ProjectInvoiceID"].ToString()), LocationID);

        if (ds.Tables[1].Rows.Count > 0)
        {
            string strHtml = objInvoice.BindDataForPdf(ds.Tables[0], ds.Tables[1].Rows[0]["Value"].ToString(), Request.QueryString["Logo"].ToString());

            GeneratePDF(strHtml);
        }
    }
    
    private void GeneratePDF(string strHtml)
    {
        string strInvoicePath = TargetDir;
        string LocID = HttpContext.Current.Session["LocationID"].ToString();
        string InvID = HttpContext.Current.Session["ProjectInvoiceID"].ToString();
        string InvoiceNo = HttpContext.Current.Session["InvoiceNo"].ToString();
        InvoiceNo = InvoiceNo.Replace('/', '-');
        string sPathToWritePdfTo = strInvoicePath + InvoiceNo + ".pdf";


        if (File.Exists(strInvoicePath + InvoiceNo + "_" + LocID + ".pdf"))
        {
            try
            {
                //sPathToWritePdfTo = strInvoicePath + projID + "_" + InvID + ".pdf";
                File.Copy(strInvoicePath + InvoiceNo + ".pdf", strInvoicePath + InvoiceNo + "_A.pdf", true);
                File.Delete(strInvoicePath + InvoiceNo + ".pdf");
            }
            catch
            {
                Response.Write("Error in deleting PDF");
            }
        }
        //////////////// genrate pdf

        if (Request.QueryString["p"] == "1")
        {
            sPathToWritePdfTo = InvoiceNo + ".pdf";
            Response.AddHeader("content-disposition", "inline;filename=" + sPathToWritePdfTo);  // attachment --> download and inline --> open in same tab
        }
        else
            Response.AddHeader("content-disposition", "attachment;filename=" + sPathToWritePdfTo);  // attachment --> download and inline --> open in same tab

        Response.Clear();
        Response.ContentType = "application/pdf";

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        this.Page.RenderControl(hw);
    
        StringReader sr = new StringReader(strHtml);//strHtml
        Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 0f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

        //MemoryStream memoryStream = new MemoryStream();
        if (Request.QueryString["p"] == "1")
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream); // to download at C: derive on download click
        else
        {
          
            PdfWriter.GetInstance(pdfDoc, new FileStream(sPathToWritePdfTo, FileMode.Create));  // save at specified loaction for email attachment

        }
        HttpContext.Current.Session["ProjectInvoiceID"] = null;
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();


        Response.Write(pdfDoc);
        Response.Flush();
        Response.End();

    }


    //For Proforma Invoice

    private void BindProInvData()
    {
        DataSet ds = new DataSet();
        ProformaInvoiceBLL objProInvoice = new ProformaInvoiceBLL();
        int LocationID = Convert.ToInt32(Request.QueryString["LocationID"].ToString());
        ds = objProInvoice.GeneratePDF(Convert.ToInt32(HttpContext.Current.Session["ProformaInvoiceID"].ToString()), LocationID);
       
        if (ds.Tables[1].Rows.Count > 0)
        {
            string strHtml = objProInvoice.BindDataForPdf(ds.Tables[0], ds.Tables[1].Rows[0]["Value"].ToString(), Request.QueryString["Logo"].ToString());//, "PROFORMA INVOICE");

            GenerateProformaPDF(strHtml);
        }
    }

    private void GenerateProformaPDF(string strHtml)
    {
        string strInvoicePath = TargetDir;
        string LocID = HttpContext.Current.Session["LocationID"].ToString();
        string InvID = HttpContext.Current.Session["ProformaInvoiceID"].ToString();
        string InvoiceNo = HttpContext.Current.Session["ProformaInvoiceID"].ToString();
        InvoiceNo = InvoiceNo.Replace('/', '-');
        string sPathToWritePdfTo = strInvoicePath + InvoiceNo + ".pdf";


        if (File.Exists(strInvoicePath + InvoiceNo + "_" + LocID + ".pdf"))
        {
            try
            {
                File.Copy(strInvoicePath + InvoiceNo + ".pdf", strInvoicePath + InvoiceNo + "_A.pdf", true);
                File.Delete(strInvoicePath + InvoiceNo + ".pdf");
            }
            catch
            {
                Response.Write("Error in deleting PDF");
            }
        }
        //////////////// genrate pdf

        if (Request.QueryString["p"] == "1")
        {
            sPathToWritePdfTo = InvoiceNo + ".pdf";
            Response.AddHeader("content-disposition", "inline;filename=" + sPathToWritePdfTo);  // attachment --> download and inline --> open in same tab
        }
        else
            Response.AddHeader("content-disposition", "attachment;filename=" + sPathToWritePdfTo);  // attachment --> download and inline --> open in same tab

        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        this.Page.RenderControl(hw);
        StringReader sr = new StringReader(strHtml);//strHtml
        Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 0f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        if (Request.QueryString["p"] == "1")
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream); // to download at C: derive on download click
        else
        {
            PdfWriter.GetInstance(pdfDoc, new FileStream(sPathToWritePdfTo, FileMode.Create));  // save at specified loaction for email attachment

        }
        HttpContext.Current.Session["ProformaInvoiceID"] = null;
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();


        Response.Write(pdfDoc);
        Response.Flush();
        Response.End();
    }
}