using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_SRSContent : Authentication
{
    UserMaster UM;
    public UserDetails UD;
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "ShowLoading();", true);
        UM = UserMaster.UserMasterInfo();
        int IsAdmin = 0;
        if (UM != null)
        {
            UD = HttpContext.Current.Session["MemberSession"] as UserDetails;
            if (UD.IsAdmin == true || UM.IsModuleAdmin == true)
            {
                IsAdmin = 1;
            }
        }
        HttpHelper.RedirectAndPOST(this.Page, "http://www.agoramvc.sirus/SRSDetails/Index/", UM.EmployeeID.ToString(), IsAdmin.ToString());  //  for sirus
       // HttpHelper.RedirectAndPOST(this.Page, "http://localhost:10010/SRSDetails/Index/", UM.EmployeeID.ToString(), IsAdmin.ToString());  //  for local
        //HttpHelper.RedirectAndPOST(this.Page, "http://localhost:10010/SRSDetails/Index", UM.EmployeeID.ToString(), IsAdmin.ToString());  //  for local
        
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "HideLoading();", true);
       
      
       
    }
}