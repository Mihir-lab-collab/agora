using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for ProformaInvoiceBLL
/// </summary>
public class ProformaInvoiceBLL
{
    public int ProjectInvoiceID { get; set; }
    public int ProformaInvoiceID { get; set; }
    public int projId { get; set; }
    public string projName { get; set; }
    public string InvoiceDate { get; set; }
    public int currID { get; set; }
    public string currSymbol { get; set; }
    public int custId { get; set; }
    public string custAddress { get; set; }
    public string custCompany { get; set; }
    public float ExRate { get; set; }
    public float Tax1 { get; set; }
    public float Tax2 { get; set; }
    public float Tax3 { get; set; }
    public float TransCharge { get; set; }
    public float VATCharge { get; set; }
    public float CSTCharge { get; set; }
    public float TotalAmount { get; set; }
    public string Comment { get; set; }
    public int insertedBy { get; set; }
    public int modifiedBy { get; set; }
    public int modifiedon { get; set; }
    public string DueDate { get; set; }
    public string InvoiceNo { get; set; }
    public int Inv_LocationID { get; set; }
    public string Inv_LocationName { get; set; }
    public decimal Inv_BalanceAmount { get; set; }
    public string Inv_CurBalanceAmount { get; set; }
    public int ProjID { get; set; }
    public int CurrencyID { get; set; }
    public string custName { get; set; }
    public string Amount { get; set; }
    public string Inv_Delay { get; set; }
    public string Status { get; set; }
    public string TaxInvoice { get; set; }
    public string IsVoid { get; set; }
    public bool AddPayment { get; set; }
    public int PaymentType { get; set; }
    public string PaymentComment { get; set; }
    public string PaymentDate { get; set; }
    public string EmailSentDate { get; set; }
    public int ClientStateId { get; set; }
    public int CustStateId { get; set; }
    public float GSTPercentage { get; set; }
    public float CGST { get; set; }
    public float SGST { get; set; }
    public float IGST { get; set; }
    public float GST { get; set; }
    public int CodeId { get; set; }
    public string Code { get; set; }
    public List<ProInvoiceModel> myData { get; set; }
    public List<mileStoneModel> Milestone { get; set; }    
    public int CustCountry { get; set; } //Added By Nikhil Shetye on 18-10-2017



    public ProformaInvoiceBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int InsertProInvoiceHeader(int projId, string InvoiceDate, int currID, float ExRate, float Tax1, float Tax2, float Tax3, float TransCharge, float VATCharge, float CSTCharge,
                                      float TotalAmount, string Comment, int insertedBy, string DueDate, string InvoiceNo, int locationID, float CGST, float SGST, float IGST,float GST, int CodeId)
    {
        ProformaInvoiceBLL objProInvoiceheader = new ProformaInvoiceBLL();
        objProInvoiceheader.projId = projId;
        objProInvoiceheader.InvoiceDate = InvoiceDate;
        objProInvoiceheader.currID = currID;
        objProInvoiceheader.ExRate = ExRate;
        objProInvoiceheader.Tax1 = Tax1;
        objProInvoiceheader.Tax2 = Tax2;
        objProInvoiceheader.Tax3 = Tax3;
        objProInvoiceheader.TransCharge = TransCharge;
        objProInvoiceheader.VATCharge = VATCharge;
        objProInvoiceheader.CSTCharge = CSTCharge;
        objProInvoiceheader.TotalAmount = TotalAmount;
        objProInvoiceheader.Comment = Comment;
        objProInvoiceheader.insertedBy = insertedBy;
        objProInvoiceheader.DueDate = DueDate;
        objProInvoiceheader.InvoiceNo = InvoiceNo;
        objProInvoiceheader.Inv_LocationID = locationID;
        objProInvoiceheader.CGST = CGST;
        objProInvoiceheader.SGST = SGST;
        objProInvoiceheader.IGST = IGST;
        objProInvoiceheader.GST = GST;
        objProInvoiceheader.CodeId = CodeId;

        ProformaInvoiceDAL objInsertProInvoiceheader = new ProformaInvoiceDAL();
        return objInsertProInvoiceheader.InsertProInvoiceHeader(objProInvoiceheader);
    }


    public void InsertProInvoice(ProformaInvoiceBLL header)
    {
        int ProInvoiceResult = new ProformaInvoiceBLL().InsertProInvoiceHeader(header.projId, header.InvoiceDate, header.currID, header.ExRate, header.Tax1, header.Tax2, header.Tax3, header.TransCharge, header.VATCharge, header.CSTCharge, header.TotalAmount, header.Comment, header.insertedBy, header.DueDate, header.InvoiceNo, header.Inv_LocationID, header.CGST, header.SGST, header.IGST,header.GST,header.CodeId);

        if (header.myData != null)
        {
            foreach (ProInvoiceModel inv in header.myData)
            {
                if (!(inv.ProjectMilestoneID == 0 && string.IsNullOrEmpty(inv.Description)))
                {
                    int ProInvoiceDetailResult = new ProInvoiceModel().insertProInvoiceDetails(ProInvoiceResult, inv.projId, inv.Description, inv.ProjectMilestoneID, inv.Quantity, inv.Rate, inv.Amount, inv.BalanceAmount);
                }
            }
        }
        // if (header.AddPayment == true && ProInvoiceResult > 0)
        //{
        //    header.ProformaInvoiceID = ProInvoiceResult;
        //   AddInvoicePayment(header);
        //}
    }

    public int insertTaxinvoiceheader(string status, int projId, string InvoiceDate, int currId, float Exrate, float Tax1, float Tax2, float Tax3, float transCharge, float VATCharge, float CSTCharge, float TotalAmt, string Comment, int insertedby, string DueDate, string InvoiceNo, int locationID, int ProformaInvoiceID, float CGST, float SGST, float IGST, float GST, int CodeId)
    {
        ProformaInvoiceBLL objInvoiceheader = new ProformaInvoiceBLL();
        objInvoiceheader.projId = projId;
        objInvoiceheader.InvoiceDate = InvoiceDate;
        objInvoiceheader.currID = currId;
        objInvoiceheader.ExRate = Exrate;
        objInvoiceheader.Tax1 = Tax1;
        objInvoiceheader.Tax2 = Tax2;
        objInvoiceheader.Tax3 = Tax3;
        objInvoiceheader.TransCharge = transCharge;
        objInvoiceheader.VATCharge = VATCharge;
        objInvoiceheader.CSTCharge = CSTCharge;
        objInvoiceheader.TotalAmount = TotalAmt;
        objInvoiceheader.Comment = Comment;
        objInvoiceheader.insertedBy = insertedby;
        objInvoiceheader.DueDate = DueDate;
        objInvoiceheader.InvoiceNo = InvoiceNo;
        objInvoiceheader.Inv_LocationID = locationID;
        objInvoiceheader.Status = status;
        objInvoiceheader.ProformaInvoiceID = ProformaInvoiceID;
        objInvoiceheader.CGST = CGST;
        objInvoiceheader.SGST = SGST;
        objInvoiceheader.IGST = IGST;
        objInvoiceheader.GST = GST;
        objInvoiceheader.CodeId = CodeId;

        ProformaInvoiceDAL objInsertInvoiceheader = new ProformaInvoiceDAL();
        return objInsertInvoiceheader.InsertHeaderTaxInv(objInvoiceheader);
    }

    public void PostTaxInvoice(ProformaInvoiceBLL header)
    {
        int result1 = new ProformaInvoiceBLL().insertTaxinvoiceheader("Invoiced", header.projId, header.InvoiceDate, header.currID, header.ExRate, header.Tax1, header.Tax2, header.Tax3, header.TransCharge, header.VATCharge, header.CSTCharge, header.TotalAmount, header.Comment, header.insertedBy, header.DueDate, header.InvoiceNo, header.Inv_LocationID, header.ProformaInvoiceID, header.CGST, header.SGST, header.IGST,header.GST, header.CodeId);

        foreach (ProInvoiceModel inv in header.myData)
        {
            if (!(inv.ProjectMilestoneID == 0 && string.IsNullOrEmpty(inv.Description)))
            {
                int result = new ProInvoiceModel().insertTaxInvoice(result1, inv.projId, inv.Description, inv.ProjectMilestoneID, inv.Quantity, inv.Rate, inv.Amount, inv.BalanceAmount);
            }
        }

        if (header.AddPayment == true && result1 > 0)
        {
            header.ProjectInvoiceID = result1;
            AddInvoicePayment(header);
        }
    }

