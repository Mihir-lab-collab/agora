using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int empid = 0;
        bool redirection = false;
        UserMaster objUserMaster = new UserMaster();
        if (Request.QueryString["id"] != null && Request.QueryString["datetime"] != null)
        {
            bool InvalidLink = false;
            string EmpId = string.Empty;
            string dateTime = string.Empty;

            EmpId = !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])) ? CSCode.Global.Decrypt(Convert.ToString(Request.QueryString["id"]).Replace(" ", "+"), true) : string.Empty;
            dateTime = !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["datetime"])) ? CSCode.Global.Decrypt(Convert.ToString(Request.QueryString["datetime"]).Replace(" ", "+"), true) : string.Empty;
            empid = !string.IsNullOrEmpty(EmpId) ? Convert.ToInt32(EmpId) : 0;
         
            if (empid == 0 || string.IsNullOrEmpty(dateTime))
                InvalidLink = true;
            if (InvalidLink)
            {
                Session["Message"] = "Invalid link.";
                redirection = true;
            }
            if (empid != 0 && !string.IsNullOrEmpty(dateTime))
            {
                objUserMaster = UserMaster.CheckPasswordLink(empid, "CheckForgotPwdLinkDate", Convert.ToDateTime(dateTime));
                if (!objUserMaster.status)
                {
                    Session["Message"] = objUserMaster.Message;
                    redirection = true;
                }
            }

     
        }

        if (redirection)
        {
            Response.Redirect("/Member/Login.aspx");
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {

        int empid = 0;
        bool redirection = false;
        UserMaster objUserMaster = new UserMaster();
        try
        {


            if (Request.QueryString["id"] != null && Request.QueryString["datetime"] != null)
            {
                bool InvalidLink = false;
                string EmpId = string.Empty;
                string dateTime = string.Empty;
                EmpId = !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])) ? CSCode.Global.Decrypt(Convert.ToString(Request.QueryString["id"]).Replace(" ", "+"), true) : string.Empty;

                dateTime = !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["datetime"]).Replace(" ", "+")) ? CSCode.Global.Decrypt(Convert.ToString(Request.QueryString["datetime"]).Replace(" ", "+"), true) : string.Empty;
                empid = !string.IsNullOrEmpty(EmpId) ? Convert.ToInt32(EmpId) : 0;
                if (empid == 0 || string.IsNullOrEmpty(dateTime))
                    InvalidLink = true;
                if (InvalidLink)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ChangePassword", "<script>alert('Invalid link.');</script>", false);
                    return;
                }
                objUserMaster = UserMaster.CheckPasswordLink(empid, "CheckForgotPwdLinkDate", Convert.ToDateTime(dateTime));

                if (objUserMaster.status)
                {
                    objUserMaster = UserMaster.VerifyUserByEmpid(Convert.ToInt16(empid));
                    if (!string.IsNullOrEmpty(Convert.ToString(objUserMaster.EmployeeID)) && objUserMaster.EmployeeID > 1 && !string.IsNullOrEmpty(objUserMaster.EmailID))
                    {
                        if (UserMaster.UpdatePassword(objUserMaster.EmployeeID, objUserMaster.EmailID, CSCode.Global.CreatePassword(txtNewPassword.Value)))
                        {
                            string message = "Your password has been changed successfully";
                            objUserMaster.UpdatePasswordLinkDate(empid, null);
                            objUserMaster.SendMailForChangepassword(objUserMaster.Name ,objUserMaster.EmailID);
                            Session["Message"] = message;
                            redirection = true;
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ChangePassword", "<script>alert('Change password operation failed .');</script>", false);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ChangePassword", "<script>alert('Change password operation failed .');</script>", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ChangePassword", "<script>alert('" + objUserMaster.Message + "');</script>", false);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ChangePassword", "<script>alert('Invalid link .');</script>", false);
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        if (redirection)
        {
            Response.Redirect("/Member/Login.aspx");
        }
    }
}