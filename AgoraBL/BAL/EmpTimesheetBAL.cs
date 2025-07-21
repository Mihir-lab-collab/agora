using AgoraBL.DAL;
using AgoraBL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.BAL
{
    public class EmpTimesheetBAL
    {
       static string AIChannels = ConfigurationManager.AppSettings.Get("AIChannels").ToString();
        public int TSID { get; set; }
        public string EmpName { get; set; }
        public string WBSName { get; set; }
        public DateTime EndDate { get; set; }
        public static Boolean Update(int ModuleID, int EmpID, DateTime TSDate, int TSHour, string TSComment, int TSID)
        {
            EmpTimesheetDAL objEmpTimesheetDAL = new EmpTimesheetDAL();
            return objEmpTimesheetDAL.Update(ModuleID, EmpID, TSDate, TSHour, TSComment, TSID);
        }
        public static Boolean Delete(int TSID)
        {
            EmpTimesheetDAL objEmpTimesheetDAL = new EmpTimesheetDAL();
            return objEmpTimesheetDAL.Delete(TSID);
        }
        public static IList<TimesheetDTO> GetEmailDetails(int ProjID)
        {
            EmpTimesheetDAL objEmpTimesheetDAL = new EmpTimesheetDAL();
            return objEmpTimesheetDAL.GetEmailDetailsByProjID(ProjID);
        }
        public static IList<clsTimeSheetEmail> getWBSID(int empId)
        {
            EmpTimesheetDAL objTS = new EmpTimesheetDAL();
            return objTS.getWBSID(empId);
        }
        public void SendMail(bool IsWBS, DateTime StartDate, string ModuleName, string AttTSHour,
       string TSComment, int ProjID, string ProjName, int EmpID, string Name, int intID)
        {
            EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
            int projectID = ProjID;
            IList<TimesheetDTO> lstTimesheet = GetEmailDetails(projectID);

            string PM_Name, PM_Email, AccEmail,BaEmail = "";
            if (lstTimesheet.Count > 0)
            {
                PM_Name = lstTimesheet[0].ManagerName.ToString();
                PM_Email = lstTimesheet[0].ProjectManagerEmail.ToString();
                AccEmail = lstTimesheet[0].AccountManagerEmail.ToString();
                BaEmail = lstTimesheet[0].BaEmail.ToString();
            }
            else
            {
                PM_Name = "";
                PM_Email = "";
                AccEmail = "";
                BaEmail = "";
            }

            string projectName = ProjName;
            string strBody, strSubject, mailTo, mailFrom, message, CC = "";
            //if (projectName == "Intelegain Internal")
            //{
            //    mailTo = AccEmail;
            //}
            //else
            //{
            //mailTo = PM_Email;
            //}
            var mailToList = new List<string>();

            if (!string.IsNullOrWhiteSpace(PM_Email))
                mailToList.Add(PM_Email);

            if (!string.IsNullOrWhiteSpace(AccEmail) && AccEmail != PM_Email)
                mailToList.Add(AccEmail);

            if (!string.IsNullOrWhiteSpace(BaEmail) && BaEmail != PM_Email && BaEmail != AccEmail)
                mailToList.Add(BaEmail);

            mailTo = string.Join(",", mailToList);

            bool istrue;

            if (IsWBS == true)
            {
                string ddlModule = ModuleName;
                int WBSID = intID;
                string module = ddlModule;
                string Comment = TSComment;
                if (WBSID == 0)
                {
                    IList<clsTimeSheetEmail> clsTimeSheetEmails = getWBSID(EmpID);
                    if (clsTimeSheetEmails.Count > 0)
                    {
                        WBSID = clsTimeSheetEmails[0].intWBSID;
                    }

                    strSubject = "TimeSheet created of [" + StartDate.ToString("dd/MM/yyyy") + "] for " + projectName + " by " + Name;
                }
                else
                {
                    strSubject = "TimeSheet updated of [" + StartDate.ToString("dd/MM/yyyy") + "] for " + projectName + " by " + Name;
                }

                strBody = @"<table><tr><td><b><u>Time Sheet Details<u><b></td></tr><tr><td><b>Name of Employee: </b>" + EmpName + "</td></tr><tr><td><b>Project Name: </b>" + projectName + "</td></tr><tr><td><b>WBS ID </b>" + WBSID + "</td></tr><tr><td><b>WBS Name: </b>" + WBSName + " </td></tr><tr><td><b>Start Date: </b>" + StartDate + " </td></tr><tr><td><b>End Date: </b>" + EndDate + " </td></tr><tr><td><b>Module: </b>" + ddlModule + " </td></tr><tr><td><b>Comment: </b>" + Comment + " </td></tr></table>";
                string dt = DateTime.Now.ToString("dd MMM yyyy");

                mailFrom = CC = "";
                message = objEmployee.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "", out istrue);
            }
            else
            {
                DateTime Entrydate = StartDate;
                TSID = intID;
                if (TSID == 0)
                {
                    strSubject = "TimeSheet created of [" + Entrydate.ToString("dd/MM/yyyy") + "] for " + projectName + " by " + Name;
                }
                else
                {
                    strSubject = "TimeSheet updated of [" + Entrydate.ToString("dd/MM/yyyy") + "] for " + projectName + " by " + Name;
                }

                mailFrom = CC = "";
                strBody = @"<table><tr><td><b><u>Time Sheet Details<u><b></td></tr><tr><td><b>Name of Employee: </b>" + Name + "</td></tr><tr><td><b>Project Name: </b>" + projectName + "</td></tr><tr><td><b>TimeSheet Date: </b>" + Entrydate.ToString("dd/MMM/yyyy") + " </td></tr><tr><td><b>Module: </b>" + ModuleName + " </td></tr><tr><td><b>Hours: </b>" + AttTSHour + "</td></tr><tr><td><b>Description: </b>" + TSComment + " </td></tr></table>";

                message = objEmployee.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "", out istrue);
            }
        }
        public static List<ProjectModulelistDTO> GetProjectAndModulelist(UserIdentityData userIdentityData)
        {
            List<ProjectModulelistDTO> Projectlst = new List<ProjectModulelistDTO>();
            EmpTimesheetDAL objEmpTimesheetDAL = new EmpTimesheetDAL();
            return objEmpTimesheetDAL.GetProjectAndModulelist(userIdentityData);
        }
        public static void EmployeeTimesheetRequest(SendTimesheetNotificationDTO.EmployeeTimesheetRequest emp)
        {
            try
            {
                EmployeeMasterBAL objEmployeeMasterBAL = new EmployeeMasterBAL();
                EmployeeMasterDAL objEmployeeMasterDAL = new EmployeeMasterDAL();
                var ds = objEmployeeMasterDAL.GetTimesheetNotificationDetails(Convert.ToInt32(emp.EmpID));
                if (ds != null && ds.Tables.Count > 0)
                {
                    emp.EmpTimesheetStatus = "Pending";
                    Task.Run(() => objEmployeeMasterBAL.SendTimesheetNotificationToHrThroughAI("SendRequestFirstTime", Convert.ToInt32(emp.EmpID), emp.FromDate, emp.ToDate, emp.EmpName, ds,
                     emp.Requested,emp.EmpTimesheetStatus));
                    //objEmployeeMasterBAL.SendTimesheetNotificationToHrThroughAI("SendRequestFirstTime", Convert.ToInt32(emp.EmpID), emp.FromDate, emp.ToDate, emp.EmpName, ds, 
                    //    emp.Requested,emp.EmpTimesheetStatus);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        public static void EmployeeTimesheetRequestUpdate(SendTimesheetNotificationDTO.EmployeeTimesheetRequest emp)
        {
            try
            {
                string hrUpdatedBy = string.Empty;
                EmployeeMasterBAL objEmployeeMasterBAL = new EmployeeMasterBAL();
                EmployeeMasterDAL objEmployeeMasterDAL = new EmployeeMasterDAL();
                var ds = objEmployeeMasterDAL.GetTsDatabyUniqueId(emp.UniqueId, Convert.ToInt32(emp.EmpID), Convert.ToInt32(emp.UpdatedBy));
                if (ds != null && ds.Tables[0].Rows.Count>0 && ds.Tables.Count > 0)
                {
                    emp.EmpID = ds.Tables[1].Rows[0]["empid"].ToString();
                    emp.EmpName = ds.Tables[1].Rows[0]["empName"].ToString();
                    emp.FromDate = ds.Tables[1].Rows[0]["fromDate"].ToString();
                    emp.ToDate = ds.Tables[1].Rows[0]["toDate"].ToString();
                    emp.Requested = ds.Tables[1].Rows[0]["requested"].ToString();
                    emp.UpdateDate = DateTime.Now.ToString();
                    if (ds.Tables[2].Rows.Count > 0 && ds.Tables.Count > 2)
                    {
                        hrUpdatedBy = ds.Tables[3].Rows[0]["empName"].ToString();
                    }
                    if (!(string.Compare(emp.EmpTimesheetStatus,"Rejected",true)==0))
                    {
                        int count=SaveEmployeeTimesheetRequest(Convert.ToInt32(emp.EmpID), DateTime.Now.Date, emp.Requested, Convert.ToInt32(emp.UpdatedBy));
                    }
                    Task.Run(() => objEmployeeMasterBAL.SendTimesheetNotificationToHrThroughAI("SendRequestSecondTime", Convert.ToInt32(emp.EmpID), emp.FromDate, emp.ToDate, emp.EmpName,
                    ds, emp.Requested,emp.EmpTimesheetStatus,emp.UpdateReason,emp.UpdateDate,hrUpdatedBy, emp.UniqueId));

                    //objEmployeeMasterBAL.SendTimesheetNotificationToHrThroughAI("SendRequestSecondTime", Convert.ToInt32(emp.EmpID), emp.FromDate, emp.ToDate, emp.EmpName,
                    //    ds, emp.Requested,emp.EmpTimesheetStatus,emp.UpdateReason,emp.UpdateDate,emp.UpdatedBy,emp.UniqueId);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        public static int SaveEmployeeTimesheetRequest(int EmpId, DateTime RequestDate, string description, int InsertedBy)
        {
            try
            {
                EmpTimesheetDAL objEmpTimesheetDAL = new EmpTimesheetDAL();
                return objEmpTimesheetDAL.SaveEmployeeTimesheetRequest(EmpId,RequestDate, description, InsertedBy);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool CheckForApproval(int EmpId, DateTime requestdate)
        {
            EmpTimesheetDAL objEmpTimesheetDAL = new EmpTimesheetDAL();
            return objEmpTimesheetDAL.CheckforApproval(EmpId, requestdate);

        }
        public static bool CheckIsProjectMember(int tsId, int empId)
        {
            EmpTimesheetDAL objEmpTimesheetDAL = new EmpTimesheetDAL();
            return objEmpTimesheetDAL.CheckIsProjectMember(tsId, empId);
        }
        public static GetIncompleteTimesheetDTO GetIncompleteTimesheet(string entityName, string fromDate, string toDate)
        {
            try
            {
                List<IncompleteTimesheetGroupDTO> groupedList = new List<IncompleteTimesheetGroupDTO>();
                EmpTimesheetDAL objEmpTimesheetDAL = new EmpTimesheetDAL();
                GetIncompleteTimesheetDTO objGetIncompleteTimesheetDTO = new GetIncompleteTimesheetDTO();
                var objDT = objEmpTimesheetDAL.GetIncompleteTimesheet(entityName, fromDate, toDate);

                if (objDT != null && objDT.Rows.Count > 0)
                {
                    var hrDetails = GetAdminDetails();

                    groupedList = objDT.AsEnumerable()
                        .GroupBy(row => new
                        {
                            EmpId = row.Field<int>("EmpID"),
                            EmpName = row.Field<string>("EMPName"),
                            MSTeam = row.Field<string>("MSTeam")
                        })
                        .Select(group => new IncompleteTimesheetGroupDTO
                        {
                            EmpId = group.Key.EmpId,
                            EmpName = group.Key.EmpName,
                            MSTeam = group.Key.MSTeam,
                            IncompleteTimesheets = group.Select(row => new IncompleteTimesheetDTO
                            {
                                Date = DateTime.Parse(row.Field<string>("Date")).ToString("yyyy-MM-dd"),
                        IncompleteHours = row.Field<int>("IncompleteHours"),
                                TimeAvailable = row.Field<string>("TimeAvailable")
                            }).ToList(),
                })
                        .ToList();
                    objGetIncompleteTimesheetDTO.IncompleteTimesheetDetails = groupedList;
                    objGetIncompleteTimesheetDTO.HRList = hrDetails;
                }

                return objGetIncompleteTimesheetDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static List<AgoraBL.Models.SendLeaveNotification.HRIdList> GetAdminDetails()
        {
            try
            {
                EmpTimesheetDAL objEmpTimesheetDAL = new EmpTimesheetDAL();
                var ds = objEmpTimesheetDAL.GetAdminDetails();

                List<AgoraBL.Models.SendLeaveNotification.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendLeaveNotification.HRIdList>();

                if (ds != null && ds.Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Rows)
                    {
                        List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();

                        if (!string.IsNullOrEmpty(AIChannels))
                        {
                            List<string> lstChannels = AIChannels.Split(',').ToList();

                            foreach (var channel in lstChannels)
                            {
                                if (ds.Columns.Contains(channel))
                                {
                                    objLstChannelList.Add(new AgoraBL.Models.SendLeaveNotification.ChannelList
                                    {
                                        ChannelName = channel,
                                        ChannelUniqueId = Convert.ToString(row[channel]),
                                        ChannelNotificationUniqueId = null,
                                        EmpId = Convert.ToString(row["empid"])
                                    });
                                }
                            }
                        }

                        lstHRIdList.Add(new AgoraBL.Models.SendLeaveNotification.HRIdList
                        {
                            EmpId = Convert.ToString(row["empid"]),
                            HRName = Convert.ToString(row["empName"]),
                            ChannelList = objLstChannelList
                        });
                    }
                }

                return lstHRIdList;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
