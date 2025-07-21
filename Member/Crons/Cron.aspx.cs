using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using Customer.DAL;
using System.Text;
using System.Globalization;
using CSCode;

public partial class Crons_Cron : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string Mode = "";
        string Days = string.Empty;
        int days = 0;
        Mode = Request.QueryString["m"];
        if (Request.QueryString["d"] != null)
        {
            Days = Request.QueryString["d"];
            days = Convert.ToInt32(Days);
        }

        switch (Mode)
        {
            //case "TM":
            //    TestMail();
            //    break;
            case "D":
                Wishes();
                DailyReport();
                //SendchartReport();
                break;
            case "EMPA":
                EmpAppraisals();
                break;
            case "BI":
                BIReportProcess();
                break;
            case "TS":
                TSAlert();
                break;
            case "MS":
                ProcessRecurringMilestones();
                break;
            case "DUEDATE":
                ProcessMilestonesDueDate(days);
                break;
            case "TDS":
                SendMonthlyTdsReport();
                break;
            default:
                DivStatus.InnerHtml = "Invalid Access";
                break;
        }
    }
    private void SendMonthlyTdsReport()
    {
        if (CSCode.Global.CheckTdsReportLog())
        {
            Response.Write("<BR>" + "An attempt has already been made for this month " + "<BR>");
            return;
        }
        else
        {
            Response.Write("<BR>" + CSCode.Global.GetMonthlyTdsReportMail("GetMonthlyTdsReport") + "<BR>");
        }
    }

    private void TSAlert()
    {
        DateTime StartDate = DateTime.Now.AddDays(-7);
        String SubjectStr = "Incomplete timesheet report between " + StartDate.AddDays(-30).ToString("dd-MMM-yy") + " and " + StartDate.ToString("dd-MMM-yy");
        String BodyStr = "";

        DataTable dt = CSCode.Global.IncompleteTS(0, 30, StartDate, 10);
        BodyStr = CSCode.Global.ExportDatatableToHtml(dt);
        if (dt.Rows.Count > 0)
        {
            string incompleteTimesheetReportRecepientLocation10 = System.Configuration.ConfigurationManager.AppSettings["IncompleteTimesheetReportRecepientLocation10"];
            Response.Write("<BR>" + CSCode.Global.SendMail(BodyStr, SubjectStr, incompleteTimesheetReportRecepientLocation10) + "<BR>");//kiran.y@intelgain.com;shalini@intelgain.com;kapil@intelgain.com;neeraj@intelgain.com
        }

        dt = CSCode.Global.IncompleteTS(0, 30, StartDate, 11);
        BodyStr = CSCode.Global.ExportDatatableToHtml(dt);
        if (dt.Rows.Count > 0)
        {
            string incompleteTimesheetReportRecepientLocation11 = System.Configuration.ConfigurationManager.AppSettings["IncompleteTimesheetReportRecepientLocation11"];
            Response.Write("<BR>" + CSCode.Global.SendMail(BodyStr, SubjectStr, incompleteTimesheetReportRecepientLocation11));//jimmy@intelgain.com,aman.g@intelgain.com,neeraj@intelgain.com
        }
    }

    private void BIReportProcess()
    {
        Response.Write("<BR>" + CSCode.Global.BIReportProcess());
    }

    private void ProcessRecurringMilestones()
    {
        CSCode.Global.ProcessRecurringMilestones();
    }

    private void ProcessMilestonesDueDate(int days)
    {
        DataSet ds = mileStoneDue.getMilestoneDue(days);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];

        var distinctProjectManagerEmail = dt.AsEnumerable()
                    .Select(s => new
                    {
                        Email = s.Field<string>("ProjectManagerEmail"),
                    })
                    .Distinct().ToList();

        var distinctAccountManagerEmail = dt.AsEnumerable()
                   .Select(s => new
                   {
                       Email = s.Field<string>("AccountManagerEmail"),
                   })
                   .Distinct().ToList();

        var totalEmail = distinctProjectManagerEmail.Concat(distinctAccountManagerEmail).ToList();
        var distinctEmail = totalEmail.Select(o => o.Email).Distinct().ToList();

        DataTable dtMSDue = new DataTable();
        dtMSDue.Columns.Add("MilestoneID");
        dtMSDue.Columns.Add("Name");
        dtMSDue.Columns.Add("projID");
        dtMSDue.Columns.Add("projName");
        dtMSDue.Columns.Add("Amount");
        dtMSDue.Columns.Add("projManagerId");
        dtMSDue.Columns.Add("AccountManagerId");
        dtMSDue.Columns.Add("Project Mananger");
        dtMSDue.Columns.Add("Project Mananger Email");
        dtMSDue.Columns.Add("Account Mananger");
        dtMSDue.Columns.Add("Account Mananger Email");
        dtMSDue.Columns.Add("DueDate");

        DataTable dtMSUpcomming = new DataTable();
        dtMSUpcomming.Columns.Add("MilestoneID");
        dtMSUpcomming.Columns.Add("Name");
        dtMSUpcomming.Columns.Add("projID");
        dtMSUpcomming.Columns.Add("projName");
        dtMSUpcomming.Columns.Add("Amount");
        dtMSUpcomming.Columns.Add("projManagerId");
        dtMSUpcomming.Columns.Add("AccountManagerId");
        dtMSUpcomming.Columns.Add("Project Mananger");
        dtMSUpcomming.Columns.Add("Project Mananger Email");
        dtMSUpcomming.Columns.Add("Account Mananger");
        dtMSUpcomming.Columns.Add("Account Mananger Email");
        dtMSUpcomming.Columns.Add("DueDate");

        var dateAndTime = DateTime.Now.ToShortDateString();

        foreach (DataRow row in dt.Rows)
        {
            if (Convert.ToDateTime(row["DueDate"]) <= Convert.ToDateTime(dateAndTime))
            {
                DataRow rowMSDue = dtMSDue.NewRow();
                rowMSDue["MilestoneID"] = row[0];
                rowMSDue["Name"] = row[1];
                rowMSDue["projID"] = row[2];
                rowMSDue["projName"] = row[3];
                rowMSDue["Amount"] = row[4];
                rowMSDue["projManagerId"] = row[5];
                rowMSDue["AccountManagerId"] = row[6];
                rowMSDue["Project Mananger"] = row[7];
                rowMSDue["Project Mananger Email"] = row[8];
                rowMSDue["Account Mananger"] = row[9];
                rowMSDue["Account Mananger Email"] = row[10];
                rowMSDue["DueDate"] = row[11];
                dtMSDue.Rows.Add(rowMSDue);

            }
            else
            {
                DataRow rowMSUpcomming = dtMSUpcomming.NewRow();
                rowMSUpcomming["MilestoneID"] = row[0];
                rowMSUpcomming["Name"] = row[1];
                rowMSUpcomming["projID"] = row[2];
                rowMSUpcomming["projName"] = row[3];
                rowMSUpcomming["Amount"] = row[4];
                rowMSUpcomming["projManagerId"] = row[5];
                rowMSUpcomming["AccountManagerId"] = row[6];
                rowMSUpcomming["Project Mananger"] = row[7];
                rowMSUpcomming["Project Mananger Email"] = row[8];
                rowMSUpcomming["Account Mananger"] = row[9];
                rowMSUpcomming["Account Mananger Email"] = row[10];
                rowMSUpcomming["DueDate"] = row[11];
                dtMSUpcomming.Rows.Add(rowMSUpcomming);
            }
        }

        DataTable dtMSUpcommingemail = new DataTable();
        dtMSUpcommingemail.Columns.Add("projName");
        dtMSUpcommingemail.Columns.Add("Name");
        dtMSUpcommingemail.Columns.Add("DueDate");
        dtMSUpcommingemail.Columns.Add("Amount");

        DataTable dtMSDueemail = new DataTable();
        dtMSDueemail.Columns.Add("projName");
        dtMSDueemail.Columns.Add("Name");
        dtMSDueemail.Columns.Add("DueDate");
        dtMSDueemail.Columns.Add("Amount");
        string strEmail = string.Empty;
        foreach (var email in distinctEmail)
        {
            int i = 0;
            foreach (DataRow dr in dtMSDue.Rows)
            {

                if (dr["Project Mananger Email"].ToString() == email || dr["Account Mananger Email"].ToString() == email)
                {
                    i = 2;
                    DataRow dtrMSDueemail = dtMSDueemail.NewRow();
                    dtrMSDueemail["projName"] = dr[3];
                    dtrMSDueemail["Name"] = dr[1];
                    dtrMSDueemail["DueDate"] = dr[11];
                    dtrMSDueemail["Amount"] = dr[4];
                    dtMSDueemail.Rows.Add(dtrMSDueemail);
                }

            }

            if (dtMSDueemail != null)
            {
                if (dtMSDueemail.Rows.Count > 0)
                {
                    strEmail = getMailBody(dtMSDueemail, "MilestonDue");
                    dtMSDueemail.Clear();

                }
            }

            foreach (DataRow dr in dtMSUpcomming.Rows)
            {
                if (dr["Project Mananger Email"].ToString() == email || dr["Account Mananger Email"].ToString() == email)
                {
                    i = 3;

                    DataRow dtrMSUpcommingemail = dtMSUpcommingemail.NewRow();
                    dtrMSUpcommingemail["projName"] = dr[3];
                    dtrMSUpcommingemail["Name"] = dr[1];
                    dtrMSUpcommingemail["DueDate"] = dr[11];
                    dtrMSUpcommingemail["Amount"] = dr[4];
                    dtMSUpcommingemail.Rows.Add(dtrMSUpcommingemail);

                }
            }

            if (dtMSUpcommingemail != null)
            {
                if (dtMSUpcommingemail.Rows.Count > 0)
                {
                    strEmail += getMailBody(dtMSUpcommingemail, "UpcomingMilestone");
                    dtMSUpcommingemail.Clear();

                }
            }

            if (i == 2 || i == 3)
            {
                SendMilestoneEmail(strEmail, email);
                strEmail = string.Empty;
            }

            i = 0;
        }
    }

    private void SendMilestoneEmail(string strEmail, string email)
    {
        string MailBody = "";
        string CC = "";
        MailBody = Convert.ToString(strEmail);
        Response.Write("<BR>" + CSCode.Global.SendMail(MailBody, "Milestone Due Report", strEmail, CC) + "<BR>");
    }

    private string getMailBody(DataTable dt, string key)
    {
        string str = string.Empty;
        string strEmailBody = string.Empty;
        string heading = string.Empty;
        if (key == "MilestonDue")
        {
            heading = "Milestone Due";
        }
        else
        {
            heading = "Upcoming Milestones";
        }
        str = "<div style=" + "background-color:#ffffff;font-family:Arial, Helvetica, sans-serif;font-size:14px;" + "><h2><strong><span style=" + "text-decoration:underline;color:#000000;" + ">" + heading + "</span></strong></h2><table bgcolor=" + "#ffe6e6" + " border=" + "1" + "width=" + "1000" + "><thead><tr><th>Project Name</th><th>Milestone</th><th>Due Date</th><th>Amount</th></tr></thead>";


        str += "<tbody>";
        foreach (DataRow dr in dt.Rows)
        {
            str += "<tr><td style=" + "text-align:center" + ">" + dr["projName"].ToString() + "</td><td style=" + "text-align:center" + ">" + dr["Name"].ToString() + "</td><td style=" + "text-align:center" + ">" + dr["DueDate"].ToString() + "</td><td style=" + "text-align:center" + ">" + Convert.ToString(Global.GetCurrencyFormat(Convert.ToInt32(dr["Amount"]))) + "</td></tr>";
        }
        str += "</tbody></table></div>";
        // strEmailBody = strEmailBody.Replace("{Content}", str);
        strEmailBody = str;
        return strEmailBody;
    }

    private void Wishes()
    {
        // Method name in global 'BirthdayWishes' 
        Response.Write("<BR>" + CSCode.Global.BirthdayWishes("GetBirthday") + "<BR>");
    }
    //private void SendchartReport()
    //{
    //    Response.Write("<BR>" + CSCode.Global.GetChartData("GetChart") + "<BR>");
    //}
    private void EmpAppraisals()
    {
        DataTable DT = CSCode.Global.EmployeeQuarterlyAppraisal();
        Response.Write("<BR>" + "Record added successfully" + "<BR>");
    }


    private void DailyReport()
    {

        string MailBody = "";
        //const string ConstEmail = "kapil@intelgain.com;pmo@intelgain.com";////For live
        const string ConstEmail = "";////for local

        string CC = "";
        StringBuilder strHTMLBuilder = new StringBuilder();
        strHTMLBuilder.Append("<html >");
        strHTMLBuilder.Append("<head>");
        strHTMLBuilder.Append("</head>");
        strHTMLBuilder.Append("<body>");
        strHTMLBuilder.Append("<h1><center>Consolidated Work Report<center></h3>");
        string strUpcomingLeaves = UpcomingLeaves();
        strHTMLBuilder.Append(strUpcomingLeaves);

        string strBIReport = BIReport();
        strHTMLBuilder.Append(strBIReport);

        string strNoWorks = NoWork();
        strHTMLBuilder.Append(strNoWorks);

        string strTimeSheet = TimeSheetDetails();
        strHTMLBuilder.Append(strTimeSheet);

        strHTMLBuilder.Append("</body>");
        strHTMLBuilder.Append("</html >");

       // MailBody = Convert.ToString(strHTMLBuilder);
        MailBody = Convert.ToString(strHTMLBuilder) + string.Format("<br />Thank You, <br /> Agora team ");
        DataSet dailyR = BITSDAL.BIReport("DailyBIReport");
        DataTable dtDays = dailyR.Tables[2];
        CC = ConstEmail; ////For live
        ////CC = "";///// for local
        string sub = "Consolidated Work Report for " + dtDays.Rows[0][0].ToString();
        // Response.Write("<BR>" + CSCode.Global.SendMail(MailBody, sub, "neeraj@intelgain.com", CC) + "<BR>");//;neeraj@intelgain.com    for live
        //Response.Write("<BR>" + CSCode.Global.SendMail(MailBody, "Daily Management Report", "trupti.d@intelgain.com",CC) + "<BR>");//;neeraj@intelgain.com  For Local
        Response.Write("<BR>" + CSCode.Global.SendMail(MailBody, sub, "trupti.d@intelegain.com", CC) + "<BR>"); //for local by trupti
    }

    private string UpcomingLeaves()
    {
        String BodyStr = "";
        StringBuilder strHTMLBuilder = new StringBuilder();

        strHTMLBuilder.Append("<h4 align=left>Upcoming Leave as on :    " + DateTime.Now.ToString("dd-MMM-yy") + "</h4>");
        DataTable dt = CSCode.Global.UpcomingLeaves("UpcomingLeaves");
        //if (dt != null && dt.Rows.Count > 0)
        //{
            BodyStr = Convert.ToString(strHTMLBuilder) + CSCode.Global.ExportDatatableToHtml(dt);
        //}
        
        return BodyStr;
    }
    private string TimeSheetDetails()
    {
        DataSet dailyR = BITSDAL.BIReport("DailyBIReport");
        DataSet ds = BITSDAL.getTimeSheetdetails("GetTimeSheetData");
        StringBuilder strHTMLBuilder = new StringBuilder();
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dtMonth = ds.Tables[0];
            // DataTable dtData = ds.Tables[1];
            DataTable dtDays = dailyR.Tables[2];
            DataTable dtPrevDaydata = ds.Tables[1];

            if (ds != null && ds.Tables.Count > 0)
            {
                if (dtPrevDaydata != null && dtPrevDaydata.Rows.Count > 0)
                {
                    strHTMLBuilder.Append("<H4 align=left>Timesheet report as on:    " + dtDays.Rows[0][0].ToString() + " </H4>");
                    strHTMLBuilder.Append("<table width='100%' BorderColor='black' cellspacing='0' cellpadding='5'  style='font-size:15px;border-style:solid; border-width:thin;'>");
                    
                    strHTMLBuilder.Append("<tr bgcolor='#00B0F0' BorderColor='black' style='border-style:solid;border-width:thin;border-left:none;'>");

                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;border-left:none;'><font color=white>Srl. No</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>Emp Name</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>Project Name</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>Module Name</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>Last Month</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>"+ dtDays.Rows[0][2].ToString() + "</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>"+ dtDays.Rows[0][1].ToString() + "</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>"+ dtDays.Rows[0][0].ToString() + "</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>"+DateTime.Now.ToString("dd MMM yyyy")+"</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>This Week</font></td>");
                    strHTMLBuilder.Append("<td style='bgcolor=#00B0F0;font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;'><font color=white>This Month</font></td>");
                    


                    strHTMLBuilder.Append("</tr>");
                    int projid = 0;
                    int empId = 0;
                    int modulecount = 0;
                    int rcount = 0;
                    foreach (DataRow row in dtPrevDaydata.Rows)
                    {

                        strHTMLBuilder.Append("<tr align=left>");
                        foreach (DataColumn myColumn in dtPrevDaydata.Columns)
                        {
                            
                            if (myColumn.ColumnName != "Date")
                            {
                               
                                if (myColumn.ColumnName == "empId")
                                {
                                    empId = Convert.ToInt32(row[myColumn.ColumnName].ToString());
                                    DataSet dts = BITSDAL.getprojNamebyEmpId("GetProjNameByEmpId", empId);
                                    DataTable dtm = dts.Tables[0];
                                    modulecount = dtm.Rows.Count;
                                   
                                }
                                else if (myColumn.ColumnName == "Srl. No")
                                {
                                    if (dtPrevDaydata.Rows.Count > rcount)
                                    {

                                        empId = Convert.ToInt32(ds.Tables[1].Rows[rcount][1].ToString());
                                        DataSet dts = BITSDAL.getprojNamebyEmpId("GetProjNameByEmpId", empId);
                                        DataTable dtm = dts.Tables[0];
                                        modulecount = dtm.Rows.Count;
                                    }
                                   rcount ++;
                                    strHTMLBuilder.Append("<td rowspan="+ modulecount + " style='border:0.5px solid;border-right:none;border-left:none;'>");
                                    strHTMLBuilder.Append(row[myColumn.ColumnName].ToString());
                                    strHTMLBuilder.Append("</td>");
                                }
                                else
                                {
                                    strHTMLBuilder.Append("<td rowspan="+modulecount+" style='border:0.5px solid;border-right:none;'>");
                                    strHTMLBuilder.Append(row[myColumn.ColumnName].ToString());
                                    strHTMLBuilder.Append("</td>");
                                }
                            }
                        }



                        ///----------------------------------------------------------------------------------------------

                        DataSet dsEmp = BITSDAL.getprojNamebyEmpId("GetProjNameByEmpId", empId);
                        DataTable dtDataEmp = dsEmp.Tables[0];
                        int trowcount = dtDataEmp.Rows.Count + 1;

                        int i = 0;
                        foreach (DataRow mrow in dtDataEmp.Rows)
                        { 
                            foreach (DataColumn myColumnEmp in dtDataEmp.Columns)
                            {
                                if (myColumnEmp.ColumnName != "Date")
                                {
                                    if (myColumnEmp.ColumnName == "projId")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (dtDataEmp.Rows.Count > i)
                                        {
                                            projid = Convert.ToInt32(dsEmp.Tables[0].Rows[i][1].ToString());
                                        }
                                      
                                        i++;
                                       
                                        strHTMLBuilder.Append("<td style='border-style:solid;border-width:thin;border-bottom:none;border-right:none;'>");
                                        strHTMLBuilder.Append(mrow[myColumnEmp.ColumnName].ToString());
                                        strHTMLBuilder.Append("</td>");
                                        
                                    }
                                }

                               
                                

                                DataSet dsM = BITSDAL.GetTSData("TimeSheetReportbyEmp", projid, empId);
                                DataTable dtDataM = dsM.Tables[0];
                                int rowcount = dtDataM.Rows.Count + 1;

                                // strHTMLBuilder.Append("<td Colspan=" + 3 + " rowspan=" + rowcount + " style='font-size:15px;border-style:solid; border-width:thin;border-bottom:none; '>");

                                //foreach (DataColumn myColumn1 in dtDataM.Columns)
                                //{
                                //    if (myColumn1.ColumnName != "Date")
                                //    {
                                //        if (myColumn1.ColumnName == "Module Name")
                                //        {
                                //            strHTMLBuilder.Append("<th bgcolor='#00B0F0' style='font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-left:none;'>");
                                //            strHTMLBuilder.Append("<h5 align=left><font color=white>" + myColumn1.ColumnName + "</font></h5>");
                                //            strHTMLBuilder.Append("</th>");
                                //        }
                                //        else
                                //        {
                                //            strHTMLBuilder.Append("<th bgcolor='#00B0F0' style='font-size:15px;border-style:solid; border-width:thin;border-left:none;border-bottom:none;'>");
                                //            strHTMLBuilder.Append("<h5 align=left><font color=white>" + myColumn1.ColumnName + "</font></h5>");
                                //            strHTMLBuilder.Append("</th>");
                                //        }
                                //    }
                                //}
                                // strHTMLBuilder.Append("</tr>");





                                int rc = 0;
                                foreach (DataColumn myColumn1 in dtDataM.Columns)
                                {
                                    strHTMLBuilder.Append("<td style='border-style:solid;border-width:thin;border-bottom:none;border-right:none;'>");
                                    strHTMLBuilder.Append("<table cellspacing='0' width='100 %'>");
                                    strHTMLBuilder.Append("<tbody>");



                                    foreach (DataRow mrow1 in dtDataM.Rows)
                                    {
                                        
                                        if (rc == (dtDataM.Rows.Count-1))
                                        {
                                            if (myColumn1.ColumnName != "Date")
                                            {
                                                strHTMLBuilder.Append("<tr>");
                                                strHTMLBuilder.Append("<td style='border-style:solid;border-width:thin;border-bottom:none;border-top:none;border-right:none;border-left:none;'>");
                                                strHTMLBuilder.Append(mrow1[myColumn1.ColumnName].ToString());
                                                strHTMLBuilder.Append("</td>");
                                                strHTMLBuilder.Append("</tr>");
                                                rc = 0;
                                            }
                                        }

                                        else
                                        {

                                            if (myColumn1.ColumnName != "Date")
                                            {
                                                strHTMLBuilder.Append("<tr>");
                                                strHTMLBuilder.Append("<td style='border-style:solid;border-width:thin;border-top:none;border-right:none;border-left:none;'>");
                                                strHTMLBuilder.Append(mrow1[myColumn1.ColumnName].ToString());
                                                strHTMLBuilder.Append("</td>");
                                                strHTMLBuilder.Append("</tr>");
                                                rc++;
                                            }
                                        }

                                    }
                                    strHTMLBuilder.Append("</tbody>");
                                    strHTMLBuilder.Append("</table>");
                                    strHTMLBuilder.Append("</td>");

                                }
                                strHTMLBuilder.Append("</tr>");

                            }
                        }
                    }
                    strHTMLBuilder.Append("</tbody>");
                    strHTMLBuilder.Append("</table>");

                }
            }

        }
        return Convert.ToString(strHTMLBuilder); ;
    }


    private string BIReport()
    {
        DataSet ds = BITSDAL.BIReport("DailyBIReport");
        StringBuilder strHTMLBuilder = new StringBuilder();

        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dtMonth = ds.Tables[0];
             DataTable dtData = ds.Tables[2];
            DataTable dtPrevDaydata = ds.Tables[1];

            if (ds != null && ds.Tables.Count > 0)
            {
                if (dtPrevDaydata != null && dtPrevDaydata.Rows.Count > 0)
                {
                    strHTMLBuilder.Append("<H4 align=left>Summary work report as on:    " + dtData.Rows[0][0].ToString() + " </H4>");
                    strHTMLBuilder.Append("<table width='100%' BorderColor='black' cellspacing='0' cellpadding='5'  style='font-size:15px;border-style:solid; border-width:thin;border-left:none;'>");

                    strHTMLBuilder.Append("<tr bgcolor='#00B0F0'>");
                    //strHTMLBuilder.Append("<td Colspan=" + dtPrevDaydata.Columns.Count + ">");
                    strHTMLBuilder.Append("<td style='border-style:solid;border-top:none;border-right:none;border-bottom:none;border-width:thin;'></td>");
                    strHTMLBuilder.Append("<td style='font-size:15px; border-style:solid;border-width:thin;border-bottom:none;border-top:none;'' Colspan=" + 3 + "><font color=white><center><b>Project<b></center></font></td>");
                    strHTMLBuilder.Append("<td style='font-size:15px' Colspan=" + 6 + "><font color=white><center><b>Hours Consumed<b></center></font></td>");
                    // strHTMLBuilder.Append("<h3>" + Convert.ToString(dtPrevDaydata.Rows[1]["Date"]) + "</h3>");
                    strHTMLBuilder.Append("</td>");
                    strHTMLBuilder.Append("<tr align=left>");
                    //strHTMLBuilder.Append("<tr align=left>");
                    foreach (DataColumn myColumn in dtPrevDaydata.Columns)
                    {
                        if (myColumn.ColumnName != "Date")
                        {
                            if (myColumn.ColumnName == "projId")
                            { }

                            else if (myColumn.ColumnName == "Previous Date")
                            {
                                strHTMLBuilder.Append("<th bgcolor='#00B0F0' style='font-size:15px;border-style:solid; border-width:thin; border-right:none;border-bottom:none;'>");
                                strHTMLBuilder.Append("<h5 align=left><font color=white>" + dtData.Rows[0][0].ToString() + "</font></h5>");
                                strHTMLBuilder.Append("</th>");
                            }
                            else
                            {

                                strHTMLBuilder.Append("<th bgcolor='#00B0F0' style='font-size:15px;border-style:solid; border-width:thin; border-right:none;border-bottom:none;'>");
                                strHTMLBuilder.Append("<h5 align=left><font color=white>" + myColumn.ColumnName + "</font></h5>");
                                strHTMLBuilder.Append("</th>");

                            }
                        }
                    }
                    strHTMLBuilder.Append("</tr>");
                    int projid = 0;
                    foreach (DataRow row in dtPrevDaydata.Rows)
                    {
                        strHTMLBuilder.Append("<tr align=left>");
                        foreach (DataColumn myColumn in dtPrevDaydata.Columns)
                        {

                            if (myColumn.ColumnName != "Date")
                            {
                                if (myColumn.ColumnName == "projId")
                                {
                                    //projid = 0;
                                    projid = Convert.ToInt32(row[myColumn.ColumnName].ToString());

                                }
                                else
                                {
                                    if (myColumn.ColumnName == "Srl. No")
                                    {

                                    }
                                    if (myColumn.ColumnName == "Effort Variance")
                                    {
                                        var Totalhours = row["Total Till Date"].ToString();
                                        var Budgetedhours = row["Budgeted Hours"].ToString();

                                        //if ((Totalhours != "0" || Totalhours != "") && (Budgetedhours != "0" || Budgetedhours != ""))
                                        if (Totalhours != "" && Budgetedhours != "")
                                        {
                                            int total = Convert.ToInt32(Totalhours);
                                            int budgetedHours = Convert.ToInt32(Budgetedhours);
                                            if (total >= budgetedHours)
                                            {
                                                strHTMLBuilder.Append("<td style='font-size:15px;border-style:solid; border-width:thin; border-right:none;'>");
                                                strHTMLBuilder.Append("<font color=red><b>" + row[myColumn.ColumnName].ToString() + "<b></font>");
                                                strHTMLBuilder.Append("</td>");
                                            }
                                            else
                                            {
                                                int num = Convert.ToInt32(row[myColumn.ColumnName].ToString()) * (-1);

                                                strHTMLBuilder.Append("<td  style='font-size:15px;border-style:solid; border-width:thin;border-right:none;'>");

                                                strHTMLBuilder.Append(num);
                                                strHTMLBuilder.Append("</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (Totalhours != "0" || Totalhours != "" && Budgetedhours == "0" || Budgetedhours == "")
                                            {
                                                strHTMLBuilder.Append("<td style='font-size:15px;border-style:solid; border-width:thin;border-right:none;'>");
                                                strHTMLBuilder.Append("<font color=red><b>" + row[myColumn.ColumnName].ToString() + "</b></font>");
                                                strHTMLBuilder.Append("</td>");
                                            }
                                            else
                                            {
                                                strHTMLBuilder.Append("<td  style='font-size:15px;border-style:solid; border-width:thin;border-right:none;'>");
                                                strHTMLBuilder.Append(row[myColumn.ColumnName].ToString());
                                                strHTMLBuilder.Append("</td>");
                                            }
                                        }
                                    }
                                    //int Total = dtPrevDaydata.Rows[0][].ToString();


                                    else
                                    {

                                        strHTMLBuilder.Append("<td style='font-size:15px;border-style:solid; border-width:thin;border-right:none;border-bottom:none;'>");
                                        strHTMLBuilder.Append(row[myColumn.ColumnName].ToString());
                                        strHTMLBuilder.Append("</td>");
                                    }
                                }
                            }

                        }
                        strHTMLBuilder.Append("</tr>");
                        strHTMLBuilder.Append("<tr>");



                        //strHTMLBuilder.Append("<table width='100%' BorderColor='black' cellspacing='0' cellpadding='5'  style='font-size:15px;border-style:solid;border-width:thin;'>");
                        DataSet dsM = BITSDAL.getDetailReportbyProjId("GetReportByProjId", projid);
                        DataTable dtDataM = dsM.Tables[0];
                        

                        int rowcount = dtDataM.Rows.Count + 1;
                        strHTMLBuilder.Append("<td Colspan=" + 3 + " rowspan=" + rowcount + " style='font-size:15px;border-style:solid; border-width:thin;border-bottom:none;'>");
                        //strHTMLBuilder.Append("<th Colspan=" + 3 + " rowspan="+ rowcount + " style='font-size:15px;border-style:solid; border-width:thin;border-right:none;></th>");

                        foreach (DataColumn myColumn in dtDataM.Columns)
                        {
                            if (myColumn.ColumnName != "Date")
                            {
                                if (myColumn.ColumnName == "Module Name")
                                {
                                    strHTMLBuilder.Append("<th bgcolor='#00B0F0' style='font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-left:none;'>");
                                    strHTMLBuilder.Append("<h5 align=left><font color=white>" + myColumn.ColumnName + "</font></h5>");
                                    strHTMLBuilder.Append("</th>");
                                }
                                else
                                if (myColumn.ColumnName == "Previous Date")
                                {
                                        strHTMLBuilder.Append("<th bgcolor='#00B0F0' style='font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-left:none;'>");
                                        strHTMLBuilder.Append("<h5 align=left><font color=white>" + dtData.Rows[0][0].ToString() + "</font></h5>");
                                        strHTMLBuilder.Append("</th>");
                                 
                                }
                                else
                                {
                                    strHTMLBuilder.Append("<th bgcolor='#00B0F0' style='font-size:15px;border-style:solid; border-width:thin;border-left:none;border-bottom:none;'>");
                                    strHTMLBuilder.Append("<h5 align=left><font color=white>" + myColumn.ColumnName + "</font></h5>");
                                    strHTMLBuilder.Append("</th>");
                                }
                            }
                        }
                        strHTMLBuilder.Append("</tr>");

                        foreach (DataRow mrow in dtDataM.Rows)
                        {

                            strHTMLBuilder.Append("<tr align=left>");
                            //strHTMLBuilder.Append("<td style='font-size:15px;border-style:solid;border-width:thin;border-left:none;border-bottom:none;'>");
                            //strHTMLBuilder.Append("</td>");
                            //strHTMLBuilder.Append("<td style='font-size:15px;border-style:solid;border-width:thin;></td>");
                            foreach (DataColumn myColumn in dtDataM.Columns)
                            {


                                if (myColumn.ColumnName != "Date")
                                {

                                    strHTMLBuilder.Append("<td style='font-size:15px;border-style:solid;border-width:thin;border-left:none;border-bottom:none;'>");
                                    strHTMLBuilder.Append(mrow[myColumn.ColumnName].ToString());
                                    strHTMLBuilder.Append("</td>");

                                }
                            }
                            strHTMLBuilder.Append("</tr>");
                        }


                        if (dtDataM.Rows.Count > 0)
                            strHTMLBuilder.Append("</td>");
                        strHTMLBuilder.Append("</tr>");




                        strHTMLBuilder.Append("</td>");
                        strHTMLBuilder.Append("</tr>");

                    }


                    if (dtPrevDaydata.Rows.Count > 0)
                        strHTMLBuilder.Append("</table></BR>");
                }
            }


            //if (dtMonth != null && dtMonth.Rows.Count > 0)
            //{
            //    strHTMLBuilder.Append("<H4 align=left>Detailed work report as on: " + DateTime.Today.AddDays(-1).ToString("dd-MMM-yy") + " </H4>");
            //    strHTMLBuilder.Append("<style>td:last-child{border-right: none;}</style>");
            //    strHTMLBuilder.Append("<table width='100%' BorderColor='black' cellspacing='0' cellpadding='5'  style='font-size:15px;border-style:solid;border-width:thin;border-left:none;'>");

            //    foreach (DataColumn myColumn in dtData.Columns)
            //    {
            //        if (myColumn.ColumnName != "Date")
            //        {
            //            strHTMLBuilder.Append("<th bgcolor='#00B0F0' style='font-size:15px;border-style:solid; border-width:thin;border-right:none;border-bottom:none;border-top:none;'>");
            //            strHTMLBuilder.Append("<h5 align=left><font color=white>" + myColumn.ColumnName + "</font></h5>");
            //            strHTMLBuilder.Append("</th>");
            //        }
            //    }
            //    strHTMLBuilder.Append("</tr>");

            //    foreach (DataRow row in dtData.Rows)
            //    {

            //        strHTMLBuilder.Append("<tr align=left>");
            //        foreach (DataColumn myColumn in dtData.Columns)
            //        {


            //            if (myColumn.ColumnName != "Date")
            //            {

            //                strHTMLBuilder.Append("<td style='font-size:15px;border-style:solid;border-width:thin;border-bottom:none;border-right:none;'>");
            //                strHTMLBuilder.Append(row[myColumn.ColumnName].ToString());
            //                strHTMLBuilder.Append("</td>");

            //            }
            //        }
            //        strHTMLBuilder.Append("</tr>");
            //    }


            //    if (dtData.Rows.Count > 0)
            //        strHTMLBuilder.Append("</table></BR>");
            //}

        }
        return Convert.ToString(strHTMLBuilder);

    }




    private string NoWork()
    {
        DataSet ds = TimeSheetDAL.NoWork("NOWORK");
        StringBuilder strHTMLBuilder = new StringBuilder();
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dtName = ds.Tables[0];
            DataTable dtData = ds.Tables[1];
            if (dtName != null && dtName.Rows.Count > 0)
            {
                strHTMLBuilder.Append("<div><div><H3><u>No Work Report: </u></H3></div><div style='margin: -40px 0px 0px 600px;'></div></div><br><br>");
                //strHTMLBuilder.Append("<div><div><H3><u>No Work Report: </u></H3></div><div style='margin: -40px 0px 0px 600px;'><H3>Total Hours: " + Convert.ToString(dtName.Rows[0]["TotalHours"]) + "</H3></div></div>");

                for (int i = 0; i < dtName.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        strHTMLBuilder.Append("<table width='100%' border='0.1em' cellspacing='0' cellpadding='5'  style='font-size:15px border: 0.1px solid black;'>");


                        strHTMLBuilder.Append("<tr align=left>");
                        strHTMLBuilder.Append("<td Colspan=2>");
                        strHTMLBuilder.Append("<h3> Total Hours: " + Convert.ToString(dtName.Rows[0]["TotalHours"]) + "</h3>");
                        strHTMLBuilder.Append("</td>");
                        strHTMLBuilder.Append("</tr>");

                        strHTMLBuilder.Append("<tr align=left>");

                        foreach (DataColumn myColumn in dtData.Columns)
                        {


                            if (myColumn.ColumnName != "Name")
                            {
                                strHTMLBuilder.Append("<td>");
                                strHTMLBuilder.Append("<h3>" + myColumn.ColumnName + "</h3>");
                                strHTMLBuilder.Append("</td>");
                            }
                        }
                        strHTMLBuilder.Append("</tr>");
                    }

                    strHTMLBuilder.Append("<tr>");
                    strHTMLBuilder.Append("<td Colspan=" + dtData.Columns.Count + ">");
                    strHTMLBuilder.Append("<h3>" + Convert.ToString(dtName.Rows[i]["Name"]) + "</h3>");
                    strHTMLBuilder.Append("</td>");
                    strHTMLBuilder.Append("<tr align=left>");

                    DataRow[] drlist = dtData.Rows.Cast<DataRow>().Where(r1 => Convert.ToString(r1["Name"]).Equals(Convert.ToString(dtName.Rows[i]["Name"]))).ToArray();

                    if (drlist.Count() > 0)
                    {
                        foreach (DataRow myRow in drlist)
                        {
                            strHTMLBuilder.Append("<tr align=left>");
                            foreach (DataColumn myColumn in dtData.Columns)
                            {
                                if (myColumn.ColumnName != "Name")
                                {
                                    strHTMLBuilder.Append("<td >");
                                    strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
                                    strHTMLBuilder.Append("</td>");
                                }
                            }
                            strHTMLBuilder.Append("</tr>");
                        }
                    }
                }
                if (dtName.Rows.Count > 0)
                    strHTMLBuilder.Append("</table>");
            }
        }
        return Convert.ToString(strHTMLBuilder);
    }
}






