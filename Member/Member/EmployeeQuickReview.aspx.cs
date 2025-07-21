using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmployeeQuickReview : Authentication
{
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UM = UserMaster.UserMasterInfo();
        }
        catch (Exception ex)
        {
            string strSubject = "Error in Agora_EmpQuickReview_Page_Load_UM";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, strSubject, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
        }
    }
    [WebMethod]
    public static String Get_EmpQuickReviewList(string Mode)
    {
        try
        {
            List<EmpQuickReviewBLL> lstforInit = new List<EmpQuickReviewBLL>();
            lstforInit = EmpQuickReviewBLL.Get_EmpQuickReviewList(Mode);
            var data = (from obj in lstforInit
                        select new
                        {
                            EmployeeCode = obj.EmployeeCode,
                            EmployeeName = obj.EmployeeName,
                            EmpReviewCount = obj.EmpReviewCount,
                            LastReviewDate = obj.LastReviewDate,
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            string strSubject = "Error in Agora_EmpQuickReview_Get_EmpQuickReviewList";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, strSubject, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
            return null;
        }
    }

    [WebMethod]
    public static String Get_ReviewListByEmpCode(int EmployeeCode, string Mode)
    {
        try
        {
            List<EmpQuickReviewBLL> lstforInit = new List<EmpQuickReviewBLL>();
            lstforInit = EmpQuickReviewBLL.Get_ReviewListByEmpCode(EmployeeCode, Mode);
            var data = (from obj in lstforInit
                        select new
                        {
                            ReviewId = obj.ReviewId,
                            InsertedOn = obj.InsertedOn,
                            ReviewText = obj.ReviewText,
                            ReviewCreatedBy=obj.ReviewCreatedBy,
                            AcceptedDateTime = obj.AcceptedDateTime,
                            AcceptedStatus = obj.AcceptedStatus,
                            EmployeeCode = EmployeeCode
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            string strSubject = "Error in Agora_EmpQuickReview_Get_ReviewListByEmpCode";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, strSubject, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
            return null;
        }
    }

    [WebMethod]
    public static String FillEmployeeDropDown(string Mode)
    {
        try
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(EmpQuickReviewBLL.Get_AllEmployees(Mode));
        }
        catch (Exception ex)
        {
            string strSubject = "Error in Agora_EmpQuickReview_FillEmployeeDropDown";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, strSubject, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
            return null;
        }
    }

    protected void Btn_SaveReview_Click(object sender, EventArgs e)
    {
        try
        {
            int CreatedBy = UM.EmployeeID; EmpQuickReviewBLL obj = new EmpQuickReviewBLL();
            int EmpId = (!string.IsNullOrEmpty(hf_EmpId.Value) ? Convert.ToInt32(hf_EmpId.Value) : 0);
            int ReviewId = (!string.IsNullOrEmpty(hdn_ReviewId.Value) ? Convert.ToInt32(hdn_ReviewId.Value) : 0);
            if (ReviewId > 0 && !string.IsNullOrEmpty(hf_Review.Value))
            {
                obj.SaveReviewText("Update_EmpReviews", ReviewId, EmpId, hf_Review.Value, CreatedBy);
            }
            else if (EmpId > 0 && !string.IsNullOrEmpty(hf_Review.Value))
            {
                obj.SaveReviewText("Insert_EmpReviews", 0, EmpId, hf_Review.Value, CreatedBy);
                obj.Send_ReviewMailToEmp(EmpId);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Review has been saved Successfully.')", true);
            clearFields();
        }
        catch (Exception ex)
        {
            string strSubject = "Error in Agora_EmpQuickReview_Btn_SaveReview_Click";
            CSCode.Global.SendMail(ex.Message + " :: " + ex.InnerException + " :: " + ex.StackTrace, strSubject, ConfigurationManager.AppSettings.Get("HREmail"), ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
        }
    }
    public void clearFields()
    {
        hf_Review.Value = "";
        hf_EmpId.Value = "";
        hdn_ReviewId.Value = "";
    }
}