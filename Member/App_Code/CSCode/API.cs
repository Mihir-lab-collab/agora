using System;
using System.Collections.Generic;
using System.Web;
using CommonFunctionLib;
using System.Runtime.Serialization;
using System.Configuration;
using dwtDAL;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]

public class API : IAPIService
{
    public DBFunc objDBFunction = new DBFunc();
    private readonly HttpContext context;
    public static string StrVal = string.Empty;
    public API()
    {
        context = HttpContext.Current;
    }

    public List<Result>DoLogin(InputData Data)
    {
        Result result = new Result();

        UserMasterDAL Obj = new UserMasterDAL();
        UserMaster ObjUser = new UserMaster();

        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            int EmpID = 0;
            string Password = "";

            EmpID = Convert.ToInt32(Data.EmpId);
            Password = Data.Password;

            ObjUser = Obj.Login(Convert.ToInt32(Data.EmpId), CSCode.Global.CreatePassword(Password));

            UserInfo clsUserInfo = new UserInfo();
            List<UserInfo> lstUserInfo = new List<UserInfo>();

            if (ObjUser != null)
            {
                clsUserInfo.Status = 1;
                clsUserInfo.LoginMessage = "Login Successful.";
                clsUserInfo.EmployeeID = ObjUser.EmployeeID;
                clsUserInfo.Name = ObjUser.Name;
                clsUserInfo.Address = ObjUser.Address;
                clsUserInfo.EmailID = ObjUser.EmailID;
                clsUserInfo.Contact = ObjUser.Contact;
                clsUserInfo.IsAdmin = ObjUser.IsAdmin;
                clsUserInfo.BDate = String.Format("{0:MM/dd/yyyy}",ObjUser.BDate);
               //// clsUserInfo.ConfirmationDate = String.Format("{0:MM/dd/yyyy}",ObjUser);
                ////clsUserInfo.Designation =
                clsUserInfo.JoiningDate = String.Format("{0:MM/dd/yyyy}", ObjUser.JoiningDate);
                clsUserInfo.ProbationPeriod = ObjUser.ProbationPeriod;

                lstUserInfo.Add(clsUserInfo);
                result.Status = "1";
                result.Message = "Login Successful.";
                result.lstUserInfo = lstUserInfo;

            }
            else
            {


                result.Status = "0"; //added later
                result.Message = "No data found"; //added later
            }
        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }

        List<Result> data = new List<Result>();
        data.Add(result);

       ///// var json = new JavaScriptSerializer().Serialize(ObjUser);

