<%@ Page Language="VB" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<script language="VB" runat="server">
Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim Conn As SqlConnection = New SqlConnection(dsn)
    Dim gf As New generalFunction
Dim Cmd as New SQLCommand()
Dim SortField As String
    Dim cust_name, cust_add, comp_name, cust_email, cust_regdate, cust_prjname As String
Sub Page_Load(sender As Object, e As EventArgs) 
		 
        gf.checkEmpLogin()
		  if cStr(Session("dynoEmpIdSession")) = "" Then
			Response.Redirect ("emplogin.aspx")
		  End If


        Dim sql As String
    Conn.Open()
   	Dim search as integer
 	 
	
	If dd_prj_name.SelectedValue <> "" Then
		search=dd_prj_name.SelectedValue
	end if 
	
	if search <> -1 then
	 Dim cmdProject as SqlCommand 
	 Dim strProject as String 
	 Dim dtrProject as SqlDataReader 
	 sql="select * from Customermaster ,projectMaster where Customermaster.custid=projectMaster.custid and projectMaster.projid=" & search 
	 cmdProject =  new SqlCommand(sql,Conn)
	 dtrProject =  cmdProject.ExecuteReader()
	
	 Do while  (dtrProject.Read())
		  cust_name=dtrProject("custname").ToString()
		  cust_add=dtrProject("custAddress").ToString()
		  comp_name=dtrProject("custCompany").ToString()
		   cust_email=dtrProject("custemail").ToString()
		    cust_regdate=dtrProject("custregdate").ToString()
		    cust_prjname=dtrProject("projname").ToString()
	 Loop
	 dtrProject.close()
	end if 
	Conn.Close	
		if page.ispostback = "False" then
		fillProject()
		BindGrid()
		end if 
End Sub
    
Sub MyDataGrid_Sort(sender As Object, e As DataGridSortCommandEventArgs)
	
End Sub
Sub fillProject()
    Dim Con as SqlConnection
	Dim cmdProject as SqlCommand 
	Dim strProject as String 
	Dim dtrProject as SqlDataReader 
    Con=New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
	'strProject ="select projId , projName, custId, from projectMaster order by projname "
	'strProject="select projectMaster.projId,projectMaster.projName,projectMaster.custId, CustomerMaster.custName from projectMaster, CustomerMaster where projectMaster.custid=CustomerMaster.custid  order by projname,custname "
	'-------------------------------------------------------
	'fill Second dropdownlist-projects
	'------------------------------------------------------
	
	strProject="select projId , projName from projectMaster where projManager= "& Session("dynoEmpIdSession") &" order by projName"
	' response.write(strProject)
   'response.end
	Con.Open()
	cmdProject =  new SqlCommand(strProject,Con)
	dtrProject =  cmdProject.ExecuteReader()
	if dd_prj_name.items.count = 0 then
	dd_prj_name.Items.Add( new  ListItem ("All",-1))
	Do while  (dtrProject.Read())
		dd_prj_name.Items.Add( new  ListItem (dtrProject("projName").ToString(),dtrProject("projId").ToString()))
	Loop
	end if 
	dtrProject.close()
	
	cmdProject.Dispose()
	Con.Close()
	
End Sub
Sub BindGrid()
    Dim da as SQLDataAdapter
	Dim ds as DataSet
    Cmd.Connection = Conn
    Dim strSQL as string
	Dim prjName as string
    Conn.Open()
   	Dim search as integer
	If dd_prj_name.SelectedValue <> "" Then
		search=dd_prj_name.SelectedValue
	
	
		if search=-1 then
			strSQL= "select * from changeRequest,projectMaster where changeRequest.chgProjId=projectMaster.projId and projManager= " & Session("dynoEmpIdSession") & " order by changeRequest.chgDate desc"
     else
	
		strSQL= "select changeRequest.chgid,changeRequest.chgTitle,changeRequest.chgDesc,changeRequest.chgCompDate,changeRequest.chgFeedback,changeRequest.chgApproved,changeRequest.chgEstTime,changeRequest.chgProjId,changeRequest.chgDate,projectMaster.projName from  " & _
		"changeRequest,projectMaster where changeRequest.chgProjId=" & search & "and projectMaster.projId =" & search & " and projManager= " &  Session("dynoEmpIdSession") & " order by changeRequest.chgDate desc"
   end if
'response.write(strSQL)
 ' response.end
		Cmd.CommandText = strSQL 
		da= New SqlDataAdapter(strSQL,Conn)
	    ds = New DataSet()
	    
		da.Fill(ds)
	    myDataGrid.DataSource = ds
		myDataGrid.DataBind()
	End If
End Sub


