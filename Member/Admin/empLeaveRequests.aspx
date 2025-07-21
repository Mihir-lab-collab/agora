<%@ Page Language="vb" AutoEventWireup="false" enableSessionState="true"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.IO"%>

<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD html 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
 <head>
  <title>Dynamic web Tech</title>
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" /> 
     <script src="../js/jquery.min.js" type="text/javascript"></script>
      <script src="../js/jquery.min.1.9.1.js" type="text/javascript"></script>
      <script src="../JSController/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../JSController/ScrollableGridPlugin.js" type="text/javascript"></script>
     <style type="text/css">
         div.DialogueBackground {
             position: fixed;
             width: 100%;
             height: 100%;
             top: 0;
             left: 0;
             background-color: #777;
             opacity: 0.9;
             filter: alpha(opacity=50);
             text-align: center;
         }

             div.DialogueBackground div.Dialogue {
                 width: 300px;
                 height: 400px;
                 overflow: auto;
                 position: absolute;
                 left: 50%;
                 top: 20%;
                 margin-left: -150px;
                 margin-top: -50px;
                 padding: 10px;
                 background-color: #fff;
                 border: solid 2px #000;
                 z-index: 10090;
             }


         div.close-button {
             float: right;
             margin: -10px -10px 0 0;
         }

         div.k-overlayDisplaynone {
             display: none;
         }

         div.k-overlay {
             display: block;
             z-index: 10002;
             opacity: 0.5;
         }

         div.a_popbox {
             position: absolute;
             padding: 25px;
             z-index: 10050;
             background-color: white;
         }

         div.popup_wrap {
             border: 6px solid #252e34;
             background-color: #FFF;
             position: absolute;
             padding: 15px;
             z-index: 2;
             overflow-x: scroll;
         }

             div.popup_wrap h1 {
                 padding: 0 0 10px 0;
                 margin-bottom: 15px;
                 border-bottom: 1px solid #e5e7e4;
                 font-size: 22px;
                 color: #728c3a;
             }

         div.popup_msg {
             border: 6px solid #252e34;
             background-color: #FFF;
             position: absolute;
             padding: 15px;
             top: 20%;
             left: 25%;
             right: 25%;
             z-index: 1;
         }
     </style>
     <script src="../JSController/empLeaveRequests.js" type="text/javascript"></script>
