<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empAddCodeReview.aspx.vb" Inherits="emp_empAddCodeReview1" %>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader"  Src="~/controls/empHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EmpAddcodereview</title>
</head>

	<script language="JavaScript" type="text/javascript">
		 function popupProjectDetail(Id)
		 {
		   
			 var win = window.open('empRatingDetails.aspx?revId='+ Id,'winWatch','scrollbars=yes,toolbar=no,menubar=no,location=right,width=950,height=670,left=100,top=50');
			 win.focus(); 
		 } 
	</script>	 

  <style type="text/css">

div.sample_attach1
{
  width: 400px;
  border: 1px solid black;
  font-family: Arial;
  background: #C5D5AE;
  padding: 0px 5px;
  font-weight: 900;
  color: RED;
}
input.btn{
   
   font-family: Verdana;
   font-size:84%;
   font-weight:bold;
   color: #A2921E;
   border: 1px solid black;
   border:1px solid;
   background:#C5D5AE;
   border-top-color:#696;
   border-left-color:#696;
   border-right-color:#363;
   border-bottom-color:#363;
   
   }

</style>

    <script language="javascript" type="text/javascript" src="../includes/dropdown.js"></script>
	<script language="javascript"  type="text/javascript">

	 function popupProjectDetailFeedback(Id)
	{
		   //alert('ewfwefwe');
			// var win = window.open('FeedBack.aspx?projId='+ Id,'toolbar=no,location=right,resizable=no');
			 window.open("FeedBack.aspx?projId="+ Id,null, "height=250,width=500,status=yes,toolbar=no,menubar=no,location=no");
			// win.focus(); 
	} 
	 </script>
