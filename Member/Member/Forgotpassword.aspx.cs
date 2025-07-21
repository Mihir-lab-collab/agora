using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_Forgotpassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btngotoLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Member/Login.aspx");
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        string message = string.Empty;
        UserMaster objUserMaster = new UserMaster();
        try
        {
            objUserMaster = UserMaster.SendMailForForgotPassword(txtEmpId.Value, txtEmailId.Value);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ForgotPassword", "<script>alert('" + objUserMaster.Message + "');</script>", false);
            clear();

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }
    private void clear()
    {
        txtEmailId.Value = string.Empty;
        txtEmpId.Value = string.Empty;
    }
}