<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empLeave.aspx.vb" Inherits="emp_empLeave" %>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >    
	<head>
   
		<meta http-equiv="Content-Language" content="en-us">
		<title>Intelgain Technologies Pvt Ltd</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="Table3" height="100%" cellSpacing="0" cellPadding="2" width="100%" align="center"
				border="0">
				<tr>
					<td colspan="3">
                    <EMPHEADER:EMPHEADER id="Empheader" runat="server">
                    </EMPHEADER:EMPHEADER><BR>
						<uc1:empMenuBar id="EmpMenuBar" runat="server">
                    </uc1:empMenuBar></td>
				</tr>
				<tr>
					<td align="center" height="90%" colspan="3">
					<table cellspacing="0" cellpadding="2" border="1" bordercolor="#C5D5AE" width="450" height="299">
						<tr>
							<td colspan="5" bgcolor="#C5D5AE" height="53" width="394"><b>
                            <font face="Verdana" color="#a2921e" size="2">Leave 
                            Details (<%=currStartDate%> To <%=currEndDate%>)</font></b></td>
						</tr>
                        <tr>
							<td bgcolor="#EDF2E6" nowrap="nowrap" height="30"><b>
                            <font face="Verdana" size="2" color="#A2921E">
                            Joining Date</font></b></td>
							<td width="75%" nowrap="nowrap" align="center" colspan="4" height="30">
							<asp:label id="empJoiningDateLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label></td>
							</tr>
                        <tr>
							<td bgcolor="#EDF2E6" nowrap="nowrap" height="30"><b>
                            <font face="Verdana" size="2" color="#A2921E">
                            Confirmation Date</font></b></td>
							<td width="75%" nowrap="nowrap" align="center" colspan="4" height="30">
							<asp:label id="empConfDateLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
						</tr>
                        <tr>
							<td bgcolor="#EDF2E6" nowrap="nowrap" height="29">&nbsp;</td>
							<td bgcolor="#EDF2E6" width="20%" nowrap="nowrap" align="center" height="29">
                            <font face="Verdana" size="2" color="#A2921E">CL</font></td>
							<td bgcolor="#EDF2E6" width="20%" nowrap="nowrap" align="center" height="29">
                            <font face="Verdana" size="2" color="#A2921E">SL</font></td>
							<td bgcolor="#EDF2E6" width="20%" nowrap="nowrap" align="center" height="29">
                            <font face="Verdana" size="2" color="#A2921E">PL</font></td>
              <td bgcolor="#EDF2E6" width="20%" nowrap="nowrap" align="center" height="29">
                            <font face="Verdana" size="2" color="#A2921E">CO</font></td>              
						</tr>
                        <tr>
							<td bgcolor="#EDF2E6" nowrap="nowrap" height="29">
                            <font face="Verdana" size="2" color="#A2921E">Total (Annual)</font></td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:label id="empTotCLLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:label id="empTotSLLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:label id="empTotPLLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:Label ID="empTotCompOfflbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:Label> </td>
						</tr>
                        <tr>
							<td bgcolor="#EDF2E6" nowrap="nowrap" height="25">
                            <font face="Verdana" size="2" color="#A2921E">Consumed</font></td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:label id="empTotCLCLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:label id="empTotSLCLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:label id="empTotPLCLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:Label ID="empToCompOffClbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:Label> </td>
						</tr>
						<tr>
							<td bgcolor="#EDF2E6" nowrap="nowrap" height="26"><b>
                            <font face="Verdana" size="2" color="#A2921E">Balance</font></b></td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:label id="empTotCLBLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:label id="empTotSLBLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:label id="empTotPLBLbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:label>
							</td>
							<td width="20%" nowrap="nowrap" align="center" height="29">
							<asp:Label ID="empToCompOffBlbl" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"></asp:Label> </td>
						</tr>
					</table>	
					</td>
				</tr>
				<tr>
					<td>
				<table width="100%" cellpadding="2" cellspacing="1" border="0">
				<tr>
					<td colspan="4" bgcolor="#C5D5AE" height="25">
						<b><font face="Verdana" color="#a2921e" size="2">
                        &nbsp;Leave Details</font></b></td>
					</tr>
				<tr>
					<td width="25%" valign="top">
						<ASP:DataGrid id="clDataGrid" runat="server" width="100%" 
						 CellPadding="2" Font-Name="Verdana" Font-Size="10pt"
						MaintainState="true" AutoGenerateColumns="false" Font-Names="Verdana">
						<HeaderStyle Forecolor="#a2921e" Font-Size="10pt" BackColor="#C5D5AE"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="attDate" HeaderText="Casual Leave" DataFormatString="{0:dd-MMM-yyyy ddd}">
				            </asp:BoundColumn>
						</Columns>
						</ASP:DataGrid>
					</td>
					<td width="25%" valign="top">
						<ASP:DataGrid id="slDataGrid" runat="server" width="100%" 
						CellPadding="2" Font-Name="Verdana" Font-Size="10pt"
						MaintainState="true" AutoGenerateColumns="false" Font-Names="Verdana">
						<HeaderStyle Forecolor="#a2921e" Font-Size="10pt" BackColor="#C5D5AE"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="attDate" HeaderText="Sick Leave" DataFormatString="{0:dd-MMM-yyyy ddd}">
				            </asp:BoundColumn>
						</Columns>
						</ASP:DataGrid>
					</td>
					<td width="25%" valign="top">
						<ASP:DataGrid id="plDataGrid" runat="server" width="100%" 
						CellPadding="2" Font-Name="Verdana" Font-Size="10pt"
						MaintainState="true" AutoGenerateColumns="false" Font-Names="Verdana">
						<HeaderStyle Forecolor="#a2921e" Font-Size="10pt" BackColor="#C5D5AE"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="attDate" HeaderText="Paid Leave" DataFormatString="{0:dd-MMM-yyyy ddd}">
				            </asp:BoundColumn>
						</Columns>
						</ASP:DataGrid>
						</td>
						<td width="25%" valign="top">
							<asp:DataGrid ID="coDataGrid" runat="server" Width="100%" CellPadding="2" Font-Names="Verdana" Font-Size="10pt"
							 maintainState="true" AutoGenerateColumns="false">
								<HeaderStyle ForeColor="#a2921e" Font-Size="10pt" BackColor="#C5D5AE" />
								<Columns>
										<asp:BoundColumn DataField="attDate" HeaderText="Comp Off" DataFormatString="{0:dd-MM-yyyy ddd}"></asp:BoundColumn>
								</Columns>
							 </asp:DataGrid>
						</td>					
						</tr>
						<tr>
						<td colspan="4" height="25">
						</td>
						</tr>
						<tr>
						<td colspan="4" height="25">
						</td>
						</tr>
						<tr>
					<td colspan="3" bgcolor="#C5D5AE" height="25">
						<b><font face="Verdana" color="#a2921e" size="2">
                        &nbsp;Leave Applied</font></b></td>
					<td bgcolor="#C5D5AE" height="25">
                        <asp:HiddenField ID="hf_CLBalance" runat="server" Value="hidden value"/>
                        <asp:HiddenField ID="hf_SLBalance" runat="server" Value="hidden value"/>
                        <asp:HiddenField ID="hf_PLBalance" runat="server" Value="hidden value"/>
                         <asp:HiddenField ID="hf_CompOffBalance" runat="server" Value="hidden value"/>
						<input type="button" name="bt1" value="Apply for Leave" onclick="javascript: sdff = window.open('empleaveApp.aspx?CLBalance=' + hf_CLBalance.value + ' &SLBalance=' + hf_SLBalance.value + ' &PLBalance=' + hf_PLBalance.value + ' &CompOffBalance=' + hf_CompOffBalance.value, 'winWatch', 'scrollbars=no,toolbar=no,menubar=no,location=right,width=550,height=450,left=150,top=90')"/>
					</td>
					</tr>
					<tr>
						<td colspan="4" height="25">
						<ASP:DataGrid id="laDataGrid" runat="server" width="100%" 
						CellPadding="2" Font-Name="Verdana" Font-Size="10pt" DataKeyField="empLeaveId"
						MaintainState="true" AutoGenerateColumns="false" AllowPaging="true" OnItemDataBound="laDataGrid_ItemDataBound" OnPageIndexChanged="laDataGrid_PageIndexChanged"  PageSize="10" PagerStyle-Mode="NumericPages"  Font-Names="Verdana">
						<HeaderStyle Forecolor="#a2921e" Font-Size="10pt" BackColor="#C5D5AE"></HeaderStyle>
						<Columns>
							<asp:BoundColumn visible="false" DataField="empLeaveId"/>
							<asp:BoundColumn DataField="leaveFrom" HeaderText="Leave From" HeaderStyle-Font-Bold="true" DataFormatString="{0:dd-MMM-yy }">
				            </asp:BoundColumn>
							<asp:BoundColumn DataField="leaveTo"  HeaderText="To" HeaderStyle-Font-Bold="true" DataFormatString="{0:dd-MMM-yy }">
				            </asp:BoundColumn>
							<asp:BoundColumn DataField="desc1" HeaderText="Leave Type" HeaderStyle-Font-Bold="true" >
				            </asp:BoundColumn>
							<asp:BoundColumn DataField="leaveDesc" HeaderText="Leave Reason" HeaderStyle-Font-Bold="true" >
							 </asp:BoundColumn>
							<asp:BoundColumn DataField="ls" HeaderText="Leave Status" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="left" HeaderStyle-Font-Bold="true" >
				            </asp:BoundColumn>
							<asp:BoundColumn DataField="leaveComment" HeaderText="Admin Comments" HeaderStyle-Font-Bold="true" >
							</asp:BoundColumn>
							<ASP:TemplateColumn >
							  <ItemTemplate>
							    <asp:LinkButton ID="lbtnDelete" runat="server"  Text="Delete" Visible="false" CommandName="Delete"  ></asp:LinkButton>
							  </ItemTemplate>
							</ASP:TemplateColumn>
						</Columns>
						</ASP:DataGrid> 
						</td>
						</tr
					></table>	
					</td>
				</tr>
				<tr>
						<td colspan="4" height="25">
						</td>
					</tr>	
					<tr>
						<td colspan="4" height="25">
						</td>
					</tr>					
			</table>
		</form>
	</body>
</html>