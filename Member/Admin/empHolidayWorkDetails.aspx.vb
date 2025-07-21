Imports System.Data
Imports System.Data.SqlClient
Partial Class Admin_empHolidayWorkDetails
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objDataReader As SqlDataReader
    Dim objCmd As SqlCommand
    Dim empId As Integer
    Dim DataAdapter As SqlDataAdapter
    Dim gridDataset As DataSet
    Dim strSQL As String
    Dim strOrderBy As String
    Dim txtcomment As TextBox
    Dim gf As New generalFunction
    Dim objCommon As New clsCommon()



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            gf.checkEmpLogin()
            empId = CInt(Session("dynoEmpIdSession"))

            If CStr(Session("dynoEmpIdSession")) = "" Then
                Response.Redirect("emplogin.aspx")
            End If
            If Not IsPostBack Then

                Dim userDetail As New UserDetails
                userDetail = Session("DynoEmpSessionObject")

                If userDetail.ProfileID <> "" Then
                    hdLocationId.Value = objCommon.GetLocationAcess(userDetail.ProfileID).ToString()
                    'hdHasAllLocationAcess.Value = (hdLocationId.Value.Equals("0") ? "1" : "0")
                End If
                BindLocation()
                BindDropDownSearchEmp()
                Binddata()

            End If
        Catch ex As Exception
        End Try
    End Sub
    Sub Binddata()
        If (hdnStatus.Value <> "" And Request.QueryString("Empid") <> "") Then
            ddlEmp.SelectedValue = Request.QueryString("Empid")
            If (ddlEmp.SelectedValue = -1) Then
                Admingrd.Visible = False
            Else
                Admingrd.Visible = True
                Usergrd.Visible = False
                HolidayDetailsWorkList(Trim(hdnStatus.Value))
            End If



        ElseIf (hdnStatus.Value = "") Then
            BindHolidayWorkList()
            lnkPending.ForeColor = Drawing.Color.Black
            Admingrd.Visible = False
            Usergrd.Visible = True
        Else
            Admingrd.Visible = True
            Usergrd.Visible = False
            HolidayDetailsWorkList(Trim(hdnStatus.Value))
        End If

    End Sub

    Sub HolidayDetailsWorkList(ByVal flag As String)
        Dim status As Integer
        If flag = "P" Then
            status = 0
        ElseIf flag = "A" Then
            status = 1
        ElseIf flag = "R" Then
            status = 2
        Else
            status = 3
        End If
        'strConn.Close()
        'If (status <> 3) Then
        '    If (ddlEmp.SelectedValue = -1 And (txtFromDate.Text = "" Or txtToDate.Text = "")) Then
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '                      " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName,holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason  " & _
        '                      " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                         " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & " " ' & _
        '        '"  order by holidayWorking.Id desc   "

        '    ElseIf (txtFromDate.Text = "" And txtToDate.Text = "") Then
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '            " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName ,holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason " & _
        '            " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '               " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & " and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "'" '& _
        '        '"  order by holidayWorking.Id desc   "
        '    ElseIf (txtFromDate.Text = "" And txtToDate.Text <> "") Then

        '        Dim todt As DateTime = DateTime.Parse(txtToDate.Text)
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '                 " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName ,holidayWorking.AdminReason as AdminComment,holidayWorking.AdminCancelReason as AdminCanReason  " & _
        '                 " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                    " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & "  and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "'" & _
        '                    " and convert(date,holidayWorking.HolidayDate) <= ' " & todt & " ' "
        '        'order by holidayWorking.Id desc   "
        '    ElseIf (txtToDate.Text = "" And txtFromDate.Text <> "") Then

        '        Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
        '        Dim todt As DateTime = Date.Now()
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '                " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName ,holidayWorking.AdminReason as AdminComment,holidayWorking.AdminCancelReason as AdminCanReason  " & _
        '                " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                   " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & "  and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "'" & _
        '                   " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & "' "
        '        ' order by holidayWorking.Id desc   "
        '    Else
        '        Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
        '        Dim todt As DateTime = DateTime.Parse(txtToDate.Text)
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '                " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName, holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason  " & _
        '                " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                   " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & "  and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "'" & _
        '                   " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & " ' "
        '        'order by holidayWorking.Id desc   "
        '    End If
        'Else

        '    If (ddlEmp.SelectedValue = -1 And (txtFromDate.Text = "" Or txtToDate.Text = "")) Then
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '                      " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName,holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason  " & _
        '                      " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                         " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & " and holidayWorking.AdminCancelReason is not null " ' & _
        '        '"  order by holidayWorking.Id desc   "

        '    ElseIf (txtFromDate.Text = "" And txtToDate.Text = "") Then
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '            " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName ,holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason  " & _
        '            " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '               " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & " and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "' and holidayWorking.AdminCancelReason is not null " '& _
        '        ' "  order by holidayWorking.Id desc   "
        '    ElseIf (txtFromDate.Text = "" And txtToDate.Text <> "") Then

        '        Dim todt As DateTime = DateTime.Parse(txtToDate.Text)
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '                 " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName ,holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason " & _
        '                 " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                    " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & "  and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "' and holidayWorking.AdminCancelReason is not null " & _
        '                    " and convert(date,holidayWorking.HolidayDate) <= ' " & todt & " ' "
        '        'order by holidayWorking.Id desc   "
        '    ElseIf (txtToDate.Text = "" And txtFromDate.Text <> "") Then

        '        Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
        '        Dim todt As DateTime = Date.Now()
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '                " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName ,holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason " & _
        '                " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                   " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & "  and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "' and holidayWorking.AdminCancelReason is not null " & _
        '                   " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & " ' "
        '        'order by holidayWorking.Id desc   "
        '    Else
        '        Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
        '        Dim todt As DateTime = DateTime.Parse(txtToDate.Text)
        '        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '                " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName, holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason " & _
        '                " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                   " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & "  and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "' and holidayWorking.AdminCancelReason is not null " & _
        '                   " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & " ' "
        '        'order by holidayWorking.Id desc   "
        '    End If
        'End If
        If (status <> 3) Then
            strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
                                 " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName,holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason  " & _
                                 " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
                                    " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & " "

        Else
            strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
                            " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName,holidayWorking.AdminReason as AdminComment ,holidayWorking.AdminCancelReason as AdminCanReason  " & _
                                " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
                                   " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = " & status & " and holidayWorking.AdminCancelReason is not null "

        End If
        Dim todt As DateTime = Date.Now()
        If (ddlEmp.SelectedValue <> -1) Then
            strSQL = strSQL & " and employeeMaster.Empid = '" & ddlEmp.SelectedValue & "' "
        End If

        If (txtFromDate.Text <> "" And txtToDate.Text <> "") Then
            todt = DateTime.Parse(txtToDate.Text)
            Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
            strSQL = strSQL & " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & " '"

        ElseIf (txtFromDate.Text <> "" And txtToDate.Text = "") Then
            Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
            strSQL = strSQL & " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & " '"

        ElseIf (txtFromDate.Text = "" And txtToDate.Text <> "") Then
            todt = DateTime.Parse(txtToDate.Text)
            strSQL = strSQL & " and convert(date,holidayWorking.HolidayDate) <= ' " & todt & " ' "
        End If

        If (hdLocationId.Value <> "0") Then
            strSQL = strSQL & " and employeeMaster.LocationFKID = '" & hdLocationId.Value & "' "
        End If
        strSQL = strSQL & " order by holidayWorking.Id desc"

        strConn.Open()
        objCmd = New SqlCommand(strSQL, strConn)
        DataAdapter = New SqlDataAdapter(strSQL, strConn)
        gridDataset = New DataSet()
        DataAdapter.Fill(gridDataset, "temp")

        Dim dtTimeSheet As DataTable = gridDataset.Tables("temp")
        If gridDataset.Tables("temp").Rows.Count = 0 Then
            dgAdminGrd.ShowFooter = False

        Else
            dgAdminGrd.ShowFooter = False

        End If

        Dim dv As New DataView(dtTimeSheet)
        dv.Sort = strOrderBy
        If dgAdminGrd.Items.Count <= 1 Then
            If dgAdminGrd.CurrentPageIndex <= dgAdminGrd.PageCount And dgAdminGrd.CurrentPageIndex > 0 Then
                dgAdminGrd.CurrentPageIndex = dgAdminGrd.CurrentPageIndex - 1
            End If
        End If
        dgAdminGrd.DataSource = dv
        dgAdminGrd.DataBind()
        strConn.Close()

        objCmd.Dispose()


    End Sub
    Sub BindDropDownSearchEmp()

        ddlEmp.Items.Clear()
        ddlEmp.Items.Add(New ListItem("Select", -1))

        strSQL = "select distinct em.empname,em.empid from employeeMaster em " & _
                 "where em.empLeavingDate is null "

        If (hdLocationId.Value <> "0") Then
            strSQL = strSQL & " and em.LocationFKID = '" & hdLocationId.Value & "' "
        End If

        strSQL = strSQL & " order by em.empname"

        objCmd = New SqlCommand(strSQL, strConn)
        strConn.Open()
        objDataReader = objCmd.ExecuteReader
        If (objDataReader.HasRows) Then
            While objDataReader.Read
                ddlEmp.Items.Add(New ListItem(objDataReader("empname"), objDataReader("empid")))
            End While
        End If

        strConn.Close()
        objDataReader.Close()
        objCmd.Dispose()


    End Sub

    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        If (txtFromDate.Text <> "" And txtToDate.Text <> "") Then
            Dim todt As DateTime = DateTime.Parse(txtToDate.Text)
            Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
            Dim compdt As Integer = fromdt.Date.CompareTo(todt)
            If compdt > 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert(' To Date should be greater  than From Date. ')", True)
            End If
        End If
        If (txtToDate.Text = "" And txtFromDate.Text <> "") Then
            Dim todt As DateTime = Date.Now()
            Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
            Dim compdt As Integer = fromdt.Date.CompareTo(todt)
            If compdt > 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert(' To Date should be greater than From Date. ')", True)
            End If
        End If
        dgAdminGrd.CurrentPageIndex = 0
        dgHolidayWorkdtl.CurrentPageIndex = 0
        If (hdnStatus.Value = "") Then
            BindHolidayWorkList()
            Admingrd.Visible = False
            Usergrd.Visible = True
        Else
            Admingrd.Visible = True
            Usergrd.Visible = False
            HolidayDetailsWorkList(Trim(hdnStatus.Value))
        End If



    End Sub
    Sub BindHolidayWorkList()

        'If (ddlEmp.SelectedValue = -1 And (txtFromDate.Text = "" Or txtToDate.Text = "")) Then
        '    strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '                  " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName " & _
        '                  " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                     " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = 0  " '& _
        '    ' "  order by holidayWorking.Id desc   "

        'ElseIf (txtFromDate.Text = "" And txtToDate.Text = "") Then
        '    strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '        " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName " & _
        '        " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '           " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = 0 and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "'"
        '    '& _  "  order by holidayWorking.Id desc   "
        'ElseIf (txtFromDate.Text = "" And txtToDate.Text <> "") Then

        '    Dim todt As DateTime = DateTime.Parse(txtToDate.Text)
        '    strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '             " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName " & _
        '             " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '                " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = 0 and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "'" & _
        '                " and convert(date,holidayWorking.HolidayDate) <= ' " & todt & " ' "
        '    'order by holidayWorking.Id desc   "
        'ElseIf (txtToDate.Text = "" And txtFromDate.Text <> "") Then

        '    Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
        '    Dim todt As DateTime = Date.Now()
        '    strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '            " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName " & _
        '            " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '               " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = 0 and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "'" & _
        '               " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & " '"
        '    'order by holidayWorking.Id desc   "
        'Else
        '    Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
        '    Dim todt As DateTime = DateTime.Parse(txtToDate.Text)
        '    strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
        '            " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName " & _
        '            " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
        '               " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = 0 and holidayWorking.Empid= '" & ddlEmp.SelectedValue & "'" & _
        '               " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & " ' "
        '    'order by holidayWorking.Id desc   "
        'End If

        strSQL = "SELECT  employeeMaster.empName, holidayWorking.ID, holidayWorking.HolidayDate, " & _
                         " holidayWorking.Empid, holidayWorking.ProjId, holidayWorking.ExpectedHours, holidayWorking.UserReason, projectMaster.projName " & _
                         " FROM holidayWorking INNER JOIN projectMaster ON holidayWorking.ProjId = projectMaster.projId INNER JOIN " & _
                            " employeeMaster ON holidayWorking.Empid = employeeMaster.empid where holidayWorking.Status = 0  "

       
        Dim todt As DateTime = Date.Now()
        If (ddlEmp.SelectedValue <> -1) Then
            strSQL = strSQL & " and employeeMaster.Empid = '" & ddlEmp.SelectedValue & "' "
        End If
       
        If (txtFromDate.Text <> "" And txtToDate.Text <> "") Then
            todt = DateTime.Parse(txtToDate.Text)
            Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
            strSQL = strSQL & " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & " '"

        ElseIf (txtFromDate.Text <> "" And txtToDate.Text = "") Then
            Dim fromdt As DateTime = DateTime.Parse(txtFromDate.Text)
            strSQL = strSQL & " and convert(date,holidayWorking.HolidayDate) between ' " & fromdt & " '  and ' " & todt & " '"

        ElseIf (txtFromDate.Text = "" And txtToDate.Text <> "") Then
            todt = DateTime.Parse(txtToDate.Text)
            strSQL = strSQL & " and convert(date,holidayWorking.HolidayDate) <= ' " & todt & " ' "
        End If

        If (hdLocationId.Value <> "0") Then
            strSQL = strSQL & " and employeeMaster.LocationFKID = '" & hdLocationId.Value & "' "
        End If
        strSQL = strSQL & " order by holidayWorking.Id desc"

        strConn.Open()
        objCmd = New SqlCommand(strSQL, strConn)
        DataAdapter = New SqlDataAdapter(strSQL, strConn)
        gridDataset = New DataSet()
        DataAdapter.Fill(gridDataset, "temp")
        'Response.Write(gridDataset.Tables(0).Rows.Count)
        Dim dtTimeSheet As DataTable = gridDataset.Tables("temp")
        If gridDataset.Tables("temp").Rows.Count = 0 Then
            dgHolidayWorkdtl.ShowFooter = False

        Else
            dgHolidayWorkdtl.ShowFooter = False

        End If

        Dim dv As New DataView(dtTimeSheet)
        dv.Sort = strOrderBy
        If dgHolidayWorkdtl.Items.Count <= 1 Then
            If dgHolidayWorkdtl.CurrentPageIndex <= dgHolidayWorkdtl.PageCount And dgHolidayWorkdtl.CurrentPageIndex > 0 Then
                dgHolidayWorkdtl.CurrentPageIndex = dgHolidayWorkdtl.CurrentPageIndex - 1
            End If
        End If
        dgHolidayWorkdtl.DataSource = dv
        dgHolidayWorkdtl.DataBind()
        strConn.Close()

        objCmd.Dispose()


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

    Protected Sub dgHolidayWorkdtl_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgHolidayWorkdtl.PageIndexChanged

        dgHolidayWorkdtl.CurrentPageIndex = e.NewPageIndex
        BindHolidayWorkList()



    End Sub

    Protected Sub dgHolidayWorkdtl_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgHolidayWorkdtl.ItemCommand
        If e.CommandName = "Approve" Then
            strConn.Close()
            txtcomment = CType(e.Item.Cells(8).FindControl("txtComment"), TextBox)

            strSQL = " UPDATE holidayWorking SET  Status=1,AdminEmpid= '" & empId & "',AdminReason= '" & sqlSafe(txtcomment.Text) & "',AdminEntryDate=GETDATE() WHERE Status = 0 And ID = " & e.Item.Cells(0).Text & ""
            strConn.Open()
            objCmd = New SqlCommand(strSQL, strConn)
            objCmd.ExecuteNonQuery()
            objCmd.Dispose()
            strConn.Close()

        End If
        If e.CommandName = "Reject" Then
            strConn.Close()
            txtcomment = CType(e.Item.Cells(8).FindControl("txtComment"), TextBox)

            strSQL = " UPDATE holidayWorking SET  Status=2,AdminEmpid= '" & empId & "',AdminReason= '" & sqlSafe(txtcomment.Text) & "',AdminEntryDate=GETDATE() WHERE Status = 0 And ID = " & e.Item.Cells(0).Text & ""
            strConn.Open()
            objCmd = New SqlCommand(strSQL, strConn)
            objCmd.ExecuteNonQuery()
            objCmd.Dispose()
            strConn.Close()

        End If
        BindHolidayWorkList()
    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ClearData()
        dgAdminGrd.CurrentPageIndex = 0
        dgHolidayWorkdtl.CurrentPageIndex = 0
        If (hdnStatus.Value = "") Then
            BindHolidayWorkList()
            Admingrd.Visible = False
            Usergrd.Visible = True
        Else
            Admingrd.Visible = True
            Usergrd.Visible = False
            HolidayDetailsWorkList(Trim(hdnStatus.Value))
        End If

    End Sub
    Sub ClearData()
        ddlEmp.SelectedIndex = -1
        txtFromDate.Text = ""
        txtToDate.Text = ""
        If (lblLocationId.Visible = False) Then
            dlLocation.SelectedValue = "10"
            hdLocationId.Value = dlLocation.SelectedValue
        End If
       
    End Sub
    Function sqlSafe(ByVal str)
        If str & "" <> "" Then
            str = Replace(str, "'", "''")
        End If
        sqlSafe = str
    End Function

    Protected Sub dgAdminGrd_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAdminGrd.ItemDataBound
        If (Trim(hdnStatus.Value) = "R") Then
            e.Item.Cells(9).Visible = False
            e.Item.Cells(10).Visible = False
            e.Item.Cells(11).Visible = False
        End If
        If (Trim(hdnStatus.Value) = "A") Then
            e.Item.Cells(9).Visible = False
        End If
        If (Trim(hdnStatus.Value) = "C") Then
            e.Item.Cells(10).Visible = False
            e.Item.Cells(11).Visible = False
        End If

    End Sub

    Protected Sub dgAdminGrd_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAdminGrd.ItemCommand
        If e.CommandName = "Cancel" Then
            Dim codate = Convert.ToDateTime(e.Item.Cells(5).Text)
            txtcomment = CType(e.Item.Cells(8).FindControl("txtCanComment"), TextBox)
            strConn.Close()
            strSQL = " UPDATE holidayWorking set Status =3,AdminCancelReason='" & sqlSafe(txtcomment.Text) & "' where Empid= '" & e.Item.Cells(1).Text & "'  AND Id= '" & e.Item.Cells(0).Text & " ' AND Status=1 ; if exists " & _
                    " (select codate from empcompoff where empid='" & e.Item.Cells(1).Text & "' AND convert(date,coDate,103)= '" & codate & "')" & _
                     "   begin DELETE FROM empCompOff where  empid='" & e.Item.Cells(1).Text & "' AND convert(date,coDate,103)= '" & codate & "' end"
            strConn.Open()
            objCmd = New SqlCommand(strSQL, strConn)
            objCmd.ExecuteNonQuery()
            objCmd.Dispose()
            strConn.Close()
        End If
        If e.CommandName = "compoff" Then
            Dim holidaydt = Convert.ToDateTime(e.Item.Cells(5).Text)

            strSQL = " select coDate from empCompOff where empid='" & e.Item.Cells(1).Text & "' and convert(date,coDate,103)= '" & holidaydt & "'"
            strConn.Open()
            objCmd = New SqlCommand(strSQL, strConn)
            DataAdapter = New SqlDataAdapter(strSQL, strConn)
            gridDataset = New DataSet()
            DataAdapter.Fill(gridDataset, "temp")
            Dim dtCompInfo As DataTable = gridDataset.Tables("temp")
            If (dtCompInfo.Rows.Count > 0) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "script", "alert('Compoff already exist')", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "script", "Confirm(" + e.Item.Cells(0).Text + ")", True)
            End If
            objCmd.Dispose()
            strConn.Close()
        End If
        HolidayDetailsWorkList(Trim(hdnStatus.Value))
    End Sub

    Protected Sub dgAdminGrd_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgAdminGrd.SortCommand
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

        HolidayDetailsWorkList(Trim(hdnStatus.Value))
    End Sub

    Protected Sub dgAdminGrd_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgAdminGrd.PageIndexChanged
        dgAdminGrd.CurrentPageIndex = e.NewPageIndex
        HolidayDetailsWorkList(Trim(hdnStatus.Value))

    End Sub
    Sub BindLocation()

        Dim dtEmployeeLocation As DataTable = New DataTable()
        dtEmployeeLocation = objCommon.EmployeeLocationList()
        If (hdLocationId.Value.Equals("0")) Then

            dlLocation.DataSource = dtEmployeeLocation
            dlLocation.DataTextField = "Name"
            dlLocation.DataValueField = "LocationID"
            dlLocation.DataBind()
            dlLocation.SelectedValue = dtEmployeeLocation.Select("Name='Mumbai'").FirstOrDefault()("LocationID").ToString()
            hdLocationId.Value = dlLocation.SelectedValue
            dlLocation.Visible = True
            lblLocation.Visible = True
        Else
            lblLocationId.Visible = True
            lblLocation.Visible = True
            dlLocation.Visible = False
            Dim location As String = objCommon.GetLocationName(Convert.ToInt32(hdLocationId.Value))
            lblLocationId.Text = location
        End If

    End Sub



    Protected Sub lnkPending_Click(sender As Object, e As EventArgs)
        hdnStatus.Value = ""
        dgHolidayWorkdtl.CurrentPageIndex = 0
        lnkCancel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkPending.ForeColor = Drawing.Color.Black
        lnkApproved.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkRejected.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        Binddata()
    End Sub

    Protected Sub lnkApproved_Click(sender As Object, e As EventArgs)
        hdnStatus.Value = "A"
        dgAdminGrd.CurrentPageIndex = 0
        lnkCancel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkPending.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkApproved.ForeColor = Drawing.Color.Black
        lnkRejected.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        Binddata()
    End Sub

    Protected Sub lnkRejected_Click(sender As Object, e As EventArgs)
        hdnStatus.Value = "R"
        dgAdminGrd.CurrentPageIndex = 0
        lnkCancel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkPending.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkApproved.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkRejected.ForeColor = Drawing.Color.Black
        Binddata()
    End Sub
    Protected Sub lnkCancel_Click(sender As Object, e As EventArgs)
        hdnStatus.Value = "C"
        dgAdminGrd.CurrentPageIndex = 0
        lnkCancel.ForeColor = Drawing.Color.Black
        lnkPending.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkApproved.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        lnkRejected.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
        Binddata()
    End Sub
    Protected Sub dlLocation_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dlLocation.SelectedIndexChanged
        hdLocationId.Value = dlLocation.SelectedValue
        dgAdminGrd.CurrentPageIndex = 0
        dgHolidayWorkdtl.CurrentPageIndex = 0
        BindDropDownSearchEmp()
        Binddata()
    End Sub
End Class
