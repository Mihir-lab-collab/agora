Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Data
Imports System.Net
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.String

Public Class generalFunction
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim cmdCust As SqlCommand
    Dim dtrCust As SqlDataReader
    Dim dstTaskManage As New DataSet
    Dim dadTaskManage As New SqlDataAdapter
    Public EmpSession As New Collection

    Public Function funcSelect(ByVal strSQL As String) As SqlDataReader
        cmdCust = New SqlCommand(strSQL, strConn)
        If (strConn.State = ConnectionState.Open) Then
            strConn.Close()
        End If
        strConn.Open()
        dtrCust = cmdCust.ExecuteReader()
        Return dtrCust
    End Function

    Public Sub funcConClose()
        strConn.Close()
    End Sub

    Public Function funcSelect1(ByVal strSQL As String) As DataSet
        funcConClose()
        cmdCust = New SqlCommand(strSQL, strConn)
        strConn.Open()
        cmdCust.ExecuteNonQuery()
        dadTaskManage = New SqlDataAdapter(cmdCust)
        dadTaskManage.Fill(dstTaskManage)
        cmdCust.Dispose()
        strConn.Close()
        Return dstTaskManage
    End Function

    Public Sub delete(ByVal strDelete As String)
        strConn.Open()
        cmdCust = New SqlCommand(strDelete, strConn)
        cmdCust.ExecuteNonQuery()
        strConn.Close()
    End Sub

    Public Function sqlSafe(ByVal strText As String) As String
        If strText & "" <> "" Then
            strText = strText.Replace("'", "''")
        End If
        sqlSafe = strText
    End Function

    Public Sub checkEmpLogin()
        Try
            EmpSession = Web.HttpContext.Current.Session("DynoEmpSession")
        Catch ex As Exception
            Web.HttpContext.Current.Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("SiteRoot") + "Login.aspx")
        End Try
    End Sub

    Public Sub checkCustLogin()
        If Not IsNumeric(Web.HttpContext.Current.Session("dynamicCustId")) And Not IsNumeric(Web.HttpContext.Current.Session("dynamicUserId")) Then
            Web.HttpContext.Current.Response.Redirect("/cust/custLogin.aspx")
        End If
    End Sub

    Public Function SendEmail(ByVal bodyStr As String, ByVal subjectStr As String, ByVal toStr As String, Optional ByVal fromStr As String = "", Optional ByVal strAttach As String = "", Optional ByVal isHTML As Boolean = True, Optional ByVal ccStr As String = "", Optional ByVal bccStr As String = "") As Boolean
        Dim objMail As New MailMessage()
        Dim i As Integer
        Dim objArr

        If toStr <> "" Then
            toStr = toStr.Replace(";", ",")
            objArr = toStr.Split(",")
            For i = 0 To UBound(objArr)
                If InStr(objArr(i), "@") > 0 Then
                    objMail.To.Add(objArr(i))
                End If
            Next
        End If

        If fromStr <> "" Then
            fromStr = fromStr.Replace(";", ",")
            objArr = fromStr.Split(",")
            For i = 0 To UBound(objArr)
                If InStr(objArr(i), "@") > 0 Then
                    objMail.CC.Add(objArr(i))
                End If
            Next
        End If
        If ccStr <> "" Then
            ccStr = ccStr.Replace(";", ",")
            objArr = ccStr.Split(",")
            For i = 0 To UBound(objArr)
                If InStr(objArr(i), "@") > 0 Then
                    objMail.CC.Add(objArr(i))
                End If
            Next
        End If

        If bccStr <> "" Then
            'Web.HttpContext.Current.Response.Write(bccStr)
            'Web.HttpContext.Current.Response.End()
            bccStr = bccStr.Replace(";", ",")
            objArr = bccStr.Split(",")
            For i = 0 To UBound(objArr)
                If InStr(objArr(i), "@") > 0 Then
                    objMail.Bcc.Add(objArr(i))
                End If
            Next
        End If

        If fromStr = "" Then
            fromStr = ConfigurationManager.AppSettings("supportEmail").ToString()
        End If

        objMail.From = New MailAddress(fromStr)
        objMail.Bcc.Add(ConfigurationManager.AppSettings("bccMail").ToString())
        objMail.Subject = subjectStr
        objMail.Body = bodyStr
        objMail.Priority = MailPriority.High

        If isHTML Then
            objMail.IsBodyHtml = True
        End If

        If strAttach <> "" Then
            strAttach = strAttach.Replace(";", ",")
            Dim strAttachArr As String() = strAttach.Split(",")

            Dim MyAttachment As Attachment
            For i = 0 To UBound(strAttachArr)
                If strAttachArr(i) <> "" Then
                    MyAttachment = New Attachment(strAttachArr(i))
                    objMail.Attachments.Add(MyAttachment)
                End If
            Next
        End If

        Dim SmtpMail As New SmtpClient("localhost")
        SmtpMail.UseDefaultCredentials = True
        SmtpMail.Port = 25

        SmtpMail.Send(objMail)
        Return True
    End Function

    Public Function GetRandomPasswordUsingGUID(ByVal length As Integer) As String
        'Get the GUID
        Dim guidResult As String = System.Guid.NewGuid().ToString()

        'Remove the hyphens
        guidResult = guidResult.Replace("-", String.Empty)

        'Make sure length is valid
        If length <= 0 OrElse length > guidResult.Length Then
            Throw New ArgumentException("Length must be between 1 and " & guidResult.Length)
        End If

        'Return the first length bytes
        Return guidResult.Substring(0, length)
    End Function

    Public Function sqlSafe1(ByVal strText1 As String) As String
        If strText1 & "" <> "" Then
            strText1 = strText1.Replace(Chr(13), "<br/>")
        End If
        sqlSafe1 = strText1
    End Function

    Public Function EnryptString(strEncrypted As String) As String
        Try
            Dim b As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted)
            Dim encryptedConnectionString As String = Convert.ToBase64String(b)
            Return encryptedConnectionString
        Catch
            Throw
        End Try
    End Function

    Public Function DecryptString(encrString As String) As String
        Try
            Dim b As Byte() = Convert.FromBase64String(encrString)
            Dim decryptedConnectionString As String = System.Text.ASCIIEncoding.ASCII.GetString(b)
            Return decryptedConnectionString
        Catch
            Throw
        End Try
    End Function
End Class

