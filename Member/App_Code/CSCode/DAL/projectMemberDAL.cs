using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Customer.DAL
{
    /// <summary>
    /// Summary description for CustUserDAL
    /// </summary>
    public class projectMemberDAL : projectMaster
    {
        public projectMemberDAL()
        {
        }
        public int InsertprojectMember(projectMember objprojectMember)
        {
            int projId = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_InsertprojectMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@projId", objprojectMember.projId);
                cmd.Parameters.AddWithValue("@empid", objprojectMember.empid);
                using (con)
                {
                    con.Open();
                    projId = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {

            }
            return projId;
        }
        public int DeleteprojectMemberByprojid(int projId)
        {
            int isdeletd = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_DeleteprojectMemberByprojid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@projId", projId);
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

        public List<projectMember> GetProjectMembersByProjId(int ProjId)
        {
            List<projectMember> curprojectmembers = new List<projectMember>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetProjectMembersByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjId", ProjId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curprojectmembers.Add(new projectMember(
                        Convert.ToInt32(reader["projId"]),
                        Convert.ToInt32(reader["empid"].ToString())
                        ));
                }
            }
            return curprojectmembers;
        }

        //Added by Trupti on 19 Jully 2018

        public List<projectMember> GetProjectIdByEmpId(int EmpId)
        {
            List<projectMember> curproject = new List<projectMember>();
            List<projectMaster> projectNamelist = new List<projectMaster>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetProjectIdByEmpId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpId", EmpId);
            SqlDataReader reader = null;
            try
            {
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        curproject.Add(new projectMember(
                            Convert.ToInt32(reader["empid"].ToString()),
                            Convert.ToString(reader["projName"]),
                            Convert.ToString(reader["empName"].ToString())

                            ));


                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (con.State == ConnectionState.Open)
                { con.Close(); }

                //con.Dispose();
            }
            return curproject;
        }

        //

        public int InsertUpdateProjectMem(projectMember objprojectMember)
        {
            int projId = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_UpdateTeamDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@projId", objprojectMember.projId);
                cmd.Parameters.AddWithValue("@empid", objprojectMember.empid);
                using (con)
                {
                    con.Open();
                    projId = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {

            }
            return projId;
        }

        public int DeleteprojectMember(projectMember objprojectMember)
        {
            int projId = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_DeleteTeamMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@projId", objprojectMember.projId);
                //cmd.Parameters.AddWithValue("@empid", objprojectMember.empid);
                using (con)
                {
                    con.Open();
                    projId = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {

            }
            return projId;
        }


        //added by Trupti for removing employee from all projectmemberlist
        public int DeleteprojectMemberbyEmpId(projectMember objprojectMember)
        {
            int empid = 0;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                
                SqlCommand cmd = new SqlCommand("[sp_DeleteTeamMemberByEmpId]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", objprojectMember.empid);
                //cmd.Parameters.AddWithValue("@empid", objprojectMember.empid);
                using (con)
                {
                    con.Open();
                    empid = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {

            }

            finally
            {
                if (con.State == ConnectionState.Open)
                { con.Close(); }


                con.Dispose();
            }
            return empid;
        }
//
        public int DeleteprojectAppraisalAuthorityMemberByprojid(int projId)
        {
            int isdeletd = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_DeleteprojectAppraisalAuthorityMemberByprojid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@projId", projId);
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

        public int InsertprojectAppraisalAuthorityMember(projectMember curprojectMember)
        {
            int projId = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("sp_InsertprojectAppraisalAuthorityMember", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@projId", curprojectMember.projId);
                cmd.Parameters.AddWithValue("@empid", curprojectMember.empid);
                using (con)
                {
                    con.Open();
                    projId = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {

            }
            return projId;
        }

        internal List<projectMember> GetProjectAppraisalAuthorityMemberByProjId(int ProjId)
        {
            List<projectMember> curprojectmembers = new List<projectMember>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetprojectAppraisalAuthorityMemberByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjId", ProjId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curprojectmembers.Add(new projectMember(
                        Convert.ToInt32(reader["projId"]),
                        Convert.ToInt32(reader["empid"].ToString())
                        ));
                }
            }
            return curprojectmembers;
        }
        public List<projectMember> GetProjectMembersNameByProjId(int ProjId)
        {
            List<projectMember> curprojectmembers = new List<projectMember>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetProjectMembersNameByProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjId", ProjId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curprojectmembers.Add(new projectMember(
                        Convert.ToInt32(reader["projId"]),
                        Convert.ToString(reader["empName"].ToString())
                        ));
                }
            }
            return curprojectmembers;
        }
    }
}