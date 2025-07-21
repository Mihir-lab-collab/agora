using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_EmpSelfAppraisal : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();      
        if (!IsPostBack)
        {}
    }

    // Get self appraise project wise data..
    [System.Web.Services.WebMethod]
    public static String FillAllProject()
    {
        List<AppraisalReportBLL> lstforInit = new List<AppraisalReportBLL>();
        lstforInit = AppraisalReportBLL.GetAllProjects(Convert.ToInt32(HttpContext.Current.Session["empid"]));

        var data = (from CurGetProjDetailsByProjId in lstforInit
                    select new AppraisalReportBLL
                    {
                        Id = CurGetProjDetailsByProjId.Id,
                        empid = CurGetProjDetailsByProjId.empid,
                        //AppraiseBy = CurGetProjDetailsByProjId.AppraiseBy,
                        projId = CurGetProjDetailsByProjId.projId,
                        ProjName = CurGetProjDetailsByProjId.ProjName,
                        Quarter = CurGetProjDetailsByProjId.Quarter,
                        Status = CurGetProjDetailsByProjId.Status,
                        EmpSelfAppraisaDate = CurGetProjDetailsByProjId.EmpSelfAppraisaDate,
                        //EmpFinalAppraiseDate = CurGetProjDetailsByProjId.EmpFinalAppraiseDate,
                        SelfAppQuarterDate = CurGetProjDetailsByProjId.SelfAppQuarterDate,
                        ReportingManager = CurGetProjDetailsByProjId.ReportingManager
                    }).ToList();
        ;

        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(data);

    }

    // Get popup self appraise data for given ratings
    [System.Web.Services.WebMethod]
    public static String GetSelfApprData(string SubMode, int EmpId, int projId, DateTime QuarterStartDate)
    {
        try
        {
            List<AppraisalReportBLL> lstforSelfAppr = new List<AppraisalReportBLL>();
            lstforSelfAppr = AppraisalReportBLL.GetSelfAppraisal(SubMode, EmpId, projId, QuarterStartDate);

            var data = (from CurGetSelfApprDetails in lstforSelfAppr
                        select new
                        {
                            CurGetSelfApprDetails.KRANames,
                            CurGetSelfApprDetails.KRAID,
                            CurGetSelfApprDetails.AppraisalId,
                            CurGetSelfApprDetails.Comments,
                            CurGetSelfApprDetails.empid,
                            CurGetSelfApprDetails.TransQuarterStartDate,
                            CurGetSelfApprDetails.EmpAprId,
                            CurGetSelfApprDetails.Id
                        }).ToList();
            ;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception ex)
        {

            return null;
        }
    }

    // Provide self appraisal ratings
    [System.Web.Services.WebMethod]
    public static void SaveTDS(List<AppraisalReportBLL> GridData, int Project)
    {
        AppraisalReportBLL objEmpappr = new AppraisalReportBLL(0, "");

        try
        {
            objEmpappr.Saveempappr(GridData, Project);
            FillAllProject();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //Get the Employee self appraisal report
    [System.Web.Services.WebMethod]
    public static String GetEmpSelfAppraisalReport(int empid, int projId, int AppraiseBy, DateTime CurrentQuarterDate)
    {
        try
        {
            if (projId > 0)
            {
                List<AppraisalReportBLL> lstforInit = new List<AppraisalReportBLL>();
                lstforInit = AppraisalReportBLL.GetEmpSelfAppraisalReport(empid, projId,AppraiseBy, CurrentQuarterDate);

                var data = (from CurGetProjDetailsByProjId in lstforInit
                            select new AppraisalReportBLL
                            {
                                Id = CurGetProjDetailsByProjId.Id,
                                //AuthorityName = CurGetProjDetailsByProjId.AuthorityName,
                                empName = CurGetProjDetailsByProjId.empName,
                                projId = CurGetProjDetailsByProjId.projId,
                                ProjName = CurGetProjDetailsByProjId.ProjName,
                                KRANames = CurGetProjDetailsByProjId.KRANames,
                                EmployeeRatings = CurGetProjDetailsByProjId.EmployeeRatings,
                                //ManagerRatings = CurGetProjDetailsByProjId.ManagerRatings,
                                Quarter = CurGetProjDetailsByProjId.Quarter,
                                //Comments = CurGetProjDetailsByProjId.Comments
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
