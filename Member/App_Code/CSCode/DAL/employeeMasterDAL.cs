using nsMobileAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;



public class EmployeeMasterDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public EmployeeMasterDAL()
    {
    }
    public EmployeeMaster GetEmpDetailsByEmployeeId(int empId)
    {

        EmployeeMaster objGetEmpDetailsByEmployeeId = new EmployeeMaster();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("Sp_GetEmpDetailsByEmployeeId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpId", empId);
            SqlDataReader reader = null;
            Nullable<DateTime> dt = null;


            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objGetEmpDetailsByEmployeeId = new EmployeeMaster(
                    Convert.ToInt32(reader["empid"].ToString()), reader["empPassword"].ToString(),
                    Convert.ToInt32(reader["skillid"].ToString()), reader["empName"].ToString(), reader["empAddress"].ToString(),
                    reader["empContact"].ToString(), string.IsNullOrEmpty(Convert.ToString(reader["empJoiningDate"])) ? dt : Convert.ToDateTime(reader["empJoiningDate"]),
                    string.IsNullOrEmpty(Convert.ToString(reader["empLeavingDate"])) ? dt : Convert.ToDateTime(reader["empLeavingDate"]),
                    Convert.ToInt32(reader["empProbationPeriod"].ToString()), reader["empNotes"].ToString(),
                    reader["empEmail"].ToString(), Convert.ToBoolean(reader["empTester"].ToString()), reader["empAccountNo"].ToString(),
                    string.IsNullOrEmpty(Convert.ToString(reader["empBDate"])) ? dt : Convert.ToDateTime(reader["empBDate"]),
                    string.IsNullOrEmpty(Convert.ToString(reader["empADate"])) ? dt : Convert.ToDateTime(reader["empADate"]),
                    reader["empPrevEmployer"].ToString(), Convert.ToInt32(reader["empExperince"].ToString()),
                    Convert.ToBoolean(reader["IsSuperAdmin"].ToString()), Convert.ToBoolean(reader["IsAccountAdmin"].ToString()), Convert.ToBoolean(reader["IsPayrollAdmin"].ToString()),
                    Convert.ToBoolean(reader["IsPM"].ToString()), Convert.ToBoolean(reader["IsTester"].ToString()),
                    Convert.ToBoolean(reader["IsProjectReport"].ToString()), Convert.ToBoolean(reader["IsProjectStatus"].ToString()), Convert.ToBoolean(reader["IsLeaveAdmin"].ToString()),
                    Convert.ToBoolean(reader["IsActive"].ToString()),
                    string.IsNullOrEmpty(Convert.ToString(reader["InsertedOn"])) ? dt : Convert.ToDateTime(reader["InsertedOn"]),
                    Convert.ToInt32(reader["InsertedBy"].ToString()), reader["InsertedIP"].ToString(),
                    string.IsNullOrEmpty(Convert.ToString(reader["ModifiedOn"])) ? dt : Convert.ToDateTime(reader["ModifiedOn"]),
                   string.IsNullOrEmpty(Convert.ToString(reader["ModifiedBy"])) ? 0 : Convert.ToInt32(reader["ModifiedBy"]), reader["ModifiedIP"].ToString(), string.IsNullOrEmpty(Convert.ToString(reader["LocationFKID"])) ? 0 : Convert.ToInt32(reader["LocationFKID"])
                   , _empGender: string.IsNullOrEmpty(Convert.ToString(reader["empGender"])) ? string.Empty : Convert.ToString(reader["empGender"])
                   , _IsRemoteEmployee: Convert.ToBoolean(string.IsNullOrEmpty(Convert.ToString(reader["isremoteemployee"])) ? "0" : Convert.ToString(reader["isremoteemployee"]))

                       );

                }
            }
        }
        catch (Exception ex)
        { }
        return objGetEmpDetailsByEmployeeId;
    }

    public List<EmployeeMaster> GetEmpDetailsByProjId(int ProjId)
    {

        List<EmployeeMaster> objGetEmpDetailsByProjId = new List<EmployeeMaster>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("Sp_GetEmpDetailsByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjId", ProjId);
            SqlDataReader reader = null;
            Nullable<DateTime> dt = null;


            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objGetEmpDetailsByProjId.Add(new EmployeeMaster(
                    Convert.ToInt32(reader["empid"].ToString()), reader["empPassword"].ToString(),
                    Convert.ToInt32(reader["skillid"].ToString()), reader["empName"].ToString(), reader["empAddress"].ToString(),
                    reader["empContact"].ToString(), string.IsNullOrEmpty(Convert.ToString(reader["empJoiningDate"])) ? dt : Convert.ToDateTime(reader["empJoiningDate"]),
                    string.IsNullOrEmpty(Convert.ToString(reader["empLeavingDate"])) ? dt : Convert.ToDateTime(reader["empLeavingDate"]),
                    Convert.ToInt32(reader["empProbationPeriod"].ToString()), reader["empNotes"].ToString(),
                    reader["empEmail"].ToString(), Convert.ToBoolean(reader["empTester"].ToString()), reader["empAccountNo"].ToString(),
                    string.IsNullOrEmpty(Convert.ToString(reader["empBDate"])) ? dt : Convert.ToDateTime(reader["empBDate"]),
                    string.IsNullOrEmpty(Convert.ToString(reader["empADate"])) ? dt : Convert.ToDateTime(reader["empADate"]),
                    reader["empPrevEmployer"].ToString(), Convert.ToInt32(reader["empExperince"].ToString()),
                    Convert.ToBoolean(reader["IsSuperAdmin"].ToString()), Convert.ToBoolean(reader["IsAccountAdmin"].ToString()), Convert.ToBoolean(reader["IsPayrollAdmin"].ToString()),
                    Convert.ToBoolean(reader["IsPM"].ToString()), Convert.ToBoolean(reader["IsTester"].ToString()),
                    Convert.ToBoolean(reader["IsProjectReport"].ToString()), Convert.ToBoolean(reader["IsProjectStatus"].ToString()), Convert.ToBoolean(reader["IsLeaveAdmin"].ToString()),
                    Convert.ToBoolean(reader["IsActive"].ToString()),
                    string.IsNullOrEmpty(Convert.ToString(reader["InsertedOn"])) ? dt : Convert.ToDateTime(reader["InsertedOn"]),
                    Convert.ToInt32(reader["InsertedBy"].ToString()), reader["InsertedIP"].ToString(),
                    string.IsNullOrEmpty(Convert.ToString(reader["ModifiedOn"])) ? dt : Convert.ToDateTime(reader["ModifiedOn"]),
                   string.IsNullOrEmpty(Convert.ToString(reader["ModifiedBy"])) ? 0 : Convert.ToInt32(reader["ModifiedBy"]), reader["ModifiedIP"].ToString(), string.IsNullOrEmpty(Convert.ToString(reader["LocationFKID"])) ? 0 : Convert.ToInt32(reader["LocationFKID"])
                   , _empGender: string.IsNullOrEmpty(Convert.ToString(reader["empGender"])) ? string.Empty : Convert.ToString(reader["empGender"])
                   , _IsRemoteEmployee: Convert.ToBoolean(string.IsNullOrEmpty(Convert.ToString(reader["isremoteemployee"])) ? "0" : Convert.ToString(reader["isremoteemployee"]))
                    ));

                }
            }
        }
        catch (Exception ex)
        { }
        return objGetEmpDetailsByProjId;
    }

    public EmployeeMaster GetEmpMailIDByReportID(int ReportID)
    {
        EmployeeMaster curGetEmpMailIDByReportID = new EmployeeMaster();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("GetProjectReportToEmail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ReportID", ReportID);

        SqlDataReader reader = null;
        Nullable<DateTime> dt = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curGetEmpMailIDByReportID = new EmployeeMaster(
                    0,
                    "",
                    0,
                    "",
                    "",
                   "",
                  dt,
                   dt,
                   0,
                    "",
                    reader["empEmail"].ToString(),
                    false,
                    "",
                   dt,
                   dt,
                    "",
                  0,
                   false,
                   false,
                    false,
                    false,
                    false,
                    false,
                   false,
                   false,
                   false,
                   dt,
                    0,
                    "",
                   dt,
                   0,
                   "",
                  0, _empGender: string.Empty, _IsRemoteEmployee: false
                    );
            }
        }
        return curGetEmpMailIDByReportID;
    }
    public List<EmployeeMaster> GetAllEmployees()
    {
        List<EmployeeMaster> curEmployeeMaster = new List<EmployeeMaster>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("sp_GetAllEmployees", con);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curEmployeeMaster.Add(new EmployeeMaster(
                    Convert.ToInt32(reader["empid"]),
                    reader["empName"].ToString()
                    //reader["empAddress"].ToString(),
                    //reader["InsertedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                    //reader["ModifiedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"].ToString())
                    ));
            }
        }
        return curEmployeeMaster;
    }


    public List<EmployeeMaster> GetAllAccountMgrs()
    {
        List<EmployeeMaster> curEmployeeMaster = new List<EmployeeMaster>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "Select");
        cmd.Parameters.AddWithValue("@LeavingStatus", "Current");
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EmployeeMaster EM = new EmployeeMaster();
                EM.empid = Convert.ToInt32(reader["empid"]);
                EM.empName = reader["empName"].ToString();
                curEmployeeMaster.Add(EM);
            }
        }
        return curEmployeeMaster;
    }

    public List<EmployeeMaster> GetEmployeeJoinLeaveDate(string mode, int empid)
    {
        List<EmployeeMaster> curEmployeeMaster = new List<EmployeeMaster>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmployeeID", empid);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curEmployeeMaster.Add(new EmployeeMaster(
                    reader["empLeavingDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["empLeavingDate"].ToString()),
                    reader["empJoiningDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["empJoiningDate"].ToString())
                    ));
            }
        }
        return curEmployeeMaster;
    }



    public List<EmployeeMaster> GetEmployeeDetails(string mode, int locId, string strLStatus)//, int ProjId
    {
        List<EmployeeMaster> lstEmployee = new List<EmployeeMaster>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);

        if (locId > 0)
            cmd.Parameters.AddWithValue("@LocationFKID", locId);
        if (strLStatus != string.Empty)
            cmd.Parameters.AddWithValue("@LeavingStatus", strLStatus);

        //if (ProjId > 0)
        //    cmd.Parameters.AddWithValue("@ProjectID", ProjId);

        cmd.Parameters.AddWithValue("@mode", mode);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        Nullable<DateTime> dt = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EmployeeMaster obj = new EmployeeMaster();
                obj.empAccountNo = (reader["empAccountNo"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["empAccountNo"]);
                obj.EmpPAN = (reader["EmpPAN"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["EmpPAN"]);
                obj.EmpUAN = (reader["EmpUAN"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["EmpUAN"]);
                obj.EmpEPF = (reader["EmpEPF"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["EmpEPF"]);
                obj.empAddress = Convert.ToString(reader["empAddress"]);
                obj.empGender = Convert.ToString(reader["empGender"]);
                obj.empADate = string.IsNullOrEmpty(Convert.ToString(reader["empADate"])) ? dt : Convert.ToDateTime(reader["empADate"]);
                obj.CAddress = Convert.ToString(reader["EmpCurrentAddress"]);
                obj.empBDate = string.IsNullOrEmpty(Convert.ToString(reader["empBDate"])) ? dt : Convert.ToDateTime(reader["empBDate"]);
                obj.empContact = Convert.ToString(reader["empContact"]);
                obj.empEmail = Convert.ToString(reader["empEmail"]);
                obj.empid = Convert.ToInt32(reader["empid"]);
                obj.empExperince = Convert.ToInt32(reader["empExperince"]);
                obj.intelegainExperince = Convert.ToInt32(reader["IntelegainExperince"]);
                obj.empJoiningDate = string.IsNullOrEmpty(Convert.ToString(reader["empJoiningDate"])) ? dt : Convert.ToDateTime(reader["empJoiningDate"]);
                obj.empLeavingDate = string.IsNullOrEmpty(Convert.ToString(reader["empLeavingDate"])) ? dt : Convert.ToDateTime(reader["empLeavingDate"]);
                obj.LocationFKID = (reader["LocationFKID"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["LocationFKID"]);
                obj.empName = Convert.ToString(reader["empName"]);
                obj.empNotes = (reader["empNotes"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["empNotes"]);
                obj.empPrevEmployer = (reader["empPrevEmployer"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["empPrevEmployer"]);
                obj.skillid = Convert.ToInt32(reader["SkillId"]);
                obj.Designation = Convert.ToString(reader["Designation"]);
                obj.empProbationPeriod = Convert.ToInt32(reader["empProbationPeriod"]);
                obj.PrimarySkill = (reader["PrimarySkillId"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["PrimarySkillId"]);
                obj.IsSuperAdmin = Convert.ToBoolean(reader["IsSuperAdmin"]);
                obj.IsAccountAdmin = Convert.ToBoolean(reader["IsAccountAdmin"]);
                obj.IsPayrollAdmin = Convert.ToBoolean(reader["IsPayrollAdmin"]);
                obj.IsPM = Convert.ToBoolean(reader["IsPM"]);
                obj.IsProjectReport = Convert.ToBoolean(reader["IsProjectReport"]);
                obj.IsProjectStatus = Convert.ToBoolean(reader["IsProjectStatus"]);
                obj.IsLeaveAdmin = Convert.ToBoolean(reader["IsLeaveAdmin"]);
                obj.IsActive = Convert.ToBoolean(reader["IsActive"]);
                obj.empTester = Convert.ToBoolean(reader["empTester"]);
                obj.Resume = (reader["Resume"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["Resume"]);
                obj.EmpStatus = (reader["EmployeeStatus"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["EmployeeStatus"]);

                obj.ProjectID = Convert.ToInt32(reader["projId"]);
                obj.Net = (reader["Net"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["Net"]);

                obj.CTC = (reader["CTC"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["CTC"]);

                obj.Gross = (reader["Gross"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["Gross"]);

                obj.AnnualCTC = (reader["AnnualCTC"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["AnnualCTC"]);
                obj.Skill = Convert.ToString(reader["Skill"]);
                obj.SecurityLevel = Convert.ToInt32(reader["SecurityLevel"]);
                obj.PrimarySkillDesc = Convert.ToString(reader["PrimarySkill"]);

                obj.Qualification = Convert.ToString(reader["Qualifications"]).TrimStart(',');
                obj.QualificationId = Convert.ToString(reader["QualificationIds"]).TrimStart(',');
                obj.SecSkills = Convert.ToString(reader["SecondarySkills"]).TrimStart(',').Trim();
                obj.SecSkillsId = Convert.ToString(reader["SecondarySkillsIds"]).TrimStart(',');

                obj.ProfileID = (reader["ProfileID"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["ProfileID"]);
                obj.Event = Convert.ToString(reader["empJoiningDate"]);
                obj.ADUserName = Convert.ToString(reader["ADUserName"]);
                obj.empExpectedLWD = string.IsNullOrEmpty(Convert.ToString(reader["empExpectedLWD"])) ? dt : Convert.ToDateTime(reader["empExpectedLWD"]);
                obj.IFSCCode = Convert.ToString(reader["IFSCCode"]);
                obj.MSTeam = Convert.ToString(reader["MSTeam"]);
                obj.IsRemoteEmployee = reader["IsRemoteEmployee"] != DBNull.Value ? Convert.ToBoolean(reader["IsRemoteEmployee"]) : false;

                lstEmployee.Add(obj);
            }
        }
        return lstEmployee;
    }

    public int SaveEmployee(EmployeeMaster objEmployee)
    {
        int outputid = 0;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_Employee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", objEmployee.Mode);
            cmd.Parameters.AddWithValue("@EmployeeID", objEmployee.empid);
            cmd.Parameters.AddWithValue("@Name", objEmployee.empName);
            cmd.Parameters.AddWithValue("@Address", objEmployee.empAddress);
            cmd.Parameters.AddWithValue("@Gender", objEmployee.empGender);
            cmd.Parameters.AddWithValue("@Contact", objEmployee.empContact);
            cmd.Parameters.AddWithValue("@SkillId", objEmployee.skillid);
            cmd.Parameters.AddWithValue("@Notes ", objEmployee.empNotes);
            cmd.Parameters.AddWithValue("@JoiningDate", objEmployee.empJoiningDate);
            cmd.Parameters.AddWithValue("@ProbationPeriod", objEmployee.empProbationPeriod);
            cmd.Parameters.AddWithValue("@EmailID", objEmployee.empEmail);
            cmd.Parameters.AddWithValue("@AccountNo", objEmployee.empAccountNo);
            cmd.Parameters.AddWithValue("@PAN", objEmployee.EmpPAN);
            cmd.Parameters.AddWithValue("@UAN", objEmployee.EmpUAN);
            cmd.Parameters.AddWithValue("@EPFAccountNo", objEmployee.EmpEPF);
            if (Convert.ToString(objEmployee.empBDate) != string.Empty)
                cmd.Parameters.AddWithValue("@BirthDate", objEmployee.empBDate);
            if (Convert.ToString(objEmployee.empADate) != string.Empty)
                cmd.Parameters.AddWithValue("@AnniversaryDate", objEmployee.empADate);
            cmd.Parameters.AddWithValue("@PreviousEmployer", objEmployee.empPrevEmployer);
            cmd.Parameters.AddWithValue("@Experience", objEmployee.empExperince);
            cmd.Parameters.AddWithValue("@InsertedOn", objEmployee.InsertedOn);
            cmd.Parameters.AddWithValue("@InsertedBy", objEmployee.InsertedBy);
            cmd.Parameters.AddWithValue("@InsertedIP", objEmployee.InsertedIP);
            cmd.Parameters.AddWithValue("@Type", objEmployee.Type);
            cmd.Parameters.AddWithValue("@LeavingDate", objEmployee.empLeavingDate);
            cmd.Parameters.AddWithValue("@Photo", objEmployee.Photo);
            cmd.Parameters.AddWithValue("@PrimarySkill", objEmployee.PrimarySkill);
            cmd.Parameters.AddWithValue("@Resume", objEmployee.Resume);
            cmd.Parameters.AddWithValue("@LocationFKID", objEmployee.LocationFKID);
            cmd.Parameters.AddWithValue("@EmployeeStatus", objEmployee.EmpStatus);
            cmd.Parameters.AddWithValue("@CurrentAddress", objEmployee.CAddress);
            cmd.Parameters.AddWithValue("@ADUserName", objEmployee.ADUserName);
            cmd.Parameters.AddWithValue("@empExpectedLWD", objEmployee.empExpectedLWD);
            cmd.Parameters.AddWithValue("@IFSCCode", objEmployee.IFSCCode);//ifsccode
            if (objEmployee.ProfileID > 0)
                cmd.Parameters.AddWithValue("@ProfileID", objEmployee.ProfileID);
            cmd.Parameters.AddWithValue("@MSTeam", objEmployee.MSTeam);//ifsccode
            cmd.Parameters.AddWithValue("@IsRemoteEmployee", objEmployee.IsRemoteEmployee);


            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return outputid;
    }

    public void SaveSecondarySkills(int intEmpId, int intTechSkillId, string strMode)
    {
        try
        {
            int outputid = 0;
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmployeeSecondarySkills", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", intEmpId);
            cmd.Parameters.AddWithValue("@TechskillId", intTechSkillId);
            cmd.Parameters.AddWithValue("@Mode", strMode);
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteNonQuery());
                con.Close();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public void SaveQualification(int intEmpId, int intQualificationId, string strMode)
    {
        try
        {
            int outputid = 0;
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmployeeQualifications", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", intEmpId);
            cmd.Parameters.AddWithValue("@QualificationId", intQualificationId);
            cmd.Parameters.AddWithValue("@Mode", strMode);
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteNonQuery());
                con.Close();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }


    //public string GetSecondarySkills(int intEmpId, int intTechSkillId, string strMode)
    //{
    //    string output = string.Empty;
    //    try
    //    {
    //        SqlConnection con = new SqlConnection(_strConnection);

    //        SqlCommand cmd = new SqlCommand("SP_EmployeeSecondarySkills", con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@EmployeeID", intEmpId);
    //        cmd.Parameters.AddWithValue("@TechskillId", intTechSkillId);
    //        cmd.Parameters.AddWithValue("@Mode", strMode);

    //        SqlDataReader reader = null;
    //        using (con)
    //        {
    //            con.Open();
    //            reader = cmd.ExecuteReader();
    //            while (reader.Read())
    //            {
    //                output = Convert.ToString(reader["TechSkillId"]);
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new ApplicationException(ex.Message);
    //    }
    //    return output;
    //}

    //public string GetQualification(int intEmpId, int intQualificationId, string strMode)
    //{

    //    string output = string.Empty;
    //    try
    //    {
    //        SqlConnection con = new SqlConnection(_strConnection);

    //        SqlCommand cmd = new SqlCommand("SP_EmployeeQualifications", con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@EmployeeID", intEmpId);
    //        cmd.Parameters.AddWithValue("@QualificationId", intQualificationId);
    //        cmd.Parameters.AddWithValue("@Mode", strMode);

    //        SqlDataReader reader = null;
    //        using (con)
    //        {
    //            con.Open();
    //            reader = cmd.ExecuteReader();
    //            while (reader.Read())
    //            {
    //                output = Convert.ToString(reader["empQualification"]);
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new ApplicationException(ex.Message);
    //    }
    //    return output;
    //}

    public string GetExistsClient(int intEmpid, string Emailid, string strmode)
    {
        string output = string.Empty;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_Employee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", strmode);
            cmd.Parameters.AddWithValue("@EmailID", Emailid);
            cmd.Parameters.AddWithValue("@EmployeeID", intEmpid);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        output = Convert.ToString(reader["empid"]);
                    }
                }
                else
                    output = "0";

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return output;
    }

    public EmployeeMaster GetEmployeeDetails(int empId)
    {
        EmployeeMaster objGetEmpDetails = new EmployeeMaster();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("Sp_GetEmpDetailsByEmployeeId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpId", empId);
            SqlDataReader reader = null;
            Nullable<DateTime> dt = null;


            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objGetEmpDetails = new EmployeeMaster();

                    objGetEmpDetails.empid = Convert.ToInt32(reader["empid"].ToString());
                    objGetEmpDetails.empPassword = reader["empPassword"].ToString();
                    objGetEmpDetails.skillid = Convert.ToInt32(reader["skillid"].ToString());
                    objGetEmpDetails.empName = reader["empName"].ToString();
                    objGetEmpDetails.empAddress = reader["empAddress"].ToString();
                    objGetEmpDetails.CAddress = reader["EmpCurrentAddress"].ToString();
                    objGetEmpDetails.empContact = reader["empContact"].ToString();
                    objGetEmpDetails.empJoiningDate = string.IsNullOrEmpty(Convert.ToString(reader["empJoiningDate"])) ? dt : Convert.ToDateTime(reader["empJoiningDate"]);
                    objGetEmpDetails.empLeavingDate = string.IsNullOrEmpty(Convert.ToString(reader["empLeavingDate"])) ? dt : Convert.ToDateTime(reader["empLeavingDate"]);
                    objGetEmpDetails.empProbationPeriod = Convert.ToInt32(reader["empProbationPeriod"].ToString());
                    objGetEmpDetails.empNotes = reader["empNotes"].ToString();
                    objGetEmpDetails.empEmail = reader["empEmail"].ToString();
                    objGetEmpDetails.empTester = Convert.ToBoolean(reader["empTester"].ToString());
                    objGetEmpDetails.empAccountNo = reader["empAccountNo"].ToString();
                    objGetEmpDetails.empBDate = string.IsNullOrEmpty(Convert.ToString(reader["empBDate"])) ? dt : Convert.ToDateTime(reader["empBDate"]);
                    objGetEmpDetails.empADate = string.IsNullOrEmpty(Convert.ToString(reader["empADate"])) ? dt : Convert.ToDateTime(reader["empADate"]);
                    objGetEmpDetails.empPrevEmployer = reader["empPrevEmployer"].ToString();
                    objGetEmpDetails.empExperince = Convert.ToInt32(reader["empExperince"].ToString());
                    objGetEmpDetails.IsSuperAdmin = Convert.ToBoolean(reader["IsSuperAdmin"].ToString());
                    objGetEmpDetails.IsAccountAdmin = Convert.ToBoolean(reader["IsAccountAdmin"].ToString());
                    objGetEmpDetails.IsPayrollAdmin = Convert.ToBoolean(reader["IsPayrollAdmin"].ToString());
                    objGetEmpDetails.IsPM = Convert.ToBoolean(reader["IsPM"].ToString());
                    objGetEmpDetails.IsTester = Convert.ToBoolean(reader["IsTester"].ToString());
                    objGetEmpDetails.IsProjectReport = Convert.ToBoolean(reader["IsProjectReport"].ToString());
                    objGetEmpDetails.IsProjectStatus = objGetEmpDetails.empTester = Convert.ToBoolean(reader["IsProjectStatus"].ToString());
                    objGetEmpDetails.IsLeaveAdmin = Convert.ToBoolean(reader["IsLeaveAdmin"].ToString());
                    objGetEmpDetails.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                    objGetEmpDetails.InsertedOn = string.IsNullOrEmpty(Convert.ToString(reader["InsertedOn"])) ? dt : Convert.ToDateTime(reader["InsertedOn"]);
                    objGetEmpDetails.InsertedBy = Convert.ToInt32(reader["InsertedBy"].ToString());
                    objGetEmpDetails.InsertedIP = reader["InsertedIP"].ToString();
                    objGetEmpDetails.ModifiedOn = string.IsNullOrEmpty(Convert.ToString(reader["ModifiedOn"])) ? dt : Convert.ToDateTime(reader["ModifiedOn"]);
                    objGetEmpDetails.ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader["ModifiedBy"])) ? 0 : Convert.ToInt32(reader["ModifiedBy"]);
                    objGetEmpDetails.ModifiedIP = reader["ModifiedIP"].ToString();
                    objGetEmpDetails.LocationFKID = string.IsNullOrEmpty(Convert.ToString(reader["LocationFKID"])) ? 0 : Convert.ToInt32(reader["LocationFKID"]);
                    objGetEmpDetails.Photo = Convert.IsDBNull(reader["Photo"]) ? null : (byte[])reader["Photo"];
                    objGetEmpDetails.Resume = string.IsNullOrEmpty(Convert.ToString(reader["Resume"])) ? string.Empty : Convert.ToString(reader["Resume"]);
                    objGetEmpDetails.IsRemoteEmployee = reader["IsRemoteEmployee"] != DBNull.Value ? Convert.ToBoolean(reader["IsRemoteEmployee"]) : false;

                }
            }
        }
        catch (Exception ex)
        { }
        return objGetEmpDetails;
    }


    public List<EmployeeMaster> SelectEvent(string mode, int locId, int days)
    {
        List<EmployeeMaster> empEvents = new List<EmployeeMaster>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@LocationFKID", locId);
        cmd.Parameters.AddWithValue("@Days", days);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                empEvents.Add(new EmployeeMaster(
                     Convert.ToString(reader["EmpName"].ToString()),
                      Convert.ToString(reader["Type"].ToString()),
                      Convert.ToString(reader["Event"].ToString()),
                      Convert.ToDateTime(reader["DOB"].ToString())
                    ));
            }
        }
        return empEvents;
    }




    public List<EmpProfile> BindProfile(int locationId, string mode)
    {
        List<EmpProfile> listProfile = new List<EmpProfile>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@LocationFKID", locationId);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EmpProfile profile = new EmpProfile();
                profile.ProfileId = Convert.ToInt32(reader["ProfileId"]);
                profile.ProfileName = Convert.ToString(reader["Name"]);
                listProfile.Add(profile);
            }
        }
        return listProfile;
    }




    public List<EmployeeMaster> GetTimeSheet(string mode, int empID)
    {
        List<EmployeeMaster> lstTimesheet = new List<EmployeeMaster>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmployeeID", empID);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EmployeeMaster lst = new EmployeeMaster();
                lst.empName = reader["tsDate"].ToString();
                lst.empNotes = reader["tsHour"].ToString();
                lst.Net = Convert.ToInt16(reader["holiday"].ToString());
                lstTimesheet.Add(lst);
            }
        }
        return lstTimesheet;
    }

    public List<EmployeeMaster> GetEmpHistory(string mode, int empID)
    {
        List<EmployeeMaster> lstHistory = new List<EmployeeMaster>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmployeeID", empID);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        Nullable<DateTime> dt = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EmployeeMaster lst = new EmployeeMaster();
                lst.HistoryID = Convert.ToInt16(reader["HID"].ToString());
                lst.CAddress = reader["EmpCurrentAddress"].ToString();
                lst.empContact = reader["empContact"].ToString();
                lst.InsertedIP = reader["empADate"].ToString();
                lst.empAccountNo = reader["empADateTemp"].ToString();
                //lst.empADate = string.IsNullOrEmpty(Convert.ToString(reader["empADateTemp"])) ? dt : Convert.ToDateTime(reader["empADateTemp"]);
                lst.EmpStatus = reader["status"].ToString();
                lst.empName = reader["ModifiedBy"].ToString();
                lst.ModifiedIP = reader["ModifiedOn"].ToString();
                lstHistory.Add(lst);
            }
        }
        return lstHistory;
    }

    public string SaveApprovedData(string mode, int empID, int HID, int approvalStatus, int modifiedBy)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmployeeID", empID);
        cmd.Parameters.AddWithValue("@HID", HID);
        cmd.Parameters.AddWithValue("@Status", approvalStatus);
        cmd.Parameters.AddWithValue("@InsertedBy", modifiedBy);
        cmd.CommandType = CommandType.StoredProcedure;
        con.Open();
        int output = cmd.ExecuteNonQuery();
        con.Close();
        return output.ToString();
    }

    public string SaveEmpProfile(string mode, int empID, string cAddress, string contact, string aDate, int modifiedBy)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmployeeID", empID);
        cmd.Parameters.AddWithValue("@CurrentAddress", cAddress);
        cmd.Parameters.AddWithValue("@Contact", contact);
        cmd.Parameters.AddWithValue("@AnniversaryDate", aDate);
        cmd.Parameters.AddWithValue("@InsertedBy", modifiedBy);

        con.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        int output = cmd.ExecuteNonQuery();
        con.Close();
        return output.ToString();
    }

    public string GetEditedProfileStatus(string mode, int empID)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmployeeID", empID);

        con.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();
        string output = "";
        if (dt.Rows.Count > 0)
        {
            output = dt.Rows[0]["status"].ToString();
        }
        else
            output = "1";

        return output.ToString();
    }

    internal object GetAllAppraisalAuth()
    {
        List<EmployeeMaster> curEmployeeMaster = new List<EmployeeMaster>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "Select");
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EmployeeMaster EM = new EmployeeMaster();

                EM.empid = Convert.ToInt32(reader["empid"]);
                EM.empName = reader["empName"].ToString();

                curEmployeeMaster.Add(EM);
                //Convert.ToInt32(reader["empid"]),
                // reader["empName"].ToString()
                //reader["empAddress"].ToString(), 
                //reader["InsertedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                //reader["ModifiedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"].ToString())
                //));
            }
        }
        return curEmployeeMaster;
    }

    public string GetExistsADUserName(int intEmpid, string ADUserName, string strmode)
    {
        string output = string.Empty;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_Employee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", strmode);
            cmd.Parameters.AddWithValue("@ADUserName", ADUserName);
            cmd.Parameters.AddWithValue("@EmployeeID", intEmpid);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        output = Convert.ToString(reader["empid"]);
                    }
                }
                else
                    output = "0";

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return output;
    }

    /// <summary>
    /// Get Exists ADUserName
    /// Added by bhavana 24-06-2016
    /// </summary>
    /// <param name="intEmpid">emp id</param>
    /// <param name="strMode">mode</param>
    /// <returns>AD user name</returns>
    public string GetExistsADUserName(int intEmpid, string strmode)
    {
        string output = string.Empty;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_Employee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", strmode);
            cmd.Parameters.AddWithValue("@EmployeeID", intEmpid);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        output = Convert.ToString(reader["ADUserName"]);
                    }
                }
                else
                    output = "";

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return output;
    }

    /// <summary>
    /// This method is to existing SP_Employee sp and get the employee details from (Employee master and FavouriteEmp)
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="empName"></param>
    /// <param name="empID"></param>
    /// <returns>Return employee details</returns>
    public DataTable GetEmpDetails(string mode, string empName, int empID)
    {
        DataTable dtEmpDetails = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                SqlCommand cmd = new SqlCommand("SP_Employee", con);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@Name", empName);
                cmd.Parameters.AddWithValue("@EmployeeID", empID);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtEmpDetails);
                con.Close();
                return dtEmpDetails;
            }
        }
        catch (Exception ex)
        {
            return dtEmpDetails;
        }
    }


    /// <summary>
    /// This method used to add FavouriteEmployee to FavouriteEmp. 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="empID"></param>
    /// <param name="FavouriteEmpId"></param>
    /// <returns>Return datatable as result</returns>
    public DataTable InsertFavouriteEmployee(string mode, int empID, int favouriteEmpId)
    {
        DataTable dtEmpDetails = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                SqlCommand cmd = new SqlCommand("SP_Employee", con);
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@EmployeeID", empID);
                cmd.Parameters.AddWithValue("@FavouriteEmpId", favouriteEmpId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtEmpDetails);
                con.Close();
                return dtEmpDetails;
            }
        }
        catch (Exception ex)
        {
            return dtEmpDetails;
        }

    }


    /// <summary>
    /// This method used to get the favourite Employees. 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="empID"></param>
    /// <returns>Return FavouriteEmployees</returns>
    public DataTable GetFavouriteEmployees(string mode, int empID)
    {
        DataTable dtEmpDetails = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                SqlCommand cmd = new SqlCommand("SP_Employee", con);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@EmployeeID", empID);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtEmpDetails);
                con.Close();
                return dtEmpDetails;
            }
        }
        catch (Exception ex)
        {
            return dtEmpDetails;
        }
    }

    /// <summary>
    /// This method used to delete the favourite Employees. 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="empID"></param>
    /// <param name="favouriteEmpId"></param>
    /// <returns>Return message</returns>
    public DataTable DeleteFavouriteEmployee(string mode, int empID, int favouriteEmpId)
    {
        DataTable dtEmpDetails = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                SqlCommand cmd = new SqlCommand("SP_Employee", con);
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@EmployeeID", empID);
                cmd.Parameters.AddWithValue("@FavouriteEmpId", favouriteEmpId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtEmpDetails);
                con.Close();
                return dtEmpDetails;
            }
        }
        catch (Exception ex)
        {
            return dtEmpDetails;
        }

    }
    //Added By Nikhil Shetye
    public string UploadImage(string mode, int empID, byte[] byteImage)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmployeeID", empID);
        cmd.Parameters.AddWithValue("@Photo", byteImage);
        con.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        int output = cmd.ExecuteNonQuery();
        con.Close();
        return output.ToString();
    }
    //End Nikhil Shetye
    public UserIdentityData GetEmpDetailsbyId(string UserId, string Name)
    {
        UserIdentityData objUserData = new UserIdentityData();
        DataTable dt = new DataTable();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_Employee", con);
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
                objUserData.IsSuccess = true;
                objUserData.StatusMessage = "Success";

            }
        }
        catch (Exception ex)
        {
            objUserData.IsSuccess = false;
            objUserData.StatusMessage = ex.Message;

        }
        return objUserData;
    }
}


