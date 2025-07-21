using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;


public partial class Member_Notes : Authentication
{
    string modeGlobal = string.Empty;
    int noteTypeIDGlobal = 0;
    int refIDGlobal = 0;
    string noteGlobal = string.Empty;
    int insertedByGlobal = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    static UserMaster UM;


    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (UM.IsAdmin == true || UM.IsModuleAdmin == true)
        {
            SpanAddNote.Style.Add("display", "block");
        }
        else
        {
            SpanAddNote.Style.Add("display", "none");
        }

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["TypeId"]))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["TypeId"]))
                {
                    Session["NoteType"] = Convert.ToInt32(Request.QueryString["TypeId"]);
                }
            }
            else
            {
               
                Session["NoteType"] = Convert.ToString(HttpContext.Current.Session["EmpTypeId"]);

            }

            FillNoteType();
            if (!string.IsNullOrEmpty(Request.QueryString["RefID"]))
            {
                Session["RefID"] = Convert.ToInt32(Request.QueryString["RefID"]);
            }
            else
            {
                Session["RefID"] = "0";
            }
            BindReference();
        }
    }

    [System.Web.Services.WebMethod]
    public static Data BindNotes(string fromDate, string toDate, string noteTypeId, string refID, string reference)
    {
        DateTime Fromdate = DateTime.Today;
        Fromdate = Fromdate.AddMonths(-1);
        string frmoDateNew = Fromdate.ToString("dd/MM/yyyy");
        DateTime now = DateTime.Now;
        string ToDateNew = now.ToString("dd/MM/yyyy");

        Data data = new Data();
        if (string.IsNullOrEmpty(fromDate))
        {
            fromDate = Convert.ToString(HttpContext.Current.Session["fromDate"]);
        }

        if (string.IsNullOrEmpty(toDate))
        {
            toDate = Convert.ToString(HttpContext.Current.Session["toDate"]);
        }

        if (string.IsNullOrEmpty(reference))
        {
            reference = Convert.ToString(HttpContext.Current.Session["Reference"]);
        }

        List<NotesBLL> lstNotes = new List<NotesBLL>();

        try
        {
            HttpContext.Current.Session["fromDate"] = string.Empty;
            HttpContext.Current.Session["toDate"] = string.Empty;
            HttpContext.Current.Session["Reference"] = string.Empty;

            lstNotes = NotesBLL.GetNotes(string.IsNullOrEmpty(fromDate) ? frmoDateNew : fromDate, string.IsNullOrEmpty(toDate) ? ToDateNew : toDate, string.IsNullOrEmpty(noteTypeId) ? 0 : Convert.ToInt16(noteTypeId), string.IsNullOrEmpty(refID) ? 0 : Convert.ToInt16(refID), reference);
            if (lstNotes.Count > 0)
            {
                HttpContext.Current.Session["fromDate"] = fromDate;
                HttpContext.Current.Session["toDate"] = toDate;
                HttpContext.Current.Session["Reference"] = reference;

                data.Reference = Convert.ToString(HttpContext.Current.Session["Reference"]);
                data.Fromdate = Convert.ToString(HttpContext.Current.Session["fromDate"]);
                data.Todate = Convert.ToString(HttpContext.Current.Session["toDate"]);
            }
            else
            {
                //if (lstNotes.Count > 0)
                //{
                //    if (string.IsNullOrEmpty(fromDate))
                //    {
                //        data.Fromdate = string.Empty;
                //    }
                //    else
                //    {
                //        data.Fromdate = fromDate;
                //    }
                //}


                if (string.IsNullOrEmpty(reference))
                {
                    data.Reference = string.Empty;
                }
                else
                {
                    data.Reference = reference;
                }

                if (string.IsNullOrEmpty(fromDate))
                {
                    data.Fromdate = string.Empty;
                }
                else
                {
                    data.Fromdate = fromDate;
                }
                if (string.IsNullOrEmpty(toDate))
                {
                    data.Todate = string.Empty;
                }
                else
                {
                    data.Todate = toDate;
                }
            }
            data.lst = lstNotes;
            return data;
        }
        catch (Exception ex)
        {
            lstNotes.Clear();
            return data;
        }
        //// return data;
    }
    public void BindReference()
    {
        List<NotesBLL> lstReference = NotesBLL.BindReference("GetReference", Convert.ToInt32(Session["NoteType"]), Convert.ToInt32(Session["RefID"]));
        ddlReference.DataSource = lstReference;
        ddlReference.DataValueField = "refID";
        ddlReference.DataTextField = "reff";
        ddlReference.DataBind();
        ddlReference.Items.Insert(0, new ListItem("--Select Reference--", "0"));
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;
            modeGlobal = "InsertNote";

            noteTypeIDGlobal = Convert.ToInt32(Session["NoteType"]);

            refIDGlobal = Convert.ToInt32(ddlReference.SelectedValue);
            noteGlobal = Convert.ToString(txtNotes.Text).Trim();
            insertedByGlobal = UM.EmployeeID;
            NotesBLL objNotesBLL = new NotesBLL();
            result = NotesBLL.SaveNote(modeGlobal, noteTypeIDGlobal, refIDGlobal, noteGlobal, insertedByGlobal);
            if (result > 0)
            {
                Clear();
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Save", "<script>alert('Record saved successfully.');</script>", false);
            }
        }
        catch (Exception ex)
        {

        }

    }
    
    public void FillNoteType()
    {
        DataTable dt = new DataTable();
        NotesBLL ObjNoteBll = new NotesBLL();
        string mode = "GetNoteType";
        int noteTypeid = Convert.ToInt16(Session["NoteType"].ToString());
        try
        {
            dt = ObjNoteBll.GetNoteType(mode, noteTypeid);
            if (dt != null && dt.Rows.Count > 0)
            {
                lblNotes.Text = "Notes -  " + Convert.ToString(dt.Rows[0]["Name"]);
            }
            else
            {
                lblNotes.Text = "Notes - ";
            }
        }
        catch (Exception ex)
        {
            ////  ex.Message;
        }

    }
    public class Data
    {
        public string Reference { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public List<NotesBLL> lst { get; set; }
    }
    [System.Web.Services.WebMethod]
    public static void Clear()
    {
        HttpContext.Current.Session["Reference"] = string.Empty;
        HttpContext.Current.Session["fromDate"] = string.Empty;
        HttpContext.Current.Session["toDate"] = string.Empty;
    }


    [System.Web.Services.WebMethod]
    public static void setActiveTab(string tabtext)
    {
        HttpContext.Current.Session["tabFrom"] = tabtext;
    }


}