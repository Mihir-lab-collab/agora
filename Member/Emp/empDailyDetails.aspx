<%@ Page Language="VB" Debug="true" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Web.Mail" %>
<%@ Import Namespace="System.IO" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Project Report Detail</title>
</head>

<script language="VB" runat="server">
    Dim strBody As String
    Dim intRepId As Integer
    Dim intProjId As Integer
    Dim strCommentHistory As String
    Dim Fromid As String
    Dim Mailid As String
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim Conn As SqlConnection = New SqlConnection(dsn)
    Dim dtrItemDesc As SqlDataReader
    Dim Cmd As SqlCommand
    Dim strEmpId, strEmpName As String
    Dim strfilepath, strfileext As String
    Dim len As Integer
    Dim strInsert As String
    Dim gf As New generalFunction
    Dim TargetPath As String = ConfigurationSettings.AppSettings("DataBank").ToString() + "/ProjReport/"
    '================================================================
    'PAGE LOAD TO SEE ALL THE REPORT DETAILS AT PAGE LOAD
    '================================================================
    Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        gf.checkEmpLogin()
        intRepId = CInt(Request.QueryString("repId"))
		 If Not IsPostBack Then
            Dim strItemDesc As String
            Dim dtrItemDesc As SqlDataReader
            Dim strProjName As String
            Dim dtrProjName As SqlDataReader
            Conn.Open()
            strProjName = "select * from projectMaster where projId=(select projId from  " & _
             "projDailyReport where reportId=" & intRepId & ") ORDER BY projName"
            Cmd = New SqlCommand(strProjName, Conn)
            dtrProjName = Cmd.ExecuteReader
            If dtrProjName.Read Then
                lblprjName.Text = dtrProjName("projName").ToString()
            End If
            Cmd.Dispose()
            dtrProjName.Close()
            Conn.Close()
			
            strItemDesc = "select * from  projDailyReport where reportId=" & intRepId

            Conn.Open()
			
            Cmd = New SqlCommand(strItemDesc, Conn)
            dtrItemDesc = Cmd.ExecuteReader

            If dtrItemDesc.Read Then
			              
                lblItemName.Text = Day(dtrItemDesc("reportDate")) & "-" & MonthName(Month(dtrItemDesc("reportDate")), 2) & "-" & Year(dtrItemDesc("reportDate"))
			  
                lblReportTitle.Text = dtrItemDesc("reportSubject")
                lblItemDesc.Text = dtrItemDesc("reportDesc")
                lblLastModDate.Text = Day(dtrItemDesc("reportLastModified")) & "-" & MonthName(Month(dtrItemDesc("reportLastModified")), 2) & "-" & Year(dtrItemDesc("reportLastModified"))
			   
                If IsDBNull(dtrItemDesc("reportComment")) Then
                Else
                    txtCommentHstry.Text = dtrItemDesc("reportComment")
                End If
			 
            End If
            'end if
            Cmd.Dispose()
            dtrItemDesc.Close()
			 
			
            Conn.Close()
        End If
		
    End Sub
    '================================================================
    'SEE ALL THE REPORT DETAILS AND CAN UPDATE REPORT ALSO
    '================================================================
    Sub btninsert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            
    
            Dim strReportComment As String
            Dim strUpdate As String
            Dim EmpName As String
            Dim strComment As String
            Dim empCmd As SqlCommand
            Dim empDataDr As SqlDataReader
            Dim strEmpName As String
            Dim strFileName As String
            Dim strFName As String
	 
            EmpName = "select empName from employeeMaster where empId=" & CStr(Session("dynoEmpIdSession")) & ""
            Conn.Open()
            empCmd = New SqlCommand(EmpName, Conn)
            empDataDr = empCmd.ExecuteReader
            If empDataDr.Read Then
                strEmpName = empDataDr("empName")
            End If
            empDataDr.Close()
            empCmd.Dispose()
            Conn.Close()
	
            '========================CODE, IF REPORT ALREADY EXIST====================
            EmpName = "select reportComment from projDailyReport where reportId=" & intRepId
            Conn.Open()
            empCmd = New SqlCommand(EmpName, Conn)
            empDataDr = empCmd.ExecuteReader
            If (empDataDr.HasRows) Then
                If empDataDr.Read Then
                    If Not IsDBNull(empDataDr("reportComment")) Then
                        strReportComment = empDataDr("reportComment")
                    End If
		   
                End If
            End If
            empDataDr.Close()
            empCmd.Dispose()
            Conn.Close()
            '========================CODE END, IF REPORT ALREADY EXIST====================
		
            '========================================================================
            'FILE UPLOADING
            '================================================
            If (fileAdd.Value <> "") Then
                 
                strfilepath = fileAdd.PostedFile.FileName
                len = strfilepath.LastIndexOf(".")
                strfileext = strfilepath.Substring(len)
                                           
                ' only the attched file name not its path
                strFileName = System.IO.Path.GetFileName(strfilepath)
                strFName = Trim(Replace(strFileName, strfileext, "")) & "_" & Day(Now()) & Month(Now()) & Right(Year(Now()), 2) & Hour(Now()) & Minute(Now()) & Second(Now())
                fileAdd.PostedFile.SaveAs(TargetPath & strFName & strfileext)
		 		 
                strInsert = "insert into projDailyReportAttachment(reportId,attachmentFile)values(" & intRepId & ",'" & Trim(strFName & strfileext) & "')"
					
                Conn.Open()
                Cmd = New SqlCommand(strInsert, Conn)
                Cmd.ExecuteNonQuery()
                Cmd.Dispose()
                Conn.Close()
            End If
					
            If strReportComment <> " " Then
                strComment = strEmpName & ":" & Left(Now, 15) & ": " & txtComments.Text & vbCrLf & _
                "--------------------------------------------------" & vbCrLf & strReportComment & vbCrLf
            Else
                strComment = strEmpName & ":" & Left(Now, 15) & ": " & txtComments.Text & vbCrLf & _
                "--------------------------------------------------"
            End If
 
            strUpdate = "update projDailyReport set reportComment='" & gf.sqlSafe(strComment) & _
            "',reportLastModified='" & Now() & "' where reportId=" & intRepId
            Conn.Open()

            Cmd = New SqlCommand(strUpdate, Conn)

            Cmd.ExecuteNonQuery()
            Cmd.Dispose()
            Conn.Close()
 
 
            '================================================================
            'SEND MAIL TO THE CUSTOMER
            '================================================================
            Dim drCustEmail As SqlDataReader
            Dim strCustEmail, strCustMailId As String
            Conn.Open()

            strCustEmail = "select custEmail from customerMaster where custId=(select custId from projectMaster where projName='" & lblprjName.Text & "')"
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
            Dim strEmpId As String
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
 
            Dim msgMail As MailMessage
            Dim strDate As Date = Left(Now(), 9)
            msgMail = New MailMessage

            msgMail.To = Trim(strCustMailId)
            msgMail.Cc = "one@dynamicwebtech.com;" & strEmpId

            msgMail.From = Trim(strEmpId)

            msgMail.Subject = lblprjName.Text & " Project Report On " & Day(lblItemName.Text) & "-" & MonthName(Month(lblItemName.Text), 1) & "-" & Right(Year(lblItemName.Text), 2)
            msgMail.BodyFormat = MailFormat.Html
            strBody = "<table border=""1"" cellpadding=""2"" cellspacing=""0"" style=""border-collapse: collapse"" bordercolor=""#111111"" width=""600"" id=""AutoNumber1"">" & _
               "<tr><td width=""100%"" bgcolor=""#3366CC""><p align=""center"">" & _
               "<font face=""Verdana"" style=""font-size: 8pt; font-weight: 700"" color=""#FFFFFF"">" & _
               "Project Report</font></td></tr>" & _
               "<tr><td width=""100%""><font face=""Verdana"" style=""font-size: 8pt""><br>" & _
               "Project Name: " & lblprjName.Text & "<br><BR>" & _
               "Report Date: " & Day(lblItemName.Text) & "-" & MonthName(Month(lblItemName.Text), 1) & "-" & Right(Year(lblItemName.Text), 2) & "<br><BR>" & _
               "Report Title: " & lblReportTitle.Text & "<br><BR>" & _
               "Report Description: " & Replace(lblItemDesc.Text, vbCrLf, "<BR>") & "<br><BR>" & _
               "Comment: " & Replace(txtComments.Text, vbCrLf, "<BR>") & "<br><BR>" & _
               "Report By: " & strEmpName & "<br><BR>" & _
               "Comment Date: " & Day(Now()) & "-" & MonthName(Month(Now()), 1) & "-" & Right(Year(Now()), 2) & "</font></td></tr></table>"
			
            msgMail.Body = strBody

            Dim sFile As String
            If strFName <> "" Or strfileext <> "" Then
                sFile = (TargetPath & strFName & strfileext)
                Dim oAttach As MailAttachment
                oAttach = New MailAttachment(sFile)
                msgMail.Attachments.Add(oAttach)
            End If
            SmtpMail.SmtpServer = "localhost"
            'SmtpMail.SmtpServer="82.165.255.121"
            SmtpMail.Send(msgMail)

            '================================================================
            'END CODE SEND MAIL TO THE CUSTOMER
            '================================================================

            '================================
            'CLOSE WINDOW AFTER INSERT RECORD
            '=================================
            Dim sp As String
            sp = "<Script language=JavaScript>"
            sp += "window.opener.parent.location.href('empDailyReport.aspx');"
            sp += "window.close();"
            sp += "</" + "script>"
            RegisterStartupScript("script123", sp)
            Response.Write("sdsd")
        Catch ex As Exception
          
        End Try
    End Sub
