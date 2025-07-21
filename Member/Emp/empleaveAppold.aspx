<%@ Page Language="VB" ContentType="text/html" ResponseEncoding="iso-8859-1" %>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<%@ Import NameSpace="System.Web.Mail" %>
<%@ Import NameSpace="System.Web.Mail.MailMessage" %>
<%@ Import NameSpace="System.Web.Mail.SmtpMail" %>
<%@ Import NameSpace="System.Web.Mail.MailAttachment" %>

<script language="VB" runat="server">
Dim sql  as String
Dim dsn As String = ConfigurationSettings.AppSettings("dsn")
Dim conn As SqlConnection = New SqlConnection(dsn)
Dim objdatareader As SqlDataReader

Sub Page_Load(sender As Object, e As EventArgs)
Dim Sqlcmd as new sqlcommand("select statusdesc, statusid from empStatus where sttatusLeave=1", conn)
conn.open()
objdatareader= SqlCmd.ExecuteReader()
While objdatareader.Read()
	dropLeaveType.items.add(new listitem(objdatareader(0),objdatareader(1)))
End While
 conn.close()
End Sub

    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
                    
            Dim strreason As String
            Dim strDateFrom As String
            Dim strDateTo As String
            strreason = Request.Form("txtRofLeave")
            'strreason=Replace(strreason,"'","''")
            strDateFrom = Request.Form("txtDateFrom")
            strDateTo = Request.Form("txtDateTo")
            'response.Write(strDateFrom)
            'response.End()
            Dim strSQl As String
            Dim sp1 As String

            ' strSQl = "insert into empLeaveDetails(empId,leaveId,leaveFrom,leaveTo,leaveDesc) values( " & Session("dynoEmpIdSession") & ",'" & dropLeaveType.SelectedValue & "','" & txtDateFrom.Text & "','" & txtDateTo.Text & "','" & txtRofLeave.Text.Replace("'", "''") & "')"
            strSQl = "insert into empLeaveDetails(empId,leaveId,leaveFrom,leaveTo,leaveDesc) values( " & Session("dynoEmpIdSession") & ",'" & dropLeaveType.SelectedValue & "',cast('" & txtDateFrom.Text & "' as smalldatetime),cast('" & txtDateTo.Text & "' as smalldatetime),'" & txtRofLeave.Text.Replace("'", "''") & "')"

            Dim Sqlcmd As New SqlCommand(strSQl, conn)
            conn.Open()
            Sqlcmd.ExecuteNonQuery()
            Sqlcmd.Dispose()
            conn.Close()
 
            Dim strmail As String = ConfigurationSettings.AppSettings("Email")
            Dim Mailmsg As New MailMessage
            Dim strSmtpServer As String = ConfigurationSettings.AppSettings("smtpServerAddress")
            Dim strempEmail As String
            Dim strempName As String
            strempEmail = ""
            strempName = ""
            Sqlcmd = New SqlCommand("select empName,empEmail from employeeMaster where empId= " & Session("dynoEmpIdSession") & " ", conn)
            conn.Open()
            objdatareader = Sqlcmd.ExecuteReader()
            While objdatareader.Read()
                strempEmail = objdatareader("empEmail")
                strempName = objdatareader("empName")
            End While
            objdatareader.Close()
            Sqlcmd.Dispose()
            conn.Close()
				
            Mailmsg.From = strempEmail '"leave_alert@dynamicwebtech.com"
            Mailmsg.To = "kapil@dynamicwebtech.com,neeraj@dynamicwebtech.com"
            'Mailmsg.To = "rajesh.patel@dynamicwebtech.com,rajeshimrd@gmail.com"
            'Mailmsg.cc = "phool.chandra@dynamicwebtech.com"
            Mailmsg.Subject = strempName & " " & "has applied for leave "
            'Mailmsg.Body = strMsgType & " by " &  strcustName & ":" & strBody
            Mailmsg.Body = strempName & " " & "has applied for leave " & "<b>From</b>" & ":" & strDateFrom & " " & "<b>TO</b>" & ":" & strDateTo & " " & "<br> Reason for Leave Is" & "<b>:</b>" & strreason
            Mailmsg.BodyFormat = MailFormat.Html
            Mailmsg.Priority = MailPriority.Normal
            SmtpMail.SmtpServer = "localhost"
            SmtpMail.Send(Mailmsg)
					
            sp1 = "<Script language=JavaScript>"
            sp1 += "window.opener.location=window.opener.location;"
            sp1 += "window.close();"
            'sp1+ ="alert('in');"
            sp1 += "</" + "script>"
            'response.write (sp1)
            RegisterStartupScript("script123", sp1)
            'Response.Redirect("empLeave.aspx")
        Catch ex As Exception

        End Try
    End Sub

