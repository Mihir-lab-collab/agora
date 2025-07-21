using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using System.Data;
using nsMobileAPI;

public class EmployeeMaster
{
    public EmployeeMaster()
    {
        //
        // TODO: Add constructor logic here
        //
    }



    public int empid { get; set; }
    public string empName { get; set; }
    public string empAddress { get; set; }
    public string empGender { get; set; }
    public string empContact { get; set; }
    public DateTime? empJoiningDate { get; set; }
    public DateTime? empExpectedLWD { get; set; }
    public DateTime? empLeavingDate { get; set; }
    public int empProbationPeriod { get; set; }
    public string empNotes { get; set; }
    public string empEmail { get; set; }
    public bool empTester { get; set; }
    public string empAccountNo { get; set; }
    public string EmpPAN { get; set; }
    public string EmpUAN { get; set; }
    public string EmpEPF { get; set; }
    public DateTime? empBDate { get; set; }
    public DateTime? empADate { get; set; }
    public string empPrevEmployer { get; set; }
    public int empExperince { get; set; }
    public int intelegainExperince { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsAccountAdmin { get; set; }
    public bool IsPayrollAdmin { get; set; }
    public bool IsPM { get; set; }
    public bool IsProjectReport { get; set; }
    public bool IsProjectStatus { get; set; }
    public bool IsLeaveAdmin { get; set; }
    public bool IsActive { get; set; }
    public int LocationFKID { get; set; }
    public int skillid { get; set; }
    public DateTime? InsertedOn { get; set; }
    public int InsertedBy { get; set; }
    public string InsertedIP { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int ModifiedBy { get; set; }
    public string ModifiedIP { get; set; }
    public bool IsTester { get; set; }
    public string empPassword { get; set; }

    public int AnnualCTC { get; set; }
    public int CTC { get; set; }
    public int Gross { get; set; }
    public int Net { get; set; }
    public string Resume { get; set; }

    public string Qualification { get; set; }
    public string QualificationId { get; set; }
    public string SecSkills { get; set; }
    public string SecSkillsId { get; set; }

    public int PrimarySkill { get; set; }
    public string Designation { get; set; }
    public string Skill { get; set; }

    public string Type { get; set; }
    public byte[] Photo { get; set; }
    public string Mode { get; set; }
    public string EmpStatus { get; set; }
    public string LeavingStatus { get; set; }
    public int ProjectID { get; set; }
    public int SecurityLevel { get; set; }
    public string PrimarySkillDesc { get; set; }
    public string Event { get; set; }
    public int ProfileID { get; set; }
    public string CAddress { get; set; }
    public int HistoryID { get; set; }
    public string ADUserName { get; set; }
    public string IFSCCode { get; set; }
    public string MSTeam { get; set; }
    public bool IsRemoteEmployee { get; set; }

    public string Flag { get; set; } ////For device use
    public string strBirthday { get; set; } ////For device use

    public EmployeeMaster(string LeavingStatus, int ProjectID, int EmployeeID, string Name, int LocationFKID, string Contact,
        string EmailID, string Address, string Gender, string AccountNo, DateTime? JoiningDate, DateTime? BirthDate, int Experience, int IntelegainExperince, DateTime? AnniversaryDate,
        string Notes, string PreviousEmployer, int SkillId, int PrimarySkill, int ProbationPeriod, DateTime? LeavingDate,
        string PrimarySkillDesc, int SecurityLevel, string Skill, int AnnualCTC, int Gross, int CTC, int Net, string EmpStatus, string Resume,
        bool Tester, bool IsActive, bool IsLeaveAdmin, bool IsProjectStatus, bool IsProjectReport, bool IsPM, bool IsPayrollAdmin, bool IsAccountAdmin,
        bool IsSuperAdmin, string Qualification, string QualificationId, string SecSkills, string SecSkillsId, int ProfileID, string IFSCCode, bool isRemoteEmployee)
    {
        this.LeavingStatus = LeavingStatus;
        this.ProjectID = ProjectID;
        this.empid = EmployeeID;
        this.empName = Name;
        this.LocationFKID = LocationFKID;
        this.empContact = Contact;
        this.empEmail = EmailID;
        this.empAddress = Address;
        this.empGender = Gender;
        this.empAccountNo = AccountNo;
        this.empJoiningDate = JoiningDate;
        this.empBDate = BirthDate;
        this.empExperince = Experience;
        this.intelegainExperince = IntelegainExperince;
        this.empADate = AnniversaryDate;
        this.empNotes = Notes;
        this.empPrevEmployer = PreviousEmployer;
        this.skillid = SkillId;
        this.PrimarySkill = PrimarySkill;
        this.empProbationPeriod = ProbationPeriod;
        this.empLeavingDate = LeavingDate;
        this.PrimarySkillDesc = PrimarySkillDesc;
        this.SecurityLevel = SecurityLevel;
        this.Skill = Skill;
        this.AnnualCTC = AnnualCTC;
        this.Gross = Gross;
        this.CTC = CTC;
        this.Net = Net;
        this.EmpStatus = EmpStatus;
        this.Resume = Resume;
        this.empTester = Tester;
        this.IsActive = IsActive;
        this.IsLeaveAdmin = IsLeaveAdmin;
        this.IsProjectStatus = IsProjectStatus;
        this.IsProjectReport = IsProjectReport;
        this.IsPM = IsPM;
        this.IsPayrollAdmin = IsPayrollAdmin;
        this.IsAccountAdmin = IsAccountAdmin;
        this.IsSuperAdmin = IsSuperAdmin;
        this.Qualification = Qualification;
        this.QualificationId = QualificationId;
        this.SecSkills = SecSkills;
        this.SecSkillsId = SecSkillsId;
        this.ProfileID = ProfileID;
        this.IFSCCode = IFSCCode;
       this.IsRemoteEmployee = isRemoteEmployee;
    }//, string ProfileName


    public EmployeeMaster(ref int intEmpID, int strLocation, string strName, string strAddress, string strContact, int strSkill, string strNotes,
    DateTime? strJoiningDate, int strProbationPeriod, string strEmail, string strAccountno, DateTime? strBDate, DateTime? strADate,
    string strPrevEmployer, int intExperince, DateTime? strInsertedOn, int intInsertedBy, string strInsertedIP,
    string strType, DateTime? strLeavingDate, string strResume, byte[] strPhoto, int primarySkillId, string Empstatus, string strQualification, string strQualificationId, string strSecSkills, string strSecSkillsId, string IFSCCode, bool isRemoteEmployee)
    {
        this.empid = intEmpID;
        this.LocationFKID = strLocation;
        this.empName = strName;
        this.empAddress = strAddress;
        this.empContact = strContact;
        this.skillid = strSkill;
        this.empNotes = strNotes;
        this.empJoiningDate = strJoiningDate;
        this.empProbationPeriod = strProbationPeriod;
        this.empEmail = strEmail;
        this.empAccountNo = strAccountno;
        this.empBDate = strBDate;
        this.empADate = strADate;
        this.empPrevEmployer = strPrevEmployer;
        this.empExperince = intExperince;
        this.InsertedOn = strInsertedOn;
        this.InsertedBy = intInsertedBy;
        this.InsertedIP = strInsertedIP;
        this.Type = strType;
        this.empLeavingDate = strLeavingDate;
        this.Resume = strResume;
        this.Photo = strPhoto;
        this.PrimarySkill = primarySkillId;
        this.EmpStatus = EmpStatus;
        this.Qualification = strQualification;
        this.QualificationId = strQualificationId;
        this.SecSkills = strSecSkills;
        this.SecSkillsId = strSecSkillsId;
        this.IFSCCode = IFSCCode;
        this.IsRemoteEmployee = IsRemoteEmployee;
    }


    public EmployeeMaster(int _empid, string _empPassword, int _skillid, string _empName, string _empAddress, string _empContact, DateTime? _empJoiningDate, DateTime? _empLeavingDate, int _empProbationPeriod, string _empNotes, string _empEmail, bool _empTester, string _empAccountNo, DateTime? _empBDate, DateTime? _empADate, string _empPrevEmployer, int _empExperince, bool _IsSuperAdmin, bool _IsAccountAdmin, bool _IsPayrollAdmin, bool _IsPM, bool _IsTester, bool _IsProjectReport, bool _IsProjectStatus, bool _IsLeaveAdmin, bool _IsActive, DateTime? _InsertedOn, int _InsertedBy, string _InsertedIP, DateTime? _ModifiedOn, int _ModifiedBy, string _ModifiedIP, int _LocationFKID,string _empGender,bool _IsRemoteEmployee)
    {
        this.empid = _empid;
        this.empPassword = _empPassword;
        this.skillid = _skillid;
        this.empName = _empName;
        this.empAddress = _empAddress;
        this.empContact = _empContact;
        this.empJoiningDate = _empJoiningDate;
        this.empLeavingDate = _empLeavingDate;
        this.empProbationPeriod = _empProbationPeriod;
        this.empNotes = _empNotes;
        this.empEmail = _empEmail;
        this.empTester = _empTester;
        this.empAccountNo = _empAccountNo;
        this.empBDate = _empBDate;
        this.empADate = _empADate;
        this.empPrevEmployer = _empPrevEmployer;
        this.empExperince = _empExperince;
        this.IsSuperAdmin = _IsSuperAdmin;
        this.IsAccountAdmin = _IsAccountAdmin;
        this.IsPayrollAdmin = _IsPayrollAdmin;
        this.IsPM = _IsPM;
        this.IsTester = _IsTester;
        this.IsProjectReport = _IsProjectReport;
        this.IsProjectStatus = _IsProjectStatus;
        this.IsLeaveAdmin = _IsLeaveAdmin;
        this.IsActive = _IsActive;
        this.InsertedOn = _InsertedOn;
        this.InsertedBy = _InsertedBy;
        this.InsertedIP = _InsertedIP;
        this.ModifiedOn = _ModifiedOn;
        this.ModifiedBy = _ModifiedBy;
        this.ModifiedIP = _ModifiedIP;
        this.LocationFKID = _LocationFKID;
        this.empGender = _empGender;
        this.IsRemoteEmployee = _IsRemoteEmployee;
    }

    public static EmployeeMaster GetEmpDetailsByEmployeeId(int empId)
    {
        EmployeeMasterDAL objEmpDtl = new EmployeeMasterDAL();
        return objEmpDtl.GetEmpDetailsByEmployeeId(empId);

    }

    public static List<EmployeeMaster> GetEmpDetailsByProjId(int ProjId)
    {
        EmployeeMasterDAL objGetEmpDetailsByProjId = new EmployeeMasterDAL();
        return objGetEmpDetailsByProjId.GetEmpDetailsByProjId(ProjId);

    }
    public static EmployeeMaster GetEmpMailIDByReportID(int ReportID)
    {
        EmployeeMasterDAL objGetEmpMailIDByReportId = new EmployeeMasterDAL();
        return objGetEmpMailIDByReportId.GetEmpMailIDByReportID(ReportID);

    }
    public static List<EmployeeMaster> GetAllEmployees()
    {
        EmployeeMasterDAL objEmployeeMaster = new EmployeeMasterDAL();
        return objEmployeeMaster.GetAllEmployees();
    }

    public static List<EmployeeMaster> GetAllAccountMgr()
    {
        EmployeeMasterDAL objEmployeeMaster = new EmployeeMasterDAL();
        return objEmployeeMaster.GetAllAccountMgrs();
    }

    public static List<EmployeeMaster> GetEmployeeJoinLeaveDate(string mode, int empid)
    {
        EmployeeMasterDAL objEmployeeMaster = new EmployeeMasterDAL();
        return objEmployeeMaster.GetEmployeeJoinLeaveDate(mode, empid);
    }
    public EmployeeMaster(int _empId, string _empName)
    {
        this.empid = _empId;
        this.empName = _empName;
        //this.empAddress = _empAddress;
        //this.InsertedOn = _InsertedOn;
        //this.ModifiedOn = _ModifiedOn;
    }
    public EmployeeMaster(DateTime leavingDate, DateTime joiningDate)
    {
        this.empLeavingDate = leavingDate;
        this.empJoiningDate = joiningDate;
    }


    public static List<EmployeeMaster> GetEmployeeDetails(string mode, int locId, string strLStatus)//, int ProjId
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.GetEmployeeDetails(mode, locId, strLStatus);//, ProjId
    }

    public static int Save(int intEmpID, int strLocation, string strName, string strAddress, string strGender, string strContact, int strSkill, string strNotes,
   DateTime strJoiningDate, int strProbationPeriod, string strEmail, string strAccountno, DateTime? strBDate, DateTime? strADate,DateTime? strExpectedLWD,
   string strPrevEmployer, int intExperince, DateTime? strInsertedOn, int intInsertedBy, string strInsertedIP,
   string strType, DateTime? strLeavingDate, string strResume, byte[] strPhoto, int primarySkillId, string empstatus, int intProfileID, string strCAddress, string strADUserName, string strPAN, string strUAN, string strEPFAccountNo, string IFSCCode, string strMSTeamID, bool IsRemoteEmployee) //new IFSCCode
    {
        try
        {

            EmployeeMaster objEmployee = new EmployeeMaster();
            objEmployee.empid = intEmpID;
            objEmployee.LocationFKID = strLocation;
            objEmployee.empName = strName;
            objEmployee.empAddress = strAddress;
            objEmployee.empGender = strGender;
            objEmployee.empContact = strContact;
            objEmployee.skillid = strSkill;
            objEmployee.empNotes = strNotes;
            objEmployee.empJoiningDate = strJoiningDate;
            objEmployee.empProbationPeriod = strProbationPeriod;
            objEmployee.empEmail = strEmail;
            objEmployee.empAccountNo = strAccountno;
            objEmployee.EmpPAN = strPAN;
            objEmployee.EmpUAN = strUAN;
            objEmployee.EmpEPF = strEPFAccountNo;
            objEmployee.empBDate = strBDate;
            objEmployee.empADate = strADate;
            objEmployee.empExpectedLWD = strExpectedLWD;
            objEmployee.empPrevEmployer = strPrevEmployer;
            objEmployee.empExperince = intExperince;
            objEmployee.InsertedOn = strInsertedOn;
            objEmployee.InsertedBy = intInsertedBy;
            objEmployee.InsertedIP = strInsertedIP;
            objEmployee.Type = strType;
            objEmployee.empLeavingDate = strLeavingDate;
            objEmployee.Resume = strResume;
            objEmployee.Photo = strPhoto;
            objEmployee.PrimarySkill = primarySkillId;
            objEmployee.EmpStatus = empstatus;
            objEmployee.ProfileID = intProfileID;
            objEmployee.CAddress = strCAddress;
            objEmployee.ADUserName = strADUserName;
            objEmployee.IFSCCode = IFSCCode;
            objEmployee.MSTeam = strMSTeamID;
            objEmployee.IsRemoteEmployee = IsRemoteEmployee;
            
            if (intEmpID > 0)
                objEmployee.Mode = "Update";
            else
                objEmployee.Mode = "Save";

            EmployeeMasterDAL objEmployeeDAL = new EmployeeMasterDAL();
            return objEmployeeDAL.SaveEmployee(objEmployee);

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.StackTrace);
        }

        return 0;
    }

