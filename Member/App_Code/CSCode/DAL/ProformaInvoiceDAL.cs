using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for ProformaInvoiceDAL
/// </summary>
public class ProformaInvoiceDAL
{

    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public ProformaInvoiceDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<ProformaInvoiceBLL> GetProformaInvoices(int projId, int locid, string status)
    {
        CultureInfo cuInfo = new CultureInfo("en-IN");
        List<ProformaInvoiceBLL> savedInvoice = new List<ProformaInvoiceBLL>();
        SqlConnection con = new SqlConnection(_strConnection);

        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GetInvoice");
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
                    ProformaInvoiceBLL obj = new ProformaInvoiceBLL();
                    obj.custId = Convert.ToInt32(reader["custId"]);
                    obj.ProformaInvoiceID = Convert.ToInt32(reader["ProformaInvoiceID"]);
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
                    // obj.Inv_CurBalanceAmount = Convert.ToString(reader["FormtdBalAmt"]);
                    // obj.Inv_BalanceAmount = Convert.ToDecimal(reader["BalanceAmount"]);
                    obj.Inv_Delay = Convert.ToString(reader["Inv_Delay"]);
                    obj.DueDate = reader["DueDate"].ToString();//== "" ? "" : Convert.ToDateTime(reader["DueDate"]).ToString();// 
                    obj.Inv_LocationID = Convert.ToInt32(reader["Inv_LocationID"]);
                    obj.Inv_LocationName = Convert.ToString(reader["Inv_LocationName"]);
                    obj.Status = Convert.ToString(reader["Status"]);
                    obj.IsVoid = Convert.ToString(reader["IsVoid"]);
                    obj.EmailSentDate = Convert.ToString(reader["EmailDate"]);

                    obj.Amount = obj.currSymbol + (Convert.ToDecimal(obj.Amount)).ToString("C", cuInfo).Remove(0, 2).Trim();
                    obj.Inv_CurBalanceAmount = obj.currSymbol + ((Convert.ToDecimal(obj.Inv_CurBalanceAmount)).ToString("C", cuInfo)).Remove(0, 2).Trim();
                    obj.TaxInvoice = Convert.ToString(reader["TaxInvoice"]);
                    savedInvoice.Add(obj);
                }
            }

        }
        return savedInvoice;
    }

    public int InsertProInvoiceHeader(ProformaInvoiceBLL objProInvoiceHeader)
    {
        int ProformaInvoiceID = 0;
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "SaveInvoice");
            cmd.Parameters.AddWithValue("@ProjID", objProInvoiceHeader.projId);
            cmd.Parameters.AddWithValue("@InvoiceDate", dateFormatYMD(objProInvoiceHeader.InvoiceDate));
            cmd.Parameters.AddWithValue("@CurrencyID", objProInvoiceHeader.currID);
            cmd.Parameters.AddWithValue("@ExRate", objProInvoiceHeader.ExRate);
            cmd.Parameters.AddWithValue("@Tax1", objProInvoiceHeader.Tax1);
            cmd.Parameters.AddWithValue("@Tax2", objProInvoiceHeader.Tax2);
            cmd.Parameters.AddWithValue("@Tax3", objProInvoiceHeader.Tax3);
            cmd.Parameters.AddWithValue("@TransCharge", objProInvoiceHeader.TransCharge);
            cmd.Parameters.AddWithValue("@VATCharge", objProInvoiceHeader.VATCharge);
            cmd.Parameters.AddWithValue("@CSTCharge", objProInvoiceHeader.CSTCharge);
            cmd.Parameters.AddWithValue("@TotalAmount", objProInvoiceHeader.TotalAmount);
            cmd.Parameters.AddWithValue("@Comment", objProInvoiceHeader.Comment);
            cmd.Parameters.AddWithValue("@InvoiceNo", objProInvoiceHeader.InvoiceNo);
            cmd.Parameters.AddWithValue("@LocationID", objProInvoiceHeader.Inv_LocationID);
            cmd.Parameters.AddWithValue("@DueDate", dateFormatYMD(objProInvoiceHeader.DueDate));
            cmd.Parameters.AddWithValue("@userId", objProInvoiceHeader.insertedBy);
            cmd.Parameters.AddWithValue("@CGST", objProInvoiceHeader.CGST);
            cmd.Parameters.AddWithValue("@SGST", objProInvoiceHeader.SGST);
            cmd.Parameters.AddWithValue("@IGST", objProInvoiceHeader.IGST);
            cmd.Parameters.AddWithValue("@GST", objProInvoiceHeader.GST);
            cmd.Parameters.AddWithValue("@CodeId", objProInvoiceHeader.CodeId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            ProformaInvoiceID = Convert.ToInt32(cmd.ExecuteScalar());
        }
        return ProformaInvoiceID;
    }

    public int InsertProInvoiceDetails(ProInvoiceModel objProInvoice)
    {
        int output = 0;
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "SaveInvoiceDetails");
            cmd.Parameters.AddWithValue("@ProformaInvoiceID", objProInvoice.ProformaInvoiceID);
            if (objProInvoice.ProjectMilestoneID == 0)
            {
                cmd.Parameters.AddWithValue("@ProjectMilestoneID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ProjectMilestoneID", objProInvoice.ProjectMilestoneID);
            }
            cmd.Parameters.AddWithValue("@Description", objProInvoice.Description);
            cmd.Parameters.AddWithValue("@Quantity", objProInvoice.Quantity);
            cmd.Parameters.AddWithValue("@Rate", objProInvoice.Rate);
            cmd.Parameters.AddWithValue("@Amount", objProInvoice.Amount);
            cmd.Parameters.AddWithValue("@BalanceAmount", objProInvoice.BalanceAmount);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);
        }
        return output;
    }

    public int InsertHeaderTaxInv(ProformaInvoiceBLL objInvoiceheader)
    {
        int dtProjInvID = 0;
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "SaveTaxInvoice");
            cmd.Parameters.AddWithValue("@ProjID", objInvoiceheader.projId);
            cmd.Parameters.AddWithValue("@InvoiceDate", dateFormatYMD(objInvoiceheader.InvoiceDate));
            cmd.Parameters.AddWithValue("@DueDate", dateFormatYMD(objInvoiceheader.DueDate));
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
            cmd.Parameters.AddWithValue("@Status", objInvoiceheader.Status);
            cmd.Parameters.AddWithValue("@ProformaInvoiceID", objInvoiceheader.ProformaInvoiceID); //AP
            cmd.Parameters.AddWithValue("@CGST", objInvoiceheader.CGST);
            cmd.Parameters.AddWithValue("@SGST", objInvoiceheader.SGST);
            cmd.Parameters.AddWithValue("@IGST", objInvoiceheader.IGST);
            cmd.Parameters.AddWithValue("@GST", objInvoiceheader.GST);
            cmd.Parameters.AddWithValue("@CodeID", objInvoiceheader.CodeId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            dtProjInvID = Convert.ToInt32(cmd.ExecuteScalar());
        }
        return dtProjInvID;
    }

    public int insertTaxInvoice(ProInvoiceModel objInvoice)
    {
        int outputid = 0;
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();
            //SqlCommand cmd = new SqlCommand("SP_InvoiceGridInsert", con);
            SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "SaveTaxInvoiceDetails");
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

    public int UpdateInvoiceHeader(ProformaInvoiceBLL objIHeader)
    {
        int dtProjInvID = 0;
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "UpdateInvoice");
            cmd.Parameters.AddWithValue("@ProjID", objIHeader.projId);
            cmd.Parameters.AddWithValue("@ProformaInvoiceID", objIHeader.ProformaInvoiceID);
            cmd.Parameters.AddWithValue("@InvoiceDate", dateFormatYMD(objIHeader.InvoiceDate));
            cmd.Parameters.AddWithValue("@DueDate", dateFormatYMD(objIHeader.DueDate));
            cmd.Parameters.AddWithValue("@ExRate", objIHeader.ExRate);
            cmd.Parameters.AddWithValue("@CurrencyID", objIHeader.currID);
            cmd.Parameters.AddWithValue("@Tax1", objIHeader.Tax1);
            cmd.Parameters.AddWithValue("@Tax2", objIHeader.Tax2);
            cmd.Parameters.AddWithValue("@Tax3", objIHeader.Tax3);
            cmd.Parameters.AddWithValue("@TransCharge", objIHeader.TransCharge);
            cmd.Parameters.AddWithValue("@VATCharge", objIHeader.VATCharge);
            cmd.Parameters.AddWithValue("@CSTCharge", objIHeader.CSTCharge);
            cmd.Parameters.AddWithValue("@TotalAmount", objIHeader.TotalAmount);
            cmd.Parameters.AddWithValue("@Comment", objIHeader.Comment);
            cmd.Parameters.AddWithValue("@ModifiedBy", objIHeader.modifiedBy);
            cmd.Parameters.AddWithValue("@InvoiceNo", objIHeader.InvoiceNo);
            cmd.Parameters.AddWithValue("@CGST", objIHeader.CGST);
            cmd.Parameters.AddWithValue("@SGST", objIHeader.SGST);
            cmd.Parameters.AddWithValue("@IGST", objIHeader.IGST);
            cmd.Parameters.AddWithValue("@GST", objIHeader.GST);
            cmd.Parameters.AddWithValue("@CodeId", objIHeader.CodeId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);
            dtProjInvID = Convert.ToInt32(cmd.ExecuteScalar());
        }
        return dtProjInvID;
    }

    public int UpdateInvoiceDetails(ProInvoiceModel objIDetails)
    {
        int outputid = 0;
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "UpdateInvoiceDetail");
            cmd.Parameters.AddWithValue("@ProformaInvoiceDetailsID", objIDetails.ProformaInvoiceDetailID);
            cmd.Parameters.AddWithValue("@ProformaInvoiceID", objIDetails.ProformaInvoiceID);
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

    public string dateFormatYMD(string sDate)
    {
        DateTime fDate = DateTime.ParseExact(sDate, "d/M/yyyy", CultureInfo.InvariantCulture);
        sDate = fDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        return sDate;
    }

    public bool IfExistsInvoiceNo(string InvoiceNo, int ProformaInvoiceID, int ProjID, int LocationID)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", "Check_InvoiceNo");
        cmd.Parameters.AddWithValue("@ProjID", ProjID);
        cmd.Parameters.AddWithValue("@LocationID", LocationID);
        cmd.Parameters.AddWithValue("@ProformaInvoiceID", ProformaInvoiceID);
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

    public void DeleteInvoiceMile(int ProformaInvoiceDetailID)
    {
        using (SqlConnection con = new SqlConnection(_strConnection))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "InvoiceMilestoneDelete");
            cmd.Parameters.AddWithValue("@ProformaInvoiceDetailsID", ProformaInvoiceDetailID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);
        }
    }

    public List<ProformaInvoiceBLL> GetInvoicesStatus(int pInvId)
    {
        CultureInfo cuInfo = new CultureInfo("en-IN");
        List<ProformaInvoiceBLL> GetStatus = new List<ProformaInvoiceBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "InvoiceStatus");
        cmd.Parameters.AddWithValue("@ProjID", 0);
        cmd.Parameters.AddWithValue("@LocationID", 0);
        cmd.Parameters.AddWithValue("@ProformaInvoiceID", pInvId);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProformaInvoiceBLL obj = new ProformaInvoiceBLL();
                    obj.custName = Convert.ToString(reader["status"]);
                    obj.DueDate = reader["paymentdate"].ToString();
                    obj.Amount = reader["paidAmount"].ToString();

                    GetStatus.Add(obj);
                }
            }

        }
        return GetStatus;
    }


    public int VoidInvoice(string pInvId)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "VOIDIPROFORMANVOICE");
        cmd.Parameters.AddWithValue("@ProformaInvoiceID", pInvId);

        using (con)
        {
            con.Open();

            return cmd.ExecuteNonQuery();
        }
    }

    // MAIL
    public List<ProformaInvoiceBLL> GetMailInfo(int pInvId)
    {
        string TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\INVOICE\";
        CultureInfo cuInfo = new CultureInfo("en-IN");
        List<ProformaInvoiceBLL> lstInfo = new List<ProformaInvoiceBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", "MailInfo");
        cmd.Parameters.AddWithValue("@ProformaInvoiceID", pInvId);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        using (con)
        {
            con.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                string strInvoicePath = TargetDir;//HttpContext.Current.Server.MapPath(@"\Admin\Invoice\");


                string strFileName = strInvoicePath + ds.Tables[0].Rows[0]["ProformaInvoiceID"].ToString() + ".pdf";
                string DisplayFileName = ds.Tables[0].Rows[0]["ProformaInvoiceID"].ToString().Replace('/', '-') + ".pdf";

                string strEmail = getMailBody(ds);
                ProformaInvoiceBLL obj = new ProformaInvoiceBLL();
                obj.custName = ds.Tables[0].Rows[0]["CustName"].ToString();
                obj.projName = ds.Tables[0].Rows[0]["projName"].ToString();
                obj.custAddress = ds.Tables[0].Rows[0]["CustEmail"].ToString();
                obj.custCompany = ds.Tables[0].Rows[0]["EmailCC"].ToString();
                //obj.InvoiceNo = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                obj.InvoiceNo = ds.Tables[0].Rows[0]["ProformaInvoiceID"].ToString();
                obj.Comment = strEmail;
                obj.Amount = strFileName;
                obj.DueDate = DisplayFileName;
                lstInfo.Add(obj);
            }

        }
        return lstInfo;
    }

    private string getMailBody(DataSet ds)
    {
        string strEmailBody = string.Empty;

        strEmailBody = ds.Tables[1].Rows[0]["Value"].ToString();
        strEmailBody = strEmailBody.Replace("{CustomerName}", ds.Tables[0].Rows[0]["CustName"].ToString() + ",\n");
        strEmailBody = strEmailBody.Replace("{ProjectName}", ds.Tables[0].Rows[0]["projName"].ToString());
        // strEmailBody = strEmailBody.Replace("{InvoiceNo}", ds.Tables[0].Rows[0]["InvoiceNo"].ToString());
        strEmailBody = strEmailBody.Replace("{InvoiceNo}", ds.Tables[0].Rows[0]["ProformaInvoiceID"].ToString());
        strEmailBody = strEmailBody.Replace("{InvoiceDate}", ds.Tables[0].Rows[0]["InvoiceDate"].ToString());
        strEmailBody = strEmailBody.Replace("{InvoiceAmount}", ds.Tables[0].Rows[0]["TotalAmount"].ToString());
        strEmailBody = strEmailBody.Replace("{currSymbol}", ds.Tables[0].Rows[0]["currSymbol"].ToString());
        strEmailBody = strEmailBody.Replace("{Description}", FormatDescription(ds.Tables[0].Rows[0]["Description"].ToString()));

        return strEmailBody;
    }

    private string FormatDescription(string mileDesc)
    {
        string rDesc = "Description : ";
        string[] mDesc = mileDesc.Split(',');
        for (int i = 0; i < mDesc.Length; i++)
            rDesc = rDesc + "<p>" + (i + 1) + ". " + mDesc[i].ToString() + "</p>";

        return rDesc.ToString();
    }

    public void UpdateMailSentDate(int pInvId)
    {

        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.Parameters.AddWithValue("@mode", "MailSent");
        cmd.Parameters.AddWithValue("@ProformaInvoiceID", pInvId);
        cmd.CommandType = CommandType.StoredProcedure;
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }


    //PDF

    public DataSet GetPDFContent(int pInvId, int LocationID)
    {
        string sPDFContent = "";
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.Parameters.AddWithValue("@mode", "InvoicePdf");
        cmd.Parameters.AddWithValue("@ProformaInvoiceID", pInvId);
        cmd.Parameters.AddWithValue("@LocationID", Convert.ToInt32(LocationID));
        cmd.CommandType = CommandType.StoredProcedure;
        con.Open();
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        con.Close();
        return ds;
    }

    // GST Code bind:

    public List<ProformaInvoiceBLL> BindCode(string mode)//, int? Id)
    {
        List<ProformaInvoiceBLL> Code = new List<ProformaInvoiceBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        // cmd.Parameters.AddWithValue("@CodeId", Id);
        SqlDataReader Dr = null;
        using (con)
        {
            con.Open();
            Dr = cmd.ExecuteReader();
            while (Dr.Read())
            {
                ProformaInvoiceBLL obj = new ProformaInvoiceBLL();
                obj.CodeId = Convert.ToInt32(Dr["Id"]);
                obj.Code = Convert.ToString(Dr["Code"]);
                Code.Add(obj);
            }
        }
        return Code;
    }

    public List<ProformaInvoiceBLL> GetLocationDetails(int projId, int LocationId, string mode)
    {
        List<ProformaInvoiceBLL> Location = new List<ProformaInvoiceBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProjID", projId);
        cmd.Parameters.AddWithValue("@LocationID", LocationId);
        cmd.Parameters.AddWithValue("@mode", mode);

        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProformaInvoiceBLL obj = new ProformaInvoiceBLL();
                    obj.ClientStateId = Convert.ToInt32(reader["ClientStateId"]);
                    obj.CustStateId = Convert.ToInt32(reader["CustStateId"]);
                    //Added custCountry by Nikhil Shetye on 18-10-2017 for checking india or other country
                    obj.CustCountry = Convert.ToInt32(reader["CustCountry"]);
                    Location.Add(obj);
                }
            }
        }
        return Location;
    }

    public List<ProformaInvoiceBLL> GetGSTPercent(int CodeID, string mode)
    {
        List<ProformaInvoiceBLL> objCodeID = new List<ProformaInvoiceBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CodeID", CodeID);
        cmd.Parameters.AddWithValue("@mode", mode);

        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProformaInvoiceBLL obj = new ProformaInvoiceBLL();
                    obj.GSTPercentage = Convert.ToInt32(reader["GSTPercentage"]);

                    objCodeID.Add(obj);
                }
            }
        }
        return objCodeID;

    }
}