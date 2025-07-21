Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Partial Class emp_empAddCodeReview1
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim conn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim dtrItemDesc As SqlDataReader
    Dim cmdProjDetail As SQLCommand
    Dim strProjDetail As String
    Dim intProjId As Integer
    Dim dtrProjDetail As SqlDataReader
    Dim projectid As Integer
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Response.Write(Trim(Request.QueryString("projId")))
        gf.checkEmpLogin()
        Dim i As Integer
        i = Request.QueryString("feedbackId")
        If Trim(Request.QueryString("id")) <> "" Then
            projectid = Trim(Request.QueryString("id"))
            show_Details()
            showweightmaster()
            ViewReport()
            trmember.Visible = False
            trReportStatus.Visible = False
            ddl1.Visible = False
            ddl2.Visible = False
            ddl3.Visible = False
            ddl4.Visible = False
            ddl5.Visible = False
            ddl6.Visible = False
            ddl7.Visible = False

        ElseIf Trim(Request.QueryString("projId")) <> "" Then

            ''Dim projectid as integer
            projectid = Trim(Request.QueryString("projId"))
            show_Details()
            show_ProjectMembers()
            showReport()
            showweightmaster()
            trmember.Visible = True
            trReportStatus.Visible = True
            ddl1.Visible = True
            ddl2.Visible = True
            ddl3.Visible = True
            ddl4.Visible = True
            ddl5.Visible = True
            ddl6.Visible = True
            ddl7.Visible = True

            'trajax.Visible= False

        ElseIf Trim(Request.QueryString("feedbackId")) <> "" Then
            'fillComment()
            projectid = Trim(Request.QueryString("feedbackId"))
            show_Details()
            show_ProjectMembers()
            ViewReport()
            showweightmaster()

            trReportStatus.Visible = False
            ddl1.Visible = False
            ddl2.Visible = False
            ddl3.Visible = False
            ddl4.Visible = False
            ddl5.Visible = False
            ddl6.Visible = False
            ddl7.Visible = False

        End If

    End Sub

    '================================================================
    'FUNCTION FOR DISPLAY PROJECT DETAILS AT PAGE LOAD
    '================================================================
    Sub show_Details()

        Dim intStartDate As Date
        Dim intEndDate As Date
        Conn.Open()

        strProjDetail = "Select PM.projName,PM.projDuration,PM.projStartDate,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) " & _
        " AS projExpComp,PM.projActComp,CM.custId,CM.custName,CM.custCompany,CM.custAddress,EM.empid,EM.empName " & _
                 " from projectMaster PM,customerMaster CM,employeeMaster EM " & _
                 " where CM.custId=PM.custId and EM.empid=PM.projManager and PM.projId=" & projectid & ""


        cmdProjDetail = New SqlCommand(strProjDetail, conn)
        dtrProjDetail = cmdProjDetail.ExecuteReader

        If dtrProjDetail.Read Then

            lblprjName.Text = dtrProjDetail("projName")
            lblCustName.text = dtrProjDetail("custName") & "(" & dtrProjDetail("custId") & ")"
            lblCustAddress.Text = dtrProjDetail("custCompany") & "<br>" & dtrProjDetail("custAddress")
            If (dtrProjDetail("projDuration").ToString() = "0.25") Then
                lblProjDurat.Text = "1 Week"
            ElseIf (dtrProjDetail("projDuration").ToString() = "0.5" Or dtrProjDetail("projDuration").ToString() = "0.50") Then
                lblProjDurat.Text = "2 Week"
            ElseIf (dtrProjDetail("projDuration").ToString() = "0.75") Then
                lblProjDurat.Text = "3 Week"
            ElseIf (dtrProjDetail("projDuration").ToString() = "1.25") Then
                lblProjDurat.Text = "4 Week"
            ElseIf (dtrProjDetail("projDuration").ToString() = "1.50" Or dtrProjDetail("projDuration").ToString() = "1.5") Then
                lblProjDurat.Text = "5 Week"
            ElseIf (dtrProjDetail("projDuration").ToString() = "1.75") Then
                lblProjDurat.Text = "6 Week"
            Else
                lblProjDurat.Text = dtrProjDetail("projDuration") & " Month"
            End If


            lblProjMang.Text = dtrProjDetail("empName") & "(" & dtrProjDetail("empId") & ")"
            lblStartDate.Text = Day(dtrProjDetail("projStartDate")) & "-" & MonthName(Month(dtrProjDetail("projStartDate")), 2) & "-" & Year(dtrProjDetail("projStartDate")) & ""
            lblExpDate.Text = Day(dtrProjDetail("projExpComp")) & "-" & MonthName(Month(dtrProjDetail("projExpComp")), 2) & "-" & Year(dtrProjDetail("projExpComp")) & ""
            If IsDBNull(dtrProjDetail("projActComp")) = False Then
                lblActCompDate.Text = Day(dtrProjDetail("projActComp")) & "-" & MonthName(Month(dtrProjDetail("projActComp")), 2) & "-" & Year(dtrProjDetail("projActComp")) & ""

            End If
            End If

        cmdProjDetail.Dispose()
        dtrProjDetail.Close()
        conn.Close()
        conn.Open()

        Dim compDays As Integer
        compDays = DateDiff(DateInterval.Day, intStartDate, intEndDate)
        Dim nowDays As Integer
        nowDays = DateDiff(DateInterval.Day, intStartDate, DateAndTime.Now())

        strProjDetail = "select * from  projectStatus where projId=" & projectid & " and proStatusDate=(Select max(proStatusDate) from projectStatus where projId=" & projectid & ") "

        cmdProjDetail = New SqlCommand(strProjDetail, conn)
        dtrProjDetail = cmdProjDetail.ExecuteReader

        If dtrProjDetail.Read Then
            If Trim(dtrProjDetail("projStatus")) = 100 Then
                lblprojStatus.Text = "Completed"
            Else
                lblprojStatus.Text = Trim(dtrProjDetail("projStatus")) & "% as on " & Day(dtrProjDetail("proStatusDate")) & "-" & MonthName(Month(dtrProjDetail("proStatusDate")), 2) & "-" & Year(dtrProjDetail("proStatusDate"))
                If Request.QueryString("expCompleted") <> "N" Then
                    lblExpProjStatus.Text = Request.QueryString("expCompleted") & " as on " & Day(dtrProjDetail("proStatusDate")) & "-" & MonthName(Month(dtrProjDetail("proStatusDate")), 2) & "-" & Year(dtrProjDetail("proStatusDate"))
                    lblprojStatus.BackColor = Color.Red
                Else
                    lblExpProjStatus.Text = ""
                End If
            End If
        End If
        conn.Close()

    End Sub

    '================================================================
    'PROCEDURE FOR BIND PROJECT MEMBERS AT PAGE LOAD
    '================================================================
    Sub show_ProjectMembers()

        Dim daProjDetail As SqlDataAdapter
        Dim dsProjMem As New dataset
        conn.open()

        daProjDetail = New SqlDataAdapter("select empId,empName +'('+ CONVERT(varchar, empId) +')' as empName from employeeMaster where empId in " & _
       "(select empId from projectMember where projId=" & projectid & ")and empLeavingDate is null group by empId,empName", conn)

        Dim dt As DataTable = New DataTable("table1")
        Dim dc As DataColumn = New DataColumn("srno", GetType(Int32))
        dc.AutoIncrement = True
        dc.AutoIncrementSeed = 1

        dt.Columns.Add(dc)
        dsProjMem.Tables.Add(dt)
        daProjDetail.Fill(dsProjMem, "table1")
        dgProjectTeamMem.DataSource = dsProjMem
        dgProjectTeamMem.DataBind()

        conn.Close()
    End Sub

    Sub showReport()

        Dim daReport As SqlDataAdapter
        Dim dsReport As New DataSet
        'Dim strshow As String = "select ratedate,comments,(20*codingConventions)/100.0 as codingConventions,(10*fileStructure)/100.0 as fileStructure,(20*codeOptimization)/100.0  as codeOptimization ,(25*codeDocumentation)/100.0 as codeDocumentation,(25*dbStructure)/100.0 as dbStructure,((20*codingConventions)/100.0 + (10*fileStructure)/100.0+(20*codeOptimization)/100.0  +(25*codeDocumentation)/100.0 +(25*dbStructure)/100.0 ) as total from tblcodeRevReport,tblcodereview where tblcodeRevReport.codeRevId=tblcodereview.codereviewid and proj_id=" & Trim(Request.QueryString("projId")) & " and emp_id=" & Trim(Session("dynoEmpIdSession")) & "  order by  ratedate desc  "
        Dim strshow As String = "select employeemaster.empName,tblcodeRevReport.coderevid,tblcodeRevReport.ratedate,tblcodeRevReport.comments,floor((tblcodeRevReport.codingConventions/5.0)*100) as codingConventions,floor((tblcodeRevReport.fileStructure/5.0)*100) " & _
                      "as fileStructure,floor((tblcodeRevReport.codeOptimization/5.0)*100) as codeOptimization ,floor((tblcodeRevReport.codeDocumentation/5.0)*100)" & _
                      "as codeDocumentation,floor((tblcodeRevReport.dbStructure/5.0)*100) " & _
                      "as dbStructure, " & _
                      "Floor(((tblcodeRevReport.codingConventions/5.0)*100.0 + (tblcodeRevReport.fileStructure/5.0)*100.0+(tblcodeRevReport.codeOptimization/5.0)*100.0 +(codeDocumentation/5.0)*100.0 + " & _
                      "(tblcodeRevReport.dbStructure/5.0)*100.0)/5) " & _
                      "as total from tblcodeRevReport,employeemaster " & _
                "where tblcodeRevReport.projectid=" & Trim(Request.QueryString("projId")) & " and employeemaster.empid=tblcodeRevReport.empid  and  tblcodeRevReport.empid=" & Trim(Session("dynoEmpIdSession")) & " order by ratedate desc "
        
        daReport = New SqlDataAdapter(strshow, conn)

        daReport.Fill(dsReport, "table1")


        dgrdrating.DataSource = dsReport
        dgrdrating.DataBind()


    End Sub
    Sub showweightmaster()
        Try
            Dim daweight As SqlDataAdapter
            Dim dsweight As New DataSet
            daweight = New SqlDataAdapter("select * from tblcoderevmaster", conn)
            daweight.Fill(dsweight, "tblcoderevmaster")
            Dim strrowCC As String = (dsweight.Tables("tblcoderevmaster").Rows(0).Item("attName"))
            Dim strcolCC As String = ((dsweight.Tables("tblcoderevmaster").Rows(0).Item("attWeightage")))
            lblCodeC.Text = strrowCC & "(" & strcolCC & "%)"


            Dim strrowfs As String = (dsweight.Tables("tblcoderevmaster").Rows(1).Item("attName"))
            Dim strColfs As String = (dsweight.Tables("tblcoderevmaster").Rows(1).Item("attWeightage"))
            lblFS.Text = strrowfs & " (" & strColfs & "%)"

            Dim strrowCo As String = (dsweight.Tables("tblcoderevmaster").Rows(2).Item("attName"))
            Dim strcolCo As String = (dsweight.Tables("tblcoderevmaster").Rows(2).Item("attWeightage"))
            lblCo.Text = strrowCo & " (" & strcolCo & "%)"

            Dim strRowCodedoc As String = (dsweight.Tables("tblcoderevmaster").Rows(3).Item("attName"))
            Dim strColCodeDoc As String = (dsweight.Tables("tblcoderevmaster").Rows(3).Item("attWeightage"))
            lblCodeDoc.Text = strRowCodedoc & " (" & strColCodeDoc & "%)"

            Dim strrowds As String = (dsweight.Tables("tblcoderevmaster").Rows(4).Item("attName"))
            Dim strColds As String = (dsweight.Tables("tblcoderevmaster").Rows(4).Item("attWeightage"))
            lblDS.Text = strrowds & " (" & strColds & "%)"
        Catch ex As Exception

        End Try


        ' Response.End()





    End Sub

    Sub dgrdRatingbount(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            ' Dim dgItem As DataGridItem
            ' For Each dgItem In dgrdrating.Items
            Dim detail As Button = CType(e.Item.Cells(10).FindControl("btnDetails"), Button)
            If e.Item.Cells(8).text <> "" Then
                detail.Attributes.Add("onclick", "popupProjectDetail(" & e.Item.Cells(8).text & "); return false;")
            Else
                e.Item.Cells(8).text = ""
            End If
            ' Next
        End If

        Dim dgItem As DataGridItem
        For Each dgItem In dgrdrating.Items
            dgItem.Cells(9).Text = Trim(replace(dgItem.Cells(11).Text, vbcrlf, "<br>"))
        Next
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim strSqlReportinsert As String
        Dim cmdReport As SqlCommand
        strSqlReportinsert = "insert into tblcodeRevReport(projectId,empid,RateDate,codingConventions,fileStructure,codeOptimization,codeDocumentation,dbStructure,comments)" & _
       " values('" & Trim(Request.QueryString("projId")) & "',' " & Session("dynoEmpIdSession") & " ', getdate(),' " & ddlcodingConv.SelectedValue & " ',' " & ddlFileStructure.SelectedValue & " ',' " & ddlCodeOptimization.SelectedValue & " ',' " & ddlCodeDocument.SelectedValue & " ',' " & ddlDataStructure.SelectedValue & " ',' " & txtComment.Text.Replace("'", "''").ToString() & " ')"

        ' Response.Write(txtComment.Text.Replace("'", "''").ToString() & "</br>")
        'Response.Write(strSqlReportinsert)
        ' response.end()

        conn.Open()
        cmdReport = New SqlCommand(strSqlReportinsert, conn)
        cmdReport.ExecuteNonQuery()
        btnSubmit.Enabled = False
        Response.Redirect("empAddCodeReview.aspx?projId=" & Trim(Request.QueryString("projId")))

    End Sub

    '------------ code to view code review report


    Sub ViewReport()

        Dim daReport As SqlDataAdapter
        Dim dsReport As New DataSet
        'Dim strshow As String = "select ratedate,comments,(20*codingConventions)/100.0 as codingConventions,(10*fileStructure)/100.0 as fileStructure,(20*codeOptimization)/100.0  as codeOptimization ,(25*codeDocumentation)/100.0 as codeDocumentation,(25*dbStructure)/100.0 as dbStructure,((20*codingConventions)/100.0 + (10*fileStructure)/100.0+(20*codeOptimization)/100.0  +(25*codeDocumentation)/100.0 +(25*dbStructure)/100.0 ) as total from tblcodeRevReport,tblcodereview where tblcodeRevReport.codeRevId=tblcodereview.codereviewid and proj_id=" & Trim(Request.QueryString("projId")) & " and emp_id=" & Trim(Session("dynoEmpIdSession")) & "  order by  ratedate desc  "
        Dim strshow As String = "select employeemaster.empName,tblcodeRevReport.coderevid,tblcodeRevReport.ratedate,tblcodeRevReport.comments,floor((tblcodeRevReport.codingConventions/5.0)*100) as codingConventions,floor((tblcodeRevReport.fileStructure/5.0)*100) " & _
                 "as fileStructure,floor((tblcodeRevReport.codeOptimization/5.0)*100) as codeOptimization ,floor((tblcodeRevReport.codeDocumentation/5.0)*100)" & _
                 "as codeDocumentation,floor((tblcodeRevReport.dbStructure/5.0)*100) " & _
                 "as dbStructure, " & _
                 "Floor(((tblcodeRevReport.codingConventions/5.0)*100.0 + (tblcodeRevReport.fileStructure/5.0)*100.0+(tblcodeRevReport.codeOptimization/5.0)*100.0 +(codeDocumentation/5.0)*100.0 + " & _
                 "(tblcodeRevReport.dbStructure/5.0)*100.0)/5) " & _
                 "as total from tblcodeRevReport,employeemaster " & _
                 "where tblcodeRevReport.empid=employeemaster.empid and  tblcodeRevReport.projectid=" & projectid & " order by ratedate desc "
        'Response.Write(strshow)
        'Response.End()
        daReport = New SqlDataAdapter(strshow, conn)
        daReport.Fill(dsReport, "table1")
        dgrdrating.DataSource = dsReport
        dgrdrating.DataBind()
        ' End Sub


        ''--------------------------------------------


        'Sub fillComment()
        'Dim fldresolution as String
        'Dim strSQLhistory as String
        'Dim cmdhistory as sqlcommand
        'Dim dtrhistory as sqlDataReader
        'Dim comm as String
        'Dim empname as String

        ''strSQLhistory="select * from tblFeedback where projectid=" & Request.QueryString("feedbackId")
        '      strSQLhistory = "select tblEmpFeedback.FeedBack,employeemaster.empname from tblFeedback,employeemaster where " & _
        '          "tblEmpFeedback.empid=employeemaster.empid and tblEmpFeedback.projectid=" & Request.QueryString("feedbackId")

        'cmdhistory = New SqlCommand(strSQLhistory,conn)
        'conn.open()
        'dtrhistory = cmdhistory.ExecuteReader
        ''  while dtrhistory.Read 
        ''	 empname = dtrhistory("empname")
        ''	 comm=dtrhistory("FeedBack")
        ''	fldresolution=empname &  " --- "  & comm	

        ''	txtFeedbakHistory.Text= fldresolution
        '' End While



        ' while dtrhistory.read  
        '								If fldresolution = ""  Then 
        '									fldresolution =  " - " & dtrhistory("empname") & " - " &  dtrhistory("FeedBack") 
        '								Else
        '									fldresolution =  fldresolution & vbcrlf & " - " &  dtrhistory("empname") & " - " & dtrhistory("FeedBack")
        '								End If
        '								txtFeedbakHistory.Text= fldresolution
        ' End While
        ' conn.close
    End Sub
End Class

