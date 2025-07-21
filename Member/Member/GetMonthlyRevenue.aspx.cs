using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }



    [WebMethod]
    public static string GetMonthlyRevenue(string FromDate, string ToDate)
    {
        String daresult = null;
        DataTable AmountDT = new DataTable();
        DataSet ds = new DataSet();
        if (FromDate != "" || ToDate != "")
        {
            DateTime frm = DateTime.ParseExact(FromDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            FromDate = frm.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(ToDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            ToDate = to.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        try
        {

            GetInvoice objInvoice = new GetInvoice();
            ds = objInvoice.GetMonthlyRevenue(FromDate, ToDate);

            //string str = string.Empty;
            StringBuilder sb = new StringBuilder();
            AmountDT = ds.Tables[0];

            if (ds.Tables[0].Rows.Count > 2)
            {
                sb.Append("<table id='dtAmountTable' class=''   style='width: 100 % '>");

                sb.Append("<thead ><tr >");
                foreach (DataColumn dc in AmountDT.Columns)
                {
                    sb.Append("<th >" + dc.ColumnName + "</th>");
                }
                sb.Append("</tr></thead>");
                sb.Append("<tbody>");
                foreach (DataRow dr in AmountDT.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn dc in AmountDT.Columns)
                    {
                        sb.Append("<td>" + String.Format("{0:n}", dr[dc]) + "</td>");
                    }
                    sb.Append("</tr>");
                }

                Double GrandTotal = 0.00, Total = 0.00;
                //sb.Append("<tr><td colspan='2'> Total:</td>");
                sb.Append("<tr><td> </td><td>Total:</td>");
                foreach (DataColumn dc in AmountDT.Columns)
                {
                    if (dc.ColumnName.ToLower() != "app development" && dc.ColumnName.ToLower() != "account manager")
                    {
                        foreach (DataRow dr in AmountDT.Rows)
                        {
                            Total += Convert.ToDouble(dr[dc] == DBNull.Value ? 0 : dr[dc]);
                            GrandTotal += Total;
                        }
                        sb.Append("<td>" + Convert.ToString(String.Format("{0:n}", Total)) + "</td>");
                    }

                    Total = 0.00;
                }

                //sb.Append("</tr>");
                //sb.Append("<tr><td> </td><td>Grand Total: </td><td>" + Convert.ToString(String.Format("{0:n}", GrandTotal)) + "</td>");
                //for (int i = 0; i < AmountDT.Columns.Count - 3; i++)
                //{
                //    sb.Append("<td></td>");
                //}
                //sb.Append("</tr>");

                //sb.Append("</tbody></table>");

                //str = JsonConvert.SerializeObject(ds);
                return Convert.ToString(sb);
            }
            else
            {
                sb.Append("<table id='dtAmountTable' class='' style='width: 100 % '>");
                sb.Append("<thead role='rowgroup'><tr role='row'>");
                sb.Append("<th role='columnheader'>Message</th>");
                sb.Append("</tr></thead>");
                sb.Append("<tbody>");
                sb.Append("<tr><td>Record Not Found </td></tr>");
                sb.Append("</tbody></table>");
                return Convert.ToString(sb);
            }


        }
        catch (Exception ex)
        {
            return ex.Message;
        }


        //ds.Tables.Add(yourDataTable);
        //daresult = DataSetToJSON(ds);
    }


    [WebMethod]
    public static void ExportToExcel(string FromDate, string ToDate)
    {
        DataSet ds = new DataSet();
        if (FromDate != "" || ToDate != "")
        {
            DateTime frm = DateTime.ParseExact(FromDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            FromDate = frm.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(ToDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            ToDate = to.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        try
        {
            GetInvoice objInvoice = new GetInvoice();
            ds = objInvoice.GetMonthlyRevenue(FromDate, ToDate);
            _Default df = new _Default();
            df.ExportToExcel(ds.Tables[0]);

        }
        catch (Exception ex)
        {
        }

    }

    public void ExportToExcel(DataTable dt)
    {

        string FileName = "MonthlyRevenue";

        string filename = FileName + ".xls";
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        DataGrid dgGrid = new DataGrid();
        dgGrid.DataSource = dt;
        dgGrid.DataBind();

        //Get the HTML for the control.
        dgGrid.RenderControl(hw);
        //Write the HTML back to the browser.
        //Response.ContentType = application/vnd.ms-excel;
        System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
        this.EnableViewState = false;
        System.Web.HttpContext.Current.Response.Write(tw.ToString());
        //System.Web.HttpContext.Current.Response.End();

    }

}