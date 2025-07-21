using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProjectReviewMeeting : Authentication
{
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UM = UserMaster.UserMasterInfo();
            Admin MasterPage = (Admin)Page.Master;
            if (!IsPostBack)
            {
                if (UM == null)
                    return;

                if (!(UM.IsAdmin || UM.IsModuleAdmin))
                {
                    hdnAdmin.Value = Convert.ToString(UM.EmployeeID);
                }

            }
        }
        catch (Exception ex)
        {
            string strSubject = "Agora_ProjectReviewMeeting_PageLoad Failed";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, strSubject, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
        }
    }

    private void messageBox(string statusmsg)
    {
        throw new NotImplementedException();
    }
    [WebMethod]
    public static String Get_ProjectReviewMeetingList(string Mode, int MeetingId)
    {
        try
        {
            List<PeojectsReviewBLL> lstforInit = new List<PeojectsReviewBLL>();
            lstforInit = PeojectsReviewBLL.Get_ProjectReviewMeetingList(Mode, MeetingId);
            var data = (from obj in lstforInit
                        select new
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
            string strSubject = "Agora_GetProjectReviewMeetingList Failed";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, strSubject, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
            return strSubject;
        }
    }
    
}