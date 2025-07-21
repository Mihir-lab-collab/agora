using AgoraBL.BAL;
using AgoraBL.Common;
using AgoraBL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
//using static AgoraBL.Models.SendLeaveNotification;

namespace AI.API.Controllers
{
    [RoutePrefix("api/AI")]
    public class AIController : ApiController
    {
        private readonly EmployeeMasterBAL _data;
        private readonly EmpLeaveApprovalBAL _bal;


        #region WebConfig Details
        private string RecipientIdValue = string.IsNullOrEmpty(ConfigurationManager.AppSettings["RecipientId"].ToString()) ? string.Empty : ConfigurationManager.AppSettings["RecipientId"].ToString();
        private string RecipientNameValue = string.IsNullOrEmpty(ConfigurationManager.AppSettings["RecipientName"].ToString()) ? string.Empty : ConfigurationManager.AppSettings["RecipientName"].ToString();
        private string SenderApplicationValue = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SenderApplication"].ToString()) ? string.Empty : ConfigurationManager.AppSettings["SenderApplication"].ToString();
        #endregion

        #region Constructor
        public AIController()
        {
            _data = new EmployeeMasterBAL();
        }
        #endregion

        #region Login Endpoint

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] UserIdentityData userIdentityData)
        {
            try
            {
                if (ValidateHeader())
                {
                    var objUserData = _data.GetEmpDetailsbyId(userIdentityData);
                    if (objUserData.IsSuccess && objUserData.StatusMessage == "Success")
                    {
                        return Ok(objUserData);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpPost]
        [Route("Login-Common")]
        public async Task<IHttpActionResult> ADLogin([FromBody] ClsLogin request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request cannot be null.");
                }

                if (string.IsNullOrEmpty(request.EmpId) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Employee ID and Password cannot be empty.");
                }

                var user = EmployeeMasterBAL.EmployeeLogin(request.EmpId, request.Password, (LoginType)request.Logintype);
                if (user == null)
                {
                    return Content(HttpStatusCode.Unauthorized, "Invalid login credentials or login type.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("An internal server error occurred."));
            }
        }
        #endregion

        #region Employee Details Endpoint
        [HttpPost]
        [Route("Employee-Details")]
        public IHttpActionResult EmployeeDetails([FromBody] UserIdentityData userIdentityData)
        {
            try
            {
                if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString()))
                {
                    if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                    {
                        var projectList = _data.GetEmployeeDetails(Convert.ToInt32(userIdentityData.EmpID));
                        return Ok(projectList);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest("Employee ID and Skill ID are required.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        #endregion

        #region HR-Related EndPoints
        [HttpPost]
        [Route("HR-Employee")]
        public IHttpActionResult GetEmployeeDetailsByHR([FromBody] Entity entity)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckAdmin(objCheckAuthentication.EmpId))
                    {
                        if (!string.IsNullOrEmpty(entity.EntityName))
                        {
                            var empDetails = _data.GetEmployeeDetailsByHR(entity.EntityName);
                            return Ok(empDetails);
                        }
                        var message = ClsCommon.CheckRequiredField(entity);
                        return BadRequest(message);
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpPost]
        [Route("HR-Timesheet")]
        public IHttpActionResult GetTimesheetDetailsByHR([FromBody] Entity entity)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckIsPMBAAM(objCheckAuthentication.EmpId))
                    {
                        if (entity.DurationFrom != null && entity.DurationTo != null)
                        {
                            if (string.IsNullOrEmpty(entity.EntityName) && string.IsNullOrEmpty(entity.Project) )
                            {
                                return BadRequest("EntityName or Project are required.");
                            }
                            var timesheetDetails = _data.GetTimesheetDetailsByHR(entity);
                            return Ok(timesheetDetails);
                        }
                        return BadRequest("DurationFrom and DurationTo are required.");
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("HR-Pending-Leave")]
        public IHttpActionResult GetPendingLeaveStatus([FromBody] Entity entity)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckAdmin(objCheckAuthentication.EmpId))
                    {
                        {
                            var PendingLeaveStatus = _data.GetHRPendingLeave(entity);
                            return Ok(PendingLeaveStatus);
                        }
                        var message = ClsCommon.CheckRequiredField(entity);
                        return BadRequest(message);
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("HR-Leave")]
        public IHttpActionResult GetLeaveDetailsByHR([FromBody] Entity entity)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckAdmin(objCheckAuthentication.EmpId))
                    {
                        if (entity.DurationFrom != null && entity.DurationTo != null)
                        {
                            var leaveDetails = _data.GetLeaveDetailsByHR(entity);
                            return Ok(leaveDetails);
                        }
                        return BadRequest("DurationFrom and DurationTo are required.");
                    }
                    return Unauthorized();
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpPost]
        [Route("RoleWise-Timesheet-TotalHours")]
        public IHttpActionResult GetTimesheetDetailsTotalHoursByRole([FromBody] Entity entity)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    entity.EntityID = objCheckAuthentication.EmpId;
                    if (string.IsNullOrEmpty(entity.EntityName) && string.IsNullOrEmpty(entity.Project))
                    {
                        return BadRequest("Project and EntityName are required.");
                    }
                    var tsDetails = _data.GetTimesheetDetailsTotalHoursByRole(entity);
                    if (tsDetails.Count>0 && !tsDetails[0].IsAccessible)
                    {
                        return Unauthorized();
                    }
                    return Ok(tsDetails);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("HR-Pending-Timesheet")]
        public IHttpActionResult GetPendingTimesheetByHR([FromBody] Entity entity)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckAdmin(objCheckAuthentication.EmpId))
                    {
                        if (!string.IsNullOrEmpty(entity.EntityName))
                        {
                            var leaveDetails = _data.GetPendingTimesheetByHR(entity);
                            return Ok(leaveDetails);
                        }
                        var message = ClsCommon.CheckRequiredField(entity);
                        return BadRequest(message);
                    }
                    return Unauthorized();
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("HR-UpdateLeaveStatus")]
        public IHttpActionResult UpdateLeaveStatus([FromBody] EmpLeaveApprovalBAL objEmpLeaveApprovalBAL)
        {
            try
            {
                AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto objSendLeaveNotificationDto = new AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto();
                AgoraBL.Models.SendLeaveNotification objSendLeaveNotification = new AgoraBL.Models.SendLeaveNotification();
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckAdmin(objCheckAuthentication.EmpId))
                    {
                        EmpLeaveApprovalBAL _objEmpLeaveApprovalBAL = new EmpLeaveApprovalBAL();
                        var dsEmpLeave = _objEmpLeaveApprovalBAL.UpdateEmpLeaveStatus("UpdateStatus", objEmpLeaveApprovalBAL.EmpID, objEmpLeaveApprovalBAL.EmpLeaveID,
                            objEmpLeaveApprovalBAL.LeaveStatus, objEmpLeaveApprovalBAL.LeaveType, objEmpLeaveApprovalBAL.AdminComment, objEmpLeaveApprovalBAL.LeaveSanctionBy, objEmpLeaveApprovalBAL.LeaveFrom, objEmpLeaveApprovalBAL.LeaveTo);
                        if (dsEmpLeave != null && dsEmpLeave.Tables[0].Rows.Count > 0)
                        {
                            //string AIChannels = ConfigurationManager.AppSettings.Get("AIChannels").ToString();
                            //if (dsEmpLeave != null && dsEmpLeave.Tables[1].Rows.Count > 0)
                            //{
                            //string leavestatus = objEmpLeaveApprovalBAL.LeaveStatus;
                            //if (!string.IsNullOrEmpty(leavestatus))
                            //{
                            //    leavestatus = string.Compare(objEmpLeaveApprovalBAL.LeaveStatus, "Approved", true) == 0 ? "a" : "r";
                            //}
                            //     objSendLeaveNotification = new SendLeaveNotification();
                            //    objSendLeaveNotificationDto.EmpLeaveDetail = new AgoraBL.Models.SendLeaveNotification.EmpLeaveDetail()
                            //    {
                            //        EmpID = objEmpLeaveApprovalBAL.EmpID.ToString(),
                            //        EmpLeaveId = Convert.ToInt32(objEmpLeaveApprovalBAL.EmpLeaveID),
                            //        EmpName = objEmpLeaveApprovalBAL.EmpName,
                            //        FromDate = objEmpLeaveApprovalBAL.LeaveFrom,
                            //        LeaveType = objEmpLeaveApprovalBAL.LeaveType,
                            //        Reason = objEmpLeaveApprovalBAL.LeaveReason,
                            //        ToDate = objEmpLeaveApprovalBAL.LeaveTo,
                            //        EmpLeaveStatus = leavestatus,
                            //        UpdateDate = DateTime.UtcNow.ToString(),
                            //        UpdatedBy = string.Empty
                            //    };
                            //    List<AgoraBL.Models.SendLeaveNotification.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendLeaveNotification.HRIdList>();
                            //    for (int i = 0; i < dsEmpLeave.Tables[1].Rows.Count; i++)
                            //    {
                            //        List<string> lstChannels = new List<string>();
                            //        lstChannels = AIChannels.Split(',').ToList();
                            //        List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                            //        for (int channelcount = 0; channelcount < lstChannels.Count; channelcount++)
                            //        {
                            //            if (dsEmpLeave.Tables[1].Columns.Contains(lstChannels[channelcount]))
                            //            {
                            //                AgoraBL.Models.SendLeaveNotification.ChannelList objChannelList = new AgoraBL.Models.SendLeaveNotification.ChannelList();
                            //                objChannelList.ChannelName = lstChannels[channelcount];
                            //                objChannelList.ChannelUniqueId = Convert.ToString(dsEmpLeave.Tables[1].Rows[i][lstChannels[channelcount]]);
                            //                objChannelList.ChannelNotificationUniqueId = null;
                            //                objChannelList.EmpId = Convert.ToString(dsEmpLeave.Tables[1].Rows[i]["empid"]);
                            //                objLstChannelList.Add(objChannelList);
                            //            }
                            //        }
                            //        AgoraBL.Models.SendLeaveNotification.HRIdList objHRIdList = new AgoraBL.Models.SendLeaveNotification.HRIdList()
                            //        {
                            //            EmpId = Convert.ToString(dsEmpLeave.Tables[1].Rows[i]["empid"]),
                            //            HRName = Convert.ToString(dsEmpLeave.Tables[1].Rows[i]["empName"]),
                            //            //ObjectId = Convert.ToString(ds.Tables[1].Rows[0]["MSTeam"]),
                            //            ChannelList = objLstChannelList,
                            //        };
                            //        lstHRIdList.Add(objHRIdList);
                            //    }
                            //    objSendLeaveNotificationDto.HRIdList = lstHRIdList;
                            DataColumnCollection columns = dsEmpLeave.Tables[0].Columns;
                            if (columns.Contains("LeaveUpdated"))
                            {
                                objSendLeaveNotification.Message = "Leave already updated";
                                objSendLeaveNotification.Status = "1";
                            }
                            else if (string.Compare(objEmpLeaveApprovalBAL.LeaveStatus, "Approved", true) == 0)
                            {
                                objSendLeaveNotification.Message = "Approved";
                                objSendLeaveNotification.Status = "1";
                            }
                            else
                            {
                                objSendLeaveNotification.Message = "Rejected";
                                objSendLeaveNotification.Status = "1";
                            }

                            //}
                            return Ok(objSendLeaveNotification);

                        }
                        else
                        {
                            return Ok("Failed! to update Leave");
                        }
                    }
                    return Unauthorized();
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("HR-Timesheet-Request-Update")]
        public IHttpActionResult HRTimesheetRequestStatus([FromBody] SendTimesheetNotificationDTO.EmployeeTimesheetRequest emp)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckAdmin(objCheckAuthentication.EmpId))
                    {
                        if (!string.IsNullOrEmpty(emp.EmpID.ToString()) && !string.IsNullOrEmpty(emp.UniqueId.ToString())
                                        && !string.IsNullOrEmpty(emp.UpdatedBy.ToString()) && !string.IsNullOrEmpty(emp.EmpTimesheetStatus.ToString()))
                        {
                            EmpTimesheetBAL.EmployeeTimesheetRequestUpdate(emp);
                            return Ok("Request sent successfully.");
                        }
                        return BadRequest("EmpId, UniqueId, EmpTimesheetStatus and UpdatedBy are required.");
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("ProjectList")]
        public IHttpActionResult GetProjectListByEmpId()
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    ProjectBAL objProjectBAL = new ProjectBAL();
                    var data=objProjectBAL.GetProjectByRole(objCheckAuthentication.EmpId, "CheckAuth");
                    if (data.Count > 0 && !data[0].IsAccessible)
                    {
                        return Unauthorized();
                    }
                    return Ok(data);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpPost]
        [Route("Add-Project-Member")]
        public IHttpActionResult AddProjectMember([FromBody] AddProjectMember addProjectMember)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (!ProjectBAL.CheckAuth(objCheckAuthentication.EmpId, "CheckAuth"))
                    {
                        return Unauthorized();
                    }
                    if (addProjectMember.ProjId > 0 && !string.IsNullOrEmpty(addProjectMember.EmpId))
                    {
                        ProjectBAL.DeleteprojectMemberByprojid(addProjectMember.ProjId, "DeleteMemberByProjId");
                        int count = 0;
                        var noOfEmpId=addProjectMember.EmpId.Split(',');
                        for (int i = 0; i <= noOfEmpId.Length - 1; i++)
                        {
                            if (noOfEmpId[i].ToString().Trim() != "")
                            {
                                count += ProjectBAL.InsertprojectMember(addProjectMember.ProjId, Convert.ToInt32(noOfEmpId[i]), "InsertMember");
                            }
                        }
                        if (count > 0)
                        {
                            return Ok("Member(s) inserted successfully.");
                        }
                        else
                        {
                            return BadRequest("Something went wrong. Please try again!");
                        }
                    }
                    else
                    {
                        return BadRequest("Project Id and Employee Id are required.");
                    } 
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Remove-Project-Member")]
        public IHttpActionResult RemoveProjectMember([FromBody] RemoveProjectMember removeProjectMember)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (!ProjectBAL.CheckAuth(objCheckAuthentication.EmpId, "CheckAuth"))
                    {
                        return Unauthorized();
                    }
                    if (string.Compare(removeProjectMember.Mode, "All", true) == 0)
                    {
                        removeProjectMember.EmpId = "0";
                    }
                    if (removeProjectMember.ProjId > 0 && !string.IsNullOrEmpty(removeProjectMember.EmpId) && !string.IsNullOrEmpty(removeProjectMember.Mode))
                    {
                        if (string.Compare(removeProjectMember.Mode, "All", true) == 0)
                        {
                            ProjectBAL.DeleteprojectMemberByprojid(removeProjectMember.ProjId, "DeleteMemberByProjId");
                            return Ok("All members removed successfully.");
                        }
                        else if (string.Compare(removeProjectMember.Mode, "Select", true) == 0)
                        {
                            int count = 0;
                            var noOfEmpId = removeProjectMember.EmpId.Split(',');
                            for (int i = 0; i <= noOfEmpId.Length - 1; i++)
                            {
                                if (noOfEmpId[i].ToString().Trim() != "")
                                {
                                    count += ProjectBAL.DeleteprojectMemberByprojid(removeProjectMember.ProjId, "DeleteMemberByEmpId",Convert.ToInt32(noOfEmpId[i]));
                                }
                            }
                            if (count>0)
                            {
                                return Ok("Selected members removed successfully.");
                            }
                            else
                            {
                                return BadRequest("Something went wrong. Please try again!");
                            }
                        }
                        else
                        {
                            return BadRequest("Invalid mode. Please select a proper mode.");
                        }
                    }
                    else
                    {
                        return BadRequest("Project Id, Employee Id, and Mode are required.");
                    } 
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("GetEmployee")]
        public IHttpActionResult GetEmpDetailsByEntityName([FromBody] Entity entity)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (ProjectBAL.CheckAuth(objCheckAuthentication.EmpId, "CheckAuth"))
                    {
                        if (!string.IsNullOrEmpty(entity.EntityName))
                        {
                            var data = ProjectBAL.GetEmpDetailsByEntityName(entity.EntityName, "GetEmpDetail");
                            return Ok(data);
                        }
                        else
                        {
                            return BadRequest("EntityName are required.");
                        } 
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Update-ProjectStackHolder")]
        public IHttpActionResult UpdateProjectMember([FromBody] List<UpdateProjectStackHolder> objPSH)
        {
            try
            {
                ProjectStackHolderDTO objProjectStackHolderDTO = new ProjectStackHolderDTO();
                List<ProjectStackHolderDTO> lstProjectStackHolderDTO = new List<ProjectStackHolderDTO>();
                CheckAuthentication objCheckAuthentication = ValidateToken();

                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (objPSH != null && objPSH.Count > 0)
                    {
                        foreach (var psh in objPSH)
                        {
                            if (psh.EmpId== 0 || psh.ProjId== 0 || string.IsNullOrEmpty(psh.Role))
                            {
                                objProjectStackHolderDTO = new ProjectStackHolderDTO()
                                {
                                    Message = "Project Id,EmpId and Role are required.",
                                    IsSucess = false
                                };
                                lstProjectStackHolderDTO.Add(objProjectStackHolderDTO);
                                continue;
                            }
                            var objProject = ProjectBAL.GetProjectStackHolder(psh.ProjId, psh.Role,psh.EmpId);
                            var result=ProjectBAL.UpdateProjectStackHolder(psh.ProjId, psh.Role, psh.EmpId, objCheckAuthentication.EmpId);

                            if (result>0)
                            {
                                objProjectStackHolderDTO = new ProjectStackHolderDTO()
                                {
                                    OldEmpId=objProject.OldEmpId,
                                    OldEmpName=objProject.OldEmpName,
                                    NewEmpId=objProject.NewEmpId,
                                    NewEmpName=objProject.NewEmpName,
                                    ProjId=objProject.ProjId,
                                    ProjName=objProject.ProjName,
                                    Role=objProject.Role,
                                    Message=objProject.Role + " updated Successfully",
                                    IsSucess=true
                                };
                                lstProjectStackHolderDTO.Add(objProjectStackHolderDTO);

                            }
                            else
                            {
                                objProjectStackHolderDTO = new ProjectStackHolderDTO()
                                {
                                    OldEmpId = objProject.OldEmpId,
                                    OldEmpName = objProject.OldEmpName,
                                    NewEmpId = objProject.NewEmpId,
                                    NewEmpName = objProject.NewEmpName,
                                    ProjId = objProject.ProjId,
                                    ProjName = objProject.ProjName,
                                    Role = objProject.Role,
                                    Message = objProject.Role + " Update failed.",
                                    IsSucess = false
                                };
                                lstProjectStackHolderDTO.Add(objProjectStackHolderDTO);
                            }
                        }

                        return Ok(lstProjectStackHolderDTO);
                    }
                    else
                    {
                        return BadRequest("No project stakeholders found.");
                    }
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("HR-Incomplete-Timesheets")]
        public IHttpActionResult HRIncompleteTimesheets([FromBody] IncompleteTimesheet objIncompleteTimesheet)
        {
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckAdmin(objCheckAuthentication.EmpId))
                    {
                        if (objIncompleteTimesheet != null && !string.IsNullOrEmpty(objIncompleteTimesheet.FromDate) && !string.IsNullOrEmpty(objIncompleteTimesheet.ToDate) )
                        {
                            var data = EmpTimesheetBAL.GetIncompleteTimesheet(objIncompleteTimesheet.EntityName, objIncompleteTimesheet.FromDate, objIncompleteTimesheet.ToDate);
                            return Ok(data);
                        }
                        else
                        {
                            return BadRequest("FromDate and ToDate are required.");
                        }
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("HR-Provide-Timesheet-Access")]
        public IHttpActionResult HRProvideTimesheetAccess([FromBody] List<ProvideTimesheetAccess> TSAccess)
        {
            ProvideTimesheetAccess objProvideTimesheetAccess = new ProvideTimesheetAccess();
            List<ProvideTimesheetAccess> lstProvideTimesheetAccess = new List<ProvideTimesheetAccess>();
            try
            {
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckAdmin(objCheckAuthentication.EmpId))
                    {
                        if (TSAccess != null && TSAccess.Count > 0)
                        {
                            foreach (var TS in TSAccess)
                            {
                                if (TS.EmpId == 0 || string.IsNullOrEmpty(TS.Description))
                                {
                                    objProvideTimesheetAccess = new ProvideTimesheetAccess()
                                    {
                                        Message = "EmpId and Description are required.",
                                        IsSucess = false
                                    };
                                    lstProvideTimesheetAccess.Add(objProvideTimesheetAccess);
                                    continue;
                                }
                                var result = EmpTimesheetBAL.SaveEmployeeTimesheetRequest(TS.EmpId, DateTime.Now.Date, TS.Description, objCheckAuthentication.EmpId);
                                if (result > 0)
                                {
                                    objProvideTimesheetAccess = new ProvideTimesheetAccess()
                                    {
                                        EmpId=TS.EmpId,
                                        Description= TS.Description,
                                        Message = "Timesheet access has been granted successfully.",
                                        IsSucess = true
                                    };
                                    lstProvideTimesheetAccess.Add(objProvideTimesheetAccess);
                                }
                                else
                                {
                                    objProvideTimesheetAccess = new ProvideTimesheetAccess()
                                    {
                                        EmpId = TS.EmpId,
                                        Description = TS.Description,
                                        Message = "Failed to grant timesheet access.",
                                        IsSucess = false
                                    };
                                    lstProvideTimesheetAccess.Add(objProvideTimesheetAccess);
                                }
                            }
                            return Ok(lstProvideTimesheetAccess);
                        }
                        else
                        {
                            return BadRequest("EmpId and Description are required.");   
                        }
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("HR-UpdateWFHStatus")]
        public IHttpActionResult UpdateWFHStatus([FromBody] EmpWFHApprovalBAL objEmpWFHApprovalBAL)
        {
            try
            {
                AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification objSendWFHNotificationDto = new AgoraBL.Models.SendWFHNotificationDTO.SendWFHNotification();
                AgoraBL.Models.SendWFHNotificationDTO objSendWFHNotification = new AgoraBL.Models.SendWFHNotificationDTO();
                CheckAuthentication objCheckAuthentication = ValidateToken();
                if (ValidateHeader() && objCheckAuthentication.CheckToken)
                {
                    if (_data.CheckAdmin(objCheckAuthentication.EmpId))
                    {
                        EmpWFHApprovalBAL _objEmpWFHApprovalBAL = new EmpWFHApprovalBAL();
                        var dsEmpWFH = _objEmpWFHApprovalBAL.UpdateEmpWFHStatus("UpdateStatus", objEmpWFHApprovalBAL.EmpID, objEmpWFHApprovalBAL.EmpWFHID,
                            objEmpWFHApprovalBAL.WFHStatus, objEmpWFHApprovalBAL.AdminComment, objEmpWFHApprovalBAL.WFHSanctionBy, objEmpWFHApprovalBAL.WFHFrom, objEmpWFHApprovalBAL.WFHTo);
                        if (dsEmpWFH != null && dsEmpWFH.Tables[0].Rows.Count > 0)
                        {
                            DataColumnCollection columns = dsEmpWFH.Tables[0].Columns;
                            if (columns.Contains("WFHUpdated"))
                            {
                                objSendWFHNotification.Message = "WFH already updated";
                                objSendWFHNotification.Status = "1";
                            }
                            else if (string.Compare(_objEmpWFHApprovalBAL.WFHStatus, "Approved", true) == 0)
                            {
                                objSendWFHNotification.Message = "Approved";
                                objSendWFHNotification.Status = "1";
                            }
                            else
                            {
                                objSendWFHNotification.Message = "Rejected";
                                objSendWFHNotification.Status = "1";
                            }
                            return Ok(objSendWFHNotification);

                        }
                        else
                        {
                            return Ok("Failed! to update WFH");
                        }
                    }
                    return Unauthorized();
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            
        }
        #endregion

        #region Employee-Related Endpoints
        [HttpPost]
        [Route("Employee-Timesheet")]
        public IHttpActionResult GetTimesheetDetails([FromBody] UserIdentityData userIdentityData)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString()))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(userIdentityData.DurationFrom)) && !string.IsNullOrEmpty(Convert.ToString(userIdentityData.DurationTo)))
                        {
                            var timesheetList = _data.GetTimesheetDetails(userIdentityData);
                            return Ok(timesheetList);
                        }
                        else
                        {
                            return BadRequest("From Date and To Date are required.");
                        }
                    }
                    return BadRequest("Employee ID and Skill ID are required.");
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpPost]
        [Route("Employee-Leave")]
        public IHttpActionResult GetLeaveDetails([FromBody] UserIdentityData userIdentityData)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString()))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(userIdentityData.DurationFrom)) && !string.IsNullOrEmpty(Convert.ToString(userIdentityData.DurationTo)))
                        {
                            var leaveList = _data.GetLeaveDetails(userIdentityData);
                            return Ok(leaveList);
                        }
                        else
                        {
                            return BadRequest("From Date and To Date are required.");
                        }
                    }
                    return BadRequest("Employee ID and Skill ID are required.");
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        [HttpPost]
        [Route("Employee-Pending-Leave")]
        public IHttpActionResult GetEmployeePendingLeave([FromBody] UserIdentityData userIdentityData)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString()))
                    {
                        List<Leave> leaveList = _data.GetEmployeePendingLeave(userIdentityData);
                        return Ok(leaveList);
                    }
                    else
                    {
                        return BadRequest("Employee ID and Skill ID are required.");
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        [HttpPost]
        [Route("Employee-Delete-Leave")]
        public IHttpActionResult DeleteLeave([FromBody] UserIdentityData userIdentityData)
        {
            ResponseData objResponseData = new ResponseData();
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString())
                        && userIdentityData.EmpLeaveId > 0)
                    {
                        int outId = _data.DeleteLeave(userIdentityData.EmpLeaveId);
                        if (outId > 0)
                        {
                            objResponseData.Status = "1";
                            objResponseData.Message = "Leave deleted succesfully.";
                        }
                        else
                        {
                            objResponseData.Status = "0";
                            objResponseData.Message = "Leave delete failed.";
                        }
                        return Ok(objResponseData);
                    }
                    else
                    {
                        return BadRequest("Employee ID , Skill ID and Leave-Id are required.");
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }







        [HttpPost]
        [Route("Employee-Timesheet-TotalHours")]
        public IHttpActionResult GetTimesheetDetailsTotalHours([FromBody] UserIdentityData userIdentityData)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString()))
                    {
                        var timesheetDetailsTotalHours = _data.GetTimesheetDetailsTotalHours(userIdentityData);
                        return Ok(timesheetDetailsTotalHours);
                    }
                    return BadRequest("Employee ID and Skill ID are required.");
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpPost]
        [Route("Employee-Demographic")]
        public IHttpActionResult GetDemographicDetail([FromBody] UserIdentityData userIdentityData)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString()))
                    {
                        var demographicDetail = _data.GetDemographicDetail(userIdentityData);
                        return Ok(demographicDetail);
                    }
                    return BadRequest("Employee ID and Skill ID are required.");
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }


        [HttpPost]
        [Route("Employee-Pending-Timesheet")]
        public IHttpActionResult GetPendingTimesheet([FromBody] UserIdentityData userIdentityData)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString()))
                    {
                        var pendingTimesheets = _data.GetPendingTimesheetEmployee(userIdentityData);
                        return Ok(pendingTimesheets);
                    }
                    return BadRequest("Employee ID and Skill ID are required.");
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Employee-doApply-Leave")]
        public IHttpActionResult doApplyLeave([FromBody] UserIdentityData userIdentityData)
        {
            AgoraBL.Models.SendLeaveNotification objSendLeaveNotification = new AgoraBL.Models.SendLeaveNotification();
            //ResponseData objResponseData = new ResponseData();
            DataSet dsEmpLeave = new DataSet();
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString()))
                    {
                        if (_data.IfExistsLeave("IFEXISTSLEAVE", Convert.ToInt32(userIdentityData.EmpID), userIdentityData.LeaveFrom, userIdentityData.LeaveTo))
                        {
                            objSendLeaveNotification.Status = "0";
                            objSendLeaveNotification.Message = "Leave for same date already exists.";
                        }
                        else
                        {
                            userIdentityData.DurationFrom = DateTime.ParseExact(userIdentityData.LeaveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            userIdentityData.DurationTo = DateTime.ParseExact(userIdentityData.LeaveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            ConsolidateLeaves leaveList = _data.GetLeaveDetails(userIdentityData);

                            if (leaveList.Leave_Details.Count() > 0)
                            {
                                DateTime durationFrom = Convert.ToDateTime(userIdentityData.DurationFrom).AddDays(-1);
                                DateTime durationTo = Convert.ToDateTime(userIdentityData.DurationTo);

                                TimeSpan timeSpan = durationTo - durationFrom;
                                int noOfDaysLeaveApplied = Convert.ToInt32(timeSpan.Days.ToString().Replace("-", ""));

                                var message = CheckLeaveBalance(userIdentityData.LeaveType,
                                                               noOfDaysLeaveApplied,
                                                               leaveList.Leave_Details);
                                if (message)
                                {
                                    bool IsEmailSent = false;
                                    dsEmpLeave = _data.SaveLeave(Convert.ToInt32(userIdentityData.EmpID),
                                                             userIdentityData.LeaveType,
                                                             userIdentityData.LeaveFrom,
                                                             userIdentityData.LeaveTo,
                                                             userIdentityData.LeaveReason,
                                                             userIdentityData.EmpName, out IsEmailSent);
                                    //if (dsEmpLeave != null && dsEmpLeave.Tables[0].Rows.Count > 0)
                                    //{
                                    //    //Added for send leave notification through AI
                                    //    //EmployeeMasterBAL objEmployeeMasterBAL = new EmployeeMasterBAL();
                                    //    //objEmployeeMasterBAL.SendLeaveNotificationToHrThroughAI(Convert.ToInt32(userIdentityData.EmpID), userIdentityData.LeaveFrom, userIdentityData.LeaveType,
                                    //    //    userIdentityData.LeaveReason, userIdentityData.LeaveTo, dsEmpLeave,userIdentityData.EmpName);
                                    //    ////Ended
                                    //    //_data.SendMail(dsEmpLeave.Tables[0],
                                    //    //               userIdentityData.LeaveFrom,
                                    //    //               userIdentityData.LeaveTo,
                                    //    //               userIdentityData.LeaveReason);
                                    //    if (dsEmpLeave != null && dsEmpLeave.Tables[0].Rows.Count > 0)
                                    //    {
                                    //        int empLeaveId = 0;
                                    //        if (dsEmpLeave != null && dsEmpLeave.Tables[2].Rows.Count > 0)
                                    //        {
                                    //            empLeaveId = Convert.ToInt32(dsEmpLeave.Tables[2].Rows[0]["LeaveId"]);
                                    //        };
                                    //        string AIChannels = ConfigurationManager.AppSettings.Get("AIChannels").ToString();
                                    //        if (dsEmpLeave != null && dsEmpLeave.Tables[1].Rows.Count > 0)
                                    //        {
                                    //            AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto objSendLeaveNotificationDto = new AgoraBL.Models.SendLeaveNotification.SendLeaveNotificationDto();
                                    //            objSendLeaveNotificationDto.EmpLeaveDetail = new AgoraBL.Models.SendLeaveNotification.EmpLeaveDetail()
                                    //            {
                                    //                EmpID = userIdentityData.EmpID.ToString(),
                                    //                EmpLeaveId = empLeaveId,
                                    //                EmpName = userIdentityData.EmpName,
                                    //                FromDate = userIdentityData.LeaveFrom,
                                    //                LeaveType = userIdentityData.LeaveType,
                                    //                Reason = userIdentityData.LeaveReason,
                                    //                ToDate = userIdentityData.LeaveTo,
                                    //                EmpLeaveStatus = ClsConstant.EmpLeaveStatus.Pending,
                                    //                UpdateDate = string.Empty,
                                    //                UpdatedBy = string.Empty
                                    //            };
                                    //            List<AgoraBL.Models.SendLeaveNotification.HRIdList> lstHRIdList = new List<AgoraBL.Models.SendLeaveNotification.HRIdList>();
                                    //            for (int i = 0; i < dsEmpLeave.Tables[1].Rows.Count; i++)
                                    //            {
                                    //                List<string> lstChannels = new List<string>();
                                    //                lstChannels = AIChannels.Split(',').ToList();
                                    //                List<AgoraBL.Models.SendLeaveNotification.ChannelList> objLstChannelList = new List<AgoraBL.Models.SendLeaveNotification.ChannelList>();
                                    //                for (int channelcount = 0; channelcount < lstChannels.Count; channelcount++)
                                    //                {
                                    //                    if (dsEmpLeave.Tables[1].Columns.Contains(lstChannels[channelcount]))
                                    //                    {
                                    //                        AgoraBL.Models.SendLeaveNotification.ChannelList objChannelList = new AgoraBL.Models.SendLeaveNotification.ChannelList();
                                    //                        objChannelList.ChannelName = lstChannels[channelcount];
                                    //                        objChannelList.ChannelUniqueId = Convert.ToString(dsEmpLeave.Tables[1].Rows[i][lstChannels[channelcount]]);
                                    //                        objChannelList.ChannelNotificationUniqueId = null;
                                    //                        objChannelList.EmpId = Convert.ToString(dsEmpLeave.Tables[1].Rows[i]["empid"]);
                                    //                        objLstChannelList.Add(objChannelList);
                                    //                    }
                                    //                }
                                    //                AgoraBL.Models.SendLeaveNotification.HRIdList objHRIdList = new AgoraBL.Models.SendLeaveNotification.HRIdList()
                                    //                {
                                    //                    EmpId = Convert.ToString(dsEmpLeave.Tables[1].Rows[i]["empid"]),
                                    //                    HRName = Convert.ToString(dsEmpLeave.Tables[1].Rows[i]["empName"]),
                                    //                    //ObjectId = Convert.ToString(ds.Tables[1].Rows[0]["MSTeam"]),
                                    //                    ChannelList = objLstChannelList,
                                    //                };
                                    //                lstHRIdList.Add(objHRIdList);
                                    //            }
                                    //            objSendLeaveNotificationDto.HRIdList = lstHRIdList;

                                    //            objSendLeaveNotification.Message= "Leave updated succesfully.";
                                    //        }
                                    //    }
                                    objSendLeaveNotification.Status = "1";
                                    objSendLeaveNotification.Message = "Leave saved succesfully.";
                                    //}
                                }
                                else
                                {
                                    objSendLeaveNotification.Status = "0";
                                    objSendLeaveNotification.Message = "Insufficient Leave balance!";
                                }
                            }
                            else
                            {
                                objSendLeaveNotification.Status = "0";
                                objSendLeaveNotification.Message = "Failed! to apply Leave";
                            }
                        }
                        return Ok(objSendLeaveNotification);
                    }
                    return BadRequest("Employee ID and Skill ID are required.");
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Employee-doUpdate-Leave")]
        public IHttpActionResult doUpdateLeave([FromBody] UserIdentityData userIdentityData)
        {
            ResponseData objResponseData = new ResponseData();
            DataSet dsEmpLeave = new DataSet();
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (userIdentityData.EmpLeaveId > 0)
                    {
                        DataTable dtResult = _data.GETLeaveByEMPLeaveID("GETLeaveByEMPLeaveID", userIdentityData.EmpLeaveId);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            userIdentityData.DurationFrom = DateTime.ParseExact(userIdentityData.LeaveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            userIdentityData.DurationTo = DateTime.ParseExact(userIdentityData.LeaveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            ConsolidateLeaves leaveList = _data.GetLeaveDetails(userIdentityData);
                            string StrLeaveStatus = string.Empty;
                            if (string.Compare(userIdentityData.LeaveUpdateStatus, AgoraBL.Common.ClsConstant.EmpLeaveStatus.Approved, true) == 0)
                            {
                                StrLeaveStatus = "a";
                            }
                            else if (string.Compare(userIdentityData.LeaveUpdateStatus, AgoraBL.Common.ClsConstant.EmpLeaveStatus.Rejected, true) == 0)
                            {
                                StrLeaveStatus = "r";
                            }
                            else
                            {
                                objResponseData.Status = "0";
                                objResponseData.Message = "Invalid Leave Status";
                            }

                            if (leaveList.Leave_Details.Count() > 0)
                            {
                                //bool IsEmailSent = false;
                                var strStatus = _bal.UpdateEmpLeaveStatus(mode: "UpdateStatus",
                                                            empID: userIdentityData.EmpID,
                                                            empLeaveID: Convert.ToString(userIdentityData.EmpLeaveId),
                                                            leaveStatus: StrLeaveStatus,
                                                            LeaveType: userIdentityData.LeaveType,
                                                            AdminComment: userIdentityData.LeaveUpdateComment,
                                                            SanctionedBy: Convert.ToString(userIdentityData.LeaveUpdateByEmpId),
                                                            fDate: string.Empty,
                                                            tDate: string.Empty);
                                if (dsEmpLeave != null && dsEmpLeave.Tables[0].Rows.Count > 0)
                                {
                                    objResponseData.Message = "Leave updated succesfully.";
                                }
                            }
                            else
                            {
                                objResponseData.Status = "0";
                                objResponseData.Message = "Failed! to update Leave";
                            }
                        }
                        return Ok(objResponseData);
                    }
                    return BadRequest("EmpLeaveID required.");
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Employee-Add-Timesheet")]
        public IHttpActionResult AddTimesheet([FromBody] TimesheetDTO timesheet)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(timesheet.EmpID.ToString(), timesheet.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(timesheet.EmpID.ToString()) && !string.IsNullOrEmpty(timesheet.SkillId.ToString()))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(timesheet.TSComment)) && !string.IsNullOrEmpty(Convert.ToString(timesheet.TSDate))
                            && !string.IsNullOrEmpty(Convert.ToString(timesheet.TSHour)) && !string.IsNullOrEmpty(Convert.ToString(timesheet.ModuleID)))
                        {
                            if (timesheet.TSDate >= DateTime.Now)
                            {
                                return BadRequest("Date should not be greater than currentdate");
                            }
                            if (timesheet.TSDate < DateTime.Now.AddDays(-4))
                            {
                                bool Approval = AgoraBL.BAL.EmpTimesheetBAL.CheckForApproval(timesheet.EmpID, DateTime.Now.Date);
                                if (!Approval)
                                {
                                    return BadRequest("You can only fill timesheets for the past 3 days.");
                                }
                            }
                            var timesheetAdded = EmpTimesheetBAL.Update(timesheet.ModuleID, timesheet.EmpID, timesheet.TSDate, Convert.ToInt32(timesheet.TSHour), timesheet.TSComment, 0);
                            if (timesheetAdded)
                            {
                                return Ok("The timesheet has been added successfully.");
                            }
                            else
                            {
                                return BadRequest("Something went wrong. Please try again!");
                            }
                        }
                        else
                        {
                            return BadRequest("TSComment,TSDate,TSHour and ModuleID are required.");
                        }
                    }
                    return BadRequest("Employee ID and Skill ID are required.");
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("Employee-Update-Timesheet")]
        public IHttpActionResult UpdateTimesheet([FromBody] AddTimesheet timesheet)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(timesheet.EmpID.ToString(), timesheet.SkillId.ToString()))
                {
                    if (string.IsNullOrEmpty(timesheet.EmpID.ToString()) || string.IsNullOrEmpty(timesheet.SkillId.ToString()))
                    {
                        return BadRequest("Employee ID and Skill ID are required.");
                    }
                    if (timesheet == null || timesheet.lstTimesheet == null || timesheet.lstTimesheet.Count == 0)
                    {
                        return BadRequest("TSComment, TSDate, TSHour, TSID, and ModuleID are required.");
                    }
                    var objAddTimesheet = new AddTimesheet
                    {
                        EmpID = timesheet.EmpID,
                        SkillId = timesheet.SkillId,
                        lstTimesheet = new List<TimesheetDTO>()
                    };
                    bool approval = AgoraBL.BAL.EmpTimesheetBAL.CheckForApproval(timesheet.EmpID, DateTime.Now.Date);
                    foreach (var ts in timesheet.lstTimesheet)
                    {
                        var objTimesheetDTO = new TimesheetDTO
                        {
                            TSComment = ts.TSComment,
                            TSDate = ts.TSDate,
                            TSHour = ts.TSHour,
                            TSID = ts.TSID,
                            ModuleID = ts.ModuleID
                        };

                        if (string.IsNullOrEmpty(ts.TSComment) || string.IsNullOrEmpty(ts.TSDate.ToString()) ||
                            string.IsNullOrEmpty(ts.TSHour.ToString()) || string.IsNullOrEmpty(ts.TSID.ToString()) ||
                            string.IsNullOrEmpty(ts.ModuleID.ToString()))
                        {
                            objTimesheetDTO.IsSucess = false;
                            objTimesheetDTO.Message = "TSComment, TSDate, TSHour, TSID, and ModuleID are required.";
                            objAddTimesheet.lstTimesheet.Add(objTimesheetDTO);
                            continue;
                        }

                        if (ts.TSDate >= DateTime.Now)
                        {
                            objTimesheetDTO.IsSucess = false;
                            objTimesheetDTO.Message = "Date should not be greater than the current date.";
                            objAddTimesheet.lstTimesheet.Add(objTimesheetDTO);
                        }
                        else if (ts.TSDate < DateTime.Now.AddDays(-4))
                        {
                            if (!approval)
                            {
                                objTimesheetDTO.IsSucess = false;
                                objTimesheetDTO.Message = "You can only fill timesheets for the past 3 days.";
                                objAddTimesheet.lstTimesheet.Add(objTimesheetDTO);
                            }
                            else
                            {
                                if (!EmpTimesheetBAL.CheckIsProjectMember(ts.TSID,timesheet.EmpID))
                                {
                                    objTimesheetDTO.IsSucess = false;
                                    objTimesheetDTO.Message = "You cannot update the timesheet because you are not a project member.";
                                    objAddTimesheet.lstTimesheet.Add(objTimesheetDTO);
                                }
                                else
                                {
                                    bool timesheetUpdated = EmpTimesheetBAL.Update(
                                                       ts.ModuleID, timesheet.EmpID, ts.TSDate,
                                                       Convert.ToInt32(ts.TSHour), ts.TSComment, ts.TSID);

                                    if (timesheetUpdated)
                                    {
                                        objTimesheetDTO.IsSucess = true;
                                        objTimesheetDTO.Message = "The timesheet has been updated successfully.";
                                    }
                                    else
                                    {
                                        objTimesheetDTO.IsSucess = false;
                                        objTimesheetDTO.Message = "Something went wrong. Please try again!";
                                    }

                                    objAddTimesheet.lstTimesheet.Add(objTimesheetDTO);  
                                }
                            }
                        }
                        else
                        {
                            if (!EmpTimesheetBAL.CheckIsProjectMember(ts.TSID, timesheet.EmpID))
                            {
                                objTimesheetDTO.IsSucess = false;
                                objTimesheetDTO.Message = "You cannot update the timesheet because you are not a project member.";
                                objAddTimesheet.lstTimesheet.Add(objTimesheetDTO);
                            }
                            else
                            {
                                bool timesheetUpdated = EmpTimesheetBAL.Update(
                                                   ts.ModuleID, timesheet.EmpID, ts.TSDate,
                                                   Convert.ToInt32(ts.TSHour), ts.TSComment, ts.TSID);

                                if (timesheetUpdated)
                                {
                                    objTimesheetDTO.IsSucess = true;
                                    objTimesheetDTO.Message = "The timesheet has been updated successfully.";
                                }
                                else
                                {
                                    objTimesheetDTO.IsSucess = false;
                                    objTimesheetDTO.Message = "Something went wrong. Please try again!";
                                }

                                objAddTimesheet.lstTimesheet.Add(objTimesheetDTO);  
                            }
                        }
                    }

                    return Ok(objAddTimesheet);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("Employee-Delete-Timesheet")]
        public IHttpActionResult DeleteTimesheet([FromBody] TimesheetDTO timesheet)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(timesheet.EmpID.ToString(), timesheet.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(timesheet.EmpID.ToString()) && !string.IsNullOrEmpty(timesheet.SkillId.ToString()))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(timesheet.TSID)))
                        {
                            var timesheetDeleted = EmpTimesheetBAL.Delete(timesheet.TSID);
                            if (timesheetDeleted)
                            {
                                return Ok("The timesheet has been deleted successfully.");
                            }
                            else
                            {
                                return BadRequest("Something went wrong. Please try again!");
                            }
                        }
                        else
                        {
                            return BadRequest("TSID is required.");
                        }
                    }
                    return BadRequest("Employee ID and Skill ID are required.");
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("Employee-ProjectModuleList")]
        public IHttpActionResult GetProjectAndModulelist([FromBody] UserIdentityData userIdentityData)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(userIdentityData.EmpID, userIdentityData.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(userIdentityData.EmpID) && !string.IsNullOrEmpty(userIdentityData.SkillId.ToString()))
                    {
                        var pendingTimesheets = EmpTimesheetBAL.GetProjectAndModulelist(userIdentityData);
                        return Ok(pendingTimesheets);
                    }
                    return BadRequest("Employee ID and Skill ID are required.");
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Employee-Timesheet-Request")]
        public IHttpActionResult EmployeeTimesheetRequest([FromBody] SendTimesheetNotificationDTO.EmployeeTimesheetRequest emp)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(emp.EmpID.ToString(), emp.SkillId.ToString()))
                {
                    if (!string.IsNullOrEmpty(emp.EmpID.ToString()) && !string.IsNullOrEmpty(emp.SkillId.ToString()) && !string.IsNullOrEmpty(emp.EmpName.ToString())
                        && !string.IsNullOrEmpty(emp.FromDate.ToString()) && !string.IsNullOrEmpty(emp.ToDate.ToString()) && !string.IsNullOrEmpty(emp.Requested.ToString())
                        && !string.IsNullOrEmpty(emp.EmpTimesheetStatus.ToString()))
                    {
                        EmpTimesheetBAL.EmployeeTimesheetRequest(emp);
                        return Ok("Request sent successfully.");
                    }
                    return BadRequest("EmpId, SkillId, EmpName, FromDate, ToDate, Requested and EmpTimesheetStatus are required.");
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("Employee-WFH-Details")]
        public IHttpActionResult GetEmployeeWFHDetails([FromBody] EmpWFHBAL objEmpWFHBAL)
        {
            try
            {
                if (ValidateHeader() && ValidateToken(objEmpWFHBAL.EmpID.ToString(), objEmpWFHBAL.SkillId.ToString()))
                {
                    if (objEmpWFHBAL.EmpID > 0 && objEmpWFHBAL.SkillId > 0 && !string.IsNullOrEmpty(objEmpWFHBAL.WFHStatus) && objEmpWFHBAL.Year > 0)
                    {
                        EmpWFHBAL objEmpWFHBLL = new EmpWFHBAL();
                        DataSet DsEmpWFHDetails = null;
                        DataTable DtWorkFromHomeDetails = null;
                        List<EmpWFHBAL> lstEmpWFHBLLDetails = new List<EmpWFHBAL>();
                        EmployeeWFHDetails objemployeeWFHDetails = new EmployeeWFHDetails();
                        WorkFromHomeDetails objWorkFromHomeDetails = new WorkFromHomeDetails();
                        DsEmpWFHDetails = objEmpWFHBLL.GetWFHDetails(objEmpWFHBAL.EmpID, objEmpWFHBAL.WFHStatus, objEmpWFHBAL.Year);
                        DtWorkFromHomeDetails = objEmpWFHBLL.BindWFHBalance(objEmpWFHBAL.EmpID);
                        if (DtWorkFromHomeDetails != null && DtWorkFromHomeDetails.Rows.Count > 0)
                        {
                            objWorkFromHomeDetails = new WorkFromHomeDetails
                            {
                                TotalAnnual = Convert.ToInt32(DtWorkFromHomeDetails.Rows[0]["Total"]),
                                TotalTillCurrentDate = Convert.ToInt32(DtWorkFromHomeDetails.Rows[0]["Total_Accrual"]),
                                Consumed = Convert.ToInt32(DtWorkFromHomeDetails.Rows[0]["Consumed"]),
                                Balance = Convert.ToInt32(DtWorkFromHomeDetails.Rows[0]["Balance"]),
                            };
                            objemployeeWFHDetails.WFHDetails = objWorkFromHomeDetails;
                        }
                        if (DsEmpWFHDetails != null && DsEmpWFHDetails.Tables[0] != null && DsEmpWFHDetails.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in DsEmpWFHDetails.Tables[0].Rows)
                            {
                                EmpWFHBAL objEmpWFH = new EmpWFHBAL();
                                objEmpWFH.ID = Convert.ToInt32(dr["empWFHId"]);
                                objEmpWFH.WFHFrom = Convert.ToString(dr["WFHFrom"]);
                                objEmpWFH.WFHTo = Convert.ToString(dr["WFHTo"]);
                                objEmpWFH.WFHDesc = Convert.ToString(dr["WFHDescription"]);
                                objEmpWFH.WFHStatus = Convert.ToString(dr["WFHStatus"]);
                                objEmpWFH.AdminComments = Convert.ToString(dr["WFHComment"]);
                                lstEmpWFHBLLDetails.Add(objEmpWFH);
                            }

                            objemployeeWFHDetails.WFHListData = lstEmpWFHBLLDetails;
                        }
                        else
                        {
                            objemployeeWFHDetails.WFHListData = null;
                        }
                        return Ok(objemployeeWFHDetails);
                    }
                    return BadRequest("EmpId, SkillId, WFHStatus and Year are required.");
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("Employee-Save-WFH")]
        public IHttpActionResult SaveEmployeeWFH([FromBody] EmpWFHBAL objEmpWFHBAL)
        {
            try
            {
                bool WFHExist = false;
                double days = 0;
                int balance = 0;
                DataSet ds = new DataSet();
                ResponseData objResponseData = new ResponseData();
                EmpWFHBAL objEmpWFHBLL = new EmpWFHBAL();
                EmployeeMasterBAL ObjEmpWFHBLL = new EmployeeMasterBAL();
                if (ValidateHeader() && ValidateToken(objEmpWFHBAL.EmpID.ToString(), objEmpWFHBAL.SkillId.ToString()))
                {
                    if (objEmpWFHBAL.EmpID > 0 && objEmpWFHBAL.SkillId > 0 && !string.IsNullOrEmpty(objEmpWFHBAL.WFHFrom) && !string.IsNullOrEmpty(objEmpWFHBAL.WFHTo) && !string.IsNullOrEmpty(objEmpWFHBAL.Reason))
                    {
                         days = (DateTime.ParseExact(objEmpWFHBAL.WFHTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                                - DateTime.ParseExact(objEmpWFHBAL.WFHFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1;
                        WFHExist = EmpWFHBAL.IfExistsWFH("IFEXISTSWFH", objEmpWFHBAL.EmpID, objEmpWFHBAL.WFHFrom, objEmpWFHBAL.WFHTo);
                        var datatable = objEmpWFHBAL.BindWFHBalance(objEmpWFHBAL.EmpID);
                        if (datatable != null && datatable.Rows.Count > 0)
                        {
                            balance = Convert.ToInt32(datatable.Rows[0]["Balance"]);
                        }
                        if (WFHExist)
                        {
                            objResponseData.Status = "0";
                            objResponseData.Message = "Work From Home for same date already exists.";
                        }
                        else if (days > 2 || balance == 0)
                        {
                            objResponseData.Status = "0";
                            objResponseData.Message = "Insufficient WFH balance";
                        }
                        else
                        {
                            bool IsEmailSent = false;
                            ds = objEmpWFHBLL.SaveWFH(objEmpWFHBAL.EmpID, objEmpWFHBAL.WFHFrom, objEmpWFHBAL.WFHTo, objEmpWFHBAL.Reason, Convert.ToInt32(days));
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                objEmpWFHBAL.SendMail(ds.Tables[0], objEmpWFHBAL.WFHFrom, objEmpWFHBAL.WFHTo, objEmpWFHBAL.Reason, out IsEmailSent);
                                objResponseData.Status = "1";
                                objResponseData.Message = "Work From Home saved succesfully.";
                            }
                            else
                            {
                                objResponseData.Status = "0";
                                objResponseData.Message = "Work From Home save failed.";
                            }
                        }
                        return Ok(objResponseData);
                    }
                    return BadRequest("EmpId, SkillId, WFHFrom, WFHTo and Reason are required.");
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region Check Header And Token Related Function
        private bool ValidateHeader()
        {
            UserIdentityData userIdentityData = new UserIdentityData();
            try
            {
                IEnumerable<string> headerRecipientIdValues = Request.Headers.GetValues("RecipientId");
                IEnumerable<string> headerRecipientNameValues = Request.Headers.GetValues("RecipientName");
                IEnumerable<string> headerSenderApplicationValues = Request.Headers.GetValues("SenderApplication");
                var RecipientId = headerRecipientIdValues.FirstOrDefault();
                var RecipientName = headerRecipientNameValues.FirstOrDefault();
                var SenderApplication = headerSenderApplicationValues.FirstOrDefault();
                if (string.Compare(RecipientId, RecipientIdValue, false) == 0 && string.Compare(RecipientName, RecipientNameValue, false) == 0
                    && string.Compare(SenderApplication, SenderApplicationValue, false) == 0)
                {
                    return true;

                }
                return false;
            }
            catch (Exception)
            {
                throw;

            }
        }
        private bool ValidateToken(string empId, string skillId)
        {
            IEnumerable<string> authorization = Request.Headers.GetValues("Authorization");
            var Token = authorization.FirstOrDefault();
            try
            {
                var checkValidate = ClsCommon.ValidateJwtToken(Token, empId, skillId);
                return checkValidate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private CheckAuthentication ValidateToken()
        {
            IEnumerable<string> authorization = Request.Headers.GetValues("Authorization");
            var Token = authorization.FirstOrDefault();
            try
            {
                var checkValidate = ClsCommon.ValidateJwtToken(Token);
                return checkValidate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CheckLeaveBalance(string LeaveType, int noOfDaysAppliedLeave, List<LeaveDetails> leaveList)
        {
            int CL_Leave_balance = leaveList.Where(z => z.Type.ToUpper() == "CL").FirstOrDefault().Balance;
            int CO_Leave_balance = leaveList.Where(z => z.Type.ToUpper() == "CO").FirstOrDefault().Balance;
            int PL_Leave_balance = leaveList.Where(z => z.Type.ToUpper() == "PL").FirstOrDefault().Balance;
            int SL_Leave_balance = leaveList.Where(z => z.Type.ToUpper() == "SL").FirstOrDefault().Balance;

            if (string.Compare(LeaveType, "CL", true) == 0 && noOfDaysAppliedLeave <= CL_Leave_balance)
            {
                return true;
            }
            else if (string.Compare(LeaveType, "CO", true) == 0 && noOfDaysAppliedLeave <= CO_Leave_balance)
            {
                return true;
            }
            else if (string.Compare(LeaveType, "PL", true) == 0 && noOfDaysAppliedLeave <= PL_Leave_balance)
            {
                return true;
            }
            else if (string.Compare(LeaveType, "SL", true) == 0 && noOfDaysAppliedLeave <= SL_Leave_balance)
            {
                return true;
            }
            if (string.Compare(LeaveType, "WL", true) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}