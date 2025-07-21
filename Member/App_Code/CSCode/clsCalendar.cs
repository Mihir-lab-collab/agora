using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Globalization;
/// <summary>
/// Summary description for clsCalendar
/// </summary>
public class clsCalendar
{
  
    public bool blnVisible = false;
    public bool Visible
    {
        get { return blnVisible; }
        set { blnVisible = value; }
    }

    public bool blnSaveVisible = false;
    public bool SaveVisible
    {
        get { return blnSaveVisible; }
        set { blnSaveVisible = value; }
    }

    public bool blnHoliday = false;
    public bool btnHoliday
    {
        get { return blnHoliday; }
        set { blnHoliday = value; }
    }

    public string strDay = String.Empty;
    public string Day
    {
        get { return strDay; }
        set { strDay = value; }
    }
    public string strColor = "Gray";
    public string Color
    {
        get { return strColor; }
        set { strColor = value; }
    }

    public string strInHour = string.Empty;
    public string InHour
    {
        get { return strInHour; }
        set { strInHour = value; }
    }

    public string strInMinute = string.Empty;
    public string InMinute
    {
        get { return strInMinute; }
        set { strInMinute = value; }
    }

    public string strOutHour = string.Empty;
    public string OutHour
    {
        get { return strOutHour; }
        set { strOutHour = value; }
    }

    public string strOutMinute = string.Empty;
    public string OutMinute
    {
        get { return strOutMinute; }
        set { strOutMinute = value; }
    }
    public string strbrkHour = string.Empty;
    public string brkHour
    {
        get { return strbrkHour; }
        set { strbrkHour = value; }
    }
    public string strbrkMinute = string.Empty;
    public string brkMinute
    {
        get { return strbrkMinute; }
        set { strbrkMinute = value; }
    }

    public string strTotWorkHour = string.Empty;
    public string TotWorkHour
    {
        get { return strTotWorkHour; }
        set { strTotWorkHour = value; }
    }
    public string strStatus = string.Empty;
    public string Status
    {
        get { return strStatus; }
        set { strStatus = value; }
    }

    public string DispHoliday = string.Empty;
    public string HolidayLabel
    {
        get { return DispHoliday; }
        set { DispHoliday = value; }
    }

    public string DispWorkingHours = String.Empty;
    public string WorkingHours
    {
        get { return DispWorkingHours; }
        set { DispWorkingHours = value; }
    }

    public string Checklength(String strdate)
    {
        if (strdate.Length < 2)
            strdate = "0" + strdate;
        return strdate;
    }

