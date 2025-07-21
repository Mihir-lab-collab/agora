Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class Admin_CreateCompOff
    Inherits Authentication

    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objDataReader As SqlDataReader
    Dim objCmd As SqlCommand
    Dim DataAdapter As SqlDataAdapter
    Dim gridDataset As DataSet
    Dim Dtddl As DataTable
    Dim strSQL As String
    Dim empId As Integer
    Dim Mode As String
    Dim strOrderBy As String
    Dim btnup, btncan As Button
    Dim gf As New generalFunction

    Dim logempid As Integer

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            CrateCompOff()
            Response.Write("<script language='javascript'> { self.close() }</script>")
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim empId As String = Session("dynoEmpIdSession")
        gf.checkEmpLogin()
        If (Session("dynoEmpIdSession").ToString() <> "") Then
            logempid = CInt(Session("dynoEmpIdSession"))
            ViewState("logempid") = logempid
            If (Request.QueryString("Id") <> "") Then
                Dim ID As Integer = Request.QueryString("Id")
                strSQL = "SELECT projectMaster.projName,projectMaster.projId, employeeMaster.empName, employeeMaster.empid, REPLACE(CONVERT(VARCHAR(11), holidayWorking.HolidayDate, 106), ' ', '-')as HolidayDate FROM  employeeMaster " & _
                                " INNER JOIN holidayWorking ON employeeMaster.empid = holidayWorking.Empid INNER JOIN " & _
                                " projectMaster ON holidayWorking.ProjId = projectMaster.projId where holidayWorking.ID= '" & ID & "'"
                strConn.Open()
                objCmd = New SqlCommand(strSQL, strConn)
                DataAdapter = New SqlDataAdapter(strSQL, strConn)
                gridDataset = New DataSet()
                DataAdapter.Fill(gridDataset, "temp")
                Dim dtCompInfo As DataTable = gridDataset.Tables("temp")
                If (dtCompInfo.Rows.Count > 0) Then
                    lblProject.Text = Convert.ToString(dtCompInfo.Rows(0)("projName"))
                    lblempname.Text = Convert.ToString(dtCompInfo.Rows(0)("empName"))
                    lblCompofday.Text = Convert.ToString(dtCompInfo.Rows(0)("HolidayDate"))
                    empId = (dtCompInfo.Rows(0)("empid"))
                    ViewState("empid") = empId
                    ViewState("ProjectID") = Convert.ToString(dtCompInfo.Rows(0)("projId"))
                End If
            End If

        Else
            Response.Redirect("empLogin.aspx")
        End If
    End Sub
    Sub CrateCompOff()
        Try


            objCmd = New SqlCommand()
            strConn.Close()
            strConn.Open()
            objCmd.Connection = strConn
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.CommandText = "CreateCompof"
            objCmd.Parameters.Add(New SqlParameter("@empid", SqlDbType.Int)).Value = Convert.ToInt32(ViewState("empid"))
            ' objCmd.Parameters.Add(New SqlParameter("@CompDate", SqlDbType.DateTime)).Value = Convert.ToDateTime(lblCompofday.Text)
            objCmd.Parameters.Add(New SqlParameter("@CompDate", SqlDbType.DateTime)).Value = DateTime.Parse(lblCompofday.Text.Trim())
            objCmd.Parameters.Add(New SqlParameter("@CompComment", SqlDbType.VarChar, 1000)).Value = txtComment.Text.Trim() + "...On " + lblProject.Text

            objCmd.Parameters.Add(New SqlParameter("@EntryBy", SqlDbType.Int)).Value = Convert.ToInt32(ViewState("logempid"))

            objCmd.ExecuteNonQuery()


            strConn.Close()
            objCmd.Dispose()
            ClearData()

            'Response.Redirect("empHolidayWorkDetails.aspx?Flag=A")
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('Comp Off Created Successfully.')", True)


        Catch ex As Exception

            '  ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('" + ex.Message + "')", True)

            strConn.Close()
            objCmd.Dispose()
        End Try

    End Sub

    Sub ClearData()
        txtComment.Text = ""
        lblCompofday.Text = ""
        lblempname.Text = ""
        lblProject.Text = ""

    End Sub
    
End Class
