using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmployeeAppraisalInitiation : Authentication
{
    protected void Page_Load(object sender, EventArgs e)
    {}

    public class Profiles
    {
        public int ProfileID;
        public string Name;
    }

    //Geting appraisal records as per quarter basis
    [System.Web.Services.WebMethod]
    public static String GetAppraisalData(int empId, string Mode, int Counter)
    {
        try
        {
            if (empId > 0)
            {

                List<AppraisalReportBLL> lstforInit = new List<AppraisalReportBLL>();
                lstforInit = AppraisalReportBLL.GetAppraisalData(empId, Mode, Counter);
                JavaScriptSerializer jss = new JavaScriptSerializer();

                var data = (from CurGetProjDetailsByProjId in lstforInit
                            select new AppraisalReportBLL
                            {
                                EmployeeAppraisalId = CurGetProjDetailsByProjId.EmployeeAppraisalId,
                                Id = CurGetProjDetailsByProjId.Id,
                                StatusId = CurGetProjDetailsByProjId.StatusId,
                                empid = CurGetProjDetailsByProjId.empid,
                                ProjName = CurGetProjDetailsByProjId.ProjName,
                                empName = CurGetProjDetailsByProjId.empName,
                                Status = CurGetProjDetailsByProjId.Status,
                                Quarter = CurGetProjDetailsByProjId.Quarter,
                                projId = CurGetProjDetailsByProjId.projId,
                                ModifiedOn = CurGetProjDetailsByProjId.ModifiedOn,
                                EmpSelfAppraisaDate = CurGetProjDetailsByProjId.EmpSelfAppraisaDate,
                                EmpFinalAppraiseDate = CurGetProjDetailsByProjId.EmpFinalAppraiseDate,
                                QuarterStartDate = CurGetProjDetailsByProjId.QuarterStartDate,
                                ProjectMemberId = CurGetProjDetailsByProjId.ProjectMemberId,
                                ReportingManager=CurGetProjDetailsByProjId.ReportingManager
                            }).ToList();
                ;
                return jss.Serialize(data);

            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //Bind MultiselectRoles Dropdown List On Initiate time
    [System.Web.Services.WebMethod]
    public static String GetQuarter(int Counter)
    {
        try
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(AppraisalReportBLL.GetQuarter(Counter));
        }
        catch (Exception ex)
        {            
            throw ex;
        }       
    }

    //Bind MultiselectRoles Dropdown List On Initiate time
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static String FillRolesDropDown(string ProfileID)
    {
        try
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Profiles objPrj;
            DataTable dtPrj = new ProfileModuleDAL().getProfile("");
            List<Profiles> drlist = new List<Profiles>();
            foreach (DataRow row in dtPrj.Rows)
            {
                objPrj = new Profiles();
                objPrj.ProfileID = Convert.ToInt32(row["ProfileID"]);
                objPrj.Name = Convert.ToString(row["Name"]);
                drlist.Add(objPrj);
            }

            return jss.Serialize(drlist);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //Authority Set KRA And Click Event
    protected void btnAddKRA_Click(object sender, EventArgs e)
    {
        try
        {
            string ProfileId = "";
            string Mode = "";
            ProfileId = hftxtAppraisalAuthority.Value.Replace("'", "''");
            int AppraisalID = Convert.ToInt32(hndAppraisalID.Value);
            int ProjectId = Convert.ToInt32(hdnProjectId.Value);
            string DeleteRole = (hndEditRole.Value).ToString();
            if (string.IsNullOrEmpty(DeleteRole))
            {
                Mode = "INSERTTRANS";                
            }
            else
            {
                Mode = "DELETETRANS";                
            }
            int EmpId = Convert.ToInt32(hdnEmpId.Value);
            int InitiatedBy = Convert.ToInt32(Session["empId"]);
            DateTime SelfAppraiseDate = Convert.ToDateTime(hndQuarterDate.Value);
            int ReturnValue = AppraisalReportBLL.InsertAllTransaction(Mode, AppraisalID, ProjectId, EmpId, InitiatedBy, ProfileId, "", SelfAppraiseDate);
            if (ReturnValue == 0)
            {
                Page.RegisterStartupScript("", "<script type='text/javascript'>alert('KRA not set against this profile');</script>");
                return;
            } 
        }
        catch (Exception ex)
        {
            throw ex;
        }
    } 


    //protected void KRA(string Mode, int AppraisalID, int ProjectId, int EmpId, int InitiatedBy, string ProfileId, DateTime SelfAppraiseDate)
    //{
    //    if (ProfileId != "" && ProfileId != null)
    //    {
           
    //        AppraisalReportBLL.InsertAllTransaction(Mode, AppraisalID, ProjectId, EmpId, InitiatedBy, ProfileId, "", SelfAppraiseDate);
    //    }
    //}

    //Get Popup for given Manager ratings
    [System.Web.Services.WebMethod]
    public static String GetManagerAprData(int projId)
    {
        try
        {
            if (projId > 0)
            {
                List<AppraisalReportBLL> lstforMgrAppr = new List<AppraisalReportBLL>();

                lstforMgrAppr = AppraisalReportBLL.GetManagerApprsial(projId);
                var data = (from CurGetManagerAprDetails in lstforMgrAppr
                            select new
                            {
                                CurGetManagerAprDetails.AppraisalId,
                                CurGetManagerAprDetails.KRANames,
                                CurGetManagerAprDetails.KRAID,
                                CurGetManagerAprDetails.Value,
                                CurGetManagerAprDetails.Id,
                                CurGetManagerAprDetails.ManagerValue,
                                CurGetManagerAprDetails.AppraisalTransactionId,
                                CurGetManagerAprDetails.ProjName
                            }).ToList();
                ;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                return jss.Serialize(data);
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {

            return null;
        }
    }

    //Save final appraisal data with manager ratings with comments
    [System.Web.Services.WebMethod]
    public static void SaveTDS(List<AppraisalReportBLL> GridData, string Comments)
    {
        int AppraiseById = Convert.ToInt32(HttpContext.Current.Session["empid"]);
        AppraisalReportBLL objEmpappr = new AppraisalReportBLL(0, "", 0, "", 0, 0);
        try
        {
            if (AppraiseById != null && AppraiseById != 0)
            {
                objEmpappr.SaveEmpMgrApr(GridData, Comments, AppraiseById);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // Get employee appraisal report on row click
    [System.Web.Services.WebMethod]
    public static String GetEmpAppraisalReport(int empid, int projId, DateTime CurrentQuarterDate)
    {
        try
        {
            if (projId > 0 && empid > 0)
            {
                List<AppraisalReportBLL> lstforInit = new List<AppraisalReportBLL>();
                lstforInit = AppraisalReportBLL.GetEmpAppraisalReport(empid, projId, CurrentQuarterDate);

                var data = (from CurGetProjDetailsByProjId in lstforInit
                            select new AppraisalReportBLL
                            {
                                Id = CurGetProjDetailsByProjId.Id,
                                AuthorityName = CurGetProjDetailsByProjId.AuthorityName,
                                empName = CurGetProjDetailsByProjId.empName,
                                projId = CurGetProjDetailsByProjId.projId,
                                ProjName = CurGetProjDetailsByProjId.ProjName,
                                KRANames = CurGetProjDetailsByProjId.KRANames,
                                EmployeeRatings = CurGetProjDetailsByProjId.EmployeeRatings,
                                ManagerRatings = CurGetProjDetailsByProjId.ManagerRatings,
                                Quarter = CurGetProjDetailsByProjId.Quarter,
                                Comments = CurGetProjDetailsByProjId.Comments
                            }).ToList();
                ;

                JavaScriptSerializer jss = new JavaScriptSerializer();
                return jss.Serialize(data);
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

}