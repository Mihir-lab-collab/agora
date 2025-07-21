using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Customer.Model;

public partial class Member_controls_EmployeeSkill : System.Web.UI.UserControl
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        hdnLoginID.Value = UM.EmployeeID.ToString();
        if (!UM.IsAdmin || !UM.IsModuleAdmin || Session["SKILLADMIN"].ToString() == "NO")
        {
            lblEmpName.InnerText = UM.Name.ToString();
            ddlEmployee.Visible = false;
            btnBindSkill.Visible = false;

            divImgSkill.Visible = true;
            lblEmpName.Visible = true;
            lnkRecommendSkill.Visible = true;
            hdnLoginID.Value = UM.EmployeeID.ToString();

            divGEmp.Visible = false;
            btnEmp.Visible = false;

        }
        else
        {
            hdnLoginID.Value = "0";
            divGEmp.Visible = true;
            btnEmp.Visible = true;
        }
        if(!IsPostBack)
            BindEmployee();
    }

    private void BindEmployee()
    {
        List<KeyValueModel> lstEmp = new List<KeyValueModel>();
        lstEmp = SkillMatrixBLL.GetEmployee("GetEmployee");
        ddlEmployee.DataSource = lstEmp;
        ddlEmployee.DataValueField = "Value";
        ddlEmployee.DataTextField = "Key";
        ddlEmployee.DataBind();
                
    }
        
    protected void btnMailSkill_Click(object sender, EventArgs e)
    {
        string fromMailID = UM.EmailID.ToString();
        string skillName = txtSkill.Value.ToString();
        string note = txtSkillNote.Value.ToString();
        string subject ="Skill Recommendation";
        string mBody = "<p>Hi Sir/madam,</p><p>I would like to recommend to add the following Skill \"" + skillName.ToString() + "\" in the Skill Matrix.</p><p>Note : "+note.ToString()+"</p><p>Regards</p><p>"+UM.Name+"</p>";

        CSCode.Global.SendMail(mBody, subject, "ashwini.k@intelgain.com", fromMailID.ToString(), true, "", "");//hr@intelgain.com
        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "mailMessage();", true);
    }
}