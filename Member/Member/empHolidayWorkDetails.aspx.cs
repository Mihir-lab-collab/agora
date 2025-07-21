using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Member_empHolidayWorkDetails : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            UM = UserMaster.UserMasterInfo();

            if (UM != null)
            {
                hdnEmpID.Value = Convert.ToString(UM.EmployeeID);
                hdnLocationID.Value = UM.LocationID;
                BindLocation(Convert.ToInt32(UM.LocationID));
            }
            else
            {
                Response.Redirect("~/Member/Login.aspx");
            }
        }
    }

    //protected void ddlEmp_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    hdnSelectEmp.Value = ddlEmp.SelectedValue;

    //}

    //protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    hdnStaus.Value = ddlStatus.SelectedValue;
    //}

    [System.Web.Services.WebMethod]
    public static String BindEmployee()
    {
        List<EmployeeMaster> empList = EmployeeMaster.GetEmployeeDetails("Select", 10, "Current");
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(empList);

    }

    [System.Web.Services.WebMethod]
    public static String GetHolidayWorkingData(int Empid, int Status, string HolidayStartDate, string HolidayEndDate, int LocationID)
    {
        try
        {
            List<HolidayWorking> lstHolidayWorkingData = HolidayWorking.GetHolidayWorkingData(Empid, Status, HolidayStartDate, HolidayEndDate, LocationID); //, Convert.ToInt32(HttpContext.Current.Session["ProjectID"])
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(lstHolidayWorkingData);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String ApproveHolidayWorking(int ID, int Empid, string Comment, string compOffDate)
    {
        try
        {
            HolidayWorking.HolidayLeave("ApproveHolidayLeave", Empid, Comment, ID, compOffDate);
            //HolidayWorking.HolidayLeave("ApproveHolidayLeave", Empid, Comment, ID, "");
            return "";
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String checkCompOffExists(int Empid,string CompOffDate)
    {
        try
        {
            string result= HolidayWorking.checkCompOffExists(Empid, CompOffDate);
            //HolidayWorking.HolidayLeave("ApproveHolidayLeave", Empid, Comment, ID, "");
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String RejectHolidayWorking(int Empid, string Comment, int ID)
    {
        try
        {
            HolidayWorking.HolidayLeave("RejectHolidayLeave", Empid, Comment, ID, ""); //, Convert.ToInt32(HttpContext.Current.Session["ProjectID"])
            // JavaScriptSerializer jss = new JavaScriptSerializer();
            return "";
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    [System.Web.Services.WebMethod]
    public static String CancelHolidayLeave(int ID, int Empid, string Comment, string CompOffDate)
    {
        try
        {
            HolidayWorking.HolidayLeave("CancelHolidayLeave", Empid, Comment, ID, CompOffDate); //, Convert.ToInt32(HttpContext.Current.Session["ProjectID"])
            // JavaScriptSerializer jss = new JavaScriptSerializer();
            return "";
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String AddToCompOff(int Empid, int HolidayLeaveID, int Status)
    {
        try
        {
            HolidayWorking.AddToCompOff(Empid, HolidayLeaveID, Status); //, Convert.ToInt32(HttpContext.Current.Session["ProjectID"])
            // JavaScriptSerializer jss = new JavaScriptSerializer();
            return "";
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String CreateCompOff(int Empid, string CompOffDate, string Comment, int EntryBy)
    {
        try
        {
            HolidayWorking.CreateCompOff(Empid, CompOffDate, Comment, EntryBy); //, Convert.ToInt32(HttpContext.Current.Session["ProjectID"])
            // JavaScriptSerializer jss = new JavaScriptSerializer();
            return "";
        }
        catch (Exception ex)
        {
            return null;
        }
    }
  
    public void BindLocation(int LocationID)
    {
        List<LocationBLL> location = LocationBLL.BindLocation("GetLocationByID", LocationID);
        lblLocationId.InnerText = location[0].Name;
    }
}