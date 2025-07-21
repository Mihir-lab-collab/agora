<%@ Page Language="VB" DEBUG="TRUE" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.string" %>
<%@import Namespace="System.Data.SqlClient"%>
<%@ Import NameSpace="System.Web.Mail" %>
<%@ Import NameSpace="System.Web.Mail.MailMessage" %>
<%@ Import NameSpace="System.Web.Mail.SmtpMail" %>
<%@ Import NameSpace="System.Web.Mail.MailAttachment" %>

<html xmlns="http://www.w3.org/1999/xhtml" >

<head>
<meta name="GENERATOR" content="Microsoft FrontPage 5.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<title>Delete Complaint</title>
</head>
<script language="VB" runat="server">

Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
Dim Conn As SqlConnection = New SqlConnection(dsn)
Dim Cmd as New SQLCommand()
Dim i as string 
Dim strOrderBy As string
dim id as integer
dim strSQL AS String
Dim dtrInfo as SqlDataReader
    Dim gf As New generalFunction

    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

        '******************************************************************
        'this code delete the selected record from database
        '******************************************************************
        gf.checkEmpLogin()
        id = Request.QueryString(id)
        strSQL = "DELETE FROM changeRequest where chgid=" & id
        Conn.Open()
        Cmd = New SqlCommand(strSQL, Conn)
        dtrInfo = Cmd.ExecuteReader()
        Conn.Close()
        Cmd.Dispose()
    	
        Response.Redirect("show_ch_request_to_mgr.aspx")
    End Sub

</script>


<body>

</body>

</html>