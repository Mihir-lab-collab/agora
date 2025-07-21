Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Public Class LoadingNotifier
    Inherits System.Web.UI.Page
    Public Shared Response As HttpResponse
    Public Sub New(ByVal Response1 As HttpResponse)
        Response = Response1
    End Sub

    Public Sub initNotify(ByVal StrSplash As String)
        ' Only do this on the first call to the page 
        If (Not IsCallback) AndAlso (Not IsPostBack) Then
            'Register loadingNotifier.js for showing the Progress Bar 
            Response.Write(String.Format("<script type='text/javascript' src='../../includes/loadingNotifier.js'></script>" & Chr(13) & "" & Chr(10) & " <script language='javascript' type='text/javascript'>" & Chr(13) & "" & Chr(10) & " initLoader('{0}');" & Chr(13) & "" & Chr(10) & " </script>", StrSplash))
            ' Send it to the client 

            Response.Flush()
        End If

    End Sub

    Public Sub Notify(ByVal strPercent As String, ByVal strMessage As String)
        ' Only do this on the first call to the page 
        If (Not IsCallback) AndAlso (Not IsPostBack) Then
            'Update the Progress bar 

            Response.Write(String.Format("<script language='javascript' type='text/javascript'>setProgress({0},'{1}'); </script>", strPercent, strMessage))
            Response.Flush()
        End If

    End Sub
End Class
