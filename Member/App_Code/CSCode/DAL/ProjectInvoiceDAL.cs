using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;
using Customer.Model;
using System.Text;
using System.Web.UI;
/// <summary>
/// Summary description for ProjectInvoiceDAL
/// </summary>
namespace Customer.DAL
{
    public class ProjectInvoiceDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        
        public ProjectInvoiceDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetMonthlyRevenue(string FromDate, string ToDate)
        {
            CultureInfo cuInfo = new CultureInfo("en-IN");
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("sp_MonthlyDueAmount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
            cmd.Parameters.AddWithValue("@TODATE", ToDate);
            using (con)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public List<GetInvoice> GetInvoices(int ProjID, string FromDate, string ToDate)
        {
            List<GetInvoice> Invoice = new List<GetInvoice>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
            cmd.Parameters.AddWithValue("@Mode", "GETINVOICE");
            cmd.Parameters.AddWithValue("@ProjectID", ProjID);
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", 0);
            cmd.Parameters.AddWithValue("@PaymentID", 0);
            cmd.Parameters.AddWithValue("@Amount", 0);
            cmd.Parameters.AddWithValue("@TaxCollected", 0);
            cmd.Parameters.AddWithValue("@PaymentType", 0);
            cmd.Parameters.AddWithValue("@CurrencID", 0);
            cmd.Parameters.AddWithValue("@ExRate", 0);
            cmd.Parameters.AddWithValue("@CreditAmount", 0);
            cmd.Parameters.AddWithValue("@Comment", "");
            cmd.Parameters.AddWithValue("@userId", 0);
            cmd.Parameters.AddWithValue("@ProjectDetailID", 0);
            cmd.Parameters.AddWithValue("@Description","");
            cmd.Parameters.AddWithValue("@PaidAmount", 0);
           // cmd.Parameters.AddWithValue("@BalanceAmount", 0);
            cmd.Parameters.AddWithValue("@isEdited",0);
            cmd.Parameters.AddWithValue("@PType",0);
            cmd.Parameters.AddWithValue("@FromDate", FromDate);
            cmd.Parameters.AddWithValue("@ToDate", ToDate);

            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    CultureInfo cuInfo = new CultureInfo("en-IN");
                    while (reader.Read())
                    {
                        GetInvoice obj = new GetInvoice();
                        obj.custId = Convert.ToInt32(reader["custId"]);
                        obj.PaymentID = Convert.ToInt32(reader["PaymentID"]);
                        obj.ProjID = Convert.ToInt32(reader["ProjID"]);
                        obj.projName = Convert.ToString(reader["projName"]);
                       // obj.CurrencyID = Convert.ToInt32(reader["CurrencyID"]);
                        obj.currSymbol = Convert.ToString(reader["currSymbol"]);
                       // obj.custName = Convert.ToString(reader["custName"]);
                       // obj.ExRate = Convert.ToInt32(reader["ExRate"]);
                        obj.InvoiceDate = Convert.ToString(reader["PaidDate"]);
                        obj.Amount = Convert.ToString(reader["Amount"]);
                        obj.TaxCollected = Convert.ToInt32(reader["TaxCollected"]);
                        obj.BalanceAmount = Convert.ToString(reader["CreditAmount"]);
                        obj.Description = Convert.ToString(reader["paymenttype"]);
                        obj.ReceiptDate = Convert.ToString(reader["ReceiptDate"]);

                        obj.Amount = obj.currSymbol + ((Convert.ToDecimal(obj.Amount)).ToString("C", cuInfo)).Remove(0, 2).Trim();
                        obj.BalanceAmount = obj.currSymbol + ((Convert.ToDecimal(obj.BalanceAmount)).ToString("C", cuInfo)).Remove(0, 2).Trim();

                        Invoice.Add(obj);
                    }
                }

            }
            return Invoice;
        }


