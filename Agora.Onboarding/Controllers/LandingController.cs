using Agora.Onboarding.DAL;
using Agora.Onboarding.Encryption;
using Agora.Onboarding.Models;
using System.Web;
using System.Web.Mvc;
using System;
using Agora.Onboarding.filter;
using System.IO;
using System.Configuration;
using Agora.Onboarding.Common;
using System.Collections.Generic;
using System.Net;

namespace Agora.Onboarding.Controllers
{
    [CustomException()]
    public class LandingController : Controller
    {

        private readonly Onboardings onboarding;
        private readonly DbContext dbContext;
        private readonly EmployeeMaster employeeMaster;
        public LandingController()
        {
            onboarding = new Onboardings();
            dbContext = new DbContext();
            employeeMaster = new EmployeeMaster();
        }
        public ActionResult GetStart(string Emp_Id,string check)
        {
            Session["EmpId"] = Emp_Id;
            TempData["Ischeck"] = check;
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            string check = string.Empty;
            string emp_Id = string.Empty;
            if (Session["EmpId"]!=null)
            {
                emp_Id = Session["EmpId"].ToString();
            }
            if (TempData["Ischeck"]!=null)
            {
                check = TempData["Ischeck"].ToString();
            }
            if (emp_Id!="" && emp_Id!=null)
            {
                int? id = null;
                var decrypt_emp_id = string.Empty;
                if (!string.IsNullOrEmpty(emp_Id))
                {
                    decrypt_emp_id = Utility.Decrypt(emp_Id.Replace(' ', '+'));
                }

                if (!string.IsNullOrEmpty(emp_Id) && !string.IsNullOrEmpty(decrypt_emp_id))
                {
                    id = Convert.ToInt32(decrypt_emp_id);
                }
                Onboardings obj = null;
                if (id != null)
                {
                    var gettingOnborading = dbContext.GetOnboardingById(id).Find(x => x.EmpId == id);
                    obj = gettingOnborading;
                    Session["onboradObj"] = gettingOnborading;
                    var getEmployeeMasterList = dbContext.GetEmployeeMasterList(id).Find(x => x.EmpId == id);
                    ViewBag.getNameById = getEmployeeMasterList;
                    Session["getObjById"] = getEmployeeMasterList;

                }
                if (!string.IsNullOrEmpty(check))
                {
                    return View();
                }
                if (obj != null && !obj.IsCompleted)
                {
                    if (obj.EmpId == id && !string.IsNullOrEmpty(obj.Empname) && !(obj.Timesheet_check && obj.Leave_check && obj.WFH_check && obj.Confidentiality_check ) && !obj.HR_Manual_check)
                    {
                        return RedirectToAction("ReviewHRManual");
                    }
                    else if (obj.EmpId == id && !string.IsNullOrEmpty(obj.Empname) && !(obj.Timesheet_check && obj.Leave_check && obj.WFH_check && obj.Confidentiality_check) && obj.HR_Manual_check)
                    {
                        return RedirectToAction("AgoraLoginPortal");
                    }
                    else if (obj.EmpId == id && !string.IsNullOrEmpty(obj.Empname) && obj.Timesheet_check && obj.Leave_check && obj.WFH_check && obj.Confidentiality_check && obj.HR_Manual_check && !obj.ITInduction_check&&!obj.Form_check)
                    {
                        return RedirectToAction("AppointmentLetter");
                    }
                    else if (obj.EmpId == id && !string.IsNullOrEmpty(obj.Empname) && obj.Timesheet_check && obj.Leave_check && obj.WFH_check && obj.Confidentiality_check && obj.HR_Manual_check && obj.ITInduction_check&&obj.Form_check)
                    {
                        return RedirectToAction("WelldonePage");
                    }
                }
                if (obj != null && obj.IsCompleted)
                {
                    var url = System.Configuration.ConfigurationManager.AppSettings["AgoraURL"];
                    return Redirect(url);
                }
                if (obj == null)
                {
                    return View();
                }
                return RedirectToAction("ReviewHRManual"); 
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Index(Onboardings onboardings, int? EmpId)
        {
            EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
            Onboardings obj = (Onboardings)Session["onboradObj"];
            Onboardings getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
            if (getonboardvalue != null)
            {
                return RedirectToAction("ReviewHRManual");
            }
            else
            {
                dbContext.SaveOnboarding("insert", employee.EmpId, employee.EmpName, onboardings.Timesheet_check,
                    onboardings.Leave_check, onboardings.WFH_check, onboarding.Confidentiality_check, onboardings.HR_Manual_check,
                    onboardings.ITInduction_check, onboardings.SkypeAccount_check, onboardings.MS365_check, onboardings.SkypeInvite_check,
                    onboardings.Registration_check, onboardings.PI_check, onboardings.Form_check,
                    onboardings.IsCompleted, null, null);
                return RedirectToAction("ReviewHRManual");
            }
            #region old code
            //else if (employee.EmpId != 0 && !string.IsNullOrEmpty(employee.EmpName) && onboardings.Timesheet_check && onboardings.Leave_check && onboardings.WFH_check)
            //{
            //    if (!(getonboardvalue.Timesheet_check && getonboardvalue.Leave_check && getonboardvalue.WFH_check))
            //    {
            //        dbContext.SaveOnboarding("update", employee.EmpId, employee.EmpName, onboardings.Timesheet_check, onboardings.Leave_check, onboardings.WFH_check, getonboardvalue.HR_Manual_check, getonboardvalue.Dwn_Letter, getonboardvalue.IntroductionChecked, onboardings.IsCompleted, getonboardvalue.SignImage);
            //    }
            //    return RedirectToAction("ReviewHRManual");
            //}
            #endregion
        }
        public ActionResult ReviewHRManual()
        {
            int fileCount;
            List<GetHrPolicy> ListFile = dbContext.GetHrPolicyFilesDetails("GET");
            fileCount = ListFile.Count;
            if (fileCount > 0)
            {
                fileCount = ListFile.Count - 1;
                for (int i = 0; i <= fileCount; i++)
                {
                    if (ListFile[i].FileName.StartsWith("HR_Policy", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ViewBag.FileName = ListFile[i].DisplayFileURL.ToString();
                        ViewBag.FilePath = ListFile[i].FileUploadPath.ToString();
                    }
                }
            }
            ViewBag.IsSuccess = "true";

            if (Session["getObjById"]!=null)
            {
                var getHrManualPDFValue = ConfigurationManager.AppSettings["HrManualpdf"];
                EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
                Onboardings getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
                return View(getonboardvalue); 
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult ReviewHRManual(Onboardings onboard)
        {
            EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
            Onboardings getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
            if (employee != null && getonboardvalue.EmpId != 0 && onboard.HR_Manual_check)
            {
                dbContext.SaveOnboarding("update", employee.EmpId, employee.EmpName, getonboardvalue.Timesheet_check, getonboardvalue.Leave_check,
                    getonboardvalue.WFH_check, getonboardvalue.Confidentiality_check, onboard.HR_Manual_check, getonboardvalue.ITInduction_check,
                    getonboardvalue.SkypeAccount_check, getonboardvalue.MS365_check, getonboardvalue.SkypeInvite_check,
                    getonboardvalue.Registration_check, getonboardvalue.PI_check, getonboardvalue.Form_check,
                    onboard.IsCompleted, getonboardvalue.SignImage, getonboardvalue.InsertedOn);
            }
            return RedirectToAction("AgoraLoginPortal");
        }
        public ActionResult AgoraLoginPortal()
        {
            if (Session["getObjById"] != null)
            {
                EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
                Onboardings getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
                return View(getonboardvalue);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult AgoraLoginPortal(Onboardings onboardings)
        {
            if (Session["getObjById"] != null)
            {
                EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
                Onboardings obj = (Onboardings)Session["onboradObj"];
                Onboardings getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
                if (employee.EmpId != 0 && !string.IsNullOrEmpty(employee.EmpName) && onboardings.Timesheet_check && onboardings.Leave_check && onboardings.WFH_check)
                {
                    if (!(getonboardvalue.Timesheet_check && getonboardvalue.Leave_check && getonboardvalue.WFH_check))
                    {
                        dbContext.SaveOnboarding("update", employee.EmpId, employee.EmpName, onboardings.Timesheet_check, onboardings.Leave_check,
                            onboardings.WFH_check, onboardings.Confidentiality_check, getonboardvalue.HR_Manual_check,
                            getonboardvalue.ITInduction_check, getonboardvalue.SkypeAccount_check, getonboardvalue.MS365_check, getonboardvalue.SkypeInvite_check,
                            getonboardvalue.Registration_check, getonboardvalue.PI_check, getonboardvalue.Form_check, onboardings.IsCompleted,
                            getonboardvalue.SignImage, getonboardvalue.InsertedOn);
                    }

                }
                return RedirectToAction("AppointmentLetter");
            }
            return HttpNotFound();

        }
        public ActionResult AppointmentLetter()
        {
            if (Session["getObjById"] != null)
            {
                EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
                Onboardings getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
                return View(getonboardvalue);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult AppointmentLetter(HttpPostedFileBase file, Onboardings onboard)
        {

            Onboardings obj = new Onboardings();
            EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
            Onboardings getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
            dynamic storePath = null;
            if (file != null)
            {
                storePath = UploadFile(file, getonboardvalue.EmpId);
            }
            if (employee != null && getonboardvalue.EmpId != 0 && onboard.ITInduction_check && onboard.Form_check)
            {
                dbContext.SaveOnboarding("update", employee.EmpId, employee.EmpName, getonboardvalue.Timesheet_check, getonboardvalue.Leave_check,
                    getonboardvalue.WFH_check, getonboardvalue.Confidentiality_check, getonboardvalue.HR_Manual_check, onboard.ITInduction_check,
                    onboard.SkypeAccount_check, onboard.MS365_check, onboard.SkypeInvite_check,
                    onboard.Registration_check, onboard.PI_check, onboard.Form_check,
                    onboard.IsCompleted, storePath, getonboardvalue.InsertedOn);
            }
            return RedirectToAction("WelldonePage");
        }
        public ActionResult WelldonePage()
        {
            if (Session["getObjById"] != null)
            {

                EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
                Onboardings getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
                return View(getonboardvalue); 
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult WelldonePage(Onboardings onboard)
        {
            ClsCommon common = new ClsCommon();
            //var url = ConfigurAppendAllTextationSettings.AppSettings["AgoraURL"];
            var url = System.Configuration.ConfigurationManager.AppSettings["AgoraURL"];
            bool isCompleted;
            EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
            Onboardings getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
            if (employee != null && getonboardvalue.EmpId != 0 && getonboardvalue.Timesheet_check && getonboardvalue.Leave_check &&
                getonboardvalue.WFH_check && getonboardvalue.HR_Manual_check && getonboardvalue.ITInduction_check&&getonboardvalue.Form_check)
            {
                common.SendEmail(employee);
                isCompleted = true;
                getonboardvalue.InsertedOn = DateTime.Now.ToString();
                dbContext.SaveOnboarding("update", employee.EmpId, employee.EmpName, getonboardvalue.Timesheet_check, getonboardvalue.Leave_check,
                    getonboardvalue.WFH_check, getonboardvalue.Confidentiality_check, getonboardvalue.HR_Manual_check, getonboardvalue.ITInduction_check,
                    getonboardvalue.SkypeAccount_check, getonboardvalue.MS365_check, getonboardvalue.SkypeInvite_check,
                    getonboardvalue.Registration_check, getonboardvalue.PI_check, getonboardvalue.Form_check,
                    isCompleted, getonboardvalue.SignImage, getonboardvalue.InsertedOn);
                //Session.Abandon(); Session.Clear(); Session.RemoveAll();
            }
            else if (employee != null && getonboardvalue.EmpId != 0)
            {

                dbContext.SaveOnboarding("update", employee.EmpId, employee.EmpName, getonboardvalue.Timesheet_check, getonboardvalue.Leave_check,
                    getonboardvalue.WFH_check, getonboardvalue.Confidentiality_check, getonboardvalue.HR_Manual_check, getonboardvalue.ITInduction_check,
                    getonboardvalue.SkypeAccount_check, getonboardvalue.MS365_check, getonboardvalue.SkypeInvite_check,
                    getonboardvalue.Registration_check, getonboardvalue.PI_check, getonboardvalue.Form_check,
                    getonboardvalue.IsCompleted, getonboardvalue.SignImage, getonboardvalue.InsertedOn);
                // Session.Abandon(); Session.Clear(); Session.RemoveAll();
            }
            //dbContext.SaveOnboarding("update", employee.EmpId, employee.EmpName, getonboardvalue.Timesheet_check, getonboardvalue.Leave_check, getonboardvalue.WFH_check, getonboardvalue.HR_Manual_check, getonboardvalue.Dwn_Letter, getonboardvalue.IntroductionChecked, onboard.IsCompleted, "Img");
            return Redirect(url);
        }

        public ActionResult GetHrManual()
        {
            return PartialView();
        }
        public ActionResult GetAppointmentLetter()
        {
            return PartialView();
        }
        #region old code pdf download
        //public ActionResult DownloadAppointmentLetter(string emp_id, Onboardings obj)
        //{
        //    Onboardings getonboardvalue = null;
        //    var empId = Utility.Decrypt(emp_id.Replace(' ', '+'));
        //    EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
        //    if (!string.IsNullOrEmpty(empId))
        //    {
        //        getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
        //        obj.Dwn_Letter = true;
        //        getonboardvalue.Dwn_Letter = obj.Dwn_Letter;

        //    }
        //    byte[] bytes = { 0 };
        //    TempData["Dwn_Letter"] = obj.Dwn_Letter;
        //    string existFile = ConfigurationManager.AppSettings["AppointmentLetterFilePath"] + empId + ".pdf";
        //    if (System.IO.File.Exists(existFile))
        //    {
        //        bytes = System.IO.File.ReadAllBytes(existFile);
        //        string pdfName = "Employee_Appointment_" + empId + ".pdf";
        //        dbContext.SaveOnboarding("update", employee.EmpId, employee.EmpName, getonboardvalue.Timesheet_check, getonboardvalue.Leave_check, getonboardvalue.WFH_check, getonboardvalue.HR_Manual_check, true, getonboardvalue.IntroductionChecked, getonboardvalue.IsCompleted, getonboardvalue.SignImage);
        //        return File(bytes, "application/pdf", pdfName);
        //    }
        //    ViewBag.Appointment = "Appointment Letter is not Exist!";
        //    return View("AppointmentLetter");

        //}
        #endregion
        private string UploadFile(HttpPostedFileBase file, int emp_Id)
        {

            string path = "";
            string finalPath = string.Empty;
            if (file.ContentLength > 0 && file != null)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() == ".docx" || extension.ToLower() == ".pdf" || extension.ToLower() == ".doc")
                {
                    try
                    {
                        path = ConfigurationManager.AppSettings["SignefFileUpload"].ToString();
                        // path=Path.Combine(Server.MapPath("~/UploadSignedFile/") + Path.GetFileName("SignedFile_" + emp_Id+extension));
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        file.SaveAs(path + @"\SignedFile_" + emp_Id + extension);
                        finalPath = path + @"\SignedFile_" + emp_Id + extension;
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                }
                else
                {
                    return null;

                }

            }
            else
            {

                return null;
            }
            return finalPath;
        }
        #region store the value through a cookies.
        //public ActionResult Test(int id)
        //{
        //    var objects = Server.UrlEncode(Utility.Encrypt(id.ToString()));
        //    var obj1 = Utility.Encrypt("1000");
        //    var obj2 = Utility.Decrypt(obj1);
        //    Onboardings onborading = dbContext.GetOnboardingById(id).Find(x => x.EmpId == id);
        //    ViewBag.getOnboard = dbContext.GetOnboardingById(id).Find(x => x.EmpId == id);
        //    var getEmployeeMasterList = dbContext.GetEmployeeMasterList(id).Find(x => x.EmpId == id);
        //    ViewBag.getNameById = getEmployeeMasterList;
        //    TempData["getNameById"] = getEmployeeMasterList;
        //    if (onboarding.EmpId == id)
        //    {
        //        return Redirect("http://emp.intelgain.com/Member/Login.aspx");
        //    }
        //    //Session["CurrentUrl"] = Request.Url.ToString();
        //    //var url = Url.RequestContext.RouteData.Values["id"];
        //    //Onboardings onborading = dbContext.GetOnboardingById(id).Find(x => x.EmpId == id);

        //    //TempData["getOnboardById"] = getOnboardById;
        //    //dbContext.SaveOnboarding("insert", 2995, "atif", true, true, true, true, true, true, true);
        //    //if (getOnboardById != id)
        //    //{

        //    //}
        //    //Session["empname"] = 1;

        //    //HttpCookie httpCookie = new HttpCookie("Indexpage");
        //    //httpCookie.Values.Add ("EmpId",employee?.EmpId.ToString());
        //    //httpCookie.Values.Add("Empname",employee?.EmpName);
        //    //httpCookie.Values.Add("Timesheet_check", onboardings.Timesheet_check.ToString());
        //    //httpCookie.Values.Add("Leave_check", onboardings.Leave_check.ToString());
        //    //httpCookie.Values.Add("FaqCheck",Convert.ToString(onboardings.WFH_check));
        //    //httpCookie.Values.Add("HR_Manual_check", onboardings.HR_Manual_check.ToString());
        //    //httpCookie.Values.Add("IntroductionChecked", onboardings.IntroductionChecked.ToString());
        //    //httpCookie.Expires = System.DateTime.Now.AddDays(30);
        //    //Response.Cookies.Add(httpCookie);
        //    //else if (getonboardvalue.Timesheet_check == true && getonboardvalue.Leave_check == true && getonboardvalue.WFH_check == true && onboardings.HR_Manual_check==true || onboardings.HR_Manual_check == false)
        //    //{
        //    //       dbContext.SaveOnboarding("update", getonboardvalue.EmpId, getonboardvalue.Empname, getonboardvalue.Timesheet_check, getonboardvalue.Leave_check, getonboardvalue.WFH_check, onboardings.HR_Manual_check, onboardings.Dwn_Letter, onboardings.IntroductionChecked, onboardings.IsCompleted, "Img");
        //    //       return RedirectToAction("AppointmentLetter");
        //    //}

        //    //else if (onboardings.Timesheet_check == true && onboardings.Leave_check == true && onboardings.WFH_check == true && !string.IsNullOrEmpty(onboardings.HR_Manual_check.ToString()) && !string.IsNullOrEmpty(onboardings.HR_Manual_check.ToString()))
        //    //{
        //    //    dbContext.SaveOnboarding("update", employee.EmpId, employee.EmpName, onboardings.Timesheet_check, onboardings.Leave_check, onboardings.WFH_check, onboardings.HR_Manual_check, onboardings.Dwn_Letter, onboardings.IntroductionChecked, onboardings.IsCompleted, "Img");
        //    //    return RedirectToAction("WelldonePage");

        //    //}

        //    //EmployeeMaster employee = (EmployeeMaster)Session["getObjById"];
        //    //Onboardings getonboardvalue=null;
        //    //if (getonboardvalue!=null)
        //    //{
        //    //    getonboardvalue = dbContext.GetOnboardingById(employee.EmpId).Find(x => x.EmpId == employee.EmpId);
        //    //}
        //    //if (getonboardvalue==null)
        //    //{
        //    //    return RedirectToAction("Index");
        //    //}
        //    //var id1="";
        //    //if (!string.IsNullOrEmpty(id.ToString()))
        //    //{
        //    //     id1= (Utility.Encrypt(id.ToString()));
        //    //}

        //    return View(id);

        //}
        #endregion
    }
}