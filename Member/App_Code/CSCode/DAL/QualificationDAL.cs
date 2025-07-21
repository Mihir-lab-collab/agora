using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for QualificationDAL
/// </summary>
public class QualificationDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public static string IPAddress
    {
        get
        {
            return Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        }
    }
    public QualificationDAL()
    {
    }
    public List<QualificationBLL> GetQualificationDetails(string mode, int QID)
    {
        List<QualificationBLL> lstQual = new List<QualificationBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Qualification", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@QID", QID);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstQual.Add(new QualificationBLL(
                     reader["qualificationId"].ToString() == "" ? 0 : Convert.ToInt32(reader["qualificationId"].ToString()),
                     reader["qualificationDesc"].ToString() == "" ? "" : Convert.ToString(reader["qualificationDesc"].ToString()),
                    reader["qualificationType"].ToString() == "" ? "" : Convert.ToString(reader["qualificationType"].ToString()),
                    reader["modifiedBy"].ToString() == "" ? 0 : Convert.ToInt32(reader["ModifiedBy"].ToString())

                    ));
            }
        }
        return lstQual;
    }

     public List<QualificationBLL> GetQualification(string mode)
    {
        List<QualificationBLL> lstQual = new List<QualificationBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Qualification", con);
        cmd.Parameters.AddWithValue("@mode", mode);
       
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstQual.Add(new QualificationBLL(
                     reader["qualificationId"].ToString() == "" ? 0 : Convert.ToInt32(reader["qualificationId"].ToString()),
                     reader["qualificationDesc"].ToString() == "" ? "" : Convert.ToString(reader["qualificationDesc"].ToString())
                   

                    ));
            }
        }
        return lstQual;
    }




    public int InsertQualification(QualificationBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("SP_Qualification", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@QID", objInsert.QID);
        cmd.Parameters.AddWithValue("@QDesp", objInsert.QualDesc);
        cmd.Parameters.AddWithValue("@QType", objInsert.QualType);
        cmd.Parameters.AddWithValue("@IpAddress", IPAddress);
        cmd.Parameters.AddWithValue("@UserID", objInsert.insertedBy);
        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }

        }
        catch (Exception ex)
        { }
        return outputid;
    }
    public bool UpdateQualification(QualificationBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_Qualification", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@QID", objInsert.QID);
        cmd.Parameters.AddWithValue("@QDesp", objInsert.QualDesc);
        cmd.Parameters.AddWithValue("@QType", objInsert.QualType);
        cmd.Parameters.AddWithValue("@IpAddress", IPAddress);
        cmd.Parameters.AddWithValue("@UserID", objInsert.modifiedBy);
        try
        {
            using (con)
            {
                con.Open();
                int output = Convert.ToInt32(cmd.ExecuteScalar());
                if (output == 1)
                    updated = true;
            }
        }
        catch (Exception ex)
        { }
        return updated;
    }
  
}