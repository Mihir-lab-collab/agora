using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for HolidayWorkingDAL
/// </summary>
public class HolidayWorkingDAL
{
    public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public HolidayWorkingDAL()
    {
    }

    public List<HolidayWorking> GetHolidayWorkingDetails(int? ID, string mode, int? EmpId, DateTime? HolidayDate, int? ProjId, int? Hours, int? UserReason)
    {
        List<HolidayWorking> lstHolidayWorking = new List<HolidayWorking>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@Empid", EmpId);
        cmd.Parameters.AddWithValue("@ProjId", ProjId);
        cmd.Parameters.AddWithValue("@Hours", Hours);
        cmd.Parameters.AddWithValue("@UserReason", UserReason);
        cmd.Parameters.AddWithValue("@ID", ID);
        cmd.Parameters.AddWithValue("@HolidayDate", HolidayDate);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstHolidayWorking.Add(new HolidayWorking(
                     Convert.ToInt32(reader["Id"]),
                     Convert.ToInt32(reader["Empid"]),
                     reader["holidayDate"].ToString(),
                     Convert.ToInt32(reader["ProjId"]),
                     Convert.ToInt32(reader["ExpectedHours"]),
                     reader["UserReason"].ToString(),
                     reader["ProjectName"].ToString(),
                     reader["UserEntryDate"].ToString(),
                     Convert.ToInt32(reader["Status"]),
                     reader["AdminComment"].ToString(),
                     reader["AdminCanReason"].ToString(),
                     reader["Statusflag"].ToString()

                    ));
            }
        }
        return lstHolidayWorking;
    }


    public List<HolidayWorking> GetHolidayDate()
    {
        List<HolidayWorking> lstHolidayWorking = new List<HolidayWorking>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("GET_HolidayList", con);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HolidayWorking objHW = new HolidayWorking();
                objHW.HolidayDate = (reader["Holiday"]).ToString();
                lstHolidayWorking.Add(objHW);
            }
        }
        return lstHolidayWorking;


    }

    public string SaveHolidayWorking(string mode, HolidayWorking objHW)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@ID", objHW.Id);
        cmd.Parameters.AddWithValue("@Empid", objHW.EmpId);
        cmd.Parameters.AddWithValue("@ProjId", objHW.ProjId);
        cmd.Parameters.AddWithValue("@Hours", objHW.ExpectedHours);
        cmd.Parameters.AddWithValue("@UserReason", objHW.UserReason);
        cmd.Parameters.AddWithValue("@HolidayDate", DateTime.ParseExact(objHW.HolidayDate, "dd/MM/yyyy",null));// Convert.ToDateTime(HDate.ToString()));
        try
        {
            using (con)
            {
                Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "";
    }

    public void CancelHolidayWorking(string mode, HolidayWorking objHW)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@Empid", objHW.EmpId);
                cmd.Parameters.AddWithValue("@HolidayDate", Convert.ToDateTime(objHW.HolidayDate));
                cmd.Parameters.AddWithValue("@ProjId", objHW.ProjId);
                Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public List<HolidayWorking> GetPmDetailsByProjid(int? ProjId)
    {
        List<HolidayWorking> lstHolidayWorking = new List<HolidayWorking>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("Sp_GetpmDetailsByProjId", con);     
        cmd.Parameters.AddWithValue("@ProjId", ProjId);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstHolidayWorking.Add(new HolidayWorking(
                     Convert.ToInt32(reader["Empid"]),
                       reader["empEmail"].ToString(), 
                     reader["ProjName"].ToString()                                    
                    ));
            }
        }
        return lstHolidayWorking;
    }

    public List<HolidayWorking> GetCompOffDetails(int? HolidayLeaveId)
    {
        List<HolidayWorking> lstHolidayWorking = new List<HolidayWorking>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
        cmd.Parameters.AddWithValue("@mode", "GetCompOffDetails");
        cmd.Parameters.AddWithValue("@HolidayLeaveID", HolidayLeaveId);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstHolidayWorking.Add(new HolidayWorking(
                     reader["empName"].ToString(),
                       Convert.ToInt32(reader["projId"].ToString()),
                     reader["projName"].ToString(),
                       Convert.ToInt32(reader["empid"].ToString()),
                     reader["HolidayDate"].ToString()
                    ));
            }
        }
        return lstHolidayWorking;
    }

    public List<HolidayWorking> GetHolidayWorkingData(int Empid, int Status, string HolidayStartDate, string HolidayEndDate, int LocationID)
    {
        Nullable<DateTime> dt = null;

        string strstartdate = string.Empty;
        string strenddate = string.Empty;
       
        if( HolidayStartDate!="")
        { 
           DateTime temp = DateTime.ParseExact(HolidayStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
           strstartdate = temp.ToString("yyyy/MM/dd");
        }

        if (HolidayEndDate != "")
        {
            DateTime temp1 = DateTime.ParseExact(HolidayEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            strenddate = temp1.ToString("yyyy/MM/dd");
        }

        List<HolidayWorking> lstHolidayWorking = new List<HolidayWorking>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
        cmd.Parameters.AddWithValue("@mode", "GetHolidayWorkingData");
        cmd.Parameters.AddWithValue("@Empid", Empid);
        cmd.Parameters.AddWithValue("@Status", Status);
        
        if( HolidayStartDate!="" && HolidayEndDate != "")
        { 
         cmd.Parameters.AddWithValue("@HolidayStartDate", strstartdate);
         cmd.Parameters.AddWithValue("@HolidayEndDate", HolidayEndDate == "" ? HolidayEndDate : strenddate);
        }

        cmd.Parameters.AddWithValue("@locationId", LocationID);
         cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstHolidayWorking.Add(new HolidayWorking(
                     reader["empName"].ToString(),
                       Convert.ToInt32(reader["ID"].ToString()),
                     reader["HolidayDate"].ToString(),
                       Convert.ToInt32(reader["Empid"].ToString()),
                     Convert.ToInt32(reader["ProjId"]),
                     Convert.ToInt32(reader["ExpectedHours"]),
                     reader["UserReason"].ToString(),
                      reader["AdminReason"].ToString(),
                        reader["AdminCancelReason"].ToString(),
                       reader["projName"].ToString()
                   
                    ));
            }
        }
        return lstHolidayWorking;
    }

    public List<HolidayWorking> GetCompOffReportData(int empID, int LocationID)
    {
        List<HolidayWorking> lstCompOffReport = new List<HolidayWorking>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
        cmd.Parameters.AddWithValue("@mode", "GetCompOffReportData");
        cmd.Parameters.AddWithValue("@Empid", empID);
       // cmd.Parameters.AddWithValue("@locationId", LocationID);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HolidayWorking obj=new HolidayWorking();
                obj.Id = Convert.ToInt32(reader["coID"]);
                obj.EmpId = Convert.ToInt32(reader["empId"].ToString());
                obj.EmpName = Convert.ToString(reader["Name"]);
                obj.HolidayDate = Convert.ToString(reader["coDate"].ToString());
                obj.AdminComment = Convert.ToString(reader["coComment"]);
                obj.UserEntryDate = Convert.ToString(reader["entryDate"]);
                obj.EntryBy = Convert.ToInt32(reader["entryBy"]);
                obj.EntryByName =  Convert.ToString(reader["EntryByName"]);
                lstCompOffReport.Add(obj);
            }
        }
        return lstCompOffReport;
    }

    public void AddToCompOff(int Empid, int HolidayLeaveID,int Status)
    {
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "AddToCompOff");
            cmd.Parameters.AddWithValue("@Empid", Empid);
            cmd.Parameters.AddWithValue("@HolidayLeaveID", HolidayLeaveID);
            cmd.Parameters.AddWithValue("@Status", Status);
            Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }
    }

    public void CreateCompOff(int Empid, string CompOffDate, string Comment, int EntryBy)
    {
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "CreateCompoff");
            cmd.Parameters.AddWithValue("@Empid", Empid);
            cmd.Parameters.AddWithValue("@CompOffDate", CompOffDate);
            cmd.Parameters.AddWithValue("@Comment", Comment);
            cmd.Parameters.AddWithValue("@EntryBy", EntryBy);
            Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }
    }
    
    public void HolidayLeave(string mode, int Empid, string Comment, int HolidayLeaveID, string CompOffDate)
    {
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode",mode);
            cmd.Parameters.AddWithValue("@Empid", Empid);
            cmd.Parameters.AddWithValue("@Comment", Comment);
            cmd.Parameters.AddWithValue("@HolidayLeaveID", HolidayLeaveID);
            cmd.Parameters.AddWithValue("@CompOffDate", CompOffDate); 
            Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }
    }


     public String checkCompOffExists(int Empid,string CompOffDate)
    {
        string sReturn = "";
        SqlConnection con = new SqlConnection(_strConnection);
        // con.Open();
        SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", "checkCompOffExists");
        cmd.Parameters.AddWithValue("@Empid", Empid);
        cmd.Parameters.AddWithValue("@CompOffDate", CompOffDate);
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

     public void CreateCompof(int Empid, string CompOffDate, int Comment, int EntryBy)
    {
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "CreateCompof");
            cmd.Parameters.AddWithValue("@Empid", Empid);
            cmd.Parameters.AddWithValue("@CompOffDate", CompOffDate);
            cmd.Parameters.AddWithValue("@Comment", Comment);
            cmd.Parameters.AddWithValue("@EntryBy", EntryBy);
            Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }
    }
   
    public int GetLocationAcess(int ProfileId)
    {
        int LocationID = 0;
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "GetProfile");
            cmd.Parameters.AddWithValue("@profileId", ProfileId);
            
            LocationID=Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }
        return LocationID;
    }

    public string GetLocationName(int LocationId)
    {
        string LocationName = string.Empty;
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_HolidayWorking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "GetProfile");
            cmd.Parameters.AddWithValue("@locationId", LocationId);

            LocationName = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
        }
        return LocationName;
    }
}