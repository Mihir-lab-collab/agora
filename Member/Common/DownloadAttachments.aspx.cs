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

public partial class Common_DownloadAttachments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string attachmentPath = HttpContext.Current.Server.MapPath("Attachment/TaskManager/").Replace("Common","Member");
        string EmpattachmentPath = ConfigurationManager.AppSettings["EmpFilePath"].ToString() + "TaskManager\\";
        string CustAttachmentPath = ConfigurationManager.AppSettings["CustFilePath"].ToString() + "TaskManager\\";
        //string CustAttachmentPath = HttpContext.Current.Server.MapPath("Attachment/TempFilesChangeRequest/").Replace("Common", "Member");
        Response.Clear();
        Response.Buffer = false;
        if (Request.QueryString["DocName"] != null)
        {
            string DocName =Request.QueryString["DocName"];
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
                //if (!string.IsNullOrEmpty(Request.QueryString["from"]) && Request.QueryString["from"].Equals("Certificate"))
                //{
                //    attachmentPath = System.Configuration.ConfigurationManager.AppSettings["CertificatePath"].ToString();
                //    Response.AppendHeader("Content-Disposition", "attachment; filename=Certificate");
                //}
                //else
                //{
                //    string[] name = DocName.Trim().Split('_');
                //   
                //}

                string destFileCust = System.IO.Path.Combine(CustAttachmentPath, DocName);
                string destFileEmp = System.IO.Path.Combine(EmpattachmentPath, DocName);
              
                if (!File.Exists(destFileCust))
                {
                    if(File.Exists(destFileEmp))
                    {
                        System.IO.File.Move(destFileEmp,destFileCust);
                    }

                }
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + DocName);
                Response.WriteFile(CustAttachmentPath + DocName);
                //Response.Redirect(CustAttachmentPath + DocName);
               Response.End();

            }
            catch (Exception ex) { }
        }
    }
}