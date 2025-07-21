using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using Customer.Model;
using System.Globalization;
using System.Web.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ProjecInvoiceBLL
/// </summary>
namespace Customer.BLL
{
    public class GetInvoice
    {
        public int ProjectInvoiceID { get; set; }
        public int ProjID { get; set; }
        public int CurrencyID { get; set; }
        public float ExRate { get; set; }
        public string projName { get; set; }
        public int custId { get; set; }
        public string custName { get; set; }
        public string InvoiceDate { get; set; }
        public string currSymbol { get; set; }
        public float TotalAmount { get; set; }
        public string Amount { get; set; }

        public int PaymentType { get; set; }
        public string Description { get; set; }
        public int ProjectDetailID { get; set; }
        public int PaymentID { get; set; }
        public string BalanceAmount { get; set; }
        public string ReceiptDate { get; set; }
        public float TaxCollected { get; set; }

        public GetInvoice()
        {

        }

        public List<GetInvoice> GetInvoices(int ProjID, string FromDate, string ToDate)
        {
            ProjectInvoiceDAL objInvoice = new ProjectInvoiceDAL();
            return objInvoice.GetInvoices(ProjID,FromDate,ToDate);

        }

        public DataSet GetMonthlyRevenue(string FromDate, string ToDate)
        {
            ProjectInvoiceDAL objInvoice = new ProjectInvoiceDAL();
            return objInvoice.GetMonthlyRevenue(FromDate, ToDate);

        }
    }
    
    public class InvoicePaymentModel
    {
        public int ProjectID { get; set; }
        public int PaymentType { get; set; }
        public string Description { get; set; }
        public int ProjectDetailID { get; set; }
        public int PaymentID { get; set; }
        public int ProjectInvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public float Amount { get; set; }
        public string invoiceDate { get; set; }
        public float payAmount { get; set; }
        public float InvBalance { get; set; }
        public int InsertedBy { get; set; }
        public int isEdited { get; set; }
        public string DisplayAmount { get; set; }
        public string DisplayBalance { get; set; }
        public float PreviousBalance { get; set; }
        public string IDs { get; set; }
        public float AppliedCreditAmount { get; set; }
        public string CreditedPaymentID { get; set; }
        public int counter { get; set; }
        public bool bFlag { get; set; }
        public float AmountEntered { get; set; }

        public float TaxCollected { get; set; }

        public InvoicePaymentModel(int ProjectID, int PaymentType, string Description, int ProjectDetailID, int PaymentID, int ProjectInvoiceID, float Amount, int InsertedBy, float payAmount, float TaxCollected)
        {
            this.ProjectID = ProjectID;
            this.PaymentType = PaymentType;
            this.Description = Description;
            this.ProjectDetailID = ProjectDetailID;
            this.PaymentID = PaymentID;
            this.ProjectInvoiceID = ProjectInvoiceID; 
            this.Amount = Amount;
            this.InsertedBy=InsertedBy;
            this.payAmount = payAmount;
            this.TaxCollected = TaxCollected;
        }

        public InvoicePaymentModel()
        {
            // TODO: Complete member initialization
        }

        public int SaveInvoicePayment(int ProjectDetailID, int ProjectInvoiceID, int projId, int paymentID, string Description, int paymentType, float Amount, float PayAmount,float TaxCollected, float invBalance, int isEdited, string invoiceIDs, float AppliedCreditAmount,int Counter,string CreditedPaymentID, bool bFlag,int InsertedBy,ProjecInvoiceBLL header)
        {
            InvoicePaymentModel objInvoice = new InvoicePaymentModel();
            objInvoice.ProjectInvoiceID = ProjectInvoiceID;
            objInvoice.ProjectID = projId;
            objInvoice.PaymentID = paymentID;
            objInvoice.Description = Description;
            objInvoice.payAmount = PayAmount;
            objInvoice.InsertedBy = InsertedBy;
            objInvoice.ProjectDetailID = ProjectDetailID;
            objInvoice.isEdited = isEdited;
            objInvoice.IDs = invoiceIDs;
            objInvoice.AppliedCreditAmount = AppliedCreditAmount;
            objInvoice.counter = Counter;
            objInvoice.CreditedPaymentID = CreditedPaymentID;
            objInvoice.bFlag = bFlag;
            objInvoice.TaxCollected = TaxCollected;
            ProjectInvoiceDAL objInsertInvoice = new ProjectInvoiceDAL();
            if (objInvoice.payAmount > 0)
            {
                if (isEdited > 0)
                    objInvoice.InvBalance =Math.Abs((invBalance - PayAmount));
                else
                    objInvoice.InvBalance =Math.Abs((Amount - PayAmount));

                
                return objInsertInvoice.SavePaymentDetails(objInvoice,header);
            }
            else if (objInvoice.payAmount == 0 && isEdited == 0)
            {
                objInvoice.InvBalance = Math.Abs((invBalance - PayAmount));
                 return objInsertInvoice.SavePaymentDetails(objInvoice,header);
            }
            else
                return 0;

        }

    }