</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
  <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
<meta http-equiv="Content-Language" content="en-us">
<title>Employee Leave Management</title>
</head>
 <script language="JavaScript" src="../includes/CalendarControl.js" type="text/javascript">
    </script>
<%--<script language="JavaScript" src="/includes/calender.js">
</script>--%>

<script language="javascript">
function chkvalues()
{ 
if(document.getElementById("txtDateFrom").value=="") 
	{ 
	alert("Please select From Date");	
	return false;
	}
 else if(document.getElementById("txtDateTo").value =="")
	{ 
		alert("Please select TO date");
		return false;
	}
 else if(document.getElementById("txtRofLeave").value =="")
	{ 
		alert("Please give the reason");
		return false;
	}
}
</script>
<body>
<form id="Form1" method="post" onSubmit="return chkvalues();" runat="server" action="empleaveApp.aspx?save=Yes">
<table width="100%"  border="0" height="218" cellSpacing="0" cellPadding="0" align="center">
  <tr>
    <td width="49%" height="41" align="center" bgcolor="#C5D5AE" ><b>
	   <font face="Verdana" color="#a2921e" size="4"  >Employee Leave Details/Management</b></td>
    </tr>
  <tr valign="top">
    <td height="132"><table width="100%" height="10"  border="1" cellpadding="1" cellspacing="0"  bordercolor="#C5D5AE" >
      <tr align="center">
        <td  colspan="4" bgcolor="#fffff" height="1">&nbsp;</td>
      </tr>
      <tr align="center">
        <td  colspan="4" bgcolor="#C5D5AE" height="30"><b> <font face="Verdana" color="#a2921e" size="2"  >
        Add Leave</font></b></td>
      </tr>
      <tr >
        <td colspan="3" align="left" nowrap=true bgcolor="#EDF2E6"  height="29"> <font face="Verdana" size="2" color="#A2921E"> 
        Leave Type </font></td>
        <td width="276" bgcolor="#EDF2E6">
          <asp:DropDownList ID="dropLeaveType" runat="server">
          </asp:DropDownList>
        </td>
      </tr>
      <tr>
        <td nowrap=true align="left" width="92"  bgcolor="#EDF2E6"  height="29"> <font face="Verdana" size="2" color="#A2921E"> 
        Leave From </font></td>
        <td width="64" align="left" bgcolor="#EDF2E6">
          <asp:TextBox  ID=txtDateFrom size="7" onclick="popupCalender('txtDateFrom')"  runat="server" Width="91px" onkeypress="return false;"/></td>
        <td width="31"  bgcolor="#EDF2E6"  height="29" align="center"> <font face="Verdana" size="2" color="#A2921E"> 
        TO</font></td>
        <td width="276" align="left" bgcolor="#EDF2E6">
          <asp:TextBox ID=txtDateTo size="7" onclick="popupCalender('txtDateTo')" runat="server" Width="91px" onkeypress="return false;"/></td>
      </tr>
      <tr>
        <td colspan="3" nowrap=true align="left"  bgcolor="#EDF2E6"  height="29"> <font face="Verdana" size="2" color="#A2921E"> 
        Reason Of Leave </font></td>
        <td width="276" align="left" nowrap="true" bgcolor="#EDF2E6" >
          <asp:TextBox ID="txtRofLeave" MaxLength="255" TextMode="SingleLine" Width="100%" runat="server" />    
          <font face="Verdana" size="1" color="#A2921E" nowrap="true"> </font></td>
      </tr>
      <tr align="center"  bgcolor="#EDF2E6">
        <td height="44" colspan=4>
          <asp:Button ID="btnSubmit" Text="Submit" runat="server"  onclick="btnSubmit_Click"/></td>
      </tr>
    </table></td>
  </tr>
</table>
</form>
</body>
</html>