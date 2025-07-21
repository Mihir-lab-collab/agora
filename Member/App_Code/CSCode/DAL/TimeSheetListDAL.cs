using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public class TimeSheetListDAL
{
    public IList<TimeSheetList> GetAllProject(string mode, int projId, int prjMemberId, int prjMonth, int PrjYear, int moduleid)
    {
        List<TimeSheetList> objProjTimesheet = new List<TimeSheetList>();
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_TimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@Tm_ProjectId", projId);
            cmd.Parameters.AddWithValue("@ProjMemberId", prjMemberId);
            cmd.Parameters.AddWithValue("@prjmonth", prjMonth);
            cmd.Parameters.AddWithValue("@Projyear", PrjYear);
            cmd.Parameters.AddWithValue("@ModuleId", moduleid);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objProjTimesheet.Add(new TimeSheetList(

                           Convert.ToInt32(reader["project_Id"]),
                           reader["project_Name"].ToString()
                          ));
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return objProjTimesheet;
    }

    //all module
    public IList<TimesheetModule> GetAllModule(string mode, int projId, int prjMemberId, int prjMonth, int PrjYear, int moduleid)
    {

        List<TimesheetModule> objModuleTimesheet = new List<TimesheetModule>();
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_TimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@Tm_ProjectId", projId);
            cmd.Parameters.AddWithValue("@ProjMemberId", prjMemberId);
            cmd.Parameters.AddWithValue("@prjmonth", prjMonth);
            cmd.Parameters.AddWithValue("@Projyear", PrjYear);
            cmd.Parameters.AddWithValue("@ModuleId", moduleid);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objModuleTimesheet.Add(new TimesheetModule(

                       Convert.ToInt32(reader["moduleId"]),
                        reader["ModuleName"].ToString()

                      ));
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return objModuleTimesheet;
    }

    public IList<TimesheetProjMember> GetMember(int LocationID, int ProjID)
    {

        List<TimesheetProjMember> objPrjMemberTimesheet = new List<TimesheetProjMember>();
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_TimeSheet1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GETMEMBER");
            cmd.Parameters.AddWithValue("@ProjId", ProjID);
            cmd.Parameters.AddWithValue("@LocationId", LocationID);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objPrjMemberTimesheet.Add(new TimesheetProjMember(
                           Convert.ToInt32(reader["empid"]),
                         reader["empName"].ToString()
                          ));
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return objPrjMemberTimesheet;
    }

    public IList<TimesheetProjMember> GetAllProjMember(string mode, int projId, int prjMemberId, int prjMonth, int PrjYear, int moduleid, int Location)
    {

        List<TimesheetProjMember> objPrjMemberTimesheet = new List<TimesheetProjMember>();
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_TimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@Tm_ProjectId", projId);
            cmd.Parameters.AddWithValue("@ProjMemberId", prjMemberId);
            cmd.Parameters.AddWithValue("@prjmonth", prjMonth);
            cmd.Parameters.AddWithValue("@Projyear", PrjYear);
            cmd.Parameters.AddWithValue("@ModuleId", moduleid);
            cmd.Parameters.AddWithValue("@LocationId", Location);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objPrjMemberTimesheet.Add(new TimesheetProjMember(
                           Convert.ToInt32(reader["empid"]),
                         reader["empName"].ToString()
                          ));
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return objPrjMemberTimesheet;
    }
    //all employee projects
    public IList<TimesheetEmpProj> GetEmpProject(string mode, int prjMemberId, int prjMonth, int PrjYear)
    {

        List<TimesheetEmpProj> objEmpProject = new List<TimesheetEmpProj>();
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_TimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@ProjMemberId", prjMemberId);
            cmd.Parameters.AddWithValue("@prjmonth", prjMonth);
            cmd.Parameters.AddWithValue("@Projyear", PrjYear);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objEmpProject.Add(new TimesheetEmpProj(
                       Convert.ToInt32(reader["projId"]),
                        reader["projName"].ToString()
                      ));
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return objEmpProject;
    }
    public void DeleteTimesheet(projectTimeSheet insertList, string mode, int id)
    {


        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_InsertTimesheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ModuleId ", DBNull.Value);
            cmd.Parameters.AddWithValue("@Empid ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsDate ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsHour ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsComment ", DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", insertList.tsId);
            cmd.Parameters.AddWithValue("@Mode", mode);

            using (con)
            {
                con.Open();
                id = Convert.ToInt32(cmd.ExecuteScalar());

            }
        }
        catch (Exception ex)
        {

        }

    }


    public void UpdateStatus(projectTimeSheet insertList, string mode, int id)
    {


        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_InsertTimesheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ModuleId ", DBNull.Value);
            cmd.Parameters.AddWithValue("@Empid ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsDate ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsHour ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsComment ", DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", insertList.tsId);
            cmd.Parameters.AddWithValue("@Mode", mode);

            using (con)
            {
                con.Open();
                id = Convert.ToInt32(cmd.ExecuteScalar());

            }
        }
        catch (Exception ex)
        {

        }

    }

    public int InsertTimesheet(projectTimeSheet insertList, string mode, int id)
    {

        if (mode == "INSERT")
        {
            id = 0;
        }
        else if (mode == "EDIT")
        {
            id = insertList.tsId;
        }


        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_InsertTimesheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ModuleId ", insertList.moduleId);
            cmd.Parameters.AddWithValue("@Empid ", insertList.empid);
            cmd.Parameters.AddWithValue("@TsDate ", insertList.tsDate);
            cmd.Parameters.AddWithValue("@TsHour ", insertList.tsHour);
            cmd.Parameters.AddWithValue("@TsComment ", insertList.tsComment);
            cmd.Parameters.AddWithValue("@Id", insertList.tsId);
            cmd.Parameters.AddWithValue("@Mode", mode);

            using (con)
            {
                con.Open();
                insertList.tsId = Convert.ToInt32(cmd.ExecuteScalar());


            }
        }
        catch (Exception ex)
        {

        }
        return insertList.tsId;
    }

    public projectTimeSheet getdata(int tsId, string mode)
    {
        projectTimeSheet displist = new projectTimeSheet();
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_InsertTimesheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ModuleId ", displist.moduleId);
            cmd.Parameters.AddWithValue("@Empid ", displist.empid);
            cmd.Parameters.AddWithValue("@TsDate ", displist.tsDate);
            cmd.Parameters.AddWithValue("@TsHour ", displist.tsHour);
            cmd.Parameters.AddWithValue("@TsComment ", displist.tsComment);
            cmd.Parameters.AddWithValue("@tsId", tsId);
            cmd.Parameters.AddWithValue("@Mode ", mode);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {


                        displist = new projectTimeSheet(

                        Convert.ToInt32(reader["_tsId"]),
                        Convert.ToInt32(reader["moduleId"]),
                        Convert.ToInt32(reader["empid"]),
                        Convert.ToDateTime(reader["tsDate"]),
                        Convert.ToInt32(reader["tsHour"]),
                        reader["tsComment"].ToString()
                      );
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return displist;
    }


    public projectTimeSheet getAllTimesheetList(int EmpId, string mode)
    {
        projectTimeSheet displist = new projectTimeSheet();
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_UpdateEmpTimesheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ModuleId ", DBNull.Value);
            cmd.Parameters.AddWithValue("@Empid ", displist.empid);
            cmd.Parameters.AddWithValue("@TsDate ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsHour ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsComment ", DBNull.Value);
            cmd.Parameters.AddWithValue("@tsId", DBNull.Value);
            cmd.Parameters.AddWithValue("@Mode ", mode);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {


                        displist = new projectTimeSheet(
                        Convert.ToInt32(reader["_tsId"]),
                        Convert.ToInt32(reader["moduleId"]),
                        Convert.ToInt32(reader["empid"]),
                        Convert.ToDateTime(reader["tsDate"]),
                        Convert.ToInt32(reader["tsHour"]),
                        reader["tsComment"].ToString()

                     );
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return displist;
    }

    //get list as per id

    public IList<projectTimeSheet> GetTaskData(string mode, int taskID)
    {

        List<projectTimeSheet> objEmpProject = new List<projectTimeSheet>();
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_InsertTimesheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ModuleId ", DBNull.Value);
            cmd.Parameters.AddWithValue("@Empid ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsDate ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsHour ", DBNull.Value);
            cmd.Parameters.AddWithValue("@TsComment ", DBNull.Value);
            cmd.Parameters.AddWithValue("@tsId", taskID);
            cmd.Parameters.AddWithValue("@Mode ", mode);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objEmpProject.Add(new projectTimeSheet(
                       Convert.ToInt32(reader["tsID"]),
                        Convert.ToInt32(reader["moduleId"]),
                        Convert.ToInt32(reader["empid"]),
                         Convert.ToDateTime(reader["tsDate"]),
                         Convert.ToInt32(reader["tsHour"]),
                         Convert.ToString(reader["tsComment"])

                      ));
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return objEmpProject;
    }

    public List<clsTimeSheetEmail> getWBSID()
    {
        UserMaster UM;
        UM = UserMaster.UserMasterInfo();
        List<clsTimeSheetEmail> curProjectWBS = new List<clsTimeSheetEmail>();
        try
        {
            //   SqlConnection con = new SqlConnection(_strConnection);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_ProjectWBS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "GetWBSID");
            cmd.Parameters.AddWithValue("@EmpId", Convert.ToInt32(UM.EmployeeID));

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
}