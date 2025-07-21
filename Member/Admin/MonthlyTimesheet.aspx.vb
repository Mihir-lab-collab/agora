Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Partial Class Admin_MonthlyTimesheet
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objDataReader As SqlDataReader
    Dim objCmd As SqlCommand
    Dim DataAdapter As SqlDataAdapter
    Dim gridDataset As DataSet
    Dim Dtddl As DataTable
    Dim strSQL As String
    Dim empId As Integer
    Dim Month As Integer
    Dim Year As Integer
    Dim arg As String
    Dim strOrderBy As String
    Dim gf As New generalFunction
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        Try

            If Not IsPostBack Then
                empId = Request.QueryString("Empid")
                ViewState("Empid") = empId
                Month = Request.QueryString("Month")
                ViewState("Month") = Month
                Year = Request.QueryString("Year")
                ViewState("Year") = Year
                BindmonthlyReport()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Sub BindmonthlyReport()
        strConn.Close()

        strSQL = "SELECT projectTimeSheet.tsDate, projectTimeSheet.tsHour, projectTimeSheet.tsComment, projectTimeSheet.tsEntryDate, projectModuleMaster.moduleName, " & _
                    " projectMaster.projName FROM  projectMaster INNER JOIN projectModuleMaster ON projectMaster.projId = projectModuleMaster.projId INNER JOIN " & _
                    " projectTimeSheet ON projectModuleMaster.moduleId = projectTimeSheet.moduleId " & _
                    "where projectTimeSheet.empid = '" & Convert.ToInt32(ViewState("Empid")) & "' And Month(tsdate) = '" & Convert.ToInt32(ViewState("Month")) & "' And Year(tsDate) ='" & Convert.ToInt32(ViewState("Year")) & "' order by projectTimeSheet.tsDate desc"
        strConn.Open()
        objCmd = New SqlCommand(strSQL, strConn)
        DataAdapter = New SqlDataAdapter(strSQL, strConn)
        gridDataset = New DataSet()
        DataAdapter.Fill(gridDataset, "temp")
        Dim dtTimeSheet As DataTable = gridDataset.Tables("temp")
        If gridDataset.Tables("temp").Rows.Count = 0 Then
            dgMonthTimeSheet.ShowFooter = False
            lblInfoMsg.Visible = True
        Else
            lblInfoMsg.Visible = False
            dgMonthTimeSheet.ShowFooter = True
            Dim dv As New DataView(dtTimeSheet)
            dv.Sort = strOrderBy



            dgMonthTimeSheet.DataSource = dv
            dgMonthTimeSheet.DataBind()

            Dim d As Integer
            Dim thrs As Integer
            thrs = 0
            Dim t As String
            t = ""
            Dim l As Label
            For d = 0 To dgMonthTimeSheet.Items.Count - 1
                l = dgMonthTimeSheet.Items(d).Cells(2).FindControl("lblhours")
                t = l.Text.ToString() & "  " & t
                thrs = thrs + CInt(l.Text.ToString())
            Next
            lblhourstotal.Text = ""
            lblhourstotal.Text = thrs.ToString()
           
            End If



            strConn.Close()

            objCmd.Dispose()


    End Sub

   
    Protected Sub dgMonthTimeSheet_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgMonthTimeSheet.SortCommand
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
        BindmonthlyReport()
    End Sub

    
End Class
