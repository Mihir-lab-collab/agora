using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_Payslip : Authentication
{
    public string ddate, dMonth, ddate1;
    DateTime date1;
    int intmonth, strDate;
    public int flgsalgenerated = 0;
    string empname;
    generalFunction gf = new generalFunction();

    UserMaster UM;

    private void Page_Load(System.Object sender, System.EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            int intOffMonthset = -1;
            if (DateTime.Now.Day < 10)
                intOffMonthset = -2;

            lbtnnext.Visible = false;

            Session.Add("currentslipdate", DateAndTime.DateAdd("m", intOffMonthset, DateAndTime.Now));
            ddate = Session["currentslipdate"].ToString();
            Session.Add("LastUpdateDate", DateAndTime.DateAdd("m", intOffMonthset, DateAndTime.Now));
            dMonth = DateAndTime.Month(DateTime.Parse(ddate)).ToString();

            intmonth = Convert.ToInt32(dMonth);

            calculatepayslip();
            PrintSlip();
        }

    }

    private void calculatepayslip()
    {
        ddate = Session["currentslipdate"].ToString();
        dMonth = DateAndTime.Month(DateTime.Parse(ddate)).ToString();
        intmonth = Convert.ToInt32(dMonth);
        dynamic intYear = Convert.ToInt32(DateAndTime.Year(DateTime.Parse(ddate)).ToString());
        ddate1 = ddate;
        ddate1 = "1-" + DateAndTime.MonthName(DateAndTime.Month(DateTime.Parse(ddate1))) + "-" + DateAndTime.Year(DateTime.Parse(ddate1));

        List<PayrollBLL> listSalDetails = PayrollBLL.GetSalaryDetails("GETDETAIL", UM.EmployeeID, intYear, intmonth, Int32.Parse(UM.LocationID));
        if (listSalDetails.Count > 0)
        {
            lblname.Text = Convert.ToString(listSalDetails[0].EmpName) + " ( " + UM.EmployeeID + " )";
            lbldesignation.Text = Convert.ToString(listSalDetails[0].SkillDesc);
            lblemployedsince.Text = string.Format(listSalDetails[0].EmpJoiningDate.ToString("dd-MMM-yyyy"));
            Session["pemployedsince"] = lblemployedsince.Text;
            Session["pdesignation"] = lbldesignation.Text;
            flgsalgenerated = 1;

            lblbasicsal.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].Basic));
            lblhra.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].HRA));
            lblta.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].Conveyance));

            lbllta.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].LTA));
            lblfoodallow.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].Food));
            lblmedical.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].Medical));
            lblspecialallow.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].Special));
            lbladvancea.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].Advance));
            lblbonus.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].Bonus));
            lblincometax.Text = string.Format("{0:n0}", Convert.ToInt32(listSalDetails[0].Tax));
            lblproffesiontax.Text = string.Format("{0:n0}", (listSalDetails[0].PT));
            lblInsurance.Text = string.Format("{0:n0}", (listSalDetails[0].Insurance));
            lblepf.Text = string.Format("{0:n0}", (listSalDetails[0].PF));
            lblloanrepayment.Text = string.Format("{0:n0}", (listSalDetails[0].Loan));
            lblleavededuction.Text = string.Format("{0:n0}", Convert.ToDecimal(listSalDetails[0].Ded_Leave));
            lbladvanceab.Text = string.Format("{0:n0}", listSalDetails[0].Addition);
            lblothers.Text = string.Format("{0:n0}", listSalDetails[0].Deduction);
            lbldayspresent.Text = Convert.ToString(listSalDetails[0].Presents);
            lbldaysabsent.Text = Convert.ToString(listSalDetails[0].Leaves);

            lblnoofdays.Text = DateAndTime.Day(DateAndTime.DateAdd("d", -1, DateAndTime.DateAdd("m", 1, DateTime.Parse(ddate1)))).ToString();

            lbltotaldeduction.Text = string.Format("{0:n0}", Convert.ToDouble(listSalDetails[0].TotalDeduction));
            lblgrosssal.Text = string.Format("{0:n0}", Convert.ToDouble(listSalDetails[0].Gross));

            lblab.Text = string.Format("{0:n0}", (Convert.ToDouble(listSalDetails[0].AB)));
            lblnetpayable.Text = string.Format("{0:n0}", (Convert.ToDouble(listSalDetails[0].Net)));
            Session["pempname"] = listSalDetails[0].EmpName;
            lblEmpPan.Text = listSalDetails[0].EmpPAN;
            lblEmpUan.Text = listSalDetails[0].EmpUAN;
            lblEmpfacno.Text = listSalDetails[0].EmpEPF;          
        }
        else
        {
            flgsalgenerated = 0;
            lblbasicsal.Text = "0";
            lblhra.Text = "0";
            lbllta.Text = "0";
            lblta.Text = "0";
            lblfoodallow.Text = "0";
            lblmedical.Text = "0";
            lblspecialallow.Text = "0";
            lbladvancea.Text = "0";
            lblbonus.Text = "0";
            lblincometax.Text = "0";
            lblproffesiontax.Text = "0";
            lblepf.Text = "0";
            lblloan.Text = "0";
            lblothersab.Text = "0";
            lbldayspresent.Text = "0";
            lbldaysabsent.Text = "0";
            lblnoofdays.Text = DateAndTime.Day(DateAndTime.DateAdd("d", -1, DateAndTime.DateAdd("m", 1, DateTime.Parse(ddate1)))).ToString();
            lblab.Text = "0";
            lbltotaldeduction.Text = "0";
            lblgrosssal.Text = "0";
            lblnetpayable.Text = "0";
            lblloanrepayment.Text = "0";
            lblleavededuction.Text = "0";
            lblothers.Text = "0";
            lbladvanceab.Text = "0";
            lblEmpPan.Text = string.Empty;
            lblEmpUan.Text = string.Empty;
            lblEmpfacno.Text = string.Empty;  
        }
    }


    private void PrintSlip()
    {
        List<LocationBLL> listCompDetails = LocationBLL.BindLocation("GetLocationByID", Int32.Parse(UM.LocationID));
        string CompName = Convert.ToString(listCompDetails[0].LegalName);
        string CompAddress = Convert.ToString(listCompDetails[0].Address);
        string CompLogo = Convert.ToString(listCompDetails[0].Logo);
        //imgLogo.Src = "Services/ViewImage.ashx?FileName=" + CompLogo;
        imgLogo.Src = "images/" + CompLogo;
        empname = Convert.ToString(Session["pempname"]);
        ddate = Session["currentslipdate"].ToString();
        dMonth = DateAndTime.Month(DateTime.Parse(ddate)).ToString();
        ddate1 = "1-" + DateAndTime.MonthName(DateAndTime.Month(DateTime.Parse(ddate1))) + "-" + DateAndTime.Year(DateTime.Parse(ddate1));
        string SalaryDate = DateAndTime.MonthName(DateAndTime.Month(DateTime.Parse(ddate1))) + "-" + DateAndTime.Year(DateTime.Parse(ddate1));
        DateTime Currentdate = DateTime.Now;
        string dateWithFormat = Currentdate.ToString("dd-MMM-yyyy");
        List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 2);
        var html = Convert.ToString(lstConfig[0].value);
        html = html.Replace("{CompanyLogo}", imgLogo.Src);
        html = html.Replace("{CompanyName}", CompName);
        html = html.Replace("{CompanyAddress}", CompAddress);
        html = html.Replace("{SalaryDate}", SalaryDate);
        html = html.Replace("{CurrentDate}", dateWithFormat);
        html = html.Replace("{EmpName}", empname);
        html = html.Replace("{EmpDesignation}", Convert.ToString(lbldesignation.Text));
        html = html.Replace("{EmpJoiningDate}", Convert.ToString(lblemployedsince.Text));
        html = html.Replace("{MonthDays}", Convert.ToString(lblnoofdays.Text));
        html = html.Replace("{PresentDays}", Convert.ToString(lbldayspresent.Text));
        html = html.Replace("{AbsentDays}", Convert.ToString(lbldaysabsent.Text));
        html = html.Replace("{Basic}", Convert.ToString(lblbasicsal.Text));
        html = html.Replace("{HRA}", Convert.ToString(lblhra.Text));
        html = html.Replace("{Conveyance}", Convert.ToString(lblta.Text));
        html = html.Replace("{Medical}", Convert.ToString(lblmedical.Text));
        html = html.Replace("{LTA}", Convert.ToString(lbllta.Text));
        html = html.Replace("{Food}", Convert.ToString(lblfoodallow.Text));
        html = html.Replace("{Special}", Convert.ToString(lblspecialallow.Text));
        html = html.Replace("{Advance}", Convert.ToString(lbladvancea.Text));
        html = html.Replace("{Tax}", Convert.ToString(lblincometax.Text));
        html = html.Replace("{PT}", Convert.ToString(lblproffesiontax.Text));
        html = html.Replace("{PF}", Convert.ToString(lblepf.Text));
        html = html.Replace("{Loan}", Convert.ToString(lblloanrepayment.Text));
        html = html.Replace("{LeaveDeduction}", Convert.ToString(lblleavededuction.Text));
        html = html.Replace("{Other Deduction}", Convert.ToString(lblothers.Text));
        html = html.Replace("{CalcGross}", Convert.ToString(lblab.Text));
        html = html.Replace("{Gross}", Convert.ToString(lblgrosssal.Text));
        html = html.Replace("{TotalDeduction}", Convert.ToString(lbltotaldeduction.Text));
        html = html.Replace("{Bonus}", Convert.ToString(lblbonus.Text));
      //  html = html.Replace("{AdvanceAB}", Convert.ToString(lbladvanceab.Text));
        html = html.Replace("{Other Addition}", Convert.ToString(lbladvanceab.Text));
        html = html.Replace("{Loan}", Convert.ToString(lblloan.Text));
        html = html.Replace("{Paid Leaves}", Convert.ToString(lblleavededuction.Text));
        html = html.Replace("{OtherAddition}", Convert.ToString(lblothersab.Text));
        html = html.Replace("{Net}", Convert.ToString(lblnetpayable.Text));

        html = html.Replace("{GroupInsurance}", Convert.ToString(lblInsurance.Text));

        dvContainer.InnerHtml = html;

    }
    protected void lbtnpre_Click(object sender, EventArgs e)
    {
        Session["currentslipdate"] = DateAndTime.DateAdd("m", -1, DateTime.Parse(Session["currentslipdate"].ToString()));
        calculatepayslip();
        PrintSlip();
        lbtnnext.Visible = true;
    }
    protected void lbtnnext_Click(object sender, EventArgs e)
    {
        Session["currentslipdate"] = DateAndTime.DateAdd("m", 1, DateTime.Parse(Session["currentslipdate"].ToString()));

        int intOffMonthset = -1;
        if (DateTime.Now.Day < 10)
            intOffMonthset = -2;

        DateTime CurrentDate = DateAndTime.DateAdd("m", intOffMonthset, DateTime.Now);

        DateTime LastUpdateDate = Convert.ToDateTime(Session["LastUpdateDate"]);

        DateTime SessionDate = Convert.ToDateTime(Session["currentslipdate"]);


        if (CurrentDate.CompareTo(SessionDate) < 0)
        {
            Session["currentslipdate"] = Session["LastUpdateDate"];
            SessionDate = Convert.ToDateTime(Session["currentslipdate"]);
        }

        if (SessionDate.Date == CurrentDate.Date)
        {
            ddate = Session["currentslipdate"].ToString();
            dMonth = DateAndTime.Month(DateTime.Parse(ddate)).ToString();
            lbtnnext.Visible = false;
            Session["LastUpdateDate"] = Session["currentslipdate"];
        }

        calculatepayslip();
        PrintSlip();
    }
}