using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectWBSBLL
/// </summary>
public class ProjectWBSBLL
{


    //Project Milestone
    public string mode { get; set; }
    public int projID { get; set; }
    public int projMilestoneID { get; set; }
    public string name { get; set; }
    public string dueDate { get; set; }
    public string DeliveryDate { get; set; }
    public int EstHours { get; set; }
    public string MilestoneHours { get; set; }
    //Project WBS
    public string Milestone { get; set; }
    public int WBSID { get; set; }
    public string Hours { get; set; }
    public string WBS { get; set; }
    public DateTime StartDate { get; set; }
    public string ActualHrs { get; set; }
    public DateTime EndDate { get; set; }
    public string AssignedTo { get; set; }
    public string Status { get; set; }
    public string Remark { get; set; }
    public string Insertedby { get; set; }
    public DateTime Modifiedon { get; set; }
    public string tempempName { get; set; }
    public string ActualStartDate { get; set; }
    public string ActualEndDate { get; set; }
    public string tempSDate { get; set; }
    public string tempEDate { get; set; }
    public string Text { get; set; }
    public string Value { get; set; }

    //ProjectWBSDetails
    public string Name { get; set; }
    public DateTime SDate { get; set; }
    public DateTime EDate { get; set; }
    public int DHours { get; set; }
    public string strHours { get; set; }
    public string strMinutes { get; set; }
    public int WBSId { get; set; }
    public string Comment { get; set; }
    public int InsertedBy { get; set; }
    public string empId { get; set; }
    public int ProjectWBSID { get; set; }
    public string Description { get; set; }
    public int ModuleID { get; set; }
    public string ModuleName { get; set; }


    public ProjectWBSBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public ProjectWBSBLL(int projID, int projMilestoneID, string name, string dueDate, int EstHours)
    {
        // TODO: Complete member initialization
        this.projID = projID;
        this.projMilestoneID = projMilestoneID;
        this.name = name;
        this.dueDate = dueDate;
        this.EstHours = EstHours;
    }

    public static List<ProjectWBSBLL> getMileStone(string mode, int projid)
    {
        ProjectWBSDAL objmil = new ProjectWBSDAL();
        return objmil.getMileStone(mode, projid);

    }

    public static string checkWBSExists(string mode,string StartDate,string EndDate,string EmpID,int ProjID)
    {
        ProjectWBSDAL objmil = new ProjectWBSDAL();
        return objmil.checkWBSExists(mode,StartDate,EndDate,EmpID,ProjID);

    }
    public string GetProfileAccess(string mode, string empID)
    {
        ProjectWBSDAL objmil = new ProjectWBSDAL();
        return objmil.GetProfileAccess(mode, empID);
    }


    public ProjectWBSBLL(int projMilestoneID, string Milestone, int WBSID, int Hours, string WBS, DateTime StartDate, int ActualHrs, DateTime EndDate, string AssignedTo, string Status, string Remark)
    {
        this.projMilestoneID = projMilestoneID;
        this.Milestone = Milestone;
        this.WBSID = WBSID;
        this.Hours = Convert.ToString(Hours);
        this.WBS = WBS;
        this.StartDate = StartDate;
        this.ActualHrs = Convert.ToString(ActualHrs);
        this.EndDate = EndDate;
        this.AssignedTo = AssignedTo;
        this.Status = Status;
        this.Remark = Remark;
    }

    //ap

    public ProjectWBSBLL(int ProjMileId, string WBS, string StartDate, string EndDate, string Status, string Remark)
    {
        this.projMilestoneID = ProjMileId;
        this.WBS = WBS;
        this.tempSDate = StartDate;
        this.tempEDate = EndDate;
        this.Status = Status;
        this.Remark = Remark;
    }


    public static List<ProjectWBSBLL> getProjectWBS(string mode, int projid, int showCompletedStatus)
    {
        ProjectWBSDAL objwbs = new ProjectWBSDAL();
        return objwbs.getProjectWBS(mode, projid, showCompletedStatus);
    }
    //ap

    public string InsertWBS(string mode, int _Milestoneid, string wbs, string startdate, string end_date, int hours, string status, string remark, string _insertedby, int _projId, int WBSID = 0)
    {
        ProjectWBSBLL objwbs = new ProjectWBSBLL();
        objwbs.mode = mode;
        objwbs.projMilestoneID = _Milestoneid;

        objwbs.WBS = wbs;
        //objwbs.WBSID = wbsId;
        objwbs.tempSDate = startdate;
        objwbs.tempEDate = end_date;
        objwbs.Hours = Convert.ToString(hours);
        //objwbs.AssignedTo = AssignTo;
        //objwbs.ActualHrs = actual_hrs;
        objwbs.Status = status;
        objwbs.Remark = remark;
        objwbs.Insertedby = _insertedby;
        objwbs.projID = _projId;
        objwbs.WBSID = WBSID;

        ProjectWBSDAL objwbsdal = new ProjectWBSDAL();
        return objwbsdal.InsertProjectWbs(objwbs);
    }

