using dwtDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing.Imaging;
using WebSupergoo.ABCpdf6;


public partial class Member_Default : Authentication
{
    #region Variables.
    private const int intModuleID = 6;
    clsCommon objCommon = new clsCommon();
    public int intYears = 0;
    public int intMonth = 0;
    public int intDays = 0;
    int intIndex = 0;
    public static int intMEmpID = 0;
    const int rowLimit = 65000;
    public string filePath;
    string filenameData;
    //string strFolder = System.Configuration.ConfigurationManager.AppSettings["uploadPath"].ToString();
    string TargetDir = "";
    string XMLFile = "";
    string Uploadedfiles;
    string fileExtension = "";
    string NameData;
    byte[] photoimg = null;
    UserMaster UM;
 
    #endregion

    #region Events.
     
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Form.Attributes.Add("enctype", "multipart/form-data");
      
         UM = UserMaster.UserMasterInfo();
      

        TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\CV\";

        if (!Directory.Exists(TargetDir))
        {
            Directory.CreateDirectory(TargetDir);
        }

        XMLFile = HttpContext.Current.Server.MapPath("~/Common/EmployeeColumnsList.xml");
        if (!IsPostBack)
        {
            DataTable dtSL = objCommon.SecurityLevel(UM.EmployeeID);
            pnlEmployeeAdd.Visible = false;
            btnView.Visible = false;
            DataTable dtQualification = objCommon.EmployeeQualificationList(0);
            lstempQual.DataSource = dtQualification;
            lstempQual.DataTextField = "qualificationDesc";
            lstempQual.DataBind();

            DataTable dtSkill = objCommon.EmployeeSkillList(Convert.ToInt32(dtSL.Rows[0]["SecurityLevel"]));
            empSkill.DataSource = dtSkill;
            empSkill.DataTextField = "skillDesc";
            empSkill.DataBind();

            DataTable techSkills = objCommon.GetTechSkills(0);
            lstSecondarySkill.DataSource = ddlPrimarySkill.DataSource = techSkills;
            lstSecondarySkill.DataValueField = ddlPrimarySkill.DataValueField = "techId";
            lstSecondarySkill.DataTextField = ddlPrimarySkill.DataTextField = "skillDesc";
            ddlPrimarySkill.DataBind();
            lstSecondarySkill.DataBind();

            for (int i = 0; i < 26; i++)
            {
                dropempExpyears.Items.Add(i.ToString());
                if (i < 13)
                {
                    dropempExpmonths.Items.Add(i.ToString());
                }
            }

            if (!string.IsNullOrEmpty(UM.ProfileID))
            {
                hdLocationId.Value = objCommon.GetLocationAcess(UM.ProfileID).ToString();
                hdHasAllLocationAcess.Value = (hdLocationId.Value.Equals("0") ? "1" : "0");
            }

            BindLocation();
            BindSearchData();

        }

        BindData();
        this.Form.DefaultButton = this.btnSearch.UniqueID;
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {

        if (btnAdd.Text == "ADD")
        {
            pnlEmployeeAdd.Visible = true;
            pnlEmployeeView.Visible = false;
            btnCancel.Visible = true;
            btnAdd.Visible = false;
            txtempid.Visible = false;
            Editempid.Visible = false;
            ClearFields();
            intDays = 0;
            intYears = 0;
            intMonth = 0;
            lblYear.Visible = true;
            lblMonth.Visible = true;
            dropempExpmonths.Visible = true;
            dropempExpyears.Visible = true;
            lblExp.Text = "";
            hndExperienceStore.Value = "0";
            btnSendMail.Visible = false;
            //lblMessage.Text = "";
            hdnDocName.Value = "0";
            hdnDocName.Value = "";
            lblUploadedName.Text = "";
            photoImage.Src = "../Images/NoImage.jpg";

        }
        else if (btnAdd.Text == "EDIT")
        {

            DisableOrEnableControls(true);

            txtempid.Visible = true;
            //empEmail.Enabled = false;
            Editempid.Visible = true;
            btnSave.Visible = true;
            btnSave.Text = "UPDATE";
            btnCancel.Text = "CANCEL";
            btnCancel.Visible = true;
            btnAdd.Visible = false;
            btnView.Visible = false;
            lblYear.Visible = true;
            lblMonth.Visible = true;
            dropempExpmonths.Visible = true;
            dropempExpyears.Visible = true;

            btnSave.CommandArgument = btnAdd.CommandArgument.ToString();
            CalculateTotalExp(empJoiningDate.Text, Convert.ToInt32(hndExperienceStore.Value), true);

        }
    }

    private Boolean InsertUpdateData(SqlCommand cmd)
    {
        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        pnlEmployeeAdd.Visible = false;
        pnlEmployeeView.Visible = true;
    }

