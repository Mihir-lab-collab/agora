Imports System.Data
Imports System.Data.SqlClient
Partial Class admin_plleaveajax
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        Dim rs As SqlDataReader
        Dim conn As New SqlConnection(dsn)
        Dim objcmd As SqlCommand
        Dim str As String
        Dim empTotPL As Integer
        Dim plAvailMonth, plDateStart, currStartDate As String
        Dim strDate As String = ""
        Dim strData As String = "<table class=sample_attach1 width=100%><tr><td align=center style=font-family:Verdana;font-size:13;color:Red; font-weight:800:width=40%;color:Red; >Year</td><td style=font-family:Verdana;color:Red;font-size:12; font-weight:800:width=15%;>Issued</td><td style=font-family:Verdana;color:Red;font-size:12; font-weight:800:width=15%;>Available</td><td style=font-family:Verdana;font-size:12;color:Red;font-weight:800:width=20%;>Consumed</td><td style=font-family:Verdana;font-size:12;color:Red; font-weight:800:width=15%;>Balance</td></tr>"
        Dim empJoiningDate As DateTime
        conn.Open()
        str = "select empJoiningdate from dbo.employeeMaster where empID =" & Request.QueryString("empid")

        objcmd = New SqlCommand(str, conn)
        rs = objcmd.ExecuteReader
        While rs.Read
            strDate = Year(rs(0))
            empJoiningDate = Day(rs(0)) & "-" & Mid(MonthName(Month(rs(0))), 1, 3) & "-" & Year(rs(0))
        End While
        objcmd.Dispose()
        rs.Close()

        Dim yearleave As String
        If strDate <> "" Then
            Dim findate As String = "31-Mar-" & strDate
            Dim Sql As String
            Dim currEndDate As String
            If Month(Now()) > 3 Then
                currEndDate = "31-Mar-" & Year(Now()) + 1
                currStartDate = "1-Apr-" & Year(Now())
            Else
                currEndDate = "31-Mar-" & Year(Now())
                currStartDate = "1-Apr-" & Year(Now()) - 1
            End If

            While (Year(findate) < Year(currEndDate) - 1)
                Dim rdr As SqlDataReader
                Dim cmdIn As SqlCommand
                findate = "31-Mar-" & (Year(findate) + 1)
                Sql = "select count(EmpID) as plused from empatt where EmpID =" & Request.QueryString("empid") & "  and attStatus = 'PL' and attdate   between '" & "1-apr-" & (Year(findate)) & "' and '" & "31-Mar-" & (Year(findate) + 1) & "'"
  
                cmdIn = New SqlCommand(Sql, conn)
                rdr = cmdIn.ExecuteReader
                yearleave = ""
                While rdr.Read
                    yearleave = rdr(0)
                End While
                rdr.Close()
                cmdIn.Dispose()

                Dim sql1 As String
                Dim plissued As Integer
                Dim objcmdlimit As SqlCommand
                Dim empLimitTotPL As Integer
                Dim objdatareaderlimit As SqlDataReader
                sql1 = "SELECT * FROM empStatus WHERE statusLimit > 0"
                objcmdlimit = New SqlCommand(sql1, conn)
                objdatareaderlimit = objcmdlimit.ExecuteReader
                Do While objdatareaderlimit.Read()
                    If objdatareaderlimit("statusId") = "PL" Then
                        empLimitTotPL = objdatareaderlimit("statusLimit")
                        plissued = objdatareaderlimit("statusLimit")
                        empTotPL = objdatareaderlimit("statusLimit")
                    End If
                Loop
                '  empTotPL = DateDiff("m", empJoiningDate, findate)
                'plissued = DateDiff("m", empJoiningDate, findate)
                'If empTotPL < 12 Then
                '    empTotPL = 0
                '    '   plissued = 0
                'End If
                'Else
                '    empTotPL = (empTotPL - 12) * (empLimitTotPL / 12)
                '    plissued = (plissued - 12) * (empLimitTotPL / 12)
                '    plissued = plissued
                '    If plissued > 20 Then
                '        plissued = 20
                '    End If
                'End If

                plDateStart = DateAdd("m", 12, empJoiningDate)
                '  plAvailMonth = DateDiff("m", plDateStart, currEndDate)

                If DateDiff("d", plDateStart, currStartDate) > 0 Then
                    ' empTotPL = empTotPL
                    plAvailMonth = DateDiff("m", plDateStart, "31 - Mar - " & (Year(findate) + 1) & "")
                   
                    ' Response.Write(plAvailMonth & "plAvailMonth")
                    empTotPL = CInt(FormatNumber((empTotPL / 12) * plAvailMonth, 0))
                    If empTotPL > 20 Then
                        empTotPL = 20
                    End If

                    '  Response.Write(empTotPL & "<br>")
                ElseIf plDateStart > currEndDate Then
                    empTotPL = 0
                Else
                    ' Response.Write("wefwefwefwef")
                    plAvailMonth = DateDiff("m", plDateStart, currEndDate)
                    ' Response.Write(plAvailMonth & "plAvailMonth")
                    empTotPL = CInt(FormatNumber((empTotPL / 12) * plAvailMonth, 0))
                End If

                If DateAdd("m", 12, empJoiningDate) > Now() Then
                    ' plissued = 0
                    empTotPL = 0
                End If
                Dim yearpl As String
                yearpl = Year(findate)
                objdatareaderlimit.Close()
                Dim leavebal As Integer
                Dim leaveaval As Integer
                ' Response.Write(leaveaval & "leaveaval")
                If leaveaval > 90 Then
                    leaveaval = 90
                End If
                Dim leaveconsume As Integer
                leaveconsume = yearleave
                leaveaval = empTotPL + leavebal
                leavebal = leaveaval - leaveconsume
                If leavebal >= 90 Then
                    leavebal = 90
                End If
                'Response.Write(yearpl & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & plissued & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & vbCrLf & leaveaval & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & leaveconsume & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & leavebal & "<br>")
                strData = strData & "<tr><td style=font-family:Verdana;font-size:12; font-weight:800:width=40%; align=center>" & yearpl & "-" & yearpl + 1 & " </td><td style=font-family:Verdana;font-size:12; font-weight:800:width=15%; align=center>" & empTotPL & "</td><td style=font-family:Verdana;font-size:12; font-weight:800:width=15%; align=center>" & leaveaval & "</td><td style=font-family:Verdana;font-size:12; font-weight:800:width=15%; align=center>" & leaveconsume & "</td><td  style=font-family:Verdana;font-size:12; font-weight:800:width=15%; align=center>" & leavebal & "</td></tr>"
            End While
            strData = strData & "</table>"
        End If
        Response.Write(strData)
    End Sub
End Class
