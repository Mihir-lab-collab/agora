<%@ Page Language="VB" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>

<script language="JavaScript" type="text/javascript">
     function popupProjectDetail(Id)
    {
   
     var win = window.open('view_complaints.aspx?id=' + Id  ,'winWatch','scrollbars=yes,toolbar=no,menubar=no,location=right,width=850,height=550,left=100,top=50');
    win.focus(); 
    } 
    

</script>

<script language="VB" runat="server">
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim Conn As SqlConnection = New SqlConnection(dsn)
    Dim Cmd As New SqlCommand()
    Dim SortField As String
    Dim cust_name, cust_add, comp_name, cust_email, cust_regdate, cust_prjname As String
    Dim gf As New generalFunction
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        gf.checkEmpLogin()
        Dim sql As String
        Conn.Open()
        Dim search As Integer
 	 
	
        If dd_prj_name.SelectedValue <> "" Then
            search = dd_prj_name.SelectedValue
        End If
	
        If search <> -1 Then
            Dim cmdProject As SqlCommand
            Dim dtrProject As SqlDataReader
            sql = "select * from Customermaster ,projectMaster where Customermaster.custid=projectMaster.custid and projectMaster.projid=" & search
            cmdProject = New SqlCommand(sql, Conn)
            dtrProject = cmdProject.ExecuteReader()
	
            Do While (dtrProject.Read())
                cust_name = dtrProject("custname").ToString()
                cust_add = dtrProject("custAddress").ToString()
                comp_name = dtrProject("custCompany").ToString()
                cust_email = dtrProject("custemail").ToString()
                cust_regdate = dtrProject("custregdate").ToString()
                cust_prjname = dtrProject("projname").ToString()
            Loop
            dtrProject.Close()
        End If
        Conn.Close()
	
        If Not IsPostBack Then
            fillProject()
            fillCategories()
            ddlcategory.SelectedValue = "-1"
            BindGrid()
        End If

    End Sub

    '***********************************************************************
    'To fill the dropdown list of project Name
    '***********************************************************************
    Sub fillProject()
        Dim Con As SqlConnection
        Dim cmdProject As SqlCommand
        Dim strProject As String
        Dim dtrProject As SqlDataReader
        Con = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
	
        '-------------------------------------------------------
        'fill dropdownlist-projects
        '------------------------------------------------------
	
        strProject = "select projId , projName from projectMaster order by projName"
        Con.Open()
        cmdProject = New SqlCommand(strProject, Con)
        dtrProject = cmdProject.ExecuteReader()
        'ddcust_name.Items.Clear()
        dd_prj_name.Items.Add(New ListItem("All", -1))
        Do While (dtrProject.Read())
            dd_prj_name.Items.Add(New ListItem(dtrProject("projName").ToString(), dtrProject("projId").ToString()))
        Loop
        dtrProject.Close()
        cmdProject.Dispose()
        Con.Close()
	
    End Sub
    '****************************************************************************
    'End of dropdown list code
    '****************************************************************************

    '****************************************************************************
    ' To bind the Grid with the data
    '****************************************************************************
    Sub BindGrid()
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Cmd.Connection = Conn
        Dim strSQL As String = String.Empty
        Dim sql As String = String.Empty
        Conn.Open()
        Dim search As Integer
        Dim cat As String = String.Empty
        cat = ddlcategory.SelectedValue
	
        If dd_prj_name.SelectedValue <> "" Then
            search = dd_prj_name.SelectedValue
	
            Dim cmdProject As SqlCommand
            Dim dtrProject As SqlDataReader
            sql = "select * from Customermaster ,projectMaster where Customermaster.custid=projectMaster.custid and projectMaster.projid=" & search
            cmdProject = New SqlCommand(sql, Conn)
            dtrProject = cmdProject.ExecuteReader()
	
            Do While (dtrProject.Read())
                cust_name = dtrProject("custname").ToString()
                cust_add = dtrProject("custAddress").ToString()
                comp_name = dtrProject("custCompany").ToString()
                cust_email = dtrProject("custemail").ToString()
                cust_regdate = dtrProject("custregdate").ToString()
                cust_prjname = dtrProject("projname").ToString()
            Loop
            dtrProject.Close()
     
            If search = -1 And cat = "-1" Then

                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                "custcomplaints,ProjectMaster where custcomplaints.compProjId=ProjectMaster.projId" & " order by custcomplaints.compDate desc"

            End If

            If search <> -1 And cat = "-1" Then
	
                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                   "custcomplaints,ProjectMaster where custcomplaints.compProjId=" & search & " and projectMaster.projId=" & search & " order by custcomplaints.compDate desc"
	
            End If
	
            If search = -1 And cat <> "-1" Then

                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                "custcomplaints,ProjectMaster where  custcomplaints.compCategory = " & "'" & cat & "'" & " and custcomplaints.compProjId=ProjectMaster.projId" & " order by custcomplaints.compDate desc"

            End If
	
            If search <> -1 And cat <> "-1" Then

                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                "custcomplaints,ProjectMaster where  custcomplaints.compCategory = " & "'" & cat & "'" & " and custcomplaints.compProjId=" & search & " and projectMaster.projId=" & search & " order by custcomplaints.compDate desc"

            End If
		
            Cmd.CommandText = strSQL
            da = New SqlDataAdapter(strSQL, Conn)
            ds = New DataSet()
	    
            da.Fill(ds)
		
            MyDataGrid.DataSource = ds
            MyDataGrid.DataBind()
        End If
    End Sub

    '**********************************************************************************
    ' End of the bindgrid code
    '**********************************************************************************





    '****************************************************************************
    ' To bind the Grid with the data for resolved complaints
    '****************************************************************************
    Sub BindGridResolved()
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Cmd.Connection = Conn
        Dim strSQL As String = String.Empty
        Dim sql As String
        Conn.Open()
        Dim search As Integer
        Dim cat As String
        cat = ddlcategory.SelectedValue
	
        If dd_prj_name.SelectedValue <> "" Then
            search = dd_prj_name.SelectedValue
	
            Dim cmdProject As SqlCommand
            Dim dtrProject As SqlDataReader
            sql = "select * from Customermaster ,projectMaster where Customermaster.custid=projectMaster.custid and projectMaster.projid=" & search
            cmdProject = New SqlCommand(sql, Conn)
            dtrProject = cmdProject.ExecuteReader()
	
            Do While (dtrProject.Read())
                cust_name = dtrProject("custname").ToString()
                cust_add = dtrProject("custAddress").ToString()
                comp_name = dtrProject("custCompany").ToString()
                cust_email = dtrProject("custemail").ToString()
                cust_regdate = dtrProject("custregdate").ToString()
                cust_prjname = dtrProject("projname").ToString()
            Loop
            dtrProject.Close()
     
            If search = -1 And cat = "-1" Then

                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                "custcomplaints,ProjectMaster where custcomplaints.compresolved = 1 and custcomplaints.compProjId=ProjectMaster.projId" & " order by custcomplaints.compDate desc"

            End If

            If search <> -1 And cat = "-1" Then
	
                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                   "custcomplaints,ProjectMaster where custcomplaints.compresolved = 1 and custcomplaints.compProjId=" & search & " and projectMaster.projId=" & search & " order by custcomplaints.compDate desc"
	
            End If
	
            If search = -1 And cat <> "-1" Then

                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                "custcomplaints,ProjectMaster where  custcomplaints.compresolved = 1 and custcomplaints.compCategory = " & "'" & cat & "'" & " and custcomplaints.compProjId=ProjectMaster.projId" & " order by custcomplaints.compDate desc"

            End If
	
            If search <> -1 And cat <> "-1" Then

                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                "custcomplaints,ProjectMaster where  custcomplaints.compresolved = 1 and custcomplaints.compCategory = " & "'" & cat & "'" & " and custcomplaints.compProjId=" & search & " and projectMaster.projId=" & search & " order by custcomplaints.compDate desc"

            End If
		
            Cmd.CommandText = strSQL
            da = New SqlDataAdapter(strSQL, Conn)
            ds = New DataSet()
	    
            da.Fill(ds)
		
            MyDataGrid.DataSource = ds
            MyDataGrid.DataBind()
        End If
    End Sub

    '**********************************************************************************
    ' End of the bindgrid for resolved complaints
    '**********************************************************************************



    '****************************************************************************
    ' To bind the Grid with the data for Not resolved complaints
    '****************************************************************************
    Sub BindGridNResolved()
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Cmd.Connection = Conn
        Dim strSQL As String = String.Empty
        Dim sql As String = ""
        Conn.Open()
        Dim search As Integer
        Dim cat As String
        cat = ddlcategory.SelectedValue
	
        If dd_prj_name.SelectedValue <> "" Then
            search = dd_prj_name.SelectedValue
	
            Dim cmdProject As SqlCommand
            Dim dtrProject As SqlDataReader
            sql = "select * from Customermaster ,projectMaster where Customermaster.custid=projectMaster.custid and projectMaster.projid=" & search
            cmdProject = New SqlCommand(sql, Conn)
            dtrProject = cmdProject.ExecuteReader()
	
            Do While (dtrProject.Read())
                cust_name = dtrProject("custname").ToString()
                cust_add = dtrProject("custAddress").ToString()
                comp_name = dtrProject("custCompany").ToString()
                cust_email = dtrProject("custemail").ToString()
                cust_regdate = dtrProject("custregdate").ToString()
                cust_prjname = dtrProject("projname").ToString()
            Loop
            dtrProject.Close()
     
            If search = -1 And cat = "-1" Then

                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                "custcomplaints,ProjectMaster where custcomplaints.compresolved = 0 and custcomplaints.compProjId=ProjectMaster.projId" & " order by custcomplaints.compDate desc"

            End If

            If search <> -1 And cat = "-1" Then
	
                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                   "custcomplaints,ProjectMaster where custcomplaints.compresolved = 0 and custcomplaints.compProjId=" & search & " and projectMaster.projId=" & search & " order by custcomplaints.compDate desc"
	
            End If
	
            If search = -1 And cat <> "-1" Then

                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                "custcomplaints,ProjectMaster where  custcomplaints.compresolved = 0 and custcomplaints.compCategory = " & "'" & cat & "'" & " and custcomplaints.compProjId=ProjectMaster.projId" & " order by custcomplaints.compDate desc"

            End If
	
            If search <> -1 And cat <> "-1" Then

                strSQL = "SELECT custcomplaints.compId,custcomplaints.compDate,custcomplaints.comptitle,custcomplaints.compDesc,custcomplaints.compProjId,custcomplaints.compresolved,custcomplaints.compCategory,projectMaster.projName from  " & _
                "custcomplaints,ProjectMaster where  custcomplaints.compresolved = 0 and custcomplaints.compCategory = " & "'" & cat & "'" & " and custcomplaints.compProjId=" & search & " and projectMaster.projId=" & search & " order by custcomplaints.compDate desc"

            End If
		
            Cmd.CommandText = strSQL
            da = New SqlDataAdapter(strSQL, Conn)
            ds = New DataSet()
	  
            da.Fill(ds)
		
            MyDataGrid.DataSource = ds
            MyDataGrid.DataBind()
        End If
    End Sub

    '**********************************************************************************
    ' End of the bindgrid for Not resolved complaints
    '**********************************************************************************

    Sub dd_prj_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        BindGrid()
    End Sub

    Sub ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)

        Dim id As Integer
        id = e.Item.Cells(0).Text
 
        '******************************************************************
        'check is it view click event
        '******************************************************************
        If e.CommandName = "delete" Then
      
            Dim sp As String
            sp = "<Script language=JavaScript>"
            sp += " val= confirm('Do you really want to delete this record?');"
            sp += " if (val==true)  "
            sp += " window.location.href = 'delete_complaint.aspx?ID=" & id & "';"
            sp += "</" + "script>"
            ClientScript.RegisterStartupScript(Me.GetType, "script123", sp)

        End If
    End Sub


    Private Sub btnAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindGrid()
    End Sub


    Private Sub btnResolved_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindGridResolved()
    End Sub


    Private Sub btnNResolved_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindGridNResolved()
    End Sub


    Private Sub ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If (e.Item.Cells(6).Text = "True") Then
            e.Item.Cells(6).Text = "Yes"
            e.Item.Cells(6).ForeColor = System.Drawing.Color.Blue
        End If
        If (e.Item.Cells(6).Text = "False") Then
            e.Item.Cells(6).Text = "No"
            e.Item.Cells(6).ForeColor = System.Drawing.Color.Red
        End If
		
        If (e.Item.Cells(5).Text <> "Category") Then
            Dim Con As SqlConnection
            Dim cmdCat As SqlCommand
            Dim strCat As String
            Dim dtrCat As SqlDataReader
            Try
                Con = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
                strCat = ""
                strCat = "select * from compCategoryMaster where id = " & e.Item.Cells(5).Text
                Con.Open()
                cmdCat = New SqlCommand(strCat, Con)
                dtrCat = cmdCat.ExecuteReader()
                If (dtrCat.Read()) Then
                    e.Item.Cells(5).Text = dtrCat("compCategory").ToString()
                End If
                cmdCat.Dispose()
                Con.Close()
            Catch
            End Try
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim view As Button = CType(e.Item.Cells(8).FindControl("view"), Button)
    		
            view.Attributes.Add("onclick", "popupProjectDetail(" & e.Item.Cells(0).Text & "); return false; ")
        End If
    End Sub
 
    Sub fillCategories()
        Dim Con As SqlConnection
        Dim cmdCat As SqlCommand
        Dim strCat As String
        Dim dtrCat As SqlDataReader
        Con = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        strCat = "select * from compCategoryMaster"
        Con.Open()
        cmdCat = New SqlCommand(strCat, Con)
        dtrCat = cmdCat.ExecuteReader()
        ddlcategory.Items.Clear()
        Do While (dtrCat.Read())
            ddlcategory.Items.Add(New ListItem(dtrCat("compCategory").ToString(), dtrCat("id").ToString()))
        Loop
        ddlcategory.Items.Add(New ListItem("All", "-1"))
        cmdCat.Dispose()
        Con.Close()
    End Sub

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta name="GENERATOR" content="Microsoft FrontPage 5.0" />
    <meta name="ProgId" content="FrontPage.Editor.Document" />
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <title>Show Complaint</title>
</head>
<body>
    <form id="Form1" runat="server" method="post">
        <input type="hidden" id="empId" runat="server" size="20" />
        <table cellpadding="4" width="100%" border="1" style="border-collapse: collapse;
            border-color: #E8E8E8">
            <ucl:adminMenu ID="adminMenu" runat="server" />
        </table>
        <table cellpadding="4" width="100%" border="0" style="border-collapse: collapse;
            border-color: #E8E8E8">
            <tr>
                <td height="12" width="100%" colspan="2" bgcolor="#edf2e6">
                    <b><font face="Verdana" color="#A2921E">Customer Complaints</font></b></td>
            </tr>
            <tr>
                <td align="left" bgcolor="#edf2e6">
                    <b><font color="#FF0000" face="Verdana" size="2">&nbsp;</font><font face="Verdana"
                        size="2" color="#A2921E">Project Name :&nbsp;</font></b><asp:DropDownList ID="dd_prj_name"
                            runat="server" CssClass="cssData" AutoPostBack="True" OnSelectedIndexChanged="dd_prj_name_SelectedIndexChanged">
                        </asp:DropDownList><u><font color="#FF0000"><b> </b></font></u><font color="#FF0000"
                            face="Verdana" size="2">&nbsp;</font><b><font face="Verdana" size="2" color="#A2921E">Complaint
                                Category :&nbsp;</font></b><asp:DropDownList ID="ddlcategory" runat="server" CssClass="cssData"
                                    AutoPostBack="True" OnSelectedIndexChanged="dd_prj_name_SelectedIndexChanged"
                                    Width="220">
                                </asp:DropDownList><u><font color="#FF0000"><b> </b></font></u>
                    <asp:Button ID="btnAll" runat="server" Text=" All " OnClick="btnAll_Click" Style="font-family: Verdana;
                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"></asp:Button>
                    <asp:Button ID="btnResolved" runat="server" Text="Resolved" OnClick="btnResolved_Click"
                        Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                        background-color: #C5D5AE"></asp:Button>
                    <asp:Button ID="btnNResolved" runat="server" Text="Not Resolved" Width="100px" OnClick="btnNResolved_Click"
                        Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                        background-color: #C5D5AE"></asp:Button>
                </td>
            </tr>
        </table>
        <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse;
            border-color: #EAEAEA; height: 1" width="100%" id="AutoNumber1">
            <%If dd_prj_name.SelectedValue <> -1 Then%>
            <tr>
                <td width="18%" height="22" valign="top">
                    <font face="Verdana" size="2">&nbsp;Customer Name:</font></td>
                <td width="28%" height="22" colspan="3" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(cust_name)%>
                    </font>
                </td>
                <td width="22%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;Customer E-mail</font></td>
                <td width="44%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(cust_email)%>
                    </font>
                </td>
            </tr>
            <tr>
                <td width="18%" height="22" valign="top">
                    <font face="Verdana" size="2">&nbsp;Customer Company :</font></td>
                <td width="28%" height="22" colspan="3" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(comp_name)%>
                    </font>
                </td>
                <td width="22%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;Project Name :</font></td>
                <td width="44%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(cust_prjname)%>
                    </font>
                </td>
            </tr>
            <tr>
                <td width="18%" height="22" valign="top">
                    <font face="Verdana" size="2">&nbsp;Customer Address :</font></td>
                <td width="28%" height="22" colspan="3" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(cust_add)%>
                    </font>
                </td>
                <td width="22%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;Registration Date :</font></td>
                <td width="44%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(Day(cust_regdate) & "-" & Left(MonthName(Month(cust_regdate)), 3) & "-" & Year(cust_regdate))%>
                    </font>
                </td>
            </tr>
        </table>
        <%End If%>
        <asp:DataGrid ID="MyDataGrid" runat="server" CellPadding="2" CellSpacing="0" Font-Name="Verdana"
            Font-Size="10pt" MaintainState="true" AutoGenerateColumns="False" OnItemCommand="ItemCommand"
            OnItemDataBound="ItemDataBound" BorderColor="Black" BackColor="White" Width="100%"
            AllowSorting="True" Font-Names="Verdana">
            <HeaderStyle Font-Size="11pt" BackColor="#edf2e6" ForeColor="#A2921E" Font-Bold="True">
            </HeaderStyle>
            <AlternatingItemStyle BackColor="#edf2e6"></AlternatingItemStyle>
            <Columns>
                <asp:BoundColumn Visible="false" DataField="compId" HeaderText="CompId"></asp:BoundColumn>
                <asp:BoundColumn DataField="compDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yy}">
                    <HeaderStyle Width="11%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="projName" HeaderText="Project Name">
                    <HeaderStyle Width="22%" HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="compTitle" HeaderText="Title">
                    <HeaderStyle Width="35%" HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn Visible="false" DataField="compProjId" HeaderText="projID"></asp:BoundColumn>
                <asp:BoundColumn DataField="compcategory" HeaderText="Category">
                    <HeaderStyle Width="19%" HorizontalAlign="Left" VerticalAlign="middle"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="compresolved" HeaderText="Resolved">
                    <HeaderStyle Width="13%" HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="">
                    <ItemStyle Width="12%" Wrap="false"></ItemStyle>
                    <ItemTemplate>
                        <asp:Button ID="view" runat="server" CommandName="view" Text="View" align="right"
                            Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                            background-color: #C5D5AE" Font-Bold="true"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="">
                    <ItemStyle Width="18%" Wrap="false" HorizontalAlign="center" VerticalAlign="middle">
                    </ItemStyle>
                    <ItemTemplate>
                        <asp:Button ID="delete" runat="server" CommandName="delete" Text="Delete" align="right"
                            Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                            background-color: #C5D5AE" Font-Bold="true"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </form>
</body>
</html>
