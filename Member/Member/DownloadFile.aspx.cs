using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Member_DownloadFile : Authentication
{
    clsCommon objCommon = new clsCommon();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["Filename"] != null && Request.QueryString["Folder"] != null)
        {
            FileDownload(objCommon.GetDecriptFileName(Request.QueryString["Filename"]), objCommon.GetDecriptFileName(Request.QueryString["Folder"]));
            //  FileDownload("HRManual-2012.pdf", "~/ManualsDocument/");
        }
        else //Generate employeeDetails in excel format.
        {
            ExportToExcel();
        }
    }

    private void ExportToExcel()
    {
        StringWriter sw = Session["gridviewdata"] as StringWriter;
        if (sw != null)
        {
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            Response.AddHeader("content-disposition", "attachment;filename=EmployeeDetails.xls");
            Response.Output.Write(sw.ToString());
            Session.Remove("gridviewdata");
            Response.Flush();
            Response.End(); //VERY IMPORTANT           
        }
    }


    private void FileDownload(string FileName, string Folder)
    {
        Response.ContentType = "application/pdf";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.TransmitFile(Server.MapPath(Folder + FileName));
        Response.End();
    }
}