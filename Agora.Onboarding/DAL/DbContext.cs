using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Agora.Onboarding.Models;
using System.Runtime.InteropServices;

namespace Agora.Onboarding.DAL
{
    public class DbContext
    {
        private string cs = ConfigurationManager.ConnectionStrings["AgoraConnStr"].ConnectionString;
        private static string connStr = ConfigurationManager.ConnectionStrings["AgoraConnStr"].ConnectionString;
        public List<EmployeeMaster> GetEmployeeMasterList(int? empID)
        {
            List<EmployeeMaster> employeeMasters = new List<EmployeeMaster>();
            EmployeeMaster employee = new EmployeeMaster();
            SqlConnection con = new SqlConnection(cs);
            var storeProcedure = "[dbo].[Sp_GetEmpDetailsByEmployeeId]";
            SqlCommand cmd = new SqlCommand(storeProcedure, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@empID", empID);
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    employee.EmpId = Convert.ToInt32(dt.Rows[0]["empid"]);
                    employee.EmpName = dt.Rows[0]["empName"].ToString();
                    employee.EmpPassword = dt.Rows[0]["empPassword"].ToString();
                    employee.EmpAddress = dt.Rows[0]["empEmail"].ToString();
                }
                employeeMasters.Add(employee);


                return employeeMasters;
            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }
        }
        public void SaveOnboarding(string type, int empId, string empName, bool
                                  timesheetCheck, bool leaveCheck, bool faqCheck
                                  , bool confCheck, bool hrCheck, bool IT_check
                                  , bool skypeAccount_check,bool MS365_check,bool skypeInvite_check
                                  , bool registration_check,bool PI_check,bool form_check
                                  , bool isCompleted, string signImage, string InsertedOn)
        {

            SqlConnection con = new SqlConnection(cs);
            var storeProcedure = "[dbo].[Save_Onboarding]";
            SqlCommand cmd = new SqlCommand(storeProcedure, con);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@EmpId", empId);
                cmd.Parameters.AddWithValue("@Empname", empName);
                cmd.Parameters.AddWithValue("@Timesheet_check", timesheetCheck);
                cmd.Parameters.AddWithValue("@Leave_check", leaveCheck);
                cmd.Parameters.AddWithValue("@WFH_check", faqCheck);
                cmd.Parameters.AddWithValue("@Confidentiality_check", confCheck);
                cmd.Parameters.AddWithValue("@HR_check", hrCheck);
                cmd.Parameters.AddWithValue("@ITInduction_check", IT_check);
                cmd.Parameters.AddWithValue("@SkypeAccount_check", skypeAccount_check);
                cmd.Parameters.AddWithValue("@MS365_check", MS365_check);
                cmd.Parameters.AddWithValue("@SkypeInvite_check", skypeInvite_check);
                cmd.Parameters.AddWithValue("@Registration_check", registration_check);
                cmd.Parameters.AddWithValue("@PI_check", PI_check);
                cmd.Parameters.AddWithValue("@Form_check", form_check);
                cmd.Parameters.AddWithValue("@IsCompleted", isCompleted);
                if (!string.IsNullOrEmpty(InsertedOn))
                {
                    cmd.Parameters.AddWithValue("@InsertedOn", Convert.ToDateTime(InsertedOn));
                }
                if (!string.IsNullOrEmpty(signImage))
                {
                    cmd.Parameters.AddWithValue("@SignImage", signImage);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }
        }
        public List<Onboardings> GetOnboardingById(int? empID)
        {
            List<Onboardings> onboardings = new List<Onboardings>();
            Onboardings onbordingObj = new Onboardings();
            SqlConnection con = new SqlConnection(cs);
            var storeProcedureName = "[dbo].[GetOnboardingById]";
            SqlCommand cmd = new SqlCommand(storeProcedureName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpID", empID);
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    onbordingObj.EmpId = Convert.ToInt32(dt.Rows[0]["empid"]);
                    onbordingObj.Empname = dt.Rows[0]["empName"].ToString();
                    onbordingObj.Timesheet_check = Convert.ToBoolean(dt.Rows[0]["Timesheet_check"]);
                    onbordingObj.Leave_check = Convert.ToBoolean(dt.Rows[0]["Leave_check"]);
                    onbordingObj.WFH_check = Convert.ToBoolean(dt.Rows[0]["WFH_check"]);
                    onbordingObj.Confidentiality_check = Convert.ToBoolean(dt.Rows[0]["Confidentiality_check"]);
                    onbordingObj.HR_Manual_check = Convert.ToBoolean(dt.Rows[0]["HR_Manual_check"]);
                    onbordingObj.ITInduction_check = Convert.ToBoolean(dt.Rows[0]["ITInduction_check"]);
                    onbordingObj.SkypeAccount_check = Convert.ToBoolean(dt.Rows[0]["SkypeAccount_check"]);
                    onbordingObj.MS365_check = Convert.ToBoolean(dt.Rows[0]["MS365_check"]);
                    onbordingObj.SkypeInvite_check = Convert.ToBoolean(dt.Rows[0]["SkypeInvite_check"]);
                    onbordingObj.Registration_check = Convert.ToBoolean(dt.Rows[0]["Registration_check"]);
                    onbordingObj.PI_check = Convert.ToBoolean(dt.Rows[0]["PI_check"]);
                    onbordingObj.Form_check = Convert.ToBoolean(dt.Rows[0]["Form_check"]);
                    onbordingObj.IsCompleted = Convert.ToBoolean(dt.Rows[0]["IsCompleted"]);
                    onbordingObj.SignImage = dt.Rows[0]["SignImage"].ToString();
                    onbordingObj.InsertedOn = dt.Rows[0]["InsertedOn"].ToString();


                }
                onboardings.Add(onbordingObj);


                return onboardings;
            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }


        }
        public static void LogErrorToDB(LogExceptions logExceptions)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection sqlConn = new SqlConnection(connStr);
            try
            {
                HttpContext ctxObject = HttpContext.Current;
                //string strLogConnString = ConfigurationManager.ConnectionStrings["DbAgora"].ConnectionString;
                string logDateTime = DateTime.Now.ToString("g");
                string strReqURL = (ctxObject.Request.Url != null) ? ctxObject.Request.Url.ToString() : String.Empty;
                string strReqQS = (ctxObject.Request.QueryString != null) ? ctxObject.Request.QueryString.ToString() : String.Empty;
                string strServerName = String.Empty;
                if (ctxObject.Request.ServerVariables["HTTP_REFERER"] != null)
                {
                    strServerName = ctxObject.Request.ServerVariables["HTTP_REFERER"].ToString();
                }
                string strUserAgent = (ctxObject.Request.UserAgent != null) ? ctxObject.Request.UserAgent : String.Empty;
                string strUserIP = (ctxObject.Request.UserHostAddress != null) ? ctxObject.Request.UserHostAddress : String.Empty;
                string strUserAuthen = (ctxObject.User.Identity.IsAuthenticated.ToString() != null) ? ctxObject.User.Identity.IsAuthenticated.ToString() : String.Empty;
                string strUserName = (ctxObject.User.Identity.Name != null) ? ctxObject.User.Identity.Name : String.Empty;
                string strMessage = string.Empty, strSource = string.Empty, strTargetSite = string.Empty, strStackTrace = string.Empty;
                if (logExceptions != null)
                {
                    strMessage = logExceptions.Message;
                    strSource = logExceptions.Source;
                    strTargetSite = logExceptions.TargetSite.ToString();
                    strStackTrace = logExceptions.StackTrace;
                    //logExceptions = logExceptions.InnerException;
                }
                if (connStr.Length > 0)
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_LogExceptionToDB";
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.Add(new SqlParameter("@Source", strSource));
                    cmd.Parameters.Add(new SqlParameter("@LogDateTime", logDateTime));
                    cmd.Parameters.Add(new SqlParameter("@Message", strMessage));
                    cmd.Parameters.Add(new SqlParameter("@QueryString", strReqQS));
                    cmd.Parameters.Add(new SqlParameter("@TargetSite", strTargetSite));
                    cmd.Parameters.Add(new SqlParameter("@StackTrace", strStackTrace));
                    cmd.Parameters.Add(new SqlParameter("@ServerName", strServerName));
                    cmd.Parameters.Add(new SqlParameter("@RequestURL", strReqURL));
                    cmd.Parameters.Add(new SqlParameter("@UserAgent", strUserAgent));
                    cmd.Parameters.Add(new SqlParameter("@UserIP", strUserIP));
                    cmd.Parameters.Add(new SqlParameter("@UserName", strUserName));
                    cmd.Parameters.Add(new SqlParameter("@Mode", "Insert"));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sqlConn.Close();
                }
            }
            catch (Exception exc)
            {
                throw exc;

            }
            finally
            {
                cmd.Dispose();
                sqlConn.Close();
            }


        }
        public List<GetHrPolicy> GetHrPolicyFilesDetails(string mode)
        {
            List<GetHrPolicy> objPolicyBLL = new List<GetHrPolicy>();
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("USP_HrPolicy", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@FileName", "");
            cmd.Parameters.AddWithValue("@CustomFileName", "");
            cmd.Parameters.AddWithValue("@FileUploadPath", "");
            cmd.Parameters.AddWithValue("@DisplayFileURL", "");
            cmd.Parameters.AddWithValue("@FileTypeId", "");
            SqlDataReader dr = null;
            using (con)
            {
                con.Open();
                dr = cmd.ExecuteReader();
                int i = 0;
                while (dr.Read())
                {
                    GetHrPolicy obj = new GetHrPolicy();
                    obj.FileName = dr["FileName"].ToString();
                    obj.CustomFileName = dr["CustomFileName"].ToString();
                    obj.FileUploadPath = dr["FileUploadPath"].ToString();
                    obj.DisplayFileURL = dr["DisplayFileURL"].ToString();
                    obj.IsActive = Convert.ToInt32(dr["IsActive"]);
                    obj.CreateDate = Convert.ToDateTime(dr["CreateDate"]);
                    objPolicyBLL.Add(obj);
                    i++;

                }
            }
            return objPolicyBLL;
        }
    }
}