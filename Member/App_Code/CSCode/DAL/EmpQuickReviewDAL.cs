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
public class EmpQuickReviewDAL
{
    public EmpQuickReviewDAL() { }

    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public static string IPAddress
    {
        get
        {
            return Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        }
    }
    public List<EmpQuickReviewBLL> Get_EmpQuickReviewList(string Mode)
    {
        List<EmpQuickReviewBLL> curproject = new List<EmpQuickReviewBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("sp_EmpQuickReview", con);
        cmd.Parameters.AddWithValue("@EmpID", 0);
        cmd.Parameters.AddWithValue("@MODE", Mode);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curproject.Add(new EmpQuickReviewBLL(
                    Convert.ToInt32(reader["EmployeeCode"]),
                    reader["EmployeeName"].ToString(),
                    Convert.ToInt32(reader["EmpReviewCount"]),
                    Convert.ToDateTime(reader["LastReviewDate"])
                    ));
            }
        }
        return curproject;
    }

    public List<EmpQuickReviewBLL> Get_ReviewListByEmpCode(int EmployeeCode, string Mode)
    {
        List<EmpQuickReviewBLL> curproject = new List<EmpQuickReviewBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("sp_EmpQuickReview", con);
        cmd.Parameters.AddWithValue("@EmpID", EmployeeCode);
        cmd.Parameters.AddWithValue("@MODE", Mode);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curproject.Add(new EmpQuickReviewBLL(
                    Convert.ToInt32(reader["ReviewId"]),
                    Convert.ToDateTime(reader["InsertedOn"]),
                    reader["ReviewText"].ToString(),
                    reader["ReviewCreatedBy"].ToString(),
                    reader["AcceptedStatus"].ToString(),
                    reader["AcceptedOn"] == null ? "" : reader["AcceptedOn"].ToString()
                    ));
            }
        }
        return curproject;
    }

    public void SaveReviewText(string mode, EmpQuickReviewBLL objReview)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("sp_EmpQuickReview", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@ReviewId", objReview.ReviewId);
        cmd.Parameters.AddWithValue("@EmpID", objReview.EmployeeCode);
        cmd.Parameters.AddWithValue("@ReviewText", objReview.ReviewText);
        cmd.Parameters.AddWithValue("@InsertedBy", objReview.InsertedBy);
        cmd.Parameters.AddWithValue("@InsertedOn", DateTime.Now);
        cmd.Parameters.AddWithValue("@InsertedIP", IPAddress);
        try
        {
            using (con)
            {
                cmd.ExecuteNonQuery();
                //outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        catch (Exception ex)
        {
            string err = ex.Message;
        }
    }

    public void Update_AcceptedReviews(string mode, int EmployeeCode)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("sp_EmpQuickReview", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", EmployeeCode);
        cmd.Parameters.AddWithValue("@AcceptedOn", DateTime.Now);
        cmd.Parameters.AddWithValue("@AcceptedIP", IPAddress);
        try
        {
            using (con)
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            string err = ex.Message;
        }
    }

    public List<EmployeeMaster> GetEmployees(string mode)
    {
        List<EmployeeMaster> obj = new List<EmployeeMaster>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("sp_EmpQuickReview", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EmployeeMaster EM = new EmployeeMaster();
                EM.empid = Convert.ToInt32(reader["empid"]);
                EM.empName = reader["empName"].ToString();
                obj.Add(EM);
            }
        }
        return obj;
    }
}