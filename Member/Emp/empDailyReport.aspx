<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Project Daily Report</title>

    <script language="javascript">
	function Window_AddReport()
	{
		window.open('addNewReport.aspx','Window','scrollbars=1,width=750,height=500,top=60,resizable=yes');
	}
    </script>

    <script language="VB" runat="server">
        Dim strOrderBy As String
        Dim intProjId As Integer
        Dim sql As String
        Dim strSQL As String
        Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
        Dim strConn As SqlConnection = New SqlConnection(dsn)
        Dim objDataReader As SqlDataReader
        Dim objCmd As SqlCommand
        Dim empId As Integer
        Dim gridAdapter As SqlDataAdapter
        Dim gridDataset As DataSet
        Dim gf As New generalFunction
        '================================================================================
        'BIND DROPDOWN AND ACCORDING TO DROPDOWN SELECTION BIND THE GRID AT PAGE LOAD
        '=================================================================================
        Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            gf.checkEmpLogin()
            dgDailyReport.CurrentPageIndex = 0
            If Not IsPostBack Then
                empId = CInt(Session("dynoEmpIdSession"))
                If Session("dynoBugAdminSession") = 1 Then
                    BindDropDown(1)
                Else
                    BindDropDown(empId)
                End If
            End If
        End Sub
        '================================================================================
        'END BIND DROPDOWN AND ACCORDING TO DROPDOWN SELECTION BIND THE GRID AT PAGE LOAD
        '=================================================================================
	 
        '==================================================
        'BIND DROPDOWN FROM DATABASE AND DISPLAY PROJECTS
        '===================================================   	
        Sub BindDropDown(ByVal empId As Integer)
            ddlProj.Items.Clear()
            ddlProj.Items.Add(New ListItem("All", 0))
            If empId = 1 Then
                strSQL = "SELECT projName,projId FROM projectMaster ORDER BY projName"
            Else
                strSQL = "SELECT projName,projId FROM projectMaster WHERE projId IN(SELECT projId FROM " & _
                "projectMember WHERE empId=" & empId & ") OR projManager=" & empId & " ORDER BY projName"
            End If
            objCmd = New SqlCommand(strSQL, strConn)
            strConn.Open()
            objDataReader = objCmd.ExecuteReader
            If (objDataReader.HasRows) Then
                While objDataReader.Read
                    ddlProj.Items.Add(New ListItem(objDataReader("projName"), objDataReader("projId")))
                End While
            End If
            strConn.Close()
            objDataReader.Close()
            objCmd.Dispose()
            BindGrid()
        End Sub
        '=====================================================
        'END BIND DROPDOWN FROM DATABASE AND DISPLAY PROJECTS
        '======================================================
        '==========================================
        'BINDGRID FROM DATABASE
        '===========================================   	
        Sub BindGrid()
            Dim strSQL As String
            strConn.Open()
            strSQL = ""
            If CStr(Session("dynoEmpIdSession")) <> "" Then
                If ddlProj.SelectedItem.Value = 0 Then
                    If Session("dynoBugAdminSession") = 1 Then
                        strSQL = "select  * from  projDailyReport as p , employeeMaster as e,projectMaster as pm where p.reportEmpId = e.empId and p.projID=pm.projId order by reportDate desc"
                    ElseIf Session("dynoBugAdminSession") <> 1 Then
                        strSQL = "select * from projDailyreport,employeemaster,projectMaster where reportId in(select distinct(p.reportId) from projDailyReport as p , employeeMaster as e,projectMember as m,projectMaster as pm where p.reportEmpId = e.empId and p.projId = m.projId and p.projID=pm.projId and (m.empid=" & Session("dynoEmpIdSession") & " or pm.projmanager=" & Session("dynoEmpIdSession") & "))and  employeeMaster.empId =projDailyreport.reportEmpId and projDailyReport.projId=projectMaster.ProjId  order by reportDate desc"
                    End If
                Else
                    If Session("dynoBugAdminSession") = 1 Then
                        strSQL = "select  * from  projDailyReport as p , employeeMaster as e,projectMaster as pm where p.reportEmpId = e.empId and p.projID=pm.projId and p.projId=" & ddlProj.SelectedItem.Value & " order by reportDate desc"
                    ElseIf Session("dynoBugAdminSession") <> 1 Then
                        strSQL = "select * from projDailyreport,employeemaster,projectMaster where reportId in(select distinct(p.reportId) from projDailyReport as p , employeeMaster as e,projectMember as m,projectMaster as pm where p.reportEmpId = e.empId and p.projId = m.projId and p.projID=pm.projId and (m.empid=" & Session("dynoEmpIdSession") & " or pm.projmanager=" & Session("dynoEmpIdSession") & "))and  employeeMaster.empId =projDailyreport.reportEmpId and projDailyReport.projId=projectMaster.ProjId and projDailyReport.projId= " & ddlProj.SelectedItem.Value & " order by reportDate desc"
                    End If
                End If

                gridAdapter = New SqlDataAdapter(strSQL, strConn)
                gridDataset = New DataSet
                gridAdapter.Fill(gridDataset, "temp")
                dgDailyReport.DataSource = gridDataset
                dgDailyReport.DataBind()
            End If
        End Sub
        '==========================================
        'END BINDGRID FROM DATABASE
        '===========================================   
        '==========================================
        'BINDGRID AT DROPDOWN ITEM SELECTION CHANGE 
        '===========================================   
        Sub ddlProj_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            BindGrid()
        End Sub
        '===================================================
        'END CODE BINDGRID AT DROPDOWN ITEM SELECTION CHANGE 
        '=====================================================  
        '=====================================
        'CALL BINDGRID FOR PAGING 
        '======================================
        Sub dgDailyReport_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
            dgDailyReport.CurrentPageIndex = e.NewPageIndex
            Session("pageindex") = dgDailyReport.CurrentPageIndex
            bindData()
        End Sub
        '=====================================
        'CALL BINDGRID FOR PAGING 
        '======================================
        '=====================================
        'THIS CODE FOR GRID ITEM SORTING
        '======================================
        Sub dgDailyReport_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
            'Check to see if same column clicked again
            If e.SortExpression.ToString() = Session("Column") Then
                'Reverse the sort order
                If Session("Order") = "ASC" Then
                    Me.ViewState("strOrderBy") = e.SortExpression.ToString() & " DESC"
                    Session("Order") = "DESC"
                Else
                    strOrderBy = e.SortExpression.ToString() & " ASC"
                    Session("Order") = "ASC"
                End If
            Else
                'Different column selected, so default to ascending order
                Me.ViewState("strOrderBy") = e.SortExpression.ToString() & " ASC"
                Session("Order") = "ASC"
            End If
            Session("Column") = e.SortExpression.ToString()
            bindData()
        End Sub
        '=====================================
        'END CODE FOR GRID ITEM SORTING
        '======================================
        '=====================================
        'BINDDATA FOR SORTING BY COLUMN NAME
        '======================================
        Sub bindData()
            intProjId = ddlProj.SelectedItem.Value
            Dim strSQL As String
            strConn.Open()
            strSQL = ""
            If ddlProj.SelectedItem.Value = 0 Then
                If Session("dynoBugAdminSession") = 1 Then
                    strSQL = "select  * from  projDailyReport as p , employeeMaster as e,projectMaster as pm where p.reportEmpId = e.empId and p.projID=pm.projId"
                ElseIf Session("dynoBugAdminSession") <> 1 Then
                    strSQL = "select * from  projDailyReport as p , employeeMaster as e,projectMember as  m,projectMaster as pm where p.reportEmpId = e.empId  and p.projId  =  m.projId and p.projID=pm.projId and m.empid=" & Session("dynoEmpIdSession") & ""
                End If
            Else
                If Session("dynoBugAdminSession") = 1 Then
                    strSQL = "select  * from  projDailyReport as p , employeeMaster as e,projectMaster as pm where p.reportEmpId = e.empId and p.projID=pm.projId and p.projId=" & ddlProj.SelectedItem.Value & ""
                ElseIf Session("dynoBugAdminSession") <> 1 Then
                    strSQL = "select * from  projDailyReport as p , employeeMaster as e,projectMember as  m,projectMaster as pm where p.reportEmpId = e.empId  and p.projId  =  m.projId and p.projID=pm.projId and m.empid=" & Session("dynoEmpIdSession") & " and p.projId=" & ddlProj.SelectedItem.Value & ""
                End If
					   					
            End If
					
            gridAdapter = New SqlDataAdapter(strSQL, strConn)
            gridDataset = New DataSet
            gridAdapter.Fill(gridDataset, "temp")
            Dim dtEmployee As DataTable = gridDataset.Tables("temp")

            Dim dv As New DataView(dtEmployee)
            dv.Sort = Me.ViewState("strOrderBy")
            dgDailyReport.DataSource = dv
            dgDailyReport.DataBind()
        End Sub
        '=====================================
        'END CODE BINDDATA FOR SORTING BY COLUMN NAME
        '======================================
        '======================================================================
        ' SELECTED ITEM CHANGE DETAILS
        '======================================================================
        Sub dgDailyReport_SelectedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim intId As Integer = dgDailyReport.SelectedItem.Cells.Item(6).Text
            hdnid.Value = intId
	       
            Dim sp As String
            sp = "<script language=JavaScript>window.open('empDailyDetails.aspx?repId='+ " & intId & ")" & "<" & "/script>"
            sp = "<Script language=JavaScript>"
            sp += " window.open('empDailyDetails.aspx?repId='+ document.Form1.hdnid.value ,'Window','width=600,height=530,left=320,top=130,resizable=yes,scrollbars=yes');"
            sp += "</" + "script>"
            RegisterStartupScript("script123", sp)
            dgDailyReport.CurrentPageIndex = Session("pageindex")
        End Sub

    </script>

