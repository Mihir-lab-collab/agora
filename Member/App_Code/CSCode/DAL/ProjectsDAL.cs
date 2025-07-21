using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Customer.BLL;
using System.Data.SqlClient;
using System.Data;
using Customer.BLL;
using System.Configuration;
namespace Customer.DAL
{
    /// <summary>
    /// Summary description for CustUsersDAL
    /// </summary>
    public class ProjectsDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        public ProjectsDAL()
        {
        }

        public List<Projects> CustomerProjectList(int CustomerId)
        {
            List<Projects> curprojects = new List<Projects>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_Projects", con);//sp_CustomerProjectList
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@custId", CustomerId);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curprojects.Add(new Projects(
                        Convert.ToInt32(reader["projId"]),
                        Convert.ToInt32(reader["custId"]),
                        reader["projName"].ToString(),
                        reader["projDesc"].ToString(),
                        Convert.ToInt32(reader["projManager"].ToString()),
                        reader["projStartDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projStartDate"].ToString()),
                        Convert.ToDecimal(reader["projDuration"].ToString()),
                        reader["projActComp"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projActComp"].ToString()),
                        Convert.ToInt16(reader["currID"].ToString()),
                        Convert.ToDecimal(reader["currExRate"].ToString()),
                        Convert.ToDecimal(reader["projCost"].ToString()),
                        reader["projProcMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projProcMonth"].ToString()),
                        Convert.ToInt16(reader["noOfPayments"].ToString()),
                        reader["lastPaymentMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["lastPaymentMonth"].ToString()),
                        reader["codeRevTeam"].ToString(),
                        Convert.ToBoolean(reader["allowTSEmployee"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        reader["OtherEmailId"].ToString(),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                        Convert.ToBoolean(reader["IsSendMail"].ToString())
                        ));
                }
            }
            return curprojects;
        }

        public List<Projects> GetProjectList(int EmpID, int isAdmin)
        {
            List<Projects> curprojects = new List<Projects>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_MemberProjects", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curprojects.Add(new Projects(
                        Convert.ToInt32(reader["projId"]),
                        Convert.ToInt32(reader["custId"]),
                        reader["projName"].ToString(),
                        reader["projDesc"].ToString(),
                        Convert.ToInt32(reader["projManager"].ToString()),
                        reader["projStartDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projStartDate"].ToString()),
                        Convert.ToDecimal(reader["projDuration"].ToString()),
                        reader["projActComp"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projActComp"].ToString()),
                        Convert.ToInt16(reader["currID"].ToString()),
                        Convert.ToDecimal(reader["currExRate"].ToString()),
                        Convert.ToDecimal(reader["projCost"].ToString()),
                        reader["projProcMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projProcMonth"].ToString()),
                        Convert.ToInt16(reader["noOfPayments"].ToString()),
                        reader["lastPaymentMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["lastPaymentMonth"].ToString()),
                        reader["codeRevTeam"].ToString(),
                        Convert.ToBoolean(reader["allowTSEmployee"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        reader["OtherEmailId"].ToString(),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                       Convert.ToBoolean(reader["IsSendMail"].ToString())
                        ));
                }
            }
            return curprojects;
        }

        public List<Projects> CustomerUserProjectList(int CustomerId, int UserId)
        {
            List<Projects> curprojects = new List<Projects>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_EmployeeUserProjectList", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@custId", CustomerId);
            cmd.Parameters.AddWithValue("@UserId", UserId);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curprojects.Add(new Projects(
                        Convert.ToInt32(reader["projId"]),
                        Convert.ToInt32(reader["custId"]),
                        reader["projName"].ToString(),
                        reader["projDesc"].ToString(),
                        Convert.ToInt32(reader["projManager"].ToString()),
                        reader["projStartDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projStartDate"].ToString()),
                        Convert.ToDecimal(reader["projDuration"].ToString()),
                        reader["projActComp"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projActComp"].ToString()),
                        Convert.ToInt16(reader["currID"].ToString()),
                        Convert.ToDecimal(reader["currExRate"].ToString()),
                        Convert.ToDecimal(reader["projCost"].ToString()),
                        reader["projProcMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projProcMonth"].ToString()),
                        Convert.ToInt16(reader["noOfPayments"].ToString()),
                        reader["lastPaymentMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["lastPaymentMonth"].ToString()),
                        reader["codeRevTeam"].ToString(),
                        Convert.ToBoolean(reader["allowTSEmployee"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        reader["OtherEmailId"].ToString(),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                        Convert.ToBoolean(reader["IsSendMail"].ToString())
                        ));
                }
            }
            return curprojects;
        }
        public Projects GetCustomerProjectbyProjId(int projId)
        {
            Projects curproject = new Projects();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetCustomerProjectbyProjId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projId", projId);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curproject = new Projects(
                        Convert.ToInt32(reader["projId"]),
                        Convert.ToInt32(reader["custId"]),
                        reader["projName"].ToString(),
                        reader["projDesc"].ToString(),
                        Convert.ToInt32(reader["projManager"].ToString()),
                        reader["projStartDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projStartDate"].ToString()),
                        Convert.ToDecimal(reader["projDuration"].ToString()),
                        reader["projActComp"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projActComp"].ToString()),
                        Convert.ToInt16(reader["currID"].ToString()),
                        Convert.ToDecimal(reader["currExRate"].ToString()),
                        Convert.ToDecimal(reader["projCost"].ToString()),
                        reader["projProcMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projProcMonth"].ToString()),
                        Convert.ToInt16(reader["noOfPayments"].ToString()),
                        reader["lastPaymentMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["lastPaymentMonth"].ToString()),
                        reader["codeRevTeam"].ToString(),
                        Convert.ToBoolean(reader["allowTSEmployee"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        reader["OtherEmailId"].ToString(),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                        Convert.ToBoolean(reader["IsSendMail"].ToString())
                        );
                }
            }
            return curproject;
        }
        public List<Projects> GetProjectsAssignedToUser(int UserMasterID)
        {
            List<Projects> curprojects = new List<Projects>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetProjectsAssignedToUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserMasterID", UserMasterID);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curprojects.Add(new Projects(
                        Convert.ToInt32(reader["projId"]),
                        Convert.ToInt32(reader["custId"]),
                        reader["projName"].ToString(),
                        reader["projDesc"].ToString(),
                        Convert.ToInt32(reader["projManager"].ToString()),
                        reader["projStartDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projStartDate"].ToString()),
                        Convert.ToDecimal(reader["projDuration"].ToString()),
                        reader["projActComp"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projActComp"].ToString()),
                        Convert.ToInt16(reader["currID"].ToString()),
                        Convert.ToDecimal(reader["currExRate"].ToString()),
                        Convert.ToDecimal(reader["projCost"].ToString()),
                        reader["projProcMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["projProcMonth"].ToString()),
                        Convert.ToInt16(reader["noOfPayments"].ToString()),
                        reader["lastPaymentMonth"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["lastPaymentMonth"].ToString()),
                        reader["codeRevTeam"].ToString(),
                        Convert.ToBoolean(reader["allowTSEmployee"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        reader["OtherEmailId"].ToString(),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                        Convert.ToBoolean(reader["IsSendMail"].ToString())
                        
                        ));
                }
            }
            return curprojects;
        }

        public List<Projects> ProjectStatus(string mode)
        {
            List<Projects> curprojects = new List<Projects>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Project", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curprojects.Add(new Projects(
                        Convert.ToInt32(reader["projStatusTId"]),
                        reader["projStatusTDesc"].ToString()
                        ));
                }
            }
            return curprojects;
        }
    }
}