using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;
using System.Data;
/// <summary>
/// Summary description for EmployeeCVDAL
/// </summary>
public class EmployeeCVDAL
{
    public EmployeeCVDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public List<EmployeeCVBLL> getEmployeeData()
    {
        List<EmployeeCVBLL> curproject = new List<EmployeeCVBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("USP_EmployeeMasterData", con);
        cmd.Parameters.AddWithValue("@mode", "List");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curproject.Add(new EmployeeCVBLL(
                    Convert.ToInt32(reader["empid"]),
                    reader["empName"].ToString(),
                    reader["SkillDesc"].ToString(),
                    reader["ResumePath"].ToString(),
                     reader["LastUploadedDate"].ToString(),
                     reader["LastUploadedBy"].ToString(),
                     Convert.ToInt32(reader["empExperince"].ToString()),
                     reader["empJoiningDate"].ToString(),
                     reader["skillDesc"].ToString(),
                      reader["empAddress"].ToString(),
                       reader["projectsWorkingOn"].ToString(),
                       reader["skillMatrixs"].ToString()
                ));
            }
        }
        return curproject;
    }

    public List<EmployeeCVBLL> UpdateEmpData(string ResumePath, int EmpId, int LastUploadedBy)
    {
        List<EmployeeCVBLL> curproject = new List<EmployeeCVBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("USP_EmployeeMasterData", con);
        cmd.Parameters.AddWithValue("@mode", "Update");
        cmd.Parameters.AddWithValue("@ResumePath", ResumePath);
        cmd.Parameters.AddWithValue("@empId", EmpId);
        cmd.Parameters.AddWithValue("@LastUploadedBy", LastUploadedBy);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
        }
        return curproject;
    }
}