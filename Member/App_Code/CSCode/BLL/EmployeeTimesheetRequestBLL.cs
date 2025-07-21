using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for EmployeeTimesheetRequestBLL
/// </summary>
public class EmployeeTimesheetRequestBLL
{

    public int Id { get; set; }
    public int EmpId { get; set; }
    public DateTime Requestdate { get; set; }
    public string Requestdate1 { get; set; }
    public string Descripation { get; set; }
    public string EmployeeName { get; set; }
    public int InsertedBy { get; set; }
    public string InsertedOn { get; set; }
    public string ApprovedBy { get; set; }

    public EmployeeTimesheetRequestBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static List<EmployeeTimesheetRequestBLL> GetAllEmployeeTimesheetRequestList()
    {
        EmployeeTimesheetRequestDLL objreqdll = new EmployeeTimesheetRequestDLL();
        EmployeeTimesheetRequestBLL objreqbll = new EmployeeTimesheetRequestBLL();
        DataTable dt = new DataTable();
        dt = objreqdll.GetAllEmployeeTimesheetRequest();
        return objreqbll.BindList(dt);
    }
   
    private List<EmployeeTimesheetRequestBLL> BindList(DataTable dt)
    {
        List<EmployeeTimesheetRequestBLL> lstETRBLL = new List<EmployeeTimesheetRequestBLL>();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                EmployeeTimesheetRequestBLL objETRBLL = new EmployeeTimesheetRequestBLL();
                objETRBLL.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                objETRBLL.EmpId = Convert.ToInt32(dt.Rows[i]["EmpId"]);
                objETRBLL.EmployeeName = Convert.ToString(dt.Rows[i]["EmployeeName"]);
                objETRBLL.Requestdate = Convert.ToDateTime(dt.Rows[i]["Requestdate"]);
                objETRBLL.Requestdate1 =Convert.ToString(dt.Rows[i]["Requestdate"]);
                objETRBLL.Descripation = Convert.ToString(dt.Rows[i]["Descripation"]);
                objETRBLL.ApprovedBy = Convert.ToString(dt.Rows[i]["ApprovedBy"]);
                objETRBLL.InsertedOn = Convert.ToString(dt.Rows[i]["insertedOn"]);
                lstETRBLL.Add(objETRBLL);
            }
        }
        return lstETRBLL;
    }

    public static int Save(int EmpId, DateTime RequestDate, string description, int InsertedBy)
    {
        EmployeeTimesheetRequestDLL objETRDAL = new EmployeeTimesheetRequestDLL();
        EmployeeTimesheetRequestBLL objETRBLL = new EmployeeTimesheetRequestBLL();
        objETRBLL.EmpId = EmpId;
        objETRBLL.Requestdate = RequestDate;
        objETRBLL.Descripation = description;
        objETRBLL.InsertedBy = InsertedBy;

        return objETRDAL.Save(objETRBLL);
    }

    public static bool CheckForApproval(int EmpId, DateTime requestdate)
    {
        EmployeeTimesheetRequestDLL objETRDAL = new EmployeeTimesheetRequestDLL();
        EmployeeTimesheetRequestBLL objETRBLL = new EmployeeTimesheetRequestBLL();
        return objETRDAL.CheckforApproval(EmpId, requestdate);

    }


}