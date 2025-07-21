using System;

namespace AgoraBL.Models
{
    public class ClsLogin
    {
        public string EmpId { get; set; }
        public string Password { get; set; }
        public int Logintype { get; set; }
        public Boolean IsAdmin = false;
        public Boolean IsModuleAdmin = false;
        public int SkillID;
        public string Name;
        public string EmailID;
        public string Address;
        public string Contact;
        public string JoiningDate;
        public string LeavingDate;
        public string ProbationPeriod;
        public string Notes;
        public string AccountNo;
        public string BDate;
        public string ADate;
        public string PreviousEmployer;
        public int Experince;
        public string LocationID;
        public string ProfileID;
        public int ProfileLocationID = 0;
        public Boolean IsActive = false;
        public string Message = "";
        public string UserType = "";
        public string CurrentAddress;
        public string empConfDate;
        public string Designation;
        public bool status;
        public string empForgotpwdLinkDate = null;
        public string empPassword;
        public bool onbaordingCompleted;
        public bool IsRemoteEmployee;
    }
    public enum LoginType
    {
        AD = 1,
        Agora = 2
    }
}

