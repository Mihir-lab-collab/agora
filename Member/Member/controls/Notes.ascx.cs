using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonFunctionLib;
using System.Data.SqlClient;

public partial class Controls_Notes : System.Web.UI.UserControl
{
    DBFunc ObjDB = new DBFunc();
    static string TableNameStr = "";
    static string RefIDStr = "";
    static string InfoStr = "";
	string NoteCount = "";
    Boolean IsVisible = true;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
            NotesDiv.Visible = false;

        spndisplaymsg.Visible = false;
    }
    public void NotesInit(String empid , Boolean Visible , Int16 mode)
    {
        if (Visible)
        {
            hdnempid.Value = String.Empty;
            hdnempid.Value = empid;
            NotesLink.Visible = true;
            NotesCount();
        }
        else
        {
            if (mode == 0)
            {
                NotesLink.Visible = Visible;
                NotesDiv.Visible = Visible;
            }
            else
            {
                NotesDiv.Visible = Visible;
            }
        }
    }

    protected void NotesLink_Click(object sender, EventArgs e)
    {
        IsVisible = true;
        NotesGet();
    }

    //protected void NotesClose_Click(object sender, EventArgs e)
    //{
    //    IsVisible = false;
    //    NotesGet();
    //}

    protected void AddBtn_Click(object sender, EventArgs e)
    {
        if (NotesTxt.Text != "")
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());

            String ValNotes = String.Empty;
            if (NotesGrid.Rows.Count > 0)
            {
                foreach (GridViewRow row in NotesGrid.Rows)
                {
                    Label lVal = (Label)row.FindControl("lblnotese");
                    ValNotes += lVal.Text + "#";
                }
            }

            string query = "update employeeMaster set empNotes ='" + ValNotes +
                              NotesTxt.Text + "\n<b>  Added on " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + "</b>' where empid=" + hdnempid.Value;
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
           int cnt = cmd.ExecuteNonQuery();

           if (cnt > 0)
           {
               NotesGet();
           }
           con.Close();
           NotesTxt.Text = String.Empty;
           spndisplaymsg.Visible = true;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType() , "NoteScript", "alert('" + "Note can not be empty. " + "')", true);   
        }
    }
    public void NotesGet()
    {
        DataTable dt = new DataTable();
        DataTable dtMyTable = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("select empid,empName,empNotes,InsertedOn,empJoiningDate from employeeMaster where empid=" + hdnempid.Value, con);

        
        da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            if (!String.IsNullOrEmpty(dt.Rows[0]["empNotes"].ToString().Trim()))
            {
                String[] strNotes = (dt.Rows[0]["empNotes"].ToString().Trim()).Split('#');
                dtMyTable = new DataTable("MyTable");
                dtMyTable.Columns.Add("sno", typeof(Int32));
                dtMyTable.Columns.Add("empid", typeof(String));
                dtMyTable.Columns.Add("empName", typeof(String));
                dtMyTable.Columns.Add("empNotes", typeof(String));
                dtMyTable.Columns.Add("InsertedOn", typeof(String));
                dtMyTable.Columns.Add("empJoiningDate", typeof(String));

                if (strNotes.Length > 0)
                {
                    for (int i = 0; i < strNotes.Length; i++)
                    {
                        DataRow dr = dtMyTable.NewRow();

                        dr["sno"] = i;
                        dr["empid"] = dt.Rows[0]["empid"].ToString();
                        dr["InsertedOn"] = Convert.ToDateTime(dt.Rows[0]["InsertedOn"]).ToString("dd-MMM-yyyy");
                        dr["empJoiningDate"] = Convert.ToDateTime(dt.Rows[0]["empJoiningDate"]).ToString("dd-MMM-yyyy");
                        dr["empName"] = dt.Rows[0]["empName"].ToString();
                        dr["empNotes"] = strNotes[i].ToString();

                        dtMyTable.Rows.Add(dr);
                    }
                }
            }
        }
       if (dtMyTable.Rows.Count > 0)
          dtMyTable.DefaultView.Sort = "sno DESC";

        NotesGet(dtMyTable);
    }

	protected void NotesGet(DataTable dt)
    {
		NoteCount = dt.Rows.Count.ToString();
		NotesCount();
        if (dt.Rows.Count > 0)
        {
            NotesGrid.DataSource = dt;
            NotesGrid.DataBind();  
        }
        else
        {
            NotesGrid.DataSource = null;
            NotesGrid.DataBind();
        }
        NotesDiv.Visible = true;
    }

	protected void NotesCount()
	{
		if (NoteCount == "")
		{
			NotesGet();
		}
		NotesLink.Text = "Notes (" + NoteCount + ")"; 
	}

    protected void NotesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lblActiondate = (Label)e.Row.FindControl("lblActiondate");
            //LinkButton lbActionDate = (LinkButton)e.Row.FindControl("lbActionDate");
            //Label lbldate =(Label)e.Row.FindControl("lbldate");
            //if (lblActiondate.Text == "")
            //{
            //    lbActionDate.Visible = false;
            //}
        }
		
    }
    protected void NotesGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            //int index = Convert.ToInt32(e.CommandArgument);

           
            //GridViewRow row = NotesGrid.Rows[index];
           
            //string NoteID = this.NotesGrid.DataKeys[index]["NotesID"].ToString();
            //object[] ObjParam = new object[9];
            //ObjParam[0] = TableNameStr;
            //ObjParam[1] = RefIDStr;
            //ObjParam[2] = GeneralFunction.generalFunction.GetLocalIP();
            //ObjParam[3] = AdminSession.AdminID;
            //ObjParam[6] = NoteID;
            //DataTable dt = ObjDB.ExecuteProcedureRtnDT("NotesManage", ObjParam);
            //NotesGet(dt);
        }
    }
}
