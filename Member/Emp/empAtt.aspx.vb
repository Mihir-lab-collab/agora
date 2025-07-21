Imports System.Data
Imports System.Data.SqlClient
Partial Class emp_empAtt
    Inherits Authentication

    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
	Dim empId As String
	Dim strSQL As String
	Dim arg As String
	Dim cmd As New SqlCommand
	Dim dtr As SqlDataReader
	Dim empJoining
	Dim arrHoliday()
	Dim arrDate()
	Dim arrStatus()
	Dim arrTimeIN()
	Dim arrTimeOUT()
	Dim colHoliday = "#C5D5AE"
	Dim colAbsent = "#FFBBBB"
	Dim colLeave = "#BBFFBB"
	Dim colNormal = "#FFFFFF"
    Dim count As Integer = "0"
    Dim gf As New generalFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        empId = CInt(Session("dynoEmpIdSession"))

        If CStr(Session("dynoEmpIdSession")) = "" Then
            Response.Redirect("emplogin.aspx")
        End If

        Dim strDate = Request.QueryString("strDate")
        If strDate = Nothing Then
            strDate = Now()

        ElseIf Not IsDate(strDate) Or Month(strDate) = Month(Date.Today) Then
            strDate = Now()
        End If


        Dim startDate = ("1-" & MonthName(Month(FormatDateTime(strDate, 2))) & "-" & Year(strDate))
        Dim endDate = (DateAdd("m", 1, strDate))
        endDate = (DateAdd("d", -1, "1-" & MonthName(Month(FormatDateTime(endDate, 2))) & "-" & Year(endDate)))

        If Not IsPostBack Then

            Dim i
            Dim satStatus = 0
            Dim z
            Dim Sql = "SELECT holidayDate,holidayDesc FROM holidayMaster WHERE holidayDate BETWEEN '" & _
                   startDate & "' AND '" & endDate & "' ORDER BY holidayDate"
            strConn.Open()
            cmd = New SqlCommand(Sql, strConn)
            dtr = (cmd.ExecuteReader())
            While dtr.Read()
                ReDim Preserve arrHoliday(count)
                arrHoliday(count) = dtr("holidayDate")
                count += 1
            End While
            dtr.Close()
            strConn.Close()

            strSQL = "SELECT empJoiningDate,empLeavingDate FROM employeeMaster WHERE empId=" & empId
            strConn.Open()
            cmd = New SqlCommand(strSQL, strConn)
            dtr = (cmd.ExecuteReader())
            If dtr.Read Then
                empJoining = dtr("empJoiningDate") & "," & dtr("empLeavingDate")
            End If
            dtr.Close()
            strConn.Close()
            Dim str = Split(empJoining, ",")

            strSQL = "SELECT * FROM empAtt WHERE  attDate BETWEEN '" & startDate & "' AND '" & endDate & _
             "' AND empId=" & empId
            ' Response.Write(strSQL)
            'Response.End()
            strConn.Open()
            cmd = New SqlCommand(strSQL, strConn)
            dtr = (cmd.ExecuteReader())
            While dtr.Read()
                ReDim Preserve arrDate(count)
                ReDim Preserve arrStatus(count)
                ReDim Preserve arrTimeIN(count)
                ReDim Preserve arrTimeOUT(count)
                arrDate(count) = dtr("attDate")
                arrStatus(count) = dtr("attStatus")
                arrTimeIN(count) = dtr("attInTime")
                arrTimeOUT(count) = dtr("attOutTime")
                count += 1
            End While
            dtr.Close()
            strConn.Close()

            For i = 1 To DateDiff("d", startDate, endDate) + 1
                Dim trow As New TableRow
                Dim tcel1 As New TableCell
                Dim tcel2 As New TableCell
                Dim tcel3 As New TableCell
                Dim tcel4 As New TableCell

                Dim currDate = i & "-" & MonthName(Month(startDate)) & "-" & Year(startDate)
                Dim attDt = i & " " & WeekdayName(Weekday(currDate))
                tcel1.Controls.Add(New LiteralControl(attDt))
                tcel1.Style.Value = "width:32%;color:Navy;text-align:left;font-face:verdana;font-size:14px;"
                If Weekday(currDate) = 7 Then
                    satStatus = satStatus + 1
                End If

                Dim holidayFlag = False
                If IsArray(arrHoliday) Then
                    For z = 0 To UBound(arrHoliday)
                        If (arrHoliday(z)) = currDate Then
                            holidayFlag = True
                        End If
                    Next
                End If

                If Weekday(currDate) = 1 Or (Weekday(currDate) = 7 And satStatus Mod 2 = 0) Or holidayFlag Then
                    tcel2.Style.Value = "width:12%;background-color:#C5D5AE;"
                    tcel3.Style.Value = "width:14%;background-color:#C5D5AE;"
                    tcel4.Style.Value = "width:16%;background-color:#C5D5AE;"
                    holidayFlag = False

                Else
                    Dim empPrint
                    Dim col
                    Dim fontCol = ""
                    Dim timeIn
                    Dim timeOut

                    If IsArray(arrStatus) Then
                        For z = 0 To UBound(arrStatus)
                            Dim empStatus = arrStatus(z)

                            If i = Day(arrDate(z)) Then
                                If Right(empStatus, 1) = "L" Or empStatus = "CO" Then
                                    col = colLeave
                                    If empStatus = "WL" Then
                                        col = colAbsent
                                    End If
                                    empPrint = empStatus
                                Else
                                    If IsDBNull(arrTimeOUT(z)) Then
                                        timeIn = FormatDateTime(arrTimeIN(z), 4)
                                        timeOut = ""
                                    Else
                                        timeIn = FormatDateTime(arrTimeIN(z), 4)
                                        timeOut = FormatDateTime(arrTimeOUT(z), 4)
                                    End If
                                    If timeIn > "10:15" Then
                                        empPrint = "HD"
                                        fontCol = "#FF0000"
                                        col = colNormal
                                        tcel3.Controls.Add(New LiteralControl(timeIn))
                                        tcel4.Controls.Add(New LiteralControl(timeOut))
                                    Else
                                        empPrint = empStatus
                                        col = colNormal
                                        tcel3.Controls.Add(New LiteralControl(timeIn))
                                        tcel4.Controls.Add(New LiteralControl(timeOut))
                                    End If
                                    tcel3.Style.Value = "width:14%;text-align:center;font-face:verdana;font-size:14px;"
                                    tcel4.Style.Value = "width:17%;text-align:center;font-face:verdana;font-size:14px;"
                                End If

                                Exit For
                            ElseIf DateDiff("d", str(0), currDate) < 0 Or str(1) <> "" Then
                                empPrint = ""
                                col = colNormal
                            ElseIf DateDiff("d", (Date.Today), (currDate)) > 0 Then  'remaining days 
                                empPrint = ""
                                col = colNormal
                            ElseIf (currDate) = (Date.Today) Then
                                empPrint = "P"
                                col = colNormal
                            Else
                                empPrint = "A"
                                col = colAbsent
                            End If
                        Next
                    ElseIf currDate = (Date.Today) Then
                        empPrint = "P"
                        col = colNormal
                    Else
                        empPrint = ""
                        col = colNormal
                    End If

                    tcel2.BackColor = Drawing.Color.FromName(col)
                    tcel2.ForeColor = Drawing.Color.FromName(fontCol)
                    tcel2.Style.Value = "width:12%;text-align:center;font-face:verdana;font-size:14px"
                    tcel2.Controls.Add(New LiteralControl(empPrint))

                End If
                tcel1.BorderStyle = BorderStyle.Groove
                tcel2.BorderStyle = BorderStyle.Groove
                tcel3.BorderStyle = BorderStyle.Groove
                tcel4.BorderStyle = BorderStyle.Groove
                tcel1.BorderColor = Drawing.Color.Snow
                tcel2.BorderColor = Drawing.Color.Snow
                tcel3.BorderColor = Drawing.Color.Snow
                tcel4.BorderColor = Drawing.Color.Snow
                trow.Controls.Add(tcel1)
                trow.Controls.Add(tcel2)
                trow.Controls.Add(tcel3)
                trow.Controls.Add(tcel4)
                attTable.Rows.Add(trow)

            Next

        End If

    End Sub


End Class
