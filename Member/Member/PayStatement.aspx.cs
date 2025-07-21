using CSCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_PayStatement : Authentication
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    UserMaster UM;
    double sum = 0;
    public int Month, Year;
    public string Date;
    public string flag = string.Empty; 

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (Session["PayId"] == null)
        {
            Response.Redirect("PayProcess.aspx");
        }

        if (!IsPostBack)
        {
            FillPayStatement();
            if (hdnMonth.Value != "" && hdnYear.Value != "")
                Date = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(hdnMonth.Value)).ToString() + " " + hdnYear.Value;
        }

    }

    protected void FillPayStatement()
    {
        btnSubmit.Visible = false;
        if (!string.IsNullOrEmpty(Convert.ToString(Session["PayId"])))
        {
            List<PayrollBLL> lsPayroll = PayrollBLL.GetPayStatementDetails("GetPayStatementDetail", Convert.ToInt32(Session["PayId"]));
            if (lsPayroll.Count > 0)
            {
                hdnMonth.Value = Convert.ToString(lsPayroll[0].PayDate.Month);
                hdnYear.Value = Convert.ToString(lsPayroll[0].PayDate.Year);
                hdnLocId.Value = Convert.ToString(lsPayroll[0].locationId);
                dgrdCLetter.DataSource = lsPayroll;
                dgrdCLetter.DataBind();
                if (lsPayroll.Count > 0)
                {
                    btnSubmit.Visible = true;
                }
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Session["PayId"] != null)
        {
            hdnEmpIds.Value = HttpContext.Current.Request.Form["chkSelect"].ToString();
            divPayStmtDetail.Style.Add("display", "none");
            divBankStatement.Style.Add("display", "block");
            FillBankStatement();
        }
    }
    protected void dgrdCLetter_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string cellValue = DataBinder.Eval(e.Item.DataItem, "Net").ToString();
            if (!string.IsNullOrEmpty(cellValue))
            {
                sum += Convert.ToDouble(cellValue);
            }
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            e.Item.Cells[2].Text = "<b>Grand Total</b>";
            e.Item.Height = 30;
            e.Item.Cells[6].Text = "<b>Rs. " + Global.GetCurrencyFormat(Convert.ToDouble(sum)) + " </b>";
        }
    }


    private void FillBankStatement()
    {
        string StmtBody = "";
        flag = "True";
        List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 3);
        if (lstConfig.Count > 0)
        {
            List<LocationBLL> location = LocationBLL.BindLocation("GetLocationByID", Convert.ToInt32(hdnLocId.Value));
            StmtBody = lstConfig[0].value1.ToString();
            StmtBody = StmtBody.Replace("{Date}", DateTime.Today.ToString("dd-MMM-yyy"));
            StmtBody = StmtBody.Replace("{BankAccount}", location[0].BankAccount);
            StmtBody = StmtBody.Replace("{Address}", (location[0].Bank.IndexOf(',') != -1 ? location[0].Bank.ToString().Replace(",", ",<BR>") : location[0].Bank.ToString()));
            StmtBody = StmtBody.Replace("{Month}", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(hdnMonth.Value)).ToString());
            StmtBody = StmtBody.Replace("{year}", hdnYear.Value);
            StmtBody = StmtBody.Replace("{trProcess}", GetProceedEmpDetail());
            StmtBody = StmtBody.Replace("{Sum}", Global.GetCurrencyFormat(Convert.ToDouble(hdnSumValue.Value)));
            StmtBody = StmtBody.Replace("{Company}", location[0].LegalName);

            divInBankStatement.InnerHtml = StmtBody;
        }

    }
    public string GetProceedEmpDetail()
    {
        string str = "";
        sum = 0;
        if (!string.IsNullOrEmpty(Convert.ToString(Session["PayId"])))
        {
            List<PayrollBLL> lstPayEmp = PayrollBLL.GetPayStatementDetails("GetPayStatementDetail", Convert.ToInt32(Session["PayId"]), hdnEmpIds.Value);
            str += "<table cellspacing='0' cellpadding='2' border='1' style='background-color:White;border-color:Black;font-size:12pt; width:100%; max-width:825px; border-collapse:collapse;color: black; font-family: Times New Roman; margin: 20px auto;' id='dgrdCLetter1'  rules='all'>" +
                              "<tr style='font-size:11pt;font-weight:bold;width:70px;'>" +
                              "<td style='padding:5px;width:7%'>Sr.</td>" +
                                       "<td align='left' style='padding:5px;text-wrap:initial;width:45%;'> Employee Name </td>" +
                                       "<td align='center' style='width:15%;padding:5px;text-wrap:initial'>Empolyee Code</td>" +
                                       "<td align='center' style='width:16%;padding:5px;'>Account Number</td>" +
                                       "<td align='center' style='width:16%;padding:5px;'>IFSC Code</td>" +
                                       "<td style='padding:5px;width:17%;'>Net Salary(Rs.)</td>" +
                                   "</tr>";
            if (lstPayEmp.Count > 0)
            {
                string formatedAccountNumber=string.Empty;
     
                foreach (var item in lstPayEmp)
                {
                    if (flag == "True")
                    {
                        formatedAccountNumber = "<td align='center' style='font-weight:bold;padding:5px;width:16%'>" + item.AccountNo + "</td>";
                       
                    }
                    else
                    {
                        formatedAccountNumber = "<td align='center' style='font-weight:bold;padding:5px;width:16%'>" + "=\"" + item.AccountNo + " \"" + "</td>";
                    }

                    str += "<tr valign='middle' style='font-size:11pt;'>" +
                                    "<td height:22px style='padding:5px;width:7%'>" + (lstPayEmp.IndexOf(item) + 1).ToString() + "</td>" +
                                    "<td align='left' style='font-weight:bold;white-space:nowrap; padding:5px;width:45%;'><span style='display:inline-block;'>" + item.EmpName + "</span></td>" +
                                    "<td align='center' style='padding:5px;width:15%'>" + item.EmpID.ToString() + "</td>" + formatedAccountNumber +
                                       "<td align='left' style='font-weight:bold;white-space:nowrap; padding:5px;width:45%;'><span style='display:inline-block;'>" + item.IFSCCOde + "</span></td>" +
                                    "<td align='right' style='width:17%;padding:5px;'> <label>" + Global.GetCurrencyFormat(Convert.ToDouble(item.Net)) + "</label ></td>" +
                                "</tr>";
                    sum += Convert.ToDouble(item.Net);
                    formatedAccountNumber = "";
                }
               
            }
            str += " <tr align='right'>" +
                                    " <td style='padding:5px;'>&nbsp;</td>" +
                                     "<td style='padding:5px;'><b>Grand Total</b></td>" +
                                     "<td style='padding:5px;'>&nbsp;</td>" +
                                     "<td>&nbsp;</td>" +
                                     "<td>&nbsp;</td>" + 
                                     "<td align='right' style='padding:5px;'><b>Rs. " + Global.GetCurrencyFormat(sum) + "</b></td>" +
                                 "</tr>" +
                         "</table>";
            hdnSumValue.Value = sum.ToString();
            flag = "False";
        }

        return str;
    }

    //protected void btnExport_Click(object sender, EventArgs e)
    //{

    //    string TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\BankStatement\";

    //    List<LocationBLL> location = LocationBLL.BindLocation("GetLocationByID", Convert.ToInt32(hdnLocId.Value));
    //    string strTo = "";
    //    string StrLast = "";
    //    strTo = "\r\r\r Date : " + DateTime.Today.ToString("dd-MMM-yyyy") + "<BR>" +
    //            "To, <BR>" +
    //            "The Manager, <BR>" +
    //             (location[0].Bank.IndexOf(',') != -1 ? location[0].Bank.ToString().Replace(",", ",<BR>") : location[0].Bank.ToString()) + "<BR><BR>" +
    //            "<B>Subject: Credit Of Salary A/C No. " + location[0].BankAccount + "</B><BR><BR>" +
    //            "Sir," + "<BR>" +
    //            "Please find enclosed here with Cheque No. " + hdnCheckno.Value + " of   Rs. " + Global.GetCurrencyFormat(Convert.ToDouble(hdnSumValue.Value)) + "  only drawn on " + hdntxtDate.Value +
    //            " towards salary of the employee for the month of " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(hdnMonth.Value)).ToString() + " " + hdnYear.Value.ToString() + ".<BR>" +
    //            "Please credit salary as per following details," + "<BR>";

    //    StrLast = "Kindly do the needfull and acknowledge." + "<BR>" +
    //              "Thanking you," + "<BR><BR>" +
    //              "Yours Sincerely," + "<BR>" +
    //              "<B> For " + location[0].LegalName + "</B><BR><BR><BR>" +
    //              "<B>Director " + "</B><BR>";

    //    string attachment = "attachment; filename=" + "BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls";
    //    string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    //    string pathDownload = Path.Combine(pathUser, "Downloads");
    //    string sourceFile = pathDownload + "\\" + "BankStatment_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls";
    //    Response.ClearContent();
    //    Response.AddHeader("content-disposition", attachment);
    //    Response.ContentType = "application/ms-excel";
    //    Response.Write(strTo);
    //    Response.Write("<BR>");
    //    Response.Write(GetProceedEmpDetail());
    //    Response.Write("<BR>");
    //    Response.Write(StrLast);
    //    string style = @"<style> .textmode { mso-number-format:\@; } </style>";

    //    TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\BankStatement\";

    //    if (!Directory.Exists(TargetDir))
    //    {
    //        Directory.CreateDirectory(TargetDir);
    //    }

    //    DirectoryInfo dinfo2 = new DirectoryInfo(pathDownload);
    //    string destinationFile = TargetDir + @"BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls";

    //    if (File.Exists(TargetDir + @"BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls"))
    //    {
    //        try
    //        {
    //            File.Delete(TargetDir + @"BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls");
    //        }
    //        catch
    //        {
    //            Response.Write("Error in deleting PDF");
    //        }
    //    }
    //    Response.Write(style);


    //    StringBuilder excel = new StringBuilder();

    //    excel.Append(strTo);
    //    excel.Append("<BR>");
    //    excel.Append(GetProceedEmpDetail());
    //    excel.Append("<BR>");
    //    excel.Append(StrLast);
    //    excel.Append(style);
    //    // save to file
    //    File.WriteAllText(destinationFile, excel.ToString());

    //    // output to response
    //    Response.Clear();
    //    Response.Buffer = true;

    //    Response.Write(excel.ToString());
    //    Response.End();

    //}

    protected void btnExport_Click(object sender, EventArgs e)
    {

        string TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\BankStatement\";

        List<LocationBLL> location = LocationBLL.BindLocation("GetLocationByID", Convert.ToInt32(hdnLocId.Value));
        string strTo = "";
        string StrLast = "";
        strTo = "\r\r\r Date : " + DateTime.Today.ToString("dd-MMM-yyyy") + "<BR>" +
                "To, <BR>" +
                "The Manager, <BR>" +
                 (location[0].Bank.IndexOf(',') != -1 ? location[0].Bank.ToString().Replace(",", ",<BR>") : location[0].Bank.ToString()) + "<BR><BR>" +
                "<B>Subject: Credit Of Salary A/C No. " + location[0].BankAccount + "</B><BR><BR>" +
                "Sir," + "<BR>" +
                "Please find enclosed here with Cheque No. " + hdnCheckno.Value + " of   Rs. " + Global.GetCurrencyFormat(Convert.ToDouble(hdnSumValue.Value)) + "  only drawn on " + hdntxtDate.Value +
                " towards salary of the employee for the month of " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(hdnMonth.Value)).ToString() + " " + hdnYear.Value.ToString() + ".<BR>" +
                "Please credit salary as per following details," + "<BR>";

        StrLast = "Kindly do the needfull and acknowledge." + "<BR>" +
                  "Thanking you," + "<BR><BR>" +
                  "Yours Sincerely," + "<BR>" +
                  "<B> For " + location[0].LegalName + "</B><BR><BR><BR>" +
                  "<B>Director " + "</B><BR>";

        string attachment = "attachment; filename=" + "BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls";
        string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string pathDownload = Path.Combine(pathUser, "Downloads");
        string sourceFile = pathDownload + "\\" + "BankStatment_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        Response.Write(strTo);
        Response.Write("<BR>");
        Response.Write(GetProceedEmpDetail());
        Response.Write("<BR>");
        Response.Write(StrLast);
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";

        TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\BankStatement\";

        if (!Directory.Exists(TargetDir))
        {
            Directory.CreateDirectory(TargetDir);
        }

        DirectoryInfo dinfo2 = new DirectoryInfo(pathDownload);
        string destinationFile = TargetDir + @"BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls";

        if (File.Exists(TargetDir + @"BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls"))
        {
            try
            {
                File.Delete(TargetDir + @"BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls");
            }
            catch
            {
                Response.Write("Error in deleting PDF");
            }
        }
        Response.Write(style);


        StringBuilder excel = new StringBuilder();

        excel.Append(strTo);
        excel.Append("<BR>");
        excel.Append(GetProceedEmpDetail());
        excel.Append("<BR>");
        excel.Append(StrLast);
        excel.Append(style);
        // save to file
        File.WriteAllText(destinationFile, excel.ToString());

        // output to response
        Response.Clear();
        Response.Buffer = true;

        Response.Write(excel.ToString());
        Response.End();

    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetMailInfo()
    {
        string TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"BankStatement\";

        try
        {

            var data = new MailInfo
           {
               ccName = "accounts@intelgain.com",
               subject = "Bank Satatement of Intelgain Technologies for the month " + DateTime.Now.AddMonths(-1).ToString("MMMM") + "-" + DateTime.Now.Year.ToString(),
               description = "<p>Dear Sir / Madam,<p>" + "<p>Please find herewith attached salary sheet for the month of " +
               DateTime.Now.AddMonths(-1).ToString("MMMM") + "-" + DateTime.Now.Year.ToString() + "</P>Kindly acknowledge receipt of this mail.</P>" + "<br><P>Sincerely,<br/>Accounts Team <br/>Intelegain Technologies</P>",
               filePath = TargetDir + @"BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls",
               fileName = @"BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls"
           };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string str = jss.Serialize(data);
            return str;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string SendMail(string To, string Cc, string Bcc, string Subject, string MsgBody)
    {
        string sReturn = "Success";
        string FromMailID = "accounts@intelgain.com";
        try
        {
            if (To != "" && MsgBody != "")
            {
                string TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\BankStatement\";
                string strFileName = TargetDir + @"BankStatement_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls";
                FileInfo objInfo = new FileInfo(strFileName);
                sReturn = CSCode.Global.SendMailWithAttachments(MsgBody, Subject, To, FromMailID, true, Cc, Bcc, strFileName);


            }
        }
        catch (Exception ex)
        {

        }

        return sReturn;
    }
}