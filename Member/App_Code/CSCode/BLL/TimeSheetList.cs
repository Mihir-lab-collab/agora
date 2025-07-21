using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Customer.BLL
{
    public class TimeSheetList
    {

        public int project_Id { get; set; }
        public string project_Name { get; set; }


        public TimeSheetList(int _projectId, string _projectName)
        {
            this.project_Id = _projectId;
            this.project_Name = _projectName;
        }

        public TimeSheetList()
        {
            // TODO: Complete member initialization
        }
        public static IList<TimeSheetList> GetAllProject()
        {
            TimeSheetListDAL objGetProject = new TimeSheetListDAL();
            return objGetProject.GetAllProject("Tm_Project", 0, 0, 0, 0, 0);

        }


    }

    public class TimesheetModule
    {
       
        public int M_ID { get; set; }
        public int moduleId {get; set; }
        public string ModuleName { get; set; }

        public TimesheetModule(int _moduleId, string _modulename)
        {
            this.moduleId = _moduleId;
            this.ModuleName = _modulename;
           
        }
        //public TimesheetModule(int _moduleId, string _modulename)
        //{
        //    this.moduleId = _moduleId;
        //    this.ModuleName = _modulename;
        //}

        public static IList<TimesheetModule> GetAllTimesheetModule(int projId,string mode)
        {

            TimeSheetListDAL objGetModule = new TimeSheetListDAL();
            return objGetModule.GetAllModule(mode, projId, 0, 0, 0, 0);
       }
        public static IList<TimesheetModule> EmpModule(int projId, string mode)
        {

            TimeSheetListDAL objGetModule = new TimeSheetListDAL();
            return objGetModule.GetAllModule(mode, projId, 0, 0, 0, 0);
           
        }

        //public static IList<TimesheetModule> ProjectModule(int projId)

       
    }
    public class TimesheetProjMember
    {

        public int empid { get; set; }
        public string empName { get; set; }

        public TimesheetProjMember(int _empid, string _empName)
        {
            this.empid = _empid;
            this.empName = _empName;
        }

        public static IList<TimesheetProjMember> GetAllTimesheetProjMember(int projId, int moduleId, int prjMonth, int prjYear)
        {
            TimeSheetListDAL objGetModule = new TimeSheetListDAL();
            return objGetModule.GetAllProjMember("prjMemberByProjId",projId,0,prjMonth,prjYear,moduleId, 0);
          //and projectModuleMaster.projid=@Tm_ProjectId and  (Month(projectTimeSheet.tsDate)=@prjmonth ) AND (YEAR(projectTimeSheet.tsDate)=@Projyear )  and  projectTimeSheet.moduleId=@ModuleId  ORDER BY employeeMaster.empName
        }
        public static IList<TimesheetProjMember> EmpProjMember()
        {
            TimeSheetListDAL objGetModule = new TimeSheetListDAL();
            return objGetModule.GetAllProjMember("EmpProjectMember", 0, 0, 0, 0, 0, 0);
        }
        public static IList<TimesheetProjMember> AdminProjMemberList(String mode, int prjId)
        {
            TimeSheetListDAL objGetModule = new TimeSheetListDAL();
            return objGetModule.GetAllProjMember(mode, prjId, 0, 0, 0, 0, 0);
        }
        public static IList<TimesheetProjMember> PrjmemberByLocId(int LocationId)
        {
            TimeSheetListDAL objGetModule = new TimeSheetListDAL();
            return objGetModule.GetAllProjMember("PrjmemberByLocId", 0, 0, 0, 0, 0, LocationId);
        }
        
        public static IList<TimesheetProjMember> GetMember(int LocationId, int ProjectID)
        {
            TimeSheetListDAL objGetModule = new TimeSheetListDAL();
            return objGetModule.GetMember(LocationId, ProjectID);
        }
    }


    public class TimesheetEmpProj
    {

        public int projId { get; set; }
        public string projName { get; set; }

        public TimesheetEmpProj(int _projId, string _projName)
        {
            this.projId = _projId;
            this.projName = _projName;
        }

        public static IList<TimesheetEmpProj> GetEmpProject(int memberId, int prjMonth, int prjYear)
        {
            TimeSheetListDAL objGetModule = new TimeSheetListDAL();
            return objGetModule.GetEmpProject("FindEmpProject",memberId,prjMonth,prjYear);

        }
    }//'FindEmpProject',null,1000,08,2013,null

    public class projectTimeSheet
    {

        public int tsId { get; set; }
        public int moduleId { get; set; }
        public int empid { get; set; }
        public DateTime tsDate { get; set; }
      // public int tsHour { get; set; }
        public double tsHour { get; set; }
        public string tsComment { get; set; }

        public projectTimeSheet()
        { }

        public projectTimeSheet(int _tsId, int _moduleId, int _empid, DateTime _tsDate, double _tsHour, string _tsComment)
        {
            this.tsId = _tsId;
            this.moduleId = _moduleId;
            this.empid = _empid;
            this.tsDate = _tsDate;
            this.tsHour = _tsHour;
            this.tsComment = _tsComment;
        }
        //public static int InsertTimesheetList(int ModuleId, int EmpId, DateTime TsDate, int TsHour, string TsComment, int tskId, string mode)
        //{
        //    projectTimeSheet curTimesheet = new projectTimeSheet();
        //    curTimesheet.moduleId = ModuleId;
        //    curTimesheet.empid = EmpId;
        //    curTimesheet.tsDate = TsDate;
        //    curTimesheet.tsHour = TsHour;
        //    curTimesheet.tsComment = TsComment;
        //    curTimesheet.tsId = tskId;
        //    TimeSheetListDAL objTime = new TimeSheetListDAL();
        //    return objTime.InsertTimesheet(curTimesheet, mode, 0);
        //}

        public static int InsertTimesheetList(int ModuleId, int EmpId, DateTime TsDate, double TsHour, string TsComment, int tskId, string mode)
        {
            projectTimeSheet curTimesheet = new projectTimeSheet();
            curTimesheet.moduleId = ModuleId;
            curTimesheet.empid = EmpId;
            curTimesheet.tsDate = TsDate;
            curTimesheet.tsHour = TsHour;
            curTimesheet.tsComment = TsComment;
            curTimesheet.tsId = tskId;
            TimeSheetListDAL objTime = new TimeSheetListDAL();
            return objTime.InsertTimesheet(curTimesheet, mode, 0);
        }

        public static void DeleteTimesheet(int taskId)
        {
            projectTimeSheet curTimesheet=new projectTimeSheet();
            curTimesheet.tsId=taskId;
            TimeSheetListDAL objDelete = new TimeSheetListDAL();
            objDelete.DeleteTimesheet(curTimesheet, "DELETE", taskId);
        }

        public static void UpdateTaskApprove(int taskId)
        {
            projectTimeSheet curTimesheet = new projectTimeSheet();
            curTimesheet.tsId = taskId;
            TimeSheetListDAL objupdateTask = new TimeSheetListDAL();
            objupdateTask.UpdateStatus(curTimesheet, "TaskApprove", taskId);
        }

        public static void UpdateTaskUnApprove(int taskId)
        {
            projectTimeSheet curTimesheet = new projectTimeSheet();
            curTimesheet.tsId=taskId;
              TimeSheetListDAL objupdateTask=new TimeSheetListDAL();
              objupdateTask.UpdateStatus(curTimesheet, "TaskUnApprove", taskId);
        }
        public static int UpdateTimesheetList(int ModuleId, int EmpId, DateTime TsDate, int TsHour, string TsComment,int tsid, string mode)
        {
            projectTimeSheet curTimesheet = new projectTimeSheet();
            curTimesheet.moduleId = ModuleId;
            curTimesheet.empid = EmpId;
            curTimesheet.tsDate = TsDate;
            curTimesheet.tsHour = TsHour;
            curTimesheet.tsComment = TsComment;
            curTimesheet.tsId = tsid;
            TimeSheetListDAL objTime = new TimeSheetListDAL();
            return objTime.InsertTimesheet(curTimesheet, mode, tsid);
        }
        public static int EditTimesheetList(int ModuleId, DateTime TsDate, int TsHour, string TsComment, int tsid, string mode)
        {
            projectTimeSheet curTimesheet = new projectTimeSheet();
            curTimesheet.moduleId = ModuleId;
            //curTimesheet.empid = EmpId;
            curTimesheet.tsDate = TsDate;
            curTimesheet.tsHour = TsHour;
            curTimesheet.tsComment = TsComment;
            curTimesheet.tsId = tsid;
            TimeSheetListDAL objTime = new TimeSheetListDAL();
            return objTime.InsertTimesheet(curTimesheet, mode, tsid);
        }
      
        public static projectTimeSheet GetallTimesheet(int EmpId, string mode)
        {
            TimeSheetListDAL objCustProjects = new TimeSheetListDAL();
            return objCustProjects.getAllTimesheetList(EmpId, mode);
        }

    }
}