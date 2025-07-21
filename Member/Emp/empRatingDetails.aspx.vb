Imports System.Data
Imports System.Data.SqlClient
Partial Class emp_empRatingDetails
    Inherits Authentication
    Dim dsn As String = ConfigurationSettings.AppSettings("dsn")
    Dim conn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        Try
            btnsubmit.Attributes.Add("onclick", "javascript:return feedback1();")
            fillprojectName()
            showweightmaster()
            fillgridDetails()
            fillbox()
        Catch ex As Exception
        End Try
    End Sub

	Public Sub fillprojectName()
		Dim cmdProjname as sqlcommand
        Dim dtrProjname as sqldatareader
		Dim projectname as string
		Dim strSqlname as string="select projname from projectmaster where projid=(select projectId from tblCodeRevReport where coderevid="& Request.QueryString("revId") & ")"
		conn.open
        cmdProjname = New SqlCommand(strSqlname, conn)
        dtrProjname = cmdProjname.ExecuteReader

        If dtrProjname.Read Then
				lblprjName.Text=dtrProjname("projname")
        End if
		conn.close
	End Sub
	
	  public Sub showweightmaster()
        Dim daweight As SqlDataAdapter
        Dim dsweight As New DataSet
        daweight = New SqlDataAdapter("select * from tblcoderevmaster", conn)
        daweight.Fill(dsweight, "tblcoderevmaster")
        Dim strrowCC As String = (dsweight.Tables("tblcoderevmaster").Rows(0).Item("attName"))
        Dim strcolCC As String = ((dsweight.Tables("tblcoderevmaster").Rows(0).Item("attWeightage")))
        session("lblCodeC") = strrowCC & "(" & strcolCC & "%)"


        Dim strrowfs As String = (dsweight.Tables("tblcoderevmaster").Rows(1).Item("attName"))
        Dim strColfs As String = (dsweight.Tables("tblcoderevmaster").Rows(1).Item("attWeightage"))
		session("lblFS") = strrowfs & " (" & strColfs & "%)"

        Dim strrowCo As String = (dsweight.Tables("tblcoderevmaster").Rows(2).Item("attName"))
        Dim strcolCo As String = (dsweight.Tables("tblcoderevmaster").Rows(2).Item("attWeightage"))
		 session("lblCo")  = strrowCo & " (" & strcolCo & "%)"

        Dim strRowCodedoc As String = (dsweight.Tables("tblcoderevmaster").Rows(3).Item("attName"))
        Dim strColCodeDoc As String = (dsweight.Tables("tblcoderevmaster").Rows(3).Item("attWeightage"))
		session("lblCodeDoc") = strRowCodedoc & " (" & strColCodeDoc & "%)"

        Dim strrowds As String = (dsweight.Tables("tblcoderevmaster").Rows(4).Item("attName"))
        Dim strColds As String = (dsweight.Tables("tblcoderevmaster").Rows(4).Item("attWeightage"))
		session("lblDS") = strrowds & " (" & strColds & "%)"

    End Sub

	Sub fillgridDetails()
    
	Dim daReport As SqlDataAdapter
    Dim dsReport As New DataSet
	Dim strSqldetail as String
	strSqldetail="select employeemaster.empName,tblcodeRevReport.coderevid,tblcodeRevReport.ratedate,tblcodeRevReport.comments, " & _
				"floor((tblcodeRevReport.codingConventions/5.0)*100) as codingConventions,floor((tblcodeRevReport.fileStructure/5.0)* " & _
				"100) as fileStructure,floor((tblcodeRevReport.codeOptimization/5.0)*100) as codeOptimization , " & _
				"floor((tblcodeRevReport.codeDocumentation/5.0)*100)as codeDocumentation, " & _
				"floor((tblcodeRevReport.dbStructure/5.0)*100) as dbStructure, " & _
				" Floor(((tblcodeRevReport.codingConventions/5.0)*100.0 +  " & _
				"(tblcodeRevReport.fileStructure/5.0)*100.0+(tblcodeRevReport.codeOptimization/5.0)*100.0  " & _
				"+(codeDocumentation/5.0)*100.0 + (tblcodeRevReport.dbStructure/5.0)*100.0)/5) as total " & _
				" from tblcodeRevReport,employeemaster where tblcodeRevReport.empid=employeemaster.empid " & _
				"and tblcodeRevReport.codeRevId="& Request.QueryString("revId") 
	 daReport = New SqlDataAdapter(strSqldetail, conn)
     daReport.Fill(dsReport, "table1")
     dgrdrating.DataSource = dsReport
     dgrdrating.DataBind()
	End sub
	
	Sub fillbox()
        Dim fldresolution As String = ""
		Dim strSQLhistory as String
		Dim cmdhistory as sqlcommand
		Dim comm as String
		Dim empname as String
			Dim strsqlcomment As String
			       Dim dtrFB As SqlDataReader
			Dim i as integer=Request.QueryString("revId")
        strsqlcomment = "select tblEmpFeedback.FeedBack,employeemaster.empname,convert(varchar(10),tblEmpFeedback.fbdate,7) as datefb  from tblEmpFeedback,employeemaster where tblEmpFeedback.empid=employeemaster.empid and tblEmpFeedback.coderevid=" & i
        'strsqlcomment = "select tblCodeRevReport.Comments as FeedBack,employeemaster.empname,convert(varchar(10),tblCodeRevReport.ratedate,7) as datefb  from tblCodeRevReport,employeemaster where tblCodeRevReport.empid=employeemaster.empid and tblCodeRevReport.coderevid=" & i
        
	        Dim cmd As SqlCommand = New SqlCommand(strsqlcomment, Conn)
        conn.Open()
            dtrFB = cmd.ExecuteReader()

            while dtrFB.read  
					If strsqlcomment = ""  Then 
							fldresolution =  " - " & dtrFB("empname") & " - " &  dtrFB("datefb") & " - " & dtrFB("FeedBack")
					Else
							fldresolution =  fldresolution & vbcrlf & " - " &  dtrFB("empname") & " - " & dtrFB("datefb")& " - " & dtrFB("FeedBack") & vbcrlf & _
					 "---------------------------------------------------------------------------------------------------------------------------"
					End If
 
		   End While
        conn.Close()
		  session("strEmp")=fldresolution
		   'txtFeedbakHistory.Value= session("strEmp")
		 Conn.close

	End sub

    Protected Sub btnsubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
			Dim strsql As String
			Dim objCmd As sqlcommand
        strsql = "insert into tblEmpFeedback(Coderevid,fbdate,feedback,empid)values (" & Trim(Request.QueryString("revId")) & ",getdate(),'" & textFeedback.Value & "'," & Trim(Session("dynoEmpIdSession")) & ")"

			
			Conn.open()
			objCmd = New SqlCommand(strsql, Conn)
			objCmd.executeNonQuery()
			response.redirect("empRatingDetails.aspx?revId=" & Trim(Request.QueryString("revId")))
    End Sub

	Sub dgrdDetails(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
		   Dim dgItem As DataGridItem
		   For Each dgItem In dgrdrating.Items
				dgItem.Cells(10).Text=  Trim(replace(dgItem.Cells(9).Text,vbcrlf,"<br>"))
		   next
    End Sub
 End Class
