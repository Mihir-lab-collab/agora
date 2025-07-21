using AI.API.API.DAL;
using AI.API.Common;
using AI.API.Models;
using System;
using System.Collections.Generic;

namespace AI.API.API.BAL
{
    public class EmployeeMasterBAL
    {
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
                Consolidate_Leaves = objEmployee.GetLeaveDetailsByHR(entity);
            }
            catch (Exception)
            {

                throw;
            }
            return Consolidate_Leaves;
        }
        public TimesheetTotalHours GetTimesheetDetailsTotalHoursByHR(Entity entity)
        {
            TimesheetTotalHours timesheetTotalHours = new TimesheetTotalHours();
            try
            {
                timesheetTotalHours = objEmployee.GetTimesheetDetailsTotalHoursByHR(entity);
            }
            catch (Exception)
            {

                throw;
            }
            return timesheetTotalHours;
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
        public TimesheetTotalHours GetTimesheetDetailsTotalHours(UserIdentityData userIdentityData)
        {
            TimesheetTotalHours timesheetTotalHours = new TimesheetTotalHours();
            try
            {
                timesheetTotalHours = objEmployee.GetTimesheetDetailsTotalHours(userIdentityData);
            }
            catch (Exception)
            {

                throw;
            }
            return timesheetTotalHours;
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
        #endregion

    }
}