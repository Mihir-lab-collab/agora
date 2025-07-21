using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Holiday
/// </summary>
public class Holiday
{
    public int HolidayId { get; set; }
    public int LocationId { get; set; }
    public string Location { get; set; }
    public string HolidayDate { get; set; }
    public string Narration { get; set; }
    public string mode { get; set; }
    public string HolidayDay { get; set; }

    public Holiday()
	{
	}
    public Holiday(int LocationId, string Location)
    {
        this.LocationId = LocationId;
        this.Location = Location;
    }
    public Holiday(int HolidayId, int LocationId, string Location, string HolidayDate, string Narration,string HolidayDay)
    {
        this.HolidayId = HolidayId;
        this.LocationId = LocationId;
        this.Location = Location;
        this.HolidayDate = HolidayDate;
        this.Narration = Narration;
        this.HolidayDay = HolidayDay;
    }
    public Holiday(string HolidayDate, string holiday_descr)
    {
        this.HolidayDate = HolidayDate;
        this.Narration = holiday_descr;
    }
    public static List<Holiday> GetHolidayDetails(string mode, int LocationId)
    {
        HolidayDAL objHoliday = new HolidayDAL();
        return objHoliday.GetHolidayDetails(mode,LocationId);
    }
    public static List<Holiday> GetHolidayCalendarDetail(string mode, DateTime startDate, DateTime endDate)
    {
        HolidayDAL objHoliday = new HolidayDAL();
        return objHoliday.GetHolidayCalendarDetail(mode, startDate, endDate);
    }

    /// <summary>
    /// This method is used to get the holiday by attanance date
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="holidayDate"></param>
    /// <returns>Returns date</returns>
    public static List<Holiday> GetHolidayBydate(string mode, DateTime holidayDate)
    {
        HolidayDAL objHoliday = new HolidayDAL();
        return objHoliday.GetHolidayBydate(mode, holidayDate);
    }
    public static int SaveHoliday(string mode, int HolidayId, int LocationId, string HolidayDate, string Narration)
    {
        Holiday objHoliday = new Holiday();
        objHoliday.HolidayId = HolidayId;
        objHoliday.LocationId = LocationId;
        objHoliday.HolidayDate = HolidayDate;
        objHoliday.Narration = Narration;
        HolidayDAL objholidayDAL = new HolidayDAL();
        return objholidayDAL.SaveHoliday(mode, objHoliday);
    }
    public static void DeleteHoliday(string mode, int HolidayId)
    {
        Holiday curholiday = new Holiday();
        curholiday.HolidayId = HolidayId;
        HolidayDAL objDelete = new HolidayDAL();
        objDelete.DeleteHoliday(mode, HolidayId);
    }
}
public class Notice
{
    public string NoticeDate { get; set; }
    public string notice_descr { get; set; }

    public Notice(string NoticeDate, string notice_descr)
    {
        this.NoticeDate = NoticeDate;
        this.notice_descr = notice_descr;
    }

    public static List<Notice> SelectNotice(string mode)
    {
        NoticeDAL objHoliday = new NoticeDAL();
        return objHoliday.SelectNotice(mode);
    }
}