Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class admin_timeSheetReport
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objCmd As New SqlCommand
    Dim dareport As SqlDataAdapter
    Dim dsreport As DataSet
    Dim PrjID As Integer
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        'spnhide.Visible = True
        lblCurrentdate.Text = DateTime.Now.ToString("dd, MMM, yyyy")

        If CInt(Request.QueryString("id")) > 0 Then
            PrjID = CInt(Request.QueryString("id"))
            If Not Page.IsPostBack Then
                load_Date(PrjID)
                fillProjectmem(PrjID)
                fillprojectname(PrjID)
                fillHours(PrjID)
                
            End If
        Else


            ' Response.Write("Access Denied")
            'Response.End()

        End If
    End Sub

    Public Sub load_Date(ByRef PrjID)

        Dim strSQL As String
        Dim i As Integer
        Dim strConcat As String
        strConcat = txthdnFrom.Text
        If strConcat = "" Then
            ' strSQL = "select tblTemp.totalHours , projectModuleMaster.moduleName as moduleName ,projectModuleMaster.moduleid from (select sum(projectTimeSheet.tsHour) as totalHours ,projectModuleMaster.moduleid as  moduleID from    dbo.projectModuleMaster left outer Join  projectTimeSheet on  projectTimeSheet.moduleid = projectModuleMaster.moduleid   group by projectModuleMaster.moduleid ) as  tblTemp, projectModuleMaster where  projectModuleMaster.moduleId = tblTemp.moduleID and  projectModuleMaster.moduleid = projectModuleMaster.moduleRefID and  projectModuleMaster.ProjId=" & PrjID
            strSQL = "exec cnthourload " & PrjID & ""

        Else
            ' strSQL = "select tblTemp.totalHours , projectModuleMaster.moduleName as moduleName,projectModuleMaster.moduleid  " & _
            '       "from (select sum(projectTimeSheet.tsHour) as totalHours ,projectModuleMaster.moduleid as moduleID  " & _
            '      "from dbo.projectModuleMaster left outer Join projectTimeSheet on projectTimeSheet.moduleid = projectModuleMaster.moduleid  " & txthdnFrom.Text & "  " & _
            '      "group by projectModuleMaster.moduleid ) as tblTemp, projectModuleMaster where projectModuleMaster.moduleId = tblTemp.moduleID  " & _
            '     "and projectModuleMaster.moduleid = projectModuleMaster.moduleRefID and projectModuleMaster.ProjId=" & PrjID

            strSQL = "exec cnthour " & PrjID & ",' " & txtDateFrom.Text & " ','" & txtDateTo.Text & "'"

        End If


        
        'objCmd = New SqlCommand(strSQL, strConn)
        'Response.Write(strSQL)
        'Response.End()

        dareport = New SqlDataAdapter(strSQL, strConn)
        dsreport = New DataSet()
        dareport.Fill(dsreport)
        For i = 0 To tblReport.Rows.Count - 1
            tblReport.Rows(1).Dispose()
        Next
        For i = 0 To dsreport.Tables(0).Rows.Count - 1
            Dim tmpRow As New TableRow
            Dim tmpRow1 As New TableRow
            Dim tmpCell1 As New TableCell
            Dim tmpCell2 As New TableCell
            Dim tmpCell3 As New TableCell
            Dim tmpCell4 As New TableCell
            Dim tmpcell5 As New TableCell
            Dim tmpCellDummy As New TableCell
            Dim tmpCellDummy1 As New TableCell
            tmpCell1.Width = Unit.Pixel(10)

            tmpCell2.Width = Unit.Percentage(80)
            tmpCell3.Width = Unit.Percentage(30)
            'response.write(dsreport.Tables(0).Rows(i).Item(0).ToString)
            If Trim(dsreport.Tables(0).Rows(i).Item(0).ToString) = "0" Or Trim(dsreport.Tables(0).Rows(i).Item(0).ToString) = "" Then

                'tmpCell1.Text = dsreport.Tables(0).Rows(i).Item(1).ToString
                tmpCell2.Text = dsreport.Tables(0).Rows(i).Item(1).ToString

            Else
                tmpCell1.Text = "<a href=""#."" onclick=""call_Ajax('" & dsreport.Tables(0).Rows(i).Item(3).ToString & "',1)""><img id='plus" & dsreport.Tables(0).Rows(i).Item(3).ToString & "' name='plus" & dsreport.Tables(0).Rows(i).Item(3).ToString & "' border=""0"" src=""../images/plus.gif""</a>"
                tmpCell2.Text = "<a href=""#."" style=""text-decoration: none"" onclick=""call_Ajax('" & dsreport.Tables(0).Rows(i).Item(3).ToString & "',1)"">" & dsreport.Tables(0).Rows(i).Item(1).ToString & "</a>"
                ' tmpcell5.Text = "<a href=""#."" style=""text-decoration: none"" onclick=""call_Ajax('" & dsreport.Tables(0).Rows(i).Item(3).ToString & "',1)"">" & dsreport.Tables(0).Rows(i).Item(1).ToString & "</a>"
            End If

            '  tmpCell2.Text = dsreport.Tables(0).Rows(i).Item(1).ToString
            tmpCell3.Text = dsreport.Tables(0).Rows(i).Item(0).ToString
            tmpcell5.Text = dsreport.Tables(0).Rows(i).Item(2).ToString
            tmpCell3.HorizontalAlign = HorizontalAlign.Right
            tmpRow.Cells.Add(tmpCell1)
            tmpRow.ForeColor = Drawing.Color.Black
            ' tmpRow.BackColor = Drawing.Color.FromName("#edf2e6")
            tmpRow.Font.Name = "Verdana"
            tmpRow.Font.Size = "11"
            tmpRow.Font.Bold = "true"
            tmpRow.Cells.Add(tmpCell2)
            tmpRow.Cells.Add(tmpcell5)
            tmpRow.Cells.Add(tmpCell3)
            tmpCell4.ColumnSpan = 4
            tmpCell4.Text = "<div id='module_" & dsreport.Tables(0).Rows(i).Item(3).ToString & "' name='module_" & dsreport.Tables(0).Rows(i).Item(3).ToString & "'></div>"
            tmpRow1.Cells.Add(tmpCell4)
            tblReport.Rows.Add(tmpRow)
            tblReport.Rows.Add(tmpRow1)

        Next
        objCmd.Dispose()


    End Sub
    Public Sub fillProjectmem(ByRef PrjID)

        Dim strsql As String
        Dim cmdmember As SqlCommand

        Dim dtr As SqlDataReader
        'strConn.Close()
        strsql = "select empname from employeemaster where empid in(select empid from projectMember where projId=" & PrjID & ")"

        cmdmember = New SqlCommand(strsql, strConn)
        strConn.Open()
        dtr = cmdmember.ExecuteReader
        dgrdProjMember.DataSource = dtr
        dgrdProjMember.DataBind()
        dtr.Close()
        strConn.Close()
    End Sub
    Protected Sub btnGO_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGO.ServerClick

        'txthdnFrom.Text = " and  projectTimeSheet.tsEntryDate  between '" & txtDateFrom.Text & "' and '" & txtDateTo.Text & "'"
        txthdnFrom.Text = " and datediff(D,'" & txtDateFrom.Text & "',tsDate) >= 0 and datediff(D,'" & txtDateTo.Text & "',tsDate) <= 0 "
        load_Date(PrjID)
        LoadHours(PrjID)
        fillprojectname(PrjID)
        ' spnhide.Visible = False
    End Sub

    Sub fillprojectname(ByRef PrjID)
        Dim drprojname As SqlDataReader
        Dim cmd As SqlCommand
        strConn.Open()
        cmd = New SqlCommand("select projname from projectmaster where projid=" & PrjID, strConn)

        drprojname = cmd.ExecuteReader()
        While drprojname.Read()
            lblproject.Text = drprojname("projname")
            lbldivhide.Text = drprojname("projname")
        End While
        strConn.Close()
    End Sub

    Public Sub fillHours(ByVal PrjID)
        Dim strSqlHours As String
        Dim dtrhours As SqlDataReader
        Dim cmdhours As New SqlCommand
        Dim lblMonth As String = Left(MonthName(Month(Now())), 3) & " " & Year(Now())

        strSqlHours = ""
        strSqlHours = "SELECT sum(tshour) as totalHours FROM projectTimeSheet AS pt,projectModuleMaster AS pm,projectMaster AS prjmaster " & _
                      "WHERE pt.moduleId IN(SELECT moduleId FROM projectModuleMaster WHERE projId=" & PrjID & " AND  " & _
                      " pt.moduleId=pm.moduleId AND pm.projId=prjmaster.projId ) "

        'Response.Write(strSqlHours)
        'Response.End()

        cmdhours = New SqlCommand(strSqlHours, strConn)
        strConn.Open()
        dtrhours = cmdhours.ExecuteReader()
        While dtrhours.Read()
            If IsDBNull(dtrhours("totalHours")) Then
                lbltotalhours.Text = "Total Hours 0"
            Else
                lbltotalhours.Text = "Total Hours " & dtrhours("totalHours")
            End If
        End While

       
        strConn.Close()
    End Sub

    Public Sub LoadHours(ByVal prjid)
        Dim strSqlloadH As String
        Dim dtrLoadh As SqlDataReader
        Dim cmdLoadh As New SqlCommand
        strSqlloadH = ""
        strSqlloadH = "SELECT sum(tshour) as totalHours FROM projectTimeSheet AS pt,projectModuleMaster AS pm,projectMaster AS prjmaster " & _
                      "WHERE pt.moduleId IN(SELECT moduleId FROM projectModuleMaster WHERE projId=" & prjid & " " & _
                      "AND pt.moduleId=pm.moduleId AND pm.projId=prjmaster.projId ) " & txthdnFrom.Text
        'Response.Write(strSqlloadH)
        'Response.End()
        cmdLoadh = New SqlCommand(strSqlloadH, strConn)
        strConn.Open()
        dtrLoadh = cmdLoadh.ExecuteReader()

        While dtrLoadh.Read()
            If IsDBNull(dtrLoadh("totalHours")) Then
                lbltotalhours.Text = "Total Hours 0"
            Else
                lbltotalhours.Text = "Total Hours " & dtrLoadh("totalHours")
            End If

        End While
        strConn.Close()
        
    End Sub
End Class