       ///// return json;
        return data;
    }
    public List<Result> Getlogin(InputData Data)
    {
        Result result = new Result();

        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            UserInfo clsUserInfo = new UserInfo();
            List<UserInfo> lstUserInfo = new List<UserInfo>();
            //test code STARTS HERE
            object a;
            a = Data.EmpId;

            if (a.GetType().FullName != "System.Int32")
            {
            }
            //test code ENDS HERE

            DataTable dtUserInfo = objDBFunction.ExecuteSQLRtnDT("Declare @ProfileId varchar(50) select @ProfileId=ProfileId from profile where profile.ProfileId=(select top(1)ProfileID from employeeMaster where empid =" + Convert.ToString(Data.EmpId) + ") if(@ProfileId!='') begin Select UserMaster.UserType, UserMaster.SecurityLevel, profile.IsAdmin, employeeMaster.*,DateAdd(month,employeeMaster.empProbationPeriod,employeeMaster.empJoiningDate) AS empConfDate,SM.SkillDesc as Designation from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid inner join profile on profile.ProfileId=employeeMaster.ProfileID and UserMaster.UserMasterID ='" + Convert.ToString(Data.EmpId) + "' inner join SkillMaster SM on SM.SkillId =employeeMaster.skillid end else begin Select UserMaster.UserType, UserMaster.SecurityLevel, 0 as IsAdmin, employeeMaster.*,DateAdd(month,employeeMaster.empProbationPeriod,employeeMaster.empJoiningDate) AS empConfDate,SM.SkillDesc as Designation  from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid and UserMaster.UserMasterID ='" + Convert.ToString(Data.EmpId) + "'  inner join SkillMaster SM on SM.SkillId =employeeMaster.skillid end ");
            if (dtUserInfo.Rows.Count > 0)
            {
                //Verify Password start
                bool check = false;
                string comppwd = null;
                string haskey = ConfigurationManager.AppSettings["hashKey"].ToString();
                byte tempbyte = byte.Parse("123");
                byte[] bytearr = new byte[2];
                bytearr[0] = tempbyte;

                comppwd = SessionHelper.ComputeHash(Data.Password, haskey, bytearr);
                check = SessionHelper.VerifyHash(Data.Password, haskey, dtUserInfo.Rows[0]["empPassword"].ToString());
                //Verify Password end


                if (dtUserInfo.Rows.Count > 0 && Convert.ToBoolean(dtUserInfo.Rows[0]["IsActive"]))
                {
                    if (check)
                    {
                        foreach (DataRow dr in dtUserInfo.Rows)
                        {
                            clsUserInfo.Status = 1;
                            clsUserInfo.LoginMessage = "Login Successful.";
                            clsUserInfo.EmployeeID = Convert.ToInt32(dr["empid"]);
                            clsUserInfo.UserType = Convert.ToString(dr["UserType"]);
                            clsUserInfo.SecurityLevel = Convert.ToInt32(dr["SecurityLevel"]);
                            clsUserInfo.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
                            clsUserInfo.SkillID = Convert.ToInt32(dr["skillid"]);
                            clsUserInfo.Designation = Convert.ToString(dr["Designation"]);
                            clsUserInfo.Name = Convert.ToString(dr["empName"]);
                            clsUserInfo.Address = Convert.ToString(dr["empAddress"]);
                            clsUserInfo.Contact = Convert.ToString(dr["empContact"]);
                            clsUserInfo.JoiningDate = String.Format("{0:MM/dd/yyyy}", dr["empJoiningDate"]);
                            clsUserInfo.ConfirmationDate = String.Format("{0:MM/dd/yyyy}", dr["empConfDate"]);  
                            clsUserInfo.LeavingDate = Convert.ToString(dr["empLeavingDate"]);
                            clsUserInfo.ProbationPeriod = Convert.ToString(dr["empProbationPeriod"]);
                            clsUserInfo.Notes = Convert.ToString(dr["empNotes"]);
                            clsUserInfo.EmailID = Convert.ToString(dr["empEmail"]);
                            clsUserInfo.Tester = Convert.ToInt32(dr["empTester"]);
                            clsUserInfo.AccountNo = Convert.ToString(dr["empAccountNo"]);
                            clsUserInfo.BDate = String.Format("{0:MM/dd/yyyy}", dr["empBDate"]);  
                            clsUserInfo.ADate = String.Format("{0:MM/dd/yyyy}", dr["empADate"]);
                            clsUserInfo.PreviousEmployer = Convert.ToString(dr["empPrevEmployer"]);
                            clsUserInfo.Experience = Convert.ToInt32(dr["empExperince"]);
                            clsUserInfo.IsSuperAdmin = Convert.ToBoolean(dr["IsSuperAdmin"]);
                            clsUserInfo.IsAccountAdmin = Convert.ToBoolean(dr["IsAccountAdmin"]);
                            clsUserInfo.IsPayrollAdmin = Convert.ToBoolean(dr["IsPayrollAdmin"]);
                            clsUserInfo.IsPM = Convert.ToBoolean(dr["IsPM"]);
                            clsUserInfo.IsTester = Convert.ToBoolean(dr["IsTester"]);
                            clsUserInfo.IsProjectReport = Convert.ToBoolean(dr["IsProjectReport"]);
                            clsUserInfo.IsProjectStatus = Convert.ToBoolean(dr["IsProjectStatus"]);
                            clsUserInfo.IsLeaveAdmin = Convert.ToBoolean(dr["IsLeaveAdmin"]);
                            clsUserInfo.InsertedOn = String.Format("{0:MM/dd/yyyy}", dr["InsertedOn"]);  
                            clsUserInfo.InsertedBy = Convert.ToString(dr["InsertedBy"]);
                            clsUserInfo.InsertedIP = Convert.ToString(dr["InsertedIP"]);
                            clsUserInfo.LocationID = Convert.ToString(dr["LocationFKID"]);
                            clsUserInfo.ProfileID = Convert.ToString(dr["ProfileID"]);
                            lstUserInfo.Add(clsUserInfo);
                        }

                        result.Status = "1"; //added later
                        result.Message = "Login Successful."; //added later
                        result.lstUserInfo = lstUserInfo;
                    }
                    else
                    {
                        //UserInfo clsUserInfo = new UserInfo();
                        clsUserInfo.Status = 0;
                        clsUserInfo.LoginMessage = "You have entered wrong password.";

                        lstUserInfo.Add(clsUserInfo);
                        result.lstUserInfo = lstUserInfo;
                        result.Status = "0"; //added later
                        result.Message = "You have entered wrong password."; //added later
                    }
                }
                else if (dtUserInfo.Rows.Count > 0 && !Convert.ToBoolean(dtUserInfo.Rows[0]["IsActive"]))
                {
                    //UserInfo clsUserInfo = new UserInfo();
                    clsUserInfo.Status = 0;
                    clsUserInfo.LoginMessage = "Account Deactivated";
                    lstUserInfo.Add(clsUserInfo);
                    result.lstUserInfo = lstUserInfo;
                    result.Status = "0"; //added later
                    result.Message = "Account Deactivated"; //added later
                }
                else
                {
                    // UserInfo clsUserInfo = new UserInfo();
                    clsUserInfo.Status = 0;
                    clsUserInfo.LoginMessage = "You are not an authorized user.";
                    lstUserInfo.Add(clsUserInfo);
                    result.lstUserInfo = lstUserInfo;
                    result.Status = "0"; //added later
                    result.Message = "You are not an authorized user."; //added later
                }
            }
            else
            {
                result.Status = "0"; //added later
                result.Message = "No data found"; //added later
            }
            //result.Status = "1"; //added later
        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }

        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

    // Function Dashboard
    public List<Result> Dashboard()
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            result.lstHoliday = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);
        }

        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }

        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

    // Function fnGetEmpList Starts Here
    public List<Result> fnGetEmpList(InputData Data)
    {
        String strEmpName;
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {

            List<EmployeeName> lstEmployeeName = new List<EmployeeName>();
            strEmpName = Convert.ToString(Data.strEmpName);

            EmployeeName clsBoard = new EmployeeName();
            clsBoard.EmpName = "Board Number";
            clsBoard.EmployeeID = 0;
            clsBoard.EmpEmail = "contactus@intelgain.com";
            clsBoard.EmpContact = "91 22 41516100";
            clsBoard.Flag = "Generic";
            lstEmployeeName.Add(clsBoard);

            EmployeeName clsHR = new EmployeeName();
            clsHR.EmpName = "HR";
            clsHR.EmployeeID = 0;
            clsHR.EmpEmail = "hr@intelgain.com";
            clsHR.EmpContact = "91 22 41516102";
            clsHR.Flag = "Generic";
            lstEmployeeName.Add(clsHR);

            EmployeeName clsAccounts = new EmployeeName();
            clsAccounts.EmpName = "Accounts";
            clsAccounts.EmployeeID = 0;
            clsAccounts.EmpEmail = "accounts@intelgain.com";
            clsAccounts.EmpContact = "91 22 41516106";
            clsAccounts.Flag = "Generic";
            lstEmployeeName.Add(clsAccounts);

            DataTable dtEmployeeName = objDBFunction.ExecuteSQLRtnDT("EXEC SP_GetEmpContactList '" + Convert.ToString(Data.strEmpName) + "','" + Data.EmpId + "'");

            if (dtEmployeeName.Rows.Count > 0)
            {
                foreach (DataRow dr in dtEmployeeName.Rows)
                {
                    EmployeeName clsEmployeeName = new EmployeeName();
                    clsEmployeeName.EmpName = Convert.ToString(dr["empName"]);
                    clsEmployeeName.EmployeeID = Convert.ToInt32(dr["empid"]);
                    clsEmployeeName.EmpEmail = Convert.ToString(dr["empEmail"]);
                    clsEmployeeName.EmpContact = Convert.ToString(dr["empContact"]);
                    clsEmployeeName.Flag = Convert.ToString(dr["Flag"]);
                    //clsEmployeeName.Photo =(byte[])dr["Photo"];
                    lstEmployeeName.Add(clsEmployeeName);
                }

                result.Status = "1";
                result.Message = "Data found.";
            }
            else
            {
                EmployeeName clsEmployeeName = new EmployeeName();
                lstEmployeeName.Add(clsEmployeeName);
                result.Status = "0";
                result.Message = "Data not found";

            }

            result.lstEmployeeName = lstEmployeeName;
        }

        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }

        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }
    // Function fnGetEmpList Ends Here

    public List<Result> fnInsertFavouriteEmp(InputData Data)
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            List<EmployeeName> lstEmployeeName = new List<EmployeeName>();
            DataTable dtFavourite = objDBFunction.ExecuteSQLRtnDT("EXEC sp_InsertFavouriteEmp  '" + Data.EmpId + "','" + Data.FavEmpId + "'");
            if (dtFavourite.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFavourite.Rows)
                {
                    result.Status = Convert.ToString(dr["Status"]);
                    result.Message = Convert.ToString(dr["Message"]);
                }
            }
            else
            {
                result.Status = "0";
                result.Message = "Not added to Favourites.";
            }
        }
        catch (Exception ex)
        {
            result.Status = "0";
            //result.Message = "Execution failed : " + ex.Message;
            result.Message = "" + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

    public List<Result> fnGetFavEmpList(InputData Data)
    {
        String strEmpName;
        int EmpID = 0;
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        List<FavouriteEmployees> lstFavouriteEmployees = new List<FavouriteEmployees>();
        try
        {
           
            strEmpName = Convert.ToString(Data.strEmpName);

            FavouriteEmployees clsBoard = new FavouriteEmployees();
            clsBoard.FavouriteEmpName = "Board Number";
            clsBoard.FavouriteEmpId = 0;
            clsBoard.FavouriteEmpEmail = "contactus@intelgain.com";
            clsBoard.FavouriteEmpContact = "91 22 41516100";
            clsBoard.FavouriteFlag = "Generic";
            lstFavouriteEmployees.Add(clsBoard);

            FavouriteEmployees clsHR = new FavouriteEmployees();
            clsHR.FavouriteEmpName = "HR";
            clsHR.FavouriteEmpId = 0;
            clsHR.FavouriteEmpEmail = "hr@intelgain.com";
            clsHR.FavouriteEmpContact = "91 22 41516102";
            clsHR.FavouriteFlag = "Generic";
            lstFavouriteEmployees.Add(clsHR);

            FavouriteEmployees clsAccounts = new FavouriteEmployees();
            clsAccounts.FavouriteEmpName = "Accounts";
            clsAccounts.FavouriteEmpId = 0;
            clsAccounts.FavouriteEmpEmail = "accounts@intelgain.com";
            clsAccounts.FavouriteEmpContact = "91 22 41516106";
            clsAccounts.FavouriteFlag = "Generic";
            lstFavouriteEmployees.Add(clsAccounts);
            EmpID = Convert.ToInt32(Data.EmpId);

            if (EmpID != 0)
            {
                DataTable dtFavouriteList = objDBFunction.ExecuteSQLRtnDT("EXEC SP_GetFavouriteEmp '" + Data.EmpId + "'");
                if (dtFavouriteList != null && dtFavouriteList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFavouriteList.Rows)
                    {
                        FavouriteEmployees clsFavouriteEmployees = new FavouriteEmployees();
                        clsFavouriteEmployees.FavouriteEmpId = Convert.ToInt32(dr["FavouriteEmpId"]);
                        clsFavouriteEmployees.FavouriteEmpName = Convert.ToString(dr["FavouriteEmpName"]);
                        clsFavouriteEmployees.FavouriteEmpContact = Convert.ToString(dr["FavouriteEmpContact"]);
                        clsFavouriteEmployees.FavouriteEmpEmail = Convert.ToString(dr["FavouriteEmpEmail"]);
                        clsFavouriteEmployees.FavouriteFlag = Convert.ToString(dr["flag"]);
                        lstFavouriteEmployees.Add(clsFavouriteEmployees);
                    }

                    result.Status = "1";
                    result.Message = "Data found.";
                }
                else
                {
                    FavouriteEmployees clsFavouriteEmployees = new FavouriteEmployees();
                    lstFavouriteEmployees.Add(clsFavouriteEmployees);
                    result.Status = "0";
                    result.Message = "Data not found";
                }
            }
            else
            {
                result.Status = "0";
                result.Message = "Data not found";
            }

        }
        catch (Exception ex)
        {
            if (string.Compare(Convert.ToString(ex.Message), "Favourite mapping does not exist ", true) == 0 || string.Compare(Convert.ToString(ex.Message), "Invalid Employee Id", true) == 0)
            {
                result.Status = "1";
                result.Message = " " + ex.Message;
            }
            else
            {
                result.Status = "0";
                result.Message = " " + ex.Message;
            }
        }
        result.lstFavouriteEmployees = lstFavouriteEmployees;
        List<Result> data = new List<Result>();
        data.Add(result);
        return data;
    }

    public List<Result> fnGetMissedDates(InputData Data)
    {
        String strMonth, strYear;
        String strStartDate = string.Empty, strEndDate = string.Empty;

        int EmpID = 0;
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {

            List<MissedDates> lstMissedDates = new List<MissedDates>();
            strMonth = Convert.ToString(Data.strMonth);
            strYear = Convert.ToString(Data.strYear);
            strStartDate = Convert.ToString(Data.strStartDate);
            strEndDate = Convert.ToString(Data.strEndDate);
            EmpID = Convert.ToInt32(Data.EmpId);
            if (string.IsNullOrEmpty(strMonth) == false || string.IsNullOrEmpty(strYear) == false)
            {
                if (EmpID != 0)
                {
                    DataTable dtMissedDates = objDBFunction.ExecuteSQLRtnDT("EXEC SP_GetMissedDates_WithLeaves '" + Convert.ToString(Data.strMonth) + "','" + Convert.ToString(Data.strYear) + "','" + Convert.ToString(Data.strStartDate) + "','" + Convert.ToString(Data.strEndDate) + "','" + Data.EmpId + "'");

                    if (dtMissedDates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtMissedDates.Rows)
                        {
                            MissedDates clsMissedDates = new MissedDates();
                            //clsMissedDates.MissedDate = Convert.ToString(dr["MissedDate"]);
                            clsMissedDates.MissedDate = Convert.ToString(dr["MissedDate"]);
                            clsMissedDates.Flag = Convert.ToString(dr["Flag"]);
                            clsMissedDates.AttInTime = Convert.ToString(dr["attInTime"]);
                            clsMissedDates.AttOutTime = Convert.ToString(dr["attOutTime"]);
                            clsMissedDates.AttendanceStatus = Convert.ToString(dr["attStatus"]);
                            clsMissedDates.WorkingHours = Convert.ToString(dr["workinghours"]);
                            clsMissedDates.TimesheetHours = Convert.ToString(dr["Timesheethours"]);
                            clsMissedDates.Description = Convert.ToString(dr["Description"]);

                            lstMissedDates.Add(clsMissedDates);
                        }

                        result.Status = "1";
                        result.Message = "Data found.";
                        result.lstMissedDates = lstMissedDates;
                    }
                    else
                    {
                        MissedDates clsMissedDates = new MissedDates();
                        lstMissedDates.Add(clsMissedDates);
                        result.Status = "0";
                        result.Message = "Data not found";
                        result.lstMissedDates = lstMissedDates;
                    }
                }
                else
                {
                    result.Status = "0";
                    result.Message = "Data not found";
                }
            }
            else
            {
                result.Status = "0";
                result.Message = "Data not found";
            }

        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }

        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

    public List<Result> fnForgetPassword(InputData Data)
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            List<ForgotPassword> lstForgotPassword = new List<ForgotPassword>();
            DataTable dtForgotPassword = objDBFunction.ExecuteSQLRtnDT("Declare @ProfileId varchar(50) select @ProfileId=ProfileId from profile where profile.ProfileId=(select top(1)ProfileID from employeeMaster where empid =" + Data.EmpId + ") if(@ProfileId!='') begin Select UserMaster.UserType, UserMaster.SecurityLevel, profile.IsAdmin, employeeMaster.* from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid inner join profile on profile.ProfileId=employeeMaster.ProfileID and UserMaster.UserMasterID ='" + Data.EmpId + "'and employeeMaster.empEmail='" + Data.EmailID + "' end else begin Select UserMaster.UserType, UserMaster.SecurityLevel, 0 as IsAdmin, employeeMaster.* from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid and UserMaster.UserMasterID ='" + Data.EmpId + "'and employeeMaster.empEmail='" + Data.EmailID + "' end ");


            if (dtForgotPassword.Rows.Count > 0)
            {
                MailMessage msg = new MailMessage();
                MailAddress msgFrom = new MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"),
                    ConfigurationManager.AppSettings.Get("compName"));
                msg.From = msgFrom;



                foreach (string emailAddress in dtForgotPassword.Rows[0]["empEmail"].ToString().Trim().Split(','))
                {
                    msg.To.Add(emailAddress);
                }

                msg.IsBodyHtml = true;
                msg.Subject = "Agora - Request For Password";

                msg.Body = CreateMsgBody(dtForgotPassword.Rows[0]["empName"].ToString(), dtForgotPassword.Rows[0]["empEmail"].ToString(), Data.EmpId, false, "").ToString();

                SmtpClient mailClient = new SmtpClient();
                mailClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings.Get("SMTP"));
                mailClient.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Port"));
                mailClient.Credentials = new System.Net.NetworkCredential("smtptest@dweb.in", "intelgain");
                mailClient.EnableSsl = true;
                try
                {
                    mailClient.Send(msg);
                    result.Status = "1";
                    result.Message = "Your login details have been send to your mail.";
                }

                catch (Exception ex)
                {
                    result.Message = "Execution failed : " + ex.Message;
                }

                result.Status = "1";
            }
            else
            {
                result.Message = "Login details are incorrect";
            }


        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

    public List<Result> fnChangedPassword(InputData Data)
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            List<ChangePassword> lstChangePassword = new List<ChangePassword>();
            DataTable dtChangedPassword = objDBFunction.ExecuteSQLRtnDT("Declare @ProfileId varchar(50) select @ProfileId=ProfileId from profile where profile.ProfileId=(select top(1)ProfileID from employeeMaster where empid =" + Data.EmpId + ") if(@ProfileId!='') begin Select UserMaster.UserType, UserMaster.SecurityLevel, profile.IsAdmin, employeeMaster.* from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid inner join profile on profile.ProfileId=employeeMaster.ProfileID and UserMaster.UserMasterID =" + Data.EmpId + " end else begin Select UserMaster.UserType, UserMaster.SecurityLevel, 0 as IsAdmin, employeeMaster.* from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid and UserMaster.UserMasterID =" + Data.EmpId + " end ");
            if (dtChangedPassword.Rows.Count > 0)
            {
                if (dtChangedPassword.Rows[0]["empPassword"].ToString() == CreatrePassword(Data.OldPassword))
                {
                    MailMessage msg = new MailMessage();
                    MailAddress msgFrom = new MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"),
                        ConfigurationManager.AppSettings.Get("compName"));
                    msg.From = msgFrom;
                    if (Data.NewPassword == Data.ConfirmPassword)
                    {

                        foreach (string emailAddress in dtChangedPassword.Rows[0]["empEmail"].ToString().Trim().Split(','))
                        {
                            msg.To.Add(emailAddress);
                        }

                        msg.IsBodyHtml = true;
                        msg.Subject = "Agora - Password Changed";
                        msg.Body = CreateMsgBody(dtChangedPassword.Rows[0]["empName"].ToString(), dtChangedPassword.Rows[0]["empEmail"].ToString(), Data.EmpId, true, Data.NewPassword).ToString();
                        SmtpClient mailClient = new SmtpClient();
                        mailClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings.Get("SMTP"));
                        mailClient.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Port"));
                        mailClient.Credentials = new System.Net.NetworkCredential("smtptest@dweb.in", "intelgain");
                        mailClient.EnableSsl = true;
                        try
                        {
                            mailClient.Send(msg);
                            result.Status = "1";
                            result.Message = "your password has been changed successfully";

                        }
                        catch (Exception ex)
                        {
                            result.Message = "Execution failed : " + ex.Message;
                        }
                    }
                    else
                    {
                        result.Message = "Password does not matches";
                    }
                }
                else
                {
                    result.Message = "Old password is incorrect";
                }

                result.Status = "1";
            }


        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);

        return data;

    }

    protected StringBuilder CreateMsgBody(string strUserName, string strEmail, int intEmpID, bool isChangePassword, string newPwd)
    {
        StringBuilder mailBody = new StringBuilder();
        mailBody.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title></title></head><body>");
        mailBody.Append("<table  cellspacing=\"0\" border=\"0\" style=\"background-color: #FFCC00;border-width:1px;font-family:Gill Sans MT;font-size:12px;border-style:double; border-color:#FF9900;\">");
        mailBody.Append("<tr style=\"font-weight:bold;\"><td colspan=\"2\" style=\"padding-left:5px;padding-top:5px\">");
        //Add the header to the email body
        mailBody.Append("Dear " + strUserName + ",");
        mailBody.Append("<br/><br/>We are pleased to send your new password on your request. Please don't forward this mail to any email id for security purpose of your account.<br/><br/>");
        mailBody.Append("</td></tr>");
        Random r = new Random();
        int intRandom = r.Next();
        string strNewPassword = string.Empty;
        if (!isChangePassword)
        {
            strNewPassword = strUserName.Trim().Substring(0, 3) + intRandom.ToString().Substring(0, 4);
        }
        else
        {
            strNewPassword = newPwd;// Convert.ToString(context.Session["NewPassWord"]);
        }
        mailBody.Append("<tr>");
        mailBody.Append(string.Format("<td style=\"padding-left:5px;font-weight:bold\" colspan=\"1\">&nbsp;{0}</td>", "User ID:"));
        mailBody.Append(string.Format("<td style=\"width:90%;font-weight:bold\" colspan=\"1\">{0}</td>", intEmpID.ToString()));
        mailBody.Append("</tr>");

        mailBody.Append("<tr>");
        mailBody.Append(string.Format("<td style=\"padding-left:5px;font-weight:bold\" colspan=\"1\">&nbsp;{0}</td>", "Password:"));

        mailBody.Append(string.Format("<td style=\"font-weight:bold\" colspan=\"1\">{0}</td>", strNewPassword));

        mailBody.Append("</tr>");
        mailBody.Append("</table>");

        mailBody.Append("<br/> Regards,");
        mailBody.Append("<br/> Intelgain Pvt Ltd.");

        mailBody.Append("<br />");
        mailBody.Append("</body></html>");
        string strEncrptPassword = string.Empty;
        if (!isChangePassword)
        {
            strEncrptPassword = CreatrePassword(strNewPassword);
        }
        else
        {
            HttpContext context = HttpContext.Current;
            strEncrptPassword = CreatrePassword(newPwd);
        }

        UpdatePassword(strEmail, intEmpID, strEncrptPassword);

        return mailBody;
    }

    private string CreatrePassword(string strPassword)
    {
        string comppwd = null;
        string haskey = ConfigurationManager.AppSettings["hashKey"].ToString();
        byte tempbyte = byte.Parse("123");
        byte[] bytearr = new byte[2];
        bytearr[0] = tempbyte;
        return comppwd = SessionHelper.ComputeHash(strPassword, haskey, bytearr);
    }
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    public void UpdatePassword(string strEmail, int intEmpId, string strPassword)
    {
        string sql = string.Empty;
        sql = "UPDATE employeeMaster SET empPassword ='" + strPassword + "' WHERE empid = " + intEmpId.ToString() + " and empEmail = '" + strEmail + "'";

        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand(sql, con);
        Convert.ToInt32(cmd.ExecuteScalar());
        con.Close();
    }

    public List<Result> GetLeaveType()
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            UsersDAL.LeaveType objLeaveType = new UsersDAL.LeaveType();
            List<UsersDAL.LeaveType> lstLeaveType = objLeaveType.GetLeaveType();
            result.lstDropDownData = new List<DropDownData>();
            foreach (UsersDAL.LeaveType leavetype in lstLeaveType)
            {
                DropDownData ddl = new DropDownData();
                ddl.statusID = Convert.ToString(leavetype.statusid);
                ddl.statusDescription = leavetype.statusdesc;
                result.lstDropDownData.Add(ddl);
            }
        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);
        return data;
    }



    public List<Result> fnApplyingForLeaves(InputData Data)
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {

            DataTable dtLeaves = objDBFunction.ExecuteSQLRtnDT("EXEC spLeaveRequest'" + Data.EmpId + "','" + Data.leaveType + "','" + Data.strFromDate + "','" + Data.strToDate + "','" + Data.strReasonForLeave + "'");

            foreach (DataRow dr in dtLeaves.Rows)
            {
                int status = 0;
                status = (Convert.ToInt32(dr["Status"]));
                if (status != 1)
                {

                    DataTable dtApplyingForLeaves1 = objDBFunction.ExecuteSQLRtnDT("SELECT empName,empEmail FROM employeeMaster WHERE empId= '" + Data.EmpId + "'");
                    if (dtApplyingForLeaves1.Rows.Count > 0)
                    {
                        MailMessage msg = new MailMessage();
                        MailAddress msgFrom = new MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"),
                            ConfigurationManager.AppSettings.Get("compName"));
                        msg.From = msgFrom;



                        foreach (string emailAddress in dtApplyingForLeaves1.Rows[0]["empEmail"].ToString().Trim().Split(','))
                        {
                            msg.CC.Add(emailAddress);
                            msg.To.Add(System.Configuration.ConfigurationManager.AppSettings["HREmail"]);
                        }

                        msg.IsBodyHtml = true;
                        msg.Subject = dtApplyingForLeaves1.Rows[0]["empName"].ToString() + " has applied for leave.";
                        msg.Body = dtApplyingForLeaves1.Rows[0]["empName"].ToString() + " " + "has applied for leave " + "<b>From</b>" + ":" +
                                  Data.strFromDate + " " + "<b>TO</b>" + ":" + Data.strToDate + " " + "<br> Reason for Leave Is" + "<b>:</b>" + Data.strReasonForLeave;

                        SmtpClient mailClient = new SmtpClient();
                        mailClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings.Get("SMTP"));
                        mailClient.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Port"));
                        mailClient.Credentials = new System.Net.NetworkCredential("smtptest@dweb.in", "intelgain");
                        mailClient.EnableSsl = true;

                        mailClient.Send(msg);
                        result.Message = "Leave details have been send to your mail.";


                    }
                }
                else
                {
                    result.Message = "You have already apply leave for this date";
                }

            }
            result.Status = "1";
        }

        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

    public List<Result> fnLeaveStatus(InputData Data)
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "No Records";
        try
        {
            List<LeaveStatus> lstLeaveDetails = new List<LeaveStatus>();
            DataTable dtLeaveStatus = objDBFunction.ExecuteSQLRtnDT("EXEC sp_GetLeaveStatus '" + Data.EmpId + "'");

            if (dtLeaveStatus.Rows.Count > 0)
            {
                foreach (DataRow dr in dtLeaveStatus.Rows)
                {
                    LeaveStatus clsLeaveStatus = new LeaveStatus();
                    clsLeaveStatus.LeaveId = Convert.ToInt32(dr["empLeaveId"]);
                    clsLeaveStatus.leaveFromDate = Convert.ToString(dr["leaveFrom"]);
                    clsLeaveStatus.leaveToDate = Convert.ToString(dr["leaveTo"]);
                    clsLeaveStatus.leaveType = Convert.ToString(dr["desc1"]);
                    clsLeaveStatus.leaveDesc = Convert.ToString(dr["leaveDesc"]);
                    clsLeaveStatus.statusDesc = Convert.ToString(dr["ls"]);
                    clsLeaveStatus.adminComment = Convert.ToString(dr["leaveComment"]);
                    lstLeaveDetails.Add(clsLeaveStatus);
                }
                result.Status = "1";
                result.Message = "Data found.";
                result.lstLeaveStatus = lstLeaveDetails;
            }
        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);
        return data;
    }



    public List<Result> fnEmpleaveRequests()
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            List<LeaveRequests> lstLeaveRequests = new List<LeaveRequests>();
            DataTable dtLeaveRequests = objDBFunction.ExecuteSQLRtnDT("EXEC sp_GetEmpLeaveRequests");

            if (dtLeaveRequests.Rows.Count > 0)
            {
                foreach (DataRow dr in dtLeaveRequests.Rows)
                {
                    LeaveRequests clsLeaveRequests = new LeaveRequests();
                    clsLeaveRequests.empLeaveId = Convert.ToInt32(dr["empLeaveId"]);
                    clsLeaveRequests.empId = Convert.ToInt32(dr["empId"]);
                    clsLeaveRequests.empName = Convert.ToString(dr["empName"]);
                    clsLeaveRequests.leaveFromDate = Convert.ToString(dr["leaveFrom"]);
                    clsLeaveRequests.leaveToDate = Convert.ToString(dr["leaveTo"]);
                    clsLeaveRequests.leaveType = Convert.ToString(dr["desc1"]);
                    clsLeaveRequests.leaveDesc = Convert.ToString(dr["leaveDesc"]);
                    clsLeaveRequests.statusDesc = Convert.ToString(dr["ls"]);
                    clsLeaveRequests.adminComment = Convert.ToString(dr["leaveComment"]);
                    lstLeaveRequests.Add(clsLeaveRequests);
                }
                result.Status = "1";
                result.Message = "Data found.";
                result.lstLeaveRequests = lstLeaveRequests;
            }
        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);
        return data;
    }

    public List<Result> fnRemoveFavouriteEmp(InputData Data)
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Employee not added to favourites";
        try
        {

            DataTable dtFavourite = objDBFunction.ExecuteSQLRtnDT("EXEC sp_DeleteFavouriteEmp  '" + Data.EmpId + "','" + Data.FavEmpId + "'");
            if (dtFavourite.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFavourite.Rows)
                {
                    result.Status = Convert.ToString(dr["Status"]);
                    result.Message = Convert.ToString(dr["Message"]);
                }
            }
        }
        catch (Exception ex)
        {
            result.Status = "0";
            //result.Message = "Execution failed : " + ex.Message;
            result.Message = "" + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

    public List<Result> fnDeleteAppliedLeave(InputData Data)
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            DataTable dtDeleteLeave = objDBFunction.ExecuteSQLRtnDT("DELETE FROM empLeaveDetails where empLeaveId='" + Data.empLeaveId + "'");
            result.Status = "1";
            result.Message = "Deleted successfully";
        }
        catch (Exception ex)
        {

            result.Status = "0";
            result.Message = "" + ex.Message;
        }


        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

    public List<Result> fnAvailableLeaves(InputData Data)
    {

        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        string sql = string.Empty;
        string strcurrStartDate = string.Empty;
        string strcurrEndDate = string.Empty;
        int Prorata = 1;
        int availmonth;
        decimal diff, a;
        int plAvailMonth = 0;
        DateTime now = DateTime.Now;
        if (now.Month > 3)
        {
            strcurrStartDate = "1-Apr-" + now.Year;
            strcurrEndDate = "31-Mar-" + (now.Year + 1);
        }
        else
        {
            strcurrStartDate = "1-Apr-" + (now.Year - 1);
            strcurrEndDate = "31-Mar-" + now.Year;

            Prorata = (now.Month + 8) / 12;
        }
        Prorata = 1;
        AnnualLeaveDet clsAnnualLeaveDet = new AnnualLeaveDet();
        List<AnnualLeaveDet> lstAnnualLeaveDet = new List<AnnualLeaveDet>();
        DataTable dtAnnualLeavedet = objDBFunction.ExecuteSQLRtnDT("EXEC sp_GetAvailableLeaves '" + Convert.ToString(strcurrEndDate) + "','" + strcurrStartDate + "','" + Data.EmpId + "'");
        DataTable dtAnnualLeavedet1 = objDBFunction.ExecuteSQLRtnDT("SELECT * FROM empStatus WHERE statusLimit > 0");
        int num;
        foreach (DataRow dr in dtAnnualLeavedet.Rows)
        {
            string empJoiningDate = Convert.ToDateTime(dr["empJoiningDate"].ToString()).ToString("dd-MMM-yyyy");
            foreach (DataRow dr1 in dtAnnualLeavedet1.Rows)
            {
                if (Convert.ToString(dr1["statusId"].ToString()) == "CL")

                    clsAnnualLeaveDet.TotalCL = Convert.ToInt32(dr1["statusLimit"].ToString());

                bool isNumeric = int.TryParse(Convert.ToString(clsAnnualLeaveDet.TotalCL), out num);
                if (isNumeric)
                {
                    //clsAnnualLeaveDet.TotalCL = Convert.ToInt32(Strings.FormatNumber(clsAnnualLeaveDet.TotalCL * Prorata, 0));//needtochange
                    clsAnnualLeaveDet.TotalCL = Convert.ToInt32(clsAnnualLeaveDet.TotalCL * Prorata);//needtochange
                }


                if (Convert.ToString(dr1["statusId"].ToString()) == "SL")

                    clsAnnualLeaveDet.TotalSL = Convert.ToInt32(dr1["statusLimit"].ToString());
                bool isNumeric1 = int.TryParse(Convert.ToString(clsAnnualLeaveDet.TotalSL), out num);
                if (isNumeric1)
                {
                    //clsAnnualLeaveDet.TotalSL = Convert.ToInt32(Strings.FormatNumber(clsAnnualLeaveDet.TotalSL * Prorata, 0));//needtochange
                    clsAnnualLeaveDet.TotalSL = Convert.ToInt32(clsAnnualLeaveDet.TotalSL * Prorata);//needtochange
                }

                if (Convert.ToString(dr1["statusId"].ToString()) == "PL")
                {
                    clsAnnualLeaveDet.TotalPL = Convert.ToInt32(dr1["statusLimit"].ToString());
                    diff = (Convert.ToInt32(dr["DiffbetnEndJoiningDate"].ToString()) - 13) * (Convert.ToDecimal(dr1["statusLimit"].ToString()) / 12);
                    clsAnnualLeaveDet.TotalPL = Convert.ToInt32(diff);
                    a = (Convert.ToInt32(dr["DiffbetnStartJoiningDate"].ToString()) - 13) * (Convert.ToDecimal(dr1["statusLimit"].ToString()) / 12);
                    clsAnnualLeaveDet.TotalPL = Convert.ToInt32(dr1["statusLimit"].ToString()) + Convert.ToInt32(a);
                }
            }

            DataTable dtAnnualLeavedet2 = objDBFunction.ExecuteSQLRtnDT("select count(coID)as compOff from empCompOff  where empID='" + Data.EmpId + "' and codate >= DATEADD(D,-60,GETDATE())");
            foreach (DataRow dr2 in dtAnnualLeavedet2.Rows)
            {
                clsAnnualLeaveDet.TotalCO = Convert.ToInt32(dr2["compOff"].ToString());
            }

            if (Convert.ToInt32(dr["daysbetnconfrmtn"].ToString()) < 0)
            {
                availmonth = 11 - Convert.ToInt32(dr["CurStartnConfDate"].ToString());
                clsAnnualLeaveDet.TotalCL = Convert.ToInt32(Convert.ToDecimal(clsAnnualLeaveDet.TotalCL / 12) * availmonth); //needtochange
                clsAnnualLeaveDet.TotalSL = Convert.ToInt32(Math.Round(Convert.ToDecimal(clsAnnualLeaveDet.TotalSL / 12.0M) * availmonth, MidpointRounding.AwayFromZero));//needtochange
            }
            availmonth = Convert.ToInt32(dr["availMonth"].ToString());
            if (Convert.ToInt32(dr["TtlDateStart"].ToString()) < 0)
                clsAnnualLeaveDet.TotalPL = clsAnnualLeaveDet.TotalPL;
            else if ((Convert.ToDateTime(dr["plDateStart"].ToString())) > Convert.ToDateTime(strcurrEndDate))
                clsAnnualLeaveDet.TotalPL = 0;
            else
                plAvailMonth = Convert.ToInt32(dr["plAvailMonth"].ToString());

            DataTable dtAnnualLeavedet3 = objDBFunction.ExecuteSQLRtnDT("SELECT attStatus,count(*) as lCount FROM empAtt " +
                "WHERE empId=" + Data.EmpId + " AND attDate BETWEEN '" + strcurrStartDate +
                "' AND '" + strcurrEndDate + "' GROUP BY attStatus");
            foreach (DataRow dr3 in dtAnnualLeavedet3.Rows)
            {
                if (dr3["attStatus"].ToString() == "CL")
                    clsAnnualLeaveDet.ConsumedCL = Convert.ToInt32(dr3["lCount"].ToString());
                else if (dr3["attStatus"].ToString() == "SL")
                    clsAnnualLeaveDet.ConsumedSL = Convert.ToInt32(dr3["lCount"].ToString());
                else if (dr3["attStatus"].ToString() == "PL")
                    clsAnnualLeaveDet.ConsumedPL = Convert.ToInt32(dr3["lCount"].ToString());
                else if (dr3["attStatus"].ToString() == "CO")
                    clsAnnualLeaveDet.ConsumedCO = Convert.ToInt32(dr3["lCount"].ToString());
            }

            DataTable dtAnnualLeavedet4 = objDBFunction.ExecuteSQLRtnDT("SELECT count(*) as 'GetToatalPL' FROM empAtt " +
               "WHERE  attStatus='PL' and empId=" + Data.EmpId + " AND attDate BETWEEN '" + empJoiningDate +
               "' AND '" + strcurrStartDate + "'");
            foreach (DataRow dr4 in dtAnnualLeavedet4.Rows)
            {
                clsAnnualLeaveDet.TotalPL = clsAnnualLeaveDet.TotalPL - (Convert.ToInt32(dr4["GetToatalPL"].ToString()));
            }

            DataTable dtAnnualLeavedet5 = objDBFunction.ExecuteSQLRtnDT("SELECT count(*) as 'TotalCO'  FROM empAtt WHERE upper(attStatus)='CO' and empId=" + Data.EmpId + " and attDate >= DATEADD(D,-60,GETDATE())");
            foreach (DataRow dr5 in dtAnnualLeavedet5.Rows)
            {
                clsAnnualLeaveDet.ConsumedCO = Convert.ToInt32(dr5["TotalCO"].ToString());
            }
            DataTable dtAnnualLeavedet6 = objDBFunction.ExecuteSQLRtnDT("SELECT attStatus,count(*) as lCount FROM empAtt " +
                "WHERE empId=" + Data.EmpId + " AND attDate BETWEEN '" + strcurrStartDate +
                "' AND '" + strcurrEndDate + "' GROUP BY attStatus");

            foreach (DataRow dr6 in dtAnnualLeavedet5.Rows)
            {
                if (clsAnnualLeaveDet.TotalPL >= 90)
                {
                    clsAnnualLeaveDet.TotalPL = 90;
                }
            }

            clsAnnualLeaveDet.BalanceCL = clsAnnualLeaveDet.TotalCL - clsAnnualLeaveDet.ConsumedCL;
            clsAnnualLeaveDet.BalanceSL = clsAnnualLeaveDet.TotalSL - clsAnnualLeaveDet.ConsumedSL;
            clsAnnualLeaveDet.BalancePL = clsAnnualLeaveDet.TotalPL - clsAnnualLeaveDet.ConsumedPL;
            clsAnnualLeaveDet.BalanceCO = clsAnnualLeaveDet.TotalCO - clsAnnualLeaveDet.ConsumedCO;
            if (clsAnnualLeaveDet.TotalCO == 0)
            {
                clsAnnualLeaveDet.TotalCO = 0;
            }

            if (Convert.ToDateTime(dr["plDateStart"].ToString()) > DateTime.Now)
            {
                clsAnnualLeaveDet.BalancePL = 0;
                clsAnnualLeaveDet.ConsumedPL = 0;
                clsAnnualLeaveDet.TotalPL = 0;

            }

            if (Convert.ToDateTime(dr["empConfDate"].ToString()) > DateTime.Now)
            {
                clsAnnualLeaveDet.BalanceCL = 0;
                clsAnnualLeaveDet.ConsumedCL = 0;
                clsAnnualLeaveDet.TotalCL = 0;

                clsAnnualLeaveDet.BalanceSL = 0;
                clsAnnualLeaveDet.ConsumedSL = 0;
                clsAnnualLeaveDet.TotalSL = 0;
            }

            lstAnnualLeaveDet.Add(clsAnnualLeaveDet);
        }
        result.Status = "1";
        result.Message = "Data found.";
        result.lstAnnualLeaveDet = lstAnnualLeaveDet;



        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

    public List<Result> GetTimesheetDetails(InputData Data)
    {
        Result result = new Result();
        result.Status = "Success";
        result.Message = "Execution complete";
        try
        {
            ProjectModule objModule = new ProjectModule();
            List<ProjectModule> lstModule = ProjectModule.GetProjectModulesByProjId(Data.projectID);

            foreach (ProjectModule moduleDetail in lstModule)
            {
                TimesheetModuleData clsModule = new TimesheetModuleData();
                clsModule.ModuleID = Convert.ToInt32(moduleDetail.ID);
                clsModule.ModuleName = Convert.ToString(moduleDetail.Name);
                result.lstModule.Add(clsModule);

            }

            UsersDAL.Timesheet objTimesheet = new UsersDAL.Timesheet();
            List<UsersDAL.Timesheet> lstTimesheet = objTimesheet.getTimesheetData(Data.projectID, Data.EmpId, Data.strMonth, Data.strYear, Data.arg, Data.status);

            foreach (UsersDAL.Timesheet timesheetDetails in lstTimesheet)
            {

                TimesheetGridData clsTimesheet = new TimesheetGridData();
                clsTimesheet.TimesheetID = Convert.ToInt32(timesheetDetails.timesheetID);
                clsTimesheet.TimesheetDate = Convert.ToString(timesheetDetails.TimesheetDate);
                clsTimesheet.ProjectName = Convert.ToString(timesheetDetails.ProjectName);
                clsTimesheet.Module = Convert.ToString(timesheetDetails.Module);
                clsTimesheet.TimesheetHours = Convert.ToString(timesheetDetails.TimesheetHours);
                clsTimesheet.Comment = Convert.ToString(timesheetDetails.Comment);
                clsTimesheet.TimesheetEntryDate = Convert.ToString(timesheetDetails.TimesheetEntryDate);
                result.lstTimesheet.Add(clsTimesheet);
            }
        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);
        return data;
    }

    public List<Result> defaultBindTimesheet(InputData Data)
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        int intMonth = DateTime.Now.Month;
        int intYear = DateTime.Now.Year;
        DateTime currentDate = DateTime.Now;

        intMonth = currentDate.Month;
        intYear = currentDate.Year;
        string projID = "0";

        try
        {
            if ((intMonth.ToString() == Data.strMonth) && (intYear.ToString() == Data.strYear))
            {
                intMonth = currentDate.Month;
                intYear = currentDate.Year;
            }
            else
            {
                intMonth = Convert.ToInt32(Data.strMonth);
                intYear = Convert.ToInt32(Data.strYear);
            }
            List<TimesheetGridData> lstTimesheetGridData = new List<TimesheetGridData>();
            DataTable dtTimesheet = objDBFunction.ExecuteSQLRtnDT("EXEC sp_MemberTimeSheet '" + Data.EmpId + "','" + projID + "','" + intMonth + "','" + intYear + "'");
            if (dtTimesheet.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTimesheet.Rows)
                {
                    TimesheetGridData clsTimesheet = new TimesheetGridData();
                    clsTimesheet.TimesheetID = Convert.ToInt32(dr["tsId"]);
                    clsTimesheet.TimesheetDate = Convert.ToString(dr["tsDate"]);
                    clsTimesheet.ProjectName = Convert.ToString(dr["projName"]);
                    clsTimesheet.Module = Convert.ToString(dr["moduleName1"]);
                    clsTimesheet.TimesheetHours = Convert.ToString(dr["tsHour"]);
                    clsTimesheet.Comment = Convert.ToString(dr["tsComment"]);
                    clsTimesheet.TimesheetEntryDate = Convert.ToString(dr["tsEntryDate"]);
                    lstTimesheetGridData.Add(clsTimesheet);
                }
                result.Status = "1";
                result.Message = "Data found.";
                result.lstTimesheet = lstTimesheetGridData;
            }
        }
        catch (Exception ex)
        {
            result.Status = "0";
            result.Message = "Execution failed : " + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);
        return data;
    }

    public List<Result> fnInsertLateComing(InputData Data)
    {
        Result result = new Result();
        result.Status = "0";
        result.Message = "Execution complete";
        try
        {
            List<LateComing> lstLateComing = new List<LateComing>();
            DataTable dtLateComing = objDBFunction.ExecuteSQLRtnDT("EXEC sp_InsertLateComing  '" + Data.EmpCode + "','" + Data.ApplyDate + "', '" + Data.ExpectedInTime + "', '" + Data.CreatedOn + "', '" + Data.CreatedBy + "', '" + Data.LateCommingReason + "', '" + Data.ApprovedOn + "', '" + Data.ApprovedBy + "', '" + Data.ApprovalComment + "', '" + Data.IsApproveStatus + "'");
            if (dtLateComing.Rows.Count > 0)
            {
                foreach (DataRow dr in dtLateComing.Rows)
                {
                    result.Status = Convert.ToString(dr["Status"]);
                    result.Message = Convert.ToString(dr["Message"]);
                }
            }
            else
            {
                result.Status = "0";
                result.Message = "Not added to Favourites.";
            }
        }
        catch (Exception ex)
        {
            result.Status = "0";
            //result.Message = "Execution failed : " + ex.Message;
            result.Message = "" + ex.Message;
        }
        List<Result> data = new List<Result>();
        data.Add(result);

        return data;
    }

}

