using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_PayslipArchive : Authentication
{
    UserMaster UM;
    public int flgsalgenerated = 0;
    string empname;
    string empid;
    string month;
    string year ;
    string monthName;
    int LocationID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string empid = hdnEmpID.Value;
       // UM = UserMaster.UserMasterInfo();
      
       
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true, false, true);
        //txtEmployeeID.Attributes["value"] = empid;
        if (!IsPostBack)
        {
            tblsalaryslip.Attributes.Add("style", "display:none");
        }
       
    }

    private void calculatepayslip()
    {
       
        string ddate1;
        dynamic intYear = Convert.ToInt32(year);
        int intmonth = Convert.ToInt32(month);
        LocationID = UserMaster.GetLocatoionId(Convert.ToInt32 (empid));
        List<PayrollBLL> listSalDetails = PayrollBLL.GetSalaryDetails("GETDETAIL", Convert.ToInt32(empid), intYear, intmonth, LocationID);
        if (listSalDetails.Count > 0)
        {
            tblsalaryslip.Attributes.Add("style", "display:block");
            lblSalaryDate.InnerText = monthName.Trim().ToUpper() + "-" + year;
            ddate1 = "1-" + monthName + "-" + year;
            lblname.Text = Convert.ToString(listSalDetails[0].EmpName) + " ( " + empid + " )";
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

        }
        else
        {
            ddate1 = "1-" + monthName + "-" + year;
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
        }
    }


    private void PrintSlip()
    {
        string ddate1;
        List<LocationBLL> listCompDetails = LocationBLL.BindLocation("GetLocationByID", LocationID);
        string CompName = "";
        string CompAddress = "";
        string CompLogo = "";
        if (listCompDetails.Count > 0)
        {
            CompName = Convert.ToString(listCompDetails[0].LegalName);
            CompAddress = Convert.ToString(listCompDetails[0].Address);
            CompLogo = Convert.ToString(listCompDetails[0].Logo);
        }        
        //imgLogo.Src = "Services/ViewImage.ashx?FileName=" + CompLogo;
        imgLogo.Src = "images/" + CompLogo;
        empname = Convert.ToString(Session["pempname"]);
        ddate1 = "1-" + monthName + "-" + year;
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
         empid= hdnEmpID.Value;
         month= hdnMonth.Value;
         year= hdnYear.Value;
         monthName = ddlMonth.SelectedItem.Text;
        calculatepayslip();
        PrintSlip();
    }
}