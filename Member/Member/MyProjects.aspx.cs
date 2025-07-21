using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Customer.BLL;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using Customer.DAL;
using CSCode;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Web.Services;


public partial class Member_MyProjects : Authentication
{
    public static int ProjID;
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();

        if (!IsPostBack)
        {
            Session["UserName"] = UM.Name.ToString();
            Session["CustId"] = UM.EmployeeID.ToString();
            Session["UserId"] = UM.EmployeeID.ToString();
            Session["UserMailid"] = UM.EmailID.ToString();
            if (UM.IsAdmin || UM.IsModuleAdmin)
            {
                hdn.Value = "true";
            }
            else
            {
                hdn.Value = "false";
            }
            BindListTeam();
        }
    }

    [System.Web.Services.WebMethod]
    public static String BindMyProjId(int EmpId, string include)
    {
        try
        {
            Boolean IsAdmin = false;

            if (UM.IsAdmin || UM.IsModuleAdmin)
            {
                IsAdmin = true;
            }
            List<Customer.BLL.MyprojectsBLL> lstGetMyProjId = Customer.BLL.MyprojectsBLL.GetMyProjId(EmpId, IsAdmin, include);
            if (include == "")
            {
                var data = from curMyProjId in lstGetMyProjId
                           //where (curMyProjId.projActComp == "")
                           select new
                           {
                               curMyProjId.projId,
                               curMyProjId.custId,
                               curMyProjId.projName,
                               curMyProjId.projDesc,
                               curMyProjId.projManager,
                               curMyProjId.projStartDate,
                               curMyProjId.custName,
                               curMyProjId.empName,
                               curMyProjId.AccountManager,
                               curMyProjId.projExpComp,
                               curMyProjId.projActComp,
                               curMyProjId.projRemark,
                               curMyProjId.projStatusTDesc,
                               curMyProjId.projStatusTId,
                               curMyProjId.ExpCompleted,
                               Projdevname.GetProjdevname(curMyProjId.codeDevTeam).codedevname,
                               ProjEmpname.GetProjempname(curMyProjId.codeRevTeam).coderevname,
                               curMyProjId.projStatus,
                               ProjectOverallrating.GetOverallratingByPRojId(curMyProjId.projId).Overallrating
                           };
                JavaScriptSerializer jss = new JavaScriptSerializer();
                return jss.Serialize(data);

            }
            else
            {
                var data = from curMyProjId in lstGetMyProjId

                           select new
                           {
                               curMyProjId.projId,
                               curMyProjId.custId,
                               curMyProjId.projName,
                               curMyProjId.projDesc,
                               curMyProjId.projManager,
                               curMyProjId.projStartDate,
                               curMyProjId.custName,
                               curMyProjId.empName,
                               curMyProjId.projExpComp,
                               curMyProjId.projActComp,
                               curMyProjId.projStatusTDesc,
                               curMyProjId.ExpCompleted,
                               Projdevname.GetProjdevname(curMyProjId.codeDevTeam).codedevname,
                               ProjEmpname.GetProjempname(curMyProjId.codeRevTeam).coderevname,
                               curMyProjId.projStatus,
                               curMyProjId.proStatusDate,
                               ProjectOverallrating.GetOverallratingByPRojId(curMyProjId.projId).Overallrating

                           };
                JavaScriptSerializer jss = new JavaScriptSerializer();
                return jss.Serialize(data);

            }
        }

        catch (Exception ex)
        {
            return null;
        }


    }

    protected void lnkSaveNewTask_Click(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethod]
    public static string BindTeam(int proid)
    {
        ProjID = proid;
        DataTable dt = new DataTable();
        List<TeamMemberDetails> details = new List<TeamMemberDetails>();
        string cs = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("sp_GetTeamDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projId", proid);
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dtrow in dt.Rows)
            {
                TeamMemberDetails user = new TeamMemberDetails();
                user.empid = Convert.ToInt32(dtrow["empid"]);
                user.empName = dtrow["empName"].ToString();
                details.Add(user);
            }
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(details);

    }

    [System.Web.Services.WebMethod]
    public static string CodeReviewTeam(int projid)
    {
        DataTable dt = new DataTable();
        List<TeamMemberDetails> details = new List<TeamMemberDetails>();
        string cs = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("sp_GetRevTeamDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projId", projid);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dtrow in dt.Rows)
            {
                TeamMemberDetails user = new TeamMemberDetails();
                user.empid = Convert.ToInt32(dtrow["empId"]);
                user.empName = dtrow["empName"].ToString();

                details.Add(user);
            }
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(details);
    }

    [System.Web.Services.WebMethod]
    public static string projectStatus(int proid)
    {
        DataTable dt = new DataTable();
        List<ProjectStatusDetails> details = new List<ProjectStatusDetails>();
        string cs = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("SP_ProjectStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projId", proid);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dtrow in dt.Rows)
            {
                ProjectStatusDetails prostatus = new ProjectStatusDetails();
                prostatus.projectstartdate = Convert.ToDateTime(dtrow["proStatusDate"]);
                prostatus.expcompleted = dtrow["projStatusTDesc"].ToString();
                prostatus.actualcompleted = dtrow["projStatus"].ToString();
                prostatus.remarks = Convert.ToString(dtrow["projRemark"]);
                prostatus.PostedBy = Convert.ToString(dtrow["PostedBy"]); 
                details.Add(prostatus);
            }

        }

        //return details.ToArray();
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(details);
    }


    [WebMethod]
    public static String GetProjStatusByProjId(string projid)
    {
        try
        {
            List<Customer.BLL.MyprojectsBLL> lstGetMyProjId = Customer.BLL.MyprojectsBLL.GetProjectStatusDetail(Convert.ToInt32(projid));

            var data = from curMyProjId in lstGetMyProjId
                           //where (curMyProjId.projActComp == "")
                       select new
                       {
                           curMyProjId.projId,
                           curMyProjId.custId,
                           curMyProjId.projName,
                           curMyProjId.projDesc,
                           curMyProjId.projManager,
                           curMyProjId.projStartDate,
                           curMyProjId.custName,
                           curMyProjId.empName,
                           curMyProjId.AccountManager,
                           curMyProjId.projExpComp,
                           curMyProjId.projActComp,
                           curMyProjId.projRemark,
                           curMyProjId.projStatusTDesc,
                           curMyProjId.projStatusTId,
                           curMyProjId.ExpCompleted,
                           Projdevname.GetProjdevname(curMyProjId.codeDevTeam).codedevname,
                           ProjEmpname.GetProjempname(curMyProjId.codeRevTeam).coderevname,
                           curMyProjId.projStatus,
                           ProjectOverallrating.GetOverallratingByPRojId(curMyProjId.projId).Overallrating
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
    public static string BindProjectDetails(int proid)
    {
        DataTable dt = new DataTable();
        List<ProjectDetails> details = new List<ProjectDetails>();
        string cs = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("sp_GetProjectDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projId", proid);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dtrow in dt.Rows)
            {
                ProjectDetails projectdetailobj = new ProjectDetails();
                projectdetailobj.projid = Convert.ToInt32(dtrow["projid"]);
                projectdetailobj.projectname = Convert.ToString(dtrow["projName"]);
                projectdetailobj.customername = Convert.ToString(dtrow["custName"]);
                projectdetailobj.customeraddress = Convert.ToString(dtrow["custAddress"]);
                projectdetailobj.projectduration = Convert.ToString(dtrow["projDuration"]);
                projectdetailobj.projectmanager = Convert.ToString(dtrow["empName"]);
                projectdetailobj.projectstatus = Convert.ToString(dtrow["projStatus"]);
                projectdetailobj.expprojectstatus = Convert.ToString(dtrow["expprojStatus"]);
                projectdetailobj.startdate = Convert.ToDateTime(dtrow["projStartDate"]);
                if (dtrow["projExpComp"] != null || dtrow["projExpComp"] != "" || dtrow["projExmComp"] != DBNull.Value)
                {
                    projectdetailobj.expcompletiondate = Convert.ToDateTime(dtrow["projExpComp"]);
                }
                if ((dtrow["projActComp"] != "") || (dtrow["projActComp"] != null))
                {
                    projectdetailobj.actcompletiondate = Convert.ToString(dtrow["projActComp"]);
                }
                // projectdetailobj.actcompletiondate = Convert.ToDateTime(dtrow["projStartDate"]);

                details.Add(projectdetailobj);
            }

        }

        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(details);
    }

    [System.Web.Services.WebMethod]
    public static string BindProjectStatusId()
    {
        DataTable dt = new DataTable();
        List<ProjectStatusId> details = new List<ProjectStatusId>();
        string cs = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("sp_GetStatusIds", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dtrow in dt.Rows)
            {
                ProjectStatusId projectstatusobj = new ProjectStatusId();
                projectstatusobj.projStatusTId = Convert.ToInt32(dtrow["projStatusTId"]);
                projectstatusobj.projStatusTDesc = Convert.ToString(dtrow["projStatusTDesc"]);

                details.Add(projectstatusobj);
            }

        }

        //return details.ToArray();
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(details);
    }

    public class ProjectStatusDetails
    {
        public DateTime projectstartdate { get; set; }
        public string expcompleted { get; set; }
        public string actualcompleted { get; set; }
        public string remarks { get; set; }
        public string PostedBy { get; set; }
    }
    public class TeamMemberDetails
    {
        public int empid { get; set; }
        public string empName { get; set; }
    }
    public class ProjectDetails
    {
        public int projid { get; set; }
        public string projectname { get; set; }
        public string customername { get; set; }
        public string customeraddress { get; set; }
        public string projectduration { get; set; }
        public string projectmanager { get; set; }
        public string projectstatus { get; set; }
        public string expprojectstatus { get; set; }
        public DateTime startdate { get; set; }
        public DateTime expcompletiondate { get; set; }
        public string actcompletiondate { get; set; }
    }
    public class ProjectStatusId
    {
        public int projStatusTId { get; set; }
        public string projStatusTDesc { get; set; }

    }
    [System.Web.Services.WebMethod]
    public static String BindProjectManagerDropDown()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        // Console.WriteLine(jss.Serialize(EmployeeMaster.GetAllEmployees()));
        return jss.Serialize(EmployeeMaster.GetAllEmployees());
    }




    protected void BindListTeam()
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("sp_GetAllEmployees", con);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            lstteam.DataSource = reader;
            lstteam.DataTextField = "empName";
            lstteam.DataValueField = "empid";
            lstteam.DataBind();
        }
        con.Close();

    }


    protected void lnkSaveProjectStatus_Click(object sender, EventArgs e)
    {
        String strDate = hfprojstatusDate.Value.ToString();
        string msg = string.Empty;
        int pid = Convert.ToInt32(hfprojid.Value);
        // int projid = Convert.ToInt32(Session["ProjectId"]);
        DateTime dt = DateTime.ParseExact(hfprojstatusDate.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

        // DateTime dt = Convert.ToDateTime(strDate).ToShortDateString;
        int statusid = Convert.ToInt32(hfProjectStatus.Value);
        int projcompletion = hfprojcompletion.Value != "" ? Convert.ToInt32(hfprojcompletion.Value) : 0;
        string remarks = Convert.ToString(hfRemarks.Value);


        string cs = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("SP_ProjectStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "SET");
            cmd.Parameters.AddWithValue("@projId", pid);
            cmd.Parameters.AddWithValue("@dt", dt);
            cmd.Parameters.AddWithValue("@statusid", statusid);
            cmd.Parameters.AddWithValue("@projectcompletion", projcompletion);
            cmd.Parameters.AddWithValue("@projremarks", remarks);
            cmd.Parameters.AddWithValue("@InsertedBy", UM.EmployeeID);
            con.Open();
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            if (result == 2)
            {
                errormessage.Text = "Project status date can not be less than start date";
                errormessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                //  msg = "Project status updated successfully";
                //  string curUrl = Request.RawUrl;
                //  messageBoxandSamepage(msg, curUrl);
                errormessage.Text = "Project status updated successfully";
                errormessage.ForeColor = System.Drawing.Color.Black;


            }


        }


    }
    
    protected void lnkSaveTeam_Click(object sender, EventArgs e)
    {
        string DevelopmentTeam = hftxtDevelopmentTeam.Value.Replace("'", "''");
        string[] projectMembers = DevelopmentTeam.Split(',');

        string msg = string.Empty;
        if (ProjID != 0)
        {

            projectMember.DeleteprojectMember(ProjID);

            for (int i = 0; i < projectMembers.Length; i++)
            {

                if (projectMembers[i].Trim() != "")
                {

                    projectMember.InsertUpdateProjectMem(ProjID, Convert.ToInt32(projectMembers[i].ToString()));

                }
            }

            msg = "Updated Sucessfully";
            string curUrl = Request.RawUrl;
            messageBoxandSamepage(msg, curUrl);
        }

    }


    private void messageBoxandSamepage(string message, string Url)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgboxsamepage", "alert('" + message + "');window.location='" + Url + "'; ", true);
    }

}