using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
public class PeojectsReviewBLL
{
    public long MeetingId { get; set; }
    public int EmployeeCode { get; set; }
    public string EmployeeName { get; set; }
    public DateTime MeetingDate { get; set; }
    public int CalledById { get; set; }
    public string CalledByName { get; set; }
    public string Attendees { get; set; }
    public string AttendeesId { get; set; }
    public string MeetingType { get; set; }
    public string MeetingStatus { get; set; }
    public string AgendaTopic { get; set; }
    public int FacilitatorId { get; set; }
    public string FacilitatorName { get; set; }
    public string TimeAlloted { get; set; }
    public DateTime InsertedOn { get; set; }
    public int ProjId { get; set; }
    public string ProjName { get; set; }
    public int InsertedBy { get; set; }

    public PeojectsReviewBLL()
    {

    }
    public PeojectsReviewBLL(long _MeetingId, DateTime _MeetingDate, int _CalledById, string _CalledByName, string _Attendees, string _AttendeesId, string _MeetingType, string _AgendaTopic, int _FacilitatorId, string _FacilitatorName, string _TimeAlloted, DateTime _InsertedOn, int _InsertedBy, int _MeetingStatus)
    {
        this.MeetingId = _MeetingId;
        this.MeetingDate = _MeetingDate;
        this.AgendaTopic = _AgendaTopic;
        this.CalledById = _CalledById;
        this.CalledByName = _CalledByName;
        this.Attendees = _Attendees;
        this.AttendeesId = _AttendeesId;
        this.MeetingType = _MeetingType;
        this.FacilitatorId = _FacilitatorId;
        this.FacilitatorName = _FacilitatorName;
        this.TimeAlloted = _TimeAlloted;
        this.InsertedOn = _InsertedOn;
        this.InsertedBy = _InsertedBy;
        this.MeetingStatus = (_MeetingStatus == 1 ? "Rescheduled" : (_MeetingStatus == 2 ? "Cancelled" : (_MeetingStatus == 3 ? "Completed" : "Pending")));
    }
    public PeojectsReviewBLL(int _ProjReviewId, int _EmployeeCode, string _EmployeeName, string _Attendees, DateTime _MeetingDate, int _MeetingCalledBy, string _MeetingType, string _AgendaTopic, int _Facilitator, string _TimeAlloted, int _InsertedBy)
    {
        this.MeetingId = _ProjReviewId;
        this.EmployeeCode = _EmployeeCode;
        this.EmployeeName = _EmployeeName;
        this.MeetingDate = _MeetingDate;
        this.CalledById = _MeetingCalledBy;
        this.Attendees = _Attendees;
        this.MeetingType = _MeetingType;
        this.AgendaTopic = _AgendaTopic;
        this.FacilitatorId = _Facilitator;
        this.TimeAlloted = _TimeAlloted;
        this.InsertedBy = _InsertedBy;
    }
    public PeojectsReviewBLL(DateTime _MeetingDate, int _MeetingCalledBy, string _Attendees, string _MeetingType, string _AgendaTopic, int _Facilitator, string _TimeAlloted, int _InsertedBy)
    {
        this.MeetingDate = _MeetingDate;
        this.CalledById = _MeetingCalledBy;
        this.Attendees = _Attendees;
        this.MeetingType = _MeetingType;
        this.AgendaTopic = _AgendaTopic;
        this.FacilitatorId = _Facilitator;
        this.TimeAlloted = _TimeAlloted;
        this.InsertedBy = _InsertedBy;
    }
    public static void SaveMeetingDetails(string Mode,int MeetingId, DateTime MeetingDate, int MeetingCalledBy, string MeetingType, string Attendees, string AgendaTopic, int Facilitator, string TimeAlloted, int InsertedBy)
    {
        PeojectsReviewBLL projectReview = new PeojectsReviewBLL();
        projectReview.MeetingId = MeetingId;
        projectReview.MeetingDate = MeetingDate;
        projectReview.MeetingType = MeetingType;
        projectReview.Attendees = Attendees;
        projectReview.AgendaTopic = AgendaTopic;
        projectReview.CalledById = MeetingCalledBy;
        projectReview.FacilitatorId = Facilitator;
        projectReview.TimeAlloted = TimeAlloted;
        projectReview.InsertedBy = InsertedBy;
        ProjectsReviewDAL objprojectMaster = new ProjectsReviewDAL();
        objprojectMaster.SaveProjectMeeting(Mode, projectReview);
    }
    public static void Cancel_Meeting(string Mode, int MeetingId)
    {
        ProjectsReviewDAL objprojectMaster = new ProjectsReviewDAL();
        objprojectMaster.Cancel_Meeting(Mode, MeetingId);
    }
    public static List<PeojectsReviewBLL> Get_ProjectReviewMeetingList(string Mode, int MeetingId)
    {
        ProjectsReviewDAL obj = new ProjectsReviewDAL();
        List<PeojectsReviewBLL> objList = obj.Get_ProjectReviewMeetingList(Mode, MeetingId);
        return objList;
    }
}