<script language="VB" runat="server">
    Dim sql1 As String
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim objcmd As SqlCommand
    Dim gf As New generalFunction
    Dim objCommon As New clsCommon()
    Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gf.checkEmpLogin()
        If Me.IsPostBack = False Then
           
            Dim userDetail As New UserDetails
            userDetail = Session("DynoEmpSessionObject")
            
            If Not userDetail Is Nothing Then
                If Not String.IsNullOrEmpty(userDetail.ProfileID) Then
                    If userDetail.ProfileID <> "" Then
                        hdLocationId.Value = objCommon.GetLocationAcess(userDetail.ProfileID).ToString()
                        'hdHasAllLocationAcess.Value = (hdLocationId.Value.Equals("0") ? "1" : "0")
                    End If
                End If
            End If
        
           
            
            Call BindLocation()
            Call BindDataGrid()
            Call BindCheckList() 'Added by pravin on 1 Jul 2014
           
        End If
    End Sub

    Sub BindDataGrid()
        Dim conn1 As SqlConnection = New SqlConnection(dsn)
        Dim dad As New SqlDataAdapter
        Dim ds1 As New DataSet
        'If Request.QueryString("search") = "" Or Request.QueryString("search") = "all" Then
        If hdnStatus.Value = "" Or hdnStatus.Value = "all" Then
            sql1 = ("select leave.*,CASE WHEN emp.empProbationPeriod<>0 and (DateAdd(mm,emp.empProbationPeriod,emp.empJoiningDate) >= GETDATE()) THEN 'WL'  else leave.LeaveId end as desc1,dbo.getBalanceLeave(leave.empid) as balanceleave,dbo.GetNoOfLeavesApplied(leave.leaveFrom,leave.leaveTo) as TotLeave,case when leavestatus='p' then 'Pending' when leavestatus='r' then 'Rejected' when leavestatus='a' then 'Approved' end as ls, emp.empId,emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as empName from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid ")
        Else
            If hdnStatus.Value = "pending" Then
                sql1 = ("select leave.*,CASE WHEN emp.empProbationPeriod<>0 and (DateAdd(mm,emp.empProbationPeriod,emp.empJoiningDate) >= GETDATE()) THEN 'WL'  else leave.LeaveId end as desc1,dbo.getBalanceLeave(leave.empid) as balanceleave,dbo.GetNoOfLeavesApplied(leave.leaveFrom,leave.leaveTo) as TotLeave,case when leavestatus='p' then 'Pending' when leavestatus='r' then 'Rejected' when leavestatus='a' then 'Approved' end as ls, emp.empId,emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as empName from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid AND leave.leaveStatus='p'")
            Else
                If hdnStatus.Value = "approved" Then
                    sql1 = ("select leave.*,CASE WHEN emp.empProbationPeriod<>0 and (DateAdd(mm,emp.empProbationPeriod,emp.empJoiningDate) >= GETDATE()) THEN 'WL'  else leave.LeaveId end as desc1,dbo.getBalanceLeave(leave.empid) as balanceleave,dbo.GetNoOfLeavesApplied(leave.leaveFrom,leave.leaveTo) as TotLeave,case when leavestatus='p' then 'Pending' when leavestatus='r' then 'Rejected' when leavestatus='a' then 'Approved' end as ls, emp.empId,emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as empName from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid AND leave.leaveStatus='a'")
                Else
                    If hdnStatus.Value = "rejected" Then
                        sql1 = ("select leave.*,CASE WHEN emp.empProbationPeriod<>0 and (DateAdd(mm,emp.empProbationPeriod,emp.empJoiningDate) >= GETDATE()) THEN 'WL'  else leave.LeaveId end as desc1,dbo.getBalanceLeave(leave.empid) as balanceleave,dbo.GetNoOfLeavesApplied(leave.leaveFrom,leave.leaveTo) as TotLeave,case when leavestatus='p' then 'Pending' when leavestatus='r' then 'Rejected' when leavestatus='a' then 'Approved' end as ls, emp.empId,emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as empName from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid AND leave.leaveStatus='r' ")
                    End If
                End If
            End If
        End If
        
        If (hdLocationId.Value <> "0") Then
            sql1 = sql1 & " and emp.LocationFKID = '" & hdLocationId.Value & "' "
        End If
        If (txtEmpName.Text <> "") Then
            sql1 = sql1 & " and emp.empName Like '" & txtEmpName.Text & "%'"
        End If
        If (txtFromDate.Text <> "") Then
            sql1 = sql1 & " and leave.leaveFrom >= '" & Convert.ToDateTime(txtFromDate.Text + " 00:00:00") & "' "
        End If
        If (txtToDate.Text <> "") Then
            sql1 = sql1 & " and leave.leaveFrom <= '" & Convert.ToDateTime(txtToDate.Text + " 23:59:00") & "' "
        End If
       
        sql1 = sql1 & " order by leave.leaveFrom desc"
        
        dad = New SqlDataAdapter(sql1, conn1)
        Dim dt As DataTable = New DataTable("table1")
        Dim dc As DataColumn = New DataColumn("srno", GetType(Int32))
        dc.AutoIncrement = True
        dc.AutoIncrementSeed = 1
        dt.Columns.Add(dc)
        ds1.Tables.Add(dt)
        dad.Fill(ds1, "table1")
        'Response.Write(dt.Rows.Count)
        'emplrDataGrid.CurrentPageIndex = 0
        If ds1.Tables(0).Rows.Count = 0 Then
            lblNoData.Visible = True
            lblNoData.Text = "No Data found"
        Else
            lblNoData.Visible = False
            lblNoData.Text = ""
        End If

        emplrDataGrid.DataSource = ds1
        emplrDataGrid.DataBind()
	
    End Sub
    Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim FromDate, ToDate As Date
        FromDate = Convert.ToDateTime(txtFromDate.Text + " 00:00:00")
        ToDate = Convert.ToDateTime(txtToDate.Text + " 00:00:00")
        If IsDate(FromDate) And IsDate(ToDate) Then
            If FromDate > ToDate Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('To Date should be greater than From Date.'); ", True)
            Else
                emplrDataGrid.CurrentPageIndex = 0
                BindDataGrid()
            End If
        End If
        'emplrDataGrid.CurrentPageIndex = 0
        'BindDataGrid()
    End Sub

    Sub emplrDataGrid_PageIndexChanged(Source As Object, E As DataGridPageChangedEventArgs)
        emplrDataGrid.CurrentPageIndex = E.NewPageIndex
        BindDataGrid()
    End Sub
    Sub emplrDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
             
        
        Dim leavedetails As Button = CType(e.Item.Cells(13).FindControl("leavedetails"), Button)
        If e.Item.Cells(9).Text <> "&nbsp;" Then
            leavedetails.Attributes.Add("onclick", "popupLeaveDetails(" & e.Item.Cells(0).Text & ",'" & e.Item.Cells(9).Text & "'); return false;")
            'else
            'leavedetails.Attributes.Add("onclick", "popupLeaveDetails("& e.Item.Cells(0).text  &",'"& 1  &"'); return false;")
        End If
        If e.Item.Cells(9).Text = "Rejected" Then
            e.Item.Cells(9).ForeColor = System.Drawing.Color.Red
        End If
    End Sub
   
    Protected Sub emplrDataGrid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub lnkAll_Click(sender As Object, e As EventArgs)
        hdnStatus.Value = "all"
        emplrDataGrid.CurrentPageIndex = 0
        lnkAll.ForeColor = Drawing.Color.Black
        lnkPending.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkApproved.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkRejected.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        BindDataGrid()
    End Sub
    Protected Sub lnkPending_Click(sender As Object, e As EventArgs)
        hdnStatus.Value = "pending"
        emplrDataGrid.CurrentPageIndex = 0
        lnkAll.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkPending.ForeColor = Drawing.Color.Black
        lnkApproved.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkRejected.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        BindDataGrid()
    End Sub
    Protected Sub lnkApproved_Click(sender As Object, e As EventArgs)
        emplrDataGrid.CurrentPageIndex = 0
        hdnStatus.Value = "approved"
        lnkAll.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkPending.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkApproved.ForeColor = Drawing.Color.Black
        lnkRejected.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        BindDataGrid()
    End Sub
    Protected Sub lnkRejected_Click(sender As Object, e As EventArgs)
        hdnStatus.Value = "rejected"
        emplrDataGrid.CurrentPageIndex = 0
        lnkAll.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkPending.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkApproved.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkRejected.ForeColor = Drawing.Color.Black
        BindDataGrid()
    End Sub
    
    Protected Sub btnReset_Click(sender As Object, e As EventArgs)
        txtEmpName.Text = ""
        txtFromDate.Text = ""
        txtToDate.Text = ""
        If (lblLocationId.Visible = False) Then
            dlLocation.SelectedValue = "10"
            hdLocationId.Value = dlLocation.SelectedValue
        End If
        emplrDataGrid.CurrentPageIndex = 0
        BindDataGrid()
    End Sub
    Sub BindLocation()

        Dim dtEmployeeLocation As DataTable = New DataTable()
        dtEmployeeLocation = objCommon.EmployeeLocationList()
        If (hdLocationId.Value.Equals("0")) Then
       
            dlLocation.DataSource = dtEmployeeLocation
            dlLocation.DataTextField = "Name"
            dlLocation.DataValueField = "LocationID"
            dlLocation.DataBind()
            dlLocation.SelectedValue = dtEmployeeLocation.Select("Name='Mumbai'").FirstOrDefault()("LocationID").ToString()
            hdLocationId.Value = dlLocation.SelectedValue
            dlLocation.Visible = True
            lblLocation.Visible = True
        Else
            lblLocationId.Visible = True
            lblLocation.Visible = True
            dlLocation.Visible = False
            Dim location As String = objCommon.GetLocationName(Convert.ToInt32(hdLocationId.Value))
            lblLocationId.Text = location
        End If
        
    End Sub
    Protected Sub dlLocation_SelectedIndexChanged(sender As Object, e As EventArgs)
        hdLocationId.Value = dlLocation.SelectedValue
        emplrDataGrid.CurrentPageIndex = 0
        Call BindDataGrid()
    End Sub
    
    'added by Pravin on 15 May 2014 : Start
    Public Sub ShowHideDialogue(sender As Object, e As EventArgs)
        Dim FromDate, ToDate As Date
        FromDate = Convert.ToDateTime(txtFromDate.Text + " 00:00:00")
        ToDate = Convert.ToDateTime(txtToDate.Text + " 00:00:00")
        If IsDate(FromDate) And IsDate(ToDate) Then
            If FromDate > ToDate Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('To Date should be greater than From Date.'); ", True)
            Else
                'BindColumnsNames()
                BindCheckList()
                Dim button As Button = TryCast(sender, Button)
                Me.ShowHideDialogueInternal(button.CommandArgument.Equals("show"))
            End If
        End If
           
    End Sub
    
    Sub BindCheckList()
        Dim conn1 As SqlConnection = New SqlConnection(dsn)
        Dim dad As New SqlDataAdapter
        Dim ds1 As New DataSet
        'If Request.QueryString("search") = "" Or Request.QueryString("search") = "all" Then
        
        
        If hdnStatus.Value = "" Or hdnStatus.Value = "all" Then
            sql1 = ("select distinct emp.empId as empId,emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as empName from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid ")
        Else
            If hdnStatus.Value = "pending" Then
                sql1 = ("select distinct emp.empId as empId, emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as empName from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid AND leave.leaveStatus='p'")
            Else
                If hdnStatus.Value = "approved" Then
                    sql1 = ("select distinct  emp.empId as empId, emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as empName from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid AND leave.leaveStatus='a'")
                Else
                    If hdnStatus.Value = "rejected" Then
                        sql1 = ("select distinct  emp.empId as empId, emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as empName from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid AND leave.leaveStatus='r' ")
                    End If
                End If
            End If
        End If
        
        If (hdLocationId.Value <> "0") Then
            sql1 = sql1 & " and emp.LocationFKID = '" & hdLocationId.Value & "' "
        End If
        If (txtEmpName.Text <> "") Then
            sql1 = sql1 & " and emp.empName Like '" & txtEmpName.Text & "%'"
        End If
        If (txtFromDate.Text <> "") Then
            sql1 = sql1 & " and leave.leaveFrom >= '" & Convert.ToDateTime(txtFromDate.Text + " 00:00:00") & "' "
        End If
        If (txtToDate.Text <> "") Then
            sql1 = sql1 & " and leave.leaveFrom <= '" & Convert.ToDateTime(txtToDate.Text + " 23:59:00") & "' "
        End If
       
        'sql1 = sql1 & " order by leave.leaveFrom desc"
        
        dad = New SqlDataAdapter(sql1, conn1)
        Dim dt As DataTable = New DataTable("table1")
        Dim dc As DataColumn = New DataColumn("srno", GetType(Int32))
        dc.AutoIncrement = True
        dc.AutoIncrementSeed = 1
        dt.Columns.Add(dc)
        ds1.Tables.Add(dt)
        dad.Fill(ds1, "table1")
        
        'emplrDataGrid.DataSource = ds1
        'emplrDataGrid.DataBind()
        Me.chkColumnsList.DataSource = ds1
        Me.chkColumnsList.DataTextField = "empName"
        Me.chkColumnsList.DataValueField = "empId"
        Me.chkColumnsList.DataBind()
    End Sub
    
    Private Sub ShowHideDialogueInternal(state As Boolean)
        Me.pnlColumns.Visible = state
        ' AddGridViewColumnsInRuntime()
        Call BindDataGrid()
    End Sub
    
    'Export to Excel function
    Protected Sub ExportToExcel(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddColumns.Click
        'Get the data from database into datatable
        Dim conn1 As SqlConnection = New SqlConnection(dsn)
        Dim dad As New SqlDataAdapter
        Dim ds1 As New DataSet
        Dim ListItem As New StringBuilder
        Dim commaString As String = String.Empty
        Dim sql1 As String
        
        
        Dim counter As Integer = 0
        
        For Each chk In chkColumnsList.Items
            If chk.selected Then
                commaString += chk.Value + ","
                counter += 1
            End If
        Next
        
       
        
        If counter > 0 Then
            commaString = commaString.Substring(0, commaString.Length - 1)
            sql1 = "select emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as 'Employee Name', CONVERT(VARCHAR(10),leave.leaveFrom,105) as 'Leave From' , CONVERT(VARCHAR(10),leave.leaveTo,105) as 'Leave To', " & _
                    " CASE WHEN emp.empProbationPeriod<>0 and (DateAdd(mm,emp.empProbationPeriod,emp.empJoiningDate) >= GETDATE()) THEN 'WL'  else leave.LeaveId end As 'Leave Type', " & _
                    " dbo.GetNoOfLeavesApplied(leave.leaveFrom,leave.leaveTo) as 'No.Of Leaves Applied',dbo.getBalanceLeave(leave.empid) as 'Current Leave Balance', " & _
                    " leave.leaveDesc as 'Leave Reason', CONVERT(VARCHAR(10),leave.leaveEntryDate,105) as 'Leave Applied On', " & _
                    " case when leavestatus='p' then 'Pending' when leavestatus='r' then 'Rejected' when leavestatus='a' then 'Approved' end as 'Leave Status',  " & _
                    " CONVERT(VARCHAR(10),leave.leaveSenctionedDate,105) as 'Leave Sanctioned Date', leave.leaveSanctionBy 'Leave Sanction By' , leave.leaveComment as 'Admin Comments' " & _
                    " from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid and emp.empid in (" & commaString & ") "
        Else
            sql1 = "select emp.empName +'('+ CONVERT(varchar, emp.empId) +')' as 'Employee Name',CONVERT(VARCHAR(10),leave.leaveFrom,105) as 'Leave From' , CONVERT(VARCHAR(10),leave.leaveTo,105) as 'Leave To', " & _
                    " CASE WHEN emp.empProbationPeriod<>0 and (DateAdd(mm,emp.empProbationPeriod,emp.empJoiningDate) >= GETDATE()) THEN 'WL'  else leave.LeaveId end As 'Leave Type', " & _
                    " dbo.GetNoOfLeavesApplied(leave.leaveFrom,leave.leaveTo) as 'No.Of Leaves Applied',dbo.getBalanceLeave(leave.empid) as 'Current Leave Balance', " & _
                    " leave.leaveDesc as 'Leave Reason', CONVERT(VARCHAR(10),leave.leaveEntryDate,105) as 'Leave Applied On', " & _
                    " case when leavestatus='p' then 'Pending' when leavestatus='r' then 'Rejected' when leavestatus='a' then 'Approved' end as 'Leave Status',  " & _
                    " CONVERT(VARCHAR(10),leave.leaveSenctionedDate,105) as 'Leave Sanctioned Date', leave.leaveSanctionBy 'Leave Sanction By' , leave.leaveComment as 'Admin Comments' " & _
                    " from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid  "
        End If
        
        If (hdLocationId.Value <> "0") Then
            sql1 = sql1 & " and emp.LocationFKID = '" & hdLocationId.Value & "' "
        End If
        If (txtEmpName.Text <> "") Then
            sql1 = sql1 & " and emp.empName Like '" & txtEmpName.Text & "%'"
        End If
        If (txtFromDate.Text <> "") Then
            sql1 = sql1 & " and leave.leaveFrom >= '" & Convert.ToDateTime(txtFromDate.Text + " 00:00:00") & "' "
        End If
        If (txtToDate.Text <> "") Then
            sql1 = sql1 & " and leave.leaveFrom <= '" & Convert.ToDateTime(txtToDate.Text + " 23:59:00") & "' "
        End If
        sql1 = sql1 & " order by leave.leaveFrom desc "
        

        Dim dt As DataTable = New DataTable("table1")
        Dim dc As DataColumn = New DataColumn("SrNo", GetType(Int32))
        dc.AutoIncrement = True
        dc.AutoIncrementSeed = 1
        dt.Columns.Add(dc)
        'ds1.Tables.Add(dt)
        
        conn1.Open()

        Dim com As New SqlCommand(sql1, conn1)
        Dim da As New SqlDataAdapter(com)
        Dim ds As New DataSet()
        da.Fill(ds)
        ds.Tables.Add(dt)
        conn1.Close()
        
        
        If ds.Tables(0).Rows.Count > 0 Then
            Dim a As Boolean
            a = pnlColumns.Visible
            CheckBox1.Checked = False
            Me.ShowHideDialogueInternal(False)
            GetExel(ds)
        Else
            lblError.Visible = True
            lblError.Text = "No Data found"
        End If
        
        'Create a dummy GridView
        'Dim GridView1 As New GridView()
        'GridView1.AllowPaging = False
        'GridView1.DataSource = ds.Tables(0)
        'GridView1.DataBind()
        
        'Response.Clear()
        'Response.Buffer = True
        'Response.AddHeader("content-disposition", _
        '     "attachment;filename=DataTable.xls")
        'Response.Charset = ""
        'Response.ContentType = "application/vnd.ms-excel"
        'Dim sw As New StringWriter()
        'Dim hw As New HtmlTextWriter(sw)
 
        'For i As Integer = 0 To GridView1.Rows.Count - 1
        '    'Apply text style to each Row
        '    GridView1.Rows(i).Attributes.Add("class", "textmode")
        'Next
        'GridView1.RenderControl(hw)
 
        ''style to format numbers to string
        'Dim style As String = "<style> .textmode{mso-number-format:\@;}</style>"
        'Response.Write(style)
        'Response.Output.Write(sw.ToString())
        'Response.Flush()
        'Response.End()
        
        'Remove Panel
        CheckBox1.Checked = False
        Me.ShowHideDialogueInternal(False)
        'Me.ShowHideDialogueInternal(True)
    End Sub
    
    
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        'Me.ShowHideDialogueInternal(False)
        CheckBox1.Checked = False
        For Each item As ListItem In chkColumnsList.Items
            item.Selected = False
        Next
    End Sub
    Protected Sub btnCloseAddCoumns_Click(sender As Object, e As EventArgs)
        CheckBox1.Checked = False
        Me.ShowHideDialogueInternal(False)
    End Sub
    
    Protected Sub checkBox1_CheckedChanged(sender As Object, e As EventArgs)
        For Each item As ListItem In chkColumnsList.Items
            item.Selected = CheckBox1.Checked
        Next
    End Sub
    
    Protected Sub chkColumnsList_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim checkListCount As Integer = 0
        Dim counter As Integer = 0

        checkListCount = chkColumnsList.Items.Count()

        For Each chk In chkColumnsList.Items
            If chk.selected Then
                counter += 1
            End If
        Next

        If checkListCount <> 0 And counter <> 0 Then
            If checkListCount = counter Then
                CheckBox1.Checked = True
            Else
                CheckBox1.Checked = False
            End If
        End If

    End Sub
    Protected Sub GetExel(ByVal ds As DataSet)
        'Create a dummy GridView
        Dim GridView1 As New GridView()
        GridView1.AllowPaging = False
        GridView1.DataSource = ds.Tables(0)
        'GridView1.DataSource = ds
        GridView1.DataBind()

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", _
             "attachment;filename=DataTable.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        For i As Integer = 0 To GridView1.Rows.Count - 1
            'Apply text style to each Row
            GridView1.Rows(i).Attributes.Add("class", "textmode")
        Next
        GridView1.RenderControl(hw)

        'style to format numbers to string
        Dim style As String = "<style> .textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
        Me.ShowHideDialogueInternal(False)
    End Sub
    'added by Pravin on 15 May 2014 : End
</SCRIPT>


<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        $('#<%=emplrDataGrid.ClientID %>').Scrollable({
               ScrollHeight: 300,
               IsInUpdatePanel: true
           });
       });

    function popupLeaveDetails(Id, Status) {

        var location = document.getElementById('hdLocationId').value;
        var ename = document.getElementById('txtEmpName').value;
        var FDate = document.getElementById('txtFromDate').value;
        var TDate = document.getElementById('txtToDate').value;
        var win = window.open('leavedetails.aspx?empLeaveId=' + Id + "&Status=" + Status + "&Loc=" + location + "&Ename=" + ename + "&FDate=" + FDate + "&TDate=" + TDate, 'winWatch', 'scrollbars=yes,toolbar=no,menubar=no,location=center,resizable=no,width=650,height=370,left=50,top40');

        win.focus();
    }
    function ConfirmRptGen() {
        var CHK = document.getElementById("<%=chkColumnsList.ClientID%>");
        var checkbox = CHK.getElementsByTagName("input");
        var counter = 0;
        for (var i = 0; i < checkbox.length; i++) {
            if (checkbox[i].checked) {
                counter++;
            }
        }
        if (counter < 1) {
            //alert("Do you want report for All the Employees ?");
            //return true;
            if (confirm("Do you want report for All the Employees ?") == true) {
                return true;
            } else {
                return false;
            }
        }
    }

