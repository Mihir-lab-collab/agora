using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for EmpWFHApprovalDAL
/// </summary>
public class EmpWFHApprovalDAL
{
    public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    DataTable dt = null;
    public EmpWFHApprovalDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public DataTable GetEmpWFH(string mode, EmpWFHApprovalBLL objBLL, int loginID, int includeArchive)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpWFHApproval", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@LoginID", loginID);
        cmd.Parameters.AddWithValue("@LocationID", objBLL.LocationID);
        cmd.Parameters.AddWithValue("@Name", objBLL.EmpName);
        cmd.Parameters.AddWithValue("@Status", objBLL.WFHStatus);
        if (!string.IsNullOrEmpty(objBLL.WFHFrom))
        {
            cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(objBLL.WFHFrom));
        }
        else
        {
            cmd.Parameters.AddWithValue("@StartDate", null);
        }
        if (!string.IsNullOrEmpty(objBLL.WFHFrom))
        {
            cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(objBLL.WFHTo));
        }
        else
        {
            cmd.Parameters.AddWithValue("@EndDate", null);
        }
        cmd.Parameters.AddWithValue("@isArchive", 1);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

    }
    public string UpdateEmpWFHStatus(string mode, string ip, EmpWFHApprovalBLL objBLL)
    {
        int sReturn = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_EmpWFHApproval", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", objBLL.EmpID);
        cmd.Parameters.AddWithValue("@EmpWFHID", objBLL.EmpWFHID);
        cmd.Parameters.AddWithValue("@StartDate", objBLL.WFHFrom);
        cmd.Parameters.AddWithValue("@EndDate", objBLL.WFHTo);
        cmd.Parameters.AddWithValue("@Status", objBLL.WFHStatus);
        cmd.Parameters.AddWithValue("@WFHComment", objBLL.AdminComment);
        cmd.Parameters.AddWithValue("@WFHSanctionBy", objBLL.WFHSanctionBy);
        cmd.Parameters.AddWithValue("@ip", ip);
        cmd.Parameters.AddWithValue("@LoginID", Convert.ToInt32(objBLL.LoginEmpId));
        try
        {
            using (con)
            {
                sReturn = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception)
        {
            throw;
        }
        return sReturn.ToString();
    }
    public string GetProfile(string mode, string empID)
    {
        string sReturn = "";
        SqlConnection con = new SqlConnection(_strConnection);
        // con.Open();
        SqlCommand cmd = new SqlCommand("SP_Employee", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmployeeID", Convert.ToInt32(empID));
        try
        {
            using (con)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "1")
                    {
                        sReturn = "True";
                    }
                    else
                    {
                        sReturn = "False";
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;

        }
        return sReturn;
    }
    public List<EmpWFHApprovalBLL> EmployeeList(string mode)
    {
        List<EmpWFHApprovalBLL> EmpList = new List<EmpWFHApprovalBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpWFHApproval", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    EmpWFHApprovalBLL empWFHApprovalBLL = new EmpWFHApprovalBLL()
                    {
                        EmpID = Convert.ToInt32(dt.Rows[i]["empId"]).ToString(),
                        EmpName = dt.Rows[i]["empName"].ToString()
                    };
                    EmpList.Add(empWFHApprovalBLL);
                }
            }
            return EmpList;
        }
    }
    public int BulkApplyWFH(int[] empList, string mode,
        string WFHFrom, string WFHTo, string WFHDescription, string WFHSanctionedDate, string WFHSanctionBy)
    {
        int rowsAffected = 0;
        int count = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();

        for (int i = 0; i < empList.Length; i++)
        {
            using (SqlCommand cmd = new SqlCommand("SP_EmpWFHApproval", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@EmpID", empList[i]);
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(WFHFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(WFHTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@WFHDescription", WFHDescription);
                //cmd.Parameters.AddWithValue("@WFHSanctionedDate", WFHSanctionedDate);
                cmd.Parameters.AddWithValue("@WFHSanctionBy", WFHSanctionBy);
                try
                {
                    rowsAffected = cmd.ExecuteNonQuery();
                    count++;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        con.Close();
        return count;
    }
    public DataTable SendMail(string mode, int empId)
    {
        List<EmpWFHApprovalBLL> EmpList = new List<EmpWFHApprovalBLL>();
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_EmpWFHApproval", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", empId);
        try
        {
            using (con)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }
        }
        catch (Exception)
        {
            throw;
        }
        return dt;
    }
}