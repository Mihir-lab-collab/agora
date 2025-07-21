Imports System.Data
Imports System.Data.SqlClient
Partial Class emp_candSchSearch
    Inherits Authentication
    Dim dsn As String
    Dim strconn As SqlConnection
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        dsn = ConfigurationManager.ConnectionStrings("conString").ToString()
        strconn = New System.Data.SqlClient.SqlConnection(dsn)

    End Sub

    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Dim dsdetil As New DataSet
        Dim strsqldet As String = "select (candidateMaster.candFname + ' ' + candidateMaster.candMName + ' ' +  candidateMaster.candLName)" & _
                                  "as ccname,(select statusDesc from candStatusMaster " & _
                                  "where statusId=candSchDetail.statusId) as ccstatus,convert(varchar,candSchDetail.schdate,106) " & _
                                  "as ccdate ,candidateMaster.candId as ccid,(select skilldesc from skillMaster where  " & _
                                  "skillId=candidateMaster.candpost) as ccpost from candSchDetail,candidateMaster where  " & _
                                  "candSchDetail.schdate='" & txtdate.Text & "' and " & _
                                  "candidateMaster.candId=candSchDetail.candId"
     
        Dim dadet As SqlDataAdapter
        dadet = New SqlDataAdapter(strsqldet, strconn)
        dadet.Fill(dsdetil)
        gridShowCandiate.DataSource = dsdetil
        gridShowCandiate.DataBind()
    End Sub
End Class
