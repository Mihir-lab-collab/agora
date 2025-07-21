using CSCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_EPF_Statement : Authentication
{
    private static List<EPFStatement> objEPFStatementList = new List<EPFStatement>();
    EPFStatement objEpfBal = new EPFStatement();
    static DataTable dtRecordTableEPF = new DataTable();
    static DataTable dtRecordTableEPFCalculation = new DataTable();
    static string DateReport = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
    }


    protected void btnEpf_Click(object sender, EventArgs e)
    {
        if (dtRecordTableEPF.Rows.Count > 0)
        {
            ExportToExcel();
            Response.Redirect("EPF_Statement.aspx");
        }
        else
        {
            messageBox("No Record Found ");

        }

    }

    [System.Web.Services.WebMethod]
    public static List<EPFStatement> GetEpfStatement(string data)
    {
        try
        {

            if (data != "")
            {
                string[] month = data.Split('/');
                DateReport = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(month[0])) + month[1];
                EPFStatement objEPFStatementBAL = new EPFStatement();
                DataSet DS = new DataSet();
                DS = objEPFStatementBAL.GetEpfStatement(data);

                dtRecordTableEPF = DS.Tables[0];
                dtRecordTableEPFCalculation = DS.Tables[1];

            }
        }
        catch (Exception Ex)
        {
            throw;
        }

        return ConvertDataTableToList(dtRecordTableEPF);
    }

    private void ExportToExcel()
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "EPF Statement_" + DateReport + ".xls"));
        Response.ContentType = "application/ms-excel";
        string str = string.Empty;
        if (dtRecordTableEPF.Rows.Count > 0)
        {

            Response.Write(str + " EPF STATEMENT FOR THE MONTH OF " + DateReport);
            Response.Write("\n");
            Response.Write("\n");
            str = "";
            int srnumber = 1;
            Response.Write(str + "Sr");
            str = "\t";
            foreach (DataColumn dtcol in dtRecordTableEPF.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");

            foreach (DataRow dr in dtRecordTableEPF.Rows)
            {
                str = "";
                if (srnumber != dtRecordTableEPF.Rows.Count)
                {
                    Response.Write(srnumber);
                }
                else
                {
                    Response.Write("TOTAL");
                }

                str = "\t";
                for (int j = 0; j < dtRecordTableEPF.Columns.Count; j++)
                {
                    if (j == 0 || j == 1)
                    {
                        if (Convert.ToString(dr[j]) == "0")
                        {
                            Response.Write(str + "");
                        }
                        else
                        {
                            if (Convert.ToString(dr[j]) == "TOTAL")
                            {
                                Response.Write(str + "");
                            }
                            else
                            {
                                Response.Write(str + Convert.ToString(dr[j]));
                            }

                        }

                    }
                    else
                    {
                        Response.Write(str + Convert.ToString(Global.GetCurrencyFormat(Convert.ToInt32(dr[j]))));

                    }

                    str = "\t";
                }
                Response.Write("\n");
                srnumber = srnumber + 1;
            }
            Response.Write("\n");
            Response.Write("\n");
            Response.Write("\t");
            Response.Write("EPF WORKING");
            Response.Write("\n");
            str = "";
            srnumber = 1;
            Response.Write(str + "");
            str = "\t";
            foreach (DataColumn dtcol in dtRecordTableEPFCalculation.Columns)
            {
                if (dtcol.ColumnName == "EPFWORKING")
                {
                    Response.Write(str + "");
                }
                else
                {
                    Response.Write(str + dtcol.ColumnName);
                }

                str = "\t";
            }

            Response.Write("\n");
            Response.Write("\t\t\t");
            Response.Write("0.85%");
            Response.Write("\t");
            Response.Write("EPS");
            Response.Write("\t");
            Response.Write(" DLI");
            Response.Write("\n");

            foreach (DataRow dr in dtRecordTableEPFCalculation.Rows)
            {
                str = "";
                Response.Write("");
                str = "\t";
                for (int j = 0; j < dtRecordTableEPFCalculation.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        Response.Write(str + Convert.ToString(dr[j]));
                    }
                    else
                    {
                        Response.Write(str + Convert.ToString(Global.GetCurrencyFormat(Convert.ToInt32(dr[j]))));
                    }

                    str = "\t";
                }
                Response.Write("\n");
                Response.Write("\n");
            }

            Response.End();
        }
    }

    private static List<EPFStatement> ConvertDataTableToList(DataTable dt)
    {
        try
        {
            objEPFStatementList.Clear();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    EPFStatement objEpf = new EPFStatement();
                    objEpf.empId = (dt.Rows[i][0].ToString() == "0") ? "" : dt.Rows[i][0].ToString();
                    objEpf.EmployeeName = dt.Rows[i][1].ToString();
                    objEpf.Basic = Convert.ToInt32(dt.Rows[i][2]);
                    objEpf.EPSWages = Convert.ToInt32(dt.Rows[i][3]);
                    objEpf.PF = Convert.ToInt32(dt.Rows[i][4]);
                    objEpf.EPSContribution = Convert.ToInt32(dt.Rows[i][5]);
                    objEpf.BalenceER = Convert.ToInt32(dt.Rows[i][6]);
                    objEPFStatementList.Add(objEpf);
                }

            }
            else
            {
                objEPFStatementList.Clear();
            }

        }
        catch (Exception)
        {

            throw;
        }
        return objEPFStatementList;
    }

    private void messageBox(string message)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('" + message + "');", true);
    }

}

