using Customer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

public partial class Member_SkillMatrix : System.Web.UI.Page
{
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, false, false, false);
        hdnLoginID.Value = UM.EmployeeID.ToString();
        Session["UID"] = hdnLoginID.Value.ToString();
        Session["SKILLADMIN"] = "Yes";
        if (!IsPostBack)
            BindCategory();

        //if(Request.QueryString["Empid"] !=null){
        //    int empID =Convert.ToInt32(Request.QueryString["EmpID"].ToString());
        //    GetEmployeeSkill(empID);
        //}
    }

    private void BindCategory()
    {
        List<KeyValueModel> lstCategory = new List<KeyValueModel>();
        lstCategory = SkillMatrixBLL.GetCategory();
        ddlCategory.DataSource = lstCategory;
        ddlCategory.DataValueField = "Value";
        ddlCategory.DataTextField = "Key";
        ddlCategory.DataBind();
        
    }

    #region GET
    [System.Web.Services.WebMethod]
    public static String BindSkills(string skillName = "")
    {
        try
        {
            List<SkillMatrixBLL> lstEvents = SkillMatrixBLL.GetSkills("Select", skillName);

            var data = from SKItems in lstEvents
                       select new
                       {
                           SKItems.SkillID,
                           SKItems.CategoryId,
                           SKItems.Category,
                           SKItems.SkillName,
                           SKItems.EmpCount,
                           SKItems.Status,
                           SKItems.MaxExperience
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
    public static List<KeyValueModel> GetSkill(int CategoryID = 0)
    {
        List<KeyValueModel> lstSkill = new List<KeyValueModel>();
        try
        {
            lstSkill = SkillMatrixBLL.GetSkill(CategoryID);
        }
        catch (Exception ex)
        {

        }
        return lstSkill;
    }

    [System.Web.Services.WebMethod]
    public static string GetCategory()
    {
        List<KeyValueModel> lstCategory = new List<KeyValueModel>();

        lstCategory = SkillMatrixBLL.GetCategory();
        var data = from Items in lstCategory
                   select new
                   {
                       Items.Key,   //name
                       Items.Value  //id
                   };
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(data);

    }

    // --------------- Employee Skill section
    [System.Web.Services.WebMethod]
    public static SkillMatrixBLL GetEmployeeSkill(int EmpID, string skillName = "", int toggleSkill = 0)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        objBLL = objBLL.GetEmployeeSkill("EmployeeSkill", EmpID, skillName, toggleSkill);

        return objBLL;
    }

    [System.Web.Services.WebMethod]
    public static string GetEmployeeDetail(int SkillID)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        objBLL = objBLL.GetEmployeeDetail("EmployeeDetail", SkillID);


        var data = from SKItems in objBLL.lstEmpSkill
                   select new
                   {
                       SKItems.EmpName,
                       SKItems.Category,
                       SKItems.SkillName,
                       SKItems.Experience,
                       SKItems.Level
                   };
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(data);

        //return objBLL;
    }

    [System.Web.Services.WebMethod]
    public static SkillMatrixBLL GetEmployeeSkillCount()
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        objBLL = objBLL.GetEmployeeSkillCount("ShowEmployees");


        //var data = from SKItems in objBLL.lstEmpSkill
        //           select new
        //           {
        //               SKItems.EmpName,
        //               SKItems.EmpCount,
        //               SKItems.InsertedDate                       
        //           };
        //JavaScriptSerializer jss = new JavaScriptSerializer();
        //return jss.Serialize(data);

        return objBLL;
    }

    #endregion

    #region Post
    public void SaveSkill()
    {
        int skillID = 0;
        if (hdnSkillID.Value != "")
            skillID = Convert.ToInt32(hdnSkillID.Value.ToString());

        try
        {
            int outputID = SkillMatrixBLL.SaveSkill("SaveSkill", Convert.ToInt32(hdnLoginID.Value.ToString()), skillID, Convert.ToInt32(ddlCategory.SelectedValue), txtSkillName.Value);
            if (outputID == 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "DuplicateSkill();", true);


        }
        catch (Exception ex)
        {

        }

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "call();", true);

    }

    public void SaveCategory()
    {
        int categoryID = 0;
        if (hdnCategoryID.Value != "")
            categoryID = Convert.ToInt32(hdnCategoryID.Value.ToString());
        try
        {
            int outputID = SkillMatrixBLL.SaveCategory("SaveCategory", Convert.ToInt32(hdnLoginID.Value.ToString()), categoryID, txtCategoryName.Value);
            if (outputID == 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "DuplicateCategory();", true);
            else
                BindCategory();
        }
        catch (Exception ex)
        {

        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "call();", true);
        //Response.Redirect("SkillMatrix.aspx");
    }

    // ---Employee Skill section post
    [System.Web.Services.WebMethod]
    public static string SaveEmployeeSkill(string JSONData)
    {
        UM = UserMaster.UserMasterInfo();
        SkillMatrixBLL objEmpSkill = new SkillMatrixBLL();
        objEmpSkill = JsonConvert.DeserializeObject<SkillMatrixBLL>(JSONData);

        string result1 = new SkillMatrixBLL().SaveEmployeeSkill("SaveEmployeeSkill", objEmpSkill);
        if(result1 != "" && UM != null)
        {
            DataTable dt = SkillMatrixBLL.GetHighlightSkill("HighlightSkill", UM.EmployeeID);
            HttpContext.Current.Cache["EmpSkill"] = dt;
        }
        return result1;
    }

    #endregion

    #region Events
    protected void btnSaveCategory_Click(object sender, EventArgs e)
    {
        SaveCategory();
    }
    protected void btnSaveSkill_Click(object sender, EventArgs e)
    {
        SaveSkill();

        Control btn = (Control)sender;
        if (btn.ID == "btnSaveMore")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "ShowMsg();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "ShowSkillPopup();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "call();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeSkillPopUP();", true);
            Response.Redirect("SkillMatrix.aspx");
        }
    }
    #endregion

    #region Masterpage Event

    [System.Web.Services.WebMethod]
    public static string CheckCookieTime(int EmpID)
    {
        string result = "0";

        string msg = SkillMatrixBLL.GetRemainderMsg();

        if (msg != "")
        {
            if (HttpContext.Current.Request.Cookies["SKILL"] == null)
                result = msg;
            else
                result = "0";
        }
        return result;
    }

    [System.Web.Services.WebMethod]
    public static string RestCookieTime(string RTime)
    {  // 1 hour 
        string result = "0";

        HttpContext.Current.Response.Cookies["SKILL"].Value = "Timer";
        string setvalue = RTime;
        switch (setvalue)
        {
            case "1 hour":
                HttpContext.Current.Response.Cookies["SKILL"].Expires = DateTime.Now.AddHours(1);
                result = " 1 hour";
                break;
            case "2 hour":
                HttpContext.Current.Response.Cookies["SKILL"].Expires = DateTime.Now.AddHours(2);
                result = " 2 hour";
                break;
            case "4 hour":
                HttpContext.Current.Response.Cookies["SKILL"].Expires = DateTime.Now.AddHours(4);
                result = " 2 hour";
                break;
            case "8 hour":
                HttpContext.Current.Response.Cookies["SKILL"].Expires = DateTime.Now.AddHours(8);
                result = " 2 hour";
                break;
            case "Never":
                HttpContext.Current.Response.Cookies["SKILL"].Expires = DateTime.Now.AddDays(7);
                result = " never";
                break;
        }

        return result;
    }

    [System.Web.Services.WebMethod]
    public static SkillMatrixBLL GetSkillByID(int skillID)
    {
        DataTable dt = HttpContext.Current.Cache["EmpSkill"] as DataTable;
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        objBLL = objBLL.GetSkillByID("GetSkillByID", skillID);

        return objBLL;
    }

    [System.Web.Services.WebMethod]
    public static string SaveSkillHighlight(int empID, int skillID, int experience, string level)
    {
        SkillMatrixBLL objEmpSkill = new SkillMatrixBLL();
        objEmpSkill.EmpID = empID;
        objEmpSkill.UserID = empID;

        List<SkillMatrixBLL> lstSkill = new List<SkillMatrixBLL>();
        SkillMatrixBLL obj = new SkillMatrixBLL();
        obj.ActiveSkill = true;
        obj.EmployeeSkillID = 0;
        obj.CategoryId = 0;
        obj.SkillID = skillID;
        obj.Experience = experience;
        obj.Level = level;
        lstSkill.Add(obj);

        objEmpSkill.lstEmpSkill = lstSkill;

        string result1 = new SkillMatrixBLL().SaveEmployeeSkill("SaveEmployeeSkill", objEmpSkill);

        DataTable dt = HttpContext.Current.Cache["EmpSkill"] as DataTable;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["SkillID"]) == skillID)
                {
                    dt.Rows.Remove(dr);
                    dt.AcceptChanges();
                    break;
                }
            }

            HttpContext.Current.Cache["EmpSkill"] = dt;
        }
       
        return result1;
    }
      
    [System.Web.Services.WebMethod]
    public static string RightSidePaging(int pageNum)
    {
        pageNum = pageNum + 1;
        string result1 = dtPaging(pageNum);
        return result1;
    }

    [System.Web.Services.WebMethod]
    public static string LeftSidePaging(int pageNum)
    {
        pageNum = pageNum - 1;
        string result1 = dtPaging(pageNum);
        return result1;
    }

    private static DataTable dtPageData(DataTable dt)
    {
        DataTable dtPg = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
        return dtPg;
    }
    private static string dtPaging(int pageNum)
    {
        DataTable dt = HttpContext.Current.Cache["EmpSkill"] as DataTable;

        DataTable dtPage = dt.Rows.Cast<System.Data.DataRow>().Skip((pageNum - 1) * 5).Take(5).CopyToDataTable();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < dtPage.Rows.Count; i++)
        {

            sb.Append("<div class=\"divContent\" onclick=\"OpenPopup(" + dtPage.Rows[i]["SkillID"].ToString() + ");\" >");
            sb.Append("<img src=" + "\"images/skillcategory.png\"" + "/>&nbsp;");
            sb.Append(dtPage.Rows[i]["Name"].ToString() + "&nbsp;&nbsp;&nbsp;");
            sb.Append("</div>");
        }

        return sb.ToString();
    }

    #endregion
}