    protected void gvEmployeeView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (e.NewPageIndex != -1 && e.NewPageIndex != gvEmployeeView.PageCount)
            {
                gvEmployeeView.PageIndex = e.NewPageIndex;
                BindData();
            }
        }
        catch
        {

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
       
        int intexperience = 0;
        int intEmpID = 0;
        int skillid = 0;
        string fileExtension = "";
        string Docpath = string.Empty;
        string DocName = string.Empty;
        try
        {
            string filePath = fileUploadImage.PostedFile.FileName;

            string filename = Path.GetFileName(filePath);
            fileExtension = Path.GetExtension(filename);
            string contenttype = String.Empty;

            if ((fileUploadImage.PostedFile != null) && (fileUploadImage.PostedFile.ContentLength > 0))
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
                        hdnDocName.Value = fileUploadImage.FileName;
                }
            }
        }

        catch (Exception ex)
        {
            Response.Write(ex);
        }
        if (btnSave.Text == "SAVE")
        {
            if (Page.IsValid)
            {
                try
                {
                    lblResult.Visible = false;
                    if (filePhotoUpload.HasFile)
                        photoimg = filePhotoUpload.FileBytes;

                    intexperience = Convert.ToInt16(dropempExpyears.Text) * 12 + Convert.ToInt16(dropempExpmonths.Text);
                    skillid = Convert.ToInt16(((DataTable)objCommon.GetSkillId(empSkill.Text)).Rows[0][0].ToString());

                    objCommon.Employee(ddlLocationList.SelectedValue, empName.Text.ToString(),
                                          empAddress.Text.ToString(),
                                          empContact.Text.ToString(),
                                          skillid,
                                          empNotes.Text.ToString(),
                                          empJoiningDate.Text,
                                          empProbationPeriod.Text.ToString(),
                                          empEmail.Text.ToString(),
                                          empAccountno.Text.ToString(),
                                          empBDate.Text.ToString() == "" ? "Null" : "'" + empBDate.Text.ToString() + "'",
                                          empADate.Text.ToString() == "" ? "Null" : "'" + empADate.Text.ToString() + "'",
                                          empPrevEmployer.Text.ToString(),
                                          intexperience,
                                          DateTime.Now.ToString("dd/MM/yyyy"),
                                          Convert.ToInt32(UM.EmployeeID),
                                        
                                          Request.UserHostAddress, ref intEmpID, "Insert",
                                          empLeavingDate.Text.Trim() == "" ? "Null" : "'" + empLeavingDate.Text.Trim() + "'",
                                          right2.Checked,
                                          hdnDocName.Value, photoimg, ddlPrimarySkill.SelectedValue);


                    lstempQual.DataBind();
                    SendMail(intEmpID.ToString(), "ADD");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Save", "<script>alert('Record saved successfully.');</script>", false);
                    photoImage.Src = "/Member/Services/ViewImage.ashx?id=" + intEmpID;
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
            else
                lblResult.Visible = true;
        }

        else if (btnSave.Text == "UPDATE")
        {
            string filePath = fileUploadImage.PostedFile.FileName;

            string filename = Path.GetFileName(filePath);
            fileExtension = Path.GetExtension(filename);
            string contenttype = String.Empty;

            if ((fileUploadImage.PostedFile != null) && (fileUploadImage.PostedFile.ContentLength > 0))
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
                    //some issues in docx so replace doc "Unable to find pdf header" might be openoffice.org not installed
                    if (fileExtension == ".docx" || fileExtension == ".DOCX" || fileExtension == ".Docx")
                    {
                        fileExtension = ".doc";
                    }
                    hdnDocName.Value = fileUploadImage.FileName;

                }
            }

            lblUploadedName.Text = hdnDocName.Value;

            if (Page.IsValid)
            {
                intEmpID = Convert.ToInt16(btnSave.CommandArgument);
                if (filePhotoUpload.HasFile)
                    photoimg = filePhotoUpload.FileBytes;


                skillid = Convert.ToInt16(((DataTable)objCommon.GetSkillId(empSkill.Text)).Rows[0][0].ToString());
                intexperience = Convert.ToInt16(dropempExpyears.Text) * 12 + Convert.ToInt16(dropempExpmonths.Text);
                objCommon.Employee(ddlLocationList.SelectedValue, empName.Text.ToString(),
                                          empAddress.Text.ToString(),
                                          empContact.Text.ToString(),
                                          skillid,
                                          empNotes.Text.ToString(),
                                          "'" + empJoiningDate.Text + "'",
                                          empProbationPeriod.Text.ToString(),
                                          empEmail.Text.ToString(),
                                          empAccountno.Text.ToString(),
                                          empBDate.Text.ToString() == "" ? "Null" : "'" + empBDate.Text.ToString() + "'",
                                          empADate.Text.ToString() == "" ? "Null" : "'" + empADate.Text.ToString() + "'",
                                          empPrevEmployer.Text.ToString(),
                                          intexperience,
                                          DateTime.Now.ToString("dd/MM/yyyy"),
                                           Convert.ToInt32(UM.EmployeeID),
                                      
                                          Request.UserHostAddress,
                                          ref intEmpID,
                                          "Update",
                                          empLeavingDate.Text.Trim() == "" ? "Null" : "'" + empLeavingDate.Text.Trim() + "'",
                                          right2.Checked, hdnDocName.Value, photoimg, ddlPrimarySkill.SelectedValue);



                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Update", "<script>alert('Record updated successfully');</script>", false);
            }
        }

        InsertUpdateEmpQualification(intEmpID);
        SaveSecondarySkills(intEmpID);

        if ((fileUploadImage.PostedFile != null) && (fileUploadImage.PostedFile.ContentLength > 0))
            if (fileExtension.ToUpper().Contains(".DOC") || fileExtension.ToUpper().Contains(".PDF"))
            {
                fileUploadImage.SaveAs(TargetDir + intEmpID + "_" + empName.Text + fileExtension);
            }

        lblYear.Visible = false;
        lblMonth.Visible = false;
        dropempExpmonths.Visible = false;
        dropempExpyears.Visible = false;
        CalculateTotalExp(empJoiningDate.Text, Convert.ToInt32(hndExperienceStore.Value), true);
        btnCancel_Click(null, null);
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

    protected void dropempExpyears_SelectedIndexChanged(object sender, EventArgs e)
    {
        hndExperienceStore.Value = Convert.ToInt32(Convert.ToInt16(dropempExpyears.Text) * 12 + Convert.ToInt16(dropempExpmonths.Text)).ToString();
        if (empJoiningDate.Text != "")
        {
            CalculateTotalExp(empJoiningDate.Text, Convert.ToInt32(hndExperienceStore.Value), true);
        }
        else
        {
            hndExperienceStore.Value = "0";
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Date", "<script>alert('Enter Joining date');</script>", false);
            empJoiningDate.Focus();
        }
        fileUploadImage.Attributes.Add("Value", hdnDocName.Value);
    }

    protected void drpListJoiningDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        BindSearchData();
    }

    protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int intEmpID = Convert.ToInt16(((ImageButton)sender).CommandArgument);
        intMEmpID = intEmpID;
        DataTable dtEmployee = objCommon.EmployeeDetails("Select", intEmpID);
        lstempQual.SelectedIndex = -1;
        if (dtEmployee.Rows.Count > 0)
        {
            ClearFields();

            empName.Text = dtEmployee.Rows[0]["empName"].ToString();
            empAddress.Text = dtEmployee.Rows[0]["empAddress"].ToString();
            empContact.Text = dtEmployee.Rows[0]["empContact"].ToString();
            empNotes.Text = dtEmployee.Rows[0]["empNotes"].ToString();
            txtempid.Text = dtEmployee.Rows[0]["empid"].ToString();
            txtempid.Visible = true;
            Editempid.Visible = true;
            hdnDocName.Value = dtEmployee.Rows[0]["Resume"].ToString();
            if (hdnDocName.Value != "")
            {
                lblUploadedName.Visible = true;
                string filename = intEmpID + "_" + empName.Text + Path.GetExtension(hdnDocName.Value);
                lblUploadedName.Text = "<a href='../Common/Download.aspx?m=CV&f=" + filename + "'>" + hdnDocName.Value + "</a>";
            }
            else
            {
                lblUploadedName.Visible = false;
            }
            photoImage.Src = "/Member/Services/ViewImage.ashx?id=" + Convert.ToInt32(dtEmployee.Rows[0]["empid"]);
            empJoiningDate.Text = Convert.ToDateTime(Convert.ToString(dtEmployee.Rows[0]["empJoiningDate"])).Date.ToString("dd/MM/yyyy");
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

            if (Convert.ToString(dtEmployee.Rows[0]["empLeavingDate"]).Trim() != "")
                empLeavingDate.Text = Convert.ToDateTime(Convert.ToString(dtEmployee.Rows[0]["empLeavingDate"])).Date.ToString("dd/MM/yyyy");

            empProbationPeriod.SelectedIndex = Convert.ToInt16(dtEmployee.Rows[0]["empProbationPeriod"].ToString());
            empEmail.Text = dtEmployee.Rows[0]["empEmail"].ToString();
            empAccountno.Text = dtEmployee.Rows[0]["empAccountNo"].ToString();

            if (Convert.ToString(dtEmployee.Rows[0]["empBDate"]).Trim() != "")
            {
                
                empBDate.Text = Convert.ToDateTime(Convert.ToString(dtEmployee.Rows[0]["empBDate"])).Date.ToString("dd/MM/yyyy");
            }

            if (Convert.ToString(dtEmployee.Rows[0]["empADate"]).Trim() != "")
            {
              
                empADate.Text = Convert.ToDateTime(Convert.ToString(dtEmployee.Rows[0]["empADate"])).Date.ToString("dd/MM/yyyy");
            }

            right2.Checked = Convert.ToBoolean(dtEmployee.Rows[0]["IsTester"].ToString());
            empPrevEmployer.Text = dtEmployee.Rows[0]["empPrevEmployer"].ToString();
            empSkill.SelectedValue = dtEmployee.Rows[0]["Skill"].ToString();
            ddlPrimarySkill.SelectedValue = dtEmployee.Rows[0]["PrimarySkillId"].ToString();
            DataTable dtQual = objCommon.EmployeeQualificationList(intEmpID);
            string dtJoinDate = Convert.ToDateTime(Convert.ToString(dtEmployee.Rows[0]["empJoiningDate"])).Date.ToString("dd/MM/yyyy");

            hndExperienceStore.Value = dtEmployee.Rows[0]["empExperince"].ToString();
            CalculateTotalExp(dtJoinDate, Convert.ToInt32(dtEmployee.Rows[0]["empExperince"].ToString()), false);
            dropempExpyears.Text = Convert.ToString(intYears);
            dropempExpmonths.Text = Convert.ToString(intMonth);
            CalculateTotalExp(dtJoinDate, Convert.ToInt32(dtEmployee.Rows[0]["empExperince"].ToString()), true);
            lblYear.Visible = false;
            lblMonth.Visible = false;
            dropempExpmonths.Visible = false;
            dropempExpyears.Visible = false;
            foreach (DataRow rowQual in dtQual.Rows)
            {
                ListItem lstItem = lstempQual.Items.FindByText(rowQual["QualDesc"].ToString());
                if (lstItem != null)
                {
                    lstItem.Selected = true;
                }
            }

            DataTable dtSkills = objCommon.GetTechSkills(intEmpID);
            foreach (DataRow rowQual in dtSkills.Rows)
            {
                ListItem lstItem = lstSecondarySkill.Items.FindByValue(rowQual["techId"].ToString());
                if (lstItem != null)
                    lstItem.Selected = true;
            }
            btnCancel.Visible = true;
            btnCancel.Text = "BACK";
            pnlEmployeeAdd.Visible = true;
            pnlEmployeeView.Visible = false;
            btnAdd.Visible = true;
            btnAdd.Text = "EDIT";
            btnAdd.CommandArgument = intEmpID.ToString();
            btnView.Visible = false;
            btnSave.Visible = false;
            DisableOrEnableControls(false);
            btnSave.CommandArgument = intEmpID.ToString();
            ddlLocationList.SelectedValue = dtEmployee.Rows[0]["LocationFKID"].ToString();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnCancel.Text = "CANCEL";
        btnSave.Visible = true;
        btnSave.Text = "SAVE";
        btnAdd.Text = "ADD";
        btnCancel.Visible = false;
        pnlEmployeeAdd.Visible = false;
        pnlEmployeeView.Visible = true;
        btnAdd.Visible = true;
        btnView.Visible = false;
        btnSave.CommandArgument = "";
        btnAdd.CommandArgument = "";
        DisableOrEnableControls(true);

        dlLocation.SelectedValue = ddlLocationList.SelectedValue;
        hdLocationId.Value = dlLocation.SelectedValue;
        BindSearchData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindSearchData();
    }

    protected void ddlPageGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        intIndex = ((DropDownList)sender).SelectedIndex;
        if (((DropDownList)sender).SelectedItem.Text == "ALL")
        {
            gvEmployeeView.PageSize = gvEmployeeView.Rows.Count;
        }
        else
        {
            gvEmployeeView.PageSize = Convert.ToInt32(((DropDownList)sender).SelectedItem.Text);
        }
        BindData();
    }

    protected void gvEmployeeView_DataBound(object sender, EventArgs e)
    {

    }

    protected void gvEmployeeView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      
        if (UM == null)
            return;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataTable dtSL = objCommon.SecurityLevel(UM.EmployeeID);

            HiddenField hdSecurityLevel = (HiddenField)e.Row.FindControl("hdnSecurityLevel");
            ImageButton ImgBtn = (ImageButton)e.Row.FindControl("imgbtnView");
          

            int securityLevel = dtSL.Rows[0]["SecurityLevel"] != "" ? Convert.ToInt32(dtSL.Rows[0]["SecurityLevel"]) : 0;
            int hdnsecurityLevel = hdSecurityLevel.Value != "" ? Convert.ToInt32(hdSecurityLevel.Value) : 0;

            if (securityLevel < hdnsecurityLevel)
            {
                e.Row.BackColor = ColorTranslator.FromHtml("#EFEFEF");
                ImgBtn.Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.Pager)
        {
            if (ViewState["Pager"] != null)
            {
                CreatePagerLinkButton(e);
            }
            else
            {
                ArrayList alPager = new ArrayList();
                if (gvEmployeeView.PageCount >= 5)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        alPager.Add(i.ToString());
                    }
                }
                else
                {
                    for (int i = 1; i <= gvEmployeeView.PageCount; i++)
                    {
                        alPager.Add(i.ToString());
                    }
                }
                ViewState.Add("Pager", alPager);
                CreatePagerLinkButton(e);
            }
        }

    }

    private void CreatePagerLinkButton(GridViewRowEventArgs e)
    {
        ArrayList alPager = (ArrayList)ViewState["Pager"];
        if (alPager.Contains((gvEmployeeView.PageIndex + 1).ToString()))
        {
            foreach (string page in alPager)
            {
                LinkButton lnkbtnPage = new LinkButton();
                lnkbtnPage.ID = "Page" + page;
                lnkbtnPage.CommandName = "page";
                lnkbtnPage.CommandArgument = (Convert.ToInt32(page) - 1).ToString();
                lnkbtnPage.Click += lnkbtnPage_Click;
                lnkbtnPage.Text = page;
                lnkbtnPage.CssClass = "number";
                if (gvEmployeeView.PageIndex == (Convert.ToInt32(page) - 1))
                {
                    lnkbtnPage.CssClass = "active";
                }
                e.Row.FindControl("divListBottons").Controls.Add(lnkbtnPage);
            }
        }
        else
        {
            if (alPager.Contains((gvEmployeeView.PageIndex - 1).ToString()))
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) + 1).ToString();
                }
            }
            else if (alPager.Contains((gvEmployeeView.PageIndex + 1).ToString()))
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) - 1).ToString();
                }
            }
            else if (Convert.ToInt32(alPager[alPager.Count - 1]) > gvEmployeeView.PageIndex + 1)
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) - 1).ToString();
                }
            }
            else if (Convert.ToInt32(alPager[0]) < gvEmployeeView.PageIndex + 1)
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) + 1).ToString();
                }
            }
            ViewState["Pager"] = alPager;
            CreatePagerLinkButton(e);
        }
    }

    void lnkbtnPage_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;

        gvEmployeeView.PageIndex = Convert.ToInt32(lnk.CommandArgument);
        BindData();
    }

    protected void gvEmployeeView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Page" && e.CommandArgument.ToString() == "Next")
        {

        }
    }

    protected void gvEmployeeView_PreRender(object sender, EventArgs e)
    {
    }

    protected void dlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdLocationId.Value = dlLocation.SelectedValue;
        BindSearchData();
    }

    //By Ram on 03-08-2013.
    protected void btnGenerateReport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTable = new DataTable("Employee");
            dataTable.Columns.Add("Employee ID");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Contact No");
            dataTable.Columns.Add("Designation");
            dataTable.Columns.Add("Experience");
            dataTable.Columns.Add("Joining Date");
            dataTable.Columns.Add("Leaving Date");

            XmlDocument doc = new XmlDocument();
            doc.Load(XMLFile);

            foreach (XmlNode node in doc)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    if (node.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode childnode in node.ChildNodes)
                        {
                            foreach (XmlNode subchildnode in childnode.ChildNodes)
                            {
                                if (subchildnode.Name == "ColumnID")
                                {
                                    for (int count = 0; count < this.gvEmployeeView.Columns.Count; count++)
                                    {
                                        BoundField dndf = this.gvEmployeeView.Columns[count] as BoundField;
                                        if (dndf == null)
                                            continue;

                                        if (subchildnode.InnerText == dndf.DataField)
                                        {
                                            dataTable.Columns.Add(dndf.HeaderText);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Adding Rows for Default Columns to dataTable.

            for (int row = 0; row < this.gvEmployeeView.Rows.Count; row++)
            {
                DataRow dr = dataTable.NewRow();
                for (int column = 0; column < dataTable.Columns.Count; column++)
                {
                    Control control = gvEmployeeView.Rows[row].Cells[0].FindControl("lblEmployeeId");
                    Label lbl1 = control as Label;
                    dr[0] = lbl1.Text;

                    control = gvEmployeeView.Rows[row].Cells[1].FindControl("lblName");
                    lbl1 = control as Label;
                    dr[1] = lbl1.Text;

                    control = gvEmployeeView.Rows[row].Cells[2].FindControl("lblContact");
                    lbl1 = control as Label;
                    dr[2] = lbl1.Text;

                    control = gvEmployeeView.Rows[row].Cells[3].FindControl("lblskillid");
                    lbl1 = control as Label;
                    dr[3] = lbl1.Text;

                    control = gvEmployeeView.Rows[row].Cells[4].FindControl("lblExperince");
                    lbl1 = control as Label;
                    dr[4] = lbl1.Text;

                    control = gvEmployeeView.Rows[row].Cells[5].FindControl("lblJoiningDate");
                    lbl1 = control as Label;
                    dr[5] = lbl1.Text;

                    control = gvEmployeeView.Rows[row].Cells[6].FindControl("lblLeavingDate");
                    lbl1 = control as Label;
                    dr[6] = lbl1.Text;


                    //Adding Rows for Dynamic Columns to dataTable.
                    for (int count = 0; count < this.gvEmployeeView.Columns.Count; count++)
                    {
                        BoundField dndf = this.gvEmployeeView.Columns[count] as BoundField;
                        if (dndf == null)
                            continue;
                        if (dndf.Visible)
                        {
                            if (gvEmployeeView.Rows[row].Cells[count].Text == "&nbsp;")
                            {
                                dr[dndf.HeaderText] = string.Empty;
                            }
                            else
                            {
                                dr[dndf.HeaderText] = gvEmployeeView.Rows[row].Cells[count].Text;
                            }
                        }
                    }
                }
                dataTable.Rows.Add(dr);
            }

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            DataGrid dg = new DataGrid();
            dg.DataSource = dataTable;
            dg.DataBind();
            dg.RenderControl(hw);
            dg.Dispose();
            Session.Add("gridviewdata", sw);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "download", "window.open('DownloadFile.aspx','_blank');", true);
        }
        catch (Exception ex)
        {
            //ex.ToString();            
        }
    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //}

    protected void btnAddColumns_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ColumnID");
            dataTable.Columns.Add("ColumnName");
            dataTable.TableName = "Employee";

            foreach (ListItem item in this.chkColumnsList.Items)
            {
                if (item.Selected)
                {
                    DataRow dr = dataTable.NewRow();
                    dr[0] = item.Value;
                    dr[1] = item.Text;

                    dataTable.Rows.Add(dr);
                }
            }


            XmlDocument xmlDocument = new XmlDocument();

            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version='1.0' encoding='utf-8' ?>");

            sb.Append("<DocumentElement>");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                sb.Append("<" + dataTable.TableName + ">");

                foreach (DataColumn dataColumn in dataTable.Columns)
                {

                    sb.Append("<" + dataColumn.ColumnName + ">" +

                                    dataRow[dataColumn].ToString() +

                               "</" + dataColumn.ColumnName + ">");

                }

                sb.Append("</" + dataTable.TableName + ">");
            }

            sb.Append("</DocumentElement>");

            xmlDocument.LoadXml(sb.ToString());
            this.pnlColumns.Visible = false;
            xmlDocument.Save(XMLFile);

            this.ShowHideDialogueInternal(false);

            btnGenerateReport_Click(null, null);
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void btnCancelAddCoumns_Click(object sender, EventArgs e)
    {
        this.ShowHideDialogueInternal(false);
    }

    public void ShowHideDialogue(object sender, EventArgs e)
    {
        BindColumnsNames();
        Button button = sender as Button;
        this.ShowHideDialogueInternal(button.CommandArgument.Equals("show"));
    }
   
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetExistsClient(string EmailID, int EmpID)
    {
        int empid = new clsCommon().GetExistsClient(EmailID, EmpID);
        System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        return jss.Serialize(empid);
    }
   
    
    #endregion

    #region Methods.

    private void BindLocation()
    {

        DataTable dtEmployeeLocation = objCommon.EmployeeLocationList();
        if (hdLocationId.Value.Equals("0"))
        {
            dlLocation.DataSource = dtEmployeeLocation;
            dlLocation.DataTextField = "Name";
            dlLocation.DataValueField = "LocationID";
            dlLocation.DataBind();
            hdLocationId.Value = dlLocation.SelectedValue = dtEmployeeLocation.Select("Name='Mumbai'").FirstOrDefault()["LocationID"].ToString();
            lblLocation.Visible = dlLocation.Visible = true;
        }

        ddlLocationList.DataSource = dtEmployeeLocation;
        ddlLocationList.DataTextField = "Name";
        ddlLocationList.DataValueField = "LocationID";
        ddlLocationList.DataBind();

        if (hdLocationId.Value.Equals("0"))
            ddlLocationList.SelectedValue = dtEmployeeLocation.Select("Name='Mumbai'").FirstOrDefault()["LocationID"].ToString();
        else
            ddlLocationList.SelectedValue = hdLocationId.Value;

        ddlLocationList.Enabled = hdHasAllLocationAcess.Value.Equals("1"); ;
    }

    private void BindData()
    {
        DataTable dtTemp = objCommon.EmployeeDetails(dlstLeavingDate.Text, 0);
        if (!(dtTemp.Rows.Count > 0))
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add(new DataColumn("SecurityLevel", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empid", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("Skill", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empName", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empAddress", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empContact", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empJoiningDate", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empLeavingDate", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empProbationPeriod", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empNotes", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empEmail", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empTester", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empAccountNo", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empBDate", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empADate", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empPrevEmployer", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("empExperince", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("IsSuperAdmin", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("IsAccountAdmin", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("IsPayrollAdmin", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("IsPM", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("IsTester", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("IsProjectReport", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("IsProjectStatus", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("IsLeaveAdmin", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("InsertedOn", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("InsertedBy", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("InsertedIP", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("ModifiedOn", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("ModifiedIP", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("Resume", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("CTC", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("Gross", typeof(string)));
            dtTemp.Columns.Add(new DataColumn("Net", typeof(string)));
            dtTemp.Rows.Add(dtTemp.NewRow());

            //Store the DataTable in ViewState
            gvEmployeeView.DataSource = dtTemp;
            gvEmployeeView.DataBind();

            int columncount = gvEmployeeView.Rows[0].Cells.Count;
            gvEmployeeView.Rows[0].Cells.Clear();
            gvEmployeeView.Rows[0].Cells.Add(new TableCell());
            gvEmployeeView.Rows[0].Cells[0].ColumnSpan = columncount;
            gvEmployeeView.Rows[0].Cells[0].Text = "No Records";
        }
        else
        {
            if (!IsPostBack)
            {
                AddGridViewColumnsInRuntime();
            }
            BindSearchData();
        }
    }

    private void MakeAccessible(GridView grid)
    {
        if (grid.Rows.Count > 0)
        {
            //This replaces <td> with <th> and adds the scope attribute
            grid.UseAccessibleHeader = true;


            //This will add the <thead> and <tbody> elements
            grid.HeaderRow.TableSection = TableRowSection.TableHeader;


            //This adds the <tfoot> element. Remove if you don't have a footer row
            grid.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }

    private void InsertUpdateEmpQualification(int intEmpID)
    {
        clsCommon open = new clsCommon();
        open.EmployeeQualifcation(intEmpID, 0, "Delete");
        foreach (ListItem item in lstempQual.Items)
        {
            if (item.Selected)
            {
                open = new clsCommon();
                DataTable dtQualID = open.GetQualificationId(item.ToString());
                if (dtQualID.Rows.Count > 0)
                {
                    open = new clsCommon();
                    open.EmployeeQualifcation(intEmpID, Convert.ToInt16(dtQualID.Rows[0][0].ToString()), "Insert");
                }
            }
        }
    }

    private void SaveSecondarySkills(int empId)
    {
        clsCommon common = new clsCommon();
        common.ManageEmpSecondarySkills(empId, "0", "Delete");

        foreach (ListItem item in lstSecondarySkill.Items)
        {
            if (item.Selected)
                common.ManageEmpSecondarySkills(empId, item.Value, "Insert");
        }
    }

    private void ClearFields()
    {
        empName.Text = "";
        empAddress.Text = "";
        empContact.Text = "";
        empNotes.Text = "";
        empJoiningDate.Text = "";
        empLeavingDate.Text = "";
        empProbationPeriod.SelectedIndex = 3;
        empEmail.Text = "";
        empAccountno.Text = "";
        empBDate.Text = "";
        empADate.Text = "";
        empPrevEmployer.Text = "";
        lstempQual.SelectedIndex = -1;
        lstSecondarySkill.ClearSelection();
        dropempExpmonths.SelectedIndex = 0;
        dropempExpyears.SelectedIndex = 0;
        ddlPrimarySkill.SelectedIndex = 0;
    }

    public string CalculateTotalExp(string strd2, int intPreviousExpInMonths, Boolean blnAdd)
    {
        DateTime d2 = DateTime.Now;
        if (string.IsNullOrEmpty(strd2.Trim()))
            return "";

        DateTime d1 = DateTime.Now;

        string[] strDate = strd2.Split(new string[] { " ", "-", "/" }, StringSplitOptions.None);

        if (strDate != null)
        {
            d2 = new DateTime(Convert.ToInt16(strDate[2]), Convert.ToInt16(strDate[1]), Convert.ToInt16(strDate[0]), 0, 0, 0);
        }

        if (d1 < d2)
        {
            DateTime d3 = d2;
            d2 = d1;
            d1 = d3;
        }

        // compute difference in total months
        intMonth = intPreviousExpInMonths;
        if (blnAdd)
            intMonth = 12 * (d1.Year - d2.Year) + (d1.Month - d2.Month) + intPreviousExpInMonths;
        // based upon the 'days',
        // adjust months & compute actual days difference
        if (d1.Day < d2.Day)
        {
            // intMonth--;
            intDays = DateTime.DaysInMonth(d2.Year, d2.Month) - d2.Day + d1.Day;
        }
        else
        {
            intDays = d1.Day - d2.Day;
        }
        // compute years & actual months
        intYears = intMonth / 12;
        intMonth -= intYears * 12;

        string strExp = string.Empty;

        if (intYears == 0 && intMonth != 0)
        {
            if (intMonth > 1)
            {
                strExp = intMonth.ToString() + " Months";
            }
            else
            {
                strExp = intMonth.ToString() + " Month";
            }
        }
        else if (intYears != 0 && intMonth == 0)
        {
            if (intYears > 1)
            {
                strExp = intYears.ToString() + " Years";
            }
            else
            {
                strExp = intYears.ToString() + " Year";
            }
        }
        else if (intYears != 0 && intMonth != 0)
        {
            if (intYears > 1 && intMonth > 1)
            {
                strExp = intYears.ToString() + " Years " + intMonth.ToString() + " Months";
            }
            else if (intYears == 1 && intMonth > 0)
            {
                strExp = intYears.ToString() + " Year " + intMonth.ToString() + " Months";
            }
            else if (intYears > 1 && intMonth == 1)
            {
                strExp = intYears.ToString() + " Years " + intMonth.ToString() + " Month";
            }
        }

        lblExp.Text = strExp;

        return strExp;
    }

    private void DisableOrEnableControls(bool blnApply)
    {
        if (blnApply)
            ddlLocationList.Enabled = hdHasAllLocationAcess.Value.Equals("1");
        else
            ddlLocationList.Enabled = false;
        lblUploadedName.Enabled = blnApply;
        lblMessage.Enabled = blnApply;
        fileUploadImage.Enabled = blnApply;
        filePhotoUpload.Enabled = blnApply;
        empSkill.Enabled = blnApply;
        ddlPrimarySkill.Enabled = blnApply;
        empName.ReadOnly = !blnApply;
        empAddress.ReadOnly = !blnApply;
        empContact.ReadOnly = !blnApply;
        empNotes.ReadOnly = !blnApply;
        empJoiningDate.Enabled = blnApply;
        empLeavingDate.Enabled = blnApply;
        empProbationPeriod.Enabled = blnApply;
        empEmail.ReadOnly = !blnApply;
        empAccountno.ReadOnly = !blnApply;
        empBDate.Enabled = blnApply;
        empADate.Enabled = blnApply;
        empPrevEmployer.ReadOnly = !blnApply;
        right2.Enabled = blnApply;
        dropempExpyears.Enabled = blnApply;
        dropempExpmonths.Enabled = blnApply;
        lstempQual.Enabled = blnApply;
        lstSecondarySkill.Enabled = blnApply;
        txtempid.ReadOnly = false;
        btnSendMail.Visible = true;
    }

    private void BindSearchData()
    {
        ViewState["Pager"] = null;
        DataTable dtEmp = objCommon.SearchByNameEmployeeID(txtSearch.Text.Trim(), dlstLeavingDate.Text, hdLocationId.Value);//dlLocation.SelectedValue
        ShowLeavingColumn();

        if ((dtEmp != null) && (dtEmp.Rows.Count > 0))
        {
            gvEmployeeView.DataSource = dtEmp;
            gvEmployeeView.DataBind();

        }
        else
        {
            gvEmployeeView.EmptyDataText = "No Matching Records Found !";
            gvEmployeeView.DataSource = null;
            gvEmployeeView.DataBind();
        }

        lblTotalEmployee.Text = "Total Number of Employees : " + dtEmp.Rows.Count;
    }

    protected void ShowLeavingColumn()
    {
        //if (!dlstLeavingDate.Text.Equals("Current"))
        //{
        //    gvEmployeeView.Columns[8].Visible = true;
        //}
        //else
        //    gvEmployeeView.Columns[8].Visible = false;
    }

    public string ProcessMyDataItem(object myValue)
    {
        if (myValue == null)
        {
            return "0 value";
        }

        return myValue.ToString();
    }

    private string ParseDatestring(string strDate)
    {
        string tmpDate;

        try
        {
            DateTime date = DateTime.Parse(strDate);
            tmpDate = date.ToString();
        }
        catch (Exception ex)
        {
            tmpDate = strDate;
        }
        return tmpDate;
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

            DataTable dtUserDetails = open.VerifyUser(EmpID);

            if (dtUserDetails.Rows.Count > 0)
            {
                MailMessage msg = new MailMessage();
                MailAddress msgFrom = new MailAddress(System.Configuration.ConfigurationManager.AppSettings.Get("fromEmail"),
                    System.Configuration.ConfigurationManager.AppSettings.Get("compName"));
                msg.From = msgFrom;


                foreach (string emailAddress in dtUserDetails.Rows[0]["empEmail"].ToString().Trim().Split(','))
                {
                    msg.To.Add(emailAddress);
                }


                msg.IsBodyHtml = true;
                msg.Subject = "Agora - Account Details";

                msg.Body = CreateMsgBody(dtUserDetails.Rows[0]["empName"].ToString(), dtUserDetails.Rows[0]["empEmail"].ToString(), EmpID, Mode).ToString();

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

        //string   strNewPassword = strUserName.Trim().Substring(0, 3) + intRandom.ToString().Substring(0, 4);

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

    private void ShowHideDialogueInternal(bool state)
    {
        this.pnlColumns.Visible = state;
        AddGridViewColumnsInRuntime();
        BindSearchData();
    }

    public void BindColumnsNames()
    {
        try
        {
            Dictionary<string, string> ColumnsList = new Dictionary<string, string>();
            ColumnsList.Add("Employee ID", "empid");
            ColumnsList.Add("Name", "empName");
            ColumnsList.Add("Address", "empAddress");
            ColumnsList.Add("Contact", "empContact");
            ColumnsList.Add("JoiningDate", "empJoiningDate");
            ColumnsList.Add("LeavingDate", "empLeavingDate");
            ColumnsList.Add("Probation Period", "empProbationPeriod");
            ColumnsList.Add("Notes", "empNotes");
            ColumnsList.Add("Email ID", "empEmail");
            ColumnsList.Add("Tester", "empTester");
            ColumnsList.Add("AccountNo", "empAccountNo");
            ColumnsList.Add("Birth Date", "empBDate");
            ColumnsList.Add("Anniversary  Date", "empADate");
            ColumnsList.Add("Previous Employer", "empPrevEmployer");
            ColumnsList.Add("Experience", "empExperince");
            ColumnsList.Add("AnnualCTC", "AnnualCTC");
            ColumnsList.Add("CTC", "CTC");
            ColumnsList.Add("Gross", "Gross");
            ColumnsList.Add("Net", "Net");
            ColumnsList.Add("IsSuperAdmin", "IsSuperAdmin");
            ColumnsList.Add("IsAccountAdmin", "IsAccountAdmin");
            ColumnsList.Add("IsPayrollAdmin", "IsPayrollAdmin");
            ColumnsList.Add("IsPM", "IsPM");
            ColumnsList.Add("IsTester", "IsTester");
            ColumnsList.Add("IsProjectReport", "IsProjectReport");
            ColumnsList.Add("IsProjectStatus", "IsProjectStatus");
            ColumnsList.Add("IsLeaveAdmin", "IsLeaveAdmin");
            ColumnsList.Add("IsActive", "IsActive");
            ColumnsList.Add("Resume", "Resume");
            ColumnsList.Add("Qualification", "Qualification");
            ColumnsList.Add("Primary Skill", "PrimarySkill");

            this.chkColumnsList.DataSource = ColumnsList;
            this.chkColumnsList.DataTextField = "Key";
            this.chkColumnsList.DataValueField = "Value";
            this.chkColumnsList.DataBind();

            XmlDocument doc = new XmlDocument();
            doc.Load(XMLFile);

            foreach (XmlNode node in doc)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    if (node.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode childnode in node.ChildNodes)
                        {
                            foreach (XmlNode subchildnode in childnode.ChildNodes)
                            {
                                foreach (ListItem item in this.chkColumnsList.Items)
                                {
                                    if (item.Value == subchildnode.InnerText)
                                    {
                                        if (item.Value == "Skill" || item.Value == "empid" || item.Value == "empName" || item.Value == "empContactNo" || item.Value == "empJoiningDate" || item.Value == "empExperience" || (item.Value == "empLeavingDate" && dlstLeavingDate.Text != "Current"))
                                        {
                                            item.Enabled = false;
                                        }
                                        item.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }

    public void AddGridViewColumnsInRuntime()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(XMLFile);

        foreach (XmlNode node in doc)
        {
            if (node.NodeType != XmlNodeType.Comment)
            {
                if (node.ChildNodes.Count > 0)
                {
                    foreach (XmlNode childnode in node.ChildNodes)
                    {
                        foreach (XmlNode subchildnode in childnode.ChildNodes)
                        {
                            BoundField bField = null;
                            if (subchildnode.Name == "ColumnID")
                            {
                                if (subchildnode.InnerText != "Skill" && subchildnode.InnerText != "empid" && subchildnode.InnerText != "empName" && subchildnode.InnerText != "empContactNo" && subchildnode.InnerText != "empJoiningDate" && subchildnode.InnerText != "empExperience" && subchildnode.InnerText != "empLeavingDate")
                                {
                                    foreach (XmlNode subchildnode1 in childnode.ChildNodes)
                                    {
                                        if (subchildnode1.Name == "ColumnName")
                                        {
                                            bField = new BoundField();
                                            bField.HeaderText = subchildnode1.InnerText;
                                            bField.DataField = subchildnode.InnerText;
                                            break;
                                        }
                                    }

                                    bool existingColumn = false;

                                    if (bField == null)
                                    {
                                        continue;
                                    }
                                    for (int count = 0; count < this.gvEmployeeView.Columns.Count; count++)
                                    {
                                        if (this.gvEmployeeView.Columns[count].HeaderText == "")
                                            continue;

                                        if (bField != null && this.gvEmployeeView.Columns[count].HeaderText != bField.HeaderText)
                                        {
                                            if (this.gvEmployeeView.Columns[count].HeaderText != "Employee ID" && this.gvEmployeeView.Columns[count].HeaderText != "Name" && this.gvEmployeeView.Columns[count].HeaderText != "Contact No" && this.gvEmployeeView.Columns[count].HeaderText != "Designation" && this.gvEmployeeView.Columns[count].HeaderText != "Experience" && this.gvEmployeeView.Columns[count].HeaderText != "Joining Date" && this.gvEmployeeView.Columns[count].HeaderText != "Leaving Date")
                                            {
                                                if (this.gvEmployeeView.Columns[count].HeaderText == bField.HeaderText)
                                                {
                                                    existingColumn = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            existingColumn = true;
                                        }
                                    }

                                    if (!existingColumn)
                                    {
                                        if (bField != null)
                                        {
                                            if (bField.HeaderText.Equals("Notes"))
                                                bField.HtmlEncode = false;
                                            gvEmployeeView.Columns.Add(bField);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int count = 0; count < this.gvEmployeeView.Columns.Count; count++)
        {
            if (this.gvEmployeeView.Columns[count].HeaderText == "")
                continue;

            BoundField dndf = this.gvEmployeeView.Columns[count] as BoundField;

            if (dndf == null)
                continue;

            foreach (ListItem item in this.chkColumnsList.Items)
            {
                if (item.Value == dndf.DataField)
                {
                    if (!item.Selected)
                    {
                        this.gvEmployeeView.Columns[count].Visible = false;
                    }
                    else
                    {
                        this.gvEmployeeView.Columns[count].Visible = true;
                    }
                }
            }
        }
    }

    #endregion

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        int intEmpID = intMEmpID;
        intMEmpID = 0;
        SendMail(intEmpID.ToString(), "EDIT");
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Email", "<script>alert('Mail sent successfully.');</script>", false);
    }

    protected void btnProcessData_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(5000);
        lblMessage.Visible = true;
    }

    protected void ViewResume(object sender, CommandEventArgs e)
    {
        string[] attachment = e.CommandArgument.ToString().Split('#');
        if (!string.IsNullOrEmpty(attachment[2]))
        {
            string[] attachedName = attachment[0].Trim().Split('$');
            string employeeId = attachedName[0];
            string resumeName = attachment[1];
            string empName = attachment[2];
            DataTable dtEmployee = objCommon.EmployeeDetails("Select", Convert.ToInt16(employeeId));

            if (dtEmployee.Rows.Count > 0)
            {
                var resume = dtEmployee.Rows[0]["Resume"].ToString();

                NameData = employeeId + "_" + empName;
                Doc theDoc = new Doc();
                if (resume.ToUpper().Contains(".DOC"))
                {
                    theDoc.Read(TargetDir + NameData + ".doc");
                }
                else //if (resume.ToUpper().Contains(".PDF"))
                {
                    theDoc.Read(TargetDir + NameData + ".pdf");
                }

                theDoc.FontSize = 500;
                theDoc.Color.String = "255 0 0";
                theDoc.HPos = 0.5;
                theDoc.VPos = 0.3;
                int theCount = theDoc.PageCount;
                for (int i = 1; i <= theCount; i++)
                {
                    theDoc.PageNumber = i;
                }
                if (resume.Contains(".doc") || resume.Contains(".DOC"))
                {
                    theDoc.Save(TargetDir + NameData + ".pdf");
                }

                byte[] theData = theDoc.GetData();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("content-length", theData.LongLength.ToString());
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + NameData.Replace(' ', '_') + ".pdf");
                HttpContext.Current.Response.BinaryWrite(theData);
                HttpContext.Current.Response.End();
            }
            //updatepanelEmployee.Update();


        }
    }
}







