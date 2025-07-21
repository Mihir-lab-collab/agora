using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Customer.BLL;

public partial class Member_ProjectTeam : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(true, false,true);
        //hdDiscount.Value = "";

        if (!IsPostBack)
        {
            HttpContext.Current.Session["ProjectId"] = string.Empty;
        }
        hdSessionProjId.Value = Convert.ToString(HttpContext.Current.Session["ProjectId"]);

    }
    protected void btnAddProjects_Click(object sender, EventArgs e)
    {
        try
        {
            int Id = hdMemberId.Value != "" ? Convert.ToInt32(hdMemberId.Value) : 0;
            int projectId = hdprojId.Value != "" ? Convert.ToInt32(hdprojId.Value) : 0;
            int employeeId = hdempId.Value != "" ? Convert.ToInt32(hdempId.Value) : 0;
            string Mode = hdMode.Value != "" ? hdMode.Value : "INSERT";
            int discount = Convert.ToInt32(hdDiscount.Value.Replace("'", "''"));
            int Isactive = Convert.ToInt16(chkIsActive.Checked);
            DateTime modifieddate = DateTime.Now;
            messageBox(ProjectTeam.UpdateProjectTeam(projectId, employeeId, discount, Isactive, modifieddate, Mode,Id));
        }
        catch (Exception Ex)
        {
            messageBox("Save Failed." + Ex.Message);
        }
    }
   
    [System.Web.Services.WebMethod]
    public static String FillEmployeeDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(ProjectTeam.GetEmployees());//EmployeeMaster.GetAllEmployees());
    }

    [System.Web.Services.WebMethod]
    public static String FillProjectDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(projectMaster.GetProjectsTitles());
    }

    [System.Web.Services.WebMethod]
    public static String FillDiscountDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
       var discountList = Enumerable.Range(1, 100).ToList();
       return jss.Serialize(discountList);
    }

    [System.Web.Services.WebMethod]
    public static String ProjectList()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        List<ProjectTeam> AllProjects = ProjectTeam.Projects();
        var data = from curproject in AllProjects
                   orderby curproject.ModifiedOn descending
                   select new
                   {
                       curproject.projName,
                       curproject.empName,
                       curproject.Discount,
                       curproject.Isactive,
                       curproject.ModifiedOn,
                       curproject.empId,
                       curproject.projId,
                       curproject.MemberId
                   };
        return jss.Serialize(data);
    }


    [System.Web.Services.WebMethod]
    public static String BindProjectList()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        List<ProjectTeam> AllProjects = new List<ProjectTeam>();
        if (HttpContext.Current.Session["ProjectId"] != "" && HttpContext.Current.Session["ProjectId"].ToString() != "0")
        {
           AllProjects = ProjectTeam.Projects("SelectbyId", Convert.ToInt32(HttpContext.Current.Session["ProjectId"]));
        }
        else
        {
            AllProjects = ProjectTeam.Projects();
        }
            var data = from curproject in AllProjects
                       orderby curproject.ModifiedOn descending
                       select new
                       {
                           curproject.projName,
                           curproject.empName,
                           curproject.Discount,
                           curproject.Isactive,
                           curproject.ModifiedOn,
                           curproject.empId,
                           curproject.projId,
                           curproject.MemberId
                       };
            return jss.Serialize(data);
    }


    [System.Web.Services.WebMethod]
    public static String UpdateProjectProjTeam(int projId, string projName, int empId, string empName, int Discount)
    {
        string statusmsg = "Update Failed";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(statusmsg);
    }

    private void messageBox(string message)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('" + message + "');", true);
    }
}