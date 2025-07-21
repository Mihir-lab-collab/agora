Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections

Partial Class admin_empattdetails
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objgeneralFunction As New generalFunction
    Dim cmdEmp As SqlCommand
    Dim dtrEmp As SqlDataReader
    Dim cmd As SqlCommand
    Dim empId As String
    Dim strSQL As String
    Dim innersql As String
    Dim totalemp As Integer
    Dim totaldays As Integer
    Dim year As Integer = DateTime.Now.Year
    Dim Month As Integer = DateTime.Now.Month
    Dim i As Integer
    Dim j As Integer
    Dim strouttime As String
    Dim da As New SqlDataAdapter
    Dim attdate As String = ""
    Dim months As String
    Dim years As String
    Dim grdtotaldays As Integer
    Dim cnt As Integer = 0
    Dim gf As New generalFunction
    Dim dateList As New ArrayList
    Dim objCommon As New clsCommon()



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        If Not IsPostBack Then
            ' commented by satya cause given error
            'Dim userDetail As New UserDetails
            'userDetail = Session("DynoEmpSessionObject")
            'If userDetail.ProfileID <> "" Then
            '    hdLocationId.Value = objCommon.GetLocationAcess(userDetail.ProfileID).ToString()
            '    Call BindLocations()

            'End If
            Call BindYear()
            If ddlYear.SelectedIndex <> 0 Then
                Call BindMonth()
            End If
            hdLocationId.Value = 10
            BindLocations()
            bindgrid()
            findholidayofmonth()
        End If
        lblError.Visible = False

    End Sub
    Sub bindgrid()
        attdate = Request.QueryString("strDate")

        If (attdate = "") Then
            months = DateTime.Now.Month.ToString()
            years = DateTime.Now.Year.ToString()
            totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
        Else
            ' months = attdate.Substring(0, 1).ToString()
            'years = attdate.Substring(4, 4).ToString()
            Dim myArray(3)

            myArray = attdate.Split("/")
            months = myArray(0).ToString()
            years = myArray(2).ToString()
            totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
        End If

        cmd = New SqlCommand()
        cmd.CommandText = "fillattendance"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@days", totaldays)
        cmd.Parameters.AddWithValue("@months", months)
        cmd.Parameters.AddWithValue("@years", years)
        cmd.Parameters.AddWithValue("@Location", If(Not hdLocationId.Value.Equals("0"), hdLocationId.Value, DBNull.Value))
        cmd.Connection = strConn
        Dim ds As New DataSet
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        grdtotaldays = totaldays
        totaldays = totaldays + 1
        If (totaldays <= 31) Then
            For i = totaldays To 31
                'Response.Write("</br>aa" & i)
                ds.Tables(0).Columns.Add("aa" & i.ToString())
            Next
        End If
        'grdatt.Columns(3).HeaderText = "Test"
        '        ds.Tables(0).Columns(1).ColumnName = " Employee  Name  "
        grdatt.DataSource = ds.Tables(0).DefaultView
        grdatt.DataBind()

        ' Response.Write(grdtotaldays)
        grdtotaldays = grdtotaldays + 1
        If (grdtotaldays <= 31) Then
            For i = grdtotaldays To 31
                grdatt.Columns(i + 1).Visible = False
            Next
        End If
        grdatt.Columns(0).Visible = False

    End Sub


    Protected Sub grdatt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdatt.RowDataBound
        Dim gvr As GridViewRow = e.Row
        Dim j As Integer = 0
        If (gvr.RowType = DataControlRowType.DataRow) Then
            Dim l1 As Label = DirectCast(e.Row.Cells(2).FindControl("Label1"), Label)

            'Dim chk3 As CheckBox = DirectCast(e.Row.Cells(2).FindControl("chk3"), CheckBox)
            'Dim lbl3 As Label = DirectCast(e.Row.Cells(2).FindControl("Label3"), Label)
            'If (lbl3.Text.ToString() <> "A") Then
            '    chk3.Visible = False
            'Else
            '    chk3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "')")
            '    lbl3.Visible = False
            'End If
            For j = 1 To 31
                attdate = Request.QueryString("strDate")

                If (attdate = "") Then
                    months = DateTime.Now.Month.ToString()
                    years = DateTime.Now.Year.ToString()
                    totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
                Else
                    ' months = attdate.Substring(0, 1).ToString()
                    'years = attdate.Substring(4, 4).ToString()
                    Dim myArray(3)

                    myArray = attdate.Split("/")
                    months = myArray(0).ToString()
                    years = myArray(2).ToString()
                    totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
                End If

                Dim strdate As String = grdatt.Columns(j + 1).HeaderText.ToString() & "-" & months & "-" & CStr(years)
                Dim strUpdate As String = "strUpdate"
                Dim chk3 As CheckBox = DirectCast(e.Row.Cells(j + 1).FindControl("chk" & CStr(j + 2)), CheckBox)
                Dim lbl3 As Label = DirectCast(e.Row.Cells(j + 1).FindControl("Label" & CStr(j + 2)), Label)
                If (lbl3.Text.ToString() <> "A") Then

                    If (lbl3.Text.EndsWith("L") = True) Then
                        lbl3.BackColor = Color.FromArgb(134, 198, 124)
                    End If
                    chk3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "')")
                    chk3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "','" & strUpdate & "')")
                    chk3.Visible = False

                Else
                    chk3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "')")
                    lbl3.Visible = False
                End If
                'Added by pravin on 1 Jul 2014- starts here
                'If (lbl3.Text) <> "") Then
                '    chk3.Visible = False
                'Else
                '    chk3.Visible = True
                'End If
                'Added by pravin on 1 Jul 2014- ends here


                lbl3.Attributes.Add("style", "cursor:pointer;")
                lbl3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "','" & strUpdate & "')")
            Next
        End If

    End Sub


    Protected Sub grdatt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdatt.SelectedIndexChanged

    End Sub
    Sub findholidayofmonth()
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim t As Integer = 0
        Dim arrholiday As New ArrayList
        Dim bccolor As Color = Color.FromArgb(197, 213, 174)
        totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))


        For x = 1 To totaldays
            Dim d As New Date(CInt(years), CInt(months), x)
            If (d.ToString("dddd").ToUpper() = "SATURDAY") Then
                If (y <> 1) Then

                    x = x + 1
                    If x <= totaldays Then
                        arrholiday.Add(x.ToString())
                    End If

                    y = 1
                Else
                    t = x + 1
                    arrholiday.Add(t.ToString())
                    arrholiday.Add(x.ToString())
                    y = 0
                End If

            Else
                If (d.ToString("dddd").ToUpper() = "SUNDAY") Then
                    arrholiday.Add(x.ToString())
                End If
            End If

        Next

        strSQL = "select day(holidaydate) as holidate from holidaymaster where month(holidaydate)=" & months & " and year(holidaydate)=" & years

        dtrEmp = objgeneralFunction.funcSelect(strSQL)
        If dtrEmp.HasRows Then
            While (dtrEmp.Read())
                arrholiday.Add(CStr(dtrEmp(0)))
            End While
        End If
        objgeneralFunction.funcConClose()

        For Each t In arrholiday
            grdatt.Columns(CInt(t + 1)).ItemStyle.BackColor = bccolor
        Next


    End Sub

    Protected Sub dlLocation_SelectedIndexChanged(sender As Object, e As EventArgs)
        hdLocationId.Value = dlLocation.SelectedValue
        Call bindgrid()
    End Sub

    Sub BindLocations()

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

    'added by Pravin on 17 May 2014 : Start
    Public Sub ShowHideDialogue(sender As Object, e As EventArgs)
        'BindColumnsNames()
        BindCheckList()
        Dim button As Button = TryCast(sender, Button)
        Me.ShowHideDialogueInternal(button.CommandArgument.Equals("show"))
    End Sub

    Private Sub ShowHideDialogueInternal(state As Boolean)
        ddlYear.Items.Clear()
        ddlYear.Items.Add(New ListItem("Select", 0))
        ddlYear.SelectedIndex = 0
        Call BindYear()

        ddlMonth.Items.Clear()
        ddlMonth.Items.Add(New ListItem("Select", 0))
        ddlMonth.SelectedIndex = 0
        If ddlYear.SelectedIndex <> 0 Then
            Call BindMonth()
        End If

        Me.pnlColumns.Visible = state
        ' AddGridViewColumnsInRuntime()
    End Sub

    Public Function GetMonthYear(dtStart As DateTime, dtEnd As DateTime) As ArrayList

        Dim monthList As New ArrayList()
        Dim dt As DateTime = dtStart
        While dt.Month <= dtEnd.Month
            'monthList.Add(New ListItem(dt.ToString("MMMM yyyy"), dt.ToString("yyyy")))
            monthList.Add(New ListItem(dt.Month, dt.Year))
            dt = dt.AddMonths(1)
        End While

        Return monthList
    End Function
    Sub BindCheckList()
        Dim conn1 As SqlConnection = New SqlConnection(dsn)
        Dim dad As New SqlDataAdapter
        Dim ds1 As New DataSet
        Dim strListQuery As String
        attdate = Request.QueryString("strDate")

        If (attdate = "") Then
            months = DateTime.Now.Month.ToString()
            years = DateTime.Now.Year.ToString()
            totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
        Else
            ' months = attdate.Substring(0, 1).ToString()
            'years = attdate.Substring(4, 4).ToString()
            Dim myArray(3)

            myArray = attdate.Split("/")
            months = myArray(0).ToString()
            years = myArray(2).ToString()
            totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
        End If
        cmd = New SqlCommand()
        cmd.CommandText = "GetEmpAttendanecList"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@days", totaldays)
        cmd.Parameters.AddWithValue("@months", months)
        cmd.Parameters.AddWithValue("@years", years)
        cmd.Parameters.AddWithValue("@Location", If(Not hdLocationId.Value.Equals("0"), hdLocationId.Value, DBNull.Value))
        cmd.Connection = strConn
        Dim ds As New DataSet
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)

        'strListQuery = ("select distinct emp.empId as empId,emp.empName")

        'dad = New SqlDataAdapter(strListQuery, conn1)
        'Dim dt As DataTable = New DataTable("table1")
        'Dim dc As DataColumn = New DataColumn("srno", GetType(Int32))
        'dc.AutoIncrement = True
        'dc.AutoIncrementSeed = 1
        'dt.Columns.Add(dc)
        'ds1.Tables.Add(dt)
        'dad.Fill(ds1, "table1")

        Me.chkColumnsList.DataSource = ds
        Me.chkColumnsList.DataTextField = "empname"
        Me.chkColumnsList.DataValueField = "empid"
        Me.chkColumnsList.DataBind()

    End Sub

    'Export to Excel function
    Protected Sub ExportToExcel(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddColumns.Click
        attdate = Request.QueryString("strDate")
        If (attdate = "") Then
            'months = DateTime.Now.Month.ToString()
            'years = DateTime.Now.Year.ToString()
            'totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
            months = ddlMonth.SelectedValue.ToString()
            years = ddlYear.SelectedValue.ToString()
            totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
        Else
            Dim myArray(3)
            myArray = attdate.Split("/")
            months = myArray(0).ToString()
            years = myArray(2).ToString()
            totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
        End If


        'Get the data from database into datatable
        Dim conn1 As SqlConnection = New SqlConnection(dsn)
        Dim dad As New SqlDataAdapter
        Dim ds1 As New DataSet
        Dim ListItem As New StringBuilder
        Dim commaString As String = String.Empty
        Dim counter As Integer = 0

        For Each chk In chkColumnsList.Items
            If chk.selected Then
                commaString += chk.Value + ","
                counter += 1
            End If
        Next

        If counter > 0 Then
            commaString = commaString.Substring(0, commaString.Length - 1)
        End If

        cmd = New SqlCommand()
        cmd.CommandText = "fillattendance_Excel"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@days", totaldays)
        cmd.Parameters.AddWithValue("@months", months)
        cmd.Parameters.AddWithValue("@years", years)
        cmd.Parameters.AddWithValue("@Location", If(Not hdLocationId.Value.Equals("0"), hdLocationId.Value, DBNull.Value))
        cmd.Parameters.AddWithValue("@StrEmployeeList", IIf(commaString = "", DBNull.Value, commaString))
        cmd.Connection = strConn
        Dim ds As New DataSet
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)

        'set column names
        ds.Tables(0).Columns(0).ColumnName = "Employee ID"
        ds.Tables(0).Columns(1).ColumnName = "Employee Name"
        ''''''''''''

        If ds.Tables(0).Rows.Count > 0 Then

            GetExel(ds)
        Else
            lblError.Visible = True
            lblError.Text = "No Data found"
        End If


        'Create a dummy GridView
        'Dim GridView1 As New GridView()
        'GridView1.AllowPaging = False
        'GridView1.DataSource = ds.Tables(0)
        ''GridView1.DataSource = ds
        'GridView1.DataBind()

        'Response.Clear()
        'Response.Buffer = True
        'Response.AddHeader("content-disposition", _
        '     "attachment;filename=DataTable.xls")
        'Response.Charset = ""
        'Response.ContentType = "application/vnd.ms-excel"
        'Dim sw As New StringWriter()
        'Dim hw As New HtmlTextWriter(sw)

        'For i As Integer = 0 To GridView1.Rows.Count - 1
        '    'Apply text style to each Row
        '    GridView1.Rows(i).Attributes.Add("class", "textmode")
        'Next
        'GridView1.RenderControl(hw)

        ''style to format numbers to string
        'Dim style As String = "<style> .textmode{mso-number-format:\@;}</style>"
        'Response.Write(style)
        'Response.Output.Write(sw.ToString())
        'Response.Flush()
        'Response.End()

        'Remove Panel
        Me.ShowHideDialogueInternal(True)

    End Sub
    Protected Sub btnCloseAddCoumns_Click(sender As Object, e As EventArgs)
        CheckBox1.Checked = False
        Me.ShowHideDialogueInternal(False)
    End Sub
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        CheckBox1.Checked = False
        ddlYear.SelectedIndex = 0

        ddlMonth.Items.Clear()
        ddlMonth.Items.Add(New ListItem("Select", 0))
        ddlMonth.SelectedIndex = 0

        For Each item As ListItem In chkColumnsList.Items
            item.Selected = False
        Next
    End Sub

    Protected Sub GetExel(ByVal ds As DataSet)
        'Create a dummy GridView
        Dim GridView1 As New GridView()
        GridView1.AllowPaging = False
        GridView1.DataSource = ds.Tables(0)
        'GridView1.DataSource = ds
        GridView1.DataBind()

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", _
             "attachment;filename=DataTable.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        For i As Integer = 0 To GridView1.Rows.Count - 1
            'Apply text style to each Row
            GridView1.Rows(i).Attributes.Add("class", "textmode")
        Next
        GridView1.RenderControl(hw)

        'style to format numbers to string
        Dim style As String = "<style> .textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
        Me.ShowHideDialogueInternal(False)
    End Sub
    'Private Shared Function GetTable() As DataTable
    '    '
    '    ' Here we create a DataTable with four columns.
    '    '
    '    Dim table As New DataTable()
    '    table.Columns.Add("Dosage", GetType(Integer))
    '    table.Columns.Add("Drug", GetType(String))
    '    table.Columns.Add("Patient", GetType(String))
    '    table.Columns.Add("Date", GetType(DateTime))

    '    '
    '    ' Here we add five DataRows.
    '    '
    '    table.Rows.Add(25, "Indocin", "David", DateTime.Now)
    '    table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now)
    '    table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now)
    '    table.Rows.Add(21, "Combivent", "Janet", DateTime.Now)
    '    table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now)
    '    Return table
    'End Function

    Sub BindYear()
        Dim table2 As New DataTable()
        table2.Columns.Add("Year", GetType(String))
        Dim now As DateTime = DateTime.Now

        For i As Integer = 2000 To DateTime.Now.Year
            Dim dtDate As New DateTime(i, 1, 1)

            'Console.WriteLine(now.ToString("MMMM"))
            table2.Rows.Add(dtDate.Year)
        Next
        'binding to the Dropdown
        ddlYear.DataTextField = "Year"
        ddlYear.DataValueField = "Year"
        ddlYear.DataSource = table2
        ddlYear.DataBind()
    End Sub

    Sub BindMonth()

        Dim SelectedYear As String
        SelectedYear = ddlYear.SelectedItem.Text

        Dim table2 As New DataTable()
        table2.Columns.Add("Month", GetType(String))
        table2.Columns.Add("MonthNo", GetType(String))

        Dim now As DateTime = DateTime.Now
        If SelectedYear = DateTime.Now.Year Then
            For i As Integer = 1 To DateTime.Now.Month
                Dim dtDate As New DateTime(SelectedYear, i, 1)
                table2.Rows.Add(dtDate.ToString("MMMM"), i)
            Next
        Else
            For i As Integer = 1 To 12
                Dim dtDate As New DateTime(SelectedYear, i, 1)
                table2.Rows.Add(dtDate.ToString("MMMM"), i)
            Next
        End If


        'binding to the Dropdown
        ddlMonth.DataTextField = "Month"
        ddlMonth.DataValueField = "MonthNo"
        ddlMonth.DataSource = table2
        ddlMonth.DataBind()
    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlMonth.Items.Clear()
        ddlMonth.Items.Add(New ListItem("Select", 0))
        ddlMonth.SelectedIndex = 0
        If ddlYear.SelectedIndex <> 0 Then
            Call BindMonth()
        End If
    End Sub
    Protected Sub checkBox1_CheckedChanged(sender As Object, e As EventArgs)
        For Each item As ListItem In chkColumnsList.Items
            item.Selected = CheckBox1.Checked
        Next
    End Sub

    Protected Sub chkColumnsList_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim checkListCount As Integer = 0
        Dim counter As Integer = 0

        checkListCount = chkColumnsList.Items.Count()

        For Each chk In chkColumnsList.Items
            If chk.selected Then
                counter += 1
            End If
        Next

        If checkListCount <> 0 And counter <> 0 Then
            If checkListCount = counter Then
                CheckBox1.Checked = True
            Else
                CheckBox1.Checked = False
            End If
        End If

    End Sub
    'added by Pravin on 17 May 2014 : End
End Class
