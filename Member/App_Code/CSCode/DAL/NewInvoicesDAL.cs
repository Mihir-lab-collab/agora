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
using System.Configuration;


/// <summary>
/// Summary description for InvoicesDAL
/// </summary>
///
namespace Customer.DAL
{
    public class NewInvoicesDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        public NewInvoicesDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetInvoiceData(NewInvoicesBLL objProjInvoice)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            //SqlCommand cmd = new SqlCommand("SP_InvoiceGetProjectDetail", con); 
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GETINVOICEDATA");
            cmd.Parameters.AddWithValue("@ProjID", Convert.ToInt32(objProjInvoice.projId));
            using (con)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public int insertInvoice(NewInvoiceModel objInvoice)
        {
            int outputid = 0;
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                //SqlCommand cmd = new SqlCommand("SP_InvoiceGridInsert", con);
                SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "SAVEINVOICEDETAILS");
                cmd.Parameters.AddWithValue("@ProjectInvoiceID", objInvoice.ProjectInvoiceID);
                if (objInvoice.ProjectMilestoneID == 0)
                {
                    cmd.Parameters.AddWithValue("@ProjectMilestoneID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProjectMilestoneID", objInvoice.ProjectMilestoneID);
                }
                cmd.Parameters.AddWithValue("@Description", objInvoice.Description);
                cmd.Parameters.AddWithValue("@Quantity", objInvoice.Quantity);
                cmd.Parameters.AddWithValue("@Rate", objInvoice.Rate);
                cmd.Parameters.AddWithValue("@Amount", objInvoice.Amount);
                cmd.Parameters.AddWithValue("@BalanceAmount", objInvoice.BalanceAmount);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
            return outputid;

        }

        public int InsertHeaderInv(NewInvoice objInvoiceheader)
        {
            int dtProjInvID = 0;
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                //SqlCommand cmd = new SqlCommand("SP_InvoiceInsert", con);  
                SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "SAVEINVOICE");
                cmd.Parameters.AddWithValue("@ProjID", objInvoiceheader.projId);
                cmd.Parameters.AddWithValue("@InvoiceDate", dateFormatYMD(objInvoiceheader.InvoiceDate));//DateTime.ParseExact(objInvoiceheader.InvoiceDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@DueDate", dateFormatYMD(objInvoiceheader.DueDate));//DateTime.ParseExact(objInvoiceheader.DueDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture
                cmd.Parameters.AddWithValue("@ExRate", objInvoiceheader.ExRate);
                cmd.Parameters.AddWithValue("@CurrencyID", objInvoiceheader.currID);
                cmd.Parameters.AddWithValue("@Tax1", objInvoiceheader.Tax1);
                cmd.Parameters.AddWithValue("@Tax2", objInvoiceheader.Tax2);
                cmd.Parameters.AddWithValue("@Tax3", objInvoiceheader.Tax3);
                cmd.Parameters.AddWithValue("@TransCharge", objInvoiceheader.TransCharge);
                cmd.Parameters.AddWithValue("@VATCharge", objInvoiceheader.VATCharge);
                cmd.Parameters.AddWithValue("@CSTCharge", objInvoiceheader.CSTCharge);
                cmd.Parameters.AddWithValue("@TotalAmt", objInvoiceheader.TotalAmount);
                cmd.Parameters.AddWithValue("@Comment", objInvoiceheader.Comment);
                cmd.Parameters.AddWithValue("@userId", objInvoiceheader.insertedBy);
                cmd.Parameters.AddWithValue("@InvoiceNo", objInvoiceheader.InvoiceNo);
                cmd.Parameters.AddWithValue("@LocationID", objInvoiceheader.Inv_LocationID);
                cmd.Parameters.AddWithValue("@CGST", objInvoiceheader.CGST);
                cmd.Parameters.AddWithValue("@SGST", objInvoiceheader.SGST);
                cmd.Parameters.AddWithValue("@IGST", objInvoiceheader.IGST);
                cmd.Parameters.AddWithValue("@GST", objInvoiceheader.GST);
                cmd.Parameters.AddWithValue("@CodeId", objInvoiceheader.CodeId);
                cmd.Parameters.AddWithValue("@TDSCheck", objInvoiceheader.TDSCheck);
                //cmd.Parameters.AddWithValue("@GST",Convert.ToDecimal("0.00"));//Added by Heramb Sharma on 5-04-2019
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                dtProjInvID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return dtProjInvID;
        }

        public int UpdateInvoiceHeader(NewInvoice objIHeader)
        {
            int dtProjInvID = 0;
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                //SqlCommand cmd = new SqlCommand("SP_InvoiceUpdate", con); 
                SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "UPDATEINVOICE");
                cmd.Parameters.AddWithValue("@ProjID", objIHeader.projId);
                cmd.Parameters.AddWithValue("@ProjectInvoiceID", objIHeader.ProjectInvoiceID);
                cmd.Parameters.AddWithValue("@InvoiceDate", dateFormatYMD(objIHeader.InvoiceDate));//DateTime.ParseExact(objIHeader.InvoiceDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@DueDate", dateFormatYMD(objIHeader.DueDate));//DateTime.ParseExact(objIHeader.DueDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@ExRate", objIHeader.ExRate);
                cmd.Parameters.AddWithValue("@CurrencyID", objIHeader.currID);
                cmd.Parameters.AddWithValue("@Tax1", objIHeader.Tax1);
                cmd.Parameters.AddWithValue("@Tax2", objIHeader.Tax2);
                cmd.Parameters.AddWithValue("@Tax3", objIHeader.Tax3);
                cmd.Parameters.AddWithValue("@CGST", objIHeader.CGST);
                cmd.Parameters.AddWithValue("@SGST", objIHeader.SGST);
                cmd.Parameters.AddWithValue("@IGST", objIHeader.IGST);

                cmd.Parameters.AddWithValue("@GST", objIHeader.GST);
                cmd.Parameters.AddWithValue("@TransCharge", objIHeader.TransCharge);
                cmd.Parameters.AddWithValue("@VATCharge", objIHeader.VATCharge);
                cmd.Parameters.AddWithValue("@CSTCharge", objIHeader.CSTCharge);
                cmd.Parameters.AddWithValue("@TotalAmount", objIHeader.TotalAmount);
                cmd.Parameters.AddWithValue("@Comment", objIHeader.Comment);
                cmd.Parameters.AddWithValue("@ModifiedBy", objIHeader.modifiedBy);
                cmd.Parameters.AddWithValue("@InvoiceNo", objIHeader.InvoiceNo);
                cmd.Parameters.AddWithValue("@CodeId", objIHeader.CodeId);
                cmd.Parameters.AddWithValue("@TDSCheck",objIHeader.TDSCheck);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
                dtProjInvID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return dtProjInvID;
        }

        public int UpdateInvoiceDetails(NewInvoiceModel objIDetails)
        {
            int outputid = 0;
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                //SqlCommand cmd = new SqlCommand("SP_InvoiceDetailUpdate", con);
                SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "UPDATEINVOICEDETAIL");
                cmd.Parameters.AddWithValue("@ProjectInvoiceDetailID", objIDetails.ProjectInvoiceDetailID);
                cmd.Parameters.AddWithValue("@ProjectInvoiceID", objIDetails.ProjectInvoiceID);
                if (objIDetails.ProjectMilestoneID == 0)
                {
                    cmd.Parameters.AddWithValue("@ProjectMilestoneID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProjectMilestoneID", objIDetails.ProjectMilestoneID);
                }
                cmd.Parameters.AddWithValue("@Description", objIDetails.Description);
                cmd.Parameters.AddWithValue("@Quantity", objIDetails.Quantity);
                cmd.Parameters.AddWithValue("@Rate", objIDetails.Rate);
                cmd.Parameters.AddWithValue("@Amount", objIDetails.Amount);
                cmd.Parameters.AddWithValue("@BalanceAmount", objIDetails.BalanceAmount);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
            return outputid;
        }

        public void DeleteInvoiceMile(int ProjectInvoiceDetailID)
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                //SqlCommand cmd = new SqlCommand("sp_InvoiceMileStoneDelete", con);
                SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "INVOICE_MILESTONE_DELETE");
                cmd.Parameters.AddWithValue("@ProjectInvoiceDetailID", ProjectInvoiceDetailID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
        }

        public string getLocationKeyword(string locid)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("Select isnull(Keyword,'') Keyword from Location where LocationID=" + locid, con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = null;
            string locationkeyword = "";
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    locationkeyword = Convert.ToString(reader["Keyword"]);
                }
                return locationkeyword;
            }
        }

        public bool IfExistsInvoiceNo(string InvoiceNo, int ProjectInvoiceID, int ProjID, int LocationID)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "CHECK_INVOICENO");
            cmd.Parameters.AddWithValue("@ProjID", ProjID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", ProjectInvoiceID);
            cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            using (con)
            {
                con.Open();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public List<NewSavedInvoice> getProjInvoice(int projId)
        {
            List<NewSavedInvoice> curInvoices = new List<NewSavedInvoice>();
            SqlConnection con = new SqlConnection(_strConnection);
            //SqlCommand cmd = new SqlCommand("SP_InvoicesForProject", con); //USP_InvoicesForProject
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "INVOICESFORPROJECT");
            cmd.Parameters.AddWithValue("@ProjID", projId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NewSavedInvoice obj = new NewSavedInvoice();
                        obj.custId = Convert.ToInt32(reader["custId"]);
                        obj.ProjectInvoiceID = Convert.ToInt32(reader["ProjectInvoiceID"]);
                        obj.ProjID = Convert.ToInt32(reader["ProjID"]);
                        obj.projName = Convert.ToString(reader["projName"]);
                        obj.custName = Convert.ToString(reader["custName"]);
                        obj.InvoiceDate = Convert.ToString(reader["InvoiceDate"]);
                        obj.Amount = Convert.ToString(reader["Amount"]);
                        curInvoices.Add(obj);
                    }
                }
            }
            return curInvoices;
        }

        public List<NewInvoice> GetProjectInvoices(int projId, int locid, string status)
        {
            CultureInfo cuInfo = new CultureInfo("en-IN");
            List<NewInvoice> savedInvoice = new List<NewInvoice>();
            SqlConnection con = new SqlConnection(_strConnection);
            //SqlCommand cmd = new SqlCommand("SP_SavedInvoiceGrid", con); 
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GETINVOICE");
            cmd.Parameters.AddWithValue("@ProjID", projId);
            cmd.Parameters.AddWithValue("@LocationID", locid);
            cmd.Parameters.AddWithValue("@Status", status);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // ISNULL(PPD.PaidAmount,0) As PaidAmount,

                        NewInvoice obj = new NewInvoice();
                        obj.custId = Convert.ToInt32(reader["custId"]);
                        obj.TDSCheck = Convert.ToInt32(reader["TDSCheck"]);
                        obj.Reminder_text = reader["SentOn"].ToString();
                        obj.ProjectInvoiceID = Convert.ToInt32(reader["ProjectInvoiceID"]);
                        obj.ProjID = Convert.ToInt32(reader["ProjID"]);
                        obj.projName = Convert.ToString(reader["projName"]);
                        obj.CurrencyID = Convert.ToInt32(reader["CurrencyID"]);
                        obj.currSymbol = Convert.ToString(reader["currSymbol"]);
                        obj.custName = Convert.ToString(reader["custName"]);
                        obj.ExRate = Convert.ToInt32(reader["ExRate"]);
                        obj.InvoiceNo = Convert.ToString(reader["InvoiceNo"]);
                        obj.InvoiceDate = reader["InvoiceDate"].ToString() == "" ? "" : Convert.ToDateTime(reader["InvoiceDate"]).ToString();
                        obj.TotalAmount = Convert.ToInt32(reader["TotalAmount"]);
                        obj.Amount = Convert.ToString(reader["Amount"]);
                        obj.Inv_CurBalanceAmount = Convert.ToString(reader["FormtdBalAmt"]);
                        obj.Inv_BalanceAmount = Convert.ToDecimal(reader["BalanceAmount"]);
                        obj.Inv_Delay = Convert.ToString(reader["Inv_Delay"]);
                        obj.DueDate = reader["DueDate"].ToString();//== "" ? "" : Convert.ToDateTime(reader["DueDate"]).ToString();// 
                        obj.Inv_LocationID = Convert.ToInt32(reader["Inv_LocationID"]);
                        obj.Inv_LocationName = Convert.ToString(reader["Inv_LocationName"]);
                        obj.Status = Convert.ToString(reader["status"]);
                        obj.IsVoid = Convert.ToString(reader["IsVoid"]);
                        obj.EmailSentDate = Convert.ToString(reader["EmailDate"]);

                        obj.Amount = obj.currSymbol + (Convert.ToDecimal(obj.Amount)).ToString("C", cuInfo).Remove(0, 2).Trim();
                        obj.Inv_CurBalanceAmount = obj.currSymbol + ((Convert.ToDecimal(obj.Inv_CurBalanceAmount)).ToString("C", cuInfo)).Remove(0, 2).Trim();

                        savedInvoice.Add(obj);
                    }
                }

            }
            return savedInvoice;
        }

        public string dateFormatYMD(string sDate)
        {
            DateTime fDate = DateTime.ParseExact(sDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            sDate = fDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            return sDate;
        }

        public List<NewInvoice> GetInvoicesStatus(int pInvId)
        {
            CultureInfo cuInfo = new CultureInfo("en-IN");
            List<NewInvoice> GetStatus = new List<NewInvoice>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "INVOICE_STATUS");
            cmd.Parameters.AddWithValue("@ProjID", 0);
            cmd.Parameters.AddWithValue("@LocationID", 0);
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", pInvId);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NewInvoice obj = new NewInvoice();
                        obj.custName = Convert.ToString(reader["status"]);
                        obj.DueDate = reader["paymentdate"].ToString();
                        obj.Amount = reader["paidAmount"].ToString();

                        GetStatus.Add(obj);
                    }
                }

            }
            return GetStatus;
        }

        public List<NewInvoice> GetMailInfo(int pInvId, string Inv_Delay)
        {
            string TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\INVOICE\";
            CultureInfo cuInfo = new CultureInfo("en-IN");
            List<NewInvoice> lstInfo = new List<NewInvoice>();
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "ProjectPaymentDue");
            //cmd.Parameters.AddWithValue("@ProjID", 0);
            //cmd.Parameters.AddWithValue("@LocationID", 0);
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", pInvId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            using (con)
            {
                con.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    string strInvoicePath = TargetDir;//HttpContext.Current.Server.MapPath(@"\Admin\Invoice\");
                    string strFileName = strInvoicePath + ds.Tables[0].Rows[0]["InvoiceNo"].ToString() + ".pdf";
                    string DisplayFileName = ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Replace('/', '-') + ".pdf";

                    string strEmail = getMailBody(ds, Inv_Delay);
                    NewInvoice obj = new NewInvoice();
                    obj.custName = ds.Tables[0].Rows[0]["CustName"].ToString();
                    obj.projName = ds.Tables[0].Rows[0]["projName"].ToString();
                    obj.custAddress = ds.Tables[0].Rows[0]["CustEmail"].ToString();
                    obj.custCompany = ds.Tables[0].Rows[0]["EmailCC"].ToString();
                    obj.InvoiceNo = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();

                    obj.Comment = strEmail;
                    obj.Amount = strFileName;
                    obj.DueDate = DisplayFileName;
                    lstInfo.Add(obj);
                }

            }
            return lstInfo;
        }

        private string FormatDescription(string mileDesc)
        {
            string rDesc = "Description : ";
            string[] mDesc = mileDesc.Split(',');
            for (int i = 0; i < mDesc.Length; i++)
                rDesc = rDesc + "<p>" + (i + 1) + ". " + mDesc[i].ToString() + "</p>";

            return rDesc.ToString();
        }

        private string getMailBody(DataSet ds, string Inv_Delay)
        {
            string strEmailBody = string.Empty;

            strEmailBody = ds.Tables[1].Rows[0]["Value"].ToString();
            strEmailBody = strEmailBody.Replace("{CustomerName}", ds.Tables[0].Rows[0]["CustName"].ToString() + ",\n");
            strEmailBody = strEmailBody.Replace("{ProjectName}", ds.Tables[0].Rows[0]["projName"].ToString());
            strEmailBody = strEmailBody.Replace("{InvoiceNo}", ds.Tables[0].Rows[0]["InvoiceNo"].ToString());
            strEmailBody = strEmailBody.Replace("{InvoiceDate}", ds.Tables[0].Rows[0]["InvoiceDate"].ToString());
            strEmailBody = strEmailBody.Replace("{InvoiceAmount}", ds.Tables[0].Rows[0]["TotalAmount"].ToString());
            strEmailBody = strEmailBody.Replace("{currSymbol}", ds.Tables[0].Rows[0]["currSymbol"].ToString());
            strEmailBody = strEmailBody.Replace("{Description}", FormatDescription(ds.Tables[0].Rows[0]["Description"].ToString()));
            if (!string.IsNullOrEmpty(Inv_Delay))
            {
                strEmailBody = strEmailBody.Replace("{DelayDays}", Inv_Delay);
            }

            //strEmailBody = strEmailBody.Replace("&nbsp;", "");
            //strEmailBody = strEmailBody.Replace("<strong>", " ");
            //strEmailBody = strEmailBody.Replace("</strong>", " ");
            //strEmailBody = strEmailBody.Replace("<p>", "\n");
            //strEmailBody = strEmailBody.Replace("</p>", "\n");
            //strEmailBody = strEmailBody.Replace("@","\n");
            return strEmailBody;
        }

        public int VoidInvoice(string pInvId)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "VOIDINVOICE");
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", pInvId);

            using (con)
            {
                con.Open();

                return cmd.ExecuteNonQuery();
            }
        }

        public void UpdateMailSentDate(int pInvId)
        {

            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.Parameters.AddWithValue("@Mode", "MAILSENT");
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", pInvId);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public DataSet GetPDFContent(int pInvId, int LocationID)
        {
            string sPDFContent = "";
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.Parameters.AddWithValue("@Mode", "INVOICEPDF");
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", pInvId);
            cmd.Parameters.AddWithValue("@LocationID", Convert.ToInt32(LocationID));
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            //if (ds.Tables.Count > 0)
            //{
            //    sPDFContent = FormatHTML(ds);
            //}
            con.Close();

            return ds;
        }

        private string FormatHTML(DataSet ds)
        {
            string sContent = ds.Tables[1].Rows[0]["Value"].ToString();

            sContent = sContent.Replace("{CustCompany}", ds.Tables[0].Rows[0]["custCompany"].ToString());
            sContent = sContent.Replace("{InvoiceNO}", ds.Tables[0].Rows[0]["InvoiceNO"].ToString());
            sContent = sContent.Replace("{CustName}", ds.Tables[0].Rows[0]["custName"].ToString());
            sContent = sContent.Replace("{CustAddress}", ds.Tables[0].Rows[0]["custAddress"].ToString());
            sContent = sContent.Replace("{InvoiceDate}", ds.Tables[0].Rows[0]["InvoiceDate"].ToString());
            sContent = sContent.Replace("{CurrencySymbol}", ds.Tables[0].Rows[0]["currSymbol"].ToString());
            sContent = sContent.Replace("{TotalCost}", ds.Tables[0].Rows[0]["TotalAmount"].ToString());
            sContent = sContent.Replace("{AmountPayable}", ds.Tables[0].Rows[0]["PaidAmount"].ToString());
            sContent = sContent.Replace("{Description}", ds.Tables[0].Rows[0]["Description"].ToString());
            // sContent = sContent.Replace("{CostInWord}", ds.Tables[0].Rows[0]["CostInWord"].ToString());  // Numberic To Words

            return sContent.ToString();
        }


        public string Get_InvoiceReminderDetails(int InvoiceID)
        {
            string result = string.Empty;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GET_ReminderDtls");
            cmd.Parameters.AddWithValue("@ProjectInvoiceID", InvoiceID);
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //result = Convert.ToDateTime(reader["SentOn"]).ToString("dd-MMM-yyyy hh:mm tt")+ "," + result;
                        result = reader["SentOn"].ToString();
                    }
                    //result.TrimEnd(',');
                }

            }
            return result;
        }
    }
}





