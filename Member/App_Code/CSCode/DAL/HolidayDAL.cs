using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for HolidayDAL
/// </summary>
public class HolidayDAL
{
    public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public HolidayDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public List<Holiday> GetHolidayDetails(string mode,int LocationId)
    {
        List<Holiday> lstHoliday = new List<Holiday>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Holiday", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@LocationID", LocationId);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstHoliday.Add(new Holiday(
                     Convert.ToInt32(reader["holidayId"]),
                     Convert.ToInt32(reader["LocationId"]),
                     reader["Location"].ToString(),
                    reader["holidayDate"].ToString(),
                     reader["Narration"].ToString(),          
                     Convert.ToDateTime(reader["holidayDate"]).ToString("yyyyMMdd")
                    ));
            }
        }
        return lstHoliday;
    }
    
    
    
    public List<Holiday> GetHolidayCalendarDetail(string mode, DateTime startDate, DateTime endDate)
    {
        List<Holiday> lstHoliday = new List<Holiday>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Holiday", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@startdate", startDate);
        cmd.Parameters.AddWithValue("@enddate", endDate);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstHoliday.Add(new Holiday(
                    reader["holidayDate"].ToString(),
                     reader["holidayDesc"].ToString()
                    ));
            }
        }
        return lstHoliday;
    }
    
    
    
    public int SaveHoliday(string mode, Holiday objHoliday)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Holiday", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@holidayId", objHoliday.HolidayId);
        cmd.Parameters.AddWithValue("@LocationID", objHoliday.LocationId);
        cmd.Parameters.AddWithValue("@holidayDate", DateTime.ParseExact(objHoliday.HolidayDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        cmd.Parameters.AddWithValue("@holidayDesc", objHoliday.Narration);
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

    public void DeleteHoliday(string mode, int HolidayId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Holiday", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@holidayId", HolidayId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {
        }
    }

/// <summary>
/// This method is used to get the holiday details
/// </summary>
/// <param name="mode"></param>
/// <param name="holidayDate"></param>
/// <returns></returns>
    public List<Holiday> GetHolidayBydate(string mode, DateTime holidayDate)
    {
        List<Holiday> lstHoliday = new List<Holiday>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Holiday", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@holidayDate", holidayDate);       
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstHoliday.Add(new Holiday(
                    reader["holidayDate"].ToString(),
                     reader["holidayDesc"].ToString()
                    ));
            }
        }
        return lstHoliday;
    }
  
}
public class NoticeDAL : HolidayDAL
{
    public List<Notice> SelectNotice(string mode)
    {
        List<Notice> lstNotice = new List<Notice>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Holiday", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstNotice.Add(new Notice(
                     reader["NoticeDate"].ToString(),
                       reader["notice_descr"].ToString()
                    ));
            }
        }
        return lstNotice;
    }
}