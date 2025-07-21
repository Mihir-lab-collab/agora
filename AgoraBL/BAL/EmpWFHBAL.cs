using AgoraBL.DAL;
using AgoraBL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.BAL
{
    public class EmpWFHBAL
    {
        //private readonly EmployeeMasterBAL _data;
        string AIPushNotificationAPIBaseURL = ConfigurationManager.AppSettings.Get("AIPushNotificationAPIBaseURL").ToString();
        string AIPushNotificationAPIAuthHeader = ConfigurationManager.AppSettings.Get("AIPushNotificationAPIAuthHeader").ToString();
        string AIChannels = ConfigurationManager.AppSettings.Get("AIChannels").ToString();

        public int ID { get; set; }
        public string WFHFrom { get; set; }
        public string WFHTo { get; set; }
        public string WFHDesc { get; set; }
        public string WFHStatus { get; set; }
        public string AdminComments { get; set; }
        public int WFHCount { get; set; }
        public int EmpID { get; set; }
        public int SkillId { get; set; }
        public int Year { get; set; }
        public string WFHSanctionOn { get; set; }
        public string WFHSanctionBy { get; set; }
        public string Reason { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DataSet SaveWFH(int EmpID, string WFHFrom, string WFHTo, string Reason, int WFHCount)
        {
            EmpWFHBAL obj = new EmpWFHBAL();
            obj.WFHFrom = WFHFrom;
            obj.WFHTo = WFHTo;
            obj.WFHDesc = Reason;
            obj.WFHCount = WFHCount;
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            DataSet ds = new DataSet();
            ds = objWFHDAL.SaveWFH("SAVE", EmpID, obj);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //Added for send wfh notification through AI                
                //SendWFHNotificationToHrThroughAI("WFHAdd", EmpID, WFHFrom, Reason, WFHTo, ds, EmpName: string.Empty, EmpWFHStatus: "Pending", UpdateDate: "", UpdatedBy: "");
                //Task.Run(() => SendWFHNotificationToHrThroughAI("WFHAdd", EmpID, WFHFrom, Reason, WFHTo, ds, EmpName: string.Empty, EmpWFHStatus: "Pending", UpdateDate: "", UpdatedBy: ""));
                //Ended
            }
            return ds;

        }

        public int DeleteWFHById(int empLeaveID)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.DeleteWFH("DELETE", empLeaveID);
        }

        public static bool IfExistsWFH(string mode, int empid, string WFHFrom, string WFHTo)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.IfExistsWFH(mode, empid, WFHFrom, WFHTo);
        }

        public DataSet GetWFHDetails(int empid, string lStatus = "0", int year = 0)
        {
            if (lStatus == "0")
                return GetWFHList("WFHDETAILS", empid, lStatus, year);
            return GetWFHList("GetWFHStatus", empid, lStatus, year);

        }
        public DataSet AppliedWFHFromTo(int empid, string from, string to, string mode = "")
        {
            if (string.IsNullOrEmpty(mode))
            {
                mode = "AppliedWFHFromTo";
            }
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            from = from.Replace("-", "/").Replace("00:00:00", "").Trim();
            to = to.Replace("-", "/").Replace("00:00:00", "").Trim();
            return objWFHDAL.AppliedWFHFromTo(mode, empid, from, to);
        }
        private DataSet GetWFHList(string mode, int empid, string lStatus = "0", int year = 0)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.GetWFHList(mode, empid, lStatus, year);//LeaveMonth);
        }
        public int InsertWFHAttendance(int empId)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.InsertWFHAttendance("InsertAttendance", empId);
        }

        public int InsertRAAttendance(int empId)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.InsertRAAttendance("InsertAttendance", empId);
        }

        public int UpdateRAAttendance(int empId, DateTime attDate)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.UpdateRAAttendance("UpdateAttendance", empId, attDate);
        }

        public bool GetAttendanceByDate(int empId, DateTime attDate)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            int count = objWFHDAL.CheckAttendanceExistence(empId, attDate);
            return count > 0;
        }

        public int UpdateWFHAttendance(int empId, DateTime attOutTime, DateTime attDate)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.UpdateWFHAttendance("UpdateAttendance", empId, attOutTime, attDate);
        }
        public static bool CheckInTime(string mode, int empid)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.CheckInTime(mode, empid);
        }
        public DataTable BindWFHBalance(int empid)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.BindWFHBalance(empid);
        }
        public DataTable GetLeaveStatus(int empId)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.GetLeaveStatus(empId);
        }
        public DataTable GetWFHDetailById(int wfhId)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.GetWFHDetailById(wfhId);
        }
        public static DataTable IfExistsWFHData(string mode, int empid, string WFHFrom, string WFHTo)
        {
            EmpWFHDAL objWFHDAL = new EmpWFHDAL();
            return objWFHDAL.IfExistsWFHData(mode, empid, WFHFrom, WFHTo);
        }
        private async Task<AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification> SendWFHNotificationToAI(AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification sendWFHNotificationDTO)
        {
            var configSecond = ConfigurationManager.AppSettings.Get("ConfigSecond");
            int second = 5;
            if (!string.IsNullOrEmpty(configSecond))
            {
                second = Convert.ToInt32(configSecond);
            }
            Stopwatch stopwatch = Stopwatch.StartNew();

            EmployeeMasterBAL.Delay(TimeSpan.FromSeconds(second));
            stopwatch.Stop();
            AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification finalResponse = new SendWFHNotificationDTO.SendWFHNotification();
            try
            {
                var handler = new HttpClientHandler
                {
                    SslProtocols = SslProtocols.Tls11 | SslProtocols.Tls12
                };

                using (var client = new HttpClient(handler))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, AIPushNotificationAPIBaseURL + "SendWFHNotification");

                    string jsonSerializeObject = JsonConvert.SerializeObject(sendWFHNotificationDTO);

                    var content = new StringContent(jsonSerializeObject, null, "application/json");
                    request.Content = content;
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    request.Headers.Add("Authorization", AIPushNotificationAPIAuthHeader);
                    //var response = client.SendAsync(request).GetAwaiter().GetResult();
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        finalResponse = JsonConvert.DeserializeObject<AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification>(jsonResponse);
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
        public async void SendWFHNotificationToHrThroughAI(string mode, int EmpID, string WFHFrom, string Reason, string WFHTo, DataSet ds, string EmpName, string EmpWFHStatus, string UpdateDate = "", string UpdatedBy = "", string UpdatedReason = "", int WFHId = 0, int noOfDays = 0)
        {
            EmployeeMasterBAL objEmployeeMasterBAL = new EmployeeMasterBAL();

            if (string.Compare(mode, "WFHAdd", true) == 0)
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
                int WFHDays = 0;
                if (string.IsNullOrEmpty(UpdatedBy))
                {
                    WFHDays = Convert.ToInt32(objEmployeeMasterBAL.CalulateLeave(WFHFrom, WFHTo));
                }
                int empWFHId = 0;
                if (ds != null && ds.Tables[2].Rows.Count > 0)
                {
                    empWFHId = Convert.ToInt32(ds.Tables[2].Rows[0]["WFHId"]);
                };
                List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList> lstChannelList = new List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList>();
                if (ds != null && ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        List<string> lstChannels = new List<string>();
                        lstChannels = AIChannels.Split(',').ToList();
                        List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList>();
                        for (int channelcount = 0; channelcount < lstChannels.Count; channelcount++)
                        {
                            if (ds.Tables[3].Columns.Contains(lstChannels[channelcount]))
                            {
                                AgoraBL.Models.SendWFHNotificationDTO.ChannelList objChannelList = new AgoraBL.Models.SendWFHNotificationDTO.ChannelList();
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
                    AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification objSendWFHNotificationDto = new AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification();
                    objSendWFHNotificationDto.EmpWFHDetail = new AgoraBL.Models.SendWFHNotificationDTO.EmpWFHDetail()
                    {
                        EmpName = empName,
                        EmpID = EmpID.ToString(),
                        ChannelList = lstChannelList,
                        FromDate = WFHFrom,
                        ToDate = WFHTo,
                        Reason = Reason,
                        NoOfDays = WFHDays,
                        EmpWFHId = empWFHId,
                        EmpWFHStatus = EmpWFHStatus,
                        UpdateReason = UpdatedReason,
                        UpdateDate = UpdateDate,
                        UpdatedBy = UpdatedBy,
                    };
                    List<AgoraBL.Models.SendWFHNotificationDTO.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendWFHNotificationDTO.HRIdList>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        List<string> lstChannels = new List<string>();
                        lstChannels = AIChannels.Split(',').ToList();
                        List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList>();
                        for (int channelcount = 0; channelcount < lstChannels.Count; channelcount++)
                        {
                            if (ds.Tables[1].Columns.Contains(lstChannels[channelcount]))
                            {
                                AgoraBL.Models.SendWFHNotificationDTO.ChannelList objChannelList = new AgoraBL.Models.SendWFHNotificationDTO.ChannelList();
                                objChannelList.ChannelName = lstChannels[channelcount];
                                objChannelList.ChannelUniqueId = Convert.ToString(ds.Tables[1].Rows[i][lstChannels[channelcount]]);
                                objChannelList.ChannelNotificationUniqueId = null;
                                objChannelList.EmpId = Convert.ToString(ds.Tables[1].Rows[i]["empid"]);
                                objLstChannelList.Add(objChannelList);
                            }
                        }
                        AgoraBL.Models.SendWFHNotificationDTO.HRIdList objHRIdList = new AgoraBL.Models.SendWFHNotificationDTO.HRIdList()
                        {
                            EmpId = Convert.ToString(ds.Tables[1].Rows[i]["empid"]),
                            HRName = Convert.ToString(ds.Tables[1].Rows[i]["empName"]),
                            //ObjectId = Convert.ToString(ds.Tables[1].Rows[0]["MSTeam"]),
                            ChannelList = objLstChannelList,
                        };
                        lstHRIdList.Add(objHRIdList);
                    }
                    objSendWFHNotificationDto.HRIdList = lstHRIdList;

                    //var notificationObject = await Task.Run(() => SendWFHNotificationToAI(objSendWFHNotificationDto));
                    var notificationObject = await SendWFHNotificationToAI(objSendWFHNotificationDto);
                    EmpWFHDAL objEmpWFHDAL = new EmpWFHDAL();
                    if (notificationObject != null && notificationObject.EmpWFHDetail != null)
                    {
                        if (notificationObject.HRIdList != null && notificationObject.HRIdList.Count > 0)
                        {
                            for (int i = 0; i < notificationObject.HRIdList.Count; i++)
                            {
                                if (notificationObject.HRIdList[i].ChannelList != null && notificationObject.HRIdList[i].ChannelList.Count > 0)
                                {
                                    for (int j = 0; j < notificationObject.HRIdList[i].ChannelList.Count; j++)
                                    {
                                        objEmpWFHDAL.AddWFHNotification(notificationObject.EmpWFHDetail.EmpWFHId, notificationObject.HRIdList[i].ChannelList[j].EmpId,
                                            notificationObject.HRIdList[i].ChannelList[j].ChannelName, notificationObject.HRIdList[i].ChannelList[j].ChannelUniqueId,
                                            notificationObject.HRIdList[i].ChannelList[j].ChannelNotificationUniqueId);
                                    }
                                }
                            }
                        }

                        if (notificationObject.EmpWFHDetail.ChannelList != null && notificationObject.EmpWFHDetail.ChannelList.Count > 0)
                        {
                            for (int i = 0; i < notificationObject.EmpWFHDetail.ChannelList.Count; i++)
                            {
                                objEmpWFHDAL.AddWFHNotification(notificationObject.EmpWFHDetail.EmpWFHId, notificationObject.EmpWFHDetail.ChannelList[i].EmpId,
                                    notificationObject.EmpWFHDetail.ChannelList[i].ChannelName, notificationObject.EmpWFHDetail.ChannelList[i].ChannelUniqueId,
                                    notificationObject.EmpWFHDetail.ChannelList[i].ChannelNotificationUniqueId);

                            }
                        }
                    }
                }
            }
            else if (string.Compare(mode, "WFHUpdate", true) == 0)
            {
                if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    EmpName = Convert.ToString(ds.Tables[1].Rows[0]["empName"]);
                }
                int WFHDays = 0;
                if (!string.IsNullOrEmpty(UpdatedBy))
                {
                    WFHDays = Convert.ToInt32(objEmployeeMasterBAL.CalulateLeave(WFHFrom, WFHTo));
                }
                List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList> lstChannelList = new List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList>();
                if (ds != null && ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        AgoraBL.Models.SendWFHNotificationDTO.ChannelList objChannelList = new AgoraBL.Models.SendWFHNotificationDTO.ChannelList();
                        objChannelList.ChannelName = Convert.ToString(ds.Tables[3].Rows[i]["ChannelName"]);
                        objChannelList.ChannelUniqueId = Convert.ToString(ds.Tables[3].Rows[i]["ChannelUniqueId"]);
                        objChannelList.ChannelNotificationUniqueId = Convert.ToString(ds.Tables[3].Rows[i]["ChannelNotificationUniqueId"]);
                        objChannelList.EmpId = Convert.ToString(ds.Tables[3].Rows[i]["Empid"]);
                        lstChannelList.Add(objChannelList);
                    }
                    lstChannelList = lstChannelList.Distinct().ToList();
                }
                AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification objSendWFHNotificationDto = new AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification();
                objSendWFHNotificationDto.EmpWFHDetail = new AgoraBL.Models.SendWFHNotificationDTO.EmpWFHDetail()
                {
                    EmpName = EmpName,
                    EmpID = EmpID.ToString(),
                    ChannelList = lstChannelList,
                    FromDate = WFHFrom,
                    ToDate = WFHTo,
                    Reason = Reason,
                    NoOfDays = WFHDays,
                    EmpWFHId = WFHId,
                    EmpWFHStatus = EmpWFHStatus,
                    UpdateReason = UpdatedReason,
                    UpdateDate = UpdateDate,
                    UpdatedBy = UpdatedBy,
                };
                List<AgoraBL.Models.SendWFHNotificationDTO.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendWFHNotificationDTO.HRIdList>();

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
                        List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendWFHNotificationDTO.ChannelList>();

                        foreach (var row in hrGroup)
                        {
                            AgoraBL.Models.SendWFHNotificationDTO.ChannelList objChannelList = new AgoraBL.Models.SendWFHNotificationDTO.ChannelList
                            {
                                ChannelName = row["ChannelName"].ToString(),
                                ChannelUniqueId = row["ChannelUniqueId"].ToString(),
                                ChannelNotificationUniqueId = row["ChannelNotificationUniqueId"].ToString(),
                                EmpId = row["Empid"].ToString()
                            };

                            objLstChannelList.Add(objChannelList);
                        }

                        objLstChannelList = objLstChannelList.Distinct().ToList();

                        AgoraBL.Models.SendWFHNotificationDTO.HRIdList objHRIdList = new AgoraBL.Models.SendWFHNotificationDTO.HRIdList
                        {
                            EmpId = hrGroup.Key.EmpId,
                            HRName = hrGroup.Key.HRName,
                            ChannelList = objLstChannelList
                        };

                        lstHRIdList.Add(objHRIdList);
                    }
                }
                objSendWFHNotificationDto.HRIdList = lstHRIdList;
                var notificationObject = await Task.Run(() => SendWFHNotificationToAI(objSendWFHNotificationDto));
            }

        }
        public void SendMail(DataTable dt, string strDateFrom, string strDateTo, string strReason, out bool IsEmailSent)
        {
            IsEmailSent = false;
            if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(strDateFrom) && !string.IsNullOrEmpty(strDateTo))
            {
                string[] DateFrom = strDateFrom.Split('/');
                string New_FromDt = DateFrom[1] + '/' + DateFrom[0] + '/' + DateFrom[2];
                DateTime dtFrom = Convert.ToDateTime(New_FromDt.ToString());
                string[] DateTo = strDateTo.Split('/');
                string New_ToDt = DateTo[1] + '/' + DateTo[0] + '/' + DateTo[2];
                DateTime dtTo = Convert.ToDateTime(New_ToDt.ToString());
                string strBody, strSubject, mailTo, mailFrom, message, CC = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // this if conditions applay to avoid duplicate emailId under CC in mail
                    if (CC.Contains(dt.Rows[i]["ProjMangerEmail"].ToString()))
                    {

                    }
                    else
                    {
                        CC = CC + dt.Rows[i]["ProjMangerEmail"].ToString();
                        CC += (i < dt.Rows.Count) ? ";" : string.Empty;
                    }
                    if (CC.Contains(dt.Rows[i]["BAEmail"].ToString()))
                    {
                    }
                    else
                    {
                        CC = CC + dt.Rows[i]["BAEmail"].ToString();
                        CC += (i < dt.Rows.Count) ? ";" : string.Empty;
                    }

                }
                if (CC.Contains(dt.Rows[0]["empEmail"].ToString()))
                {
                }
                else
                {
                    CC += dt.Rows[0]["empEmail"].ToString();
                }
                string finalCC = string.Empty;
                if (CC.Length > 0)
                {
                    var distinctCC = CC.Split(',').Select(email => email.Trim()).Distinct();
                    finalCC = string.Join(", ", distinctCC);
                }
                EmployeeMasterDAL objEmployeeMasterDAL = new EmployeeMasterDAL();

                strBody = dt.Rows[0]["empName"] + " " + "has applied for Work From Home " + "<b>From</b> " + ": " +
                    Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy") + " " + "<b>TO</b> " + ": " +
                    Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy") + " " + "<br> Reason for Work from Home is " + "<b>: </b>" + strReason;
                //strSubject = dt.Rows[0]["empName"].ToString() + " has applied for Work From Home.";
                strSubject = "Request to Work From Home";
                mailTo = ConfigurationManager.AppSettings["HREmail"];
                mailFrom = dt.Rows[0]["empEmail"].ToString();
                message = objEmployeeMasterDAL.SendMail(strBody, strSubject, mailTo, mailFrom, true, finalCC, "",out IsEmailSent);
            }
        }
    }
    public class EmployeeWFHDetails
    {
        public WorkFromHomeDetails WFHDetails { get; set; }
        public List<EmpWFHBAL> WFHListData { get; set; }
    }
    public class EmpWFHAttendance
    {
        public string AttDate { get; set; }
        public string AttInTime { get; set; }
        public string AttOutTime { get; set; }
        public string Day { get; set; }
        public string Status { get; set; }
    }
    public class WorkFromHomeDetails
    {
        public int TotalAnnual { get; set; }
        public int TotalTillCurrentDate { get; set; }
        public int Consumed { get; set; }
        public int Balance { get; set; }
    }
}
