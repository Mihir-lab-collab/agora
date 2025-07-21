using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI.API.Models
{
    public class UserIdentityData: ClsException
    {

        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public int? ProfileId { get; set; }
        //public string EmpPassword { get; set; }
        public int? SkillId { get; set; }
        public string EmpAddress { get; set; }
        public string EmpContact { get; set; }
        public string EmpJoiningDate { get; set; }
        public string EmpLeavingDate { get; set; }
        public int? EmpProbationPeriod { get; set; }
        public string EmpNotes { get; set; }
        public string EmpEmail { get; set; }
        public int EmpTester { get; set; }
        public string EmpAccountNo { get; set; }
        public string EmpBDate { get; set; }
        public string EmpADate { get; set; }
        public string EmpPrevEmployer { get; set; }
        public int EmpExperience { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsAccountAdmin { get; set; }
        public bool IsPayrollAdmin { get; set; }
        public bool IsPM { get; set; }
        public bool IsTester { get; set; }
        public bool IsProjectReport { get; set; }
        public bool IsProjectStatus { get; set; }
        public bool IsLeaveAdmin { get; set; }
        public bool IsActive { get; set; }
        public string InsertedOn { get; set; }
        public int InsertedBy { get; set; }
        public string InsertedIP { get; set; }
        public string ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedIP { get; set; }
        public int LocationFKID { get; set; }
        public string Resume { get; set; }
        public string Photo { get; set; }
        public int PrimarySkillId { get; set; }
        public int EmployeeStatusId { get; set; }
        public string EmpGender { get; set; }
        public string ADUserName { get; set; }
        public string EmpPAN { get; set; }
        public string EmpUAN { get; set; }
        public string EmpEPF { get; set; }
        public string EmpFatherName { get; set; }
        public string EmpToken { get; set; }
        public string DeviceId { get; set; }
        public string OsType { get; set; }
        public string EmpForgotPwdLinkDate { get; set; }
        public string SkypeId { get; set; }
        public string EmpExpectedLWD { get; set; }
        public string IFSCCode { get; set; }
        public decimal LeavePLOpeningBalance { get; set; }
        public string ProjectGroupEmail { get; set; }
        public string MSTeam { get; set; }
        public DateTime? DurationFrom { get; set; }
        public DateTime? DurationTo { get; set; }
        private bool _IsSuccess = false;
        public bool IsSuccess
        {
            get { return _IsSuccess; }
            set { _IsSuccess = value; }
        }

        public string StatusMessage { get; set; }
        public string UserIdentity{ get; set; }
        public string SkillDesc { get; set; }

    }
    public enum UserIdentity
    {
        SkypeId,
        MSTeam,
        EmpEmail
    }
    public class Entity
    {
        public int EntityID { get; set; }
        public string EntityName { get; set; }
        public DateTime? DurationFrom { get; set; }
        public DateTime? DurationTo { get; set; }
    }
}