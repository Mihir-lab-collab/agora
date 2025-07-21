using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Web.Configuration;
using AgoraBL.BAL;


/// <summary>
/// Summary description for EmpLeaveApproval
/// </summary>
namespace AgoraBL.DAL
{
    public class EmpLeaveApprovalDAL
    {
        public string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        DataTable dt = null;
        public EmpLeaveApprovalDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable GetEmpLeaves(string mode, EmpLeaveApprovalBAL objBLL, int loginID, int includeArchive)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpLeaveApproval", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@LoginID", loginID);
            cmd.Parameters.AddWithValue("@LocationID", objBLL.LocationID);
            cmd.Parameters.AddWithValue("@Name", objBLL.EmpName);
            cmd.Parameters.AddWithValue("@Status", objBLL.LeaveStatus);
            cmd.Parameters.AddWithValue("@StartDate", objBLL.LeaveFrom);
            cmd.Parameters.AddWithValue("@EndDate", objBLL.LeaveTo);
            cmd.Parameters.AddWithValue("@isArchive", includeArchive);
            using (con)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GtLeaveType(string mode)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpLeaveApproval", con);
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
                    // sReturn= "True";

                    // sReturn = cmd.ExecuteNonQuery().ToString();
                    // int output1 = cmd.ExecuteNonQuery();
                    //int output = Convert.ToInt32(cmd.ExecuteNonQuery());
                    //if (output == 1)
                    //    sReturn = "True";
                    //else
                    //    sReturn = "False";
                    //con.Close();
                }
            }
            catch (Exception ex)
            { }
            return sReturn;
        }

        public DataSet UpdateEmpLeaveStatus(string mode, string ip, EmpLeaveApprovalBAL objBLL)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpLeaveApproval", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", objBLL.EmpID);
            cmd.Parameters.AddWithValue("@EmpLeaveID", objBLL.EmpLeaveID);
            if (string.Compare(objBLL.LeaveStatus, AgoraBL.Common.ClsConstant.EmpLeaveStatus.Approved, true) == 0 ||
                string.Compare(objBLL.LeaveStatus, AgoraBL.Common.ClsConstant.EmpLeaveStatus.Pending, true) == 0 ||
                string.Compare(objBLL.LeaveStatus, AgoraBL.Common.ClsConstant.EmpLeaveStatus.Rejected, true) == 0)
            {
                if (string.Compare(objBLL.LeaveStatus, AgoraBL.Common.ClsConstant.EmpLeaveStatus.Approved, true) == 0)
                {
                    cmd.Parameters.AddWithValue("@Status", AgoraBL.Common.ClsConstant.EmpDBLeaveStatus.Approved);
                }
                else if (string.Compare(objBLL.LeaveStatus, AgoraBL.Common.ClsConstant.EmpLeaveStatus.Pending, true) == 0)
                {
                    cmd.Parameters.AddWithValue("@Status", AgoraBL.Common.ClsConstant.EmpDBLeaveStatus.Pending);
                }
                else if (string.Compare(objBLL.LeaveStatus, AgoraBL.Common.ClsConstant.EmpLeaveStatus.Rejected, true) == 0)
                {
                    cmd.Parameters.AddWithValue("@Status", AgoraBL.Common.ClsConstant.EmpDBLeaveStatus.Rejected);
                }
                cmd.Parameters.AddWithValue("@leaveComment", objBLL.AdminComment);
                cmd.Parameters.AddWithValue("@leaveSanctionBy", objBLL.LeaveSanctionBy);               
                cmd.Parameters.AddWithValue("@LoginID", Convert.ToInt32(objBLL.LeaveSanctionBy));
            }
                cmd.Parameters.AddWithValue("@StartDate", objBLL.LeaveFrom); //, DateTime.ParseExact(objBLL.LeaveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@EndDate", objBLL.LeaveTo);    // DateTime.ParseExact(objBLL.LeaveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@LeaveType", objBLL.LeaveType);
            cmd.Parameters.AddWithValue("@ip", ip);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
            //try
            //{
            //    using (con)
            //    {
            //        sReturn = cmd.ExecuteNonQuery().ToString();
            //        con.Close();
            //    }
            //}
            //catch (Exception ex)
            //{ }
            //return sReturn;
        }
    } 
}