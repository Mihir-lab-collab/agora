<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EmpPenalty.aspx.vb" Inherits="admin_skillMaster" %>

<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Dyno Admin Control </title>
</head>
<body>
	<ucl:adminMenu ID="adminMenu" runat="server" />
	<form runat="server">
		<table cellpadding="4" width="40%" border="1" style="border-collapse: collapse; border-color: #e8e8e8"
			align="center">
			<tr>
				<td style="background-color:#edf2e6">
					<font face="Verdana" color="#a2921e" size="2"><b>Skill </b></font>
				</td>
				<td>
					<asp:TextBox ID="txtskill" runat="server"></asp:TextBox></td>
				<td>
					<input type="button" id="addNew" runat="server" value="Add New" align="right" style="font-family: Verdana;
						font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" font-bold="true" /></td>
			</tr>
		</table>
		<table width="100%">
			<tr>
				<td height="12px" align="center">
				</td>
			</tr>
			<tr>
				<td align="center">
					<asp:DataGrid ID="grdSkill" runat="server" AllowSorting="True" Width="80%" BackColor="White"
						BorderColor="Black" CellPadding="2" Font-Names="Verdana" MaintainState="true"
						Font-Size="10pt" HeaderStyle-BackColor="lightblue" HeaderStyle-Font-Size="11pt"
						AutoGenerateColumns="False" OnEditCommand="dgrdEdit" OnCancelCommand="dgrdCancel"
						OnUpdateCommand="dgrdUpdate" DataKeyField="skillId">
						<HeaderStyle Font-Size="11pt" BackColor="#EDF2E6" ForeColor="#A2921E">
						</HeaderStyle>
						<AlternatingItemStyle BackColor="#EDF2E6"></AlternatingItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="EmpPDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yy}">
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="EmpName" HeaderText="Name">
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PDesc" HeaderText="Reason">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="EmpPCharge" HeaderText="Amount" DataFormatString="{0:##,###}">
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="empPComment" HeaderText="Comment">
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" Width="50%" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                        </Columns>
					</asp:DataGrid>
				</td>
			</tr>
		</table>
	</form>
</body>
</html>
