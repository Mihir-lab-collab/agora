using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for AppraisalReportDAL
/// </summary>
public class AppraisalReportDAL
{
	public AppraisalReportDAL()
	{}

    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    //Getting appraisal Links
    public DataSet CheckEmpAppraisal(int empid, string Employee)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        DataSet dt = new DataSet();
        using (SqlCommand cmd = new SqlCommand("SP_Appraisal", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MODE", "APRLINKS");
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.Parameters.AddWithValue("@SubMode", Employee);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }
        return dt;
    }

    //Getting appraisal records as per quarter basis
    public List<AppraisalReportBLL> GetAppraisalData(int empId, string Mode, int Counter)
    {
        List<AppraisalReportBLL> curproject = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@EmpID", empId);
        cmd.Parameters.AddWithValue("@MODE", Mode);
        cmd.Parameters.AddWithValue("@Counter", Counter);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curproject.Add(new AppraisalReportBLL(
                    Convert.ToInt32(reader["EmployeeAppraisalId"]),
                    Convert.ToInt32(reader["empid"]),
                    Convert.ToInt32(reader["Id"]),
                    Convert.ToInt32(reader["StatusId"]),
                    reader["ProjName"].ToString(),
                    reader["empName"].ToString(),
                    reader["Status"].ToString(),
                    reader["Quarter"].ToString(),
                    Convert.ToInt32(reader["projId"]),
                    reader["ModifiedOn"].ToString(),
                    reader["EmpSelfAppraisaDate"].ToString(),
                    reader["EmpFinalAppraiseDate"].ToString(),
                    Convert.ToDateTime(reader["QuarterStartDate"]),
                    Convert.ToInt32(reader["ProjectMemberID"]),
                    reader["ReportingManager"].ToString()
                    ));
            }
        }
        return curproject;
    }
    public List<AppraisalReportBLL> GetReportingManager(string Mode, int projID)
    {
        List<AppraisalReportBLL> ReportingManager = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@MODE", Mode);
        cmd.Parameters.AddWithValue("@ProjectId", projID);
       
       
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AppraisalReportBLL emp = new AppraisalReportBLL();
                emp.empName = Convert.ToString(reader["ReportigManager"]);
                ReportingManager.Add(emp);
            }
        }
        return ReportingManager;
    }

    //Bind MultiselectRoles Dropdown List
    public List<AppraisalReportBLL> GetQuarter(int Counter)
    {
        List<AppraisalReportBLL> CurrQrtr = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@MODE", "GETQUARTERS");
        cmd.Parameters.AddWithValue("@Counter", Counter);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                AppraisalReportBLL objQrtr = new AppraisalReportBLL();
                objQrtr.Quarter = Convert.ToString(reader["Quarter"]);
                CurrQrtr.Add(objQrtr);
            }
        }
        return CurrQrtr;
    }

    //Insert selected roles of the project for employee
    public int InsertAllTransaction(string Mode, int AppraisalID, int ProjectId, int EmpId, int InitiatedBy, string ProfileId, string KRAId, DateTime QuarterDate)
     {
         DataTable dt = new DataTable();
         int ReturnValue = 0;
        List<AppraisalReportBLL> curproject = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        try
        {
            if (EmpId > 0 || EmpId != null)
            {
                using (con)
                {
                    con.Open();
                    int i, j;
                    SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MODE", "KRA");
                    cmd.Parameters.AddWithValue("@SubMode", Mode);
                    cmd.Parameters.AddWithValue("@AppraisalId", AppraisalID);
                    cmd.Parameters.AddWithValue("@ProjectId", ProjectId);
                    cmd.Parameters.AddWithValue("@EmpID", EmpId);
                    cmd.Parameters.AddWithValue("@InitiatedBy", InitiatedBy);
                    cmd.Parameters.AddWithValue("@ProfileId", ProfileId);
                    cmd.Parameters.AddWithValue("@KRAID", "");
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    cmd.Parameters.AddWithValue("@SelfAppraiseDate", QuarterDate);
                    //cmd.ExecuteNonQuery();
                    SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    //ThrowException(dt);
                    ReturnValue = Convert.ToInt32(returnValue.Value);
                    con.Close();

                    return ReturnValue;
                }
            }

            
        }
        catch (Exception ex)
        {

        }
        return ReturnValue;
    }

    //Get Popup for given Manager ratings
    public List<AppraisalReportBLL> GetManagerAppraisal(int aprId)
    {
        List<AppraisalReportBLL> appraisal = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@MODE", "GETMNGRRTNGS");
        cmd.Parameters.AddWithValue("@AppraisalId", aprId);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                AppraisalReportBLL obj = new AppraisalReportBLL(0, "", 0, "", 0, 0);
                obj.AppraisalId = Convert.ToInt32(Dr["AppraisalId"]);
                obj.Value = Convert.ToInt32(Dr["Value"]);
                obj.KRANames = Convert.ToString(Dr["KRANames"]);
                obj.KRAID = Convert.ToInt32(Dr["KRAID"]);
                obj.Id = Convert.ToInt32(Dr["Id"]);
                obj.AppraisalTransactionId = Convert.ToInt32(Dr["AppraisalTransactionId"]);
                obj.ManagerValue = Convert.ToInt32(Dr["ManagerValue"]);
                obj.ProjName = Convert.ToString(Dr["projName"]);
                appraisal.Add(obj);
            }
        }
        return appraisal;
    }

    // Save final appraisal data with manager ratings with comments
    public void SaveEmpManagerReview(List<AppraisalReportBLL> GridData, string Comments, int AppraiseById)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        try
        {
            if (GridData.Count > 0)
            {
                using (con)
                {
                    con.Open();
                    foreach (var item in GridData)
                    {
                        if (item.Id != 0)
                        {
                            SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MODE", "INSERTMNGRRTNGS");
                            cmd.Parameters.AddWithValue("@AppraisalId", item.AppraisalId);
                            cmd.Parameters.AddWithValue("@AppraiseById", AppraiseById);
                            cmd.Parameters.AddWithValue("@MNGRRTNGS", item.ManagerValue);
                            cmd.Parameters.AddWithValue("@Comments", Comments);
                            cmd.Parameters.AddWithValue("@EmployeeReviewID", item.Id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    //Get the Employee self appraisal report
    public List<AppraisalReportBLL> GetEmpAppraisalReport(int empid, int projId, DateTime CurrentQuarterDate)
    {
        int AppraisalBy = Convert.ToInt32(HttpContext.Current.Session["empid"]);
        List<AppraisalReportBLL> curproject = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@MODE", "APPREPORT");
        cmd.Parameters.AddWithValue("@SubMode", "GETEMPAPPRREPORT");
        cmd.Parameters.AddWithValue("@EmpID", empid); //On row selected employee id
        cmd.Parameters.AddWithValue("@AppraiseById", AppraisalBy); //Authority member id[*Login id]
        cmd.Parameters.AddWithValue("@ProjectId", projId);
        cmd.Parameters.AddWithValue("@SelfAppraiseDate", CurrentQuarterDate);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AppraisalReportBLL objEmp = new AppraisalReportBLL();

                objEmp.Id = Convert.ToInt32(reader["EmployeeID"]);
                objEmp.empName = Convert.ToString(reader["EmpName"]);
                objEmp.AuthorityName = Convert.ToString(reader["AuthorityName"]);
                objEmp.projId = Convert.ToInt32(reader["ProjectID"]);
                objEmp.ProjName = Convert.ToString(reader["ProjName"]);
                objEmp.KRANames = Convert.ToString(reader["KRANames"]);
                objEmp.EmployeeRatings = Convert.ToInt32(reader["EmployeeRatings"]);
                objEmp.ManagerRatings = Convert.ToInt32(reader["ManagerRatings"]);
                objEmp.Quarter = Convert.ToString(reader["Quarter"]);
                objEmp.Comments = Convert.ToString(reader["Comments"]);
                curproject.Add(objEmp);
            }
        }
        return curproject;
    }

    // Get self appraise project wise data..
    public List<AppraisalReportBLL> GetAllProjects(int empId)
    {
        List<AppraisalReportBLL> curproject = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@MODE", "GETSELFAPPDATA");
        cmd.Parameters.AddWithValue("@EmpID", empId);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;

        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AppraisalReportBLL obj = new AppraisalReportBLL();

                obj.Id = Convert.ToInt32(reader["Id"]);
                obj.empid = Convert.ToInt32(reader["empid"]);
                obj.projId = Convert.ToInt32(reader["projId"]);
                obj.ProjName = Convert.ToString(reader["projName"]);
                obj.Quarter = Convert.ToString(reader["Quarter"]);
                obj.Status = Convert.ToString(reader["Status"]);
                obj.EmpSelfAppraisaDate = reader["EmpSelfAppraisaDate"].ToString();
                obj.SelfAppQuarterDate = reader["QuarterStartDate"].ToString();
                obj.ReportingManager = reader["ReportingManager"].ToString();
                curproject.Add(obj);
            }
        }
        return curproject;
    }

    // Get popup self appraise data for given ratings
    public List<AppraisalReportBLL> GetSelfpAppraisal(string SubMode, int EmpId, int ProjId, DateTime SelfAppraiseDate)
    {
        List<AppraisalReportBLL> appraisal = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@MODE", "GETPOPEMPRTNGS");
        cmd.Parameters.AddWithValue("@EmpID", EmpId);
        cmd.Parameters.AddWithValue("@ProjectId", ProjId);
        cmd.Parameters.AddWithValue("@SelfAppraiseDate", SelfAppraiseDate);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                AppraisalReportBLL obj = new AppraisalReportBLL();
                obj.KRAID = Convert.ToInt32(Dr["KRAID"]);
                obj.KRANames = Convert.ToString(Dr["KRANames"]);
                obj.AppraisalId = Convert.ToInt32(Dr["FruitID"]);
                obj.Comments = Convert.ToString(Dr["Comments"]);
                obj.EmpAprId = Convert.ToInt32(Dr["EmpAprId"]);
                obj.Id = Convert.ToInt32(Dr["Id"]);

                appraisal.Add(obj);
            }
        }
        return appraisal;
    }

    // Provide self appraisal ratings
    public void SaveEmpApprReview(List<AppraisalReportBLL> GridData, int ProjectId)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        try
        {
            if (GridData.Count > 0)
            {
                using (con)
                {
                    con.Open();
                    foreach (var item in GridData)
                    {
                        if (item.AppraisalId != 0)
                        {
                            SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MODE", "INSERTSELFAPPRTNGS");
                            cmd.Parameters.AddWithValue("@KRAID", item.KRAID);
                            cmd.Parameters.AddWithValue("@EMPRTNGS", item.AppraisalId);
                            cmd.Parameters.AddWithValue("@AppraisalId", item.EmpAprId);
                            cmd.Parameters.AddWithValue("@Comments", "");
                            cmd.Parameters.AddWithValue("@AppraisalTransId", item.Id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    //Get the Employee self appraisal report
    public List<AppraisalReportBLL> GetEmpSelfAppraisalReport(int empid, int projId, int AppraiseBy, DateTime CurrentQuarterDate)
    {
        List<AppraisalReportBLL> curproject = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@MODE", "APPREPORT");
        cmd.Parameters.AddWithValue("@SubMode", "GETEMPSELFAPPRREPORT");
        cmd.Parameters.AddWithValue("@EmpID", empid);
        cmd.Parameters.AddWithValue("@ProjectId", projId);
        cmd.Parameters.AddWithValue("@SelfAppraiseDate", CurrentQuarterDate);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AppraisalReportBLL objEmp = new AppraisalReportBLL();

                objEmp.Id = Convert.ToInt32(reader["EmployeeID"]);
                objEmp.empName = Convert.ToString(reader["EmpName"]);
                objEmp.projId = Convert.ToInt32(reader["ProjectID"]);
                objEmp.ProjName = Convert.ToString(reader["ProjName"]);
                objEmp.KRANames = Convert.ToString(reader["KRANames"]);
                objEmp.EmployeeRatings = Convert.ToInt32(reader["EmployeeRatings"]);
                objEmp.Quarter = Convert.ToString(reader["Quarter"]);
                curproject.Add(objEmp);
            }
        }
        return curproject;
    }

    //Geting appraisal records as per quarter basis
    public List<AppraisalReportBLL> GetFinalAppraisalReport(string Mode, int Counter)
    {
        List<AppraisalReportBLL> GetApprpt = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@MODE", "GETAPPFNLREPORT");
        cmd.Parameters.AddWithValue("@Counter", Counter);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AppraisalReportBLL AppRptBLL = new AppraisalReportBLL();

                AppRptBLL.empid = Convert.ToInt32(reader["AuthorityID"]);
                AppRptBLL.empName = reader["empName"].ToString();
                AppRptBLL.ProjectCounts = Convert.ToInt32(reader["ProjectCounts"]);
                AppRptBLL.QuarterStartDate = Convert.ToDateTime(reader["QuarterStartDate"]);
                GetApprpt.Add(AppRptBLL);
            }
        }
        return GetApprpt;
    }

    //Geting employee projects 
    public List<AppraisalReportBLL> GetEmployeeAppraisalProjects(int EmpId,int Counter)
    {
        List<AppraisalReportBLL> GetApprpt = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@MODE", "GETALLMNGRAPPTNGSRPT");
        cmd.Parameters.AddWithValue("@SubMode", "GETALLAPPPROJECTS");
        cmd.Parameters.AddWithValue("@EmpID", EmpId);
        cmd.Parameters.AddWithValue("@Counter", Counter);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AppraisalReportBLL AppRptBLL = new AppraisalReportBLL();
                AppRptBLL.projId = Convert.ToInt32(reader["ProjectID"]);
                AppRptBLL.empid = Convert.ToInt32(reader["ProjectMemberID"]);
                GetApprpt.Add(AppRptBLL);
            }
        }
        return GetApprpt;
    }

    //Geting appraisal all manager Comments
    public List<AppraisalReportBLL> GetManagerComments(int EmpId, int ProjectId)
    {
        List<AppraisalReportBLL> GetApprpt = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@MODE", "GETALLMNGRAPPTNGSRPT");
        cmd.Parameters.AddWithValue("@SubMode", "GETALLAPPCOMMENTS");
        cmd.Parameters.AddWithValue("@EmpID", EmpId);
        cmd.Parameters.AddWithValue("@ProjectId", ProjectId);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AppraisalReportBLL AppRptBLL = new AppraisalReportBLL();
                AppRptBLL.AuthorityName = reader["AuthorityName"].ToString();
                AppRptBLL.projId = Convert.ToInt32(reader["projId"]);
                AppRptBLL.Comments = reader["Comment"].ToString();
                GetApprpt.Add(AppRptBLL);
            }
        }
        return GetApprpt;
    }

    //Geting appraisal all manager ratings as per quarter basis
    public DataTable GetAppraisalManagerRatingsReport(int EmpId,int ProjectId, int Counter)
    {
        List<AppraisalReportBLL> GetApprpt = new List<AppraisalReportBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
        cmd.Parameters.AddWithValue("@MODE", "GETALLMNGRAPPTNGSRPT");
        cmd.Parameters.AddWithValue("@SubMode", "GETALLEMPMNGRAPPRPT");
        cmd.Parameters.AddWithValue("@EmpID", EmpId);
        cmd.Parameters.AddWithValue("@ProjectId", ProjectId);
        cmd.Parameters.AddWithValue("@Counter", Counter);
        cmd.CommandType = CommandType.StoredProcedure;

        var columns = new List<string>();
        DataTable dt = new DataTable();
        using (con)
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
        }
        return dt;
    }


}