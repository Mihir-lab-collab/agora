Imports System.Data
Imports System.Data.SqlClient
Partial Class admin_pldetails
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        Dim conn As SqlConnection = New SqlConnection(dsn)
        Dim Sqlpl As String
        Dim objcmdpl As SqlCommand
        Dim empJoiningDate As DateTime
        Dim empTotPLLbl As Integer
        Dim empConfDate, plDateStart, plAvailMonth As String
        Dim objdatareaderpl As SqlDataReader
        Dim currStartDate, currEndDate As String
        If Month(Now()) > 3 Then
            currStartDate = "1-Apr-" & Year(Now())
            currEndDate = "31-Mar-" & Year(Now()) + 1
        Else
            currStartDate = "1-Apr-" & Year(Now()) - 1
            currEndDate = "31-Mar-" & Year(Now())
        End If
        Dim sql1 As String
        Dim objcmdlimit As SqlCommand
        Dim objdatareaderlimit As SqlDataReader
        Dim diff As Integer
        conn.Open()
        sql1 = "SELECT * FROM empStatus WHERE statusLimit > 0"
        objcmdlimit = New SqlCommand(sql1, conn)
        objdatareaderlimit = objcmdlimit.ExecuteReader
        Do While objdatareaderlimit.Read()
            If objdatareaderlimit("statusId") = "PL" Then
                empTotPLLbl = objdatareaderlimit("statusLimit")
                diff = (DateDiff("m", empJoiningDate, currEndDate) - 12) * (objdatareaderlimit("statusLimit") / 12)
            End If
            'Integer.Parse(diff)
        Loop
        objdatareaderlimit.Close()
        Response.Write(diff)


        Sqlpl = "SELECT * FROM employeeMaster WHERE empId=" & Request.QueryString("empid")
        objcmdpl = New SqlCommand(Sqlpl, conn)
        objdatareaderpl = objcmdpl.ExecuteReader
        If objdatareaderpl.Read() Then
            empJoiningDate = Format(objdatareaderpl("empJoiningDate"), "dd-MMM-yyyy")
            empConfDate = Format(DateAdd("m", objdatareaderpl("empProbationPeriod"), objdatareaderpl("empJoiningDate")), "dd-MMM-yyyy")
        End If
        objdatareaderpl.Close()
        plDateStart = DateAdd("m", 12, empJoiningDate)

        If DateDiff("d", plDateStart, currStartDate) > 0 Then
            empTotPLLbl = empTotPLLbl
        ElseIf plDateStart > currEndDate Then
            empTotPLLbl = 0
        Else
            plAvailMonth = DateDiff("m", plDateStart, currEndDate)
        End If

        Dim rs As SqlDataReader
        Dim objcmd As SqlCommand
        Dim str As String
        Dim strDate As String = ""

        str = "select empJoiningdate from dbo.employeeMaster where empID =" & Request.QueryString("empid")
        objcmd = New SqlCommand(str, conn)
        rs = objcmd.ExecuteReader
        While rs.Read
            strDate = Year(rs(0))
        End While
        objcmd.Dispose()
        rs.Close()

        Dim yearleave As String
        If strDate <> "" Then
            Dim findate As String = "1-Mar-" & strDate
            Dim Sql As String
            While (Year(findate) < Year(Now()))
                'Response.Write(findate)
                Dim rdr As SqlDataReader
                Dim cmdIn As SqlCommand
                findate = "1-Mar-" & (Year(findate))
                Sql = "select count(EmpID) as plused from empatt where EmpID =" & Request.QueryString("empid") & "  and attStatus = 'PL' and attdate  between '" & "1-Mar-" & (Year(findate)) & "' and '" & "1-Mar-" & (Year(findate) + 1) & "'"
                cmdIn = New SqlCommand(Sql, conn)
                rdr = cmdIn.ExecuteReader
                yearleave = ""
                While rdr.Read
                    yearleave = rdr(0)
                End While
                rdr.Close()
                cmdIn.Dispose()

                Response.Write(findate & " " & yearleave & "" & empTotPLLbl & "<br>")


            End While
        End If
    End Sub
End Class
