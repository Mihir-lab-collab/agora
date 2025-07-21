using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Customer.BLL;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services;
using System.Runtime.Serialization;


/// <summary>
/// Summary description for CustUsersDAL
/// </summary>
public class UsersDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    #region Constructor
    public UsersDAL()
    {
    }
    #endregion

    #region Methods
    public Users login(string UserId, string Password)
    {
        Users curUser = new Users();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_CustLogin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserMasterID", UserId);
            cmd.Parameters.AddWithValue("@Password", Password);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curUser = new Users(Convert.ToInt32(reader["UserMasterID"]), Convert.ToInt32(reader["CustID"]), reader["Password"].ToString(), reader["FName"].ToString(), reader["LName"].ToString(), reader["Email"].ToString(), Convert.ToString(reader["ContactNo"]), Convert.ToBoolean(reader["IsAdmin"].ToString()), reader["LastLogin"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["LastLogin"].ToString()), reader["LastLoginIP"].ToString(), reader["Status"].ToString(), reader["InsertedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"].ToString()), reader["ModifiedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"].ToString()));
                }
            }
        }
        catch (Exception ex)
        { }
        return curUser;
    }

    public string CustUserDetails(int UserId)
    {
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("GetNameById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserMasterID", UserId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return reader["Name"].ToString();
                }
            }
        }
        catch (Exception ex)
        { }
        return "";
    }

    public bool UpdateUserProfile(string FName, string LName, string Email, string Password, string UserMasterID)
    {
        Profile custProfile = new Profile();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_UpdateCustProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FName", FName);
            cmd.Parameters.AddWithValue("@LName", LName);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@UserMasterID", Convert.ToInt32(UserMasterID));

            //SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        catch (Exception ex)
        {
            return false;
        }
        //return curUser;
    }

    public bool UpdateUserProfileAll(Users _Users)
    {
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_UpdateCustProfileAll", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FName", _Users.FName);
            cmd.Parameters.AddWithValue("@LName", _Users.LName);
            cmd.Parameters.AddWithValue("@Email", _Users.Email);
            cmd.Parameters.AddWithValue("@ContactNo", _Users.ContactNo);
            cmd.Parameters.AddWithValue("@Password", _Users.Password);
            cmd.Parameters.AddWithValue("@UserMasterID", Convert.ToInt32(_Users.UserMasterID));
            cmd.Parameters.AddWithValue("@isAdmin", _Users.IsAdmin);
            cmd.Parameters.AddWithValue("@Status", _Users.Status);
            cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);

            using (con)
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public class LeaveType
    {
        public string statusid { get; set; }
        public string statusdesc { get; set; }

        public List<LeaveType> GetLeaveType()
        {
            List<LeaveType> lstLeaveType = new List<LeaveType>();
            string ConnectionString = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["conString"]);
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT statusdesc, statusid FROM empStatus WHERE sttatusLeave=1", con);

            //cmd.Parameters.Add("@searchId", searchId);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                //da.Fill(dt);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lstLeaveType.Add(new LeaveType()
                    {
                        statusid = Convert.ToString(reader["statusid"]),
                        statusdesc = Convert.ToString(reader["statusdesc"])
                    }
                     );
                }
                con.Close();
            }
            return lstLeaveType;
        }
    }


    [DataContract]
    public class Projects
    {
        [DataMember]
        public string projectID { get; set; }
        [DataMember]
        public string projectName { get; set; }
        public List<Projects> getAllProjects(string strMode)
        {
            List<Projects> lstProjects = new List<Projects>();
            string ConnectionString = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["conString"]);
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Projects", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                //da.Fill(dt);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lstProjects.Add(new Projects()
                    {
                        projectID = Convert.ToString(reader["projId"]),
                        projectName = Convert.ToString(reader["projName"]),
                    }
                     );
                }
                con.Close();
            }
            return lstProjects;
        }


    }




    [DataContract]
    public class Timesheet
    {


        [DataMember]
        public int moduleID { get; set; }
        [DataMember]
        public string moduleName { get; set; }

        public List<ProjectModule> getModules(string strMode, int projId)
        {
            List<ProjectModule> lstModules = new List<ProjectModule>();
            lstModules = ProjectModule.GetProjectModulesByProjId(projId);    
            
            return lstModules;
        }

        [DataMember]
        public string TimesheetDate { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string Module { get; set; }
        [DataMember]
        public string TimesheetHours { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string TimesheetEntryDate { get; set; }
        [DataMember]
        public int timesheetID { get; set; }

        public List<Timesheet> getTimesheetData(int projId, int empid, string month, string year, string arg, string status)
        {
            List<Timesheet> lstModules = new List<Timesheet>();
            string ConnectionString = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("UpdateTimesheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProjId", projId);
            cmd.Parameters.Add("@EmpId", empid);
            cmd.Parameters.Add("@Month", month);
            cmd.Parameters.Add("@Year", year);
            cmd.Parameters.Add("@arg", arg);
            cmd.Parameters.Add("@Status", status);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lstModules.Add(new Timesheet()
                    {
                        timesheetID=Convert.ToInt32(reader["tsId"]),
                        TimesheetDate = Convert.ToString(reader["tsDate"]),
                        ProjectName = Convert.ToString(reader["projName"]),
                        Module = Convert.ToString(reader["moduleName"]),
                        TimesheetHours = Convert.ToString(reader["tsHour"]),
                        Comment = Convert.ToString(reader["tsComment"]),
                        TimesheetEntryDate = Convert.ToString(reader["tsEntryDate"])

                    }
                     );
                }
                con.Close();
            }
            return lstModules;
        }
    }

    public bool InsertUserProfile(Users _Users)
    {

        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_InsertUserProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustId", _Users.CustID);
            cmd.Parameters.AddWithValue("@Password", _Users.Password);
            cmd.Parameters.AddWithValue("@FName", _Users.FName);
            cmd.Parameters.AddWithValue("@LName", _Users.LName);
            cmd.Parameters.AddWithValue("@Email", _Users.Email);
            cmd.Parameters.AddWithValue("@ContactNo", _Users.ContactNo);
            cmd.Parameters.AddWithValue("@isAdmin", _Users.IsAdmin);
            cmd.Parameters.AddWithValue("@Status", _Users.Status);
            cmd.Parameters.AddWithValue("@InsertedOn", DateTime.Now);
            using (con)
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        catch (Exception ex)
        {
            return false;
        }
        //return curUser;
    }

    public List<Users> GetAllCustUsersByUserMasterID(int UserMasterID)
    {
        List<Users> _custusers = new List<Users>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetAllCustUsersByUserMasterID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserMasterID", Convert.ToInt32(UserMasterID));

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _custusers.Add(new Users(Convert.ToInt16(reader["UserMasterID"]),
                        Convert.ToInt16(reader["CustID"]),
                        Convert.ToString(reader["Password"]),
                        Convert.ToString(reader["FName"]),
                        Convert.ToString(reader["LName"]),
                        Convert.ToString(reader["Email"]),
                        Convert.ToString(reader["ContactNo"]),
                        Convert.ToBoolean(reader["IsAdmin"]),
                        Convert.ToString(reader["LastLogin"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["LastLogin"]),
                        Convert.ToString(reader["LastLoginIP"]),
                        Convert.ToString(reader["Status"]),
                         Convert.ToString(reader["InsertedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"]),
                        Convert.ToString(reader["ModifiedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"])));
                }
            }
        }
        catch (Exception ex)
        { }

        return _custusers;
    }

    public List<Users> GetAllCustUsersByCustID(int CustID)
    {
        List<Users> _custusers = new List<Users>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetAllCustUsersByCustID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustID", Convert.ToInt32(CustID));
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _custusers.Add(new Users(Convert.ToInt16(reader["UserMasterID"]),
                        Convert.ToInt16(reader["CustID"]),
                        Convert.ToString(reader["Password"]),
                        Convert.ToString(reader["FName"]),
                        Convert.ToString(reader["LName"]),
                        Convert.ToString(reader["Email"]),
                        Convert.ToString(reader["ContactNo"]),
                        Convert.ToBoolean(reader["IsAdmin"]),
                        Convert.ToString(reader["LastLogin"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["LastLogin"]),
                        Convert.ToString(reader["LastLoginIP"]),
                        Convert.ToString(reader["Status"]),
                         Convert.ToString(reader["InsertedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"]),
                        Convert.ToString(reader["ModifiedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"])));
                }
            }
        }
        catch (Exception ex)
        { }

        return _custusers;
    }

    public List<Users> GetAllCustUsers()
    {
        List<Users> _custusers = new List<Users>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetAllCustUsers", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _custusers.Add(new Users(Convert.ToInt16(reader["UserMasterID"]),
                        Convert.ToInt16(reader["CustID"]),
                        Convert.ToString(reader["Password"]),
                        Convert.ToString(reader["FName"]),
                        Convert.ToString(reader["LName"]),
                        Convert.ToString(reader["Email"]),
                        Convert.ToString(reader["ContactNo"]),
                        Convert.ToBoolean(reader["IsAdmin"]),
                        Convert.ToString(reader["LastLogin"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["LastLogin"]),
                        Convert.ToString(reader["LastLoginIP"]),
                        Convert.ToString(reader["Status"]),
                         Convert.ToString(reader["InsertedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"]),
                        Convert.ToString(reader["ModifiedOn"]) == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"])));
                }
            }
        }
        catch (Exception ex)
        { }

        return _custusers;
    }
    #endregion
}