</head>
<body>
    <form id="Form1" runat="server">
        <table height="0" cellspacing="0" cellpadding="4" width="100%" border="0" style="border-collapse: collapse"
            bordercolor="#111111" align="center">
            <tr>
                <td>
                    <table id="Table3" cellspacing="0" cellpadding="2" width="100%" border="0" height="1">
                        <tr>
                            <td>
                                <EMPHEADER:empHeader ID="Empheader" runat="server"></EMPHEADER:empHeader>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:empMenuBar ID="EmpMenuBar" runat="server"></uc1:empMenuBar>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="1" cellspacing="1" bordercolorlight="#000000" bordercolordark="#FFFFFF">
                        <tr>
                            <td align="center" bgcolor="#C5D5AE" colspan="9">
                                <a name="Search"><font face="Verdana" color="#a2921e"><b>Search</b></font></a>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#edf2e6" align="left">
                                <font face="Verdana" color="#a2921e" size="2"><b>Project</b></font>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProj" runat="server" AutoPostBack="true" Width="250" OnSelectedIndexChanged="ddlProj_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <a href="#" onclick="javascript:Window_AddReport();"><font style="font-size: 10pt;
                                    color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica; font-weight: bold">
                                    Add New Report</font></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="1" cellspacing="1" bordercolorlight="#000000" bordercolordark="#FFFFFF"
                        width="100%">
                        <tr>
                            <td align="center" bgcolor="#C5D5AE">
                                <font face="Verdana" color="#a2921e"><b>Project Report</b></font></td>
                        </tr>
                    </table>
                    <asp:DataGrid ID="dgDailyReport" runat="server" BorderColor="Black" Font-Size="10pt"
                        Font-Name="Verdana" BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"
                        FooterStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray"
                        CellPadding="2" ShowFooter="True" Width="100%" AllowPaging="True" PageSize="20"
                        AllowSorting="True" OnPageIndexChanged="dgDailyReport_PageIndexChanged" OnSortCommand="dgDailyReport_SortCommand"
                        OnSelectedIndexChanged="dgDailyReport_SelectedChanged">
                        <ItemStyle ForeColor="#000000" BackColor="#FFFFEE" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100px">
                        </HeaderStyle>
                        <FooterStyle ForeColor="#a2921e" BackColor="#edf2e6"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn DataField="projName" HeaderText="Project" SortExpression="projName"
                                HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="25%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="reportDate" HeaderText="Report Date" SortExpression="reportDate"
                                DataFormatString="{0:dd-MMM-yy}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="reportSubject" HeaderText="Report Title" SortExpression="reportSubject"
                                HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="20%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="reportLastModified" HeaderText="Last Modified" SortExpression="reportLastModified"
                                DataFormatString="{0:dd-MMM-yy}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="empName" HeaderText="Report By" SortExpression="empName"
                                HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:ButtonColumn Text="Detail" CommandName="Select" ItemStyle-ForeColor="#a2921e"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:ButtonColumn>
                            <asp:BoundColumn DataField="reportId" HeaderText="Report Id" Visible="false"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Left" ForeColor="#003399" BackColor="#C5D5AE" PageButtonCount="5"
                            Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
            <tr>
                <td>
                    <input type="hidden" id="hdnMsg" runat="server">
                    <input type="hidden" id="hdnid" runat="server">
                </td>
                <tr>
        </table>
    </form>
</body>
</html>
