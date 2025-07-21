using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmployeeLWD_DLL
/// </summary>
public class EmployeeLWD_DLL
{
    public int empid { get; set; }
    public string empName { get; set; }
    public DateTime? empExpectedLWD { get; set; }
    public string daysPending { get; set; }
    public EmployeeLWD_DLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static List<EmployeeLWD_DLL> GetEmployeeLWDDetails(string mode)//, int ProjId
    {
        EmployeeLWD_DAL objEmployee = new EmployeeLWD_DAL();
        return objEmployee.GetEmployeeLWDDetails(mode);
        //EmployeeMasterDAL objEmployee = new EmployeeMasterDAL();
        //return objEmployee.GetEmployeeLWDDetails(mode);//, ProjId
    }
}