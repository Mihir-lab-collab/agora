<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>New DAILY Report</title>

    <script language="JavaScript" src="/includes/calender.js">
    </script>

</head>

<script language="VB" runat="server">
    Dim strBody As String
    Dim objCmd As SqlCommand
    Dim objDataReader As SqlDataReader
    Dim strSQL As String
    Dim intRepId As Integer
    Dim intProjId As Integer
    Dim strCommentHistory As String
    Dim Fromid As String
    Dim Mailid As String
    Dim dsn As String = ConfigurationSettings.AppSettings("conString")
    Dim Conn As SqlConnection = New SqlConnection(dsn)
    Dim dtrItemDesc As SqlDataReader
    Dim Cmd As SqlCommand
    Dim strEmpId, strEmpName As String
    Dim strfilepath, strfileext As String
    Dim len As Integer
    Dim strInsert As String
    Dim intReportId As Integer
    Dim gf As New generalFunction
    '================================================================
    'PAGELOAD TO BIND THE DROPDOWN
    '===============================================================
    Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        gf.checkEmpLogin()
        If CStr(Session("dynoEmpIdSession")) = "" Then
            Response.Redirect("emplogin.aspx")
        End If
        If Not IsPostBack Then
            BindDropDown()
        End If
    End Sub
    '================================================================
    'END PAGELOAD TO BIND THE DROPDOWN
    '================================================================
    '================================================================
    ' BIND DROPDOWN
    '================================================================
    Sub BindDropDown()
        ddlProj.Items.Clear()
        ddlProj.Items.Add(New ListItem("-Select-", 0))
        If Session("dynoBugAdminSession") = 1 Then
            strSQL = "select projName,projId from projectMaster"
            'start code from here
        Else
            strSQL = "select projName,projId from projectMaster where projId in(select projId from projectMember where empId=" & CStr(Session("dynoEmpIdSession")) & ")or projManager=" & CStr(Session("dynoEmpIdSession")) & ""
        End If
	
        objCmd = New SqlCommand(strSQL, Conn)
        Conn.Open()
        objDataReader = objCmd.ExecuteReader
        If (objDataReader.HasRows) Then
            While objDataReader.Read
                ddlProj.Items.Add(New ListItem(objDataReader("projName"), objDataReader("projId")))
            End While
        End If
        Conn.Close()
        objDataReader.Close()
        objCmd.Dispose()
    End Sub
    '================================================================
    ' END BIND DROPDOWN
    '===============================================================
    '================================================================
    ' CLOSE WINDOW IN CANCEL CLICK
    '===============================================================
    Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sp As String
        sp = "<Script language=JavaScript>"
        sp += " window.close();"
        sp += "</" + "script>"
        RegisterStartupScript("script123", sp)
    End Sub
    '================================================================
    'END CODE CLOSE WINDOW IN CANCEL CLICK
    '===============================================================
    '================================================================
    ' INSERT VALUES FOR DAILY PROJECT REPORT IN ADD BUTTON CLICK
    '===============================================================
    Sub btninsert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim EmpName As String
        Dim strComment As String
        Dim empCmd As SqlCommand
        Dim empDataDr As SqlDataReader
        Dim strEmpName As String
        Dim strFileName As String
        Dim strFName As String
 
        If ddlProj.SelectedItem.Value <> 0 Then
            If CStr(Session("dynoEmpIdSession")) <> "" Then
                EmpName = "select empName from employeeMaster where empId=" & CStr(Session("dynoEmpIdSession")) & ""
                Conn.Open()
                empCmd = New SqlCommand(EmpName, Conn)
                empDataDr = empCmd.ExecuteReader
                If empDataDr.Read Then
                    strEmpName = empDataDr("empName")
                End If
            End If
            empDataDr.Close()
            empCmd.Dispose()
            Conn.Close()
            strComment = txtComments.Text
            Dim empId As Integer = CInt(Session("dynoEmpIdSession"))
            strInsert = ""
            strInsert = "Insert into projDailyReport(projId,reportDate,reportSubject,reportDesc,reportComment,reportEmpId)values(" & ddlProj.SelectedItem.Value & ",'" & newDate.Value & "','" & sqlSafe(txtSubject.Value) & "','" & sqlSafe(txtDesc.Text) & "','" & sqlSafe(strComment) & "'," & empId & ")"

            Conn.Open()
            Cmd = New SqlCommand(strInsert, Conn)
            Cmd.ExecuteNonQuery()
            Cmd.Dispose()
            Conn.Close()
            '========================================================================
            'FILE UPLOADING
            '================================================
            If (fileAdd.Value <> "") Then
                Dim strReportId As String = "select max(reportId)as reportId from projDailyReport"
                Conn.Open()
                empCmd = New SqlCommand(strReportId, Conn)
                empDataDr = empCmd.ExecuteReader
                If empDataDr.HasRows Then
                    If empDataDr.Read Then
                        intReportId = Trim(empDataDr("reportId"))
                    End If
                End If
  
                empDataDr.Close()
                empCmd.Dispose()
                Conn.Close()
    
                strfilepath = fileAdd.PostedFile.FileName
                len = strfilepath.LastIndexOf(".")
                strfileext = strfilepath.Substring(len)
                                           
                ' only the attched file name not its path
                strFileName = System.IO.Path.GetFileName(strfilepath)
                strFName = Trim(Replace(strFileName, strfileext, "")) & "_" & Day(Now()) & Month(Now()) & Right(Year(Now()), 2) & Hour(Now()) & Minute(Now()) & Second(Now())
                fileAdd.PostedFile.SaveAs(Server.MapPath("./projReportAttachment/") & strFName & strfileext)
                strInsert = ""
                strInsert = "insert into projDailyReportAttachment(reportId,attachmentFile)values(" & intReportId & ",'" & Trim(strFName & strfileext) & "')"
					
                Conn.Open()
                Cmd = New SqlCommand(strInsert, Conn)
                Cmd.ExecuteNonQuery()
                Cmd.Dispose()
                Conn.Close()
            End If
					
            '========================================================================
            'END FILE UPLOADING
            '================================================
            '================================================================
            'SEND MAIL TO THE CUSTOMER
            '================================================================
            Dim drCustEmail As SqlDataReader
            Dim strCustEmail, strCustMailId As String
            Conn.Open()
            strCustEmail = "select custEmail from customerMaster where custId=(select custId from " & _
            "projectMaster where projId =" & ddlProj.SelectedItem.Value & ")"
            Cmd = New SqlCommand(strCustEmail, Conn)
            drCustEmail = Cmd.ExecuteReader
            If drCustEmail.HasRows Then
                If drCustEmail.Read Then
                    strCustMailId = drCustEmail("custEmail")
                End If
            End If
            Cmd.Dispose()
            drCustEmail.Close()
            Conn.Close()
 
            Dim strEmpMailId As String
 
            Dim dtrMailId As SqlDataReader
            Dim sBody As StringBuilder
            Conn.Open()
            strEmpMailId = "select * from employeeMaster where empId=" & CStr(Session("dynoEmpIdSession")) & ""
            Cmd = New SqlCommand(strEmpMailId, Conn)
            dtrMailId = Cmd.ExecuteReader
            If dtrMailId.HasRows Then
                If dtrMailId.Read Then
                    strEmpId = dtrMailId("empEmail")
                    strEmpName = dtrMailId("empName")
                End If
            End If
 
            Cmd.Dispose()
            dtrMailId.Close()
            Conn.Close()
            Dim strDate As Date = Left(Now(), 9)

            strBody = "<table border=""1"" cellpadding=""2"" cellspacing=""0"" style=""border-collapse: collapse"" bordercolor=""#111111"" width=""600"" id=""AutoNumber1"">" & _
                "<tr><td width=""100%"" bgcolor=""#3366CC""><p align=""center"">" & _
                "<font face=""Verdana"" style=""font-size: 8pt; font-weight: 700"" color=""#FFFFFF"">" & _
                "<B>Project Report</B></font></td></tr>" & _
                "<tr><td width=""100%""><font face=""Verdana"" style=""font-size: 8pt""><br>" & _
                "<B>Project Name:</B> " & ddlProj.SelectedItem.Text & "<br><BR>" & _
                "<B>Report Date:</B> " & Day(newDate.Value) & "-" & MonthName(Month(newDate.Value), 2) & "-" & Right(Year(newDate.Value), 2) & "<br><BR>" & _
                "<B>Report Title:</B> " & txtSubject.Value & "<br><BR>" & _
                "<B>Report Description:</B> " & Replace(txtDesc.Text, vbCrLf, "<BR>") & "<br><BR>" & _
                "<B>Comment:</B> " & Replace(txtComments.Text, vbCrLf, "<BR>") & "<br><BR>" & _
                "<B>Report By:</B> " & strEmpName & "<br><BR>" & _
                "<B>Comment Date:</B> " & Day(Now()) & "-" & MonthName(Month(Now()), 1) & "-" & Right(Year(Now()), 2) & "</font></td></tr></table>"
				

            Dim sFile As String
            sFile = (Server.MapPath("./projReportAttachment/") & strFName & strfileext)

            If Not File.Exists(sFile) Then
                sFile = ""
            End If

            gf.SendEmail(strBody, ddlProj.SelectedItem.Text & " Project Report On " & newDate.Value, strCustMailId, strEmpId, sFile, True)

            Dim sp As String
            sp = "<Script language=JavaScript>"
            sp += "window.opener.parent.location.href('empDailyReport.aspx');"
            sp += "window.close();"
            sp += "</" + "script>"
            RegisterStartupScript("script123", sp)
        Else
            Dim sp As String
            sp = "<Script language=JavaScript>"
            sp += " alert('Please Select Any Project');"
            sp += "</" + "script>"
            RegisterStartupScript("script123", sp)
        End If
 
        Conn.Close()
    End Sub

    Function sqlSafe(ByVal str)
        If str & "" <> "" Then
            str = Replace(str, "'", "''")
        End If
        sqlSafe = str
    End Function
	
    '================================================================
    'END CODE INSERT VALUES FOR DAILY PROJECT REPORT IN ADD BUTTON CLICK
    '===============================================================	
		
