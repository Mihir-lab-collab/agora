using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_ProfileModule : Authentication
{
    UserMaster UM;
    public static int profileID;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        BindListKRA();
    }


    public class KRADetails
    {
        public int KRAID;
        public string KRANames;
        public string Title;
    }

    public class Profiles
    {
        public int ProfileID;
        public string Name;
        public bool IsAdmin;
        public int LocationID;
        public string LocationName;
        //public string ReportingManagerName;
        public int empid;
        public string empName;
        // public string cookingtime;
    }

    public class Location
    {
        public int LocationID;
        public string LocationName;
    }

    public class ReportingManager
    {
        public int empid;
        public string empName;
    }

    public class Module
    {
        public int ProfileID;
        public string ProfileName;
        public int ModuleID;
        public string Name;
        public bool IsChecked;
        public bool IsAdmin;
        // public string cookingtime;
    }
    public class TS
    {
        public string ProjectsID;
        public string ProjectsName;
        public string TSDate;
        public string Module;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static int InsertProfile(string ProfileID, string Name, string IsAdmin, string LocationID, string reportingmanagerID)
    {
        int result;

        result = new ProfileModuleDAL().InsertProfile(Name, Convert.ToBoolean(IsAdmin), Convert.ToInt32(LocationID), Convert.ToInt32(reportingmanagerID));



        return result;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static int UpdateProfile(string ProfileID, string Name, string IsAdmin, string LocationID, string reportingmanagerID)
    {
        int result;

        result = new ProfileModuleDAL().UpdateProfile(Name, Convert.ToBoolean(IsAdmin), Convert.ToInt32(LocationID), Convert.ToInt32(ProfileID), Convert.ToInt32(reportingmanagerID));
        return result;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static List<Location> GetLocations()
    {
        //DataTable dtPrj = null;
        Location objPrj;

        DataTable dtPrj = new ProfileModuleDAL().getLocations();
        List<Location> drlist = new List<Location>();
        foreach (DataRow row in dtPrj.Rows)
        {
            objPrj = new Location();
            objPrj.LocationID = Convert.ToInt32(row["LocationID"]);
            objPrj.LocationName = Convert.ToString(row["LocationName"]);
            drlist.Add(objPrj);
        }
        return drlist;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static string GetReportingManagers()
    {
        //DataTable dtPrj = null;
        //ReportingManager objPrj;

        ////DataTable dtPrj = new ProfileModuleDAL().getReportingManagers(); 
        //DataTable dtPrj = new EmployeeMaster.GetAllAccountMgr();
        //List<ReportingManager> drlist = new List<ReportingManager>();
        //foreach (DataRow row in dtPrj.Rows)
        //{
        //    objPrj = new ReportingManager();
        //    objPrj.empid = Convert.ToInt32(row["empid"]);
        //    objPrj.empName = Convert.ToString(row["empName"]);
        //    drlist.Add(objPrj);
        //}
        //return drlist;

        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(EmployeeMaster.GetAllAccountMgr());
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static List<Profiles> GetProfile(string ProfileID)
    {
        //DataTable dtPrj = null;
        Profiles objPrj;

        DataTable dtPrj = new ProfileModuleDAL().getProfile(ProfileID);
        List<Profiles> drlist = new List<Profiles>();
        foreach (DataRow row in dtPrj.Rows)
        {
            objPrj = new Profiles();
            objPrj.ProfileID = Convert.ToInt32(row["profileid"]);
            objPrj.Name = Convert.ToString(row["Name"]);
            if (row["IsAdmin"] != null && Convert.ToString(row["IsAdmin"]) != "" && Convert.ToBoolean(row["IsAdmin"]))
                objPrj.IsAdmin = true;
            else
                objPrj.IsAdmin = false;
            if (row["LocationID"] != null && Convert.ToString(row["LocationID"]) != "" && Convert.ToInt32(row["LocationID"]) > 0)
            {
                objPrj.LocationID = Convert.ToInt32(row["LocationID"]);
                objPrj.LocationName = Convert.ToString(row["LocationName"]);
            }
            else
            {
                objPrj.LocationID = 0;
                objPrj.LocationName = string.Empty;
            }

            if (row["empid"] != null && Convert.ToString(row["empid"]) != "" && Convert.ToInt32(row["empid"]) > 0)
            {
                objPrj.empid = Convert.ToInt32(row["empid"]);
                objPrj.empName = Convert.ToString(row["empName"]);
            }
            else
            {
                objPrj.empid = 0;
                objPrj.empName = string.Empty;
            }
            drlist.Add(objPrj);
        }
        return drlist;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static List<Module> GetParentModules(string ProfileID)
    {

        Module objPrj;
        DataTable dtPrj = new ProfileModuleDAL().getParentModules(ProfileID);

        List<Module> drlist = new List<Module>();
        foreach (DataRow row in dtPrj.Rows)
        {
            objPrj = new Module();
            objPrj.ModuleID = Convert.ToInt32(row["ModuleID"]);
            objPrj.ProfileID = Convert.ToInt32(ProfileID);
            objPrj.Name = Convert.ToString(row["Name"]);
            objPrj.ProfileName = Convert.ToString(row["ProfileName"]);
            if (row["IsAdmin"] != null && Convert.ToString(row["IsAdmin"]) != "" && Convert.ToBoolean(row["IsAdmin"]))
                objPrj.IsAdmin = true;
            else
                objPrj.IsAdmin = false;
            if (row["ProfileModuleid"] != null && Convert.ToString(row["ProfileModuleid"]) != "" && Convert.ToInt32(row["ProfileModuleid"]) > 0)
                objPrj.IsChecked = true;
            else
                objPrj.IsChecked = false;

            drlist.Add(objPrj);
        }
        return drlist;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static List<Module> GetModules(string ProfileID, string ModuleID)
    {

        Module objPrj;
        DataTable dtPrj = new ProfileModuleDAL().getModules(ProfileID, ModuleID);

        List<Module> drlist = new List<Module>();
        foreach (DataRow row in dtPrj.Rows)
        {
            objPrj = new Module();
            objPrj.ModuleID = Convert.ToInt32(row["ModuleID"]);
            objPrj.ProfileID = Convert.ToInt32(ProfileID);
            objPrj.Name = Convert.ToString(row["Name"]);
            //objPrj.Name = Convert.ToString(row["ProfileName"]);
            if (row["IsAdmin"] != null && Convert.ToString(row["IsAdmin"]) != "" && Convert.ToBoolean(row["IsAdmin"]))
                objPrj.IsAdmin = true;
            else
                objPrj.IsAdmin = false;
            if (row["ProfileModuleid"] != null && Convert.ToString(row["ProfileModuleid"]) != "" && Convert.ToInt32(row["ProfileModuleid"]) > 0)
                objPrj.IsChecked = true;
            else
                objPrj.IsChecked = false;

            drlist.Add(objPrj);
        }
        return drlist;
    }

    public static string UpdateCheckUncheck(string Mode, string ProfileID, string ModuleID, bool IsAdmin)
    {
        DataTable dtPrj = null;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        using (con)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_ProfileModule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", Mode);
            cmd.Parameters.AddWithValue("@ProfileID", Convert.ToInt32(ProfileID));
            cmd.Parameters.AddWithValue("@ModuleID", Convert.ToInt32(ModuleID));
            cmd.Parameters.AddWithValue("@IsAdmin", IsAdmin);
            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
            {
                dtPrj = new DataTable();
                sqlAdapter.Fill(dtPrj);
            }
        }
        return "";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static string UpdateAdminProfile(string ProfileID, string ModuleID, bool IsAdmin)
    {
        return UpdateCheckUncheck("UpdateAdminProfile", ProfileID, ModuleID, IsAdmin);
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static string UpdateAdminProfileModule(string ProfileID, string ModuleID, bool IsAdmin)
    {
        return UpdateCheckUncheck("UpdateAdminProfileModule", ProfileID, ModuleID, IsAdmin);
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static string DeleteProfileModule(string ProfileID, string ModuleID, bool IsAdmin)
    {
        return UpdateCheckUncheck("DeleteProfileModule", ProfileID, ModuleID, IsAdmin);
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static string InsertProfileModule(string ProfileID, string ModuleID, bool IsAdmin)
    {
        return UpdateCheckUncheck("InsertProfileModule", ProfileID, ModuleID, IsAdmin);
    }

    protected void lnkSaveKRA_Click(object sender, EventArgs e)
    {
        string KRA = hftxtKRANames.Value.Replace("'", "''");
        string[] KRANames = KRA.Split(',');

        string msg = string.Empty;
        if (profileID != 0)
        {

            KRABLL.DeleteKRAProfile(profileID);

            for (int i = 0; i < KRANames.Length; i++)
            {

                if (KRANames[i].Trim() != "")
                {

                    KRABLL.InsertUpdateKRAProfile(profileID, Convert.ToInt32(KRANames[i].ToString()));

                }
            }

            msg = "Updated Sucessfully";
            string curUrl = Request.RawUrl;
            messageBoxandSamepage(msg, curUrl);
        }

    }

    [System.Web.Services.WebMethod]
    public static string BindKRAS(int prfileid)
    {
        profileID = prfileid;
        DataTable dt = new DataTable();
        List<string> TEST = new List<string>();
        List<KRADetails> details = new List<KRADetails>();
        string cs = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("SP_GetKRAProfileDetails", con);
            cmd.Parameters.AddWithValue("@mode", "Select");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProfileId", profileID);
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dtrow in dt.Rows)
            {
                KRADetails user = new KRADetails();
                user.KRAID = Convert.ToInt32(dtrow["KRAID"]);
                user.KRANames = dtrow["KRANames"].ToString();
                user.Title = dtrow["KRANames"].ToString(); 
                details.Add(user);
            }

        }
      
        

        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(details);

    }

    private void messageBoxandSamepage(string message, string Url)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgboxsamepage", "alert('" + message + "');window.location='" + Url + "'; ", true);
    }

    protected void BindListKRA()
    {
        
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_KRA", con);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
           
            reader = cmd.ExecuteReader();
            lstKRA.DataSource = reader;
            lstKRA.DataTextField = "KRANames";
            lstKRA.DataValueField = "KRAID";
            lstKRA.DataBind();

            lstKRA.Rows = lstKRA.Items.Count;

            foreach (ListItem item in lstKRA.Items)
            {
                item.Attributes["title"] = item.Text;
            }

            //for (int i = 0; i < lstKRA.Items.Count; i++)
            //{

            //    if (i % 2 == 0)
            //    {
            //        //lstKRA.Items[i].Attributes["title"] = lstKRA.Items[0];
            //        lstKRA.Items[i].Attributes.Add("style", "");
            //        //lstKRA.Items[i].Attributes.Add("style", "color:#2e2e2e;background:#cbc8c8; height:5px; padding: 0.4em 0.6em;");
            //    }
            //    else
            //    {
            //        //lstKRA.Items[i].Attributes["title"] = lstKRA.Text;
            //        lstKRA.Items[i].Attributes.Add("style", "");
            //    }
            //}
        }
        con.Close();

    }
}