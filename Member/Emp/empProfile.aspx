<%@ Page Language="vb" AutoEventWireup="false" enableSessionState="true"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD>
		<meta http-equiv="Content-Language" content="en-us">
		<title>Dynamic Web Tech</title>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="Table3" height="100%" cellSpacing="0" cellPadding="2" width="100%" align="center"
				border="0">
				<tr>
					<td colspan="3">
                    <EMPHEADER:empHeader id="Empheader" runat="server">
                    </EMPHEADER:empHeader><BR>
						<uc1:empMenuBar id="EmpMenuBar" runat="server">
                    </uc1:empMenuBar></td>
				</tr>
				<tr>
					<td align="center" height="90%" colspan="3">
					Profile Details
					</td>
				</tr>					
			</table>
		</form>
	</body>
</html>