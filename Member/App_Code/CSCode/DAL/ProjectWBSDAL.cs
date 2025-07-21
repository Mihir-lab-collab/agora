using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for ProjectWBSDAL
/// </summary>
public class ProjectWBSDAL
{


    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    public ProjectWBSDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<ProjectWBSBLL> getMileStone(string mode, int projid)
    {
        List<ProjectWBSBLL> curMileStone = new List<ProjectWBSBLL>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@ProjID", projid);

            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProjectWBSBLL obj = new ProjectWBSBLL();
                        obj.projID = Convert.ToInt32(reader["ProjID"]);
                        obj.projMilestoneID = Convert.ToInt32(reader["ProjectMileStoneID"]);
                        obj.name = Convert.ToString(reader["Name"]);
                        obj.dueDate = Convert.ToString(reader["DueDate"]);
                        obj.DeliveryDate = Convert.ToString(reader["DeliveryDate"]);
                        obj.EstHours = Convert.ToInt32(reader["EstHours"]);
                        obj.MilestoneHours = GetProjectHours(obj.projMilestoneID);
                        obj.ActualHrs = GetMilestoneActualHrs(obj.projMilestoneID, obj.projID);//Convert.ToInt32(reader["ActualHrs"]);//GetMilestoneActualHrs(obj.projID);
                        curMileStone.Add(obj);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //ex.WriteErrorLog();
        }
        return curMileStone;
    }


    public string GetProfileAccess(string mode, string empID)
    {
        string sReturn = "";
        SqlConnection con = new SqlConnection(_strConnection);
        // con.Open();
        SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@EmpId", Convert.ToInt32(empID));
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
                        sReturn = "true";
                    }
                    else
                    {
                        sReturn = "false";
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return sReturn;
    }

 
    public string checkWBSExists(string mode,string StartDate,string EndDate,string EmpID,int ProjID)
    {
        string status = "";
        SqlConnection con = new SqlConnection(_strConnection);
        // con.Open();
        SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@StartDate",StartDate);       
        cmd.Parameters.AddWithValue("@EndDate",EndDate);
        cmd.Parameters.AddWithValue("@Name", EmpID);
        cmd.Parameters.AddWithValue("@ProjectWBSID", ProjID);
        try
        {
            using (con)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    status = "true";
                }
                else
                {
                    status = "false";

                }
            }
        }
        catch (Exception ex)
        { }
        return status;
    }


    public List<ProjectWBSBLL> getProjectWBS(string mode, int projid, int showCompletedStatus)
    {
        List<ProjectWBSBLL> curProjectWBS = new List<ProjectWBSBLL>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@ProjID", projid);
            cmd.Parameters.AddWithValue("@showallstatus", showCompletedStatus);
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        ProjectWBSBLL obj = new ProjectWBSBLL();
                        obj.projMilestoneID = Convert.ToInt32(reader["ProjectMilestoneID"]);
                        obj.Milestone = Convert.ToString(reader["Milestone"]);
                        obj.WBSID = Convert.ToInt32(reader["WBSID"]);
                        obj.Hours = Convert.ToString(reader["EstHours"]);
                        obj.WBS = Convert.ToString(reader["WBS"]);
                        obj.StartDate = Convert.ToDateTime(reader["StartDate"].ToString());
                        obj.ActualHrs = Convert.ToString(reader["Actualhrs"]);//GetProjectActualHrs(obj.WBSID);//Convert.ToInt32(reader["ActualHours"]);
                        obj.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        obj.ActualStartDate = Convert.ToString(reader["ActualStartDate"]);
                        obj.Status = Convert.ToString(reader["Status"]);
                        obj.Remark = Convert.ToString(reader["Remark"]);
                        obj.ActualEndDate = GetWBSActualEndDate(obj.WBSID);//Convert.ToDateTime(reader["ActualEndDate"]);

                        curProjectWBS.Add(obj);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //ex.WriteErrorLog();
        }
        return curProjectWBS;
    }

    public List<ProjectWBSBLL> getMilestoneWBS(string mode, int _proid)
    {
        List<ProjectWBSBLL> curMilestoneWBS = new List<ProjectWBSBLL>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@ProjID", _proid);

            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        ProjectWBSBLL obj = new ProjectWBSBLL();
                        obj.projMilestoneID = Convert.ToInt32(reader["ProjectMileStoneID"]);
                        obj.Milestone = Convert.ToString(reader["Name"]);
                        curMilestoneWBS.Add(obj);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //ex.WriteErrorLog();
        }
        return curMilestoneWBS;
    }


    internal string InsertProjectWbs(ProjectWBSBLL objwbs)
    {
        string message = string.Empty;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", objwbs.mode);
            cmd.Parameters.AddWithValue("@ProjectMilestoneID", objwbs.projMilestoneID);
            cmd.Parameters.AddWithValue("@Description", objwbs.WBS);
            cmd.Parameters.AddWithValue("@StartDate", objwbs.tempSDate);
            cmd.Parameters.AddWithValue("@EndDate", objwbs.tempEDate);
            cmd.Parameters.AddWithValue("@EstHours", objwbs.Hours);
            cmd.Parameters.AddWithValue("@Status", objwbs.Status);
            cmd.Parameters.AddWithValue("@Remark", objwbs.Remark);
            cmd.Parameters.AddWithValue("@insertedby", objwbs.Insertedby);
            cmd.Parameters.AddWithValue("@ProjID", objwbs.projID);
            cmd.Parameters.AddWithValue("@WBSID", objwbs.WBSID);
            using (con)
            {
                con.Open();
                message = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception Ex)
        {

        }
        return message;
    }

    internal int getMilestoneId(string mode, string _MilestoneName)
    {
        int Id = 0;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@ProjectMilestonename", _MilestoneName);

            using (con)
            {
                con.Open();
                Id = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception Ex)
        {

        }
        return Id;

    }

    public void DeleteMilestone(string mode, int ProjectMileStoneID)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@ProjectMilestoneID", ProjectMileStoneID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {
            // ex.WriteErrorLog();
        }
    }

    public void InsertEmpID(string mode, string empId, string WbsId, int _projId)
    {
        //string message = string.Empty;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@AssignedTo", empId);
            cmd.Parameters.AddWithValue("@WBSID", WbsId);
            cmd.Parameters.AddWithValue("@ProjID", _projId);
            using (con)
            {
                con.Open();
                cmd.ExecuteScalar();
                con.Close();
            }
        }
        catch (Exception Ex)
        {

        }
        //return message;
    }

    //===============================================================
    public string GetProjectHours(int milestoneId) //Need to work on it. Not completed yet
    {
        string hours = "";
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("USP_GetAssignEmpDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", "SelectHours");
        cmd.Parameters.AddWithValue("@milestoneId", milestoneId);
        using (con)
        {
            con.Open();
            hours = Convert.ToString(cmd.ExecuteScalar());
        }
        return hours;
    }

    public List<ProjectWBSBLL> GetProjectMembersByProjId(int wbsId)
    {
        List<ProjectWBSBLL> curprojectmembers = new List<ProjectWBSBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("USP_GetAssignEmpDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", "Select");
        cmd.Parameters.AddWithValue("@WBSID", wbsId);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curprojectmembers.Add(new ProjectWBSBLL(
                    Convert.ToInt32(reader["projId"]),
                    Convert.ToInt32(reader["EmpId"].ToString()),
                    Convert.ToString(reader["empName"])
                    ));
            }
        }
        return curprojectmembers;
    }

    public int GetProjectActualHrs(int wbsId)
    {
        int actualhrs = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("USP_GetAssignEmpDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", "SelectActualhrs");
        cmd.Parameters.AddWithValue("@WBSID", wbsId);
        using (con)
        {
            con.Open();
            actualhrs = Convert.ToInt32(cmd.ExecuteScalar());
        }
        return actualhrs;
    }

    public string GetMilestoneActualHrs(int milestoneId, int _projId)
    {
        string actualhrs = "";
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("USP_GetAssignEmpDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", "SelectMilestoneactualHrs");
        cmd.Parameters.AddWithValue("@milestoneId", milestoneId);
        cmd.Parameters.AddWithValue("@ProjID", _projId);
        using (con)
        {
            con.Open();
            actualhrs = Convert.ToString(cmd.ExecuteScalar());
        }
        return actualhrs;
    }

    public string GetWBSActualEndDate(int WbsID)
    {
        string actualEndDate = "";
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("USP_GetAssignEmpDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", "SelectactualENDDate");
        cmd.Parameters.AddWithValue("@WBSID", WbsID);
        //cmd.Parameters.AddWithValue("@WBSID", _projId);
        using (con)
        {
            con.Open();
            actualEndDate = Convert.ToString(cmd.ExecuteScalar());
        }
        return actualEndDate;
    }
    //==============================================================

    public void DeleteEmpWBS(string mode, string WbsId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("USP_GetAssignEmpDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@WBSID", WbsId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {
            // ex.WriteErrorLog();
        }
    }


    //Added by AP



    public int InsertProjWBSDetails(ProjectWBSBLL objProjWBSDetails)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objProjWBSDetails.mode);
        cmd.Parameters.AddWithValue("@ProjectWBSID", objProjWBSDetails.ProjectWBSID);
        cmd.Parameters.AddWithValue("@Name", objProjWBSDetails.Name);
        cmd.Parameters.AddWithValue("@SDate", objProjWBSDetails.tempSDate);
        cmd.Parameters.AddWithValue("@EDate", objProjWBSDetails.tempEDate);
        if (objProjWBSDetails.WBSId == 0)
        {
            cmd.Parameters.AddWithValue("@WBS", DBNull.Value);
        }
        else
        {
            cmd.Parameters.AddWithValue("@WBS", objProjWBSDetails.WBSId);
        }
        cmd.Parameters.AddWithValue("@Comment", objProjWBSDetails.Comment);
        cmd.Parameters.AddWithValue("@Hours", objProjWBSDetails.DHours);
        cmd.Parameters.AddWithValue("@ModuleID", objProjWBSDetails.ModuleID);
        cmd.Parameters.AddWithValue("@InsertedByy", objProjWBSDetails.InsertedBy);
        cmd.Parameters.AddWithValue("@ProjID", objProjWBSDetails.projID);
        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception ex)
        { }
        return outputid;
    }

    /// <summary>
    /// Created by Ganesh Pawar : 17:11:2016
    /// </summary>
    public int DeleteProjWBS(int ProjectWBSID)
    {
        int outputid = 0;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "DeleteProjWBSTimesheet");
            cmd.Parameters.AddWithValue("@ProjectWBSID", ProjectWBSID);

            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        return outputid;
    }


    public List<ProjectWBSBLL> GetProjectWBSDetails(string mode, int employeeID, int projid, string StartDate, string EndDate)
    {
        List<ProjectWBSBLL> curProjectWBSDetails = new List<ProjectWBSBLL>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@EmpId", employeeID);
            cmd.Parameters.AddWithValue("@ProjID", projid);
            if(!String.IsNullOrEmpty(StartDate))
            {
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
            }
            if (!String.IsNullOrEmpty(EndDate))
            {
                cmd.Parameters.AddWithValue("@EndDate", EndDate);
            }
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProjectWBSBLL obj = new ProjectWBSBLL();
                        obj.WBS = Convert.ToString(reader["Description"]);
                        obj.Name = Convert.ToString(reader["empName"]);
                        obj.empId = Convert.ToString(reader["Name"]);
                        obj.ModuleName = Convert.ToString(reader["moduleName"]);
                        obj.SDate = Convert.ToDateTime(reader["StartDate"]);
                        obj.EDate = Convert.ToDateTime(reader["EndDate"]);
                        obj.strHours = Convert.ToString(reader["Hours"]);
                        obj.strMinutes = Convert.ToString(reader["Minutes"]);
                        obj.Comment = Convert.ToString(reader["Comment"]);
                        obj.ProjectWBSID = Convert.ToInt32(reader["ProjectWBSID"]);
                        obj.WBSId = Convert.ToInt32(reader["WBSID"]);
                        obj.Status = Convert.ToString(reader["Status"]);
                        obj.ModuleID = Convert.ToInt32(reader["ModuleID"]);

                        curProjectWBSDetails.Add(obj);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //ex.WriteErrorLog();
        }
        return curProjectWBSDetails;
    }

    public List<ProjectWBSBLL> BindWBS(string mode, int projid, int empid)
    {
        List<ProjectWBSBLL> curProjWBS = new List<ProjectWBSBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@ProjID", projid);
        cmd.Parameters.AddWithValue("@EmpId", empid);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curProjWBS.Add(new ProjectWBSBLL
                    (
                    Convert.ToInt32(reader["ProjectWBSID"]),
                    reader["Description"].ToString()

                    ));
            }
        }
        return curProjWBS;
    }


    public List<ProjectWBSBLL> BindAllWBS(string mode, int projid, int empid)
    {
        List<ProjectWBSBLL> curAllProjWBS = new List<ProjectWBSBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@ProjID", projid);
        cmd.Parameters.AddWithValue("@EmpId", empid);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curAllProjWBS.Add(new ProjectWBSBLL
                    (
                    Convert.ToInt32(reader["ProjectWBSID"]),
                    reader["Description"].ToString()

                    ));
            }
        }
        return curAllProjWBS;
    }


}