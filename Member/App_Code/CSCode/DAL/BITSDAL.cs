using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Customer.BLL;
using System.Data.SqlClient;
using System.Data;
using Customer.BLL;
namespace Customer.DAL
{
    /// <summary>
    /// Summary description for CustUsersDAL
    /// </summary>
    public class BITSDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        public BITSDAL()
        {
        }

        public List<BITS> GetProjectDetails(int PMID, bool? showOverHeads, bool? Added, bool? Initiated, bool? InProgress, bool? UnderUAT, bool? OnHold, bool? CompletedClosed, bool? Cancelled, bool? UnderWarranty, bool? TNM, bool? FixedCost)
        {
            try
            {
                List<BITS> project = new List<BITS>();
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("sp_BI", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prjid", null);
                cmd.Parameters.AddWithValue("@year", null);
                cmd.Parameters.AddWithValue("@month", null);
                cmd.Parameters.AddWithValue("@mode", "Projects");
                cmd.Parameters.AddWithValue("@PMID", PMID);
                cmd.Parameters.AddWithValue("@showOverheads", showOverHeads);
                cmd.Parameters.AddWithValue("@Added", Added);
                cmd.Parameters.AddWithValue("@Initiated", Initiated);
                cmd.Parameters.AddWithValue("@InProgress", InProgress);
                cmd.Parameters.AddWithValue("@UnderUAT", UnderUAT);
                cmd.Parameters.AddWithValue("@OnHold", OnHold);
                cmd.Parameters.AddWithValue("@CompletedClosed", CompletedClosed);
                cmd.Parameters.AddWithValue("@Cancelled", Cancelled);
                cmd.Parameters.AddWithValue("@UnderWarranty", UnderWarranty);
                cmd.Parameters.AddWithValue("@TNM", TNM);
                cmd.Parameters.AddWithValue("@FixedCost", FixedCost);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        project.Add(new BITS(
                            Convert.ToInt32(reader["projectId"]),
                            reader["projectName"].ToString(),
                            reader["PM"].ToString(),
                            //reader["PM"].ToString(), /*made changes by sumit*/
                            //reader["PM"].ToString(),
                            reader["BA"].ToString(),
                            reader["AccManager"].ToString(),
                            reader["Duration"].ToString(),
                            Convert.ToDecimal(reader["BudgetedHour"].ToString()),
                            Convert.ToDecimal(reader["ActualHour"].ToString()),
                            reader["strUnApprovedHours"].ToString(),
                            //Convert.ToDecimal(reader["TotalHoursConsumed"].ToString()),
                            reader["Status"].ToString(),
                            Convert.ToDecimal(reader["BudgetedCost"].ToString()),
                            Convert.ToDecimal(reader["ActualCost"].ToString()),
                            Convert.ToDecimal(reader["ActualPayment"].ToString()),
                            Convert.ToDecimal(reader["ProjectHealth_Effort"].ToString()),
                            Convert.ToDecimal(reader["ProjectHealth_Cost"].ToString()),
                            Convert.ToDateTime(reader["ReportDate"].ToString())
                      ));
                    }
                }
                return project;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<BITS> GetTSBreakupDetails(int prjId)
        {
            List<BITS> project = new List<BITS>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_BI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@prjid", prjId);
            cmd.Parameters.AddWithValue("@year", null);
            cmd.Parameters.AddWithValue("@month", null);
            cmd.Parameters.AddWithValue("@mode", "WorkWiseTSBreakUp");
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    project.Add(new BITS(
                        reader["Work"].ToString(),
                        reader["Percentage_Effort"].ToString(),
                        Convert.ToDecimal(reader["Hours"].ToString()),
                        Convert.ToDecimal(reader["Cost"].ToString())
                        //Convert.ToDecimal(reader["TotalHoursConsumed"].ToString())
                        ));
                }
            }
            return project;
        }

        public List<BITS> GetTimesheetDetailsMonthwise(int prjId)
        {
            List<BITS> project = new List<BITS>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_BI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@prjid", prjId);
            cmd.Parameters.AddWithValue("@year", null);
            cmd.Parameters.AddWithValue("@month", null);
            cmd.Parameters.AddWithValue("@mode", "MonthWiseTSDetails");
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    project.Add(new BITS(
                        Convert.ToInt32(reader["ID"]),
                        reader["Month"].ToString(),
                        reader["TSYear"].ToString(),
                        reader["TSMonth"].ToString(),
                        //Convert.ToDecimal(reader["UnApprovedHours"].ToString()),
                        reader["Percentage_Effort"].ToString(),
                        Convert.ToDecimal(reader["Hours"].ToString()),
                        Convert.ToDecimal(reader["Cost"].ToString())
                        ));
                }
            }
            return project;
        }

        public List<BITS> GetTimesheetDetailsWorkwise(int prjId, int year, int month)
        {
            List<BITS> project = new List<BITS>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_BI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@prjid", prjId);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@mode", "WorkWiseTSDetails");
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    project.Add(new BITS(
                        reader["Work"].ToString(),
                         reader["Percentage_Effort"].ToString(),
                        Convert.ToDecimal(reader["Hours"].ToString()),
                        Convert.ToDecimal(reader["Cost"].ToString())
                        //Convert.ToDecimal(reader["TotalHoursConsumed"].ToString())
                        ));
                }
            }
            return project;
        }


        public static DataSet BIReport(string mode)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_BI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return ds;
        }



        public static DataSet getTimeSheetdetails(string mode)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_BI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return ds;
        }
        public static DataSet getDetailReportbyProjId(string mode, int projId)
        {
            DataSet dsM = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_BI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@prjId", projId);
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dsM);
                }
                return dsM;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return dsM;
        }

        public static DataSet getprojNamebyEmpId(string mode, int empId)
        {
            DataSet dsM = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_BI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@empId", empId);
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dsM);
                }
                return dsM;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return dsM;
        }

        public static DataSet GetTSData(string mode, int projId, int empId)
        {
            DataSet dsM = new DataSet();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_BI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@PrjId", projId);
            cmd.Parameters.AddWithValue("@empId", empId);
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dsM);
                }
                return dsM;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return dsM;
        }


    }
}