    private void AddInvoicePayment(ProformaInvoiceBLL invHeader)
    {
        int paymentID = 0;
        ProjecInvoiceBLL header = new ProjecInvoiceBLL();
        header.ProjectPaymentID = 0;
        header.ProjID = invHeader.projId;
        header.Amount = invHeader.TotalAmount;
        header.PaymentType = invHeader.PaymentType;
        header.CurrencyID = invHeader.currID;
        header.ExRate = invHeader.ExRate;
        header.CreditAmount = 0;
        header.Description = invHeader.PaymentComment;
        header.InsertedBy = invHeader.insertedBy;
        header.isCredited = false;
        header.InvoiceDate = invHeader.PaymentDate;

        paymentID = new ProjecInvoiceBLL().SavePaymentHeader(header.ProjectPaymentID, header.ProjID, header.Amount,header.TaxCollected, header.PaymentType, header.CurrencyID, header.ExRate, header.CreditAmount, header.Description, header.InsertedBy, header.isCredited, header.InvoiceDate);
        if (paymentID > 0)
        {
            header.ProjectPaymentID = paymentID;
            AddPaymentDetail(header, invHeader);
        }
    }

    private void AddPaymentDetail(ProjecInvoiceBLL header, ProformaInvoiceBLL invHeader)
    {
        int ProjectDetailID = 0;
        InvoicePaymentModel lstModel = new InvoicePaymentModel();
        lstModel.ProjectDetailID = ProjectDetailID;
        lstModel.ProjectInvoiceID = invHeader.ProjectInvoiceID;
        lstModel.PaymentID = header.ProjectPaymentID;
        lstModel.Description = header.Description;
        lstModel.PaymentType = header.PaymentType;
        lstModel.Amount = header.Amount;
        lstModel.payAmount = header.Amount;

        lstModel.InvBalance = Math.Abs(lstModel.Amount - lstModel.payAmount);
        header.isEdited = 0;
        lstModel.IDs = "";
        header.AppliedCreditAmount = 0;
        header.CreditedPaymentID = "";
        lstModel.bFlag = false;

        int result = new InvoicePaymentModel().SaveInvoicePayment(lstModel.ProjectDetailID, lstModel.ProjectInvoiceID, header.ProjID, lstModel.PaymentID, lstModel.Description, lstModel.PaymentType
            , lstModel.Amount, lstModel.payAmount,header.TaxCollected, lstModel.InvBalance, header.isEdited, lstModel.IDs, header.AppliedCreditAmount, 0, header.CreditedPaymentID, lstModel.bFlag, header.InsertedBy, header);

    }

    public List<mileStoneModel> GetProformaInvoiceMile(string projid)
    {
        List<mileStoneModel> lstgetInvoiceMile = new mileStone().getProformaInvoiceMileStone(projid);
        return lstgetInvoiceMile;
    }

    public List<ProformaInvoiceBLL> GetProformaInvoices(int projId, int locid, string status = "")
    {
        ProformaInvoiceDAL objInvoice = new ProformaInvoiceDAL();
        return objInvoice.GetProformaInvoices(projId, locid, status);

    }

    public ProformaInvoiceBLL GetInvoiceForEdit(string pInvId, string projid)
    {
        ProformaInvoiceBLL objInvoice = new ProformaInvoiceBLL();

        List<ProInvoiceModel> lstInvDetails = new List<ProInvoiceModel>();

        DataSet ds = new mileStone().GetProformaInvoiceDetails(pInvId, projid);
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    objInvoice.ProformaInvoiceID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProformaInvoiceID"]);
                    objInvoice.projId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjID"]);
                    objInvoice.currID = Convert.ToInt32(ds.Tables[0].Rows[0]["CurrencyID"]);
                    objInvoice.ExRate = Convert.ToSingle(ds.Tables[0].Rows[0]["ExRate"]);
                    objInvoice.InvoiceDate = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceDate"]);
                    objInvoice.Tax1 = Convert.ToSingle(ds.Tables[0].Rows[0]["Tax1"]);
                    objInvoice.Tax2 = Convert.ToSingle(ds.Tables[0].Rows[0]["Tax2"]);
                    objInvoice.Tax3 = Convert.ToSingle(ds.Tables[0].Rows[0]["Tax3"]);
                    objInvoice.CGST = Convert.ToSingle(ds.Tables[0].Rows[0]["CGST"]);
                    objInvoice.SGST = Convert.ToSingle(ds.Tables[0].Rows[0]["SGST"]);
                    objInvoice.IGST = Convert.ToSingle(ds.Tables[0].Rows[0]["IGST"]);
                    objInvoice.GST = Convert.ToSingle(ds.Tables[0].Rows[0]["GST"]);
                    objInvoice.CodeId = (ds.Tables[0].Rows[0]["SAC_HSN"] == DBNull.Value) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["SAC_HSN"]);

