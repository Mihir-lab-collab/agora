using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Customer.DAL;


public partial class Admin : System.Web.UI.MasterPage
{
    UserMaster UM;
    clsCommon open = new clsCommon();
    NotesBLL objNotesBLL = new NotesBLL();
    Boolean Menu = false;



    protected void Page_Init(object sender, EventArgs e)
    {
        objNotesBLL.SetAllNoteType();
        setModuledata();
        UM = UserMaster.UserMasterInfo();
        if (UM != null)
        {
            lblUserName.Text = " " + UM.Name;

            if (UM.IsAdmin == true)
            {
                UM.IsModuleAdmin = true;

            }
            else
            {
                UM.Access(UM);
            }

            if (!Menu)
            {
                CreateMenuBar(UM);
            }

            //////////////////////////////////Notice Alert/////////////////////
            hdnEmpID.Value = UM.EmployeeID.ToString();
            BindSkillRemainder();

            //---------------- Skill highlight-----------//
            hdnLoginID.Value = UM.EmployeeID.ToString();
            BindHighlightSkill();
        }
    }

    private void BindSkillRemainder()
    {
        string msg = SkillMatrixBLL.GetRemainderMsg();
        divMatter.InnerHtml = msg;
    }

    public void MasterInit(Boolean ShowProjects, Boolean ShowLocations, Boolean ProjectsShowAllOption = false, Boolean LocationsShowAllOption = false, Boolean ShowSkill = false)
    {
        if (!IsPostBack)
        {
            if (ShowProjects)
            {
                FillProjects(UM.EmployeeID.ToString(), ProjectsShowAllOption);
                DivProject.Visible = true;
            }

            if (ShowLocations)
            {
                FillLocations(LocationsShowAllOption);
                DivLocation.Visible = true;
            }
            if (ShowSkill)
                divSkillRotator.Visible = true;

        }
        else
        {
            if (ShowProjects)
            {
                Session["ProjectId"] = DDProjects.SelectedItem.Value.ToString();
                Session["ProjectName"] = DDProjects.SelectedItem.Text.Trim();
            }
            if (ShowLocations)
            {
                Session["LocationID"] = DDLocations.SelectedItem.Value.ToString();
                Session["LocationName"] = DDLocations.SelectedItem.Text.Trim();
            }
        }
    }

    public void MemberLogin()
    {

    }

    protected void btnReDirectOldVersion(object sender, EventArgs e)
    {
        Response.Redirect("~/emp/empHome.aspx");
    }