    public class ProjecInvoiceBLL
    {
        public int ProjectPaymentID { get; set; }
        public int ProjID { get; set; }
        public float Amount { get; set; }
        public float BalanceAmount { get; set; }
        public int InsertedBy { get; set; }
        public string customerName { get; set; }
        public string customerAddress { get; set; }
        public int CurrencyID  { get; set; }
        public float ExRate  { get; set; }
        public string InvoiceDate    { get; set; }          
        public string currSymbol  { get; set; }
        public float Balance { get; set; }
        public int PaymentType { get; set; }
        public float CreditAmount { get; set; }
        public string Description { get; set; }
        public bool isDiabled { get; set; }
        public bool isCredited { get; set; }
        public int isEdited { get; set; }
        public string PTypes { get; set; }
        public string ProjectName{get;set;}
        public float PreviousCredit { get; set; }
        public float AppliedCreditAmount { get; set; }
        public string CreditedPaymentID { get; set; }
        public string NoOfCredit { get; set; }
        public float TaxCollected { get; set; }

        public List<InvoicePaymentModel> lstModel = new List<InvoicePaymentModel>();

        public ProjecInvoiceBLL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int SavePaymentHeader(int ProjectPaymentID, int ProjID, float Amount,float TaxCollected, int PaymentType, int CurrencyID, float ExRate, float creditAmount, string description, int InsertedBy, bool isCredit, string PaymentDate = "")
        {
            ProjecInvoiceBLL objheader = new ProjecInvoiceBLL();
            objheader.ProjID = ProjID;
            objheader.ProjectPaymentID = ProjectPaymentID;
            objheader.Amount = Amount;
            objheader.PaymentType = PaymentType;
            objheader.InsertedBy = InsertedBy;
            objheader.CurrencyID = CurrencyID;
            objheader.ExRate = ExRate;
            objheader.CreditAmount = creditAmount;
            objheader.Description = description;
            objheader.InvoiceDate = PaymentDate;
            objheader.isCredited = isCredit;
            objheader.TaxCollected = TaxCollected;
            //objInvoiceheader.TotalAmount = TotalAmt;
            //objInvoiceheader.Comment = Comment;
            
            ProjectInvoiceDAL objSaveheader = new ProjectInvoiceDAL();
            return objSaveheader.SavePaymentHeader(objheader); 
        
        }


        public static List<KeyValueModel> GetPaymentType()
        {
            ProjectInvoiceDAL objPaymentType = new ProjectInvoiceDAL();
            return objPaymentType.GetPaymentType();
        }

        public static List<KeyValueModel> GetCurrency()
        {
            ProjectInvoiceDAL objCurrency = new ProjectInvoiceDAL();
            return objCurrency.GetCurrency();
        }

        public int SavePaymentType(string pType)
        {
            ProjecInvoiceBLL objheader = new ProjecInvoiceBLL();
            objheader.PTypes = pType;

            ProjectInvoiceDAL objSaveheader = new ProjectInvoiceDAL();
            return objSaveheader.SavePaymentType(objheader); 
        }

        public static List<ProjecInvoiceBLL> GetmailInfo(int PaymentID)
        {
            ProjectInvoiceDAL objDAL = new ProjectInvoiceDAL();
            return objDAL.GetMailInfo(PaymentID);
        }

        public void UpdateMailSentDate(int PaymentID)
        {
            ProjectInvoiceDAL objDAL = new ProjectInvoiceDAL();
            objDAL.UpdateMailSentDate(PaymentID);
        }



