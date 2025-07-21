using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Configuration;
//using Intelgain.RTO.Utils;

public partial class Common_DownloadResumes : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string uploadPath = ConfigurationManager.AppSettings["uploadPath"].ToString();

        Response.Clear();
        Response.Buffer = false;
        if (Request.QueryString["DocName"] != null)
        {
            string DocName = Request.QueryString["DocName"];
            string Extn = Path.GetExtension(DocName);
            switch (Extn.ToLower())
            {
                case (".txt"):
                    Response.ContentType = "text/plain";
                    break;
                case (".bmp"):
                    Response.ContentType = "image/x-ms-bmp";
                    break;
                case (".doc"):
                    Response.ContentType = "application/msword";
                    break;
                case (".html"):
                case (".htm"):
                    Response.ContentType = "text/html";
                    break;
                case (".js"):
                    Response.ContentType = "application/x-javascript";
                    break;
                case (".xls"):
                case (".xlsx"):
                    Response.ContentType = "application/x-msexcel";//"application/excel";
                    break;
                case (".zip"):
                    Response.ContentType = "application/x-zip-compressed";
                    break;
                case (".pdf"):
                    Response.ContentType = "application/pdf";
                    break;
                default:
                    Response.ContentType = "application/octet-stream";
                    break;

            }
            try
            {
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + DocName);
                Response.WriteFile(uploadPath + DocName);
                //Response.Redirect(CustAttachmentPath + DocName);
                Response.End();
            }
            catch (Exception ex) { }
        }
    }
}