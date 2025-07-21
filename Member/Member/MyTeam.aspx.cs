using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Customer.BLL;
using Customer.DAL;


public partial class Member_MyTeam : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo(); 
        if (!IsPostBack)
        {
            Session["UserName"] = UM.Name.ToString();
            Session["CustId"] = UM.EmployeeID.ToString();
            Session["UserId"] = UM.EmployeeID.ToString();
            Session["UserMailid"] = UM.EmailID.ToString();
            Session["empId"] = UM.EmployeeID.ToString();
            hdn.Value = UM.EmployeeID.ToString();
        }
    }

    [System.Web.Services.WebMethod]
    public static String BindMyTeam(int empId, string includeMyprojects)
    {
        try
        {
            List<MyTeamBLL> lstGetMyTeam = MyTeamBLL.GetMyTeam(empId, includeMyprojects);
            var data = from curMyTeam in lstGetMyTeam
                       select new
                       {
                           curMyTeam.empId,
                           curMyTeam.empName,
                           curMyTeam.designation,
                           curMyTeam.primarySkill,
                          curMyTeam.SecondarySkill,
                           curMyTeam.experience,
                           curMyTeam.projectsWorkingOn            
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception ex)
        {
            return null;
        }
    }
}