using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;


/// <summary>
/// Summary description for Authentication
/// </summary>
public class Authentication : System.Web.UI.Page
{
    
    public Authentication()
    {
        //
        // TODO: Add constructor logic here
        //
       
        
    }

    protected override void OnInit(EventArgs e)
    {

        string UserID = Session["EmpID"].ToString();//HttpContext.Current.Session["EmpID"].ToString();
        if (UserID == "")
        {
           Response.Redirect("~/Login.aspx");
        }
       // Session.Timeout = 2;
        }

    }
   
