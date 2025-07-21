<%@ Page Language="VB" AutoEventWireup="false" CodeFile="skillMaster.aspx.vb" Inherits="admin_skillMaster" %>

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
					<font face="Verdana" color="#a2921e" size="2"><b>Designation</b></font>
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
					<asp:DataGrid ID="grdSkill" runat="server" AllowSorting="true" Width="36%" BackColor="white"
						BorderColor="black" ShowFooter="true" CellPadding="2" Font-Names="Verdana" MaintainState="true"
						Font-Size="10pt" HeaderStyle-BackColor="lightblue" HeaderStyle-Font-Size="11pt"
						AutoGenerateColumns="false" OnEditCommand="dgrdEdit" OnCancelCommand="dgrdCancel"
						OnUpdateCommand="dgrdUpdate" DataKeyField="skillId">
						<HeaderStyle Font-Size="11pt" BackColor="#EDF2E6" ForeColor="#A2921E" Font-Bold="True">
						</HeaderStyle>
						<AlternatingItemStyle BackColor="#EDF2E6"></AlternatingItemStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Sr.">
								<ItemTemplate>
									<%# container.ItemIndex+1 %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="skillDesc" HeaderText="Designation"></asp:BoundColumn>
							<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel"
								EditText="Edit">
								<ItemStyle BackColor="#EDF2E6" ForeColor="#A2921E" />
							</asp:EditCommandColumn>
						</Columns>
					</asp:DataGrid>
				</td>
			</tr>
		</table>
	</form>
</body>
</html>