    public static string UpdateWBS(string mode, int _Milestoneid, string wbs, string startdate, string end_date, int hours, string status, string remark, string _insertedby, int _projId, int WBSID)
    {
        ProjectWBSBLL objwbs = new ProjectWBSBLL();
        objwbs.mode = mode;
        objwbs.projMilestoneID = _Milestoneid;
        // objwbs.projMilestoneID = MilestoneId;
        objwbs.WBS = wbs;
        //objwbs.WBSID = wbsId;
        objwbs.tempSDate = startdate;
        objwbs.tempEDate = end_date;
        objwbs.Hours = Convert.ToString(hours);
        //objwbs.AssignedTo = AssignTo;
        //objwbs.ActualHrs = actual_hrs;
        objwbs.Status = status;
        objwbs.Remark = remark;
        objwbs.Insertedby = _insertedby;
        objwbs.projID = _projId;
        objwbs.WBSID = WBSID;

        ProjectWBSDAL objwbsdal = new ProjectWBSDAL();
        return objwbsdal.InsertProjectWbs(objwbs);
    }



    public static List<ProjectWBSBLL> GetMilestone(string mode, int _projid)
    {
        ProjectWBSDAL objwbs = new ProjectWBSDAL();
        return objwbs.getMilestoneWBS(mode, _projid);
    }

    public static int SelectprojMilestoneID(string mode, string _MilestoneName)
    {
        ProjectWBSDAL objwbs = new ProjectWBSDAL();
        return objwbs.getMilestoneId(mode, _MilestoneName);
    }

    public static int DeleteProjWBS(int ProjectWBSID)
    {
        ProjectWBSDAL objwbs = new ProjectWBSDAL();
        return objwbs.DeleteProjWBS(ProjectWBSID);
    }

   
    //=========================================

    public static List<ProjectWBSBLL> GetProjectMembersByProjId(int wbsId)
    {
        ProjectWBSDAL objprojectMember = new ProjectWBSDAL();
        return objprojectMember.GetProjectMembersByProjId(wbsId);
    }

    public ProjectWBSBLL(int _projId, int _empid, string empname)
    {
        this.projID = _projId;
        this.empId = Convert.ToString(_empid);
        this.tempempName = empname;
    }

    public ProjectWBSBLL(int _projMilestoneID, string _Milestone, int _WBSID, int _Hours, string _WBS, DateTime _projStartDate, int _ActualHrs, DateTime _projEndDate, string _Status, string _Remark, int _empId)
    {
        this.projMilestoneID = _projMilestoneID;
        this.Milestone = _Milestone;
        this.WBSID = _WBSID;
        this.Hours = Convert.ToString(_Hours);
        this.WBS = _WBS;
        this.StartDate = _projStartDate;
        this.ActualHrs = Convert.ToString(_ActualHrs);
        this.EndDate = _projEndDate;
        this.Status = _Status;
        this.Remark = _Remark;
        this.empId = Convert.ToString(_empId);
    }
    //========================================


    //AP

    public ProjectWBSBLL(string Name, DateTime SDate, DateTime EDate, int DHours, int WBSId, string Comment, int InsertedBy, string empId, int ModuleID)
    {
        this.Name = Name;
        this.SDate = SDate;
        this.EDate = EDate;
        this.DHours = DHours;
        this.WBSId = WBSId;
        this.Comment = Comment;
        this.InsertedBy = InsertedBy;
        this.empId = empId;
        this.ModuleID = ModuleID;

    }


    public static int InsertProjWBSDetails(int ProjectWBSID, string Name, string SDate, string EDate, int WBSId, string Comment, int DHours, int ModuleID, int InsertedBy, int ProjID)
    {
        ProjectWBSBLL objProjWBSDetails = new ProjectWBSBLL();
        objProjWBSDetails.ProjectWBSID = ProjectWBSID;
        objProjWBSDetails.Name = Name;
        objProjWBSDetails.tempSDate = SDate;
        objProjWBSDetails.tempEDate = EDate;
        objProjWBSDetails.DHours = DHours;
        objProjWBSDetails.WBSId = WBSId;
        objProjWBSDetails.Comment = Comment;
        objProjWBSDetails.ModuleID = ModuleID;
        objProjWBSDetails.InsertedBy = InsertedBy;
        objProjWBSDetails.projID = ProjID;

        if (ProjectWBSID > 0)
            objProjWBSDetails.mode = "UpdateProjWBSDetails";
        else
            objProjWBSDetails.mode = "InsertProjWBSDetails";
        ProjectWBSDAL objProjWBSDetailsDAL = new ProjectWBSDAL();
        return objProjWBSDetailsDAL.InsertProjWBSDetails(objProjWBSDetails);
    }


    public static List<ProjectWBSBLL> GetProjectWBSDetails(string mode, int employeeID, int projid, string StartDate, string EndDate)
    {
        ProjectWBSDAL objWBS = new ProjectWBSDAL();
        return objWBS.GetProjectWBSDetails(mode, employeeID, projid, StartDate, EndDate);

    }

    public static List<ProjectWBSBLL> BindWBS(string mode, int projid, int empid)
    {
        ProjectWBSDAL objWBS = new ProjectWBSDAL();
        return objWBS.BindWBS(mode, projid, empid);
    }

    public static List<ProjectWBSBLL> BindAllWBS(string mode, int projid, int empid)
    {
        ProjectWBSDAL objAllWBS = new ProjectWBSDAL();
        return objAllWBS.BindAllWBS(mode, projid, empid);
    }
    public ProjectWBSBLL(int ProjectWBSID, string Description)
    {
        // TODO: Complete member initialization
        this.ProjectWBSID = ProjectWBSID;
        this.Description = Description;
    }

    public ProjectWBSBLL(int actualhrs)
    {
        this.ActualHrs = Convert.ToString(actualhrs);
    }

}