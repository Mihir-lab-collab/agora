using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;
using System.Configuration;

public partial class Member_PayRevision : Authentication
{
    PayrollBLL Obj;
    PayrollBLL objPayrollBLL = new PayrollBLL();
    UserMaster UM;
    public string EmpGender = "";
    public int PayMonth = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Admin MasterPage = (Admin)Page.Master;
        UM = UserMaster.UserMasterInfo();
        MasterPage.MasterInit(false, true);
        bindDropdown();

        if (!IsPostBack)
        {
            showEmpSalData.Visible = false;
            BindMonths();
            BindYear();
        }
    }

    string SalarySave(PayrollBLL Obj)
    {

        string mode = "ADD";
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SP_SalaryAdd", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        cmd.Parameters.AddWithValue("@EmpID", Obj.EmpID);
        cmd.Parameters.AddWithValue("@RevisionDate", Obj.RevisionDate);
        cmd.Parameters.AddWithValue("@Gross", Obj.Gross);
        cmd.Parameters.AddWithValue("@CTC", Obj.MonthlyCTC);
        cmd.Parameters.AddWithValue("@Net", Obj.Net);
        cmd.Parameters.AddWithValue("@AnnualBonus", Obj.Bonus);
        cmd.Parameters.AddWithValue("@IsPBB", Obj.PBB);
        cmd.Parameters.AddWithValue("@Basic", Obj.Basic);
        cmd.Parameters.AddWithValue("@HRA", Obj.HRA);
        cmd.Parameters.AddWithValue("@Conveyance", Obj.Conveyance);
        cmd.Parameters.AddWithValue("@Medical", Obj.Medical);
        cmd.Parameters.AddWithValue("@LTA", Obj.LTA);
        cmd.Parameters.AddWithValue("@Food", Obj.Food);
        cmd.Parameters.AddWithValue("@Special", Obj.Special);
        cmd.Parameters.AddWithValue("@PT", Obj.PT);
        cmd.Parameters.AddWithValue("@Insurance", Obj.Insurance);
        if (Obj.PF > 0)
        {
            cmd.Parameters.AddWithValue("@IsPF", "1");
        }
        cmd.Parameters.AddWithValue("@ModifiedBy", UM.EmployeeID);
        cmd.Parameters.AddWithValue("@Gratuity", Obj.CalcGratuity);

        using (con)
        {
            con.Open();
            cmd.ExecuteNonQuery();
        }
        return "Done";
    }

    PayrollBLL SalaryCalc(decimal AnnualCTC, int Bonus, Boolean IsPBB, Boolean IsPF, int Insurance)
    {
        decimal Balance;
        decimal Conveyance = 1600;
        decimal Medical = 1250;
        decimal ConveyanceMin = 800;

        PayrollBLL Obj = new PayrollBLL();

        if (AnnualCTC > 0 && Bonus >= 0)
        {
            #region New calculation on 16 Jun 2025
            decimal dcmlTotal = AnnualCTC; 
            decimal dcmlVariable = Convert.ToDecimal(Bonus); 
            decimal dcmlFixedAnnual = 0.00m;
            decimal dcmlAnnualBasic = 0.00m;
            decimal dcmlAnnualEPF = 0.00m;
            //decimal dcmlAnnualCTC = 0.00m;
            decimal dcmlAnnualGratuity = 0.00m;
            decimal dcmlMonthlyCTC = 0.00m;
            decimal dcmlBasic = 0.00m;
            decimal dcmlAnnualGross = 0.00m;
            decimal dcmlMonthlyGross = 0.00m;
            #endregion


            //#1 Fixed Annual
            dcmlFixedAnnual = dcmlTotal - dcmlVariable;
            //#2 Annual Basic
            dcmlAnnualBasic = dcmlFixedAnnual * 0.375m;
            //#3 Annual EPF
            #region Annual EPF
                decimal calcAnnualEPF = 0.00m;
            calcAnnualEPF = dcmlAnnualBasic * 0.12m;
            if (calcAnnualEPF > 21600m)
            {
                dcmlAnnualEPF = 21600m;
            }
            else
            {
                dcmlAnnualEPF = calcAnnualEPF;
            }
            #endregion
            //#4 Annual Gratuity
            dcmlAnnualGratuity = dcmlAnnualBasic * 0.0481m;
            //#5 Annual Gross
            if (IsPF)
            {
                dcmlAnnualGross = dcmlFixedAnnual - dcmlAnnualGratuity - dcmlAnnualEPF;
            }
            else
            {
                dcmlAnnualGross = dcmlFixedAnnual - dcmlAnnualGratuity;// - dcmlAnnualEPF;
            }
            //#6 Monthly Gross
            dcmlMonthlyGross = dcmlAnnualGross / 12m;
            //#7 Monthly Basic
            dcmlBasic = dcmlMonthlyGross * 0.4m;

            Obj.CalcGratuity = Math.Round(dcmlAnnualGratuity/12m);
            Obj.Gross = Math.Round(dcmlMonthlyGross);
            Obj.AnnualCTC = Math.Round(dcmlTotal- dcmlAnnualGratuity);
            dcmlMonthlyCTC = Math.Round(dcmlAnnualGross / 12m);
            Obj.MonthlyCTC = Math.Round(dcmlMonthlyCTC);
            Obj.Basic = Math.Round(dcmlBasic);
            Obj.Bonus = Bonus;
            Obj.PBB = IsPBB;
            Obj.CalcGross = Math.Round(dcmlMonthlyGross);
            Balance = Obj.CalcGross;
            Balance = Balance - dcmlBasic;

            if (IsPF)
            {
                Obj.IsPF = true;
                Obj.PF = Math.Round(dcmlAnnualEPF /12m);// Obj.CalcPF(Convert.ToInt32(hdnEmpId.Value), CTC, Basic: Obj.Basic);
            }
            if (Balance > 0)
            {
                if (Balance >= Conveyance)
                {
                    Obj.Conveyance = Conveyance;
                }
                else if (Balance >= ConveyanceMin)
                {
                    Obj.Conveyance = Balance;
                }
                Balance = Balance - Obj.Conveyance;
            }

            if (Balance > 0)
            {
                if (Obj.CalcGross > 40000 && Balance > 4000)
                {
                    Obj.Food = 4000;
                }
                else if (Obj.CalcGross > 10000 && Balance > 2000)
                {
                    Obj.Food = 2000;
                }
                else if (Balance > 1000)
                {
                    Obj.Food = 1000;
                }
                Balance = Balance - Obj.Food;
            }

            if (Balance >= Medical)
            {
                if (Balance > Medical)
                {
                    Obj.Medical = Medical;
                }
                else
                {
                    Obj.Medical = Balance;
                }

                Balance = Balance - Obj.Medical;
            }

            if (Obj.MonthlyCTC > 25000 && Balance > 0)
            {
                if (Balance > Obj.Basic * Convert.ToDecimal(0.5))
                {
                    Obj.HRA = Obj.Basic * Convert.ToDecimal(0.5);
                }
                else
                {
                    Obj.HRA = Balance;
                }
                Balance = Balance - Obj.HRA;
            }

            if (Obj.MonthlyCTC > 40000 && Balance > 0)
            {
                if (Obj.CalcGross > 40000 && Balance > 4000)
                {
                    Obj.LTA = 4000;
                }
                else if (Obj.CalcGross > 18000 && Balance > 2000)
                {
                    Obj.LTA = 2000;
                }
                else if (Balance > 1000)
                {
                    Obj.LTA = Balance;
                }
                Balance = Balance - Obj.LTA;
            }

            if (Balance > 0)
            {
                Obj.Special = Balance;
            }

            Obj.PT = objPayrollBLL.CalcPT(Obj.CalcGross, PayMonth, EmpGender);
            Obj.Insurance = Insurance;

            Obj.Net = Obj.CalcGross - Obj.PT - Obj.PF - Obj.Insurance;
            objPayrollBLL.AnnualCTC = Obj.AnnualCTC;
            objPayrollBLL.Special = Obj.Special;
            objPayrollBLL.Net = Obj.Net;
            objPayrollBLL.MonthlyCTC = Obj.MonthlyCTC;
            objPayrollBLL.CalcGratuity = Obj.CalcGratuity;
        }
        return Obj;
    }


    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        hdnEmpId.Value = ddlEmployee.SelectedValue;
        if (ddlEmployee.SelectedValue.ToString() == "0")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Please Select Employee')", true);
            return;
        }

        EmployeeMasterDAL objEmpDtl = new EmployeeMasterDAL();
        EmployeeMaster ObjEmp = new EmployeeMaster();
        ObjEmp = objEmpDtl.GetEmpDetailsByEmployeeId(Convert.ToInt32(hdnEmpId.Value));
        PayMonth = Convert.ToInt32(ddlMonth.SelectedValue.ToString());
        EmpGender = ObjEmp.empGender;

        string employeeName = ddlEmployee.SelectedItem.Text;
        string effectiveFrom = ddlMonth.SelectedItem.Text.ToString() + " " + ddlYear.SelectedValue.ToString();

        int annualCTC = Convert.ToInt32(txtAnnualFixedCTC.Text);
        int Bonus = Convert.ToInt32(txtAnnualBonus.Text);
        bool isPBB = chkFixed.Checked;
        bool isPF = chkPF.Checked;
        int insurance = Convert.ToInt32(txtInsurancePremium.Text);
        int CurrentPF = 0;

        Obj = SalaryCalc(annualCTC, Bonus, isPBB, isPF, insurance);

        hdnbonus.Value = Obj.Bonus.ToString();

        CultureInfo cuInfo = new CultureInfo("en-IN");


        if (isPBB == true)
        {

            lblBonus.Text = (Obj.Bonus.ToString("C", cuInfo)).Remove(0, 2).Trim() + " (Fixed)";
            hdnpbb.Value = "true";
        }
        else
        {
            lblBonus.Text = (Obj.Bonus.ToString("C", cuInfo)).Remove(0, 2).Trim() + " (Variable)";
            hdnpbb.Value = "false";
        }
        //var objPayrollBLL = CalculateGratuity(Obj);

        lblEmpName.Text = employeeName;
        lblEffectiveFrom.Text = effectiveFrom;
        //lblAnnualCTC.Text = (Obj.AnnualCTC.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblAnnualCTC.Text = (objPayrollBLL.AnnualCTC.ToString("C", cuInfo)).Remove(0, 2).Trim();
        
        lblBasicSal.Text = (Obj.Basic.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblHRA.Text = (Obj.HRA.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblConveyance.Text = (Obj.Conveyance.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblMedicalAllowance.Text = (Obj.Medical.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblFoodAllowance.Text = (Obj.Food.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblSpecialAllowance.Text = (objPayrollBLL.Special.ToString("C", cuInfo)).Remove(0, 2).Trim();
        
        lblNetSalary.Text = (objPayrollBLL.Net.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblLTA.Text = (Obj.LTA.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblProvidentFund.Text = (Obj.PF.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblInsurancePremium.Text = (Obj.Insurance.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblPT.Text = (Obj.PT.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblCtc.Text = (objPayrollBLL.MonthlyCTC.ToString("C", cuInfo)).Remove(0, 2).Trim();
        //lblGross.Text = (objPayrollBLL.Gross.ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblGross.Text = ((objPayrollBLL.MonthlyCTC - Obj.PF).ToString("C", cuInfo)).Remove(0, 2).Trim();
        //lblAnnualFixedCTC.Text = ((Obj.AnnualCTC - Obj.Bonus).ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblAnnualFixedCTC.Text = ((objPayrollBLL.MonthlyCTC * 12).ToString("C", cuInfo)).Remove(0, 2).Trim();
        lblgratuity.Text= (objPayrollBLL.CalcGratuity.ToString("C", cuInfo)).Remove(0, 2).Trim();
        hdnpt.Value = Obj.PT.ToString();
        showEmpSalData.Visible = true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        string EmpID = hdnEmpId.Value;

        Obj = new PayrollBLL();
        if (hdnEmpId.Value != null)
        {

            Obj.EmpID = Convert.ToInt32(hdnEmpId.Value);
        }
        else
        {
            Obj.EmpID = 0;
        }

        Obj.Gross = Convert.ToDecimal(lblGross.Text);
        Obj.MonthlyCTC = Convert.ToDecimal(lblCtc.Text);
        Obj.Net = Convert.ToDecimal(lblNetSalary.Text);
        Obj.Bonus = Convert.ToDecimal(hdnbonus.Value);
        if (hdnpbb.Value == "true")
        {
            Obj.PBB = true;
        }
        else if (hdnpbb.Value == "false")
        {
            Obj.PBB = false;
        }

        Obj.Basic = Convert.ToDecimal(lblBasicSal.Text);
        Obj.HRA = Convert.ToDecimal(lblHRA.Text);
        Obj.Conveyance = Convert.ToDecimal(lblConveyance.Text);
        Obj.Medical = Convert.ToDecimal(lblMedicalAllowance.Text);
        Obj.LTA = Convert.ToDecimal(lblLTA.Text);
        Obj.Food = Convert.ToDecimal(lblFoodAllowance.Text);
        Obj.Special = Convert.ToDecimal(lblSpecialAllowance.Text);
        Obj.PT = Convert.ToDecimal(hdnpt.Value);
        Obj.Insurance = Convert.ToDecimal(lblInsurancePremium.Text);
        Obj.RevisionDate = Convert.ToDateTime(lblEffectiveFrom.Text);
        Obj.PF = Convert.ToDecimal(lblProvidentFund.Text);
        Obj.CalcGratuity = Convert.ToDecimal(lblgratuity.Text);
        SalarySave(Obj);

        // clearFields();
        showEmpSalData.Visible = false;
    }

    public void BindMonths()
    {
        DateTimeFormatInfo info = new DateTimeFormatInfo();
        ddlMonth.Items.Clear();
        for (int i = 1; i < 13; i++)
        {
            ddlMonth.Items.Add(new ListItem(info.GetMonthName(i), i.ToString()));
        }

        ddlMonth.SelectedIndex = DateTime.Now.Month - 1;
    }

    public void BindYear()
    {
        ddlYear.Items.Clear();

        int i = DateTime.Now.Year;
        for (i = i - 1; i <= DateTime.Now.Year + 1; i++)
        {
            ddlYear.Items.Add(Convert.ToString(i));
        }
        ddlYear.SelectedValue = DateTime.Now.Year.ToString();
    }

    public void bindDropdown()
    {
        DataTable dt = CSCode.Global.GetEmployees(Int32.Parse(Session["LocationID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ddlEmployee.DataTextField = dt.Columns["empName"].ToString();
            ddlEmployee.DataValueField = dt.Columns["empid"].ToString();
            ddlEmployee.DataSource = dt;
            ddlEmployee.DataBind();
        }
    }


    public void clearFields()
    {
        ddlEmployee.ClearSelection();
        ddlMonth.ClearSelection();
        ddlYear.ClearSelection();
        txtAnnualFixedCTC.Text = string.Empty;
        txtAnnualBonus.Text = "0";
        chkFixed.Checked = false;
        chkPF.Checked = false;
        txtInsurancePremium.Text = "0";
    }
    public PayrollBLL CalculateGratuity(PayrollBLL objPayrollBLL)
    {
        decimal empPayGratuity = 4.81m;
        string empPayGratuityConfigValue= System.Configuration.ConfigurationManager.AppSettings["empPayGratuity"];
        if (!string.IsNullOrEmpty(empPayGratuityConfigValue))
        {
            empPayGratuity = Convert.ToDecimal(empPayGratuityConfigValue);
        }
        PayrollBLL _PayrollBLL = new PayrollBLL();
        decimal gratuity = Math.Floor((objPayrollBLL.Basic * empPayGratuity) / 100);

        _PayrollBLL.Special = objPayrollBLL.Special;// - gratuity;
        _PayrollBLL.CalcGratuity = gratuity;
        _PayrollBLL.Gross = objPayrollBLL.Gross;
        _PayrollBLL.Net = objPayrollBLL.Net;// - gratuity;
        _PayrollBLL.MonthlyCTC = Obj.MonthlyCTC;// - gratuity;
        _PayrollBLL.AnnualCTC = (objPayrollBLL.AnnualCTC);// - (gratuity * 12);
        return _PayrollBLL;
    }

}