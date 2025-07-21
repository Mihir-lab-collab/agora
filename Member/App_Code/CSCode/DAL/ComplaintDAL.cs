using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for complaintsDAL
/// </summary>
public class ComplaintDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public List<ComplaintBLL> GetAllComplaints(string mode, int projId)
    {

        List<ComplaintBLL> getallcomplaints = new List<ComplaintBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_GetComplaint", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@projId", projId);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getallcomplaints.Add(new ComplaintBLL(
                (reader["compDate"].ToString()),
                Convert.ToInt32(reader["compId"]),
                reader["projName"].ToString(),
                reader["compTitle"].ToString(),
                reader["compResolved"].ToString(),
                reader["compCategory"].ToString(),
                reader["custName"].ToString(),
                reader["custRegDate"].ToString(),
                reader["custEmail"].ToString(),
                reader["custAddress"].ToString(),
                reader["custCompany"].ToString(),
                Convert.ToInt32(reader["projId"]),
                reader["compDesc"].ToString(), Convert.ToInt32(reader["ID"]),
                reader["compFeedback"].ToString()
                ));


            }
            con.Close();

        }
        return getallcomplaints;
    }

    public void DeleteComplaint(string mode, int compId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_GetComplaint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@compId", compId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {

        }
    }

    public List<ComplaintBLL> getCategoryDetail(string mode)
    {
        List<ComplaintBLL> getCategory = new List<ComplaintBLL>();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {

                SqlCommand cmd = new SqlCommand("SP_GetComplaint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", mode);


                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        getCategory.Add(new ComplaintBLL(
                        Convert.ToInt32(reader["ID"]),
                        reader["compCategory"].ToString()
                       ));


                    }
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {

        }
        return getCategory;
    }


    public void SaveComplaint(string mode, ComplaintBLL objComplaint)
    {

        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_GetComplaint", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@compId", objComplaint.compId);
        cmd.Parameters.AddWithValue("@compcategory", objComplaint.compCategory);
        if (objComplaint.compResolved == "Yes" || objComplaint.compResolved == "yes" || objComplaint.compResolved == "1")
        {
            cmd.Parameters.AddWithValue("@resolve", 1);
        }
        else
        {
            cmd.Parameters.AddWithValue("@resolve", 0);
        }
        cmd.Parameters.AddWithValue("@compFeedback", objComplaint.compFeedback);


        try
        {
            using (con)
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }
        catch (Exception ex)
        { }

    }

    public ComplaintDAL()
    {

    }
}