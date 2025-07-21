Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Partial Class Emp_HolidayWorking
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


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        empId = CInt(Session("dynoEmpIdSession"))

        If CStr(Session("dynoEmpIdSession")) = "" Then
            Response.Redirect("emplogin.aspx")
        End If
        If Not IsPostBack Then
            ViewState("Mode") = "INSERT"
            BindProjDDL()
            BindHolidayDateDDL()
            BindHolidayWorkList()
        End If
    End Sub
    Sub BindProjDDL()
        strConn.Close()
        ddlProj.Items.Clear()
        ddlProj.Items.Add(New ListItem("Select", -1))

        strSQL = "SELECT projName as project_Name,projId as project_Id from projectMaster " & _
         "WHERE custID NOT in (SELECT custID FROM customerMaster WHERE allowEmpTimesheetEntry=0) and projName <> '' and projName  is not null ORDER BY projName"

        objCmd = New SqlCommand(strSQL, strConn)
        strConn.Open()
        objDataReader = objCmd.ExecuteReader
        If (objDataReader.HasRows) Then
            While objDataReader.Read
                ddlProj.Items.Add(New ListItem(objDataReader("project_Name"), objDataReader("project_Id")))
            End While
        End If

        strConn.Close()
        objDataReader.Close()
        objCmd.Dispose()


    End Sub
    Sub BindHolidayDateDDL()
        strConn.Close()
        ddlHolidayDt.Items.Clear()
        ddlHolidayDt.Items.Add(New ListItem("Select", -1))
        objCmd = New SqlCommand()
        strConn.Open()
        objCmd.Connection = strConn
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.CommandText = "GET_HolidayList"
        DataAdapter = New SqlDataAdapter(objCmd)
        Dtddl = New DataTable()
        DataAdapter.Fill(Dtddl)

        Dim i As Integer
        For i = 0 To Dtddl.Rows.Count - 1
            ddlHolidayDt.Items.Add(New ListItem(Dtddl.Rows(i).Item("Holiday")))
        Next i
        strConn.Close()
        objCmd.Dispose()

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        ManageHolidayWork(ViewState("Mode").ToString())
        BindHolidayWorkList()

    End Sub
    Sub ManageHolidayWork(ByVal Mode As String)
        Try
            objCmd = New SqlCommand()
            strConn.Open()
            objCmd.Connection = strConn
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.CommandText = "ManageholidayWorking"
            objCmd.Parameters.Add(New SqlParameter("@empid", SqlDbType.Int)).Value = empId
            objCmd.Parameters.Add(New SqlParameter("@HolidayDate", SqlDbType.DateTime)).Value = ddlHolidayDt.SelectedValue
            objCmd.Parameters.Add(New SqlParameter("@ProjId", SqlDbType.Int, 5)).Value = ddlProj.SelectedValue
            objCmd.Parameters.Add(New SqlParameter("@Hours", SqlDbType.Int)).Value = ddlHours.SelectedValue
            objCmd.Parameters.Add(New SqlParameter("@UserReason", SqlDbType.VarChar, 1000)).Value = txtReason.Text
            objCmd.Parameters.Add(New SqlParameter("@Mode", SqlDbType.Char, 10)).Value = Mode
            objCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.Int)).Value = Convert.ToInt32(ViewState("PKID"))
            objCmd.ExecuteNonQuery()
            ClearData()
            ViewState("Mode") = "INSERT"
            strConn.Close()
            objCmd.Dispose()

        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('" + ex.Message + "')", True)
            ddlHours.SelectedIndex = -1
            strConn.Close()
            objCmd.Dispose()
        End Try

    End Sub





    Sub BindHolidayWorkList()
        strConn.Close()

        strSQL = "SELECT h.ID,h.Empid,h.HolidayDate,h.ProjId,h.ExpectedHours,h.UserReason,p.projName as ProjectName,h.UserEntryDate,h.Status,h.AdminReason as AdminComment , h.AdminCancelReason as AdminCanReason ," & _
                 " CASE WHEN Status  =0 And AdminCancelReason  is null Then 'Pending' " & _
                 " WHEN Status  =1 And AdminCancelReason  is null Then 'Approved' " & _
                 " when Status  = 2 And AdminCancelReason  is null then 'Rejected' " & _
                 " when  Status =3 And AdminCancelReason  is  null Then 'Cancelled' " & _
                 " when  Status  =3 And AdminCancelReason  is not null Then 'Admin Cancelled'  end as Statusflag  from holidayWorking h inner join dbo.projectMaster p on  h.ProjId=p.projId " & _
                 " where empid='" & empId & " ' ORDER BY UserEntryDate desc"
        strConn.Open()
        objCmd = New SqlCommand(strSQL, strConn)
        DataAdapter = New SqlDataAdapter(strSQL, strConn)
        gridDataset = New DataSet()
        DataAdapter.Fill(gridDataset, "temp")
        Dim dtTimeSheet As DataTable = gridDataset.Tables("temp")
        If gridDataset.Tables("temp").Rows.Count = 0 Then
            dgHolidayWorkdtl.ShowFooter = False

        Else
            dgHolidayWorkdtl.ShowFooter = False
            Dim dv As New DataView(dtTimeSheet)
            dv.Sort = strOrderBy

            dgHolidayWorkdtl.DataSource = dv
            dgHolidayWorkdtl.DataBind()
        End If



        strConn.Close()

        objCmd.Dispose()


    End Sub

    Sub ClearData()
        ddlHolidayDt.SelectedIndex = -1
        ddlHours.SelectedIndex = -1
        ddlProj.SelectedIndex = -1
        txtReason.Text = ""

    End Sub

    Protected Sub dgHolidayWorkdtl_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgHolidayWorkdtl.SortCommand
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

        BindHolidayWorkList()

    End Sub

    Protected Sub dgHolidayWorkdtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHolidayWorkdtl.ItemDataBound
        Dim chkStatus As Integer = CType(DataBinder.Eval(e.Item.DataItem, "Status"), Integer)
        Dim chkdate As Date = CType(DataBinder.Eval(e.Item.DataItem, "HolidayDate"), Date)
        If chkdate <> Nothing Then
            Dim lstItem As ListItem


            lstItem = ddlHolidayDt.Items.FindByText(String.Format("{0:dd-MMM-yy}", chkdate))

            If IsNothing(lstItem) Then
                btnup = CType(e.Item.Cells(6).FindControl("update"), Button)
                btnup.Enabled = False
                btncan = CType(e.Item.Cells(6).FindControl("Cancel"), Button)
                btncan.Enabled = False
            End If
        End If


        If chkStatus <> 0 Then

            '  e.Item.BackColor = System.Drawing.Color.AliceBlue

            btnup = CType(e.Item.Cells(6).FindControl("update"), Button)
            btnup.Enabled = False
            btncan = CType(e.Item.Cells(6).FindControl("Cancel"), Button)
            btncan.Enabled = False
        End If
        If chkStatus = 3 Then
            btnup = CType(e.Item.Cells(6).FindControl("update"), Button)
            btnup.Visible = False
            btncan = CType(e.Item.Cells(6).FindControl("Cancel"), Button)
            btncan.Visible = False
        End If


    End Sub

    Protected Sub dgHolidayWorkdtl_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgHolidayWorkdtl.ItemCommand

        If e.CommandName = "update" Then
            ViewState("Mode") = "UPDATE"

            'Fill the Textboxes with relevant data

            ID = e.Item.Cells(0).Text
            lId.Value = ID
            ViewState("PKID") = ID
            ddlHolidayDt.SelectedValue = e.Item.Cells(2).Text
            ddlProj.SelectedValue = e.Item.Cells(1).Text
            ddlHours.SelectedValue = e.Item.Cells(4).Text
            txtReason.Text = e.Item.Cells(5).Text

        ElseIf e.CommandName = "Cancel" Then
            'Update The status in table
            strConn.Close()
            strSQL = " UPDATE holidayWorking set Status =3 where Empid= '" & empId & "'  AND HolidayDate= '" & e.Item.Cells(2).Text & " ' AND Status=0 and ProjId= " & e.Item.Cells(1).Text & ""
            strConn.Open()
            objCmd = New SqlCommand(strSQL, strConn)
            objCmd.ExecuteNonQuery()
            objCmd.Dispose()
            strConn.Close()
            BindHolidayWorkList()
        End If
    End Sub

    Protected Sub dgHolidayWorkdtl_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgHolidayWorkdtl.PageIndexChanged
        ClearData()
        dgHolidayWorkdtl.CurrentPageIndex = e.NewPageIndex
        BindHolidayWorkList()

    End Sub
End Class
