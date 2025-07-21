using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using dwtDAL;
using System.Xml;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.DirectoryServices;
using Customer.BLL;
using Org.BouncyCastle.Ocsp;


public partial class Member_NewEmployee : Authentication
{
    string TargetDir = "";
    string XMLFile = "";
    static UserMaster UM;
    clsCommon objCommon = new clsCommon();


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            UM = UserMaster.UserMasterInfo();
            Admin MasterPage = (Admin)Page.Master;

            MasterPage.MasterInit(false, true, false, true);

            TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\CV\";


            //if (!Directory.Exists(TargetDir))
            //{
            //    Directory.CreateDirectory(TargetDir);
            //}
            XMLFile = HttpContext.Current.Server.MapPath("~/Common/EmployeeColumnsList.xml");
            DataTable dtSL = objCommon.SecurityLevel(UM.EmployeeID);

            List<LocationBLL> lstLoc = LocationBLL.BindLocation("GetLocation");
            ddlLocation.DataSource = lstLoc;
            ddlLocation.DataValueField = "LocationID";
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("--Select Location--", "0"));



            ddlProfile.Items.Insert(0, new ListItem("--Select Profile--", "0"));

            List<DesignationBLL> lstDesignations = DesignationBLL.GetDesignation("GET");
            empSkill.DataSource = lstDesignations;
            empSkill.DataValueField = "DesigID";
            empSkill.DataTextField = "Designation";
            empSkill.DataBind();
            empSkill.Items.Insert(0, new ListItem("--Select Designation--", "0"));
            // empSkill.Items.Insert(new ListItem("Select", "0", true));

            List<TechicalSkills> lstPrimarySkill = TechicalSkills.GetTechSkills();
            ddlPrimarySkill.DataSource = lstPrimarySkill;
            ddlPrimarySkill.DataValueField = "techId";
            ddlPrimarySkill.DataTextField = "skillDesc";
            ddlPrimarySkill.DataBind();
            //BindProfile();
            dropempExpyears.Items.Add("-Select Year-");
            dropempExpmonths.Items.Add("-Select Month-");
            for (int i = 0; i < 26; i++)
            {
                dropempExpyears.Items.Add(i.ToString());

                if (i < 13)
                {
                    dropempExpmonths.Items.Add(i.ToString());
                }
            }
            ClearControls();
            // trProfileID.Visible = (UM.IsAdmin || UM.IsModuleAdmin) ? true : false;
            bool checkProfile = (UM.IsAdmin || UM.IsModuleAdmin) ? true : false;
            if (checkProfile)
            {
                hdfProfile.Value = "True";
                //hdfProfile.Value = "False";
            }
            else
            {
                hdfProfile.Value = "False";
                // hdfProfile.Value = "True";
            }           
        }

    }

    [System.Web.Services.WebMethod]
    public static String BindEmployee(string leavingstatus)
    {
        try
        {

            List<EmployeeMaster> lstEmployee = EmployeeMaster.GetEmployeeDetails("Select", Convert.ToInt32(HttpContext.Current.Session["LocationID"]), leavingstatus); //, Convert.ToInt32(HttpContext.Current.Session["ProjectID"])
            var data = from EmployeeItem in lstEmployee
                       orderby EmployeeItem.empid descending
                       select new
                       {

                           EmployeeItem.empid,
                           EmployeeItem.Photo,
                           EmployeeItem.empName,
                           EmployeeItem.empContact,
                           EmployeeItem.empEmail,
                           EmployeeItem.empAddress,
                           EmployeeItem.CAddress,
                           EmployeeItem.empGender,
                           EmployeeItem.empAccountNo,
                           EmployeeItem.EmpPAN,
                           EmployeeItem.EmpUAN,
                           EmployeeItem.EmpEPF,
                           EmployeeItem.empJoiningDate,
                           EmployeeItem.empExpectedLWD,//LWD
                           EmployeeItem.empBDate,
                           EmployeeItem.empExperince,
                           EmployeeItem.intelegainExperince,
                           EmployeeItem.empADate,
                           EmployeeItem.LocationFKID,
                           EmployeeItem.empNotes,
                           EmployeeItem.empPrevEmployer,
                           EmployeeItem.skillid,
                           EmployeeItem.Designation,
                           EmployeeItem.PrimarySkill,
                           EmployeeItem.empProbationPeriod,
                           EmployeeItem.empLeavingDate,
                           EmployeeItem.IsSuperAdmin,
                           EmployeeItem.IsAccountAdmin,
                           EmployeeItem.IsPayrollAdmin,
                           EmployeeItem.IsPM,
                           EmployeeItem.IsProjectReport,
                           EmployeeItem.IsProjectStatus,
                           EmployeeItem.IsLeaveAdmin,
                           EmployeeItem.IsActive,
                           EmployeeItem.empTester,
                           EmployeeItem.Resume,
                           EmployeeItem.EmpStatus,
                           EmployeeItem.ProjectID,
                           EmployeeItem.Net,
                           EmployeeItem.CTC,
                           EmployeeItem.AnnualCTC,
                           EmployeeItem.Gross,
                           EmployeeItem.Skill,
                           EmployeeItem.SecurityLevel,
                           EmployeeItem.PrimarySkillDesc,

                           EmployeeItem.Qualification,
                           EmployeeItem.QualificationId,
                           EmployeeItem.SecSkills,
                           EmployeeItem.SecSkillsId,
                           EmployeeItem.ProfileID,
                           EmployeeItem.Type,
                           EmployeeItem.Event,
                           EmployeeItem.ADUserName,
                           EmployeeItem.IFSCCode,
                           EmployeeItem.IsRemoteEmployee,
                           EmployeeItem.MSTeam


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
    public static String BindLocation()
    {
        try
        {
            List<LocationBLL> lstLoc = LocationBLL.BindLocation("GetLocation");
            var data = from lstLocItem in lstLoc
                       select new
                       {
                           lstLocItem.LocationID,
                           lstLocItem.Name

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
    public static String BindSecondarySkills()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(TechicalSkills.GetSecondarySkills());
    }

    [System.Web.Services.WebMethod]
    public static String BindQualification()
    {
        try
        {
            List<QualificationBLL> lstQual = QualificationBLL.GetQualification("SELECT");
            var data = from QualItem in lstQual
                       select new
                       {
                           QualItem.QID,
                           QualItem.QualDesc

                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //[System.Web.Services.WebMethod]
    //public static String GetSecondarySkills(int empId)
    //{
    //    return EmployeeMaster.GetSecondarySkills(empId, 0, "Select");
    //}

    //[System.Web.Services.WebMethod]
    //public static String GetQualifications(int empId)
    //{
    //    return EmployeeMaster.GetQualification(empId, 0, "Select");
    //}


    private void SaveSecondarySkills(int empId)
    {
        EmployeeMaster.SaveSecondarySkills(empId, 0, "Delete");
        string[] arrSecondarySkills = hfSecondarySkills.Value.Replace("'", "''").Split(',');
        for (int i = 0; i <= arrSecondarySkills.Count() - 1; i++)
        {
            if (arrSecondarySkills[i].Trim() != "")
            {
                try
                {
                    EmployeeMaster.SaveSecondarySkills(empId, Convert.ToInt32(arrSecondarySkills[i].ToString()), "Insert");
                }
                catch { }
            }
        }
    }

    private void SaveQualifications(int empId)
    {
        EmployeeMaster.SaveQualification(empId, 0, "Delete");

        string[] arrQualification = hfQualification.Value.Replace("'", "''").Split(',');
        for (int i = 0; i <= arrQualification.Count() - 1; i++)
        {
            if (arrQualification[i].Trim() != "")
            {
                try
                {
                    EmployeeMaster.SaveQualification(empId, Convert.ToInt32(arrQualification[i].ToString()), "Insert");
                }
                catch { }
            }
        }
    }

    protected void btn_No_click(object sender, EventArgs e)
    {
        return;
        //Response.Write("<script language=javascript>alert('Please Inform to the Project Manager and then Proceed')</script>");
    }
    protected void btn_yes_click(object sender, EventArgs e)
    {
        int intEmpID = 0;
        if (hdnempId.Value != "")
            intEmpID = Convert.ToInt32(hdnempId.Value.ToString());

        projectMember.DeleteTeamMemberFromAllProjects(intEmpID);

        btnSave_Click(sender, e);
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (Page.IsValid)
        {
            string strexistADusername = "";

            UM = UserMaster.UserMasterInfo();
            int intEmpID = 0;
            if (hdnempId.Value != "")
                intEmpID = Convert.ToInt32(hdnempId.Value.ToString());

            //Added by bhavana 24-06-2016
            if (intEmpID != 0)
            {
                strexistADusername = EmployeeMaster.GetExistsADUserName(intEmpID, "ExistsADemp");
            }
            //////

            string strGender = rbtnGender.SelectedValue;
            int strLocation = Convert.ToInt32(ddlLocation.SelectedValue);
            string strName = Convert.ToString(txtEmpName.Text).Trim();
            string strAddress = Convert.ToString(txtAddress.Text).Trim();
            string strCAddress = Convert.ToString(txtCAddress.Text).Trim();
            string strContact = Convert.ToString(txtContact.Text).Trim();
            int strSkill = Convert.ToInt32(empSkill.SelectedValue);
            string strNotes = Convert.ToString(txtNotes.Text).Trim();
            DateTime strJoiningDate = DateTime.ParseExact(txtJoiningDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            int intProbationPeriod = Convert.ToInt32(empProbationPeriod.SelectedValue);
            string strEmail = Convert.ToString(txtEmail.Text).Trim();
            string strAccountno = Convert.ToString(txtAccountNo.Value).Trim();
            string strPan = Convert.ToString(txtPan.Value).Trim();
            string strUan = Convert.ToString(txtUan.Value).Trim();
            string strEpfAcNo = Convert.ToString(txtEpfacno.Value).Trim();
            DateTime? strBDate = string.IsNullOrEmpty(txtBirthDate.Value) ? (DateTime?)null : DateTime.ParseExact(txtBirthDate.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime? strADate = string.IsNullOrEmpty(txtAnniversaryDate.Value) ? (DateTime?)null : DateTime.ParseExact(txtAnniversaryDate.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime? strExpectedLWD = string.IsNullOrEmpty(txtExpectedLWD.Value) ? (DateTime?)null : DateTime.ParseExact(txtExpectedLWD.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string strPrevEmployer = Convert.ToString(txtPrevEmployer.Text).Trim();
            string IFSCCode = Convert.ToString(txtIFSCCode.Value).Trim();
            string MSTeamID = Convert.ToString(txtMSTeamID.Text).Trim();
            Boolean IsRemoteEmployee = chkRemote.Checked;

            int intExperince = 0;
            if (intEmpID == 0)
                intExperince = Convert.ToInt16(dropempExpyears.Text) * 12 + Convert.ToInt16(dropempExpmonths.Text);
            else
                intExperince = Convert.ToInt16(hdnPrevExp.Value.ToString());
            DateTime strInsertedOn = DateTime.Now;
            int intInsertedBy = Convert.ToInt32(UM.EmployeeID);
            string strInsertedIP = Request.UserHostAddress;
            string strType = "";
            DateTime? strLeavingDate = string.IsNullOrEmpty(txtLeavingDate.Value) ? (DateTime?)null : DateTime.ParseExact(txtLeavingDate.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);


            string empstatus = !string.IsNullOrEmpty(txtLeavingDate.Value) ? "InActive" : ddlEmpStatus.SelectedValue;

            // Added by Trupti on 19 Jully 2018
            // string strProjID = "";
            if (intEmpID != 0 && empstatus == "InActive" && strLeavingDate != null)
            {
                string CurprojectName = "";
                string ProjectManagerName = "";
                lbl_EmpName.Text = strName.ToString();
                foreach (var item in projectMember.GetProjectDetailsByEmpId(intEmpID))
                {
                    CurprojectName = item.projectName + "," + CurprojectName;
                    ProjectManagerName = item.empName + "," + ProjectManagerName;
                    lbl_ProjectName.Text = CurprojectName;
                    lbl_projectManager.Text = ProjectManagerName;

                }
                if (!string.IsNullOrEmpty(CurprojectName))
                {

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowPopUpInActive();", true);
                    return;

                }


            }
            ////


            byte[] strPhoto = null;
            string strResume = "";
           
            string fileExtension = "";
            string fileExtension1 = "";
            string Docpath = string.Empty;
            string DocName = string.Empty;
            int intProfileID = (!Convert.ToString(hfProfileID.Value).Trim().Equals("")) ? Convert.ToInt32(hfProfileID.Value) : 0;

            if (!Convert.ToString(hdnempId.Value).Equals(string.Empty))
            {
                EmployeeMaster emp = EmployeeMaster.GetEmployeeDetails(Convert.ToInt32(hdnempId.Value));
                if (!filePhotoUpload.HasFile)
                    strPhoto = emp.Photo;

                HttpContext.Current.Session["attachment"] = FileAttachment.PostedFile.FileName;
                if (Convert.ToString(HttpContext.Current.Session["attachment"]) == "")
                {
                    strResume = emp.Resume;
                }

                
            }


            if (filePhotoUpload.HasFile)
                strPhoto = filePhotoUpload.FileBytes;

            int primarySkillId = Convert.ToInt32(ddlPrimarySkill.SelectedValue);
            string strADUserName = txtADUserName.Value.Trim();
            if(!string.IsNullOrEmpty(hdnempId.Value))
                intEmpID = Convert.ToInt32(hdnempId.Value);

                try
                {
                    //for Appointmnet//
                    
                    string filePath1 = fileAppointment.PostedFile.FileName;
                    string filename1 = Path.GetFileName(filePath1);
                    fileExtension1 = Path.GetExtension(filename1);
                    string contenttype1 = String.Empty;


                    if ((fileAppointment.PostedFile != null) && (fileAppointment.PostedFile.ContentLength > 0))
                    {
                        switch (fileExtension1)
                        {
                            //case ".doc":
                            //    contenttype1 = "application/vnd.ms-word";
                            //    break;

                            //case ".docx":
                            //    contenttype1 = "application/vnd.ms-word";
                            //    break;
                            case ".pdf":
                                contenttype1 = "application/pdf";
                                break;
                        }


                        if (contenttype1 != String.Empty)
                        {
                            int index = filename1.IndexOf(".");
                            if (index > 0)
                                filename1 = filename1.Substring(0, index);
                            if (fileExtension1 == ".pdf" /*|| fileExtension == ".DOCX" || fileExtension == ".Docx"*/)
                            {
                                //fileExtension1 = ".doc";
                            }
                            if (string.IsNullOrEmpty(hdnAppointmentname.Value))
                                hdnAppointmentname.Value = fileAppointment.FileName;
                            // strResume = hdnAppointmentname.Value;
                        }

                        //for attachements //
                        string filePath = FileAttachment.PostedFile.FileName;
                        string filename = Path.GetFileName(filePath);
                        fileExtension = Path.GetExtension(filename);
                        string contenttype = String.Empty;

                        if ((FileAttachment.PostedFile != null) && (FileAttachment.PostedFile.ContentLength > 0))
                        {
                            switch (fileExtension)
                            {
                                case ".doc":
                                    contenttype = "application/vnd.ms-word";
                                    break;

                                case ".docx":
                                    contenttype = "application/vnd.ms-word";
                                    break;
                                case ".pdf":
                                    contenttype = "application/pdf";
                                    break;
                            }
                            if (contenttype != String.Empty)
                            {
                                int index = filename.IndexOf(".");
                                if (index > 0)
                                    filename = filename.Substring(0, index);
                                if (fileExtension == ".docx" || fileExtension == ".DOCX" || fileExtension == ".Docx")
                                {
                                    fileExtension = ".doc";
                                }
                                if (string.IsNullOrEmpty(hdnDocName.Value))
                                    hdnDocName.Value = FileAttachment.FileName;
                                strResume = hdnDocName.Value;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }
              
            //added strExpectedLWD by VW 8 April 2021
            int outputID = 0;
            if (Convert.ToString(HttpContext.Current.Session["attachment"]) == "")
            {
                outputID = EmployeeMaster.Save(intEmpID, strLocation, strName, strAddress, strGender, strContact, strSkill, strNotes, strJoiningDate, intProbationPeriod, strEmail, strAccountno,
                   strBDate, strADate, strExpectedLWD, strPrevEmployer, intExperince, strInsertedOn, intInsertedBy, strInsertedIP, strType, strLeavingDate, strResume, strPhoto, primarySkillId, empstatus, intProfileID, strCAddress, strADUserName, strPan, strUan, strEpfAcNo, IFSCCode, MSTeamID, IsRemoteEmployee);
            }
            else
            {
                strResume = HttpContext.Current.Session["attachment"].ToString();
                outputID = EmployeeMaster.Save(intEmpID, strLocation, strName, strAddress, strGender, strContact, strSkill, strNotes, strJoiningDate, intProbationPeriod, strEmail, strAccountno,
               strBDate, strADate, strExpectedLWD, strPrevEmployer, intExperince, strInsertedOn, intInsertedBy, strInsertedIP, strType, strLeavingDate, strResume, strPhoto, primarySkillId, empstatus, intProfileID, strCAddress, strADUserName, strPan, strUan, strEpfAcNo, IFSCCode, MSTeamID, IsRemoteEmployee);

            }
            SaveSecondarySkills(outputID);
            SaveQualifications(outputID);
            //For Appointment//
            if ((fileAppointment.PostedFile != null) && (fileAppointment.PostedFile.ContentLength > 0))
                if (fileExtension1.ToUpper().Contains(".DOC") || fileExtension1.ToUpper().Contains(".PDF"))
                {
                    TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\Employee_Appointment\";

                    if (!Directory.Exists(TargetDir))
                    {
                        Directory.CreateDirectory(TargetDir);
                    }

                    fileAppointment.SaveAs(TargetDir + "Employee_Appointment" + "_" + outputID + fileExtension1);

                }
            //For Attachment//
            if ((FileAttachment.PostedFile != null) && (FileAttachment.PostedFile.ContentLength > 0))
                if (fileExtension.ToUpper().Contains(".DOC") || fileExtension.ToUpper().Contains(".PDF"))
                {
                    TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\CV\";

                    if (!Directory.Exists(TargetDir))
                    {
                        Directory.CreateDirectory(TargetDir);
                    }

                    FileAttachment.SaveAs(TargetDir + outputID + "_" + txtEmpName.Text + fileExtension);
                }

            //Added by bhavana 24-06-2016
            if (intEmpID == 0 || (strexistADusername != "" && strexistADusername != strADUserName))
            {
                bool blcreated = AddADUserInfo(outputID, strexistADusername, strADUserName);
                if (blcreated)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Save", "<script>alert('Record saved successfully, and also AD user created successfully.');</script>", false);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Save", "<script>alert('Record saved successfully, you don't have permissions to create AD user.');</script>", false);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Save", "<script>alert('Record saved successfully.');</script>", false);

            }

            if (empstatus == "InActive")
            {
                InactiveAddADUserInfo(outputID, strexistADusername);
            }

            photoImage.Src = "/Member/Services/ViewImage.ashx?id=" + intEmpID;
            //  Response.Redirect("/Member/Employee.aspx?" + strAccountno.ToString());
            Response.Redirect("/Member/Employee.aspx");

        }
    }

    private void SendMail(string empid, string Mode)
    {
        if (empid != "")
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

            }


            if (Convert.ToInt32(hdnempId.Value) > 0)
            {
                MailMessage msg = new MailMessage();
                MailAddress msgFrom = new MailAddress(System.Configuration.ConfigurationManager.AppSettings.Get("fromEmail"),
                    System.Configuration.ConfigurationManager.AppSettings.Get("compName"));
                msg.From = msgFrom;

                msg.To.Add(txtEmail.Text);



                msg.IsBodyHtml = true;
                msg.Subject = "Agora - Account Details";

                msg.Body = CreateMsgBody(txtEmpName.Text, txtEmail.Text, EmpID, Mode).ToString();

                SmtpClient mailClient = new SmtpClient();
                mailClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings.Get("SMTP"));
                mailClient.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Port"));
                mailClient.Credentials = new System.Net.NetworkCredential("smtp@intelgain.com", "intelgain");
                mailClient.EnableSsl = true;

                try
                {
                    mailClient.Send(msg);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }

    protected StringBuilder CreateMsgBody(string strUserName, string strEmail, int intEmpID, string Mode)
    {
        StringBuilder mailBody = new StringBuilder();
        mailBody.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title></title></head><body>");
        mailBody.Append("<table  cellspacing=\"0\" border=\"0\" style=\"background-color: #FFCC00;border-width:1px;font-family:Gill Sans MT;font-size:12px;border-style:double; border-color:#FF9900;\">");
        mailBody.Append("<tr style=\"font-weight:bold;\"><td colspan=\"2\" style=\"padding-left:5px;padding-top:5px\">");
        //Add the header to the email body
        mailBody.Append("Dear " + strUserName + ",");
        mailBody.Append("<br/><br/>We are pleased to send you your account details. Please don't forward this mail to any email id for security purpose of your account.");

        mailBody.Append("</td></tr>");
        Random r = new Random();
        int intRandom = r.Next();
        //string strPassword = "dynamic";

        string strNewPassword = string.Empty;
        if (Mode == "EDIT")
        {
            strNewPassword = strUserName.Trim().Substring(0, 3) + intRandom.ToString().Substring(0, 4);
        }
        else
        {
            strNewPassword = "dynamic";
        }

        mailBody.Append("<tr>");
        mailBody.Append("<td style=\"padding-left:5px;padding-top:5px;font-weight:bold\" colspan=\"1\">User ID:</td>");
        mailBody.Append("<td style=\"width:90%;font-weight:bold\" colspan=\"1\">" + intEmpID.ToString() + "</td>");
        mailBody.Append("</tr>");

        mailBody.Append("<tr>");
        mailBody.Append("<td style=\"padding-left:5px;font-weight:bold\" colspan=\"1\">Password:</td>");

        mailBody.Append("<td style=\"font-weight:bold\" colspan=\"1\">" + strNewPassword + "</td>");

        mailBody.Append("</tr>");
        mailBody.Append("<tr style=\"font-weight:bold;\"><td colspan=\"2\" style=\"padding-left:5px;padding-top:5px\"><a href='http://emp.intelgain.com/Member/Login.aspx'>To login please click here</a><td/><tr/>");

        mailBody.Append("<tr style=\"font-weight:bold;\"><td colspan=\"2\" style=\"padding-left:5px;padding-top:5px\"><br>Regards,");
        mailBody.Append("<br/> Intelgain Pvt Ltd.<br/><td></tr>");
        mailBody.Append("</table>");
        mailBody.Append("</body></html>");

        string strEncrptPassword = string.Empty;
        if (Mode == "EDIT")
        {
            strEncrptPassword = CreatrePassword(strNewPassword);
            clsCommon open = new clsCommon();
            open.UpdatePassword(strEmail, intEmpID, strEncrptPassword);
        }
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

    protected void btnSendMail_Click(object sender, EventArgs e)
    {

        int intEmpID = Convert.ToInt32(hdnempId.Value);

        SendMail(intEmpID.ToString(), "EDIT");
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Email", "<script>alert('Mail sent successfully.');</script>", false);
    }

    private void ClearControls()
    {
        txtEmpName.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtContact.Text = string.Empty;
        txtBirthDate.Value = string.Empty;
        txtAnniversaryDate.Value = string.Empty;
        txtExpectedLWD.Value = string.Empty;
        txtAccountNo.Value = string.Empty;
        txtPrevEmployer.Text = string.Empty;
        ddlEmpStatus.SelectedValue = "Active";
        txtJoiningDate.Text = string.Empty;
        txtLeavingDate.Value = string.Empty;
        dropempExpyears.SelectedValue = string.Empty;
        dropempExpmonths.SelectedValue = string.Empty;
        lblPreExp.Text = string.Empty;
        empProbationPeriod.SelectedValue = "3";
        empSkill.SelectedValue = "100";
        ddlPrimarySkill.SelectedValue = "0";
        lstempQual.Value = string.Empty;
        lstSecondarySkill.Value = string.Empty;
        txtNotes.Text = string.Empty;
        rbtnGender.SelectedValue = string.Empty;
        photoImage.Src = "";
        hdnDocName.Value = "";
         hndExperienceStore.Value = "";
        hfSecondarySkills.Value = "";
        hfQualification.Value = "";
        hdnempId.Value = "";
        txtADUserName.Value = string.Empty;
        txtIFSCCode.Value = string.Empty;
        txtMSTeamID.Text = string.Empty;
        chkRemote.Checked =true;
    }

    [System.Web.Services.WebMethod]
    public static String GetExistsClient(int EmpID, string EmailID)
    {

        return EmployeeMaster.GetExistsClient(EmpID, EmailID, "Exists");

    }

    [System.Web.Services.WebMethod]
    public static string BindProfile(int LocationId)
    {

        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(EmployeeMaster.BindProfile(LocationId).ToList());

    }

    [System.Web.Services.WebMethod]
    public static String GetTimeSheet(int empID)
    {
        try
        {
            List<EmployeeMaster> lstTimeSheet = EmployeeMaster.GetTimeSheet("EmployeeTS", empID);
            var data = from item in lstTimeSheet
                       select new
                       {
                           item.empName, // <-- date
                           item.empNotes, // <-- hours
                           item.Net
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
    public static String GetEmpHistory(int empID)
    {
        try
        {
            List<EmployeeMaster> lstHistory = EmployeeMaster.GetEmpHistory("GetEmployeeHistory", empID);
            var data = from item in lstHistory
                       select new
                       {
                           item.HistoryID,
                           item.CAddress,
                           item.empContact,
                           item.InsertedIP, // <-- annivarsary date
                           item.empAccountNo,
                           item.EmpStatus,
                           item.empName,
                           item.ModifiedIP  // <-- modified date
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
    public static String SaveApprovedData(int empID, int HID, int approvalStatus)
    {
        try
        {
            string result = EmployeeMaster.SaveApprovedData("UpdateApprovedData", empID, HID, approvalStatus, Convert.ToInt32(UM.EmployeeID));

            return result;

        }
        catch (Exception ex)
        {
            return null;
        }
    }
    protected void chkRemote_CheckedChanged(object sender, EventArgs e)
    {
        hdfRemoteValue.Value = chkRemote.Checked ? "1" : "0";
    }


    [System.Web.Services.WebMethod]
    public static String GetExistsADUserName(int EmpID, string ADUserName)
    {

        return EmployeeMaster.GetExistsADUserName(EmpID, ADUserName, "ExistsADUserName");

    }

    #region "ADUser"
    public struct ADUserInfo
    {
        public string username;
        public string sAMAccountName;
        public string password;
        public string firstname;
        public string initials;
        public string lastName;
        public string displayName;
        public string telephoneNumber;
        public string emailAddress;
    }

    /// <summary>
    /// Get Existing AD user list
    /// Added by bhavana 24-06-2016
    /// </summary>
    /// <returns>list of AD user</returns>
    public List<ADUserInfo> GetADUsers()
    {
        List<ADUserInfo> lstADUsers = new List<ADUserInfo>();
        try
        {
            //Directory path
            string DomainPath = Convert.ToString(ConfigurationManager.AppSettings["DirectoryPath"]);
            DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
            //find organizational Unit directory
            DirectoryEntry organizationalUnit = searchRoot.Children.Find("OU=intelgain EMP", "organizationalUnit");
            DirectorySearcher search = new DirectorySearcher(organizationalUnit);
            //add filter and properties
            search.Filter = "(&(objectClass=top)(objectClass=person)(objectClass=organizationalPerson)(objectClass=user))";
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("usergroup");
            search.PropertiesToLoad.Add("displayname");
            SearchResult result;
            SearchResultCollection resultCol = search.FindAll();
            if (resultCol != null)
            {
                for (int counter = 0; counter < resultCol.Count; counter++)
                {
                    string UserNameEmailString = string.Empty;
                    result = resultCol[counter];
                    if (result.Properties.Contains("samaccountname"))
                    {
                        ADUserInfo objUsers = new ADUserInfo();
                        objUsers.sAMAccountName = (String)result.Properties["samaccountname"][0];
                        objUsers.displayName = (String)result.Properties["displayName"][0];
                        lstADUsers.Add(objUsers);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
        return lstADUsers;
    }

    /// <summary>
    /// inactive AD user only for exsting user
    /// </summary>
    /// <param name="EmpID">Employee ID eg-1000</param>
    /// <param name="strexistADuser"></param>
    public void InactiveAddADUserInfo(int EmpID, string strexistADuser)
    {
        try
        {
            string LdapPath = Convert.ToString(ConfigurationManager.AppSettings["DirectoryPath"]);

            DirectoryEntry direntry = new DirectoryEntry(LdapPath);

            direntry.AuthenticationType = AuthenticationTypes.Secure;

            DirectoryEntry organizationalUnit = direntry.Children.Find("OU=intelgain EMP", "organizationalUnit");

            ADUserInfo userinfo = new ADUserInfo();
            string[] strName = txtEmpName.Text.Trim().Split(' ');
            if (strName.Count() > 1)
            {
                userinfo.firstname = strName[0];
                userinfo.lastName = strName[1];
            }
            else
            {
                userinfo.firstname = strName[0];
                userinfo.lastName = "";
            }
            userinfo.displayName = userinfo.firstname + " " + userinfo.lastName;
            userinfo.sAMAccountName = EmpID.ToString();
            userinfo.username = EmpID.ToString();

            if (organizationalUnit != null)
            {
                DirectoryEntries users = organizationalUnit.Children;

                //for existing user
                if (strexistADuser != "" && strexistADuser != null)
                {
                    List<ADUserInfo> lstADUsers = new List<ADUserInfo>();
                    lstADUsers = GetADUsers();

                    ADUserInfo ADUser = lstADUsers.Where(obj => obj.sAMAccountName != null && obj.sAMAccountName != ""
                        && obj.sAMAccountName == strexistADuser).ToList().FirstOrDefault();

                    string existuser = ADUser.sAMAccountName != "" ? ADUser.sAMAccountName : "";
                    string existdisplayname = ADUser.displayName != "" ? ADUser.displayName : "";
                    if (existdisplayname != "" && existdisplayname != null)
                    {
                        DirectoryEntry finduser = users.Find("CN=" + existdisplayname, "user");
                        if (finduser != null)
                        {
                            //inactive user
                            finduser.Properties["userAccountControl"].Value = 2;
                            finduser.CommitChanges();
                            direntry.Close();
                            finduser.Close();
                            //return true;
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {

            ex.Message.ToString();


            //return false;
        }
        //return false;
    }

    /// <summary>
    /// Create AD User
    /// Added by bhavana 24-06-2016
    /// </summary>
    /// <param name="EmpID">Employee ID eg-1000</param>
    /// <param name="strexistADuser">DB ADUserName</param>
    /// <param name="strupdateADUserName">update ADUserName</param>
    /// <returns>bool value</returns>
    public bool AddADUserInfo(int EmpID, string strexistADuser, string strupdateADUserName)
    {
        try
        {
            string LdapPath = Convert.ToString(ConfigurationManager.AppSettings["DirectoryPath"]);

            DirectoryEntry direntry = new DirectoryEntry(LdapPath);

            direntry.AuthenticationType = AuthenticationTypes.Secure;

            DirectoryEntry organizationalUnit = direntry.Children.Find("OU=intelgain EMP", "organizationalUnit");

            ADUserInfo userinfo = new ADUserInfo();
            string[] strName = txtEmpName.Text.Trim().Split(' ');
            if (strName.Count() > 1)
            {
                userinfo.firstname = strName[0];
                userinfo.lastName = strName[1];
            }
            else
            {
                userinfo.firstname = strName[0];
                userinfo.lastName = "";
            }
            userinfo.displayName = userinfo.firstname + " " + userinfo.lastName;
            userinfo.sAMAccountName = EmpID.ToString();
            userinfo.username = EmpID.ToString();

            if (organizationalUnit != null)
            {
                DirectoryEntries users = organizationalUnit.Children;

                //for existing user
                if (strexistADuser != "" && strexistADuser != null)
                {
                    List<ADUserInfo> lstADUsers = new List<ADUserInfo>();
                    lstADUsers = GetADUsers();

                    ADUserInfo ADUser = lstADUsers.Where(obj => obj.sAMAccountName != null && obj.sAMAccountName != ""
                        && obj.sAMAccountName == strexistADuser).ToList().FirstOrDefault();

                    string existuser = ADUser.sAMAccountName != "" ? ADUser.sAMAccountName : "";
                    string existdisplayname = ADUser.displayName != "" ? ADUser.displayName : "";
                    if (existdisplayname != "" && existdisplayname != null)
                    {
                        DirectoryEntry finduser = users.Find("CN=" + existdisplayname, "user");
                        if (finduser != null && strupdateADUserName != "")
                        {
                            finduser.Properties["givenName"].Value = userinfo.firstname;
                            finduser.Properties["sn"].Value = userinfo.lastName;
                            finduser.Properties["userPrincipalName"].Value = strupdateADUserName + "@intelgain.com";
                            finduser.Properties["sAMAccountName"].Value = strupdateADUserName;
                            finduser.Properties["displayName"].Value = userinfo.displayName;
                            finduser.CommitChanges();
                            direntry.Close();
                            finduser.Close();
                            return true;
                        }
                    }
                }
                else
                {
                    //new user
                    DirectoryEntry newuser = users.Add("CN=" + userinfo.displayName, "user");

                    newuser.Properties["givenName"].Value = userinfo.firstname;
                    newuser.Properties["sn"].Value = userinfo.lastName;
                    newuser.Properties["userPrincipalName"].Value = userinfo.username + "@intelgain.com";
                    newuser.Properties["sAMAccountName"].Value = userinfo.sAMAccountName;
                    newuser.Properties["displayName"].Value = userinfo.displayName;
                    newuser.CommitChanges();
                    newuser.Invoke("setpassword", "dynamic");
                    newuser.Properties["userAccountControl"].Value = 512;
                    newuser.Properties["pwdLastSet"].Value = 0;
                    newuser.CommitChanges();
                    direntry.Close();
                    newuser.Close();

                    return true;
                }
            }
        }

        catch (Exception)
        {
            return false;
        }
        return false;
    }
    #endregion "ADUser"

    #region"Note"
    [System.Web.Services.WebMethod]
    public static String GetNoteTypeId(string mode, string noteType)
    {
        return NotesBLL.GetNoteTypeId(mode, noteType);
    }

    #endregion"Note"

    //protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int selectedLocation = Convert.ToInt32(ddlLocation.SelectedValue);
    //    BindProfile(selectedLocation);
    //}

    //public void BindProfile(int locationId)
    //{
    //    ddlProfile.DataSource = new EmployeeMaster().BindProfile(locationId);
    //    ddlProfile.DataValueField="ProfileID";
    //    ddlProfile.DataTextField="ProfileName";
    //    ddlProfile.DataBind();
    //}
}




//SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
//SqlCommand cmd = new SqlCommand("Select ProfileId,Name from Profile", con);
//SqlDataAdapter da = new SqlDataAdapter(cmd);
//DataSet ds = new DataSet();
//da.Fill(ds);
//ddlProfile.DataTextField = ds.Tables[0].Columns["Name"].ToString();
//ddlProfile.DataValueField = ds.Tables[0].Columns["ProfileId"].ToString();
//ddlProfile.DataSource = ds.Tables[0];
//ddlProfile.DataBind();
//ddlProfile.Items.Insert(0, new ListItem("--Select Profile--", "0"));
//con.Close();

