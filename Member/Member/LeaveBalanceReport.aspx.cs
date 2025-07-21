using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_LeaveBalanceReport : System.Web.UI.Page
{
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UM = UserMaster.UserMasterInfo();
            spndatetime.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    [System.Web.Services.WebMethod]
    public static String BindLeaveBalance()
    {
        try
        {

            List<LeaveBalanceBLL> lstConfig = LeaveBalanceBLL.GetLeaveBalance("GetLeaveSummary");
            var data = from curLeaveBalance in lstConfig
                       select new
                       {
                           curLeaveBalance.EmpID,
                           curLeaveBalance.EmpName,
                           curLeaveBalance.LeaveSummary
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception ex)
        {

            return null;
        }
    }
    [System.Web.Services.WebMethod]
    public static string GetLeaveDetails(int empId)
    {
        DataTable dt = LeaveBalanceBLL.GetLeaveDetails(empId, "GetLeaveDetailsByEmployeeId");
        StringBuilder html = new StringBuilder();

        html.Append("<div style='display: flex; justify-content: center; padding: 20px;'>");
        html.Append("<div style='max-width: 900px; width: 100%; padding: 20px; ");
        html.Append("border-radius: 10px;'>");

        // Title
        html.Append("<h3 style='text-align: center; margin-bottom: 5px;'>Employee Leave Summary</h3>");

        // Date
        html.Append("<p style='text-align: right; margin-bottom: 20px;'>");
        html.Append(DateTime.Now.ToString("dd-MMM-yyyy"));
        html.Append("</p>");

        // Table
        html.Append("<table style='width: 100%; border-collapse: collapse; text-align: center; font-family: Arial, sans-serif;'>");
        html.Append("<thead style='background-color: #e8e8e8; height: 28px !important;'>");
        html.Append("<tr>");
        html.Append("<th style='padding: 10px; border: 1px solid #ccc; height: 28px !important;'>Type</th>");
        html.Append("<th style='padding: 10px; border: 1px solid #ccc; height: 28px !important;'>Total (Annual)</th>");
        html.Append("<th style='padding: 10px; border: 1px solid #ccc; height: 28px !important;'>Total (Till Date)</th>");
        html.Append("<th style='padding: 10px; border: 1px solid #ccc; height: 28px !important;'>Consumed</th>");
        html.Append("<th style='padding: 10px; border: 1px solid #ccc; height: 28px !important;'>Balance</th>");
        html.Append("</tr>");
        html.Append("</thead>");
        html.Append("<tbody>");

        foreach (DataRow row in dt.Rows)
        {
            html.Append("<tr>");
            html.Append("<td style='padding: 10px; border: 1px solid #ccc;'>" + row["Type"] + "</td>");
            html.Append("<td style='padding: 10px; border: 1px solid #ccc;'>" + row["Total"] + "</td>");
            html.Append("<td style='padding: 10px; border: 1px solid #ccc;'>" + row["Total_Accrual"] + "</td>");
            html.Append("<td style='padding: 10px; border: 1px solid #ccc;'>" + row["Consumed"] + "</td>");
            html.Append("<td style='padding: 10px; border: 1px solid #ccc;'>" + row["Balance"] + "</td>");
            html.Append("</tr>");
        }

        html.Append("</tbody>");
        html.Append("</table>");
        html.Append("</div>");
        html.Append("</div>");

        return html.ToString();
    }


}