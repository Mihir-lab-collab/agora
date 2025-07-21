using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

public class PayrollDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    int PayId = 0;
    public PayrollDAL()
    {
    }

    public List<PayrollBLL> GetPayrollDetails(string mode, bool? isActive, int? locationId, int? EmpId)
    {
        List<PayrollBLL> payroll = new List<PayrollBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Salary", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@IsActive", isActive);
        cmd.Parameters.AddWithValue("@LocationID", locationId);
        cmd.Parameters.AddWithValue("@EmpID", EmpId);
        SqlDataReader reader = null;
        PayrollBLL ObjPayroll = new PayrollBLL();

        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                decimal PF = Convert.ToDecimal(reader["PF"].ToString());
                decimal Gross = Convert.ToDecimal(reader["MonthlyCTC"].ToString());
                decimal MonthlyCTC = Convert.ToDecimal(reader["MonthlyCTC"].ToString());
                decimal Gratuity = Convert.ToDecimal(reader["Gratuity"].ToString());

                decimal AnnualGratuity = Gratuity * 12;

                if (reader["PF"].ToString() != "" && reader["PF"].ToString() != "0")
                {
                    PF = ObjPayroll.CalcPF(Convert.ToInt32(reader["EmpID"]), 0, Convert.ToDecimal(reader["Basic"]));
                    Gross = Convert.ToDecimal(reader["Gross"].ToString());
                }

                payroll.Add(new PayrollBLL(
                    Convert.ToInt32(reader["EmpID"]),
                    reader["Name"].ToString(),
                    Convert.ToDateTime(reader["JoiningDate"].ToString()),
                    Convert.ToDecimal(reader["PastExperince"].ToString()),
                    Convert.ToDecimal(reader["TotalExperince"].ToString()),
                    Convert.ToDateTime(reader["RevisionDate"].ToString()),
                    Gross,
                    Convert.ToDecimal(reader["Bonus"].ToString()),
                    PF,
                    MonthlyCTC,//Gross + PF,
                    ((Gross + PF)*12 + Convert.ToDecimal(reader["Bonus"].ToString()))- AnnualGratuity,
                    Convert.ToDecimal(reader["Net"].ToString()),
                    Convert.ToDecimal(reader["PT"].ToString()),
                    Convert.ToDecimal(reader["Insurance"].ToString()),
                    Convert.ToBoolean(reader["PBB"].ToString()),
                    Convert.ToDecimal(reader["Basic"].ToString()),
                    Convert.ToDecimal(reader["HRA"].ToString()),
                    Convert.ToDecimal(reader["Conveyance"].ToString()),
                    Convert.ToDecimal(reader["Medical"].ToString()),
                    Convert.ToDecimal(reader["Food"].ToString()),
                    Convert.ToDecimal(reader["Special"].ToString()),
                    Convert.ToDecimal(reader["LTA"].ToString())
                    ));
            }
        }
        return payroll;
    }

    public List<PayrollBLL> GetSalaryDetails(string mode, int? EmpId, int? year, int? month, int? locationId)
    {
        List<PayrollBLL> payroll = new List<PayrollBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Salary", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@EmpID", EmpId);
        cmd.Parameters.AddWithValue("@LocationID", locationId);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                payroll.Add(new PayrollBLL(
                     Convert.ToInt32(reader["EmpID"]),
                     reader["Name"].ToString(),
                     Convert.ToDateTime(reader["Date"].ToString()),
                     Convert.ToString(reader["SkillDesc"].ToString()),
                     Convert.ToDateTime(reader["JoiningDate"].ToString()),
                     Convert.ToDecimal(reader["Basic"].ToString()),
                     Convert.ToDecimal(reader["HRA"].ToString()),
                     Convert.ToDecimal(reader["Conveyance"].ToString()),
                     Convert.ToDecimal(reader["Medical"].ToString()),
                     Convert.ToDecimal(reader["Food"].ToString()),
                     Convert.ToDecimal(reader["Special"].ToString()),
                     Convert.ToDecimal(reader["LTA"].ToString()),
                     Convert.ToDecimal(reader["PF"].ToString()),
                     Convert.ToDecimal(reader["PT"].ToString()),
                     Convert.ToDecimal(reader["Insurance"].ToString()),
                     Convert.ToDecimal(reader["Loan"].ToString()),
                     Convert.ToDecimal(reader["Advance"].ToString()),
                     Convert.ToDecimal(reader["Leaves"].ToString()),
                     Convert.ToDecimal(reader["Presents"].ToString()),
                     Convert.ToDecimal(reader["Tax"].ToString()),
                     Convert.ToDecimal(reader["Deduction"].ToString()),
                     Convert.ToDecimal(reader["LeaveDeduction"].ToString()),
                     Convert.ToDecimal(reader["Bonus"].ToString()),
                     Convert.ToDecimal(reader["Addition"].ToString()),
                     reader["Remark"].ToString(),
                     Convert.ToDecimal(reader["CTC"].ToString()),
                     Convert.ToDecimal(reader["Gross"].ToString()),
                     Convert.ToDecimal(reader["AB"].ToString()),
                     Convert.ToDecimal(reader["Net"].ToString()),
                     Convert.ToDecimal(reader["TotalAddition"].ToString()),
                     Convert.ToDecimal(reader["TotalDeduction"].ToString()),
                     reader["Comment"].ToString(),
                     Convert.ToInt32(reader["PayId"]),
                     Convert.ToString(reader["Days"]) == "" ? 0 : Convert.ToDecimal(reader["Days"].ToString()),
                     Convert.ToString(reader["CalBasic"]) =="" ? 0 : Convert.ToDecimal(reader["CalBasic"].ToString()),
                     Convert.ToString(reader["CalGross"]) == "" ? 0 : Convert.ToDecimal(reader["CalGross"].ToString()),
                     (reader["EmpPAN"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["EmpPAN"]),
                     (reader["EmpUAN"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["EmpUAN"]),
                     (reader["EmpEPF"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["EmpEPF"])
                    ));
            }
        }
        return payroll;
    }

    public bool IFExistsPayProcess(int? month, int? year, int? locationId)
    {
        List<PayrollBLL> payroll = new List<PayrollBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SELECT * FROM employeePayProcess WHERE LocationID= " + locationId + "and Month(payDate) = " + month + " and year(payDate)=" + year, con);
        cmd.CommandType = CommandType.Text;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
                return true;
            else
                return false;
        }

    }

    public List<PayrollBLL> GetAddPayDetails(string mode, int? year, int? month, int? locationId, int? days)
    {
        List<PayrollBLL> payroll = new List<PayrollBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_SalaryAdd", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@LocationID", locationId);
        cmd.Parameters.AddWithValue("@Days", days);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                payroll.Add(new PayrollBLL(
                     Convert.ToInt32(reader["EmpID"]),
                     reader["Name"].ToString(),
                     Convert.ToDecimal(reader["Gross"].ToString()),
                     Convert.ToBoolean(reader["IsPF"].ToString()),
                     Convert.ToDecimal(reader["PT"].ToString()),
                     Convert.ToDecimal(reader["Insurance"].ToString()),
                     Convert.ToDecimal(reader["Basic"].ToString()),
                     Convert.ToDecimal(reader["HRA"].ToString()),
                     Convert.ToDecimal(reader["Conveyance"].ToString()),
                     Convert.ToDecimal(reader["Medical"].ToString()),
                     Convert.ToDecimal(reader["Food"].ToString()),
                     Convert.ToDecimal(reader["Special"].ToString()),
                     Convert.ToDecimal(reader["LTA"].ToString()),
                     reader["Leaves"].ToString() == "" ? 0 : Convert.ToDecimal(reader["Leaves"].ToString()),
                     Convert.ToDecimal(reader["Days"].ToString()),
                     Convert.ToDecimal(reader["PresentDays"].ToString()),
                     reader["Gender"].ToString(),
                     Convert.ToDecimal(reader["empPayGratuity"].ToString()),
                     Convert.ToDecimal(reader["CTC"].ToString()),
                     Convert.ToDecimal(reader["PF"].ToString())
                    ));
            }
        }
        return payroll;
    }

    public bool IFExistsPayProcess_History(string mode, int? locationId)
    {
        List<PayrollBLL> payroll = new List<PayrollBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_SalaryAdd", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@LocationID", locationId);
        bool success = false;
        using (con)
        {
            con.Open();
            cmd.ExecuteNonQuery();
            success = true;
        }
        return success;
    }

    public List<PayrollBLL> GetAddPayDetails_History(string mode, int? locationId)
    {
        List<PayrollBLL> payroll = new List<PayrollBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_SalaryAdd", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@LocationID", locationId);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                payroll.Add(new PayrollBLL(
                      Convert.ToInt32(reader["locId"].ToString()),
                      Convert.ToInt32(reader["empId"]),
                      Convert.ToDecimal(reader["payLoanInstl"].ToString()),
                      Convert.ToDecimal(reader["payAdvance"].ToString()),
                      Convert.ToDecimal(reader["payDeduction"].ToString()),
                      Convert.ToDecimal(reader["payOtherDeduction"].ToString()),
                      Convert.ToDecimal(reader["payBonus"].ToString()),
                      Convert.ToDecimal(reader["payAddition"].ToString()),
                      reader["payRemark"].ToString()
                    ));
            }
        }
        return payroll;
    }

    public int InsertPayProcess(string mode, int? locationId, DateTime? payDate, string payComment, DateTime? payProcessDate, bool? payStatus)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SalaryAdd", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@LocationID", locationId);
        cmd.Parameters.AddWithValue("@payDate", payDate);
        cmd.Parameters.AddWithValue("@payComment", payComment);
        cmd.Parameters.AddWithValue("@payStatus", payStatus);

        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                PayId = outputid;
                con.Close();

            }
        }
        catch (Exception ex)
        { }
        return outputid;
    }

    public void InsertPayProcessDetails(PayrollBLL objInsert)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SalaryAdd", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.Mode);
        cmd.Parameters.AddWithValue("@payId", objInsert.PayId);
        cmd.Parameters.AddWithValue("@empId", objInsert.EmpID);
        cmd.Parameters.AddWithValue("@Basic", objInsert.Basic);
        cmd.Parameters.AddWithValue("@Hra", objInsert.HRA);
        cmd.Parameters.AddWithValue("@Conveyance", objInsert.Conveyance);
        cmd.Parameters.AddWithValue("@Medical", objInsert.Medical);
        cmd.Parameters.AddWithValue("@Food", objInsert.Food);
        cmd.Parameters.AddWithValue("@Special", objInsert.Special);
        cmd.Parameters.AddWithValue("@LTA", objInsert.LTA);
        cmd.Parameters.AddWithValue("@PF", objInsert.PF);
        cmd.Parameters.AddWithValue("@EPF", 0);
        cmd.Parameters.AddWithValue("@AT", 0);
        cmd.Parameters.AddWithValue("@PT", objInsert.PT);
        cmd.Parameters.AddWithValue("@Insurance", objInsert.Insurance);
        cmd.Parameters.AddWithValue("@payLoanInstl", objInsert.Ded_Loan);
        cmd.Parameters.AddWithValue("@payAdvance", objInsert.Ded_Advance);
        cmd.Parameters.AddWithValue("@payLeave", objInsert.Leaves);
        cmd.Parameters.AddWithValue("@payLeaveDays", objInsert.Days);
        cmd.Parameters.AddWithValue("@payLeaveDeduction", objInsert.Ded_Leave);
        cmd.Parameters.AddWithValue("@payOthers", 0);
        cmd.Parameters.AddWithValue("@payDeduction", objInsert.Ded_Tax);
        cmd.Parameters.AddWithValue("@payOtherDeduction", objInsert.Ded_Deduction);
        cmd.Parameters.AddWithValue("@payBonus", objInsert.Add_Bonus);
        cmd.Parameters.AddWithValue("@payAddition", objInsert.Add_Addition);
        cmd.Parameters.AddWithValue("@payRemark", objInsert.Remark);
        cmd.Parameters.AddWithValue("@CTC", objInsert.CTC);
        cmd.Parameters.AddWithValue("@Gross", objInsert.Gross);
        cmd.Parameters.AddWithValue("@Net", objInsert.Net);
        try
        {
            using (con)
            {
                cmd.ExecuteScalar();
                con.Close();

            }
        }
        catch (Exception ex)
        { }

    }

    public void InsertPayProcessDetails_History(PayrollBLL objInsert_History)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_SalaryAdd", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert_History.Mode);
        cmd.Parameters.AddWithValue("@LocationID", objInsert_History.locationId);
        cmd.Parameters.AddWithValue("@empId", objInsert_History.EmpID);
        cmd.Parameters.AddWithValue("@payLoanInstl", objInsert_History.Ded_Loan);
        cmd.Parameters.AddWithValue("@payAdvance", objInsert_History.Ded_Advance);
        cmd.Parameters.AddWithValue("@payDeduction", objInsert_History.Ded_Tax);
        cmd.Parameters.AddWithValue("@payOtherDeduction", objInsert_History.Ded_Deduction);
        cmd.Parameters.AddWithValue("@payBonus", objInsert_History.Add_Bonus);
        cmd.Parameters.AddWithValue("@payAddition", objInsert_History.Add_Addition);
        cmd.Parameters.AddWithValue("@payRemark", objInsert_History.Remark);
        try
        {
            using (con)
            {
                cmd.ExecuteScalar();
                con.Close();

            }
        }
        catch (Exception ex)
        { }

    }


    public List<PayrollBLL> GetPayStatementDetails(string mode, int? PayID, string ListEmpids)
    {
        List<PayrollBLL> payroll = new List<PayrollBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Salary", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@PayID", PayID);
        cmd.Parameters.AddWithValue("@ListEmpids", ListEmpids);

        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                payroll.Add(new PayrollBLL(
                     Convert.ToInt32(reader["EmpID"]),
                     reader["Name"].ToString(),
                     Convert.ToDateTime(reader["JoiningDate"].ToString()),
                     Convert.ToDateTime(reader["Date"].ToString()),
                     Convert.ToDecimal(reader["Net"].ToString()),
                     reader["AccountNo"].ToString(),
                     Convert.ToInt32(reader["PayId"]),
                     Convert.ToInt32(reader["LocationID"]),
                     reader["IFSCCode"].ToString()
                    ));
            }
        }
        return payroll;
    }

    public void IsSuccessPayProcess(int? payId)
    {
        bool success = false;
        List<PayrollBLL> payroll = new List<PayrollBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SELECT * FROM employeePayProcessDetail WHERE payId= " + payId, con);
        SqlCommand cmd1 = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
                success = true;
            else
                success = false;
            reader.Close();
            if (success == false)
            {
                cmd1 = new SqlCommand("Delete From  employeePayProcess where Payid =" + payId, con);
                cmd1.CommandType = CommandType.Text;
                cmd1.ExecuteNonQuery();
            }
        }
    }

    public string GetEmpGender(string mode, int? EmpId)
    {
        PayrollBLL payroll = new PayrollBLL();
        string gender = "";
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Employees", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", EmpId);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                gender = reader["Gender"].ToString();
            }
        }
        return gender;
    }
}
