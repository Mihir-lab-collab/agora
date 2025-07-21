Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Partial Class emp_CandResume
    Inherits Authentication
    Dim dsn As String
    Dim strConn As SqlConnection
    Dim gf As New generalFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '  Dim clsCon As clsData = New clsData
        gf.checkEmpLogin()
        Try
            Dim candid As Integer
            candid = Request.QueryString("id")
            dsn = ConfigurationManager.ConnectionStrings("conString").ToString()
            strConn = New System.Data.SqlClient.SqlConnection(dsn)

            Dim cmdData As SqlCommand = New SqlCommand
            Dim strFilldata As String = "select resumetype,resumes from candidateMaster where candId=" & candid
            strConn.Open()
            cmdData = New SqlCommand(strFilldata, strConn)
            Dim dtrDeviceDetails As SqlDataReader = cmdData.ExecuteReader

            While dtrDeviceDetails.Read
                Response.ContentType = dtrDeviceDetails("resumetype").ToString
                Response.BinaryWrite(CType(dtrDeviceDetails("resumes"), Byte()))

            End While
            strConn.Close()

        Catch ex As Exception

        End Try
       
        '  Dim sp As String
        ' sp = "<script language='JavaScript'>"
        'sp += "alert('Record has been add/updated successfully.');"
        'sp += "</" + "script>"
        '  Response.Write(sp)
        '  Response.Write("<script language='javascript'>window.close();</" & "script>")
    End Sub
End Class
