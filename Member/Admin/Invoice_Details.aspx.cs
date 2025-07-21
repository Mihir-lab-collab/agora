using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

public partial class Admin_Invoice_Details : Authentication
{
    string projId;
    string payId;
    string payCodeID;
    string projCodeID;
    string Curr;
    public string copyID = "";
    public string strProject = "";
    public string strInvoices = "1";
    public string strpayType = "";
    string strPayMode = "";

    public string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["Mode"].ToString() == "CID")
        {
            copyID = Request.QueryString["copyId"];
            projId = Request.QueryString["projId"];
        }
        if (Request.QueryString["Mode"].ToString() == "PrID")
            projId = Request.QueryString["projId"];
        if (Request.QueryString["Mode"].ToString() == "PID")
            payId = Request.QueryString["payId"];
        payCodeID = payId;
        projCodeID = projId;


        if (!IsPostBack)
        {
            GetCurreny();
            if (Request.QueryString["payId"] != null)
            {
                btnAddComment.Enabled = true;
            }
            else
                btnAddComment.Enabled = false;
            loadData();
          
        }

    }
    public void loadData()
    {
        SqlDataReader Rdr = default(SqlDataReader);
        SqlConnection conn = new SqlConnection(connectionstring);
        conn.Open();
        string ID = "";
        string sql = "";


        if ((!string.IsNullOrEmpty(payCodeID)) || (!string.IsNullOrEmpty(copyID)))
        {
            if (!string.IsNullOrEmpty(payCodeID))
            {
                ID = payCodeID;
                btnCopy.Visible = false;
            }
            else
            {
                ID = copyID;
                btnCopy.Visible = true;

            }

            sql = "SELECT *, case when paymentMaster.payConfirmedDate = '1900-01-01' then null  else paymentMaster.payConfirmedDate end as ConfirmedDate FROM projectMaster,customerMaster,currencyMaster,paymentMaster "
                + "WHERE projectMaster.custId=customerMaster.custId AND projectMaster."
                + "currId=currencyMaster.currId AND paymentMaster.payprojid="
                + "projectMaster.projId AND payId=" + ID + " ORDER BY projName";


            SqlCommand Cmd = new SqlCommand(sql, conn);
            Rdr = Cmd.ExecuteReader();
            if (Rdr.Read())
            {
                payCode.Value = Convert.ToString(Rdr["payId"]);

                projNameLbl.Text = Convert.ToString(Rdr["projName"]);
                projCode.Value = Convert.ToString(Rdr["projId"]);
                custCompany.Text = Convert.ToString(Rdr["custCompany"]);

                GetCurrenyDetails(Convert.ToString(Rdr["payCurrency"]));

                //currency.Text = Convert.ToString(Rdr["currName"]);
                //Curr = Convert.ToString(Rdr["currSymbol"]);
                payAmount.Value = Convert.ToString(Rdr["payAmount"]);
                payDate.Value = GetFormattedDate(Convert.ToDateTime(Rdr["payDate"]));
                if (!(string.IsNullOrEmpty(Rdr["ConfirmedDate"].ToString())))
                    payConfirmDate.Value = GetFormattedDate(Convert.ToDateTime(Rdr["ConfirmedDate"]));
                else
                    payConfirmDate.Value = "";

                

                payExRate.Value = Convert.ToString(Rdr["payExRate"]);
                payTransCharge.Value = Convert.ToString(Rdr["payTransCharge"]);
                strpayType = Convert.ToString(Rdr["paymentType"]);
                strPayMode = Convert.ToString(Rdr["PaymentMode"]);

                lblComment.Text = Convert.ToString(Rdr["payComment"]);
                lblCRID.Text = Rdr["crId"].ToString();
                // Response.Write("CRID"+Rdr["crId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(Rdr["paymentstatus"])))
                    DisableControls(false);
            }
            Rdr.Close();


            /*get change request title*/
            if (!string.IsNullOrEmpty(lblCRID.Text))
            {
                SqlDataReader tSQlDataReader;
                string tsql = "select chgtitle from changerequest where chgid =" + lblCRID.Text;
                //Response.Write(tsql);
                SqlCommand tCmd = new SqlCommand(tsql, conn);
                tSQlDataReader = tCmd.ExecuteReader();

                if (tSQlDataReader.Read())
                {

                    //Response.Write(tSQlDataReader["chgtitle"]);
                    lblCRTitle.Text = tSQlDataReader["chgtitle"].ToString();
                }

            }

            getData(ID, strpayType);

            rdRegular.Enabled = false;
            rdTM.Enabled = false;

            if (strpayType == "1")
            {
                lblTotalCost.Text = strPayMode;
                lblRegCurr.Text = Curr;
                lblCostCurr.Text = Curr;
                rdRegular.Checked = true;
                rdTM.Checked = false;

                HtmlTable tbl = (HtmlTable)Page.FindControl("tblTMInvoice");
                tbl.Visible = false;
                drpCost.Visible = true;
            }
            else
            {
                lblRateCurr.Text = Curr;
                lblTMCurr.Text = Curr;
                rdTM.Checked = true;
                rdRegular.Checked = false;

                HtmlTable tbl = (HtmlTable)Page.FindControl("tblRegularInvoice");
                tbl.Visible = false;
                drpCost.Visible = false;
            }

        }
        else if (!string.IsNullOrEmpty(projId))
        {
            sql = "SELECT * FROM projectMaster,customerMaster,currencyMaster "
                + "WHERE projectMaster.custId=customerMaster.custId AND projectMaster."
                + "currId=currencyMaster.currId AND projId=" + projId + " ORDER BY projName";
            SqlCommand Cmd = new SqlCommand(sql, conn);
            Rdr = Cmd.ExecuteReader();

            if (Rdr.Read())
            {
                projNameLbl.Text = Convert.ToString(Rdr["projName"]);
                projCode.Value = Convert.ToString(Rdr["projId"]);
                custCompany.Text = Convert.ToString(Rdr["custCompany"]);
                GetCurrenyDetails(Convert.ToString(Rdr["currId"]));
                //currency.Text = Convert.ToString(Rdr["currName"]);
                //Curr = Convert.ToString(Rdr["currSymbol"]);
            }
            strProject = "True";
            Rdr.Close();
            lblTMCurr.Text = Curr;
            lblRegCurr.Text = Curr;
            lblRateCurr.Text = Curr;
            lblCostCurr.Text = Curr;
        }
    }

    private void GetCurreny()
    {
        string sql = "";
        SqlConnection conn = new SqlConnection(connectionstring);
        sql = "Select currId,currName from currencyMaster";
        SqlCommand CmdCurreny = new SqlCommand(sql, conn);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(CmdCurreny);
        DataTable tCurreny = new DataTable();
        dataAdapter.Fill(tCurreny);
        drpCurrency.DataValueField = "currId";
        drpCurrency.DataTextField = "currName";
        drpCurrency.DataSource = tCurreny;
        drpCurrency.DataBind();
    }
    private string GetFormattedDate(DateTime dt)
    {
        return dt.Day.ToString() + "/" + dt.Month.ToString() + "/" + dt.Year.ToString();
    }

    protected void getData(string _payID, string strType)
    {

        SqlConnection conn;
        SqlDataAdapter da;
        DataSet ds;
        conn = new SqlConnection(connectionstring);

        string strSql = "select * from invoiceDetails where invID=" + _payID;

        da = new SqlDataAdapter(strSql, conn);
        ds = new DataSet();
        da.Fill(ds);
        int count = ds.Tables[0].Rows.Count;
        strInvoices = count.ToString();
        if (strType == "1")
        {
            for (int i = 0; i < count; i++)
            {
                int counter = i + 1;

                TextBox txtdescription = (TextBox)tblRegularInvoice.FindControl("txtRegDescription" + counter);
                if (txtdescription != null)
                    txtdescription.Text = ds.Tables[0].Rows[i]["Description"].ToString();
                TextBox txtTotalCost = (TextBox)tblRegularInvoice.FindControl("txtRegTotalCost" + counter);
                if (txtTotalCost != null)
                    txtTotalCost.Text = ds.Tables[0].Rows[i]["Cost"].ToString();

                TextBox txtpayAmount = (TextBox)tblRegularInvoice.FindControl("txtRegAmount" + counter);
                if (txtpayAmount != null)
                    txtpayAmount.Text = ds.Tables[0].Rows[i]["AmountPay"].ToString();
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                int counter = i + 1;
                TextBox txtDescription = (TextBox)tblTMInvoice.FindControl("txtTMDescription" + counter);
                if (txtDescription != null)
                    txtDescription.Text = ds.Tables[0].Rows[i]["Description"].ToString();
                TextBox txtRate = (TextBox)tblTMInvoice.FindControl("txtTMRate" + counter);
                if (txtRate != null)
                    txtRate.Text = ds.Tables[0].Rows[i]["RatePerHours"].ToString();
                TextBox txtHours = (TextBox)tblTMInvoice.FindControl("txtTMHours" + counter);
                if (txtHours != null)
                    txtHours.Text = ds.Tables[0].Rows[i]["Hours"].ToString();

            }

        }
        ds.Dispose();

    }

    protected void btnRegGenerate_Click(object sender, EventArgs e)
    {
        string paymentMode = drpCost.SelectedItem.Text;

        SqlConnection conn = new SqlConnection(connectionstring);
        SqlCommand cmd;

        string sql = "";

        if (!(string.IsNullOrEmpty(payCodeID)))
        {
            string strUpdateSql = " Update paymentMaster " +
                   " SET payDate ='" + convertDate(payDate.Value) + "', " +
                   " payAmount = " + payAmount.Value + ", " +
                   " payExRate = " + payExRate.Value + "," +
                   " payTransCharge = " + payTransCharge.Value + " ," +
                   " payConfirmedDate = '" + convertDate(payConfirmDate.Value) + "', " +
                   " PaymentMode = '" + paymentMode + "', " +
                   " payCurrency = " + drpCurrency.SelectedValue.ToString() +
                   " WHERE payId = " + payCodeID;
            cmd = new SqlCommand(strUpdateSql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            sql = "DELETE FROM invoiceDetails where invID=" + payCodeID;
            cmd = new SqlCommand(sql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

        }
        else
        {

            string strPayType1 = "";
            if (rdRegular.Checked)
                strPayType1 = "1";
            else
                strPayType1 = "2";

            string comment = "" + DateTime.Now.ToString("dd-MMM-yyyy") + "( " + txtComment.Text + " ) <br/>";

            sql = "INSERT INTO paymentMaster(payProjId,payDate,payAmount,payExRate,payConfirmedDate,payComment,payTransCharge,paymentType,PaymentMode,payCurrency)" +
                 "VALUES('" + projCodeID + "','" + convertDate(payDate.Value) + "','" + payAmount.Value + "','" + payExRate.Value + "','" + convertDate(payConfirmDate.Value) + "','" + comment + "','" + payTransCharge.Value + "','" + strPayType1 + "','" + paymentMode + "','" + drpCurrency.SelectedValue.ToString() + "')" +
                 " SELECT @@identity";
            cmd = new SqlCommand(sql, conn);

            conn.Open();
            payCodeID = Convert.ToString(cmd.ExecuteScalar());
            conn.Close();
        }

        for (int counter = 1; counter <= 5; counter++)
        {
            TextBox txtRegDescription = (TextBox)tblRegularInvoice.FindControl("txtRegDescription" + counter);
            TextBox txtRegTotalCost = (TextBox)tblRegularInvoice.FindControl("txtRegTotalCost" + counter);
            TextBox txtRegAmount = (TextBox)tblRegularInvoice.FindControl("txtRegAmount" + counter);



            if (!((string.IsNullOrEmpty(txtRegDescription.Text)) && (string.IsNullOrEmpty(txtRegTotalCost.Text)) && (string.IsNullOrEmpty(txtRegAmount.Text))))
            {
                InsertIntoInvoiceDetails(Convert.ToInt32(payCodeID), txtRegDescription.Text, Convert.ToDouble(txtRegTotalCost.Text), Convert.ToDouble(txtRegAmount.Text));

            }
        }

        string PayType = "";
        if (rdRegular.Checked)
            PayType = "1";
        else
            PayType = "2";

        Response.Redirect("Test.aspx?projid=" + projCode.Value + "&invID=" + payCodeID + "&type=" + PayType);
    }

    protected void InsertIntoInvoiceDetails(int PayCode, string Description, double Cost, double Amount)
    {
        SqlConnection conn = new SqlConnection(connectionstring);
        string strInsertSql = "INSERT INTO invoiceDetails(invID,Type,Description,Cost,AmountPay)" +
                              "VALUES(" + PayCode + ",1,'" + Description.Trim().Replace("'", "") + "'," + Cost + "," + Amount + ")";
        SqlCommand sqlCmdInsert = new SqlCommand(strInsertSql, conn);
        conn.Open();
        try
        {
            sqlCmdInsert.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Response.Write("Error == >> " + ex.Message + " <br>" + ex.StackTrace.ToString());
            Response.End();
        }
        finally
        {
            conn.Close();
        }

    }

    private string convertDate(string strDate)
    {
        string[] strAry = strDate.Split('/');
        string strRtn = "";
        try
        {
            strRtn = strAry[1] + "/" + strAry[0] + "/" + strAry[2];
        }
        catch
        {
        }
        return strRtn;
    }

    protected void btnTMGenerate_Click(object sender, EventArgs e)
    {
        SqlConnection conn = new SqlConnection(connectionstring);
        SqlCommand cmd;

        string sql = "";

        if (!(string.IsNullOrEmpty(payCodeID)))
        {
            string strUpdateSql = " Update paymentMaster " +
                   " SET payDate ='" + convertDate(payDate.Value) + "', " +
                   " payAmount = " + payAmount.Value + ", " +
                   " payExRate = " + payExRate.Value + "," +
                   " payTransCharge = " + payTransCharge.Value + ", " +
                   " payConfirmedDate = '" + convertDate(payConfirmDate.Value) + "', " +
                   " payCurrency = " + drpCurrency.SelectedValue +
                   " WHERE payId = " + payCodeID;

            cmd = new SqlCommand(strUpdateSql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            sql = "DELETE FROM invoiceDetails where invID=" + payCodeID;
            cmd = new SqlCommand(sql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

        }
        else
        {
            string strPayType1 = "";
            if (rdRegular.Checked)
                strPayType1 = "1";
            else
                strPayType1 = "2";

            sql = "INSERT INTO paymentMaster(payProjId,payDate,payAmount,payExRate,payConfirmedDate,payTransCharge,paymentType,payCurrency)" +
                  "VALUES('" + projCodeID + "','" + convertDate(payDate.Value) + "','" + payAmount.Value + "','" + payExRate.Value + "','" + convertDate(payConfirmDate.Value) + "','" + payTransCharge.Value + "','" + strPayType1 + "','" + drpCurrency.SelectedValue + "')" +
                  " SELECT @@identity";

            cmd = new SqlCommand(sql, conn);
            conn.Open();
            payCodeID = Convert.ToString(cmd.ExecuteScalar());
            conn.Close();

        }

        for (int counter = 1; counter <= 5; counter++)
        {
            TextBox txtTMDescription = (TextBox)tblTMInvoice.FindControl("txtTMDescription" + counter);
            TextBox txtTMRate = (TextBox)tblTMInvoice.FindControl("txtTMRate" + counter);
            TextBox txtTMHours = (TextBox)tblTMInvoice.FindControl("txtTMHours" + counter);

            if (!((string.IsNullOrEmpty(txtTMDescription.Text)) && (string.IsNullOrEmpty(txtTMRate.Text)) && (string.IsNullOrEmpty(txtTMHours.Text))))
            {
                InsertIntoTMInvoiceDetails(Convert.ToInt32(payCodeID), txtTMDescription.Text, Convert.ToDouble(txtTMRate.Text), Convert.ToDouble(txtTMHours.Text));
                //lblCRID
            }
        }


        string PayType1 = "";
        if (rdRegular.Checked == true)
            PayType1 = "1";
        else
            PayType1 = "2";

        Response.Redirect("Test.aspx?projid=" + projCode.Value + "&invID=" + payCodeID + "&type=" + PayType1);
    }

    protected void InsertIntoTMInvoiceDetails(int PayCode, string Description, double Rate, double Hour)
    {
        SqlConnection conn = new SqlConnection(connectionstring);

        string strInsertSql = " INSERT INTO invoiceDetails(invID,Type,Description,RatePerHours,Hours) " +
                              " VALUES(" + PayCode + ",2,'" + Description.Trim().Replace("'", "") + "'," + Rate + "," + Hour + ")";

        SqlCommand sqlCmdInsert = new SqlCommand(strInsertSql, conn);
        conn.Open();
        try
        {
            sqlCmdInsert.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Response.Write("Error == >> " + ex.Message + " <br>" + ex.StackTrace.ToString());
            Response.End();
        }
        finally
        {
            conn.Close();
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("paymentSummary.aspx?projid=" + projCode.Value);
    }


    protected void btnCopy_Click(object sender, EventArgs e)
    {
        string type = "";
        if (rdRegular.Checked == true)
            type = "1";
        else
            type = "2";


        SqlDataReader Rdr = default(SqlDataReader);
        SqlConnection conn = new SqlConnection(connectionstring);
        conn.Open();
        string strSql = "";
        string invoiceID = "";
        strSql = " select TOP 1 * from invoiceDetails inner join paymentMaster" +
                 " on invoiceDetails.invID=paymentMaster.payId" +
                 " where paymentMaster.payProjId=" + projCode.Value + "and Type=" + type +
                 " order by invID desc";
        SqlCommand Cmd = new SqlCommand(strSql, conn);
        Rdr = Cmd.ExecuteReader();
        if (Rdr.Read())
        {
            invoiceID = Convert.ToString(Rdr["invID"]);
        }
        Rdr.Close();
        conn.Close();
        Response.Redirect("Invoice_Details.aspx?Mode=CID&copyId=" + invoiceID + "&projid=" + projCode.Value);

    }

    protected void update_Click(object sender, EventArgs e)
    {
        if (payConfirmDate.Value != "")
        {
            SqlConnection conn = new SqlConnection(connectionstring);
            string strUpdateSql = "update paymentMaster set PayConfirmedDate='" + convertDate(payConfirmDate.Value) + "' where payId='" + payCodeID + "'";
            SqlCommand sqlCmdUpdate = new SqlCommand(strUpdateSql, conn);
            conn.Open();
            try
            {
                sqlCmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Response.Write("Error == >> " + ex.Message + " <br>" + ex.StackTrace.ToString());
                Response.End();
            }
            finally
            {
                conn.Close();
            }
        }
        //Response.Redirect(Request.RawUrl);
        loadData();
    }
    protected void btnAddComment_Click(object sender, EventArgs e)
    {
        if (txtComment.Text != "")
        {
            string getComment = "";
            //if (lblComment.Text == "")
            //{
            //    getComment = "<br/>-----------------------";
            //}
            //else
            //getComment += lblComment.Text + "</br>Payment For"+lblCRID.Text + Dated : " + DateTime.Now.ToString("dd-MMM-yy") + txtComment.Text + "<br/>  + "<br/>-----------------------";

            //getComment = "<br/>"+lblComment.Text + "Payment For CR_" + lblCRID.Text + " Dated : " + DateTime.Now.ToString("dd-MMM-yyyy") + "( " + txtComment.Text + " ) <br/>";
            getComment = lblComment.Text + "<br/> " + DateTime.Now.ToString("dd-MMM-yyyy") + "( " + txtComment.Text + " ) <br/>";
            SqlConnection conn = new SqlConnection(connectionstring);
            string strUpdateComment = "update paymentMaster set payComment='" + getComment + "' where payId='" + payCodeID + "'";
            SqlCommand sqlCmdUpdate = new SqlCommand(strUpdateComment, conn);
            conn.Open();
            try
            {
                sqlCmdUpdate.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Response.Write("Error == >> " + ex.Message + " <br>" + ex.StackTrace.ToString());
                Response.End();
            }
            finally
            {
                conn.Close();
            }
            //Response.Redirect(Request.RawUrl);
            loadData();
        }
    }


    protected void drpCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnCurreny.Value = drpCurrency.SelectedValue.ToString();
        GetCurrenyDetails(hdnCurreny.Value);
    }

    private void GetCurrenyDetails(string strCurrValue)
    {
        string sql = "";
        SqlConnection conn = new SqlConnection(connectionstring);
       
            sql = "Select currId,currName, currSymbol from currencyMaster where currId = " + strCurrValue;
    
        SqlCommand CmdCurreny = new SqlCommand(sql, conn);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(CmdCurreny);
        DataTable tCurreny = new DataTable();
        dataAdapter.Fill(tCurreny);

        if (tCurreny.Rows.Count > 0)
        {
            Curr = Convert.ToString(tCurreny.Rows[0]["currSymbol"]);
            drpCurrency.SelectedValue = Convert.ToString(tCurreny.Rows[0]["currId"]);
        }
    }

    public class CurrencyData
    {
        public CurrencyData()
        {
        }


    }
    protected void btnCancelInvoice_Click(object sender, EventArgs e)
    {
        string sql = "";
        SqlConnection conn = new SqlConnection(connectionstring);

        sql = "Update paymentMaster set paymentStatus = 'Cancel' Where payId = " + payId.ToString();

        SqlCommand sqlCmdUpdate = new SqlCommand(sql, conn);
        conn.Open();
        try
        {
            sqlCmdUpdate.ExecuteNonQuery();
            
        }
        catch (Exception ex)
        {
            Response.Write("Error == >> " + ex.Message + " <br>" + ex.StackTrace.ToString());
            Response.End();
        }
        finally
        {
            conn.Close();
        }
        loadData();
        DisableControls(false);
    }


    public void DisableControls(bool isEnabled)
    {
        update.Enabled = isEnabled;
        btnRegGenerate.Enabled = isEnabled;
        btnAddNew.Disabled = !isEnabled;
        drpCurrency.Enabled = isEnabled;
        btnCancelInvoice.Enabled = isEnabled;
        payTransCharge.Disabled = !isEnabled;
        txtComment.Enabled = isEnabled;
        payDate.Disabled = !isEnabled;
        payAmount.Disabled = !isEnabled;
        payExRate.Disabled = !isEnabled;
        payConfirmDate.Disabled = !isEnabled;
        drpCost.Enabled = isEnabled;
        btnAddComment.Enabled = isEnabled;
        txtRegDescription1.Enabled = isEnabled;
        txtRegTotalCost1.Enabled = isEnabled;
        txtRegAmount1.Enabled = isEnabled;
        regDelete1.Enabled = isEnabled;

        txtRegDescription2.Enabled = isEnabled;
        txtRegTotalCost2.Enabled = isEnabled;
        txtRegAmount2.Enabled = isEnabled;
        regDelete2.Enabled = isEnabled;

        txtRegDescription3.Enabled = isEnabled;
        txtRegTotalCost3.Enabled = isEnabled;
        txtRegAmount3.Enabled = isEnabled;
        regDelete3.Enabled = isEnabled;

        txtRegDescription4.Enabled = isEnabled;
        txtRegTotalCost4.Enabled = isEnabled;
        txtRegAmount4.Enabled = isEnabled;
        regDelete4.Enabled = isEnabled;

        txtRegDescription5.Enabled = isEnabled;
        txtRegTotalCost5.Enabled = isEnabled;
        txtRegAmount5.Enabled = isEnabled;
        regDelete5.Enabled = isEnabled;
    }
}


