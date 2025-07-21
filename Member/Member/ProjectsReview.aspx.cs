using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Customer.BLL;
using AjaxControlToolkit;
using System.Data;
using System.Data.OleDb;
using Customer.DAL;
using System.Text;
using System.Globalization;
using CSCode;
using System.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Collections;

public partial class Member_ProjectsReview : System.Web.UI.Page
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UM = UserMaster.UserMasterInfo();
            Admin MasterPage = (Admin)Page.Master;
            if (!IsPostBack)
            {
                if (UM == null) { return; }

                if (!(UM.IsAdmin || UM.IsModuleAdmin))
                {
                    hdnAdmin.Value = Convert.ToString(UM.EmployeeID);
                }
            }
        }
        catch (Exception ex)
        {
            string strSubject = "Agora_ProjectsReview_PageLoad Failed";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, strSubject, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
        }
    }




    [System.Web.Services.WebMethod]
    public static string GetTSBreakupDetails(int prjId)
    {
        try
        {
            List<BITS> lsProjects = new List<BITS>();

            lsProjects = BITS.GetTSBreakupDetails(prjId);

            var data = from pr in lsProjects
                       select new
                       {
                           pr.Module,
                           pr.ActualHour,
                           pr.UnApprovedHours,
                           pr.Cost,
                           pr.Reportdate
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
    public static string BindProjectDetails(int PMID, bool? showOverHeads, bool? Added, bool? Initiated, bool? InProgress, bool? UnderUAT, bool? OnHold, bool? CompletedClosed, bool? Cancelled, bool? UnderWarranty, bool? TNM, bool? FixedCost)
    {
        try
        {
            List<BITS> lsProjects = new List<BITS>();

            lsProjects = BITS.GetProjectDetails(PMID, showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);

            var data = from pr in lsProjects
                       select new
                       {
                           pr.ID,
                           pr.Name,
                           pr.PM,
                           pr.BA,
                           pr.AccManager,
                           pr.Duration,
                           pr.BudgetedHour,
                           pr.ActualHour,
                           pr.UnApprovedHours,
                           pr.Status,
                           pr.BudgetedCost,
                           pr.ActualCost,
                           pr.ActualPayment,
                           pr.ProjectHealth_Effort,
                           pr.ProjectHealth_Cost,
                           pr.Reportdate,
                           //pr.PayReceivedIncreased,
                           //pr.ActualCostIncreased
                       };


            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception ex)
        {
            return null;
        }

    }



    #region UnUsed
    [System.Web.Services.WebMethod]
    public static String FillAttendeeslist()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(EmployeeMaster.GetAllEmployees());
    }
    private void messageBox(string statusmsg)
    {
        throw new NotImplementedException();
    }
    #endregion UnUsed


    #region Shubh
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string Save_MeetingDetails(string MODE, int MeetingId, string MeetingDate, int CalledBy, string MeetingType, string Attendees, string AgendaTopic, int Facilitator, string TimeAlloted)
    {
        string sReturn = "Success";
        try
        {
            DateTime Meeting_Date = Convert.ToDateTime(MeetingDate);
            int InsertedBy = 0;
            PeojectsReviewBLL.SaveMeetingDetails(MODE, MeetingId, Meeting_Date, CalledBy, MeetingType, Attendees, AgendaTopic, Facilitator, TimeAlloted, InsertedBy);
            sReturn = "Success";
        }
        catch (Exception ex)
        {
            sReturn = "Agora_Save_MeetingDetails Failed::";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, sReturn, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
        }
        return sReturn;
    }
    [WebMethod]
    public static String Cancel_ProjectReviewMeeting(string Mode, int MeetingId)
    {
        string sReturn = "";
        try
        {
            PeojectsReviewBLL.Cancel_Meeting(Mode, MeetingId);
            sReturn = "Success";
            return sReturn;
        }
        catch (Exception ex)
        {
            sReturn = "Agora_Cancel_ProjectReviewMeeting Failed";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, sReturn, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
            return sReturn;
        }
    }
    [WebMethod]
    public static String FillEmployeeDropDown()
    {
        try
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(EmployeeMaster.GetAllEmployees());
        }
        catch (Exception ex)
        {
            string sReturn = "Agora_FillEmployeeDropDown Failed::";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, sReturn, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
            return sReturn;
        }
    }
    
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string Get_MeetingDetails(string MODE, int MeetingId)
    {
        try
        {
            List<PeojectsReviewBLL> lstforInit = new List<PeojectsReviewBLL>();
            lstforInit = PeojectsReviewBLL.Get_ProjectReviewMeetingList(MODE, MeetingId);
            var data = (from obj in lstforInit
                        select new PeojectsReviewBLL
                        {
                            MeetingId = obj.MeetingId,
                            MeetingDate = obj.MeetingDate,
                            AgendaTopic = obj.AgendaTopic,
                            CalledById = obj.CalledById,
                            CalledByName = obj.CalledByName,
                            Attendees = obj.Attendees,
                            AttendeesId = obj.AttendeesId,
                            MeetingType = obj.MeetingType,
                            FacilitatorId = obj.FacilitatorId,
                            FacilitatorName = obj.FacilitatorName,
                            TimeAlloted = obj.TimeAlloted,
                            InsertedOn = obj.InsertedOn,
                            InsertedBy = obj.InsertedBy,
                            MeetingStatus = obj.MeetingStatus,
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            string strSubject = "Agora_Get_MeetingDetails Failed";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, strSubject, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
            return strSubject;
        }
    }
   
    #endregion Shubh


}