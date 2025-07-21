using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_TimesheetIncomplete : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true,false, true);
        hdLocationId.Value = Session["LocationID"].ToString ();
        if (lblMonth.Text != "")
        {
            lblMonth.Text = DateTime.Parse(lblMonth.Text).ToString("MMM yyyy");
        }
        else
        {
            lblMonth.Text = DateTime.Today.ToString("MMM yyyy");
        }
    }

    [System.Web.Services.WebMethod]
    public static String BindReports(DateTime Date, string Month, string Year, string EmpID, string LocationID)
    {
        try
        {
            List<TimeSheet> lstTS = TimeSheet.GetIncomepleteTS(Date.Month, Date.Year,
                Convert.ToInt32(LocationID), Convert.ToInt32(EmpID));
            var data = from pr in lstTS
                       orderby pr.TSDate descending
                       select new
                       {
                           pr.EmpID,
                           pr.EmpName,
                           pr.TSDate,
                           pr.AttHour,
                           pr.AttTSHour
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }
    }

    protected void PagerButtonClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string arg = "";
        arg = btn.CommandArgument.ToString();
        switch (arg)
        {
            case "next":
                lblMonth.Text = DateTime.Parse(lblMonth.Text).AddMonths(1).ToString("MMM yyyy");
                break;

            case "prev":
                lblMonth.Text = DateTime.Parse(lblMonth.Text).AddMonths(-1).ToString("MMM yyyy");
                break;

        }
    }
}