using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class Member_MonthlyTimesheet : Authentication
{
    int count;
    string intMonth;
    string intYear;
    int monthNumber;
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            //if (!string.IsNullOrEmpty(Request.QueryString["Month"]) && !string.IsNullOrEmpty(Request.QueryString["Year"]))
            //{
            //    intMonth = Request.QueryString["Month"].ToString();
            //    intYear = Request.QueryString["Year"].ToString();
            //}

            ////string monthName = "September";
            // monthNumber = DateTime.ParseExact(intMonth, "MMMM", CultureInfo.CurrentCulture).Month;

            BindgridTimesheet("", "");
        }
    }
    #region Sort Order Function
    public string sortOrder
    {
        get
        {
            if (ViewState["sortOrder"] == null)
            {
                ViewState["sortOrder"] = " asc";
            }

            else if (ViewState["sortOrder"].ToString() == "desc")
            {
                ViewState["sortOrder"] = "asc";
            }
            else
            {
                ViewState["sortOrder"] = "desc";
            }

            return ViewState["sortOrder"].ToString();
        }
        set
        {
            ViewState["sortOrder"] = value;
        }
    }
    #endregion
    protected void grdTimesheet_Sorting(object sender, GridViewSortEventArgs e)
    {
        BindgridTimesheet(e.SortExpression, sortOrder);
    }
    protected void grdTimesheet_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTimesheet.PageIndex = e.NewPageIndex;
        BindgridTimesheet("", "");
    }
    private void BindgridTimesheet(string sortExp, string sortDir)
    {
        //if (!string.IsNullOrEmpty(Request.QueryString["Month"]) && !string.IsNullOrEmpty(Request.QueryString["Year"]))
        // {
        //     intMonth = Request.QueryString["Month"].ToString();
        //     intYear = Request.QueryString["Year"].ToString();
        // }
        //string monthName = "September";
        if (!string.IsNullOrEmpty(Request.QueryString["Empid"]))
        {
            string[] sdate = Session["Timesheet"].ToString().Split(' ');
            intMonth = sdate[0].ToString();
            intYear = sdate[1].ToString();
        }
        monthNumber = DateTime.ParseExact(intMonth, "MMMM", CultureInfo.CurrentCulture).Month;
        int EmployeeId = Convert.ToInt16(Request.QueryString["empId"]);
        SqlDataAdapter da;
        DataSet ds;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("sp_TimeSheet", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Mode ", "TimesheetReport");
        //cmd.Parameters.AddWithValue("@Tm_ProjectId ", DBNull.Value);
        cmd.Parameters.AddWithValue("@EmpID", EmployeeId);
        cmd.Parameters.AddWithValue("@Month", monthNumber);
        cmd.Parameters.AddWithValue("@Year", Convert.ToInt16(intYear));
        //cmd.Parameters.AddWithValue("@ModuleId", DBNull.Value);
        da = new SqlDataAdapter(cmd);
        ds = new DataSet();
        da.Fill(ds);
        DataView myDataView = new DataView();
        myDataView = ds.Tables[0].DefaultView;
        ViewState["ExportData"] = ds.Tables[0];
        if (sortExp != string.Empty)
        {
            myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }

        grdTimesheet.DataSource = myDataView;
        grdTimesheet.DataBind();
        //grdTimesheet.ShowFooter = true;

    }


    protected void grdTimesheet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //don't defined count=0 inside function

        DataRowView dtview = e.Row.DataItem as DataRowView;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label hrsValue = (Label)e.Row.Cells[2].FindControl("lblCountHours");
            count = count + Convert.ToInt32(hrsValue.Text);
            //string status = e.Row.Cells[0].Text;
            string tsDay = dtview["tsDay"].ToString();
            bool isSaturday = Convert.ToBoolean(dtview["isSaturday"]);
            bool isSunday = Convert.ToBoolean(dtview["isSunday"]);
            bool isLeave = Convert.ToBoolean(dtview["isLeave"]);
            bool isPublicHoliday = Convert.ToBoolean(dtview["isPublicHoliday"]);
            if (isSaturday || isSunday)
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFF8DC"); // light beige
            }

            if (isLeave)
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6F9FF"); // light blue
            }

            if (isPublicHoliday)
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FDEBD0"); // light orange
            }

        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {

            Label lblHoursTotal = (Label)e.Row.Cells[3].FindControl("lblTotalHours");
            lblHoursTotal.Text = Convert.ToString(count);
        }
        //not displayed in design


    }

    public void ExportToExcel(DataTable dt)
    {
        int EmployeeId = Convert.ToInt16(Request.QueryString["empId"]);
        string fileName = "MonthlyTimesheet_" + EmployeeId + ".xls";

        DataGrid dg = new DataGrid
        {
            AllowPaging = false,
            AutoGenerateColumns = false
        };

        dg.HeaderStyle.Font.Bold = true;
        dg.HeaderStyle.BackColor = System.Drawing.Color.LightGray;

        Dictionary<string, string> headerMappings = new Dictionary<string, string>
    {
        { "projName", "Project Name" },
        { "moduleName", "Module Name" },
        { "tsHour", "Hours" },
        { "tsComment", "Comment" },
        { "tsEntryDate", "Entry Date" }
    };

        bool isFirstColumn = true;
        string tsHourColumnName = "tsHour";
        string moduleNameColumnName = "moduleName";

        double totalHours = 0;
        if (dt.Columns.Contains(tsHourColumnName))
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row[tsHourColumnName] != DBNull.Value)
                {
                    double value;
                    if (double.TryParse(row[tsHourColumnName].ToString(), out value))
                    {
                        totalHours += value;
                    }
                }
            }
        }      
        
        DataRow totalRow = dt.NewRow();
        if (dt.Columns.Contains(tsHourColumnName))
        {
            totalRow[tsHourColumnName] = totalHours;
        }
        if (dt.Columns.Contains(moduleNameColumnName))
        {
            totalRow[moduleNameColumnName] = "Total";
        }
        dt.Rows.Add(totalRow);

        foreach (DataColumn col in dt.Columns)
        {
            if (col.ColumnName != "isSaturday" &&
                col.ColumnName != "isSunday" &&
                col.ColumnName != "isPublicHoliday" &&
                col.ColumnName != "isLeave" &&
                col.ColumnName != "tsDate" &&
                col.ColumnName != "tsDay")
            {
                BoundColumn column = new BoundColumn
                {
                    DataField = col.ColumnName,
                    HeaderText = isFirstColumn ? "Date" :
                                 (headerMappings.ContainsKey(col.ColumnName)
                                    ? headerMappings[col.ColumnName]
                                    : col.ColumnName)
                };

                isFirstColumn = false;
                dg.Columns.Add(column);
            }
        }

        dg.DataSource = dt;

        dg.ItemDataBound += (s, e) =>
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;

                bool isSaturday = dt.Columns.Contains("isSaturday") && row["isSaturday"] != DBNull.Value && Convert.ToBoolean(row["isSaturday"]);
                bool isSunday = dt.Columns.Contains("isSunday") && row["isSunday"] != DBNull.Value && Convert.ToBoolean(row["isSunday"]);
                bool isLeave = dt.Columns.Contains("isLeave") && row["isLeave"] != DBNull.Value && Convert.ToBoolean(row["isLeave"]);
                bool isPublicHoliday = dt.Columns.Contains("isPublicHoliday") && row["isPublicHoliday"] != DBNull.Value && Convert.ToBoolean(row["isPublicHoliday"]);

                if (isPublicHoliday)
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FDEBD0");
                else if (isLeave)
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6F9FF");
                else if (isSaturday || isSunday)
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFF8DC");
            }
        };

        dg.DataBind();

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

        using (System.IO.StringWriter stringWriter = new System.IO.StringWriter())
        using (HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter))
        {
            dg.RenderControl(htmlTextWriter);
            HttpContext.Current.Response.Write(stringWriter.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = null;
        if (ViewState["ExportData"] != null && ViewState["ExportData"] is DataTable)
        {
            dt = (DataTable)ViewState["ExportData"];
            ExportToExcel(dt);
        }
    }   
}