using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_PayAdd : Authentication
{
    clsCommon objCommon = new clsCommon();
    UserMaster UM;
    PayrollBLL objPayrollBLL = new PayrollBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UM = UserMaster.UserMasterInfo();
            if (UM == null)
                return;

            Admin MasterPage = (Admin)Page.Master;
            MasterPage.MasterInit(false, true, false, false);
            hdLocationId.Value = HttpContext.Current.Session["LocationID"].ToString(); 

            //BindLocation();
            GetMonths();

            hdnCurrMonth.Value = (DateTime.Today.Month - 1).ToString();
            hdnCurrYear.Value = DateTime.Today.Year.ToString();
            hdnCurrLoc.Value = hdLocationId.Value;
        }
    }

    public void GetMonths()
    {
        DateTimeFormatInfo info = new DateTimeFormatInfo();
        for (int i = 1; i < 13; i++)
        {
            dlMonth.Items.Add(new ListItem(info.GetMonthName(i), i.ToString()));
        }
        dlMonth.SelectedValue = (DateTime.Today.Month - 1).ToString();
        txtYear.Text = DateTime.Today.Year.ToString();
        //hdnNoOfDays.Value = Convert.ToString(DateTime.DaysInMonth(DateTime.Today.Year, (DateTime.Today.Month - 1)));
         hdnNoOfDays.Value = Convert.ToString(DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Now.AddMonths(-1).Month));

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
    //    hdLocationId.Value = dlLocation.SelectedValue;
    //    dlLocation.Visible = true;
    //}


    [System.Web.Services.WebMethod]
    public static String IFExistsPayProcess(int? month, int? year)
    {
        string locName = HttpContext.Current.Session["LocationName"].ToString();
        try
        {
            bool output = PayrollBLL.IFExistsPayProcess(month, year, Convert.ToInt32(HttpContext.Current.Session["LocationID"].ToString()));
            string isrecordexists = "false";
            if (output == true)
            {
                isrecordexists = "true";
            }

            Hashtable hashtable = new Hashtable();
            hashtable.Add("key", "Record exists");
            hashtable.Add("value", isrecordexists);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(hashtable);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string BindAddPayDetails(int? year, int? month, int? noOfdays)
    {
        try
        {
            List<PayrollBLL> lsPayroll = new List<PayrollBLL>();
            List<PayrollBLL> lsPayroll_History = new List<PayrollBLL>();

            lsPayroll = PayrollBLL.GetAddPayDetails("GETDETAIL", year, month, Convert.ToInt32(HttpContext.Current.Session["LocationID"].ToString()), noOfdays);

            lsPayroll_History = PayrollBLL.GetAddPayDetails_History("GETSPAYPROCDET_HST", Convert.ToInt32(HttpContext.Current.Session["LocationID"].ToString()));

            var data = from pr in lsPayroll
                       join pr_hst in lsPayroll_History on pr.EmpID equals pr_hst.EmpID
                       into temp  from pr_hst1 in temp.DefaultIfEmpty()
                       select new
                       {
                           pr.EmpID,
                           pr.EmpName,
                           pr.Gross,
                           pr.IsPF,
                           pr.PT,
                           pr.Insurance,
                           pr.Basic,
                           pr.HRA,
                           pr.Conveyance,
                           pr.Medical,
                           pr.Food,
                           pr.Special,
                           pr.LTA,
                           pr.Days,
                           pr.Leaves,
                           Ded_Loan=(pr_hst1 == null ? 0 : pr_hst1.Ded_Loan),
                           Ded_Advance=(pr_hst1 == null ? 0: pr_hst1.Ded_Advance),
                           Ded_Tax=(pr_hst1 == null ? 0 : pr_hst1.Ded_Tax),
                           Ded_Deduction=(pr_hst1 == null ? 0 : pr_hst1.Ded_Deduction),
                           Add_Bonus=(pr_hst1 == null ? 0 : pr_hst1.Add_Bonus),
                           Add_Addition=(pr_hst1 == null ? 0 : pr_hst1.Add_Addition),
                           pr.Net,
                           pr.CTC,
                           pr.TotalAddition,
                           pr.TotalDeduction,
                           pr.Ded_Leave,
                           Remark = (pr_hst1 == null ? String.Empty : pr_hst1.Remark),
                           pr.CalcGross,
                           pr.Presents,
                           pr.CalcBasic,
                           pr.Gender,
                           pr.CalcGratuity,
                           pr.PF
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }

    }


    public T ConvertJSonToObject<T>(string jsonString)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)serializer.ReadObject(ms);
        return obj;
    }

    [System.Web.Services.WebMethod]
    public static string CalculateData(string hdnJSONData, int month)
    {
        Member_PayAdd helper = new Member_PayAdd();
        PayrollBLL objPayroll = new PayrollBLL();
        List<PayrollBLL> pay = new List<PayrollBLL>();
        pay = null;
        try
        {
            pay = helper.ConvertJSonToObject<List<PayrollBLL>>((hdnJSONData));
            if (pay.Count > 0)
            {
                PayrollBLL.IFExistsPayProcess_History("ISEXISTSPAYPROCDET_HST", Convert.ToInt32(HttpContext.Current.Session["LocationID"].ToString()));
                Member_PayAdd.SavePayProcessDetails_History(pay);
            }
            for (int i = 0; i < pay.Count(); i++)
            {
                decimal CalcBasic;
                //decimal CalcCTC;
                //decimal AnnualGratuity= Math.Round(pay[i].CalcGratuity * 12);
                pay[i].Ded_Leave = Math.Round((pay[i].Leaves / pay[i].Days) * pay[i].Gross);
                CalcBasic = pay[i].Basic - Math.Round((pay[i].Leaves / pay[i].Days) * pay[i].Basic);
                pay[i].CalcBasic = CalcBasic;
                pay[i].CalcGross = pay[i].Gross - pay[i].Ded_Leave;
                if (pay[i].IsPF)
                {
                    pay[i].PF = objPayroll.CalcPF(pay[i].EmpID, pay[i].CTC, CalcBasic);
                }

                //decimal CTC = Math.Ceiling(((pay[i].AnnualCTC + AnnualGratuity) - pay[i].Bonus) / 12);

                //CalcCTC = pay[i].Basic + pay[i].HRA + pay[i].Conveyance + pay[i].Medical + pay[i].LTA + pay[i].Food + pay[i].Special + pay[i].PF;
                //if (pay[i].IsPF)
                //{
                //    pay[i].PF = objPayroll.CalcPF(pay[i].EmpID, pay[i].CTC, pay[i].Basic);
                //}

                #region Old PT Calculation
                //if (pay[i].PT == 200 && month == 2)
                //{
                //    pay[i].PT = 300;
                //}
                //if (pay[i].Gender == "Female" && pay[i].Gross <= 10000)
                //{
                //    pay[i].PT = 0;
                //} 
                #endregion

                pay[i].PT = objPayroll.CalcPT(pay[i].Gross, month, pay[i].Gender);
                pay[i].TotalAddition = pay[i].Add_Bonus + pay[i].Add_Addition;
                pay[i].TotalDeduction = pay[i].Ded_Loan + pay[i].Ded_Advance + pay[i].Ded_Tax + pay[i].Ded_Deduction + pay[i].Ded_Leave + pay[i].PF + pay[i].PT + pay[i].Insurance;// + pay[i].CalcGratuity;
                pay[i].Net = pay[i].Gross - pay[i].TotalDeduction + pay[i].TotalAddition;
                pay[i].CTC = pay[i].Gross + pay[i].PF;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(pay);
        }

        catch (Exception)
        {
            return null;
        }

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static bool SaveSalary(int? month, int? year, string PayComment, string calculatedData)
    {
        Member_PayAdd helper = new Member_PayAdd();
        List<PayrollBLL> pay = new List<PayrollBLL>();
        pay = helper.ConvertJSonToObject<List<PayrollBLL>>((calculatedData));
        int payId = 0;
        try
        {
            if (pay.Count() > 0)
            {
                payId = PayrollBLL.InsertemployeePayProcess("INSERTPAYPROC", Convert.ToInt32(HttpContext.Current.Session["LocationID"].ToString()), Convert.ToDateTime(year + "-" + month + "-01"), PayComment, Convert.ToDateTime(DateTime.Now), false);
                if (payId == 0)
                    return false;
                PayrollBLL objpay = new PayrollBLL();
                objpay.Mode = "INSERTPAYPROCDETAIL";
                objpay.PayId = payId;
                for (int i = 0; i < pay.Count(); i++)
                {
                    objpay.EmpID = pay[i].EmpID;
                    objpay.Basic = pay[i].Basic;
                    objpay.HRA = pay[i].HRA;
                    objpay.Conveyance = pay[i].Conveyance;
                    objpay.Medical = pay[i].Medical;
                    objpay.Food = pay[i].Food;
                    objpay.Special = pay[i].Special;
                    objpay.LTA = pay[i].LTA;
                    objpay.PF = pay[i].PF;
                    objpay.PT = pay[i].PT;
                    objpay.Insurance = pay[i].Insurance;
                    objpay.Ded_Loan = pay[i].Ded_Loan;
                    objpay.Ded_Advance = pay[i].Ded_Advance;
                    objpay.Leaves = pay[i].Leaves;
                    objpay.Days = pay[i].Presents;
                    objpay.Ded_Leave = pay[i].Ded_Leave;
                    objpay.Ded_Deduction = pay[i].Ded_Deduction;
                    objpay.Ded_Tax = pay[i].Ded_Tax;
                    objpay.Add_Bonus = pay[i].Add_Bonus;
                    objpay.Add_Addition = pay[i].Add_Addition;
                    objpay.Remark = pay[i].Remark;
                    objpay.CTC = pay[i].CTC;
                    objpay.Gross = pay[i].Gross;
                    objpay.Net = pay[i].Net;
                    //objpay.CalcBasic = pay[i].CalcBasic;
                    //objpay.CalcGross = pay[i].CalcGross;

                    PayrollBLL.InsertemployeePayProcessDetails(objpay);
                }
            }
            HttpContext.Current.Session["PayId"] = payId;
            return true;
        }

        catch (Exception)
        {
            IsSuccessPayProcess(payId);
            return false;
        }
    }

    [System.Web.Services.WebMethod]
    public static string getNoofDays(int? month, int? year)
    {
        return Convert.ToString(DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)));

    }

    public static void IsSuccessPayProcess(int? payId)
    {
        try
        {
            PayrollBLL.IsSuccessPayProcess(payId);
        }
        catch (Exception ex)
        {
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static bool SavePayProcessDetails_History(List<PayrollBLL> pay_History)
    {
        Member_PayAdd helper = new Member_PayAdd();
        try
        {
            if (pay_History.Count() > 0)
            {
                PayrollBLL objpay_History = new PayrollBLL();
                objpay_History.Mode = "INSERTPAYPROCDET_HST";
                objpay_History.locationId = Convert.ToInt32(HttpContext.Current.Session["LocationID"].ToString());

                for (int i = 0; i < pay_History.Count(); i++)
                {
                    objpay_History.EmpID = pay_History[i].EmpID;
                    objpay_History.Ded_Loan = pay_History[i].Ded_Loan;
                    objpay_History.Ded_Advance = pay_History[i].Ded_Advance;
                    objpay_History.Ded_Deduction = pay_History[i].Ded_Deduction;
                    objpay_History.Ded_Tax = pay_History[i].Ded_Tax;
                    objpay_History.Add_Bonus = pay_History[i].Add_Bonus;
                    objpay_History.Add_Addition = pay_History[i].Add_Addition;
                    //objpay_History.Remark = pay_History[i].Remark;
                    objpay_History.Remark = pay_History[i].Remark.Replace('|', '\'');
                    PayrollBLL.InsertPayProcessDetails_History(objpay_History);
                }
            }
            return true;
        }

        catch (Exception)
        {
            return false;
        }
    }

    public bool IFExistsPayProcess_History(string mode)
    {
        return PayrollBLL.IFExistsPayProcess_History(mode, Convert.ToInt32(HttpContext.Current.Session["LocationID"].ToString()));
    }
}

