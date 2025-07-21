Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class admin_compOff
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim Cmd As New SqlCommand
    Dim gf As New generalFunction
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim empId As String = Session("dynoEmpIdSession")
        gf.checkEmpLogin()
		If (Session("dynoEmpIdSession").ToString() <> "") Then

			If Not IsPostBack Then 'bind for first time page load only
				ddlBind()
				binddgrd()
				If Request.QueryString("CoID") <> "" Then
					loadData(Request.QueryString("CoID"))
                    'btnSubmit.Text = "Update"
				Else
                    'btnSubmit.Text = "Submit"
				End If
			End If

		Else
			Response.Redirect("empLogin.aspx")
		End If

	End Sub
	Sub binddgrd()
        Dim sql As String
        Dim empid As String
        If (empddl.SelectedValue = 0) Then

            sql = "SELECT  a.coID,a.empId ,b.empName ,a.coDate,a.coComment,a.entryDate,a.entryBy" & _
      " FROM empCompOff a, employeeMaster b WHERE a.empId = b.empid  order by a.coDate desc"
        Else
            sql = "SELECT  a.coID,a.empId ,b.empName ,a.coDate,a.coComment,a.entryDate,a.entryBy" & _
                  " FROM empCompOff a, employeeMaster b WHERE a.empId = b.empid and a.empid=isnull('" & empddl.SelectedValue & "',a.empid) order by a.coDate desc"

        End If
       
		strConn.Open()
		Cmd = New SqlCommand(sql, strConn)
		Dim dtr As SqlDataReader
        dtr = Cmd.ExecuteReader()
        If (dtr.HasRows) Then
            lblInfoMsg.Visible = False
            dgrdCompOff.Visible = True
            dgrdCompOff.DataSource = dtr
            dgrdCompOff.DataBind()
        Else
            lblInfoMsg.Visible = True
            dgrdCompOff.Visible = False
        End If
		
		dtr.Close()
		strConn.Close()
	End Sub
    Sub loadData(ByVal CoID As String)
        'Dim strSQL As String
        'strSQL = "SELECT  * from empCompOff WHERE coID = '" & CoID & "'"
        'strConn.Open()
        'Cmd = New SqlCommand(strSQL, strConn)
        'Dim dtr As SqlDataReader
        'dtr = Cmd.ExecuteReader()
        'If dtr.Read Then
        '    txtDate.Text = dtr("coDate")
        '    txtComment.Text = dtr("coComment")
        '    empddl.SelectedValue = dtr("empId")
        'End If
        'dtr.Close()
        'strConn.Close()

    End Sub
	Sub ddlBind()
        Dim strSQL As String
        strSQL = "SELECT empid ,empName + ' ('+ cast(empid as varchar) +') ' as empName from employeeMaster " & _
        "WHERE empLeavingDate is null ORDER BY empName"
        strConn.Open()
        Cmd = New SqlCommand(strSQL, strConn)
        Dim dtr As SqlDataReader
        dtr = Cmd.ExecuteReader()
        empddl.DataSource = dtr
        empddl.Items.Add(New ListItem("- - Select - -", "0"))
        empddl.DataTextField = "empName"
        empddl.DataValueField = "empId"
        empddl.DataBind()
        dtr.Close()
        strConn.Close()
	End Sub

    'Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
    '       'Dim strSQL As String
    '       '      Dim empId As String = Session("dynoEmpIdSession")
    '       'Dim strdate As String
    '       'strdate = Date.Now
    '       'Dim value As String = empddl.SelectedValue.ToString
    '       'Dim codate As String
    '       'codate = txtDate.Text
    '       'Try
    '       '	If Request.QueryString("CoID") <> "" And btnSubmit.Text = "Update" Then
    '       '		strSQL = "update  empCompOff set empId='" & value & "', coDate = '" & codate & "',coComment = '" & txtComment.Text & "' where CoID = '" & Request.QueryString("CoID") & "'"

    '       '	Else

    '       '		strSQL = " insert into empCompOff (empId,coDate,entryBy,coComment)values ('" & value & "','" & codate & "'," & empId & ",'" & txtComment.Text & "')"
    '       '	End If
    '       '	strConn.Open()
    '       '	Cmd = New SqlCommand(strSQL, strConn)
    '       '	Cmd.ExecuteNonQuery()
    '       '	strConn.Close()
    '       '	binddgrd()
    '       '	txtDate.Text = ""
    '       '	txtComment.Text = ""
    '       '	empddl.SelectedIndex = "-1"
    '       '	btnSubmit.Text = "Submit"
    '       'Catch ex As Exception
    '       '	Dim sp As String
    '       '	sp = "<script language='JavaScript'>"
    '       '	sp += "alert('Can not enter duplicate record!!');"
    '       '	sp += "</script>"
    '       '	ClientScript.RegisterStartupScript(Me.GetType, "1", sp)
    '       'End Try

    'End Sub

	Protected Sub dgrdCompOff_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgrdCompOff.DeleteCommand
        Dim index As String = e.Item.ItemIndex
        Dim coId As String = dgrdCompOff.DataKeys(e.Item.ItemIndex).ToString()
		Dim strSQL As String
		strSQL = "DELETE FROM empCompOff where coID=" & coId
		strConn.Open()
		Cmd = New SqlCommand(strSQL, strConn)
		Cmd.ExecuteNonQuery()
		Cmd.Dispose()
		strConn.Close()
		binddgrd()

	End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        Try
            binddgrd()
        Catch ex As Exception

        End Try

    End Sub
End Class
