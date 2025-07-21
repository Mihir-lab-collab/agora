using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;
using System.Net.Mail;
using System.Text;

using System.Security.Cryptography;



public partial class Member_KnowledgeBase : Authentication
{
    public string CustAttachmentPath;
    static string TargetDir;
    static string TargetTempDir;
    static UserMaster UM;
    public UserDetails UD;
    PagedDataSource pgsource = new PagedDataSource();
    protected void Page_Load(object sender, EventArgs e)
    {
        TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\KB\";
        UM = UserMaster.UserMasterInfo();

        if (UM != null)
        {
            UD = HttpContext.Current.Session["MemberSession"] as UserDetails;
            txtkbempName.Text = " " + UD.Name;

        }
        Session["UserName"] = UM.Name.ToString();
        hdnempName.Value = HttpContext.Current.Session["UserName"].ToString();

        if (UM.IsAdmin)
        {
            hdn.Value = "true";
        }
        else
        {
            if (UM.IsModuleAdmin)
            {
                hdn.Value = "true";
            }
            else
            {
                hdn.Value = "false";
            }
        }

        if (!Directory.Exists(TargetDir))
        {
            Directory.CreateDirectory(TargetDir);
        }

        if (!IsPostBack)
        {
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
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    string nm = Request.Files[i].FileName;
                    Request.Files[i].SaveAs(TargetDir + nm);

                    fileName = nm + "," + fileName;
                }
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            Session["kbFile"] = Session["kbFile"] + fileName;
        }
        return success;
    }

    //public static string InsertAttachments(int reportId)
    //{
    //    string Attachments = "";
    //    try
    //    {
    //        if (HttpContext.Current.Session["FileNames"] != null)
    //        {
    //            string[] filenames = HttpContext.Current.Session["FileNames"].ToString().TrimStart(',').Split(',');

    //            for (int i = 0; i < filenames.Length; i++)
    //            {
    //                string OriginalFileName = filenames[i];
    //                string filename = System.IO.Path.GetFileNameWithoutExtension(filenames[i]).ToLower();
    //                string fileextension = System.IO.Path.GetExtension(filenames[i]).ToLower();
    //                string DesFileName = reportId + "-" + filename + fileextension;

    //                string sourceFile = System.IO.Path.Combine(TargetTempDir, filenames[i]);
    //                string destFile = System.IO.Path.Combine(TargetDir, DesFileName);
    //                if (File.Exists(sourceFile))
    //                {
    //                    System.IO.File.Copy(sourceFile, destFile, true);
    //                    if (File.Exists(sourceFile))
    //                        File.Delete(sourceFile);

    //                    projectReports.InsertAttachments(reportId, DesFileName);
    //                    Attachments = destFile + ";" + Attachments;
    //                }
    //            }
    //        }
    //        HttpContext.Current.Session["FileNames"] = null;
    //    }
    //    catch (Exception)
    //    {

    //    }
    //    return Attachments;
    //}

    [System.Web.Services.WebMethod]
    public static string GetReportAttachments(int kbId)
    {
        try
        {
            List<KnowledgeBaseBLL> lsComplaints = new List<KnowledgeBaseBLL>();
            lsComplaints = KnowledgeBaseBLL.getAttchment(kbId);
            lsComplaints = lsComplaints.Where(l => l.kbId == kbId).ToList();
            var data = (from getAttachment in lsComplaints
                        select new
                        {
                            getAttachment.kbId,
                            getAttachment.kbFile
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
    public static String GetProjectNameByProjId()
    {
        try
        {
            List<KnowledgeBaseBLL> lstGetEmpDetailsByProjId = new List<KnowledgeBaseBLL>();
            lstGetEmpDetailsByProjId = KnowledgeBaseBLL.getallProj("GetProj");

            var data = (from CurGetProjDetailsByProjId in lstGetEmpDetailsByProjId
                        select new
                        {
                            CurGetProjDetailsByProjId.projId,
                            CurGetProjDetailsByProjId.projName,
                            CurGetProjDetailsByProjId.custId,
                            CurGetProjDetailsByProjId.projDesc
                        }).ToList();
            ;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception)
        {

            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string BindKnowledgeBase1(int kbId)
    {
        try
        {
            List<KnowledgeBaseBLL> lsComplaints = new List<KnowledgeBaseBLL>();
            lsComplaints = KnowledgeBaseBLL.view("Get1", kbId);
            var data = from pr in lsComplaints
                       select new
                       {
                           pr.kbId,
                           pr.empId,
                           pr.empName,
                           pr.kbDate,
                           pr.kbDescrptn,
                           pr.kbComments,
                           pr.kbFile,
                           pr.kbTitle,
                           pr.techId,
                           pr.techName,
                           pr.subtechName,
                           pr.projId,
                           pr.projName,
                           pr.Url
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
    public static string BindKnowledgeBase(string techName)
    {
        try
        {
            List<KnowledgeBaseBLL> lsComplaints = new List<KnowledgeBaseBLL>();
            lsComplaints = KnowledgeBaseBLL.getall("Get");
            var data = from pr in lsComplaints
                       select new
                       {
                           pr.kbId,
                           pr.empId,
                           pr.empName,
                           pr.kbDate,
                           pr.kbDescrptn,
                           pr.kbComments,
                           pr.kbFile,
                           pr.kbTitle,
                           pr.techId,
                           pr.techName,
                           pr.subtechName,
                           pr.projId,
                           pr.projName,
                           pr.Url
                       };
            if (techName != "")
            {
                data = from pr in lsComplaints
                       where (pr.techName.ToLower().Contains(techName.ToLower()) || pr.subtechName.ToLower().Contains(techName.ToLower()) || pr.kbTitle.ToLower().Contains(techName.ToLower()))
                       select new
                       {
                           pr.kbId,
                           pr.empId,
                           pr.empName,
                           pr.kbDate,
                           pr.kbDescrptn,
                           pr.kbComments,
                           pr.kbFile,
                           pr.kbTitle,
                           pr.techId,
                           pr.techName,
                           pr.subtechName,
                           pr.projId,
                           pr.projName,
                           pr.Url
                       };
            }


            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {

            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static void DeleteKB(int kbId)
    {
        KnowledgeBaseBLL.Delete("Delete", kbId);
    }

    protected void lnkKnowledgen_Click(object sender, EventArgs e)
    {
        string output = "Insert Failed";

        string empName, kbTitle, kbFile, kbComments, kbDescrptn, kbDate, Url, subtechname;
        int projId = hdnprojId.Value != "" ? Convert.ToInt32(hdnprojId.Value) : 0;
        int empId = UD.EmployeeID;
        empName = UD.Name;
        kbTitle = hdnkbTitle.Value;
        kbComments = hdnkbComments.Value;
        kbDescrptn = txtkbDescrptn.InnerHtml;
        kbFile = hdnkbFile.Value;
        kbDate = hdnkbDate.Value;
        subtechname = hdnsubtechName.Value;
        int techId = Convert.ToInt32(hdntechId.Value);
        KnowledgeBaseBLL obj = new KnowledgeBaseBLL();
        if (HttpContext.Current.Session["kbFile"] != null)
        {
            obj.kbFile = HttpContext.Current.Session["kbFile"].ToString();
            HttpContext.Current.Session["kbFile"] = null;

        }
        Url = hdnUrl.Value;
        int id = KnowledgeBaseBLL.InsertKb("Insert", empId, kbDescrptn, kbComments, obj.kbFile, kbTitle, techId, projId, Url, subtechname);
        if (id != 0)
        {
            output = "Inserted successfully.";
        }
        txtkbDescrptn.InnerHtml = "";
        //Send KB mail to all employees.

        SendKnowledgeBaseMail("Insert", kbTitle, empName, kbDescrptn);

        Response.Redirect("/Member/KnowledgeBase.aspx");
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string UpdateComplaint(int kbId, string kbTitle, string kbComments, string kbDescrptn, int techId, string Url, string subtechName, string kbFile)
    {
        UserDetails UD1 = HttpContext.Current.Session["MemberSession"] as UserDetails;

        string empName = UD1.Name;
        string output = "Update Failed";

        try
        {
            KnowledgeBaseBLL obj = new KnowledgeBaseBLL();
            if (HttpContext.Current.Session["kbFile"] != null)
            {
                obj.kbFile = HttpContext.Current.Session["kbFile"].ToString();
                HttpContext.Current.Session["kbFile"] = null;
                kbFile = obj.kbFile;
            }
            else
            {
                obj.kbFile = kbFile;
            }


            KnowledgeBaseBLL.UpdateKB("Update", kbId, kbTitle, kbComments, kbDescrptn, techId, Url, subtechName, kbFile);
            SendKnowledgeBaseMail("Update", kbTitle, empName, kbDescrptn);
        }
        catch (Exception ex)
        {

        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }

    [System.Web.Services.WebMethod]
    public static String BindEmployee()
    {
        try
        {
            List<KnowledgeBaseBLL> lstQual = KnowledgeBaseBLL.getallEmployee("Emp");
            var data = from emp in lstQual
                       select new
                       {
                           emp.empId,
                           emp.empName
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {

            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String BindTech()
    {
        try
        {
            List<KnowledgeBaseBLL> lstQual = KnowledgeBaseBLL.getallTech("GetTech");
            var data = from tech in lstQual
                       select new
                       {
                           tech.techId,
                           tech.techName,
                           tech.subtechName
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {

            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String BindsubTech()
    {
        try
        {
            List<KnowledgeBaseBLL> lstQual = KnowledgeBaseBLL.getallTech("GetTech");
            var data = from tech in lstQual
                       select new
                       {
                           tech.techId,
                           tech.techName,
                           tech.subtechName
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {

            return null;
        }
    }

    protected void lnkHistory_Click(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            // string output = "Insert Failed";

            string empName = UD.Name;
            int empId = UD.EmployeeID;
            string commentHistory = Textarea1.InnerText;
            int kbId = Convert.ToInt32(hdnkbId.Value);
            //  throw new Exception("alert");

            KnowledgeBaseBLL.insertcommenthistory(commentHistory, kbId, empId);

            //   int id = KnowledgeBaseBLL.Insertcmmnt( commentHistory, empId, kbId);       
            //if (id != 0)
            //{
            //    output = "Inserted Successfully";
            //}
            Textarea1.InnerText = "";
        }

    }

    [System.Web.Services.WebMethod]
    public static String getHistory(int kbId)
    {
        try
        {
            List<KnowledgeBaseBLL> lstgetHistory = new List<KnowledgeBaseBLL>();
            lstgetHistory = KnowledgeBaseBLL.GetCmmntHistory(kbId);

            var data = (from CurGetProjDetailsByProjId in lstgetHistory
                        select new
                        {
                            CurGetProjDetailsByProjId.masterId,
                            CurGetProjDetailsByProjId.empName,
                            CurGetProjDetailsByProjId.commentHistory,
                            CurGetProjDetailsByProjId.hDate,
                            CurGetProjDetailsByProjId.kbId
                        }).ToList();
            ;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception)
        {

            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String getRepater(int kbId)
    {
        try
        {
            List<KnowledgeBaseBLL> lstrepeater = new List<KnowledgeBaseBLL>();
            lstrepeater = KnowledgeBaseBLL.GetCmmntHistory(kbId);

            var data = (from CurGetProjDetailsByProjId in lstrepeater
                        select new
                        {
                            CurGetProjDetailsByProjId.masterId,
                            CurGetProjDetailsByProjId.empName,
                            CurGetProjDetailsByProjId.commentHistory,
                            CurGetProjDetailsByProjId.hDate,
                            CurGetProjDetailsByProjId.kbId
                        }).ToList();
            ;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception)
        {

            return null;
        }
    }

    private static string SendKnowledgeBaseMail(string mode, string strKBTitle, string strKBempname, string strKBDescription)
    {
        string output = "Could not send mail";
        string mailSubject = "";

        System.IO.StreamReader DynamicFileReader = null;
        string fileContent = null;
        if (mode == "Insert")
        {
            mailSubject = "Agora - New Knowledge Byte Added in KnowledgeBase";
            DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"../MailTemplates/KbInsertTemplate.htm"));
        }
        else
        {
            mailSubject = "Agora - Knowledge Byte Updated in KnowledgeBase";
            DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"../MailTemplates/KbUpdateTemplate.htm"));
        }
        fileContent = DynamicFileReader.ReadToEnd();
        fileContent = fileContent.Replace("{empname}", strKBempname);
        fileContent = fileContent.Replace("{title}", strKBTitle);
        fileContent = fileContent.Replace("{description}", strKBDescription);
        string strUrl = "<a href=http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/Member/Login.aspx>Click here.</a>";
        fileContent = fileContent.Replace("{url}", strUrl);
        output = CSCode.Global.SendKBMail(fileContent, mailSubject, "web@intelgain.com", ConfigurationManager.AppSettings.Get("fromEmail"), true);//ConfigurationManager.AppSettings.Get("CommonEmail")

        return output;

    }

}