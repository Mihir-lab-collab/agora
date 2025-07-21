using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Member_EmployeeLWD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [System.Web.Services.WebMethod]
    public static String BindEmployeeLWD()
    {
        try
        {

            List<EmployeeLWD_DLL> lstEmployee = EmployeeLWD_DLL.GetEmployeeLWDDetails("LWD"); //, Convert.ToInt32(HttpContext.Current.Session["ProjectID"])
            var data = from EmployeeItem in lstEmployee
                       select new
                       {

                           EmployeeItem.empid,
                           EmployeeItem.empName,
                           EmployeeItem.empExpectedLWD,
                           EmployeeItem.daysPending

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