<body   topmargin="0" leftmargin="0" rightmargin="0" bottommargin="0" >
	<form id="Form1" runat="server">
		<table cellspacing="1" cellpadding="4"  align="center"  border="2" bordercolor="#F1F4EC" width="100%">
   			<tr>
				<td>
					<table id="Table3" cellSpacing="0" cellPadding="2" width="100%" border="0" height="1">
						<tr>
							<td><EMPHEADER:empHeader id="Empheader" runat="server"></EMPHEADER:empHeader></td>
						</tr>
						<tr>
							<td><uc1:empMenuBar id="EmpMenuBar" runat="server"></uc1:empMenuBar></td>
						</tr>
				   </table>
			   </td>
	       </tr>
		  <tr>
				<td align="center"  style="background: #C5D5AE" ><font  face="Verdana" color="#a2921e"><b>Project Details</b></font></td>
		  </tr>
		  <tr>
				<td width="100%">
					<table width="100%">
						<tr>
							<td style="background:#C5D5AE;" nowrap="nowrap"  valign="top" width="25%"><b> 
								<font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Project Name:</b></font></b>
							</td>
							<td style="background:#edf2e6" valign="top" width="25%"><font face="verdana" size="4"><b>
								<asp:Label ID="lblprjName" runat="server" ></asp:Label></b></font>
							</td>
							<td style="background:#C5D5AE;" nowrap="nowrap"  valign="top" width="25%"><b> 
								<font style="font-size: 10pt; color: #A2921E; font-family: Arial">Project Status:</font></b>
							</td>
							<td  style="background:#edf2e6;width:25%" valign="top" ><font face="verdana" size="2">
								<asp:Label ID="lblprojStatus" runat="server" ></asp:Label></font>
							</td>
					  </tr>
					  <tr>
						<td  style="background:#C5D5AE;" nowrap="nowrap"   valign="top"  style="width:25%"><b>
							<font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Customer Name:</b></font></b>
						</td>
					    <td  style="background:#edf2e6;width:25%" valign="top" ><font face="verdana" size="2">
							<asp:Label ID="lblCustName" runat="server"></asp:Label></font>
						</td>
						<td  style="background:#C5D5AE;width:25%"nowrap="nowrap"   valign="top"><b>
							<font style="font-size: 10pt; color: #A2921E; font-family: Arial">Exp Project Status:</font></b>
						</td>
						<td  style="background:#edf2e6;width:25%" valign="top"><font face="verdana" size="2">
							<asp:Label ID="lblExpProjStatus" runat="server" ></asp:Label></font>
						</td>
					 </tr>
					 <tr>
						<td style="background:#C5D5AE;" valign="top" nowrap="nowrap" align="left"> 
							<font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Customer Address:</b></font>
						</td>
						<td  style="background:#edf2e6; width:25%" valign="top" ><font face="verdana" size="2"><asp:Label ID="lblCustAddress" runat="server"></asp:Label></font>
						</td>
						<td style="background:#C5D5AE;width:25%" valign="top" nowrap="nowrap" > 
							<font style="color: #A2921E; font-family: Arial"><b> 
							<font style="font-size: 10pt; color: #A2921E; font-family: Arial">Start Date:</font></b><font size="2"></font></font> 
						</td>
						<td  style="background:#edf2e6;width:25%" valign="top" ><font face="verdana" size="2">
							<asp:Label ID="lblStartDate" runat="server" ></asp:Label></font>
						</td>
				    </tr>
					<tr>
						<td  style="background:#C5D5AE;width:25%" valign="top" nowrap="nowrap" align="left" ><font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Project Duration:</b> </font>
						</td>
						<td  style="background:#edf2e6;width:25%" valign="top" ><font face="verdana" size="2"> <asp:Label ID="lblProjDurat" runat="server"></asp:Label></font>
						</td>
						<td  style="background:#C5D5AE;width:25%" valign="top" nowrap="nowrap" ><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial">Exp Comp Date: </font> </b></td>
						<td  style="background:#edf2e6;width:25%" valign="top"><font face="verdana" size="2"><asp:Label ID="lblExpDate" runat="server"></asp:Label></font>
						</td>
					</tr>
					<tr>
						<td style="background:#C5D5AE;width:25%"" valign="top" nowrap="nowrap" align="left" ><font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Project Manager:</b> </font></td>
						<td  style="background:#edf2e6;width:25%" valign="top" ><font face="verdana" size="2"><asp:Label ID="lblProjMang" runat="server"></asp:Label></font></td>
					    <td  style="background:#C5D5AE;" valign="top" nowrap="nowrap" ><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial">Act Comp Date: </font></b></td>
					    <td  style="background:#edf2e6; width:25%" valign="top" ><font face="verdana" size="2"><asp:Label ID="lblActCompDate" runat="server" ></asp:Label></font></td>
					</tr>
			        </table>
			    </td>
          </tr> 
		  <tr>
			<td  colspan="4" align="center" style="background:#F1F4EC">&nbsp;</td>
		 </tr>
		 <tr id="trmember" runat="server">
			 <td   colspan="4" align="left" style="background:#F1F4EC"><font face="Verdana" color="#a2921e" size="2"><b>Project Team Member</b></font><b></b></td>
      
		 </tr>
		 <tr>
			<td  align="left" bgcolor="#edf2e6" valign="top" width="100%" colspan="2">
					<asp:DataGrid ID="dgProjectTeamMem" runat="server" BorderColor="Black" Font-Size="10pt" Font-Name="Verdana"
					BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"	HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray" CellPadding="2"
					Width="100%" >
					<itemstyle ForeColor="#000000" BackColor="#FFFFEE" VerticalAlign="Top"></itemstyle>
					<headerstyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100%"></headerstyle>
					<columns>
					<asp:BoundColumn DataField="srno" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"  ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="Center">
					<headerstyle></headerstyle>
					</asp:BoundColumn>
					<asp:BoundColumn HeaderText="Employee" DataField="empName">
					<headerstyle></headerstyle>
					</asp:BoundColumn>
					</columns>
					</asp:DataGrid>
        
          </td>
       </tr>
  
       <tr>
			<td   align="left" bgcolor="#edf2e6" valign="top" >
				<table width="100%" border="1" cellspacing="2" cellpadding="2">
					<tr width="100%">
						 <td> 
							 <table border="0" cellspacing="1" bordercolorlight="#000000" bordercolordark="#FFFFFF" width="100%">
								<tr>
									 <td colspan="6" align="center" bgcolor="#F1F4EC"> <a><b><font face="Verdana" color="#A2921E"></font></b></a></td>
								</tr>
								<tr>
									 <td colspan="6"align="center" bgcolor="#C5D5AE"> <a name="Search"><b><font face="Verdana" color="#A2921E"> Rating Report</font> </b></a></td>
								</tr>
								<tr>
									<td colspan="6"width="100%">
										<table width="100%" >
											 <tr width="100%">
												  <td  colspan="4" style="background:#C5D5AE;width:6%"  valign="middle" align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Date</b> </font> </b></td>
												  <td valign="middle" valign="middle" style="background:#C5D5AE;width:10%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Name</b> </font> </b></td>
												  <td valign="middle" colspan="4" style="background:#C5D5AE;width:7%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%=lblCodeC.Text%></b> </font> </b></td>
												  <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%=lblFS.Text%></b> </font> </b></td>
												  <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%=lblCo.Text%></b> </font> </b></td>
												  <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%=lblCodeDoc.Text%></b> </font> </b></td>
												  <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%=lblDS.Text%></b> </font> </b></td>
												  <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Overall Rating</b> </font> </b></td>
												   <td valign="middle" style="background:#C5D5AE;width:30%"  align="left"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Comment</b> </font> </b></td>
											</tr>
									  </table>
											<asp:DataGrid ID="dgrdrating" runat="server" BorderColor="Black" Font-Size="10pt" Font-Name="Verdana" border="0"
															 BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"	HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray" CellPadding="2"
															  Width="100%"   ShowHeader="false"  OnItemDataBound="dgrdRatingbount"  >
											<itemstyle ForeColor="#000000" BackColor="#FFFFEE" VerticalAlign="Top" Width="100%"></itemstyle>
											<headerstyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100%"></headerstyle>
											<columns>

											<asp:BoundColumn DataField="ratedate"   HeaderText="Date" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="10%" DataFormatString="{0:dd-MMM-yy}" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>

											<asp:BoundColumn DataField="empName" HeaderText="Name"  ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="left"></asp:BoundColumn>

											<asp:TemplateColumn HeaderText="coding Conventions<br>%" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
											  <itemtemplate> <%#DataBinder.Eval(Container.DataItem, "codingConventions")&"%"%> </itemtemplate>
											</asp:TemplateColumn>

											<asp:TemplateColumn  HeaderText="file Structure %"  HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%" >
											  <itemtemplate> <%#DataBinder.Eval(Container.DataItem, "fileStructure")&"%"%> </itemtemplate>
											</asp:TemplateColumn>

											<asp:TemplateColumn  HeaderText="code Optimization %"  HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%" >
											  <itemtemplate > <%#DataBinder.Eval(Container.DataItem, "codeOptimization")&"%"%> </itemtemplate>
											</asp:TemplateColumn>

											<asp:TemplateColumn HeaderText="code Documentation %"  HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"  >
											  <itemtemplate > <%#DataBinder.Eval(Container.DataItem, "codeDocumentation")&"%"%> </itemtemplate>
											</asp:TemplateColumn>


											<asp:TemplateColumn HeaderText="DB Structure %"  HeaderStyle-HorizontalAlign="Center"   ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
											  <itemtemplate > <%#DataBinder.Eval(Container.DataItem, "dbStructure")&"%"%> </itemtemplate>
											</asp:TemplateColumn>

											 <asp:TemplateColumn HeaderText="Overall" ItemStyle-Font-Bold="true"  HeaderStyle-HorizontalAlign="Center"   ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
											  <itemtemplate > <%#DataBinder.Eval(Container.DataItem, "total")&"%"%> </itemtemplate>
											</asp:TemplateColumn>




											<asp:BoundColumn DataField="coderevid" Visible="False" ></asp:BoundColumn>

											 <asp:BoundColumn  ></asp:BoundColumn>

											<asp:TemplateColumn ItemStyle-Width="7%" >
											  <itemtemplate>
												<asp:Button ID="btnDetails"  runat="server"    Text="Details" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"></asp:Button>
											  </itemtemplate>
											</asp:TemplateColumn >
											   <asp:BoundColumn DataField="comments" visible="false" ></asp:BoundColumn>


											</columns>
											</asp:DataGrid>
											</td>
											</tr>
											<tr><td height="15px"></td></tr>
											<tr id="trReportStatus" runat="server">
											<td colspan="6" align="center" style="background:#C5D5AE"> <b><font face="Verdana" color="#a2921e" size="2">Report Status</font></b></td>
											</tr>
											<tr id="ddl1" runat="server">
											<td  style="background:#C5D5AE;width:150" nowrap="nowrap"   valign="top"><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial">
											<asp:Label
											ID="lblCodeC" runat="server" Text="Label"></asp:Label>
											</font></b></td>
											<td colspan="3" bgcolor="#F1F4EC">
											<asp:DropDownList ID="ddlcodingConv" runat="server">
											<asp:ListItem Value="1" selected> </asp:ListItem>
											<asp:ListItem Value="2">2</asp:ListItem>
											<asp:ListItem Value="3">3</asp:ListItem>
											<asp:ListItem Value="4">4</asp:ListItem>
											<asp:ListItem Value="5">5</asp:ListItem>
											</asp:DropDownList>
                                    </td>
                                </tr>
								<tr id="ddl2" runat="server">
									<td  style="background:#C5D5AE;width:150" nowrap="nowrap"   valign="top"><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial"> <asp:Label ID="lblFS" runat="server"></asp:Label></font></b></td>
									<td colspan="3" bgcolor="#F1F4EC">
										<asp:DropDownList ID="ddlFileStructure" runat="server">
											<asp:ListItem Value="1" selected> </asp:ListItem>
											<asp:ListItem Value="2">2</asp:ListItem>
											<asp:ListItem Value="3">3</asp:ListItem>
											<asp:ListItem Value="4">4</asp:ListItem>
											<asp:ListItem Value="5">5</asp:ListItem>
										</asp:DropDownList>
									</td>
							   </tr>
							   <tr id="ddl3" runat="server">
									<td  style="background:#C5D5AE;width:150" nowrap="nowrap" valign="top"><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial"><asp:Label ID="lblCo" runat="server"></asp:Label></font></b></td>
									<td colspan="3" bgcolor="#F1F4EC">
										<asp:DropDownList ID="ddlCodeOptimization" runat="server">
											<asp:ListItem Value="1" selected> </asp:ListItem>
											<asp:ListItem Value="2">2</asp:ListItem>
											<asp:ListItem Value="3">3</asp:ListItem>
											<asp:ListItem Value="4">4</asp:ListItem>
											<asp:ListItem Value="5">5</asp:ListItem>
										</asp:DropDownList>
									</td>
							 </tr>
							 <tr id="ddl4" runat="server">
								 <td  style="background:#C5D5AE;width:150" nowrap="nowrap"  valign="top"><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial"><asp:Label ID="lblCodeDoc" runat="server"></asp:Label></font></b></td>
				                <td colspan="3" bgcolor="#F1F4EC">
									<asp:DropDownList ID="ddlCodeDocument" runat="server">
										<asp:ListItem Value="1" selected> </asp:ListItem>
										<asp:ListItem Value="2">2</asp:ListItem>
										<asp:ListItem Value="3">3</asp:ListItem>
										<asp:ListItem Value="4">4</asp:ListItem>
										<asp:ListItem Value="5">5</asp:ListItem>
								   </asp:DropDownList>
							   </td>
                            </tr>
                           <tr id="ddl5" runat="server">
								<td  style="background:#C5D5AE;width:150; height: 14px;" nowrap="nowrap"   valign="top"><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial"><asp:Label ID="lblDS" runat="server"></asp:Label> </font></b></td>
								<td colspan="3" bgcolor="#F1F4EC" style="height: 14px">
									<asp:DropDownList ID="ddlDataStructure" runat="server">
										<asp:ListItem Value="1" selected> </asp:ListItem>
										<asp:ListItem Value="2">2</asp:ListItem>
										<asp:ListItem Value="3">3</asp:ListItem>
										<asp:ListItem Value="4">4</asp:ListItem>
										<asp:ListItem Value="5">5</asp:ListItem>
									</asp:DropDownList>
							 </td>
				         </tr>
						 <tr id="ddl6" runat="server">
							<td style="background:#C5D5AE;width:150" nowrap="nowrap"   valign="top"><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial">Comment</font></b></td>
							<td colspan="3" bgcolor="#F1F4EC"><asp:TextBox runat="server" TextMode="MultiLine"   ID="txtComment" Rows="4" Columns="60"></asp:TextBox></td>
						</tr>
						<tr id="ddl7" runat="server">
							<td colspan="4" align="center"   valign="top" nowrap="nowrap"  style="background:#F1F4EC;width:150"><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial"><asp:Button  runat="server" ID="btnSubmit" Text="Submit" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"/></font></b></td>
						</tr>
					           </table>
							</td>
					</tr> 
	            </table>
			</td>
		</tr>
	</table> 
  </form>
 </body>
</html>
