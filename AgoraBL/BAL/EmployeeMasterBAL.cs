using AgoraBL.Common;
using AgoraBL.DAL;
using AgoraBL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Channels;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml.Linq;

namespace AgoraBL.BAL
{
    public class EmployeeMasterBAL
    {
        private readonly EmployeeMasterBAL _data;
        string AIPushNotificationAPIBaseURL = ConfigurationManager.AppSettings.Get("AIPushNotificationAPIBaseURL").ToString();
        string AIPushNotificationAPIAuthHeader = ConfigurationManager.AppSettings.Get("AIPushNotificationAPIAuthHeader").ToString();
        string AIChannels = ConfigurationManager.AppSettings.Get("AIChannels").ToString();
        private EmployeeMasterDAL objEmployee;
        public EmployeeMasterBAL()
        {
            objEmployee = new EmployeeMasterDAL();
        }
        public UserIdentityData GetEmpDetailsbyId(UserIdentityData userIdentityData)
        {
            try
            {
                UserIdentityData objUserData = new UserIdentityData();
                var Id = userIdentityData.UserIdentity.Split('|');
                if (string.Compare("SkypeId", Id[0], true) == 0)
                {
                    objUserData = objEmployee.GetEmpDetailsbyId(Id[1], UserIdentity.SkypeId.ToString());
                }
                else if (string.Compare("MSTeam", Id[0], true) == 0)
                {
                    objUserData = objEmployee.GetEmpDetailsbyId(Id[1], UserIdentity.MSTeam.ToString());
                }
                if (objUserData.IsSuccess && objUserData.StatusMessage == "Success")
                {
                    objUserData.EmpToken = ClsCommon.GenerateJwtToken(objUserData);
                }
                return objUserData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static ClsLogin EmployeeLogin(string EmpID, string Password, LoginType Logintype)
        {
            EmployeeMasterDAL objUser = new EmployeeMasterDAL();
            ClsLogin loginResult = new ClsLogin();

            if (Logintype == LoginType.Agora)
            {
                return objUser.EmployeeLogin(EmpID, Password);
            }
            else if (Logintype == LoginType.AD) // AD login type
            {
                return objUser.EmployeeADLogin(EmpID);
                //string strError = string.Empty;
                //string domainName = ConfigurationManager.AppSettings["DirectoryDomain"];
                //string adPath = ConfigurationManager.AppSettings["DirectoryPath"];

                //if (string.IsNullOrEmpty(domainName) || string.IsNullOrEmpty(adPath))
                //{
                //    throw new ConfigurationErrorsException("Active Directory domain or path is not configured properly.");
                //}

                //if (AuthenticateUser(domainName, EmpID, Password, adPath, out strError))
                //{
                //return objUser.EmployeeADLogin(EmpID);
                //}
                //else
                //{
                //    loginResult.Message = !string.IsNullOrEmpty(strError) && strError.Replace("\r\n", "") == "The server is not operational."
                //        ? "AD server is not available."
                //        : "Invalid username or password!";

                //    loginResult.IsActive = false;
                //    return loginResult;
                //}
            }
            else
            {
                throw new ArgumentException("Unsupported login type provided.");
            }
        }
        public EmployeeDetails GetEmployeeDetails(int empId)
        {
            EmployeeDetails empDeatils = new EmployeeDetails();
            try
            {
                empDeatils = objEmployee.GetEmployeeDetails(empId);
            }
            catch (Exception)
            {

                throw;
            }
            return empDeatils;
        }

        #region HR-Related Function
        public EmployeeDetailsHR GetEmployeeDetailsByHR(string EmpName)
        {
            EmployeeDetailsHR empDeatils = new EmployeeDetailsHR();
            try
            {
                empDeatils = objEmployee.GetEmployeeDetailsByHR(EmpName);
            }
            catch (Exception)
            {

                throw;
            }
            return empDeatils;
        }
        public List<Timesheet> GetTimesheetDetailsByHR(Entity entity)
        {
            List<Timesheet> lstTimesheet = new List<Timesheet>();
            try
            {
                lstTimesheet = objEmployee.GetTimesheetDetailsByHR(entity);
            }
            catch (Exception)
            {

                throw;
            }
            return lstTimesheet;
        }
        public ConsolidateLeaves GetLeaveDetailsByHR(Entity entity)
        {
            ConsolidateLeaves Consolidate_Leaves = new ConsolidateLeaves();
            try
            {
                string mode = !string.IsNullOrEmpty(entity.EntityName) ? "GetDataFromEntityName" : "GetDataFromToDate";
                Consolidate_Leaves = objEmployee.GetLeaveDetailsByHR(entity, mode);
            }
            catch (Exception)
            {

                throw;
            }
            return Consolidate_Leaves;
        }
        public List<TimesheetTotalHours> GetTimesheetDetailsTotalHoursByRole(Entity entity)
        {
            List<TimesheetTotalHours> lstTimesheetTotalHours = new List<TimesheetTotalHours>();
            try
            {
                lstTimesheetTotalHours = objEmployee.GetTimesheetDetailsTotalHoursByRole(entity);
            }
            catch (Exception)
            {

                throw;
            }
            return lstTimesheetTotalHours;
        }
        public List<PendingTimeSheet> GetPendingTimesheetByHR(Entity entity)
        {
            List<PendingTimeSheet> pendingTimesheet = new List<PendingTimeSheet>();
            try
            {
                pendingTimesheet = objEmployee.GetPendingTimesheetByHR(entity);
            }
            catch (Exception ex)
            {

                throw;
            }
            return pendingTimesheet;
        }
        #endregion

        #region Employee Related Function
        public List<Timesheet> GetTimesheetDetails(UserIdentityData userIdentityData)
        {
            List<Timesheet> lstTimesheet = new List<Timesheet>();
            try
            {
                lstTimesheet = objEmployee.GetTimesheetDetails(userIdentityData);
            }

            catch (Exception)
            {

                throw;
            }
            return lstTimesheet;
        }
        public ConsolidateLeaves GetLeaveDetails(UserIdentityData userIdentityData)
        {
            ConsolidateLeaves lstLeave = new ConsolidateLeaves();
            try
            {
                lstLeave = objEmployee.GetLeaveDetails(userIdentityData);
            }

            catch (Exception)
            {

                throw;
            }
            return lstLeave;
        }
        public List<Leave> GetEmployeePendingLeave(UserIdentityData userIdentityData)
        {
            try
            {
                return objEmployee.GetEmployeePendingLeave(userIdentityData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<Leave> GetHRPendingLeave(Entity entity)
        {
            try
            {
                return objEmployee.GetHRPendingLeave(entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<TimesheetTotalHours> GetTimesheetDetailsTotalHours(UserIdentityData userIdentityData)
        {
            List<TimesheetTotalHours> lstTimesheetTotalHours = new List<TimesheetTotalHours>();
            try
            {
                lstTimesheetTotalHours = objEmployee.GetTimesheetDetailsTotalHours(userIdentityData);
            }
            catch (Exception)
            {

                throw;
            }
            return lstTimesheetTotalHours;
        }
        public Demographic GetDemographicDetail(UserIdentityData userIdentityData)
        {
            Demographic demographic = new Demographic();
            try
            {
                demographic = objEmployee.GetDemographicDetail(userIdentityData);
            }
            catch (Exception)
            {

                throw;
            }
            return demographic;
        }
        public List<PendingTimeSheet> GetPendingTimesheetEmployee(UserIdentityData userIdentityData)
        {

            List<PendingTimeSheet> pendingTimesheet = new List<PendingTimeSheet>();
            try
            {
                pendingTimesheet = objEmployee.GetPendingTimesheetEmployee(userIdentityData);
            }
            catch (Exception)
            {
                throw;
            }
            return pendingTimesheet;
        }
        public DataSet SaveLeave(int EmpID, string leaveType, string leaveFrom, string leaveTo, string Reason, string EmpName, out bool IsEmailSent)
        {
            DataSet dsEmpLeave = new DataSet();
            Leave obj = new Leave();
            obj.LeaveType = leaveType;
            obj.LeaveFrom = leaveFrom;
            obj.LeaveTo = leaveTo;
            obj.LeaveDescription = Reason;
            dsEmpLeave = objEmployee.SaveLeave("SAVE", EmpID, obj);

            //Stopwatch stopwatch = Stopwatch.StartNew();
            //// Delay for 5 seconds
            //Delay(TimeSpan.FromSeconds(5));
            //stopwatch.Stop();
            if (dsEmpLeave != null && dsEmpLeave.Tables.Count > 0 && dsEmpLeave.Tables[0].Rows.Count > 0)
            {
                //Added for send leave notification through AI
                EmployeeMasterBAL objEmployeeMasterBAL = new EmployeeMasterBAL();
                //objEmployeeMasterBAL.SendLeaveNotificationToHrThroughAI(Convert.ToInt32(userIdentityData.EmpID), userIdentityData.LeaveFrom, userIdentityData.LeaveType,
                //    userIdentityData.LeaveReason, userIdentityData.LeaveTo, dsEmpLeave, userIdentityData.EmpName);
                //objEmployeeMasterBAL.SendLeaveNotificationToHrThroughAI(EmpID, leaveFrom, leaveType, Reason, leaveTo, dsEmpLeave, EmpName, EmpLeaveStatus: ClsConstant.EmpLeaveStatus.Pending, UpdateDate: "", UpdatedBy:"");
                Task.Run(() => SendLeaveNotificationToHrThroughAI("LeaveAdd", EmpID, leaveFrom, leaveType, Reason, leaveTo, dsEmpLeave, EmpName, EmpLeaveStatus: ClsConstant.EmpLeaveStatus.Pending, UpdateDate: "", UpdatedBy: ""));
                //Ended
                SendMail(dsEmpLeave.Tables[0],
                               leaveFrom,
                               leaveTo,
                               Reason, out IsEmailSent);
                //objResponseData.Status = "1";
                //objResponseData.Message = "Leave saved succesfully.";
                return dsEmpLeave;
            }
            else
            {
                IsEmailSent = false;
                return null;
            }

        }
        public static void Delay(TimeSpan delay)
        {
            // Using Thread.Sleep to pause execution
            Thread.Sleep(delay);
        }
        public string CalulateLeave(string FromDate, string ToDate)
        {
            string days = "";

            if (ToDate != string.Empty && FromDate != string.Empty)
            {
                DateTime dtf = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dtt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                TimeSpan ts = Convert.ToDateTime(dtf) - Convert.ToDateTime(dtt);
                int da = System.Math.Abs(Convert.ToInt16(ts.TotalDays)) + 1;
                days = da.ToString();
            }
            return days;
        }
        public int DeleteLeave(int EmpLeaveId)
        {
            string empID = string.Empty;
            string LeaveFrom = string.Empty;
            string LeaveType = string.Empty;
            string LeaveReason = string.Empty;
            string LeaveTo = string.Empty;
            string EmpName = string.Empty;
            string strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Deleted;
            string DBUpdateDate = string.Empty;
            string DBUpdatedBy = string.Empty;
            string DBEmpLeaveStatus = string.Empty;
            string DBReason = string.Empty;
            string fromDateFormatted = string.Empty;
            string toDateFormatted = string.Empty;
            int noOfDays = 0;
            DataSet dsEmpLeaveUpdate = new DataSet();
            DataTable dtResult = this.GETLeaveByEMPLeaveID("GETLeaveByEMPLeaveID", EmpLeaveId);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                empID = Convert.ToString(dtResult.Rows[0]["EmpID"]);
                LeaveFrom = Convert.ToString(dtResult.Rows[0]["LeaveFrom"]);
                LeaveType = Convert.ToString(dtResult.Rows[0]["leaveid"]);
                LeaveReason = Convert.ToString(dtResult.Rows[0]["leavedesc"]);
                LeaveTo = Convert.ToString(dtResult.Rows[0]["LeaveTo"]);
                DBEmpLeaveStatus = Convert.ToString(dtResult.Rows[0]["leaveStatus"]);
                DBReason = Convert.ToString(dtResult.Rows[0]["leaveDesc"]);
                noOfDays = Convert.ToInt32(dtResult.Rows[0]["noOfDays"]);
                if (!string.IsNullOrEmpty(LeaveFrom) && !string.IsNullOrEmpty(LeaveTo))
                {
                    fromDateFormatted = LeaveFrom.ToString().Replace("-", "/").Replace("00:00:00", " ").Trim();
                    toDateFormatted = LeaveTo.ToString().Replace("-", "/").Replace("00:00:00", " ").Trim();
                }
                if (!string.IsNullOrEmpty(empID) && Convert.ToInt32(empID) > 0 && EmpLeaveId > 0)
                {
                    dsEmpLeaveUpdate = objEmployee.GetLeaveNotificationDetails("GetLeaveNotificationDetails", EmpLeaveId, Convert.ToInt32(empID));
                    if (dsEmpLeaveUpdate != null && dsEmpLeaveUpdate.Tables[1].Rows.Count > 0)
                    {
                        EmpName = Convert.ToString(dsEmpLeaveUpdate.Tables[1].Rows[0]["empName"]);
                    }
                }
            }
            int returnvalue = 0;
            returnvalue = objEmployee.DeleteLeave(EmpLeaveId, "DELETE");
            if (returnvalue == 1 && string.Compare(DBEmpLeaveStatus, "p", true) == 0)
            {
                if (dsEmpLeaveUpdate != null && dsEmpLeaveUpdate.Tables[0].Rows.Count > 0)
                {
                    if (string.Compare(DBEmpLeaveStatus, "a", true) == 0)
                    {
                        strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Approved;
                    }
                    else if (string.Compare(DBEmpLeaveStatus, "r", true) == 0)
                    {
                        strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Rejected;
                    }
                    else if (string.Compare(DBEmpLeaveStatus, "p", true) == 0)
                    {
                        strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Pending;
                    }
                    else
                    {
                        strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Pending;
                    }
                    Task.Run(() => SendLeaveNotificationToHrThroughAI("LeaveDelete", Convert.ToInt32(empID), fromDateFormatted, LeaveType, DBReason, toDateFormatted, dsEmpLeaveUpdate,
                        EmpName, EmpLeaveStatus: "Deleted", UpdateDate: DateTime.UtcNow.ToString(), UpdatedBy: EmpName, leaveId: EmpLeaveId,
                        UpdatedReason: "Deleted by " + EmpName, noOfDays: noOfDays));

                }
            }
            return returnvalue;
        }
        private async Task<AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto> SendLeaveNotificationToAI(AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto sendLeaveNotificationDto)
        {
            var configSecond = ConfigurationManager.AppSettings.Get("ConfigSecond");
            int second = 5;
            if (!string.IsNullOrEmpty(configSecond))
            {
                second = Convert.ToInt32(configSecond);
            }
            Stopwatch stopwatch = Stopwatch.StartNew();

            Delay(TimeSpan.FromSeconds(second));
            stopwatch.Stop();
            AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto finalResponse = new SendLeaveNotification.SendLeaveNotificationDto();
            try
            {
                var handler = new HttpClientHandler
                {
                    SslProtocols = SslProtocols.Tls11 | SslProtocols.Tls12
                };

                using (var client = new HttpClient(handler))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, AIPushNotificationAPIBaseURL + "SendLeaveNotification");

                    string jsonSerializeObject = JsonConvert.SerializeObject(sendLeaveNotificationDto);

                    var content = new StringContent(jsonSerializeObject, null, "application/json");
                    //var content = new StringContent("{\r\n    \"EmpLeaveDetail\": {\r\n        \"EmpName\": \"Sayeed\",\r\n        \"EmpID\": \"3645\",\r\n        \"ChannelList\":[\r\n                {\r\n                    \"ChannelName\": \"MSTeam\",\r\n                    \"ChannelUniqueId\": \"01105934-5d64-40a2-8a27-4cd1d4e2d43e\",\r\n                    \"ChannelNotificationUniqueId\": \"1721052725265\",\r\n                    \"EmpId\": \"3645\"\r\n                },\r\n                {\r\n                    \"ChannelName\": \"SkypeID\",\r\n                    \"ChannelUniqueId\": \"\",\r\n                    \"ChannelNotificationUniqueId\": null,\r\n                    \"EmpId\": \"3645\"\r\n                }\r\n        ],\r\n        \"FromDate\": \"15/07/2024\",\r\n        \"ToDate\": \"15/07/2024\",\r\n        \"LeaveType\": \"CL\",\r\n        \"Reason\": \"Personal\",\r\n        \"NoOfDays\": 1,\r\n        \"EmpLeaveId\": 18260,\r\n        \"EmpLeaveStatus\": \"Pending\",\r\n        \"UpdateReason\": \"\",\r\n        \"UpdateDate\": \"\",\r\n        \"UpdatedBy\": \"\"\r\n    },\r\n    \"HRIdList\": [\r\n        {\r\n            \"HRName\": \"Sayeed\",\r\n            \"ChannelList\": [\r\n                {\r\n                    \"ChannelName\": \"MSTeam\",\r\n                    \"ChannelUniqueId\": \"01105934-5d64-40a2-8a27-4cd1d4e2d43e\",\r\n                    \"ChannelNotificationUniqueId\": \"\",\r\n                    \"EmpId\": \"3645\"\r\n                },\r\n                {\r\n                    \"ChannelName\": \"SkypeID\",\r\n                    \"ChannelUniqueId\": \"\",\r\n                    \"ChannelNotificationUniqueId\": null,\r\n                    \"EmpId\": \"3645\"\r\n                }\r\n            ],\r\n            \"EmpId\": \"3645\"\r\n        },\r\n        {\r\n            \"HRName\": \"Shaad\",\r\n            \"ChannelList\": [\r\n                {\r\n                    \"ChannelName\": \"MSTeam\",\r\n                    \"ChannelUniqueId\": \"277f73e8-fc86-4470-bb98-57881f285cb8\",\r\n                    \"ChannelNotificationUniqueId\": \"\",\r\n                    \"EmpId\": \"3640\"\r\n                },\r\n                {\r\n                    \"ChannelName\": \"SkypeID\",\r\n                    \"ChannelUniqueId\": \"\",\r\n                    \"ChannelNotificationUniqueId\": \"None\",\r\n                    \"EmpId\": \"3640\"\r\n                }\r\n            ],\r\n            \"EmpId\": \"3640\"\r\n        }\r\n    ]\r\n}", null, "application/json");
                    request.Content = content;
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    request.Headers.Add("Authorization", AIPushNotificationAPIAuthHeader);
                    //var response = client.SendAsync(request).GetAwaiter().GetResult();
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        finalResponse = JsonConvert.DeserializeObject<AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto>(jsonResponse);
                        return finalResponse;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async void SendLeaveNotificationToHrThroughAI(string mode, int EmpID, string leaveFrom, string leaveType, string Reason, string leaveTo, DataSet ds, string EmpName, string EmpLeaveStatus, string UpdateDate = "", string UpdatedBy = "", string UpdatedReason = "", int leaveId = 0, int noOfDays = 0)
        {

            if (string.Compare(mode, "LeaveAdd", true) == 0)
            {
                string empName = string.Empty;
                if (!string.IsNullOrEmpty(EmpName))
                {
                    empName = EmpName;
                }
                else
                {
                    if (ds != null && ds.Tables[2].Rows.Count > 0)
                    {
                        empName = Convert.ToString(ds.Tables[0].Rows[0]["empName"]);
                    }
                }
                int LeaveDays = 0;
                if (string.IsNullOrEmpty(UpdatedBy))
                {
                    LeaveDays = Convert.ToInt32(CalulateLeave(leaveFrom, leaveTo));
                }
                int empLeaveId = 0;
                if (ds != null && ds.Tables[2].Rows.Count > 0)
                {
                    empLeaveId = Convert.ToInt32(ds.Tables[2].Rows[0]["LeaveId"]);
                };
                List<AgoraBL.Models.SendLeaveNotification.ChannelList> lstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                if (ds != null && ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        List<string> lstChannels = new List<string>();
                        lstChannels = AIChannels.Split(',').ToList();
                        List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                        for (int channelcount = 0; channelcount < lstChannels.Count; channelcount++)
                        {
                            if (ds.Tables[3].Columns.Contains(lstChannels[channelcount]))
                            {
                                AgoraBL.Models.SendLeaveNotification.ChannelList objChannelList = new AgoraBL.Models.SendLeaveNotification.ChannelList();
                                objChannelList.ChannelName = lstChannels[channelcount];
                                objChannelList.ChannelUniqueId = Convert.ToString(ds.Tables[3].Rows[i][lstChannels[channelcount]]);
                                objChannelList.ChannelNotificationUniqueId = null;
                                objChannelList.EmpId = Convert.ToString(ds.Tables[3].Rows[i]["empid"]);
                                lstChannelList.Add(objChannelList);
                            }
                        }

                    }
                }
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto objSendLeaveNotificationDto = new AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto();
                    objSendLeaveNotificationDto.EmpLeaveDetail = new AgoraBL.Models.SendLeaveNotification.EmpLeaveDetail()
                    {
                        EmpName = empName,
                        EmpID = EmpID.ToString(),
                        ChannelList = lstChannelList,
                        FromDate = leaveFrom,
                        ToDate = leaveTo,
                        LeaveType = leaveType,
                        Reason = Reason,
                        NoOfDays = LeaveDays,
                        EmpLeaveId = empLeaveId,
                        EmpLeaveStatus = EmpLeaveStatus,
                        UpdateReason = UpdatedReason,
                        UpdateDate = UpdateDate,
                        UpdatedBy = UpdatedBy,
                    };
                    List<AgoraBL.Models.SendLeaveNotification.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendLeaveNotification.HRIdList>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        List<string> lstChannels = new List<string>();
                        lstChannels = AIChannels.Split(',').ToList();
                        List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                        for (int channelcount = 0; channelcount < lstChannels.Count; channelcount++)
                        {
                            if (ds.Tables[1].Columns.Contains(lstChannels[channelcount]))
                            {
                                AgoraBL.Models.SendLeaveNotification.ChannelList objChannelList = new AgoraBL.Models.SendLeaveNotification.ChannelList();
                                objChannelList.ChannelName = lstChannels[channelcount];
                                objChannelList.ChannelUniqueId = Convert.ToString(ds.Tables[1].Rows[i][lstChannels[channelcount]]);
                                objChannelList.ChannelNotificationUniqueId = null;
                                objChannelList.EmpId = Convert.ToString(ds.Tables[1].Rows[i]["empid"]);
                                objLstChannelList.Add(objChannelList);
                            }
                        }
                        AgoraBL.Models.SendLeaveNotification.HRIdList objHRIdList = new AgoraBL.Models.SendLeaveNotification.HRIdList()
                        {
                            EmpId = Convert.ToString(ds.Tables[1].Rows[i]["empid"]),
                            HRName = Convert.ToString(ds.Tables[1].Rows[i]["empName"]),
                            //ObjectId = Convert.ToString(ds.Tables[1].Rows[0]["MSTeam"]),
                            ChannelList = objLstChannelList,
                        };
                        lstHRIdList.Add(objHRIdList);
                    }
                    objSendLeaveNotificationDto.HRIdList = lstHRIdList;
                    // Code added for Stakeholder
                    DataSet dsStakeholders = objEmployee.GetProjectRoleEmailsByEmpIDs(EmpID);
                    if (dsStakeholders != null && dsStakeholders.Tables.Count > 0)
                    {
                        DataTable stakeholderChannelTable = FlattenStakeholderChannels(dsStakeholders.Tables[0]);
                        if (stakeholderChannelTable != null && stakeholderChannelTable.Rows.Count > 0)
                        {
                            var stakeholderGroups = stakeholderChannelTable.AsEnumerable()
                                .Where(row => row != null)
                                .GroupBy(row => new
                                {
                                    EmpId = row.Table.Columns.Contains("Empid") ? row["Empid"]?.ToString() ?? string.Empty : string.Empty,
                                    StakeHolderName = row.Table.Columns.Contains("empName") ? row["empName"]?.ToString() ?? string.Empty : string.Empty,
                                    RoleName = row.Table.Columns.Contains("RoleName") ? row["RoleName"]?.ToString() ?? string.Empty : string.Empty
                                });

                            List<AgoraBL.Models.SendLeaveNotification.StakeHolderInfo> lstStakeHolder = new List<AgoraBL.Models.SendLeaveNotification.StakeHolderInfo>();

                            foreach (var group in stakeholderGroups)
                            {
                                List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                                foreach (var row in group)
                                {
                                    objLstChannelList.Add(new AgoraBL.Models.SendLeaveNotification.ChannelList
                                    {
                                        EmpId = row.Table.Columns.Contains("Empid") ? row["Empid"]?.ToString() ?? string.Empty : string.Empty,
                                        ChannelName = row.Table.Columns.Contains("ChannelName") ? row["ChannelName"]?.ToString() ?? string.Empty : string.Empty,
                                        ChannelUniqueId = row.Table.Columns.Contains("ChannelUniqueId") ? row["ChannelUniqueId"]?.ToString() ?? string.Empty : string.Empty,
                                        ChannelNotificationUniqueId = row.Table.Columns.Contains("ChannelNotificationUniqueId") ? row["ChannelNotificationUniqueId"]?.ToString() ?? string.Empty : string.Empty
                                    });
                                }

                                objLstChannelList = objLstChannelList
                                    .GroupBy(x => new { x.ChannelName, x.ChannelUniqueId, x.ChannelNotificationUniqueId, x.EmpId })
                                    .Select(g => g.First())
                                    .ToList();

                                lstStakeHolder.Add(new AgoraBL.Models.SendLeaveNotification.StakeHolderInfo
                                {
                                    RoleName = group.Key.RoleName,
                                    StakeHolderName = group.Key.StakeHolderName,
                                    EmpId = group.Key.EmpId,
                                    ChannelList = objLstChannelList
                                });
                            }

                            if (objSendLeaveNotificationDto.StakeHolders == null)
                                objSendLeaveNotificationDto.StakeHolders = new List<AgoraBL.Models.SendLeaveNotification.StakeHolderInfo>();

                            objSendLeaveNotificationDto.StakeHolders.AddRange(lstStakeHolder);
                        }
                    }


                    var notificationObject = await Task.Run(() => SendLeaveNotificationToAI(objSendLeaveNotificationDto));
                    if (notificationObject != null && notificationObject.EmpLeaveDetail != null)
                    {
                        if (notificationObject.HRIdList != null && notificationObject.HRIdList.Count > 0)
                        {
                            for (int i = 0; i < notificationObject.HRIdList.Count; i++)
                            {
                                if (notificationObject.HRIdList[i].ChannelList != null && notificationObject.HRIdList[i].ChannelList.Count > 0)
                                {
                                    for (int j = 0; j < notificationObject.HRIdList[i].ChannelList.Count; j++)
                                    {
                                        objEmployee.AddLeaveNotification(notificationObject.EmpLeaveDetail.EmpLeaveId, notificationObject.HRIdList[i].ChannelList[j].EmpId,
                                            notificationObject.HRIdList[i].ChannelList[j].ChannelName, notificationObject.HRIdList[i].ChannelList[j].ChannelUniqueId,
                                            notificationObject.HRIdList[i].ChannelList[j].ChannelNotificationUniqueId);
                                    }
                                }
                            }
                        }

                        if (notificationObject.EmpLeaveDetail.ChannelList != null && notificationObject.EmpLeaveDetail.ChannelList.Count > 0)
                        {
                            for (int i = 0; i < notificationObject.EmpLeaveDetail.ChannelList.Count; i++)
                            {
                                objEmployee.AddLeaveNotification(notificationObject.EmpLeaveDetail.EmpLeaveId, notificationObject.EmpLeaveDetail.ChannelList[i].EmpId,
                                    notificationObject.EmpLeaveDetail.ChannelList[i].ChannelName, notificationObject.EmpLeaveDetail.ChannelList[i].ChannelUniqueId,
                                    notificationObject.EmpLeaveDetail.ChannelList[i].ChannelNotificationUniqueId);

                            }
                        }
                    }
                }
            }
            else if (string.Compare(mode, "LeaveUpdate", true) == 0)
            {
                if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    EmpName = Convert.ToString(ds.Tables[1].Rows[0]["empName"]);
                }
                int LeaveDays = 0;
                if (!string.IsNullOrEmpty(UpdatedBy))
                {
                    LeaveDays = Convert.ToInt32(CalulateLeave(leaveFrom, leaveTo));
                }
                List<AgoraBL.Models.SendLeaveNotification.ChannelList> lstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                if (ds != null && ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        AgoraBL.Models.SendLeaveNotification.ChannelList objChannelList = new AgoraBL.Models.SendLeaveNotification.ChannelList();
                        objChannelList.ChannelName = Convert.ToString(ds.Tables[3].Rows[i]["ChannelName"]);
                        objChannelList.ChannelUniqueId = Convert.ToString(ds.Tables[3].Rows[i]["ChannelUniqueId"]);
                        objChannelList.ChannelNotificationUniqueId = Convert.ToString(ds.Tables[3].Rows[i]["ChannelNotificationUniqueId"]);
                        objChannelList.EmpId = Convert.ToString(ds.Tables[3].Rows[i]["Empid"]);
                        lstChannelList.Add(objChannelList);
                    }
                    lstChannelList = lstChannelList.Distinct().ToList();
                }
                AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto objSendLeaveNotificationDto = new AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto();
                objSendLeaveNotificationDto.EmpLeaveDetail = new AgoraBL.Models.SendLeaveNotification.EmpLeaveDetail()
                {
                    EmpName = EmpName,
                    EmpID = EmpID.ToString(),
                    ChannelList = lstChannelList,
                    FromDate = leaveFrom,
                    ToDate = leaveTo,
                    LeaveType = leaveType,
                    Reason = Reason,
                    NoOfDays = LeaveDays,
                    EmpLeaveId = leaveId,
                    EmpLeaveStatus = EmpLeaveStatus,
                    UpdateReason = UpdatedReason,
                    UpdateDate = UpdateDate,
                    UpdatedBy = UpdatedBy,
                };
                List<AgoraBL.Models.SendLeaveNotification.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendLeaveNotification.HRIdList>();

                if (ds != null && ds.Tables[4].Rows.Count > 0)
                {
                    var hrGroups = ds.Tables[4].AsEnumerable()
                                          .GroupBy(row => new
                                          {
                                              EmpId = row["Empid"].ToString(),
                                              HRName = row["empName"].ToString()
                                          });

                    foreach (var hrGroup in hrGroups)
                    {
                        List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();

                        foreach (var row in hrGroup)
                        {
                            AgoraBL.Models.SendLeaveNotification.ChannelList objChannelList = new AgoraBL.Models.SendLeaveNotification.ChannelList
                            {
                                ChannelName = row["ChannelName"].ToString(),
                                ChannelUniqueId = row["ChannelUniqueId"].ToString(),
                                ChannelNotificationUniqueId = row["ChannelNotificationUniqueId"].ToString(),
                                EmpId = row["Empid"].ToString()
                            };

                            objLstChannelList.Add(objChannelList);
                        }

                        objLstChannelList = objLstChannelList.Distinct().ToList();

                        AgoraBL.Models.SendLeaveNotification.HRIdList objHRIdList = new AgoraBL.Models.SendLeaveNotification.HRIdList
                        {
                            EmpId = hrGroup.Key.EmpId,
                            HRName = hrGroup.Key.HRName,
                            ChannelList = objLstChannelList
                        };

                        lstHRIdList.Add(objHRIdList);
                    }
                }
                objSendLeaveNotificationDto.HRIdList = lstHRIdList;
                DataSet dsStakeholders = objEmployee.GetProjectRoleEmailsByEmpIDs(EmpID);
                if (dsStakeholders != null && dsStakeholders.Tables.Count > 0)
                {
                    DataTable stakeholderChannelTable = FlattenStakeholderChannels(dsStakeholders.Tables[0]);
                    if (stakeholderChannelTable != null && stakeholderChannelTable.Rows.Count > 0)
                    {
                        var stakeholderGroups = stakeholderChannelTable.AsEnumerable()
                            .Where(row => row != null)
                            .GroupBy(row => new
                            {
                                EmpId = row.Table.Columns.Contains("Empid") ? row["Empid"]?.ToString() ?? string.Empty : string.Empty,
                                StakeHolderName = row.Table.Columns.Contains("empName") ? row["empName"]?.ToString() ?? string.Empty : string.Empty,
                                RoleName = row.Table.Columns.Contains("RoleName") ? row["RoleName"]?.ToString() ?? string.Empty : string.Empty
                            });

                        List<AgoraBL.Models.SendLeaveNotification.StakeHolderInfo> lstStakeHolder = new List<AgoraBL.Models.SendLeaveNotification.StakeHolderInfo>();

                        foreach (var group in stakeholderGroups)
                        {
                            List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                            foreach (var row in group)
                            {
                                objLstChannelList.Add(new AgoraBL.Models.SendLeaveNotification.ChannelList
                                {
                                    EmpId = row.Table.Columns.Contains("Empid") ? row["Empid"]?.ToString() ?? string.Empty : string.Empty,
                                    ChannelName = row.Table.Columns.Contains("ChannelName") ? row["ChannelName"]?.ToString() ?? string.Empty : string.Empty,
                                    ChannelUniqueId = row.Table.Columns.Contains("ChannelUniqueId") ? row["ChannelUniqueId"]?.ToString() ?? string.Empty : string.Empty,
                                    ChannelNotificationUniqueId = row.Table.Columns.Contains("ChannelNotificationUniqueId") ? row["ChannelNotificationUniqueId"]?.ToString() ?? string.Empty : string.Empty
                                });
                            }

                            objLstChannelList = objLstChannelList
                                .GroupBy(x => new { x.ChannelName, x.ChannelUniqueId, x.ChannelNotificationUniqueId, x.EmpId })
                                .Select(g => g.First())
                                .ToList();

                            lstStakeHolder.Add(new AgoraBL.Models.SendLeaveNotification.StakeHolderInfo
                            {
                                RoleName = group.Key.RoleName,
                                StakeHolderName = group.Key.StakeHolderName,
                                EmpId = group.Key.EmpId,
                                ChannelList = objLstChannelList
                            });
                        }

                        if (objSendLeaveNotificationDto.StakeHolders == null)
                            objSendLeaveNotificationDto.StakeHolders = new List<AgoraBL.Models.SendLeaveNotification.StakeHolderInfo>();

                        objSendLeaveNotificationDto.StakeHolders.AddRange(lstStakeHolder);
                    }
                }

                var notificationObject = await Task.Run(() => SendLeaveNotificationToAI(objSendLeaveNotificationDto));
            }
            else
            {
                List<AgoraBL.Models.SendLeaveNotification.ChannelList> lstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                if (ds != null && ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        AgoraBL.Models.SendLeaveNotification.ChannelList objChannelList = new AgoraBL.Models.SendLeaveNotification.ChannelList();
                        objChannelList.ChannelName = Convert.ToString(ds.Tables[3].Rows[i]["ChannelName"]);
                        objChannelList.ChannelUniqueId = Convert.ToString(ds.Tables[3].Rows[i]["ChannelUniqueId"]);
                        objChannelList.ChannelNotificationUniqueId = Convert.ToString(ds.Tables[3].Rows[i]["ChannelNotificationUniqueId"]);
                        objChannelList.EmpId = Convert.ToString(ds.Tables[3].Rows[i]["Empid"]);
                        lstChannelList.Add(objChannelList);
                    }
                    lstChannelList = lstChannelList.Distinct().ToList();
                }
                AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto objSendLeaveNotificationDto = new AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto();
                objSendLeaveNotificationDto.EmpLeaveDetail = new AgoraBL.Models.SendLeaveNotification.EmpLeaveDetail()
                {
                    EmpName = EmpName,
                    EmpID = EmpID.ToString(),
                    ChannelList = lstChannelList,
                    FromDate = leaveFrom,
                    ToDate = leaveTo,
                    LeaveType = leaveType,
                    Reason = Reason,
                    NoOfDays = noOfDays,
                    EmpLeaveId = leaveId,
                    EmpLeaveStatus = EmpLeaveStatus,
                    UpdateReason = UpdatedReason,
                    UpdateDate = UpdateDate,
                    UpdatedBy = UpdatedBy,
                };
                List<AgoraBL.Models.SendLeaveNotification.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendLeaveNotification.HRIdList>();

                if (ds != null && ds.Tables[4].Rows.Count > 0)
                {
                    var hrGroups = ds.Tables[4].AsEnumerable()
                                          .GroupBy(row => new
                                          {
                                              EmpId = row["Empid"].ToString(),
                                              HRName = row["empName"].ToString()
                                          });

                    foreach (var hrGroup in hrGroups)
                    {
                        List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();

                        foreach (var row in hrGroup)
                        {
                            AgoraBL.Models.SendLeaveNotification.ChannelList objChannelList = new AgoraBL.Models.SendLeaveNotification.ChannelList
                            {
                                ChannelName = row["ChannelName"].ToString(),
                                ChannelUniqueId = row["ChannelUniqueId"].ToString(),
                                ChannelNotificationUniqueId = row["ChannelNotificationUniqueId"].ToString(),
                                EmpId = row["Empid"].ToString()
                            };

                            objLstChannelList.Add(objChannelList);
                        }

                        objLstChannelList = objLstChannelList.Distinct().ToList();

                        AgoraBL.Models.SendLeaveNotification.HRIdList objHRIdList = new AgoraBL.Models.SendLeaveNotification.HRIdList
                        {
                            EmpId = hrGroup.Key.EmpId,
                            HRName = hrGroup.Key.HRName,
                            ChannelList = objLstChannelList
                        };

                        lstHRIdList.Add(objHRIdList);
                    }
                }
                objSendLeaveNotificationDto.HRIdList = lstHRIdList;
                DataSet dsStakeholders = objEmployee.GetProjectRoleEmailsByEmpIDs(EmpID);
                if (dsStakeholders != null && dsStakeholders.Tables.Count > 0)
                {
                    DataTable stakeholderChannelTable = FlattenStakeholderChannels(dsStakeholders.Tables[0]);
                    if (stakeholderChannelTable != null && stakeholderChannelTable.Rows.Count > 0)
                    {
                        var stakeholderGroups = stakeholderChannelTable.AsEnumerable()
                            .Where(row => row != null)
                            .GroupBy(row => new
                            {
                                EmpId = row.Table.Columns.Contains("Empid") ? row["Empid"]?.ToString() ?? string.Empty : string.Empty,
                                StakeHolderName = row.Table.Columns.Contains("empName") ? row["empName"]?.ToString() ?? string.Empty : string.Empty,
                                RoleName = row.Table.Columns.Contains("RoleName") ? row["RoleName"]?.ToString() ?? string.Empty : string.Empty
                            });

                        List<AgoraBL.Models.SendLeaveNotification.StakeHolderInfo> lstStakeHolder = new List<AgoraBL.Models.SendLeaveNotification.StakeHolderInfo>();

                        foreach (var group in stakeholderGroups)
                        {
                            List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                            foreach (var row in group)
                            {
                                objLstChannelList.Add(new AgoraBL.Models.SendLeaveNotification.ChannelList
                                {
                                    EmpId = row.Table.Columns.Contains("Empid") ? row["Empid"]?.ToString() ?? string.Empty : string.Empty,
                                    ChannelName = row.Table.Columns.Contains("ChannelName") ? row["ChannelName"]?.ToString() ?? string.Empty : string.Empty,
                                    ChannelUniqueId = row.Table.Columns.Contains("ChannelUniqueId") ? row["ChannelUniqueId"]?.ToString() ?? string.Empty : string.Empty,
                                    ChannelNotificationUniqueId = row.Table.Columns.Contains("ChannelNotificationUniqueId") ? row["ChannelNotificationUniqueId"]?.ToString() ?? string.Empty : string.Empty
                                });
                            }

                            objLstChannelList = objLstChannelList
                                .GroupBy(x => new { x.ChannelName, x.ChannelUniqueId, x.ChannelNotificationUniqueId, x.EmpId })
                                .Select(g => g.First())
                                .ToList();

                            lstStakeHolder.Add(new AgoraBL.Models.SendLeaveNotification.StakeHolderInfo
                            {
                                RoleName = group.Key.RoleName,
                                StakeHolderName = group.Key.StakeHolderName,
                                EmpId = group.Key.EmpId,
                                ChannelList = objLstChannelList
                            });
                        }

                        if (objSendLeaveNotificationDto.StakeHolders == null)
                            objSendLeaveNotificationDto.StakeHolders = new List<AgoraBL.Models.SendLeaveNotification.StakeHolderInfo>();

                        objSendLeaveNotificationDto.StakeHolders.AddRange(lstStakeHolder);
                    }
                }

                var notificationObject = await Task.Run(() => SendLeaveNotificationToAI(objSendLeaveNotificationDto));
            }
        }
        public bool IfExistsLeave(string mode, int empid, string leaveFrom, string leaveTo)
        {
            return objEmployee.IfExistsLeave(mode, empid, leaveFrom, leaveTo);
        }
        public DataTable GETLeaveByEMPLeaveID(string mode, int EmpLeaveID)
        {
            return objEmployee.GETLeaveByEMPLeaveID(mode, EmpLeaveID);
        }
        public void SendMail(DataTable dt, string strDateFrom, string strDateTo, string strReason, out bool IsEmailSent)
        {
            string[] DateFrom = strDateFrom.Split('/');
            string New_FromDt = DateFrom[1] + '/' + DateFrom[0] + '/' + DateFrom[2];
            DateTime dtFrom = Convert.ToDateTime(New_FromDt.ToString());

            string[] DateTo = strDateTo.Split('/');
            string New_ToDt = DateTo[1] + '/' + DateTo[0] + '/' + DateTo[2];
            DateTime dtTo = Convert.ToDateTime(New_ToDt.ToString());

            string strBody, strSubject, mailTo, mailFrom, CC = "";
            List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 44);
            strBody = lstConfig[0].value.ToString();
            strBody = strBody.Replace("{UserNmae}", Convert.ToString(dt.Rows[0]["empName"]));
            strBody = strBody.Replace("{Fromdate}", Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy"));
            strBody = strBody.Replace("{Todate}", Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy"));
            strBody = strBody.Replace("{Reason}", strReason);
            strSubject = Convert.ToString(dt.Rows[0]["empName"]) + " has applied for leave.";

            mailTo = ConfigurationManager.AppSettings.Get("HREmail").ToString();
            mailFrom = dt.Rows[0]["empEmail"].ToString();
            HashSet<string> ccList = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            ccList.Add(mailFrom);

            // Add role-based emails from SP
            int empId = Convert.ToInt32(dt.Rows[0]["empId"]);
            DataTable dtRoleEmails = objEmployee.GetProjectRoleEmailsByEmpID(empId);
            foreach (DataRow row in dtRoleEmails.Rows)
            {
                string email = row["EmailID"].ToString();
                if (!string.IsNullOrWhiteSpace(email))
                {
                    ccList.Add(email);
                }
            }

            CC = string.Join(",", ccList);

            objEmployee.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "", out IsEmailSent);
        }
        //public string UpdateEmpLeaveStatus(string mode, string empID, string empLeaveID, string leaveStatus, string LeaveType, string AdminComment, string SanctionedBy, string fDate, string tDate)
        //{
        //    string ip = "0.0.0.0";
        //    EmpLeaveApprovalBLL objBLL = new EmpLeaveApprovalBLL();
        //    EmpLeaveApprovalDAL objDAL = new EmpLeaveApprovalDAL();
        //    objBLL.EmpID = empID;
        //    objBLL.EmpLeaveID = empLeaveID;
        //    objBLL.LeaveStatus = leaveStatus;
        //    objBLL.LeaveType = LeaveType;
        //    objBLL.AdminComment = AdminComment;
        //    objBLL.LeaveSanctionBy = SanctionedBy;
        //    objBLL.LeaveFrom = fDate;
        //    objBLL.LeaveTo = tDate;

        //    return objDAL.UpdateEmpLeaveStatus(mode, ip, objBLL);

        //}
        private DataTable FlattenStakeholderChannels(DataTable originalTable)
        {
            DataTable flatTable = new DataTable();
            flatTable.Columns.Add("Empid");
            flatTable.Columns.Add("empName");
            flatTable.Columns.Add("RoleName");
            flatTable.Columns.Add("ChannelName");
            flatTable.Columns.Add("ChannelUniqueId");
            flatTable.Columns.Add("ChannelNotificationUniqueId");

            foreach (DataRow row in originalTable.Rows)
            {
                string empId = row["EmpID"].ToString();
                string name = row["EmpName"].ToString();
                string role = row["Role"].ToString();

                // MSTeam
                if (originalTable.Columns.Contains("MSTeam") && !string.IsNullOrEmpty(row["MSTeam"]?.ToString()))
                {
                    flatTable.Rows.Add(empId, name, role, "MSTeam", row["MSTeam"].ToString(), null);
                }

                // SkypeID
                if (originalTable.Columns.Contains("SkypeID") && !string.IsNullOrEmpty(row["SkypeID"]?.ToString()))
                {
                    flatTable.Rows.Add(empId, name, role, "SkypeID", row["SkypeID"].ToString(), null);
                }

                // Add other channels here if needed
            }

            return flatTable;
        }


        #endregion

        #region Check Admin
        public bool CheckAdmin(int EmpId)
        {
            try
            {
                return objEmployee.CheckAdmin(EmpId); ;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool CheckIsPMBAAM(int EmpId) // check PM BA AM
        {
            try
            {
                return objEmployee.CheckIsPMBAAM(EmpId); ;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        public static bool AuthenticateUser(string domain, string username, string password, string LdapPath, out string Errmsg)
        {
            Errmsg = string.Empty;
            using (HostingEnvironment.Impersonate())
            {
                DirectoryEntry entry = new DirectoryEntry(LdapPath, username, password, System.DirectoryServices.AuthenticationTypes.Secure);
                try
                {
                    // Bind to the native AdsObject to force authentication.
                    Object obj = entry.NativeObject;
                    DirectorySearcher search = new DirectorySearcher(entry);
                    search.Filter = "(&(objectClass=top)(objectClass=person)(objectClass=organizationalPerson)(objectClass=user))";
                    search.PropertyNamesOnly = false;
                    search.PropertiesToLoad.Add("cn");
                    SearchResult result = search.FindOne();
                    if (null == result)
                    {
                        return false;
                    }
                    // Update the new path to the user in the directory
                    LdapPath = result.Path;
                    string _filterAttribute = (String)result.Properties["cn"][0];
                }
                catch (Exception ex)
                {
                    Errmsg = ex.Message;
                    return false;
                }
                return true;
            }
        }
        private async Task<AgoraBL.Models.SendTimesheetNotificationDTO.SendTimesheetNotification> SendTimesheetNotificationToAI(AgoraBL.Models.SendTimesheetNotificationDTO.SendTimesheetNotification sendTimesheetNotificationDTO)
        {
            var configSecond = ConfigurationManager.AppSettings.Get("ConfigSecond");
            int second = 5;
            if (!string.IsNullOrEmpty(configSecond))
            {
                second = Convert.ToInt32(configSecond);
            }
            Stopwatch stopwatch = Stopwatch.StartNew();

            Delay(TimeSpan.FromSeconds(second));
            stopwatch.Stop();
            AgoraBL.Models.SendTimesheetNotificationDTO.SendTimesheetNotification finalResponse = new SendTimesheetNotificationDTO.SendTimesheetNotification();
            try
            {
                var handler = new HttpClientHandler
                {
                    SslProtocols = SslProtocols.Tls11 | SslProtocols.Tls12
                };

                using (var client = new HttpClient(handler))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, AIPushNotificationAPIBaseURL + "SendTimesheetNotification");

                    string jsonSerializeObject = JsonConvert.SerializeObject(sendTimesheetNotificationDTO);

                    var content = new StringContent(jsonSerializeObject, null, "application/json");
                    request.Content = content;
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    request.Headers.Add("Authorization", AIPushNotificationAPIAuthHeader);
                    //var response = client.SendAsync(request).GetAwaiter().GetResult();
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        finalResponse = JsonConvert.DeserializeObject<AgoraBL.Models.SendTimesheetNotificationDTO.SendTimesheetNotification>(jsonResponse);
                        return finalResponse;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async void SendTimesheetNotificationToHrThroughAI(string mode, int EmpID, string tsFrom, string tsTo, string EmpName, DataSet ds,
             string Requested, string EmpTimesheetStatus = "", string UpdatedReason = "", string UpdateDate = "", string UpdatedBy = "", string UniqueId = "")
        {

            if (string.Compare(mode, "SendRequestFirstTime", true) == 0)
            {
                var uniqueId = Guid.NewGuid();
                string empName = string.Empty;
                if (!string.IsNullOrEmpty(EmpName))
                {
                    empName = EmpName;
                }
                List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList> lstChannelList = new List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList>();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        List<string> lstChannels = new List<string>();
                        lstChannels = AIChannels.Split(',').ToList();
                        List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList>();
                        for (int channelcount = 0; channelcount < lstChannels.Count; channelcount++)
                        {
                            if (ds.Tables[0].Columns.Contains(lstChannels[channelcount]))
                            {
                                AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList objChannelList = new AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList();
                                objChannelList.ChannelName = lstChannels[channelcount];
                                objChannelList.ChannelUniqueId = Convert.ToString(ds.Tables[0].Rows[i][lstChannels[channelcount]]);
                                objChannelList.ChannelNotificationUniqueId = null;
                                objChannelList.EmpId = Convert.ToString(ds.Tables[0].Rows[i]["empid"]);
                                lstChannelList.Add(objChannelList);
                            }
                        }

                    }
                }
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    AgoraBL.Models.SendTimesheetNotificationDTO.SendTimesheetNotification objSendTimesheetNotificationDto = new AgoraBL.Models.SendTimesheetNotificationDTO.SendTimesheetNotification();
                    objSendTimesheetNotificationDto.EmpTimesheetDetail = new AgoraBL.Models.SendTimesheetNotificationDTO.EmpTimesheetDetail()
                    {
                        EmpName = empName,
                        EmpID = EmpID.ToString(),
                        ChannelList = lstChannelList,
                        FromDate = tsFrom,
                        ToDate = tsTo,
                        Requested = Requested,
                        EmpTimesheetStatus = EmpTimesheetStatus,
                        UpdateReason = UpdatedReason,
                        UpdateDate = UpdateDate,
                        UpdatedBy = UpdatedBy,
                        UniqueId = uniqueId.ToString(),
                    };
                    List<AgoraBL.Models.SendTimesheetNotificationDTO.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendTimesheetNotificationDTO.HRIdList>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        List<string> lstChannels = new List<string>();
                        lstChannels = AIChannels.Split(',').ToList();
                        List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList>();
                        for (int channelcount = 0; channelcount < lstChannels.Count; channelcount++)
                        {
                            if (ds.Tables[1].Columns.Contains(lstChannels[channelcount]))
                            {
                                AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList objChannelList = new AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList();
                                objChannelList.ChannelName = lstChannels[channelcount];
                                objChannelList.ChannelUniqueId = Convert.ToString(ds.Tables[1].Rows[i][lstChannels[channelcount]]);
                                objChannelList.ChannelNotificationUniqueId = null;
                                objChannelList.EmpId = Convert.ToString(ds.Tables[1].Rows[i]["empid"]);
                                objLstChannelList.Add(objChannelList);
                            }
                        }
                        AgoraBL.Models.SendTimesheetNotificationDTO.HRIdList objHRIdList = new AgoraBL.Models.SendTimesheetNotificationDTO.HRIdList()
                        {
                            EmpId = Convert.ToString(ds.Tables[1].Rows[i]["empid"]),
                            HRName = Convert.ToString(ds.Tables[1].Rows[i]["empName"]),
                            ChannelList = objLstChannelList,
                        };
                        lstHRIdList.Add(objHRIdList);
                    }
                    objSendTimesheetNotificationDto.HRIdList = lstHRIdList;
                    var notificationObject = await Task.Run(() => SendTimesheetNotificationToAI(objSendTimesheetNotificationDto));
                    //var notificationObject = await SendTimesheetNotificationToAI(objSendTimesheetNotificationDto);
                    if (notificationObject != null && notificationObject.EmpTimesheetDetail != null)
                    {
                        EmployeeMasterDAL objEmployeeMasterDAL = new EmployeeMasterDAL();
                        objEmployeeMasterDAL.AddTimesheetNotificationDetails(uniqueId.ToString(), EmpID.ToString(), EmpName, tsFrom, tsTo, Requested);
                        if (notificationObject.HRIdList != null && notificationObject.HRIdList.Count > 0)
                        {
                            for (int i = 0; i < notificationObject.HRIdList.Count; i++)
                            {
                                if (notificationObject.HRIdList[i].ChannelList != null && notificationObject.HRIdList[i].ChannelList.Count > 0)
                                {
                                    for (int j = 0; j < notificationObject.HRIdList[i].ChannelList.Count; j++)
                                    {
                                        objEmployee.AddTimesheetNotification(uniqueId.ToString(), notificationObject.HRIdList[i].ChannelList[j].EmpId,
                                            notificationObject.HRIdList[i].ChannelList[j].ChannelName, notificationObject.HRIdList[i].ChannelList[j].ChannelUniqueId,
                                            notificationObject.HRIdList[i].ChannelList[j].ChannelNotificationUniqueId);
                                    }
                                }
                            }
                        }

                        if (notificationObject.EmpTimesheetDetail.ChannelList != null && notificationObject.EmpTimesheetDetail.ChannelList.Count > 0)
                        {
                            for (int i = 0; i < notificationObject.EmpTimesheetDetail.ChannelList.Count; i++)
                            {
                                objEmployee.AddTimesheetNotification(uniqueId.ToString(), notificationObject.EmpTimesheetDetail.ChannelList[i].EmpId,
                                    notificationObject.EmpTimesheetDetail.ChannelList[i].ChannelName, notificationObject.EmpTimesheetDetail.ChannelList[i].ChannelUniqueId,
                                    notificationObject.EmpTimesheetDetail.ChannelList[i].ChannelNotificationUniqueId);

                            }
                        }
                    }
                }
            }
            else if (string.Compare(mode, "SendRequestSecondTime", true) == 0)
            {
                string empName = string.Empty;
                if (!string.IsNullOrEmpty(EmpName))
                {
                    empName = EmpName;
                }
                List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList> lstChannelList = new List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList>();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList objChannelList = new AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList();
                        objChannelList.ChannelName = Convert.ToString(ds.Tables[0].Rows[i]["ChannelName"]);
                        objChannelList.ChannelUniqueId = Convert.ToString(ds.Tables[0].Rows[i]["ChannelUniqueId"]);
                        objChannelList.ChannelNotificationUniqueId = Convert.ToString(ds.Tables[0].Rows[i]["ChannelNotificationUniqueId"]);
                        objChannelList.EmpId = Convert.ToString(ds.Tables[0].Rows[i]["Empid"]);
                        lstChannelList.Add(objChannelList);
                    }
                }
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    AgoraBL.Models.SendTimesheetNotificationDTO.SendTimesheetNotification objSendTimesheetNotificationDto = new AgoraBL.Models.SendTimesheetNotificationDTO.SendTimesheetNotification();
                    objSendTimesheetNotificationDto.EmpTimesheetDetail = new AgoraBL.Models.SendTimesheetNotificationDTO.EmpTimesheetDetail()
                    {
                        EmpName = empName,
                        EmpID = EmpID.ToString(),
                        ChannelList = lstChannelList,
                        FromDate = tsFrom,
                        ToDate = tsTo,
                        Requested = Requested,
                        EmpTimesheetStatus = EmpTimesheetStatus,
                        UpdateReason = UpdatedReason,
                        UpdateDate = UpdateDate,
                        UpdatedBy = UpdatedBy,
                        UniqueId = UniqueId,
                    };
                    List<AgoraBL.Models.SendTimesheetNotificationDTO.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendTimesheetNotificationDTO.HRIdList>();

                    if (ds != null && ds.Tables[2].Rows.Count > 0)
                    {
                        var hrGroups = ds.Tables[2].AsEnumerable()
                                              .GroupBy(row => new
                                              {
                                                  EmpId = row["Empid"].ToString(),
                                                  HRName = row["empName"].ToString()
                                              });

                        foreach (var hrGroup in hrGroups)
                        {
                            List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList>();

                            foreach (var row in hrGroup)
                            {
                                AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList objChannelLists = new AgoraBL.Models.SendTimesheetNotificationDTO.ChannelList
                                {
                                    ChannelName = row["ChannelName"].ToString(),
                                    ChannelUniqueId = row["ChannelUniqueId"].ToString(),
                                    ChannelNotificationUniqueId = row["ChannelNotificationUniqueId"].ToString(),
                                    EmpId = row["Empid"].ToString()
                                };

                                objLstChannelList.Add(objChannelLists);
                            }

                            objLstChannelList = objLstChannelList.Distinct().ToList();

                            AgoraBL.Models.SendTimesheetNotificationDTO.HRIdList objChannelList = new AgoraBL.Models.SendTimesheetNotificationDTO.HRIdList
                            {
                                EmpId = hrGroup.Key.EmpId,
                                HRName = hrGroup.Key.HRName,
                                ChannelList = objLstChannelList
                            };

                            lstHRIdList.Add(objChannelList);
                        }
                    }
                    objSendTimesheetNotificationDto.HRIdList = lstHRIdList;
                    var notificationObject = await Task.Run(() => SendTimesheetNotificationToAI(objSendTimesheetNotificationDto));
                }
            }
        }
    }
}