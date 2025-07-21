using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WebSupergoo.ABCpdf6;

public partial class Admin_pdfConverter : Authentication
{
    string _strRegID = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DynoEmpSession"] != null)
        {
        }
        else
        {
            Response.Redirect("/emp/empLogin.aspx");
        }


        if (Request.QueryString["RegID"] != null)
        {
            _strRegID = Request.QueryString["RegID"].ToString();
            CreatePDFConverter(_strRegID);
        }
        else
        {
            Response.Redirect("traineeApplication.aspx");
        }
    }

    #region To Create PDF :
    public void CreatePDFConverter(string RegID)
    {
        string _strFooter = "<html><body><div><table width='100%'><tr><td align='center'>Dynamic Web Technologies Pvt. Ltd.</td></tr><tr><td align='center'>B-203, Sanpada Station Complex, Navi Mumbai-400705, India</td></tr><tr><td align='center'>Tel: +91(22) 41516100 </td></tr></table></div></body></html>";       
           
        try
        {
            Doc theDoc = new Doc();
            theDoc.HtmlOptions.AddMovies = true;
            string theURL = string.Empty;
            try
            {
                if ((Request.ServerVariables["SERVER_NAME"].Trim() == "dwt") || (Request.ServerVariables["SERVER_NAME"] == "38.99.180.111"))
                {
                    theURL = "http://" + Request.ServerVariables["SERVER_NAME"].ToString() + ":" + Request.ServerVariables["SERVER_PORT"].ToString() + "/Admin/adminPdfCreatorNew.aspx?no=" + DateTime.Now.Ticks.ToString() + "&RegID=" + RegID;
                }
                else
                {
                    theURL = "http://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/Admin/adminPdfCreatorNew.aspx?no=" + DateTime.Now.Ticks.ToString() + "&RegID=" + RegID;
                }
            }
            catch(Exception ex)
            {
            }

           
            theDoc.Rect.Bottom = 40;
            theDoc.HtmlOptions.Timeout = 10000;
            int theID = theDoc.AddImageUrl(theURL);
            try
            {

                while (true)
                {
                    if (theDoc.PageCount > 0)
                    {
                        theDoc.Rect.Top = 750;
                    }

                    if (!theDoc.Chainable(theID))
                        break;
                    theDoc.MediaBox.String = "A4";
                    theDoc.Page = theDoc.AddPage();
                    theID = theDoc.AddImageToChain(theID);                  

                }
            }
            catch (Exception Ex)
            {              
            }                        
            try
            {

                for (int i = 1; i <= theDoc.PageCount; i++)
                {
                    theDoc.Color.String = "255 255 255 ";
                    theDoc.Rect.Top = 45;
                    theDoc.Rect.Bottom = 32.8;
                    theDoc.Rect.Left = 0;
                    theDoc.Rect.Right = 174.2;
                    theDoc.FillRect();

                    theDoc.Color.String = "255 255 255 ";
                    theDoc.Rect.Top = 40;
                    theDoc.Rect.Bottom = 32.8;
                    theDoc.Rect.Left = 176;
                    theDoc.Rect.Right = 650;
                    theDoc.FillRect();



                    theDoc.Rect.Top = 32; ;
                    theDoc.Rect.Bottom = 0;
                    theDoc.Rect.Left = 0;
                    theDoc.Rect.Right = 650;
                    theDoc.HPos = 0.1;
                    theDoc.VPos = 0.5;

                    theDoc.FontSize = 11;


                    theDoc.PageNumber = i;
                    theDoc.TextStyle.Bold = true;
                    theDoc.Color.String = "4 91 170";                   
                    theDoc.FillRect();
                    theDoc.Color.String = "255 255 255 ";
                    string theFont = "Arial";
                    theDoc.Font = theDoc.AddFont(theFont);
                                 
                  
                    theDoc.AddHtml(_strFooter);
                    theDoc.FrameRect();

                    byte[] theData = theDoc.GetData();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", theData.Length.ToString());
                    Response.AddHeader("content-disposition", "attachment; filename=Confirmation.pdf");
                    Response.BinaryWrite(theData);
                }
            }
            catch (Exception Ex)
            {
            }
           theDoc.Clear();
        }
        catch (Exception Ex)
        {
           
        }
    }
    #endregion
}
