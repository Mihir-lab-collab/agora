Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class admin_skillMaster
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
	Dim strConDesc As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim Cmd As New SqlCommand
    Dim intCount As Integer
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()

        Dim empId As String = Session("dynoEmpIdSession")

        If Not IsPostBack Then 'bind for first time page load only
            BindGrid()
        End If
    End Sub
	Sub BindGrid()
		Dim strSQL As String
        strSQL = "SELECT * FROM EmpPenaltyDetail, PenaltyMaster, EmployeeMaster " & _
        "WHERE EmployeeMaster.empID=EmpPenaltyDetail.empID AND EmpPenaltyDetail.PID=PenaltyMaster.PID " & _
        " ORDER BY EmpPID"
		strConn.Open()
        Cmd = New SqlCommand(strSQL, strConn)
        Dim dr As SqlDataReader
        dr = Cmd.ExecuteReader()
        grdSkill.DataSource = dr
		grdSkill.DataBind()
		strConn.Close()
		txtskill.Text = ""
	End Sub
	Protected Sub addNew_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles addNew.ServerClick
		Dim strSql As String
		Dim Desc As String
		Dim sp As String
		Desc = txtskill.Text
		If findDuplicateDesc(Desc) Then
			sp = "<script language='JavaScript'>"
			sp += "alert('Skill Already Exist!!');"
			sp += "</script>"
			ClientScript.RegisterStartupScript(Me.GetType, "1", sp)
		Else
			strSql = "insert into skillMaster (skillDesc) values ('" & txtskill.Text & "') "
			strConn.Open()
            Cmd = New SqlCommand(strSql, strConn)
            Cmd.ExecuteNonQuery()
			strConn.Close()
			BindGrid()
		End If
	End Sub
	Protected Sub dgrdEdit(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdSkill.EditCommand
		grdSkill.EditItemIndex = e.Item.ItemIndex
		intCount = 0
		BindGrid()
	End Sub
	Protected Sub dgrdCancel(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdSkill.CancelCommand
		grdSkill.EditItemIndex = -1
		BindGrid()
	End Sub
	Protected Sub dgrdUpdate(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdSkill.UpdateCommand
		Dim Desc As String
		Dim Id, index As Integer
		Dim sp As String
		Dim sql As String
		index = e.Item.ItemIndex
		If intCount = 0 Then
			Desc = CType(e.Item.Cells(1).Controls(0), TextBox).Text
			Id = grdSkill.DataKeys(index)
			If Desc <> "" Then
				If findDuplicateDesc(Desc) Then
					intCount = 0
					sp = "<script language='JavaScript'>"
					sp += "alert('Skill Already Exist!!');"
					sp += "</script>"
					ClientScript.RegisterStartupScript(Me.GetType, "1", sp)
				Else
					intCount = 1
					sql = "update skillMaster set skillDesc='" & Desc & "'where skillId=" & Id
                    Cmd = New SqlCommand(sql, strConDesc)
					strConDesc.Open()
                    Cmd.ExecuteNonQuery()
					strConDesc.Close()
					grdSkill.EditItemIndex = -1
					BindGrid()
				End If
			End If
		End If
	End Sub
    Function findDuplicateDesc(ByVal strDesc As String) As String
        Dim sql As String
        Dim strResult As Boolean
        sql = "select * from skillMaster where skillDesc='" & strDesc & "'"
        strConDesc.Open()
        Dim skillCmd As SqlCommand = New SqlCommand(sql, strConDesc)
        Dim dtrSkill As SqlDataReader = skillCmd.ExecuteReader()
        If dtrSkill.Read Then
            strResult = True
        Else
            strResult = False
        End If
        dtrSkill.Close()
        strConDesc.Close()
        findDuplicateDesc = strResult
    End Function
End Class
