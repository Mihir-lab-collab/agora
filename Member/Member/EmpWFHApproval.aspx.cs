using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_EmpWFHApproval : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true, false, true);
        hdnLoginID.Value = UM.EmployeeID.ToString();
        hdnEmpName.Value = UM.Name.Trim();
        EmpWFHApprovalBLL obj = new EmpWFHApprovalBLL();
        string status = obj.GetProfile("checkHRProfile", UM.EmployeeID.ToString());
        if (status == "True")
        {
            hdnHrProfileStatus.Value = status;
        }
        divMsg.Visible = false;
        if (!this.IsPostBack)
        {
            txtSearch.Value = "";
        }
    }
    [System.Web.Services.WebMethod]
    public static String GetEmpWFH(string status, string name, string from, string to, int loginID, int includeArchive)
    {
        try
        {
            List<EmpWFHApprovalBLL> listWFH = EmpWFHApprovalBLL.GetEmpWFH("Select", status, name, from, to, Convert.ToInt32(HttpContext.Current.Session["LocationID"]), loginID, includeArchive);
            foreach (var changeFormate in listWFH)
            {
                if (!string.IsNullOrEmpty(changeFormate.AttInTime) && !string.IsNullOrEmpty(changeFormate.AttOutTime))
                {
                    changeFormate.AttInTime = Convert.ToDateTime(changeFormate.AttInTime).ToString("HH:mm");
                    changeFormate.AttOutTime = Convert.ToDateTime(changeFormate.AttOutTime).ToString("HH:mm");
                }

            }
            HttpContext.Current.Session["lstEmp"] = listWFH;
            var data = from EItems in listWFH
                       select new
                       {
                           EItems.EmpID,
                           EItems.EmpName,
                           EItems.EmpNameID,
                           EItems.EmpWFHID,
                           EItems.WFHType,
                           EItems.WFHFrom,
                           EItems.WFHTo,
                           EItems.WFHAppliedOn,
                           EItems.WFHReason,
                           EItems.AdminComment,
                           EItems.WFHStatus,
                           EItems.WFHSanctionOn,
                           EItems.WFHSanctionBy,
                           EItems.IsApproved,
                           EItems.TotalWFH,
                           EItems.IsTeam,
                           EItems.WFHEntryDate,
                           EItems.AttInTime,
                           EItems.AttOutTime,
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    [System.Web.Services.WebMethod]
    public static String UpdateEmpWFHStatus(string empID, string empWFHID, string WFHStatus, string AdminComment, string SanctionedBy, string fDate, string tDate,string LoginEmpId)
    {
        string sReturn = "";
        try
        {
            sReturn = EmpWFHApprovalBLL.UpdateEmpWFHStatus("UpdateStatus", empID, empWFHID, WFHStatus, AdminComment, SanctionedBy, fDate, tDate, LoginEmpId);
            if (Convert.ToInt32(sReturn) > 0)
            {
                EmpWFHApprovalBLL objEmpWFHApprovalBLL = new EmpWFHApprovalBLL();
                DataTable dt = new DataTable();
                dt = objEmpWFHApprovalBLL.SendMail("SendEmail", Convert.ToInt32(empID));
                if (dt.Rows.Count > 0)
                {
                    if (WFHStatus == "a")
                    {
                        WFHStatus = "Approved";
                    }
                    else if (WFHStatus == "r")
                    {
                        WFHStatus = "Rejected";
                    }
                    else
                    {
                        WFHStatus = "Pending";
                    }
                    objEmpWFHApprovalBLL.SendMail(dt, WFHStatus, AdminComment);
                }
            }
            return sReturn;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    protected void btnShowList_Click(object sender, EventArgs e)
    {
        divEmpPopup.Visible = true;
        List<EmpWFHApprovalBLL> lstEmp = (List<EmpWFHApprovalBLL>)Session["lstEmp"];
        var DistinctItems = lstEmp.GroupBy(x => x.EmpID).Select(y => y.First());
        lstEmp = DistinctItems.ToList();

        //List<int> result = lstEmp.Select(o => Convert.ToInt32(o.EmpID)).Distinct().ToList();
        chkEmplist.DataSource = lstEmp;
        chkEmplist.DataTextField = "EmpNameID";
        chkEmplist.DataValueField = "EmpID";
        chkEmplist.DataBind();

    }

    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        divEmpPopup.Visible = false;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string empID = "";
        DataTable dt = new DataTable();
        for (int i = 0; i < chkEmplist.Items.Count; i++)
        {
            if (chkEmplist.Items[i].Selected)
                empID += chkEmplist.Items[i].Value + ",";
        }
        if (empID != "")
        {
            string year = string.Empty;
            string currentYear = DateAndTime.DateAdd("m", 0, DateAndTime.Now).ToString();
            if (DateTime.Now.Month < 4)
            {
                year = Convert.ToString(Convert.ToInt32((Convert.ToDateTime(currentYear).Year) - 1));
            }
            else
            {
                year = Convert.ToString((Convert.ToDateTime(currentYear).Year));
            }
            divMsg.Visible = false;
            List<EmpWFHApprovalBLL> lstEmp = (List<EmpWFHApprovalBLL>)Session["lstEmp"];
            var filteredOrders = lstEmp.Where(o => empID.Contains(o.EmpID) && o.WFHEntryDate.Contains(year));    // filter selected empid record
            lstEmp = filteredOrders.ToList();
            dt = GetTable(lstEmp); // list to datatable;

            ExportToExcel(dt);

        }
        else
        {
            divMsg.Visible = true;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        divMsg.Visible = false;
        SetChkBox(false);
    }
    private void SetChkBox(bool bflag)
    {
        for (int i = 0; i < chkEmplist.Items.Count; i++)
            chkEmplist.Items[i].Selected = bflag;
    }
    public void ExportToExcel(DataTable dt)
    {

        string fileName = "EmployeeWFH.xls";

        DataGrid dg = new DataGrid();
        dg.AllowPaging = false;
        dg.DataSource = dt;
        dg.DataBind();

        System.Web.HttpContext.Current.Response.Clear();
        System.Web.HttpContext.Current.Response.Buffer = true;
        System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
        System.Web.HttpContext.Current.Response.Charset = "";
        System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

        System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlTextWriter =
          new System.Web.UI.HtmlTextWriter(stringWriter);
        //dg.RenderControl(htmlTextWriter);
        //System.Web.HttpContext.Current.Response.Write(stringWriter.ToString());
        //System.Web.HttpContext.Current.Response.End();

        dg.RenderControl(new HtmlTextWriter(Response.Output));
        Response.Flush();
        Response.End();
    }
    private DataTable GetTable(List<EmpWFHApprovalBLL> lstEmp)
    {
        DataTable table = new DataTable();
        table.Columns.Add("Employee Name", typeof(string));
        table.Columns.Add("WFH From", typeof(string));
        table.Columns.Add("WFH To", typeof(string));
        table.Columns.Add("AttInTime", typeof(string));
        table.Columns.Add("AttOutTime", typeof(string));
        //table.Columns.Add("WFH Type", typeof(string));
        // table.Columns.Add("No.Of WFH Applied", typeof(int));
        table.Columns.Add("WFH Reason", typeof(string));
        table.Columns.Add("WFH Applied On", typeof(string));
        table.Columns.Add("WFH Status", typeof(string));
        table.Columns.Add("WFH Sanctioned Date", typeof(string));
        table.Columns.Add("WFH Sanction By", typeof(string));
        table.Columns.Add("Admin Comments", typeof(string));
        table.Columns.Add("Employee WFH Count", typeof(string));

        for (int i = 0; i < lstEmp.Count; i++)
        {
            table.Rows.Add(lstEmp[i].EmpNameID, lstEmp[i].WFHFrom, lstEmp[i].WFHTo, lstEmp[i].AttInTime, lstEmp[i].AttOutTime
                            , lstEmp[i].WFHReason, lstEmp[i].WFHAppliedOn, lstEmp[i].IsApproved, lstEmp[i].WFHSanctionOn, lstEmp[i].WFHSanctionBy, lstEmp[i].AdminComment, lstEmp[i].EmployeeWFHCount);
        }

        return table;
    }
    protected void btnBulkAppliedWFH_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtSearch.Value))
        {

            showPopUp.Visible = true;
            List<EmpWFHApprovalBLL> lstEmp = new List<EmpWFHApprovalBLL>();
            EmpWFHApprovalBLL objBLL = new EmpWFHApprovalBLL();
            lstEmp = objBLL.EmployeeList("EmployeeList");
            cblEmployeeWFH.DataSource = lstEmp;
            cblEmployeeWFH.DataTextField = "empName";
            cblEmployeeWFH.DataValueField = "empId";
            cblEmployeeWFH.DataBind();
        }
        else
        {
            bool containsOnlyLettersAndNumbers = Regex.IsMatch(txtSearch.Value.Trim(), "^[a-zA-Z0-9]+$");
            if (containsOnlyLettersAndNumbers)
            {
                string empName = txtSearch.Value.Trim();
                bool containsLetter = empName.Any(c => Char.IsLetter(c));
                bool containsNumber = empName.Any(c => Char.IsDigit(c));
                List<EmpWFHApprovalBLL> lstEmp = new List<EmpWFHApprovalBLL>();
                EmpWFHApprovalBLL objBLL = new EmpWFHApprovalBLL();
                if (containsLetter)
                {
                    var filteredEmployees = objBLL.EmployeeList("EmployeeList").Where(x => x.EmpName.StartsWith(empName.ToString()
                            , StringComparison.OrdinalIgnoreCase));
                    cblEmployeeWFH.DataSource = filteredEmployees;
                    cblEmployeeWFH.DataTextField = "empName";
                    cblEmployeeWFH.DataValueField = "empId";
                    cblEmployeeWFH.DataBind();
                }
                if (containsNumber)
                {
                    var filteredEmployees = objBLL.EmployeeList("EmployeeList").Where(x => Convert.ToInt32(x.EmpID) == Convert.ToInt32(empName));
                    cblEmployeeWFH.DataSource = filteredEmployees;
                    cblEmployeeWFH.DataTextField = "empName";
                    cblEmployeeWFH.DataValueField = "empId";
                    cblEmployeeWFH.DataBind();
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('please select valid input ! ')", true);
            }
        }
        
    }

    protected void imgClosePopUp_Click(object sender, ImageClickEventArgs e)
    {
        showPopUp.Visible = false;
        txtSearch.Value = "";
        txtReasonForWFH.Value = "";
    }

    protected void btnBulkWFHOk_Click(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(txtReasonForWFH.Value))
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Reason field cannot be blank !')", true);
            return;
        }
        string empID = "";
        for (int i = 0; i < cblEmployeeWFH.Items.Count; i++)
        {
            if (cblEmployeeWFH.Items[i].Selected)
                empID += cblEmployeeWFH.Items[i].Value + ",";
        }
        if (empID != "")
        {
            string[] listEmployeeID = empID.Split(',');
            listEmployeeID = listEmployeeID.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (listEmployeeID.Length > 0)
            {
                string description = txtReasonForWFH.Value;
                string WFHfrom = hdnFromDateWFH.Value;
                string WFHTo = hdnToDateWFH.Value;
                EmpWFHApprovalBLL objBLL = new EmpWFHApprovalBLL();
               int count= objBLL.BulkApplyWFH(listEmployeeID, "ApplyWFH", WFHfrom, WFHTo,
                    description, DateTime.Today.ToString(), UM.Name.ToString());
                if (count>0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('saved successfully.')", true);
                }
                //showPopUp.Visible = false;
            }
        }
    }

    protected void btnBulkWFHClear_Click(object sender, EventArgs e)
    {
        List<EmpWFHApprovalBLL> lstEmp = new List<EmpWFHApprovalBLL>();
        EmpWFHApprovalBLL objBLL = new EmpWFHApprovalBLL();
        lstEmp = objBLL.EmployeeList("EmployeeList");
        cblEmployeeWFH.DataSource = lstEmp;
        cblEmployeeWFH.DataTextField = "empName";
        cblEmployeeWFH.DataValueField = "empId";
        cblEmployeeWFH.DataBind();
        txtReasonForWFH.Value = "";
        txtSearch.Value = "";

    }
}