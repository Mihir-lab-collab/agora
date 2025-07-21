using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Member_ProjectWBS : System.Web.UI.Page // Authentication
{

    UserMaster UM;
    public static string CurrUserid = string.Empty;
    public static string[] empId = null;
    public static string projectMileId = "";



    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;

        hdnGetCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyy");

        MasterPage.MasterInit(true, false, true, false, false);
        CurrUserid = UM.EmployeeID.ToString();
        hdnEmpName.Value = UM.Name.ToString();
        hdnEmpId.Value = UM.EmployeeID.ToString();

        HttpContext.Current.Session["EmployeeId"] = Convert.ToString(UM.EmployeeID);
        lblProjName.Text =
           Convert.ToString(HttpContext.Current.Session["ProjectName"]);

        if (!IsPostBack)
        {

            hdprojectMileId.Value = "";
        }

        hdProjId.Value = Convert.ToString(HttpContext.Current.Session["ProjectID"]);
        if (!string.IsNullOrEmpty(Session["Moduleid"] as string))
        {
            hdnModuleID.Value = Session["Moduleid"].ToString();
        }
        projectMileId = hdprojectMileId.Value;

        ProjectWBSBLL obj = new ProjectWBSBLL();
        string Result = obj.GetProfileAccess("CheckProfileAccess", UM.EmployeeID.ToString());
        hdnGetProfileAccess.Value = Result;

        int projid = Convert.ToInt16(hdProjId.Value.ToString());
        if (projid == 0)
        {
            dvall.Visible = true;
            ProjectWBS.Visible = false;
            dvbg.Style.Add("display", "block");
        }
        else
        {
            dvall.Visible = false;
            ProjectWBS.Visible = true;
            dvbg.Style.Add("display", "none");
        }

        if (UM.IsAdmin == true || UM.IsModuleAdmin == true)
        {
            btnWBS.Visible = true;

            hdnAdmin.Value = "true";
        }
        else
        {
            btnWBS.Visible = false;
            hdnAdmin.Value = "false";
        }
    }


    [System.Web.Services.WebMethod]
    public static String GetData()
    {
        try
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<ProjectWBSBLL> lstGetMileStone = ProjectWBSBLL.getMileStone("SelectMilestone", Convert.ToInt32(HttpContext.Current.Session["ProjectID"]));
            var data = from curproject in lstGetMileStone
                       orderby curproject.projMilestoneID descending
                       select new
                       {
                           curproject.projMilestoneID,
                           curproject.projID,
                           curproject.name,
                           curproject.dueDate,
                           curproject.DeliveryDate,
                           curproject.EstHours,
                           curproject.MilestoneHours,
                           curproject.ActualHrs
                       };
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }
    }




    [System.Web.Services.WebMethod]
    public static String GetProjectWBS(int showCompletedStatus)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        List<ProjectWBSBLL> AllProjects = ProjectWBSBLL.getProjectWBS("SelectProjectWBS", Convert.ToInt32(HttpContext.Current.Session["ProjectID"]), showCompletedStatus);//Projects(isChecked, CustId, status);
        var data = from curproject in AllProjects

                   select new
                   {
                       curproject.projMilestoneID,
                       curproject.Milestone,
                       curproject.WBSID,
                       curproject.Hours,
                       curproject.WBS,
                       curproject.StartDate,
                       curproject.ActualHrs,

                       AssignedTo = String.Join(",", (from A in ProjectWBSBLL.GetProjectMembersByProjId(curproject.WBSID).ToList()
                                                      select A.tempempName).ToList()),
                       curproject.EndDate,
                       curproject.Status,
                       curproject.Remark,
                       empId = String.Join(",", (from A in ProjectWBSBLL.GetProjectMembersByProjId(curproject.WBSID).ToList()
                                                 select A.empId).ToList()),
                       curproject.ActualStartDate,
                       curproject.ActualEndDate,
                   };
        return jss.Serialize(data);
    }



    [System.Web.Services.WebMethod]
    public static String FillEmployeeMultiselect()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(EmployeeMaster.GetAllEmployees());

    }

    [System.Web.Services.WebMethod]
    public static String FillMilestoneList()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(ProjectWBSBLL.GetMilestone("SelectAllMilestone", Convert.ToInt32(HttpContext.Current.Session["ProjectID"])));

    }

    [System.Web.Services.WebMethod]
    public static String FillStatusList()
    {
        var status = new List<ProjectWBSBLL>
        {
          new ProjectWBSBLL { Text = "Planned", Value = "Planned" },
          new ProjectWBSBLL { Text = "WIP", Value = "WIP" } ,
           new ProjectWBSBLL { Text = "Completed", Value = "Completed" }
        };
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(status);
    }

    [System.Web.Services.WebMethod]
    public static List<ProjectWBSBLL> FillMilestoneDropDown()
    {
        return ProjectWBSBLL.GetMilestone("SelectAllMilestone", Convert.ToInt32(HttpContext.Current.Session["ProjectID"]));
    }




    [System.Web.Services.WebMethod]
    public static String BindNameDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(EmployeeMaster.GetAllEmployees());
    }


    [System.Web.Services.WebMethod]
    public static String BindWBS()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(ProjectWBSBLL.BindWBS("BindWBS", Convert.ToInt32(HttpContext.Current.Session["ProjectID"]), Convert.ToInt32(HttpContext.Current.Session["EmployeeId"])));
    }

    [System.Web.Services.WebMethod]
    public static String BindAllWBS()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(ProjectWBSBLL.BindAllWBS("BindAllWBS", Convert.ToInt32(HttpContext.Current.Session["ProjectID"]), Convert.ToInt32(HttpContext.Current.Session["EmployeeId"])));
    }

    [System.Web.Services.WebMethod]
    public static String BindModuleDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(ProjectModule.GetModules(Convert.ToInt32(HttpContext.Current.Session["ProjectID"])));

    }

    protected void btnSaveWBS_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            DateTime Entrydate = Convert.ToDateTime(Request.Form["ctl00$ContentPlaceHolder1$txtSDate"]);
            DateTime Fixdays = Convert.ToDateTime(DateTime.Now.AddDays(-3));
            int result = DateTime.Compare(Fixdays.Date, Entrydate.Date);

            if (result > 0)
            {
                bool Approval = EmployeeTimesheetRequestBLL.CheckForApproval(UM.EmployeeID, DateTime.Now.Date);
                if (Approval == false)
                {
                    ProjectWBSBLL obj = new ProjectWBSBLL();
                    string GetProfileAccess = obj.GetProfileAccess("CheckProfileAccess", UM.EmployeeID.ToString());
                    if (GetProfileAccess !="true")
                    {
                        Page.RegisterStartupScript("", "<script type='text/javascript'>alert('You are not allowed to fill timesheet for this date');</script>");
                        return;
                    }
                }
            }

            UM = UserMaster.UserMasterInfo();

            int intWBSID = 0;
            int intWBS = 0;
            if (hdnWBSID.Value != "")
            {
                intWBSID = Convert.ToInt32(hdnWBSID.Value.ToString());
                intWBS = Convert.ToInt32(hdnWBSName.Value);
            }
            else
            {
                intWBS = Convert.ToInt32(txtWBS.Value);
            }
            string strName = "";
            string Admin = hdnAdmin.Value;
            if (Admin == "true")
            {
                strName = Convert.ToString(txtName.Value).Trim();
            }
            else
            {
                strName = Convert.ToString(UM.EmployeeID);
            }

            int ModuleID = Convert.ToInt32(txtModule.Value);


            int intInsertedBy = Convert.ToInt32(UM.EmployeeID);

            string strComment = Convert.ToString(txtComment.Value).Trim();
            int Hours = CalculateHrs(hdnSTime.Value, hdnETime.Value);

            int ProjID = Convert.ToInt32(HttpContext.Current.Session["ProjectID"]);
            Session["Moduleid"] = Convert.ToString(ModuleID);
            int output = ProjectWBSBLL.InsertProjWBSDetails(intWBSID, strName, txtSDate.Value, txtEDate.Value, intWBS, strComment, Hours, ModuleID, intInsertedBy, ProjID);
            // SendMail(intWBSID,strName, intWBS);
            string isSendMail = Session["IsSendMail"].ToString();
            if (isSendMail == "True" || isSendMail == "true")
            {
                clsTimeSheetEmail objTSEmail = new clsTimeSheetEmail();
                objTSEmail.ModuleName = Convert.ToString(Request.Form["ctl00$ContentPlaceHolder1$hdnModuleName"]);
                objTSEmail.ProjName = Session["ProjectName"].ToString();
                //objTSEmail.EmpName = Convert.ToString(Request.Form["ctl00$ContentPlaceHolder1$hdnSelectedEmpName"]);
                var Employeename = Convert.ToString(Request.Form["ctl00$ContentPlaceHolder1$hdnSelectedEmpName"]);    // get value when dropdown chnage function call
                if (Employeename == "" || Employeename == null)
                {
                    if (Admin == "true")
                    {
                        objTSEmail.EmpName = hdnEmpName.Value; 
                    }
                    else
                    {
                        string Empname = txtName.Value;
                        objTSEmail.EmpName = Empname; 
                    }
                }
                else
                {
                    objTSEmail.EmpName = Convert.ToString(Request.Form["ctl00$ContentPlaceHolder1$hdnSelectedEmpName"]);
                }
                objTSEmail.StartDate = Convert.ToDateTime(Request.Form["ctl00$ContentPlaceHolder1$txtSDate"]);
                objTSEmail.EndDate = Convert.ToDateTime(Request.Form["ctl00$ContentPlaceHolder1$txtEDate"]);
                objTSEmail.TSComment = Convert.ToString(txtComment.Value).Trim();
                objTSEmail.ProjID = Convert.ToInt32(Session["ProjectId"].ToString());
                objTSEmail.WBSName = Convert.ToString(Request.Form["ctl00$ContentPlaceHolder1$hdnWBS"]);

                objTSEmail.intWBSID = intWBSID;
                objTSEmail.UserID = UM.EmployeeID;
                objTSEmail.IsWBS = true;
                objTSEmail.SendMail(intWBSID);
            }
            hdnCheckExists.Value = string.Empty;
            Response.Redirect("/Member/ProjectWBS.aspx");
        }

    }

    [System.Web.Services.WebMethod]
    public static List<ProjectWBSBLL> GetProjectWBSDetails(string StartDate,string EndDate)
    {
        try
        {
            //Changes it later on 
            int employeeID = Convert.ToInt32(HttpContext.Current.Session["EmployeeId"]);
            List<ProjectWBSBLL> lstGetProjectWBSDetails = ProjectWBSBLL.GetProjectWBSDetails("SelectProjectWBSDetails", employeeID, Convert.ToInt32(HttpContext.Current.Session["ProjectID"]),StartDate,EndDate);
            return lstGetProjectWBSDetails;
        }

        catch (Exception)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string checkWBSExists(string StartDate, string EndDate, string EmpID)
    {
        try
        {
            // string EmpID = HttpContext.Current.Session["EmployeeId"].ToString();
            string msg = ProjectWBSBLL.checkWBSExists("checkWBSExists", StartDate, EndDate, EmpID, Convert.ToInt32(HttpContext.Current.Session["ProjectID"]));
            return msg;
        }

        catch (Exception)
        {
            return null;
        }
    }

    public static int ConvertToMin(int Hours)
    {
        return Hours * 60;
    }

    public static int CalculateHrs(string _startdatetime, string _endDatetime)
    {
        int Value2, Value1, Value = 0;
        try
        {
            DateTime dtstartdate = Convert.ToDateTime(_startdatetime);
            DateTime dtenddate = Convert.ToDateTime(_endDatetime);
            int totaldays = (dtenddate - dtstartdate).Days;

            if (dtstartdate.Date != dtenddate.Date)
            {
                TimeSpan defaultStartTime = new TimeSpan(19, 00, 00);
                //DateTime strtDateToCheck = dtstartdate.Date + defaultStartTime;
                TimeSpan Start = DateTime.Parse(Convert.ToString(dtstartdate)).Subtract(DateTime.Parse(Convert.ToString(dtenddate)));
                Value1 = (int)Start.TotalMinutes;
                if (Convert.ToString(Value1).Contains("-"))
                {
                    Value1 = Convert.ToInt32(Convert.ToString(Value1).Replace('-', ' '));
                }

                if (totaldays == 1)
                {
                    Value = Value1;
                }
                else
                {
                    //int daydiff = ((totaldays - 1) * 9) * 60;
                    //Value = Value1 + daydiff;
                    TimeSpan duration = DateTime.Parse(Convert.ToString(dtstartdate)).Subtract(DateTime.Parse(Convert.ToString(dtenddate)));
                    Value = (int)duration.TotalMinutes;

                    if (Convert.ToString(Value).Contains("-"))
                    {
                        Value = Convert.ToInt32(Convert.ToString(Value).Replace('-', ' '));
                    }
                }
            }
            else
            {
                TimeSpan duration = DateTime.Parse(Convert.ToString(dtstartdate)).Subtract(DateTime.Parse(Convert.ToString(dtenddate)));
                Value = (int)duration.TotalMinutes;

                if (Convert.ToString(Value).Contains("-"))
                {
                    Value = Convert.ToInt32(Convert.ToString(Value).Replace('-', ' '));
                }
            }
        }
        catch (Exception Ex)
        {
        }
        return Value;

    }


    public void ClearFields()
    {
        hdprojectMileId.Value = "";
        txteditWBS.Value = "";
        txtWBSSDate.Value = "";
        txtWBSEDate.Value = "";
        txtStatus.Value = "";
        txtremark.Value = "";
    }

    [System.Web.Services.WebMethod]
    public static string CalculateHours(string StartDate, string EndDate)
    {

        int WBShrs = CalculateHrs(StartDate, EndDate);
        var timeSpan = TimeSpan.FromMinutes(WBShrs);
        var hh = (int)timeSpan.TotalHours;
        var mm = timeSpan.Minutes;
        int ss = timeSpan.Seconds;
        string result = Convert.ToString(hh) + "$" + Convert.ToString(mm);
        return result;
    }

    [System.Web.Services.WebMethod]
    public static string SaveWBS(int ProjMileId, string WBS, string StartDate, string EndDate, string Status, string Remark, string Assignto, int hours, int min)
    {

        List<ProjectWBSBLL> ProjWBS = new List<ProjectWBSBLL>();
        ProjectWBSDAL objdal = new ProjectWBSDAL();
        try
        {
            int hour = Convert.ToInt32(hours);
            int minutes = ConvertToMin(hour);
            minutes = minutes + min;
            int WBShrs = minutes;

            string result = new ProjectWBSBLL().InsertWBS("Insert", ProjMileId, WBS, StartDate, EndDate, WBShrs, Status, Remark, CurrUserid, Convert.ToInt32(HttpContext.Current.Session["ProjectID"]));
            if (Assignto != "")
            {
                objdal.DeleteEmpWBS("Delete", result);
                empId = Assignto.Split(',');
                for (int i = 0; i < empId.Length - 1; i++)
                {
                    objdal.InsertEmpID("InsertEmpId", empId[i], result, Convert.ToInt32(HttpContext.Current.Session["ProjectID"]));
                }
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(ProjWBS);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Created by Ganesh Pawar : 17/11/2016
    /// </summary>
    protected void btnDeleteWBSTimeId_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(hdnProjWBSTimesheetId.Value))
            {
                int ProjectId = ProjectWBSBLL.DeleteProjWBS(Convert.ToInt32(hdnProjWBSTimesheetId.Value));
            }
        }
        catch (Exception ex)
        { }
        //System.Web.UI.ScriptManager.RegisterStartupScript(this, true);
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            ProjectWBSDAL objdal = new ProjectWBSDAL();

            if (hdprojectMileId.Value != "")
            {
                int min = Convert.ToInt32(hdnMin.Value);
                int hour = Convert.ToInt32(txtPlannedHours.Value);
                int minutes = ConvertToMin(hour);
                minutes = minutes + min;
                int WBShrs = minutes;

                ProjectWBSBLL.UpdateWBS("Update", Convert.ToInt32(hdprojectMileId.Value), txtEditeditWBS.Value, txtEditWBSSDate.Value, txtEditWBSEDate.Value, WBShrs, txtEditStatus.Value, txtEditremark.Value, CurrUserid, Convert.ToInt32(HttpContext.Current.Session["ProjectID"]), Convert.ToInt32(hdprojectWBSID.Value));

                if (hdAssignTo.Value != "")
                {
                    objdal.DeleteEmpWBS("Delete", hdprojectWBSID.Value);
                    empId = hdAssignTo.Value.Split(',');
                    for (int i = 0; i < empId.Length - 1; i++)
                    {
                        objdal.InsertEmpID("InsertEmpId", empId[i], hdprojectWBSID.Value, Convert.ToInt32(HttpContext.Current.Session["ProjectID"]));
                    }
                }
                ClearFields();
            }
            Response.Redirect("ProjectWBS.aspx", false);
        }
        catch (Exception Ex)
        {

        }
    }

}