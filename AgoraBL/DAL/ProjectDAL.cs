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
    public class ProjectDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public int InsertprojectMember(int projId, int empid, string mode)
        {
            int projectId = 0;
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_AddRemoveProjectMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjId", projId);
                cmd.Parameters.AddWithValue("@Empid", empid);
                cmd.Parameters.AddWithValue("@Mode", mode);
                using (con)
                {
                    con.Open();
                    projectId = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {

            }
            return projId;
        }
        public int DeleteprojectMemberByprojid(int projId, string mode,int empId)
        {
            int isdeletd = 0;
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_AddRemoveProjectMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@projId", projId);
                if (empId>0)
                {
                    cmd.Parameters.AddWithValue("@EmpId", empId); 
                }
                using (con)
                {
                    con.Open();
                    isdeletd = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
            return isdeletd;
        }
        public List<Projects> GetProjectByRole(int empId, string mode)
        {
            List<Projects> lstprojects = new List<Projects>();
            Projects objProjects;
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_AddRemoveProjectMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@empId", empId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objProjects = new Projects
                        {
                            ProjectId = Convert.ToInt32(dt.Rows[i]["projId"]),
                            ProjectName = Convert.ToString(dt.Rows[i]["projName"]),
                            ProjectDesc = Convert.ToString(dt.Rows[i]["projDesc"]),
                            ProjectStartDate = Convert.ToString(dt.Rows[i]["projStartDate"]),
                            IsInhouse = Convert.ToBoolean(dt.Rows[i]["IsInhouse"]),
                            IsTracked = Convert.ToBoolean(dt.Rows[i]["IsTracked"]),
                            ProjStatus = Convert.ToInt32(dt.Rows[i]["ProjStatus"]),
                            Status = Convert.ToString(dt.Rows[i]["Status"]),
                            PM = new PM()
                            {
                                EmpId = Convert.ToInt32(dt.Rows[i]["PMId"]),
                                EmpName = Convert.ToString(dt.Rows[i]["PMName"])
                            },
                            BA = new BA()
                            {
                                EmpId = Convert.ToInt32(dt.Rows[i]["BAId"]),
                                EmpName = Convert.ToString(dt.Rows[i]["BAName"])
                            },
                            AM = new AM()
                            {
                                EmpId = Convert.ToInt32(dt.Rows[i]["AMId"]),
                                EmpName = Convert.ToString(dt.Rows[i]["AMName"])
                            },
                            EmployeeDetails = Convert.ToString(dt.Rows[i]["EmployeeDetails"]),
                            //ProjectMemberList = GetProjectMemberByProjId(Convert.ToInt32(dt.Rows[i]["projId"])),
                            IsAccessible = Convert.ToBoolean(dt.Rows[i]["IsAccessible"]),
                        };
                        lstprojects.Add(objProjects);
                    }
                }
                else
                {
                    objProjects = new Projects
                    {
                        IsAccessible = false
                    };
                    lstprojects.Add(objProjects);
                }
            }
            catch (Exception ex)
            {

            }
            return lstprojects;
        }
        public List<ProjectMember> GetEmpDetailsByEntityName(string entityName, string mode)
        {
            List<ProjectMember> lstProjectMember = new List<ProjectMember>();
            ProjectMember objProjectMember = new ProjectMember();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_AddRemoveProjectMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EntityName", entityName);
                cmd.Parameters.AddWithValue("@mode", mode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objProjectMember = new ProjectMember
                        {
                            EmpId = Convert.ToInt32(dt.Rows[i]["empid"]),
                            EmpName = Convert.ToString(dt.Rows[i]["empName"]),
                        };
                        lstProjectMember.Add(objProjectMember);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstProjectMember;
        }
        public bool CheckAuth(int empId, string mode)
        {
            bool isAuthCheck = false;
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_AddRemoveProjectMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@empId", empId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    isAuthCheck = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isAuthCheck;
        }
        public ProjectStackHolderDTO GetProjectStackHolder(int projId,string role,int newEmpId)
        {
            ProjectStackHolderDTO objProject = new ProjectStackHolderDTO();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_AddRemoveProjectMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjId", projId);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Empid", newEmpId);
                cmd.Parameters.AddWithValue("@Mode", "GetProjectStackHolder");
                using (con)
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        objProject = new ProjectStackHolderDTO
                        {
                            OldEmpId = Convert.ToInt32(dt.Rows[0]["OldEmpId"]),
                            OldEmpName= Convert.ToString(dt.Rows[0]["OldEmpName"]),
                            NewEmpId = Convert.ToInt32(dt.Rows[0]["NewEmpId"]),
                            NewEmpName = Convert.ToString(dt.Rows[0]["NewEmpName"]),
                            ProjId = Convert.ToInt32(dt.Rows[0]["projId"]),
                            ProjName= Convert.ToString(dt.Rows[0]["projName"]),
                            Role = Convert.ToString(dt.Rows[0]["Role"]),
                            IsSucess = true
                        };

                    }
                    else
                    {
                        objProject = new ProjectStackHolderDTO
                        {
                            IsSucess = false
                        };

                    }

                }
            }
            catch (Exception ex)
            {

            }
            return objProject;
        }
        public int UpdateProjectStackHolder(int projId, string role, int stackHolderId, int empId)
        {
            int updatedMember = 0;
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("USP_AI_AddRemoveProjectMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjId", projId);
                cmd.Parameters.AddWithValue("@Empid", empId);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@StackHolder", stackHolderId);
                cmd.Parameters.AddWithValue("@Mode", "UpdateProjectStackHolder");
                using (con)
                {
                    con.Open();
                    updatedMember = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

            }
            return updatedMember;
        }

    }
}
