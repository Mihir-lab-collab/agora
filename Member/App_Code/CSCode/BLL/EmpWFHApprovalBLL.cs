using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmpWFHApprovalBLL
/// </summary>
public class EmpWFHApprovalBLL
{
    public EmpWFHApprovalBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string EmpWFHID { get; set; }
    public string EmpID { get; set; }
    public string LoginEmpId { get; set; }
    public int LocationID { get; set; }
    public string EmpName { get; set; }
    public string EmpNameID { get; set; }
    public string WFHType { get; set; }
    public string WFHFrom { get; set; }
    public string WFHTo { get; set; }
    public string WFHAppliedOn { get; set; }
    public string WFHReason { get; set; }
    public string AdminComment { get; set; }
    public string WFHStatus { get; set; }
    public string WFHSanctionOn { get; set; }
    public string WFHSanctionBy { get; set; }
    public string IsApproved { get; set; }
    public int IsTeam { get; set; }
    public int TotalWFH { get; set; }
    public string WFHEntryDate { get; set; }
    public string AttOutTime { get; set; }
    public string AttInTime { get; set; }
    public string EmployeeWFHCount { get; set; }

    public static string UpdateEmpWFHStatus(string mode, string empID, string empWFHID, string wFHStatus, string adminComment, string sanctionedBy, string fDate, string tDate,string LoginEmpId)
    {
        string ip = "0.0.0.0";
        EmpWFHApprovalBLL objBLL = new EmpWFHApprovalBLL();
        EmpWFHApprovalDAL objDAL = new EmpWFHApprovalDAL();
        objBLL.EmpID = empID;
        objBLL.EmpWFHID = empWFHID;
        objBLL.WFHStatus = wFHStatus;
        //objBLL.WFHType = wFHType;
        objBLL.AdminComment = adminComment;
        objBLL.WFHSanctionBy = sanctionedBy;
        objBLL.WFHFrom = fDate;
        objBLL.WFHTo = tDate;
        objBLL.LoginEmpId = LoginEmpId;

        return objDAL.UpdateEmpWFHStatus(mode, ip, objBLL);
    }
    public static List<EmpWFHApprovalBLL> GetEmpWFH(string mode, string status, string name, string from, string to, int locationID, int loginID, int includeArchive)
    {
        EmpWFHApprovalBLL objBLL = new EmpWFHApprovalBLL();
        EmpWFHApprovalDAL objDAL = new EmpWFHApprovalDAL();
        objBLL.WFHStatus = status;
        objBLL.EmpName = name;

        if (!string.IsNullOrEmpty(from))
        {
            string[] DateFrom = from.Split('/');
            string New_FromDt = DateFrom[1] + '/' + DateFrom[0] + '/' + DateFrom[2];
            objBLL.WFHFrom = New_FromDt;
        }
        if (!string.IsNullOrEmpty(to))
        {
            string[] DateTo = to.Split('/');
            string New_ToDt = DateTo[1] + '/' + DateTo[0] + '/' + DateTo[2];
            objBLL.WFHTo = New_ToDt;
        }
     
        objBLL.LocationID = locationID;
        return BindWFH(objDAL.GetEmpWFH(mode, objBLL, loginID, includeArchive));
    }
    private static List<EmpWFHApprovalBLL> BindWFH(DataTable dt)
    {
        List<EmpWFHApprovalBLL> lst = new List<EmpWFHApprovalBLL>();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                EmpWFHApprovalBLL obj = new EmpWFHApprovalBLL();
                obj.EmpWFHID = dt.Rows[i]["EmpWFHID"].ToString();
                obj.EmpID = dt.Rows[i]["EmpID"].ToString();
                obj.EmpName = dt.Rows[i]["EmpName"].ToString();
                obj.EmpNameID = dt.Rows[i]["EmpName"].ToString() + "(" + dt.Rows[i]["EmpID"].ToString() + ")";
                obj.WFHFrom = dt.Rows[i]["WFHFrom"].ToString();
                obj.WFHTo = dt.Rows[i]["WFHTo"].ToString();
                obj.WFHAppliedOn = dt.Rows[i]["WFHEntryDate"].ToString();
                obj.WFHReason = dt.Rows[i]["WFHDescription"].ToString();
                obj.AdminComment = dt.Rows[i]["WFHComment"].ToString();
                obj.WFHStatus = dt.Rows[i]["WFHstatus"].ToString();
                obj.WFHSanctionOn = dt.Rows[i]["WFHSenctionedDate"].ToString();
                obj.WFHSanctionBy = dt.Rows[i]["WFHSenctionBy"].ToString();
                obj.IsApproved = dt.Rows[i]["isApproved"].ToString();
                obj.WFHEntryDate = dt.Rows[i]["WFHEntryDate"].ToString();
                obj.IsTeam = Convert.ToInt32(dt.Rows[i]["isTeam"].ToString());
                obj.AttInTime = dt.Rows[i]["AttInTime"].ToString();
                obj.AttOutTime = dt.Rows[i]["AttOutTime"].ToString();
                obj.EmployeeWFHCount = dt.Rows[i]["EmployeeWFHCount"].ToString();

                lst.Add(obj);
            }
        }
        return lst;
    }
    public string GetProfile(string mode, string empID)
    {

        EmpWFHApprovalDAL objDAL = new EmpWFHApprovalDAL();

        return objDAL.GetProfile(mode, empID);
    }
    public List<EmpWFHApprovalBLL> EmployeeList(string mode)
    {
        EmpWFHApprovalDAL objDAL = new EmpWFHApprovalDAL();
        EmpWFHApprovalBLL objBLL = new EmpWFHApprovalBLL();
        List<EmpWFHApprovalBLL> EmpList = objDAL.EmployeeList(mode);
        return EmpList;

    }
    public int BulkApplyWFH(string[] empList, string mode,
        string WFHFrom, string WFHTo, string WFHDescription, string WFHSanctionedDate, string WFHSanctionBy)
    {
        int[] employeeList = Array.ConvertAll(empList, int.Parse);
        EmpWFHApprovalDAL objDAL = new EmpWFHApprovalDAL();
        EmpWFHApprovalBLL objBLL = new EmpWFHApprovalBLL();
        return objDAL.BulkApplyWFH(employeeList, mode, WFHFrom, WFHTo, WFHDescription, WFHSanctionedDate, WFHSanctionBy);

    }
    public DataTable SendMail(string mode, int empId)
    {
        EmpWFHApprovalDAL objDAL = new EmpWFHApprovalDAL();
        DataTable dt = new DataTable();
        dt = objDAL.SendMail(mode, empId);
        return dt;
    }
    public void SendMail(DataTable dt,string status,string adminComment)
    {
        string strBody, strSubject, mailTo, mailFrom, message, CC = "";

        if (dt.Rows.Count>0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // this if conditions applay to avoid duplicate emailId under CC in mail
                if (CC.Contains(dt.Rows[i]["ProjMangerEmail"].ToString()))
                {

                }
                else
                {
                    CC = CC + dt.Rows[i]["ProjMangerEmail"].ToString();
                    CC += (i < dt.Rows.Count) ? ";" : string.Empty;
                }
                if (CC.Contains(dt.Rows[i]["BAEmail"].ToString()))
                {
                }
                else
                {
                    CC = CC + dt.Rows[i]["BAEmail"].ToString();
                    CC += (i < dt.Rows.Count) ? ";" : string.Empty;
                }

            }
            if (CC.Contains(dt.Rows[0]["empEmail"].ToString()))
            {
            }
            else
            {
                CC += dt.Rows[0]["empEmail"].ToString();
            }
            string finalCC = string.Empty;
            if (CC.Length>0)
            {
                var distinctCC = CC.Split(',').Select(email => email.Trim()).Distinct();
                finalCC = string.Join(", ", distinctCC);
            }
            strSubject = "Work From Home Status";
            strBody = dt.Rows[0]["empName"].ToString() + " Work From Home request has been "+status+".<br/> Reason : "+ adminComment;
            mailFrom = ConfigurationManager.AppSettings["HREmail"];
            mailTo = dt.Rows[0]["empEmail"].ToString();
            message = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, finalCC, ""); 
        }
    }
}