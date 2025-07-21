Imports System.Data
Imports System.Data.SqlClient
Partial Class emp_showCandiate
    Inherits Authentication
    Dim dsn As String
    Dim strConn As SqlConnection
    Dim dsdetails As New DataSet
    Dim dadetails As SqlDataAdapter
    Dim strsqldetail As String
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        dsn = System.Configuration.ConfigurationManager.ConnectionStrings("conString").ToString()
        strConn = New System.Data.SqlClient.SqlConnection(dsn)
        If Not IsPostBack Then
            ViewState("sortExpr") = "datecand"
            SortDirection = "ASC"
            bindGridView()
            loaddropdown()
	        loadSkilldropdown()
        End If

    End Sub
	Private Property SortDirection() As String
        Get
            If (ViewState("SortDirection") Is Nothing) Then ViewState("SortDirection") = String.Empty
            Return ViewState("SortDirection").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortDirection") = value
        End Set
    End Property

    Private Sub loaddropdown()
        Dim strSql As String
        Dim dtTmp As New DataTable()
        strSql = "select * from dbo.candStatusMaster"
        dadetails = New SqlDataAdapter(strSql, strConn)
        dadetails.Fill(dtTmp)
        drpStatus.DataSource = dtTmp
        drpStatus.DataValueField = "statusId"
        drpStatus.DataTextField = "statusDesc"
        drpStatus.DataBind()

        dtTmp.Dispose()
       
    End Sub
    Private Sub loadSkilldropdown()
        Dim strSql As String
        Dim dtTmp As New DataTable()
        strSql = "select * from dbo.skillmaster"

        dadetails = New SqlDataAdapter(strSql, strConn)
        dadetails.Fill(dtTmp)
        drpJobType.DataSource = dtTmp
        drpJobType.DataValueField = "skillId"
        drpJobType.DataTextField = "skillDesc"
        drpJobType.DataBind()
        dtTmp.Dispose()

    End Sub
    Private Sub  bindGridView()
        strsqldetail = "select (candFName  + ' ' + candMName + ' ' +  candLName) as candname ,convert(varchar,canddate,7) as canddate," & _
                       "isnull((select top 1 schdate from candSchDetail where Candid = candidateMaster.candId order by schId desc),convert(varchar,canddate,7)) as datecand ," & _
                       "convert(varchar,candbirthdate,7) as birthdate,(select skilldesc from  skillmaster where skillid=candidateMaster.candpost) " & _
                       "as skill,convert(varchar,(CONVERT(int,candexp/12.0)))+ '.' + convert(varchar,  " & _
                       "(candexp - CONVERT(int,candexp/12.0) * 12) ) as totexp, " & _
                       " isnull((select StatusDesc from candStatusMaster where statusID in (select top 1 statusID from candSchDetail where Candid = candidateMaster.candId order by schdate desc)),'') as candstatus, " & _
                       " isnull((select top 1 schComment from candSchDetail where Candid = candidateMaster.candId order by schdate desc),'') as schComment ," & _
                       " * from candidateMaster  where 1=1 "
        'where  canddate='" & txtdate.Text & "'  " & _
        '" or  candFName like '%" & txtName.Text & "' or candLName like '%" & txtName.Text & "' "
        'convert(varchar,canddate,7) as canddate,
        If (txtdate.Text <> "") Then
            strsqldetail = strsqldetail & " and canddate='" & txtdate.Text & "'"
        End If
        If (txtName.Text <> "") Then
            strsqldetail = strsqldetail & " and ( candfname like '%" & txtName.Text & "' or candLName like '%" & txtName.Text & "' ) "
        End If
        If (drpStatus.SelectedValue <> "") Then
            strsqldetail = strsqldetail & " and isnull((select Top 1 StatusID from dbo.candSchDetail where candid = candidateMaster.Candid order by schID desc),0) = '" & drpStatus.SelectedValue & "'"
        End If
        If (drpJobType.SelectedValue <> "") Then
            strsqldetail = strsqldetail & " and candidateMaster.candpost = '" & drpJobType.SelectedValue & "'"
        End If

		Dim  sql as string 

	
        If (SortDirection.ToUpper().IndexOf("ASC") <> -1) Then
            sql = strsqldetail + " order by " + ViewState("sortExpr") + " DESC"
            SortDirection = "DESC"
        Else
            sql = strsqldetail + " order by " + ViewState("sortExpr") + " ASC"
            SortDirection = "ASC"
        End If
 	
        dadetails = New SqlDataAdapter(sql, strConn)
        dadetails.Fill(dsdetails)
        gridShowCandiate.DataSource = dsdetails
        gridShowCandiate.DataBind()
    End Sub
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("candiadateDetails.aspx")
    End Sub

    Public Function getSkills(ByVal strID As String) As String
        Dim Rdrddl As SqlDataReader
        Dim sql As String = "select techSkills.skillDesc from techSkills,candskill where  " & _
                               "techSkills.techId=candskill.skillid and " & _
                               "candskill.candID=" & strID
        Dim Cmdddl As New SqlCommand(sql, strConn)
        strConn.Open()
        Rdrddl = Cmdddl.ExecuteReader()
        Dim st As String
        Dim i As Integer
        st = ""
        While Rdrddl.Read
            If st = "" Then
                st = "- " & Rdrddl(0)
            Else
                st = st & "<BR>- " & Rdrddl(0)
            End If
            i = i + 1
        End While
        getSkills = st
        Rdrddl.Close()
        strConn.Close()
    End Function

    Protected Sub gridShowCandiate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridShowCandiate.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then
           
            Dim btnResum As New LinkButton
            btnResum = e.Row.Cells(1).FindControl("btnResume")
            btnResum.Attributes.Add("onclick", "javascript:window.open('../emp/candResume.aspx?id=" & e.Row.Cells(0).Text & "','null','scrollbars=yes,toolbar=no,menubar=no,location=right,resizable=no,width=700,height=550,left=50,top=10');return false;")
            btnResum.Attributes.Add("onclick", "javascript:window.open('../emp/candResume.aspx?id=" & e.Row.Cells(0).Text & "','null','scrollbars=yes,toolbar=no,menubar=no,location=right,resizable=no,width=700,height=550,left=50,top=10');return false;")


            'Dim btnSchedule As New Button
            'btnSchedule = e.Row.Cells(1).FindControl("btnSchedule")
            'btnSchedule.Attributes.Add("onclick", "javascript:window.open('../emp/candSchedule.aspx?id=" & e.Row.Cells(0).Text & "','null','scrollbars=yes,toolbar=no,menubar=no,location=right,resizable=no,width=700,height=550,left=50,top=10');return false;")

            'Dim btnshowAs As New Button
            'btnshowAs = e.Row.Cells(16).FindControl("btnshow")
            'btnshowAs.Attributes.Add("onclick", "javascript:window.location.href='../emp/candShowSchedule.aspx?id=" & e.Row.Cells(0).Text & "','null','scrollbars=yes,toolbar=no,menubar=no,location=right,resizable=no,width=700,height=550,left=50,top=10';return false;")
            'Dim btnDlt As New Button
            'btnDlt = CType(e.Row.Cells(18).FindControl("btndelete"), Button)
            'btnDlt.Attributes.Add("onclick", "return confirm('Are you sure, you want to Delete?');")
        End If
    End Sub

   

    Protected Sub gridShowCandiate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridShowCandiate.RowCommand
       ' Dim strSQL As String
       ' Dim id As String
       ' id = e.CommandArgument.ToString
       ' Dim objCmd As SqlCommand
       ' strSQL = "DELETE from candSkill where candid=" & id & "DELETE from candQualification where candid=" & id & "DELETE from candSchDetail where candid=" & id & "DELETE from candidateMaster where candid=" & id
       ' strConn.Open()
       ' objCmd = New SqlCommand(strSQL, strConn)
       ' objCmd.ExecuteNonQuery()
       ' objCmd.Dispose()
       ' strConn.Close()
        'End If
       ' bindGridView()
    End Sub
    Protected Sub gwListAccessUnits_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
    End Sub

    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
      '  strsqldetail = "select (candFName  + ' ' + candMName + ' ' +  candLName) as candname ,convert(varchar,canddate,7) as datecand," & _
      '                  "convert(varchar,candbirthdate,7) as birthdate,(select skilldesc from  skillmaster where skillid=candidateMaster.candpost) " & _
      '                  "as skill,convert(varchar,(CONVERT(int,candexp/12.0)))+ '.' + convert(varchar,  " & _
      '                  "(candexp - CONVERT(int,candexp/12.0) * 12) ) as totexp, " & _
      '                  " isnull((select StatusDesc from candStatusMaster where statusID in (select top 1 statusID from candSchDetail where Candid = candidateMaster.candId order by schdate desc)),'') as candstatus, " & _
      '                  " isnull((select top 1 schComment from candSchDetail where Candid = candidateMaster.candId order by schdate desc),'') as schComment ," & _
      '                   " * from candidateMaster  where 1=1 "
      '  'where  canddate='" & txtdate.Text & "'  " & _
      '  '" or  candfname like '%" & txtName.Text & "' or candLName like '%" & txtName.Text & "' "

      '  If (txtdate.Text <> "") Then
      '      strsqldetail = strsqldetail & " canddate='" & txtdate.Text & "' "
      '  End If
      '  If (txtName.Text <> "") Then
      '      strsqldetail = strsqldetail & " ( candfname like '%" & txtName.Text & "' or candLName like '%" & txtName.Text & "' ) "
      '  End If
      '  If (drpStatus.SelectedValue <> "") Then
      '      strsqldetail = strsqldetail & " and isnull((select Top 1 StatusID from dbo.candSchDetail where candid = candidateMaster.Candid order by schID desc),0) = '" & drpStatus.SelectedValue & "'"
      '  End If
      '  If (drpJobType.SelectedValue <> "") Then
      '      strsqldetail = strsqldetail & " and candidateMaster.candpost = '" & drpJobType.SelectedValue & "'"
      '  End If
      '  dadetails = New SqlDataAdapter(strsqldetail, strConn)
      '  dadetails.Fill(dsdetails)
      '  gridShowCandiate.DataSource = dsdetails
      '  gridShowCandiate.DataBind()
		bindGridView()
    End Sub

    Protected Sub btnschSerch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnschSerch.Click
        Response.Redirect("candSchSearch.aspx")
    End Sub

    Protected Sub gridShowCandiate_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gridShowCandiate.PageIndexChanging
        gridShowCandiate.PageIndex = e.NewPageIndex
        bindGridView()
    End Sub
	
    Protected Sub gridShowCandiate_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gridShowCandiate.Sorting
     
		ViewState("sortExpr") = e.SortExpression.ToString()   

        bindGridView()

    End Sub
End Class