                    objInvoice.TransCharge = Convert.ToSingle(ds.Tables[0].Rows[0]["TransCharge"]);
                    objInvoice.VATCharge = Convert.ToSingle(ds.Tables[0].Rows[0]["VATCharge"]);
                    objInvoice.CSTCharge = Convert.ToSingle(ds.Tables[0].Rows[0]["CSTCharge"]);
                    objInvoice.Comment = Convert.ToString(ds.Tables[0].Rows[0]["Comment"]);
                    objInvoice.TotalAmount = Convert.ToSingle(ds.Tables[0].Rows[0]["TotalAmount"]);
                    objInvoice.currSymbol = Convert.ToString(ds.Tables[0].Rows[0]["currSymbol"]);
                    objInvoice.custCompany = Convert.ToString(ds.Tables[0].Rows[0]["custCompany"]);
                    objInvoice.custAddress = Convert.ToString(ds.Tables[0].Rows[0]["custAddress"]);
                    objInvoice.Milestone = GetProformaInvoiceMile(Convert.ToString(objInvoice.projId));
                    objInvoice.DueDate = Convert.ToString(ds.Tables[0].Rows[0]["DueDate"]);
                    objInvoice.InvoiceNo = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceNo"]);

                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        ProInvoiceModel obj = new ProInvoiceModel();
                        obj.ProformaInvoiceDetailID = Convert.ToInt32(item["ProformaInvoiceDetailsID"]);
                        obj.ProformaInvoiceID = Convert.ToInt32(item["ProformaInvoiceID"]);
                        obj.ProjectMilestoneID = Convert.ToInt32(item["ProjectMilestoneID"]);
                        obj.Description = Convert.ToString(item["Description"]);
                        obj.Quantity = Convert.ToInt32(item["Quantity"]);
                        obj.Rate = Convert.ToSingle(item["OriginalAmount"]);
                        obj.Amount = Convert.ToSingle(item["Amount"]);
                        obj.OriginalAmount = Convert.ToSingle(item["OriginalAmount"]);
                        obj.BalanceAmount = Convert.ToSingle(item["BalanceAmount"]);
                        obj.CalBalance = Convert.ToSingle(item["BalanceAmount"]);
                        lstInvDetails.Add(obj);
                    }

                }
            }

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

        objInvoice.myData = lstInvDetails;

        return objInvoice;
    }


    public ProformaInvoiceBLL GetDetailsForTaxInvoice(string pInvId, string projid)
    {
        ProformaInvoiceBLL objTaxInvoice = new ProformaInvoiceBLL();

        List<ProInvoiceModel> lstInvDetails = new List<ProInvoiceModel>();

        DataSet ds = new mileStone().GetProformaInvoiceDetails(pInvId, projid);//Convert.ToString(objTaxInvoice.projId)
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    objTaxInvoice.ProformaInvoiceID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProformaInvoiceID"]);
                    objTaxInvoice.projId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjID"]);
                    objTaxInvoice.currID = Convert.ToInt32(ds.Tables[0].Rows[0]["CurrencyID"]);
                    objTaxInvoice.ExRate = Convert.ToSingle(ds.Tables[0].Rows[0]["ExRate"]);
                    objTaxInvoice.InvoiceDate = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceDate"]);
                    objTaxInvoice.Tax1 = Convert.ToSingle(ds.Tables[0].Rows[0]["Tax1"]);
                    objTaxInvoice.Tax2 = Convert.ToSingle(ds.Tables[0].Rows[0]["Tax2"]);
                    objTaxInvoice.Tax3 = Convert.ToSingle(ds.Tables[0].Rows[0]["Tax3"]);
                    objTaxInvoice.TransCharge = Convert.ToSingle(ds.Tables[0].Rows[0]["TransCharge"]);
                    objTaxInvoice.VATCharge = Convert.ToSingle(ds.Tables[0].Rows[0]["VATCharge"]);
                    objTaxInvoice.CSTCharge = Convert.ToSingle(ds.Tables[0].Rows[0]["CSTCharge"]);
                    objTaxInvoice.Comment = Convert.ToString(ds.Tables[0].Rows[0]["Comment"]);
                    objTaxInvoice.TotalAmount = Convert.ToSingle(ds.Tables[0].Rows[0]["TotalAmount"]);
                    objTaxInvoice.currSymbol = Convert.ToString(ds.Tables[0].Rows[0]["currSymbol"]);
                    objTaxInvoice.custCompany = Convert.ToString(ds.Tables[0].Rows[0]["custCompany"]);
                    objTaxInvoice.custAddress = Convert.ToString(ds.Tables[0].Rows[0]["custAddress"]);
                    objTaxInvoice.Milestone = GetProformaInvoiceMile(Convert.ToString(objTaxInvoice.projId));
                    objTaxInvoice.DueDate = Convert.ToString(ds.Tables[0].Rows[0]["DueDate"]);
                    objTaxInvoice.InvoiceNo = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceNo"]);
                    objTaxInvoice.CodeId = Convert.ToInt32(ds.Tables[0].Rows[0]["SAC_HSN"]);
                    objTaxInvoice.CGST = Convert.ToSingle(ds.Tables[0].Rows[0]["CGST"]);
                    objTaxInvoice.SGST = Convert.ToSingle(ds.Tables[0].Rows[0]["SGST"]);
                    objTaxInvoice.IGST = Convert.ToSingle(ds.Tables[0].Rows[0]["IGST"]);
                    objTaxInvoice.GST = Convert.ToSingle(ds.Tables[0].Rows[0]["GST"]);
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        ProInvoiceModel obj = new ProInvoiceModel();
                        obj.ProformaInvoiceDetailID = Convert.ToInt32(item["ProformaInvoiceDetailsID"]);
                        obj.ProformaInvoiceID = Convert.ToInt32(item["ProformaInvoiceID"]);
                        obj.ProjectMilestoneID = Convert.ToInt32(item["ProjectMilestoneID"]);
                        obj.Description = Convert.ToString(item["Description"]);
                        obj.Quantity = Convert.ToInt32(item["Quantity"]);
                        obj.Rate = Convert.ToSingle(item["OriginalAmount"]);
                        obj.Amount = Convert.ToSingle(item["Amount"]);
                        obj.OriginalAmount = Convert.ToSingle(item["OriginalAmount"]);
                        obj.BalanceAmount = Convert.ToSingle(item["BalanceAmount"]);
                        obj.CalBalance = Convert.ToSingle(item["BalanceAmount"]);
                        lstInvDetails.Add(obj);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        objTaxInvoice.myData = lstInvDetails;

            return objTaxInvoice;
    }



    public List<ProformaInvoiceBLL> GetInvoicesStatus(int pInvId)
    {
        ProformaInvoiceDAL objInvoice = new ProformaInvoiceDAL();
        return objInvoice.GetInvoicesStatus(pInvId);
    }


    public List<mileStoneModel> GetMileStoneDetails(mileStoneModel msmodel)
    {
        List<mileStoneModel> lstgetinvoicemile = new mileStone().GetMileStoneDetails(msmodel.projID, msmodel.projMilestoneID);
        return lstgetinvoicemile;

    }

    public void updateinvoice(ProformaInvoiceBLL header)
    {
        int result1 = new ProformaInvoiceBLL().updateinvoice(header.projId, header.ProformaInvoiceID, header.InvoiceDate, header.currID, header.ExRate, header.Tax1, header.Tax2, header.Tax3, header.TransCharge, header.VATCharge, header.CSTCharge, header.TotalAmount, header.Comment, header.modifiedBy, header.modifiedon, header.DueDate, header.InvoiceNo, header.CGST, header.SGST, header.IGST,header.GST, header.CodeId);

        foreach (ProInvoiceModel inv in header.myData)
        {
            int result = 0;
            if (inv.ProformaInvoiceDetailID == 0)//&& string.IsNullOrEmpty(inv.Description))
            {
                result = new ProInvoiceModel().insertProInvoiceDetails(result1, inv.projId, inv.Description, inv.ProjectMilestoneID, inv.Quantity, inv.Rate, inv.Amount, inv.BalanceAmount);
            }
            else
                result = new ProInvoiceModel().updateProInvoiceDetails(inv.ProformaInvoiceDetailID, inv.ProformaInvoiceID, inv.ProjectMilestoneID, inv.Description, inv.Quantity, inv.Rate, inv.Amount, inv.modifiedon, inv.BalanceAmount);

        }

    }

    public int updateinvoice(int projId, int ProformaInvoiceID, string InvoiceDate, int currID, float ExRate, float Tax1, float Tax2, float Tax3, float TransCharge, float VATCharge, float CSTCharge, float TotalAmount, string Comment, int modifiedBy, int modifiedon, string DueDate, string InvoiceNo, float CGST, float SGST, float IGST,float GST, int CodeId)
    {
        ProformaInvoiceBLL objIHeader = new ProformaInvoiceBLL();
        objIHeader.projId = projId;
        objIHeader.ProformaInvoiceID = ProformaInvoiceID;
        objIHeader.InvoiceDate = InvoiceDate;
        objIHeader.currID = currID;
        objIHeader.ExRate = ExRate;
        objIHeader.Tax1 = Tax1;
        objIHeader.Tax2 = Tax2;
        objIHeader.Tax3 = Tax3;
        objIHeader.TransCharge = TransCharge;
        objIHeader.VATCharge = VATCharge;
        objIHeader.CSTCharge = CSTCharge;
        objIHeader.TotalAmount = TotalAmount;
        objIHeader.Comment = Comment;
        objIHeader.modifiedBy = modifiedBy;
        objIHeader.modifiedon = modifiedon;
        objIHeader.DueDate = DueDate;
        objIHeader.InvoiceNo = InvoiceNo;
        objIHeader.CGST = CGST;
        objIHeader.SGST = SGST;
        objIHeader.IGST = IGST;
        objIHeader.GST = GST;
        objIHeader.CodeId = CodeId;
        ProformaInvoiceDAL objUpdateInvoiceHeader = new ProformaInvoiceDAL();
        return objUpdateInvoiceHeader.UpdateInvoiceHeader(objIHeader);
    }


    public static int VoidInvoice(string pInvId)
    {
        ProformaInvoiceDAL objDAL = new ProformaInvoiceDAL();
        return objDAL.VoidInvoice(pInvId);
    }

    public static List<ProformaInvoiceBLL> GetmailInfo(int pInvId)
    {
        ProformaInvoiceDAL objDAL = new ProformaInvoiceDAL();
        return objDAL.GetMailInfo(pInvId);
    }

    public void UpdateMailSentDate(int pInvId)
    {
        ProformaInvoiceDAL objDAL = new ProformaInvoiceDAL();
        objDAL.UpdateMailSentDate(pInvId);
    }

    //PDF

    public DataSet GeneratePDF(int pInvId, int LocationID)
    {
        DataSet ds = new DataSet();
        ProformaInvoiceDAL objDAL = new ProformaInvoiceDAL();
        ds = objDAL.GetPDFContent(pInvId, LocationID);
        return ds;
    }


    public string BindDataForPdf(DataTable dt, string strHtml, string logo = "Yes")//, string InvoiceType = "")
    {
        strHtml = AddInvocieItems_Test(dt, strHtml);

        string ProjectName = Convert.ToString(HttpContext.Current.Session["ProformaInvoiceProjectName"]);
        string host = string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Headers["host"]);

        if (logo == "Yes")
            strHtml = strHtml.Replace("{logo}", host + "/Images/LocationLogo/" + dt.Rows[0]["Logo"].ToString());
        else
            strHtml = strHtml.Replace("{logo}", host + "/Images/LocationLogo/blank_logo.png");

        strHtml = strHtml.Replace("{CustCompany}", dt.Rows[0]["custCompany"].ToString());
        strHtml = strHtml.Replace("{CustName}", dt.Rows[0]["custName"].ToString());
        strHtml = strHtml.Replace("{PorjectName}", ProjectName);
        strHtml = strHtml.Replace("{CustAddress}", dt.Rows[0]["custAddress"].ToString());
        strHtml = strHtml.Replace("{GSTIN}", dt.Rows[0]["GSTIN"].ToString());

        strHtml = strHtml.Replace("{CustEmail}", dt.Rows[0]["custEmail"].ToString());
        strHtml = strHtml.Replace("{InvoiceNo}", dt.Rows[0]["ProformaInvoiceID"].ToString());
        strHtml = strHtml.Replace("{InvoiceDate}", dt.Rows[0]["InvoiceDate"].ToString());
        strHtml = strHtml.Replace("{CurSymbol}", dt.Rows[0]["currSymbol"].ToString());
        strHtml = strHtml.Replace("{TelePhone}", dt.Rows[0]["TEL"].ToString());
        strHtml = strHtml.Replace("{Fax}", dt.Rows[0]["Fax"].ToString());
        strHtml = strHtml.Replace("{PanNo}", dt.Rows[0]["PanNo"].ToString());
        strHtml = strHtml.Replace("{ServiceTaxNo}", dt.Rows[0]["ServiceTaxNo"].ToString());
        strHtml = strHtml.Replace("{BankWireTransfer}", dt.Rows[0]["WireDetail"].ToString());
        //strHtml = strHtml.Replace("{Note}", "This is a computer generated invoice and does not require signature.");
        string grandTotal = (Math.Round(Convert.ToDecimal(dt.Rows[0]["TotalAmount"].ToString()))).ToString();
        strHtml = strHtml.Replace("{GrandTotal}", getCurrencyFormat(float.Parse(grandTotal)));
        strHtml = strHtml.Replace("{TotalInvoice}", getCurrencyFormat(float.Parse(dt.Rows[0]["InvoiceTotal"].ToString())));
        //strHtml = strHtml.Replace("{CostinWords}", IntegerToWords(dt.Rows[0]["TotalAmount"].ToString()));
        strHtml = strHtml.Replace("{CostinWords}", IntegerToWordsConversion(Convert.ToInt64(dt.Rows[0]["TotalAmount"].ToString()), dt.Rows[0]["currSymbol"].ToString().Trim()));
        strHtml = strHtml.Replace("{CompanyAddress}", dt.Rows[0]["CompanyAddress"].ToString());
        strHtml = strHtml.Replace("{CompanyName}", dt.Rows[0]["CompanyName"].ToString());
        strHtml = dt.Rows[0]["SAC_HSN"].ToString() != "" ? strHtml.Replace("{SAC_HSN_Code}", dt.Rows[0]["SAC_HSN"].ToString()) : strHtml.Replace("{SAC_HSN_Code}", "NA");
        return strHtml;
    }


    public string IntegerToWordsConversion(Int64 rup, string CurrencySymbol)
    {
        string result = "";

        Int64 res;
        if (CurrencySymbol == "INR")
        {
            if ((rup / 10000000) > 0)
            {
                res = rup / 10000000;
                rup = rup % 10000000;
                result = result + ' ' + RupeesToWords(res) + " Crore";
            }
            if ((rup / 100000) > 0)
            {
                res = rup / 100000;
                rup = rup % 100000;
                result = result + ' ' + RupeesToWords(res) + " Lakh";
            }
            if ((rup / 1000) > 0)
            {
                res = rup / 1000;
                rup = rup % 1000;
                result = result + ' ' + RupeesToWords(res) + " Thousand";
            }
            if ((rup / 100) > 0)
            {
                res = rup / 100;
                rup = rup % 100;
                result = result + ' ' + RupeesToWords(res) + " Hundred";
            }
            if ((rup % 10) >= 0)
            {
                res = rup % 100;
                result = result + " " + RupeesToWords(res);
            }
            result = result + ' ' + " Rupees only";
        }
        else if (CurrencySymbol == "US $")
        {
            result = IntegerToWords(Convert.ToString(rup));
        }
        else if (string.Compare(CurrencySymbol, "AU $", true) == 0)
        {
            result = IntegerToWords(Convert.ToString(rup));
        }
        return result;
    }

    public string RupeesToWords(Int64 rup)
    {
        string result = "";
        if ((rup >= 1) && (rup <= 10))
        {
            if ((rup % 10) == 1) result = "One";
            if ((rup % 10) == 2) result = "Two";
            if ((rup % 10) == 3) result = "Three";
            if ((rup % 10) == 4) result = "Four";
            if ((rup % 10) == 5) result = "Five";
            if ((rup % 10) == 6) result = "Six";
            if ((rup % 10) == 7) result = "Seven";
            if ((rup % 10) == 8) result = "Eight";
            if ((rup % 10) == 9) result = "Nine";
            if ((rup % 10) == 0) result = "Ten";
        }
        if (rup > 9 && rup < 20)
        {
            if (rup == 11) result = "Eleven";
            if (rup == 12) result = "Twelve";
            if (rup == 13) result = "Thirteen";
            if (rup == 14) result = "Forteen";
            if (rup == 15) result = "Fifteen";
            if (rup == 16) result = "Sixteen";
            if (rup == 17) result = "Seventeen";
            if (rup == 18) result = "Eighteen";
            if (rup == 19) result = "Nineteen";
        }
        if (rup > 20 && (rup / 10) == 2 && (rup % 10) == 0) result = "Twenty";
        if (rup > 20 && (rup / 10) == 3 && (rup % 10) == 0) result = "Thirty";
        if (rup > 20 && (rup / 10) == 4 && (rup % 10) == 0) result = "Forty";
        if (rup > 20 && (rup / 10) == 5 && (rup % 10) == 0) result = "Fifty";
        if (rup > 20 && (rup / 10) == 6 && (rup % 10) == 0) result = "Sixty";
        if (rup > 20 && (rup / 10) == 7 && (rup % 10) == 0) result = "Seventy";
        if (rup > 20 && (rup / 10) == 8 && (rup % 10) == 0) result = "Eighty";
        if (rup > 20 && (rup / 10) == 9 && (rup % 10) == 0) result = "Ninty";

        if (rup > 20 && (rup / 10) == 2 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Twenty One";
            if ((rup % 10) == 2) result = "Twenty Two";
            if ((rup % 10) == 3) result = "Twenty Three";
            if ((rup % 10) == 4) result = "Twenty Four";
            if ((rup % 10) == 5) result = "Twenty Five";
            if ((rup % 10) == 6) result = "Twenty Six";
            if ((rup % 10) == 7) result = "Twenty Seven";
            if ((rup % 10) == 8) result = "Twenty Eight";
            if ((rup % 10) == 9) result = "Twenty Nine";
        }
        if (rup > 20 && (rup / 10) == 3 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Thirty One";
            if ((rup % 10) == 2) result = "Thirty Two";
            if ((rup % 10) == 3) result = "Thirty Three";
            if ((rup % 10) == 4) result = "Thirty Four";
            if ((rup % 10) == 5) result = "Thirty Five";
            if ((rup % 10) == 6) result = "Thirty Six";
            if ((rup % 10) == 7) result = "Thirty Seven";
            if ((rup % 10) == 8) result = "Thirty Eight";
            if ((rup % 10) == 9) result = "Thirty Nine";
        }
        if (rup > 20 && (rup / 10) == 4 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Forty One";
            if ((rup % 10) == 2) result = "Forty Two";
            if ((rup % 10) == 3) result = "Forty Three";
            if ((rup % 10) == 4) result = "Forty Four";
            if ((rup % 10) == 5) result = "Forty Five";
            if ((rup % 10) == 6) result = "Forty Six";
            if ((rup % 10) == 7) result = "Forty Seven";
            if ((rup % 10) == 8) result = "Forty Eight";
            if ((rup % 10) == 9) result = "Forty Nine";
        }
        if (rup > 20 && (rup / 10) == 5 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Fifty One";
            if ((rup % 10) == 2) result = "Fifty Two";
            if ((rup % 10) == 3) result = "Fifty Three";
            if ((rup % 10) == 4) result = "Fifty Four";
            if ((rup % 10) == 5) result = "Fifty Five";
            if ((rup % 10) == 6) result = "Fifty Six";
            if ((rup % 10) == 7) result = "Fifty Seven";
            if ((rup % 10) == 8) result = "Fifty Eight";
            if ((rup % 10) == 9) result = "Fifty Nine";
        }
        if (rup > 20 && (rup / 10) == 6 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Sixty One";
            if ((rup % 10) == 2) result = "Sixty Two";
            if ((rup % 10) == 3) result = "Sixty Three";
            if ((rup % 10) == 4) result = "Sixty Four";
            if ((rup % 10) == 5) result = "Sixty Five";
            if ((rup % 10) == 6) result = "Sixty Six";
            if ((rup % 10) == 7) result = "Sixty Seven";
            if ((rup % 10) == 8) result = "Sixty Eight";
            if ((rup % 10) == 9) result = "Sixty Nine";
        }
        if (rup > 20 && (rup / 10) == 7 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Seventy One";
            if ((rup % 10) == 2) result = "Seventy Two";
            if ((rup % 10) == 3) result = "Seventy Three";
            if ((rup % 10) == 4) result = "Seventy Four";
            if ((rup % 10) == 5) result = "Seventy Five";
            if ((rup % 10) == 6) result = "Seventy Six";
            if ((rup % 10) == 7) result = "Seventy Seven";
            if ((rup % 10) == 8) result = "Seventy Eight";
            if ((rup % 10) == 9) result = "Seventy Nine";
        }
        if (rup > 20 && (rup / 10) == 8 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Eighty One";
            if ((rup % 10) == 2) result = "Eighty Two";
            if ((rup % 10) == 3) result = "Eighty Three";
            if ((rup % 10) == 4) result = "Eighty Four";
            if ((rup % 10) == 5) result = "Eighty Five";
            if ((rup % 10) == 6) result = "Eighty Six";
            if ((rup % 10) == 7) result = "Eighty Seven";
            if ((rup % 10) == 8) result = "Eighty Eight";
            if ((rup % 10) == 9) result = "Eighty Nine";
        }
        if (rup > 20 && (rup / 10) == 9 && (rup % 10) != 0)
        {
            if ((rup % 10) == 1) result = "Ninty One";
            if ((rup % 10) == 2) result = "Ninty Two";
            if ((rup % 10) == 3) result = "Ninty Three";
            if ((rup % 10) == 4) result = "Ninty Four";
            if ((rup % 10) == 5) result = "Ninty Five";
            if ((rup % 10) == 6) result = "Ninty Six";
            if ((rup % 10) == 7) result = "Ninty Seven";
            if ((rup % 10) == 8) result = "Ninty Eight";
            if ((rup % 10) == 9) result = "Ninty Nine";
        }
        return result;
    }
    private string AddInvocieItems_Test(DataTable dt, string strHtml)
    {
        strHtml = strHtml.Replace("\n", "");
        string Html = "<div style=\"width:1000px; margin:0 auto;\">   <table style=\"line-height:15px;\" width=\"100%\">      <thead>         <tr>            <td width=\"50%\" valign=\"bottom\"><img src=\"{logo}\" height=\"75px\" /></td>            <td width=\"50%\" align=\"right\"><strong>Tele :</strong>  91 22 41516100/05<br /><strong>Email :</strong> contactus@intelegain.com<br /><strong>Website :</strong> www.intelegain.com </td>         </tr>      </thead>   </table>   <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse;\" width=\"100%\">      <tbody>         <tr>            <td  style=\"line-height:20px; align:left;border-right:#9d9d9d 1px solid;\" ><strong>{CustName}<br />{CustCompany}<br />{CustAddress}<br />{CustEmail}</strong></td>           <td></td>            <td align=\"right\" valign=\"top\">            <table width=\"100%\" border=\"1\" cellpadding=\"2\" cellspacing=\"0\" frame=\"box\" style=\"border-collapse:collapse; align:right;\">                  <tbody>                     <tr align=\"right\">                        <td align=\"left\" colspan=\"2\" style=\"font-size:15px;border-bottom:#000 1px solid;\"><b>INVOICE</b></td>                     </tr>                     <tr>                        <td>Invoice No:</td>                        <td>{InvoiceNo}</td>                     </tr>                     <tr>                        <td>Invoice Date:</td>                        <td>{InvoiceDate}</td>                     </tr>                  </tbody>               </table>            </td>         </tr>      </tbody>   </table>   <table border=\"1\" bordercolor=\"#000\" cellpadding=\"2\" cellspacing=\"0\" style=\"border-collapse:collapse;\" width=\"100%\">      <thead style=\"border:none;\">         <tr valign=\"top\">            <th align=\"left\"><b>Sr. No.</b></th>            <th align=\"left\" colspan=\"3\"><b>Product / Service Description</b></th>            <th><b>Price</b></th>            <th align=\"right\"><b>Amount</b></th>         </tr>      </thead>      <tbody>      <tr valign=\"top\">      <td colspan=\"6\" valign=\"top\">      <table valign=\"top\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse; margin;0 !important; padding:0;\"><tbody>         <Repeater><tr valign=\"top\">            <td colspan=\"2\">{SrNo}</td>            <td align=\"left\" colspan=\"5\">{Description}</td>            <td align=\"right\" colspan=\"2\">{Rate}</td>            <td align=\"right\" colspan=\"2\">{Price}</td>         </tr></Repeater>         </tbody></table>         </td>         </tr>         <tr align=\"right\">            <td colspan=\"5\">Current Invoice Total</td>            <td>{CurSymbol}{TotalInvoice}</td>         </tr>         <tr align=\"right\">            <td colspan=\"5\">{ServiceTaxTitle}</td>            <td>{CurSymbol}{Tax}</td>         </tr>         <tr align=\"right\">            <td colspan=\"5\"><strong>Total Due </strong></td>            <td><strong>{CurSymbol}{GrandTotal}</strong></td>         </tr>         <tr align=\"left\">            <td colspan=\"6\"><strong>{CurSymbol}{CostinWords} </strong></td>         </tr>      </tbody>   </table>   <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">      <tbody>         <tr><td>PAN No. ABC12345<br />Service Tax No. 12345222</td>         </tr>      </tbody>   </table>   <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse;margin-bottom:5px;\" width=\"100%\">      <thead>         <tr>            <th align=\"left\">Courier cheque at: </th>         </tr>      </thead>      <tbody>         <tr>            <td>{CompanyAddress}</td>         </tr>      </tbody>   </table>   <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse;margin:0;\" width=\"100%\">      <tbody>         <tr>            <td valign=\"top\" align=\"left\"><b>Payment Options</b><br />               <table border=\"1\" width=\"100%\" cellspacing=\"0\" cellpadding=\"10\">                  <tbody>                     <tr>                        <td style=\"border:#000 1px solid;border-radius:2px;\" width=\"100%\"><b>Cheque: </b>Intelegain Technologies Pvt Ltd <br /><b>Bank Wire Transfer: </b>Beneficiary Account Name: Intelegain Technologies Pvt. Ltd. <br />Beneficiary Account Number: 0990317119  <br />Receiving Bank Name: Citibank N.A   <br />Receiving Bank Swiftbic: CITIINBX    <br />IFSC Code: CITI0000038  <br />ABA Routing no: 021000089   <br />Receiving Bank Address: <br />Citibank N.A  <br />Mahatma Phule Bhavan  <br />Palm Beach Road Junction <br />Sector-17, Vashi, New Mumbai – 400705, India                         </td>                     </tr> <tr><td>Note: 'This is a computer generated invoice and does not require signature.'</td></tr>                 </tbody>               </table>            </td>         </tr>      </tbody>   </table></div>";
        string desc = "";
        string tCost = "";
        string amount = "";
        string srno = "";
        string description = "In my example I made a class noBorder that I gave to one Then I use a simple selector tr.noBorder td to makefdsfdfdsfdfdfdfdsfdfdsfdsfdsfdfdsfd";
        int icount = 0;
        Regex regex = new Regex("<Repeater>(.*)</Repeater>");
        var extractTag = regex.Match(strHtml);
        string strReplace = extractTag.Groups[1].ToString();
        string findTag = strReplace;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            desc = "{Description" + i + "}";
            tCost = "{Rate" + i + "}";
            amount = "{Price" + i + "}";
            srno = "{SrNo" + i + "}";

            strReplace = strReplace.Replace("{Description}", desc);
            strReplace = strReplace.Replace("{Rate}", tCost);
            strReplace = strReplace.Replace("{Price}", amount);
            strReplace = strReplace.Replace("{SrNo}", srno);

            if (((dt.Rows.Count) - i) > 1)
                strReplace = strReplace + findTag;

        }
        if (strReplace != "")
            strHtml = strHtml.Replace(findTag, strReplace);

        //strHtml = strHtml.Replace("{ServiceTaxTitle}", "{ServiceTaxTitle1}{ServiceTaxTitle2}{ServiceTaxTitle3}");
        //strHtml = strHtml.Replace("{Tax}", "{Tax1}{Tax2}{Tax3}");

        strHtml = strHtml.Replace("{ServiceTaxTitle}", "{ServiceTaxTitle1}{ServiceTaxTitle2}{ServiceTaxTitle3}{ServiceTaxTitle7}{ServiceTaxTitle8}{ServiceTaxTitle9}{ServiceTaxTitle10}{ServiceTaxTitle4}{ServiceTaxTitle5}{ServiceTaxTitle6}");
        strHtml = strHtml.Replace("{Tax}", "{Tax1}{Tax2}{Tax3}{Tax7}{Tax8}{Tax9}{Tax10}{Tax4}{Tax5}{Tax6}");

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strHtml = strHtml.Replace("{SrNo" + i + "}", (i + 1) + "<br />");
            strHtml = strHtml.Replace("{Description" + i + "}", dt.Rows[i]["Description"].ToString() + "<br />");
            strHtml = strHtml.Replace("{Rate" + i + "}", getCurrencyFormat(float.Parse(dt.Rows[i]["Rate"].ToString())) + "<br />");

            if (((dt.Rows.Count) - i) > 1)
                strHtml = strHtml.Replace("{Price" + i + "}", getCurrencyFormat(float.Parse(dt.Rows[i]["Price"].ToString())) + "<br />");
            else
                strHtml = strHtml.Replace("{Price" + i + "}", getCurrencyFormat(float.Parse(dt.Rows[i]["Price"].ToString())) + "<br /><br />");

            if (float.Parse(dt.Rows[i]["Tax1"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle1}", "Service Tax " + dt.Rows[i]["Tax1"].ToString() + "%" + "<br />");
                strHtml = strHtml.Replace("{Tax1}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["Tax1"].ToString())).ToString() + "<br />"); //dt.Rows[0]["currSymbol"].ToString() +


                //strHtml = strHtml.Replace("{Tax1}", getCurrencyFormat((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["Tax1"].ToString())) + "<br />"); //dt.Rows[0]["currSymbol"].ToString() +

            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle1}", "");
                strHtml = strHtml.Replace("{Tax1}", "");
            }
            if (float.Parse(dt.Rows[i]["Tax2"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle2}", "Swach Bharat Cess " + dt.Rows[i]["Tax2"].ToString() + "%" + "<br />");
                strHtml = strHtml.Replace("{Tax2}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["Tax2"].ToString())).ToString() + "<br />");//dt.Rows[0]["currSymbol"].ToString() +

                //strHtml = strHtml.Replace("{Tax2}", getCurrencyFormat((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["Tax2"].ToString())) + "<br />");//dt.Rows[0]["currSymbol"].ToString() +

            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle2}", "");
                strHtml = strHtml.Replace("{Tax2}", "");
            }

            if (float.Parse(dt.Rows[i]["Tax3"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle3}", "Krishi Kalyan Cess " + dt.Rows[i]["Tax3"].ToString() + "%" + "<br />");

                strHtml = strHtml.Replace("{Tax3}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["Tax3"].ToString())).ToString() + "<br />");//dt.Rows[0]["currSymbol"].ToString() +

                // strHtml = strHtml.Replace("{Tax3}", getCurrencyFormat((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["Tax3"].ToString())) + "<br />");//dt.Rows[0]["currSymbol"].ToString() +

            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle3}", "");
                strHtml = strHtml.Replace("{Tax3}", "");
            }

            if (float.Parse(dt.Rows[i]["CGST"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle7}", "CGST " + dt.Rows[i]["CGST"].ToString() + "%" + "<br />");

                strHtml = strHtml.Replace("{Tax7}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["CGST"].ToString())).ToString() + "<br />");//dt.Rows[0]["currSymbol"].ToString() +
            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle7}", "");
                strHtml = strHtml.Replace("{Tax7}", "");
            }

            if (float.Parse(dt.Rows[i]["SGST"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle8}", "SGST " + dt.Rows[i]["SGST"].ToString() + "%" + "<br />");

                strHtml = strHtml.Replace("{Tax8}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["SGST"].ToString())).ToString() + "<br />");//dt.Rows[0]["currSymbol"].ToString() +
            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle8}", "");
                strHtml = strHtml.Replace("{Tax8}", "");
            }
            if (float.Parse(dt.Rows[i]["IGST"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle9}", "IGST " + dt.Rows[i]["IGST"].ToString() + "%" + "<br />");

                strHtml = strHtml.Replace("{Tax9}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["IGST"].ToString())).ToString() + "<br />");//dt.Rows[0]["currSymbol"].ToString() +
            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle9}", "");
                strHtml = strHtml.Replace("{Tax9}", "");
            }


            if (float.Parse(dt.Rows[i]["GST"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle10}", "GST " + dt.Rows[i]["GST"].ToString() + "%" + "<br />");

                strHtml = strHtml.Replace("{Tax10}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["GST"].ToString())).ToString() + "<br />");//dt.Rows[0]["currSymbol"].ToString() +
            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle10}", "");
                strHtml = strHtml.Replace("{Tax10}", "");
            }

            if (float.Parse(dt.Rows[i]["TransCharge"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle4}", "Other Charges " + dt.Rows[i]["TransCharge"].ToString() + "%" + "<br />");
                strHtml = strHtml.Replace("{Tax4}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["TransCharge"].ToString())).ToString() + "<br />");//dt.Rows[0]["currSymbol"].ToString() +


                // strHtml = strHtml.Replace("{Tax4}", getCurrencyFormat((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["TransCharge"].ToString())) + "<br />");//dt.Rows[0]["currSymbol"].ToString() +

            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle4}", "");
                strHtml = strHtml.Replace("{Tax4}", "");
            }

            if (float.Parse(dt.Rows[i]["CSTCharge"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle5}", "CST " + dt.Rows[i]["CSTCharge"].ToString() + "%" + "<br />");
                strHtml = strHtml.Replace("{Tax5}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["CSTCharge"].ToString())).ToString() + "<br />");//dt.Rows[0]["currSymbol"].ToString() +


                // strHtml = strHtml.Replace("{Tax4}", getCurrencyFormat((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["TransCharge"].ToString())) + "<br />");//dt.Rows[0]["currSymbol"].ToString() +

            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle5}", "");
                strHtml = strHtml.Replace("{Tax5}", "");
            }

            if (float.Parse(dt.Rows[i]["VATCharge"].ToString()) > 0)
            {
                icount += 1;
                strHtml = strHtml.Replace("{ServiceTaxTitle6}", "VAT " + dt.Rows[i]["VATCharge"].ToString() + "%" + "<br />");
                strHtml = strHtml.Replace("{Tax6}", Math.Round((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["VATCharge"].ToString())).ToString() + "<br />");//dt.Rows[0]["currSymbol"].ToString() +


                // strHtml = strHtml.Replace("{Tax4}", getCurrencyFormat((float.Parse(dt.Rows[i]["InvoiceTotal"].ToString()) / 100) * float.Parse(dt.Rows[i]["TransCharge"].ToString())) + "<br />");//dt.Rows[0]["currSymbol"].ToString() +

            }
            else
            {
                strHtml = strHtml.Replace("{ServiceTaxTitle6}", "");
                strHtml = strHtml.Replace("{Tax6}", "");
            }


        }
        if (icount == 0)
        {
            Regex regexTR = new Regex("<Tax>(.*)</Tax>");
            var extractTag_TR = regexTR.Match(strHtml);
            string strReplace_TR = extractTag_TR.Groups[1].ToString();
            //strHtml = strHtml.Replace(strReplace_TR, "");
        }
        //Added by trupti
        if (dt.Rows[0][8].ToString() == "")
        {
            strHtml = strHtml.Replace("GSTIN:", "");
        }

        return strHtml;
    }
    //code by azar

    //public string IntegerToWordsConversion(Int64 rup, string CurrencySymbol)
    //{
    //    if (CurrencySymbol == "INR")
    //        return NumberToWordsINR(int.Parse(rup.ToString()));
    //    else
    //        return NumberToWords(int.Parse(rup.ToString()));
    //}

    //public static string NumberToWordsINR(int number)
    //{
    //    if (number == 0)
    //        return "zero";

    //    if (number < 0)
    //        return "minus " + NumberToWordsINR(Math.Abs(number));

    //    string words = "";

    //    if ((number / 10000000) > 0)
    //    {
    //        words += NumberToWordsINR(number / 10000000) + " crror ";
    //        number %= 10000000;
    //    }

    //    if ((number / 100000) > 0)
    //    {
    //        words += NumberToWordsINR(number / 100000) + " lakh ";
    //        number %= 100000;
    //    }

    //    if ((number / 1000) > 0)
    //    {
    //        words += NumberToWordsINR(number / 1000) + " thousand ";
    //        number %= 1000;
    //    }

    //    if ((number / 100) > 0)
    //    {
    //        words += NumberToWordsINR(number / 100) + " hundred ";
    //        number %= 100;
    //    }

    //    if (number > 0)
    //    {
    //        if (words != "")
    //            words += " ";

    //        var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
    //        var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

    //        if (number < 20)
    //            words += unitsMap[number];
    //        else
    //        {
    //            words += tensMap[number / 10];
    //            if ((number % 10) > 0)
    //                words += "-" + unitsMap[number % 10];
    //        }
    //    }

    //    return words;
    //}
    //public static string NumberToWords(int number)
    //    {
    //        if (number == 0)
    //            return "zero";

    //        if (number < 0)
    //            return "minus " + NumberToWords(Math.Abs(number));

    //        string words = "";

    //        if ((number / 1000000000) > 0)
    //        {
    //            words += NumberToWords(number / 1000000000) + " billion ";
    //            number %= 1000000000;
    //        }

    //        if ((number / 1000000) > 0)
    //        {
    //            words += NumberToWords(number / 1000000) + " million ";
    //            number %= 1000000;
    //        }

    //        if ((number / 1000) > 0)
    //        {
    //            words += NumberToWords(number / 1000) + " thousand ";
    //            number %= 1000;
    //        }

    //        if ((number / 100) > 0)
    //        {
    //            words += NumberToWords(number / 100) + " hundred ";
    //            number %= 100;
    //        }

    //        if (number > 0)
    //        {
    //            if (words != "")
    //                words += " ";

    //            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
    //            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

    //            if (number < 20)
    //                words += unitsMap[number];
    //            else
    //            {
    //                words += tensMap[number / 10];
    //                if ((number % 10) > 0)
    //                    words += "-" + unitsMap[number % 10];
    //            }
    //        }

    //        return words;
    //    }
    
    //public string IntegerToWordsConversion1(Int64 rup, string CurrencySymbol)
    //{
    //    string result = "";

    //    Int64 res;
    //    if (CurrencySymbol == "INR")
    //    {
    //        if ((rup / 10000000) > 0)
    //        {
    //            res = rup / 10000000;
    //            rup = rup % 10000000;
    //            result = result + ' ' + RupeesToWords(res) + " Crore";
    //        }
    //        if ((rup / 100000) > 0)
    //        {
    //            res = rup / 100000;
    //            rup = rup % 100000;
    //            result = result + ' ' + RupeesToWords(res) + " Lakh";
    //        }
    //        if ((rup / 1000) > 0)
    //        {
    //            res = rup / 1000;
    //            rup = rup % 1000;
    //            result = result + ' ' + RupeesToWords(res) + " Thousand";
    //        }
    //        if ((rup / 100) > 0)
    //        {
    //            res = rup / 100;
    //            rup = rup % 100;
    //            result = result + ' ' + RupeesToWords(res) + " Hundred";
    //        }
    //        if ((rup % 10) >= 0)
    //        {
    //            res = rup % 100;
    //            result = result + " " + RupeesToWords(res);
    //        }
    //        result = result + ' ' + "Rupees only";
    //    }
    //    else if (CurrencySymbol == "US $")
    //    {
    //        result = IntegerToWords(Convert.ToString(rup));
    //    }
    //    else if (string.Compare(CurrencySymbol, "AU $", true) == 0)
    //    {
    //        result = IntegerToWords(Convert.ToString(rup));
    //    }
    //    return result;
    //}
    //public string RupeesToWords(Int64 rup)
    //{
    //    string result = "";
    //    if ((rup >= 1) && (rup <= 10))
    //    {
    //        if ((rup % 10) == 1) result = "One";
    //        if ((rup % 10) == 2) result = "Two";
    //        if ((rup % 10) == 3) result = "Three";
    //        if ((rup % 10) == 4) result = "Four";
    //        if ((rup % 10) == 5) result = "Five";
    //        if ((rup % 10) == 6) result = "Six";
    //        if ((rup % 10) == 7) result = "Seven";
    //        if ((rup % 10) == 8) result = "Eight";
    //        if ((rup % 10) == 9) result = "Nine";
    //        if ((rup % 10) == 0) result = "Ten";
    //    }
    //    if (rup > 9 && rup < 20)
    //    {
    //        if (rup == 11) result = "Eleven";
    //        if (rup == 12) result = "Twelve";
    //        if (rup == 13) result = "Thirteen";
    //        if (rup == 14) result = "Forteen";
    //        if (rup == 15) result = "Fifteen";
    //        if (rup == 16) result = "Sixteen";
    //        if (rup == 17) result = "Seventeen";
    //        if (rup == 18) result = "Eighteen";
    //        if (rup == 19) result = "Nineteen";
    //    }
    //    if (rup > 20 && (rup / 10) == 2 && (rup % 10) == 0) result = "Twenty";
    //    if (rup > 20 && (rup / 10) == 3 && (rup % 10) == 0) result = "Thirty";
    //    if (rup > 20 && (rup / 10) == 4 && (rup % 10) == 0) result = "Forty";
    //    if (rup > 20 && (rup / 10) == 5 && (rup % 10) == 0) result = "Fifty";
    //    if (rup > 20 && (rup / 10) == 6 && (rup % 10) == 0) result = "Sixty";
    //    if (rup > 20 && (rup / 10) == 7 && (rup % 10) == 0) result = "Seventy";
    //    if (rup > 20 && (rup / 10) == 8 && (rup % 10) == 0) result = "Eighty";
    //    if (rup > 20 && (rup / 10) == 9 && (rup % 10) == 0) result = "Ninty";

    //    if (rup > 20 && (rup / 10) == 2 && (rup % 10) != 0)
    //    {
    //        if ((rup % 10) == 1) result = "Twenty One";
    //        if ((rup % 10) == 2) result = "Twenty Two";
    //        if ((rup % 10) == 3) result = "Twenty Three";
    //        if ((rup % 10) == 4) result = "Twenty Four";
    //        if ((rup % 10) == 5) result = "Twenty Five";
    //        if ((rup % 10) == 6) result = "Twenty Six";
    //        if ((rup % 10) == 7) result = "Twenty Seven";
    //        if ((rup % 10) == 8) result = "Twenty Eight";
    //        if ((rup % 10) == 9) result = "Twenty Nine";
    //    }
    //    if (rup > 20 && (rup / 10) == 3 && (rup % 10) != 0)
    //    {
    //        if ((rup % 10) == 1) result = "Thirty One";
    //        if ((rup % 10) == 2) result = "Thirty Two";
    //        if ((rup % 10) == 3) result = "Thirty Three";
    //        if ((rup % 10) == 4) result = "Thirty Four";
    //        if ((rup % 10) == 5) result = "Thirty Five";
    //        if ((rup % 10) == 6) result = "Thirty Six";
    //        if ((rup % 10) == 7) result = "Thirty Seven";
    //        if ((rup % 10) == 8) result = "Thirty Eight";
    //        if ((rup % 10) == 9) result = "Thirty Nine";
    //    }
    //    if (rup > 20 && (rup / 10) == 4 && (rup % 10) != 0)
    //    {
    //        if ((rup % 10) == 1) result = "Forty One";
    //        if ((rup % 10) == 2) result = "Forty Two";
    //        if ((rup % 10) == 3) result = "Forty Three";
    //        if ((rup % 10) == 4) result = "Forty Four";
    //        if ((rup % 10) == 5) result = "Forty Five";
    //        if ((rup % 10) == 6) result = "Forty Six";
    //        if ((rup % 10) == 7) result = "Forty Seven";
    //        if ((rup % 10) == 8) result = "Forty Eight";
    //        if ((rup % 10) == 9) result = "Forty Nine";
    //    }
    //    if (rup > 20 && (rup / 10) == 5 && (rup % 10) != 0)
    //    {
    //        if ((rup % 10) == 1) result = "Fifty One";
    //        if ((rup % 10) == 2) result = "Fifty Two";
    //        if ((rup % 10) == 3) result = "Fifty Three";
    //        if ((rup % 10) == 4) result = "Fifty Four";
    //        if ((rup % 10) == 5) result = "Fifty Five";
    //        if ((rup % 10) == 6) result = "Fifty Six";
    //        if ((rup % 10) == 7) result = "Fifty Seven";
    //        if ((rup % 10) == 8) result = "Fifty Eight";
    //        if ((rup % 10) == 9) result = "Fifty Nine";
    //    }
    //    if (rup > 20 && (rup / 10) == 6 && (rup % 10) != 0)
    //    {
    //        if ((rup % 10) == 1) result = "Sixty One";
    //        if ((rup % 10) == 2) result = "Sixty Two";
    //        if ((rup % 10) == 3) result = "Sixty Three";
    //        if ((rup % 10) == 4) result = "Sixty Four";
    //        if ((rup % 10) == 5) result = "Sixty Five";
    //        if ((rup % 10) == 6) result = "Sixty Six";
    //        if ((rup % 10) == 7) result = "Sixty Seven";
    //        if ((rup % 10) == 8) result = "Sixty Eight";
    //        if ((rup % 10) == 9) result = "Sixty Nine";
    //    }
    //    if (rup > 20 && (rup / 10) == 7 && (rup % 10) != 0)
    //    {
    //        if ((rup % 10) == 1) result = "Seventy One";
    //        if ((rup % 10) == 2) result = "Seventy Two";
    //        if ((rup % 10) == 3) result = "Seventy Three";
    //        if ((rup % 10) == 4) result = "Seventy Four";
    //        if ((rup % 10) == 5) result = "Seventy Five";
    //        if ((rup % 10) == 6) result = "Seventy Six";
    //        if ((rup % 10) == 7) result = "Seventy Seven";
    //        if ((rup % 10) == 8) result = "Seventy Eight";
    //        if ((rup % 10) == 9) result = "Seventy Nine";
    //    }
    //    if (rup > 20 && (rup / 10) == 8 && (rup % 10) != 0)
    //    {
    //        if ((rup % 10) == 1) result = "Eighty One";
    //        if ((rup % 10) == 2) result = "Eighty Two";
    //        if ((rup % 10) == 3) result = "Eighty Three";
    //        if ((rup % 10) == 4) result = "Eighty Four";
    //        if ((rup % 10) == 5) result = "Eighty Five";
    //        if ((rup % 10) == 6) result = "Eighty Six";
    //        if ((rup % 10) == 7) result = "Eighty Seven";
    //        if ((rup % 10) == 8) result = "Eighty Eight";
    //        if ((rup % 10) == 9) result = "Eighty Nine";
    //    }
    //    if (rup > 20 && (rup / 10) == 9 && (rup % 10) != 0)
    //    {
    //        if ((rup % 10) == 1) result = "Ninty One";
    //        if ((rup % 10) == 2) result = "Ninty Two";
    //        if ((rup % 10) == 3) result = "Ninty Three";
    //        if ((rup % 10) == 4) result = "Ninty Four";
    //        if ((rup % 10) == 5) result = "Ninty Five";
    //        if ((rup % 10) == 6) result = "Ninty Six";
    //        if ((rup % 10) == 7) result = "Ninty Seven";
    //        if ((rup % 10) == 8) result = "Ninty Eight";
    //        if ((rup % 10) == 9) result = "Ninty Nine";
    //    }
    //    return result;
    //}
    public string IntegerToWords(string inputNum1)
    {
        inputNum1 = (Math.Round(Convert.ToDecimal(inputNum1))).ToString();
        int dig1, dig2, dig3, level = 0, lasttwo, threeDigits;
        double inputNum = Convert.ToDouble(inputNum1);
        string retval = "";
        string x = "";
        string[] ones ={
                        "Zero",
                        "One",
                        "Two",
                        "Three",
                        "Four",
                        "Five",
                        "Six",
                        "Seven",
                        "Eight",
                        "Nine",
                        "Ten",
                        "Eleven",
                        "Twelve",
                        "Thirteen",
                        "Fourteen",
                        "Fifteen",
                        "Sixteen",
                        "Seventeen",
                        "Eighteen",
                        "Nineteen"
                      };
        string[] tens ={
                        "Zero",
                        "Ten",
                        "Twenty",
                        "Thirty",
                        "Forty",
                        "Fifty",
                        "Sixty",
                        "Seventy",
                        "Eighty",
                        "Ninety"
                      };
        string[] thou ={
                        "",
                        "Thousand",
                        "Million"
                      };

        bool isNegative = false;
        if (inputNum < 0)
        {
            isNegative = true;
            inputNum *= -1;
        }

        if (inputNum == 0)
            return ("zero");

        string s = inputNum.ToString();

        while (s.Length > 0)
        {
            // Get the three rightmost characters
            x = (s.Length < 3) ? s : s.Substring(s.Length - 3, 3);

            // Separate the three digits
            threeDigits = int.Parse(x);
            lasttwo = threeDigits % 100;
            dig1 = threeDigits / 100;
            dig2 = lasttwo / 10;
            dig3 = (threeDigits % 10);

            // append a "thousand" where appropriate
            if (level > 0 && dig1 + dig2 + dig3 > 0)
            {
                retval = thou[level] + " " + retval;
                retval = retval.Trim();
            }

            // check that the last two digits is not a zero
            if (lasttwo > 0)
            {
                if (lasttwo < 20) // if less than 20, use "ones" only
                    retval = ones[lasttwo] + " " + retval;
                else // otherwise, use both "tens" and "ones" array
                    retval = tens[dig2] + " " + ones[dig3] + " " + retval;
            }

            // if a hundreds part is there, translate it
            if (dig1 > 0)
                retval = ones[dig1] + " Hundred " + retval;

            s = (s.Length - 3) > 0 ? s.Substring(0, s.Length - 3) : "";
            level++;
        }

        while (retval.IndexOf("  ") > 0)
            retval = retval.Replace("  ", " ");

        retval = retval.Trim();

        if (isNegative)
            retval = "negative " + retval;

        return (retval.Replace("Zero", ""));

    }
    

    private string getCurrencyFormat(float amnt)
    {
        CultureInfo cuInfo = new CultureInfo("en-IN");
        string curr = ((Convert.ToDecimal(amnt)).ToString("C", cuInfo)).Remove(0, 2).Trim('0').Trim('.');  //.trim() for 100.00 format
        return curr;
    }


    //GST Code:
    public static List<ProformaInvoiceBLL> BindCode(string mode)
    {
        ProformaInvoiceDAL objCode = new ProformaInvoiceDAL();
        return objCode.BindCode(mode);//, 0);
    }

    public static List<ProformaInvoiceBLL> GetLocationDetails(int projId, int LocationId)
    {
        List<ProformaInvoiceBLL> objLocation = new List<ProformaInvoiceBLL>();
        objLocation = new ProformaInvoiceDAL().GetLocationDetails(projId, LocationId, "GetLocationDetails");
        return objLocation;
    }

    public static List<ProformaInvoiceBLL> GetGSTPercent(int CodeID)
    {
        List<ProformaInvoiceBLL> objCodeID = new List<ProformaInvoiceBLL>();
        objCodeID = new ProformaInvoiceDAL().GetGSTPercent(CodeID, "GetGSTPercentage");
        return objCodeID;
    }

}



public class ProInvoiceModel
{
    public int ProformaInvoiceID { get; set; }
    public int projId { get; set; }
    public string Description { get; set; }
    public int ProjectMilestoneID { get; set; }
    public int Quantity { get; set; }
    public float Rate { get; set; }
    public float Amount { get; set; }
    public float BalanceAmount { get; set; }
    public int ProformaInvoiceDetailID { get; set; }
    public int modifiedon { get; set; }
    public float OriginalAmount { get; set; }
    public float CalBalance { get; set; }
    public int ProjectInvoiceID { get; set; }


    public ProInvoiceModel(int ProformaInvoiceID, int projId, string Description, int ProjectMilestoneID, int Quantity, float Rate, float Amount)
    {
        this.ProformaInvoiceID = ProformaInvoiceID;
        this.projId = projId;
        this.Description = Description;
        this.ProjectMilestoneID = ProjectMilestoneID;
        this.Quantity = Quantity;
        this.Rate = Rate;
        this.Amount = Amount;
    }

    public ProInvoiceModel()
    {
        // TODO: Complete member initialization
    }

    public int insertProInvoiceDetails(int ProformaInvoiceID, int projId, string Description, int ProjectMilestoneID, int Quantity, float Rate, float Amount, float BalanceAmount)
    {
        ProInvoiceModel objProInvoice = new ProInvoiceModel();
        objProInvoice.ProformaInvoiceID = ProformaInvoiceID;
        objProInvoice.projId = projId;
        objProInvoice.Description = Description;
        objProInvoice.ProjectMilestoneID = ProjectMilestoneID;
        objProInvoice.Quantity = Quantity;
        objProInvoice.Rate = Rate;
        objProInvoice.Amount = Amount;
        objProInvoice.BalanceAmount = BalanceAmount;
        ProformaInvoiceDAL objInsertInvoice = new ProformaInvoiceDAL();
        return objInsertInvoice.InsertProInvoiceDetails(objProInvoice);
    }

    public int insertTaxInvoice(int ProjectInvoiceID, int projId, string Description, int ProjectMilestoneID, int Quantity, float Rate, float Amount, float BalanceAmount)
    {
        ProInvoiceModel objInvoice = new ProInvoiceModel();
        objInvoice.ProjectInvoiceID = ProjectInvoiceID;
        objInvoice.projId = projId;
        objInvoice.Description = Description;
        objInvoice.ProjectMilestoneID = ProjectMilestoneID;
        objInvoice.Quantity = Quantity;
        objInvoice.Rate = Rate;
        objInvoice.Amount = Amount;
        objInvoice.BalanceAmount = BalanceAmount;
        ProformaInvoiceDAL objInsertInvoice = new ProformaInvoiceDAL();
        return objInsertInvoice.insertTaxInvoice(objInvoice);
    }

    public int updateProInvoiceDetails(int ProformaInvoiceIDDetailsID, int ProformaInvoiceID, int ProjectMilestoneID, string Description, int Quantity, float Rate, float Amount, int modifiedon, float BalanceAmount)
    {
        ProInvoiceModel objIDetails = new ProInvoiceModel();
        objIDetails.ProformaInvoiceDetailID = ProformaInvoiceIDDetailsID;
        objIDetails.ProformaInvoiceID = ProformaInvoiceID;
        objIDetails.ProjectMilestoneID = ProjectMilestoneID;
        objIDetails.Description = Description;
        objIDetails.Quantity = Quantity;
        objIDetails.Rate = Rate;
        objIDetails.Amount = Amount;
        objIDetails.modifiedon = modifiedon;
        objIDetails.BalanceAmount = BalanceAmount;
        ProformaInvoiceDAL objInsertInvoice = new ProformaInvoiceDAL();
        return objInsertInvoice.UpdateInvoiceDetails(objIDetails);
    }

    public bool IfExistsInvoiceNo(string InvoiceNo, int ProformaInvoiceID, int ProjID, int LocationID)
    {
        ProformaInvoiceDAL objInvoice = new ProformaInvoiceDAL();
        return objInvoice.IfExistsInvoiceNo(InvoiceNo, ProformaInvoiceID, ProjID, LocationID);
    }


    public static void DeleteInvoiceMilestone(int ProformaInvoiceDetailID)
    {
        ProInvoiceModel delmile = new ProInvoiceModel();
        delmile.ProformaInvoiceDetailID = ProformaInvoiceDetailID;
        ProformaInvoiceDAL objDelete = new ProformaInvoiceDAL();
        objDelete.DeleteInvoiceMile(ProformaInvoiceDetailID);
    }
}