Sub ddcust_name_SelectedIndexChanged(sender as object , e as System.EventArgs )
	BindGrid()
End Sub
Sub dd_prj_name_SelectedIndexChanged(sender as object , e as System.EventArgs )
	BindGrid()
End Sub



Sub ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)

 dim id as integer
 id = e.Item.Cells(0).Text
 
'******************************************************************
'check is it view click or delete click event
'******************************************************************
If e.CommandName = "view" Then 
      
       Dim sp As String
       sp = "<Script language=JavaScript>"
       'sp += " window.open('view_changeRequest.aspx?id=" & id & "','winWatch','scrollbars=yes,toolbar=no,menubar=no,location=right,resizable=no,width=700,height=550,left=50,top=10');"
       sp += " window.open('view_changeRequest.aspx?id=" & id & "','winWatch','scrollbars=yes,toolbar=no,menubar=no,location=right,resizable=no,width=660,height=488,left=50,top=10');"         
	   sp += "</" + "script>"
       RegisterStartupScript("script123", sp)

else if e.CommandName = "delete" Then
      
      Dim sp As String
      sp = "<Script language=JavaScript>"
      sp += " val= confirm('Do You Really Want To Delete Record?');"
      sp += " if (val==true)  "
      sp += " window.location.href = 'delete_changeRequest.aspx?id=" & id & "';"    
      sp += "</" + "script>"
      RegisterStartupScript("script123", sp)

end if

End Sub	

Private Sub ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
		if (e.Item.Cells(5).Text = "P") then
			e.Item.Cells(5).Text = "Pending"
			e.Item.Cells(5).ForeColor = System.Drawing.Color.Blue
		end if 
		if (e.Item.Cells(5).Text = "R") then
			e.Item.Cells(5).Text = "Rejected"
			e.Item.Cells(5).ForeColor = System.Drawing.Color.Red
		end if
		if (e.Item.Cells(5).Text = "A") then
			e.Item.Cells(5).Text = "Accepted"
			e.Item.Cells(5).ForeColor = System.Drawing.Color.Green
		end if 
End Sub

