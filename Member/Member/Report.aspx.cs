using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using System.Text;


public partial class Member_Report : Authentication
{
    UserMaster UM;
    [System.Web.Services.WebMethod]
    public static String BindReport()
    {
        try
        {
            List<Report> listReport = new List<Report>();
            if (UserMaster.UserMasterInfo().IsModuleAdmin || UserMaster.UserMasterInfo().IsAdmin)
            {
                listReport = Report.GetReportList("SelectAll", 0);
            }
            else
            {
                listReport = Report.GetReportList("SelectWrtIsAdmin", 0);
            }

            var data = from ReportItem in listReport
                       select new
                       {
                           ReportItem.reportId,
                           ReportItem.name,
                           //ReportItem.numberOfField,
                           ReportItem.field1,
                           ReportItem.field2,
                           ReportItem.field3,
                           ReportItem.field4,
                           ReportItem.field5,
                           ReportItem.query,
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
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "GetReportDetails();", true);
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
            //dt.Columns.Add("Result", typeof(string));

            divRun.Visible = true;
            lblReportName.Text = " - " + report[0].name;
            hdnChartType.Value = Convert.ToString(report[0].chartType);
            string chartType = Convert.ToString(report[0].chartType);

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
          
            if (fieldcount == 0)
            {
               // myImage.Visible = false;
                btnRun.Visible = false;
            }
            else
            {
               // myImage.Visible = true;
                btnRun.Visible = true;
            }

            string controlsbody = "";
            txtrunquery.Text = report[0].query;
            txtReportName.Text = report[0].name;

            //   hdnChartRun.Value = Convert.ToString(report[0].showChart);
            string strQuery = report[0].query;
            controlsbody = "<table width=\"100%\" style=\"overflow: hidden;\">";

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
            ltchartRun.Text = string.Empty;
            string ReportData = (fieldcount == 0) ? Report.GetRecord(strQuery) : string.Empty;

            if (fieldcount == 0)
            {

                if (chartType == "Single_line_chart")
                    {
                        Linechart_bind(strQuery);
                    }
                else if (chartType == "Pie_Chart")
                    {
                        Piechart_bind(strQuery);
                    }
                else if (chartType == "Bar_Chart")
                    {
                        Barchart_bind(strQuery);
                    }
                else if (chartType == "twobythree_line_chart")
                    {
                        twoBythreechart_bind(strQuery);
                    }
                }
           
            string data = ConvertDataTabletoString(dt);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "GetLogData(" + ReportData + "); AddRunReport(); EnableChartRun();GenerateReportControls(" + data + ");", true);//hide();
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

    protected void btnRun_Click(object sender, EventArgs e)
    {
        int fieldcount = 0;
        fieldcount = Convert.ToInt32(hdnFieldCount.Value);
        string strQuery = txtrunquery.Text.Trim();
        string controlsbody = "";
        lblReportName.Text = " - " + txtReportName.Text;

        string chartType = Convert.ToString(hdnChartType.Value);
        controlsbody = "<table width=\"100%\" style=\"overflow: hidden;\">";

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
                Linechart_bind(strQuery);
            }
            else if (chartType == "Pie_Chart")
            {
                Piechart_bind(strQuery);
            }
            else if (chartType == "Bar_Chart")
            {
                Barchart_bind(strQuery);
            }
            else if (chartType == "twobythree_line_chart")
            {
                twoBythreechart_bind(strQuery);
            }
  

        string data = ConvertDataTabletoString(dt);
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "showdata", "GetLogData(" + ReportData + ");AddRunReport(); EnableChartRun();GenerateReportControls(" + data + ");", true);//hide();
    }
    protected void btnRunReport_Click(object sender, EventArgs e)
    {
        BindRunReportDetails(hdnReportId.Value);
    }

    public void Linechart_bind(string Query)
    {
        StringBuilder str = new StringBuilder();
        DataTable Dt = new DataTable();
        Dt = Report.GetRecordDT(Query);
        if (Dt.Rows.Count > 0)
        {
            str.Append(@"<script type=*text/javascript*> google.load( *visualization*, *1*, {packages:[*corechart*]});
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
            str.Append("   var chart = new google.visualization.LineChart(document.getElementById('chart_div_run'));");
            str.Append(" chart.draw(data, options,{width: 660, height: 300}); }");
            str.Append("</script>");
            ltchartRun.Text = str.ToString().Replace('*', '"');

        }
    }
    public void Barchart_bind(string Query)
    {
        StringBuilder str = new StringBuilder();
        DataTable Dt = new DataTable();
        Dt = Report.GetRecordDT(Query);
        if (Dt.Rows.Count > 0)
        {
            str.Append(@"<script type=*text/javascript*> google.load( *visualization*, *1*, {packages:[*corechart*]});
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

            str.Append("   var chart = new google.visualization.BarChart(document.getElementById('chart_div_run'));");
            str.Append(" chart.draw(data, options,{width: 660, height: 300}); }");
            str.Append("</script>");

            ltchartRun.Text = str.ToString().Replace('*', '"');


        }
    }
    public void Piechart_bind(string Query)
    {
        StringBuilder str = new StringBuilder();
        DataTable Dt = new DataTable();
        Dt = Report.GetRecordDT(Query);
        if (Dt.Rows.Count > 0)
        {

            str.Append(@"<script type=*text/javascript*> google.load( *visualization*, *1*, {packages:[*corechart*]});
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

            str.Append("   var chart = new google.visualization.PieChart(document.getElementById('chart_div_run'));");
            str.Append(" chart.draw(data, options,{width: 660, height: 300}); }");
            str.Append("</script>");

            ltchartRun.Text = str.ToString().Replace('*', '"');

        }
    }
    public void twoBythreechart_bind(string Query)
    {
        StringBuilder str = new StringBuilder();
        DataTable Dt = new DataTable();
        Dt = Report.GetRecordDT(Query);


        if (Dt.Rows.Count > 0)
        {
            str.Append(@"<script type='text/javascript'> google.load( 'visualization', '1.1', {packages:['line']});
            google.setOnLoadCallback(drawChart);
            function drawChart() {
            var data = new google.visualization.DataTable(); 
            data.addColumn('string', '');");

            //For 3 columns only.
            for (int i = 1; i < 4; i++)
            {
                str.Append(" data.addColumn('number', '" + Dt.Columns[i].ColumnName + "');");
            }
            str.Append("data.addColumn({type: 'string', role: 'tooltip' ,'p': {'html': true}} );");

            str.Append("data.addRows([");
            for (int i = 0; i <= Dt.Rows.Count - 1; i++)
            {
                str.Append("['" + Dt.Rows[i][0].ToString() + "',"
                                + Dt.Rows[i][1].ToString().Replace(",", "") + ","
                                + Dt.Rows[i][2].ToString().Replace(",", "") + ","
                                + Dt.Rows[i][3].ToString().Replace(",", "") + ",'<b>"
                                + Dt.Rows[i][0].ToString() + "</b> <br/> " + Dt.Rows[i][1].ToString() + "'],");
            }
            str.Append("]);");
            str.Append(" var options = {tooltip: {isHtml: true}, width: 900, height: 500}; ");

            str.Append(" var chart = new google.charts.Line(document.getElementById('chart_div_run'));");
            str.Append(" chart.draw(data, options); }");
            str.Append("</script>");

            ltchartRun.Text = str.ToString().Replace('*', '"');

        }
    }

}







