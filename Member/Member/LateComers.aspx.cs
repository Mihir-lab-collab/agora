using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_LateComers : System.Web.UI.Page// Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        //UM = UserMaster.UserMasterInfo();
        //Admin MasterPage = (Admin)Page.Master;
        //hdnLoginID.Value = UM.EmployeeID.ToString();
        if (!IsPostBack)
        {
            UM = UserMaster.UserMasterInfo();

            if (UM != null)
            {
                hdnLoginID.Value = Convert.ToString(UM.EmployeeID);
                hdnLocationID.Value = UM.LocationID;
                BindLocation(Convert.ToInt32(UM.LocationID));
            }
            else
            {
                Response.Redirect("~/Member/Login.aspx");
            }
        }

    }
    [System.Web.Services.WebMethod]
    public static String BindLateComers(string mDate = "")
    {
        try
        {
             List<LateComing> lstMDue = LateComing.GetLateComersData("Select");
            var data = from EItems in lstMDue
                       select new
                       {
                           EItems.ID,
                           EItems.EmpNameId,
                           EItems.EmpCode,
                           EItems.ApplyDate,
                           EItems.ExpectedInTime,
                           EItems.CreatedOn,
                           EItems.CreatedBy,
                           EItems.LateCommingReason,
                           EItems.ApprovedOn,
                           EItems.ApprovedBy,
                           EItems.ApprovalComment,
                           EItems.IsApproveStatus,
                           EItems.Statusflag,

                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String CancelLateComing(int ID, int Empid, string Comment, DateTime Applydate)
    {
        try
        {
            LateComing.LateComingEmp("CancelHolidayLeave", Empid, Comment, ID, Applydate); //, Convert.ToInt32(HttpContext.Current.Session["ProjectID"])
            // JavaScriptSerializer jss = new JavaScriptSerializer();
            return "";
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    [System.Web.Services.WebMethod]
    public static String GetLateComingData(int Empid, int Status, string StartDate, string EndDate, int LocationID)
    {
        try
        {
            List<LateComing> lstLateComingData = LateComing.GetLateComingSearchData(Empid, Status, StartDate, EndDate, LocationID); //, Convert.ToInt32(HttpContext.Current.Session["ProjectID"])
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(lstLateComingData);
        }
        catch (Exception ex)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            return null;
        }
    }


    [System.Web.Services.WebMethod]
    public static String CancleLateComing(int ID, int EmpCode, string ApprovalComment)
    {
        try
        {
            LateComing.LateComingCancle("CancelLateComing", ID,EmpCode, ApprovalComment); 
            // JavaScriptSerializer jss = new JavaScriptSerializer();
            return "";
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String BindEmployee()
    {
        List<EmployeeMaster> empList = EmployeeMaster.GetEmployeeDetails("Select", 10, "Current");
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(empList);

    }

    [System.Web.Services.WebMethod]
    public static String ApproveLateComers(int EmpCode, string ApprovalComment, int ID, int IsApproveStatus, int ApprovedBy)
    {
        try
        {
            LateComing.LateComingApprove("ApproveLatecomer", EmpCode, ApprovalComment, ID, IsApproveStatus, ApprovedBy);
            //HolidayWorking.HolidayLeave("ApproveHolidayLeave", Empid, Comment, ID, compOffDate);
            //HolidayWorking.HolidayLeave("ApproveHolidayLeave", Empid, Comment, ID, "");
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