    public static void SaveSecondarySkills(int intEmpId, int intTechSkillId, string strMode)
    {
        try
        {
            new EmployeeMasterDAL().SaveSecondarySkills(intEmpId, intTechSkillId, strMode);

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static void SaveQualification(int intEmpId, int intQualificationId, string strMode)
    {
        try
        {
            new EmployeeMasterDAL().SaveQualification(intEmpId, intQualificationId, strMode);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    //public static string GetSecondarySkills(int intEmpId, int intTechSkillId, string strMode)
    //{
    //    try
    //    {
    //        return new EmployeeMasterDAL().GetSecondarySkills(intEmpId, intTechSkillId, strMode);

    //    }
    //    catch (Exception ex)
    //    {
    //        throw new ApplicationException(ex.Message);
    //    }
    //}

    //public static string GetQualification(int intEmpId, int intQualificationId, string strMode)
    //{
    //    try
    //    {
    //        return new EmployeeMasterDAL().GetQualification(intEmpId, intQualificationId, strMode);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new ApplicationException(ex.Message);
    //    }
    //}

    public static string GetExistsClient(int intEmpid, string Emailid, string strMode)
    {
        try
        {
            return new EmployeeMasterDAL().GetExistsClient(intEmpid, Emailid, strMode);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static EmployeeMaster GetEmployeeDetails(int empId)
    {
        EmployeeMasterDAL objEmpDt = new EmployeeMasterDAL();
        return objEmpDt.GetEmployeeDetails(empId);
    }

    public static List<EmployeeMaster> SelectEvents(string mode, int locId, int days)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.SelectEvent(mode, locId, days);
    }

    public EmployeeMaster(string _empName, string _type, string _Event, DateTime _DOB)
    {

        this.empName = _empName;
        this.Type = _type;
        this.Event = _Event;
        this.empBDate = _DOB;
    }




    public static List<EmpProfile> BindProfile(int locationId)
    {
        List<EmpProfile> objProfile = new List<EmpProfile>();
        objProfile = new EmployeeMasterDAL().BindProfile(locationId, "GetProfile");
        return objProfile;
    }


    //public List<EmpProfile> BindProfile(int locationId)
    //{
    //    List<EmpProfile> profileList = new List<EmpProfile>();
    //    profileList = new EmployeeMasterDAL().BindProfile(locationId, "GetProfile");
    //    return profileList;
    //}




    public static List<EmployeeMaster> GetTimeSheet(string mode, int empID)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.GetTimeSheet(mode, empID);
    }

    public static List<EmployeeMaster> GetEmpHistory(string mode, int empID)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.GetEmpHistory(mode, empID);
    }

    public static string SaveApprovedData(string mode, int empID, int HID, int approvalStatus, int modifiedBy)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.SaveApprovedData(mode, empID, HID, approvalStatus, modifiedBy);
    }

    public string SaveEmpProfile(string mode, int empID, string cAddress, string contact, string aDate, int modifiedBy)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.SaveEmpProfile(mode, empID, cAddress, contact, aDate, modifiedBy);
    }

    public string GetEditedProfileStatus(string mode, int empID)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.GetEditedProfileStatus(mode, empID);
    }