        //////////////////////------
        public ProjecInvoiceBLL BindPayment(DataSet ds, string mode)
        {
            ProjecInvoiceBLL objInvoice = new ProjecInvoiceBLL();

            List<InvoicePaymentModel> lstDetails = new List<InvoicePaymentModel>();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    objInvoice.customerName = Convert.ToString(ds.Tables[0].Rows[0]["custCompany"]);
                    objInvoice.customerAddress = Convert.ToString(ds.Tables[0].Rows[0]["custAddress"]);
                    objInvoice.ProjectPaymentID = Convert.ToInt32(ds.Tables[0].Rows[0]["PaymentID"]);
                    objInvoice.ProjID = Convert.ToInt32(ds.Tables[0].Rows[0]["projId"]);
                    objInvoice.CurrencyID = Convert.ToInt32(ds.Tables[0].Rows[0]["currID"]);
                    objInvoice.currSymbol = Convert.ToString(ds.Tables[0].Rows[0]["currSymbol"]);
                    objInvoice.ExRate = Convert.ToSingle(ds.Tables[0].Rows[0]["ExRate"]);
                    objInvoice.InvoiceDate = Convert.ToString(ds.Tables[0].Rows[0]["ProjectDate"]);
                    objInvoice.currSymbol = Convert.ToString(ds.Tables[0].Rows[0]["currSymbol"]);
                    objInvoice.Amount = float.Parse(ds.Tables[0].Rows[0]["PaidAmount"].ToString());
                    objInvoice.PaymentType = Convert.ToInt32(ds.Tables[0].Rows[0]["PaymentTypeID"]);
                    objInvoice.CreditAmount = float.Parse(ds.Tables[0].Rows[0]["CreditAmount"].ToString());
                    objInvoice.Description = Convert.ToString(ds.Tables[0].Rows[0]["Description"]);
                    objInvoice.ProjectName = Convert.ToString(ds.Tables[0].Rows[0]["projName"]);
                    objInvoice.PreviousCredit = float.Parse(ds.Tables[0].Rows[0]["PreviousCredit"].ToString());
                    objInvoice.isCredited = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsCreditused"]);
                    objInvoice.CreditedPaymentID = Convert.ToString(ds.Tables[0].Rows[0]["CreditedPaymentID"]);
                    objInvoice.NoOfCredit = Convert.ToString(ds.Tables[0].Rows[0]["NoOfCredits"]);
                    objInvoice.TaxCollected = float.Parse(ds.Tables[0].Rows[0]["TaxCollected"].ToString());
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        InvoicePaymentModel obj = new InvoicePaymentModel();
                        obj.InvoiceNo = Convert.ToString(item["InvoiceNo"]);
                        obj.invoiceDate = Convert.ToString(item["InvoiceDate"]);
                        obj.Description = Convert.ToString(item["Description"]);
                        obj.Amount = float.Parse(item["TotalAmount"].ToString());
                        obj.ProjectInvoiceID = Convert.ToInt32(item["ProjectInvoiceID"]);
                        obj.ProjectDetailID = Convert.ToInt32(item["PaymentDetailID"]);
                        obj.InvBalance = float.Parse(item["BalanceAmount"].ToString());
                        obj.payAmount = float.Parse(item["PaidAmount"].ToString());
                        obj.PreviousBalance = float.Parse(item["PreviousBalance"].ToString());
                        obj.DisplayAmount = getCurrencyFormat(obj.Amount);
                        obj.DisplayBalance = getCurrencyFormat(obj.InvBalance);
                       // obj.TaxCollected = float.Parse(item["TaxCollected"].ToString());
                        if (mode == "EDIT")
                        {
                            obj.InvBalance = float.Parse(item["PreviousBalance"].ToString());
                            obj.DisplayBalance = getCurrencyFormat(obj.InvBalance);
                            //if(obj.InvBalance == 0)
                            //obj.payAmount =  obj.Amount; //Convert.ToInt32(item["PaidAmount"]);

                        }

                        lstDetails.Add(obj);
                    }

                }
            }

            objInvoice.lstModel = lstDetails;

            return objInvoice;
        }
        private string getCurrencyFormat(float amnt)
        {
            CultureInfo cuInfo = new CultureInfo("en-IN");
            string curr = ((Convert.ToDecimal(amnt)).ToString("C", cuInfo)).Remove(0, 2).Trim();
            return curr;
        }


        public void PostInvoicePayments(ProjecInvoiceBLL header)
        {
            string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlTransaction transaction;
            con.Open();
            transaction = con.BeginTransaction();
            try
            {
                int counter = 0;
                if (header.isCredited)
                    counter = Convert.ToInt16(getInvoiceForAppliedCredit(header.AppliedCreditAmount, header)); // to get which  invoices  used hw much credit amount 
                int result1 = 0;

                if (!header.isCredited)
                    result1 = new ProjecInvoiceBLL().SavePaymentHeader(header.ProjectPaymentID, header.ProjID, header.Amount, header.TaxCollected,header.PaymentType, header.CurrencyID, header.ExRate, header.CreditAmount, header.Description, header.InsertedBy, header.isCredited, header.InvoiceDate);

                if (result1 > 0 || header.isCredited == true)
                {
                    //counter = header.lstModel.Count;

                    int icnt = 0;
                    string[] cpID = header.CreditedPaymentID.Split(',');

                    foreach (InvoicePaymentModel inv in header.lstModel)
                    {
                        if (header.isCredited)
                            inv.PaymentID = Convert.ToInt32(cpID[icnt]);
                        else
                            inv.PaymentID = result1;

                        if (header.isEdited == 1)
                            inv.ProjectDetailID = 0;
                        int result = new InvoicePaymentModel().SaveInvoicePayment(inv.ProjectDetailID, inv.ProjectInvoiceID, header.ProjID, inv.PaymentID, inv.Description, inv.PaymentType, inv.Amount,inv.payAmount, inv.TaxCollected,inv.InvBalance, header.isEdited, inv.IDs, header.AppliedCreditAmount, counter, header.CreditedPaymentID, inv.bFlag, header.InsertedBy, header);
                        //iresult will be modified applied credit
                        //header.AppliedCreditAmount = result;
                        counter = counter - 1;
                        icnt = icnt + 1;
                    }
                }
                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
            }
            con.Close();
        }

        private string getInvoiceForAppliedCredit(float AppliedCredit, ProjecInvoiceBLL header)
        {
            // header.CreditedPaymentID = "10,20";
            string id = "";
            string inv = "";
            int PaidCount = 0;
            float cr = 0;
            string[] creditpaymentid = header.CreditedPaymentID.Split(',');
            string[] NoOfCredit = header.NoOfCredit.Split(',');

            if (header.Amount == 0)
                GetAppliedCreditAmount(header);

            for (int j = 0; j < header.lstModel.Count; j++)
            {
                if (header.lstModel[j].payAmount > 0)
                    PaidCount = PaidCount + 1;
                id = "";
                if (!header.lstModel[j].bFlag && header.lstModel[j].payAmount != 0)
                {
                    string[] invoiceids = header.lstModel[j].IDs.Split(',');//65 credit applied inv id
                    int icnt = 0;
                    int AppliedPerInv = Convert.ToInt32(header.lstModel[j].AppliedCreditAmount); // aplied credit amt for the particluar invoice
                    float rem = 0;
                    float AmountEntered = 0;
                    float diff;


                    if (header.Amount == 0)
                        AmountEntered = 0;
                    else
                        AmountEntered = header.lstModel[j].AmountEntered;

                    for (int i = 0; i < NoOfCredit.Length; i++)
                    {
                        if (Convert.ToInt32(NoOfCredit[i]) != 0)
                        {
                            if ((header.lstModel[j].Amount) != (AmountEntered))
                            {
                                diff = header.lstModel[j].Amount - AmountEntered;
                                if (Convert.ToDouble(NoOfCredit[i]) < diff && creditpaymentid[icnt] != "")
                                {
                                    id = id + invoiceids[0] + "@" + creditpaymentid[icnt] + ",";
                                    AmountEntered = AmountEntered + Convert.ToInt32(NoOfCredit[i]);
                                    NoOfCredit[i] = "0";
                                }
                                else if (creditpaymentid[icnt] != "")
                                {
                                    rem = Convert.ToInt32(NoOfCredit[i]) - diff;
                                    AmountEntered = AmountEntered + rem;
                                    id = id + invoiceids[0] + "@" + creditpaymentid[icnt] + ",";

                                    NoOfCredit[i] = rem.ToString();
                                }
                                //icnt = icnt + 1;
                            }
                            //else
                            //    id = id + invoiceids[icnt] + "@" + creditpaymentid[icnt] + ",";
                            cr = cr + float.Parse(NoOfCredit[i]);
                        }
                        icnt = icnt + 1;
                    }
               if (NoOfCredit[j] == "0")
                 creditpaymentid[j] = "";
                    //for (int k = 0; k < icnt; k++)
                    //    NoOfCredit[k].Remove(1);

                }
                header.lstModel[j].IDs = id;


            }
            float re = Math.Abs((AppliedCredit - cr));
            header.CreditAmount = Math.Abs((header.CreditAmount - re));
            return PaidCount.ToString();
        }

        private void GetAppliedCreditAmount(ProjecInvoiceBLL header)
        {
            float crAmount = 0;
            for (int i = 0; i < header.lstModel.Count; i++)
            {
                crAmount = crAmount + header.lstModel[i].payAmount;
            }

            header.AppliedCreditAmount = crAmount;
        }

        public void DeletePayment(int PaymentID)
        {
            ProjectInvoiceDAL objDAL = new ProjectInvoiceDAL();
            objDAL.DeletePayment(PaymentID);
        }
    }
}