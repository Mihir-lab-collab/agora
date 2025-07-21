Imports System.Data
Imports System.Data.SqlClient


Partial Class emp_candShowSchedule
    Inherits Authentication
    Dim dsn As String
    Dim strConn As SqlConnection
    Dim strsqlsechdule As String
    Dim dashow As SqlDataAdapter
    Dim dsshow As New DataSet
    Dim gf As New generalFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       ' Button1.Attributes.Add("onclick", "javascript:blankBlank();return true; ")
        gf.checkEmpLogin()
        dsn = ConfigurationManager.ConnectionStrings("conString").ToString()
        strConn = New System.Data.SqlClient.SqlConnection(dsn)
        lnkResume.Attributes.Add("onclick", "javascript:window.open('../emp/candResume.aspx?id=" & Request.QueryString("id") & "','null','scrollbars=yes,toolbar=no,menubar=no,location=right,resizable=no,width=700,height=550,left=50,top=10');return false;")
        If Not IsPostBack Then
            showdetails()
            candScheduleGrid()
            fillStatus()
        End If
        'strsqlsechdule = "select (select statusDesc from candStatusMaster where statusId=candSchDetail.statusId) as candstatus , " & _
        '                 "(select candFName + ' ' + candMName + ' ' candLName from candidateMaster where candId= " & Request.QueryString("id") & " ) as candname " & _
        '                 ",convert(varchar,candidateMaster.canddate,7) as candintdate,candSchDetail.schComment as comment from " & _
        '                 "candSchDetail,candidateMaster where candidateMaster.candid=candSchDetail.candid "

        ' txtcomment.ReadOnly = True

    End Sub

    Sub showdetails()
        strsqlsechdule = "select (candFName + ' ' + candMName + ' ' + candLName )as candname,convert(varchar,candidateMaster.canddate,7) " & _
                             "as candintdate,(select skillDesc from skillMaster where skillid=candidateMaster.candpost )as post,convert(varchar,(CONVERT(int,candexp/12.0)))+ '.' + convert(varchar, " & _
                      "(candexp - CONVERT(int,candexp/12.0) * 12) ) as totexp,* from candidateMaster " & _
                             " where candId=" & Request.QueryString("id")
        dashow = New SqlDataAdapter(strsqlsechdule, strConn)
        dashow.Fill(dsshow)

        lblName.Text = dsshow.Tables(0).Rows(0).Item("candname")
        Page.Title = "Candidate information : " + lblName.Text
        candMobile.Text = dsshow.Tables(0).Rows(0).Item("candmobileno")
        caneRelative.Text = dsshow.Tables(0).Rows(0).Item("candtelno")
        lblDate.Text = dsshow.Tables(0).Rows(0).Item("candbirthdate")
        lblPost.Text = dsshow.Tables(0).Rows(0).Item("post")
        lblCurrSal.Text = dsshow.Tables(0).Rows(0).Item("candCurrSalary")
        lblExpectedsal.Text = dsshow.Tables(0).Rows(0).Item("candExpSalary")
        lblPrevious.Text = dsshow.Tables(0).Rows(0).Item("candpreviemp")
        lblReason.Text = vbCrLf & dsshow.Tables(0).Rows(0).Item("candReasonChange")
        lblPermAdd.Text = vbCrLf & dsshow.Tables(0).Rows(0).Item("candpermaddress")
        lblExp.Text = dsshow.Tables(0).Rows(0).Item("totexp")
        '  lblexp.Text=
    End Sub

    Sub candScheduleGrid()
        Dim strSqlgrid As String
        strSqlgrid = "select schId,candId,convert(varchar(50),schDate,7) as schDate,(select statusDesc from candStatusMaster where statusid=candSchDetail.statusId) " & _
                    "as candStstus,schComment from candSchDetail where candid=" & Request.QueryString("id")
        dashow = New SqlDataAdapter(strSqlgrid, strConn)
        dsshow = New DataSet
        dashow.Fill(dsshow)
        ' Response.Write(dsshow.Tables(0).Rows.Count)
        If dsshow.Tables(0).Rows.Count > 0 Then
            gridSchedule.DataSource = dsshow
            gridSchedule.DataBind()
        Else
            ' Response.Write("No Record Found")
        End If
    End Sub

    Protected Sub gridSchedule_RowDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridSchedule.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(3).Text = vbCrLf & e.Row.Cells(3).Text
        End If
    End Sub

    Sub fillStatus()
        Dim strsqlpost As String
        Dim rdrlista As SqlDataReader
        strsqlpost = "select * from  dbo.candStatusMaster"
        Dim Cmdststus As New SqlCommand(strsqlpost, strConn)
        strConn.Open()
        rdrlista = Cmdststus.ExecuteReader()
        candStatus.DataSource = rdrlista
        candStatus.DataTextField = "statusDesc"
        candStatus.DataValueField = "statusId"
        candStatus.DataBind()
        rdrlista.Close()
        strConn.Close()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim strsqlDetails As String
        'Response.End()
        Dim cid As Integer
        cid = Request.QueryString("id")
        strsqlDetails = "insert into candSchDetail (candId,statusId,schDate,schComment) values(" & Request.QueryString("id") & "," & candStatus.Value & ",'" & txtschdate.Text & "','" & txtAddComment.Text & "')"
        strConn.Open()
        Dim cmdinsert As New SqlCommand(strsqlDetails, strConn)
        cmdinsert.ExecuteNonQuery()
        strConn.Close()

        Response.Write("<script language='javascript'>{window.location.href='/emp/candShowSchedule.aspx?id=" & cid & "'; } </" & "script>")
    End Sub

    Protected Sub lnkResume_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkResume.Click
        '  lnkResume.Attributes.Add("onclick", "javascript:window.open('../emp/candResume.aspx?id=" & Request.QueryString("id") & "','null','scrollbars=yes,toolbar=no,menubar=no,location=right,resizable=no,width=700,height=550,left=50,top=10');return false;")
    End Sub
End Class