</script>

<body topmargin="0">
    <form id="Form1" runat="server">
        <table height="0" cellspacing="0" cellpadding="0" width="100%" border="1" bordercolor="#C5D5AE">
            <tr width="50%">
                <td align="center" bgcolor="#C5D5AE" colspan="2">
                    <font face="Verdana" color="#a2921e"><b>Add Report</b></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Project:</b></font></td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:DropDownList ID="ddlProj" runat="server">
                        </asp:DropDownList></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Report Date:</b></font></td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <input type="text" id="newDate" runat="server" onkeypress="return false;" onclick="popupCalender('newDate')"
                            size="20">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Valid Date"
                            ControlToValidate="newDate">
                        </asp:RequiredFieldValidator>
                    </font>
                </td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Report Title:</b> </font>
                </td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <input type="text" id="txtSubject" runat="server" style="width: 90%" size="20"></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Report Description:</b></font></P>
                </td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:TextBox ID="txtDesc" runat="server" TextMode="multiline" Height="200px" Width="100%">
                        </asp:TextBox></font></td>
            </tr>
            <tr style="display: none">
                <td bgcolor="#C5D5AE" valign="top">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Comment: </b></font></P>
                    <p>
                        &nbsp;</p>
                </td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:TextBox ID="txtComments" runat="server" TextMode="multiline" Height="90px" Width="100%">
                        </asp:TextBox></font>
                </td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Add File:</b></font>
                </td>
                <td bgcolor="#edf2e6">
                    <input id="fileAdd" type="file" runat="server" style="height: 25; width: 300;" size="20">
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Button ID="btninsert" runat="server" Text="Add" OnClick="btninsert_Click" Width="70"
                        Height="25"></asp:Button>
                    <asp:Button ID="btncancel" runat="server" Text="Cancel" OnClick="btncancel_Click"
                        Width="70" Height="25"></asp:Button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
