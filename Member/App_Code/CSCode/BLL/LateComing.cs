using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LateComing
/// </summary>
public class LateComing
{
    public int ID { get; set; }
    public string empName { get; set; }
    public string EmpNameId { get; set; }
    public int EmpCode { get; set; }
    public DateTime ApplyDate { get; set; }
    public string ExpectedInTime { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public string LateCommingReason { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public int? ApprovedBy { get; set; }
    public string ApprovalComment { get; set; }
    public int? IsApproveStatus { get; set; }
    public string Statusflag { get; set; }
    public string AdminCancelReason { get; set; }


    public LateComing(int v, string v1)
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public LateComing()
    {

    }
    public LateComing(int _ID, string _EmpNameId, int _EmpCode, DateTime _ApplyDate, string _ExpectedInTime, DateTime _CreatedOn, int _CreatedBy,
                              string _LateCommingReason, DateTime? _ApprovedOn, int? _ApprovedBy,
                              string _ApprovalComment, int _IsApproveStatus, string _Statusflag)
    {
        this.ID = _ID;
        this.EmpNameId = _EmpNameId;
        this.EmpCode = _EmpCode;
        this.ApplyDate = _ApplyDate;
        this.ExpectedInTime = _ExpectedInTime;
        this.CreatedOn = _CreatedOn;
        this.CreatedBy = _CreatedBy;
        this.LateCommingReason = _LateCommingReason;
        this.ApprovedOn = _ApprovedOn;
        this.ApprovedBy = _ApprovedBy;
        this.ApprovalComment = _ApprovalComment;
        this.IsApproveStatus = _IsApproveStatus;
        this.Statusflag = _Statusflag;

    }


    public LateComing(int _EmpCode, int _ID, string Comment)//(string empName, int projId, string projName, int empid, string HolidayDate)
    {
        // TODO: Complete member initialization
       
        this.EmpCode = _EmpCode;
        this.ID = _ID;
        this.ApprovalComment = Comment;
        //this.ApplyDate = ApplyDate;
            }

    public LateComing(int _ID, string _EmpName, DateTime _ApplyDate, int _EmpCode, string _ExpectedInTime,
                             string _LateCommingReason,
                             string _ApprovalComment)
    {
        this.ID = _ID;
        this.empName = _EmpName;

        this.ApplyDate = _ApplyDate;
        this.EmpCode = _EmpCode;
        this.ExpectedInTime = _ExpectedInTime;

        this.LateCommingReason = _LateCommingReason;

        this.ApprovalComment = _ApprovalComment;



    }

    
    public static DataTable InsertLateComing(int EmpCode, DateTime ApplyDate, string ExpectedInTime, string LateCommingReason, DateTime? ApprovedOn, int? ApprovedBy, string ApprovalComment, int IsApproveStatus)
    {
        LateComing curLateComing = new LateComing();
        curLateComing.ID = 0;
        curLateComing.EmpCode = EmpCode;
        curLateComing.ApplyDate = ApplyDate;
        curLateComing.ExpectedInTime = ExpectedInTime;
        curLateComing.CreatedOn = DateTime.Now;
        curLateComing.CreatedBy = EmpCode;
        curLateComing.LateCommingReason = LateCommingReason;
        curLateComing.ApprovedOn = null;
        curLateComing.ApprovedBy = null;
        curLateComing.ApprovalComment = null;
        curLateComing.IsApproveStatus = 0;
        LateComingDAL objLateComingDAL = new LateComingDAL();
        return objLateComingDAL.InsertLateComing(curLateComing);
    }

    public static List<LateComing> GetLateComersData(string mode)
    {
        LateComingDAL objDAL = new LateComingDAL();
        return BindLateComersData(objDAL.GetLateComersData(mode));
    }

    public static void LateComingEmp(string mode, int Empid, string Comment, int ID, DateTime AppyDate)
    {
        LateComingDAL objHolidayW = new LateComingDAL();
        objHolidayW.LateComingReject(mode, Empid, Comment, ID, AppyDate);
    }

    public static List<LateComing> GetLateComingSearchData(int EmpCode, int Status, string StartDate, string EndDate, int LocationID)
    {
        LateComingDAL objLateComing = new LateComingDAL();
        return objLateComing.GetLateComeData(EmpCode, Status, StartDate, EndDate, LocationID);
    }
    private static List<LateComing> BindLateComersData(DataTable dt)
    {
        List<LateComing> lst = new List<LateComing>();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LateComing obj = new LateComing();
                obj.ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                obj.EmpCode = Convert.ToInt32(dt.Rows[i]["EmpCode"].ToString());
                obj.EmpNameId = dt.Rows[i]["empName"].ToString()+ '('+obj.EmpCode+')';
                obj.ApplyDate = Convert.ToDateTime(dt.Rows[i]["ApplyDate"].ToString());
                obj.ApprovalComment = dt.Rows[i]["ApprovalComment"].ToString();
                if (obj.ApprovedOn != null)
                {
                    obj.ApprovedOn = Convert.ToDateTime(dt.Rows[i]["ApprovedOn"].ToString());
                }
                if (dt.Rows[i]["ExpectedInTime"].ToString() != null)
                {
                    obj.ExpectedInTime = dt.Rows[i]["ExpectedInTime"].ToString();
                }
                

                obj.CreatedOn = Convert.ToDateTime(dt.Rows[i]["CreatedOn"].ToString());
                obj.CreatedBy = Convert.ToInt32(dt.Rows[i]["CreatedBy"].ToString());
                if (dt.Rows[i]["LateCommingReason"].ToString() != null)
                {
                    obj.LateCommingReason = dt.Rows[i]["LateCommingReason"].ToString();
                }

                if (dt.Rows[i]["ApprovedBy"].ToString() != "0" )
                {
                    obj.ApprovedBy = Convert.ToInt32(dt.Rows[i]["ApprovedBy"].ToString());
                }
                
                
                obj.IsApproveStatus = Convert.ToInt32(dt.Rows[i]["IsApproveStatus"].ToString());
                obj.Statusflag = dt.Rows[i]["Statusflag"].ToString();
                lst.Add(obj);
            }
        }
        return lst;
    }


    public static void LateComingCancle(string mode,   int ID, int EmpCode, string ApprovalComment)
    {
        LateComingDAL objHolidayW = new LateComingDAL();
        objHolidayW.LateComingCancle(mode, ID , EmpCode, ApprovalComment);
    }

    public static void LateComingApprove(string mode, int EmpCode, string Comment, int ID,int approveStatus, int ApprovedBy)
    {
        LateComingDAL objHolidayW = new LateComingDAL();
        objHolidayW.ApproveLateComing(mode, EmpCode, Comment, ID,approveStatus, ApprovedBy);
    }


}