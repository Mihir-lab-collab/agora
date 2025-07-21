using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;


/// <summary>
/// Summary description for Notes
/// </summary>
public class NotesDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public NotesDAL()
    {
    }


    public List<NotesBLL> GetNotes(string fromDate, string toDate, int noteTypeId, int refID, string reference)
    {

        List<NotesBLL> lstNotes = new List<NotesBLL>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("GenericNotes", con);
            cmd.CommandType = CommandType.StoredProcedure;     
            cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(fromDate, "dd/MM/yyyy", null));          
            cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(toDate, "dd/MM/yyyy", null));
               cmd.Parameters.AddWithValue("@Type", noteTypeId);
               cmd.Parameters.AddWithValue("@RefID", refID);
               cmd.Parameters.AddWithValue("@Reference", reference);
            cmd.Parameters.AddWithValue("@Mode", "Select");
            SqlDataReader reader = null;
            Nullable<DateTime> dt = null;

            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    NotesBLL obj = new NotesBLL();
                    obj.noteID= Convert.ToInt32(reader["NoteID"].ToString());
                    obj.insertedOn=  string.IsNullOrEmpty(Convert.ToString(reader["InsertedOn"])) ? dt : Convert.ToDateTime(reader["InsertedOn"]);
                    obj.noteTypeID = Convert.ToInt32(reader["NoteTypeID"].ToString());
                   
                    obj.reff= reader["Reff"].ToString();
                    obj.note= reader["Note"].ToString();
                    obj.addedBy=reader["AddedBy"].ToString() ;
                    lstNotes.Add(obj);
                }
            }
             return lstNotes;
        }
        catch (Exception ex)
        { }
        return lstNotes;
    }



    public List<NotesBLL> BindReference(string mode,int type,int refId)
    {
        List<NotesBLL> Reference = new List<NotesBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("GenericNotes", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", mode);
        cmd.Parameters.AddWithValue("@Type", type);
        cmd.Parameters.AddWithValue("@RefID", refId);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                NotesBLL obj = new NotesBLL();
                obj.refID = Convert.ToInt32(Dr["ID"]);
                obj.reff = Convert.ToString(Dr["Name"]);
                Reference.Add(obj);
            }
        }
        return Reference;
    }


    public int SaveNote(string strMode,int noteTypeId,int refId,string note,int insertedBy)
    {
        int outputid = 0;
        try
        {
           
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("GenericNotes", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", strMode);
            cmd.Parameters.AddWithValue("@NoteTypeID", noteTypeId);
            cmd.Parameters.AddWithValue("@RefID", refId);
            cmd.Parameters.AddWithValue("@Note", note);
            cmd.Parameters.AddWithValue("@InsertedBy", insertedBy);
          
            using (con)
            {
               //// outputid = Convert.ToInt32(cmd.ExecuteNonQuery());
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
              
                con.Close();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return outputid;
    }


   

    public DataTable GetNoteType(string mode, int noteTypeId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                SqlCommand cmd = new SqlCommand("GenericNotes", con);
                // cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@NoteTypeID", noteTypeId);
               
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // ds = new DataSet();
                da.Fill(dt);
                con.Close();
                return dt;
            }
        }
        catch (Exception ex)
        {
            return dt;
        }
    }


    public string GetNoteTypeId(string mode, string noteType)
    {
        string output = string.Empty;
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("GenericNotes", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@TypeName", noteType);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        output = Convert.ToString(reader["ID"]);
                    }
                }
                else
                    output = "0";

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return output;
    }



    public DataTable GetAllNoteType(string mode)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                SqlCommand cmd = new SqlCommand("GenericNotes", con);
                // cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Mode", mode);            
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // ds = new DataSet();
                da.Fill(dt);
                con.Close();
                return dt;
            }
        }
        catch (Exception ex)
        {
            return dt;
        }
    }
   
}