using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_PayMaster : Authentication
{
    UserMaster UM;
    clsCommon objCommon = new clsCommon();

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (UM == null)
        {
            Session.Abandon();
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["SiteRoot"] + "Login.aspx");
        }

        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true, false,false);
      
        if (!IsPostBack)
        {
            if (UM == null)
                return;

            //hdLocationId.Value = HttpContext.Current.Session["LocationID"].ToString();
            //BindLocation();
        }
       // hdLocationId.Value = HttpContext.Current.Session["LocationID"].ToString();
    }

    [System.Web.Services.WebMethod]
    public static string BindPayrollDetails(bool? isActive, int? locationId, int? EmpId)
    {
        try
        {
            List<PayrollBLL> lsPayroll = new List<PayrollBLL>();

            lsPayroll = PayrollBLL.GetPayrollDetails("GETMASTER", isActive, locationId, EmpId);

            var data = from pr in lsPayroll
                       select new
                       {
                           pr.EmpID,
                           pr.EmpName,
                           pr.EmpJoiningDate,
                           pr.PastExperince,
                           pr.TotalExperince,
                           pr.RevisionDate,
                           pr.Gross,
                           pr.Bonus,
                           pr.PF,
                           pr.MonthlyCTC,
                           pr.AnnualCTC,
                           pr.Net,
                           pr.PT,
                           pr.Basic,
                           pr.PBB
                       };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }

    }

    [System.Web.Services.WebMethod]
    public static string BindPayrollDetailsForEmp(int EmpId)
    {
        try
        {
            List<PayrollBLL> lsPayroll = new List<PayrollBLL>();

            lsPayroll = PayrollBLL.GetPayrollDetails("GETMASTER", null, null, EmpId);

            var data = from pr in lsPayroll
                       select new
                       {
                           pr.EmpID,
                           pr.EmpName,
                           pr.EmpJoiningDate,
                           pr.PastExperince,
                           pr.TotalExperince,
                           pr.RevisionDate,
                           pr.Gross,
                           pr.Bonus,
                           pr.PF,
                           pr.MonthlyCTC,
                           pr.AnnualCTC,
                           pr.Net,
                           pr.PT,
                           pr.Basic,
                           pr.HRA,
                           pr.Conveyance,
                           pr.Medical,
                           pr.Food,
                           pr.Special,
                           pr.LTA,
                           pr.Insurance,
                           pr.PBB
                       };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }

    }

    [System.Web.Services.WebMethod]
    public static string BindSalaryDetailsForEmp(int EmpId, int? year)
    {
        try
        {
            List<PayrollBLL> lsPayroll = new List<PayrollBLL>();

            lsPayroll = PayrollBLL.GetSalaryDetails("GETDETAIL", EmpId, year,null,null);

            var data = from pr in lsPayroll
                       select new
                       {
                           pr.PayDate,
                           pr.Basic,
                           pr.HRA,
                           pr.Conveyance,
                           pr.Medical,
                           pr.Food,
                           pr.Special,
                           pr.LTA,
                           pr.PF,
                           pr.PT,
                           pr.Insurance,
                           pr.Loan,
                           pr.Advance,
                           pr.Leaves,
                           pr.Presents,
                           pr.Tax,
                           pr.Deduction,
                           pr.Bonus,
                           pr.Addition,
                           pr.Remark,
                           pr.CTC,
                           pr.Gross,
                           pr.Net,
                           pr.TotalAddition,
                           pr.TotalDeduction,
                           pr.Ded_Leave,
                           pr.Ded_Tax
                       };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }

    }

    //protected void dlLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    hdLocationId.Value = dlLocation.SelectedValue;
    //}

    //private void BindLocation()
    //{
    //    DataTable dtEmployeeLocation = new DataTable();
    //    dtEmployeeLocation = objCommon.EmployeeLocationList();
    //    dlLocation.DataSource = dtEmployeeLocation;
    //    dlLocation.DataTextField = "Name";
    //    dlLocation.DataValueField = "LocationID";
    //    dlLocation.DataBind();
    //    dlLocation.SelectedValue = "10";
    //    hdLocationId.Value = HttpContext.Current.Session["LocationID"].ToString(); 
    //    dlLocation.Visible = true;
    //}


    public string CalculateTotalExp(string strd2, int intPreviousExpInMonths, Boolean blnAdd)
    {
        if (string.IsNullOrEmpty(strd2.Trim()))
        {
            return "";
        }
        // compute & return the difference of two dates,
        // returning years, months & days
        // d1 should be the larger (newest) of the two dates
        // we want d1 to be the larger (newest) date
        // flip if we need to
        DateTime d1 = DateTime.Now;

        string[] strDate = strd2.Split(new string[] {
		" ",
		"-",
		"/"
	}, StringSplitOptions.None);



        DateTime d2 = new DateTime(Convert.ToInt16(strDate[2]), Convert.ToInt16(strDate[1]), Convert.ToInt16(strDate[0]), 0, 0, 0);

        if (d1 < d2)
        {
            DateTime d3 = d2;
            d2 = d1;
            d1 = d3;
        }
        int intMonth = 0;
        int intDays = 0;
        int intYears = 0;
        // compute difference in total months
        intMonth = intPreviousExpInMonths;
        if (blnAdd)
        {
            intMonth = 12 * (d1.Year - d2.Year) + (d1.Month - d2.Month) + intPreviousExpInMonths;
        }
        // based upon the 'days',
        // adjust months & compute actual days difference
        if (d1.Day < d2.Day)
        {
            // intMonth--;
            intDays = DateTime.DaysInMonth(d2.Year, d2.Month) - d2.Day + d1.Day;
        }
        else
        {
            intDays = d1.Day - d2.Day;
        }
        // compute years & actual months
        intYears = intMonth / 12;
        intMonth = Math.Abs(intMonth - (intYears * 12));

        string strExp = string.Empty;

        if (intYears == 0 && intMonth != 0)
        {
            if (intMonth > 1)
            {
                strExp = intMonth.ToString() + " Months";
            }
            else
            {
                strExp = intMonth.ToString() + " Month";
            }
        }
        else if (intYears != 0 && intMonth == 0)
        {
            if (intYears > 1)
            {
                strExp = intYears.ToString() + " Years";
            }
            else
            {
                strExp = intYears.ToString() + " Year";
            }
        }
        else if (intYears != 0 && intMonth != 0)
        {
            if (intYears > 1 && intMonth > 1)
            {
                strExp = intYears.ToString() + " Years " + intMonth.ToString() + " Months";
            }
            else if (intYears == 1 && intMonth > 0)
            {
                strExp = intYears.ToString() + " Year " + intMonth.ToString() + " Months";
            }
            else if (intYears > 1 && intMonth == 1)
            {
                strExp = intYears.ToString() + " Years " + intMonth.ToString() + " Month";
            }
        }
        return strExp;
    }

}