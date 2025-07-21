<%@ Page Language="VB" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>

<script language="javascript" type="text/javascript">
     function popupProjectDetail(Id)
    {
   
     var win = window.open('view_feedback.aspx?id=' + Id  ,'winWatch','scrollbars=yes,toolbar=no,menubar=no,location=right,width=850,height=550,left=100,top=50');
    win.focus(); 
    } 
    

</script>

<script language="VB" runat="server">
	Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	Dim Conn As SqlConnection = New SqlConnection(dsn)
	Dim Cmd As New SqlCommand()
	Dim SortField As String
	Dim cust_name, cust_add, comp_name, cust_email, cust_regdate, cust_prjname
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
            'Dim strProject As String
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
		If Page.IsPostBack = "False" Then
			fillProject()
			BindGrid()
		End If
	End Sub
    
	Sub MyDataGrid_Sort(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
	
	End Sub
	Sub fillProject()
		Dim Con As SqlConnection
		Dim cmdProject As SqlCommand
		Dim strProject As String
		Dim dtrProject As SqlDataReader
		Con = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
	
		'-------------------------------------------------------
		'fill Second dropdownlist-projects
		'------------------------------------------------------
	
		strProject = "select projId , projName from projectMaster order by projName"
		Con.Open()
		cmdProject = New SqlCommand(strProject, Con)
		dtrProject = cmdProject.ExecuteReader()
		If dd_prj_name.Items.Count = 0 Then
			dd_prj_name.Items.Add(New ListItem("All", -1))
			Do While (dtrProject.Read())
				dd_prj_name.Items.Add(New ListItem(dtrProject("projName").ToString(), dtrProject("projId").ToString()))
			Loop
		End If
		dtrProject.Close()
	
		cmdProject.Dispose()
		Con.Close()
	
	End Sub

	Sub BindGrid()
		Dim da As SqlDataAdapter
		Dim ds As DataSet
		Cmd.Connection = Conn
		Dim strSQL As String
        Dim sql As String
		Conn.Open()
		Dim search As Integer
 	
		If dd_prj_name.SelectedValue <> "" Then
			search = dd_prj_name.SelectedValue
	
			If search = -1 Then
				strSQL = "select custfeedback.Id,custfeedback.projId,custfeedback.feedback_date,custfeedback.technical_rt,custfeedback.creativity_rt,custfeedback.timeliness_rt,custfeedback.future_interest,custfeedback.status,custfeedback.communication_rt,custfeedback.feedback,projectMaster.projName from  " & _
				"custfeedback,projectMaster where custfeedback.projId=projectMaster.projId" & " order by custfeedback.feedback_date desc"

			Else
				Dim cmdProject As SqlCommand
                'Dim strProject As String
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
     
				strSQL = "select custfeedback.Id,custfeedback.projId,custfeedback.feedback_date,custfeedback.technical_rt,custfeedback.creativity_rt,custfeedback.timeliness_rt,custfeedback.status,custfeedback.future_interest,custfeedback.communication_rt,custfeedback.feedback,projectMaster.projName from  " & _
				"custfeedback,projectMaster where custfeedback.projId=" & search & "and projectMaster.projId =" & search & "order by feedback_date desc"
		
			End If
			Cmd.CommandText = strSQL
			da = New SqlDataAdapter(strSQL, Conn)
			ds = New DataSet()
	    
			da.Fill(ds)
			Dim i, rt1, rt2, rt3, rt4 As Integer
            'Dim my
			Dim technical As New DataColumn("technical", GetType(String))
			ds.Tables(0).Columns.Add(technical)
			Dim creativity As New DataColumn("creativity", GetType(String))
			ds.Tables(0).Columns.Add(creativity)
			Dim timeliness As New DataColumn("timeliness", GetType(String))
			ds.Tables(0).Columns.Add(timeliness)
			Dim comm As New DataColumn("comm", GetType(String))
			ds.Tables(0).Columns.Add(comm)
       
			For i = 0 To ds.Tables(0).Rows.Count - 1
				rt1 = ds.Tables(0).Rows(i).Item("technical_rt")
				rt2 = ds.Tables(0).Rows(i).Item("creativity_rt")
				rt3 = ds.Tables(0).Rows(i).Item("timeliness_rt")
				rt4 = ds.Tables(0).Rows(i).Item("communication_rt")
		     
				ds.Tables(0).Rows(i).Item("technical") = Mid("*****", 1, rt1)
				ds.Tables(0).Rows(i).Item("creativity") = Mid("*****", 1, rt2)
				ds.Tables(0).Rows(i).Item("timeliness") = Mid("*****", 1, rt3)
				ds.Tables(0).Rows(i).Item("comm") = Mid("*****", 1, rt4)
			Next
	
			MyDataGrid.DataSource = ds
			MyDataGrid.DataBind()
		End If
	End Sub

	Dim sum As Double
	Dim sum_price As Double

	Sub ddcust_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
		BindGrid()
	End Sub
	Sub dd_prj_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
		BindGrid()
	End Sub
	Private Sub ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)

		If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
			Dim view As Button = CType(e.Item.Cells(12).FindControl("view"), Button)
    		
			view.Attributes.Add("onclick", "popupProjectDetail(" & e.Item.Cells(0).Text & "); return false; ")
		End If


	End Sub

	Sub ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)

		Dim id As Integer
		id = e.Item.Cells(0).Text
 
		'******************************************************************
		'check is it view click or delete click event
		'******************************************************************
		If e.CommandName = "delete" Then
      
			Dim sp As String
			sp = "<Script language=JavaScript>"
            sp += " val= confirm('Do you really want to delete this record?');"
			sp += " if (val==true)  "
            sp += " window.location.href = 'delete_feedback.aspx?ID=" & id & "';"
			sp += "</" + "script>"
            ClientScript.RegisterStartupScript(Me.GetType, "script123", sp)

		End If

	End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
	<title>Dyno Admin Control</title>
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
					<b><font face="Verdana" color="#A2921E">Customer Feedback</font></b></td>
			</tr>
			<tr>
				<td align="left" bgcolor="#edf2e6">
					<b><font color="#FF0000" face="Verdana" size="2">&nbsp;</font><font face="Verdana"
						size="2" color="#A2921E">Project Name :&nbsp;</font></b><asp:DropDownList ID="dd_prj_name"
							runat="server" CssClass="cssData" AutoPostBack="True" OnSelectedIndexChanged="dd_prj_name_SelectedIndexChanged">
						</asp:DropDownList><u><font color="#FF0000"><b> </b></font></u>
				</td>
			</tr>
		</table>
		<table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse;
			border-color: #EAEAEA; height: 1" width="100%" id="AutoNumber1">
			<%		If dd_prj_name.SelectedValue <> -1 Then%>
			<tr>
				<td width="18%" height="22" valign="top">
					<font face="Verdana" size="2">&nbsp;Customer Name:</font></td>
				<td width="28%" height="22" colspan="3" valign="top">
					<font face="Verdana" size="2">&nbsp;<%		Response.Write(cust_name)%>
					</font>
				</td>
				<td width="22%" height="22" colspan="2" valign="top">
					<font face="Verdana" size="2">&nbsp;Customer E-mail</font></td>
				<td width="44%" height="22" colspan="2" valign="top">
					<font face="Verdana" size="2">&nbsp;<%		Response.Write(cust_email)%>
					</font>
				</td>
			</tr>
			<tr>
				<td width="18%" height="22" valign="top">
					<font face="Verdana" size="2">&nbsp;Customer Company :</font></td>
				<td width="28%" height="22" colspan="3" valign="top">
					<font face="Verdana" size="2">&nbsp;<%		Response.Write(comp_name)%>
					</font>
				</td>
				<td width="22%" height="22" colspan="2" valign="top">
					<font face="Verdana" size="2">&nbsp;Project Name :</font></td>
				<td width="44%" height="22" colspan="2" valign="top">
					<font face="Verdana" size="2">&nbsp;<%		Response.Write(cust_prjname)%>
					</font>
				</td>
			</tr>
			<tr>
				<td width="18%" height="22" valign="top">
					<font face="Verdana" size="2">&nbsp;Customer Address :</font></td>
				<td width="28%" height="22" colspan="3" valign="top">
					<font face="Verdana" size="2">&nbsp;<%		Response.Write(cust_add)%>
					</font>
				</td>
				<td width="22%" height="22" colspan="2" valign="top">
					<font face="Verdana" size="2">&nbsp;Registration Date :</font></td>
				<td width="44%" height="22" colspan="2" valign="top">
					<font face="Verdana" size="2">&nbsp;<%		Response.Write(Day(cust_regdate) & "-" & Left(MonthName(Month(cust_regdate)), 3) & "-" & Year(cust_regdate))%>
					</font>
				</td>
			</tr>
		</table>
		<%		End If%>
		<tr><td>
			<table border="1" cellpadding="2" cellspacing="0" style="border-collapse: collapse;
				border-color: black; height: 1" width="100%" id="AutoNumber1">
				<tr>
				<td width="10%" rowspan="2" bgcolor="#edf2e6" height="42">
					<p align="center">
						<font face="Verdana" size="2" color="#A2921E">Date</font>
				</td>
				<td width="10%" rowspan="2" bgcolor="#edf2e6" height="42">
					<p align="center">
						<font face="Verdana" size="2" color="#A2921E" font-bold="True">Project Name</font>
				</td>
				<td width="28%" colspan="4" bgcolor="#edf2e6" height="16">
					<p align="center">
						<font face="Verdana" size="2" color="#A2921E">Ratings</font>
				</td>
				<td width="9%" bgcolor="#edf2e6" height="41" rowspan="2">
					<p align="center">
						<font face="Verdana" size="2" color="#A2921E">Project Status</font>
				</td>
				<td width="9%" bgcolor="#edf2e6" height="41" rowspan="2">
					<p align="center">
						<font color="#A2921E" size="2" face="Verdana">Future Interest</font>
				</td>
				<td width="15%" bgcolor="#edf2e6" rowspan="2" height="42">
					<p align="center">
						<font face="Verdana" size="2" color="#A2921E">Feedback</font>
				</td>
				<td width="8%" bgcolor="#edf2e6" rowspan="2" height="42">
					<p align="center">
						<font face="Verdana" size="2" color="#A2921E"></font>
				</td>
				<td width="8%" bgcolor="#edf2e6" rowspan="2" height="42">
					<p align="center">
						<font face="Verdana" size="2" color="#A2921E"></font>
				</td>
		</tr>
		<tr>
			<td width="7%" bgcolor="#edf2e6" align="center" height="25">
				<font face="Verdana" size="2" color="#A2921E">Technical</font></td>
			<td width="7%" bgcolor="#edf2e6" align="center" height="25">
				<font face="Verdana" size="2" color="#A2921E">Creativity</font></td>
			<td width="7%" bgcolor="#edf2e6" align="center" height="25">
				<font face="Verdana" size="2" color="#A2921E">Timeliness</font></td>
			<td width="10%" bgcolor="#edf2e6" align="center" height="25">
				<font face="Verdana" size="2" color="#A2921E">Communication</font></td>
		</tr>
		</table></td>
		</tr>
		<asp:DataGrid ID="MyDataGrid" runat="server" AllowSorting="true" Width="100%" BackColor="white"
			CellPadding="2" CellSpacing="0" Font-Name="Verdana" Font-Size="10pt" MaintainState="true"
			AutoGenerateColumns="False" OnItemCommand="ItemCommand" OnItemDataBound="ItemDataBound"
			BorderColor="Black" ShowHeader="False">
			<AlternatingItemStyle BackColor="#edf2e6"></AlternatingItemStyle>
			<Columns>
				<asp:BoundColumn Visible="false" DataField="Id" HeaderText="projID"></asp:BoundColumn>
				<asp:BoundColumn Visible="false" DataField="projId" HeaderText="projID"></asp:BoundColumn>
				<asp:BoundColumn DataField="feedback_date" SortExpression="reportDate" DataFormatString="{0:dd-MMM-yy}">
					<ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="projName" SortExpression="reportDate">
					<ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="technical" SortExpression="reportSubject">
					<ItemStyle Width="7%" HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="creativity" SortExpression="reportLastModified">
					<ItemStyle Width="7%" HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="timeliness" SortExpression="reportSubject">
					<ItemStyle Width="7%" HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="comm" SortExpression="reportSubject">
					<ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="status" SortExpression="reportSubject">
					<ItemStyle Width="9%" HorizontalAlign="center" VerticalAlign="top"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="future_interest" SortExpression="reportSubject">
					<ItemStyle Width="9%" HorizontalAlign="center" VerticalAlign="top"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="feedback" SortExpression="reportSubject">
					<ItemStyle Width="15%" HorizontalAlign="Left" VerticalAlign="top"></ItemStyle>
				</asp:BoundColumn>
				<asp:TemplateColumn HeaderText="">
					<ItemStyle Width="8%" HorizontalAlign="center" VerticalAlign="middle"></ItemStyle>
					<ItemTemplate>
						<asp:Button ID="view" runat="server" CommandName="view" Text="View" align="right"
							Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
							background-color: #C5D5AE" Font-Bold="true"></asp:Button>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="">
					<ItemStyle Width="8%" HorizontalAlign="center" VerticalAlign="middle"></ItemStyle>
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
