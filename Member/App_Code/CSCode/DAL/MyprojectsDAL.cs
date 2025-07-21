using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;


namespace Customer.DAL
{
    public class MyprojectsDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        String[] arr_string;
        private string sqlquery, empname;

        public MyprojectsDAL()
        {

        }


        public List<MyprojectsBLL> GetMycoderevteam(string empid)
        {
            List<MyprojectsBLL> CurMycodeteam = new List<MyprojectsBLL>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd;
            SqlDataReader rdr = null;
            using (con)
            {
                con.Open();
                arr_string = empid.Split(',');
                for (int i = 0; i < arr_string.Length - 1; i++)
                {
                    sqlquery = "select empname  from employeemaster where  empid ='" + arr_string[i].ToString();
                    cmd = new SqlCommand(sqlquery, con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (empname == "")
                        { empname = "- " + rdr["empname"].ToString(); }
                        else { empname += "<BR> " + rdr["empname"].ToString(); }

                    }

                }
                CurMycodeteam.Add(new MyprojectsBLL(
                      empname
                      ));
            }
            return CurMycodeteam;

        }


        public List<MyprojectsBLL> GetMyProjId(int empid, Boolean IsAdmin, string include)
        {
            List<MyprojectsBLL> CurMyProjId = new List<MyprojectsBLL>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetMyProjects", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpId", empid);
            cmd.Parameters.AddWithValue("@IsAdmin", IsAdmin);
            cmd.Parameters.AddWithValue("@IsCompleted", include);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CurMyProjId.Add(new MyprojectsBLL(
                        reader["projId"].ToString() == "" ? 0 : Convert.ToInt32(reader["projid"]),
                        reader["custId"].ToString() == "" ? 0 : Convert.ToInt32(reader["custId"]),
                        reader["projName"].ToString() == "" ? "" : Convert.ToString(reader["projName"].ToString()),
                        reader["projDesc"].ToString() == "" ? "" : Convert.ToString(reader["projDesc"].ToString()),
                        reader["projManager"].ToString() == "" ? 0 : Convert.ToInt32(reader["projManager"]),
                        reader["projStartDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["projStartDate"].ToString()),
                        reader["custName"].ToString() == "" ? "" : Convert.ToString(reader["custName"].ToString()),
                        reader["empName"].ToString() == "" ? "" : Convert.ToString(reader["empName"].ToString()),
                        reader["AccountManager"].ToString() == "" ? "" : Convert.ToString(reader["AccountManager"].ToString()),
                        reader["projExpComp"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["projExpComp"].ToString()),
                        reader["projActComp"].ToString(),
                        reader["projRemark"].ToString() == "" ? "No Remark" : reader["projRemark"].ToString(),
                        //  Convert.ToDateTime(reader["projActComp"].ToString()),
                        reader["projStatusTDesc"].ToString() == "" ? "To Be Started" : Convert.ToString(reader["projStatusTDesc"].ToString()),
                        reader["ExpCompleted"].ToString() == "" ? "" : Convert.ToString(reader["ExpCompleted"].ToString()),
                        reader["devid"].ToString() == "" ? "" : Convert.ToString(reader["devid"].ToString()),
                        reader["codeRevTeam"].ToString() == "" ? "" : Convert.ToString(reader["codeRevTeam"].ToString()),
                        reader["projStatus1"].ToString() == "" ? "" : Convert.ToString(reader["projStatus1"].ToString()),
                         reader["projStatusActive"].ToString() == "" ? 0 : Convert.ToInt32(reader["projStatusActive"]),
                         reader["projStatusTId"].ToString() == "" ? 0 : Convert.ToInt32(reader["projStatusTId"])
                        ));
                }
            }
            return CurMyProjId;
        }





        //Added by trupti for ProjectStatus Details in BI Report Page

        public List<MyprojectsBLL> GetProjStatusDetails(int ProjId)
        {
            List<MyprojectsBLL> CurMyProjId = new List<MyprojectsBLL>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_GetProjectStatusDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projid", ProjId);

            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    CurMyProjId.Add(new MyprojectsBLL(
                        reader["projId"].ToString() == "" ? 0 : Convert.ToInt32(reader["projid"]),
                        reader["custId"].ToString() == "" ? 0 : Convert.ToInt32(reader["custId"]),
                        reader["projName"].ToString() == "" ? "" : Convert.ToString(reader["projName"].ToString()),
                        reader["projDesc"].ToString() == "" ? "" : Convert.ToString(reader["projDesc"].ToString()),
                        reader["projManager"].ToString() == "" ? 0 : Convert.ToInt32(reader["projManager"]),
                        reader["projStartDate"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["projStartDate"].ToString()),
                        reader["custName"].ToString() == "" ? "" : Convert.ToString(reader["custName"].ToString()),
                        reader["empName"].ToString() == "" ? "" : Convert.ToString(reader["empName"].ToString()),
                        reader["AccountManager"].ToString() == "" ? "" : Convert.ToString(reader["AccountManager"].ToString()),
                        reader["projExpComp"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["projExpComp"].ToString()),
                        reader["projActComp"].ToString(),
                        reader["projRemark"].ToString() == "" ? "No Remark" : reader["projRemark"].ToString(),
                        //  Convert.ToDateTime(reader["projActComp"].ToString()),
                        reader["projStatusTDesc"].ToString() == "" ? "To Be Started" : Convert.ToString(reader["projStatusTDesc"].ToString()),
                        reader["ExpCompleted"].ToString() == "" ? "" : Convert.ToString(reader["ExpCompleted"].ToString()),
                        reader["devid"].ToString() == "" ? "" : Convert.ToString(reader["devid"].ToString()),
                        reader["codeRevTeam"].ToString() == "" ? "" : Convert.ToString(reader["codeRevTeam"].ToString()),
                        reader["projStatus1"].ToString() == "" ? "" : Convert.ToString(reader["projStatus1"].ToString()),
                         reader["projStatusActive"].ToString() == "" ? 0 : Convert.ToInt32(reader["projStatusActive"]),
                         reader["projStatusTId"].ToString() == "" ? 0 : Convert.ToInt32(reader["projStatusTId"])
                        ));
                }
            }
            return CurMyProjId;
        }


    }
}



