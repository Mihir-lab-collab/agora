<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empAtt.aspx.vb" Inherits="emp_empAtt"
	Debug="true" %>

<%@ Register Src="~/controls/empMenuBar.ascx" TagName="empMenuBar" TagPrefix="uc1" %>
<%@ Register Src="~/controls/empHeader.ascx" TagName="empHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Attendance Register</title>

	<script language="javascript" type="text/javascript">

function timeset()
{
	dt =new Date();
	Dthour=dt.getHours();
	document.Attendence.updateTime.value = Dthour.toString()  + ":" + dt.getMinutes().toString() + ":" + dt.getSeconds().toString();
	setTimeout("timeset()",1000);
}
setTimeout("timeset()",1000);

	</script>

</head>
<body>
	<form id="Attendence" runat="server">
		<table cellspacing="0" cellpadding="4" width="100%" border="0" style="border-collapse: collapse;
			border-color: #111111;" align="center">
			<tr>
				<td>
					<uc2:empHeader ID="EmpHeader1" runat="server" />
				</td>
			</tr>
			<tr>
				<td>
					<uc1:empMenuBar ID="EmpMenuBar1" runat="server" />
				</td>
			</tr>
            <tr bgcolor="#edf2e6">
            <td bgcolor="#edf2e6" align="left" >
                <b><font face="Verdana" size="2">
                    
                        <%
                            Dim strDate = Request.QueryString("strDate")
                            If strDate = Nothing Then
                                strDate = Now()
                            ElseIf Not IsDate(strDate) Or Month(strDate) = Month(Date.Today) Then
                                strDate = Now()
                            End If									
                        %>
                        <a href="empAtt.aspx?strDate=<% =strDate%>" style="color:#A2921E;"><font color="#A2921E">Attendance</font></a>|<a
                            href="HolidayWorking.aspx?strDate=<% =strDate%>" style="color:#A2921E;"><font color="#A2921E" >Holiday Working</font></a>
                </b>
            </td>
        </tr>
			<tr>
				<td align="center">
					<table id="Table1" cellspacing="0" cellpadding="2" width="32%" border="1" style="border-collapse: collapse;
						border-color: #EFEBDC" runat="server">
						<tr style="background-color: #C5D5AE">
							<td colspan="4" align="center">
								<%
									Dim strDate = Request.QueryString("strDate")
								    If strDate = Nothing Then
								        strDate = Now()

								    ElseIf Not IsDate(strDate) Or Month(strDate) = Month(Date.Today) Then
								        strDate = Now()
								    End If
									
							
								%>
								<font color="#A2921E">
									<input type="text" disabled="disabled" style="border: 0px none; text-align: center;
										font-weight: bold" name="UpdateDate" size="10" value='<%=day(strDate) & "-" & Left(MonthName(Month(strDate)), 3) & "-" & Year(strDate)%>' />
									<input type="text" disabled="disabled" style="border: 0px none; text-align: center;
										font-weight: bold" name="updateTime" size="8" />
								</font>
							</td>
						</tr>
						<tr style="background-color: #C5D5AE">
							<td style="width:35%;white-space:nowrap" align="center">
								<%
									Dim strDate = Request.QueryString("strDate")
								    If strDate = Nothing Then
								        strDate = Now()

								    ElseIf Not IsDate(strDate) Or Month(strDate) = Month(Date.Today) Then
								        strDate = Now()
								    End If
									
									Dim nextMonth = FormatDateTime((DateAdd("m", 1, "1-" & MonthName(Month(strDate)) & "-" & Year(strDate))), 2)
									Dim prevMonth = FormatDateTime((DateAdd("m", -1, "1-" & MonthName(Month(strDate)) & "-" & Year(strDate))), 2)
								%>
								<a href="empAtt.aspx?strDate=<% =prevMonth%>" style="text-decoration: none"><font
									color="#A2921E"><b><<</b></font></a>&nbsp; <font color="#A2921E"></font><font face="Verdana"
										size="2" color="#A2921E"><b>
											<%=Left(MonthName(Month(strDate)), 3) & " " & Year(strDate)%>
										</b></font><font color="#A2921E"></font><a href="empAtt.aspx?strDate=<%=nextMonth%>"
											style="text-decoration: none"><font color="#A2921E"><b>&nbsp;>></b></font></a><font color="#A2921E"></font>
											</td>
							<td style="width: 3%;white-space:nowrap" align="center">
								<font face="Verdana" size="2" color="#A2921E"><b>Status</b></font></td>
							<td style="width: 2%;white-space:nowrap" align="center">
								<font face="Verdana" size="2" color="#A2921E"><b>Check In</b></font></td>
							<td style="width: 2%;white-space:nowrap" align="center">
								<font face="Verdana" size="2" color="#A2921E"><b>Check Out</b></font></td>
						</tr>
						<tr>
							<td colspan="4">
								<asp:Table ID="attTable" runat="server" Font-Names="Verdana" CellPadding="2" CellSpacing="0"
									Width="100%">
								</asp:Table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<%--<center>--%>
		<%--	</center>--%>
	</form>
</body>
</html>