    public static object GetAllAppraisalAuth()
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.GetAllAppraisalAuth();
    }

    public static string GetExistsADUserName(int intEmpid, string ADUserName, string strMode)
    {
        try
        {
            return new EmployeeMasterDAL().GetExistsADUserName(intEmpid, ADUserName, strMode);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    /// <summary>
    /// Get Exists ADUserName
    /// Added by bhavana 24-06-2016
    /// </summary>
    /// <param name="intEmpid">emp id</param>
    /// <param name="strMode">mode</param>
    /// <returns>AD user name</returns>
    public static string GetExistsADUserName(int intEmpid, string strMode)
    {
        try
        {
            return new EmployeeMasterDAL().GetExistsADUserName(intEmpid, strMode);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    /// <summary>
    /// This method is used to get employee details from (Employee master and FavouriteEmp)
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="empName"></param>
    /// <param name="empId"></param>
    /// <returns>Return  employee details </returns>
    public static DataTable GetEmpDetails(string mode, string empName, int empId)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.GetEmpDetails(mode, empName, empId);
    }

    /// <summary>
    /// This method used to insert InsertFavouriteEmployee.
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="empID"></param>
    /// <param name="FavouriteEmpId"></param>
    /// <returns>Result</returns>
    public static DataTable InsertFavouriteEmployee(string mode, int empID, int favouriteEmpId)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.InsertFavouriteEmployee(mode, empID, favouriteEmpId);
    }

    /// <summary>
    /// This method used to get favourite employee 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="empID"></param>
    /// <returns>Return favourite employee</returns>
    public static DataTable GetFavouriteEmployees(string mode, int empID)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.GetFavouriteEmployees(mode, empID);
    }

    /// <summary>
    /// This method to used to delete  favourite employee
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="empID"></param>
    /// <param name="favouriteEmpId"></param>
    /// <returns>Return Message</returns>
    public static DataTable DeleteFavouriteEmployee(string mode, int empID, int favouriteEmpId)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.DeleteFavouriteEmployee(mode, empID, favouriteEmpId);
    }
    //Added By Nikhil Shetye
    public string UploadImage(string mode,int empID,byte[] byteImage)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.UploadImage(mode,empID, byteImage);
    }
    //End Nikhil Shetye
    public static UserIdentityData GetEmpDetailsbyId(string UserId, string Name)
    {
        EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        return objEmployee.GetEmpDetailsbyId(UserId, Name);
    }
}

public class EmpProfile
{
    public int ProfileId { get; set; }
    public string ProfileName { get; set; }
}