</script>
</head>
<body>
   
     <script language="JavaScript" src="../Includes/CalendarControl.js" type="text/javascript">
    </script>
<ucl:adminMenu ID="adminMenu" runat="server" />

<form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<table  cellpadding="0" width="100%" border="0" cellspacing="0">
  <tr>
     <td>
	    <table width="100%" border="0" cellpadding="0" cellspacing="0">
             
			<tr style="height:30px;">
						<td width="60%" align="left"  nowrap="nowrap"="true" font face="Verdana"  size="2"><b><font face="Verdana"  size="2" >|<a href="empLeaveRequests.aspx"><font color="#A2921E">Leave Requests</font></a>|<a href="empLeave.aspx"><font color="#A2921E">Annual Leaves</font></a>|</b>
						</td>
					
						<td width="40%" align="center"><b><font face="Verdana"  size="2" >
							<p align="right">|<asp:LinkButton ID="lnkAll" runat="server" OnClick="lnkAll_Click" Text="All" ForeColor="#A2921E"></asp:LinkButton></td>
							<td nowrap="nowrap"="true" align="left" >
							<b><font face="Verdana"  size="2" >
							<p align="right">|<asp:LinkButton ID="lnkPending" runat="server" OnClick="lnkPending_Click" Text="Pending" ForeColor="#A2921E"></asp:LinkButton></td>
							<td nowrap="nowrap"="true" align="left" >
							<b><font face="Verdana"  size="2" >
							<p align="right">|<asp:LinkButton ID="lnkApproved" runat="server" OnClick="lnkApproved_Click" Text="Approved" ForeColor="#A2921E"></asp:LinkButton></td>
							<td nowrap="nowrap"="true" align="left" >
							<b><font face="Verdana"  size="2" >
							<p align="right">|<asp:LinkButton ID="lnkRejected" runat="server" OnClick="lnkRejected_Click" Text="Rejected" ForeColor="#A2921E"></asp:LinkButton>|</b>
                           
						</td>
                
		  </tr>
				<tr>
                    <td colspan="2">
                        <table id="Table1" bordercolor="#c5d5ae" cellspacing="0" cellpadding="4" border="1"
                                        width="55%">
                                    <tr>
                                            <td bgcolor="#edf2e6" align="left" width="25%">
                                               <b><font face="Verdana" color="#a2921e" size="2"> <asp:Label ID="lblLocation" Text ="Location:" runat="server" Visible="false"/></font></b>
                                             </td>
                                             <td bgcolor="#edf2e6" align="left"  colspan="3">
                                                <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass="b_dropdown" OnSelectedIndexChanged="dlLocation_SelectedIndexChanged"  Visible="false" AppendDataBoundItems="true" Width="200px">
                                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <b><font face="Verdana" color="#a2921e" size="2"> <asp:Label ID="lblLocationId" runat="server" Visible="false"/></font></b>
                                             </td>
        
                                    </tr>
                                    <tr>
                                            <td bgcolor="#edf2e6" align="left" width="25%">
                                                <b><font face="Verdana" color="#a2921e" size="2">Employee Name</font></b>
                                            </td>
                                            <td width="75%" align="left" colspan="3">
                                                <asp:TextBox ID="txtEmpName" runat="server" Width="200px" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td class="textcolumn" align="left">
                                                Date
                                            </td>
                                            <td align="left">
                                                <font color="#a2921e" size="2" face="Verdana, Arial, Helvetica, sans-serif"><strong>
                                                    &nbsp;From&nbsp;</strong> </font>
                                                <asp:TextBox ID="txtFromDate" runat="server" size="7" Width="100px" onclick="popupCalender('txtFromDate');"
                                                    onkeypress="return false;"></asp:TextBox>
                                                <font color="#a2921e" size="2" face="Verdana, Arial, Helvetica, sans-serif"><strong>
                                                    &nbsp;To&nbsp;</strong></font>
                                                <asp:TextBox ID="txtToDate" runat="server" size="7" Width="100px" onclick="popupCalender('txtToDate');"
                                                    onkeypress="return false;"></asp:TextBox>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td align="left" width="75%" colspan="4" height="30" style="padding-left: 180px">
                                                <asp:Button ID="btnsearch" runat="server" Style="font-family: Verdana; font-size: 8pt;
                                                    color: #A2921E; font-weight: bold; background-color: #C5D5AE" Text="Search" OnClick="btnSearch_Click" OnClientClick="return CheckDates();" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnReset" runat="server" Style="font-family: Verdana; font-size: 8pt;
                                                    color: #A2921E; font-weight: bold; background-color: #C5D5AE" Text="Reset" OnClick="btnReset_Click"></asp:Button>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnColumnsMapping" runat="server" Text="Generate Report" Style="font-family: Verdana; font-size: 8pt;
                                                    color: #A2921E; font-weight: bold; background-color: #C5D5AE" OnClick="ShowHideDialogue" CommandArgument="show" />
                                            </td>
                                        </tr>
                            </table>
                    </td>
				</tr>
            <asp:HiddenField ID="hdnStatus" runat="server"/>
             <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
        </table>
     </td>
  </tr>
   