</script>

<body style="margin-top:0px">
    <form id="Form1" runat="server">
        <table height="0" cellspacing="0" cellpadding="0" width="100%" border="1" bordercolor="#C5D5AE">
            <tr>
                <td align="center" style="background-color:#C5D5AE" colspan="2">
                    <font face="Verdana" color="#a2921e"><b>Project Report Detail</b></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Project:</b></font></td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:Label ID="lblprjName" runat="server" Width="408px"></asp:Label></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Report Date:</b></font></td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:Label ID="lblItemName" runat="server" Width="408px"></asp:Label></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Last Modified Date:</b></font></td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:Label ID="lblLastModDate" runat="server" Width="408px"></asp:Label></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Report Title:</b> </font>
                </td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:Label ID="lblReportTitle" runat="server" Width="408px"></asp:Label></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Report Description:</b></font><p>
                    <p>
                        &nbsp;</p>
                </td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:Label ID="lblItemDesc" runat="server" Width="408px" Height="50px"></asp:Label>
                    </font>
                </td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Comment: </b></font>
                    <p>
                        &nbsp;</p>
                </td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:TextBox ID="txtComments" runat="server" TextMode="multiline" Height="70" Width="100%">
                        </asp:TextBox></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Comment History:</b></font>
                    <p>
                        &nbsp;</p>
                </td>
                <td bgcolor="#edf2e6">
                    <font face="verdana" size="2">
                        <asp:TextBox ID="txtCommentHstry" runat="server" TextMode="multiline" Height="120"
                            Width="100%" ReadOnly="True" BorderWidth="0px">
                        </asp:TextBox></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap">
                    <font style="font-size: 10pt; color: #A2921E; font-family: Arial, Tahoma, Verdana, Helvetica">
                        <b>Attachments:</b></font>
                </td>
                <td>
                    <div style="overflow-x: hidden; overflow: scroll; height: 100px">
                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                            <%	
                                Dim strFileName As String
                                Dim dtrFileName As SqlDataReader
                                Dim strFileCmd As SqlCommand
                                Conn.Open()
                                strFileName = "select attachmentFile from projDailyReportAttachment where reportId=" & intRepId & ""
                                strFileCmd = New SqlCommand(strFileName, Conn)
                                dtrFileName = strFileCmd.ExecuteReader
                                If dtrFileName.HasRows Then
                                    While dtrFileName.Read
                            %>
                            <tr>
                                <td>
                                    <a href="/Common/Download.aspx?m=PR&f=<%=dtrFileName("attachmentFile")%>" target="_blank">
                                        <font face="Verdana" size="2" color="#A2921E">
                                            <%=dtrFileName("attachmentFile")%>
                                        </font></a>
                                </td>
                            </tr>
                            <%
                            End While
                        End If
                        strFileCmd.Dispose()
                        dtrFileName.Close()
                        Conn.Close()
                            %>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE">
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
                    <asp:Button ID="btninsert" runat="server" Text="Submit" OnClick="btninsert_Click"
                        Width="70" Height="25"></asp:Button>
                    <input type="button" id="btncancel" name="btncancel" value="Close" style="width: 70;
                        height: 25;" onclick="javascript: window.close();">
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
