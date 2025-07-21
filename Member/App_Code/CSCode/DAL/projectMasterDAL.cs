using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Customer.BLL;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Customer.DAL
{
    public class projectMasterDAL
    {
        public projectMasterDAL()
        {
        }

        public List<projectMaster> Projects(bool All = false, int CustID = 0, int ProjID = 0, string status = "")
        {
            List<projectMaster> curprojects = new List<projectMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Project", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (All)
            {
                cmd.Parameters.AddWithValue("@Mode", "SELECTALL");
            }
            else
            {
                cmd.Parameters.AddWithValue("@Mode", "SELECT");
            }

            cmd.Parameters.AddWithValue("@CustID", CustID);
            cmd.Parameters.AddWithValue("@ProjID", ProjID);
            cmd.Parameters.AddWithValue("@Status", status);
            SqlDataReader reader = null;
            Nullable<DateTime> dt = null;

            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curprojects.Add(new projectMaster(
                       Convert.ToInt32(reader["projId"]),
                       Convert.ToInt32(reader["custId"]),
                       reader["projName"].ToString(),
                       reader["projDesc"].ToString(),
                       Convert.ToInt32(reader["projManager"].ToString()),
                       Convert.ToString(reader["AccountMgr"]) == "" ? 0 : Convert.ToInt32(Convert.ToString(reader["AccountMgr"])),
                       reader["projStartDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projStartDate"].ToString()),
                       (reader["duration"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["duration"]),
                       (reader["projActComp"] == DBNull.Value) ? dt : Convert.ToDateTime(reader["projActComp"]),
                       Convert.ToInt16(reader["currID"].ToString()),
                       reader["currExRate"].ToString() == "" ? 0 : Convert.ToDecimal(reader["currExRate"].ToString()),
                       (reader["Budget"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["Budget"].ToString()),
                       (reader["OtherEmailId"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["OtherEmailId"]),
                       reader["Status"].ToString(),
                       Convert.ToString(reader["RevisedBudget"]) == "" ? 0 : Convert.ToDecimal(reader["RevisedBudget"].ToString()),
                       Convert.ToString(reader["CreditAmount"]) == "" ? 0 : Convert.ToDecimal(reader["CreditAmount"].ToString()),
                       Convert.ToInt16(reader["IsInhouse"]),
                       Convert.ToInt16(reader["IsOngoing"]),
                       Convert.ToString(reader["ReportDate"]) == "" ? DateTime.MinValue : Convert.ToDateTime(reader["ReportDate"].ToString()),
                        (reader["TotalInvoiced"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["TotalInvoiced"]),
                       (reader["TotalRecieved"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["TotalRecieved"]),
                     Convert.ToInt16(reader["IsSendMail"]), reader["ProjectType"].ToString(),
                     (reader["InitialProjectCost"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["InitialProjectCost"]),
                     Convert.ToInt16(reader["IsTracked"]),
                     Convert.ToString(reader["BA"]) == "" ? 0 : Convert.ToInt32(Convert.ToString(reader["BA"])), reader["currSymbol"].ToString(),
                       reader["custCompany"].ToString()

                        ));
                }
            }
            return curprojects;
        }

        public int UpdateProject(projectMaster objprojectMaster)
        {
            int projId = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_Project", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MODE", "UPDATE");
                cmd.Parameters.AddWithValue("@projId", objprojectMaster.projId);
                cmd.Parameters.AddWithValue("@custId", objprojectMaster.custId);
                cmd.Parameters.AddWithValue("@projName", objprojectMaster.projName);
                cmd.Parameters.AddWithValue("@projDesc", objprojectMaster.projDesc);
                cmd.Parameters.AddWithValue("@projManager", objprojectMaster.projManager);
                cmd.Parameters.AddWithValue("@AccountMgr", objprojectMaster.AccountMgr);
                cmd.Parameters.AddWithValue("@projStartDate", objprojectMaster.projStartDate);
                cmd.Parameters.AddWithValue("@projDuration", objprojectMaster.projDuration);
                cmd.Parameters.AddWithValue("@projActComp", objprojectMaster.projActComp);
                cmd.Parameters.AddWithValue("@currID", objprojectMaster.currID);
                cmd.Parameters.AddWithValue("@currExRate", objprojectMaster.currExRate);
                cmd.Parameters.AddWithValue("@projCost", objprojectMaster.projCost);
                cmd.Parameters.AddWithValue("@OtherEmailId", objprojectMaster.OtherEmailId);
                cmd.Parameters.AddWithValue("@inHouse", objprojectMaster.InHouse);
                cmd.Parameters.AddWithValue("@onGoing", objprojectMaster.OnGoing);
                cmd.Parameters.AddWithValue("@ReportDate", objprojectMaster.projReportDate);
                cmd.Parameters.AddWithValue("@IsSendMail", objprojectMaster.isSendEmail);
                cmd.Parameters.AddWithValue("@ProjectType", objprojectMaster.ProjectType);
                cmd.Parameters.AddWithValue("@InitialProjectCost", objprojectMaster.InitialProjectCost);
                cmd.Parameters.AddWithValue("@IsTracked", objprojectMaster.IsTracked);
                cmd.Parameters.AddWithValue("@BA", objprojectMaster.BA);
                using (con)
                {
                    con.Open();
                    projId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception)
            {

            }
            return projId;
        }

        public bool GetAlertCC(int projId)
        {
            bool projAlertCC = false;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_GetAlertCCByProjID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectID", projId);

                using (con)
                {
                    con.Open();
                    projAlertCC = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            catch (Exception)
            {

            }
            return projAlertCC;
        }

        public List<projectMaster> GetProjectTitle()
        {
            List<projectMaster> curprojects = new List<projectMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Project", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "SELECTALL_PROJTITLE");

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    curprojects.Add(new projectMaster(

                        reader["projName"].ToString(),
                        Convert.ToInt32(reader["projId"].ToString())
                        ));
                }
            }
            return curprojects;
        }

        public string GetConfigValue()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_Project", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GetConfigDetails");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string ConfigValue = dt.Rows[0]["Value"].ToString();
            return ConfigValue;
        }

        public List<projectMaster> GetInCompleteProjectsStatus(string mode, string empID)
        {
            List<projectMaster> InCompleteStatusList = new List<projectMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Project", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmployeeID", Convert.ToInt32(empID));

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                Nullable<DateTime> dt = null;
                while (reader.Read())
                {
                    InCompleteStatusList.Add(new projectMaster(

                        Convert.ToInt32(reader["projId"]),
                       reader["projName"].ToString(),

                       string.IsNullOrEmpty(Convert.ToString(reader["projectStatusDate"])) ? dt : Convert.ToDateTime(reader["projectStatusDate"])
                        ));
                }
            }
            return InCompleteStatusList;
        }

        public string checkManagerforProject(string mode, string empID)
        {
            string sReturn = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_Project", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmployeeID", Convert.ToInt32(empID));
            try
            {
                using (con)
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Result"].ToString() == "1")
                        {
                            sReturn = "True";
                        }
                        else
                        {
                            sReturn = "False";
                        }
                    }

                }
            }
            catch (Exception ex)
            { }
            return sReturn;
        }

        public List<ProjectHourDetails> GetDetails(int EmpID)
        {
            List<ProjectHourDetails> curprojects = new List<ProjectHourDetails>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Project", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "selectProjectHourDetails");
            cmd.Parameters.AddWithValue("@EmployeeID", EmpID);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProjectHourDetails obj = new ProjectHourDetails();
                    obj.ProjID=Convert.ToInt32(reader["ProjID"]);
                    obj.projName = Convert.ToString(reader["projName"]);
                    obj.EstHours=Convert.ToInt32(reader["EstHours"]);
                    obj.ActualHours = Convert.ToString(reader["CalActualHours"]).Substring(0, Convert.ToString(reader["CalActualHours"]).IndexOf(":"));
                    obj.ProjectManager = Convert.ToString(reader["ProjectManager"]);
                    obj.AccountManager = Convert.ToString(reader["AccountManager"]);
                    obj.BusinessAnalyst = Convert.ToString(reader["BusinessAnalyst"]);

                    curprojects.Add(obj);
                }
            }
            return curprojects;
        }
       
        public List<ProjecEstDetails> GetEstDetails()
        {
            List<ProjecEstDetails> curprojects = new List<ProjecEstDetails>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Project", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "selectProjectEstDetails");

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProjecEstDetails obj = new ProjecEstDetails();
                    obj.ProjID = Convert.ToInt32(reader["ProjID"]);
                    obj.ProjectMileStoneID = Convert.ToInt32(reader["ProjectMileStoneID"]);
                    obj.Name = Convert.ToString(reader["Name"]);
                    obj.DueDate = Convert.ToString(reader["DueDate"]);
                    obj.DeliveryDate = Convert.ToString(reader["DeliveryDate"]);
                    obj.EstHours = Convert.ToString(reader["CalEstHours"]).Substring(0, Convert.ToString(reader["CalEstHours"]).IndexOf(":"));
                    obj.ActualHours = Convert.ToString(reader["CalActualHours"]).Substring(0, Convert.ToString(reader["CalActualHours"]).IndexOf(":"));
                    obj.ComputedMSHours = Convert.ToString(reader["ComputedMSHours"]);

                    curprojects.Add(obj);
                }
            }
            return curprojects;
        }
        public List<ProjectStatusDetails> GetProjectStatusDetails()
        {
            List<ProjectStatusDetails> curprojects = new List<ProjectStatusDetails>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetAllprojectStatusTypeMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProjectStatusDetails obj = new ProjectStatusDetails();
                    obj.projStatusTId = Convert.ToInt32(reader["projStatusTId"]);
                    obj.projStatusTDesc = Convert.ToString(reader["projStatusTDesc"]);
                    curprojects.Add(obj);
                }
            }
            return curprojects;
        }
    }
}