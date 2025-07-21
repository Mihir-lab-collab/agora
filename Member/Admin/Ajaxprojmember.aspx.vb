Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class admin_Ajaxprojmember
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objCmd As New SqlCommand
    Dim dareport As SqlDataAdapter
    Dim dsreport As DataSet
    Dim PrjID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim moduleID As Integer
        moduleID = Request.QueryString("ID")
        Response.Write(moduleID)
        Response.End()
        fillmember(moduleID)
    End Sub
    Public Sub fillmember(ByVal moduleId As String)
        Dim strSQL As String
        Dim i As Integer
        strSQL = "select sum(projectTimeSheet.tsHour) as hours,employeeMaster.empname from projectTimeSheet,employeeMaster where moduleID='" & moduleId & "' and projectTimeSheet.empid=employeeMaster.empid and projectTimeSheet.tsEntryDate between '2007-09-01' and '2007-09-15' group by employeeMaster.empname,projectTimeSheet.empid"
        objCmd = New SqlCommand(strSQL, strConn)
        dareport = New SqlDataAdapter(strSQL, strConn)
        dsreport = New DataSet()
        dareport.Fill(dsreport)
        For i = 0 To dsreport.Tables(0).Rows.Count - 1
            Dim tmpRow As New TableRow
            Dim tmpCell1 As New TableCell
            Dim tmpCell2 As New TableCell
            Dim tmpCell3 As New TableCell
            tmpCell1.Text = ""
            tmpCell2.Text = dsreport.Tables(0).Rows(i).Item(1).ToString
            tmpCell3.Text = dsreport.Tables(0).Rows(i).Item(0).ToString
            tmpRow.Cells.Add(tmpCell1)
            tmpRow.ForeColor = Drawing.Color.Black
            tmpRow.BackColor = Drawing.Color.FromName("#edf2e6")
            tmpRow.Font.Name = "Verdana"
            tmpRow.Font.Size = "10"
            tmpRow.Cells.Add(tmpCell2)
            tmpRow.Cells.Add(tmpCell3)
            tblReportmember.Rows.Add(tmpRow)


        Next
    End Sub
End Class