        public List<KeyValueModel> GetPaymentType()
        {

            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "PAYMENTTYPE");
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                List<KeyValueModel> list = new List<KeyValueModel>();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new KeyValueModel()
                    {
                        Key = reader["PaymentType"].ToString(),
                        Value = Convert.ToInt32(reader["TypeID"].ToString())
                    });

                }
                return list;
            }

        }


        //public int SaveInvoicePayment(InvoicePaymentModel objInvoice)
        //{
        //    int iReturn = 0;

        //    return iReturn;
        //}

        public DataSet GetInvoicePaymentDetails(string PID,string mode )
        {
            CultureInfo cuInfo = new CultureInfo("en-IN");
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@ProjectID", Convert.ToInt32(PID));
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", 0);
            cmd.Parameters.AddWithValue("@PaymentID", Convert.ToInt32(PID));
            using (con)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                //if (ds.Tables[1].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds.Tables[1].Rows.Count;i++ )
                //        ds.Tables[1].Rows[i]["TotalAmount"] = (((Convert.ToDecimal(ds.Tables[1].Rows[i]["TotalAmount"])).ToString("C", cuInfo)).Remove(0, 2).Trim()).ToString();
                //}
                
                return ds;
            }
        }

        public int SavePaymentHeader(ProjecInvoiceBLL objheader)
        {
            int dtProjInvID = 0;
            DateTime frm = DateTime.ParseExact(objheader.InvoiceDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            objheader.InvoiceDate = frm.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "SAVEPAYMENT");
                cmd.Parameters.AddWithValue("@ProjectID", objheader.ProjID);
                cmd.Parameters.AddWithValue("@ProjectInvoiceID", 0);
                cmd.Parameters.AddWithValue("@PaymentID", objheader.ProjectPaymentID);
                cmd.Parameters.AddWithValue("@Amount", objheader.Amount);
                cmd.Parameters.AddWithValue("@PaymentType", objheader.PaymentType);
                cmd.Parameters.AddWithValue("@CurrencID", objheader.CurrencyID);
                cmd.Parameters.AddWithValue("@ExRate", objheader.ExRate);
                cmd.Parameters.AddWithValue("@CreditAmount", objheader.CreditAmount);
                cmd.Parameters.AddWithValue("@Comment", objheader.Description);
                cmd.Parameters.AddWithValue("@userId", objheader.InsertedBy);
                cmd.Parameters.AddWithValue("@ProjectDetailID", 0);
                cmd.Parameters.AddWithValue("@Description", "");
                cmd.Parameters.AddWithValue("@PaidAmount", 0);
                cmd.Parameters.AddWithValue("@BalanceAmount", 0);
                cmd.Parameters.AddWithValue("@isEdited", 0);
                cmd.Parameters.AddWithValue("@PType", 0);
                cmd.Parameters.AddWithValue("@FromDate", "");
                cmd.Parameters.AddWithValue("@ToDate", "");
                cmd.Parameters.AddWithValue("@PaymentDate",objheader.InvoiceDate);
                cmd.Parameters.AddWithValue("@InvoiceIDS", "");
                cmd.Parameters.AddWithValue("@isCredit", objheader.isCredited);
                cmd.Parameters.AddWithValue("@TaxCollected", objheader.TaxCollected);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                dtProjInvID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return dtProjInvID;
        }

        public int SavePaymentDetails(InvoicePaymentModel objDetails,ProjecInvoiceBLL header)
        {
            int dtDetailID = 0;
            try
            {                
                using (SqlConnection con = new SqlConnection(_strConnection))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", "SAVEDETAIL");
                    cmd.Parameters.AddWithValue("@ProjectID", objDetails.ProjectID);
                    cmd.Parameters.AddWithValue("@ProjectInvoiceID", objDetails.ProjectInvoiceID);
                    cmd.Parameters.AddWithValue("@PaymentID", header.ProjectPaymentID);
                    cmd.Parameters.AddWithValue("@Amount", header.Amount);
                    cmd.Parameters.AddWithValue("@PaymentType", header.PaymentType);
                    cmd.Parameters.AddWithValue("@CurrencID", header.CurrencyID);
                    cmd.Parameters.AddWithValue("@ExRate", header.ExRate);
                    cmd.Parameters.AddWithValue("@CreditAmount", header.CreditAmount);
                    cmd.Parameters.AddWithValue("@Comment", header.Description);
                    cmd.Parameters.AddWithValue("@userId", objDetails.InsertedBy);
                    cmd.Parameters.AddWithValue("@ProjectDetailID", objDetails.ProjectDetailID);
                    cmd.Parameters.AddWithValue("@Description", objDetails.Description);
                    cmd.Parameters.AddWithValue("@PaidAmount", objDetails.payAmount);
                    cmd.Parameters.AddWithValue("@BalanceAmount", objDetails.InvBalance);
                    cmd.Parameters.AddWithValue("@isEdited", objDetails.isEdited);
                    cmd.Parameters.AddWithValue("@PType", "");
                    cmd.Parameters.AddWithValue("@FromDate", "");
                    cmd.Parameters.AddWithValue("@ToDate", "");
                    cmd.Parameters.AddWithValue("@PaymentDate", header.InvoiceDate);
                    cmd.Parameters.AddWithValue("@InvoiceIDS", objDetails.IDs);
                    cmd.Parameters.AddWithValue("@isCredit", header.isCredited);
                    cmd.Parameters.AddWithValue("@AppliedCreditAmount", objDetails.AppliedCreditAmount);
                    cmd.Parameters.AddWithValue("@counter", objDetails.counter);
                    cmd.Parameters.AddWithValue("@CreditedPaymentID", objDetails.CreditedPaymentID);
                    cmd.Parameters.AddWithValue("@bFlag", objDetails.bFlag);
                    cmd.Parameters.AddWithValue("@ProjectPaymentID", objDetails.PaymentID);
                    cmd.Parameters.AddWithValue("@TaxCollected", header.TaxCollected);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    dtDetailID = Convert.ToInt32(cmd.ExecuteScalar());
                }
               
            }
            catch(SqlException ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record can not be inserted')", true);
            }
            return dtDetailID;
        }


        public List<KeyValueModel> GetCurrency()
        {

            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GETCURRENCY");
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                List<KeyValueModel> list = new List<KeyValueModel>();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new KeyValueModel()
                    {
                        Key = reader["currSymbol"].ToString(),
                        Value = Convert.ToInt32(reader["currID"].ToString())
                    });

                }
                return list;
            }

        }


        public int SavePaymentType(ProjecInvoiceBLL objheader)
        {
            int ID = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(_strConnection))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", "SAVEPAYMENTTYPE");
                    cmd.Parameters.AddWithValue("@ProjectID",0);
                    cmd.Parameters.AddWithValue("@ProjectInvoiceID", 0);
                    cmd.Parameters.AddWithValue("@PaymentID", 0);
                    cmd.Parameters.AddWithValue("@Amount", 0);
                    cmd.Parameters.AddWithValue("@PaymentType", 0);
                    cmd.Parameters.AddWithValue("@CurrencID", 0);
                    cmd.Parameters.AddWithValue("@ExRate", 0);
                    cmd.Parameters.AddWithValue("@CreditAmount", 0);
                    cmd.Parameters.AddWithValue("@Comment", "");
                    cmd.Parameters.AddWithValue("@userId", objheader.InsertedBy);
                    cmd.Parameters.AddWithValue("@ProjectDetailID", 0);
                    cmd.Parameters.AddWithValue("@Description", objheader.Description);
                    cmd.Parameters.AddWithValue("@PaidAmount", 0);
                    cmd.Parameters.AddWithValue("@BalanceAmount", 0);
                    cmd.Parameters.AddWithValue("@isEdited", objheader.isEdited);
                    cmd.Parameters.AddWithValue("@PType", objheader.PTypes);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    ID = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            catch (SqlException ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record can not be inserted')", true);
            }
            return ID;
        }

        public List<ProjecInvoiceBLL> GetMailInfo(int PaymentID)
        {
            List<ProjecInvoiceBLL> lstBLL = new List<ProjecInvoiceBLL>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
            cmd.Parameters.AddWithValue("@Mode", "GETMAILADD");
            cmd.Parameters.AddWithValue("@ProjectID", 0);
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", 0);
            cmd.Parameters.AddWithValue("@PaymentID", PaymentID);            
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            using (con)
            {
                con.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    string strEmail = getMailBody(ds);
                    ProjecInvoiceBLL obj = new ProjecInvoiceBLL();
                    obj.customerName = ds.Tables[0].Rows[0]["CustName"].ToString();
                    obj.customerAddress = ds.Tables[0].Rows[0]["CustEmail"].ToString();
                    //obj.ProjectName = ds.Tables[0].Rows[0]["ProjName"].ToString();
                    obj.Description = strEmail;
                    lstBLL.Add(obj);
                }

            }
            return lstBLL;
        }

        private string getMailBody(DataSet ds)
        {
            string strEmail = string.Empty;
            strEmail = ds.Tables[1].Rows[0]["Value"].ToString() ;
            strEmail = strEmail.Replace("{CustomerName}", ds.Tables[0].Rows[0]["CustName"].ToString()+",\n\n");
            strEmail = strEmail.Replace("{LocationCompany}", ds.Tables[0].Rows[0]["custaddress"].ToString());
            strEmail = strEmail.Replace("{PaymentDate}.", ds.Tables[0].Rows[0]["PAYMENTDATE"].ToString()+".\n\n");
            strEmail = strEmail.Replace("{InvoiceNumbers}", ds.Tables[0].Rows[0]["InvoiceNo"].ToString());
            strEmail = strEmail.Replace("{Amount}", ds.Tables[0].Rows[0]["AMOUNT"].ToString());
            //strEmail = strEmail.Replace("<p>", ""); 
            //strEmail = strEmail.Replace("</p>","");
            //strEmail = strEmail.Replace("Regards", "\n\n Regards, \n\n");
            //strEmail = strEmail.Replace("Team", "Team \n\n");
            //strEmail = strEmail.Replace("<br />", "");


            return strEmail;
        }

        public void UpdateMailSentDate(int PaymentID)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
            cmd.Parameters.AddWithValue("@Mode", "MAILSENT");
            cmd.Parameters.AddWithValue("@ProjectID", 0);
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", 0);
            cmd.Parameters.AddWithValue("@PaymentID", PaymentID);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void DeletePayment(int PaymentID)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectPayment", con);
            cmd.Parameters.AddWithValue("@Mode", "DELETE_PAYMENT");
            cmd.Parameters.AddWithValue("@PaymentID", PaymentID);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}