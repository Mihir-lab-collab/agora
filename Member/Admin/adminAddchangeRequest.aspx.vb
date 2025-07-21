Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class admin_adminAddchangeRequest
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        btnSave.Attributes.Add("onclick", "return chkblank();  ")
		Dim PrjID as integer
		PrjID=Trim(Request.QueryString("projId"))
		fillprojectname(PrjID)

    End Sub

    Sub fillprojectname(ByRef PrjID As Integer)

        Dim drprojname As SqlDataReader
        Dim cmd As SqlCommand

        cmd = New SqlCommand("select projectMaster.projname,Customermaster.custname,Customermaster.custcompany from Customermaster,projectMaster where Customermaster.custid=projectMaster.custid and projectMaster.projid= " & PrjID, strConn)
        strConn.Open()
        drprojname = cmd.ExecuteReader()
        While drprojname.Read()
            lblprojectname.Text = drprojname("projname")
            lblCompName.Text = drprojname("custcompany")
            lblCustname.Text = drprojname("custname")
        End While
        strConn.Close()

    End Sub

    Protected Sub btnSave_Click1(ByVal sender As Object, ByVal e As System.EventArgs) 
        
        Dim strSqlInsert As String
        Dim TodayDate As Date
        TodayDate = Date.Today
        Dim cost As Double
        cost = 0
        If (Not String.IsNullOrEmpty(estComCost.Text)) Then
            cost = Convert.ToDouble(estComCost.Text)
        End If

        strSqlInsert = "INSERT INTO changeRequest(chgTitle,chgDesc,chgApproved,chgProjId,chgDate,chgApprovedBy,chgEstCost,chgEstTime)values('" & txtchangetitle.Text & "','" & gf.sqlSafe(chgdescription.Text) & "','P'," & Trim(Request.QueryString("projId")) & ",'" & txtchange.Text & "'," & Session("dynoEmpIdSession") & "," & cost & "," & estComTime.Text & ")"
        Dim Sqlcmd As New SqlCommand(strSqlInsert, strConn)
        strConn.Open()
        Sqlcmd.ExecuteNonQuery()
        Sqlcmd.Dispose()
        strConn.Close()
        Dim sp As String
        sp = "<script>javascript:window.close();" & "</" & "script>"
        Response.Write(sp)
        Response.Write("<script language='javascript'>{ window.opener.window.location.href='/admin/show_ch_request_to_admin.aspx' ; } </" & "script>")
    End Sub
End Class
