Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class admin_ajaxGet
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
	Dim Cmd As New SqlCommand
	Dim dtr As SqlDataReader
	Dim sql As String
	Dim projNm As String
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("pojectId") <> "" Then
            Fillemail()
        Else
            Dim strpprojCode As String = Request.QueryString("pprojCode")

            Dim projNm As String = Request.QueryString("pojectName")

            If projNm <> "" Then
                sql = "select projId from projectMaster where projName= '" & projNm & "'  and  projId <> '" & strpprojCode & "'"
            Else
                sql = "select projId from projectMaster where projName= '" & projNm & "'"
            End If


            strConn.Open()
            Cmd = New SqlCommand(sql, strConn)
            dtr = Cmd.ExecuteReader()
            If dtr.Read Then
                Response.Write("True")
            Else
                Response.Write("False")
            End If
            dtr.Close()
            strConn.Close()
        End If
    End Sub
    Sub Fillemail()
        projNm = Request.QueryString("pojectId")
        Dim drCustEmail As SqlDataReader
        Dim strCustEmail, strCustMailId, strCustName As String
        strCustMailId = ""
        strCustName = ""
        strConn.Open()
        strCustEmail = "SELECT custEmail,custName FROM customerMaster WHERE custId=(SELECT custId FROM projectMaster WHERE projId =" & projNm & ")"

        Cmd = New SqlCommand(strCustEmail, strConn)
        drCustEmail = Cmd.ExecuteReader
        If drCustEmail.HasRows Then
            If drCustEmail.Read Then
                strCustMailId = drCustEmail("custEmail")
                strCustName = drCustEmail("custName")
            End If
        End If
        Cmd.Dispose()
        drCustEmail.Close()
        strConn.Close()
        Response.Write(strCustMailId + "|" + strCustName)
    End Sub

End Class
