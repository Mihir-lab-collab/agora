<%@ Page Language="VB" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
	<title>Dyno Admin Control</title>

	<script language="javascript" src="/includes/calender.js" type="text/javascript">
	</script>

</head>
<body>

	<script language="VB" runat="server">
		Dim SortField As String
		Dim empCode As String
	    Dim sql As String
		Dim rights As Integer
	    Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
		Dim conn As SqlConnection = New SqlConnection(dsn1)
	    Dim gf As New generalFunction

	    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	        gf.checkEmpLogin()
	        conn.Open()
	        If Not IsPostBack Then
	            loadData()
	        End If
	    End Sub

		Sub addBT_OnClick(ByVal objSource As Object, ByVal objArgs As EventArgs)
	    End Sub

		Sub loadData()
			sql = "select * from projectmaster" ' where empprojAsigned .empid=projectmaster.empid"
	        Dim objCmd As SqlCommand
			Dim gridAdapter As SqlDataAdapter
			Dim gridDataset As DataSet
         
			Dim objCmd1 As SqlCommand
			Dim gridAdapter1 As SqlDataAdapter
			Dim gridDataset1 As DataSet
         
			empCode = Request.QueryString("empId")
         
			objCmd = New SqlCommand(sql, conn)
			gridAdapter = New SqlDataAdapter(sql, conn)

			gridDataset = New DataSet()
			gridAdapter.Fill(gridDataset)
		 
	        Dim i As Integer, status As String, i1 As Integer
			gridDataset.Tables(0).Columns.Add(New DataColumn("chkstate", GetType(System.String)))
	     
	        sql = "select * from EMPLOYEEMASTER,projectmember ,projectmaster where EMPLOYEEMASTER.empid='" & _
	        empCode & "' and projectmember.empid=' " & empCode & " 'and projectmaster.projid=projectmember.projid "
			objCmd1 = New SqlCommand(sql, conn)
			gridAdapter1 = New SqlDataAdapter(sql, conn)

			gridDataset1 = New DataSet()
			gridAdapter1.Fill(gridDataset1)

			For i = 0 To gridDataset.Tables(0).Rows.Count - 1
				gridDataset.AcceptChanges()
				gridDataset.Tables(0).Rows(i).Item("chkstate") = "false"
				status = gridDataset.Tables(0).Rows(i).Item("projid")
				For i1 = 0 To gridDataset1.Tables(0).Rows.Count - 1
            
					If CInt(gridDataset.Tables(0).Rows(i).Item("projid")) = CInt(gridDataset1.Tables(0).Rows(i1).Item("projid")) Then
            
						gridDataset.Tables(0).Rows(i).Item("chkstate") = "true"
						Response.Write(gridDataset.Tables(0).Rows(i).Item("chkstate"))
                              
					End If
            
				Next
			Next
     
        
			DataGrid1.DataSource = gridDataset
			DataGrid1.DataBind()
			conn.Close()
			gridDataset.Dispose()
			gridAdapter.Dispose()
			objCmd.Dispose()
				
		End Sub

		Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	        Dim ret1 As String
	        Dim dg As DataGridItem
			ret1 = Request.Form("C1")
			Dim chk_status As CheckBox
 	
			For Each dg In DataGrid1.Items
	            chk_status = CType(dg.FindControl("prj_chkbox"), CheckBox)
	            Response.Write(chk_status.Checked)
			Next
		End Sub


	</script>

	<script language='JavaScript'>
function open1()
{
window.open("project_list.aspx",'winWatch','scrollbars=no,toolbar=no,menubar=no,location=right,resizable=yes,width=215,height=510,left=50,top=100')

}
	</script>

	<form runat="server" id="custForm">
		<input type="hidden" id="empId" runat="server" size="20" />
		<table cellpadding="4" width="437" border="1" style="border-collapse: collapse; border-color: #e8e8e8;
			height: 50">
			<tr>
				<td width="427">
					<asp:DataGrid ID="DataGrid1" runat="server" Font-Size="X-Small" Font-Names="Verdana"
						Width="300px" AutoGenerateColumns="False">
						<HeaderStyle Font-Bold="True" BackColor="#D8EDF9"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Projects">
								<ItemTemplate>
									<asp:CheckBox Checked='<%#Container.DataItem("chkstate")%>' ID="prj_chkbox" runat="server"
										Width="250" value='<%#Container.DataItem("projname")%>' Text='<%#Container.DataItem("projname")%>'>
									</asp:CheckBox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>&nbsp;</td>
			</tr>
			<tr>
				<td width="427">
					<asp:Button ID="btnSave" onclick="btnSave_Click" runat="server" Width="90px" Text="Sumbit">
					</asp:Button></td>
			</tr>
		</table>
	</form>
</body>
</html>
