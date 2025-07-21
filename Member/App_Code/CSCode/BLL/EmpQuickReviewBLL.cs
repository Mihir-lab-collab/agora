using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;

/// <summary>
/// Summary description for AppraisalReportBLL
/// </summary>
/// 

public class EmpQuickReviewBLL
{
    public int ReviewId { get; set; }
    public int EmployeeCode { get; set; }
    public string EmployeeName { get; set; }
    public int EmpReviewCount { get; set; }
    public string ReviewCreatedBy { get; set; }
    public DateTime LastReviewDate { get; set; }
    public string ReviewText { get; set; }
    public int InsertedBy { get; set; }
    public DateTime InsertedOn { get; set; }
    public string InsertedIP { get; set; }
    public int ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string ModifiedIP { get; set; }
    public string AcceptedIP { get; set; }
    public string AcceptedStatus { get; set; }
    public DateTime AcceptedOn { get; set; }
    public string AcceptedDateTime { get; set; }

    public EmpQuickReviewBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public EmpQuickReviewBLL(int _EmployeeCode, string _EmployeeName, int _EmpReviewCount, DateTime _LastReviewDate)
    {
        this.EmployeeCode = _EmployeeCode;
        this.EmployeeName = _EmployeeName;
        this.EmpReviewCount = _EmpReviewCount;
        this.LastReviewDate = _LastReviewDate;
    }

    public EmpQuickReviewBLL(int _ReviewId, DateTime _InsertedOn, string _ReviewText,string _ReviewCreatedBy, string _AcceptedStatus, string _AcceptedDateTime)
    {
        this.ReviewId = _ReviewId;
        this.InsertedOn = _InsertedOn;
        this.ReviewText = _ReviewText;
        this.ReviewCreatedBy = _ReviewCreatedBy;
        this.AcceptedDateTime = _AcceptedDateTime;
        this.AcceptedStatus = _AcceptedStatus;
    }

    public EmpQuickReviewBLL(int _EmployeeCode, string _EmployeeName)
    {
        this.EmployeeCode = _EmployeeCode;
        this.EmployeeName = _EmployeeName;
    }
    public static List<EmpQuickReviewBLL> Get_EmpQuickReviewList(string Mode)
    {
        EmpQuickReviewDAL objempApr = new EmpQuickReviewDAL();
        List<EmpQuickReviewBLL> emp = objempApr.Get_EmpQuickReviewList(Mode);
        return emp;
    }
    public static List<EmpQuickReviewBLL> Get_ReviewListByEmpCode(int EmployeeCode, string Mode)
    {
        EmpQuickReviewDAL objempApr = new EmpQuickReviewDAL();
        List<EmpQuickReviewBLL> emp = objempApr.Get_ReviewListByEmpCode(EmployeeCode, Mode);
        return emp;
    }

    public void SaveReviewText(string mode, int ReviewId, int EmpID, string ReviewText, int CreatedBy)
    {
        EmpQuickReviewBLL objReview = new EmpQuickReviewBLL();
        objReview.ReviewId = ReviewId;
        objReview.EmployeeCode = EmpID;
        objReview.ReviewText = ReviewText;
        objReview.InsertedBy = CreatedBy;
        EmpQuickReviewDAL objDAL = new EmpQuickReviewDAL();
        objDAL.SaveReviewText(mode, objReview);
    }
    public void Update_AcceptedReviews(string mode, int EmpID)
    {
        EmpQuickReviewDAL objDAL = new EmpQuickReviewDAL();
        objDAL.Update_AcceptedReviews(mode, EmpID);
    }

    public UserMaster Send_ReviewMailToEmp(int empId)
    {
        string resetPasswoedLink = string.Empty;
        UserMaster objUserMaster = new UserMaster();
        string Message = string.Empty;
        bool status = false;
        try
        {
            string strNewPassword = string.Empty;
            if (empId > 0)
            {
                UserMasterDAL UMD_obj = new UserMasterDAL();
                objUserMaster = UMD_obj.VerifyUserByEmpid(Convert.ToInt32(empId));
                if (objUserMaster != null && string.IsNullOrEmpty(objUserMaster.Message))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(objUserMaster.EmployeeID)) && objUserMaster.EmployeeID > 1 && !string.IsNullOrEmpty(objUserMaster.EmailID))
                    {
                        objUserMaster = UMD_obj.VerifyUserByEmpid(Convert.ToInt32(empId));
                        List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 46);  //14 on Local, 46 on staging
                        string EmailBody = lstConfig[0].value.ToString();
                        EmailBody = EmailBody.Replace("<%UserFName%>", objUserMaster.Name);
                        string strBody, strSubject, mailTo, mailFrom, result, CC = string.Empty;
                        strBody = EmailBody;
                        strSubject = "Agora: Please check Review by Agora Employee Login.";
                        mailTo = objUserMaster.EmailID;
                        mailFrom = "";
                        CC = ConfigurationManager.AppSettings.Get("HREmail");
                        result = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
                        objUserMaster.Message = "Review has been mailed.";

                    }
                }
                else
                {
                    Message = objUserMaster.Message;
                    status = objUserMaster.status;

                }
            }
            else
            {
                Message = "Invalid Employee id.";
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return objUserMaster;
    }
    public UserMaster Send_ReviewAcceptanceMailToHR(int empId)
    {
        string resetPasswoedLink = string.Empty;
        UserMaster objUserMaster = new UserMaster();
        string Message = string.Empty;
        bool status = false;
        try
        {
            string strNewPassword = string.Empty;
            if (empId > 0)
            {
                UserMasterDAL UMD_obj = new UserMasterDAL();
                objUserMaster = UMD_obj.VerifyUserByEmpid(Convert.ToInt32(empId));
                if (objUserMaster != null && string.IsNullOrEmpty(objUserMaster.Message))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(objUserMaster.EmployeeID)) && objUserMaster.EmployeeID > 1 && !string.IsNullOrEmpty(objUserMaster.EmailID))
                    {
                        objUserMaster = UMD_obj.VerifyUserByEmpid(Convert.ToInt32(empId));
                        List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 46); //14 on Local, 46 on staging
                        string EmailBody = lstConfig[0].value1.ToString();
                        EmailBody = EmailBody.Replace("<%UserFName%>", objUserMaster.Name);
                        string strBody, strSubject, mailTo, mailFrom, result, CC = string.Empty;
                        strBody = EmailBody;
                        strSubject = "Agora: Review has been accepted.";
                        mailTo = ConfigurationManager.AppSettings.Get("HREmail");
                        mailFrom = "";
                        CC = objUserMaster.EmailID;
                        result = CSCode.Global.SendMail(strBody, strSubject, mailTo, mailFrom, true, CC, "");
                        objUserMaster.Message = "Review has been accepted.";
                    }
                }
                else
                {
                    Message = objUserMaster.Message;
                    status = objUserMaster.status;

                }
            }
            else
            {
                Message = "Invalid Employee id.";
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return objUserMaster;
    }
    public static List<EmployeeMaster> Get_AllEmployees(string mode)
    {
        EmpQuickReviewDAL obj = new EmpQuickReviewDAL();
        return obj.GetEmployees(mode);
    }
}
