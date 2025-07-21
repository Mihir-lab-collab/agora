using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_EmpCompOffReport : System.Web.UI.Page
{
    public string hdnFromDate ="";
    string hdnToDate ="";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        
    }
  
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if(Request.Form["txtFromDate"]!="" && Request.Form["txtToDate"]!="")
        {
            string FromDate = Request.Form["txtFromDate"];
            string ToDate = Request.Form["txtToDate"];
            string[] a = FromDate.Split('/');
            string[] b = ToDate.Split('/');
            string hdnFromDate = a[2] + '-' + a[1] + '-' + a[0];
            string hdnToDate = b[2] + '-' + b[1] + '-' + b[0];
            string cs = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select A.[Holiday Working Date],A.[Employee Name],EM.empName as [Comp Off Provided By],ISNULL(A.[CompOff Used Date],NULL) AS [CompOff Used Date],a.[CompOff Balance] from(select co.empID as 'Employee Id',convert(date,co.coDate,103) as 'Holiday Working Date',em.empName as 'Employee Name',convert(date,eld.leaveFrom,103) as 'CompOff Used Date',co.entryBy,case when co.empLeaveId is null then '1' else '0'end as [CompOff Balance] from empcompoff co join employeeMaster em on co.empID=em.empid join empLeaveDetails eld on co.empLeaveId=eld.empLeaveId AND co.empLeaveId IS NOT NULL WHERE convert(date, co.entryDate,103) BETWEEN '" + hdnFromDate + "' AND '" + hdnToDate + "' UNION ALL SELECT co.empID as 'Employee Id',convert(date,co.coDate,103) as 'Holiday Working Date',em.empName as 'Employee Name',NULL as 'CompOff Used Date',co.entryBy,'1' as [CompOff Balance] from empcompoff co join employeeMaster em on co.empID=em.empid   " +
                    "AND co.empLeaveId IS  NULL WHERE convert(date, co.entryDate,103) BETWEEN '" + hdnFromDate + "' AND '" + hdnToDate + "')A JOIN employeeMaster EM ON A.entryBy=EM.empid";
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    DataGrid dg = new DataGrid();
                    dg.DataSource = dt;
                    dg.DataBind();

                    // THE EXCEL FILE.
                    string sFileName = "CompOffList-" + System.DateTime.Now.Date + ".xls";
                    sFileName = sFileName.Replace("/", "");

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                    Response.ContentType = "application/vnd.ms-excel";
                    EnableViewState = false;

                    System.IO.StringWriter objSW = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                    dg.HeaderStyle.Font.Bold = true;     // SET EXCEL HEADERS AS BOLD.
                    dg.RenderControl(objHTW);

                    // STYLE THE SHEET AND WRITE DATA TO IT.
                    Response.Write("<style> TABLE { border:dotted 1px #999; } " +
                        "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
                    Response.Write(objSW.ToString());



                    Response.End();
                    dg = null;
                }
            }
        }
        
    }
}