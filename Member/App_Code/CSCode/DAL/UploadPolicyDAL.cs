using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Customer.BLL;

/// <summary>
/// Summary description for UploadPolicyDAL
/// </summary>
public class UploadPolicyDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public UploadPolicyDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int SaveUploadPolicyFile(string mode, UploadPolicyBLL objUploadPolicyBLL)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("USP_HrPolicy", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@FileName", objUploadPolicyBLL.FileName);
        cmd.Parameters.AddWithValue("@CustomFileName", objUploadPolicyBLL.CustomFileName);
        cmd.Parameters.AddWithValue("@FileUploadPath", objUploadPolicyBLL.FileUploadPath);        
        cmd.Parameters.AddWithValue("@DisplayFileURL", objUploadPolicyBLL.DisplayFileURL);
        cmd.Parameters.AddWithValue("@FileTypeId", objUploadPolicyBLL.FileTypeId);
        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

            }

        }
        catch (Exception ex)
        {
            outputid = 2; // added outputid =2 to know something went wrong
        }
        return outputid;
    }
    public List<UploadPolicyBLL> GetHrPolicyFilesDetails(string mode)
    {
        List<UploadPolicyBLL> objPolicyBLL = new List<UploadPolicyBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("USP_HrPolicy", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@FileName", "");
        cmd.Parameters.AddWithValue("@CustomFileName", "");
        cmd.Parameters.AddWithValue("@FileUploadPath", "");
        cmd.Parameters.AddWithValue("@DisplayFileURL", "");
        cmd.Parameters.AddWithValue("@FileTypeId", "");
        SqlDataReader dr = null;
        using (con)
        {
            con.Open();

            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            dr = cmd.ExecuteReader();
            int i = 0;
            while(dr.Read())
            {
                UploadPolicyBLL obj = new UploadPolicyBLL();
                obj.FileName = dr["FileName"].ToString();
                obj.CustomFileName = dr["CustomFileName"].ToString();
                obj.FileUploadPath = dr["FileUploadPath"].ToString();
                obj.DisplayFileURL = dr["DisplayFileURL"].ToString();
                obj.IsActive = Convert.ToInt32(dr["IsActive"]);
                obj.CreateDate = Convert.ToDateTime(dr["CreateDate"]);
                objPolicyBLL.Add(obj);
                i++;

            }
        }
        return objPolicyBLL;
    }
}