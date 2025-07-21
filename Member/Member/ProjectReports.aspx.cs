using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Member_ProjectReports : Authentication
{
    public static int projectid = 0;
    static UserMaster UM;
    [System.Web.Services.WebMethod]
    public static String BindReports(string jsFromDate, string jsToDate)
    {
        UM = UserMaster.UserMasterInfo();
        Boolean IsAdmin = false;
        try
        {

            if (UM.IsAdmin)
            {
                IsAdmin = true;
            }

            CultureInfo provider = CultureInfo.InvariantCulture;
            string format = "dd/MM/yyyy";

            string TodateString = Convert.ToString(jsToDate);
            DateTime ToDate = DateTime.ParseExact(TodateString, format, provider);

            string FromdateString = Convert.ToString(jsFromDate);
            DateTime FromDate = DateTime.ParseExact(FromdateString, format, provider);

            List<projectReports> lstGetReports = new List<projectReports>();

            int empId = UM.EmployeeID; 

            if (IsAdmin == true)
            {
                lstGetReports = projectReports.getProjectReports(1, empId, FromDate, ToDate);
            }
            else if (IsAdmin == false)
            {
                lstGetReports = projectReports.getProjectReports(2, empId, FromDate, ToDate);
            }
          
            var data = from pr in lstGetReports
                       orderby pr.reportDate descending
                       select new
                       {
                           pr.projID,
                           pr.projectName,
                           pr.reportDate,
                           pr.reportTitle,
                           pr.lastModified,
                           pr.ReportEmpID,
                           pr.reportId,
                           pr.Description,
                           pr.ReportedBy
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string UpdateReports(string mode, int projId, string ReportTitle, string Description, DateTime ReportDate, int ReportEmpId, string lastmodified, int reportId)
    {

        string output = "Update Failed";
        try
        {
            bool isupdated = projectReports.updateReports("Update", projId, ReportTitle, Description, ReportDate, ReportEmpId, lastmodified, reportId);
            if (isupdated == true)
            {
                output = "Report updated successfully";

            }
            else if (isupdated == false)
            {
                output = "Update Failed";

            }
        }
        catch (Exception ex)
        {

        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        hdnProjID.Value = Convert.ToString(Request.QueryString["p"]);

        if (hdnProjID.Value == "")
        {
            projectid = 0;
        }
        else
        {
            projectid = Convert.ToInt32(hdnProjID.Value);
        }

        if (!IsPostBack)
        {
            bindDropdown();
            selectFiles();
        }
    }

    public bool selectFiles()
    {
        bool success = false;
        //save attached files
        if (Request.Files.Count > 0)
        {
            string fileName = string.Empty;
            Storage ObjStorge = new Storage();
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    string nm = Request.Files[i].FileName;
                    ObjStorge.FileOperation(Storage.ModuleType.Temp, Storage.OpType.Add, nm, Request.Files[i].InputStream);
                    fileName = fileName + "," + nm;
                }
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            Session["ReportFileNames"] = Session["ReportFileNames"] + fileName;
        }
        return success;
    }

    //----------fetch data of attached files start----------//
    [System.Web.Services.WebMethod]
    public static string GetReportAttachments(string reportId)
    {
        try
        {
            List<projectReports> lstGetReportAttachments = projectReports.GetReportAttachments(Convert.ToInt32(reportId));

            var data = (from getAttachment in lstGetReportAttachments
                        select new
                        {
                            getAttachment.attachmentFile,
                            getAttachment.attachmentId
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception)
        {
            return null;
        }
    }


    [System.Web.Services.WebMethod]
    public static String ClearTempFilesandSession()
    {
        try
        {
            string[] filenames = HttpContext.Current.Session["ReportFileNames"].ToString().TrimStart(',').Split(',');
            HttpContext.Current.Session["ReportFileNames"] = null;
            Storage ObjStorge = new Storage();

            for (int i = 0; i < filenames.Length; i++)
            {
                ObjStorge.FileOperation(Storage.ModuleType.Temp, Storage.OpType.Delete, filenames[i]);
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize("sucesses");

        }
        catch (Exception ex)
        {
            return null;
        }
    }


    public static string InsertAttachments(int reportId)
    {
        string Attachments = "";
        try
        {
            if (HttpContext.Current.Session["ReportFileNames"] != null)
            {
                string[] filenames = HttpContext.Current.Session["ReportFileNames"].ToString().TrimStart(',').Split(',');
                Storage ObjStorge = new Storage();

                for (int i = 0; i < filenames.Length; i++)
                {
                    string OriginalFileName = filenames[i];
                    Stream s = ObjStorge.FileRead(Storage.ModuleType.Temp, OriginalFileName);

                    string filename = System.IO.Path.GetFileNameWithoutExtension(filenames[i]).ToLower().Replace(" ", "");
                    string fileextension = System.IO.Path.GetExtension(filenames[i]).ToLower();
                    string DesFileName = HttpContext.Current.Server.UrlEncode(filename + "-" + reportId + fileextension);
                    HttpContext.Current.Response.Write(ObjStorge.FileOperation(Storage.ModuleType.ProjectReport, Storage.OpType.Add, DesFileName, s));
                    projectReports.InsertAttachments(reportId, DesFileName);
                    Attachments = DesFileName + ";" + Attachments;
                }

                ClearTempFilesandSession();
            }
            HttpContext.Current.Session["ReportFileNames"] = null;
        }
        catch (Exception ex)
        {

        }
        return Attachments;
    }

    //----------file upload methods end----------//


    public void bindDropdown()
    {
        UM = UserMaster.UserMasterInfo();
        int employeeId = UM.EmployeeID;

        DataTable dt = new DataTable();
        dt = projectReports.bindProjectDropdown(employeeId);
        ddlProj.DataTextField = dt.Columns["projName"].ToString();
        ddlProj.DataValueField = dt.Columns["projid"].ToString();
        ddlProj.DataSource = dt;
        ddlProj.DataBind();
    }

    protected void ddlProj_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProj.SelectedValue != "0")
        {
            string projectId = ddlProj.SelectedValue != "" ? ddlProj.SelectedValue : "0";
            List<projectReports> reports = projectReports.getReportsData(Convert.ToInt32(projectId));
            lblCustomerName.Text = reports[0].custName;
            lblEmailTo.Text = reports[0].custEmail;
        }
        else
        {
            lblCustomerName.Text = string.Empty;
            lblEmailTo.Text = string.Empty;
        }
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myFunction", "attachDatepicker();fileUpload();attachHtmlControl();", true);
    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        int projectid, empid = 0;
        string CustomerName = string.Empty;
        string ReportTitle = string.Empty;
        string EmailTo = string.Empty;
        string Cc = string.Empty;
        string ReportDescription = string.Empty;
        string mode = "Insert";
        string ProjectName = string.Empty;
        string ReportedBy = string.Empty;

        projectid = Convert.ToInt32(ddlProj.SelectedValue);
        CustomerName = lblCustomerName.Text;
        ReportTitle = txtReportTitle.Value;
        EmailTo = lblEmailTo.Text;
        Cc = txtCc.Value + ',' + UM.EmailID;

        if (string.IsNullOrEmpty(ReportTitle))
        {
            lblmsgTitle.InnerHtml = "Please enter report title.";
            return;
        }
        StringWriter myWriter = new StringWriter();
        // Decode the encoded string.
        HttpUtility.HtmlDecode(txtDescription.Text, myWriter);

        StringBuilder sb = new StringBuilder(myWriter.ToString());

       

        sb.Replace("\n", "<br>");
        ReportDescription = sb.ToString();
        CultureInfo provider = CultureInfo.InvariantCulture;
        string dateString = Convert.ToString(hdnReportDate.Value);
        string format = "dd/MM/yyyy";
        DateTime ReportDate = DateTime.ParseExact(dateString, format, provider);
        empid = UM.EmployeeID;
        ProjectName = ddlProj.SelectedItem.Text;
        ReportedBy = UM.Name;


        projectReports prj = new projectReports();
        int reportId = prj.Insertdata(projectid, ReportTitle, ReportDescription, ReportDate, empid, mode);

        if (reportId > 0)
        {
           string attachments = InsertAttachments(reportId);
            List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 1);

            string EmailBody = lstConfig[0].value.ToString();
            EmailBody = EmailBody.Replace("{ProjectName}", ProjectName);
            EmailBody = EmailBody.Replace("{ReportDate}", ReportDate.ToShortDateString());
            EmailBody = EmailBody.Replace("{ReportTitle}", ReportTitle);
            EmailBody = EmailBody.Replace("{ReportDescription}", ReportDescription);
            EmailBody = EmailBody.Replace("{ReportedBy}", ReportedBy);

            string message = CSCode.Global.SendMail(EmailBody, ProjectName + "- Project Report", EmailTo, UM.EmailID, true, Cc, "", Storage.ModuleType.ProjectReport, attachments);
            if (message == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myFunction", "ClearTempFilesandSession();closeAddPopUP();GetReportDetails();alert('Record Inserted Successfully And Email Sent Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myFunction", "ClearTempFilesandSession();closeAddPopUP();GetReportDetails();alert('Record Inserted Successfully But Email Sending Failed')", true);
            }

            clearData();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record can not be inserted')", true);
        }

        Session["ReportFileNames"] = null;
    }

    public void clearData()
    {
        ddlProj.ClearSelection();
        txtCc.Value = string.Empty;
        lblCustomerName.Text = string.Empty;
        txtDescription.Text = string.Empty;
        lblEmailTo.Text = string.Empty;
        txtReportTitle.Value = string.Empty;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        clearData();
        Session["ReportFileNames"] = null;
    }

}