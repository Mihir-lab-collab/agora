using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for EmployeeAttendance
/// </summary>
public class Attendance
{
    public int totaldays { get; set; }
    public int months { get; set; }
    public int years { get; set; }
    public int LocationID { get; set; }

    public int empId { get; set; }
    public DateTime attDate { get; set; }
    public string attStatus { get; set; }
    public DateTime attInTime { get; set; }
    public DateTime attOutTime { get; set; }
    public string attComment { get; set; }
    public string attIP { get; set; }
    public int adminID { get; set; }
    public string mode { get; set; }
    
    public static DataSet GetRecord(int _days, int _months, int _years, int _locationID, int _empId, DateTime _startDate, DateTime _endDate,string filter, string _mode)
    {
        AttendanceDAL objattendanceDAL = new AttendanceDAL();
        return objattendanceDAL.GetRecordDs(_days, _months, _years, _locationID, _empId, _startDate, _endDate, filter, _mode);
    }

    public static double GetValue(int _days, int _months, int _years, int _locationID, int _empId, DateTime _startDate, DateTime _endDate, string filter, string _mode)
    {
        AttendanceDAL objattendanceDAL = new AttendanceDAL();
        return objattendanceDAL.GetValue(_days, _months, _years, _locationID, _empId, _startDate, _endDate, filter, _mode);
    }

    public static int getLocationFkbyEmpId(int _empId,string _mode)
    {
        AttendanceDAL objattendanceDAL = new AttendanceDAL();
        return objattendanceDAL.getlocationFkEmpId(_empId, _mode);
    }
    public static DataTable getEmpStatus()
    {
        AttendanceDAL objemployeeAttendanceDAL = new AttendanceDAL();
        return objemployeeAttendanceDAL.getEmpStatus();
    }

    public static void AddUpdateEmpAtt(Attendance objdata)
    {
        AttendanceDAL objemployeeAttendanceDAL = new AttendanceDAL();
        objemployeeAttendanceDAL.AddUpdateEmpAtt(objdata);
    }
    public int CheckExistsEmp(string mode,int Empid,string attdate)
    {
        AttendanceDAL objemployeeAttendanceDAL = new AttendanceDAL();
       return objemployeeAttendanceDAL.CheckExistsEmp(mode,Empid,attdate);
    }
	public Attendance()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}