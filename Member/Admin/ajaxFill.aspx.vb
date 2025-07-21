Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class admin_ajaxFill
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
        Dim ModuleID As Integer
        ModuleID = Request.QueryString("ID")
        PrjID = Request.QueryString("prjID")
        If Request.QueryString("module") = 1 Then
            load_Data(ModuleID, PrjID)
        Else
            load_Data_employee(ModuleID, PrjID)
        End If
    End Sub
    Public Sub load_Data(ByRef ModuleID As String, ByVal PrjID As String)
        Dim strSQL As String
        Dim i As Integer
        Dim sWhere As String
        sWhere = ""
        If Trim(Request.QueryString("condition")) <> "" Then
            sWhere = Trim(Request.QueryString("condition"))
        End If
        strSQL = "select tblTemp.totalHours,projectModuleMaster.moduleName  as  subModuleName ,projectModuleMaster.moduleid from  projectModuleMaster left outer join  (select sum(projectTimeSheet.tsHour) as totalHours ,projectModuleMaster.moduleid as  moduleID from dbo.projectModuleMaster,projectTimeSheet where  projectTimeSheet.moduleid = projectModuleMaster.moduleid " & sWhere & " group by projectModuleMaster.moduleid ) as  tblTemp on  projectModuleMaster.moduleId = tblTemp.moduleID where   projectModuleMaster.moduleid <> projectModuleMaster.moduleRefID and  projectModuleMaster.ProjId =  " & PrjID & " and  projectModuleMaster.moduleRefID = " & ModuleID

        objCmd = New SqlCommand(strSQL, strConn)
        dareport = New SqlDataAdapter(strSQL, strConn)
        dsreport = New DataSet()
        dareport.Fill(dsreport)

        For i = 0 To dsreport.Tables(0).Rows.Count - 1
            Dim tmpRow As New TableRow
            Dim tmpRow1 As New TableRow
            Dim tmpCell1 As New TableCell
            Dim tmpCell2 As New TableCell
            Dim tmpCell3 As New TableCell
            Dim tmpCell4 As New TableCell
            Dim tmpCell5 As New TableCell

            Dim tmpCellDummy As New TableCell
            Dim tmpCellDummy1 As New TableCell
            tmpCell1.Width = Unit.Percentage(5)
            tmpCell2.Width = Unit.Percentage(95)

            tmpRow.Font.Name = "Verdana"
            tmpRow.Font.Size = "10"
            tmpRow.Font.Bold = "true"
            tmpCell1.HorizontalAlign = HorizontalAlign.Right
            tmpCell3.Width = Unit.Percentage(10)
            tmpCell3.HorizontalAlign = HorizontalAlign.Left

            If Trim(dsreport.Tables(0).Rows(i).Item(0).ToString) = "" Or Trim(dsreport.Tables(0).Rows(i).Item(0).ToString) = "0" Then
                tmpCell2.Text = dsreport.Tables(0).Rows(i).Item(1).ToString
            Else
                tmpCell1.Text = "<a href=""#.""  style=""text-decoration: none"" onclick=""call_Ajax('" & dsreport.Tables(0).Rows(i).Item(2).ToString & "','2')""><img id='plus" & dsreport.Tables(0).Rows(i).Item(2).ToString & "' name='plus" & dsreport.Tables(0).Rows(i).Item(2).ToString & "'  border=""0"" src=""../images/plus.gif"" /></a>"
                tmpCell2.Text = "<a href=""#."" style=""text-decoration: none"" onclick=""call_Ajax('" & dsreport.Tables(0).Rows(i).Item(2).ToString & "','2')"">" & dsreport.Tables(0).Rows(i).Item(1).ToString & "</a>"
            End If
            tmpCell3.Text = dsreport.Tables(0).Rows(i).Item(0).ToString
            tmpCell3.HorizontalAlign = HorizontalAlign.Right
            tmpCell5.ColumnSpan = 3
            tmpCell5.Text = "<div id= 'submodule_" & dsreport.Tables(0).Rows(i).Item(2).ToString & "'></div>"
            tmpRow1.Cells.Add(tmpCell5)
            tmpRow.Cells.Add(tmpCell1)
            tmpRow.ForeColor = Drawing.Color.Black
            tmpRow.Font.Name = "Verdana"
            tmpRow.Font.Size = "10"
            tmpRow.Cells.Add(tmpCell2)
            tmpRow.Cells.Add(tmpCell3)
            tblReport1.Rows.Add(tmpRow)
            tblReport1.Rows.Add(tmpRow1)
        Next
        If dsreport.Tables(0).Rows.Count <= 0 Then
            load_Data_mailemployee(ModuleID, PrjID)
        End If
    End Sub
    Public Sub load_Data_employee(ByRef ModuleID, ByVal PrjID)
        Dim strSQL As String
        Dim i As Integer
        Dim sWhere As String
        sWhere = ""
        If Trim(Request.QueryString("condition")) <> "" Then
            sWhere = Trim(Request.QueryString("condition"))
        End If
        strSQL = " select sum(tblTemp.totalHours),employeeMaster.empName  from  (  " & _
                    " select sum(projectTimeSheet.tsHour) as totalHours ,ProjectTimeSheet.empID,  " & _
                    " projectModuleMaster.moduleid as  moduleID from dbo.projectModuleMaster,projectTimeSheet where    " & _
                    " projectTimeSheet.moduleid = projectModuleMaster.moduleid " & sWhere & " group by projectTimeSheet.empID ,projectModuleMaster.moduleid )   " & _
                    " as  tblTemp,projectModuleMaster ,employeeMaster where projectModuleMaster.moduleId = tblTemp.moduleID and   " & _
                    " projectModuleMaster.moduleid <> projectModuleMaster.moduleRefID And employeeMaster.empid = tblTemp.empid And " & _
                    " projectModuleMaster.ProjId = " & PrjID & " and  projectModuleMaster.moduleID = " & ModuleID & " group by tblTemp.empID,employeeMaster.empName  "

        dareport = New SqlDataAdapter(strSQL, strConn)
        dsreport = New DataSet()
        dareport.Fill(dsreport)

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
            tmpCell1.Width = Unit.Percentage(5)
            tmpCell1.HorizontalAlign = HorizontalAlign.Right
            tmpCell3.Width = Unit.Percentage(10)
            tmpCell2.Width = Unit.Percentage(80)
            tmpcell5.ColumnSpan = 3
            tmpCell2.Text = dsreport.Tables(0).Rows(i).Item(1).ToString
            tmpCell3.Text = ""
            tmpCell4.Text = dsreport.Tables(0).Rows(i).Item(0).ToString
            tmpCell4.HorizontalAlign = HorizontalAlign.Right
            tmpRow.Cells.Add(tmpCell1)
            tmpRow.ForeColor = Drawing.Color.Black
            tmpRow.Font.Name = "Verdana"
            tmpRow.Font.Size = "10"
            tmpRow.Cells.Add(tmpCell2)
            tmpRow.Cells.Add(tmpCell4)
            tblReport1.Rows.Add(tmpRow)
        Next
    End Sub
    Public Sub load_Data_mailemployee(ByRef ModuleID As String, ByVal PrjID As String)
        Dim strSQL As String
        Dim i As Integer
        Dim sWhere As String
        sWhere = ""
        If Trim(Request.QueryString("condition")) <> "" Then
            sWhere = Trim(Request.QueryString("condition"))
        End If
        strSQL = " select tblTemp.totalHours ,employeemaster.empName  from " & _
                " (  " & _
                " select sum(projectTimeSheet.tsHour) as totalHours ,count(projectTimeSheet.empid) as tmp ,projectModuleMaster.moduleid as  moduleID ,projectTimeSheet.empid " & _
                " from " & _
                " dbo.projectModuleMaster left outer Join  projectTimeSheet on  projectTimeSheet.moduleid = projectModuleMaster.moduleid " & sWhere & " " & _
                " group by projectTimeSheet.empid,projectModuleMaster.moduleid " & _
                " ) as  tblTemp, projectModuleMaster,employeemaster where  projectModuleMaster.moduleId = " & _
                " tblTemp.moduleID And projectModuleMaster.moduleid = projectModuleMaster.moduleRefID And " & _
                " projectModuleMaster.ProjId =" & PrjID & " And projectModuleMaster.moduleid = " & ModuleID & " And employeemaster.empid = tblTemp.empid "
        dareport = New SqlDataAdapter(strSQL, strConn)
        dsreport = New DataSet()
        dareport.Fill(dsreport)

        For i = 0 To dsreport.Tables(0).Rows.Count - 1
            Dim tmpRow As New TableRow
            Dim tmpRow1 As New TableRow
            Dim tmpCell1 As New TableCell
            Dim tmpCell2 As New TableCell
            Dim tmpCell3 As New TableCell
            Dim tmpCell4 As New TableCell


            Dim tmpCellDummy As New TableCell
            Dim tmpCellDummy1 As New TableCell
            'tmpCell1.Width = Unit.Percentage(5)
            tmpCell1.HorizontalAlign = HorizontalAlign.Right
            'tmpCell2.Width = Unit.Percentage(80)

            tmpCell1.Text = ""
            tmpCell2.Text = dsreport.Tables(0).Rows(i).Item(1).ToString
            tmpCell3.Text = dsreport.Tables(0).Rows(i).Item(0).ToString


            tmpRow.Cells.Add(tmpCell1)
            tmpCell3.HorizontalAlign = HorizontalAlign.Right
            tmpRow.ForeColor = Drawing.Color.Black
            tmpRow.Font.Name = "Verdana"
            tmpRow.Font.Size = "10"
            tmpRow.Cells.Add(tmpCell2)

            tmpRow.Cells.Add(tmpCell3)

            tblReport1.Rows.Add(tmpRow)
        Next
    End Sub
End Class
