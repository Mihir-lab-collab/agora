using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration; 

public class TimeSheetDAL
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);

	public TimeSheetDAL()
	{

	}

    public IList<TimeSheet> GetManageTS(int TSID, int ProjID, int EmpID, int Month, int Year, int ModuleID, int LocationID, string IsApproved)
    {

        List<TimeSheet> objPrjMemberTimesheet = new List<TimeSheet>();
        try
        {
            SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GETManageTS");
            cmd.Parameters.AddWithValue("@TSID", TSID);
            cmd.Parameters.AddWithValue("@ProjID", ProjID);
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@Month", Month);
            cmd.Parameters.AddWithValue("@Year", Year);
            cmd.Parameters.AddWithValue("@ModuleID", ModuleID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@IsApproved", IsApproved);
            //cmd.Parameters.AddWithValue("@IsSendMail", 1);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objPrjMemberTimesheet.Add(new TimeSheet(
                           Convert.ToInt32(reader["TSID"]),
                           Convert.ToInt32(reader["ProjID"]),
                           reader["ProjName"].ToString(),
                           Convert.ToInt32(reader["ModuleID"]),
                           reader["ModuleName"].ToString(),
                           Convert.ToInt32(reader["EmpID"]),
                           reader["EmpName"].ToString(),
                           Convert.ToDateTime(reader["TSDate"]),
                           "",//reader["tsDisplayDate"].ToString(),
                           Convert.ToInt32(reader["TSHour"]),
                           reader["TSComment"].ToString(),
                           Convert.ToBoolean(reader["tsVerified"]),
                           Convert.ToDateTime(reader["tsEntryDate"]),
                           reader["tsVerifiedBy"].ToString(),
                           reader["tsVerifiedOn"].ToString(),
                           reader["ProjectTotalHrs"].ToString(),
                           Convert.ToBoolean(reader["IsSendMail"].ToString()),
                           false,//Convert.ToBoolean(reader["IsSaturday"]),
                           false,//Convert.ToBoolean(reader["IsSunday"]),
                           false,//Convert.ToBoolean(reader["IsHoliday"]),
                           false//Convert.ToBoolean(reader["IsLeave"])
                          ));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        return objPrjMemberTimesheet;
    }
    public IList<TimeSheet> GetTS(int TSID, int ProjID, int EmpID, int Month, int Year, int ModuleID, int LocationID, string IsApproved)
    {

        List<TimeSheet> objPrjMemberTimesheet = new List<TimeSheet>();
        try
        {
            SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GETTS");
            cmd.Parameters.AddWithValue("@TSID", TSID);
            cmd.Parameters.AddWithValue("@ProjID", ProjID);
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@Month", Month);
            cmd.Parameters.AddWithValue("@Year", Year);
            cmd.Parameters.AddWithValue("@ModuleID", ModuleID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@IsApproved", IsApproved);
            //cmd.Parameters.AddWithValue("@IsSendMail", 1);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objPrjMemberTimesheet.Add(new TimeSheet(
                           Convert.ToInt32(reader["TSID"]),
                           Convert.ToInt32(reader["ProjID"]),
                           reader["ProjName"].ToString(),
                           Convert.ToInt32(reader["ModuleID"]),
                           reader["ModuleName"].ToString(),
                           Convert.ToInt32(reader["EmpID"]),
                           reader["EmpName"].ToString(),
                           Convert.ToDateTime(reader["TSDate"]),
                           reader["tsDisplayDate"].ToString(),
                           Convert.ToInt32(reader["TSHour"]),
                           reader["TSComment"].ToString(),
                           Convert.ToBoolean(reader["tsVerified"]),
                           Convert.ToDateTime(reader["tsEntryDate"]),
                           reader["tsVerifiedBy"].ToString(),
                           reader["tsVerifiedOn"].ToString(),
                           reader["ProjectTotalHrs"].ToString(),
                           Convert.ToBoolean(reader["IsSendMail"].ToString()),
                           Convert.ToBoolean(reader["IsSaturday"]),
                           Convert.ToBoolean(reader["IsSunday"]),
                           Convert.ToBoolean(reader["IsHoliday"]),
                           Convert.ToBoolean(reader["IsLeave"])
                          )); 
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        return objPrjMemberTimesheet;
    }

    //Added by Trupti Dandekar for GetAll TimeSheet Data for Mobile App
    public List<TimeSheet> GetAllTimeSheetData(string mode)
    {

        List<TimeSheet> getTimeSheet = new List<TimeSheet>();

        SqlCommand cmd = new SqlCommand("sp_TimeSheet", con);
        cmd.Parameters.AddWithValue("@mode", mode);//GetAllTimeSheetData
        //cmd.Parameters.AddWithValue("@projId", projId);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getTimeSheet.Add(new TimeSheet(
                    Convert.ToInt32(reader["tsId"]),
                    Convert.ToInt32(reader["moduleId"]),
                    Convert.ToInt32(reader["empid"].ToString()),
                   Convert.ToDateTime(reader["tsDate"].ToString()),
                   Convert.ToInt32(reader["tsHour"].ToString()),
                    reader["tsComment"].ToString(),
                  Convert.ToDateTime(reader["tsEntryDate"].ToString()),
                  Convert.ToBoolean(reader["tsVerified"].ToString())
                    ));
                   
                    //reader["tsVerifiedBy"].ToString(),
                    //reader["tsVerifiedOn"].ToString(),
                    
                
            }
            con.Close();
        }
        return getTimeSheet;
    }

    //Added by vishal w for GetMultiResultUserDetails Data for Mobile App    
    public MultiResultUserDetails GetMultiResultUserDetails(string mode, int Month, int Year, int Empid,int TSID,DateTime TSDate)
    {

        MultiResultUserDetails ObjMRUD = new MultiResultUserDetails();        

        List<TimeSheet> getTimeSheet = new List<TimeSheet>();
        List<EmployeeProjectsNames> EmployeeProjectsNames = new List<EmployeeProjectsNames>();
        List<EmployeeProjectModule> EmployeeProjectModule = new List<EmployeeProjectModule>();
        List<EmployeePreviousRequestDates> EmployeePreviousRequestDates = new List<EmployeePreviousRequestDates>();
        List<EmployeeIncompleteTimesheet> EmployeeIncompleteTimesheet = new List<EmployeeIncompleteTimesheet>();
        List<EmployeeTimesheetFilledHrs> EmployeeTimesheetFilledHrs = new List<EmployeeTimesheetFilledHrs>();

        SqlCommand cmd = new SqlCommand("sp_TimeSheet", con);
        cmd.Parameters.AddWithValue("@mode", mode);//GetAllMultiResultUserDetailsData
        cmd.Parameters.AddWithValue("@Month", Month);
        cmd.Parameters.AddWithValue("@Year", Year);
        cmd.Parameters.AddWithValue("@EmpID", Empid);
        cmd.Parameters.AddWithValue("@TSID", TSID);
        cmd.Parameters.AddWithValue("@TSDate", TSDate);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;

        //var result = new DataSet();
        //var dataAdapter = new SqlDataAdapter(cmd);
        //dataAdapter.Fill(result);
        //getTimeSheet = result.Tables[0];

        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();

            //getTimeSheet
            while (reader.Read())
            {                
                getTimeSheet.Add(new TimeSheet(
                    Convert.ToInt32(reader["tsId"]),
                    Convert.ToInt32(reader["moduleId"]),
                    Convert.ToInt32(reader["empid"].ToString()),
                    Convert.ToDateTime(reader["tsDate"].ToString()),
                    Convert.ToInt32(reader["tsHour"].ToString()),
                    reader["tsComment"].ToString(),
                    Convert.ToDateTime(reader["tsEntryDate"].ToString()),
                    Convert.ToBoolean(reader["tsVerified"].ToString()),
                    reader["tsVerifiedBy"].ToString(),
                    reader["tsVerifiedOn"].ToString(),
                    Convert.ToInt32(reader["projId"].ToString()),
                    reader["projName"].ToString(),
                    reader["moduleName"].ToString()
                    ));
            }

            //EmployeeProjectsNames
            if (reader.NextResult())
            {
                while (reader.Read())
                {                    
                    EmployeeProjectsNames.Add(new EmployeeProjectsNames(
                    Convert.ToInt32(reader["projId"]),
                    reader["projName"].ToString(),
                    reader["empName"].ToString()
                    ));

                }
            }

            //EmployeeProjectModule
            if (reader.NextResult())
            {
                while (reader.Read())
                {                    
                    EmployeeProjectModule.Add(new EmployeeProjectModule(
                     Convert.ToInt32(reader["projId"]),
                    reader["moduleName"].ToString(),
                    reader["projName"].ToString(),
                    Convert.ToInt32(reader["moduleId"])
                    ));
                }
            }

            //EmployeePreviousRequestDates
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    EmployeePreviousRequestDates.Add(new EmployeePreviousRequestDates(
                        Convert.ToInt32(reader["EmpId"]),
                        reader["RequestDate"].ToString(),
                        reader["InsertedBy"].ToString(),
                        reader["insertedOn"].ToString()
                        ));
                }
            }

            //EmployeeIncompleteTimesheet
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    EmployeeIncompleteTimesheet.Add(new EmployeeIncompleteTimesheet(
                        Convert.ToInt32(reader["EmpID"]),
                        reader["EMPName"].ToString(),
                        reader["Date"].ToString(),
                        reader["TimeAvailable"].ToString(),
                        reader["TimeReported"].ToString()
                        ));
                }
            }

            //EmployeeTimesheetFilledHrs
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    EmployeeTimesheetFilledHrs.Add(new EmployeeTimesheetFilledHrs(
                        Convert.ToInt32(reader["FilledTotHrs"])
                        ));                    
                }
            }

            ObjMRUD.TimeSheet = getTimeSheet;
            ObjMRUD.EmployeeProjectsNames = EmployeeProjectsNames;
            ObjMRUD.EmployeeProjectModule = EmployeeProjectModule;
            ObjMRUD.EmployeePreviousRequestDates = EmployeePreviousRequestDates;
            ObjMRUD.EmployeeIncompleteTimesheet = EmployeeIncompleteTimesheet;
            ObjMRUD.EmployeeTimesheetFilledHrs = EmployeeTimesheetFilledHrs;
            ObjMRUD.EmployeeTimesheetFilledHrs = EmployeeTimesheetFilledHrs;            
            con.Close();
        }
        return ObjMRUD;
    }
    public MultiResultUserDetails GetMultiResultUserDetails_old(string mode, int Month, int Year, int Empid)
    {

        MultiResultUserDetails ObjMRUD = new MultiResultUserDetails();

        List<TimeSheet> getTimeSheet = new List<TimeSheet>();
        List<EmployeeProjectsNames> EmployeeProjectsNames = new List<EmployeeProjectsNames>();
        List<EmployeeProjectModule> EmployeeProjectModule = new List<EmployeeProjectModule>();

        SqlCommand cmd = new SqlCommand("sp_TimeSheet", con);
        cmd.Parameters.AddWithValue("@mode", mode);//GetAllMultiResultUserDetailsData
        cmd.Parameters.AddWithValue("@Month", Month);//GetAllMultiResultUserDetailsData
        cmd.Parameters.AddWithValue("@Year", Year);//GetAllMultiResultUserDetailsData
        cmd.Parameters.AddWithValue("@EmpID", Empid);//GetAllMultiResultUserDetailsData
        //cmd.Parameters.AddWithValue("@projId", projId);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;        

        //var result = new DataSet();
        //var dataAdapter = new SqlDataAdapter(cmd);
        //dataAdapter.Fill(result);
        //getTimeSheet = result.Tables[0];

        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //getTimeSheet.Add(new TimeSheet(
                //    Convert.ToInt32(reader["tsId"]),
                //    Convert.ToInt32(reader["moduleId"]),
                //    Convert.ToInt32(reader["empid"].ToString()),
                //    Convert.ToDateTime(reader["tsDate"].ToString()),
                //    Convert.ToInt32(reader["tsHour"].ToString()),
                //    reader["tsComment"].ToString(),
                //    Convert.ToDateTime(reader["tsEntryDate"].ToString()),
                //    Convert.ToBoolean(reader["tsVerified"].ToString()),
                //    reader["tsVerifiedBy"].ToString(),
                //    reader["tsVerifiedOn"].ToString()
                //    ));                

                reader.NextResult();

                EmployeeProjectsNames.Add(new EmployeeProjectsNames(
                    Convert.ToInt32(reader["projId"]),
                    reader["projName"].ToString(),
                    reader["empName"].ToString()
                    ));

                //ObjMRUD.EmployeeProjectsNames.Add(new EmployeeProjectsNames(
                //    Convert.ToInt32(reader["projId"]),
                //    reader["projName"].ToString(),
                //    reader["empName"].ToString()
                //    ));

                reader.NextResult();

                EmployeeProjectModule.Add(new EmployeeProjectModule(
                     Convert.ToInt32(reader["projId"]),
                    reader["moduleName"].ToString(),
                    reader["projName"].ToString(),
                    Convert.ToInt32(reader["moduleId"])
                    ));

                //ObjMRUD.EmployeeProjectModule.Add(new EmployeeProjectModule(
                //     Convert.ToInt32(reader["projId"]),
                //    reader["moduleName"].ToString(),
                //    reader["projName"].ToString()
                //    ));
            }
            ObjMRUD.TimeSheet = getTimeSheet;
            ObjMRUD.EmployeeProjectsNames = EmployeeProjectsNames;
            ObjMRUD.EmployeeProjectModule = EmployeeProjectModule;
            con.Close();
        }
        return ObjMRUD;
    }

    public IList<TimeSheet> GetEmailDetailsByProjID(int ProjID)
    {

        List<TimeSheet> objPrjMemberTimesheet = new List<TimeSheet>();
        try
        {
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

                        TimeSheet obj = new TimeSheet();
                       // obj.ProjID = Convert.ToInt32("ProjId");
                        obj.AccountManagerEmail=Convert.ToString(reader["AccEmail"]);
                        obj.ProjectManagerEmail = Convert.ToString(reader["PM_EmailID"]);
                        obj.ManagerName = Convert.ToString(reader["PMName"]);
                        objPrjMemberTimesheet.Add(obj);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        return objPrjMemberTimesheet;
    }



    public IList<TimeSheet> GetMember(int LocationID, int ProjID)
    {

        List<TimeSheet> objPrjMemberTimesheet = new List<TimeSheet>();
        try
        {
            SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GETMEMBERALL");
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

                        TimeSheet obj = new TimeSheet();
                        obj.EmpID = Convert.ToInt32(reader["EmpID"]);
                        obj.EmpName = Convert.ToString(reader["empName"]);
                        objPrjMemberTimesheet.Add(obj);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        return objPrjMemberTimesheet;
    }

    public Boolean Approve(int TSID, Boolean IsApproved, int EmpID)
    {
        Boolean Status = false;
        try
        {
            SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "APPROVE");
            cmd.Parameters.AddWithValue("@TSID", TSID);
            cmd.Parameters.AddWithValue("@IsApproved", IsApproved);
            cmd.Parameters.AddWithValue("@EmpID", EmpID);

            using (con)
            {
                con.Open();
                if(cmd.ExecuteScalar() == "1")
                {
                    Status = true;
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        return Status;
    }    

    public Boolean Delete(int TSID)
    {
        Boolean Status = false;
        try
        {
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
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        return Status;
    }

    public IList<TimeSheet> GetTSReport(int Month, int Year, int LocationID)
    {
        List<TimeSheet> objPrjMemberTimesheet = new List<TimeSheet>();
        SqlDataReader reader = null;
        SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@MODE", "REPORT");
        cmd.Parameters.AddWithValue("@Month", Month);
        cmd.Parameters.AddWithValue("@Year", Year);
        cmd.Parameters.AddWithValue("@LocationId", LocationID);
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    objPrjMemberTimesheet.Add(new TimeSheet(
                       reader["EmpName"].ToString(),
                       reader["Role"].ToString(),
                       Convert.ToInt32(reader["TSHour"].ToString()),
                       Convert.ToInt32(reader["EmpID"].ToString())
                      ));
                }
            }
        }
        return objPrjMemberTimesheet;
    }

    public List<TimeSheet> GetIncomepleteTS(int Month, int Year, int LocationID, int EmpID)
    {
        List<TimeSheet> pr = new List<TimeSheet>();
        SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "INCOMPLETE");
        cmd.Parameters.AddWithValue("@EmpID", EmpID);
        cmd.Parameters.AddWithValue("@Month", Month);
        cmd.Parameters.AddWithValue("@Year", Year);
        cmd.Parameters.AddWithValue("@LocationID", LocationID);

        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pr.Add(new TimeSheet(
                    Convert.ToInt32(reader["EmpID"].ToString()),
                     reader["EMPName"].ToString(),
                      Convert.ToDateTime (reader["Date"].ToString()),
                       reader["TimeAvailable"].ToString(),
                        reader["TimeReported"].ToString()
                    ));
            }
        }
        return pr;
    }

    
     public List<EmpSaturdayHoliday> GetEmpSaturdayHoliday()
    {
        List<EmpSaturdayHoliday> pr = new List<EmpSaturdayHoliday>();
        SqlCommand cmd = new SqlCommand("SP_TimeSheet", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GetAllSaturdays");
        
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EmpSaturdayHoliday emp = new EmpSaturdayHoliday();
                emp.WeekDate =  Convert.ToDateTime(reader["WeekDate"]).ToString("yyyyMMdd");
                emp.WeekDayName = reader["WeekDayName"].ToString();
                emp.WeekNumber = reader["WeekNumber"].ToString();
                pr.Add(emp);
                  
            }
        }
        return pr;
    }

    public Boolean Update(int ModuleID, int EmpID, DateTime TSDate, int TSHour, string TSComment, int TSID)
    {
        Boolean Status = false;
        try
        {
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
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        return Status;
    }


    public static DataSet NoWork(string mode)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_TimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return ds;
        }

    // Added for US-968-Consolidate_timesheet
    public List<TimeSheet> GetConsolidateTimesheet(int projId, int month, int year, string mode) // added by atif for Consolidate Timesheet
    {
        List<TimeSheet> objTS = new List<TimeSheet>();
        try
        {
            SqlCommand cmd = new SqlCommand("USP_ConsolidateTimesheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@ProjID", projId);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);
            
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        objTS.Add(new TimeSheet()
                        {
                            EmpName = Convert.ToString(reader["empname"]),
                            Designation = Convert.ToString(reader["Designation"]),
                            TotalHours = Convert.ToInt32(reader["Hours"]),
                            Module =  Convert.ToString(reader["moduleName"]),
                           // ProjectStartDate =  Convert.ToString(reader["projStartDate"]),
                            //ProjectStausDate =  Convert.ToString(reader["proStatusDate"]),
                           
                        });
                    }
                }
            }

        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        return objTS;
    }
    public TimeSheet GetStatus(int projId)
    {
        TimeSheet ts = new TimeSheet();
        DataTable dt = new DataTable();
        try
        {
            SqlCommand cmd = new SqlCommand("USP_ConsolidateTimesheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "GetStatus");
            cmd.Parameters.AddWithValue("@ProjID", projId);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            if (dt.Rows.Count>0)
            {
                ts.ProjectStartDate = dt.Rows[0]["projStartDate"].ToString();
               // ts.ProjectStausDate= dt.Rows[0]["proStatusDate"].ToString();
                ts.ProjectStaus = dt.Rows[0]["status"].ToString();
            }
        }
        catch (Exception)
        {

            throw;
        }
        return ts;
    }
    //end

    public List<TimeSheet> GetConsolidatedTimesheetByDateRange(int projId, DateTime fromDate, DateTime toDate)
    {
        List<TimeSheet> objTS = new List<TimeSheet>();
        try
        {
            using (SqlCommand cmd = new SqlCommand("USP_ConsolidateTimesheet", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "GetCTByDateRange");
                cmd.Parameters.AddWithValue("@ProjID", projId);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

                SqlDataReader reader = null;

                con.Open();
                using ( reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            objTS.Add(new TimeSheet()
                            {
                                EmpName = Convert.ToString(reader["empname"]),
                                Designation = Convert.ToString(reader["Designation"]),
                                TotalHours = Convert.ToInt32(reader["Hours"]),
                                Module = Convert.ToString(reader["moduleName"]),
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error " + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        return objTS;
    }

}