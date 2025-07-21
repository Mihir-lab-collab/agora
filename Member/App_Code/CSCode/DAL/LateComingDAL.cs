using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Web.Configuration;


/// <summary>
/// Summary description for LateComingDAL
/// </summary>

public class LateComingDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public LateComingDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataTable GetLateComersData(string mode)
    {
         SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("Sp_LateComersData", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
    public DataTable InsertLateComing(LateComing objLateComing)
    {
        DataTable dtLateComing = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertLateComing", con);
                cmd.Parameters.AddWithValue("@EmpCode", objLateComing.EmpCode);
                cmd.Parameters.AddWithValue("@ApplyDate", objLateComing.ApplyDate);
                cmd.Parameters.AddWithValue("@ExpectedInTime", objLateComing.ExpectedInTime);
                cmd.Parameters.AddWithValue("@CreatedOn", objLateComing.CreatedOn);
                cmd.Parameters.AddWithValue("@CreatedBy", objLateComing.CreatedBy);
                cmd.Parameters.AddWithValue("@LateCommingReason", objLateComing.LateCommingReason);
                cmd.Parameters.AddWithValue("@ApprovedOn", objLateComing.ApprovedOn);
                cmd.Parameters.AddWithValue("@ApprovalComment", objLateComing.ApprovalComment);
                cmd.Parameters.AddWithValue("@IsApproveStatus", objLateComing.IsApproveStatus);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtLateComing);
                con.Close();
                return dtLateComing;
            }
        }
        catch (Exception ex)
        {
            dtLateComing.Columns.Add("Status");
            dtLateComing.Columns.Add("Message");
            var row = dtLateComing.NewRow();
            row[0]= 0;
            dtLateComing.Rows.Add(row);
            dtLateComing.Rows[0]["Message"] = ex.Message;
            return dtLateComing;
        }

    }

    public void ApproveLateComing(string mode, int EmpCode, string Comment, int ID,int approveStatus,int ApprovedBy)
    {
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_LateComersData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@EmpCode", EmpCode);
            cmd.Parameters.AddWithValue("@ApprovalComment", Comment);
            cmd.Parameters.AddWithValue("@IsApproveStatus", approveStatus);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@ApprovedBy", ApprovedBy);
            //cmd.Parameters.AddWithValue("@HolidayLeaveID", ID);
            // cmd.Parameters.AddWithValue("@CompOffDate", CompOffDate);
            Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }
    }

    public void LateComingCancle(string mode,  int ID, int EmpCode, string ApprovalComment)
    {
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_LateComersData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@EmpCode", EmpCode);
            cmd.Parameters.AddWithValue("@AdminCancelReason", ApprovalComment);
            cmd.Parameters.AddWithValue("@ID", ID);
            //cmd.Parameters.AddWithValue("@ApplyDate", ApplyDate);
            Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }
    }
    public void LateComingReject(string mode, int EmpCode, string Comment, int ID, DateTime ApplyDate)
    {
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_LateComersData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@EmpCode", EmpCode);
            cmd.Parameters.AddWithValue("@Comment", Comment);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@ApplyDate", ApplyDate);
            Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }
    }


     public List<LateComing> GetLateComeData(int Empid, int Status, string StartDate, string EndDate, int LocationID)
    {
        //Nullable<DateTime> dt = null;

        string strstartdate = string.Empty;
        string strenddate = string.Empty;

        if (StartDate != "")
        {
            DateTime temp = DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            strstartdate = temp.ToString("yyyy/MM/dd");
        }

        if (EndDate != "")
        {
            DateTime temp1 = DateTime.ParseExact(EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            strenddate = temp1.ToString("yyyy/MM/dd");
        }

        List<LateComing> lstLateComing = new List<LateComing>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("Sp_LateComersData", con);
        cmd.Parameters.AddWithValue("@mode", "GetLateComing");
        cmd.Parameters.AddWithValue("@EmpCode", Empid);
        cmd.Parameters.AddWithValue("@IsApproveStatus", Status);

        if (StartDate != "" && EndDate != "")
        {
            cmd.Parameters.AddWithValue("@StartDate", strstartdate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate == "" ? EndDate : strenddate);
        }

        cmd.Parameters.AddWithValue("@locationId", LocationID);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstLateComing.Add(new LateComing(
                    Convert.ToInt32(reader["ID"].ToString()),
                       reader["empName"].ToString(),
                Convert.ToDateTime(reader["ApplyDate"].ToString()),
                Convert.ToInt32(reader["EmpCode"].ToString()),
                reader["ExpectedInTime"].ToString(),
                reader["LateCommingReason"].ToString(),
                reader["ApprovalComment"].ToString()
                    ));
            }
        }
        return lstLateComing;
    }
}