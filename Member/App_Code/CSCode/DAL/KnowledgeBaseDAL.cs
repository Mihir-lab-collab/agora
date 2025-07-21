using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for KnowledgeBaseDAL
/// </summary>
public class KnowledgeBaseDAL
{

    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
 
    public List<KnowledgeBaseBLL> GetAllKnowledgeBase(string mode)
    {

        List<KnowledgeBaseBLL> getallknowledgebase = new List<KnowledgeBaseBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_knowledgebase", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        //cmd.Parameters.AddWithValue("@projId", projId);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getallknowledgebase.Add(new KnowledgeBaseBLL(
                    Convert.ToInt32(reader["kbId"]),
                    Convert.ToInt32(reader["empId"]),
                    reader["empName"].ToString(),
                    reader["kbDate"].ToString(),
                    reader["kbDescrptn"].ToString(),
                    reader["kbComments"].ToString(),
                    reader["kbFile"].ToString(),
                    reader["kbTitle"].ToString(),
                    Convert.ToInt32(reader["techId"]),
                    reader["techName"].ToString(),
                    reader["subtechName"].ToString(),
                    reader["projId"] != DBNull.Value && reader["projId"] != "" && reader["projId"] != "0" ? Convert.ToInt32(reader["projId"]) : 0,
                    reader["projName"].ToString(),
                    reader["Url"].ToString()
                ));
            }
            con.Close();
        }
        return getallknowledgebase;
    }

    public void deletekb(string mode, int kbId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_knowledgebase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@kbId", kbId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
    }

    public void updateKB(string mode, KnowledgeBaseBLL objKnowledge)
    {

        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("sp_knowledgebase", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@kbId", objKnowledge.kbId);
        cmd.Parameters.AddWithValue("@kbTitle", objKnowledge.kbTitle);
        cmd.Parameters.AddWithValue("@kbDescrptn", objKnowledge.kbDescrptn);
        cmd.Parameters.AddWithValue("@kbComments", objKnowledge.kbComments);
        cmd.Parameters.AddWithValue("@techId", objKnowledge.techId);
        cmd.Parameters.AddWithValue("@subtech", objKnowledge.subtechName);
        cmd.Parameters.AddWithValue("@kbFile", objKnowledge.kbFile);
        cmd.Parameters.AddWithValue("@Url", objKnowledge.Url);

        try
        {
            using (con)
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);        
        }

    }

    public int InsertKnowB(string mode, KnowledgeBaseBLL objKnowledge)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("sp_knowledgebase", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@empId", objKnowledge.empId);
        cmd.Parameters.AddWithValue("@kbDate", Convert.ToDateTime(DateTime.Now.ToLongDateString()));
        cmd.Parameters.AddWithValue("@kbTitle", objKnowledge.kbTitle);
        cmd.Parameters.AddWithValue("@kbComments", objKnowledge.kbComments);
        cmd.Parameters.AddWithValue("@kbDescrptn", objKnowledge.kbDescrptn);
        cmd.Parameters.AddWithValue("@kbFile", objKnowledge.kbFile);
        cmd.Parameters.AddWithValue("@techId", objKnowledge.techId);
         cmd.Parameters.AddWithValue("@subtech", objKnowledge.subtechName);

        if (objKnowledge.projId.ToString() == "")
        {
            if (objKnowledge.Url == "")
            {
                cmd.Parameters.AddWithValue("@projId", 0);
                cmd.Parameters.AddWithValue("@Url", 0);
            }
        }
        else
        {
            cmd.Parameters.AddWithValue("@projId", objKnowledge.projId);
            cmd.Parameters.AddWithValue("@Url", objKnowledge.Url);
        }

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
            HttpContext.Current.Response.Write(ex.Message);        
        }
        return outputid;
    }

    public List<KnowledgeBaseBLL> GetAllKB(string mode)
    {

        List<KnowledgeBaseBLL> getallknowledgebase = new List<KnowledgeBaseBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_knowledgebase", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        //cmd.Parameters.AddWithValue("@projId", projId);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getallknowledgebase.Add(new KnowledgeBaseBLL(
                    Convert.ToInt32(reader["empId"]),
                    reader["empName"].ToString()
                ));
            }
            con.Close();
        }
        return getallknowledgebase;
    }

    public List<KnowledgeBaseBLL> GetAllTech(string mode)
    {

        List<KnowledgeBaseBLL> getallknowledgebase = new List<KnowledgeBaseBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_knowledgebase", con);
        cmd.Parameters.AddWithValue("@mode", mode);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getallknowledgebase.Add(new KnowledgeBaseBLL(
                    Convert.ToInt32(reader["techId"]),
                    reader["techName"].ToString(),
                    reader["subtechname"].ToString()
                ));
            }
            con.Close();
        }
       
        return getallknowledgebase;

    }

    public List<KnowledgeBaseBLL> GetAllsubTech(string mode)
    {

        List<KnowledgeBaseBLL> getallknowledgebase = new List<KnowledgeBaseBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_knowledgebase", con);
        cmd.Parameters.AddWithValue("@mode", mode);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getallknowledgebase.Add(new KnowledgeBaseBLL(
                    Convert.ToInt32(reader["subtechId"]),
                    reader["subtechName"].ToString(),
                    reader["techId"].ToString()
                ));
            }
            con.Close();
        }
        return getallknowledgebase;
    }

    public List<KnowledgeBaseBLL> GetAllProj(string mode)
    {

        List<KnowledgeBaseBLL> getallknowledgebase = new List<KnowledgeBaseBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_knowledgebase", con);
        cmd.Parameters.AddWithValue("@mode", mode);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getallknowledgebase.Add(new KnowledgeBaseBLL(
                    Convert.ToInt32(reader["projId"]),
                    reader["projName"].ToString(),
                   Convert.ToInt32(reader["custId"])

                ));
            }
            con.Close();
        }
        return getallknowledgebase;
    }

    public List<KnowledgeBaseBLL> GetKnowledgeBaseView(string mode, int kbId)
    {

        List<KnowledgeBaseBLL> getallknowledgebase = new List<KnowledgeBaseBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("sp_knowledgebase", con);
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@kbId", kbId);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getallknowledgebase.Add(new KnowledgeBaseBLL(
                    Convert.ToInt32(reader["kbId"]),
                    Convert.ToInt32(reader["empId"]),
                    reader["empName"].ToString(),
                    reader["kbDate"].ToString(),
                    reader["kbDescrptn"].ToString(),
                    reader["kbComments"].ToString(),
                    reader["kbFile"].ToString(),
                    reader["kbTitle"].ToString(),
                    Convert.ToInt32(reader["techId"]),
                    reader["techName"].ToString(),
                    reader["subtechName"].ToString(),
                    reader["projId"] != DBNull.Value && reader["projId"] != "" && reader["projId"] != "0" ? Convert.ToInt32(reader["projId"]) : 0,
                    reader["projName"].ToString(),
                    reader["Url"].ToString()
                ));
            }
            con.Close();
        }
        return getallknowledgebase;
    }

    public int InsertCmmntHistory( KnowledgeBaseBLL objKnowledge)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
       SqlCommand cmd = new SqlCommand("SP_commentHistory", con);
       cmd.CommandType = CommandType.StoredProcedure;
       cmd.Parameters.AddWithValue("@empId", objKnowledge.empId);
       cmd.Parameters.AddWithValue("@comments", objKnowledge.commentHistory);
        cmd.Parameters.AddWithValue("@hDate",DateTime.Now.ToString());
       cmd.Parameters.AddWithValue("@kbId", objKnowledge.kbId);
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
            HttpContext.Current.Response.Write(ex.Message);
        }
        return outputid;

    }


    public List<KnowledgeBaseBLL> GetCommentHistory( int kbId)
    {
        List<KnowledgeBaseBLL> getallknowledgebase = new List<KnowledgeBaseBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_cmmntHistory", con);
       // cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@kbId", kbId);

        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                getallknowledgebase.Add(new KnowledgeBaseBLL(
                    reader["masterId"].ToString(),
                    reader["empName"].ToString(),
                    reader["comments"].ToString(),
                    reader["hDate"].ToString(),
                    Convert.ToInt32(reader["kbId"])
                ));
            }
            con.Close();
        }
        return getallknowledgebase;
    }


    public List<KnowledgeBaseBLL> GetKBAttachments(int kbId)
    {
        List<KnowledgeBaseBLL> getAttachment = new List<KnowledgeBaseBLL>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_KbAttachment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@kbId", kbId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    getAttachment.Add(new KnowledgeBaseBLL(
                        Convert.ToInt32(reader["attchmentId"]),
                        Convert.ToInt32(reader["kbId"]),
                        reader["kbFile"].ToString()                        
                        ));
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
        return getAttachment;
    }



    //public DataTable GetCommentHistory1(string mode, int kbId)
    //{
    //    SqlConnection con = new SqlConnection(_strConnection);
    //    SqlCommand cmd = new SqlCommand("SP_commentHistory", con);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    cmd.Parameters.AddWithValue("@mode", mode);
    //    cmd.Parameters.AddWithValue("@kbId", kbId);
       

    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
    //    DataTable dt = new DataTable();
    //    da.Fill(dt);

    //    return dt;
    //}

    public KnowledgeBaseDAL()
    {

    }
}