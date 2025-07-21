using AI.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace AI.API.API.DAL
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
                cmd.Parameters.AddWithValue("@EmployeeName", entity.EntityName);
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
        public ConsolidateLeaves GetLeaveDetailsByHR(Entity entity)
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
        public TimesheetTotalHours GetTimesheetDetailsTotalHoursByHR(Entity entity)
        {
            TimesheetTotalHours timesheetTotalHours = new TimesheetTotalHours();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_HR_Timesheet_Hours", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpName", entity.EntityName);
                cmd.Parameters.AddWithValue("@DurationFrom", entity.DurationFrom);
                cmd.Parameters.AddWithValue("@DurationTo", entity.DurationTo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    timesheetTotalHours = new TimesheetTotalHours
                    {
                        EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["empId"]),
                        EmpName = Convert.ToString(ds.Tables[0].Rows[0]["empName"]),
                        TotalHours = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalHours"])
                    };

                }
                return timesheetTotalHours;
            }
            catch (Exception)
            {
                throw;
            }
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
                            EmpName = dt.Rows[i]["empName"].ToString(),
                            ProjName = dt.Rows[i]["projName"].ToString(),
                            TsComment = dt.Rows[i]["tsComment"].ToString(),
                            TsEntryDate = dt.Rows[i]["tsEntryDate"].ToString(),
                            TsHour = dt.Rows[i]["tsHour"].ToString(),
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
        public TimesheetTotalHours GetTimesheetDetailsTotalHours(UserIdentityData userIdentityData)
        {
            TimesheetTotalHours timesheetTotalHours = new TimesheetTotalHours();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);

                SqlCommand cmd = new SqlCommand("USP_AI_Emp_Timesheet_Hours", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", userIdentityData.EmpID);
                cmd.Parameters.AddWithValue("@DurationFrom", userIdentityData.DurationFrom);
                cmd.Parameters.AddWithValue("@DurationTo", userIdentityData.DurationTo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    timesheetTotalHours = new TimesheetTotalHours
                    {
                        EmpId = Convert.ToInt32(dt.Rows[0]["empId"]),
                        EmpName = Convert.ToString(dt.Rows[0]["empName"]),
                        TotalHours = Convert.ToInt32(dt.Rows[0]["TotalHours"])
                    };

                }
                return timesheetTotalHours;
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
        #endregion

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
        #endregion
    }
}