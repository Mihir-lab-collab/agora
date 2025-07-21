using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Customer.Model;
using Customer.DAL;

/// <summary>
/// Summary description for InvoicePayment
/// </summary>
public class InvoicePayment
{
	public InvoicePayment()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataSet GetInvoicePaymentDetails(string PID,string mode)
    {
        ProjectInvoiceDAL objPrjInvoice = new ProjectInvoiceDAL();
        return objPrjInvoice.GetInvoicePaymentDetails(PID, mode);
    }

    public DataTable  AdjustPayment(DataTable dt,float amount)
    {
        if (dt.Rows.Count > 0 && amount > 0)
        { 
            for(int i=0 ;i<dt.Rows.Count; i++)
            {
                float invAmount = 0;
                invAmount = Convert.ToInt32(dt.Rows[i]["Amount"]);
                if (invAmount < amount)
                {
                    dt.Rows[i]["Amount"] = amount;
                    return dt;
                }
                if (invAmount != amount)
                {
                    float adjust = 0;
                    adjust = amount;
                    if (amount > invAmount)
                    {
                        amount = invAmount - amount;
                        dt.Rows[i]["Amount"] = (adjust - amount);
                    }
                    else
                        dt.Rows[i]["Amount"] = amount;
                }
            }
            dt.AcceptChanges();
        }
        return dt;
    }
}