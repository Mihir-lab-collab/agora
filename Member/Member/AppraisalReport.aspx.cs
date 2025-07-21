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

public partial class Member_AppraisalReport : Authentication
{
    protected void Page_Load(object sender, EventArgs e)
    { }

    [WebMethod]
    public static String GetAppraisalReport(string Mode, int Counter)
    {
        try
        {
            List<AppraisalReportBLL> lstforInit = new List<AppraisalReportBLL>();
            lstforInit = AppraisalReportBLL.GetFinalAppraisalReport(Mode, Counter);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var data = (from CurGetProjDetailsByProjId in lstforInit orderby CurGetProjDetailsByProjId.empid
                        select new AppraisalReportBLL
                        {
                            empid = CurGetProjDetailsByProjId.empid,
                            //projId = CurGetProjDetailsByProjId.projId,
                            //Id = CurGetProjDetailsByProjId.Id,
                            empName = CurGetProjDetailsByProjId.empName,
                            ProjectCounts = CurGetProjDetailsByProjId.ProjectCounts,
                            QuarterStartDate = CurGetProjDetailsByProjId.QuarterStartDate,
                        }).ToList();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static String GetEmployeeAppraisalProject(int EmpId, int Counter)
    {
        try
        {
            List<AppraisalReportBLL> lstforInit = new List<AppraisalReportBLL>();
            lstforInit = AppraisalReportBLL.GetEmpolyeeAppraisalProject(EmpId, Counter);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var data = (from CurGetProjDetailsByProjId in lstforInit
                        select new AppraisalReportBLL
                        {
                            projId = CurGetProjDetailsByProjId.projId,
                            empid = CurGetProjDetailsByProjId.empid,
                            //QuarterStartDate = CurGetProjDetailsByProjId.QuarterStartDate,
                        }).ToList();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static String GetManagerComments(int EmpId, int ProjectId)
    {
        try
        {
            List<AppraisalReportBLL> lstforInit = new List<AppraisalReportBLL>();
            lstforInit = AppraisalReportBLL.GetManagerComments(EmpId, ProjectId);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var data = (from CurGetProjDetailsByProjId in lstforInit
                        select new AppraisalReportBLL
                        {
                            AuthorityName = CurGetProjDetailsByProjId.AuthorityName,
                            projId = CurGetProjDetailsByProjId.projId,
                            Comments = CurGetProjDetailsByProjId.Comments,
                        }).ToList();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static String GetAppraisalManagerRatingsReport(int EmpId,int ProjectId, int Counter)
    {
        try
        {
            if (EmpId > 0)
            {
                List<AppraisalReportBLL> lstforInit = new List<AppraisalReportBLL>();
                DataTable dt = AppraisalReportBLL.GetAppraisalManagerRatingsReport(EmpId, ProjectId, Counter);
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        //if (col.ColumnName.Contains("_"))
                        //{
                        //    row.Add(col.ColumnName, 0 );
                        //}
                        //else
                        //{
                            row.Add(col.ColumnName, dr[col]);
                        //}
                    }
                    rows.Add(row);
                }
                return serializer.Serialize(rows);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}