    public List<clsCalendar> GetDays(DateTime startdate, int intUserID, int intUserLocation)
    {
        int intDay = Convert.ToInt32(startdate.DayOfWeek);
        int intMonth = startdate.Month;
        int intYear = Convert.ToInt16(startdate.ToString("yyyy"));
        int intDaysInMonth = System.DateTime.DaysInMonth(intYear, intMonth);
        string enddate = startdate.Year.ToString() + "/" + startdate.Month.ToString() + "/" + intDaysInMonth.ToString();
        DateTime NextMonthDate = startdate.Date.AddMonths(1);
        DateTime PreviousMonthDate = startdate.Date.AddMonths(-1);

        List<Holiday> dtHoliday = Holiday.GetHolidayCalendarDetail("CalendarSelect", Convert.ToDateTime(startdate), Convert.ToDateTime(enddate));
        List<EmployeeMaster> dtJoiningLeaving = EmployeeMaster.GetEmployeeJoinLeaveDate("GetEmployeeJoinLeaveDate", intUserID).ToList();
        List<EmpAttLog> dtAttendence = EmpAttLog.GetCalendarAtt("GetCalendarAtt", intUserID, startdate.ToString(), enddate).ToList();
        List<LocationBLL> dtLocation = LocationBLL.BindLocation("GetLocationByID", intUserLocation);

        List<clsCalendar> lstCalendar = new List<clsCalendar>();
        int CountSat = 0;
        for (int index = 1; index <= 42; index++)
        {
            clsCalendar cDay = new clsCalendar();
            if (intDay <= index - 1 && intDaysInMonth >= (index - intDay))
            {
                cDay.Visible = true;
                cDay.Day = (index - intDay).ToString();
                cDay.Color = System.Drawing.Color.White.Name;
               
                DateTime Sunday = Convert.ToDateTime(startdate.Year.ToString() + "/" + startdate.Month.ToString() + "/" + cDay.Day);
                if (Sunday.DayOfWeek == 0)
                {
                    cDay.btnHoliday = false;
                    cDay.Color = System.Drawing.Color.LightGray.Name;
                    cDay.SaveVisible = false;
                }

                if (Convert.ToInt32(Sunday.DayOfWeek) == 6)
                {
                    //Added code to show all saturday off from 1 Apr 2022
                    DateTime dtImpactAllSaturdayOff = new DateTime(2022, 04, 01);
                    if (startdate >= dtImpactAllSaturdayOff)
                    {
                        cDay.btnHoliday = false;
                        cDay.Color = System.Drawing.Color.LightGray.Name;
                        cDay.SaveVisible = false;
                    }
                    else
                    {
                        CountSat = CountSat + 1;
                        if (CountSat == 2 || CountSat == 4)
                        {
                            cDay.btnHoliday = false;
                            cDay.Color = System.Drawing.Color.LightGray.Name;
                            cDay.SaveVisible = false;
                        }
                    }
                }
                if (DateTime.Now.Month == startdate.Month && DateTime.Now.Year == startdate.Year && DateTime.Now.Day == Convert.ToInt16(cDay.Day) && dtLocation.FirstOrDefault().Biometric == false)
                    cDay.SaveVisible = true;
                else
                    cDay.SaveVisible = false;

                for (int indexHoliday = 0; indexHoliday <= dtHoliday.Count() - 1; indexHoliday++)
                {
                    DateTime HolidayDate = DateTime.Parse(dtHoliday[indexHoliday].HolidayDate.ToString());
                    if (HolidayDate.Day == Convert.ToInt16(cDay.Day))
                    {
                        cDay.btnHoliday = false;
                        cDay.Color = System.Drawing.Color.LightGray.Name;
                        cDay.SaveVisible = false;
                        cDay.HolidayLabel = dtHoliday[indexHoliday].Narration.ToString();
                    }
                }

                for (int indexAttendance = 0; indexAttendance <= dtAttendence.Count() - 1; indexAttendance++)
                {
                    DateTime attDate = DateTime.Parse(dtAttendence[indexAttendance].attDate.ToString());
                    if (attDate.Day == Convert.ToInt16(cDay.Day))
                    {
                            if (dtAttendence[indexAttendance].attInTime.ToString() != "")
                            {
                                DateTime attInTime = DateTime.Parse(dtAttendence[indexAttendance].attInTime.ToString());
                                cDay.InHour = Checklength(attInTime.Hour.ToString());
                                cDay.InMinute = Checklength(attInTime.Minute.ToString());

                                DateTime attOutTime = DateTime.Parse(dtAttendence[indexAttendance].attOutTime.ToString());
                                cDay.OutHour = Checklength(attOutTime.Hour.ToString());
                                cDay.OutMinute = Checklength(attOutTime.Minute.ToString());

                                TimeSpan InOutDt = attOutTime - attInTime;

                                                                   if (dtAttendence[indexAttendance].attInTime.ToString() != "" && dtAttendence[indexAttendance].attOutTime.ToString() != "")
                                    {
                                        string FinWrkingHr = String.Format("{0:00}.{1:00} Hrs", InOutDt.Hours, InOutDt.Minutes);

                                        cDay.TotWorkHour = FinWrkingHr.ToString();
                                    }
                                    else
                                    {
                                        cDay.TotWorkHour = "00.00 Hrs";

                                    }



                                if (!string.IsNullOrEmpty(Convert.ToString(dtAttendence[indexAttendance].brktimehours)))
                                {
                                    string x = dtAttendence[indexAttendance].brktimehours;
                                    //string y = x.Substring(0, 10);
                                    // DateTime punch= DateTime.ParseExact(y.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                    // DateTime punchTime = DateTime.Parse(dtAttendence[indexAttendance].brktimehours.ToString());
                                    string punchTime = dtAttendence[indexAttendance].brktimehours;

                                    string time = punchTime.Substring(10, 5);
                                    int h = Convert.ToInt32(time.Split(':')[0]);
                                    //string j = Convert.ToString(h);
                                    //if (j.Length == 1) 
                                    //{
                                    //    j = '0' + j;
                                    //}

                                    int m = Convert.ToInt32(time.Split(':')[1]);
                                    //string k = Convert.ToString(m);
                                    //if (k.Length == 1)
                                    //{
                                    //    k = k + '0';
                                    //}
                                    // string hr = punchTime.Substring(11,1);

                                    // string min = punchTime.Substring(13, 2);
                                    cDay.brkHour = h.ToString();// Checklength(punchTime.Hour.ToString());
                                    cDay.brkMinute = m.ToString();//Checklength(punchTime.Minute.ToString());
                                    string Breaktime = cDay.brkHour + "." + cDay.brkMinute + " " + "Hrs";

                                    string FinWrkingHr = String.Format("{0:00}:{1:00}", InOutDt.Hours, InOutDt.Minutes);
                                    string BrkTime = String.Format("{0:00}:{1:00}", cDay.brkHour, cDay.brkMinute);

                                    TimeSpan totalTime = TimeSpan.Parse(FinWrkingHr);
                                    TimeSpan brkTime = TimeSpan.Parse(BrkTime);
                                    TimeSpan total = totalTime.Subtract(brkTime);
                                    string TotalHoursDisplay = string.Format("{0:00}.{1:00} Hrs", total.Hours, total.Minutes);
                                    cDay.TotWorkHour = TotalHoursDisplay;
                                    Breaktime = (cDay.brkHour.Length==1?"0"+ cDay.brkHour: cDay.brkHour) + "." + (cDay.brkMinute.Length==1? cDay.brkMinute + "0": cDay.brkMinute) + " " + "Hrs";
                                    cDay.brkHour = Breaktime.ToString();
                                }
                                else
                                {
                                    cDay.brkHour = "00.00 Hrs";
                                    // cDay.brkMinute = "00";
                                }
                            }
                        
                        if (dtAttendence[indexAttendance].attStatus.ToUpper() == "P")
                        {
                            if(string.Compare(dtAttendence[indexAttendance].Comment,"WFH",true) == 0)
                            {
                                cDay.Status = "WFH";
                                cDay.Color = System.Drawing.Color.LightCoral.Name;
                            }
                            else if (string.Compare(dtAttendence[indexAttendance].Comment, "RA", true) == 0)
                            {
                                cDay.Status = "RA";
                                cDay.Color = System.Drawing.Color.LightCoral.Name; 
                            }
                            else if(Convert.ToSingle(cDay.InHour + "." + cDay.InMinute) > 10.15)
                            {
                                cDay.Status = "HD";
                                cDay.Color = System.Drawing.Color.LightCoral.Name;
                            }
                            else
                            {
                                cDay.Status = dtAttendence[indexAttendance].attStatus.ToUpper();
                            }

                            if (Convert.ToBoolean(dtLocation.FirstOrDefault().Biometric == false))
                            {
                                cDay.btnHoliday = false;
                            }
                        }
                        else if (dtAttendence[indexAttendance].attStatus.ToUpper() == "A")
                        {
                            cDay.btnHoliday = false;
                            cDay.Color = System.Drawing.Color.LightCoral.Name;
                            cDay.SaveVisible = false;
                            cDay.Status = dtAttendence[indexAttendance].attStatus.ToString();
                        }
                        else if (dtAttendence[indexAttendance].attStatus.ToUpper() == " ")
                        {
                            cDay.btnHoliday = false;
                            cDay.SaveVisible = false;
                            cDay.Status = dtAttendence[indexAttendance].attStatus.ToString().ToUpper();
                        }
                        else
                        {
                            cDay.btnHoliday = false;
                            cDay.Color = System.Drawing.Color.LightGray.Name;
                            cDay.SaveVisible = false;
                            cDay.Status = dtAttendence[indexAttendance].attStatus.ToString().ToUpper();
                            if (!string.IsNullOrEmpty(cDay.Status) && string.Compare(cDay.Status,"WL",true)==0)
                            {
                                cDay.Status = "LOP";
                            }
                            cDay.InHour = "00";
                            cDay.InMinute = "00";
                            cDay.OutHour = "00";
                            cDay.OutMinute = "00";
                           
                        }
                        
                    }
                }
                    lstCalendar.Add(cDay);
                
            }
            else
            {
                cDay.Visible = false;
                cDay.Day = index.ToString();
                cDay.Color = System.Drawing.Color.Silver.Name;
                cDay.btnHoliday = true;
                lstCalendar.Add(cDay);
            }
        }
        return lstCalendar;
    }
   
}