using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PayrollBLL
{
    public string CompName { get; set; }
    public string CompAddress { get; set; }
    public string EmpPAN { get; set; }
    public string EmpUAN { get; set; }
    public string EmpEPF { get; set; }

    public int EmpID { get; set; }
    public string EmpName { get; set; }
    public string SkillDesc { get; set; }
    public DateTime EmpJoiningDate { get; set; }
    public decimal PastExperince { get; set; }
    public decimal TotalExperince { get; set; }
    public DateTime RevisionDate { get; set; }
    public decimal Gross { get; set; }
    public decimal Bonus { get; set; }
    public decimal PF { get; set; }
    public decimal MonthlyCTC { get; set; }
    public decimal AnnualCTC { get; set; }
    public decimal Net { get; set; }
    public decimal PT { get; set; }
    public decimal Insurance { get; set; }
    public Boolean PBB { get; set; }
    public decimal Basic { get; set; }
    public decimal HRA { get; set; }
    public decimal Conveyance { get; set; }
    public decimal Medical { get; set; }
    public decimal Food { get; set; }
    public decimal Special { get; set; }
    public decimal LTA { get; set; }
    public DateTime PayDate { get; set; }
    public decimal Loan { get; set; }
    public decimal Advance { get; set; }
    public decimal Leaves { get; set; }
    public decimal Presents { get; set; }
    public decimal Tax { get; set; }
    public decimal Deduction { get; set; }
    public decimal Addition { get; set; }
    public string Remark { get; set; }
    public decimal CTC { get; set; }
    public decimal TotalAddition { get; set; }
    public decimal TotalDeduction { get; set; }
    public string Comment { get; set; }

    public decimal Ded_Loan { get; set; }
    public decimal Ded_Advance { get; set; }
    public decimal Ded_Tax { get; set; }
    public decimal Ded_Deduction { get; set; }
    public decimal Ded_Leave { get; set; }
    public decimal Add_Bonus { get; set; }
    public decimal Add_Addition { get; set; }
    public bool IsPF { get; set; }
    public decimal Days { get; set; }
    public decimal AB { get; set; }
    public int PayId { get; set; }
    public string Mode { get; set; }
    public string AccountNo { get; set; }
    public int locationId { get; set; }
    public string BankAddress { get; set; }
    public string BankAccount { get; set; }
    public string Logo { get; set; }
    public decimal CalcGross { get; set; }
    public decimal CalcBasic { get; set; }
    public string Gender { get; set; }
    public decimal CalcGratuity { get; set; }

    private string _IFSCCode = string.Empty;      //ifsccode

    public string IFSCCOde
    {
        get { return _IFSCCode; }
        set { _IFSCCode = value; }
    }




    public PayrollBLL()
    {
    }

    public PayrollBLL(string CompName, string CompAddress, string BankAddress, string BankAccount, string Logo)
    {
        this.CompName = CompName;
        this.CompAddress = CompAddress;
        this.BankAddress = BankAddress;
        this.BankAccount = BankAccount;
        this.Logo = Logo;
    }

    public PayrollBLL(int EmpID, string EmpName,  DateTime EmpJoiningDate, decimal PastExperince, decimal TotalExperince,
            DateTime RevisionDate, decimal Gross, decimal Bonus, decimal PF, decimal MonthlyCTC, decimal AnnualCTC, decimal Net,
            decimal PT, decimal Insurance, Boolean PBB, decimal Basic, decimal HRA, decimal Conveyance, decimal Medical,
            decimal Food, decimal Special, decimal LTA)
    {
        this.EmpID = EmpID;
        this.EmpName = EmpName;
        this.EmpJoiningDate = EmpJoiningDate;
        this.PastExperince = PastExperince;
        this.TotalExperince = TotalExperince;
        this.RevisionDate = RevisionDate;
        this.Gross = Gross;
        this.Bonus = Bonus;
        this.PF = PF;
        this.MonthlyCTC = MonthlyCTC;
        this.AnnualCTC = AnnualCTC;
        this.Net = Net;
        this.PT = PT;
        this.Insurance = Insurance;
        this.PBB = PBB;
        this.Basic = Basic;
        this.HRA = HRA;
        this.Conveyance = Conveyance;
        this.Medical = Medical;
        this.Food = Food;
        this.Special = Special;
        this.LTA = LTA;
    }

    public PayrollBLL(int EmpID, string EmpName, DateTime PayDate, decimal Basic, decimal HRA, decimal Conveyance, decimal Medical, decimal Food,
            decimal Special, decimal LTA, decimal PF, decimal PT, decimal Insurance,
            decimal Loan, decimal Advance, decimal Leaves, decimal Presents, decimal Tax, decimal Deduction, decimal Bonus,
            decimal Addition, string Remark, decimal CTC, decimal Gross, decimal Net, decimal TotalAddition, decimal TotalDeduction, string Comment)
    {
        this.EmpID = EmpID;
        this.EmpName = EmpName;
        this.PayDate = PayDate;
        this.Basic = Basic;
        this.HRA = HRA;
        this.Conveyance = Conveyance;
        this.Medical = Medical;
        this.Food = Food;
        this.Special = Special;
        this.LTA = LTA;
        this.PF = PF;
        this.PT = PT;
        this.Insurance = Insurance;
        this.Loan = Loan;
        this.Advance = Advance;
        this.Leaves = Leaves;
        this.Presents = Presents;
        this.Tax = Tax;
        this.Deduction = Deduction;
        this.Bonus = Bonus;
        this.Addition = Addition;
        this.Remark = Remark;
        this.CTC = CTC;
        this.Gross = Gross;
        this.Net = Net;
        this.TotalAddition = TotalAddition;
        this.TotalDeduction = TotalDeduction;
        this.Comment = Comment;
    }

    public PayrollBLL(int EmpID, string EmpName, decimal Gross, bool IsPF, decimal PT, decimal Insurance,
            decimal Basic, decimal HRA, decimal Conveyance, decimal Medical, decimal Food, decimal Special, decimal LTA,
            decimal Leaves, decimal Days, decimal Presents, string Gender, decimal Gratuity, decimal MonthlyCTC, decimal PF)//,decimal AnnualCTC
    {
        this.EmpID = EmpID;
        this.EmpName = EmpName;
        this.Gross = Gross;
        this.IsPF = IsPF;
        this.PT = PT;
        this.Insurance = Insurance;
        this.Basic = Basic;
        this.HRA = HRA;
        this.Conveyance = Conveyance;
        this.Medical = Medical;
        this.Food = Food;
        this.Special = Special;
        this.LTA = LTA;
        this.Leaves = Leaves;
        this.Days = Days;
        this.Presents = Presents;
        this.Gender = Gender;
        this.CalcGratuity = Gratuity;
        this.CTC = MonthlyCTC;
        this.PF = PF;
    }

    public PayrollBLL(int EmpID, string EmpName, DateTime PayDate, string SkillDesc, DateTime EmpJoiningDate, decimal Basic, decimal HRA, decimal Conveyance,
                        decimal Medical, decimal Food, decimal Special, decimal LTA, decimal PF, decimal PT, decimal Insurance, decimal Loan, decimal Advance, decimal Leaves,
                        decimal Presents, decimal Tax, decimal Deduction, decimal Ded_Leave, decimal Bonus, decimal Addition, string Remark, decimal CTC, decimal Gross,
                        decimal AB, decimal Net, decimal TotalAddition, decimal TotalDeduction, string Comment, int Payid, decimal Days, decimal CalcBasic, decimal CalcGross, string EmpPAN, string EmpUAN, string EmpEPF)
    {
        this.EmpID = EmpID;
        this.EmpName = EmpName;
        this.PayDate = PayDate;
        this.SkillDesc = SkillDesc;
        this.EmpJoiningDate = EmpJoiningDate;
        this.Basic = Basic;
        this.HRA = HRA;
        this.Conveyance = Conveyance;
        this.Medical = Medical;
        this.Food = Food;
        this.Special = Special;
        this.LTA = LTA;
        this.PF = PF;
        this.PT = PT;
        this.Insurance = Insurance;
        this.Loan = Loan;
        this.Advance = Advance;
        this.Leaves = Leaves;
        this.Presents = Presents;
        this.Tax = Tax;
        this.Deduction = Deduction;
        this.Bonus = Bonus;
        this.Addition = Addition;
        this.Remark = Remark;
        this.CTC = CTC;
        this.Gross = Gross;
        this.AB = AB;
        this.Net = Net;
        this.TotalAddition = TotalAddition;
        this.TotalDeduction = TotalDeduction;
        this.Comment = Comment;
        this.Ded_Leave = Ded_Leave;
        this.PayId = Payid;
        this.Days = Days;
        this.CalcBasic = CalcBasic;
        this.CalcGross = CalcGross;
        this.EmpPAN = EmpPAN;
        this.EmpUAN = EmpUAN;
        this.EmpEPF = EmpEPF;

    }
    public PayrollBLL(int EmpID, string EmpName, DateTime EmpJoiningDate, DateTime PayDate, decimal Net,
            string AccountNo, int payId, String iIFSCCode, int LocId)   //new page ifsccode
    {
        this.EmpID = EmpID;
        this.EmpName = EmpName;
        this.EmpJoiningDate = EmpJoiningDate;
        this.PayDate = PayDate;
        this.AccountNo = AccountNo;
        this.Net = Net;
        this.PayId = payId;
        
        this.locationId = LocId;
       
        
    }
    public PayrollBLL(int EmpID, string EmpName, DateTime EmpJoiningDate, DateTime PayDate, decimal Net,
           string AccountNo, int payId, int LocId, string IFSCCode)
    {
        this.EmpID = EmpID;
        this.EmpName = EmpName;
        this.EmpJoiningDate = EmpJoiningDate;
        this.PayDate = PayDate;
        this.AccountNo = AccountNo;
        this.Net = Net;
        this.PayId = payId;
        this.locationId = LocId;
        this.IFSCCOde = IFSCCode;//IFSCCOde
    }

    public PayrollBLL(int LocId, int EmpID, decimal Ded_Loan, decimal Ded_Advance, decimal Ded_Tax,
           decimal Ded_Deduction, decimal Add_Bonus, decimal Add_Addition, string Remark)
    {
        this.locationId = LocId;
        this.EmpID = EmpID;
        this.Ded_Loan = Ded_Loan;
        this.Ded_Advance = Ded_Advance;
        this.Ded_Tax = Ded_Tax;
        this.Ded_Deduction = Ded_Deduction;
        this.Add_Bonus = Add_Bonus;
        this.Add_Addition = Add_Addition;
        this.Remark = Remark;
    }
    //public PayrollBLL(int LocId, int EmpID, decimal Ded_Loan, decimal Ded_Advance, decimal Ded_Tax,
    //       decimal Ded_Deduction, decimal Add_Bonus, decimal Add_Addition, string Remark)// string IFSCCode)
    //{
    //    this.locationId = LocId;
    //    this.EmpID = EmpID;
    //    this.Ded_Loan = Ded_Loan;
    //    this.Ded_Advance = Ded_Advance;
    //    this.Ded_Tax = Ded_Tax;
    //    this.Ded_Deduction = Ded_Deduction;
    //    this.Add_Bonus = Add_Bonus;
    //    this.Add_Addition = Add_Addition;
    //    this.Remark = Remark;
    //    /* this.IFSCCOde = IFSCCode;*/    //ifsc
    //}

    //public static List<PayrollBLL> GetCompDetails(string mode, int locationId)
    //{
    //    PayrollDAL objCompDetail = new PayrollDAL();
    //    return objCompDetail.GetCompDetails(mode, locationId);
    //}


    public static List<PayrollBLL> GetPayrollDetails(string mode, bool? isActive, int? locationId, int? EmpId)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.GetPayrollDetails(mode, isActive, locationId, EmpId);
    }

    public static List<PayrollBLL> GetPayrollDetails(int? EmpId)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.GetPayrollDetails(null, null, null, EmpId);
    }

    public static List<PayrollBLL> GetSalaryDetails(string mode, int? EmpId, int? year, int? month, int? locationId)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.GetSalaryDetails(mode, EmpId, year, month, locationId);
    }

    public static List<PayrollBLL> GetSalaryDetails(string mode, int? EmpId, int? year, int? month)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.GetSalaryDetails(mode, EmpId, year, month, null);
    }

    public static bool IFExistsPayProcess(int? month, int? year, int? locationId)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.IFExistsPayProcess(month, year, locationId);
    }

    public static List<PayrollBLL> GetAddPayDetails(string mode, int? year, int? month, int? locationId, int? days)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.GetAddPayDetails(mode, year, month, locationId, days);
    }

    public int CalcPT(decimal Gross, int? month, string EmpGender)
    {
        int PT = 0;
        #region Old PT calculation
        //if (Gross > 0)
        //{
        //    if (Gross < 7501)
        //    {
        //        PT = 0;
        //    }
        //    else if (Gross < 10000)
        //    {
        //        PT = 175;
        //    }
        //    else
        //    {
        //        if (month == 2) //Feb
        //            PT = 300;
        //        else
        //            PT = 200;
        //    }
        //    if (EmpGender == "Female" && Gross <= 10000)
        //    {
        //        PT = 0;
        //    }

        //} 
        #endregion
        #region New PT Calculation Change from 3 Jul 2023
        if (string.Compare(EmpGender, "Female", true) == 0)
        {
            if (Gross <= 25000)
            {
                PT = 0;
            }
            else if (Gross > 25000)
            {
                if (month == 2)
                {
                    PT = 300;
                }
                else
                {
                    PT = 200;
                }
            }
        }
        else
        {
            if (Gross <= 7500)
            {
                PT = 0;
            }
            else if (Gross > 7500 && Gross <= 10000)
            {
                PT = 175;
            }
            else if (Gross > 10000)
            {
                if (month == 2)
                {
                    PT = 300;
                }
                else
                {
                    PT = 200;
                }
            }
        }
        #endregion
        //System.IO.File.AppendAllText(@"C:\inetpub\wwwroot\Agora_Member\CalPTLog_" + DateTime.Now.ToShortDateString() +".txt", Environment.NewLine + "Gross=" + Convert.ToString(Gross) + " month=" + Convert.ToString(month) + " EmpGender=" + Convert.ToString(EmpGender) + " PT=" + Convert.ToString(PT));
        return PT;
    }

    public static int InsertemployeePayProcess(string mode, int? locationId, DateTime? payDate, string payComment, DateTime? payProcessDate, bool? payStatus)
    {
        PayrollDAL objInsertInto = new PayrollDAL();
        return objInsertInto.InsertPayProcess(mode, locationId, payDate, payComment, payProcessDate, payStatus);
    }

    public static void InsertemployeePayProcessDetails(PayrollBLL objInsert)
    {
        PayrollDAL objInsertInto = new PayrollDAL();
        objInsertInto.InsertPayProcessDetails(objInsert);
    }

    public static void InsertPayProcessDetails_History(PayrollBLL objInsert_History)
    {
        PayrollDAL objInsertInto = new PayrollDAL();
        objInsertInto.InsertPayProcessDetails_History(objInsert_History);
    }

    public static List<PayrollBLL> GetPayStatementDetails(string mode, int? PayId, string ListEmpids = null)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.GetPayStatementDetails(mode, PayId, ListEmpids);
    }

    public static void IsSuccessPayProcess(int? payId)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        objPayrollDAL.IsSuccessPayProcess(payId);
    }

    public static bool IFExistsPayProcess_History(string mode, int? locationId)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.IFExistsPayProcess_History(mode, locationId);
    }

    public static List<PayrollBLL> GetAddPayDetails_History(string mode, int? locationId)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.GetAddPayDetails_History(mode, locationId);
    }
    public string GetEmpGender(string mode, int? EmpId)
    {
        PayrollDAL objPayrollDAL = new PayrollDAL();
        return objPayrollDAL.GetEmpGender(mode, EmpId);
    }

    public decimal CalcPF(int EmpID, decimal CTC = 0, decimal Basic = 0)
    {
        decimal PF = 0;
        double Factor = 0.04580152672; //PF on 40% basic
        double DefaultPFPercent = 0.12;
        //Added by Heramb
        List<ConfigBLL> curConfig = new List<ConfigBLL>();
        curConfig = ConfigBLL.GetConfigDetails("GetConfig", 49);
        /////////////////////////////////////////////
        //if (CTC > 0)
        //{
        //if (CTC <= 19999)
        //{
        //    Factor = 0.056603774; //PF on 50% basic
        //}

        // PF = CTC * Convert.ToDecimal(Factor);
        //}
        ///else
        //{
        //PF = Basic * Convert.ToDecimal(0.1);
        if(curConfig!=null)
        {
            PF = Basic * Convert.ToDecimal(curConfig[0].value);
        }
        else
        {
            PF = Basic * Convert.ToDecimal(DefaultPFPercent);
        }
        //}

        if (PF > 1800 && !"1000,1001,1089,2475,2476".Contains(EmpID.ToString()))
        {
            PF = 1800;
        }

        return (int)Math.Round(PF);
    }
}