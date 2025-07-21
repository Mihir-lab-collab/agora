Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Partial Class Admin_empGroupMaster
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objDataReader As SqlDataReader
    Dim objCmd As SqlCommand
    Dim DataAdapter As SqlDataAdapter
    Dim DsEmp As DataSet
    Dim Dtddl As DataTable
    Dim strSQL As String
    Dim empId As Integer
    Dim Mode As String
    Dim strOrderBy As String
    Dim btnup, btncan As Button
    Dim gf As New generalFunction
    Dim arraylist1 As New ArrayList()
    Dim arraylist2 As New ArrayList()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'gf.checkEmpLogin()
        empId = CInt(Session("dynoEmpIdSession"))

        If CStr(Session("dynoEmpIdSession")) = "" Then
            Response.Redirect("emplogin.aspx")
        End If
        If Not IsPostBack Then
            ViewState("Mode") = "INSERT"
            ClearData()
            trhr.Visible = True
            BindEmpDropdown()
            BindOffGroupList()
        End If
    End Sub
    Sub BindEmpDropdown()

        lstAllEmp.Items.Clear()


        strSQL = "select distinct  em.empname,em.empid from employeeMaster em " & _
                 " where em.empid not in (select Empid from officeGroupEmployee) and " & _
                 " em.empLeavingDate is null order by em.empname"

        objCmd = New SqlCommand(strSQL, strConn)
        strConn.Open()
        objCmd = New SqlCommand(strSQL, strConn)
        DataAdapter = New SqlDataAdapter(strSQL, strConn)
        DsEmp = New DataSet()
        DataAdapter.Fill(DsEmp, "temp")
        Dim DtEmp As DataTable = DsEmp.Tables("temp")
        lstAllEmp.DataSource = DtEmp
        lstAllEmp.DataTextField = "empname"
        lstAllEmp.DataValueField = "empid"
        lstAllEmp.DataBind()
        'objDataReader = objCmd.ExecuteReader
        'If (objDataReader.HasRows) Then
        '    While objDataReader.Read
        '        lstAllEmp.Items.Add(New ListItem(objDataReader("empname"), objDataReader("empid")))
        '    End While
        'End If

        strConn.Close()
        '  objDataReader.Close()
        objCmd.Dispose()


    End Sub

    Protected Sub btnAllNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAllNext.Click
        If lstAllEmp.Items.Count <> 0 Then
            For i As Integer = 0 To lstAllEmp.Items.Count - 1
                lstSelEmp.Items.Add(lstAllEmp.Items(i))
            Next
        End If
        lstAllEmp.Items.Clear()
    End Sub

    Protected Sub btnAllBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAllBack.Click
        If lstSelEmp.Items.Count <> 0 Then
            For i As Integer = 0 To lstSelEmp.Items.Count - 1
                lstAllEmp.Items.Add(lstSelEmp.Items(i))
            Next
        End If
        lstSelEmp.Items.Clear()
    End Sub

    Protected Sub btnSingleNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSingleNext.Click




        If lstAllEmp.SelectedIndex >= 0 Then
            For i As Integer = 0 To lstAllEmp.Items.Count - 1
                If lstAllEmp.Items(i).Selected Then
                    If Not arraylist1.Contains(lstAllEmp.Items(i)) Then

                        arraylist1.Add(lstAllEmp.Items(i))
                    End If
                End If
            Next
            For i As Integer = 0 To arraylist1.Count - 1
                If Not lstSelEmp.Items.Contains(DirectCast(arraylist1(i), ListItem)) Then
                    lstSelEmp.Items.Add(DirectCast(arraylist1(i), ListItem))
                End If
                lstAllEmp.Items.Remove(DirectCast(arraylist1(i), ListItem))
            Next
            lstSelEmp.SelectedIndex = -1
        Else
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('Please select atleast one employee')", True)

        End If


    End Sub

    Protected Sub btnSingleBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSingleBack.Click
        If lstSelEmp.SelectedIndex >= 0 Then
            For i As Integer = 0 To lstSelEmp.Items.Count - 1
                If lstSelEmp.Items(i).Selected Then
                    If Not arraylist2.Contains(lstSelEmp.Items(i)) Then
                        arraylist2.Add(lstSelEmp.Items(i))
                    End If
                End If
            Next
            For i As Integer = 0 To arraylist2.Count - 1
                If Not lstAllEmp.Items.Contains(DirectCast(arraylist2(i), ListItem)) Then
                    lstAllEmp.Items.Add(DirectCast(arraylist2(i), ListItem))
                End If
                lstSelEmp.Items.Remove(DirectCast(arraylist2(i), ListItem))
            Next
            lstAllEmp.SelectedIndex = -1
        Else
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('Please select atleast one employee')", True)
        End If

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim tstart As TimeSpan
            Dim tsend As TimeSpan
            If (txtEndTime.Text <> "" And txtStartTime.Text <> "") Then
                Dim StartTime As String() = txtStartTime.Text.ToString().Split(":"c)
                tstart = New TimeSpan(Convert.ToInt32(StartTime(0).ToString()), Convert.ToInt32(StartTime(1).ToString()), 0)

                Dim EndTime As String() = txtEndTime.Text.ToString().Split(":"c)
                tsend = New TimeSpan(Convert.ToInt32(EndTime(0).ToString()), Convert.ToInt32(EndTime(1).ToString()), 0)

            End If

            If (lstSelEmp.Items.Count > 0) Then
                If (tstart > tsend) Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('Start time should be less than End Time.')", True)
                Else
                    If (txtEndTime.Text <> "00:00" Or txtStartTime.Text <> "00:00") Then
                        ManageOfficeGroup(ViewState("Mode").ToString())
                        BindEmpDropdown()
                        BindOffGroupList()
                    Else
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('Invalid Time.')", True)
                    End If
                End If

            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('Select at least one employee.')", True)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Sub ManageOfficeGroup(ByVal Mode As String)

        strConn.Open()

        Dim myTrn As SqlTransaction = strConn.BeginTransaction()
        objCmd = strConn.CreateCommand()
        objCmd.Connection = strConn
        objCmd.Transaction = myTrn

        Dim StartTime As String() = txtStartTime.Text.ToString().Split(":"c)
        Dim tstart As TimeSpan = New TimeSpan(Convert.ToInt32(StartTime(0).ToString()), Convert.ToInt32(StartTime(1).ToString()), 0)

        Dim EndTime As String() = txtEndTime.Text.ToString().Split(":"c)
        Dim tsend As TimeSpan = New TimeSpan(Convert.ToInt32(EndTime(0).ToString()), Convert.ToInt32(EndTime(1).ToString()), 0)


        Try

            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.CommandText = "ManageOfficeGroupMaster"
            objCmd.Parameters.Add(New SqlParameter("@GroupName", SqlDbType.NVarChar, 50)).Value = txtGrp.Text
            objCmd.Parameters.Add(New SqlParameter("@OffStartTime", SqlDbType.Time)).Value = tstart
            objCmd.Parameters.Add(New SqlParameter("@OffEndTime", SqlDbType.Time)).Value = tsend
            objCmd.Parameters.Add(New SqlParameter("@AdminEmpid", SqlDbType.Int)).Value = empId
            objCmd.Parameters.Add(New SqlParameter("@Mode", SqlDbType.Char, 10)).Value = Mode
            objCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.Int)).Value = Convert.ToInt32(ViewState("PKID"))
            objCmd.Parameters.Add(New SqlParameter("@PKID", SqlDbType.Int)).Direction = ParameterDirection.Output
            objCmd.ExecuteNonQuery()

            If (ViewState("Mode").ToString() = "INSERT") Then
                Dim Grpid As Integer = CInt(objCmd.Parameters("@PKID").Value)
                If lstSelEmp.Items.Count > 0 Then
                    For i As Integer = 0 To lstSelEmp.Items.Count - 1
                        objCmd.CommandType = CommandType.Text
                        objCmd.CommandText = " Insert INTO officeGroupEmployee (FkGroupid,Empid) VALUES(" & Grpid & "," & lstSelEmp.Items(i).Value & ")"

                        objCmd.ExecuteNonQuery()

                    Next
                End If
            Else
                If lstSelEmp.Items.Count > 0 Then
                    objCmd.CommandType = CommandType.Text
                    objCmd.CommandText = " DELETE FROM officeGroupEmployee where FkGroupid='" & Convert.ToInt32(ViewState("PKID")) & "'"
                    objCmd.ExecuteNonQuery()
                    For i As Integer = 0 To lstSelEmp.Items.Count - 1
                        objCmd.CommandText = " Insert INTO officeGroupEmployee (FkGroupid,Empid) VALUES(" & Convert.ToInt32(ViewState("PKID")) & "," & lstSelEmp.Items(i).Value & ")"
                        objCmd.ExecuteNonQuery()

                    Next
                End If
            End If


            myTrn.Commit()
            strConn.Close()
            objCmd.Dispose()
            ClearData()
            ViewState("Mode") = "INSERT"
        Catch ex As Exception
            myTrn.Rollback()

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('" + ex.Message + "')", True)

            strConn.Close()
            objCmd.Dispose()
        End Try

    End Sub
    Public Function ManageOfficeGroupEmployee(ByVal Groupid As Integer) As String
        If lstSelEmp.Items.Count > 0 Then
            For i As Integer = 0 To lstSelEmp.Items.Count - 1

                strSQL = " Insert INTO officeGroupEmployee (FkGroupid,Empid) VALUES(" & Groupid & "," & lstSelEmp.Items(i).Value & ")"
                ' strConn.Open()
                objCmd = New SqlCommand(strSQL, strConn)
                objCmd.ExecuteNonQuery()

            Next
        End If

        Return "TRUE"
    End Function
    Sub ClearData()
        txtGrp.Text = ""
        txtEndTime.Text = ""
        txtStartTime.Text = ""
        lbltotalhr.Text = ""
        lstSelEmp.Items.Clear()
    End Sub

    Sub BindOffGroupList()
        strConn.Close()

        strSQL = "SELECT PkID, GroupName,  Left(Convert(TIME(0), OfficeStartTime), 5) AS OfficeStartTime, Left(Convert(TIME(0), OfficeEndTime), 5) AS OfficeEndTime," & _
                    " AdminEntryDate FROM OfficeGroupMaster order by pkid desc"

        strConn.Open()
        objCmd = New SqlCommand(strSQL, strConn)
        DataAdapter = New SqlDataAdapter(strSQL, strConn)
        DsEmp = New DataSet()
        DataAdapter.Fill(DsEmp, "temp")
        Dim dtgropdtl As DataTable = DsEmp.Tables("temp")
        If DsEmp.Tables("temp").Rows.Count = 0 Then
            dgoffGroup.ShowFooter = False

        Else
            dgoffGroup.ShowFooter = False
            Dim dv As New DataView(dtgropdtl)
            dv.Sort = strOrderBy

            dgoffGroup.DataSource = dv
            dgoffGroup.DataBind()
        End If



        strConn.Close()

        objCmd.Dispose()


    End Sub

    Protected Sub dgoffGroup_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgoffGroup.ItemCommand
        If e.CommandName = "Update" Then
            ViewState("Mode") = "UPDATE"
            Dim grpid As Integer = e.Item.Cells(0).Text
            ViewState("PKID") = grpid
            txtGrp.Text = e.Item.Cells(1).Text
            txtStartTime.Text = e.Item.Cells(2).Text
            txtEndTime.Text = e.Item.Cells(3).Text
            Calculatehr()
            strSQL = "SELECT O.PkId, O.FkGroupid, O.Empid as empid, E.empName as empname" & _
                     " FROM  officeGroupEmployee O INNER JOIN" & _
                     " employeeMaster  E ON O.Empid = E.empid where O.FkGroupid= '" & grpid & "' order by E.empName"

            strConn.Open()
            objCmd = New SqlCommand(strSQL, strConn)
            DataAdapter = New SqlDataAdapter(strSQL, strConn)
            DsEmp = New DataSet()
            DataAdapter.Fill(DsEmp, "offEmp")
            Dim dtoffEmp As DataTable = DsEmp.Tables("offEmp")
            lstSelEmp.DataSource = dtoffEmp
            lstSelEmp.DataTextField = "empname"
            lstSelEmp.DataValueField = "empid"
            lstSelEmp.DataBind()
        End If

    End Sub

    Protected Sub dgoffGroup_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgoffGroup.PageIndexChanged
        ClearData()
        dgoffGroup.CurrentPageIndex = e.NewPageIndex
        BindOffGroupList()
    End Sub

    Protected Sub dgoffGroup_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgoffGroup.SortCommand
        If e.SortExpression.ToString() = Session("Column") Then
            'Reverse the sort order
            If Session("Order") = "ASC" Then
                strOrderBy = e.SortExpression.ToString() & " DESC"
                Session("Order") = "DESC"
            Else
                strOrderBy = e.SortExpression.ToString() & " ASC"
                Session("Order") = "ASC"
            End If
        Else
            strOrderBy = e.SortExpression.ToString() & " ASC"
            Session("Order") = "ASC"
        End If
        Session("Column") = e.SortExpression.ToString()

        BindOffGroupList()
    End Sub
    Sub Calculatehr()

        If (txtEndTime.Text <> "" And txtStartTime.Text <> "") Then
            Dim StartTime As String() = txtStartTime.Text.ToString().Split(":"c)
            Dim tstart As TimeSpan = New TimeSpan(Convert.ToInt32(StartTime(0).ToString()), Convert.ToInt32(StartTime(1).ToString()), 0)

            Dim EndTime As String() = txtEndTime.Text.ToString().Split(":"c)
            Dim tsend As TimeSpan = New TimeSpan(Convert.ToInt32(EndTime(0).ToString()), Convert.ToInt32(EndTime(1).ToString()), 0)
            Dim tsdiff As TimeSpan = tsend.Subtract(tstart)
            Dim dtt As DateTime = Convert.ToDateTime(tsdiff.ToString())
            lbltotalhr.Text = dtt.ToString("HH:mm")
        End If

    End Sub



End Class
