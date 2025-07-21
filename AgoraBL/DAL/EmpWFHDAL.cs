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
    public class EmpWFHDAL
    {
        DataSet ds = null;
        DataTable dt = null;
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public EmpWFHDAL()
        {

        }
        public DataSet GetWFHList(string mode, int empid, string lStatus, int year)//string leaveMonth )
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            // cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.Parameters.AddWithValue("@Status", lStatus);
            cmd.Parameters.AddWithValue("@Year", year);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public DataSet SaveWFH(string mode, int empid, EmpWFHBAL obj)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.Parameters.AddWithValue("@Status", "0");
            cmd.Parameters.AddWithValue("@Month", "");
            cmd.Parameters.AddWithValue("@WFHFrom", DateTime.ParseExact(obj.WFHFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@WFHTo", DateTime.ParseExact(obj.WFHTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@WFHDesc", obj.WFHDesc);
            cmd.Parameters.AddWithValue("@WFHCount", obj.WFHCount);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public int DeleteWFH(string mode, int empWFHID)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", 0);
            cmd.Parameters.AddWithValue("@Status", "0");
            cmd.Parameters.AddWithValue("@Month", "");
            cmd.Parameters.AddWithValue("@WFHFrom", "");
            cmd.Parameters.AddWithValue("@WFHTo", "");
            cmd.Parameters.AddWithValue("@WFHDesc", "");
            cmd.Parameters.AddWithValue("@empWFHId", empWFHID);
            cmd.CommandType = CommandType.StoredProcedure;
            int ireturn = cmd.ExecuteNonQuery();
            con.Close();
            return ireturn;
        }

        public bool IfExistsWFH(string mode, int empid, string WFHFrom, string WFHTo)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.Parameters.AddWithValue("@WFHFrom", DateTime.ParseExact(WFHFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@WFHTo", DateTime.ParseExact(WFHTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr;
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
                return true;
            else
                return false;
            con.Close();

        }
        public DataSet AppliedWFHFromTo(string mode, int empid, string From, string To)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@empid", empid);
            cmd.Parameters.AddWithValue("@WFHFrom", From);
            cmd.Parameters.AddWithValue("@WFHTo", To);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }
        public int InsertWFHAttendance(string mode, int empId)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empId);
            cmd.CommandType = CommandType.StoredProcedure;
            int ireturn = cmd.ExecuteNonQuery();
            con.Close();
            return ireturn;
        }

        public int InsertRAAttendance(string mode, int empId)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpRemoteAttendance", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empId);
            cmd.CommandType = CommandType.StoredProcedure;
            int ireturn = cmd.ExecuteNonQuery();
            con.Close();
            return ireturn;
        }

        public int UpdateRAAttendance(string mode, int empId, DateTime attDate)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpRemoteAttendance", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empId);
            cmd.Parameters.AddWithValue("@attDate", attDate);
            cmd.CommandType = CommandType.StoredProcedure;
            int ireturn = cmd.ExecuteNonQuery();
            con.Close();
            return ireturn;
        }
        public int CheckAttendanceExistence(int empId, DateTime attDate)
        {
            int recordCount = 0; // Declare variable outside
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EmpRemoteAttendance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "IsStartDayClicked");
                    cmd.Parameters.AddWithValue("@EmpID", empId);
                    cmd.Parameters.AddWithValue("@attDate", attDate);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int.TryParse(result.ToString(), out recordCount);
                    }
                }
            }

            return recordCount;
        }




        public int UpdateWFHAttendance(string mode, int empId, DateTime attOutTime, DateTime attDate)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empId);
            cmd.Parameters.AddWithValue("@attOutTime", attOutTime);
            cmd.Parameters.AddWithValue("@attDate", attDate);
            cmd.CommandType = CommandType.StoredProcedure;
            int ireturn = cmd.ExecuteNonQuery();
            con.Close();
            return ireturn;
        }
        public bool CheckInTime(string mode, int empid)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr;
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
                return true;
            else
                return false;
            con.Close();

        }
        public DataTable BindWFHBalance(int empid)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
            cmd.Parameters.AddWithValue("@Mode", "WFHBalance");
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable GetLeaveStatus(int empid)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_EmpRemoteAttendance", con);
            cmd.Parameters.AddWithValue("@Mode", "GetLeaveStatus");
            cmd.Parameters.AddWithValue("@EmpID", empid);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable GetWFHDetailById(int wfhId)
        {
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
                cmd.Parameters.AddWithValue("@Mode", "GetWFHDetailById");
                cmd.Parameters.AddWithValue("@EmpWFHID", wfhId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable IfExistsWFHData(string mode, int empid, string WFHFrom, string WFHTo)
        {
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@EmpID", empid);
                cmd.Parameters.AddWithValue("@WFHFrom", DateTime.ParseExact(WFHFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@WFHTo", DateTime.ParseExact(WFHTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public DataTable UpdateWFHStatus(int empID, string wfhId, string WFHStatus, string WFHComment, string SanctionedBy, string WFHFrom, string WFHTo)
        {
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_EmpWFH", con);
                cmd.Parameters.AddWithValue("@Mode", "UpdateWFHStatus");
                cmd.Parameters.AddWithValue("@EmpID", empID);
                cmd.Parameters.AddWithValue("@EmpWFHID", wfhId);
                cmd.Parameters.AddWithValue("@Status", WFHStatus);
                cmd.Parameters.AddWithValue("@WFHComment", WFHComment);
                cmd.Parameters.AddWithValue("@WFHSanctionBy", SanctionedBy);
                cmd.Parameters.AddWithValue("@WFHFrom", DateTime.ParseExact(WFHFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@WFHTo", DateTime.ParseExact(WFHTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void AddWFHNotification(int wfhId, string empId, string channelName, string channelUniqueId, string channelNotificationUniqueId)
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EmpWFHNotification", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "INSERT");
                    cmd.Parameters.AddWithValue("@wfhId", wfhId);
                    cmd.Parameters.AddWithValue("@empid", empId);
                    cmd.Parameters.AddWithValue("@ChannelName", channelName);
                    cmd.Parameters.AddWithValue("@ChannelUniqueId", channelUniqueId);
                    cmd.Parameters.AddWithValue("@ChannelNotificationUniqueId", channelNotificationUniqueId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (ex)
                        throw;
                    }
                }
            }
        }

    }
}