    private void CreateMenuBar(UserMaster UM)
    {
        try
        {
            DataTable dtMainMenu = open.GetMainMenuBarItem(UM.EmployeeID);

            string str = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=" + "\"" + "left_menu" + "\"" + ">");
            sb.Append("<ul class=" + "\"" + "accordion" + "\"" + ">");
            foreach (DataRow dr in dtMainMenu.Rows)
            {
                if (!(dr["EntryPage"] is DBNull))
                {
                    sb.Append("<li><a href=" + "\".." + dr["EntryPage"].ToString() + "\"" + " class=" + "\"" + "d_link" + "\"" + "><span>" + dr["Menu"].ToString() + "</span></a>");
                }
                else
                {
                    sb.Append("<li><a href=" + "\"" + "javascript:void(0)" + "\"" + "><span>" + dr["Menu"].ToString() + "</span></a>");
                }

                DataTable dtSubMenuItem = open.GetSubMenuItem(UM.EmployeeID, Convert.ToInt32(dr["ModuleID"].ToString()));
                int rowcount = dtSubMenuItem.Rows.Count;
                int k = 0;
                if (rowcount == 0)
                {
                    sb.Append("</li>");
                }
                foreach (DataRow dr1 in dtSubMenuItem.Rows)
                {
                    if (!(dr1["EntryPage"] is DBNull))
                    {
                        if (str == "")
                        {
                            str = "<ul>";
                            sb.Append(str);
                        }
                        sb.Append("<li><a href=" + "\".." + dr1["EntryPage"].ToString() + "\"" + " class=" + "\"" + "d_link" + "\"" + "><span>" + dr1["Menu"].ToString() + "</span></a></li>");

                    }
                    k++;
                    if (k == rowcount && str != "")
                    {
                        sb.Append("</ul>");
                    }
                }
                if (rowcount > 0)
                {
                    sb.Append("</li>");
                }
                str = "";
                k = 0;
            }
            sb.Append("<div class=" + "\"" + "clear" + "\"" + "></div>");
            sb.Append("</ul></div>");

            System.Web.UI.HtmlControls.HtmlGenericControl createDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
            createDiv.ID = "createDiv";
            createDiv.InnerHtml = sb.ToString();
            Panel1.Controls.Add(createDiv);
            Menu = true;
        }

        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
            Response.End();
        }
    }

    private void FillLocations(Boolean ShowAllOption)
    {

        try
        {
            DDLocations.DataSource = LocationBLL.BindLocation("GetLocation", UM.ProfileLocationID);
            DDLocations.DataBind();
            if (ShowAllOption)
            {
                DDLocations.Items.Insert(5, new ListItem("All", "5"));
            }

            if (Session["LocationID"] == null)
            {
                if (DDLocations.Items.Count > 0)
                {
                    Session["LocationID"] = DDLocations.SelectedItem.Value.ToString();
                    Session["LocationName"] = DDLocations.SelectedItem.Text.Trim();
                }
            }
            else
            {
                for (int i = 0; i <= DDLocations.Items.Count - 1; i++)
                {
                    if (DDLocations.Items[i].Value == Session["LocationID"].ToString())
                    {
                        DDLocations.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
            Response.End();
        }
    }

    //private void FillLocations(Boolean ShowAllOption)
    //{

    //    try
    //    {
    //        DDLocations.DataSource = LocationBLL.BindLocation("GetLocationByID", UM.ProfileLocationID);
    //        DDLocations.DataBind();
    //        if (ShowAllOption)
    //        {
    //            DDLocations.Items.Insert(0, new ListItem("All", "0"));
    //        }

    //        if (Session["LocationID"] == null)
    //        {
    //            if (DDLocations.Items.Count > 0)
    //            {
    //                Session["LocationID"] = DDLocations.SelectedItem.Value.ToString();
    //                Session["LocationName"] = DDLocations.SelectedItem.Text.Trim();
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i <= DDLocations.Items.Count - 1; i++)
    //            {
    //                if (DDLocations.Items[i].Value == Session["LocationID"].ToString())
    //                {
    //                    DDLocations.SelectedIndex = i;
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write("Error: " + ex.Message);
    //        Response.End();
    //    }
    //}

    protected void DDLocations_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["LocationID"] = DDLocations.SelectedItem.Value.ToString();
        Session["LocationName"] = DDLocations.SelectedItem.Text.Trim();

    }

    private void FillProjects(string UserId, Boolean ShowAllOption)
        {

        try
        {
            int IsAdmin = 0;
            if (UM.IsAdmin == true || (UM.IsAdmin == true && UM.IsModuleAdmin == true) ||(UM.ProfileID == Convert.ToString(Convert.ToInt32(CSCode.Global.EmProfile.Accounts))))
            {
                IsAdmin = 1;
            }

            DDProjects.DataSource = Customer.BLL.Projects.GetProjectList(UM.EmployeeID, IsAdmin);
            DDProjects.DataBind();
            //IList<TimeSheet> lstTimesheet = TimeSheet.GetTS(id);
            
            if (ShowAllOption)
            {
                DDProjects.Items.Insert(0, new ListItem("All", "0"));
            }

            if (Session["ProjectId"] == null)
            {
                if (DDProjects.Items.Count > 0)
                {
                    Session["ProjectId"] = DDProjects.SelectedItem.Value.ToString();
                    Session["ProjectName"] = DDProjects.SelectedItem.Text.Trim();
                   

                }
            }
            else
            {
                for (int i = 0; i <= DDProjects.Items.Count - 1; i++)
                {
                    if (DDProjects.Items[i].Value == Session["ProjectId"].ToString())
                    {
                        DDProjects.SelectedIndex = i;
                        //Added By Nikhil Shetye for setting Project Name on 17-10-2017
                        if (Session["ProjectName"] == null)
                        {
                            Session["ProjectName"] = DDProjects.Items[i].Text;
                        }
                        break;
                    }
                }
            }
            int projectId = Convert.ToInt32(Session["ProjectId"].ToString());
            Boolean IsSendMail =  Customer.BLL.Projects.GetCustomerProjectbyProjId(projectId).IsSendMail;

            Session["IsSendMail"] = IsSendMail;
        }
        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
            Response.End();
        }
    }



    protected void DDProjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ProjectId"] = DDProjects.SelectedItem.Value.ToString();
        Session["ProjectName"] = DDProjects.SelectedItem.Text.Trim();
        Boolean IsSendMail = Customer.BLL.Projects.GetCustomerProjectbyProjId(Convert.ToInt32(Session["ProjectId"])).IsSendMail;
        Session["IsSendMail"] = IsSendMail;

    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["islogout"] = "Logout";
        UserMaster.LogOut();
    }

    protected void btnOldVersion_Click(object sender, EventArgs e)
    {
        Session["EmpID"] = UM.EmployeeID;
        Response.Redirect("~/Emp/empLogin.aspx");
    }

    #region Skill Methods
    private void BindHighlightSkill()
    {
        DataTable dt = new DataTable(); //HttpContext.Current.Cache["EmpSkill"] as DataTable;        
        if (HttpContext.Current.Cache["EmpSkill"] == null)
        {
            dt = SkillMatrixBLL.GetHighlightSkill("HighlightSkill", UM.EmployeeID);
            HttpContext.Current.Cache.Insert("EmpSkill", dt, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        else
            dt = HttpContext.Current.Cache["EmpSkill"] as DataTable;

        if (dt.Rows.Count == 0)
        {
            dt = SkillMatrixBLL.GetHighlightSkill("HighlightSkill", UM.EmployeeID);
            HttpContext.Current.Cache["EmpSkill"] = dt;
        }
        BindContainer(dt);
    }

    private void BindContainer(DataTable dt)
    {

        //---------------------carousel slide NEW
        StringBuilder sb = new StringBuilder();

        //////sb.Append("<div class=\"nbs-flexisel-container\">");
        ////   //sb.Append("<div class=\"nbs-flexisel-inner\">");

        sb.Append("<ul id=\"flexiselDemo3\" class=\"nbs-flexisel-ul\" style=" + "\"left: -314px; display: block;\"" + ">");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sb.Append("<li id=" + dt.Rows[i]["SkillID"].ToString() + " class=\"nbs-flexisel-item\" style=" + "\"width: 350px;\"" + ">");          //style=" + "\"width: 100px;\"" + "
            //sb.Append(" <div title= '" + dt.Rows[i]["Name"].ToString() + "' class=\"divContent\" \">");                            
            sb.Append(" <div class=\"divContent\" >");

            sb.Append("<a style=\"color:white\" class=\"tooltip1\" title= '" + dt.Rows[i]["ToolTip"].ToString() + "' >");

            sb.Append("<input type=\"checkbox\" name=\"checkboxG0\" id=\"checkboxG0\" class=\"css-checkbox\"  />");
            sb.Append("<label for=\"checkboxG0\" class=\"css-label2 radGroup1\" onclick=" + "\"SaveSkillByChk(" + dt.Rows[i]["SkillID"].ToString() + ");\"" + "></label>");
            sb.Append("<label style=" + "\"cursor:pointer\"" + " onclick=\"OpenPopup(" + dt.Rows[i]["SkillID"].ToString() + ");\"" + ">");
            sb.Append(dt.Rows[i]["Name"].ToString());
            sb.Append("</label>");

            ////sb.Append("<span>");
            ////sb.Append("<b></b>");
            ////sb.Append(dt.Rows[i]["Name"].ToString());
            ////sb.Append("</span>");
            sb.Append("</a>");
            sb.Append(" </div>");
            sb.Append(" </li>");
        }
        sb.Append("</ul>");

        ////     //sb.Append("</div>");
        ////     //sb.Append("<div class=\"nbs-flexisel-nav-left\" style="+ "\"visibility: visible; top: 18px;\"" + "> </div>");
        ////     //sb.Append("<div class=\"nbs-flexisel-nav-right\" style=" + "\"visibility: visible; top: 18px;\"" + "></div>");
        //////sb.Append(" </div>");


        divSkilldata.InnerHtml = sb.ToString();
    }

    private DataTable dtPageData(DataTable dt)
    {
        DataTable dtPg = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
        return dtPg;
    }
    #endregion


    private void setModuledata()
    {
        string MenuName = "Employee Notes";
        string Pagename = "/Member/Notes.aspx";
        ModulesDAL obj = new ModulesDAL();
        ModuleParam ObjModuleParam = obj.GetModuleData("GetModuleDataByPage", MenuName, Pagename);
        HttpContext.Current.Session["ModuleParam"] = ObjModuleParam.Parameter;
    }

    protected void btnContinueSession_Click(object sender, EventArgs e)
    {
        if (Session.SessionID != null)
        {
            if (Session.SessionID != hfSessionID.Value)
            {
                RedirectLogin();
            }

        }
        else
        {
            RedirectLogin();
        }
    }

    protected void btnSessionLogout_Click(object sender, EventArgs e)
    {
        Session["islogout"] = "Logout";
        UserMaster.LogOut();
    }

    private void RedirectLogin()
    {

    }

}
