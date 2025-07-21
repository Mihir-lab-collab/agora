Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections
Partial Class TimeReports
    Inherits Authentication

    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim attdate As String = ""
    Dim years As Integer = DateTime.Now.Year
    Dim months As Integer = DateTime.Now.Month
    Dim totaldays As Integer
    Dim da As New SqlDataAdapter
    Dim daEmp As New SqlDataAdapter
    Dim cmd As SqlCommand
    Dim cmdEmp As SqlCommand
    Dim dtTable As DataTable
    Dim dtTableEmp As DataTable
    Dim dsEmp As DataSet
    Dim grdTable As New DataTable()
    Dim myArray(3)
    Dim objDate As Object

    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Try
        attdate = Request.QueryString("strDate")
        attdate = Convert.ToDateTime(attdate).ToString("MM/dd/yyyy")
        If (attdate = "") Then
            months = DateTime.Now.Month.ToString()
            years = DateTime.Now.Year.ToString()
            objDate = DateTime.Now
        Else
            myArray = attdate.Split("/")
            months = myArray(0).ToString()
            years = myArray(2).ToString()
            objDate = Convert.ToDateTime(Request.QueryString("strDate"))
        End If
        lblMonthName.Text = String.Format("{0:MMMM}", objDate) + " " + Convert.ToDateTime(objDate).Year.ToString()
        Response.Write(myArray(0).ToString() + "==" + myArray(2).ToString())
        'Response.End()
        'months = "2"
        'years = "2009"

        totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))


        cmd = New SqlCommand()
        cmd.CommandText = "GET_HOURSREPORT_NEW"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@days", totaldays)
        cmd.Parameters.AddWithValue("@months", months)
        cmd.Parameters.AddWithValue("@years", years)
        cmd.Connection = strConn
        Dim dsWeekDate As New DataSet
        da = New SqlDataAdapter(cmd)
        da.Fill(dsWeekDate)


        If (dsWeekDate.Tables.Count > 0) Then
            Dim grdColName As New DataColumn("Name")
            grdTable.Columns.Add(grdColName)
            If (dsWeekDate.Tables(0).Rows.Count > 0) Then
                Dim dr As DataRow
                Dim col As Integer = 1
                For Each dr In dsWeekDate.Tables(0).Rows
                    Dim grdCol As New DataColumn("        Week" + col.ToString() + " " + Convert.ToDateTime(dr(0).ToString()).ToString("dd-MMM") + " To " + Convert.ToDateTime(dr(1).ToString()).ToString("dd-MMM"))
                    col = col + 1
                    grdTable.Columns.Add(grdCol)
                Next
            End If
        End If

        cmdEmp = New SqlCommand()
        cmdEmp.CommandText = "GET_EMPLOYEE"
        cmdEmp.CommandType = CommandType.StoredProcedure
        cmdEmp.Connection = strConn
        Dim dsEmp As New DataSet()
        daEmp = New SqlDataAdapter(cmdEmp)
        daEmp.Fill(dsEmp)
        If (dsEmp.Tables(0).Rows.Count > 0) Then
            Dim drEmp As DataRow
            For Each drEmp In dsEmp.Tables(0).Rows 'EMP LOOP

                Dim dr As DataRow
                Dim i As Integer = 0
                Dim gdRow As DataRow = grdTable.NewRow
                gdRow(0) = drEmp(1).ToString()
                For Each dr In dsWeekDate.Tables(0).Rows ' WEEKDATE LOOP
                    i = i + 1
                    gdRow(i) = GetWeekHours(drEmp(0).ToString(), dr(0).ToString(), dr(1).ToString())

                    'Dim str As String = GetWeekHours(drEmp(0).ToString(), dr(0).ToString(), dr(1).ToString())
                    'Response.Write(drEmp(0).ToString() + " == " + i.ToString() + " == " + " == " + dr(0).ToString() + " == " + dr(1).ToString())
                    'Response.Write("<br>")
                Next
                grdTable.Rows.Add(gdRow)

            Next
        End If


        grdReport.DataSource = grdTable
        grdReport.DataBind()


        Dim tempVar As Integer = 0
        Dim tempVar2 As Double = 0
        Dim gridRow As GridViewRow
        Dim colCount As Integer = grdTable.Columns.Count

        For Each gridRow In grdReport.Rows
            For tempVar = 1 To colCount - 1
                tempVar2 = tempVar Mod 2
                Try
                    If tempVar2 <> 0 Then
                        If Convert.ToDouble(gridRow.Cells(tempVar).Text) < 50.0 Then
                            gridRow.Cells(tempVar).BackColor = Color.Yellow
                        End If
                    Else
                        If Convert.ToDouble(gridRow.Cells(tempVar).Text) < 45.0 Then
                            gridRow.Cells(tempVar).BackColor = Color.Yellow
                        End If
                    End If
                Catch ex As Exception
                    gridRow.Cells(tempVar).Text = "0.0"
                    gridRow.Cells(tempVar).BackColor = Color.Yellow
                End Try

            Next
        Next
        'Catch ex As Exception
        '    Response.Write(ex.ToString())
        'End Try

    End Sub

    Function GetWeekHours(ByVal empID As String, ByVal WkStartDate As DateTime, ByVal WkEndDate As DateTime) As String


        Dim TotalHr As Double
        Dim LeaveHr As Double
        LeaveHr = GetLeaveHours(empID, WkStartDate, WkEndDate)
        Dim cmdHrs As SqlCommand
        Dim daHrs As New SqlDataAdapter
        Dim dsHrs As New DataSet
        Dim objHrs As Object
        dsHrs = New DataSet()
        cmdHrs = New SqlCommand()
        cmdHrs.CommandText = "GET_WEEK_HOURS"
        cmdHrs.CommandType = CommandType.StoredProcedure
        cmdHrs.Parameters.AddWithValue("@empid", empID)
        cmdHrs.Parameters.AddWithValue("@StartDate", WkStartDate)
        cmdHrs.Parameters.AddWithValue("@EndDate", WkEndDate)
        cmdHrs.Connection = strConn
        strConn.Open()
        objHrs = cmdHrs.ExecuteScalar()

        ' Response.Write(objHrs.ToString())
        'Response.End()
        If IsNumeric(objHrs) Then
            TotalHr = Convert.ToDouble(objHrs) + Convert.ToDouble(LeaveHr)
        Else
            TotalHr = Convert.ToDouble(LeaveHr)
        End If


        strConn.Close()
        If (Convert.ToString(objHrs) <> "") Then
            Return TotalHr.ToString()
        Else
            Return "0"
        End If


    End Function

    Function GetLeaveHours(ByVal empID As String, ByVal WkStartDate As DateTime, ByVal WkEndDate As DateTime) As String
        'Try


        Dim cmdLeaveHrs As SqlCommand
        Dim daLeaveHrs As New SqlDataAdapter
        Dim dsLeaveHrs As New DataSet
        Dim objLeaveHrs As Object
        dsLeaveHrs = New DataSet()
        cmdLeaveHrs = New SqlCommand()
        cmdLeaveHrs.CommandText = "GET_LEAVE_HOURS"
        cmdLeaveHrs.CommandType = CommandType.StoredProcedure
        cmdLeaveHrs.Parameters.AddWithValue("@empid", empID)
        cmdLeaveHrs.Parameters.AddWithValue("@StartDate", WkStartDate)
        cmdLeaveHrs.Parameters.AddWithValue("@EndDate", WkEndDate)
        cmdLeaveHrs.Connection = strConn
        strConn.Open()
        objLeaveHrs = cmdLeaveHrs.ExecuteScalar()
        strConn.Close()
        If (Convert.ToString(dsLeaveHrs) <> "") Then
            Return objLeaveHrs.ToString()
        Else
            Return "0"
        End If
        'Catch ex As Exception
        '    Return "0"
        'End Try

    End Function
End Class