</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>Project Change Request</title>
</head>
<body>
<form id="Form1" runat="server" method="post">

						<input type="hidden" id="empId" runat="server" size="20"> 

			<TABLE height="0" cellSpacing="0" cellPadding="4" width="100%" border="0" style="border-collapse: collapse" bordercolor="#111111" align="center">
				<TR>
						<TD>
							<TABLE id="Table3" cellSpacing="0" cellPadding="2" width="100%" border="0" height="1">
								<TR>
									<TD>
										<EMPHEADER:empHeader id="Empheader" runat="server">
                                        </EMPHEADER:empHeader></TD>
								</TR>
								<TR>
									<TD>
										<uc1:empMenuBar id="EmpMenuBar" runat="server">
                                        </uc1:empMenuBar></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
									
				<tr>
				
					<td >
					<table border="1" cellspacing="1" bordercolorlight="#000000" bordercolordark="#FFFFFF" width="100%">
					<tr>
						<td align="center" bgcolor="#C5D5AE"><font face="Verdana" color="#a2921e"><b>Change Request</b></font></td>
					</tr><tr>
					<td align="left" bgcolor="#C5D5AE"><b>
               <font color="#FF0000" face="Verdana" size="2">&nbsp;</font>
			   <font face="Verdana" size="2">Project 
                     Name :&nbsp;</font></b><asp:dropdownlist id="dd_prj_name" Runat="server" CssClass="cssData" AutoPostBack="True" onSelectedIndexChanged="dd_prj_name_SelectedIndexChanged">
					 </asp:dropdownlist>
					 <u>
					 <font color="#FF0000"><b>
                  </b></font></u>

                    </td></tr>
					</table>

					<table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#EAEAEA" width="100%" id="AutoNumber1" height="1">
   <!--       <%if dd_prj_name.SelectedValue <> -1 then %>    
         
 <tr>
                        <td width="18%"  height="22" valign="top">
                        <font face="Verdana" size="2">&nbsp;Customer Name:</font></td>
                        <td width="28%"  height="22" colspan="3" valign="top">
                        <font face="Verdana" size="2">&nbsp;<%response.write(cust_name)%>
                        </font></td>
                        <td width="22%"  height="22" colspan="2" valign="top">
                        <font face="Verdana" size="2">&nbsp;Customer E-mail</font></td>
                        <td width="44%"  height="22" colspan="2" valign="top">
                        <font face="Verdana" size="2">&nbsp;<%response.write(cust_email)%>
                        </font></td>
                      </tr>

 <tr>                    <td width="18%"  height="22" valign="top">
                        <font face="Verdana" size="2">&nbsp;Customer Company :</font></td>
                        <td width="28%"  height="22" colspan="3" valign="top">
                       <font face="Verdana" size="2">&nbsp;<%response.write(comp_name)%>
                        </font></td>
                        <td width="22%"  height="22" colspan="2" valign="top">
                        <font face="Verdana" size="2">&nbsp;Project Name :</font></td>
                        <td width="44%"  height="22" colspan="2" valign="top">
                        <font face="Verdana" size="2">&nbsp;<%response.write(cust_prjname)%>
                         </font></td>
                      </tr>

 <tr>
                        <td width="18%"  height="22" valign="top">
                        <font face="Verdana" size="2">&nbsp;Customer Address :</font></td>
                        <td width="28%"  height="22" colspan="3" valign="top">
                        <font face="Verdana" size="2">&nbsp;<%response.write(cust_add)%>
                        </font></td>
                        <td width="22%" height="22" colspan="2" valign="top">
                        <font face="Verdana" size="2">&nbsp;Registration Date :</font></td>
                        <td width="44%" height="22" colspan="2" valign="top">
                        <font face="Verdana" size="2">&nbsp;<%response.write(day(cust_regdate) & "-" & left(monthname(month(cust_regdate)),3) & "-" & year(cust_regdate))%>
                        </font></td>
                      </tr>

     </table><%end if%><tr> >-->
                   <ASP:DATAGRID id="MyDataGrid"  runat="server" BorderColor="" Font-Size="10pt" Font-Name="Verdana"  BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False" Footerstyle-HorizontalAlign="Right"
							Headerstyle-Font-Size="10pt" AllowSorting="True" Headerstyle-BackColor="#C5D5AE" CellPadding="0" 
							Width="100%" OnItemCommand="ItemCommand" OnItemDataBound = "ItemDataBound">
							<FooterStyle HorizontalAlign="Right"></FooterStyle>
							<Columns>
							 
                            <asp:BoundColumn Visible="false" DataField="chgId" HeaderText="ID">
                            </asp:BoundColumn>

							<asp:BoundColumn Visible="false" DataField="chgProjId" HeaderText="projID">
                            </asp:BoundColumn>
                            
								<asp:BoundColumn DataField="chgDate" HeaderText="Date"  DataFormatString="{0:dd-MMM-yy}">
							   <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
                                </asp:BoundColumn>
                                
                                <asp:BoundColumn DataField="projName"  HeaderText="Project Name"  >
								<HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
                                </asp:BoundColumn>
                                
							     <asp:BoundColumn DataField="chgTitle"  HeaderText="Title" >
								 <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
								 <ItemStyle HorizontalAlign="Left" VerticalAlign="middle" ></ItemStyle>
								 </asp:BoundColumn>
								 
							     <asp:BoundColumn DataField="chgApproved" HeaderText="Approved" >
								 <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
								 <ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
								 </asp:BoundColumn>
								    
								    <asp:BoundColumn DataField="chgEstTime" HeaderText="Estimated Time Frame (Days)" >
								 <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
								 <ItemStyle HorizontalAlign="Left" VerticalAlign="middle" ></ItemStyle>
								 </asp:BoundColumn>
							
								<asp:BoundColumn DataField="chgcompDate" HeaderText="Completion Date" DataFormatString="{0:dd-MMM-yy}" >
								 <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
								 <ItemStyle HorizontalAlign="Left" VerticalAlign="middle" ></ItemStyle>
								 </asp:BoundColumn>
							
								    
								 <asp:TemplateColumn HeaderText="">
                    			 <ITEMSTYLE Width="12%" wrap="false" HorizontalAlign="center" VerticalAlign="middle"></ITEMSTYLE>
			                     <ItemTemplate>
                                 <asp:Button id="view" runat="server"   CommandName="view" Text="View" align="right" style="font-family: Verdana; font-size: 8pt" width="75">
                                 </asp:Button>
                                 </ItemTemplate>
                                </asp:TemplateColumn>
                                
								   <asp:TemplateColumn HeaderText="">
                    			 <ITEMSTYLE Width="13%" wrap="false" HorizontalAlign="center" VerticalAlign="middle"></ITEMSTYLE>
			                     <ItemTemplate>
                                 <asp:Button id="delete" runat="server"   CommandName="delete" Text="Delete" align="right" style="font-family: Verdana; font-size: 8pt" width="75">
                                 </asp:Button>
                                 </ItemTemplate>
                                </asp:TemplateColumn>
                                
							</Columns>
							
						</ASP:DATAGRID>
</form>
</table>
</table>
</body>
</html>