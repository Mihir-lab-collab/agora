using AgoraBL.BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace AgoraBL.DAL
{
    public class EmpWFHApprovalDAL
    {
        DataSet ds = null;
        DataTable dt = null;
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public DataTable GETWFHByEMPWFHID(string mode, int EmpLeaveID)
        {
            DataTable dtResult = new DataTable();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpWFHID", EmpLeaveID);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtResult);
            con.Close();
            return dtResult;
        }
        public DataSet UpdateEmpWFHStatus(string mode, string ip, EmpWFHApprovalBAL objBLL)
        {
            int sReturn = 0;
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpWFHApproval", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", objBLL.EmpID);
            cmd.Parameters.AddWithValue("@EmpWFHID", objBLL.EmpWFHID);
            cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(objBLL.WFHFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(objBLL.WFHTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@Status", objBLL.WFHStatus);
            cmd.Parameters.AddWithValue("@WFHComment", objBLL.AdminComment);
            cmd.Parameters.AddWithValue("@WFHSanctionBy", objBLL.WFHSanctionBy);
            cmd.Parameters.AddWithValue("@ip", ip);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }
    }
}
