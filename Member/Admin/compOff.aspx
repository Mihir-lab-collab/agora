<%@ Page Language="VB" AutoEventWireup="false" CodeFile="compOff.aspx.vb" Inherits="admin_compOff" %>

<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Dyno Admin Control </title>
    <link rel="stylesheet" href="/includes/CalendarControl.css" type="text/css" />
</head>
<body>
    <script language="javascript" src="/includes/CalendarControl.js" type="text/javascript">
    </script>
    <ucl:adminMenu ID="adminMenu" runat="server" />
    <form id="form1" runat="server">
    <br />
    <table border="1" cellpadding="4" style="border-collapse: collapse; border-color: #E8E8E8;"
        align="center" width="100%">
        <tr>
            <td colspan="7" valign="top" align="center" style="background-color: #C5D5AE">
                <font face="Verdana" color="#a2921e" size="4"><b>Comp Off Details</b></font>
            </td>
        </tr>
        <tr>
            <td style="background-color: #edf2e6; width: 11%">
                <font face="Verdana" color="#a2921e" size="2"><b>Employee Name </b></font>
            </td>
            <td>
                <asp:DropDownList ID="empddl" runat="server" AppendDataBoundItems="True">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:Button ID="btnView" runat="server" Text="View" BackColor=" #C5D5AE" Font-Size="10pt"
                    ForeColor=" #A2921E" Font-Bold="true" />
            </td>
        </tr>
        <%--<td>
                <td style="background-color: #edf2e6; width: 11%">
					<font face="Verdana" color="#a2921e" size="2"><b>Comp Off Date</b></font>
				</td>
					<asp:TextBox ID="txtDate" runat="server" size="14" onclick="popupCalender('txtDate')"
						onkeypress="return false;"> </asp:TextBox>
				</td>
				<td style="background-color: #edf2e6; width: 13%">
					<font face="Verdana" color="#a2921e" size="2"><b>Comp Off Comment</b></font>
				</td>
				<td nowrap="nowrap">
					<asp:TextBox ID="txtComment" runat="server" Columns="50"></asp:TextBox>
					<asp:RequiredFieldValidator ID="valrDate" runat="server" Display="Dynamic" ControlToValidate="txtComment"
						ErrorMessage="Required"></asp:RequiredFieldValidator>
				</td>
				<td style="background-color: #edf2e6; width: 8%" align="center">
					<asp:Button ID="btnSubmit" runat="server" Text="Submit" BackColor=" #C5D5AE" Font-Size="10pt"
						ForeColor=" #A2921E" Font-Bold="true" />
				</td>
			</tr>--%>
    </table>
    <table>
        <tr>
            <td width="20%" height="10%" align="left">
                <asp:Label ID="lblInfoMsg" Font-Names="Verdana" ForeColor="#A2921E" Text="No Data Found"
                    runat="server" Visible="False" Font-Bold="True" Font-Size="10pt"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <center>
        <asp:DataGrid ID="dgrdCompOff" runat="server" Width="90%" BackColor="White" BorderColor="Black"
            CellPadding="2" Font-Names="Verdana" MaintainState="true" Font-Size="10pt" HeaderStyle-BackColor="lightblue"
            HeaderStyle-Font-Size="11pt" AutoGenerateColumns="False" DataKeyField="coID">
            <HeaderStyle Font-Size="11pt" BackColor="#EDF2E6" ForeColor="#A2921E" Font-Bold="True">
            </HeaderStyle>
            <AlternatingItemStyle BackColor="#EDF2E6"></AlternatingItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="Sr.">
                    <ItemTemplate>
                        <%# container.ItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="empId" HeaderText="Employee Id"></asp:BoundColumn>
                <asp:BoundColumn DataField="empName" HeaderText="Employee Name"></asp:BoundColumn>
                <asp:BoundColumn DataField="coDate" HeaderText="Comp Off Date" DataFormatString="{0:dd-MMM-yyyy ddd}">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="coComment" HeaderText="Comp Off Comment"></asp:BoundColumn>
                <asp:BoundColumn DataField="entryDate" HeaderText="Entry Date" DataFormatString="{0:dd-MMM-yyyy ddd}">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="entryBy" HeaderText="Entry By"></asp:BoundColumn>
                <%--	<asp:TemplateColumn>
						<ItemTemplate>
							<a href="compOff.aspx?CoID=<%#Eval("coID") %>">Edit</a>
						</ItemTemplate>
					</asp:TemplateColumn>--%>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" Text="delete" CommandName="delete"
                            CausesValidation="false" OnClientClick="return confirm('Are you sure,you want to delete this record ?');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <a href="empHolidayWorkDetails.aspx?Flag=A&Empid=<%#Eval("empId") %>">View</a>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle Mode="NumericPages" />
        </asp:DataGrid>
    </center>
    </form>
</body>
</html>
