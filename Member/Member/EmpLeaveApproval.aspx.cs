using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AgoraBL.BAL;
using System.Globalization;

public partial class EmpLeaveApproval : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();  
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true,false,true);
        hdnLoginID.Value = UM.EmployeeID.ToString();
        EmpLeaveApprovalBLL obj = new EmpLeaveApprovalBLL();
        string status = obj.GetProfile("checkHRProfile", UM.EmployeeID.ToString());
        if(status=="True")
        {
            hdnHrProfileStatus.Value = status;
        }
        divMsg.Visible = false;
        if(!IsPostBack)
            BindLeaveType();
    }

    private void BindLeaveType()
    {
        DataTable dt = new DataTable();
        EmpLeaveApprovalBLL obj =new EmpLeaveApprovalBLL();
        dt = obj.GetLeaveType("LeaveType");
        Session["dt"] = dt;
        if (dt.Rows.Count > 0)
        {
            ddlLeaveType.DataSource = dt;
            ddlLeaveType.DataValueField = "statusId";
            ddlLeaveType.DataTextField = "statusDesc";
            ddlLeaveType.DataBind();
           
        }
    }

    [System.Web.Services.WebMethod]
    public static String GetEmpLeaves(string status, string name, string from, string to, int loginID, int includeArchive)
    {   
        try
        {
            List<EmpLeaveApprovalBLL> lstLeaves = EmpLeaveApprovalBLL.GetEmpLeaves("Select", status, name, from, to, Convert.ToInt32(HttpContext.Current.Session["LocationID"]), loginID, includeArchive);
            HttpContext.Current.Session["lstEmp"]=lstLeaves;            
            var data = from EItems in lstLeaves
                       select new
                       { 
                           EItems.EmpID,
                           EItems.EmpName,
                           EItems.EmpNameID,
                           EItems.EmpLeaveID,
                           EItems.LeaveType,
                           EItems.LeaveFrom,
                           EItems.LeaveTo,
                           EItems.LeaveAppliedOn,
                           EItems.LeaveReason,
                           EItems.AdminComment,
                           EItems.LeaveStatus,
                           EItems.LeaveSanctionOn,
                           EItems.LeaveSanctionBy,
                           EItems.BalanceLeave,
                           EItems.IsApproved,
                           EItems.TotalLeave,
                           EItems.IsTeam
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    [System.Web.Services.WebMethod]
    public static String UpdateEmpLeaveStatus(string empID, string empLeaveID, string leaveStatus, string LeaveType, string AdminComment, string SanctionedBy, string fDate="", string tDate="")
    {
        string sReturn = "";
        try
        {
            if (string.Compare(LeaveType, "LOP", true) == 0)
            {
                LeaveType = "WL";
            }
            leaveStatus = string.Compare(leaveStatus, "a", true) == 0 ? "Approved" : "Rejected";
            DateTime fromDate = DateTime.ParseExact(fDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(tDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            string fromDateFormatted = fromDate.ToString("dd/MM/yyyy").Replace("-","/");
            string toDateFormatted = toDate.ToString("dd/MM/yyyy").Replace("-", "/");
            //sReturn = EmpLeaveApprovalBLL.UpdateEmpLeaveStatus("UpdateStatus", empID, empLeaveID, leaveStatus, LeaveType, AdminComment, SanctionedBy, fDate,tDate);
            DataSet ds = new EmpLeaveApprovalBAL().UpdateEmpLeaveStatus("UpdateStatus", empID, empLeaveID, leaveStatus, LeaveType, AdminComment, SanctionedBy,fDate: fromDateFormatted, tDate: toDateFormatted);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables.Count > 2)
            {
                sReturn=Convert.ToString(ds.Tables[2].Rows[0]["LeaveStatus"]);
            }
            else
            {
                sReturn = string.Empty;
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
        List<EmpLeaveApprovalBLL> lstEmp = (List<EmpLeaveApprovalBLL>)Session["lstEmp"];
        var DistinctItems = lstEmp.GroupBy(x => x.EmpID).Select(y => y.First());
        lstEmp = DistinctItems.ToList();

        //List<int> result = lstEmp.Select(o => Convert.ToInt32(o.EmpID)).Distinct().ToList();
        chkEmplist.DataSource = lstEmp;
        chkEmplist.DataTextField = "EmpName";
        chkEmplist.DataValueField = "EmpID";
        chkEmplist.DataBind();
    }
    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        divEmpPopup.Visible = false;
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        string empID="";
        DataTable dt = new DataTable();
        for (int i = 0; i < chkEmplist.Items.Count; i++)
        {
            if (chkEmplist.Items[i].Selected)
                empID += chkEmplist.Items[i].Value + ",";
        }
        if (empID != "")
        {
            divMsg.Visible = false;
            List<EmpLeaveApprovalBLL> lstEmp = (List<EmpLeaveApprovalBLL>)Session["lstEmp"];
            var filteredOrders = lstEmp.Where(o => empID.Contains(o.EmpID));    // filter selected empid record
            lstEmp = filteredOrders.ToList();
            dt = GetTable(lstEmp); // list to datatable;
           
            ExportToExcel(dt);
            
        }
        else
        {
            divMsg.Visible = true;
        }
    }

    private DataTable GetTable(List<EmpLeaveApprovalBLL> lstEmp)
    {
        DataTable table = new DataTable();
        table.Columns.Add("Employee Name", typeof(string));
        table.Columns.Add("Leave From", typeof(string));
        table.Columns.Add("Leave To", typeof(string));
        table.Columns.Add("Leave Type", typeof(string));
        table.Columns.Add("No.Of Leaves Applied", typeof(int));
        table.Columns.Add("Current Leave Balance", typeof(string));
        table.Columns.Add("Leave Reason", typeof(string));
        table.Columns.Add("Leave Applied On", typeof(string));
        table.Columns.Add("Leave Status", typeof(string));
        table.Columns.Add("Leave Sanctioned Date", typeof(string));
        table.Columns.Add("Leave Sanction By", typeof(string));
        table.Columns.Add("Admin Comments", typeof(string));

        for (int i = 0; i < lstEmp.Count; i++)
        {
            table.Rows.Add(lstEmp[i].EmpNameID, lstEmp[i].LeaveFrom, lstEmp[i].LeaveTo, lstEmp[i].LeaveType, lstEmp[i].TotalLeave, lstEmp[i].BalanceLeave
                            , lstEmp[i].LeaveReason, lstEmp[i].LeaveAppliedOn, lstEmp[i].IsApproved, lstEmp[i].LeaveSanctionOn, lstEmp[i].LeaveSanctionBy, lstEmp[i].AdminComment);
        }

            return table;
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
       
        string fileName = "EmployeeLeave.xls";
        
        DataGrid dg = new DataGrid();
        dg.AllowPaging = false;
        dg.DataSource = dt;
        dg.DataBind();

        System.Web.HttpContext.Current.Response.Clear();
        System.Web.HttpContext.Current.Response.Buffer = true;
        System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
        System.Web.HttpContext.Current.Response.Charset = "";
        System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition","attachment; filename=" + fileName);

        System.Web.HttpContext.Current.Response.ContentType ="application/vnd.ms-excel";
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
}