public class InputData
{
    public int EmpId;
    public int FavEmpId;
    public string OldPassword = string.Empty;
    public string Password = string.Empty;
    public string NewPassword = string.Empty;
    public string ConfirmPassword = string.Empty;
    public string strEmpName = string.Empty;
    public string strMonth = string.Empty;
    public string strYear = string.Empty;
    public string strStartDate = string.Empty;
    public string strEndDate = string.Empty;
    public string EmailID = string.Empty;
    public string leaveType = string.Empty;
    public string strFromDate = string.Empty;
    public string strToDate = string.Empty;
    public string strReasonForLeave = string.Empty;
    public int SanctionById;
    public string leaveStatus = string.Empty;
    static DateTime today = DateTime.Today; // As DateTime
    public string SanctionedDate = today.ToString("yyyy/MM/dd");
    public int empLeaveId;
    public string adminComments { get; set; }
    public string SanctionByName { get; set; }
    public int projectID { get; set; }
    public string arg { get; set; }
    public string status { get; set; }
    public int moduleId { get; set; }
    public string timesheetDate { get; set; }
    public int timesheetHours { get; set; }
    public string comment { get; set; }
    public int tsID { get; set; }
    public int EmpCode { get; set; }
    public DateTime ApplyDate { get; set; }
    public DateTime ExpectedInTime { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public string LateCommingReason { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public int? ApprovedBy { get; set; }
    public string ApprovalComment { get; set; }
    public bool? IsApproveStatus { get; set; }

}



[DataContract]
public class Result
{
    [DataMember]
    public string Status = string.Empty;
    [DataMember]
    public string Message = string.Empty;

    [DataMember]
    public List<UserInfo> lstUserInfo = new List<UserInfo>();
    [DataMember]
    public List<EmployeeName> lstEmployeeName = new List<EmployeeName>();
    [DataMember]
    public List<FavouriteEmployees> lstFavouriteEmployees = new List<FavouriteEmployees>();
    [DataMember]
    public List<MissedDates> lstMissedDates = new List<MissedDates>();
    [DataMember]
    public List<AnnualLeaveDet> lstAnnualLeaveDet = new List<AnnualLeaveDet>();
    [DataMember]
    public List<ForgotPassword> lstForgotPassword = new List<ForgotPassword>();
    [DataMember]
    public List<DropDownData> lstDropDownData = new List<DropDownData>();
    [DataMember]
    public List<LeaveStatus> lstLeaveStatus = new List<LeaveStatus>();
    [DataMember]
    public List<LeaveRequests> lstLeaveRequests = new List<LeaveRequests>();

    [DataMember]
    public List<EmpProjectTimesheet> lstEmpProjectTimesheet = new List<EmpProjectTimesheet>();

    [DataMember]
    public List<UsersDAL.Projects> lstProjects = new List<UsersDAL.Projects>();

    [DataMember]
    public List<TimesheetModuleData> lstModule = new List<TimesheetModuleData>();

    [DataMember]
    public List<TimesheetGridData> lstTimesheet = new List<TimesheetGridData>();

    [DataMember]
    public List<Holiday> lstHoliday = new List<Holiday>();
}

[DataContract]
public class TimesheetModuleData
{
    private int intModuleID;
    [DataMember]
    public int ModuleID
    {
        get { return intModuleID; }
        set { intModuleID = value; }
    }

    private string strModuleName;
    [DataMember]
    public string ModuleName
    {
        get { return strModuleName; }
        set { strModuleName = value; }
    }

}
[DataContract]
public class TimesheetGridData
{
    private int intTimesheetID;
    [DataMember]
    public int TimesheetID
    {
        get { return intTimesheetID; }
        set { intTimesheetID = value; }
    }


    private string strTimesheetDate;
    [DataMember]
    public string TimesheetDate
    {
        get { return strTimesheetDate; }
        set { strTimesheetDate = value; }
    }

    private string strProjectName;
    [DataMember]
    public string ProjectName
    {
        get { return strProjectName; }
        set { strProjectName = value; }
    }

    private string strModule;
    [DataMember]
    public string Module
    {
        get { return strModule; }
        set { strModule = value; }
    }
    private string strTimesheethours;
    [DataMember]
    public string TimesheetHours
    {
        get { return strTimesheethours; }
        set { strTimesheethours = value; }
    }

    private string strComment;
    [DataMember]
    public string Comment
    {
        get { return strComment; }
        set { strComment = value; }
    }

    private string strEntryDate;
    [DataMember]
    public string TimesheetEntryDate
    {
        get { return strEntryDate; }
        set { strEntryDate = value; }
    }
}


[DataContract]
public class EmpProjectTimesheet
{



    private string strEmpName;
    [DataMember]
    public string EmployeeName
    {
        get { return strEmpName; }
        set { strEmpName = value; }
    }

    private string strTimesheetDate;
    [DataMember]
    public string TimesheetDate
    {
        get { return strTimesheetDate; }
        set { strTimesheetDate = value; }
    }

    private string strTimesheetHours;
    [DataMember]
    public string TimesheetHours
    {
        get { return strTimesheetHours; }
        set { strTimesheetHours = value; }
    }

    private string strModule;
    [DataMember]
    public string Module
    {
        get { return strModule; }
        set { strModule = value; }
    }

    private string strProjectName;
    [DataMember]
    public string ProjectName
    {
        get { return strProjectName; }
        set { strProjectName = value; }
    }

    private string strTimesheetEntryDate;
    [DataMember]
    public string TimesheetEntryDate
    {
        get { return strTimesheetEntryDate; }
        set { strTimesheetEntryDate = value; }
    }

    private string strComment;
    [DataMember]
    public string Comment
    {
        get { return strComment; }
        set { strComment = value; }
    }
}
[DataContract]
public class UserInfo
{
    private string strLoginMessage;
    [DataMember]
    public string LoginMessage
    {
        get { return strLoginMessage; }
        set { strLoginMessage = value; }
    }
    private int LoginStatus;
    [DataMember]
    public int Status
    {
        get { return LoginStatus; }
        set { LoginStatus = value; }
    }
    private int intEmpID;
    [DataMember]
    public int EmployeeID
    {
        get { return intEmpID; }
        set { intEmpID = value; }
    }

    private string strUserType;
    [DataMember]
    public string UserType
    {
        get { return strUserType; }
        set { strUserType = value; }
    }

    private int intSecurityLevel;
    [DataMember]
    public int SecurityLevel
    {
        get { return intSecurityLevel; }
        set { intSecurityLevel = value; }
    }

    private bool lbnIsAdmin;
    [DataMember]
    public bool IsAdmin
    {
        get { return lbnIsAdmin; }
        set { lbnIsAdmin = value; }
    }

    private int intSkillID;
    [DataMember]
    public int SkillID
    {
        get { return intSkillID; }
        set { intSkillID = value; }
    }


    private string strDesignation;
    [DataMember]
    public string Designation
    {
        get { return strDesignation; }
        set { strDesignation = value; }
    }


    private string strEmpName;
    [DataMember]
    public string Name
    {
        get { return strEmpName; }
        set { strEmpName = value; }
    }

    private string strEmpAddress;
    [DataMember]
    public string Address
    {
        get { return strEmpAddress; }
        set { strEmpAddress = value; }
    }

    private string strEmpContact;
    [DataMember]
    public string Contact
    {
        get { return strEmpContact; }
        set { strEmpContact = value; }
    }


    private string strEmpJoiningDate;
    [DataMember]
    public string JoiningDate
    {
        get { return strEmpJoiningDate; }
        set { strEmpJoiningDate = value; }
    }

    private string strEmpConfirmationDate;
    [DataMember]
    public string ConfirmationDate
    {
        get { return strEmpConfirmationDate; }
        set { strEmpConfirmationDate = value; }
    }

    private string strEmpLeavingDate;
    [DataMember]
    public string LeavingDate
    {
        get { return strEmpLeavingDate; }
        set { strEmpLeavingDate = value; }
    }

    private string strEmpProbationPeriod;
    [DataMember]
    public string ProbationPeriod
    {
        get { return strEmpProbationPeriod; }
        set { strEmpProbationPeriod = value; }
    }

    private string strEmpNotes;
    [DataMember]
    public string Notes
    {
        get { return strEmpNotes; }
        set { strEmpNotes = value; }
    }

    private string strEmpEmail;
    [DataMember]
    public string EmailID
    {
        get { return strEmpEmail; }
        set { strEmpEmail = value; }
    }

    private int intEmpTester;
    [DataMember]
    public int Tester
    {
        get { return intEmpTester; }
        set { intEmpTester = value; }
    }

    private string strEmpAccountNo;
    [DataMember]
    public string AccountNo
    {
        get { return strEmpAccountNo; }
        set { strEmpAccountNo = value; }
    }

    private string strEmpBDate;
    [DataMember]
    public string BDate
    {
        get { return strEmpBDate; }
        set { strEmpBDate = value; }
    }

    private string strEmpADate;
    [DataMember]
    public string ADate
    {
        get { return strEmpADate; }
        set { strEmpADate = value; }
    }

    private string strEmpPrevEmployer;
    [DataMember]
    public string PreviousEmployer
    {
        get { return strEmpPrevEmployer; }
        set { strEmpPrevEmployer = value; }
    }

    private int intEmpExperience;
    [DataMember]
    public int Experience
    {
        get { return intEmpExperience; }
        set { intEmpExperience = value; }
    }

    private bool blnEmpIsSuperAdmin;
    [DataMember]
    public bool IsSuperAdmin
    {
        get { return blnEmpIsSuperAdmin; }
        set { blnEmpIsSuperAdmin = value; }
    }

    private bool blnEmpIsAccountAdmin;
    [DataMember]
    public bool IsAccountAdmin
    {
        get { return blnEmpIsAccountAdmin; }
        set { blnEmpIsAccountAdmin = value; }
    }

    private bool blnEmpIsPayrollAdmin;
    [DataMember]
    public bool IsPayrollAdmin
    {
        get { return blnEmpIsPayrollAdmin; }
        set { blnEmpIsPayrollAdmin = value; }
    }

    private bool blnEmpIsPM;
    [DataMember]
    public bool IsPM
    {
        get { return blnEmpIsPM; }
        set { blnEmpIsPM = value; }
    }

    private bool blnEmpIsTester;
    [DataMember]
    public bool IsTester
    {
        get { return blnEmpIsTester; }
        set { blnEmpIsTester = value; }
    }

    private bool blnEmpIsProjectReport;
    [DataMember]
    public bool IsProjectReport
    {
        get { return blnEmpIsProjectReport; }
        set { blnEmpIsProjectReport = value; }
    }

    private bool blnEmpIsProjectStatus;
    [DataMember]
    public bool IsProjectStatus
    {
        get { return blnEmpIsProjectStatus; }
        set { blnEmpIsProjectStatus = value; }
    }

    private bool blnEmpIsLeaveAdmin;
    [DataMember]
    public bool IsLeaveAdmin
    {
        get { return blnEmpIsLeaveAdmin; }
        set { blnEmpIsLeaveAdmin = value; }
    }

    private string strEmpInsertedOn;
    [DataMember]
    public string InsertedOn
    {
        get { return strEmpInsertedOn; }
        set { strEmpInsertedOn = value; }
    }

    private string strEmpInsertedBy;
    [DataMember]
    public string InsertedBy
    {
        get { return strEmpInsertedBy; }
        set { strEmpInsertedBy = value; }
    }

    private string strEmpInsertedIP;
    [DataMember]
    public string InsertedIP
    {
        get { return strEmpInsertedIP; }
        set { strEmpInsertedIP = value; }
    }

    private string strEmpLocationID;
    [DataMember]
    public string LocationID
    {
        get { return strEmpLocationID; }
        set { strEmpLocationID = value; }
    }
    private string strProfileID;
    [DataMember]
    public string ProfileID
    {
        get { return strProfileID; }
        set { strProfileID = value; }
    }
}

[DataContract]
public class EmployeeName
{
    private string strEmployeeName;
    [DataMember]
    public string EmpName
    {
        get { return strEmployeeName; }
        set { strEmployeeName = value; }
    }
    private int EmpId;
    [DataMember]
    public int EmployeeID
    {
        get { return EmpId; }
        set { EmpId = value; }
    }

    private string strEmpEmail;
    [DataMember]
    public string EmpEmail
    {
        get { return strEmpEmail; }
        set { strEmpEmail = value; }
    }

    private string strEmpContact;
    [DataMember]
    public string EmpContact
    {
        get { return strEmpContact; }
        set { strEmpContact = value; }
    }

    private string strFlag;
    [DataMember]
    public string Flag
    {
        get { return strFlag; }
        set { strFlag = value; }
    }

    //private byte[] photoBytes;
    //[DataMember]
    //public byte[] Photo
    //{
    //    get { return photoBytes; }
    //    set { photoBytes = value; }
    //}
}

[DataContract]
public class FavouriteEmployees
{
    private int intFavouriteEmpId;
    [DataMember]
    public int FavouriteEmpId
    {
        get { return intFavouriteEmpId; }
        set { intFavouriteEmpId = value; }
    }

    private string strFavouriteEmpName;
    [DataMember]
    public string FavouriteEmpName
    {
        get { return strFavouriteEmpName; }
        set { strFavouriteEmpName = value; }
    }

    private string strFavouriteEmpContact;
    [DataMember]
    public string FavouriteEmpContact
    {
        get { return strFavouriteEmpContact; }
        set { strFavouriteEmpContact = value; }
    }

    private string strFavouriteFlag;
    [DataMember]
    public string FavouriteFlag
    {
        get { return strFavouriteFlag; }
        set { strFavouriteFlag = value; }
    }

    private string strFavouriteEmpEmail;
    [DataMember]
    public string FavouriteEmpEmail
    {
        get { return strFavouriteEmpEmail; }
        set { strFavouriteEmpEmail = value; }
    }
}

[DataContract]
public class MissedDates
{
    private string strMissedDate;
    [DataMember]
    public string MissedDate
    {
        get { return strMissedDate; }
        set { strMissedDate = value; }
    }

    private string strDescription;
    [DataMember]
    public string Description
    {
        get { return strDescription; }
        set { strDescription = value; }
    }
    private string strFlag;
    [DataMember]
    public string Flag
    {
        get { return strFlag; }
        set { strFlag = value; }
    }
    private string strAttendanceStatus;
    [DataMember]
    public string AttendanceStatus
    {
        get { return strAttendanceStatus; }
        set { strAttendanceStatus = value; }
    }
    private string strAttIntime;
    [DataMember]
    public string AttInTime
    {
        get { return strAttIntime; }
        set { strAttIntime = value; }
    }
    private string strAttOuttime;
    [DataMember]
    public string AttOutTime
    {
        get { return strAttOuttime; }
        set { strAttOuttime = value; }
    }
    private string strWorkingHours;
    [DataMember]
    public string WorkingHours
    {
        get { return strWorkingHours; }
        set { strWorkingHours = value; }
    }

    private string strTimesheetHours;
    [DataMember]
    public string TimesheetHours
    {
        get { return strTimesheetHours; }
        set { strTimesheetHours = value; }
    }
}

[DataContract]
public class AnnualLeaveDet
{

    private int strTotalCL;
    [DataMember]
    public int TotalCL
    {
        get { return strTotalCL; }
        set { strTotalCL = value; }
    }

    private int strTotalSL;
    [DataMember]
    public int TotalSL
    {
        get { return strTotalSL; }
        set { strTotalSL = value; }
    }

    private int strTotalPL;
    [DataMember]
    public int TotalPL
    {
        get { return strTotalPL; }
        set { strTotalPL = value; }
    }

    private int strTotalCO;
    [DataMember]
    public int TotalCO
    {
        get { return strTotalCO; }
        set { strTotalCO = value; }
    }

    private int strConsumedCL;
    [DataMember]
    public int ConsumedCL
    {
        get { return strConsumedCL; }
        set { strConsumedCL = value; }
    }



    private int strConsumedSL;
    [DataMember]
    public int ConsumedSL
    {
        get { return strConsumedSL; }
        set { strConsumedSL = value; }
    }


    private int strConsumedPL;
    [DataMember]
    public int ConsumedPL
    {
        get { return strConsumedPL; }
        set { strConsumedPL = value; }
    }


    private int strConsumedCO;
    [DataMember]
    public int ConsumedCO
    {
        get { return strConsumedCO; }
        set { strConsumedCO = value; }
    }


    private int strBalanceCL;
    [DataMember]
    public int BalanceCL
    {
        get { return strBalanceCL; }
        set { strBalanceCL = value; }
    }

    private int strBalanceSL;
    [DataMember]
    public int BalanceSL
    {
        get { return strBalanceSL; }
        set { strBalanceSL = value; }
    }

    private int strBalancePL;
    [DataMember]
    public int BalancePL
    {
        get { return strBalancePL; }
        set { strBalancePL = value; }
    }

    private int strBalanceCO;
    [DataMember]
    public int BalanceCO
    {
        get { return strBalanceCO; }
        set { strBalanceCO = value; }
    }
}

[DataContract]
public class ForgotPassword
{

}


[DataContract]
public class ChangePassword
{

}

[DataContract]
public class DropDownData
{
    [DataMember]
    public string statusID { get; set; }

    [DataMember]
    public string statusDescription { get; set; }
    [DataMember]
    public string projectID { get; set; }

    [DataMember]
    public string projectName { get; set; }
}


[DataContract]
public class LeaveStatus
{

    private int intLeaveId;
    [DataMember]
    public int LeaveId
    {
        get { return intLeaveId; }
        set { intLeaveId = value; }
    }


    private string strFromDate;
    [DataMember]
    public string leaveFromDate
    {
        get { return strFromDate; }
        set { strFromDate = value; }
    }



    private string strToDate;
    [DataMember]
    public string leaveToDate
    {
        get { return strToDate; }
        set { strToDate = value; }
    }

    private string strLeaveType;
    [DataMember]
    public string leaveType
    {
        get { return strLeaveType; }
        set { strLeaveType = value; }
    }


    private string strleaveDesc;
    [DataMember]
    public string leaveDesc
    {
        get { return strleaveDesc; }
        set { strleaveDesc = value; }
    }

    private string strStatusDesc;
    [DataMember]
    public string statusDesc
    {
        get { return strStatusDesc; }
        set { strStatusDesc = value; }
    }

    private string strLeaveComment;
    [DataMember]
    public string adminComment
    {
        get { return strLeaveComment; }
        set { strLeaveComment = value; }
    }
}
[DataContract]
public class LeaveRequests
{
    private int intEmpeaveid;
    [DataMember]
    public int empLeaveId
    {
        get { return intEmpeaveid; }
        set { intEmpeaveid = value; }
    }

    private int intEmpid;
    [DataMember]
    public int empId
    {
        get { return intEmpid; }
        set { intEmpid = value; }
    }

    private string strEmpName;
    [DataMember]
    public string empName
    {
        get { return strEmpName; }
        set { strEmpName = value; }
    }

    private string strFromDate;
    [DataMember]
    public string leaveFromDate
    {
        get { return strFromDate; }
        set { strFromDate = value; }
    }

    private string strToDate;
    [DataMember]
    public string leaveToDate
    {
        get { return strToDate; }
        set { strToDate = value; }
    }

    private string strLeaveType;
    [DataMember]
    public string leaveType
    {
        get { return strLeaveType; }
        set { strLeaveType = value; }
    }


    private string strleaveDesc;
    [DataMember]
    public string leaveDesc
    {
        get { return strleaveDesc; }
        set { strleaveDesc = value; }
    }

    private string strStatusDesc;
    [DataMember]
    public string statusDesc
    {
        get { return strStatusDesc; }
        set { strStatusDesc = value; }
    }

    private string strLeaveComment;
    [DataMember]
    public string adminComment
    {
        get { return strLeaveComment; }
        set { strLeaveComment = value; }
    }
}