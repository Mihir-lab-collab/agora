Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Partial Class Admin_MothlyTimesheetRpt
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
    Dim arg As String
    Dim strOrderBy As String
    Dim gf As New generalFunction
    Dim objCommon As New clsCommon()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        Try
            empId = CInt(Session("dynoEmpIdSession"))
            If CStr(Session("dynoEmpIdSession")) = "" Then
                Response.Redirect("emplogin.aspx")
            End If
            If Not IsPostBack Then
                lblMonth.Text = Left(MonthName(Month(Now())), 3) & " " & Year(Now())
                hdnmonth.Value = Month(lblMonth.Text)
                hdnyear.Value = Year(lblMonth.Text)
                Dim userDetail As New UserDetails
                userDetail = Session("DynoEmpSessionObject")
                hdLocationId.Value = objCommon.GetLocationAcess(userDetail.ProfileID).ToString()
                BindLocation()
                BindMontlyReport()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs) 'Handles prevMonth.Click, nextMonth.Click
        arg = sender.CommandArgument
        Select Case arg
            Case "next"
                curdate.Value = lblMonth.Text
                lblMonth.Text = DateAdd("m", 1, curdate.Value)
                lblMonth.Text = Left(MonthName(Month(lblMonth.Text)), 3) & " " & Year(lblMonth.Text)
            Case "prev"
                curdate.Value = lblMonth.Text
                lblMonth.Text = DateAdd("m", -1, curdate.Value)
                lblMonth.Text = Left(MonthName(Month(lblMonth.Text)), 3) & " " & Year(lblMonth.Text)
        End Select
        hdnmonth.Value = Month(lblMonth.Text)
        hdnyear.Value = Year(lblMonth.Text)
        BindMontlyReport()
    End Sub
    Sub BindMontlyReport()
        Try
            objCmd = New SqlCommand()
            strConn.Open()
            objCmd.Connection = strConn
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.CommandText = "GET_MonthlyReport"
            objCmd.Parameters.Add(New SqlParameter("@month", SqlDbType.Int)).Value = Month(lblMonth.Text)
            objCmd.Parameters.Add(New SqlParameter("@year", SqlDbType.Int)).Value = Year(lblMonth.Text)
            objCmd.Parameters.AddWithValue("@LocationId", If(Not hdLocationId.Value.Equals("0"), hdLocationId.Value, DBNull.Value))
            DataAdapter = New SqlDataAdapter(objCmd)
            Dtddl = New DataTable()
            DataAdapter.Fill(Dtddl)
            Dim dv As New DataView(Dtddl)
            dv.Sort = strOrderBy
            If (Dtddl.Rows.Count > 0) Then
                dgMonthTimeRpt.DataSource = dv
                dgMonthTimeRpt.DataBind()
            End If
            strConn.Close()
            objCmd.Dispose()

        Catch ex As Exception
            strConn.Close()
            objCmd.Dispose()
        End Try

    End Sub

    Protected Sub dgMonthTimeRpt_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgMonthTimeRpt.SortCommand
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
        BindMontlyReport()
    End Sub

    Protected Sub dlLocation_SelectedIndexChanged(sender As Object, e As EventArgs)
        hdLocationId.Value = dlLocation.SelectedValue
        BindMontlyReport()
    End Sub

    Sub BindLocation()

        If (hdLocationId.Value.Equals("0")) Then
            Dim dtEmployeeLocation As DataTable = New DataTable()
            dtEmployeeLocation = objCommon.EmployeeLocationList()
            dlLocation.DataSource = dtEmployeeLocation
            dlLocation.DataTextField = "Name"
            dlLocation.DataValueField = "LocationID"
            dlLocation.DataBind()
            dlLocation.SelectedValue = dtEmployeeLocation.Select("Name='Mumbai'").FirstOrDefault()("LocationID").ToString()
            hdLocationId.Value = dlLocation.SelectedValue
            dlLocation.Visible = True
        Else
            lblLocationId.Visible = True
            dlLocation.Visible = False
            Dim location As String = objCommon.GetLocationName(Convert.ToInt32(hdLocationId.Value))
            lblLocationId.Text = location
        End If

    End Sub
End Class
