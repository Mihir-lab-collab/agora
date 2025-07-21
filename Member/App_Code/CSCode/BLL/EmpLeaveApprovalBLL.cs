using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;

/// <summary>
/// Summary description for EmpLeaveApprovalBLL
/// </summary>
public class EmpLeaveApprovalBLL
{
    public string EmpLeaveID { get; set; }
    public string EmpID { get; set; }
    public int LocationID { get; set; }
    public string EmpName { get; set; }
    public string EmpNameID { get; set; }
    public string LeaveType { get; set; }
    public string LeaveFrom { get; set; }
    public string LeaveTo { get; set; }
    public string LeaveAppliedOn { get; set; }
    public string LeaveReason { get; set; }
    public string AdminComment { get; set; }
    public string LeaveStatus { get; set; }
    public string LeaveSanctionOn { get; set; }
    public string LeaveSanctionBy { get; set; }
    public string BalanceLeave { get; set; }    
    public string IsApproved { get; set; }
    public int IsTeam { get; set; }
    
    public int TotalLeave { get; set; }

	public EmpLeaveApprovalBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static List<EmpLeaveApprovalBLL> GetEmpLeaves(string mode, string status, string name, string from, string to,int locationID,int loginID,int includeArchive)
    {
        EmpLeaveApprovalBLL objBLL = new EmpLeaveApprovalBLL();
        EmpLeaveApprovalDAL objDAL = new EmpLeaveApprovalDAL();
        objBLL.LeaveStatus = status;
        objBLL.EmpName = name;
        objBLL.LeaveFrom = from;
        objBLL.LeaveTo = to;
        objBLL.LocationID = locationID;
        return BindLeaves(objDAL.GetEmpLeaves(mode, objBLL, loginID, includeArchive));
    }

    private static List<EmpLeaveApprovalBLL> BindLeaves(DataTable dt)
    {
        List<EmpLeaveApprovalBLL> lst = new List<EmpLeaveApprovalBLL>();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                EmpLeaveApprovalBLL obj = new EmpLeaveApprovalBLL();
                obj.EmpLeaveID = dt.Rows[i]["EmpLeaveID"].ToString();
                obj.EmpID = dt.Rows[i]["EmpID"].ToString();
                obj.EmpName = dt.Rows[i]["EmpName"].ToString();
                obj.EmpNameID = dt.Rows[i]["EmpName"].ToString() + "(" + dt.Rows[i]["EmpID"].ToString() + ")";
                obj.LeaveType = dt.Rows[i]["LeaveType"].ToString();
                if (!string.IsNullOrEmpty(obj.LeaveType) && string.Compare(obj.LeaveType,"WL",true)==0)
                {
                    obj.LeaveType = "LOP";
                }
                obj.LeaveFrom = dt.Rows[i]["LeaveFrom"].ToString();
                obj.LeaveTo = dt.Rows[i]["LeaveTo"].ToString();
                obj.LeaveAppliedOn = dt.Rows[i]["LeaveEntryDate"].ToString();
                obj.LeaveReason = dt.Rows[i]["leavedesc"].ToString();
                obj.AdminComment = dt.Rows[i]["leaveComment"].ToString();
                obj.LeaveStatus = dt.Rows[i]["leavestatus"].ToString();
                obj.LeaveSanctionOn = dt.Rows[i]["LeaveSenctionedDate"].ToString();
                obj.LeaveSanctionBy = dt.Rows[i]["LeaveSanctionBy"].ToString();
                obj.BalanceLeave = dt.Rows[i]["BalanceLeave"].ToString();
                obj.IsApproved = dt.Rows[i]["isApproved"].ToString();
                obj.TotalLeave = Convert.ToInt32(dt.Rows[i]["Totleave"].ToString());
                obj.IsTeam = Convert.ToInt32(dt.Rows[i]["isTeam"].ToString());

                lst.Add(obj);
            }
        }
        return lst;
    }

    public DataTable GetLeaveType(string mode)
    {
        DataTable dt = new DataTable();
        EmpLeaveApprovalDAL objDAL = new EmpLeaveApprovalDAL();
        dt = objDAL.GtLeaveType(mode);
        return dt;
    }

    public string GetProfile(string mode,string empID)
    {
       
        EmpLeaveApprovalDAL objDAL = new EmpLeaveApprovalDAL();

        return objDAL.GetProfile(mode, empID); 
    }

    public static string UpdateEmpLeaveStatus(string mode, string empID, string empLeaveID, string leaveStatus, string LeaveType, string AdminComment, string SanctionedBy, string fDate, string tDate)
    {
        string ip ="0.0.0.0";
        EmpLeaveApprovalBLL objBLL = new EmpLeaveApprovalBLL();
        EmpLeaveApprovalDAL objDAL = new EmpLeaveApprovalDAL();
        objBLL.EmpID = empID;
        objBLL.EmpLeaveID = empLeaveID;
        objBLL.LeaveStatus = leaveStatus;
        objBLL.LeaveType = LeaveType;
        objBLL.AdminComment = AdminComment;
        objBLL.LeaveSanctionBy = SanctionedBy;
        objBLL.LeaveFrom = fDate;
        objBLL.LeaveTo = tDate;

        return objDAL.UpdateEmpLeaveStatus(mode, ip, objBLL);

    }
}