<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empRatingDetails.aspx.vb" Inherits="emp_empRatingDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
	<script language="javascript" type="text/javascript">
		function feedback1()
		{
			if (document.getElementById('textFeedback').value=='')
			{
				alert("Enter FeedBack..");
				return false;
			}
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" align="center" border="1"  valign="Top">
            <tr>
                  <td align="center" colspan="2" bgcolor="#C5D5AE"><b><font face="Verdana" color="#A2921E">Code Review Report</font></b></td>
            </tr>
            <tr>
                  <td style="background:#C5D5AE;" nowrap="nowrap"  valign="top" width="25%" ><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Project Name:</b></font></b></td>
                  <td style="background:#edf2e6" valign="top" width="25%"><font face="verdana" size="4"><b> <asp:Label ID="lblprjName" runat="server" ></asp:Label></b></font></td>
            </tr>
            <tr>
                  <td colspan="2" height="20px"></td>
            </tr>
            <tr>
                  <td colspan="2"align="center" bgcolor="#C5D5AE"> <a name="Search"><b><font face="Verdana" color="#A2921E"> Rating Report</font> </b></a></td>
            </tr>
            <tr>
                  <td colspan="2" width="100%">
                    <table width="100%" >
                      <tr width="100%">
                        <td style="background:#C5D5AE;width:10%"  valign="middle" align="center"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Date</b> </font> </b></td>
                        <td valign="middle" style="background:#C5D5AE;width:11%"  align="center"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Name</b> </font> </b></td>
                        <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%= session("lblCodeC")%></b> </font> </b></td>
                        <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%=session("lblFS")%></b> </font> </b></td>
                        <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%=session("lblCo")%></b> </font> </b></td>
                        <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%=session("lblCodeDoc")%></b> </font> </b></td>
                        <td valign="middle" style="background:#C5D5AE;width:7%"  align="center"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b><%=session("lblDS")%></b> </font> </b></td>
                        <td valign="middle" style="background:#C5D5AE;width:7%"  align="left"><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Overall Rating</b> </font> </b></td>
                        <td valign="middle" style="background:#C5D5AE;width:30%"  align="left"><b><font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Comment</b> </font> </b></td>
                      </tr>
                    </table>
							<asp:DataGrid ID="dgrdrating" runat="server" BorderColor="Black" Font-Size="10pt" Font-Name="Verdana" border="0" BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"	HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray" CellPadding="2"   Width="100%"   ShowHeader="false"  OnItemDataBound="dgrdDetails">
							<itemstyle ForeColor="#000000" BackColor="#FFFFEE" VerticalAlign="Top" Width="100%"></itemstyle>
							<headerstyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100%"></headerstyle>
							<columns>
								<asp:BoundColumn DataField="ratedate"   HeaderText="Date" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="10%" DataFormatString="{0:dd-MMM-yy}" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>

								<asp:BoundColumn DataField="empName" HeaderText="Name"  ItemStyle-Width="11%"  ItemStyle-HorizontalAlign="left"></asp:BoundColumn>

								<asp:TemplateColumn HeaderText="coding Conventions<br>%" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
								<itemtemplate> <%#DataBinder.Eval(Container.DataItem, "codingConventions") &"%"%> </itemtemplate>
								</asp:TemplateColumn>

								<asp:TemplateColumn  HeaderText="file Structure %"  HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%" >
								<itemtemplate> <%#DataBinder.Eval(Container.DataItem, "fileStructure")&"%"%> </itemtemplate>
								</asp:TemplateColumn>

								<asp:TemplateColumn  HeaderText="code Optimization %"  HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="9%" >
								<itemtemplate > <%#DataBinder.Eval(Container.DataItem, "codeOptimization")&"%"%> </itemtemplate>
								</asp:TemplateColumn>

								<asp:TemplateColumn HeaderText="code Documentation %"  HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="9%"  >
								<itemtemplate > <%#DataBinder.Eval(Container.DataItem, "codeDocumentation")&"%"%> </itemtemplate>
								</asp:TemplateColumn>


								<asp:TemplateColumn HeaderText="DB Structure %"  HeaderStyle-HorizontalAlign="Center"   ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%"   >
								<itemtemplate > <%#DataBinder.Eval(Container.DataItem, "dbStructure")&"%"%> </itemtemplate>
								</asp:TemplateColumn>

								<asp:TemplateColumn HeaderText="Overall"  HeaderStyle-HorizontalAlign="left"  ItemStyle-Font-Bold="true"   ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%"   >
								<itemtemplate > <%#DataBinder.Eval(Container.DataItem, "total")&"%"%> </itemtemplate>
								</asp:TemplateColumn>


								<asp:BoundColumn DataField="coderevid" Visible="False" ></asp:BoundColumn>

								<asp:BoundColumn DataField="comments" Visible="False" ></asp:BoundColumn>

								<asp:BoundColumn  ></asp:BoundColumn>

							</columns>
							</asp:DataGrid>
                </td>
            </tr>
            <tr>
                 <td colspan="2"height="15px" style="background:#F1F4EC"></td>
            </tr>
            <tr>
                  <td colspan="2" nowrap="nowrap" bgcolor="#EDF2E6"> <font size="2" color="#A2921E"><b> <span style="font-family: Arial, Tahoma, Verdana, Helvetica">Feedback History</span></b></font><b><font color="#A2921E"> </font></b> </td>
            </tr>
            <tr>
                 <td colspan="2"><font style="font-size: 10pt; color: #000000; font-family: Arial, Tahoma, Verdana, Helvetica">
						<textarea name="resolution" cols="127" rows="7" style="border-style:solid; border-width:0; font-family: Verdana; font-size: 10pt; color:#808080" READONLY><%=(vbcrlf & session("strEmp"))%></textarea></font> </td>
            </tr>
			<tr><td height="15px"  bgcolor="#EDF2E6" colspan="2"></td></tr>
            <tr>
                <td colspan="2">
				      <table width="100%">
					      <tr>
						      <td bgcolor="#EDF2E6" nowrap="nowrap" valign="top"> <font size="2" color="#A2921E"><b> <span style="font-family: Arial, Tahoma, Verdana, Helvetica">Enter FeedBack</span></b></font><b><font color="#A2921E"> </font></b> </td>
							  <td width="80%"><font style="font-size: 10pt; color: #000000; font-family: Arial, Tahoma, Verdana,Helvetica"><textarea name="textFeedback" id="textFeedback" cols="100" rows="7" runat="server" style="border-style:solid; border-width:0; font-family: Verdana; font-size: 10pt;"></textarea></font></td>
                         </tr>
                         <tr>
							<td colspan="2" align="center" style="background:#F1F4EC"><asp:Button  ID="btnsubmit" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"  Text="submit" runat="server" ></asp:Button></td>
					     </tr>
                      </table>
				</td>
            </tr>
        </table>
    </form>
</body>
</html>