<tr>
    
<td>
    
<table border="1" cellpadding="2" cellspacing="0" style="border-collapse: collapse"  bordercolor="black" width="100%" id="AutoNumber1" height="1" >
    <tr>
						<td bgcolor="#edf2e6" height="42" width="11%" rowspan="2">
						<p align="center" >
                        <b>
                        <font face="Verdana" size="2" color="#A2921E" >Employee Name</font></td>
						<td bgcolor="#edf2e6" height="21" width="8%"  colspan="2">
						<p align="center" >
                        <b>
                        <font face="Verdana" size="2" color="#A2921E" >Leave</font></td>
						
					<td bgcolor="#edf2e6" height="42" width="6%" rowspan="2">
						<p align="center">
						<b><font face="Verdana" size="2" color="#A2921E">Leave<br /> 
                        Type</font></b></td>
						<td bgcolor="#edf2e6" height="42" width="6%" rowspan="2">
						<p align="center">
						<b><font face="Verdana" size="2" color="#A2921E">No.<br /> Of<br />
                        Leaves<br />Applied</font></b></td>
                        
					<td bgcolor="#edf2e6" height="42" width="8%" rowspan="2">
						<p align="center">
						<b><font face="Verdana" size="2" color="#A2921E">Current <br />
                        Leave<br />Balance </font></b></td>
					<td bgcolor="#edf2e6" height="42" width="10%"  rowspan="2">
						<p align="center">
						<b><font face="Verdana" size="2" color="#A2921E">Leave <br />
                        Reason</font></b></td>
						
					<td bgcolor="#edf2e6" height="42" width="9%"  rowspan="2">
						<p align="center">
						<b><font face="Verdana" size="2" color="#A2921E">Leave 
                        Applied <br />On</font></b></td>
						
					<td bgcolor="#edf2e6" height="42" width="7%"  rowspan="2">
						<p align="center">
						<b><font face="Verdana" size="2" color="#A2921E">Leave <br />
                        Status</font></b></td>
						
					<td bgcolor="#edf2e6" height="42" width="9%"  rowspan="2">
						<p align="center">
						<b><font face="Verdana" size="2" color="#A2921E">Leave <br />
                        Sanctioned <br />Date</font></b></td>
						
					<td bgcolor="#edf2e6" height="42" width="10%"  rowspan="2">
						<p align="center">
						<b><font face="Verdana" size="2" color="#A2921E">Leave</font></b>
                        <b><font face="Verdana" size="2" color="#A2921E">
                        Sanction <br />By</font></b></td>
						
					<td bgcolor="#edf2e6" height="42" width="10%"  rowspan="2">
						<p align="center">
						<b><font face="Verdana" size="2" color="#A2921E">Admin <br />
                        Comments</font></b></td>
						
					<td bgcolor="#edf2e6" height="42" width="11%"  rowspan="2">
						<p >
                        <b>
                        <font face="Verdana" size="2" color="#A2921E" >Show <br />Details</font></td>
                        <tr>
						<td bgcolor="#edf2e6" height="21" width="8%" nowrap="nowrap"="true" align="center">
						<b>
						<font face="Verdana" size="2" color="#A2921E">From</font></td>
						
						<td bgcolor="#edf2e6" height="21" width="8%" nowrap="nowrap"="true" align="center">
						<b>
						<font face="Verdana" size="2" color="#A2921E">To</font></td>
						
					    </tr>
    </tr>
