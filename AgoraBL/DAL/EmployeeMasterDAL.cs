using AgoraBL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Web.Configuration;

namespace AgoraBL.DAL
{
    public class EmployeeMasterDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public UserIdentityData GetEmpDetailsbyId(string UserId, string Name)
        {
            UserIdentityData objUserData = new UserIdentityData();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_EmployeeDeatils", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GetEmployeeByEmail");
                cmd.Parameters.AddWithValue("@ColumnName", Name);
                cmd.Parameters.AddWithValue("@ColumnValue", UserId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objUserData.EmpEmail = dt.Rows[0]["empEmail"].ToString();
                    objUserData.EmpID = dt.Rows[0]["empid"].ToString();
                    objUserData.EmpName = dt.Rows[0]["empName"].ToString();
                    objUserData.SkillDesc = dt.Rows[0]["SkillDesc"].ToString();
                    objUserData.SkillId = Convert.ToInt32(dt.Rows[0]["skillid"]);
                    objUserData.IsSuperAdmin = Convert.ToBoolean(dt.Rows[0]["IsSuperAdmin"]);
                    objUserData.IsAccountAdmin = Convert.ToBoolean(dt.Rows[0]["IsAccountAdmin"]);
                    objUserData.IsPayrollAdmin = Convert.ToBoolean(dt.Rows[0]["IsPayrollAdmin"]);
                    objUserData.IsPM = Convert.ToBoolean(dt.Rows[0]["IsPM"]);
                    objUserData.IsTester = Convert.ToBoolean(dt.Rows[0]["IsTester"]);
                    objUserData.IsSuccess = true;
                    objUserData.StatusMessage = "Success";

                }
                else
                {
                    objUserData.IsSuccess = true;
                    objUserData.StatusMessage = "Failed";
                }
            }
            catch (Exception ex)
            {
                objUserData.IsSuccess = false;
                objUserData.StatusMessage = ex.Message;

            }
            return objUserData;
        }
        public ClsLogin EmployeeLogin(string EmpID, string Password, string OsType = "", string DeviceId = "")
        {
            ClsLogin employeeLogin = new ClsLogin();
            try
            {
                using (SqlConnection con = new SqlConnection(_strConnection))
                {
                    SqlCommand cmd = new SqlCommand("SP_UserMaster", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Mode", "LOGIN");
                    cmd.Parameters.AddWithValue("@EmpID", EmpID);
                    cmd.Parameters.AddWithValue("@OSType", OsType);
                    cmd.Parameters.AddWithValue("@DeviceID", DeviceId);
                    SqlDataReader reader = null;
                    using (con)
                    {
                        con.Open();
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if (reader["empPassword"].ToString() == Password || Convert.ToInt32(reader["EmpID"]) == 0)
                            {
                                if (reader["IsActive"].ToString() == "True")
                                {
                                    employeeLogin.IsActive = true;

                                    employeeLogin.EmpId = Convert.ToString(reader["EmpID"]);
                                    if (reader["IsAdmin"].ToString() == "True")
                                    {
                                        employeeLogin.IsAdmin = true;
                                    }
                                    employeeLogin.SkillID = Convert.ToInt32(reader["SkillID"].ToString());
                                    employeeLogin.Name = reader["EmpName"].ToString();
                                    employeeLogin.EmailID = reader["EmpEmail"].ToString();
                                    employeeLogin.Address = reader["EmpAddress"].ToString();
                                    employeeLogin.Contact = reader["EmpContact"].ToString();
                                    employeeLogin.JoiningDate = reader["EmpJoiningDate"].ToString();
                                    employeeLogin.LeavingDate = reader["EmpLeavingDate"].ToString();
                                    employeeLogin.ProbationPeriod = reader["EmpProbationPeriod"].ToString();
                                    employeeLogin.Notes = reader["EmpNotes"].ToString();
                                    employeeLogin.AccountNo = reader["EmpAccountNo"].ToString();
                                    employeeLogin.BDate = reader["EmpBDate"].ToString();
                                    employeeLogin.ADate = reader["EmpADate"].ToString();
                                    employeeLogin.PreviousEmployer = reader["EmpPrevEmployer"].ToString();
                                    employeeLogin.Experince = Convert.ToInt32(reader["EmpExperince"].ToString());
                                    employeeLogin.LocationID = reader["LocationFKID"].ToString();
                                    employeeLogin.CurrentAddress = reader["EmpCurrentAddress"].ToString();
                                    employeeLogin.Message = "Login success";
                                    employeeLogin.Designation = reader["Designation"].ToString();
                                    employeeLogin.empConfDate = reader["empConfDate"].ToString();
                                    employeeLogin.onbaordingCompleted = Convert.ToBoolean(reader["IsOnbaordingCompleted"]);
                                    employeeLogin.ProfileID = (reader["ProfileId"]).ToString();
                                    if (reader["IsRemoteEmployee"] != DBNull.Value && !string.IsNullOrEmpty(reader["IsRemoteEmployee"].ToString()))
                                    {
                                        if (Convert.ToBoolean(reader["IsRemoteEmployee"].ToString()))
                                        {
                                            employeeLogin.IsRemoteEmployee = true;
                                        }
                                        else
                                        {
                                            employeeLogin.IsRemoteEmployee = false;
                                        }
                                    }

                                    if (reader["ProfileID"] != "")
                                    {
                                        employeeLogin.ProfileID = reader["ProfileID"].ToString();
                                        if (reader["LocationID"] != System.DBNull.Value)
                                        {
                                            employeeLogin.ProfileLocationID = Convert.ToInt32(reader["LocationID"].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    employeeLogin.IsActive = false;
                                    employeeLogin.Message = "Account blocked.";
                                }

                            }
                            else
                            {
                                employeeLogin.IsActive = false;
                                employeeLogin.Message = "Invalid password.";
                            }
                        }
                        else
                        {
                            employeeLogin.IsActive = false;
                            employeeLogin.Message = "Invalid login id.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                employeeLogin.IsActive = false;
                employeeLogin.Message = "Error: " + ex.Message;
            }
            return employeeLogin;
        }

        public ClsLogin EmployeeADLogin(string EmpID)
        {
            ClsLogin employeeLogin = new ClsLogin();
            try
            {
                using (SqlConnection con = new SqlConnection(_strConnection))
                {
                    SqlCommand cmd = new SqlCommand("SP_UserMaster", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Mode", "LOGIN");
                    cmd.Parameters.AddWithValue("@ADUserName", EmpID);
                    SqlDataReader reader = null;
                    using (con)
                    {
                        con.Open();
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if (reader["IsActive"].ToString() == "True")
                            {
                                employeeLogin.IsActive = true;
                                employeeLogin.EmpId = Convert.ToString(reader["EmpID"]);
                                if (reader["IsAdmin"].ToString() == "True")
                                {
                                    employeeLogin.IsAdmin = true;
                                }
                                employeeLogin.SkillID = Convert.ToInt32(reader["SkillID"].ToString());
                                employeeLogin.Name = reader["EmpName"].ToString();
                                employeeLogin.EmailID = reader["EmpEmail"].ToString();
                                employeeLogin.Address = reader["EmpAddress"].ToString();
                                employeeLogin.Contact = reader["EmpContact"].ToString();
                                employeeLogin.JoiningDate = reader["EmpJoiningDate"].ToString();
                                employeeLogin.LeavingDate = reader["EmpLeavingDate"].ToString();
                                employeeLogin.ProbationPeriod = reader["EmpProbationPeriod"].ToString();
                                employeeLogin.Notes = reader["EmpNotes"].ToString();
                                employeeLogin.AccountNo = reader["EmpAccountNo"].ToString();
                                employeeLogin.BDate = reader["EmpBDate"].ToString();
                                employeeLogin.ADate = reader["EmpADate"].ToString();
                                employeeLogin.PreviousEmployer = reader["EmpPrevEmployer"].ToString();
                                employeeLogin.Experince = Convert.ToInt32(reader["EmpExperince"].ToString());
                                employeeLogin.LocationID = reader["LocationFKID"].ToString();
                                employeeLogin.CurrentAddress = reader["EmpCurrentAddress"].ToString();
                                if (reader["IsRemoteEmployee"] != DBNull.Value && !string.IsNullOrEmpty(reader["IsRemoteEmployee"].ToString()))
                                {
                                    if (Convert.ToBoolean(reader["IsRemoteEmployee"].ToString()))
                                    {
                                        employeeLogin.IsRemoteEmployee = true;
                                    }
                                    else
                                    {
                                        employeeLogin.IsRemoteEmployee = false;
                                    }
                                }
                                if (reader["ProfileID"] != "")
                                {
                                    employeeLogin.ProfileID = reader["ProfileID"].ToString();
                                    if (reader["LocationID"] != System.DBNull.Value)
                                    {
                                        employeeLogin.ProfileLocationID = Convert.ToInt32(reader["LocationID"].ToString());
                                    }
                                }
                            }
                            else
                            {
                                employeeLogin.IsActive = false;
                                employeeLogin.Message = "Account blocked.";
                            }
                        }
                        else
                        {
                            employeeLogin.IsActive = false;
                            employeeLogin.Message = "Invalid login id.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                employeeLogin.IsActive = false;
                employeeLogin.Message = "Error: " + ex.Message;
            }
            return employeeLogin;
        }
        public EmployeeDetails GetEmployeeDetails(int empId)
        {
            EmployeeDetails empDeatils = new EmployeeDetails();
            empDeatils.Engagement_Details = new List<EngagementDetails>();
            empDeatils.Timesheet_Details = new List<Timesheet>();
            empDeatils.Leave_Details = new List<Leave>();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_EmployeeDeatils", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GetEmployeeDetailsByEmployeeId");
                cmd.Parameters.AddWithValue("@EmployeeID", empId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Demographic demographic = new Demographic()
                    {
                        EmpId = ds.Tables[0].Rows[0]["empid"].ToString(),
                        EmpEmail = ds.Tables[0].Rows[0]["empemail"].ToString(),
                        FullName = ds.Tables[0].Rows[0]["FullName"].ToString(),
                        ContactNo = ds.Tables[0].Rows[0]["empContact"].ToString(),
                        DateOfJoin = ds.Tables[0].Rows[0]["DOJ"].ToString(),
                        Address = ds.Tables[0].Rows[0]["empAddress"].ToString(),
                        DateOfBirth = ds.Tables[0].Rows[0]["DOB"].ToString(),
                        SkillDesc = ds.Tables[0].Rows[0]["SkillDesc"].ToString(),


                    };
                    empDeatils.Demographic_Details = demographic;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        EngagementDetails engagement = new EngagementDetails()
                        {
                            ProjectName = ds.Tables[1].Rows[i]["projName"].ToString(),
                            InsertedOn = ds.Tables[1].Rows[i]["projectCreatedOn"].ToString()

                        };
                        empDeatils.Engagement_Details.Add(engagement);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        Timesheet timesheet = new Timesheet()
                        {
                            ProjName = ds.Tables[2].Rows[i]["projName"].ToString(),
                            TsComment = ds.Tables[2].Rows[i]["tsComment"].ToString(),
                            TsEntryDate = ds.Tables[2].Rows[i]["tsEntryDate"].ToString(),
                            TsHour = ds.Tables[2].Rows[i]["tsHour"].ToString(),
                        };
                        empDeatils.Timesheet_Details.Add(timesheet);
                    }
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        Leave leave = new Leave()
                        {
                            LeaveFrom = ds.Tables[3].Rows[i]["leaveFrom"].ToString(),
                            LeaveTo = ds.Tables[3].Rows[i]["leaveTo"].ToString(),
                            LeaveDescription = ds.Tables[3].Rows[i]["leaveDesc"].ToString(),
                            LeaveStatus = ds.Tables[3].Rows[i]["status"].ToString(),
                            LeaveType = ds.Tables[3].Rows[i]["leaveId"].ToString(),
                            SanctionDate = ds.Tables[3].Rows[i]["leaveSenctionedDate"].ToString(),
                            SanctionBy = ds.Tables[3].Rows[i]["leaveSanctionBy"].ToString(),
                        };
                        empDeatils.Leave_Details.Add(leave);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return empDeatils;
        }

        #region HR Related Function
        public EmployeeDetailsHR GetEmployeeDetailsByHR(string EmpName)
        {
            EmployeeDetailsHR empDeatils = new EmployeeDetailsHR();
            empDeatils.Demographic_Details_HR = new List<Demographic>();
            empDeatils.Engagement_Details_HR = new List<EngagementDetails>();
            empDeatils.Timesheet_Details_HR = new List<Timesheet>();
            empDeatils.Leave_Details_HR = new List<Leave>();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_HR_Employee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeName", EmpName.Trim());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Demographic demographic = new Demographic()
                        {
                            FullName = ds.Tables[0].Rows[i]["FullName"].ToString(),
                            ContactNo = ds.Tables[0].Rows[i]["empContact"].ToString(),
                            DateOfJoin = ds.Tables[0].Rows[i]["empJoiningDate"].ToString(),
                            // EmergencyContact= ds.Tables[0].Rows[0]["projName"].ToString(),
                            Address = ds.Tables[0].Rows[i]["empAddress"].ToString(),
                            DateOfBirth = ds.Tables[0].Rows[i]["empBDate"].ToString(),
                            EmpNotes = ds.Tables[0].Rows[i]["empNotes"].ToString(),
                            SkillDesc = ds.Tables[0].Rows[i]["SkillDesc"].ToString(),
                            EmpId = ds.Tables[0].Rows[i]["empid"].ToString(),
                            EmpEmail = ds.Tables[0].Rows[i]["empemail"].ToString(),

                        };
                        empDeatils.Demographic_Details_HR.Add(demographic);
                    }

                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        EngagementDetails engagement = new EngagementDetails()
                        {
                            EmpName = ds.Tables[1].Rows[i]["empName"].ToString(),
                            ProjectName = ds.Tables[1].Rows[i]["projName"].ToString(),
                            InsertedOn = ds.Tables[1].Rows[i]["projectCreatedOn"].ToString(),

                        };
                        empDeatils.Engagement_Details_HR.Add(engagement);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        Timesheet timesheet = new Timesheet()
                        {
                            EmpName = ds.Tables[2].Rows[i]["empName"].ToString(),
                            ProjName = ds.Tables[2].Rows[i]["projName"].ToString(),
                            TsComment = ds.Tables[2].Rows[i]["tsComment"].ToString(),
                            TsEntryDate = ds.Tables[2].Rows[i]["tsEntryDate"].ToString(),
                            TsHour = ds.Tables[2].Rows[i]["tsHour"].ToString(),
                        };
                        empDeatils.Timesheet_Details_HR.Add(timesheet);
                    }
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        Leave leave = new Leave()
                        {
                            EmpName = ds.Tables[2].Rows[i]["empName"].ToString(),
                            LeaveFrom = ds.Tables[3].Rows[i]["leaveFrom"].ToString(),
                            LeaveTo = ds.Tables[3].Rows[i]["leaveTo"].ToString(),
                            LeaveDescription = ds.Tables[3].Rows[i]["leaveDesc"].ToString(),
                            LeaveStatus = ds.Tables[3].Rows[i]["status"].ToString(),
                            LeaveType = ds.Tables[3].Rows[i]["leaveId"].ToString(),
                            SanctionDate = ds.Tables[3].Rows[i]["leaveSenctionedDate"].ToString(),
                            SanctionBy = ds.Tables[3].Rows[i]["leaveSanctionBy"].ToString(),
                        };
                        empDeatils.Leave_Details_HR.Add(leave);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return empDeatils;
        }
        public List<Timesheet> GetTimesheetDetailsByHR(Entity entity)
        {
            List<Timesheet> lstTimesheet = new List<Timesheet>();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_HR_Timesheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EntityName", entity.EntityName);
                cmd.Parameters.AddWithValue("@Project", entity.Project);
                cmd.Parameters.AddWithValue("@DurationFrom", entity.DurationFrom);
                cmd.Parameters.AddWithValue("@DurationTo", entity.DurationTo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Timesheet timesheet = new Timesheet()
                        {
                            EmpId = Convert.ToInt32(dt.Rows[i]["empId"]),
                            EmpName = dt.Rows[i]["empName"].ToString(),
                            ProjName = dt.Rows[i]["projName"].ToString(),
                            TsComment = dt.Rows[i]["tsComment"].ToString(),
                            TsEntryDate = dt.Rows[i]["tsEntryDate"].ToString(),
                            ProjModule = dt.Rows[i]["ProjModule"].ToString(),
                            TsHour = dt.Rows[i]["tsHour"].ToString(),
                        };
                        lstTimesheet.Add(timesheet);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return lstTimesheet;
        }
        public ConsolidateLeaves GetLeaveDetailsByHR(Entity entity,string mode)
        {
            ConsolidateLeaves Consolidate_Leaves = new ConsolidateLeaves();
            Consolidate_Leaves.Leave = new List<Leave>();
            Consolidate_Leaves.Leave_Details = new List<LeaveDetails>();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_HR_Leave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeName", entity.EntityName);
                cmd.Parameters.AddWithValue("@DurationFrom", entity.DurationFrom);
                cmd.Parameters.AddWithValue("@DurationTo", entity.DurationTo);
                cmd.Parameters.AddWithValue("@Mode", mode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Leave leave = new Leave()
                        {
                            EmpId = Convert.ToInt32(ds.Tables[0].Rows[i]["empId"]),
                            EmpName = ds.Tables[0].Rows[i]["empName"].ToString(),
                            LeaveFrom = ds.Tables[0].Rows[i]["leaveFrom"].ToString(),
                            LeaveTo = ds.Tables[0].Rows[i]["leaveTo"].ToString(),
                            LeaveDescription = ds.Tables[0].Rows[i]["leaveDesc"].ToString(),
                            LeaveStatus = ds.Tables[0].Rows[i]["status"].ToString(),
                            LeaveType = ds.Tables[0].Rows[i]["leaveId"].ToString(),
                            SanctionDate = ds.Tables[0].Rows[i]["leaveSenctionedDate"].ToString(),
                            SanctionBy = ds.Tables[0].Rows[i]["leaveSanctionBy"].ToString(),
                            LeaveComment = ds.Tables[0].Rows[i]["leaveComment"].ToString(),
                        };
                        Consolidate_Leaves.Leave.Add(leave);
                    }
                }
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        LeaveDetails Leave_Details = new LeaveDetails()
                        {
                            EmpId = Convert.ToInt32(ds.Tables[1].Rows[i]["EmployeeId"]),
                            EmpName = ds.Tables[1].Rows[i]["EmpName"].ToString(),
                            Type = ds.Tables[1].Rows[i]["Type"].ToString(),
                            Total = Convert.ToInt32(ds.Tables[1].Rows[i]["Total"]),
                            Total_Accural = Convert.ToInt32(ds.Tables[1].Rows[i]["Total_Accrual"]),
                            Consumed = Convert.ToInt32(ds.Tables[1].Rows[i]["Consumed"]),
                            Balance = Convert.ToInt32(ds.Tables[1].Rows[i]["Balance"]),
                            CarryFwdPL = Convert.ToInt32(ds.Tables[1].Rows[i]["CarrFwdPL"]),
                            AccCurrYrPL = Convert.ToInt32(ds.Tables[1].Rows[i]["AccCurrYrPL"])
                        };
                        Consolidate_Leaves.Leave_Details.Add(Leave_Details);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Consolidate_Leaves;
        }
        public List<TimesheetTotalHours> GetTimesheetDetailsTotalHoursByRole(Entity entity)
        {
            List<TimesheetTotalHours> lstTimesheetTotalHours = new List<TimesheetTotalHours>();
            TimesheetTotalHours timesheetTotalHours = new TimesheetTotalHours();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_Employee_TimesheetDetails", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@mode", "GETTSByEnity");
                cmd.Parameters.AddWithValue("@empid", entity.EntityID);
                cmd.Parameters.AddWithValue("@projName", entity.Project);
                cmd.Parameters.AddWithValue("@durationFrom", entity.DurationFrom);
                cmd.Parameters.AddWithValue("@durationTo", entity.DurationTo);
                cmd.Parameters.AddWithValue("@enitityName", entity.EntityName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        timesheetTotalHours = new TimesheetTotalHours
                        {
                            EmpId = Convert.ToInt32(dt.Rows[i]["empId"]),
                            EmpName = Convert.ToString(dt.Rows[i]["empName"]),
                            TotalHours = Convert.ToInt32(dt.Rows[i]["TotalHours"]),
                            ProjectName = Convert.ToString(dt.Rows[i]["projName"]),
                            IsAccessible = Convert.ToBoolean(dt.Rows[i]["IsAccessible"])
                        };
                        lstTimesheetTotalHours.Add(timesheetTotalHours);
                    }

                }
                else
                {
                    timesheetTotalHours = new TimesheetTotalHours
                    {
                        IsAccessible = false
                    };
                    lstTimesheetTotalHours.Add(timesheetTotalHours);
                }
                return lstTimesheetTotalHours;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<PendingTimeSheet> GetPendingTimesheetByHR(Entity entity)
        {
            DateTime durationFrom;
            DateTime durationTo;
            List<PendingTimeSheet> objPrjMemberTimesheet = new List<PendingTimeSheet>();
            SqlDataReader reader = null;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("USP_HR_Emp_Pending_Timesheet", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (entity.DurationFrom == null)
            {
                DateTime now = DateTime.Now;
                durationFrom = new DateTime(now.Year, now.Month, 1);
            }
            else
            {
                durationFrom = Convert.ToDateTime(entity.DurationFrom);
            }
            if (entity.DurationTo == null)
            {
                durationTo = DateTime.Now;
            }
            else
            {
                durationTo = Convert.ToDateTime(entity.DurationTo);
            }

            cmd.Parameters.AddWithValue("@MODE", "PendingTimesheetHR");
            cmd.Parameters.AddWithValue("@EntityName", entity.EntityName);
            cmd.Parameters.AddWithValue("@DurationFrom", durationFrom);
            cmd.Parameters.AddWithValue("@DurationTo", durationTo);


            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objPrjMemberTimesheet.Add(new PendingTimeSheet(
                          Convert.ToInt32(reader["EmpID"].ToString()),
                           reader["EMPName"].ToString(),
                           reader["empEmail"].ToString(),
                           reader["AttDate"].ToString(),
                           reader["TimeAvailable"].ToString(),
                           reader["TimeReported"].ToString(),
                           reader["Comment"].ToString()
                          ));
                    }
                }
            }
            return objPrjMemberTimesheet;
        }
        #endregion

        #region Employee Related Function
        public List<Timesheet> GetTimesheetDetails(UserIdentityData userIdentityData)
        {
            List<Timesheet> lstTimesheet = new List<Timesheet>();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_Emp_Timesheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", userIdentityData.EmpID);
                cmd.Parameters.AddWithValue("@DurationFrom", userIdentityData.DurationFrom);
                cmd.Parameters.AddWithValue("@DurationTo", userIdentityData.DurationTo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Timesheet timesheet = new Timesheet()
                        {
                            EmpId = Convert.ToInt32(dt.Rows[i]["empId"]),
                            TSId = Convert.ToInt32(dt.Rows[i]["tsId"]),
                            EmpName = dt.Rows[i]["empName"].ToString(),
                            ProjName = dt.Rows[i]["projName"].ToString(),
                            ProjModule = dt.Rows[i]["moduleName"].ToString(),
                            TsComment = dt.Rows[i]["tsComment"].ToString(),
                            TsEntryDate = dt.Rows[i]["tsEntryDate"].ToString(),
                            TsHour = dt.Rows[i]["tsHour"].ToString(),
                            ModuleId = Convert.ToInt32(dt.Rows[i]["ModuleId"])
                        };
                        lstTimesheet.Add(timesheet);
                    }
                }
            }

            catch (Exception)
            {

                throw;
            }
            return lstTimesheet;
        }
        public ConsolidateLeaves GetLeaveDetails(UserIdentityData userIdentityData)
        {
            ConsolidateLeaves Consolidate_Leaves = new ConsolidateLeaves();
            Consolidate_Leaves.Leave = new List<Leave>();
            Consolidate_Leaves.Leave_Details = new List<LeaveDetails>();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_Emp_Leave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", userIdentityData.EmpID);
                cmd.Parameters.AddWithValue("@DurationFrom", userIdentityData.DurationFrom);
                cmd.Parameters.AddWithValue("@DurationTo", userIdentityData.DurationTo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Leave leave = new Leave()
                        {
                            EmpId = Convert.ToInt32(ds.Tables[0].Rows[i]["empId"]),
                            EmpName = ds.Tables[0].Rows[i]["empName"].ToString(),
                            LeaveFrom = ds.Tables[0].Rows[i]["leaveFrom"].ToString(),
                            LeaveTo = ds.Tables[0].Rows[i]["leaveTo"].ToString(),
                            LeaveDescription = ds.Tables[0].Rows[i]["leaveDesc"].ToString(),
                            LeaveStatus = ds.Tables[0].Rows[i]["status"].ToString(),
                            LeaveType = ds.Tables[0].Rows[i]["leaveId"].ToString(),
                            SanctionDate = ds.Tables[0].Rows[i]["leaveSenctionedDate"].ToString(),
                            SanctionBy = ds.Tables[0].Rows[i]["leaveSanctionBy"].ToString(),
                            LeaveComment = ds.Tables[0].Rows[i]["leaveComment"].ToString(),
                        };
                        Consolidate_Leaves.Leave.Add(leave);
                    }
                }
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        LeaveDetails Leave_Details = new LeaveDetails()
                        {
                            EmpId = Convert.ToInt32(ds.Tables[1].Rows[i]["EmployeeId"]),
                            EmpName = ds.Tables[1].Rows[i]["EmpName"].ToString(),
                            Type = ds.Tables[1].Rows[i]["Type"].ToString(),
                            Total = Convert.ToInt32(ds.Tables[1].Rows[i]["Total"]),
                            Total_Accural = Convert.ToInt32(ds.Tables[1].Rows[i]["Total_Accrual"]),
                            Consumed = Convert.ToInt32(ds.Tables[1].Rows[i]["Consumed"]),
                            Balance = Convert.ToInt32(ds.Tables[1].Rows[i]["Balance"]),
                            CarryFwdPL = Convert.ToInt32(ds.Tables[1].Rows[i]["CarrFwdPL"]),
                            AccCurrYrPL = Convert.ToInt32(ds.Tables[1].Rows[i]["AccCurrYrPL"])
                        };
                        Consolidate_Leaves.Leave_Details.Add(Leave_Details);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Consolidate_Leaves;
        }
        public List<Leave> GetEmployeePendingLeave(UserIdentityData userIdentityData)
        {
            DateTime durationFrom;
            DateTime durationTo;
            List<Leave> lstLeaves = new List<Leave>();
            DataSet ds = new DataSet();
            try
            {
                if (userIdentityData.DurationFrom == null)
                {
                    DateTime now = DateTime.Now;
                    durationFrom = new DateTime(now.Year, now.Month, 1);
                }
                else
                {
                    durationFrom = Convert.ToDateTime(userIdentityData.DurationFrom);
                }
                if (userIdentityData.DurationTo == null)
                {
                    durationTo = DateTime.Now;
                }
                else
                {
                    durationTo = Convert.ToDateTime(userIdentityData.DurationTo);
                }

                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_Emp_Leave", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@EmpID", userIdentityData.EmpID);
                cmd.Parameters.AddWithValue("@DurationFrom", durationFrom);
                cmd.Parameters.AddWithValue("@DurationTo", durationTo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["status"].ToString().ToUpper() == "PENDING")
                        {
                            Leave leave = new Leave()
                            {
                                EmpId = Convert.ToInt32(ds.Tables[0].Rows[i]["empId"]),
                                EmpName = ds.Tables[0].Rows[i]["empName"].ToString(),
                                EmpLeaveId = ds.Tables[0].Rows[i]["empLeaveId"].ToString(),
                                LeaveFrom = ds.Tables[0].Rows[i]["leaveFrom"].ToString(),
                                LeaveTo = ds.Tables[0].Rows[i]["leaveTo"].ToString(),
                                LeaveDescription = ds.Tables[0].Rows[i]["leaveDesc"].ToString(),
                                LeaveStatus = ds.Tables[0].Rows[i]["status"].ToString(),
                                LeaveType = ds.Tables[0].Rows[i]["leaveId"].ToString(),
                                SanctionDate = ds.Tables[0].Rows[i]["leaveSenctionedDate"].ToString(),
                                SanctionBy = ds.Tables[0].Rows[i]["leaveSanctionBy"].ToString(),
                                LeaveComment = ds.Tables[0].Rows[i]["leaveComment"].ToString(),
                            };
                            lstLeaves.Add(leave);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<Leave>();
            }
            return lstLeaves;
        }

        public List<Leave> GetHRPendingLeave(Entity entity)
        {
            List<Leave> lstLeaves = new List<Leave>();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_HR_EmployeePendingLeave", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@EmployeeName", entity.EntityName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (string.Compare(Convert.ToString(ds.Tables[0].Rows[i]["status"]), "PENDING", true) == 0)
                        {
                            Leave leave = new Leave()
                            {
                                EmpId = Convert.ToInt32(ds.Tables[0].Rows[i]["empId"]),
                                EmpName = Convert.ToString(ds.Tables[0].Rows[i]["empName"]),
                                EmpLeaveId = Convert.ToString(ds.Tables[0].Rows[i]["empLeaveId"]),
                                LeaveFrom = Convert.ToString(ds.Tables[0].Rows[i]["leaveFrom"]),
                                LeaveTo = Convert.ToString(ds.Tables[0].Rows[i]["leaveTo"]),
                                LeaveDescription = Convert.ToString(ds.Tables[0].Rows[i]["leaveDesc"]),
                                LeaveStatus = Convert.ToString(ds.Tables[0].Rows[i]["status"]),
                                LeaveType = Convert.ToString(ds.Tables[0].Rows[i]["leaveId"]),
                                SanctionDate = Convert.ToString(ds.Tables[0].Rows[i]["leaveSenctionedDate"]),
                                SanctionBy = Convert.ToString(ds.Tables[0].Rows[i]["leaveSanctionBy"]),
                                LeaveComment = Convert.ToString(ds.Tables[0].Rows[i]["leaveComment"]),
                                LeaveCount = Convert.ToInt32(ds.Tables[0].Rows[i]["leaveCount"]),
                            };
                            lstLeaves.Add(leave);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<Leave>();
            }
            return lstLeaves;
        }
        public List<TimesheetTotalHours> GetTimesheetDetailsTotalHours(UserIdentityData userIdentityData)
        {
            List<TimesheetTotalHours> lstTimesheetTotalHours = new List<TimesheetTotalHours>();
            TimesheetTotalHours timesheetTotalHours = new TimesheetTotalHours();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_Employee_TimesheetDetails", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@mode", "GETTSByEmpID");
                cmd.Parameters.AddWithValue("@empid", userIdentityData.EmpID);
                cmd.Parameters.AddWithValue("@projName", userIdentityData.ProjName);
                cmd.Parameters.AddWithValue("@durationFrom", userIdentityData.DurationFrom);
                cmd.Parameters.AddWithValue("@durationTo", userIdentityData.DurationTo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        timesheetTotalHours = new TimesheetTotalHours
                        {
                            EmpId = Convert.ToInt32(dt.Rows[i]["empId"]),
                            EmpName = Convert.ToString(dt.Rows[i]["empName"]),
                            TotalHours = Convert.ToInt32(dt.Rows[i]["TotalHours"]),
                            ProjectName = Convert.ToString(dt.Rows[i]["projName"])
                        };
                        lstTimesheetTotalHours.Add(timesheetTotalHours);
                    }

                }
                return lstTimesheetTotalHours;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Demographic GetDemographicDetail(UserIdentityData userIdentityData)
        {
            Demographic demographic = new Demographic();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand comm = new SqlCommand("USP_AI_Emp_Demographic", con);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@EmpID", userIdentityData.EmpID);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    demographic = new Demographic()
                    {
                        EmpId = dt.Rows[0]["empid"].ToString(),
                        EmpEmail = dt.Rows[0]["empemail"].ToString(),
                        FullName = dt.Rows[0]["empName"].ToString(),
                        ContactNo = dt.Rows[0]["empContact"].ToString(),
                        DateOfJoin = dt.Rows[0]["DOJ"].ToString(),
                        Address = dt.Rows[0]["empAddress"].ToString(),
                        DateOfBirth = dt.Rows[0]["DOB"].ToString(),
                        SkillDesc = dt.Rows[0]["SkillDesc"].ToString(),

                    };
                }
            }
            catch (Exception)
            {

                throw;
            }
            return demographic;
        }
        public List<PendingTimeSheet> GetPendingTimesheetEmployee(UserIdentityData userIdentityData)
        {
            DateTime durationFrom;
            DateTime durationTo;
            List<PendingTimeSheet> objPendingTimesheetEmp = new List<PendingTimeSheet>();
            SqlDataReader reader = null;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("USP_AI_Emp_Pending_Timesheet", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (userIdentityData.DurationFrom == null)
            {
                DateTime now = DateTime.Now;
                durationFrom = new DateTime(now.Year, now.Month, 1);
            }
            else
            {
                durationFrom = Convert.ToDateTime(userIdentityData.DurationFrom);
            }
            if (userIdentityData.DurationTo == null)
            {
                durationTo = DateTime.Now;
            }
            else
            {
                durationTo = Convert.ToDateTime(userIdentityData.DurationTo);
            }

            cmd.Parameters.AddWithValue("@MODE", "PendingTimesheet");
            cmd.Parameters.AddWithValue("@EmpID", userIdentityData.EmpID);
            cmd.Parameters.AddWithValue("@DurationFrom", durationFrom);
            cmd.Parameters.AddWithValue("@DurationTo", durationTo);
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objPendingTimesheetEmp.Add(new PendingTimeSheet(
                           Convert.ToInt32(reader["EmpID"].ToString()),
                           reader["EMPName"].ToString(),
                           reader["empEmail"].ToString(),
                           reader["AttDate"].ToString(),
                           reader["TimeAvailable"].ToString(),
                           reader["TimeReported"].ToString(),
                           reader["Comment"].ToString()
                          ));
                    }
                }
            }
            return objPendingTimesheetEmp;
        }
        public DataSet SaveLeave(string mode, int empid, Leave obj)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpLeave", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.Parameters.AddWithValue("@Status", "0");
            cmd.Parameters.AddWithValue("@Type", obj.LeaveType);
            cmd.Parameters.AddWithValue("@Month", "");
            cmd.Parameters.AddWithValue("@LeaveFrom", DateTime.ParseExact(obj.LeaveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@LeaveTo", DateTime.ParseExact(obj.LeaveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@LeaveDesc", obj.LeaveDescription);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public int DeleteLeave(int empLeaveID, string mode)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpLeave", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", 0);
            cmd.Parameters.AddWithValue("@Status", "0");
            cmd.Parameters.AddWithValue("@Type", "0");
            cmd.Parameters.AddWithValue("@Month", "");
            cmd.Parameters.AddWithValue("@LeaveFrom", "");
            cmd.Parameters.AddWithValue("@LeaveTo", "");
            cmd.Parameters.AddWithValue("@LeaveDesc", "");
            cmd.Parameters.AddWithValue("@EmpLeaveID", empLeaveID);
            cmd.CommandType = CommandType.StoredProcedure;
            int ireturn = cmd.ExecuteNonQuery();
            con.Close();
            return ireturn;
        }



        public bool IfExistsLeave(string mode, int empid, string leaveFrom, string leaveTo)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpLeave", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.Parameters.AddWithValue("@LeaveFrom", DateTime.ParseExact(leaveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@LeaveTo", DateTime.ParseExact(leaveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr;
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DataTable GETLeaveByEMPLeaveID(string mode, int EmpLeaveID)
        {
            DataTable dtResult = new DataTable();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpLeave", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpLeaveID", EmpLeaveID);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtResult);
            con.Close();
            return dtResult;
        }
        public DataSet GetLeaveNotificationDetails(string mode, int EmpLeaveID,int EmpId)
        {
            DataSet dsResult = new DataSet();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpLeave", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpLeaveID", EmpLeaveID);
            cmd.Parameters.AddWithValue("@EmpID", EmpId);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsResult);
            con.Close();
            return dsResult;
        }
        public string SendMail(string bodyStr, string subjectStr, string toMailId, string fromMailId, bool isHTML, string ccStr, string bccStr, out bool IsEmailSent)
        {
            return SendMail(bodyStr, subjectStr, toMailId, WebConfigurationManager.AppSettings.Get("SupportEmail"), isHTML, ccStr, bccStr, new Storage.ModuleType(), "",out IsEmailSent);
        }
        public string SendMail(string bodyStr, string subjectStr, string toMailId, string fromMailId, bool isHTML, string ccStr, string bccStr, Storage.ModuleType StorageModuleType, string Attachments, out bool IsEmailSent)
        {
            IsEmailSent = false;
            string Message;
            try
            {
                MailMessage objMail = new MailMessage();
                int i = 0;

                if (!string.IsNullOrEmpty(toMailId))
                {
                    toMailId = toMailId.Replace(";", ",");
                    string[] objTo = toMailId.Split(',');
                    for (i = 0; i < objTo.Length; i++)
                    {
                        if (objTo[i] != "")
                            objMail.To.Add(objTo[i]);
                    }
                }
                if (!string.IsNullOrEmpty(ccStr))
                {
                    ccStr = ccStr.Replace(";", ",");
                    string[] objCc = ccStr.Split(',');
                    for (i = 0; i < objCc.Length; i++)
                    {
                        if (objCc[i] != "")
                            objMail.CC.Add(objCc[i]);
                    }
                }
                if (!string.IsNullOrEmpty(bccStr))
                {
                    bccStr = bccStr.Replace(";", ",");
                    string[] objBCC = bccStr.Split(',');
                    for (i = 0; i < objBCC.Length; i++)
                    {
                        if (objBCC[i] != "")
                            objMail.Bcc.Add(objBCC[i]);
                    }
                }
                if (!string.IsNullOrEmpty(fromMailId))
                {
                    objMail.ReplyToList.Add(fromMailId);
                    objMail.From = new MailAddress(fromMailId, fromMailId);
                }

                objMail.Bcc.Add(WebConfigurationManager.AppSettings["BccMail"].ToString());
                objMail.Subject = subjectStr;
                objMail.Body = bodyStr;

                if (isHTML)
                {
                    objMail.IsBodyHtml = true;
                }

                if (Attachments != "")
                {
                    Attachments = Attachments.Replace(";", ",");
                    string[] strAttachArr = Attachments.Split(',');
                    Storage ObjStorge = new Storage();
                    for (int f = 0; f <= strAttachArr.Length - 1; f++)
                    {
                        if (strAttachArr[f] != "")
                        {
                            string FileName = strAttachArr[f];
                            MemoryStream ms = ObjStorge.FileRead(StorageModuleType, FileName);
                            MemoryStream MSAttach = new MemoryStream(ms.ToArray());
                            objMail.Attachments.Add(new Attachment(MSAttach, FileName, System.Net.Mime.MediaTypeNames.Application.Octet));
                        }
                    }
                }

                SmtpClient mailClient = new SmtpClient();
                mailClient = new SmtpClient(ConfigurationManager.AppSettings.Get("SMTPServer"))
                {
                    UseDefaultCredentials = false,
                    Port = int.Parse(ConfigurationManager.AppSettings.Get("SMTPPort")),
                    Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings.Get("SMTPLogin"),
                    ConfigurationManager.AppSettings.Get("SMTPPassword")),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("SMTPSSL")),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                mailClient.Send(objMail);
                IsEmailSent = true;
                Message = "Mail Send successfully";
            }

            catch (Exception ex)
            {
                Message = ex.Message.ToString().Replace("'", "");
                IsEmailSent = false;
            }
            return Message;
        }
        #endregion

        public void AddLeaveNotification(int leaveId, string empId, string channelName, string channelUniqueId, string channelNotificationUniqueId)
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EmpleaveNotification", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "INSERT");
                    cmd.Parameters.AddWithValue("@leaveid", leaveId);
                    cmd.Parameters.AddWithValue("@empid", empId);
                    cmd.Parameters.AddWithValue("@ChannelName", channelName);
                    cmd.Parameters.AddWithValue("@ChannelUniqueId", channelUniqueId);
                    cmd.Parameters.AddWithValue("@ChannelNotificationUniqueId", channelNotificationUniqueId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (ex)
                        throw;
                    }
                }
            }
        }
        public void AddTimesheetNotification(string uniqueId, string empId, string channelName, string channelUniqueId, string channelNotificationUniqueId)
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EmpTimesheetNotification", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "AddEmpNotification");
                    cmd.Parameters.AddWithValue("@uniqueId", uniqueId);
                    cmd.Parameters.AddWithValue("@empid", empId);
                    cmd.Parameters.AddWithValue("@ChannelName", channelName);
                    cmd.Parameters.AddWithValue("@ChannelUniqueId", channelUniqueId);
                    cmd.Parameters.AddWithValue("@ChannelNotificationUniqueId", channelNotificationUniqueId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }
        public DataSet GetTimesheetNotificationDetails(int Empid)
        {
            try
            {
                DataSet dsResult = new DataSet();
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_EmpTimesheetNotification", con);
                cmd.Parameters.AddWithValue("@Mode", "GetChannelDetails");
                cmd.Parameters.AddWithValue("@empid", Empid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                con.Close();
                return dsResult;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void AddTimesheetNotificationDetails(string uniqueId, string empId, string empName, string fromDate, string toDate,string requested)
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EmpTimesheetNotification", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "AddEmpNotificationDetails");
                    cmd.Parameters.AddWithValue("@uniqueId", uniqueId);
                    cmd.Parameters.AddWithValue("@empid", empId);
                    cmd.Parameters.AddWithValue("@empName", empName);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@requested", requested);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }
        public DataSet GetTsDatabyUniqueId(string uniqueId,int empId, int updatedId)
        {
            try
            {
                DataSet dsResult = new DataSet();
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_EmpTimesheetNotification", con);
                cmd.Parameters.AddWithValue("@Mode", "GetTsDatabyUId");
                cmd.Parameters.AddWithValue("@uniqueId", uniqueId);
                cmd.Parameters.AddWithValue("@empId", empId);
                cmd.Parameters.AddWithValue("@updatedId", updatedId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                con.Close();
                return dsResult;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataSet GetProjectRoleEmailsByEmpIDs(int empId)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(_strConnection))
            using (SqlCommand cmd = new SqlCommand("USP_GetProjectRoleEmailsByEmpID", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", empId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                }
            }

            return ds;
        }


        public DataTable GetProjectRoleEmailsByEmpID(int empId)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("USP_GetProjectRoleEmailsByEmpID", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@EmpID", empId);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }

            return dt;
        }

        #region Check Admin
        public bool CheckAdmin(int EmpId)
        {
            try
            {
                bool Status = false;
                SqlConnection con = new SqlConnection(_strConnection);
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("USP_AI_CheckIsAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", EmpId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Status = Convert.ToBoolean(dt.Rows[0]["IsAdmin"]);
                }
                else
                {
                    return Status;
                }
                return Status;
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
                bool Status = false;
                SqlConnection con = new SqlConnection(_strConnection);
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("USP_AI_CheckIsPMBAAM", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", EmpId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Status = Convert.ToBoolean(dt.Rows[0]["IsAccessible"]);
                }
                else
                {
                    return Status;
                }
                return Status;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}