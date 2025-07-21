using dwtDAL;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_Profile : Authentication
{
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            PersonalDetails();
            if (string.Compare(Convert.ToString(UM.Designation),"Consultant",true)==0)
            {
                Panel2.Visible = false;
            }
            else
            {
                Panel2.Visible = true;
            }
            YourPackage();
            GetEditedProfileStatus(UM.EmployeeID);
        }
    }

    private void GetEditedProfileStatus(int empID)
    {
        EmployeeMaster obj = new EmployeeMaster();
        string status = obj.GetEditedProfileStatus("ApprovalStatus",empID);
        if(status != "1")
        {
            lblEditStatus.Text = status.ToString();
            lblEditStatus.Visible = true;
        }
        else
            lblEditStatus.Visible = false;
    }


    private void PersonalDetails()
    {
        clsCommon open = new clsCommon();
        lblEmpId.Text = UM.EmployeeID.ToString();
        txtChnageUserid.Value = UM.EmployeeID.ToString();
        lblEmpName.Text = UM.Name;
        lblEmpAddress.Text = UM.CurrentAddress;//UM.Address;
        lblEmpContactNo.Text = UM.Contact;
        lblEmpEMail.Text = UM.EmailID;
        lblEmpSkills.Text = open.EmployeeSkillList(UM.SkillID.ToString(), "One").Rows[0]["SkillDesc"].ToString();
        lblEmpProbPeriod.Text = Convert.ToString(UM.ProbationPeriod);
        lblEmpJoinDate.Text = Convert.ToString(UM.JoiningDate);
        lblEmpADate.Text = Convert.ToString(UM.ADate);
        lblEmpBdate.Text = Convert.ToString(UM.BDate);
        lblEmpAccNo.Text = UM.AccountNo;
    }

    private void YourPackage()
    {
        List<PayrollBLL> lstGetPackage = new List<PayrollBLL>();
        lstGetPackage = PayrollBLL.GetPayrollDetails("GETMASTER", true, Convert.ToInt32(UM.LocationID), UM.EmployeeID);
        if (lstGetPackage.Count > 0)
        {

            lblEffectiveFrom.Text = DateTime.Parse(Convert.ToString(lstGetPackage[0].RevisionDate)).ToString("MMMM yyyy");
            if (Int32.Parse(lstGetPackage[0].Bonus.ToString()) > 0)
            {
                string Bonus = Strings.Format(lstGetPackage[0].Bonus, "0,00");
                if (lstGetPackage[0].PBB.ToString() == "0")
                {
                    Bonus = Bonus + "(fixed)";
                }
                else
                {
                    Bonus = Bonus + " (performance based)";
                }
                lblAnnualBonus.Text = Bonus;
            }
            //lblAnnualCTC.Text = Strings.Format(lstGetPackage[0].AnnualCTC, "0,00");
            lblAnnualCTC.Text = Strings.Format((lstGetPackage[0].MonthlyCTC * 12), "0,00");
            lblMonthlyGross.Text = Strings.Format(lstGetPackage[0].Gross, "0,00");

        }
        if (DateTime.Now.Day >= 10)
            tdPackage.Visible = true;
        else
            tdPackage.Visible = false;

    }

    protected void btnchangepass_Click(object sender, EventArgs e)
    {
        ///// SendChangepassMail(txtChnageUserid.Value);
        string message = string.Empty;
        UserMaster objUsermaster = new UserMaster();
        try
        {
            objUsermaster = UserMaster.changePassword(Convert.ToInt32(txtChnageUserid.Value), CSCode.Global.CreatePassword(txtpwd.Value), CSCode.Global.CreatePassword(txtNewPassword.Value));
        }
        catch (Exception ex)
        {
            objUsermaster.Message = "Change password operation failed";
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ChangePassword", "<script>alert('" +objUsermaster.Message + "');</script>", false);
    }

    protected StringBuilder CreateMsgBody(string strUserName, string strEmail, int intEmpID, bool isChangePassword)
    {
        StringBuilder mailBody = new StringBuilder();
        mailBody.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title></title></head><body>");
        mailBody.Append("<table  cellspacing=\"0\" border=\"0\" style=\"background-color: #FFCC00;border-width:1px;font-family:Gill Sans MT;font-size:12px;border-style:double; border-color:#FF9900;\">");
        mailBody.Append("<tr style=\"font-weight:bold;\"><td colspan=\"2\" style=\"padding-left:5px;padding-top:5px\">");
        //Add the header to the email body
        mailBody.Append("Dear " + strUserName + ",");
        mailBody.Append("<br/><br/>We are pleased to send your new password on your request. Please don't forward this mail to any email id for security purpose of your account.<br/><br/>");
        mailBody.Append("</td></tr>");
        Random r = new Random();
        int intRandom = r.Next();

        //string   strNewPassword = strUserName.Trim().Substring(0, 3) + intRandom.ToString().Substring(0, 4);
        string strNewPassword = string.Empty;
        if (!isChangePassword)
        {
            strNewPassword = strUserName.Trim().Substring(0, 3) + intRandom.ToString().Substring(0, 4);
        }
        else
        {
            strNewPassword = txtConfirmPassword.Value.Trim();
        }
        mailBody.Append("<tr>");
        mailBody.Append(string.Format("<td style=\"padding-left:5px;font-weight:bold\" colspan=\"1\">&nbsp;{0}</td>", "User ID:"));
        mailBody.Append(string.Format("<td style=\"width:90%;font-weight:bold\" colspan=\"1\">{0}</td>", intEmpID.ToString()));
        mailBody.Append("</tr>");

        mailBody.Append("<tr>");
        mailBody.Append(string.Format("<td style=\"padding-left:5px;font-weight:bold\" colspan=\"1\">&nbsp;{0}</td>", "Password:"));

        mailBody.Append(string.Format("<td style=\"font-weight:bold\" colspan=\"1\">{0}</td>", strNewPassword));

        mailBody.Append("</tr>");
        mailBody.Append("</table>");

        mailBody.Append("<br/> Regards,");
        mailBody.Append("<br/> Intelgain Pvt Ltd.");

        mailBody.Append("<br />");
        mailBody.Append("</body></html>");
        string strEncrptPassword = string.Empty;
        if (!isChangePassword)
        {
            strEncrptPassword = CreatrePassword(strNewPassword);
        }
        else
        {
            strEncrptPassword = CreatrePassword(txtConfirmPassword.Value.Trim());
        }

        clsCommon open = new clsCommon();
        open.UpdatePassword(strEmail, intEmpID, strEncrptPassword);

        return mailBody;
    }
    
    private string CreatrePassword(string strPassword)
    {
        string comppwd = null;
        string haskey = ConfigurationManager.AppSettings["hashKey"].ToString();
        byte tempbyte = byte.Parse("123");
        byte[] bytearr = new byte[2];
        bytearr[0] = tempbyte;
        return comppwd = SessionHelper.ComputeHash(strPassword, haskey, bytearr);
    }

    private void SendChangepassMail(string empid)
    {
        if (lblEmpId.Text.ToString().Trim() != "")
        {
            clsCommon open = new clsCommon();
            // Check if authorized user
            int EmpID = 0;
            try
            {
                EmpID = Convert.ToInt16(empid);
            }
            catch (Exception)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Not valid user id";
            }

            DataTable dtUserDetails = open.VerifyUser(EmpID);

            if (dtUserDetails.Rows.Count > 0)
            {
                if (dtUserDetails.Rows[0]["empPassword"].ToString() == CreatrePassword(txtpwd.Value))
                {
                    MailMessage msg = new MailMessage();
                    MailAddress msgFrom = new MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"),
                        ConfigurationManager.AppSettings.Get("compName"));
                    msg.From = msgFrom;

                    //  MailAddress msgBCC = new MailAddress(ConfigurationManager.AppSettings.Get("quotationEmailID"));
                    foreach (string emailAddress in dtUserDetails.Rows[0]["empEmail"].ToString().Trim().Split(','))
                    {
                        msg.To.Add(emailAddress);
                    }

                    msg.IsBodyHtml = true;
                    msg.Subject = "Agora - Password Changed";
                    msg.Body = CreateMsgBody(dtUserDetails.Rows[0]["empName"].ToString(), dtUserDetails.Rows[0]["empEmail"].ToString(), EmpID, true).ToString();
                    SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings.Get("SMTP"));
                    mailClient.Port = int.Parse(ConfigurationManager.AppSettings.Get("Port"));
                    mailClient.Credentials = new System.Net.NetworkCredential("smtp@intelgain.com", "intelgain");
                    // mailClient.EnableSsl = true;
                    try
                    {
                        mailClient.Send(msg);
                        lblStatus.Visible = true;
                        lblStatus.Text = "Your login details have been send to your mail.";
                        tdrow.Visible = true;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "duplicate", "<script>alert('your password has been changed successfully.');</script>", false);

                    }
                    catch (Exception ex)
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = ex.Message.ToString();
                        tdrow.Visible = true;
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Update", "<script>alert('Old password is not correct.');</script>", false);
            }
        }
    }

    protected void btnSaveProfile_Click(object sender, EventArgs e)
    {
        int empID = UM.EmployeeID;
        EmployeeMaster obj = new EmployeeMaster();
        string result = obj.SaveEmpProfile("SaveEmpProfile", empID, txtEditCAddress.Text.ToString(), txtEditContact.Text.ToString(), txtEditAnniversaryDate.Value.ToString(), empID);

        if (result != "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "callmsg();", true);
            lblEditStatus.Visible = true;
           // SendMail();
        }
    }

    private void SendMail()
    {
        if(UM ==null)
            UM = UserMaster.UserMasterInfo();

        string strBody, strSubject, mailTo, mailFrom, message, CC = "";
        strBody = UM.Name.ToString() + " " + "has submitted for change of current address.";
        strSubject = UM.Name.ToString() + " has submitted for change of current address.";
        mailTo =    ConfigurationManager.AppSettings.Get("HREmail").ToString(); 
        mailFrom  = UM.EmailID.ToString();
        message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, ""); 
    }
}