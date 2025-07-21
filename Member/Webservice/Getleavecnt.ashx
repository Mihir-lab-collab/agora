<%@ WebHandler Language="VB" Class="Getleavecnt" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient

Public Class Getleavecnt : Implements IHttpHandler
    Dim list As String
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim Frmdt As String = context.Request.QueryString("Frmdt")
        Dim Todt As String = context.Request.QueryString("Todt")
        Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
        Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
        Using cmd As New SqlCommand("SELECT dbo.GetNoOfLeavesApplied('" & DateTime.Parse(Frmdt) & "','" & DateTime.Parse(Todt) & "')", strConn)
            strConn.Open()
            Dim result = cmd.ExecuteScalar()

            strConn.Close()
            Dim oSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
            list = oSerializer.Serialize(result)
        End Using
        context.Response.Write(list)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
   
End Class