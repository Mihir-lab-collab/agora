Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI

Partial Class emp_empLeave
    Inherits Authentication
    Dim sql As String
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim conn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objdatareader As SqlDataReader
    Dim objcmd As SqlCommand
    Public currStartDate, currEndDate
    Dim empTotCL, empTotSL, empTotPL
    Dim empJoiningDate, empConfDate
    Dim availMonth
    Dim plDateStart, plAvailMonth
    Dim gf As New generalFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        If CStr(Session("dynoEmpIdSession")) <> "" Then
            Dim empId = Session("dynoEmpIdSession")
			Dim Prorata = 1
            If Month(Now()) > 3 Then
                currStartDate = "1-Apr-" & Year(Now())
                currEndDate = "31-Mar-" & Year(Now()) + 1
                'Prorata = (Month(Now()) - 4) / 12
                'If (Prorata.ToString() = "0") Then
                '    Prorata = 1
                'End If
            Else
                currStartDate = "1-Apr-" & Year(Now()) - 1
                currEndDate = "31-Mar-" & Year(Now())
                Prorata = (Month(Now()) + 8) / 12
            End If
		Prorata  = 1
	   ' Response.Write(Prorata)
  		'Response.Write(currStartDate )
			'Response.Write(currEndDate  )
            conn.Open()
          
            If Not IsPostBack() Then

                sql = "SELECT * FROM employeeMaster WHERE empId=" & empId
                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader
                If objdatareader.Read() Then
                    empJoiningDate = Format(objdatareader("empJoiningDate"), "dd-MMM-yyyy")
                    empConfDate = Format(DateAdd("m", objdatareader("empProbationPeriod"), objdatareader("empJoiningDate")), "dd-MMM-yyyy")
                    empJoiningDateLbl.Text = empJoiningDate
                    empConfDateLbl.Text = empConfDate
                End If
                objdatareader.Close()

                plDateStart = DateAdd("m", 12, empJoiningDate)
                Dim a As Double '----- lalt to last total pl
                Dim diff As Double

                Dim plsused As Integer

                sql = "SELECT attStatus,count(*) as lCount FROM empAtt " & _
           "WHERE empId=" & empId & " AND attDate BETWEEN '" & empJoiningDate & _
           "' AND '" & currEndDate & "' GROUP BY attStatus"

			
                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader
                Do While objdatareader.Read()

                    If objdatareader("attStatus") = "PL" Then
                        plsused = objdatareader("lCount")
                    End If
                Loop
                objdatareader.Close()


                plAvailMonth = 0

                sql = "SELECT * FROM empStatus WHERE statusLimit > 0"
                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader
                Do While objdatareader.Read()
                    If objdatareader("statusId") = "CL" Then
                        empTotCLLbl.Text = objdatareader("statusLimit")
                        If IsNumeric(empTotCLLbl.Text) Then
                            Dim dg As Decimal
                            empTotCLLbl.Text = FormatNumber(empTotCLLbl.Text * Prorata, 0)

                        End If
                    ElseIf objdatareader("statusId") = "SL" Then
                        empTotSLLbl.Text = objdatareader("statusLimit")
                        If IsNumeric(empTotSLLbl.Text) Then
                            empTotSLLbl.Text = FormatNumber(empTotSLLbl.Text * Prorata, 0)
                        End If
                    ElseIf objdatareader("statusId") = "PL" Then
                        empTotPLLbl.Text = objdatareader("statusLimit")
                        diff = (DateDiff("m", empJoiningDate, currEndDate) - 13) * (objdatareader("statusLimit") / 12)
                        empTotPLLbl.Text = CInt(diff)
                        a = (DateDiff("m", empJoiningDate, currStartDate) - 13) * (objdatareader("statusLimit") / 12)
                        empTotPLLbl.Text = CInt(objdatareader("statusLimit") + a).ToString()
                        If IsNumeric(empTotPLLbl.Text) Then
                            'empTotPLLbl.Text = FormatNumber(empTotPLLbl.Text * Prorata, 0)
                        End If
                    End If

                Loop

                objdatareader.Close()
                sql = "select count(coID)as compOff from empCompOff  where empID=" & empId & " and codate >= DATEADD(D,-60,GETDATE()) " ' month(codate)=" & DateTime.Now.Month.ToString() & " and year(codate)=" & DateTime.Now.Year.ToString()
                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader
                Do While objdatareader.Read()
                    empTotCompOfflbl.Text = objdatareader("compOff")
                Loop
                objdatareader.Close()
               
                If (DateDiff("d", empConfDate, currStartDate)) < 0 Then
                    availMonth = 11 - DateDiff("m", currStartDate, empConfDate)
                    empTotCLLbl.Text = FormatNumber((empTotCLLbl.Text / 12) * availMonth, 0)
                    empTotSLLbl.Text = FormatNumber((empTotSLLbl.Text / 12) * availMonth, 0)
                End If

                availMonth = DateDiff("m", currStartDate, Now())

                If DateDiff("d", plDateStart, currStartDate) > 0 Then
                    empTotPLLbl.Text = empTotPLLbl.Text
                ElseIf plDateStart > currEndDate Then
                    empTotPLLbl.Text = 0
                Else
                    plAvailMonth = DateDiff("m", plDateStart, currEndDate)
                    '	empTotPLLbl.Text = FormatNumber((empTotPLLbl.Text/12)*plAvailMonth,0)


                    'response.write(empTotPLLbl.Text &"ergergrg")
                    '---------- calculate total pl after comp of 1 yr --- response.write(empTotPLLbl.Text & "new")
                End If
                Dim strSql As String


                empTotCLCLbl.Text = 0
                empTotSLCLbl.Text = 0
                empTotPLCLbl.Text = 0
                empToCompOffClbl.Text = 0


                sql = "SELECT attStatus,count(*) as lCount FROM empAtt " & _
                "WHERE empId=" & empId & " AND attDate BETWEEN '" & currStartDate & _
                "' AND '" & currEndDate & "' GROUP BY attStatus"
                'Response.Write(sql)
                'Response.End()
                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader

                Do While objdatareader.Read()
                    If objdatareader("attStatus") = "CL" Then
                        empTotCLCLbl.Text = objdatareader("lCount")
                    ElseIf objdatareader("attStatus") = "SL" Then
                        empTotSLCLbl.Text = objdatareader("lCount")
                    ElseIf objdatareader("attStatus") = "PL" Then
                        empTotPLCLbl.Text = objdatareader("lCount")
                        'commented by Dipti
                        'ElseIf objdatareader("attStatus") = "CO" Then
                        '    empToCompOffClbl.Text = objdatareader("lCount")
                        'end commented by Dipti
                    End If
                Loop
                objdatareader.Close()

                'New Added for PL
                sql = "SELECT count(*) as 'GetToatalPL' FROM empAtt " & _
               "WHERE  attStatus='PL' and empId=" & empId & " AND attDate BETWEEN '" & empJoiningDate & _
               "' AND '" & currStartDate & "'"


                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader
                Do While objdatareader.Read()
                    empTotPLLbl.Text = empTotPLLbl.Text - objdatareader("GetToatalPL")
                Loop
                'end of pl
                objdatareader.Close()

                'commented & added  by Dipti for CO consumed
                ''New Added for CO
                'sql = "SELECT count(*) as 'TotalCO'  FROM empAtt WHERE upper(attStatus)='CO' and empId=" & empId & " and attDate >= DATEADD(D,-60,GETDATE())" ' month(attDate)="& DateTime.Now.Month.ToString()& " and year(attDate)=" & DateTime.Now.Year.ToString()
                'objcmd = New SqlCommand(sql, conn)
                'objdatareader = objcmd.ExecuteReader
                'Do While objdatareader.Read()
                '    empToCompOffClbl.Text = objdatareader("TotalCO")
                'Loop
                'objdatareader.Close()
                ''end CO

                sql = "select empid,count(coid) ConsumedCO from empcompoff " & _
               "where codate >= DATEADD(D,-60,GETDATE()) and empId=" & empId & " AND isnull(empLeaveId,0)<> 0 group by empid "

                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader
                Do While objdatareader.Read()
                    empToCompOffClbl.Text = objdatareader("ConsumedCO")
                Loop
                objdatareader.Close()

                'end commented & added by Dipti for CO consumed

                strSql = "SELECT attStatus,count(*) as lCount FROM empAtt " & _
                "WHERE empId=" & empId & " AND attDate BETWEEN '" & currStartDate & _
                "' AND '" & currEndDate & "' GROUP BY attStatus"

                If empTotPLLbl.Text >= 90 Then
                    empTotPLLbl.Text = 90
                End If


                empTotCLBLbl.Text = empTotCLLbl.Text - empTotCLCLbl.Text
                empTotSLBLbl.Text = empTotSLLbl.Text - empTotSLCLbl.Text
                empTotPLBLbl.Text = empTotPLLbl.Text - empTotPLCLbl.Text

                If (empTotCompOfflbl.Text = "") Then
                    empTotCompOfflbl.Text = "0"
                End If
                empToCompOffBlbl.Text = CInt(empTotCompOfflbl.Text) - CInt(empToCompOffClbl.Text)

                'Add values to hidden fields
                hf_CLBalance.Value = empTotCLBLbl.Text
                hf_SLBalance.Value = empTotSLBLbl.Text
                hf_PLBalance.Value = empTotPLBLbl.Text
                hf_CompOffBalance.Value = empToCompOffBlbl.Text
                '------- get after 1 yr response.write(empTotPLBLbl.Text)
                '-------  fi not 1 yr thn 
                If DateAdd("m", 12, empJoiningDate) > Now() Then

                    empTotPLBLbl.Text = 0
                    empTotPLCLbl.Text = 0
                    empTotPLLbl.Text = 0
                End If

                If empConfDate > Now() Then
                    empTotCLBLbl.Text = 0
                    empTotCLCLbl.Text = 0
                    empTotCLLbl.Text = 0

                    empTotSLBLbl.Text = 0
                    empTotSLCLbl.Text = 0
                    empTotSLLbl.Text = 0
                End If
                '------ end if not 1 yr
                sql = "SELECT attDate FROM empAtt " & _
                "WHERE empId=" & empId & " AND attDate BETWEEN '" & currStartDate & _
                "' AND '" & currEndDate & "' AND attStatus='CL' ORDER BY attDate"
                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader()
                clDataGrid.DataSource = objdatareader
                clDataGrid.DataBind()
                objdatareader.Close()

                sql = "SELECT attDate FROM empAtt " & _
                "WHERE empId=" & empId & " AND attDate BETWEEN '" & currStartDate & _
                "' AND '" & currEndDate & "' AND attStatus='SL' ORDER BY attDate"
                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader()
                slDataGrid.DataSource = objdatareader
                slDataGrid.DataBind()
                objdatareader.Close()


                sql = "SELECT attDate FROM empAtt " & _
                "WHERE empId=" & empId & " AND attDate BETWEEN '" & currStartDate & _
                "' AND '" & currEndDate & "' AND attStatus='PL' ORDER BY attDate"
                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader()
                plDataGrid.DataSource = objdatareader
                plDataGrid.DataBind()
                objdatareader.Close()

                sql = "Select attDate from empAtt " & _
                "where empId=" & empId & " and attDate between '" & currStartDate & _
                "' and '" & currEndDate & "' and attStatus='CO' order by attDate"
                objcmd = New SqlCommand(sql, conn)
                objdatareader = objcmd.ExecuteReader()
                coDataGrid.DataSource = objdatareader
                coDataGrid.DataBind()
                objdatareader.Close()

                BindDataGrid()

            End If
        Else
            Response.Redirect("empLogin.aspx")
        End If


    End Sub

    Protected Sub BindDataGrid()
        Dim empId = Session("dynoEmpIdSession")
        Dim dad As New SqlDataAdapter
        Dim ds1 As New DataSet
        Dim sql1 As String
        sql1 = "select empLeaveDetails.*,empStatus.statusDesc as desc1,case when leavestatus='p' then 'Pending' when leavestatus='r' then 'Rejected' when leavestatus='a' then 'Approved' end as ls from empLeaveDetails,empStatus where LeaveId = statusID AND empLeaveDetails.empId= " & empId & " ORDER BY empLeaveDetails.empLeaveId DESC "

        dad = New SqlDataAdapter(sql1, conn)
        dad.Fill(ds1)
        laDataGrid.DataSource = ds1
        laDataGrid.DataBind()
        dad.Dispose()
        conn.Close()

    End Sub

    Protected Sub laDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles laDataGrid.PageIndexChanged
        laDataGrid.CurrentPageIndex = e.NewPageIndex
        BindDataGrid()
    End Sub

    Protected Sub laDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles laDataGrid.ItemDataBound
        If e.Item.Cells(5).Text = "Rejected" Then
            e.Item.Cells(5).ForeColor = System.Drawing.Color.Red
            e.Item.FindControl("lbtnDelete").Visible = True
        ElseIf e.Item.Cells(5).Text = "Pending" Then
            e.Item.FindControl("lbtnDelete").Visible = True
          
        End If

       

    End Sub

    Protected Sub laDataGrid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles laDataGrid.SelectedIndexChanged

    End Sub

    Protected Sub laDataGrid_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles laDataGrid.DeleteCommand
        Dim leaveId = e.Item.Cells(0).Text
        'Response.Write(leaveId)
        'Response.End()
        Dim strSQL As String
        conn.Close()
        strSQL = "DELETE FROM empLeaveDetails where empLeaveId=" & leaveId
        conn.Open()
        objcmd = New SqlCommand(strSQL, conn)
        objcmd.ExecuteNonQuery()
        objcmd.Dispose()
        conn.Close()
        BindDataGrid()
    End Sub
End Class
