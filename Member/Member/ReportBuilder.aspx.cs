using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using CSCode;
using System.Text;
using System.Collections;


public partial class Member_ReportBuilder : Authentication
{
    UserMaster UM;

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();

        if (!IsPostBack)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "GetDetails();", true);

        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Session["ChartType"] = dlstChartType.SelectedValue;
        Report report = new Report();
        int fieldCount = 0;
        fieldCount = Convert.ToInt32(hdnFieldCount.Value);
        int ReportId = 0;
        string mode = "Insert";
        string MgmtReport = rdbMgmtReport.SelectedValue;
        string chartType = dlstChartType.SelectedValue;
        if (Convert.ToInt32(hdnReportId.Value) != 0)
        {
            ReportId = Convert.ToInt32(hdnReportId.Value);
            mode = "Update";
        }

        //Field Values
        string field1 = "";
        string field2 = "";
        string field3 = "";
        string field4 = "";
        string field5 = "";

        //Type DropDown
        string type1 = "";
        string type2 = "";
        string type3 = "";
        string type4 = "";
        string type5 = "";

        if (fieldCount >= 1)
        {
            type1 = Request.Form["ddltype_1"].ToString();
            if (type1 == "TEXT")
            {
                //TextBox
                field1 = type1 + ":" + Request.Form["text1"];
            }
            else
            {
                //SQL/CSV
                field1 = type1 + ":" + Request.Form["textarea1"];
            }
        }
        if (fieldCount >= 2)
        {
            type2 = Request.Form["ddltype_2"].ToString();
            if (type2 == "TEXT")
            {
                //TextBox
                field2 = type2 + ":" + Request.Form["text2"];
            }
            else
            {
                //SQL
                field2 = type2 + ":" + Request.Form["textarea2"];
            }
        }
        if (fieldCount >= 3)
        {
            type3 = Request.Form["ddltype_3"].ToString();
            if (type3 == "TEXT")
            {
                //TextBox
                field3 = type3 + ":" + Request.Form["text3"];
            }
            else
            {
                //SQL
                field3 = type3 + ":" + Request.Form["textarea3"];
            }
        }
        if (fieldCount >= 4)
        {
            type4 = Request.Form["ddltype_4"].ToString();
            if (type4 == "TEXT")
            {
                //TextBox
                field4 = type4 + ":" + Request.Form["text4"];
            }
            else
            {
                //SQL
                field4 = type4 + ":" + Request.Form["textarea4"];
            }
        }
        if (fieldCount >= 5)
        {
            type5 = Request.Form["ddltype_5"].ToString();
            if (type5 == "TEXT")
            {
                //TextBox
                field5 = type5 + ":" + Request.Form["text5"];
            }
            else
            {
                //SQL
                field5 = type5 + ":" + Request.Form["textarea5"];
            }
        }

        int output = Report.SaveReport(ReportId, txtName.Text,field1, field2, field3, field4, field5, txtQuery.Text, UM.EmployeeID, Global.GetLocalIPAddress(), mode, txtDescription.Text, MgmtReport, chartType);
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "GetDetails();", true);
        Response.Redirect("/Member/ReportBuilder.aspx");
    }

    protected void btnReportDetails_Click(object sender, EventArgs e)
    {
        BindreportDetails(hdnReportId.Value);
    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        BindDisplayGridwithInputsRun();
    }

    protected void btnRunReport_Click(object sender, EventArgs e)
    {
        BindRunReportDetails(hdnReportId.Value);
    }

    protected void btnDeleteReport_Click(object sender, EventArgs e)
    {
        int Id = Report.DeleteReport("Delete", Convert.ToInt32(hdnReportId.Value));
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "GetDetails();", true);
    }

    public void BindreportDetails(string ReportId)
    {
        //Edit

        List<Report> report = new List<Report>();
        report = Report.GetReportList("SelectWrtReportId", Convert.ToInt32(ReportId));
        if (report != null && report.Count == 1)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Index", typeof(int));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("TextArea", typeof(string));

            int fieldcount = 0;
            if (Convert.ToString(report[0].field1).Trim() != string.Empty)
            {
                fieldcount++;
            }
            if (Convert.ToString(report[0].field2).Trim() != string.Empty)
            {
                fieldcount++;
            }
            if (Convert.ToString(report[0].field3).Trim() != string.Empty)
            {
                fieldcount++;
            }
            if (Convert.ToString(report[0].field4).Trim() != string.Empty)
            {
                fieldcount++;
            }
            if (Convert.ToString(report[0].field5).Trim() != string.Empty)
            {
                fieldcount++;
            }
            hdnFieldCount.Value = Convert.ToString(fieldcount);

            lblReportNameBui.Text = "";
            divField.Visible = true;
            txtName.Text = report[0].name;
            txtDescription.Text = report[0].Description;
            rdbMgmtReport.SelectedValue = report[0].MgmtReport;

            dlstChartType.SelectedValue = report[0].chartType;
            string controlsbody = "";
            txtQuery.Text = report[0].query;

            controlsbody = "<table width=\"82%\" style=\"overflow: hidden;\">";
            if (fieldcount >= 1)
            {
                string strType = Convert.ToString(report[0].field1).Split(':')[0];
                string strData = Convert.ToString(report[0].field1).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 1;
                dr["Type"] = strType;



                string ddlid = "ddltype_1";

                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<tr><td>Parameter 1:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea1' name='textarea1' rows='3' cols='40' style='display:none'></textarea></td><td style='float:right;'><input type='text' id='text1' name='text1' value='' /></td></tr>";
                    dr["Text"] = Convert.ToString(strData);
                }
                else if (strType == "SQL")//SQL
                {
                    controlsbody += "<tr><td>Parameter 1:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea1' name='textarea1' rows='3' cols='40' >" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text1' name='text1' value='' style='display:none;'/></td></tr>";
                    dr["TextArea"] = Convert.ToString(strData);
                }
                else if (strType == "CSV")//CSV
                {
                    controlsbody += "<tr><td>Parameter 1:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea1' name='textarea1' rows='3' cols='40' >" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text1' name='text1' value='' style='display:none'/></td></tr>";
                    dr["TextArea"] = Convert.ToString(strData);
                }

                dt.Rows.Add(dr);
            }
            if (fieldcount >= 2)
            {

                string strType = Convert.ToString(report[0].field2).Split(':')[0];
                string strData = Convert.ToString(report[0].field2).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 2;
                dr["Type"] = Convert.ToString(strType);

                string ddlid = "ddltype_2";

                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<tr><td>Parameter 2:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea2' name='textarea2' rows='3' cols='40' style='display:none'></textarea></td><td style='float:right;'><input type='text' id='text2' name='text2' value='' /></td></tr>";
                    dr["Text"] = Convert.ToString(strData);
                }
                else if (strType == "SQL")//SQL                                                                                                     
                {
                    controlsbody += "<tr><td>Parameter 2:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea2' name='textarea2' rows='3' cols='40' >" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text2' name='text2' value='' style='display:none;'/></td></tr>";
                    dr["TextArea"] = Convert.ToString(strData);
                }
                else if (strType == "CSV")//CSV                                                                                                      
                {
                    controlsbody += "<tr><td>Parameter 2:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea2' name='textarea2' rows='3' cols='40'>" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text2' name='text2' value='' style='display:none'/></td></tr>";
                    dr["TextArea"] = Convert.ToString(strData);
                }

                dt.Rows.Add(dr);
            }
            if (fieldcount >= 3)
            {
                string strType = Convert.ToString(report[0].field3).Split(':')[0];
                string strData = Convert.ToString(report[0].field3).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 3;
                dr["Type"] = strType;

                string ddlid = "ddltype_3";

                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<tr><td>Parameter 3:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea3' name='textarea3' rows='3' cols='40' style='display:none'></textarea></td><td style='float:right;'><input type='text' id='text3' name='text3' value='' /></td></tr>";
                    dr["Text"] = strData;
                }
                else if (strType == "SQL")//SQL                                                                                
                {
                    controlsbody += "<tr><td>Parameter 3:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea3' name='textarea3' rows='3' cols='40' >" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text3' name='text3' value='' style='display:none;'/></td></tr>";
                    dr["TextArea"] = strData;

                }
                else if (strType == "CSV")//CSV                                                                                
                {
                    controlsbody += "<tr><td>Parameter 3:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea3' name='textarea3' rows='3' cols='40' >" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text3' name='text3' value='' style='display:none'/></td></tr>";
                    dr["TextArea"] = strData;

                }
                dt.Rows.Add(dr);
            }
            if (fieldcount >= 4)
            {
                string strType = Convert.ToString(report[0].field4).Split(':')[0];
                string strData = Convert.ToString(report[0].field4).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 4;
                dr["Type"] = strType;

                string ddlid = "ddltype_4";

                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<tr><td>Parameter 4:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea4' name='textarea4' rows='3' cols='40' style='display:none'></textarea></td><td style='float:right;'><input type='text' id='text4' name='text4' value='' /></td></tr>";
                    dr["Text"] = strData;
                }
                else if (strType == "SQL")//SQL                                                                                                     
                {
                    controlsbody += "<tr><td>Parameter 4:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea4' name='textarea4' rows='3' cols='40' >" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text4' name='text4' value='' style='display:none;'/></td></tr>";
                    dr["TextArea"] = strData;
                }
                else if (strType == "CSV")//CSV                                                                                                     
                {
                    controlsbody += "<tr><td>Parameter 4:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea4' name='textarea4' rows='3' cols='40' >" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text4' name='text4' value='' style='display:none'/></td></tr>";
                    dr["TextArea"] = strData;
                }
                dt.Rows.Add(dr);
            }

            if (fieldcount >= 5)
            {
                string strType = Convert.ToString(report[0].field5).Split(':')[0];
                string strData = Convert.ToString(report[0].field5).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 5;
                dr["Type"] = strType;

                string ddlid = "ddltype_5";

                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<tr><td>Parameter 5:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea5' name='textarea5' rows='3' cols='40' style='display:none'></textarea></td><td style='float:right;'><input type='text' id='text5' name='text5' value='' /></td></tr>";
                    dr["Text"] = strData;
                }
                else if (strType == "SQL")//SQL   
                {
                    controlsbody += "<tr><td>Parameter 5:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea5' name='textarea5' rows='3' cols='40' >" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text5' name='text5' value='' style='display:none'/></td></tr>";
                    dr["TextArea"] = strData;

                }
                else if (strType == "CSV")//CSV                                                                                                      
                {
                    controlsbody += "<tr><td>Parameter 5:</td><td><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);'><option value='TEXT'>Textbox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea5' name='textarea5' rows='3' cols='40' >" + strData + "</textarea></td><td style='float:right;'><input type='text' id='text5' name='text5' value='' style='display:none'/></td></tr>";
                    dr["TextArea"] = strData;
                }

                dt.Rows.Add(dr);

            }
            dt.AcceptChanges();

            controlsbody += "</table>";
            ControlsDiv.InnerHtml = controlsbody;
            RunControlsDiv.InnerHtml = string.Empty;
            ltchartRun.Text = string.Empty;
            string data = ConvertDataTabletoString(dt);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "AddReport(); setSelectedDdlvalue();EnableChart();GenerateControls(" + data + ");", true);//GetLogData(" + ReportData + "); 
        }
    }

    public void BindDisplayGridwithInputsRun()
    {

        int fieldcount = 0;
        fieldcount = Convert.ToInt32(hdnFieldCount.Value);
        string strQuery = txtrunquery.Text.Trim();
        string controlsbody = "";
        lblReportNameBui.Text = " - " + txtReportName.Text;
        controlsbody = "<table width=\"100%\" style=\"overflow: hidden;\">";
        string chartType = Convert.ToString(hdnChartType.Value);
        string strType = "";

        DataTable dt = new DataTable();
        dt.Columns.Add("Index", typeof(int));
        dt.Columns.Add("Type", typeof(string));
        dt.Columns.Add("Text", typeof(string));
        dt.Columns.Add("TextArea", typeof(string));
        dt.Columns.Add("Result", typeof(string));

        for (int j = 1; j <= fieldcount; j++)
        {
            DataRow dr = dt.NewRow();

            strType = Request.Form["hdnType_" + j.ToString()].ToString();
            dr["Index"] = j;
            dr["Type"] = strType;

            string strField = "";

            if (strType == "TEXT")
            {
                //TextBox
                strField = Request.Form["text" + j.ToString()];
                dr["Text"] = strField;
            }
            else if (strType == "SQL")
            {
                //SQL
                strField = Request.Form["ddlresult_" + j.ToString()];
                dr["TextArea"] = Request.Form["textarea" + j.ToString()];
            }
            else if (strType == "CSV")
            {
                //CSV
                strField = Request.Form["ddlresult_" + j.ToString()];
                dr["textarea"] = Request.Form["textarea" + j.ToString()]; ;
            }

            strQuery = strQuery.Replace("{F" + j.ToString() + "}", strField);


            dr["Result"] = Request.Form["ddlresult_" + j.ToString()];

            dt.Rows.Add(dr);
        }
        dt.AcceptChanges();
        controlsbody += "</table>";
        ltchartRun.Text = string.Empty;

        string ReportData = Report.GetRecord(strQuery);

        if (chartType == "Single_line_chart")
        {
            Linechart_bind(strQuery, "chartrun");
        }
        else if (chartType == "Pie_Chart")
        {
            Piechart_bind(strQuery, "chartrun");
        }
        else if (chartType == "Bar_Chart")
        {
            Barchart_bind(strQuery, "chartrun");
        }
        else if (chartType == "twobythree_line_chart")
        {
            twoBythreechart_bind(strQuery, "chartrun");
        }

        string data = ConvertDataTabletoString(dt);
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "GetLogData(" + ReportData + ");AddRunReport(); EnableChartRun();GenerateReportControls(" + data + ");", true);//hide();
    }

    public void Linechart_bind(string Query, string type)
    {
        StringBuilder str = new StringBuilder();
        DataTable Dt = new DataTable();
        Dt = Report.GetRecordDT(Query);


        if (Dt.Rows.Count > 0)
        {
            str.Append(@"<script type=*text/javascript*> 
                google.load( *visualization*, *1.1*, {packages:[*corechart*]});
                google.setOnLoadCallback(drawChart);
                function drawChart() {
                var data = new google.visualization.DataTable();
                data.addColumn('string', '');
                data.addColumn('number', '');
                data.addColumn({type: 'string', role: 'tooltip' ,'p': {'html': true}} );");
            str.Append("data.addRows([");
            for (int i = 0; i <= Dt.Rows.Count - 1; i++)
            {
                str.Append("['" + Dt.Rows[i][0].ToString() + "'," + Dt.Rows[i][1].ToString().Replace(",", "") + ",'<b>" + Dt.Rows[i][0].ToString() + "</b> <br/> " + Dt.Rows[i][1].ToString() + "'],");
            }
            str.Append("]);");
            str.Append(" var options = {tooltip: {isHtml: true}, width: 660, height: 300}; ");
            if (type == "chart")
                str.Append("   var chart = new google.visualization.LineChart(document.getElementById('chart_div'));");

            else
                str.Append("   var chart = new google.visualization.LineChart(document.getElementById('chart_div_run'));");
            str.Append(" chart.draw(data, options,{width: 660, height: 300}); }");
            str.Append("</script>");
            if (type == "chart")
            {
                //ltchart.Text = str.ToString().Replace('*', '"');
            }
            else
            {
                ltchartRun.Text = str.ToString().Replace('*', '"');
            }
        }
    }

    public void Barchart_bind(string Query, string type)
    {
        StringBuilder str = new StringBuilder();
        DataTable Dt = new DataTable();
        Dt = Report.GetRecordDT(Query);


        if (Dt.Rows.Count > 0)
        {

            str.Append(@"<script type=*text/javascript*> 
                google.load( *visualization*, *1.1*, {packages:[*corechart*]});
                google.setOnLoadCallback(drawChart);
                function drawChart() {
                var data = new google.visualization.DataTable();
                data.addColumn('string', '');
                data.addColumn('number', '');
                data.addColumn({type: 'string', role: 'tooltip' ,'p': {'html': true}} );");
            str.Append("data.addRows([");
            for (int i = 0; i <= Dt.Rows.Count - 1; i++)
            {
                str.Append("['" + Dt.Rows[i][0].ToString() + "'," + Dt.Rows[i][1].ToString().Replace(",", "") + ",'<b>" + Dt.Rows[i][0].ToString() + "</b> <br/> " + Dt.Rows[i][1].ToString() + "'],");
            }
            str.Append("]);");
            str.Append(" var options = {tooltip: {isHtml: true}, width: 660, height: 300}; ");
            if (type == "chart")

                str.Append("   var chart = new google.visualization.BarChart(document.getElementById('chart_div'));");
            else
                str.Append("   var chart = new google.visualization.BarChart(document.getElementById('chart_div_run'));");
            str.Append(" chart.draw(data, options,{width: 660, height: 300}); }");
            str.Append("</script>");
            if (type == "chart")
            {
                //ltchart.Text = str.ToString().Replace('*', '"');
            }
            else
            {
                ltchartRun.Text = str.ToString().Replace('*', '"');
            }
        }
    }

    public void Piechart_bind(string Query, string type)
    {
        StringBuilder str = new StringBuilder();
        DataTable Dt = new DataTable();
        Dt = Report.GetRecordDT(Query);


        if (Dt.Rows.Count > 0)
        {
            str.Append(@"<script type=*text/javascript*> 
                google.load( *visualization*, *1.1*, {packages:[*corechart*]});
                google.setOnLoadCallback(drawChart);
                function drawChart() {
                var data = new google.visualization.DataTable();
                data.addColumn('string', '');
                data.addColumn('number', '');
                data.addColumn({type: 'string', role: 'tooltip' ,'p': {'html': true}} );");
            str.Append("data.addRows([");
            for (int i = 0; i <= Dt.Rows.Count - 1; i++)
            {
                str.Append("['" + Dt.Rows[i][0].ToString() + "'," + Dt.Rows[i][1].ToString().Replace(",", "") + ",'<b>" + Dt.Rows[i][0].ToString() + "</b> <br/> " + Dt.Rows[i][1].ToString() + "'],");
            }
            str.Append("]);");
            str.Append(" var options = {tooltip: {isHtml: true}, width: 660, height: 300}; ");
            if (type == "chart")

                str.Append("   var chart = new google.visualization.PieChart(document.getElementById('chart_div'));");
            else
                str.Append("   var chart = new google.visualization.PieChart(document.getElementById('chart_div_run'));");
            str.Append(" chart.draw(data, options,{width: 660, height: 300}); }");
            str.Append("</script>");
            if (type == "chart")
            {
                //ltchart.Text = str.ToString().Replace('*', '"');
            }
            else
            {
                ltchartRun.Text = str.ToString().Replace('*', '"');
            }
        }
    }

    public void twoBythreechart_bind(string Query, string type)
    {

        StringBuilder str = new StringBuilder();
        DataTable Dt = new DataTable();
        Dt = Report.GetRecordDT(Query);


        if (Dt.Rows.Count > 0)
        {
            str.Append(@"<script type='text/javascript'> 
                google.load( 'visualization', '1.1', {packages:['line']});
                google.setOnLoadCallback(drawChart);
                function drawChart() {
                var data = new google.visualization.DataTable(); 
                data.addColumn('string', '');");

            //For 3 columns only.
            int count = (Dt.Columns.Count >= 4) ? 4 : Dt.Columns.Count;

            for (int i = 1; i < count; i++)
            {
                str.Append(" data.addColumn('number', '" + Dt.Columns[i].ColumnName + "');");
            }
            str.Append("data.addColumn({type: 'string', role: 'tooltip' ,'p': {'html': true}} );data.addRows([");


            for (int i = 0; i <= Dt.Rows.Count - 1; i++)
            {
                str.Append("['" + Dt.Rows[i][0].ToString() + "',");
                for (int j = 1; j < count; j++)
                {
                    str.Append("" + Dt.Rows[i][j].ToString().Replace(",", "") + "," + "");
                }
                str.Append("'<b>" + Dt.Rows[i][0].ToString() + "</b> <br/> " + Dt.Rows[i][1].ToString() + "'],");
            }

            str.Append("]);");

            str.Append(" var options = {tooltip: {isHtml: true}, width: 900, height: 500}; ");
            if (type == "chart")
                str.Append(" var chart = new google.charts.Line(document.getElementById('chart_div'));");

            else
                str.Append(" var chart = new google.charts.Line(document.getElementById('chart_div_run'));");
            str.Append(" chart.draw(data, options); }");
            str.Append("</script>");
            if (type == "chart")
            {
                //ltchart.Text = str.ToString().Replace('*', '"');
            }
            else
            {
                ltchartRun.Text = str.ToString().Replace('*', '"');
            }
        }

    }

    public string ConvertDataTabletoString(DataTable dtData)
    {
        DataTable dt = dtData;
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }


    [System.Web.Services.WebMethod]
    public static List<clsKeyValuePair> BindDropDown(string Qry)
    {
        try
        {
            List<clsKeyValuePair> list = Report.GetRecordsForDD(Qry);
            return list;
        }
        catch (Exception ex)
        {

            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String BindReport()
    {
        try
        {
            List<Report> listReport = Report.GetReportList("SelectAll", 0);
            var data = from ReportItem in listReport
                       select new
                       {
                           ReportItem.reportId,
                           ReportItem.name,
                          // ReportItem.numberOfField,
                           ReportItem.field1,
                           ReportItem.field2,
                           ReportItem.field3,
                           ReportItem.field4,
                           ReportItem.field5,
                           ReportItem.query,
                           ReportItem.insertedOn,
                           ReportItem.Description,
                           ReportItem.MgmtReport
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {

            return null;
        }
    }

    public void BindRunReportDetails(string ReportId)
    {
        List<Report> report = new List<Report>();
        report = Report.GetReportList("SelectWrtReportId", Convert.ToInt32(ReportId));
        if (report != null && report.Count == 1)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Index", typeof(int));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("TextArea", typeof(string));

            int fieldcount = 0;
            if (Convert.ToString(report[0].field1).Trim() != string.Empty)
            {
                fieldcount++;
            }
            if (Convert.ToString(report[0].field2).Trim() != string.Empty)
            {
                fieldcount++;
            }
            if (Convert.ToString(report[0].field3).Trim() != string.Empty)
            {
                fieldcount++;
            }
            if (Convert.ToString(report[0].field4).Trim() != string.Empty)
            {
                fieldcount++;
            }
            if (Convert.ToString(report[0].field5).Trim() != string.Empty)
            {
                fieldcount++;
            }
            hdnFieldCount.Value = Convert.ToString(fieldcount);

            divRun.Visible = true;
            lblReportNameBui.Text = " - " + report[0].name;
            txtName.Text = report[0].name;
           
            hdnChartType.Value = Convert.ToString(report[0].chartType);
            string chartType = Convert.ToString(report[0].chartType);
            if (fieldcount == 0)
            {
                //myImage.Visible = false;
                btnRun.Visible = false;
            }
            else
            {
                //myImage.Visible = true;
                btnRun.Visible = true;
            }

            string controlsbody = "";
            txtrunquery.Text = report[0].query;
            txtReportName.Text = report[0].name;

            string strQuery = report[0].query;
            controlsbody = "<table width=\"100%\" style=\"overflow: hidden;\"><tr>";

            if (fieldcount >= 1)
            {

                string strType = Convert.ToString(report[0].field1).Split(':')[0];
                string strData = Convert.ToString(report[0].field1).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 1;
                dr["Type"] = strType;

                strQuery = strQuery.Replace("{F1}", report[0].field1);

                string ddlid = "ddltype_1";
                string ddlresult = "ddlresult_1";
                string hdnType = "hdnType_1";
                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<td>Parameter 1:</td> <td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "' type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea1' name='textarea1' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text1' name='text1' value='" + strData + "' /></td>";
                    dr["Text"] = strData;
                }
                else if (strType == "SQL")//SQL
                {
                    controlsbody += "<td>Parameter 1:</td><td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea1' name='textarea1' rows='3' cols='40' style='display:none;'>" + strData + "</textarea><input type='text' id='text1' name='text1' value='' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }
                else if (strType == "CSV")//CSV
                {
                    controlsbody += "<td>Parameter 1:</td><td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea1' name='textarea1' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text1' name='text1' value='" + strData + "' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }

                dt.Rows.Add(dr);
            }
            if (fieldcount >= 2)
            {

                string strType = Convert.ToString(report[0].field2).Split(':')[0];
                string strData = Convert.ToString(report[0].field2).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 2;
                dr["Type"] = strType;

                strQuery = strQuery.Replace("{F1}", report[0].field2);
                string ddlid = "ddltype_2";
                string ddlresult = "ddlresult_2";
                string hdnType = "hdnType_2";

                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<td>Parameter 2:</td> <td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea2' name='textarea2' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text2' name='text2' value='" + strData + "' /></td>";
                    dr["Text"] = strData;
                }
                else if (strType == "SQL")//SQL
                {
                    controlsbody += "<td>Parameter 2:</td><td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea2' name='textarea2' rows='3' cols='40' style='display:none;'>" + strData + "</textarea><input type='text' id='text2' name='text2' value='' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }
                else if (strType == "CSV")//CSV
                {
                    controlsbody += "<td>Parameter 2:</td><td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea2' name='textarea2' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text2' name='text2' value='" + strData + "' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }

                dt.Rows.Add(dr);
            }
            if (fieldcount >= 3)
            {

                string strType = Convert.ToString(report[0].field3).Split(':')[0];
                string strData = Convert.ToString(report[0].field3).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 3;
                dr["Type"] = strType;

                strQuery = strQuery.Replace("{F1}", report[0].field3);
                string ddlid = "ddltype_3";
                string ddlresult = "ddlresult_3";
                string hdnType = "hdnType_3";
                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<td>Parameter 3:</td> <td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea3' name='textarea3' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text3' name='text3' value='" + strData + "' /></td>";
                    dr["Text"] = strData;
                }
                else if (strType == "SQL")//SQL
                {
                    controlsbody += "<td>Parameter 3:</td><td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea3' name='textarea3' rows='3' cols='40' style='display:none;'>" + strData + "</textarea><input type='text' id='text3' name='text3' value='' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }
                else if (strType == "CSV")//CSV
                {
                    controlsbody += "<td>Parameter 3:</td><td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea3' name='textarea3' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text3' name='text3' value='" + strData + "' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }

                dt.Rows.Add(dr);
            }
            if (fieldcount >= 4)
            {

                string strType = Convert.ToString(report[0].field4).Split(':')[0];
                string strData = Convert.ToString(report[0].field4).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 4;
                dr["Type"] = strType;

                strQuery = strQuery.Replace("{F1}", report[0].field4);
                string ddlid = "ddltype_4";
                string ddlresult = "ddlresult_4";
                string hdnType = "hdnType_4";
                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<td>Parameter 4:</td> <td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea4' name='textarea4' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text4' name='text4' value='" + strData + "' /></td>";
                    dr["Text"] = strData;
                }
                else if (strType == "SQL")//SQL
                {
                    controlsbody += "<td>Parameter 4:</td><td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea4' name='textarea4' rows='3' cols='40' style='display:none;'>" + strData + "</textarea><input type='text' id='text4' name='text4' value='' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }
                else if (strType == "CSV")//CSV
                {
                    controlsbody += "<td>Parameter 4:</td><td style='float:right;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea4' name='textarea4' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text4' name='text4' value='" + strData + "' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }

                dt.Rows.Add(dr);
            }
            if (fieldcount >= 5)
            {

                string strType = Convert.ToString(report[0].field5).Split(':')[0];
                string strData = Convert.ToString(report[0].field5).Split(':')[1];

                DataRow dr = dt.NewRow();
                dr["Index"] = 5;
                dr["Type"] = strType;

                strQuery = strQuery.Replace("{F1}", report[0].field5);
                string ddlid = "ddltype_5";
                string ddlresult = "ddlresult_5";
                string hdnType = "hdnType_5";
                if (strType == "TEXT")//TextBox
                {
                    controlsbody += "<td>Parameter 5:</td> <td style='float:left;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea5' name='textarea5' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text5' name='text5' value='" + strData + "' /></td>";
                    dr["Text"] = strData;
                }
                else if (strType == "SQL")//SQL
                {
                    controlsbody += "<td>Parameter 5:</td><td style='float:left;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea5' name='textarea5' rows='3' cols='40' style='display:none;'>" + strData + "</textarea><input type='text' id='text5' name='text5' value='' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }
                else if (strType == "CSV")//CSV
                {
                    controlsbody += "<td>Parameter 5:</td><td style='float:left;'><input id='" + hdnType + "' name='" + hdnType + "'  type='hidden'  value='" + strType + "' /><select Id='" + ddlid + "' name='" + ddlid + "' value='" + strType + "' onchange='ddltypeChange(this);' style='display:none;'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV3'>CSV Statement</option></select><textarea id='textarea5' name='textarea5' rows='3' cols='40' style='display:none;'></textarea><input type='text' id='text5' name='text5' value='" + strData + "' style='display:none;'/><select id='" + ddlresult + "' name='" + ddlresult + "' value='' ></select></td>";
                    dr["TextArea"] = strData;
                }

                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();

            controlsbody += "</tr></table>";
            RunControlsDiv.InnerHtml = controlsbody;
            ControlsDiv.InnerHtml = string.Empty;
            ltchartRun.Text = string.Empty;
            string ReportData = (fieldcount == 0) ? Report.GetRecord(strQuery) : string.Empty;

            if (fieldcount == 0)
            {

                if (chartType == "Single_line_chart")
                {
                    Linechart_bind(strQuery, "chartrun");
                }
                else if (chartType == "Pie_Chart")
                {
                    Piechart_bind(strQuery, "chartrun");
                }
                else if (chartType == "Bar_Chart")
                {
                    Barchart_bind(strQuery, "chartrun");
                }
                else if (chartType == "twobythree_line_chart")
                {
                    twoBythreechart_bind(strQuery, "chartrun");
                }


            }
            string data = ConvertDataTabletoString(dt);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "GetLogData(" + ReportData + "); AddRunReport(); EnableChartRun();GenerateReportControls(" + data + ");", true);//hide();
        }
    }


}







