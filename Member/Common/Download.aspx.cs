using System;
using System.IO;
using System.Configuration;
using System.Web.UI;

public partial class Common_Download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string TargetDir = "";
        string filepath = string.Empty;
        Storage ObjStorage = new Storage();

        if (Request.QueryString["m"] != null)
        {
            string Mode = Request.QueryString["m"].ToString();
            string FileName = Request.QueryString["f"].ToString();

            if (Mode == "TM")
            {
                Download(FileName, ObjStorage.FileRead(Storage.ModuleType.TaskManager, FileName));
            }
            else if (Mode == "PR")
            {
                Download(FileName, ObjStorage.FileRead(Storage.ModuleType.ProjectReport, FileName));
            }
            else if (Mode == "CV")
            {
                TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\CV\";
            }
            else if (Mode == "AL")
            {
                TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\Employee_Appointment\";
            }
            else if (Mode == "KB")
            {
                TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\KB\";
            }
            else if (Mode == "KDB")
            {
                TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\KnowledgeBaseDocument\";
            }
            else if (Mode == "INV")
            {
                TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\INVOICE\";
            }
            else if (Mode == "Pay")
            {
                TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\BankStatement\";
            }
            else if (Mode == "CVCollection")
            {
                TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\CVCollection\";
            }

            if (File.Exists(TargetDir + FileName))
            {
                MemoryStream MemStr = new MemoryStream();
                string FileStr = TargetDir + FileName;
                using (FileStream fs = File.OpenRead(FileStr))
                {
                    fs.CopyTo(MemStr);
                }
                Download(FileName, MemStr);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('File not found')", true);
            }
        }
    }

    void Download(string FileName, MemoryStream Mem)
    {
        if (FileName != null || Mem != null)
        {
            Response.Clear();
            Response.Buffer = false;

            string Extn = Path.GetExtension(FileName);

            switch (Extn.ToLower())
            {
                case (".txt"):
                    Response.ContentType = "text/plain";
                    break;
                case (".bmp"):
                    Response.ContentType = "image/x-ms-bmp";
                    break;
                case (".jpg"):
                    Response.ContentType = "image/jpg";
                    break;
                case (".png"):
                    Response.ContentType = "image/png";
                    break;
                case (".doc"):
                    Response.ContentType = "application/msword";
                    break;
                case (".docx"):
                    Response.ContentType = "application/msword";
                    break;
                case (".html"):
                    Response.ContentType = "text/html";
                    break;
                case (".htm"):
                    Response.ContentType = "text/html";
                    break;
                case (".js"):
                    Response.ContentType = "application/x-javascript";
                    break;
                case (".xls"):
                    Response.ContentType = "application/x-msexcel";
                    break;
                case (".xlsx"):
                    Response.ContentType = "application/x-msexcel";
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
                Response.AddHeader("Content-Disposition", "Attachment; filename=" + Server.UrlEncode(FileName.ToString()));
                Response.BinaryWrite(Mem.ToArray());
            }
            catch (Exception ex)
            {
                Response.Write("There is an issue in downloading file. Please contact administrator. " + ex.Message);
            }
        }
        Response.End();
    }
}