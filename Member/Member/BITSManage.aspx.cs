using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Globalization;


public partial class Member_BITSManage : Authentication
{
    UserMaster UM;
    projectMaster objprojectMaster= null;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo(); 
        
        if (!IsPostBack)
        {
            if (!(UM.IsAdmin || UM.IsModuleAdmin))
            {
                hdnAdmin.Value = Convert.ToString(UM.EmployeeID);
            }
        }
        
        // this.Form.Target = "_blank";
    }

    protected void lnkSaveProjectStatus_Click(object sender, EventArgs e)
    {
        String strDate = hfprojstatusDate.Value.ToString();
        string msg = string.Empty;
        int pid = Convert.ToInt32(hdnProjectId.Value);
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

    //added by trupti for Status In BI report
    [System.Web.Services.WebMethod]
    public static String GetProjStatusByProjId(int projid)
    {
        try
        {

            List<Customer.BLL.MyprojectsBLL> lstGetMyProjId = Customer.BLL.MyprojectsBLL.GetProjectStatusDetail(projid);

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

    //protected void imgRefresh_Click(object sender, ImageClickEventArgs e)
    //{
    //    Response.Redirect("http://localhost:51174//Crons/cron.aspx?m=BI");
    //}
    [System.Web.Services.WebMethod]
    public static string BindProjectStatusDetails()
    {
        try
        {
            projectMaster objprojectMaster = new projectMaster();
            List<ProjectStatusDetails> lsProjectStatusDetails = new List<ProjectStatusDetails>();
            lsProjectStatusDetails = objprojectMaster.GetProjectStatusDetails();
            var data = from pr in lsProjectStatusDetails
                       select new
                       {
                           pr.projStatusTId,
                           pr.projStatusTDesc
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch(Exception ex)
        {
            return null;
        }
        
    }

    }