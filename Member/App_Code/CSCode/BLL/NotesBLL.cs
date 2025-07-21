using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for Notes
/// </summary>
public class NotesBLL
{
 

	public NotesBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public NotesBLL(int noteID, DateTime? insertedOn, string reff, string note, string addedBy)
    {
        // TODO: Complete member initialization
        this.noteID = noteID;
        this.insertedOn = insertedOn;
        this.reff = reff;
        this.note = note;
        this.addedBy = addedBy;
    }

    public int noteID { get; set; }
    public int noteTypeID { get; set; }
    public int refID { get; set; }
    public string reff { get; set; }
    public string noteTypeName { get; set; }
    public string note { get; set; }
    public DateTime? insertedOn { get; set; }
    public string addedBy { get; set; }
    public int insertedBy { get; set; }

    public static List<NotesBLL> GetNotes(string fromDate, string toDate, int noteTypeId, int refID,string reference)
    {
        NotesDAL objNotesDAL = new NotesDAL();
        return objNotesDAL.GetNotes(fromDate, toDate, noteTypeId, refID, reference);
    }


    public static List<NotesBLL> BindReference(string mode,int type,int refId)
    {
        NotesDAL objType = new NotesDAL();
        return objType.BindReference(mode, type, refId);
    }


    public static int SaveNote(string strMode,int noteTypeId,int refId,string note,int insertedBy)
    {
        NotesDAL ObjNotesDAL=new NotesDAL();
        int output = 0;
        try
        {          
            output = ObjNotesDAL.SaveNote(strMode, noteTypeId, refId, note, insertedBy);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return output;
    }


    public DataTable GetNoteType(string mode, int noteTypeId)
    {
        NotesDAL ObjNoteDal=new NotesDAL();
       DataTable dt = ObjNoteDal.GetNoteType(mode, noteTypeId);
       return dt;
    
    }

    public static string GetNoteTypeId(string mode, string noteType)
    {
        try
        {          
            return new NotesDAL().GetNoteTypeId(mode, noteType);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }


    public void SetAllNoteType()
    {
        string mode = "GetAllNoteTypeId";
        NotesDAL ObjNoteDal = new NotesDAL();
        DataTable dt = ObjNoteDal.GetAllNoteType(mode);
        if (dt != null && dt.Rows.Count > 0)
        {
            HttpContext.Current.Session["EmpTypeId"] = Convert.ToInt16(dt.Rows[0]["ID"]);
            HttpContext.Current.Session["CustomerTypeId"] = Convert.ToInt16(dt.Rows[1]["ID"]);
            HttpContext.Current.Session["ProjectTypeId"] = Convert.ToInt16(dt.Rows[2]["ID"]);
        }
        else
        {
            HttpContext.Current.Session["EmpTypeId"] = 0;
            HttpContext.Current.Session["CustomerTypeId"] = 0;
            HttpContext.Current.Session["ProjectTypeId"] = 0;
        }

    }

}