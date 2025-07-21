using AgoraBL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace AgoraBL.DAL
{
    class EmpTimesheetDAL
    {
        public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        //Add/Update timesheet
        public Boolean Update(int ModuleID, int EmpID, DateTime TSDate, int TSHour, string TSComment, int TSID)
        {
            Boolean Status = false;
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "UPDATE");
                cmd.Parameters.AddWithValue("@TSID", TSID);
                cmd.Parameters.AddWithValue("@ModuleID", ModuleID);
                cmd.Parameters.AddWithValue("@EmpID", EmpID);
                cmd.Parameters.AddWithValue("@TSDate", TSDate);
                cmd.Parameters.AddWithValue("@TSHour", TSHour);
                cmd.Parameters.AddWithValue("@TSComment", TSComment);

                using (con)
                {
                    con.Open();
                    var result = cmd.ExecuteScalar();
                    if (Convert.ToString(result) == "1")
                    {
                        Status = true;
                    }
                    else if (Convert.ToInt32(result) == 1)
                    {
                        Status = true;
                    }
                    else
                    {
                        Status = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Status;
        }

        //Delete timesheet
        public Boolean Delete(int TSID)
        {
            Boolean Status = false;
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "DELETE");
                cmd.Parameters.AddWithValue("@TSID", TSID);

                using (con)
                {
                    con.Open();
                    var result = cmd.ExecuteScalar();
                    if (Convert.ToString(result) == "1")
                    {
                        Status = true;
                    }
                    else if (Convert.ToInt32(result) == 1)
                    {
                        Status = true;
                    }
                    else
                    {
                        Status = false;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return Status;
        }

        public IList<TimesheetDTO> GetEmailDetailsByProjID(int ProjID)
        {

            List<TimesheetDTO> objPrjMemberTimesheet = new List<TimesheetDTO>();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GetEmailDetails");
                cmd.Parameters.AddWithValue("@ProjId", ProjID);

                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {

                            TimesheetDTO obj = new TimesheetDTO();
                            // obj.ProjID = Convert.ToInt32("ProjId");
                            obj.AccountManagerEmail = Convert.ToString(reader["AccEmail"]);
                            obj.ProjectManagerEmail = Convert.ToString(reader["PM_EmailID"]);
                            obj.ManagerName = Convert.ToString(reader["PMName"]);
                            obj.BAName = Convert.ToString(reader["BAName"]);
                            obj.BaEmail = Convert.ToString(reader["BAEmail"]);
                            objPrjMemberTimesheet.Add(obj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objPrjMemberTimesheet;
        }

        public List<clsTimeSheetEmail> getWBSID(int empId)
        {

            List<clsTimeSheetEmail> curProjectWBS = new List<clsTimeSheetEmail>();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "GetWBSID");
                cmd.Parameters.AddWithValue("@EmpId", empId);

                using (con)
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            clsTimeSheetEmail obj = new clsTimeSheetEmail();
                            obj.intWBSID = Convert.ToInt32(reader["ProjectWBSID"]);


                            curProjectWBS.Add(obj);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //ex.WriteErrorLog();
                ex.Message.ToString();
            }
            return curProjectWBS;
        }
        public List<ProjectModulelistDTO> GetProjectAndModulelist(UserIdentityData userIdentityData)
        {
            List<ProjectModulelistDTO> lstProjects = new List<ProjectModulelistDTO>();
            try
            {
                using (SqlConnection con = new SqlConnection(_strConnection))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AI_Emp_GetProjectAndModulelist", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpID", userIdentityData.EmpID);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // First Result Set: Project List
                            while (reader.Read())
                            {
                                ProjectModulelistDTO project = new ProjectModulelistDTO()
                                {
                                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                    ProjectName = reader["ProjectName"].ToString(),
                                };
                                lstProjects.Add(project);
                            }
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    int projectId = Convert.ToInt32(reader["ProjectID"]);
                                    var project = lstProjects.FirstOrDefault(p => p.ProjectId == projectId);
                                    if (project != null)
                                    {
                                        // Add modules to the project
                                        Models.Module module = new Models.Module()
                                        {
                                            ModuleID = Convert.ToInt32(reader["ModuleID"]),
                                            ModuleName = reader["ModuleName"].ToString()
                                        };
                                        project.Modules.Add(module);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log or rethrow)
            }
            return lstProjects;
        }
        public int SaveEmployeeTimesheetRequest(int EmpId, DateTime Requestdate,string Descripation,int InsertedBy)
        {
            int outputid = 0;
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_EmployeeTimeSheetRequest", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "SAVE");
            cmd.Parameters.AddWithValue("@EmpId", EmpId);
            cmd.Parameters.AddWithValue("@RequestDate", Requestdate);
            cmd.Parameters.AddWithValue("@Descripation", Descripation);
            cmd.Parameters.AddWithValue("@InsertedBy", InsertedBy);

            try
            {
                using (con)
                {
                    outputid = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            { }
            return outputid;
        }
        public bool CheckforApproval(int EmpId, DateTime reqestdate)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_EmployeeTimeSheetRequest", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GET");
            cmd.Parameters.AddWithValue("@EmpId", EmpId);
            cmd.Parameters.AddWithValue("@RequestDate", reqestdate);
            int count = (int)cmd.ExecuteScalar();
            bool check;
            if (count > 0)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            return check;
        }
        public bool CheckIsProjectMember(int tsId, int empId)
        {
           SqlConnection con = new SqlConnection(_strConnection);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Sp_EmployeeTimeSheetRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "IsProjectMember");
                cmd.Parameters.AddWithValue("@TSId", tsId);
                cmd.Parameters.AddWithValue("@EmpId", empId);
                SqlDataReader reader = null;
                reader = cmd.ExecuteReader();
                bool check;
                if (reader.HasRows)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
                return check;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetIncompleteTimesheet(string entityName, string fromDate, string toDate)
        {
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_GetIncompleteTimesheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GetIncompleteTimesheet");
                cmd.Parameters.AddWithValue("@EntityName", entityName);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);

                using (con)
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetAdminDetails()
        {
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_GetIncompleteTimesheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GetAdminDetails");
                using (con)
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
