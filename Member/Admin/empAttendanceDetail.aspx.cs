using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
public partial class Admin_empAttendanceDetail : Authentication
{
    object strDate;

    SqlConnection strConn = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {
       
      
      if (Request.QueryString["strDate"] == null)
      {
          strDate = DateTime.Now;
      }
      else
      {
          strDate =Convert.ToDateTime(Request.QueryString["strDate"]);
      }
        FillGrid();
    }

    private void FillGrid()
    {


        lblMonthName.Text = string.Format("{0:MMMM}", strDate)+" " + Convert.ToDateTime(strDate).Year;
        int month;
        int year;
        int totalDays;
      month=Convert.ToInt16(Convert.ToDateTime(strDate).Month);
       year=Convert.ToInt16(Convert.ToDateTime(strDate).Year);
       totalDays=Convert.ToInt16(DateTime.DaysInMonth(year, month));
        SqlCommand  cmd=new SqlCommand();
        cmd.CommandText = "GetEmployeeDetail";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@days", totalDays);
        cmd.Parameters.AddWithValue("@month", month);
        cmd.Parameters.AddWithValue("@year", year);
        cmd.Connection = strConn;
        DataSet dst = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dst);
        grdattReport.DataSource = dst;
        grdattReport.DataBind();
    }
}
