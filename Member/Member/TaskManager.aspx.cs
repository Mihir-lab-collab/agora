using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Customer.BLL;
using System.Collections;
using System.IO;
using System.Configuration;
using CSCode;

public partial class Member_TaskManager : Authentication
{
    static UserMaster UM;

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(true, false);

        if (!IsPostBack)
        {
            Session["UserName"] = UM.Name.ToString();
            Session["CustId"] = UM.EmployeeID.ToString();
            Session["UserId"] = UM.EmployeeID.ToString();
            Session["UserMailid"] = UM.EmailID.ToString();
            hdn.Value = UM.IsAdmin.ToString();
            hfLoginId.Value = UM.EmployeeID.ToString();
            hfUserName.Value = UM.Name.ToString();
            hfProjId.Value = Convert.ToString(HttpContext.Current.Session["ProjectId"]);

            if (UM.IsAdmin == true || UM.IsModuleAdmin == true)
            {
                PrevMonth.Style.Add("display", "block");
            }
            else
            {
                PrevMonth.Style.Add("display", "none");
            }

            if (Request.Files.Count > 0)
            {
                Storage ObjStorge = new Storage();
                String nm = Request.Files[0].FileName;
                ObjStorge.FileOperation(Storage.ModuleType.Temp, Storage.OpType.Add, nm, Request.Files[0].InputStream);
                if (Request.Files.AllKeys[0] == "Editfiles")
                    Session["TaskFileNamesUpdate"] = nm;
                else
                    Session["TaskFileNames"] = nm;
            }
        }
    }

    [System.Web.Services.WebMethod]
    public static String ClearTempFilesandSession()
    {
        string[] filenames = null;
        try
        {
            if(HttpContext.Current.Session["TaskFileNames"]!=null)
        {
             filenames = HttpContext.Current.Session["TaskFileNames"].ToString().TrimStart(',').Split(',');
        }

            if (HttpContext.Current.Session["TaskFileNamesUpdate"] != null)
            {
                 filenames = HttpContext.Current.Session["TaskFileNames"].ToString().TrimStart(',').Split(',');
            }
         
            HttpContext.Current.Session["TaskFileNamesUpdate"] = null;
            HttpContext.Current.Session["TaskFileNames"] = null;
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

    [System.Web.Services.WebMethod]
    public static String BindBugsByProjId(int projId, string includeTerminated)
    {
        try
        {

            List<Customer.BLL.Bugs> lstGetBugsByProjId = Customer.BLL.Bugs.GetBugsByProjId(projId, includeTerminated); //zz
            List<bugStatuses> lstGetAllbugStatuses = bugStatuses.GetAllbugStatuses();
            List<bugPriorities> lstGetAllbugPriorities = bugPriorities.GetAllbugPriorities();
            string IsType;

            var data = from curBugsByProjId in lstGetBugsByProjId
                       join curbugStatuses in lstGetAllbugStatuses
                       on curBugsByProjId.status_id equals curbugStatuses.status_id
                       join curbugPriorities in lstGetAllbugPriorities
                       on curBugsByProjId.priority_id equals curbugPriorities.priority_id
                       select new
                       {
                           curBugsByProjId.bug_id,
                           curBugsByProjId.bug_name,
                           bug_desc = curBugsByProjId.bug_desc.Replace("\n", "<br>"),
                           curBugsByProjId.resolution,
                           curbugStatuses.status,
                           curbugStatuses.status_id,
                           curbugStatuses.SortOrder,
                           bug_lastModified = curBugsByProjId.bug_lastModified.Date,
                           curBugsByProjId.moduleID,
                           priority = curbugPriorities.priority_desc,
                           priority_id = curbugPriorities.priority_id,
                           empid = curBugsByProjId.assigned_to,
                           IsType = curBugsByProjId.istype == 1 ? "FD" : (curBugsByProjId.istype == 2 ? "TD" : "Change"),
                           IsTypeID = curBugsByProjId.istype,
                           EmployeeMaster.GetEmpDetailsByEmployeeId(curBugsByProjId.assigned_to).empName,
                           date_assigned = curBugsByProjId.date_assigned.Date,
                           curBugsByProjId.assigned_by,
                           curBugsByProjId.assigned_by_Name,
                           curBugsByProjId.ModuleName
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
    public static String GetTotalReportBugsCountByProjId(int projId)
    {
        try
        {

            List<Customer.BLL.Bugs> lstGetTotalReportBugsCountByProjId = Customer.BLL.Bugs.GetTotalReportBugsCountByProjId(projId, "EmpTotalReports");
            Hashtable hashtable = new Hashtable();
            hashtable.Add("key", "TotalBugsCount");
            hashtable.Add("value", lstGetTotalReportBugsCountByProjId.Count());
            var data = hashtable;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String DeletebugById(int bug_Id)
    {
        try
        {
            GetAllAttachmentToDelete(bug_Id);
            bool output = Customer.BLL.Bugs.DeletebugById(bug_Id);
            string isfileremoved = "false";
            if (output == true)
            {
                isfileremoved = "true";

            }

            Hashtable hashtable = new Hashtable();
            hashtable.Add("key", "File Deleted");
            hashtable.Add("value", isfileremoved);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(hashtable);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private static void GetAllAttachmentToDelete(int BugId)
    {
        List<Customer.BLL.bugAttachments> lstGetAllbugAttachmentsByBugId = Customer.BLL.bugAttachments.GetAllbugAttachmentsByBugId(Convert.ToInt32(BugId));

        foreach (var FileImg in lstGetAllbugAttachmentsByBugId)
        {
            Storage ObjStorge = new Storage();
            string DesFileName = FileImg.bugFilePath;

            ObjStorge.FileOperation(Storage.ModuleType.TaskManager, Storage.OpType.Delete, DesFileName);
        }
    }

    [System.Web.Services.WebMethod]
    public static String GetbugPrioritiesByProjId(int projId)
    {
        try
        {
            List<bugPriorities> lstGetbugPrioritiesByProjId = bugPriorities.GetbugPrioritiesByProjId(projId, "EmpBugPriorities");
            var data = from CurGetbugPrioritiesByProjId in lstGetbugPrioritiesByProjId
                       group CurGetbugPrioritiesByProjId by CurGetbugPrioritiesByProjId.priority_desc into grp
                       select new { key = grp.Key, value = grp.Count() };


            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String GetbugStatusesByProjId(int projId)
    {
        try
        {
            List<bugStatuses> lstGetbugStatusesByProjId = bugStatuses.GetbugStatusesByProjId(projId, "EmpStatus");
            var data = from CurGetbugStatusesByProjId in lstGetbugStatusesByProjId
                       group CurGetbugStatusesByProjId by CurGetbugStatusesByProjId.status into grp
                       select new { key = grp.Key, value = grp.Count() };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String GetAllStatusReportByProjId(int projId)
    {
        try
        {
            //Bug Status Details
            List<bugStatuses> lstGetbugStatusesByProjId = bugStatuses.GetbugStatusesByProjId(projId, "EmpStatus");
            var Statusdata = from CurGetbugStatusesByProjId in lstGetbugStatusesByProjId
                             group CurGetbugStatusesByProjId by CurGetbugStatusesByProjId.status into grp
                             select new { key = grp.Key, value = grp.Count() };


            //Bug priority Details
            List<bugPriorities> lstGetbugPrioritiesByProjId = bugPriorities.GetbugPrioritiesByProjId(projId, "EmpBugPriorities");
            var Prioritydata = from CurGetbugPrioritiesByProjId in lstGetbugPrioritiesByProjId
                               group CurGetbugPrioritiesByProjId by CurGetbugPrioritiesByProjId.priority_desc into grp
                               select new { key = grp.Key, value = grp.Count() };

            //Bug Count
            List<Customer.BLL.Bugs> lstGetTotalReportBugsCountByProjId = Customer.BLL.Bugs.GetTotalReportBugsCountByProjId(projId, "EmpTotalReports");
            getallreportdetails totalReportBugs = new getallreportdetails();
            totalReportBugs.key = "TotalBugsCount";
            totalReportBugs.value = lstGetTotalReportBugsCountByProjId.Count();

            //Merge all for report
            List<getallreportdetails> all = new List<getallreportdetails>();
            all.AddRange(Statusdata.Select(p => new getallreportdetails() { key = p.key, value = p.value }).ToList());
            all.AddRange(Prioritydata.Select(p => new getallreportdetails() { key = p.key, value = p.value }).ToList());
            all.Add(totalReportBugs);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(all);

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String GetProjectModulesByProjectId(int projId)
    {
        try
        {
            List<projectModuleMaster> lstGetprojectModuleMasterByProjectId = projectModuleMaster.GetprojectModuleMasterByProjectId(projId);

            var data = from CurGetprojectModuleMasterByProjectId in lstGetprojectModuleMasterByProjectId
                       select new { CurGetprojectModuleMasterByProjectId.moduleId, CurGetprojectModuleMasterByProjectId.moduleName };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String GetProjectEmployeesByProjId(string projId)
    {
        try
        {
            List<EmployeeMaster> lstGetEmpDetailsByProjId = EmployeeMaster.GetEmpDetailsByProjId(Convert.ToInt32(projId));

            var data = (from CurGetEmpDetailsByProjId in lstGetEmpDetailsByProjId
                        select new
                        {
                            CurGetEmpDetailsByProjId.empid,
                            CurGetEmpDetailsByProjId.empName
                        }).ToList();
            ;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String GetAllbugPriorities()
    {

        try
        {
            List<Customer.BLL.bugPriorities> lstGetAllbugPriorities = Customer.BLL.bugPriorities.GetAllbugPriorities();
            var data = (from CurGetAllbugPriorities in lstGetAllbugPriorities

                        select new
                        {
                            CurGetAllbugPriorities.priority_id,
                            CurGetAllbugPriorities.priority_desc
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception ex)
        {
            return null;
        }

    }

    [System.Web.Services.WebMethod]
    public static String GetAllbugStatuses()
    {
        try
        {
            List<bugStatuses> lstGetAllbugStatuses = bugStatuses.GetAllbugStatuses();
            var data = (from CurGetAllbugStatuses in lstGetAllbugStatuses

                        select new
                        {
                            CurGetAllbugStatuses.status_id,
                            CurGetAllbugStatuses.status
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string GetAllbugAttachmentsByBugId(string BugId)
    {
        try
        {
            List<Customer.BLL.bugAttachments> lstGetAllbugAttachmentsByBugId = Customer.BLL.bugAttachments.GetAllbugAttachmentsByBugId(Convert.ToInt32(BugId));

            var data = (from curGetAllbugAttachmentsByBugId in lstGetAllbugAttachmentsByBugId
                        select new
                        {
                            curGetAllbugAttachmentsByBugId.bugFilePath,
                            curGetAllbugAttachmentsByBugId.bugFileId

                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string GetFirstbugAttachmentsByBugId(string BugId)
    {
        try
        {
            List<Customer.BLL.bugAttachments> lstGetFirstbugAttachmentsByBugId = Customer.BLL.bugAttachments.GetAllbugAttachmentsByBugId(Convert.ToInt32(BugId));

            var data = (from curGetFirstbugAttachmentsByBugId in lstGetFirstbugAttachmentsByBugId
                        where curGetFirstbugAttachmentsByBugId.bugsResolutionId.Equals(0)
                        select new
                        {
                            curGetFirstbugAttachmentsByBugId.bugFilePath,
                            curGetFirstbugAttachmentsByBugId.bugFileId

                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string GetAllbugsResolutionByBugId(string BugId)
    {
        try
        {
            var nodata = "No records";
            List<bugsResolution> lstGetAllbugsResolutionByBugId = bugsResolution.GetAllbugsResolutionByBugId(Convert.ToInt32(BugId));
            List<bugStatuses> lstbugStatuses = bugStatuses.GetAllbugStatuses();
            List<bugPriorities> lstbugPriorities = bugPriorities.GetAllbugPriorities();

            var data = (from curGetAllbugsResolutionByBugId in lstGetAllbugsResolutionByBugId
                        join curbugStatuses in lstbugStatuses
                        on curGetAllbugsResolutionByBugId.status_id equals curbugStatuses.status_id
                        join curbugPriorities in lstbugPriorities
                        on curGetAllbugsResolutionByBugId.priority_id equals curbugPriorities.priority_id
                        orderby curGetAllbugsResolutionByBugId.bugsResolutionId descending
                        select new
                        {
                            curGetAllbugsResolutionByBugId.bugsResolutionId,
                            curGetAllbugsResolutionByBugId.bug_id,
                            status = curbugStatuses.status,
                            priority = curbugPriorities.priority_desc,
                            curGetAllbugsResolutionByBugId.resolution,
                            resolutionBy = Users.CustUserDetails(curGetAllbugsResolutionByBugId.resolutionBy),
                            curGetAllbugsResolutionByBugId.resolutionDate,
                            curGetAllbugsResolutionByBugId.insertedIp,
                            ResolutionFiles = String.Join("\n", (from curGetBugResolutionAttachmentsByBugId in bugAttachments.GetAllbugAttachmentsByBugId(curGetAllbugsResolutionByBugId.bug_id)
                                                                 where curGetBugResolutionAttachmentsByBugId.bugsResolutionId == curGetAllbugsResolutionByBugId.bugsResolutionId
                                                                 select curGetBugResolutionAttachmentsByBugId.bugFilePath).ToList())
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();

            if (data.Count() > 0)
                return jss.Serialize(data);
            else
                return jss.Serialize(nodata);

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static string GetBugResolutionAttachmentsByBugId(string BugId)
    {
        try
        {
            List<Customer.BLL.bugAttachments> lstGetBugResolutionAttachmentsByBugId = Customer.BLL.bugAttachments.GetAllbugAttachmentsByBugId(Convert.ToInt32(BugId));

            var data = (from curGetBugResolutionAttachmentsByBugId in lstGetBugResolutionAttachmentsByBugId
                        where curGetBugResolutionAttachmentsByBugId.bugsResolutionId != 0
                        select new
                        {
                            curGetBugResolutionAttachmentsByBugId.bugFilePath,
                            curGetBugResolutionAttachmentsByBugId.bugFileId

                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String DeletebugAttachmentsByFileId(int bugFileId, string bugFileName)
    {
        try
        {
            bool output = bugAttachments.DeletebugAttachmentsByFileId(bugFileId);
            string isfileremoved = "false";
            Storage ObjStorge = new Storage();
            if (output == true)
            {
                isfileremoved = "true";
                string Curfile = bugFileName;
                ObjStorge.FileOperation(Storage.ModuleType.TaskManager, Storage.OpType.Delete, Curfile);
            }

            Hashtable hashtable = new Hashtable();
            hashtable.Add("key", "File Deleted");
            hashtable.Add("value", isfileremoved);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(hashtable);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String InsertbugAttachments(int bug_Id, string bugFilePath)
    {
        try
        {
            bool output = bugAttachments.InsertbugAttachments(bug_Id, bugFilePath);
            string isfileadded = "false";
            if (output == true)
                isfileadded = "true";

            Hashtable hashtable = new Hashtable();
            hashtable.Add("key", "File Added");
            hashtable.Add("value", isfileadded);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(hashtable);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String Insertbugs(int moduleID, int priority_id, int status_id, string bug_name, string bug_desc, string resolution, string assigned_by, int assigned_to, byte IsType)
    {
        try
        {
            int output = Customer.BLL.Bugs.Insertbugs(moduleID, priority_id, status_id, bug_name, bug_desc, resolution, assigned_by, assigned_to, IsType);
            Hashtable hashtable = new Hashtable();
            hashtable.Add("key", "Bug Added");
            hashtable.Add("value", output);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(hashtable);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]//(EnableSession = true)In order to store session state in web method we use enablesession
    public static string UpdatebugByBugId(int bug_Id, int priority_id, int statusId, int empId, int loginId, string UMName, int ProjId, string CommentHistory, string Resolution, byte IsTypeId)
    {
        string output = "Update Failed";
        try
        {
            string ResolutionData = "";
            string strResolution = Resolution.Replace('|', '\\');
            bool isupdated = Customer.BLL.Bugs.UpdatebugByBugId(bug_Id, priority_id, statusId, empId, ResolutionData, IsTypeId);

            int bugsResolutionId = bugsResolution.InsertbugsResolution(bug_Id, statusId, priority_id, strResolution, loginId, Global.GetLocalIPAddress());
            if (bugsResolutionId != 0)
            {
                string UpdateAttachments = InsertResolutionbugAttachments(bug_Id, bugsResolutionId);
                output = "Task updated successfully.";


                var PriorityText = (from curpriority in bugPriorities.GetAllbugPriorities()
                                    where curpriority.priority_id == Convert.ToInt32(priority_id)
                                    select new { curpriority }).FirstOrDefault();
                string Priority = PriorityText.curpriority.priority_desc;


                var StatusText = (from curstatus in bugStatuses.GetAllbugStatuses()
                                  where curstatus.status_id == Convert.ToInt32(statusId)
                                  select new { curstatus }).FirstOrDefault();
                string Status = StatusText.curstatus.status;




                Customer.BLL.Bugs curbug = Customer.BLL.Bugs.GetBugBybug_id(bug_Id);

                string TaskName = curbug.bug_name.ToString();
                string TaskDescription = curbug.bug_desc.ToString();
                string BugAssignedDate = curbug.date_assigned.ToString("dd-MMM-yy");

                string projectName = Customer.BLL.Projects.GetCustomerProjectbyProjId(ProjId).projName;
                string Modulename = projectModuleMaster.GetprojectModuleMasterByModuleId(Convert.ToInt32(curbug.moduleID)).moduleName;
                string AssignedTo = "";
                if (curbug.assigned_to != 0)
                    AssignedTo = EmployeeMaster.GetEmpDetailsByEmployeeId(Convert.ToInt32(curbug.assigned_to)).empName;
                string AssignedBy = UMName;// UM.Name;
                SendMailUpdateTask(Convert.ToString(bugsResolutionId), Convert.ToString(curbug.bug_id), TaskName, Priority, projectName, Modulename, TaskDescription, AssignedTo, Status, BugAssignedDate, AssignedBy, strResolution, UpdateAttachments);

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);

    }
    public static string InsertResolutionbugAttachments(int bug_Id, int bugsResolutionId)
    {
        string Attachments = "";
        try
        {

            // string alok = allfiles;
            if (HttpContext.Current.Session["TaskFileNamesUpdate"] != null)
            {
                string[] filenames = HttpContext.Current.Session["TaskFileNamesUpdate"].ToString().Split(',');
                Storage ObjStorge = new Storage();
                for (int i = 0; i < filenames.Length; i++)
                {
                    string OriginalFileName = filenames[i];
                    string filename = System.IO.Path.GetFileNameWithoutExtension(filenames[i]).ToLower();
                    string fileextension = System.IO.Path.GetExtension(filenames[i]).ToLower();
                   // string DesFileName = bug_Id + "-" + filename + fileextension;
                    Stream s = ObjStorge.FileRead(Storage.ModuleType.Temp, OriginalFileName);

                    string DesFileName = HttpContext.Current.Server.UrlEncode(bug_Id + "-" + filename + fileextension);
                    HttpContext.Current.Response.Write(ObjStorge.FileOperation(Storage.ModuleType.TaskManager, Storage.OpType.Add, DesFileName, s));

                    Customer.BLL.bugAttachments.InsertResolutionbugAttachments(bug_Id, bugsResolutionId, DesFileName);
                    Attachments = DesFileName + ";" + Attachments;

                }
            }
            ClearTempFilesandSession();
            HttpContext.Current.Session["TaskFileNamesUpdate"] = null;
        }
        catch (Exception ex)
        {

        }
        return Attachments;

    }
    [System.Web.Services.WebMethod(EnableSession = true)]//In order to store session state in web method we use enablesession
    public static string UpdatebugAttachment(int bug_Id)
    {
        string returnString = "";
        try
        {
            if (HttpContext.Current.Session["TaskFileNamesUpdate"] != null)
            {
                string[] filenames = HttpContext.Current.Session["TaskFileNamesUpdate"].ToString().Split(',');
                Storage ObjStorge = new Storage();
                for (int i = 0; i < filenames.Length - 1; i++)
                {
                    string OriginalFileName = filenames[i];
                    string filename = System.IO.Path.GetFileNameWithoutExtension(filenames[i]).ToLower();
                    string fileextension = System.IO.Path.GetExtension(filenames[i]).ToLower();
                    Stream s = ObjStorge.FileRead(Storage.ModuleType.Temp, OriginalFileName);

                    string DesFileName = HttpContext.Current.Server.UrlEncode(bug_Id + "-" + filename + fileextension);
                    HttpContext.Current.Response.Write(ObjStorge.FileOperation(Storage.ModuleType.TaskManager, Storage.OpType.Add, DesFileName, s));
                    if (bug_Id != 0)
                    {
                        Customer.BLL.bugAttachments.InsertResolutionbugAttachments(bug_Id, 0, DesFileName);
                    }
                }
            }

            HttpContext.Current.Session["TaskFileNamesUpdate"] = null;
        }
        catch (Exception ex)
        {
        }
        return returnString;
    }
    [System.Web.Services.WebMethod]
    public static String FilterModule(String _parentId)
    {
        try
        {
            List<Customer.BLL.projectModuleMaster> lstGetModuleDtlByProjID = Customer.BLL.projectModuleMaster.GetprojectModuleMasterByProjectId(Convert.ToInt32(_parentId));
            var data = (from CurtModuleDtlByProjID in lstGetModuleDtlByProjID

                        select new
                        {
                            CurtModuleDtlByProjID.moduleId,
                            CurtModuleDtlByProjID.moduleName
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception ex)
        {
            return null;
        }
    }
    protected void lnkSaveNewTask_Click(object sender, EventArgs e)
    {
        string strTaskName, strTaskDesc = "";
        int ProjectID, ModuleID, PriorityID, AssgnToID = 0;
        ProjectID = Convert.ToInt32(Session["ProjectId"]);
        ModuleID = Convert.ToInt32(hfModuleID.Value);
        strTaskName = hfTaskName.Value;
        strTaskDesc = hfTaskDesc.Value;
        PriorityID = Convert.ToInt32(hfpriority.Value);
        if (hfAssignedTo.Value != "")
            AssgnToID = Convert.ToInt32(hfAssignedTo.Value);
        string BugByName = UM.Name;
        string strResolution = "";
        string AssignBy = "E-" + UM.EmployeeID.ToString();
        byte TypeID = Convert.ToByte(hfType.Value);
        if (ProjectID != 0 && PriorityID != 0 && strTaskName != "" && strTaskDesc != "")
        {
            string Attachments = "";
            int BugId = Customer.BLL.Bugs.Insertbugs(ModuleID, PriorityID, Convert.ToInt32(1), strTaskName, strTaskDesc, strResolution, AssignBy, AssgnToID, TypeID);
            hfBugID.Value = Convert.ToString(BugId);
            if (Session["TaskFileNames"] != null)
            {
                string[] filenames = Session["TaskFileNames"].ToString().Split(',');

                for (int i = 0; i < filenames.Length; i++)
                {
                    Storage ObjStorge = new Storage();
                    string OriginalFileName = filenames[i];
                    string filename = System.IO.Path.GetFileNameWithoutExtension(filenames[i]).ToLower().Replace(" ", "");
                    string fileextension = System.IO.Path.GetExtension(filenames[i]).ToLower();
                    Stream s = ObjStorge.FileRead(Storage.ModuleType.Temp, OriginalFileName);

                    string DesFileName = HttpContext.Current.Server.UrlEncode(hfBugID.Value + "-" + filename + fileextension);
                    HttpContext.Current.Response.Write(ObjStorge.FileOperation(Storage.ModuleType.TaskManager, Storage.OpType.Add, DesFileName, s));
                    if (hfBugID.Value != "")
                    {
                        Customer.BLL.bugAttachments.InsertbugAttachments(Convert.ToInt32(hfBugID.Value), DesFileName);
                        Attachments = DesFileName + ";" + Attachments;
                    }
                }

                ClearTempFilesandSession();
            }


            var PriorityText = (from curpriority in bugPriorities.GetAllbugPriorities()
                                where curpriority.priority_id == Convert.ToInt32(hfpriority.Value)
                                select new { curpriority }).FirstOrDefault();

            string Priority = PriorityText.curpriority.priority_desc;


            string projectName = Customer.BLL.Projects.GetCustomerProjectbyProjId(Convert.ToInt32(Session["ProjectId"])).projName;

            string Modulename = projectModuleMaster.GetprojectModuleMasterByModuleId(Convert.ToInt32(hfModuleID.Value)).moduleName;

            string AssignedTo = "";
            if (hfAssignedTo.Value != "")
                AssignedTo = EmployeeMaster.GetEmpDetailsByEmployeeId(Convert.ToInt32(hfAssignedTo.Value)).empName;

            string AssignedBy = UM.Name;

            string output = SendMailInsertTask(hfBugID.Value, hfTaskName.Value, Convert.ToString(Priority), projectName, Modulename, hfTaskDesc.Value, AssignedTo, AssignedBy, Attachments);

            messageBox("Task Inserted successfully.");
        }
        else
            messageBox("Insert failed");
        Session["TaskFileNames"] = null;
    }

    private void messageBox(string message)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('" + message + "');", true);
    }

    public string SendMailInsertTask(string taskid, string taskname, string Priority, string ProjectName, string ModuleName, string Taskdecription, string Assignto, string AssignedBy, string InsertAttachments)
    {

        Boolean blAlertCC = projectMaster.GetAlertCCProjId(Convert.ToInt32(Session["ProjectId"].ToString()));

        string output = "Could not send mail";
        if (blAlertCC)
        {
            try
            {


                string AllToEmails = "";
                string CurprojectMembers = "";
                foreach (var item in EmployeeMaster.GetEmpDetailsByProjId(Convert.ToInt32(Session["ProjectId"].ToString())).ToList())
                {
                    CurprojectMembers = item.empEmail + "," + CurprojectMembers;
                }

                AllToEmails = CurprojectMembers + "," + Customer.BLL.Projects.GetCustomerProjectbyProjId(Convert.ToInt32(Session["ProjectId"].ToString())).OtherEmailId;


                string CCs = UM.EmailID;//Session["UserMailid"].ToString();



                string strprt = "";
                if (Priority == "Showstopper")
                    strprt = "<span style='color:Red;font-weight:bold;'>Showstopper</span>";
                if (Priority == "Major")
                    strprt = "<span style='color:Orange;font-weight:bold;'>Major</span>";
                if (Priority == "Cosmetic")
                    strprt = "<span style='color:Green;font-weight:bold'>Cosmetic</span>";
                if (Priority == "Minor")
                    strprt = "<span style='color:#FF6EC7;font-weight:bold'>Minor</span>";

                string mailSubject = "";

                mailSubject = "Task No.  " + taskid + "(O) - " + Priority + ": " + taskname;
                System.IO.StreamReader DynamicFileReader = null;
                string fileContent = null;
                DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath("/MailTemplates/AddTaskManage.htm"));

                fileContent = DynamicFileReader.ReadToEnd();
                fileContent = fileContent.Replace("{projname}", ProjectName + "/" + ModuleName);
                fileContent = fileContent.Replace("{taskno}", taskid);
                fileContent = fileContent.Replace("{taskname}", taskname);
                fileContent = fileContent.Replace("{modifieddate}", DateTime.Today.ToString("dd-MMM-yy").ToString());
                fileContent = fileContent.Replace("{desc}", Taskdecription);
                fileContent = fileContent.Replace("{status}", "Open (O)");
                fileContent = fileContent.Replace("{assign_to}", Assignto);
                fileContent = fileContent.Replace("{priorityHeading}", Priority);
                fileContent = fileContent.Replace("{priority}", strprt);
                fileContent = fileContent.Replace("{assign_by}", AssignedBy);
                //string strUrl = "[<a href=http://" + Request.ServerVariables["HTTP_HOST"] + "/Login.aspx>Click here to view the task</a>]";
                string strUrl = "<a href=http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/Member/TaskManager.aspx?taskid=" + taskid + ">Click here to view the task</a>";
                fileContent = fileContent.Replace("{URL}", strUrl);
                output = CSCode.Global.SendMail(fileContent, mailSubject, AllToEmails.Replace(",,", ",").ToString(), ConfigurationManager.AppSettings.Get("fromEmail"), true, CCs, "", Storage.ModuleType.TaskManager, InsertAttachments);
              //output = CSCode.Global.SendMail(fileContent, mailSubject,"ashwini.k@intelgain.com", ConfigurationManager.AppSettings.Get("fromEmail"), true, CCs, "", Storage.ModuleType.TaskManager, InsertAttachments);

            }
            catch
            {
            }
        }
        else
        {
            output = "Alert off";
        }
        return output;
    }
    public static string SendMailUpdateTask(string bugsResolutionId, string taskid, string taskname, string Priority, string ProjectName, string ModuleName, string Taskdecription, string Assignto, string Status, string BugAssignedDate, string AssignedBy, string Comments, string updateAttachments)
    {

        Boolean blAlertCC = projectMaster.GetAlertCCProjId(Convert.ToInt32(HttpContext.Current.Session["ProjectId"].ToString()));

        string output = "Could not send mail";
        if (blAlertCC)
        {

            try
            {

                string AllToEmails = "";

                string CurprojectMembers = "";
                foreach (var item in EmployeeMaster.GetEmpDetailsByProjId(Convert.ToInt32(HttpContext.Current.Session["ProjectId"].ToString())).ToList())
                {
                    CurprojectMembers = item.empEmail + "," + CurprojectMembers;
                }

                AllToEmails = CurprojectMembers + "," + Customer.BLL.Projects.GetCustomerProjectbyProjId(Convert.ToInt32(HttpContext.Current.Session["ProjectId"].ToString())).OtherEmailId;

                string CCs = UM.EmailID;


                List<bugsResolution> lstGetAllbugsResolutionByBugId = bugsResolution.GetAllbugsResolutionByBugId(Convert.ToInt32(taskid));
                List<bugStatuses> lstbugStatuses = bugStatuses.GetAllbugStatuses();
                var AllCommentHistorys = (from curGetAllbugsResolutionByBugId in lstGetAllbugsResolutionByBugId
                                          join curbugStatuses in lstbugStatuses
                                          on curGetAllbugsResolutionByBugId.status_id equals curbugStatuses.status_id
                                          orderby curGetAllbugsResolutionByBugId.bugsResolutionId descending
                                          where curGetAllbugsResolutionByBugId.bugsResolutionId != Convert.ToInt32(bugsResolutionId)
                                          select new
                                          {
                                              status = curbugStatuses.status,
                                              curGetAllbugsResolutionByBugId.resolution,
                                              resolutionBy = Users.CustUserDetails(curGetAllbugsResolutionByBugId.resolutionBy),
                                              curGetAllbugsResolutionByBugId.resolutionDate
                                          }).ToList();


                string commenthistory = "";
                foreach (var curCommentHistory in AllCommentHistorys)
                {
                    commenthistory = commenthistory + "<tr><td align='left'>By: " + curCommentHistory.resolutionBy + "</td><td align='right'>On: " + curCommentHistory.resolutionDate.ToString("dd-MMM-yyyy") + "</td><td align='right'>Status:" + ShowStatus(curCommentHistory.status) + "</td></tr><tr><td colspan='3' style='border-bottom: solid 1px black;' ><br/>" + curCommentHistory.resolution + "</td></tr><tr>";
                }

                string strprt = "";
                if (Priority == "Showstopper")
                    strprt = "<span style='color:Red;font-weight:bold;'>Showstopper</span>";
                if (Priority == "Major")
                    strprt = "<span style='color:Orange;font-weight:bold;'>Major</span>";
                if (Priority == "Cosmetic")
                    strprt = "<span style='color:Green;font-weight:bold'>Cosmetic</span>";
                if (Priority == "Minor")
                    strprt = "<span style='color:#FF6EC7;font-weight:bold'>Minor</span>";

                string strsts = "";
                string strAbbrsts = "";
                if (Status == "Open")
                {
                    strsts = "<span style='color:Blue;font-weight:bold';>Open (O)</span>";
                    strAbbrsts = "(O)";
                }
                else if (Status == "On hold")
                {
                    strsts = "<span style='color:Red;font-weight:bold';>On hold (H)</span>";
                    strAbbrsts = "(H)";
                }

                else if (Status == "Completed")
                {
                    strsts = "<span style='color:Green;font-weight:bold';>Completed (C)</span>";
                    strAbbrsts = "(C)";
                }
                else if (Status == "In progress")
                {
                    strsts = "<span style='color:#FF6EC7;font-weight:bold';>In progress (P)</span>";
                    strAbbrsts = "(P)";
                }
                else if (Status == "Terminated")
                {
                    strsts = "<span style='color:Red;font-weight:bold'>Terminated (T)</span>";
                    strAbbrsts = "(T)";
                }


                string mailSubject = "";

                mailSubject = "Task No. " + taskid + strAbbrsts.ToString() + " - " + Priority + ": " + taskname;

                System.IO.StreamReader DynamicFileReader = null;
                string fileContent = null;
                DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath("../MailTemplates/EditTaskManage.htm"));
                fileContent = DynamicFileReader.ReadToEnd();
                fileContent = fileContent.Replace("{projname}", ProjectName + "/" + ModuleName);
                fileContent = fileContent.Replace("{taskno}", taskid);
                fileContent = fileContent.Replace("{taskname}", taskname);
                fileContent = fileContent.Replace("{modifieddate}", BugAssignedDate);
                fileContent = fileContent.Replace("{desc}", Taskdecription);
                fileContent = fileContent.Replace("{status}", strsts);
                fileContent = fileContent.Replace("{assign_to}", Assignto);
                fileContent = fileContent.Replace("{assign_by}", AssignedBy);
                fileContent = fileContent.Replace("{priorityHeading}", Priority);
                fileContent = fileContent.Replace("{priority}", strprt);
                fileContent = fileContent.Replace("{comment}", AssignedBy + " [" + System.DateTime.Now.ToString("dd-MMM-yy ") + " " + System.DateTime.Now.ToString("hh:mm:ss") + "]" + "<br/>" + Comments);
                fileContent = fileContent.Replace("{mailfrom}", taskname);
                fileContent = fileContent.Replace("{commenthistory}", commenthistory);
                string strUrl = "<a href=http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/Member/TaskManager.aspx?taskid=" + taskid + ">Click here to view the task</a>";
                fileContent = fileContent.Replace("{URL}", strUrl);
               output = CSCode.Global.SendMail(fileContent, mailSubject, AllToEmails.Replace(",,", ",").ToString(), ConfigurationManager.AppSettings.Get("fromEmail"), true, CCs, "", Storage.ModuleType.TaskManager, updateAttachments);
               
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }
        else
        {
            output = "Alert off";
        }

        return output;

    }

    public static string ShowStatus(string Status)
    {
        string strsts = "";
        if (Status == "Open")
        {
            strsts = "<span style='color:Blue;font-weight:bold';>Open (O)</span>";

        }
        else if (Status == "On hold")
        {
            strsts = "<span style='color:Red;font-weight:bold';>On hold (H)</span>";

        }

        else if (Status == "Completed")
        {
            strsts = "<span style='color:Green;font-weight:bold';>Completed (C)</span>";

        }
        else if (Status == "In progress")
        {
            strsts = "<span style='color:#FF6EC7;font-weight:bold';>In progress (P)</span>";

        }
        else if (Status == "Terminated")
        {
            strsts = "<span style='color:Red;font-weight:bold'>Terminated (T)</span>";

        }
        return strsts;

    }

    public class getallreportdetails
    {
        public string key { get; set; }
        public int value { get; set; }
    }

}

