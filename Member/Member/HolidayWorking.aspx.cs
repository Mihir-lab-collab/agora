using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_HolidayWorking : Authentication
{
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UM = UserMaster.UserMasterInfo();
            Admin MasterPage = (Admin)Page.Master;
            MasterPage.MasterInit(true, false, true, false);
            hdnEmpID.Value = Convert.ToString(UM.EmployeeID);
           //BindHolidayDateDDL();    //need to Commnet trupti
        }
    }
    public T ConvertJSonToObject<T>(string jsonString)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)serializer.ReadObject(ms);
        return obj;
    }
    //Need to commnet by trupti
    //public void BindHolidayDateDDL()
    //{
    //    ddlHolidayDt1.DataSource = ConvertJSonToObject<List<HolidayWorking>>(BindHolidayDate());
    //    ddlHolidayDt1.DataTextField = "HolidayDate";
    //    ddlHolidayDt1.DataValueField = "HolidayDate";
    //    ddlHolidayDt1.DataBind();
    //}
    [System.Web.Services.WebMethod]
    public static String BindExpHours()
    {
        try
        {
            List<int> hours = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                hours.Add(i);
            }
            var data = from d in hours
                       select new
                       {
                           d
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {

            return null;
        }
    }
    //Need to commnet by Trupti
    //[System.Web.Services.WebMethod]
    //public static String BindHolidayDate()
    //{
    //    try
    //    {
    //        List<HolidayWorking> lstHolidayWorking = HolidayWorking.GetHolidayDate();
    //        var data = from d in lstHolidayWorking
    //                   select new
    //                   {
    //                       d.HolidayDate,
    //                   };
    //        JavaScriptSerializer jss = new JavaScriptSerializer();
    //        return jss.Serialize(data);
    //    }
    //    catch (Exception ex)
    //    {

    //        return null;
    //    }
    //}

    [System.Web.Services.WebMethod]
    public static string BindHolidayWorking()
    {
        try
        {
            List<HolidayWorking> lstHolidayWorking = HolidayWorking.GetHolidayWorkingDetails("GETDETAIL", Convert.ToInt32(UM.EmployeeID), Convert.ToInt32(HttpContext.Current.Session["ProjectID"]));
            var data = from HolidayWorkingItem in lstHolidayWorking
                       select new
                       {
                           HolidayWorkingItem.Id,
                           HolidayWorkingItem.EmpId,
                           HolidayWorkingItem.HolidayDate,
                           HolidayWorkingItem.ProjId,
                           HolidayWorkingItem.ExpectedHours,
                           HolidayWorkingItem.UserReason,
                           HolidayWorkingItem.ProjectName,
                           HolidayWorkingItem.UserEntryDate,
                           HolidayWorkingItem.Status,
                           HolidayWorkingItem.AdminComment,
                           HolidayWorkingItem.AdminCanReason,
                           HolidayWorkingItem.Statusflag
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string UpdateHolidayWorking(int HolidayWorkingId, int EmpID, int ProjectId, string holidayWdate, int exphours,string reason)
    {
        string output = "";
        try
        {
            output=HolidayWorking.SaveHolidayWorking("UPDATE", HolidayWorkingId, EmpID, ProjectId, holidayWdate, exphours, reason);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }

    [System.Web.Services.WebMethod]
    public static void CancelHolidayWorking(int EmpID, string HolidayDate, int ProjId)
    {
        HolidayWorking.CancelHolidayWorking("CANCEL", EmpID, HolidayDate, ProjId);
    }

    protected void lnkSaveHolidayWorkingReq_Click(object sender, EventArgs e)
    {
        int ProjectId = 0;
        int HolidayWorkingId = 0;
        string holidayWdate = string.Empty;
        int exphours = 0;
        string reason = string.Empty;
        if (Session["ProjectID"] != null || Convert.ToString(Session["ProjectID"]) != string.Empty)
        {
            ProjectId = Convert.ToInt32(Session["ProjectID"]);
        }
        holidayWdate = hdnHWDate.Value;
        exphours = Convert.ToInt32(hdnExpHours.Value);
        reason = hdnReason.Value;

      

            string output = "";
            output = HolidayWorking.SaveHolidayWorking("INSERT", HolidayWorkingId, Convert.ToInt32(hdnEmpID.Value), ProjectId, holidayWdate, exphours, reason);


           

            if (!string.IsNullOrEmpty(output))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + output + "')", true);
               return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Holiday working request saved successfully')", true);
                SendMail(holidayWdate);
            }
    }
    public void SaveData()
    {
       
    }

    private void SendMail(string holidayWdate)
    {
        if (UM == null)
            UM = UserMaster.UserMasterInfo();

        int ProjectId = 0;
        string PmMailId = string.Empty;
        if (Session["ProjectID"] != null || Convert.ToString(Session["ProjectID"]) != string.Empty)
        {
            ProjectId = Convert.ToInt32(Session["ProjectID"]);
            List<HolidayWorking> lstPmdetails = HolidayWorking.GetPmDetailsByProjid(ProjectId);
            PmMailId = Convert.ToString(lstPmdetails[0].empEmail);
        }
       //List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 40); //Local
        List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 25);  ///Live   
        string EmailBody = lstConfig[0].value.ToString();
        EmailBody = EmailBody.Replace("{UserFName}", Convert.ToString(UM.Name));
        EmailBody = EmailBody.Replace("{ApplicationDate}", holidayWdate);
      
        string strBody, strSubject, mailTo, mailFrom, message, CC = "";
       //// strBody = UM.Name.ToString() + " " + "has applied for holiday working on " + holidayWdate + ".";
        strBody = EmailBody;
        strSubject = UM.Name.ToString() + " has applied for holiday working.";
        mailTo = ConfigurationManager.AppSettings.Get("HREmail").ToString();
        mailFrom = UM.EmailID.ToString();
        CC = PmMailId;
        message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
    }
}