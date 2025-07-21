using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Data;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Configuration;
using System.Drawing.Imaging;
using AgoraBL.BAL;
using System.Runtime.Serialization;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Class to serve Mobile clients via API
/// </summary>

namespace nsMobileAPI
{
    public class clsMobileAPI : clsMobileProcessing, IHttpHandler
    {
        // Blank Constructor
        public clsMobileAPI()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        # region "IHttpHandler Implementation"
        /// <summary>
        /// Propoerty not used
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }
        /// <summary>
        ///  Used to intercept HTTP Request and act accoridngly 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string data = string.Empty;
            string Token = string.Empty;
            string Password = string.Empty;
            string EmpId = "0";
            string path = context.Request.RawUrl;
            int pos = path.LastIndexOf("/") + 1;
            string Method = path.Substring(pos, path.Length - pos);
            if (context.Request.Headers.AllKeys.Contains("token"))
            {
                Token = string.IsNullOrEmpty(context.Request.Headers.GetValues("Token").First()) ? null : context.Request.Headers.GetValues("Token").First();
            }
            if (context.Request.Headers.AllKeys.Contains("empid"))
            {
                EmpId = string.IsNullOrEmpty(context.Request.Headers.GetValues("EmpId").First()) ? "0" : context.Request.Headers.GetValues("EmpId").First();
            }
            if (context.Request.Headers.AllKeys.Contains("password"))
            {
                Password = string.IsNullOrEmpty(context.Request.Headers.GetValues("Password").First()) ? null : context.Request.Headers.GetValues("Password").First();
            }
            switch (Method)
            {
                ////case "login": data = login(context);
                case "login":
                    data = login(context);
                    string tempdatalogin = data;
                    if (string.Compare(data, "null", true) == 0) { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; } else { data = tempdatalogin; break; }
                case "forgotPassword":
                    data = forgotPassword(context);
                    string tempdatafpw = data;
                    if (string.Compare(data, "null", true) == 0) { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; } else { data = tempdatafpw; break; }
                case "changePassword":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = changePassword(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "dashBoard":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = dashBoard(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "availableLeavs":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = availableLeavs(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getLeaveType":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getLeaveType(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getLeaveDetails":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getLeaveDetails(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "applyLeave":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = applyLeave(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "removeLeave":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = removeLeave(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getAttanandence":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getAttanandence(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getEmpDetails":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getEmpDetails(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "insertFavouriteEmployee":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = insertFavouriteEmployee(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getFavouriteEmployees":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getFavouriteEmployees(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "removeFavouriteEmployee":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = removeFavouriteEmployee(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                #region code by Manoj Raul
                case "empleavedetails":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = empleavedetails(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "empattendance":
                    //data = empattendance(context);
                    //string tempdatalogin5 = data;
                    //if (string.Compare(data, "null", true) == 0) { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; } else { data = tempdatalogin5; break; }
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = empattendance(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "empdashboard":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = empdashboard(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getholiday":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getholiday(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getknowledgebase":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getknowledgebase(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getTimeSheetData":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getTimeSheetData(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getcontacts":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getcontacts(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getempprofile":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getempprofile(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getknowledgeattachment":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getknowledgebaseattachment(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "insertLateComing":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = insertLateComing(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                #endregion
                #region code added by Nikhil Shetye
                case "getproject":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getproject(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "gettechnology":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = gettechnology(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "editprofile":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = editprofile(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "imageupload":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = imageupload(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "saveskillmatrix":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = saveskillmatrix(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getskillmatrix":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getskillmatrix(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "addKnowledge":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = addknowledge(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getknowledgebasebyid":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getknowledgebasebyid(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "updateknowledgebase":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = updateknowledgebase(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getdocument":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getdocument(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getEmployeeData":
                    //data = getEmployeeData(context);
                    //string tempdatalogin2 = data;
                    //if (string.Compare(data, "null", true) == 0) { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; } else { data = tempdatalogin2; break; }
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getEmployeeData(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "updateMultiResultUserDetails":
                    //data = updateMultiResultUserDetails(context);
                    //string tempdatalogin3 = data;
                    //if (string.Compare(data, "null", true) == 0) { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; } else { data = tempdatalogin3; break; }
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = updateMultiResultUserDetails(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "removeMultiResultUserDetails":
                    //data = removeMultiResultUserDetails(context);
                    //string tempdatalogin4 = data;
                    //if (string.Compare(data, "null", true) == 0) { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; } else { data = tempdatalogin4; break; }
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = removeMultiResultUserDetails(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "getWFHDetails":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getWFHDetails(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "saveWFHDetails":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = saveWFHDetails(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "removeWFH":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = removeWFH(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "processAttendanceRequest":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = processAttendanceRequest(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "doGetWFHFromtoTo":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = getWFHFromtoTo(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                case "GetUserIdentitybyID":
                    if (checkToken(Convert.ToInt32(EmpId), Token)) { data = GetUserIdentitybyID(context); break; } else { data = null; context.Response.StatusCode = 403; context.Response.StatusDescription = String.Concat("Access denied."); break; }
                #endregion
                default:
                    data = null;
                    context.Response.StatusCode = 404;
                    context.Response.StatusDescription = String.Concat("Method ", Method, " not found");//"It looks like method you are trying to access does not exists or you do not have sufficient rights. Contact administrator for more details.";
                    break;
            }
            context.Response.ContentType = "text/json";
            context.Response.Write(data);
        }
        #endregion
        #region "Mobile API Public Functions"
        #endregion
        #region "Mobile API Private Functions"
        /// <summary>
        /// Method used to identify that which function is requested by HTTP request
        /// </summary>
        /// <param name="objPriContext">This is conbtext object of HTTP request</param>
        private void executeMethod(HttpContext objPriContext)
        {
        }
        /// <summary>
        /// Method used to check the valid token.
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="Token"></param>
        /// <returns>Return boolean value </returns>
        private bool checkToken(int EmpId, string Token)
        {
            if (EmpId != 0)
            {
                string dbToken = UserMaster.GetToken(EmpId);
                if (string.Compare(dbToken, Token, true) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Method used to check employeeid with header
        /// </summary>
        /// <param name="HeaderEmpId"></param>
        /// <param name="BodyEmpid"></param>
        /// <returns>Return boolean value</returns>
        private bool checkEmpIdWithHeader(int HeaderEmpId, int BodyEmpid)
        {
            if (HeaderEmpId == BodyEmpid)
                return true;
            return false;
        }
        /// <summary>
        /// Method used to call the doLogin method for validate the user
        /// </summary>
        /// <returns>Return Details of users after login method executed </returns>
        private string login(HttpContext context)
        {
            return base.doLogin(context);
        }
        /// <summary>
        /// Method used to call doForgotPassword method
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return the result .</returns>
        private string forgotPassword(HttpContext context)
        {
            return base.doForgotPassword(context);
        }
        /// <summary>
        /// Method used to call doChangePassword method
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return result</returns>
        private string changePassword(HttpContext context)
        {
            return base.doChangePassword(context);
        }
        /// <summary>
        /// Method used to call doDashBoard method for getting the holidays list
        /// </summary>
        /// <param name="context">Null</param>
        /// <returns>Holidays list</returns>
        private string dashBoard(HttpContext context)
        {
            return base.doDashboard(context);
        }
        /// <summary>
        /// Method used to call  doGetProfile method
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return user information</returns>
        /// 
        /// <summary>
        /// Method used call doAvailableLeavs method for getting the available leavs
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns available leavs </returns>
        private string availableLeavs(HttpContext context)
        {
            return base.doAvailableLeavs(context);
        }
        /// <summary>
        /// Method used call doGetLeaveType method for getting leave type
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return Leave type</returns>
        private string getLeaveType(HttpContext context)
        {
            return base.doGetLeaveType(context);
        }
        /// <summary>
        ///  Method used call dogetLeaveDetails method for getting the  leavs details .
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns details of leave  </returns>
        private string getLeaveDetails(HttpContext context)
        {
            return base.dogetLeaveDetails(context);
        }
        /// <summary>
        /// Method used call doApplyLeave method for apply leavs .
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns message </returns>
        private string applyLeave(HttpContext context)
        {
            return base.doApplyLeave(context);
        }
        /// <summary>
        /// Method used call doRemoveLeave method to delete the apply leave
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns message </returns>
        private string removeLeave(HttpContext context)
        {
            return base.doRemoveLeave(context);
        }
        /// <summary>
        /// Method used call doGetAttanandence method for Get the attandence of employee.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Teturn attandence of employee </returns>
        private string getAttanandence(HttpContext context)
        {
            return base.doGetAttanandence(context);
        }
        /// <summary>
        /// Method used call doGetEmpDetails method for Get the  employee.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return employee list</returns>
        private string getEmpDetails(HttpContext context)
        {
            return base.doGetEmpDetails(context);
        }
        /// <summary>
        /// Method used to call doInsertFavouriteEmployee method to InsertFavouriteEmployee.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message & status</returns>
        private string insertFavouriteEmployee(HttpContext context)
        {
            return base.doInsertFavouriteEmployee(context);
        }
        /// <summary>
        /// Method used to call doGetFavouriteEmployees method to get  FavouriteEmployees 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return FavouriteEmployees</returns>
        private string getFavouriteEmployees(HttpContext context)
        {
            return base.doGetFavouriteEmployees(context);
        }
        /// <summary>
        /// Method used to call doremoveFavouriteEmployee method to remove favourite employee.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message</returns>
        private string removeFavouriteEmployee(HttpContext context)
        {
            return base.doremoveFavouriteEmployee(context);
        }
        /// <summary>
        /// Method used to call empleavedetails method to get leave details of employee.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string empleavedetails(HttpContext context)
        {
            return base.doGetEmpLeaveDetails(context);
        }
        private string empattendance(HttpContext context)
        {
            return base.doGetEmpAttendance(context);
        }
        private string empdashboard(HttpContext context)
        {
            return base.doGetEmpDashboard(context);
        }
        private string getholiday(HttpContext context)
        {
            return base.doGetHoliday(context);
        }
        private string getknowledgebase(HttpContext context)
        {
            return base.doGetKnowledgeBase(context);
        }

        //Trupti Dandekar
        private string getTimeSheetData(HttpContext context)
        {
            return base.doGetTimeSheetData(context);
        }

        //Vishal w
        private string getEmployeeData(HttpContext context)
        {
            return base.doGetEmployeeData(context);
        }
        private string updateMultiResultUserDetails(HttpContext context)
        {
            return base.doUpdateMultiResultUserDetails(context);
        }

        private string removeMultiResultUserDetails(HttpContext context)
        {
            return base.doRemoveMultiResultUserDetails(context);
        }

        private string getcontacts(HttpContext context)
        {
            return base.doGetContacts(context);
        }
        private string getempprofile(HttpContext context)
        {
            return base.doGetEmpProfile(context);
        }

        private string getknowledgebaseattachment(HttpContext context)
        {
            return base.doGetKbAttachment(context);
        }


        //Added By Nikhil Shetye on 15/11/2017
        private string getproject(HttpContext context)
        {
            return base.doGetProject(context);
        }
        private string gettechnology(HttpContext context)
        {
            return base.doGetTechnology(context);
        }
        private string editprofile(HttpContext context)
        {
            return base.doEditProfile(context);
        }
        private string imageupload(HttpContext context)
        {
            return base.doImageUpload(context);
        }
        private string saveskillmatrix(HttpContext context)
        {
            return base.doSaveSkillMatrix(context);
        }
        private string getskillmatrix(HttpContext context)
        {
            return base.doGetSkillMatrix(context);
        }
        private string addknowledge(HttpContext context)
        {
            return base.doAddKnowledge(context);
        }
        private string getknowledgebasebyid(HttpContext context)
        {
            return base.doGetKnowledgeBaseById(context);
        }
        private string updateknowledgebase(HttpContext context)
        {
            return base.doUpdateKnowledgebase(context);
        }
        private string getdocument(HttpContext context)
        {
            return base.doGetDocument(context);
        }
        private string insertLateComing(HttpContext context)
        {
            return base.doInsertLateComing(context);
        }
        //END Nikhil Shetye

        private string getWFHDetails(HttpContext context)
        {
            return base.doGetWFHDetails(context);
        }
        private string saveWFHDetails(HttpContext context)
        {
            return base.doSaveWFHDetails(context);
        }
        private string removeWFH(HttpContext context)
        {
            return base.doRemoveWFH(context);
        }
        private string processAttendanceRequest(HttpContext context)
        {
            return base.doProcessAttendanceRequest(context);
        }
        private string updateAttendance(HttpContext context)
        {
            return base.doUpdateAttendance(context);
        }
        private string getWFHFromtoTo(HttpContext context)
        {
            return base.doGetWFHFromtoTo(context);
        }
        private string GetUserIdentitybyID(HttpContext context)
        {
            return base.GetUserIdentity(context);
        }
        #endregion
    }
    /// <summary>
    /// This class is use for calling the core class method for processing.
    /// </summary>
    public class clsMobileProcessing
    {
        /// <summary>
        /// Method used to validate the login credentials. This is just wrapper calling core "DeviceLogin" method
        /// </summary>
        /// <param name="context">context </param>
        /// <returns>Return Details of users after login method executed </returns>     
        public string doLogin(HttpContext context)
        {
            UserMaster objUserMaster = new UserMaster();
            MobileUserMaster objMobileUserMaster = new MobileUserMaster();
            try
            {
                string EmpID = string.Empty;
                string Password = string.Empty;
                string Token = string.Empty;
                string DeviceId = string.Empty;
                string OsType = string.Empty;
                string strBase64 = string.Empty;
                string FullPath = string.Empty;
                int Logintype = (int)AgoraBL.Models.LoginType.Agora;

                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToString(sData["EmpId"]);
                    DeviceId = Convert.ToString(sData["DeviceId"]);
                    OsType = Convert.ToString(sData["OsType"]);
                    Password = CSCode.Global.CreatePassword(Convert.ToString(sData["Password"]));

                    //Logintype = string.IsNullOrEmpty(sData["Logintype"]) ? (int)AgoraBL.Models.LoginType.Agora : Convert.ToInt32(sData["Logintype"]);
                    if (sData.ContainsKey("Logintype") && !string.IsNullOrEmpty(sData["Logintype"]))
                    {
                        Logintype = Convert.ToInt32(sData["Logintype"]);
                    }
                    else
                    {
                        Logintype = (int)AgoraBL.Models.LoginType.Agora; // Set to Agora if not provided
                    }

                    // Call the common method for employee login
                    var loginResult = EmployeeMasterBAL.EmployeeLogin(EmpID, Password, (AgoraBL.Models.LoginType)Logintype);
                    UserMaster ObjUser = new UserMaster
                    {
                        EmployeeID = Convert.ToInt32(loginResult.EmpId),
                        IsActive = loginResult.IsActive,
                        Message = loginResult.Message,
                        ProfileID = loginResult.ProfileID,
                        onbaordingCompleted = loginResult.onbaordingCompleted,
                        status = loginResult.status,
                        IsAdmin = loginResult.IsAdmin,
                        IsModuleAdmin = loginResult.IsModuleAdmin,
                        SkillID = loginResult.SkillID,
                        Name = loginResult.Name,
                        EmailID = loginResult.EmailID,
                        Address = loginResult.Address,
                        Contact = loginResult.Contact,
                        JoiningDate = loginResult.JoiningDate,
                        LeavingDate = loginResult.LeavingDate,
                        ProbationPeriod = loginResult.ProbationPeriod,
                        Notes = loginResult.Notes,
                        AccountNo = loginResult.AccountNo,
                        BDate = loginResult.BDate,
                        ADate = loginResult.ADate,
                        PreviousEmployer = loginResult.PreviousEmployer,
                        Experince = loginResult.Experince,
                        LocationID = loginResult.LocationID,
                        ProfileLocationID = loginResult.ProfileLocationID,
                        UserType = loginResult.UserType,
                        CurrentAddress = loginResult.CurrentAddress,
                        empConfDate = loginResult.empConfDate,
                        Designation = loginResult.Designation,
                        empForgotpwdLinkDate = loginResult.empForgotpwdLinkDate,
                        empPassword = loginResult.empPassword

                    };

                    if (ObjUser != null)
                    {
                        EmpID = ObjUser.EmployeeID.ToString();
                        FullPath = ConfigurationManager.AppSettings["DataBank"] + "mobileprofileimage\\" + EmpID + ".jpg";
                        strBase64 = doGetBase64ofFile(FullPath, ObjUser.EmployeeID);

                        if (ObjUser.IsActive)
                        {
                            Token = UserMaster.UpdateToken(CSCode.Global.CreateLoginToken(Convert.ToString(Guid.NewGuid()), ObjUser.EmployeeID, ObjUser.JoiningDate), DeviceId, OsType, ObjUser.EmployeeID);

                            // Populate the MobileUserMaster object
                            objMobileUserMaster.EmployeeID = ObjUser.EmployeeID;
                            objMobileUserMaster.IsAdmin = ObjUser.IsAdmin;
                            objMobileUserMaster.IsModuleAdmin = ObjUser.IsModuleAdmin;
                            objMobileUserMaster.SkillID = ObjUser.SkillID;
                            objMobileUserMaster.Name = ObjUser.Name;
                            objMobileUserMaster.EmailID = ObjUser.EmailID;
                            objMobileUserMaster.Address = ObjUser.Address;
                            objMobileUserMaster.Contact = ObjUser.Contact;
                            objMobileUserMaster.JoiningDate = ObjUser.JoiningDate;
                            objMobileUserMaster.LeavingDate = ObjUser.LeavingDate;
                            objMobileUserMaster.ProbationPeriod = ObjUser.ProbationPeriod;
                            objMobileUserMaster.Notes = ObjUser.Notes;
                            objMobileUserMaster.BDate = ObjUser.BDate;
                            objMobileUserMaster.ADate = ObjUser.ADate;
                            objMobileUserMaster.PreviousEmployer = ObjUser.PreviousEmployer;
                            objMobileUserMaster.Experince = ObjUser.Experince;
                            objMobileUserMaster.LocationID = ObjUser.LocationID;
                            objMobileUserMaster.ProfileID = ObjUser.ProfileID;
                            objMobileUserMaster.ProfileLocationID = ObjUser.ProfileLocationID;
                            objMobileUserMaster.IsActive = ObjUser.IsActive;
                            objMobileUserMaster.Message = ObjUser.Message;
                            objMobileUserMaster.UserType = ObjUser.UserType;
                            objMobileUserMaster.CurrentAddress = ObjUser.CurrentAddress;
                            objMobileUserMaster.Token = Token;
                            objMobileUserMaster.Designation = ObjUser.Designation;
                            objMobileUserMaster.empConfDate = ObjUser.empConfDate;
                            objMobileUserMaster.UploadedImage = strBase64;
                        }
                        else
                        {
                            objMobileUserMaster.Message = objUserMaster.Message;
                            context.Response.StatusCode = 401;
                            context.Response.StatusDescription = "Unauthorized.";
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        context.Response.StatusDescription = "Unauthorized.";
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                    objMobileUserMaster = null;
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error.";
            }
            return (new JavaScriptSerializer().Serialize(objMobileUserMaster));
        }
        /// <summary>
        /// This method is used for just wrapper calling  "SendMail"  method .
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return Message</returns>
        public string doForgotPassword(HttpContext context)
        {
            ResponseData objResponseData = new ResponseData();
            string Empid = "0";
            string EmailId = string.Empty;
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    UserMaster UserMaster = new UserMaster();
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    Empid = Convert.ToString(sData["EmpId"]);
                    EmailId = Convert.ToString(sData["EmailId"]);
                    UserMaster = UserMaster.SendMailForForgotPassword(Empid, EmailId);
                    objResponseData.Status = (UserMaster.status) ? "1" : "0";
                    objResponseData.Message = UserMaster.Message;
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                    objResponseData = null;
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// This method is used for just wrapper calling  SendMail  method .
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return Message</returns>
        public string doChangePassword(HttpContext context)
        {
            string OldPassword = string.Empty;
            string NewPassword = string.Empty;
            string ConfirmPassword = string.Empty;
            ResponseData objResponseData = new ResponseData();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            int Empid = 0;
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    UserMaster UserMaster = new UserMaster();
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    Empid = Convert.ToInt16(sData["EmpId"]);
                    OldPassword = Convert.ToString(sData["OldPassword"]);
                    NewPassword = Convert.ToString(sData["NewPassword"]);
                    ConfirmPassword = Convert.ToString(sData["ConfirmPassword"]);
                    UserMaster = UserMaster.changePassword(Convert.ToInt32(Empid), CSCode.Global.CreatePassword(OldPassword), CSCode.Global.CreatePassword(NewPassword));
                    objResponseData.Status = (UserMaster.status) ? "1" : "0";
                    objResponseData.Message = UserMaster.Message;

                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// This method is used for warapper calling for geting the Holidays list
        /// </summary>
        /// <param name="context">Null</param>
        /// <returns>Return holidays list</returns>
        public string doDashboard(HttpContext context)
        {
            List<Holiday> lstHoliday = new List<Holiday>();
            try
            {
                lstHoliday = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";

            }
            return (new JavaScriptSerializer().Serialize(lstHoliday));
        }
        /// <summary>
        /// Method used to get the available leaves of employee. This is just wrapper calling core GetLeave method
        /// </summary>
        /// <param name="context"></param>
        /// <returns>return available leaves </returns>
        public string doAvailableLeavs(HttpContext context)
        {
            int EmpID = 0;

            EmpLeaveBLL objEmpLeaveBLL = new EmpLeaveBLL();
            DataSet DsEmpLeaveDetails = null;
            List<EmpLeaveBLL> lstEmpleave = new List<EmpLeaveBLL>();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    DsEmpLeaveDetails = objEmpLeaveBLL.GetLeave(EmpID);
                    if (DsEmpLeaveDetails != null && DsEmpLeaveDetails.Tables[1] != null && DsEmpLeaveDetails.Tables[1].Rows.Count > 0)
                    {
                        objEmpLeaveBLL.TotalCL = DsEmpLeaveDetails.Tables[1].Rows[0]["Total"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[0]["Total"]);
                        objEmpLeaveBLL.TotalSL = DsEmpLeaveDetails.Tables[1].Rows[1]["Total"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[1]["Total"]);
                        objEmpLeaveBLL.TotalPL = DsEmpLeaveDetails.Tables[1].Rows[2]["Total"] == DBNull.Value ? 0 : Convert.ToInt32(DsEmpLeaveDetails.Tables[1].Rows[2]["Total"]);
                        objEmpLeaveBLL.TotalCO = DsEmpLeaveDetails.Tables[1].Rows[3]["Total"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[3]["Total"]);

                        objEmpLeaveBLL.ConsumedCL = DsEmpLeaveDetails.Tables[1].Rows[0]["Consumed"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[0]["Consumed"]);
                        objEmpLeaveBLL.ConsumedSL = DsEmpLeaveDetails.Tables[1].Rows[1]["Consumed"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[1]["Consumed"]);
                        objEmpLeaveBLL.ConsumedPL = DsEmpLeaveDetails.Tables[1].Rows[2]["Consumed"] == null ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[2]["Consumed"]);
                        objEmpLeaveBLL.ConsumedCO = DsEmpLeaveDetails.Tables[1].Rows[3]["Consumed"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[3]["Consumed"]);


                        objEmpLeaveBLL.BalanceCL = DsEmpLeaveDetails.Tables[1].Rows[0]["Balance"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[0]["Balance"]);
                        objEmpLeaveBLL.BalanceSL = DsEmpLeaveDetails.Tables[1].Rows[1]["Balance"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[1]["Balance"]);
                        objEmpLeaveBLL.BalancePL = DsEmpLeaveDetails.Tables[1].Rows[2]["Balance"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[2]["Balance"]);
                        objEmpLeaveBLL.BalanceCO = DsEmpLeaveDetails.Tables[1].Rows[3]["Balance"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[3]["Balance"]);

                        objEmpLeaveBLL.TotalCLTillDate = DsEmpLeaveDetails.Tables[1].Rows[0]["Total_Accrual"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[0]["Total_Accrual"]);
                        objEmpLeaveBLL.TotalSLTillDate = DsEmpLeaveDetails.Tables[1].Rows[1]["Total_Accrual"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[1]["Total_Accrual"]);
                        objEmpLeaveBLL.TotalPLTillDate = DsEmpLeaveDetails.Tables[1].Rows[2]["Total_Accrual"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[2]["Total_Accrual"]);
                        objEmpLeaveBLL.TotalCOTillDate = DsEmpLeaveDetails.Tables[1].Rows[3]["Total_Accrual"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveDetails.Tables[1].Rows[3]["Total_Accrual"]);
                        lstEmpleave.Add(objEmpLeaveBLL);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(lstEmpleave));
        }
        /// <summary>
        /// Method used to get the available leave type from core method
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return </returns>
        public string doGetLeaveType(HttpContext context)
        {

            int EmpID = 0;

            EmpLeaveBLL objEmpLeaveBLL = new EmpLeaveBLL();
            DataSet DsEmpLeaveType = null;
            List<EmpLeaveBLL> lstEmpleaveType = new List<EmpLeaveBLL>();

            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    DsEmpLeaveType = objEmpLeaveBLL.GetLeave(EmpID);
                    if (DsEmpLeaveType != null && DsEmpLeaveType.Tables[2] != null && DsEmpLeaveType.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < DsEmpLeaveType.Tables[2].Rows.Count; i++)
                        {
                            EmpLeaveBLL objEmpLeave = new EmpLeaveBLL();
                            objEmpLeave.LeaveType = DsEmpLeaveType.Tables[2].Rows[i]["statusid"].ToString();
                            objEmpLeave.LeaveDesc = DsEmpLeaveType.Tables[2].Rows[i]["statusDesc"].ToString();
                            lstEmpleaveType.Add(objEmpLeave);
                        }

                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(lstEmpleaveType));

        }
        /// <summary>
        /// Method used to get the leave details calling by core merhod GetLeaveDetails by employee id.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>returns leave details of  employee</returns>
        public string dogetLeaveDetails(HttpContext context)
        {
            int EmpID = 0;
            string LeaveType = string.Empty;
            string LeaveStatus = string.Empty;
            //int Year = 0;
            int Year = (DateTime.Now.Month) > 3 ? DateTime.Now.Year : (DateTime.Now.Year - 1);
            EmpLeaveBLL objEmpLeaveBLL = new EmpLeaveBLL();
            DataSet DsEmpLeaveDetails = null;
            List<EmpLeaveBLL> lstLeaveDetails = new List<EmpLeaveBLL>();
            EmployeeLeaveDetails objemployeeLeaveDetails = new EmployeeLeaveDetails();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    LeaveType = Convert.ToString(sData["LeaveType"]);
                    LeaveStatus = Convert.ToString(sData["LeaveStatus"]);
                    if (sData.ContainsKey("Year"))
                    {
                        if (!string.IsNullOrEmpty(sData["Year"]))
                        {
                            if (Convert.ToInt32(sData["Year"]) > 0)
                            {
                                Year = Convert.ToInt32(sData["Year"]);
                            }
                        }
                    }

                    DsEmpLeaveDetails = objEmpLeaveBLL.GetLeaveDetails(EmpID, LeaveType, LeaveStatus, Year);
                    if (DsEmpLeaveDetails != null && DsEmpLeaveDetails.Tables[0] != null && DsEmpLeaveDetails.Tables[0].Rows.Count > 0)
                    {
                        if (DsEmpLeaveDetails.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in DsEmpLeaveDetails.Tables[0].Rows)
                            {
                                EmpLeaveBLL objEmpLeave = new EmpLeaveBLL();
                                objEmpLeave.ID = Convert.ToInt32(dr["ID"]);
                                objEmpLeave.LeaveFrom = Convert.ToString(dr["leaveFrom"]);
                                objEmpLeave.LeaveTo = Convert.ToString(dr["leaveTo"]);
                                objEmpLeave.LeaveDesc = Convert.ToString(dr["leaveDesc"]);
                                objEmpLeave.LeaveStatus = Convert.ToString(dr["leaveStatus"]);
                                objEmpLeave.LeaveType = Convert.ToString(dr["statusDesc"]);
                                objEmpLeave.AdminComments = Convert.ToString(dr["AdComments"]);
                                lstLeaveDetails.Add(objEmpLeave);
                            }
                        }

                    }
                    objemployeeLeaveDetails.FiscalYear = Year;
                    objemployeeLeaveDetails.LeaveData = lstLeaveDetails;

                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";

                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objemployeeLeaveDetails));

        }
        /// <summary>
        /// Method used to save the leaves apply by employee calling by core method  "SaveLeave".
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns message</returns>
        public string doApplyLeave(HttpContext context)
        {
            ResponseData objResponseData = new ResponseData();
            //EmployeeMasterBAL

            EmployeeMasterBAL ObjEmpLeaveBLL = new EmployeeMasterBAL();

            //EmpLeaveBLL ObjEmpLeaveBLL = new EmpLeaveBLL();
            DataSet dsEmpLeave = new DataSet();
            int EmpID = 0;
            string LeaveFrom = string.Empty;
            string LeaveTo = string.Empty;
            string Reason = string.Empty;
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            bool output = false;
            EmployeeMasterBAL objEmployeeMasterBAL = new EmployeeMasterBAL();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    string LeaveType = Convert.ToString(sData["LeaveType"]);
                    LeaveFrom = Convert.ToString(sData["LeaveFrom"]);
                    LeaveTo = Convert.ToString(sData["LeaveTo"]);
                    Reason = Convert.ToString(sData["Reason"]);
                    output = ObjEmpLeaveBLL.IfExistsLeave("IFEXISTSLEAVE", EmpID, LeaveFrom, LeaveTo);
                    if (output)
                    {
                        objResponseData.Status = "0";
                        objResponseData.Message = "Leave for same date already exists.";
                        context.Response.StatusDescription = "Leave for same date already exists.";
                    }
                    else
                    {
                        bool IsEmailSent = false;
                        dsEmpLeave = ObjEmpLeaveBLL.SaveLeave(EmpID, LeaveType, LeaveFrom, LeaveTo, Reason, EmpName: string.Empty, IsEmailSent: out IsEmailSent);
                        if (dsEmpLeave != null && dsEmpLeave.Tables[0].Rows.Count > 0)
                        {
                            //objEmployeeMasterBAL.SendLeaveNotificationToHrThroughAI(EmpID, LeaveFrom, LeaveType, Reason, LeaveTo, dsEmpLeave,string.Empty);
                            if (!IsEmailSent)
                            {
                                ObjEmpLeaveBLL.SendMail(dsEmpLeave.Tables[0], LeaveFrom, LeaveTo, Reason, out IsEmailSent);
                            }
                            objResponseData.Status = "1";
                            objResponseData.Message = "Leave saved succesfully.";
                        }
                        else
                        {
                            objResponseData.Status = "0";
                            objResponseData.Message = "Leave save failed.";
                            context.Response.StatusCode = 400;
                            context.Response.StatusDescription = "Bad Request.";
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error.";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// Method used to delete Leave
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message </returns>
        public string doRemoveLeave(HttpContext context)
        {
            ResponseData objResponseData = new ResponseData();
            //EmpLeaveBLL objEmpLeave = new EmpLeaveBLL();
            EmployeeMasterBAL objEmpLeave = new EmployeeMasterBAL();

            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            int EmpLeaveId = 0;

            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpLeaveId = Convert.ToInt32(sData["EmpLeaveId"]);
                    int outId = objEmpLeave.DeleteLeave(EmpLeaveId);
                    if (outId > 0)
                    {
                        objResponseData.Status = "1";
                        objResponseData.Message = "Leave deleted succesfully.";
                    }
                    else
                    {
                        objResponseData.Status = "0";
                        objResponseData.Message = "Leave delete failed.";
                        context.Response.StatusCode = 404; //Resource does not exist
                        context.Response.StatusDescription = "Not found.";
                    }

                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// Method used to get the attandence of the  employee calling by core method  "GetCalendarAtt"
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return attandence of employee </returns>      
        public string doGetAttanandence(HttpContext context)
        {
            List<EmpAttLog> lstAttendence = new List<EmpAttLog>();
            int EmpID = 0;
            string Startdate = string.Empty;
            string EndDate = string.Empty;
            TimeSpan t1 = new TimeSpan(10, 15, 0);
            TimeSpan t2;
            string time;
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    ////[08/01/2016](Format)
                    Startdate = Convert.ToString(sData["Startdate"]);
                    //// [08/30/2016](Format)
                    EndDate = Convert.ToString(sData["EndDate"]);
                    lstAttendence = EmpAttLog.GetCalendarAtt("GetCalendarAtt", EmpID, Startdate, EndDate);

                    lstAttendence = lstAttendence.Select(x => new EmpAttLog()
                    {
                        attDate = x.attDate,
                        //attendanceDate = Convert.ToString(x.attDate) +"-"+ x.attDate.DayOfWeek,
                        attendanceDate = String.Format("{0:dd/MM/yyyy}", x.attDate) + "-" + x.attDate.DayOfWeek,
                        attInTime = x.attInTime,
                        attOutTime = x.attOutTime,
                        attStatus = x.attStatus,
                        EmpID = x.EmpID,
                        timesheethours = x.timesheethours,
                        workinghours = x.workinghours,
                        description = computeDescription(x.timesheethours, x.attDate)
                    }).ToList();
                    //Adde Code by Nikhil Shetye for HD status
                    if (lstAttendence != null)
                    {
                        if (lstAttendence.Count > 0)
                        {
                            for (int i = 0; i < lstAttendence.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(lstAttendence[i].attInTime))
                                {
                                    time = Convert.ToDateTime(lstAttendence[i].attInTime).ToString("HH:mm");
                                    if (!string.IsNullOrEmpty(time))
                                    {
                                        t2 = new TimeSpan(Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), 0);
                                        int result = TimeSpan.Compare(t1, t2);
                                        if (result < 0)
                                        {
                                            lstAttendence[i].attStatus = "HD";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(lstAttendence));
        }

        /// <summary>
        /// Method used to get the the employee list calling by core method "GetEmpDetails"
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return employee list</returns>
        public string doGetEmpDetails(HttpContext context)
        {
            string EmpName = "";
            int EmpID = 0;
            List<EmployeeMaster> lstEmployeeMaster = new List<EmployeeMaster>();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {

                    EmployeeMaster clsBoard = new EmployeeMaster();
                    clsBoard.empName = "Board Number";
                    clsBoard.empid = 0;
                    clsBoard.empEmail = "contactus@intelgain.com";
                    clsBoard.empContact = "91 22 41516100";
                    clsBoard.Flag = "Generic";
                    lstEmployeeMaster.Add(clsBoard);
                    EmployeeMaster clsHR = new EmployeeMaster();
                    clsHR.empName = "HR";
                    clsHR.empid = 0;
                    clsHR.empEmail = "hr@intelgain.com";
                    clsHR.empContact = "91 22 41516102";
                    clsHR.Flag = "Generic";
                    lstEmployeeMaster.Add(clsHR);
                    EmployeeMaster clsAccounts = new EmployeeMaster();
                    clsAccounts.empName = "Accounts";
                    clsAccounts.empid = 0;
                    clsAccounts.empEmail = "accounts@intelgain.com";
                    clsAccounts.empContact = "91 22 41516106";
                    clsAccounts.Flag = "Generic";
                    lstEmployeeMaster.Add(clsAccounts);
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    ////  EmpName = Convert.ToString(sData["EmpName"]);
                    DataTable dttEmpDetails = EmployeeMaster.GetEmpDetails("GetEmployeeList", EmpName, EmpID);
                    if (dttEmpDetails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dttEmpDetails.Rows)
                        {
                            EmployeeMaster clsEmployeeMaster = new EmployeeMaster();
                            clsEmployeeMaster.empName = Convert.ToString(dr["empName"]);
                            clsEmployeeMaster.empid = Convert.ToInt32(dr["empid"]);
                            clsEmployeeMaster.empEmail = Convert.ToString(dr["empEmail"]);
                            clsEmployeeMaster.empContact = Convert.ToString(dr["empContact"]);
                            clsEmployeeMaster.Flag = Convert.ToString(dr["Flag"]);
                            clsEmployeeMaster.strBirthday = Convert.ToString(dr["empBDate"]);

                            lstEmployeeMaster.Add(clsEmployeeMaster);
                        }
                    }
                    else
                    {
                        EmployeeMaster clsEmployeeMaster = new EmployeeMaster();
                        lstEmployeeMaster.Add(clsEmployeeMaster);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(lstEmployeeMaster));
        }
        /// <summary>
        /// Method used to Insert favouriteEmployee 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message</returns>
        public string doInsertFavouriteEmployee(HttpContext context)
        {
            ResponseData objResponseData = new ResponseData();
            EmployeeMaster objEmployeeMaster = new EmployeeMaster();
            DataTable dtFavouriteEmp = new DataTable();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            int EmpID = 0;
            int FavouriteEmpId = 0;
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    FavouriteEmpId = Convert.ToInt32(sData["FavouriteEmpId"]);
                    dtFavouriteEmp = EmployeeMaster.InsertFavouriteEmployee("InsertFavouriteEmp", EmpID, FavouriteEmpId);
                    if (dtFavouriteEmp.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtFavouriteEmp.Rows)
                        {
                            objResponseData.Status = Convert.ToString(dr["Status"]);
                            objResponseData.Message = Convert.ToString(dr["Message"]);
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// Method used to get the favourite Employee
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return favourite employees</returns>
        public string doGetFavouriteEmployees(HttpContext context)
        {
            int EmpID = 0;
            List<EmployeeMaster> lstEmployeeMaster = new List<EmployeeMaster>();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {

                    EmployeeMaster clsBoard = new EmployeeMaster();
                    clsBoard.empName = "Board Number";
                    clsBoard.empid = 0;
                    clsBoard.empEmail = "contactus@intelgain.com";
                    clsBoard.empContact = "91 22 41516100";
                    clsBoard.Flag = "Generic";
                    lstEmployeeMaster.Add(clsBoard);
                    EmployeeMaster clsHR = new EmployeeMaster();
                    clsHR.empName = "HR";
                    clsHR.empid = 0;
                    clsHR.empEmail = "hr@intelgain.com";
                    clsHR.empContact = "91 22 41516102";
                    clsHR.Flag = "Generic";
                    lstEmployeeMaster.Add(clsHR);
                    EmployeeMaster clsAccounts = new EmployeeMaster();
                    clsAccounts.empName = "Accounts";
                    clsAccounts.empid = 0;
                    clsAccounts.empEmail = "accounts@intelgain.com";
                    clsAccounts.empContact = "91 22 41516106";
                    clsAccounts.Flag = "Generic";
                    lstEmployeeMaster.Add(clsAccounts);
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);

                    DataTable dtEmpDetails = EmployeeMaster.GetFavouriteEmployees("GetFavouriteEmp", EmpID);
                    if (dtEmpDetails != null && dtEmpDetails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtEmpDetails.Rows)
                        {
                            EmployeeMaster clsEmployeeMaster = new EmployeeMaster();
                            clsEmployeeMaster.empName = Convert.ToString(dr["FavouriteEmpName"]);
                            clsEmployeeMaster.empid = Convert.ToInt32(dr["FavouriteEmpId"]);
                            clsEmployeeMaster.empEmail = Convert.ToString(dr["FavouriteEmpEmail"]);
                            clsEmployeeMaster.empContact = Convert.ToString(dr["FavouriteEmpContact"]);
                            clsEmployeeMaster.Flag = Convert.ToString(dr["Flag"]);
                            lstEmployeeMaster.Add(clsEmployeeMaster);
                        }

                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            return (new JavaScriptSerializer().Serialize(lstEmployeeMaster));
        }
        /// <summary>
        /// Method used to delete favourite employees 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message</returns>
        public string doremoveFavouriteEmployee(HttpContext context)
        {
            ResponseData objResponseData = new ResponseData();
            EmployeeMaster objEmployeeMaster = new EmployeeMaster();
            DataTable dtFavouriteEmp = new DataTable();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            int EmpID = 0;
            int FavouriteEmpId = 0;
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    FavouriteEmpId = Convert.ToInt32(sData["FavouriteEmpId"]);
                    dtFavouriteEmp = EmployeeMaster.DeleteFavouriteEmployee("DeleteFavouriteEmp", EmpID, FavouriteEmpId);
                    if (dtFavouriteEmp != null && dtFavouriteEmp.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtFavouriteEmp.Rows)
                        {
                            objResponseData.Status = Convert.ToString(dr["Status"]);
                            objResponseData.Message = Convert.ToString(dr["Message"]);
                        }
                    }

                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// Method used to get Leave details of employee
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string doGetEmpLeaveDetails(HttpContext context)
        {
            int EmpID = 0;
            string LeaveType = string.Empty;
            string LeaveStatus = string.Empty;
            //int Year = 0;
            int Year = (DateTime.Now.Month) > 3 ? DateTime.Now.Year : (DateTime.Now.Year - 1);

            EmpLeaveBLL objEmpLeaveBLL = new EmpLeaveBLL();
            DataSet DsEmpLeaveType = null;
            DataSet DsEmpLeaveDetails = null;
            List<EmpLeaveBLL> lstEmpleave = new List<EmpLeaveBLL>();
            List<EmpLeaveBLL> lstLeaveDetails = new List<EmpLeaveBLL>();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    LeaveType = Convert.ToString(sData["LeaveType"]);
                    LeaveStatus = Convert.ToString(sData["LeaveStatus"]);
                    if (sData.ContainsKey("Year"))
                    {
                        if (!string.IsNullOrEmpty(sData["Year"]))
                        {
                            if (Convert.ToInt32(sData["Year"]) > 0)
                            {
                                Year = Convert.ToInt32(sData["Year"]);
                            }
                        }
                    }

                    DsEmpLeaveType = objEmpLeaveBLL.GetLeave(EmpID);
                    if (DsEmpLeaveType != null && DsEmpLeaveType.Tables[1] != null && DsEmpLeaveType.Tables[1].Rows.Count > 0)
                    {
                        objEmpLeaveBLL.TotalCL = DsEmpLeaveType.Tables[1].Rows[0]["Total"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[0]["Total"]);
                        objEmpLeaveBLL.TotalSL = DsEmpLeaveType.Tables[1].Rows[1]["Total"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[1]["Total"]);
                        objEmpLeaveBLL.TotalPL = DsEmpLeaveType.Tables[1].Rows[2]["Total"] == DBNull.Value ? 0 : Convert.ToInt32(DsEmpLeaveType.Tables[1].Rows[2]["Total"]);
                        objEmpLeaveBLL.TotalCO = DsEmpLeaveType.Tables[1].Rows[3]["Total"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[3]["Total"]);

                        objEmpLeaveBLL.ConsumedCL = DsEmpLeaveType.Tables[1].Rows[0]["Consumed"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[0]["Consumed"]);
                        objEmpLeaveBLL.ConsumedSL = DsEmpLeaveType.Tables[1].Rows[1]["Consumed"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[1]["Consumed"]);
                        objEmpLeaveBLL.ConsumedPL = DsEmpLeaveType.Tables[1].Rows[2]["Consumed"] == null ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[2]["Consumed"]);
                        objEmpLeaveBLL.ConsumedCO = DsEmpLeaveType.Tables[1].Rows[3]["Consumed"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[3]["Consumed"]);


                        objEmpLeaveBLL.BalanceCL = DsEmpLeaveType.Tables[1].Rows[0]["Balance"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[0]["Balance"]);
                        objEmpLeaveBLL.BalanceSL = DsEmpLeaveType.Tables[1].Rows[1]["Balance"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[1]["Balance"]);
                        objEmpLeaveBLL.BalancePL = DsEmpLeaveType.Tables[1].Rows[2]["Balance"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[2]["Balance"]);
                        objEmpLeaveBLL.BalanceCO = DsEmpLeaveType.Tables[1].Rows[3]["Balance"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[3]["Balance"]);

                        objEmpLeaveBLL.TotalCLTillDate = DsEmpLeaveType.Tables[1].Rows[0]["Total_Accrual"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[0]["Total_Accrual"]);
                        objEmpLeaveBLL.TotalSLTillDate = DsEmpLeaveType.Tables[1].Rows[1]["Total_Accrual"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[1]["Total_Accrual"]);
                        objEmpLeaveBLL.TotalPLTillDate = DsEmpLeaveType.Tables[1].Rows[2]["Total_Accrual"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[2]["Total_Accrual"]);
                        objEmpLeaveBLL.TotalCOTillDate = DsEmpLeaveType.Tables[1].Rows[3]["Total_Accrual"] == DBNull.Value ? 0 : Convert.ToInt16(DsEmpLeaveType.Tables[1].Rows[3]["Total_Accrual"]);
                        lstEmpleave.Add(objEmpLeaveBLL);
                    }


                    DsEmpLeaveDetails = objEmpLeaveBLL.GetLeaveDetails(EmpID, LeaveType, LeaveStatus, Year);
                    if (DsEmpLeaveDetails != null && DsEmpLeaveDetails.Tables[0] != null && DsEmpLeaveDetails.Tables[0].Rows.Count > 0)
                    {
                        if (DsEmpLeaveDetails.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in DsEmpLeaveDetails.Tables[0].Rows)
                            {
                                EmpLeaveBLL objEmpLeave = new EmpLeaveBLL();
                                objEmpLeave.ID = Convert.ToInt32(dr["ID"]);
                                objEmpLeave.LeaveFrom = Convert.ToString(dr["leaveFrom"]);
                                objEmpLeave.LeaveTo = Convert.ToString(dr["leaveTo"]);
                                objEmpLeave.LeaveDesc = Convert.ToString(dr["leaveDesc"]);
                                objEmpLeave.LeaveStatus = Convert.ToString(dr["leaveStatus"]);
                                objEmpLeave.LeaveType = Convert.ToString(dr["statusDesc"]);
                                objEmpLeave.AdminComments = Convert.ToString(dr["AdComments"]);
                                lstLeaveDetails.Add(objEmpLeave);
                            }
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            EmpLeaveDetails EmpLeaveDetails = new EmpLeaveDetails();
            LeaveType objLeaveType = new LeaveType();
            #region fill Leave Type Object
            if (lstEmpleave != null && lstEmpleave.Count > 0)
            {
                objLeaveType.ID = lstEmpleave[0].ID;
                objLeaveType.TotalCL = lstEmpleave[0].TotalCL;
                objLeaveType.TotalSL = lstEmpleave[0].TotalSL;
                objLeaveType.TotalPL = lstEmpleave[0].TotalPL;
                objLeaveType.TotalCO = lstEmpleave[0].TotalCO;
                objLeaveType.ConsumedCL = lstEmpleave[0].ConsumedCL;
                objLeaveType.ConsumedSL = lstEmpleave[0].ConsumedSL;
                objLeaveType.ConsumedPL = lstEmpleave[0].ConsumedPL;
                objLeaveType.ConsumedCO = lstEmpleave[0].ConsumedCO;
                objLeaveType.BalanceCL = lstEmpleave[0].BalanceCL;
                objLeaveType.BalanceSL = lstEmpleave[0].BalanceSL;
                objLeaveType.BalancePL = lstEmpleave[0].BalancePL;
                objLeaveType.BalanceCO = lstEmpleave[0].BalanceCO;
                objLeaveType.TotalCLTillDate = lstEmpleave[0].TotalCLTillDate;
                objLeaveType.TotalSLTillDate = lstEmpleave[0].TotalSLTillDate;
                objLeaveType.TotalPLTillDate = lstEmpleave[0].TotalPLTillDate;
                objLeaveType.TotalCOTillDate = lstEmpleave[0].TotalCOTillDate;
            }
            #endregion
            List<LeaveDetails> objListLeaveDetails = new List<LeaveDetails>();
            #region fill Leave details object
            if (lstLeaveDetails != null && lstLeaveDetails.Count > 0)
            {
                foreach (EmpLeaveBLL objItem in lstLeaveDetails)
                {
                    LeaveDetails objLeaveDetails = new LeaveDetails();
                    objLeaveDetails.ID = objItem.ID;
                    objLeaveDetails.LeaveType = objItem.LeaveType;
                    objLeaveDetails.LeaveFrom = objItem.LeaveFrom;
                    objLeaveDetails.LeaveTo = objItem.LeaveTo;
                    objLeaveDetails.LeaveDesc = objItem.LeaveDesc;
                    objLeaveDetails.LeaveStatus = objItem.LeaveStatus;
                    objLeaveDetails.AdminComments = objItem.AdminComments;
                    //objLeaveDetails.LeaveReason = objItem.LeaveReason;
                    objListLeaveDetails.Add(objLeaveDetails);
                }
            }
            #endregion
            EmpLeaveDetails.FiscalYear = Year;
            EmpLeaveDetails.LeaveType = objLeaveType;
            EmpLeaveDetails.LeaveData = objListLeaveDetails;
            return (new JavaScriptSerializer().Serialize(EmpLeaveDetails));
        }
        /// <summary>
        /// Method used to get attendance details of employee
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string doGetEmpAttendance(HttpContext context)
        {
            List<EmpAttLog> lstAttendence = new List<EmpAttLog>();
            int EmpID = 0;
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            string Startdate = startDate.ToString();
            string EndDate = endDate.ToString();
            string time;
            TimeSpan t1 = new TimeSpan(10, 15, 00);
            TimeSpan t2;
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    ////[08/01/2016](Format)
                    if (sData.ContainsKey("Startdate") && sData["Startdate"] != null && !string.IsNullOrEmpty(sData["Startdate"]))
                    {
                        Startdate = Convert.ToString(sData["Startdate"]);
                    }
                    //// [08/30/2016](Format)
                    if (sData.ContainsKey("EndDate") && sData["EndDate"] != null && !string.IsNullOrEmpty(sData["EndDate"]))
                    {
                        EndDate = Convert.ToString(sData["EndDate"]);
                    }
                    lstAttendence = EmpAttLog.GetCalendarAtt("GetCalendarAtt", EmpID, Startdate, EndDate);
                    lstAttendence = lstAttendence.Select(x => new EmpAttLog()
                    {
                        attDate = x.attDate,
                        //attendanceDate = Convert.ToString(x.attDate) +"-"+ x.attDate.DayOfWeek,
                        attendanceDate = String.Format("{0:dd/MM/yyyy}", x.attDate) + "-" + x.attDate.DayOfWeek,
                        attInTime = x.attInTime,
                        attOutTime = x.attOutTime,
                        //attStatus = Convert.ToString(x.attDate.DayOfWeek) == "Saturday" ? "vishal Sat" : x.attStatus,
                        attStatus = x.attStatus,
                        //attStatus = (Convert.ToDouble((x.attInTime==""?"0": x.attInTime.Split(' ')[1].Split(':')[0]+"."+ x.attInTime.Split(' ')[1].Split(':')[1])) > validInTime) ? "HD" : x.attStatus,
                        EmpID = x.EmpID,
                        timesheethours = x.timesheethours,
                        workinghours = x.workinghours,
                        description = computeDescription(x.timesheethours, x.attDate)
                    }).ToList();

                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            EmpAttendanceDetails objEmpAttendanceDetails = new EmpAttendanceDetails();
            List<Attendance> objListAttendance = new List<Attendance>();

            #region fill attendace object
            if (lstAttendence != null && lstAttendence.Count > 0)
            {
                foreach (EmpAttLog objItem in lstAttendence)
                {
                    Attendance objAttendance = new Attendance();
                    //objLeaveDetails.ID = objItem.ID;

                    objAttendance.AttInTime = !string.IsNullOrEmpty(objItem.attInTime) ? Convert.ToDateTime(objItem.attInTime).ToString("HH:mm") : "";
                    objAttendance.AttOutTime = !string.IsNullOrEmpty(objItem.attOutTime) ? Convert.ToDateTime(objItem.attOutTime).ToString("HH:mm") : "";
                    objAttendance.AttStatus = objItem.attStatus;
                    objAttendance.AttendanceDate = objItem.attDate.ToString("dd/MM/yyyy");//objItem.attendanceDate
                    objAttendance.Date = Convert.ToString(Convert.ToInt32(objItem.attDate.ToString("dd")));
                    objAttendance.Day = objItem.attDate.ToString("ddd").ToUpper();
                    objAttendance.Description = objItem.description;
                    objAttendance.HolidayImageUrl = "http:/dummy.com";// objItem;
                    objAttendance.ImageId = 10;// objItem;
                    objAttendance.RowBgColor = "#fde9eb";// objItem;
                    objAttendance.RowTextColor = "#fde9eb";// objItem;
                    objAttendance.Timesheethours = objItem.timesheethours;
                    #region code to set View type according to Attendace status
                    if (!string.IsNullOrEmpty(objItem.attStatus))
                    {
                        if (string.Compare(objItem.attStatus, " ", true) == 0 && string.Compare(objItem.description, "Weekend", true) == 0)
                        {
                            objItem.attStatus = "WE";
                        }

                        if (string.Compare(objItem.attStatus, "P", true) == 0) //for Normal
                        {
                            objAttendance.ViewType = 0;
                        }
                        else if (string.Compare(objItem.attStatus, "CL", true) == 0
                            || string.Compare(objItem.attStatus, "SL", true) == 0
                            || string.Compare(objItem.attStatus, "PL", true) == 0
                            || string.Compare(objItem.attStatus, "CO", true) == 0
                            || string.Compare(objItem.attStatus, "WL", true) == 0
                            || string.Compare(objItem.attStatus, "A", true) == 0
                            || string.Compare(objItem.attStatus, "WE", true) == 0)
                        {
                            objAttendance.ViewType = 1;// objItem;
                        }
                        else if (string.Compare(objItem.attStatus, " ", true) == 0 && string.Compare(objItem.description, "Pending", true) == 0)
                        {
                            objAttendance.ViewType = 3;// objItem;
                        }
                        else // For Holiday
                        {
                            objAttendance.ViewType = 2;// objItem;
                        }
                        //Added By Nikhil Shetye
                        if (!string.IsNullOrEmpty(objItem.attInTime))
                        {
                            time = Convert.ToDateTime(objItem.attInTime).ToString("HH:mm");
                            if (!string.IsNullOrEmpty(time))
                            {
                                t2 = new TimeSpan(Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), 0);
                                int result = TimeSpan.Compare(t1, t2);
                                if (result < 0)
                                {
                                    objAttendance.AttStatus = "HD";
                                }
                            }
                        }
                        //End Nikhil Shetye                     
                    }
                    else
                        objAttendance.ViewType = -1;// objItem;
                    #endregion
                    objAttendance.Workinghours = objItem.workinghours;
                    objListAttendance.Add(objAttendance);
                }
            }
            #endregion
            objEmpAttendanceDetails.AttendanceData = objListAttendance;
            return (new JavaScriptSerializer().Serialize(objEmpAttendanceDetails));
        }
        /// <summary>
        /// Method used to get Dashboard data for Employee
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string doGetEmpDashboard(HttpContext context)
        {
            DataTable dtOccasions = null;
            List<Notice> listNotice = new List<Notice>();
            List<CIPBLL> listCIPBLL = new List<CIPBLL>();
            //List<clsCalendar> listAttendace = new List<clsCalendar>(); //comented By Nikhil on 27/11/2017
            List<EmpAttLog> lstAttendence = new List<EmpAttLog>(); //Added By Nikhil on 27/11/2017
            int EmpID = 0;
            int Days = 7;
            string LocationID = "0";
            DateTime StartDate = DateTime.Now;
            string AppPath = string.Empty;

            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            string Startdate = startDate.ToString();
            string EndDate = endDate.ToString();
            string time;
            TimeSpan t1 = new TimeSpan(10, 15, 00);
            TimeSpan t2;
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                // AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    Days = Convert.ToInt32(sData["Days"]);
                    LocationID = Convert.ToString(sData["LocationID"]);
                    if (sData.ContainsKey("StartDate") && sData["StartDate"] != null && !string.IsNullOrEmpty(sData["StartDate"]))
                    {
                        StartDate = Convert.ToDateTime(sData["StartDate"]);
                    }
                    //upcoming occasions
                    dtOccasions = CSCode.Global.Events(LocationID, Days);
                    //news
                    listNotice = Notice.SelectNotice("SelectNotice");
                    // CIP Sessions
                    listCIPBLL = CIPBLL.GetEvents("SELECT_YEARLY", 0, 0);
                    // Dashboard Attendance
                    //listAttendace = new clsCalendar().GetDays(StartDate, EmpID, Convert.ToInt32(LocationID)); //comented By Nikhil on 27/11/2017
                    //Added By Nikhil Shetye on 27/11/2017  
                    lstAttendence = EmpAttLog.GetCalendarAtt("GetCalendarAtt", EmpID, Startdate, EndDate);
                    lstAttendence = lstAttendence.Select(x => new EmpAttLog()
                    {
                        attDate = x.attDate,
                        //attendanceDate = Convert.ToString(x.attDate) +"-"+ x.attDate.DayOfWeek,
                        attendanceDate = String.Format("{0:dd/MM/yyyy}", x.attDate) + "-" + x.attDate.DayOfWeek,
                        attInTime = x.attInTime,
                        attOutTime = x.attOutTime,
                        attStatus = x.attStatus,
                        EmpID = x.EmpID,
                        timesheethours = x.timesheethours,
                        workinghours = x.workinghours,
                        description = computeDescription(x.timesheethours, x.attDate)
                    }).ToList();
                    //End Nikhil Shetye

                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            Dashboard objDashboard = new Dashboard();
            List<Occasion> ListOccasion = new List<Occasion>();
            List<News> ListNews = new List<News>();
            List<CIPSession> ListCIPSession = new List<CIPSession>();
            //List<DashboardAttendance> ListDashboardAttendance = new List<DashboardAttendance>(); //comented By Nikhil on 27/11/2017
            List<Attendance> objListAttendance = new List<Attendance>();  //Added By Nikhil on 27/11/2017

            #region fill Occasion object
            if (dtOccasions != null && dtOccasions.Rows.Count > 0)
            {
                for (int i = 0; i < dtOccasions.Rows.Count; i++)
                {
                    Occasion objOccasion = new Occasion();
                    //Start code by Manoj Raul on 2 Jan 2018
                    DateTime dtOccasionDate = new DateTime(1900, 01, 01);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtOccasions.Rows[i]["Date"])))
                        DateTime.TryParse(Convert.ToString(dtOccasions.Rows[i]["Date"]), out dtOccasionDate);
                    if (dtOccasionDate.Year != 1900)
                        objOccasion.OccassionDate = Convert.ToString(dtOccasionDate.ToString("dd MMM"));
                    else
                        objOccasion.OccassionDate = string.Empty;
                    //End code by Manoj Raul on 2 Jan 2018
                    //objOccasion.OccassionDate = Convert.ToString(dtOccasions.Rows[i]["Date"]);
                    objOccasion.OccassionFor = Convert.ToString(dtOccasions.Rows[i]["EmpName"]);
                    objOccasion.OccassionName = Convert.ToString(dtOccasions.Rows[i]["Event"]);
                    ListOccasion.Add(objOccasion);
                }
            }
            #endregion
            #region fill News object
            if (listNotice != null && listNotice.Count > 0)
            {
                string basepageUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString();
                foreach (Notice objItem in listNotice)
                {
                    News objNews = new News();
                    //objLeaveDetails.ID = objItem.ID;
                    objNews.NewsDate = objItem.NoticeDate;
                    if (!string.IsNullOrEmpty(objItem.notice_descr) && !objItem.notice_descr.ToLower().Contains("http") && objItem.notice_descr.ToLower().Contains("href") && !string.IsNullOrEmpty(basepageUrl))
                        objItem.notice_descr = objItem.notice_descr.Replace("href=\"/", "href=\"" + basepageUrl);
                    objNews.NewsTitle = objItem.notice_descr;
                    ListNews.Add(objNews);
                }
            }
            #endregion
            #region fill CIPSessions object
            if (listCIPBLL != null && listCIPBLL.Count > 0)
            {
                foreach (CIPBLL objItem in listCIPBLL)
                {
                    CIPSession objCIPSession = new CIPSession();
                    //objLeaveDetails.ID = objItem.ID;
                    objCIPSession.Date = objItem.EventDate;
                    objCIPSession.Content = objItem.Description;
                    objCIPSession.Technology = objItem.Description;
                    ListCIPSession.Add(objCIPSession);
                }
            }
            #endregion
            //#region fill Dashboard Attendance object
            //if (listAttendace != null && listAttendace.Count > 0)
            //{
            //    foreach (clsCalendar objItem in listAttendace)
            //    {
            //        DashboardAttendance objDashboardAttendance = new DashboardAttendance();
            //        objDashboardAttendance.EventDate = objItem.Day;
            //        objDashboardAttendance.EventType = objItem.Status;
            //        objDashboardAttendance.EventName = objItem.HolidayLabel;
            //        objDashboardAttendance.InTime = objItem.InHour + ":" + objItem.InMinute;
            //        objDashboardAttendance.OutTime = objItem.OutHour + ":" + objItem.OutMinute;
            //        objDashboardAttendance.OfficeHrs = objItem.DispWorkingHours;
            //        objDashboardAttendance.TSHrs = objItem.WorkingHours;
            //        ListDashboardAttendance.Add(objDashboardAttendance);
            //    }
            //}
            //#endregion
            #region fill attendace object
            if (lstAttendence != null && lstAttendence.Count > 0)
            {
                foreach (EmpAttLog objItem in lstAttendence)
                {
                    Attendance objAttendance = new Attendance();
                    //objLeaveDetails.ID = objItem.ID;

                    objAttendance.AttInTime = !string.IsNullOrEmpty(objItem.attInTime) ? Convert.ToDateTime(objItem.attInTime).ToString("HH:mm") : "";
                    objAttendance.AttOutTime = !string.IsNullOrEmpty(objItem.attOutTime) ? Convert.ToDateTime(objItem.attOutTime).ToString("HH:mm") : "";
                    objAttendance.AttStatus = objItem.attStatus;
                    objAttendance.AttendanceDate = objItem.attDate.ToString("dd/MM/yyyy");//objItem.attendanceDate
                    objAttendance.Date = Convert.ToString(Convert.ToInt32(objItem.attDate.ToString("dd")));
                    objAttendance.Day = objItem.attDate.ToString("ddd").ToUpper();
                    objAttendance.Description = objItem.description;
                    objAttendance.HolidayImageUrl = "http:/dummy.com";// objItem;
                    objAttendance.ImageId = 10;// objItem;
                    objAttendance.RowBgColor = "#fde9eb";// objItem;
                    objAttendance.RowTextColor = "#fde9eb";// objItem;
                    objAttendance.Timesheethours = objItem.timesheethours;
                    #region code to set View type according to Attendace status
                    if (!string.IsNullOrEmpty(objItem.attStatus))
                    {
                        if (string.Compare(objItem.attStatus, " ", true) == 0 && string.Compare(objItem.description, "Weekend", true) == 0)
                        {
                            objItem.attStatus = "WE";
                        }

                        if (string.Compare(objItem.attStatus, "P", true) == 0) //for Normal
                        {
                            objAttendance.ViewType = 0;
                        }
                        else if (string.Compare(objItem.attStatus, "CL", true) == 0
                            || string.Compare(objItem.attStatus, "SL", true) == 0
                            || string.Compare(objItem.attStatus, "PL", true) == 0
                            || string.Compare(objItem.attStatus, "CO", true) == 0
                            || string.Compare(objItem.attStatus, "WL", true) == 0
                            || string.Compare(objItem.attStatus, "A", true) == 0
                            || string.Compare(objItem.attStatus, "WE", true) == 0)
                        {
                            objAttendance.ViewType = 1;// objItem;
                        }
                        else if (string.Compare(objItem.attStatus, " ", true) == 0 && string.Compare(objItem.description, "Pending", true) == 0)
                        {
                            objAttendance.ViewType = 3;// objItem;
                        }
                        else // For Holiday
                        {
                            objAttendance.ViewType = 2;// objItem;
                        }
                        //Added By Nikhil Shetye
                        if (!string.IsNullOrEmpty(objItem.attInTime))
                        {
                            time = Convert.ToDateTime(objItem.attInTime).ToString("HH:mm");
                            if (!string.IsNullOrEmpty(time))
                            {
                                t2 = new TimeSpan(Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), 0);
                                int result = TimeSpan.Compare(t1, t2);
                                if (result < 0)
                                {
                                    objAttendance.AttStatus = "HD";
                                }
                            }
                        }
                        //End Nikhil Shetye                     
                    }
                    else
                        objAttendance.ViewType = -1;// objItem;
                    #endregion
                    objAttendance.Workinghours = objItem.workinghours;
                    objListAttendance.Add(objAttendance);
                }
            }
            #endregion
            //objEmpAttendanceDetails.AttendanceData = objListAttendance;
            objDashboard.OccassionsData = ListOccasion;
            objDashboard.News = ListNews;
            objDashboard.CIPSessions = ListCIPSession;
            //objDashboard.DashboardData = ListDashboardAttendance;   
            objDashboard.DashboardData = objListAttendance;
            objDashboard.HRManualContentCode = Code.HRPolicyCode;  //doGetBase64ofFile(AppPath + "\\ManualsDocument\\HR Manual - 2014.pdf"); ;
            objDashboard.MediclaimPolicyContentCode = Code.MediclaimPolicyCode; //doGetBase64ofFile(AppPath + "\\ManualsDocument\\HEALTH INSURANCE SCHEME.pdf"); ;
            objDashboard.AntiSexualPolicyContentCode = Code.ASHPolicyCode;  //doGetBase64ofFile(AppPath + "\\ManualsDocument\\Intelegain ASHP final.pdf"); ;
            objDashboard.ProfileImageContentCode = Code.ProfileImageCode;
            return (new JavaScriptSerializer().Serialize(objDashboard));
        }
        /// <summary>
        /// Method used to get List of holidays of current financial year
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string doGetHoliday(HttpContext context)
        {
            List<Holiday> listHolidays = new List<Holiday>();
            try
            {
                listHolidays = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            ListHoliday objListHoliday = new ListHoliday();
            List<DashboardHoliday> ListDashboardHoliday = new List<DashboardHoliday>();

            #region fill News object
            if (listHolidays != null && listHolidays.Count > 0)
            {
                foreach (Holiday objItem in listHolidays)
                {
                    DashboardHoliday objDashboardHoliday = new DashboardHoliday();
                    //objLeaveDetails.ID = objItem.ID;
                    objDashboardHoliday.HolidayBgColor = "";
                    objDashboardHoliday.HolidayDay = Convert.ToString(Convert.ToInt32(Convert.ToDateTime(objItem.HolidayDate).ToString("dd")));
                    objDashboardHoliday.HolidayImageUrl = "";
                    objDashboardHoliday.HolidayMonth = Convert.ToDateTime(objItem.HolidayDate).ToString("MMM").ToUpper();
                    objDashboardHoliday.HolidayName = objItem.Narration;
                    objDashboardHoliday.ImageId = 10;
                    ListDashboardHoliday.Add(objDashboardHoliday);
                }
            }
            #endregion
            objListHoliday.HolidayList = ListDashboardHoliday;
            return (new JavaScriptSerializer().Serialize(objListHoliday));
        }
        /// <summary>
        /// Method used to get list of all knowlege base
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string doGetKnowledgeBase(HttpContext context)
        {
            List<KnowledgeBaseBLL> ListKnowledgeBaseBLL = new List<KnowledgeBaseBLL>();
            try
            {
                ListKnowledgeBaseBLL = KnowledgeBaseBLL.getall("Get");
                //listHolidays = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            KnowledgeList objKnowledgeList = new KnowledgeList();
            List<Knowledge> ListKnowledge = new List<Knowledge>();

            #region fill News object
            if (ListKnowledgeBaseBLL != null && ListKnowledgeBaseBLL.Count > 0)
            {
                foreach (KnowledgeBaseBLL objItem in ListKnowledgeBaseBLL)
                {
                    List<KnowledgeTag> ListKnowledgeTag = new List<KnowledgeTag>();
                    if (!string.IsNullOrEmpty(objItem.subtechName))
                    {
                        string[] Tags = objItem.subtechName.Split(',');
                        for (int i = 0; i < Tags.Length; i++)
                        {
                            KnowledgeTag objKnowledgeTag = new KnowledgeTag();
                            objKnowledgeTag.TagName = Convert.ToString(Tags[i]);
                            ListKnowledgeTag.Add(objKnowledgeTag);
                        }
                    }
                    Knowledge objKnowledge = new Knowledge();
                    //objLeaveDetails.ID = objItem.ID;
                    objKnowledge.ID = objItem.kbId;
                    objKnowledge.KnowledgePostedBy = objItem.empName;
                    objKnowledge.KnowledgePostedDate = Convert.ToDateTime(objItem.kbDate).ToString("dd MMM yyyy");
                    objKnowledge.KnowledgeProjectTitle = objItem.projName;
                    objKnowledge.KnowledgeTitle = objItem.kbTitle;
                    objKnowledge.KnowledgeTechnology = objItem.techName;
                    objKnowledge.KnowledgeTagList = ListKnowledgeTag;
                    objKnowledge.KnowledgeDescription = objItem.kbDescrptn;
                    objKnowledge.KnowledgeUrl = objItem.Url;
                    objKnowledge.KnowledegeFileName = objItem.kbFile;
                    ListKnowledge.Add(objKnowledge);
                }
            }
            #endregion
            objKnowledgeList.KnowledgeData = ListKnowledge;
            return (new JavaScriptSerializer().Serialize(objKnowledgeList));
        }
        //Added by Trupti Dandekar
        public string doGetTimeSheetData(HttpContext context)
        {
            List<TimeSheet> ListTimeSheet = new List<TimeSheet>();
            try
            {
                ListTimeSheet = TimeSheet.getall("GetAllTimeSheetData");
                //listHolidays = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            TimeSheetList objTimeSheetList = new TimeSheetList();

            List<TimeSheet> ListTimeSheetdata = new List<TimeSheet>();
            #region fill News object


            if (ListTimeSheet != null && ListTimeSheet.Count > 0)
            {

                //List<TimeSheet> ListTimeSheetData = new List<TimeSheet>();
                foreach (TimeSheet objItem in ListTimeSheet)
                {

                    TimeSheet objTimeSheet = new TimeSheet();
                    //objLeaveDetails.ID = objItem.ID;
                    objTimeSheet.EmpID = objItem.EmpID;
                    //objTimeSheet.EmpName = objItem.EmpName;
                    objTimeSheet.TSID = objItem.TSID;
                    objTimeSheet.ModuleID = objItem.ModuleID;
                    objTimeSheet.TSDate = objItem.TSDate;
                    objTimeSheet.TSHour = objItem.TSHour;
                    objTimeSheet.TSComment = objItem.TSComment;
                    objTimeSheet.TSEntryDate = objItem.TSEntryDate;
                    objTimeSheet.TsVerified = objItem.TsVerified;

                    ListTimeSheetdata.Add(objItem);
                }
            }
            #endregion
            objTimeSheetList.timeSheetData = ListTimeSheetdata;
            return (new JavaScriptSerializer().Serialize(objTimeSheetList));


        }

        //Added by vishal w
        public string doGetEmployeeData_23_Dec_2022(HttpContext context)
        {

            MultiResultUserDetails ObjUserdetails = new MultiResultUserDetails();
            try
            {
                DateTime togetdate;
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                int Empid = 0;
                int TSID = 0;// default value
                DateTime TSDate = DateTime.Now;

                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();

                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);

                    Empid = Convert.ToInt16(sData["EmpId"]);
                    togetdate = Convert.ToDateTime(string.IsNullOrEmpty(sData["date"]) ? DateTime.Now.ToString() : sData["date"]);
                    TSDate = Convert.ToDateTime(string.IsNullOrEmpty(sData["date"]) ? DateTime.Now.ToString() : sData["date"]);


                    if (string.IsNullOrEmpty(togetdate.ToString()))
                    {
                        month = DateTime.Now.Month;
                        year = DateTime.Now.Year;
                    }
                    else
                    {
                        month = togetdate.Month;
                        year = togetdate.Year;
                    }
                    ObjUserdetails = TimeSheet.getMultiResultUserDetails("GetAllMultiResultUserDetailsData", month, year, Empid, TSID, TSDate);
                }
                else
                {
                    context.Response.StatusCode = ConstantTexts.TimeSheetStatusCode.StatusCode;
                    context.Response.StatusDescription = ConstantTexts.TimeSheetStatusCode.StatusDescription;

                    ObjUserdetails = null;
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = ConstantTexts.TimeSheetException.StatusCode;
                context.Response.StatusDescription = ConstantTexts.TimeSheetException.StatusDescription;
            }

            return (new JavaScriptSerializer().Serialize(ObjUserdetails));
        }

        public string doGetEmployeeData(HttpContext context)
        {

            MultiResultUserDetails ObjUserdetails = new MultiResultUserDetails();
            try
            {
                DateTime togetdate;
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                int Empid = 0;
                int TSID = 0;// default value
                DateTime TSDate = DateTime.Now;

                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();

                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);

                    Empid = Convert.ToInt16(sData["EmpId"]);
                    togetdate = Convert.ToDateTime(string.IsNullOrEmpty(sData["date"]) ? DateTime.Now.ToString() : sData["date"]);
                    TSDate = Convert.ToDateTime(string.IsNullOrEmpty(sData["date"]) ? DateTime.Now.ToString() : sData["date"]);


                    if (string.IsNullOrEmpty(togetdate.ToString()))
                    {
                        month = DateTime.Now.Month;
                        year = DateTime.Now.Year;
                    }
                    else
                    {
                        month = togetdate.Month;
                        year = togetdate.Year;
                    }
                    ObjUserdetails = TimeSheet.getMultiResultUserDetails("GetAllMultiResultUserDetailsData", month, year, Empid, TSID, TSDate);

                    // ----- 23-Dec-200 start --------------------------------------------------------------------------------------------------------------
                    var Currentdate = DateTime.Now.Date;


                    if (ObjUserdetails.EmployeePreviousRequestDates.Count > 0)
                    {
                        foreach (var item in ObjUserdetails.EmployeePreviousRequestDates)
                        {
                            var Requesteddate = Convert.ToDateTime(item.RequestDate).Date;

                            if (Currentdate == Requesteddate)
                            {
                                ObjUserdetails.Alloweddate = "01/01/1991 12:00:00 AM";
                                // allowed 
                                //SettingAttribute defualt date here
                                return (new JavaScriptSerializer().Serialize(ObjUserdetails));
                            }
                            else
                            {
                                // 3 days
                                ObjUserdetails.Alloweddate = Currentdate.AddDays(-3).ToString();
                                //return (new JavaScriptSerializer().Serialize(ObjUserdetails));
                            }
                        }


                    }
                    else
                    {
                        ObjUserdetails.Alloweddate = Currentdate.AddDays(-3).ToString();
                        // last 3 days date
                    }
                    // ----- 23-Dec-200 end ----------------------------------------------------------------------------------------------------------------

                }
                else
                {
                    context.Response.StatusCode = ConstantTexts.TimeSheetStatusCode.StatusCode;
                    context.Response.StatusDescription = ConstantTexts.TimeSheetStatusCode.StatusDescription;

                    ObjUserdetails = null;
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = ConstantTexts.TimeSheetException.StatusCode;
                context.Response.StatusDescription = ConstantTexts.TimeSheetException.StatusDescription;
            }
            ObjUserdetails.EmployeePreviousRequestDates = null;// not required in ui
            return (new JavaScriptSerializer().Serialize(ObjUserdetails));
        }
        public string doUpdateMultiResultUserDetails_21_Dec_2022(HttpContext context)
        {

            ResponseData objResponseData = new ResponseData();

            try
            {
                int ModuleID = 0;
                int EmpID = 0;
                DateTime TSDate = DateTime.Now;
                int TSHour = 0;
                string TSComment = "";
                int TSID = 0;
                bool status = true;

                bool Isvalid = true;

                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);

                    ModuleID = Convert.ToInt32(sData["ModuleID"]);
                    EmpID = Convert.ToInt32(sData["EmpID"]);
                    TSDate = Convert.ToDateTime(sData["TSDate"]);
                    TSHour = Convert.ToInt32(sData["TSHour"]);
                    TSComment = Convert.ToString(sData["TSComment"]);
                    TSID = Convert.ToInt32(sData["TSID"]);


                    status = TimeSheet.Update(ModuleID, EmpID, TSDate, TSHour, TSComment, TSID);

                    if (TSComment.Length <= 500)
                    {
                        if (status == true && TSID != 0)
                        {
                            objResponseData.Status = ConstantTexts.TimeSheetResponse.Success;
                            objResponseData.Message = ConstantTexts.TimeSheetCrud.UpdateSuccess;
                        }
                        else if (status == true && TSID == 0)
                        {
                            objResponseData.Status = ConstantTexts.TimeSheetResponse.Success;
                            objResponseData.Message = ConstantTexts.TimeSheetCrud.AddSuccess;
                        }
                        else
                        {
                            objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                            objResponseData.Message = ConstantTexts.TimeSheetResponse.Failed;
                        }

                    }
                    else
                    {
                        objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                        objResponseData.Message = ConstantTexts.TimeSheetCrud.ValidationDescription;
                    }
                }
                else
                {
                    context.Response.StatusCode = ConstantTexts.TimeSheetStatusCode.StatusCode;
                    context.Response.StatusDescription = ConstantTexts.TimeSheetStatusCode.StatusDescription;

                }

            }
            catch (Exception ex)
            {

                context.Response.StatusCode = ConstantTexts.TimeSheetException.StatusCode;
                context.Response.StatusDescription = ConstantTexts.TimeSheetException.StatusDescription;
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        public string doUpdateMultiResultUserDetails(HttpContext context)
        {

            ResponseData objResponseData = new ResponseData();
            MultiResultUserDetails ObjUserdetails = new MultiResultUserDetails();
            MultiResultUserDetails objTothrs = new MultiResultUserDetails();

            try
            {
                int ModuleID = 0;
                int EmpID = 0;
                DateTime TSDate = DateTime.Now;
                int TSHour = 0;
                string TSComment = "";
                int TSID = 0;
                bool status = true;

                bool Isvalid = true;
                int objFilledTotHrs = 0;

                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);

                    ModuleID = Convert.ToInt32(sData["ModuleID"]);
                    EmpID = Convert.ToInt32(sData["EmpID"]);
                    TSDate = Convert.ToDateTime(sData["TSDate"]);
                    TSHour = Convert.ToInt32(sData["TSHour"]);
                    TSComment = Convert.ToString(sData["TSComment"]);
                    TSID = Convert.ToInt32(sData["TSID"]);

                    int StrTotDays = Convert.ToInt32((TSDate - DateTime.Now).TotalDays);// check TSDate is before 3 days back from today
                    var TotDays = Convert.ToString(StrTotDays).Replace("-", "");
                    int totalDays = Convert.ToInt32(TotDays);

                    ObjUserdetails = TimeSheet.getMultiResultUserDetails("GetAllMultiResultUserDetailsData", TSDate.Month, TSDate.Year, EmpID, TSID, TSDate);

                    if (TSHour == 0)
                    {
                        Isvalid = false;
                        objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                        objResponseData.Message = ConstantTexts.TimeSheetCrud.TimesheerZeroHrs;
                        return (new JavaScriptSerializer().Serialize(objResponseData));
                    }


                    if (ObjUserdetails.EmployeePreviousRequestDates.Count > 0)
                    {
                        foreach (var item in ObjUserdetails.EmployeePreviousRequestDates)
                        {
                            var Currentdate = DateTime.Now.Date;
                            var Requesteddate = Convert.ToDateTime(item.RequestDate).Date;

                            if (Currentdate == Requesteddate)
                            {
                                Isvalid = true;
                                // allowed 
                            }
                            else
                            {
                                Isvalid = false;
                            }
                        }

                        if (Isvalid == false)
                        {
                            if (totalDays > 4)
                            {
                                Isvalid = false;
                                objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                                objResponseData.Message = ConstantTexts.TimeSheetCrud.AddValidationFailed3Days;
                                return (new JavaScriptSerializer().Serialize(objResponseData));
                            }
                            else
                            {
                                Isvalid = true;
                            }
                        }
                    }
                    else
                    {
                        // check TSDate is before 3 days back from today
                        if (totalDays > 4)
                        {
                            Isvalid = false;
                            objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                            objResponseData.Message = ConstantTexts.TimeSheetCrud.AddValidationFailed3Days;
                            return (new JavaScriptSerializer().Serialize(objResponseData));
                        }
                    }

                    foreach (var item in ObjUserdetails.EmployeeTimesheetFilledHrs)
                    {
                        objFilledTotHrs = item.FilledTotHrs;
                    }


                    //int TotHrsfilled = ObjUserdetails.TotFilledTotHrs.FilledTotHrs;
                    int TotHrsfilled = objFilledTotHrs;

                    if (TSID == 0)
                    {
                        int insertionHrs = TSHour + TotHrsfilled;

                        if (insertionHrs > 24)
                        {
                            Isvalid = false;
                            objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                            objResponseData.Message = ConstantTexts.TimeSheetCrud.AddUpdateValidation24hrs;
                            return (new JavaScriptSerializer().Serialize(objResponseData));
                        }

                    }

                    if (TSID > 0)
                    {
                        int insertionHrs = TSHour + TotHrsfilled;

                        if (insertionHrs > 24)
                        {
                            Isvalid = false;
                            objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                            objResponseData.Message = ConstantTexts.TimeSheetCrud.AddUpdateValidation24hrs;
                            return (new JavaScriptSerializer().Serialize(objResponseData));
                        }
                    }


                    // TSDate is greater than today date
                    if (totalDays == 0)
                    {
                        Isvalid = false;
                        objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                        objResponseData.Message = ConstantTexts.TimeSheetCrud.AddValidationFailedDateGreaterTahnCurrentdate;
                        return (new JavaScriptSerializer().Serialize(objResponseData));
                    }

                    if (TSComment.Length > 500)
                    {
                        Isvalid = false;
                        objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                        objResponseData.Message = ConstantTexts.TimeSheetCrud.ValidationDescription;
                        return (new JavaScriptSerializer().Serialize(objResponseData));
                    }

                    if (Isvalid == true)
                    {
                        status = EmpTimesheetBAL.Update(ModuleID, EmpID, TSDate, TSHour, TSComment, TSID);
                        if (status == true && TSID != 0)
                        {
                            objResponseData.Status = ConstantTexts.TimeSheetResponse.Success;
                            objResponseData.Message = ConstantTexts.TimeSheetCrud.UpdateSuccess;
                        }
                        else if (status == true && TSID == 0)
                        {
                            objResponseData.Status = ConstantTexts.TimeSheetResponse.Success;
                            objResponseData.Message = ConstantTexts.TimeSheetCrud.AddSuccess;
                        }
                        else
                        {
                            objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                            objResponseData.Message = ConstantTexts.TimeSheetResponse.Failed;
                        }
                    }

                }
                else
                {
                    context.Response.StatusCode = ConstantTexts.TimeSheetStatusCode.StatusCode;
                    context.Response.StatusDescription = ConstantTexts.TimeSheetStatusCode.StatusDescription;

                }

            }
            catch (Exception ex)
            {

                context.Response.StatusCode = ConstantTexts.TimeSheetException.StatusCode;
                context.Response.StatusDescription = ConstantTexts.TimeSheetException.StatusDescription;
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        public string doRemoveMultiResultUserDetails(HttpContext context)
        {

            ResponseData objResponseData = new ResponseData();

            try
            {
                int TSID = 0;
                bool status = true;

                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    TSID = Convert.ToInt32(sData["TSID"]);
                    status = EmpTimesheetBAL.Delete(TSID);
                    if (status == true)
                    {
                        objResponseData.Status = ConstantTexts.TimeSheetResponse.Success;
                        objResponseData.Message = ConstantTexts.TimeSheetCrud.DeleteSuccess;
                    }
                    else
                    {
                        objResponseData.Status = ConstantTexts.TimeSheetResponse.Failed;
                        objResponseData.Message = ConstantTexts.TimeSheetCrud.DeleteFailed;
                    }

                }
                else
                {
                    context.Response.StatusCode = ConstantTexts.TimeSheetStatusCode.StatusCode;
                    context.Response.StatusDescription = ConstantTexts.TimeSheetStatusCode.StatusDescription;
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = ConstantTexts.TimeSheetException.StatusCode;
                context.Response.StatusDescription = ConstantTexts.TimeSheetException.StatusDescription;
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        public string doGetContacts(HttpContext context)
        {
            string EmpName = "";
            int EmpID = 0;
            List<EmployeeMaster> lstEmployeeMaster = new List<EmployeeMaster>();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {

                    EmployeeMaster clsBoard = new EmployeeMaster();
                    clsBoard.empName = "Board Number";
                    clsBoard.empid = 0;
                    clsBoard.empEmail = "contactus@intelgain.com";
                    clsBoard.empContact = "91 22 41516100";
                    clsBoard.Flag = "Generic";
                    lstEmployeeMaster.Add(clsBoard);
                    EmployeeMaster clsHR = new EmployeeMaster();
                    clsHR.empName = "HR";
                    clsHR.empid = 0;
                    clsHR.empEmail = "hr@intelgain.com";
                    clsHR.empContact = "91 22 41516102";
                    clsHR.Flag = "Generic";
                    lstEmployeeMaster.Add(clsHR);
                    EmployeeMaster clsAccounts = new EmployeeMaster();
                    clsAccounts.empName = "Accounts";
                    clsAccounts.empid = 0;
                    clsAccounts.empEmail = "accounts@intelgain.com";
                    clsAccounts.empContact = "91 22 41516106";
                    clsAccounts.Flag = "Generic";
                    lstEmployeeMaster.Add(clsAccounts);
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    ////  EmpName = Convert.ToString(sData["EmpName"]);
                    DataTable dttEmpDetails = EmployeeMaster.GetEmpDetails("GetEmployeeList", EmpName, EmpID);
                    if (dttEmpDetails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dttEmpDetails.Rows)
                        {
                            EmployeeMaster clsEmployeeMaster = new EmployeeMaster();
                            clsEmployeeMaster.empName = Convert.ToString(dr["empName"]);
                            clsEmployeeMaster.empid = Convert.ToInt32(dr["empid"]);
                            clsEmployeeMaster.empEmail = Convert.ToString(dr["empEmail"]);
                            clsEmployeeMaster.empContact = Convert.ToString(dr["empContact"]);
                            clsEmployeeMaster.Flag = Convert.ToString(dr["Flag"]);
                            clsEmployeeMaster.strBirthday = Convert.ToString(dr["empBDate"]);
                            lstEmployeeMaster.Add(clsEmployeeMaster);
                        }
                    }
                    else
                    {
                        EmployeeMaster clsEmployeeMaster = new EmployeeMaster();
                        lstEmployeeMaster.Add(clsEmployeeMaster);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            List<EmployeeContact> ListEmployeeContact = new List<EmployeeContact>();
            if (lstEmployeeMaster != null && lstEmployeeMaster.Count > 0)
            {
                for (int i = 0; i < lstEmployeeMaster.Count; i++)
                {
                    EmployeeContact objEmployeeContact = new EmployeeContact();
                    objEmployeeContact.empid = lstEmployeeMaster[i].empid;
                    objEmployeeContact.empName = lstEmployeeMaster[i].empName;
                    objEmployeeContact.empContact = lstEmployeeMaster[i].empContact;
                    objEmployeeContact.empEmail = lstEmployeeMaster[i].empEmail;
                    objEmployeeContact.Designation = lstEmployeeMaster[i].Designation;
                    objEmployeeContact.ProfileImageURL = "";
                    ListEmployeeContact.Add(objEmployeeContact);
                }
            }
            return (new JavaScriptSerializer().Serialize(ListEmployeeContact));
        }

        public string doGetKbAttachment(HttpContext context)
        {
            var attachmentData = new List<KbAttachmentData>();
            try
            {
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                var jss = new JavaScriptSerializer();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    var fileName = Convert.ToString(sData["FileName"]);
                    var kbId = Convert.ToInt32(sData["KbId"] == "" ? "0" : sData["KbId"]);
                    if (kbId != 0 && string.IsNullOrEmpty(fileName))
                    {
                        var attachment = KnowledgeBaseBLL.getAttchment(kbId);
                        if (attachment != null)
                        {
                            foreach (var objItem in attachment)
                            {
                                attachmentData.Add(new KbAttachmentData
                                {
                                    FileName = objItem.kbFile,
                                    FileData = doGetBase64ofFile(ConfigurationManager.AppSettings["DataBank"] + "KB\\" + objItem.kbFile)
                                });
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(fileName) && kbId != 0)
                    {
                        attachmentData.Add(new KbAttachmentData
                        {
                            FileName = fileName,
                            FileData = doGetBase64ofFile(ConfigurationManager.AppSettings["DataBank"] + "KB\\" + fileName)
                        });
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.StatusDescription = "Bad Request";
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            KBAttachment objKBAttachment = new KBAttachment();
            objKBAttachment.Attachments = attachmentData;
            return (new JavaScriptSerializer().Serialize(objKBAttachment));
        }

        public string doGetEmpProfile(HttpContext context)
        {
            string EmpName = "";
            int EmpID = 0;
            //List<EmployeeMaster> lstEmployeeMaster = new List<EmployeeMaster>();
            EmployeeMaster objEmployeeMaster = new EmployeeMaster();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    //EmployeeMaster clsBoard = new EmployeeMaster();
                    //clsBoard.empName = "Board Number";
                    //clsBoard.empid = 0;
                    //clsBoard.empEmail = "contactus@intelgain.com";
                    //clsBoard.empContact = "91 22 41516100";
                    //clsBoard.Flag = "Generic";
                    //lstEmployeeMaster.Add(clsBoard);
                    //EmployeeMaster clsHR = new EmployeeMaster();
                    //clsHR.empName = "HR";
                    //clsHR.empid = 0;
                    //clsHR.empEmail = "hr@intelgain.com";
                    //clsHR.empContact = "91 22 41516102";
                    //clsHR.Flag = "Generic";
                    //lstEmployeeMaster.Add(clsHR);
                    //EmployeeMaster clsAccounts = new EmployeeMaster();
                    //clsAccounts.empName = "Accounts";
                    //clsAccounts.empid = 0;
                    //clsAccounts.empEmail = "accounts@intelgain.com";
                    //clsAccounts.empContact = "91 22 41516106";
                    //clsAccounts.Flag = "Generic";
                    //lstEmployeeMaster.Add(clsAccounts);
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    ////  EmpName = Convert.ToString(sData["EmpName"]);
                    DataTable dttEmpDetails = EmployeeMaster.GetEmpDetails("GetEmployeeList", EmpName, EmpID);
                    if (dttEmpDetails.Rows.Count > 0)
                    {
                        string searchExpression = "empID = " + Convert.ToString(EmpID);
                        DataRow[] foundRows = dttEmpDetails.Select(searchExpression);
                        foreach (DataRow dr in foundRows)//dttEmpDetails.Rows)
                        {
                            objEmployeeMaster.empName = Convert.ToString(dr["empName"]);
                            objEmployeeMaster.empid = Convert.ToInt32(dr["empid"]);
                            objEmployeeMaster.empEmail = Convert.ToString(dr["empEmail"]);
                            objEmployeeMaster.empContact = Convert.ToString(dr["empContact"]);
                            objEmployeeMaster.Flag = Convert.ToString(dr["Flag"]);
                            objEmployeeMaster.strBirthday = Convert.ToString(dr["empBDate"]);
                            objEmployeeMaster.empAddress = Convert.ToString(dr["empAddress"]);
                            objEmployeeMaster.Designation = Convert.ToString(dr["Designation"]);
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            //List<Employee> ListEmployee = new List<Employee>();
            Employee objEmployee = new Employee();
            if (objEmployeeMaster != null)
            {
                //    for (int i = 0; i < lstEmployeeMaster.Count; i++)
                //    {
                objEmployee.empid = objEmployeeMaster.empid;
                objEmployee.empName = objEmployeeMaster.empName;
                objEmployee.empAddress = objEmployeeMaster.empAddress;
                objEmployee.empGender = objEmployeeMaster.empGender;
                objEmployee.empContact = objEmployeeMaster.empContact;
                objEmployee.empJoiningDate = objEmployeeMaster.empJoiningDate;
                objEmployee.empLeavingDate = objEmployeeMaster.empLeavingDate;
                objEmployee.empProbationPeriod = objEmployeeMaster.empProbationPeriod;
                objEmployee.empNotes = objEmployeeMaster.empNotes;
                objEmployee.empEmail = objEmployeeMaster.empEmail;
                objEmployee.empTester = objEmployeeMaster.empTester;
                objEmployee.empAccountNo = objEmployeeMaster.empAccountNo;
                objEmployee.EmpPAN = objEmployeeMaster.EmpPAN;
                objEmployee.EmpUAN = objEmployeeMaster.EmpUAN;
                objEmployee.EmpEPF = objEmployeeMaster.EmpEPF;
                objEmployee.empBDate = objEmployeeMaster.empBDate;
                objEmployee.empADate = objEmployeeMaster.empADate;
                objEmployee.empPrevEmployer = objEmployeeMaster.empPrevEmployer;
                objEmployee.empExperince = objEmployeeMaster.empExperince;
                objEmployee.IsSuperAdmin = objEmployeeMaster.IsSuperAdmin;
                objEmployee.IsAccountAdmin = objEmployeeMaster.IsAccountAdmin;
                objEmployee.IsPayrollAdmin = objEmployeeMaster.IsPayrollAdmin;
                objEmployee.IsPM = objEmployeeMaster.IsPM;
                objEmployee.IsProjectReport = objEmployeeMaster.IsProjectReport;
                objEmployee.IsProjectStatus = objEmployeeMaster.IsProjectStatus;
                objEmployee.IsLeaveAdmin = objEmployeeMaster.IsLeaveAdmin;
                objEmployee.IsActive = objEmployeeMaster.IsActive;
                objEmployee.LocationFKID = objEmployeeMaster.LocationFKID;
                objEmployee.skillid = objEmployeeMaster.skillid;
                objEmployee.InsertedOn = objEmployeeMaster.InsertedOn;
                objEmployee.InsertedBy = objEmployeeMaster.InsertedBy;
                objEmployee.InsertedIP = objEmployeeMaster.InsertedIP;
                objEmployee.ModifiedOn = objEmployeeMaster.ModifiedOn;
                objEmployee.ModifiedBy = objEmployeeMaster.ModifiedBy;
                objEmployee.ModifiedIP = objEmployeeMaster.ModifiedIP;
                objEmployee.IsTester = objEmployeeMaster.IsTester;
                objEmployee.empPassword = objEmployeeMaster.empPassword;
                objEmployee.AnnualCTC = objEmployeeMaster.AnnualCTC;
                objEmployee.CTC = objEmployeeMaster.CTC;
                objEmployee.Gross = objEmployeeMaster.Gross;
                objEmployee.Net = objEmployeeMaster.Net;
                objEmployee.Resume = objEmployeeMaster.Resume;
                objEmployee.Qualification = objEmployeeMaster.Qualification;
                objEmployee.QualificationId = objEmployeeMaster.QualificationId;
                objEmployee.SecSkills = objEmployeeMaster.SecSkills;
                objEmployee.SecSkillsId = objEmployeeMaster.SecSkillsId;
                objEmployee.PrimarySkill = objEmployeeMaster.PrimarySkill;
                objEmployee.Designation = objEmployeeMaster.Designation;
                objEmployee.Skill = objEmployeeMaster.Skill;
                objEmployee.Type = objEmployeeMaster.Type;
                objEmployee.Photo = objEmployeeMaster.Photo;
                objEmployee.Mode = objEmployeeMaster.Mode;
                objEmployee.EmpStatus = objEmployeeMaster.EmpStatus;
                objEmployee.LeavingStatus = objEmployeeMaster.LeavingStatus;
                objEmployee.ProjectID = objEmployeeMaster.ProjectID;
                objEmployee.SecurityLevel = objEmployeeMaster.SecurityLevel;
                objEmployee.PrimarySkillDesc = objEmployeeMaster.PrimarySkillDesc;
                objEmployee.Event = objEmployeeMaster.Event;
                objEmployee.ProfileID = objEmployeeMaster.ProfileID;
                objEmployee.CAddress = objEmployeeMaster.CAddress;
                objEmployee.HistoryID = objEmployeeMaster.HistoryID;
                objEmployee.ADUserName = objEmployeeMaster.ADUserName;
                objEmployee.Flag = objEmployeeMaster.Flag;
                objEmployee.strBirthday = objEmployeeMaster.strBirthday;
                //        ListEmployee.Add(objEmployee);
                //    }

            }
            return (new JavaScriptSerializer().Serialize(objEmployee));
        }
        //Added By Nikhil Shetye on 15/11/2017
        /// <summary>
        /// Method used to get projects names 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return list of project names</returns>
        public string doGetProject(HttpContext context)
        {
            List<KnowledgeBaseBLL> ListKnowledgeBaseBLL = new List<KnowledgeBaseBLL>();
            try
            {
                ListKnowledgeBaseBLL = KnowledgeBaseBLL.getallProj("GetProj");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            ProjectList objProjectList = new ProjectList();
            List<Project> ListProject = new List<Project>();

            Project objProjectNm = new Project();
            objProjectNm.projId = 0;
            objProjectNm.projName = "Knowledge Base";
            objProjectNm.custId = 0;
            objProjectNm.projDesc = "";
            ListProject.Add(objProjectNm);
            #region fill Project object
            if (ListKnowledgeBaseBLL != null && ListKnowledgeBaseBLL.Count > 0)
            {
                foreach (KnowledgeBaseBLL objItem in ListKnowledgeBaseBLL)
                {
                    Project objProject = new Project();
                    //objLeaveDetails.ID = objItem.ID;
                    objProject.projId = objItem.projId;
                    objProject.projName = objItem.projName;
                    objProject.custId = objItem.custId;
                    objProject.projDesc = objItem.projDesc;
                    ListProject.Add(objProject);
                }
            }
            #endregion
            objProjectList.ProjectData = ListProject;
            return (new JavaScriptSerializer().Serialize(objProjectList));
        }
        /// <summary>
        /// Method used to get technology names 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return list of technology names</returns>
        public string doGetTechnology(HttpContext context)
        {
            List<KnowledgeBaseBLL> ListKnowledgeBaseBLL = new List<KnowledgeBaseBLL>();
            try
            {
                ListKnowledgeBaseBLL = KnowledgeBaseBLL.getallTech("GetTech");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            TechnologyList objTechnologyList = new TechnologyList();
            List<Technology> ListTechnology = new List<Technology>();

            #region fill Technology object
            if (ListKnowledgeBaseBLL != null && ListKnowledgeBaseBLL.Count > 0)
            {
                foreach (KnowledgeBaseBLL objItem in ListKnowledgeBaseBLL)
                {
                    Technology objTechnology = new Technology();
                    //objLeaveDetails.ID = objItem.ID;
                    objTechnology.techId = objItem.techId;
                    objTechnology.techName = objItem.techName;
                    objTechnology.subtechName = objItem.subtechName;
                    ListTechnology.Add(objTechnology);
                }
            }
            #endregion
            objTechnologyList.TechnologyData = ListTechnology;
            return (new JavaScriptSerializer().Serialize(objTechnologyList));
        }
        /// <summary>
        /// Method used to edit profile 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message whether it is successful or failed</returns>
        public string doEditProfile(HttpContext context)
        {
            EmployeeMaster obj = new EmployeeMaster();
            ResponseData objResponseData = new ResponseData();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            string result = string.Empty;
            int NoOfRowsAffected = 0;
            int EmpID = 0;
            string CurrentAddress = string.Empty;
            string ContactNumber = string.Empty;
            string AnniversaryDate = string.Empty;
            DateTime AnniversaryDateNew = DateTime.MinValue;
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    CurrentAddress = Convert.ToString(sData["CurrentAddress"]);
                    ContactNumber = Convert.ToString(sData["ContactNumber"]);
                    AnniversaryDate = Convert.ToString(sData["AnniversaryDate"]);
                    if (!string.IsNullOrEmpty(AnniversaryDate))
                    {
                        AnniversaryDateNew = DateTime.ParseExact(AnniversaryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        result = obj.SaveEmpProfile("SaveEmpProfile", EmpID, CurrentAddress, ContactNumber, AnniversaryDateNew.ToString("yyyy/MM/dd"), EmpID);
                    }
                    else
                    {
                        result = obj.SaveEmpProfile("SaveEmpProfile", EmpID, CurrentAddress, ContactNumber, "", EmpID);
                    }
                    NoOfRowsAffected = Convert.ToInt32(result);
                    if (NoOfRowsAffected > 0)
                    {
                        objResponseData.Status = "1";
                        objResponseData.Message = "Saved successfully.";
                    }
                    else
                    {
                        objResponseData.Status = "0";
                        objResponseData.Message = "Edit profile failed.";
                        context.Response.StatusCode = 400;
                        context.Response.StatusDescription = "Bad Request.";
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// Method used to upload image in byte format 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message whether it is successful or failed</returns>
        public string doImageUpload(HttpContext context)
        {
            EmployeeMaster obj = new EmployeeMaster();
            ResponseData objResponseData = new ResponseData();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            int EmpID = 0;
            byte[] ByteImage = null;
            string result = string.Empty;
            int NoOfRowsAffected = 0;
            string EmpId = string.Empty;
            string FileName = string.Empty;
            try
            {


                //Initialise an Object of MultipartParser Class With Requested Stream                   
                ClsMultipartParser parser = new ClsMultipartParser(context.Request.InputStream);
                var uploadImageEntity = new UploadImageEntity();
                //Check that we have not null value in requested stream  
                if (parser != null && parser.Success)
                {
                    //Fetch Requested Formdata (content)   
                    //(for this example our requested formdata are UserName[String])  
                    foreach (var item in parser.MyContents)
                    {
                        //Check our requested fordata                           
                        if (item.PropertyName.Contains("EmpId"))
                        {
                            uploadImageEntity.ID = item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace(@"\", "").Replace(" ", "").Replace(",", "").Replace("{", "").Replace("}", "").Replace("\n", "");
                        }
                        else if (item.PropertyName.Contains("UploadedImage"))
                        {
                            uploadImageEntity.UploadImage = item.Data;
                        }
                        else if (item.PropertyName.Contains("removePhoto"))
                        {
                            uploadImageEntity.RemovePhoto = Convert.ToBoolean(item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace(@"\", "").Replace(" ", "").Replace(",", "").Replace("{", "").Replace("}", "").Replace("\n", ""));
                        }
                    }
                    if (uploadImageEntity.RemovePhoto)
                    {
                        try
                        {
                            string filePathname = ConfigurationManager.AppSettings["DataBank"] + "mobileprofileimage\\" + uploadImageEntity.ID + ".jpg";
                            if (File.Exists(filePathname))
                            {
                                File.Delete(filePathname);
                                objResponseData.Status = "1";
                                objResponseData.Message = "Profile image removed sucessfully.";
                            }
                            else
                            {
                                objResponseData.Status = "0";
                                objResponseData.Message = "Profile image not found.";
                            }
                        }
                        catch (Exception)
                        {
                            objResponseData.Status = "0";
                            objResponseData.Message = "Failed to remove image.";
                        }
                    }
                    else if (uploadImageEntity.UploadImage != null)
                    {
                        Image resultimg = null;
                        ImageFormat format = ImageFormat.Png;
                        resultimg = new Bitmap(new MemoryStream(parser.FileContents));
                        using (Image imageToExport = resultimg)
                        {
                            if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["DataBank"] + "mobileprofileimage"))
                            {
                                System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["DataBank"] + "mobileprofileimage");
                            }
                            string filePathname = ConfigurationManager.AppSettings["DataBank"] + "mobileprofileimage\\" + uploadImageEntity.ID + ".jpg";
                            imageToExport.Save(filePathname);
                            objResponseData.Status = "1";
                            objResponseData.Message = "Profile image saved sucessfully.";
                        }
                    }
                }
                else
                {
                    objResponseData.Status = "0";
                    objResponseData.Message = "Profile image failed to process.";
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// Method used to save skill matrix 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message whether it is successful or failed</returns>
        public string doSaveSkillMatrix(HttpContext context)
        {
            SkillMatrixBLL objEmpSkill = new SkillMatrixBLL();
            SkillMatrixBLL obj = new SkillMatrixBLL();
            ResponseData objResponseData = new ResponseData();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            string result = string.Empty;
            int NoOfRowsAffected = 0;
            int EmpID = 0;
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    //Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    objEmpSkill = JsonConvert.DeserializeObject<SkillMatrixBLL>(json);
                    result = obj.SaveEmployeeSkill("SaveEmployeeSkill", objEmpSkill);
                    //NoOfRowsAffected = Convert.ToInt32(result); string.Compare
                    //if ( result == "saved successfully")
                    if (string.Compare(result, "Saved successfully", true) == 0)
                    {
                        objResponseData.Status = "1";
                        objResponseData.Message = "Saved successfully.";
                    }
                    else
                    {
                        objResponseData.Status = "0";
                        objResponseData.Message = "Skills save failed.";
                        context.Response.StatusCode = 400;
                        context.Response.StatusDescription = "Bad Request.";
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// Method used to get skill matrix 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return list of skill</returns>
        public string doGetSkillMatrix(HttpContext context)
        {
            SkillMatrixBLL objBLL = new SkillMatrixBLL();
            ResponseData objResponseData = new ResponseData();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            string result = string.Empty;
            int EmpID = 0;
            int ToggleSkill = 0;
            string SkillName = string.Empty;
            List<SkillMatrix> objListSkill = new List<SkillMatrix>();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    ToggleSkill = Convert.ToInt32(sData["ToggleSkill"]);
                    SkillName = Convert.ToString(sData["SkillName"]);
                    objBLL = objBLL.GetEmployeeSkill("EmployeeSkill", EmpID, SkillName, ToggleSkill);
                    if (objBLL != null && objBLL.lstEmpSkill != null)
                    {
                        if (objBLL.lstEmpSkill.Count > 0)
                        {
                            for (int i = 0; i < objBLL.lstEmpSkill.Count; i++)
                            {

                                SkillMatrix objSkillMatrix = new SkillMatrix();
                                if (objBLL.lstEmpSkill[i].Experience > 0)
                                {
                                    objSkillMatrix.Years = (int)(objBLL.lstEmpSkill[i].Experience / 12);
                                    objSkillMatrix.Months = (int)(objBLL.lstEmpSkill[i].Experience % 12);
                                }
                                else
                                {
                                    objSkillMatrix.Years = objBLL.lstEmpSkill[i].Years;
                                    objSkillMatrix.Months = objBLL.lstEmpSkill[i].Months;
                                }
                                objSkillMatrix.SkillID = objBLL.lstEmpSkill[i].SkillID;
                                objSkillMatrix.CategoryId = objBLL.lstEmpSkill[i].CategoryId;
                                objSkillMatrix.EmployeeSkillID = objBLL.lstEmpSkill[i].EmployeeSkillID;
                                objSkillMatrix.EmpID = objBLL.lstEmpSkill[i].EmpID;
                                objSkillMatrix.UserID = objBLL.lstEmpSkill[i].UserID;
                                objSkillMatrix.Experience = objBLL.lstEmpSkill[i].Experience;
                                objSkillMatrix.EmpCount = objBLL.lstEmpSkill[i].EmpCount;
                                objSkillMatrix.Mode = objBLL.lstEmpSkill[i].Mode;
                                objSkillMatrix.Category = objBLL.lstEmpSkill[i].Category;
                                objSkillMatrix.SkillName = objBLL.lstEmpSkill[i].SkillName;
                                objSkillMatrix.Level = objBLL.lstEmpSkill[i].Level;
                                objSkillMatrix.ActiveSkill = objBLL.lstEmpSkill[i].ActiveSkill;
                                objSkillMatrix.Status = objBLL.lstEmpSkill[i].Status;
                                objSkillMatrix.EmpName = objBLL.lstEmpSkill[i].EmpName;
                                objSkillMatrix.MaxExperience = objBLL.lstEmpSkill[i].MaxExperience;
                                objSkillMatrix.InsertedDate = objBLL.lstEmpSkill[i].InsertedDate;
                                objListSkill.Add(objSkillMatrix);
                                //objBLL.lstEmpSkill[i].Years = (int)(objBLL.lstEmpSkill[i].Experience / 12);
                                //objBLL.lstEmpSkill[i].Months = (int)(objBLL.lstEmpSkill[i].Experience % 12);                                
                            }
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            ListSkillMatrix obj = new ListSkillMatrix();
            obj.lstEmpSkill = objListSkill;
            return (new JavaScriptSerializer().Serialize(obj));
        }
        /// <summary>
        /// Method used to add Knowledge 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message success or failed</returns>
        public string doAddKnowledge(HttpContext context)
        {
            KnowledgeBaseBLL objKnowledgeBaseBLL = new KnowledgeBaseBLL();
            ResponseData objResponseData = new ResponseData();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            string result = string.Empty;
            int EmpId = 0;
            string kbDescrptn = string.Empty;
            string kbComments = string.Empty;
            string kbFile = string.Empty;
            string kbTitle = string.Empty;
            int techId = 0;
            int projId = 0;
            string url = string.Empty;
            string subtechname = string.Empty;
            DataRow[] dr;
            string strEmpName = string.Empty;
            int id = 0;
            try
            {

                //Initialise an Object of MultipartParser Class With Requested Stream                   
                ClsMultipartParser parser = new ClsMultipartParser(context.Request.InputStream);
                var objKnowledge = new Knowledge();

                //Check that we have not null value in requested stream  

                if (parser != null && parser.Success)
                {
                    foreach (var item in parser.MyContents)
                    {
                        //Check our requested fordata                           
                        if (item.PropertyName.Contains("EmpId"))
                        {
                            objKnowledge.EmpId = Convert.ToInt32(item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace(@"\", "").Replace(" ", "").Replace(",", "").Replace("{", "").Replace("}", "").Replace("\n", ""));
                        }
                        else if (item.PropertyName.Contains("KbDescrptn"))
                        {
                            objKnowledge.KnowledgeDescription = item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace("{", "").Replace("}", "").Replace("\n", "");
                        }
                        else if (item.PropertyName.Contains("KbTitle"))
                        {
                            objKnowledge.KnowledgeProjectTitle = item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace("{", "").Replace("}", "").Replace("\n", "");
                        }
                        else if (item.PropertyName.Contains("TechId"))
                        {
                            objKnowledge.KnowledegeTechnologyId = Convert.ToInt32(item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace(@"\", "").Replace(" ", "").Replace(",", "").Replace("{", "").Replace("}", "").Replace("\n", ""));
                        }
                        else if (item.PropertyName.Contains("ProjId"))
                        {
                            objKnowledge.KnowledeProjectId = Convert.ToInt32(item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace(@"\", "").Replace(" ", "").Replace(",", "").Replace("{", "").Replace("}", "").Replace("\n", ""));
                        }
                        else if (item.PropertyName.Contains("Url"))
                        {
                            objKnowledge.KnowledgeUrl = item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace("{", "").Replace("}", "").Replace("\n", "");
                        }
                        else if (item.PropertyName.Contains("SubTechName"))
                        {
                            objKnowledge.KnowledegeSubTechName = item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace("{", "").Replace("}", "").Replace("\n", "");
                        }
                    }
                    if (!string.IsNullOrEmpty(parser.Filename))
                    {
                        objKnowledge.KnowledegeFileName = parser.Filename;
                    }
                    id = KnowledgeBaseBLL.InsertKb("Insert", objKnowledge.EmpId, objKnowledge.KnowledgeDescription, kbComments, objKnowledge.KnowledegeFileName, objKnowledge.KnowledgeProjectTitle, objKnowledge.KnowledegeTechnologyId, objKnowledge.KnowledeProjectId, objKnowledge.KnowledgeUrl, objKnowledge.KnowledegeSubTechName);
                    DataTable dttEmpDetails = EmployeeMaster.GetEmpDetails("GetEmployeeList", "", objKnowledge.EmpId);
                    if (dttEmpDetails != null)
                    {
                        if (dttEmpDetails.Rows.Count > 0)
                        {
                            dr = dttEmpDetails.Select("empid=" + objKnowledge.EmpId);
                            if (dr != null)
                            {
                                if (dr.Length > 0)
                                {
                                    strEmpName = Convert.ToString(dr[0]["empName"]);
                                    if (id > 0)
                                    {
                                        SendKnowledgeBaseMail("Insert", objKnowledge.KnowledgeProjectTitle, strEmpName, objKnowledge.KnowledgeDescription, context);
                                    }
                                }
                            }
                        }
                    }
                    if (id != 0)
                    {
                        if (parser.FileContents != null)
                        {

                            MemoryStream ms = new MemoryStream(parser.FileContents);
                            if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["DataBank"] + "KB\\MobileUplodedFile\\" + id))
                            {
                                System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["DataBank"] + "KB\\MobileUplodedFile\\" + id);
                            }
                            string filePathname = ConfigurationManager.AppSettings["DataBank"] + "KB\\MobileUplodedFile\\" + id + "\\" + objKnowledge.KnowledegeFileName;
                            FileStream FSfile = new FileStream(filePathname, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(FSfile);
                            FSfile.Close();
                            ms.Close();
                        }
                        objResponseData.Status = "1";
                        objResponseData.Message = "Saved successfully.";
                    }
                }
                else
                {
                    objResponseData.Status = "0";
                    objResponseData.Message = "Failed to save data.";
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// Method used to get Knowledgebase by Id 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return list of knowledgebase</returns>
        public string doGetKnowledgeBaseById(HttpContext context)
        {
            List<KnowledgeBaseBLL> ListKnowledgeBaseBLL = new List<KnowledgeBaseBLL>();
            ResponseData objResponseData = new ResponseData();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            int KbId = 0;
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    KbId = Convert.ToInt32(sData["KbId"]);
                    ListKnowledgeBaseBLL = KnowledgeBaseBLL.view("Get1", KbId);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                }
                //listHolidays = Holiday.GetHolidayDetails("SelectYearlyHolidays", 0);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }

            KnowledgeList objKnowledgeList = new KnowledgeList();
            List<Knowledge> ListKnowledge = new List<Knowledge>();

            //#region fill News object
            //if (ListKnowledgeBaseBLL != null && ListKnowledgeBaseBLL.Count > 0)
            //{
            //    foreach (KnowledgeBaseBLL objItem in ListKnowledgeBaseBLL)
            //    {
            //        List<KnowledgeTag> ListKnowledgeTag = new List<KnowledgeTag>();
            //        if (!string.IsNullOrEmpty(objItem.subtechName))
            //        {
            //            string[] Tags = objItem.subtechName.Split(',');
            //            for (int i = 0; i < Tags.Length; i++)
            //            {
            //                KnowledgeTag objKnowledgeTag = new KnowledgeTag();
            //                objKnowledgeTag.TagName = Convert.ToString(Tags[i]);
            //                ListKnowledgeTag.Add(objKnowledgeTag);
            //            }
            //        }
            //        Knowledge objKnowledge = new Knowledge();
            //        //objLeaveDetails.ID = objItem.ID;
            //        objKnowledge.ID = objItem.kbId;
            //        objKnowledge.KnowledgePostedBy = objItem.empName;
            //        objKnowledge.KnowledgePostedDate = Convert.ToDateTime(objItem.kbDate).ToString("dd MMM yyyy");
            //        objKnowledge.KnowledgeProjectTitle = objItem.projName;
            //        objKnowledge.KnowledgeTitle = objItem.kbTitle;
            //        objKnowledge.KnowledgeTechnology = objItem.techName;
            //        objKnowledge.KnowledgeTagList = ListKnowledgeTag;
            //        ListKnowledge.Add(objKnowledge);
            //    }
            //}
            //#endregion
            //objKnowledgeList.KnowledgeData = ListKnowledge;
            return (new JavaScriptSerializer().Serialize(ListKnowledgeBaseBLL));
        }
        /// <summary>
        /// Method used to update Knowledgebase
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return message success or failed</returns>
        public string doUpdateKnowledgebase(HttpContext context)
        {
            KnowledgeBaseBLL objKnowledgeBaseBLL = new KnowledgeBaseBLL();
            List<KnowledgeBaseBLL> ListKnowledgeBaseBLL = new List<KnowledgeBaseBLL>();
            ResponseData objResponseData = new ResponseData();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            string result = string.Empty;
            int EmpId = 0;
            string kbDescrptn = string.Empty;
            string kbComments = string.Empty;
            string kbFile = string.Empty;
            string kbTitle = string.Empty;
            int techId = 0;
            int projId = 0;
            string url = string.Empty;
            string subtechname = string.Empty;
            int id = 0;
            DataRow[] dr;
            string strEmpName = string.Empty;
            try
            {

                //Initialise an Object of MultipartParser Class With Requested Stream                   
                ClsMultipartParser parser = new ClsMultipartParser(context.Request.InputStream);
                var objKnowledge = new Knowledge();

                //Check that we have not null value in requested stream  

                if (parser != null && parser.Success)
                {
                    foreach (var item in parser.MyContents)
                    {
                        //Check our requested fordata                           
                        if (item.PropertyName.Contains("KbId"))
                        {
                            objKnowledge.ID = Convert.ToInt32(item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace(@"\", "").Replace(" ", "").Replace(",", "").Replace("{", "").Replace("}", "").Replace("\n", ""));
                        }
                        else if (item.PropertyName.Contains("KbDescrptn"))
                        {
                            objKnowledge.KnowledgeDescription = item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace("{", "").Replace("}", "").Replace("\n", "");
                        }
                        else if (item.PropertyName.Contains("KbTitle"))
                        {
                            objKnowledge.KnowledgeProjectTitle = item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace("{", "").Replace("}", "").Replace("\n", "");
                        }
                        else if (item.PropertyName.Contains("TechId"))
                        {
                            objKnowledge.KnowledegeTechnologyId = Convert.ToInt32(item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace(@"\", "").Replace(" ", "").Replace(",", "").Replace("{", "").Replace("}", "").Replace("\n", ""));
                        }
                        else if (item.PropertyName.Contains("Url"))
                        {
                            objKnowledge.KnowledgeUrl = item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace("{", "").Replace("}", "").Replace("\n", "");
                        }
                        else if (item.PropertyName.Contains("SubTechName"))
                        {
                            objKnowledge.KnowledegeSubTechName = item.StringData.Replace('"', '@').Replace("@", "").Replace("\r", "").Replace("{", "").Replace("}", "").Replace("\n", "");
                        }
                    }
                    if (!string.IsNullOrEmpty(parser.Filename))
                    {
                        objKnowledge.KnowledegeFileName = parser.Filename;
                    }
                    KnowledgeBaseBLL.UpdateKB("Update", objKnowledge.ID, objKnowledge.KnowledgeProjectTitle, kbComments, objKnowledge.KnowledgeDescription, objKnowledge.KnowledegeTechnologyId, objKnowledge.KnowledgeUrl, objKnowledge.KnowledegeSubTechName, objKnowledge.KnowledegeFileName);
                    ListKnowledgeBaseBLL = KnowledgeBaseBLL.view("Get1", objKnowledge.ID);
                    if (ListKnowledgeBaseBLL != null)
                    {
                        if (ListKnowledgeBaseBLL.Count > 0)
                        {
                            if (ListKnowledgeBaseBLL[0].empName != null)
                            {
                                strEmpName = Convert.ToString(ListKnowledgeBaseBLL[0].empName);
                                SendKnowledgeBaseMail("Update", objKnowledge.KnowledgeProjectTitle, strEmpName, objKnowledge.KnowledgeDescription, context);
                            }
                        }
                    }
                    if (parser.FileContents != null)
                    {

                        MemoryStream ms = new MemoryStream(parser.FileContents);
                        if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["DataBank"] + "KB\\MobileUplodedFile\\" + objKnowledge.ID))
                        {
                            System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["DataBank"] + "KB\\MobileUplodedFile\\" + objKnowledge.ID);
                        }
                        string filePathname = ConfigurationManager.AppSettings["DataBank"] + "KB\\MobileUplodedFile\\" + objKnowledge.ID + "\\" + objKnowledge.KnowledegeFileName;
                        FileStream FSfile = new FileStream(filePathname, FileMode.Create, FileAccess.Write);
                        ms.WriteTo(FSfile);
                        FSfile.Close();
                        ms.Close();
                    }
                    objResponseData.Status = "1";
                    objResponseData.Message = "Updated successfully.";
                }
                else
                {
                    objResponseData.Status = "0";
                    objResponseData.Message = "Failed to update data.";
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        /// <summary>
        /// Method used to get document
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return base64 string of document</returns>
        public string doGetDocument(HttpContext context)
        {
            ResponseData objResponseData = new ResponseData();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            string result = string.Empty;
            string strCode = string.Empty;
            string strBase64 = string.Empty;
            string AppPath = string.Empty;
            string FullPath = string.Empty;
            int EmpId = 0;
            DocumentData objDocumentData = new DocumentData();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    strCode = Convert.ToString(sData["Code"]);
                    if (sData.Count > 1 && sData["EmpId"] != null)
                    {
                        EmpId = Convert.ToInt32(sData["EmpId"]);
                    }
                    if (string.Compare(strCode, Code.HRPolicyCode, true) == 0)
                    {
                        FullPath = AppPath + ConfigurationManager.AppSettings["HRPolicy"].ToString();
                        strBase64 = doGetBase64ofFile(FullPath);
                    }
                    else if (string.Compare(strCode, Code.MediclaimPolicyCode, true) == 0)
                    {
                        FullPath = AppPath + ConfigurationManager.AppSettings["MediclaimPolicy"].ToString();
                        strBase64 = doGetBase64ofFile(FullPath);
                    }
                    else if (string.Compare(strCode, Code.ASHPolicyCode, true) == 0)
                    {
                        FullPath = AppPath + ConfigurationManager.AppSettings["ASHPolicy"].ToString();
                        strBase64 = doGetBase64ofFile(FullPath);
                    }
                    else if (string.Compare(strCode, Code.ProfileImageCode, true) == 0)
                    {
                        FullPath = ConfigurationManager.AppSettings["DataBank"] + "mobileprofileimage\\" + EmpId + ".jpg";
                        strBase64 = doGetBase64ofFile(FullPath, EmpId);
                    }
                    objDocumentData.DocData = strBase64;
                }
                else
                {
                    objDocumentData.DocData = strBase64;
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request.";
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objDocumentData));
        }
        public string doGetBase64ofFile(string filepath, int EmpId = 0)
        {
            string Base64String = string.Empty;
            if (File.Exists(filepath))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filepath);
                Base64String = Convert.ToBase64String(bytes);
            }
            return Base64String;
        }
        private static string SendKnowledgeBaseMail(string mode, string strKBTitle, string strKBempname, string strKBDescription, HttpContext context)
        {
            string output = "Could not send mail";
            string mailSubject = "";

            System.IO.StreamReader DynamicFileReader = null;
            string fileContent = null;
            if (mode == "Insert")
            {
                mailSubject = "Agora - New Knowledge Byte Added in KnowledgeBase";
                //DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"../MailTemplates/KbInsertTemplate.htm")); //Commented By Nikhil
                DynamicFileReader = System.IO.File.OpenText(context.Server.MapPath("//MailTemplates//KbInsertTemplate.htm"));
            }
            else
            {
                mailSubject = "Agora - Knowledge Byte Updated in KnowledgeBase";
                //DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"../MailTemplates/KbUpdateTemplate.htm")); //Commented By Nikhil
                DynamicFileReader = System.IO.File.OpenText(context.Server.MapPath("//MailTemplates//KbUpdateTemplate.htm"));
            }
            fileContent = DynamicFileReader.ReadToEnd();
            fileContent = fileContent.Replace("{empname}", strKBempname);
            fileContent = fileContent.Replace("{title}", strKBTitle);
            fileContent = fileContent.Replace("{description}", strKBDescription);
            string strUrl = "<a href=http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/Member/Login.aspx>Click here.</a>";
            fileContent = fileContent.Replace("{url}", strUrl);
            output = CSCode.Global.SendKBMail(fileContent, mailSubject, "web@intelgain.com", ConfigurationManager.AppSettings.Get("fromEmail"), true);//ConfigurationManager.AppSettings.Get("CommonEmail")

            return output;

        }

        //End Nikhil Shetye

        # region "Compute description logic"
        /// <summary>
        /// Method is used to compute the description of the employee attanance by date
        /// </summary>
        /// <param name="timesheethours"></param>
        /// <param name="attendanceDate"></param>
        /// <returns>Return description</returns>
        public string computeDescription(string timesheethours, DateTime attendanceDate)
        {
            List<Holiday> lstHoliday = Holiday.GetHolidayBydate("GetHolidayBydate", attendanceDate);
            if (!string.IsNullOrEmpty(timesheethours))
            {
                return timesheethours = timesheethours.Substring(0, timesheethours.IndexOf("s") + 1);
            }
            else if (lstHoliday.Count > 0)
            {
                return lstHoliday[0].Narration;
            }
            //else if (Convert.ToString(attendanceDate.DayOfWeek) == "Sunday" || attendanceDate == GetSaturdayByWeek(attendanceDate, 2) || attendanceDate == GetSaturdayByWeek(attendanceDate, 4))
            //{
            //    return "Weekend";
            //}
            else if (Convert.ToString(attendanceDate.DayOfWeek) == "Sunday" || Convert.ToString(attendanceDate.DayOfWeek) == "Saturday")
            {
                return "Weekend";
            }
            else
            {
                return "Pending";
            }
        }

        /// <summary>
        /// Getting the second satarday and four satarday
        /// </summary>
        /// <param name="dateofMonth"></param>
        /// <param name="weekNumber"></param>
        /// <returns></returns>
        private static DateTime GetSaturdayByWeek(DateTime dateofMonth, int weekNumber)
        {
            DateTime firstDateofMonth = new DateTime(dateofMonth.Year, dateofMonth.Month, 1);
            DateTime resultDate = System.Globalization.CultureInfo.InvariantCulture.Calendar.AddWeeks(firstDateofMonth, weekNumber - 1);
            int day = Convert.ToInt32(resultDate.DayOfWeek) < 6 ? (Convert.ToInt32(resultDate.DayOfWeek) - 6) * -1 : 0;
            return resultDate.AddDays(day);
        }
        #endregion
        public string doInsertLateComing(HttpContext context)
        {
            ResponseData objResponseData = new ResponseData();
            LateComing objLateComing = new LateComing();
            DataTable dtLateComing = new DataTable();
            objResponseData.Status = "0";
            objResponseData.Message = "Execution complete";
            int EmpCode = 0;
            DateTime ApplyDate = DateTime.Now;
            string ExpectedInTime = string.Empty;
            string LateCommingReason = string.Empty;
            DateTime? ApprovedOn = null;
            int? ApprovedBy = null;
            string ApprovalComment = null;
            int IsApproveStatus = 0;

            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpCode = Convert.ToInt32(sData["EmpCode"]);
                    ApplyDate = Convert.ToDateTime(sData["ApplyDate"]);
                    ExpectedInTime = Convert.ToString(sData["ExpectedInTime"]);
                    LateCommingReason = Convert.ToString(sData["LateCommingReason"]);
                    dtLateComing = LateComing.InsertLateComing(EmpCode, ApplyDate, ExpectedInTime, LateCommingReason, ApprovedOn, ApprovedBy, ApprovalComment, IsApproveStatus);
                    if (dtLateComing.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtLateComing.Rows)
                        {
                            objResponseData.Status = Convert.ToString(dr["Status"]);
                            objResponseData.Message = Convert.ToString(dr["Message"]);
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }

        public string doGetWFHDetails(HttpContext context)
        {
            int EmpID = 0;
            string WFHStatus = string.Empty;
            int year = 0;
            EmpWFHBAL objEmpWFHBLL = new EmpWFHBAL();
            DataSet DsEmpWFHDetails = null;
            DataTable DtWorkFromHomeDetails = null;
            List<EmpWFHBAL> lstEmpWFHBLLDetails = new List<EmpWFHBAL>();
            EmployeeWFHDetails objemployeeWFHDetails = new EmployeeWFHDetails();
            WorkFromHomeDetails objWorkFromHomeDetails = new WorkFromHomeDetails();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    WFHStatus = Convert.ToString(sData["WFHStatus"]);
                    year = Convert.ToInt32(sData["year"]);

                    DsEmpWFHDetails = objEmpWFHBLL.GetWFHDetails(EmpID, WFHStatus, year);
                    DtWorkFromHomeDetails = objEmpWFHBLL.BindWFHBalance(EmpID);
                    if (DtWorkFromHomeDetails !=null && DtWorkFromHomeDetails.Rows.Count > 0)
                    {
                        objWorkFromHomeDetails = new WorkFromHomeDetails
                        {
                            TotalAnnual =Convert.ToInt32(DtWorkFromHomeDetails.Rows[0]["Total"]),
                            TotalTillCurrentDate =Convert.ToInt32(DtWorkFromHomeDetails.Rows[0]["Total_Accrual"]),
                            Consumed =Convert.ToInt32(DtWorkFromHomeDetails.Rows[0]["Consumed"]),
                            Balance =Convert.ToInt32(DtWorkFromHomeDetails.Rows[0]["Balance"]),
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
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";

                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(objemployeeWFHDetails));
        }
        public string doSaveWFHDetails(HttpContext context)
        {
            bool WFHExist = false;
            int empId = 0;
            string fromDate = string.Empty;
            string toDate = string.Empty;
            string reason = string.Empty;
            double days = 0;
            int balance = 0;
            DataSet ds = new DataSet();
            ResponseData objResponseData = new ResponseData();
            EmpWFHBAL objEmpWFHBLL = new EmpWFHBAL();
            EmpWFHBLL objEmpWFHBAL = new EmpWFHBLL();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> data = jss.Deserialize<Dictionary<string, string>>(json);
                    empId = Convert.ToInt32(data["EmpId"]);
                    fromDate = data["From"];
                    toDate = data["To"];
                    reason = data["Reason"];
                    DateTime dtFrom, dtTo;
                    string dateFormat = "dd/MM/yyyy";
                    var culture = System.Globalization.CultureInfo.InvariantCulture;

                    if (!DateTime.TryParseExact(fromDate, dateFormat, culture, System.Globalization.DateTimeStyles.None, out dtFrom) ||
                        !DateTime.TryParseExact(toDate, dateFormat, culture, System.Globalization.DateTimeStyles.None, out dtTo))
                    {
                        objResponseData.Status = "0";
                        objResponseData.Message = "Invalid date format. Please use dd/MM/yyyy.";
                        context.Response.StatusCode = 400;
                        context.Response.StatusDescription = "Bad Request - Invalid date.";
                        return (new JavaScriptSerializer().Serialize(objResponseData));
                    }

                    days = (dtTo - dtFrom).TotalDays + 1;
                    WFHExist = EmpWFHBAL.IfExistsWFH("IFEXISTSWFH", empId, fromDate, toDate);
                    var datatable = objEmpWFHBAL.BindWFHBalance(empId);
                    if (datatable !=null && datatable.Rows.Count>0)
                    {
                        balance =Convert.ToInt32(datatable.Rows[0]["Balance"]);
                    }
                    if (WFHExist)
                    {
                        objResponseData.Status = "0";
                        objResponseData.Message = "Work From Home for same date already exists.";
                    }
                    else if (days > 2 || balance == 0 )
                    {
                        objResponseData.Status = "0";
                        objResponseData.Message = "Insufficient WFH balance";
                    }
                    else
                    {
                        ds = objEmpWFHBLL.SaveWFH(empId, fromDate, toDate, reason, Convert.ToInt32(days));
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            objEmpWFHBAL.SendMail(ds.Tables[0], fromDate, toDate, reason);
                            objResponseData.Status = "1";
                            objResponseData.Message = "Work From Home saved succesfully.";
                        }
                        else
                        {
                            objResponseData.Status = "0";
                            objResponseData.Message = "Work From Home save failed.";
                            context.Response.StatusCode = 400;
                            context.Response.StatusDescription = "Bad Request.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
                objResponseData.Status = "0";
                objResponseData.Message = "Work From Home save failed.";

            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        public string doRemoveWFH(HttpContext context)
        {
            int Id = 0;
            int count = 0;
            string status = string.Empty;
            ResponseData objResponseData = new ResponseData();
            EmpWFHBAL objEmpWFHBLL = new EmpWFHBAL();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> data = jss.Deserialize<Dictionary<string, string>>(json);
                    Id = Convert.ToInt32(data["WFHId"]);
                    var dt=objEmpWFHBLL.GetWFHDetailById(Id);
                    if (dt!=null && dt.Rows.Count>0)
                    {
                        status =Convert.ToString(dt.Rows[0]["WFHStatus"]);
                    }
                    if (string.Equals(status, "p", StringComparison.OrdinalIgnoreCase))
                    {
                        count = objEmpWFHBLL.DeleteWFHById(Id);
                        if (count > 0)
                        {
                            objResponseData.Status = "1";
                            objResponseData.Message = "WFH request deleted successfully.";
                        }
                        else
                        {
                            objResponseData.Status = "0";
                            objResponseData.Message = "Failed to delete the WFH request.";
                            context.Response.StatusCode = 404;
                            context.Response.StatusDescription = "Not found.";
                        } 
                    }
                    else
                    {
                        objResponseData.Status = "0";
                        objResponseData.Message = "WFH request cannot be deleted. Only pending request can be removed.";
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";

                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";

            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        public string doProcessAttendanceRequest(HttpContext context)
        {
            int recordCount = 0;
            int employeeId = 0;
            string actionMode = string.Empty;
            bool isWFHExist = false;
            DateTime attendanceOutTime;
            string attendanceDate;
            string status;
            DataTable dataTable = new DataTable();
            ResponseData response = new ResponseData();
            EmpWFHBAL attendanceService = new EmpWFHBAL();

            try
            {
                var serializer = new JavaScriptSerializer();
                string requestBody = new StreamReader(context.Request.InputStream).ReadToEnd();

                if (!string.IsNullOrEmpty(requestBody))
                {
                    Dictionary<string, string> requestData = serializer.Deserialize<Dictionary<string, string>>(requestBody);
                    employeeId = Convert.ToInt32(requestData["EmpId"]);
                    actionMode = Convert.ToString(requestData["Mode"]);
                    attendanceDate = requestData["AttDate"];

                    var dt = EmpWFHBAL.IfExistsWFHData("IFEXISTSWFH", employeeId, attendanceDate, attendanceDate);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        status = Convert.ToString(dt.Rows[0]["WFHStatus"]);

                        if (string.Equals(status,"p", StringComparison.OrdinalIgnoreCase) || string.Equals(status, "r", StringComparison.OrdinalIgnoreCase))
                        {
                            response.Status = "0";
                            response.Message = "Work From Home (WFH) request must be approved before proceeding.";
                            return serializer.Serialize(response); 
                        }
                    }
                    if (!string.IsNullOrEmpty(actionMode))
                    {
                        isWFHExist = EmpWFHBAL.IfExistsWFH("IFEXISTSWFH", employeeId, attendanceDate, attendanceDate);

                        if (!isWFHExist)
                        {
                            response.Status = "0";
                            response.Message = "Work From Home (WFH) record does not exist.";
                            return serializer.Serialize(response);
                        }

                        if (string.Equals(actionMode, "Insert", StringComparison.OrdinalIgnoreCase)) // Insert attendance
                        {
                            bool hasCheckedIn = EmpWFHBAL.CheckInTime("CheckInTime", employeeId);

                            if (hasCheckedIn)
                            {
                                response.Status = "0";
                                response.Message = "You have already recorded your check-in time.";
                                return serializer.Serialize(response);
                            }

                            recordCount = attendanceService.InsertWFHAttendance(employeeId);

                            response.Status = recordCount > 0 ? "1" : "0";
                            response.Message = recordCount > 0 ? "Attendance recorded successfully." : "Failed to record attendance.";

                            if (recordCount == 0)
                            {
                                context.Response.StatusCode = 404;
                                context.Response.StatusDescription = "Not Found";
                            }
                        }
                        else if (string.Equals(actionMode, "Update", StringComparison.OrdinalIgnoreCase)) // Update attendance
                        {
                            attendanceOutTime = DateTime.Now;
                            recordCount = attendanceService.UpdateWFHAttendance(employeeId, attendanceOutTime, Convert.ToDateTime(attendanceDate));

                            response.Status = recordCount > 0 ? "1" : "0";
                            response.Message = recordCount > 0 ? "Attendance updated successfully." : "Failed to update attendance.";

                            if (recordCount == 0)
                            {
                                context.Response.StatusCode = 404;
                                context.Response.StatusDescription = "Not Found";
                            }
                        }
                        else
                        {
                            response.Status = "0";
                            response.Message = "Invalid action. 'Mode' parameter is required.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal Server Error";
                response.Status = "0";
                response.Message = "An error occurred while processing attendance.";
            }

            return new JavaScriptSerializer().Serialize(response);
        }

        public string doUpdateAttendance(HttpContext context)
        {
            int count = 0;
            int empId = 0;
            DateTime attOutTime;
            DateTime attDate;
            DataTable dt = new DataTable();
            ResponseData objResponseData = new ResponseData();
            EmpWFHBAL objEmpWFHBLL = new EmpWFHBAL();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> data = jss.Deserialize<Dictionary<string, string>>(json);
                    empId = Convert.ToInt32(data["EmpId"]);
                    attOutTime = Convert.ToDateTime(data["AttOutTime"]);
                    attDate = Convert.ToDateTime(data["AttDate"]);
                    count = objEmpWFHBLL.UpdateWFHAttendance(empId, attOutTime, attDate);
                    if (count > 0)
                    {
                        objResponseData.Status = "1";
                        objResponseData.Message = "Attendance updated successfully";
                    }
                    else
                    {
                        objResponseData.Status = "0";
                        objResponseData.Message = "Attendance updated failed.";
                        context.Response.StatusCode = 404;
                        context.Response.StatusDescription = "Not found.";
                    }
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
                objResponseData.Status = "0";
                objResponseData.Message = "Attendance updated failed.";

            }
            return (new JavaScriptSerializer().Serialize(objResponseData));
        }
        public string doGetWFHFromtoTo(HttpContext context)
        {

            int EmpID = 0;
            string WFHFrom = string.Empty;
            string WFHTo = string.Empty;
            EmpWFHBAL objEmpWFHBLL = new EmpWFHBAL();
            DataSet ds = null;
            List<EmpWFHAttendance> lstEmpWFHAttendance = new List<EmpWFHAttendance>();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    EmpID = Convert.ToInt32(sData["EmpId"]);
                    WFHFrom = Convert.ToString(sData["WFHFrom"]);
                    WFHTo = Convert.ToString(sData["WFHTo"]);

                    if (!string.IsNullOrEmpty(WFHFrom) && !string.IsNullOrEmpty(WFHTo))
                    {
                        WFHFrom = Convert.ToDateTime(WFHFrom).ToString("yyyy/MM/dd");
                        WFHTo = Convert.ToDateTime(WFHTo).ToString("yyyy/MM/dd");
                        ds = objEmpWFHBLL.AppliedWFHFromTo(EmpID, WFHFrom, WFHTo, "AppliedWFHFromToAPI");
                    }
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EmpWFHAttendance objEmpWFH = new EmpWFHAttendance();
                            objEmpWFH.AttDate = Convert.ToString(dr["attDate"]);
                            objEmpWFH.Status = Convert.ToString(dr["attStatus"]);
                            objEmpWFH.AttInTime = Convert.ToString(dr["attInTime"]);
                            objEmpWFH.AttOutTime = Convert.ToString(dr["attOutTime"]);
                            objEmpWFH.Day = Convert.ToString(dr["Day"]);
                            objEmpWFH.CheckDate = DateTime.Now;
                            lstEmpWFHAttendance.Add(objEmpWFH);
                        }
                    }
                    else
                    {
                        lstEmpWFHAttendance = null;
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";

                }
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
            }
            return (new JavaScriptSerializer().Serialize(lstEmpWFHAttendance));
        }
        public string GetUserIdentity(HttpContext context)
        {
            string UserId = "";
            UserIdentityData objUserData = new UserIdentityData();
            ResponseData objResponseData = new ResponseData();
            try
            {
                var jss = new JavaScriptSerializer();
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {

                    Dictionary<string, string> sData = jss.Deserialize<Dictionary<string, string>>(json);
                    UserId = Convert.ToString(sData["UserIdentity"]);
                    var Id = UserId.Split('|');
                    if (string.Compare("SkypeId", Id[0], true) == 0)
                    {
                        objUserData = EmployeeMaster.GetEmpDetailsbyId(Id[1], UserIdentity.SkypeId.ToString());
                    }
                    else if (string.Compare("MSTeam", Id[0], true) == 0)
                    {
                        objUserData = EmployeeMaster.GetEmpDetailsbyId(Id[1], UserIdentity.MSTeam.ToString());
                    }
                    else if (string.Compare("EmpEmail", Id[0], true) == 0)
                    {
                        objUserData = EmployeeMaster.GetEmpDetailsbyId(Id[1], UserIdentity.EmpEmail.ToString());
                    }
                    if (!objUserData.IsSuccess)
                    {
                        context.Response.StatusCode = 400;
                        return (new JavaScriptSerializer().Serialize("Request for useridentity is unverified"));
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad Request";
                    objUserData.IsSuccess = false;
                    objUserData.StatusMessage = "useridentity is requried";
                    return (new JavaScriptSerializer().Serialize(objUserData));

                }
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal server Error";
                return (new JavaScriptSerializer().Serialize("Request for useridentity is requried"));

            }
            return (new JavaScriptSerializer().Serialize(objUserData));
        }
    }
    /// <summary>
    /// This class is used for  capture the response data.
    /// </summary>
    public class ResponseData
    {
        [DataMember]
        public string Status = string.Empty;
        [DataMember]
        public string Message = string.Empty;
    }
    public class LeaveType
    {
        public int ID { get; set; }
        public int TotalCL { get; set; }
        public int TotalSL { get; set; }
        public int TotalPL { get; set; }
        public int TotalCO { get; set; }
        public int ConsumedCL { get; set; }
        public int ConsumedSL { get; set; }
        public int ConsumedPL { get; set; }
        public int ConsumedCO { get; set; }
        public int BalanceCL { get; set; }
        public int BalanceSL { get; set; }
        public int BalancePL { get; set; }
        public int BalanceCO { get; set; }
        public int TotalCLTillDate { get; set; }
        public int TotalSLTillDate { get; set; }
        public int TotalPLTillDate { get; set; }
        public int TotalCOTillDate { get; set; }
    }
    public class LeaveDetails
    {
        public int ID { get; set; }
        public string LeaveType { get; set; }
        public string LeaveFrom { get; set; }
        public string LeaveTo { get; set; }
        public string LeaveDesc { get; set; }
        public string LeaveStatus { get; set; }
        public string AdminComments { get; set; }
        public string LeaveReason { get; set; }
    }
    public class EmpLeaveDetails
    {
        public int FiscalYear { get; set; }
        public LeaveType LeaveType { get; set; }
        public List<LeaveDetails> LeaveData { get; set; }
    }
    public class Attendance
    {
        public string AttInTime { get; set; }
        public string AttOutTime { get; set; }
        public string AttStatus { get; set; }
        public string AttendanceDate { get; set; }
        public string Date { get; set; }
        public string Day { get; set; }
        public string Description { get; set; }
        public string HolidayImageUrl { get; set; }
        public long ImageId { get; set; }
        public string RowBgColor { get; set; }
        public string RowTextColor { get; set; }
        public string Timesheethours { get; set; }
        public long ViewType { get; set; }
        public string Workinghours { get; set; }
    }
    public class EmpAttendanceDetails
    {
        public List<Attendance> AttendanceData { get; set; }
    }
    public class Occasion
    {
        public string OccassionDate { get; set; }
        public string OccassionName { get; set; }
        public string OccassionFor { get; set; }
        public string OccasionImageUrl { get; set; }
    }
    public class News
    {
        public string NewsDate { get; set; }
        public string NewsTitle { get; set; }
    }
    public class CIPSession
    {
        public string Date { get; set; }
        public string Technology { get; set; }
        public string Content { get; set; }
    }
    public class DashboardAttendance
    {
        public string EventDate { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string OfficeHrs { get; set; }
        public string TSHrs { get; set; }
    }
    public class Dashboard
    {
        public List<Occasion> OccassionsData { get; set; }
        public List<News> News { get; set; }
        public List<CIPSession> CIPSessions { get; set; }
        //public List<DashboardAttendance> DashboardData { get; set; } //Commented by Nikhil Shetye on 27/11/2017
        public List<Attendance> DashboardData { get; set; } //Added by Nikhil Shetye on 27/11/2017
        public string HRManualContentCode { get; set; }
        public string MediclaimPolicyContentCode { get; set; }
        public string AntiSexualPolicyContentCode { get; set; }
        public string ProfileImageContentCode { get; set; }
    }
    public class DashboardHoliday
    {
        public string HolidayBgColor { get; set; }
        public string HolidayDay { get; set; }
        public string HolidayImageUrl { get; set; }
        public string HolidayMonth { get; set; }
        public string HolidayName { get; set; }
        public long ImageId { get; set; }
    }
    public class ListHoliday
    {
        public List<DashboardHoliday> HolidayList { get; set; }
    }
    public class Knowledge
    {
        public int ID { get; set; }
        public string KnowledgePostedBy { get; set; }
        public string KnowledgePostedDate { get; set; }
        public string KnowledgeProjectTitle { get; set; }
        public List<KnowledgeTag> KnowledgeTagList { get; set; }
        public string KnowledgeTechnology { get; set; }
        public string KnowledgeTitle { get; set; }
        //Newly Added By Nikhil Shetye
        public string KnowledgeDescription { get; set; }
        public string KnowledgeUrl { get; set; }
        public int KnowledeProjectId { get; set; }
        public int KnowledegeTechnologyId { get; set; }
        public string KnowledegeSubTechName { get; set; }
        public string KnowledegeFileName { get; set; }
        public int EmpId { get; set; }
        //End Nikhil Shetye  
    }
    public class KnowledgeTag
    {
        public string TagName { get; set; }
    }
    public class KnowledgeList
    {
        public List<Knowledge> KnowledgeData { get; set; }
    }
    public class TimeSheetList
    {
        public List<TimeSheet> timeSheetData { get; set; }
    }
    public class Employee
    {
        public int empid { get; set; }
        public string empName { get; set; }
        public string empAddress { get; set; }
        public string empGender { get; set; }
        public string empContact { get; set; }
        public DateTime? empJoiningDate { get; set; }
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

        public string Flag { get; set; } ////For device use
        public string strBirthday { get; set; } ////For device use
    }
    public class EmployeeContact
    {
        public int empid { get; set; }
        public string empName { get; set; }
        public string empContact { get; set; }
        public string empEmail { get; set; }
        public string Designation { get; set; }
        public string ProfileImageURL { get; set; }
    }
    //Added By Nikhil Shetye on 15/11/2017
    public class ProjectList
    {
        public List<Project> ProjectData { get; set; }
    }
    public class Project
    {
        public int projId { get; set; }
        public string projName { get; set; }
        public int custId { get; set; }
        public string projDesc { get; set; }
    }
    public class TechnologyList
    {
        public List<Technology> TechnologyData { get; set; }
    }
    public class Technology
    {
        public int techId { get; set; }
        public string techName { get; set; }
        public string subtechName { get; set; }
    }
    public class ClsMultipartParser
    {
        private byte[] requestData;

        //Require Namespace: using System.IO;
        public ClsMultipartParser(Stream stream)
        {
            //Require Namespace: using System.Text;
            this.Parse(stream, Encoding.UTF8);
            ParseParameter(stream, Encoding.UTF8);
        }

        public ClsMultipartParser(Stream stream, Encoding encoding)
        {
            this.Parse(stream, encoding);
        }

        private void Parse(Stream stream, Encoding encoding)
        {
            this.Success = false;

            // Read the stream into a byte array
            byte[] data = ToByteArray(stream);
            requestData = data;

            // Copy to a string for header parsing
            string content = encoding.GetString(data);

            // The first line should contain the delimiter
            int delimiterEndIndex = content.IndexOf("\r\n");

            if (delimiterEndIndex > -1)
            {
                string delimiter = content.Substring(0, content.IndexOf("\r\n"));
                //string[] splitContents = content.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

                //foreach (string t in splitContents)
                //{
                //Require Namespace: using System.Text.RegularExpressions;
                // Look for Content-Type
                Regex re = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
                Match contentTypeMatch = re.Match(content);


                // Look for filename
                re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
                Match filenameMatch = re.Match(content);

                // Did we find the required values?
                if (contentTypeMatch.Success && filenameMatch.Success)
                {
                    // Set properties
                    this.ContentType = contentTypeMatch.Value.Trim();
                    this.Filename = filenameMatch.Value.Trim();

                    // Get the start & end indexes of the file contents
                    int startIndex = contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;

                    byte[] delimiterBytes = encoding.GetBytes("\r\n" + delimiter);
                    int endIndex = IndexOf(data, delimiterBytes, startIndex);
                    int contentLength = endIndex - startIndex;

                    // Extract the file contents from the byte array
                    byte[] fileData = new byte[contentLength];

                    Buffer.BlockCopy(data, startIndex, fileData, 0, contentLength);
                    this.FileContents = fileData;
                    this.Success = true;
                }
                //}
            }
        }

        private void ParseParameter(Stream stream, Encoding encoding)
        {
            this.Success = false;

            // Read the stream into a byte array
            byte[] data;
            if (requestData.Length == 0)
            {
                data = ToByteArray(stream);
            }
            else { data = requestData; }
            // Copy to a string for header parsing
            string content = encoding.GetString(data);

            // The first line should contain the delimiter
            int delimiterEndIndex = content.IndexOf("\r\n");

            if (delimiterEndIndex > -1)
            {
                string delimiter = content.Substring(0, content.IndexOf("\r\n"));
                string[] splitContents = content.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string t in splitContents)
                {
                    // Look for Content-Type
                    Regex contentTypeRegex = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
                    Match contentTypeMatch = contentTypeRegex.Match(t);

                    // Look for name of parameter
                    Regex re = new Regex(@"(?<=name\=\"")(.*)");
                    Match name = re.Match(t);

                    // Look for filename
                    re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
                    Match filenameMatch = re.Match(t);

                    // Did we find the required values?
                    if (name.Success || filenameMatch.Success)
                    {
                        // Set properties
                        //this.ContentType = name.Value.Trim();
                        int startIndex;
                        if (filenameMatch.Success)
                        {
                            this.Filename = filenameMatch.Value.Trim();
                        }
                        if (contentTypeMatch.Success)
                        {
                            // Get the start & end indexes of the file contents
                            startIndex = contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;
                        }
                        else
                        {
                            startIndex = name.Index + name.Length + "\r\n\r\n".Length;
                        }

                        //byte[] delimiterBytes = encoding.GetBytes("\r\n" + delimiter);
                        //int endIndex = IndexOf(data, delimiterBytes, startIndex);

                        //int contentLength = t.Length - startIndex;
                        string propertyData = t.Substring(startIndex - 1, t.Length - startIndex);
                        // Extract the file contents from the byte array
                        //byte[] paramData = new byte[contentLength];

                        //Buffer.BlockCopy(data, startIndex, paramData, 0, contentLength);

                        MyContent myContent = new MyContent();
                        myContent.Data = encoding.GetBytes(propertyData);
                        myContent.StringData = propertyData;
                        myContent.PropertyName = name.Value.Trim().TrimEnd('"');

                        if (MyContents == null)
                            MyContents = new List<MyContent>();

                        MyContents.Add(myContent);
                        this.Success = true;
                    }
                }
            }
        }

        private int IndexOf(byte[] searchWithin, byte[] serachFor, int startIndex)
        {
            int index = 0;
            int startPos = Array.IndexOf(searchWithin, serachFor[0], startIndex);

            if (startPos != -1)
            {
                while ((startPos + index) < searchWithin.Length)
                {
                    if (searchWithin[startPos + index] == serachFor[index])
                    {
                        index++;
                        if (index == serachFor.Length)
                        {
                            return startPos;
                        }
                    }
                    else
                    {
                        startPos = Array.IndexOf<byte>(searchWithin, serachFor[0], startPos + index);
                        if (startPos == -1)
                        {
                            return -1;
                        }
                        index = 0;
                    }
                }
            }
            return -1;
        }

        private byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public List<MyContent> MyContents { get; set; }

        public bool Success
        {
            get;
            private set;
        }

        public string ContentType
        {
            get;
            private set;
        }

        public string Filename
        {
            get;
            private set;
        }

        public byte[] FileContents
        {
            get;
            private set;
        }
    }
    public class MyContent
    {
        public byte[] Data { get; set; }
        public string PropertyName { get; set; }
        public string StringData { get; set; }
    }
    public class UploadImageEntity
    {
        public string ID { get; set; }
        public string UploadImageData { get; set; }
        public byte[] UploadImage { get; set; }
        private bool _RemovePhoto = false;
        public bool RemovePhoto
        {
            get { return _RemovePhoto; }
            set { _RemovePhoto = value; }
        }
    }
    public class SkillMatrix
    {
        public int SkillID { get; set; }
        public int CategoryId { get; set; }
        public int EmployeeSkillID { get; set; }
        public int EmpID { get; set; }
        public int UserID { get; set; }
        public int Experience { get; set; }
        public int Years { get; set; }
        public int Months { get; set; }
        public int EmpCount { get; set; }
        public string Mode { get; set; }
        public string Category { get; set; }
        public string SkillName { get; set; }
        public string Level { get; set; }
        public bool ActiveSkill { get; set; }
        public string Status { get; set; }
        public string EmpName { get; set; }
        public string MaxExperience { get; set; }
        public string InsertedDate { get; set; }
    }
    public class ListSkillMatrix
    {
        public List<SkillMatrix> lstEmpSkill { get; set; }
    }
    public static class Code
    {
        public static string HRPolicyCode = "HRPolicy";
        public static string MediclaimPolicyCode = "MediclaimPolicy";
        public static string ASHPolicyCode = "ASHPolicy";
        public static string ProfileImageCode = "ProfileImage";

    }
    public class DocumentData
    {
        public string DocData { get; set; }
    }
    public class KbAttachmentData
    {
        public string FileName { get; set; }
        public string FileData { get; set; }
    }
    public class KBAttachment
    {
        public List<KbAttachmentData> Attachments { get; set; }
    }
    public class EmployeeLeaveDetails
    {
        public int FiscalYear { get; set; }
        public List<EmpLeaveBLL> LeaveData { get; set; }
    }
    //End Nikhil Shetye
    public class EmployeeWFHDetails
    {
        public WorkFromHomeDetails WFHDetails { get; set; }
        public List<EmpWFHBAL> WFHListData { get; set; }
    }
    public class EmpWFHAttendance
    {
        public string AttDate { get; set; }
        public string AttInTime { get; set; }
        public string AttOutTime { get; set; }
        //public string AttComment { get; set; }
       // public string AdminID { get; set; }
        public string Day { get; set; }
        public string Status { get; set; }
        public DateTime CheckDate { get; set; } 
        // public string EntryDate { get; set; }
    }
    public class WorkFromHomeDetails
    {
        public int TotalAnnual { get; set; }
        public int TotalTillCurrentDate { get; set; }
        public int Consumed { get; set; }
        public int Balance { get; set; }
    }

    public class UserIdentityData
    {

        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public int? ProfileId { get; set; }
        public string EmpPassword { get; set; }
        public int? SkillId { get; set; }
        public string EmpAddress { get; set; }
        public string EmpContact { get; set; }
        public DateTime? EmpJoiningDate { get; set; }
        public DateTime? EmpLeavingDate { get; set; }
        public int? EmpProbationPeriod { get; set; }
        public string EmpNotes { get; set; }
        public string EmpEmail { get; set; }
        public int EmpTester { get; set; }
        public string EmpAccountNo { get; set; }
        public DateTime? EmpBDate { get; set; }
        public DateTime? EmpADate { get; set; }
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
        public DateTime InsertedOn { get; set; }
        public int InsertedBy { get; set; }
        public string InsertedIP { get; set; }
        public DateTime ModifiedOn { get; set; }
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
        public DateTime EmpForgotPwdLinkDate { get; set; }
        public string SkypeId { get; set; }
        public DateTime EmpExpectedLWD { get; set; }
        public string IFSCCode { get; set; }
        public decimal LeavePLOpeningBalance { get; set; }
        public string ProjectGroupEmail { get; set; }
        public string MSTeam { get; set; }
        private bool _IsSuccess = false;
        public bool IsSuccess
        {
            get { return _IsSuccess; }
            set { _IsSuccess = value; }
        }

        public string StatusMessage { get; set; }

    }
    public enum UserIdentity
    {
        SkypeId,
        MSTeam,
        EmpEmail
    }

}