</table>
</td>
</tr>
  <tr>
<td>
<ASP:DataGrid id="emplrDataGrid" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" OnPageIndexChanged="emplrDataGrid_PageIndexChanged" PagerStyle-Mode="NumericPages"
				 Font-Size="10pt" Font-Name="Verdana"  CellPadding="0" showheader="false" OnItemDataBound="emplrDataGrid_ItemDataBound"
				BorderColor="Black" BackColor="White" Width="100%" AllowSorting="True" Font-Names="Verdana">
				
				<AlternatingItemStyle BackColor="#edf2e6"></AlternatingItemStyle>
				<Columns>	 			
					
					<asp:BoundColumn visible="false" DataField="empLeaveId"/>
					<asp:BoundColumn DataField= "empname" >
					 <ItemStyle width="11%" HorizontalAlign="left" ></ItemStyle>
            </asp:BoundColumn>
						
					<asp:BoundColumn DataField= "leavefrom" DataFormatString="{0:dd-MMM-yy }">
					 <ItemStyle width="8%"  Wrap="false"></ItemStyle>
            </asp:BoundColumn>
						
					<asp:BoundColumn DataField= "leaveto" DataFormatString="{0:dd-MMM-yy }">
					 <ItemStyle width="8%" Wrap="false"></ItemStyle>
            </asp:BoundColumn>
			<asp:BoundColumn DataField="desc1">
			<ItemStyle width="6%" HorizontalAlign="Center"></ItemStyle>
			 </asp:BoundColumn>
             <asp:BoundColumn DataField="TotLeave">
			<ItemStyle width="6%"  HorizontalAlign="Center"></ItemStyle>
			 </asp:BoundColumn>
              <asp:BoundColumn DataField="balanceleave">
			<ItemStyle width="8%"></ItemStyle>
			 </asp:BoundColumn>
			<asp:BoundColumn DataField="leaveDesc">
			<ItemStyle width="10%" ></ItemStyle>
			 </asp:BoundColumn>
			 <asp:BoundColumn DataField="leaveEntryDate" DataFormatString="{0:dd-MMM-yy }">
			 <ItemStyle width="9%" ></ItemStyle>
			 </asp:BoundColumn>
			 <asp:BoundColumn DataField="ls">
			 <ItemStyle width="7%" Font-Bold="true" HorizontalAlign="center"></ItemStyle>
			 </asp:BoundColumn>
			 <asp:BoundColumn DataField="leaveSenctionedDate" DataFormatString="{0:dd-MMM-yy }">
			 <ItemStyle width="9%" Wrap="false"></ItemStyle>
			 </asp:BoundColumn>
			 <asp:BoundColumn DataField="leaveSanctionBy">
			 <ItemStyle width="10%" ></ItemStyle>
			 </asp:BoundColumn>
			 <asp:BoundColumn DataField="leaveComment">
			 <ItemStyle width="10%" ></ItemStyle>
				 </asp:BoundColumn>
			<asp:TemplateColumn HeaderText="">
          <ITEMSTYLE wrap="false" VerticalAlign="Middle"></ITEMSTYLE>
		  <ItemTemplate>
            <asp:Button id="leavedetails" runat="server" CommandName="show" Text="Show" 
			 style="font-family: Verdana; font-size: 8pt; color: #A2921E;  background-color: #C5D5AE" font-bold="true" Width="50" >
           </asp:Button>
		   <itemstyle width="10%"></itemstyle>
           </ItemTemplate>
       </asp:TemplateColumn>  
			</Columns>
			</ASP:DataGrid>

			</td>
       
			</tr>
    <tr><td align="left" >
           <asp:Label ID="lblNoData" Visible="false" runat="server"  ForeColor="Red" Text=""></asp:Label>
       </td>
        </tr>
        <tr>
        <td>
             <asp:Panel runat="server" ID="pnlColumns" Visible="false" CssClass="DialogueBackground">
            <div  class="Dialogue" id="divcolumnslistPopup">
                  <p><font size="3" face="Verdana, Arial, Helvetica, sans-serif"><strong>Select Employees</strong></font></p>
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                <table id="tbColumnsList" width="100%" class="manage_form">
                      <tr>
                        <td align="left">
                            <asp:CheckBox ID="CheckBox1" Font-Size="12px" Text="Select All" TextAlign="Right" Font-Bold="true" runat="server" Checked="false" AutoPostBack="true"  OnCheckedChanged="checkBox1_CheckedChanged"/>
                         
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:CheckBoxList ID="chkColumnsList" Font-Size="12px" OnSelectedIndexChanged="chkColumnsList_SelectedIndexChanged" RepeatDirection="Vertical" RepeatLayout="Flow" AutoPostBack="true" TextAlign="Right" runat="server" Height="100%"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr> <td  style=" height:18px;"></td> </tr>
                    <tr>
                        <td style=" text-align:center; margin-left: 40px;">
                           
                            <asp:Button ID="btnClear" Text="Clear" runat="server"  OnClick="btnClear_Click"  CssClass="small_button white_button open" CausesValidation="false" />
                            <asp:Button ID="btnAddColumns" Text="OK" runat="server" CssClass="small_button white_button open" OnClientClick="return ConfirmRptGen();" CausesValidation="false" />
                            <%--<asp:Button ID="btnClose" Text="Close" style="position:absolute;top:0;right:0;" runat="server"  OnClick="btnCloseAddCoumns_Click"  CausesValidation="false" />--%>
                           
                            <%--<asp:Button ID="btnAddColumns" Text="OK" runat="server" OnClick="btnAddColumns_Click" CssClass="small_button white_button open" CausesValidation="false" />
                            <asp:Button ID="btnClear" Text="Cancel" runat="server" OnClick="btnClear_Click" CssClass="small_button white_button open" CausesValidation="false" />--%>
                        </td>
                    </tr>
                    
                      <tr><td align="left">
                        <asp:Label ID="lblError" Visible="false" runat="server"  ForeColor="Red" Text=""></asp:Label>
                        </td></tr>
                </table>
                            </ContentTemplate>
                                <Triggers>
                                    
                                    <asp:AsyncPostBackTrigger ControlID="CheckBox1" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="chkColumnsList" EventName="SelectedIndexChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnAddColumns"/>
                                </Triggers>
                            </asp:UpdatePanel>
                 <asp:ImageButton ID="btnClose" style="position:absolute;top:0;right:0;"  runat="server" OnClick="btnCloseAddCoumns_Click" CausesValidation="false" ImageUrl="~/Images/delete_ic.png"></asp:ImageButton>
                 
            </div>
        </asp:Panel>

        </td>
    </tr>
	</table>  
</form>
</body>
</html>