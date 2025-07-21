<%@ Page Language="VB" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Math"%>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
	<title>Employee Project</title>
		<script language="JavaScript" type="text/javascript">
		 function popupProjectDetail(Id,expCompleted)
		 {
		   
			 var win = window.open('empProjectDetails.aspx?projId='+ Id + "&expCompleted=" + expCompleted,'winWatch','scrollbars=yes,toolbar=no,menubar=no,location=right,width=850,height=550,left=100,top=50');
			 win.focus(); 
		 } 
		 function popupProjectStatus(Id,Status)
		 {
			 var win = window.open('empProjectStatus.aspx?projId='+ Id + "&Status=" + Status,'winWatch','scrollbars=no,toolbar=no,menubar=no,location=right,resizable=no,width=600,height=320,left=50,top=50');
			 win.focus();
		 }
		</script>


			     <script language="javascript" type="text/javascript"  >
               
		 function popupProjectDetailView(Id)
		 {
			 window.location.href="empAddCodeReview.aspx?feedbackId="+ Id ;
		 } 
	 	</script>

	   <script language="VB" runat="server">
		
	       Dim intProjId As Integer
	       Dim sql As String
	       Dim strSQL As String
	       Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	       Dim strConn As SqlConnection = New SqlConnection(dsn)
	       Dim empId As Integer
	       Dim intProjManagerId As Integer
	       Dim str1 As String
	       Dim gf As New generalFunction
		Sub Page_Load(sender As Object, e As EventArgs) 
			gf.checkEmpLogin()
			'Response.Write()
			'Response.End()
	           Try
	               If Not IsPostBack Then
	                   empId = CInt(Session("dynoEmpIdSession"))
					  
	                   If Session("dynoBugAdminSession") = 1 Then
	                       BindGrid(1)
	                   Else
	                       BindGrid(empId)
	                   End If
	               End If
					
	               If Request.QueryString("search") <> "" Then
	                   Dim dsn1st As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	                   Dim co As SqlConnection = New SqlConnection(dsn1st)
	                   Dim str, searchcri As String
	                   searchcri = Request.QueryString("search")
	                   If searchcri = "Current" Then
	                       str = " (projStatusTDesc IS NULL OR projStatusTDesc NOT IN ('Completed','Cancelled')) "
	                   ElseIf searchcri = "Completed" Then
	                       str = " projStatusTDesc ='" & searchcri & "' "
	                   ElseIf searchcri = "Cancelled" Then
	                       str = " projStatusTDesc ='" & searchcri & "' "
	                   ElseIf searchcri = "All" Then
	                       str = " (projStatusTDesc IS NULL OR projStatusTDesc  IN ('Completed','Cancelled','To Be Started','Started','On Hold','In Progress'))"
	                   End If
	              
	             
	                   If Session("dynoBugAdminSession") = 1 Then
	                       str1 = "SELECT *,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) " & _
	         " AS projExpComp,(100*(DATEDIFF(day, projStartDate, PS.proStatusDate))) as ExpCompleted  " & _
	         "FROM projectMaster PM LEFT JOIN(select PST.projId,PSTM.projStatusTDesc,PST.projStatusId,  " & _
	         "PST.projStatus,PST.projStatusTId,PST.proStatusDate from projstatustypemaster PSTM inner join  " & _
	         "(select * from projectStatus where projStatusId in( select projStatusId from projectStatus a " & _
	         "WHERE prostatusdate in( select Max(prostatusdate) from projectStatus b where a.projId=b.projId GROUP BY projId)))PST On  " & _
	         "PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId  inner join customerMaster on PM.custId=customerMaster.custId  " & _
	         "inner join employeeMaster on PM.projManager=employeeMaster.empid  where " & str & " ORDER BY projStartDate Desc"
	                       'Response.Write(str1)
	                       'Response.End()
	            ElseIf Session("dynoMgrSession") = 1 And searchcri = "Current" Then
			 
			        str1="SELECT *,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) AS projExpComp,(100*(DATEDIFF(day, projStartDate, PS.proStatusDate))) as ExpCompleted " & _
                            " FROM projectMaster PM LEFT JOIN (select PST.projId,PSTM.projStatusTDesc,PST.projStatusId,PST.projStatus,PST.projStatusTId,PST.proStatusDate from projstatustypemaster PSTM  " & _
                            " inner join (select * from projectStatus where projStatusId in( select projStatusId from projectStatus a WHERE prostatusdate in( select Max(prostatusdate) from projectStatus b where a.projId=b.projId GROUP BY projId)))PST On PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId inner join customerMaster on PM.custId=customerMaster.custId inner join employeeMaster on PM.projManager=employeeMaster.empid " & _
                            " where " & str & " ORDER BY projStartDate Desc"
	                   
	                   Else
	                       str1 = "SELECT *,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) " & _
	         " AS projExpComp,(100*(DATEDIFF(day, projStartDate, PS.proStatusDate))) as ExpCompleted  " & _
	         "FROM projectMaster PM LEFT JOIN(select PST.projId,PSTM.projStatusTDesc,PST.projStatusId,  " & _
	         "PST.projStatus,PST.projStatusTId,PST.proStatusDate from projstatustypemaster PSTM inner join  " & _
	         "(select * from projectStatus where projStatusId in( select projStatusId from projectStatus a " & _
	         "WHERE prostatusdate in( select Max(prostatusdate) from projectStatus b where a.projId=b.projId GROUP BY projId)))PST On  " & _
	         "PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId  inner join customerMaster on PM.custId=customerMaster.custId  " & _
	         "inner join employeeMaster on PM.projManager=employeeMaster.empid where (PM.projId in(select projId from projectMember where empId=" & empId & ")or (PM.projid in(select proj_id from tblcodereview where emp_id=" & empId & ")) OR PM.projManager=" & empId & ") and " & str & " ORDER BY projStartDate Desc"
	                   End If
	              
	        
	                   co.Open()
	                   Dim cmd As SqlCommand = New SqlCommand(str1, co)
	                   Dim Rdr As SqlDataReader = cmd.ExecuteReader()
	                   dgEmpProject.DataSource = Rdr
	                   dgEmpProject.DataBind()
	                   Rdr.Close()
	               End If
	           Catch ex As Exception

	           End Try
		End Sub
			'=================================
			  'END BIND DATAGRID AT PAGE LOAD
			'==================================
		 Sub BindGrid(byval empId as integer)
	           Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
			Dim conn As SqlConnection = New SqlConnection(dsn1)
			Dim con As SqlConnection = New SqlConnection(dsn1)
			Dim strSQL,strSQL1 as String
			
		    try
			If Session("dynoBugAdminSession") = 1 then 
	                   strSQL = "SELECT *,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) " & _
	     " AS projExpComp,(100*(DATEDIFF(day, projStartDate, PS.proStatusDate))) as ExpCompleted FROM projectMaster PM LEFT JOIN " & _
	     "(select PST.projId,PSTM.projStatusTDesc,PST.projStatusId,PST.projStatus,PST.projStatusTId,PST.proStatusDate " & _
	     "from projstatustypemaster PSTM inner join (select * from projectStatus where projStatusId " & _
	     "in( select projStatusId from projectStatus a WHERE " & _
	     "prostatusdate in( select Max(prostatusdate) from projectStatus b where a.projId=b.projId GROUP BY projId)))PST " & _
	     "On PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId " & _
	     " inner join customerMaster on PM.custId=customerMaster.custId inner join employeeMaster " & _
	     " on PM.projManager=employeeMaster.empid where (projStatusTDesc is NULL OR projStatusTDesc NOT IN('Completed','Cancelled')) ORDER BY projStartDate Desc "
	                   
			 ElseIf Session("dynoMgrSession") = 1 Then
			 
			        strSQL="SELECT *,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) AS projExpComp,(100*(DATEDIFF(day, projStartDate, PS.proStatusDate))) as ExpCompleted " & _
                            " FROM projectMaster PM LEFT JOIN (select PST.projId,PSTM.projStatusTDesc,PST.projStatusId,PST.projStatus,PST.projStatusTId,PST.proStatusDate from projstatustypemaster PSTM  " & _
                            " inner join (select * from projectStatus where projStatusId in( select projStatusId from projectStatus a WHERE prostatusdate in( select Max(prostatusdate) from projectStatus b where a.projId=b.projId GROUP BY projId)))PST On PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId inner join customerMaster on PM.custId=customerMaster.custId inner join employeeMaster on PM.projManager=employeeMaster.empid " & _
                            " where (projStatusTDesc is NULL OR projStatusTDesc not in ('Completed','Cancelled')) ORDER BY projStartDate Desc"

			 Else
				
	                   strSQL = "SELECT *,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) " & _
	     " AS projExpComp,(100*(DATEDIFF(day, projStartDate, PS.proStatusDate))) as ExpCompleted FROM projectMaster PM LEFT JOIN (select PST.projId,PSTM.projStatusTDesc,PST.projStatusId,PST.projStatus,PST.projStatusTId,PST.proStatusDate " & _
	     "from projstatustypemaster PSTM inner join (select * from projectStatus where projStatusId " & _
	    "in( select projStatusId from projectStatus a WHERE " & _
	    "prostatusdate in( select Max(prostatusdate) from projectStatus b where a.projId=b.projId GROUP BY projId)))PST " & _
	    "On PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId " & _
	    " inner join customerMaster on PM.custId=customerMaster.custId inner join employeeMaster " & _
	   " on PM.projManager=employeeMaster.empid " & _
	   "where (PM.projId in(select projId from projectMember where empId=" & empId & ")or (PM.projid in(select proj_id from tblcodereview where emp_id=" & empId & ")) or " & _
	   " PM.projManager=" & empId & ") and (projStatusTDesc is NULL OR projStatusTDesc not in ('Completed','Cancelled')) ORDER BY projStartDate Desc"
	                
			 end if 
	              'Response.Write(strSQL)
	               'Response.End()
			Conn.Open()
			Dim cmd As SqlCommand = New SqlCommand(strSQL,conn)
			Dim Rdr As SqlDataReader=Cmd.ExecuteReader()
			   
			dgEmpProject.DataSource = Rdr
			dgEmpProject.DataBind()
			Rdr.close()
			catch ex as exception
			con.open()
		 
			if Session("dynoBugAdminSession") = 1 Then
		   
	                   strSQL1 = "SELECT *,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) " & _
    " AS projExpComp,(100*(DATEDIFF(day, projStartDate, PS.proStatusDate)))as ExpCompleted FROM projectMaster PM LEFT JOIN " & _
    "(select PST.projId,PSTM.projStatusTDesc,PST.projStatusId,PST.projStatus,PST.projStatusTId,PST.proStatusDate " & _
    "from projstatustypemaster PSTM inner join (select * from projectStatus where projStatusId " & _
    "in( select projStatusId from projectStatus a WHERE " & _
    "prostatusdate in( select Max(prostatusdate) from projectStatus b where a.projId=b.projId GROUP BY projId)))PST " & _
    "On PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId " & _
    " inner join customerMaster on PM.custId=customerMaster.custId inner join employeeMaster " & _
    " on PM.projManager=employeeMaster.empid where (projStatusTDesc is NULL OR projStatusTDesc not in('Completed','Cancelled')) ORDER BY projStartDate Desc "
			
	 ElseIf Session("dynoMgrSession") = 1 Then
			 
			        strSQL="SELECT *,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) AS projExpComp,(100*(DATEDIFF(day, projStartDate, PS.proStatusDate))) as ExpCompleted " & _
                            " FROM projectMaster PM LEFT JOIN (select PST.projId,PSTM.projStatusTDesc,PST.projStatusId,PST.projStatus,PST.projStatusTId,PST.proStatusDate from projstatustypemaster PSTM  " & _
                            " inner join (select * from projectStatus where projStatusId in( select projStatusId from projectStatus a WHERE prostatusdate in( select Max(prostatusdate) from projectStatus b where a.projId=b.projId GROUP BY projId)))PST On PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId inner join customerMaster on PM.custId=customerMaster.custId inner join employeeMaster on PM.projManager=employeeMaster.empid " & _
                            " where (projStatusTDesc is NULL OR projStatusTDesc not in ('Completed','Cancelled')) ORDER BY projStartDate Desc"
	Else
			
	                   strSQL1 = "SELECT *,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) " & _
    " AS projExpComp,(100*(DATEDIFF(day, projStartDate, PS.proStatusDate))) as ExpCompleted FROM projectMaster PM LEFT JOIN (select PST.projId,PSTM.projStatusTDesc,PST.projStatusId,PST.projStatus,PST.projStatusTId,PST.proStatusDate " & _
    "from projstatustypemaster PSTM inner join (select * from projectStatus where projStatusId " & _
    "in( select projStatusId from projectStatus a WHERE " & _
    "prostatusdate in( select Max(prostatusdate) from projectStatus b where a.projId=b.projId GROUP BY projId)))PST " & _
    "On PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId " & _
    " inner join customerMaster on PM.custId=customerMaster.custId inner join employeeMaster " & _
    " on PM.projManager=employeeMaster.empid " & _
    "where (PM.projId in(select projId from projectMember where empId=" & empId & ")or (PM.projid in(select proj_id from tblcodereview where emp_id=" & empId & ")) or " & _
    " PM.projManager=" & empId & ")  and (projStatusTDesc is NULL OR projStatusTDesc not in ('Completed','Cancelled'))  ORDER BY projStartDate Desc"
		
			End If
			
		
		
		
			
			Dim cmd1 As SqlCommand = New SqlCommand(strSQL1,con)
			Dim Rdr1 As SqlDataReader
			Rdr1=Cmd1.ExecuteReader()
			dgEmpProject.DataSource = Rdr1
			dgEmpProject.DataBind()
			con.close()
		   End try
		 End Sub
		'========================================
		'CHECK EMPLOYEE IS PROJECT MANAGER OR NOT
		'========================================

		 Sub dgEmpProject_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) 
				Dim intDate as date
				Dim dgItem As DataGridItem
				dim intCounrow as Integer=0
				For Each dgItem In dgEmpProject.Items
				'response.write(dgItem.Cells(8).Text)
					if dgItem.Cells(12).text<>"&nbsp;" then
					   dgItem.Cells(8).Text=cstr(dgItem.Cells(15).text) & "% as on " &  cstr(dgItem.Cells(16).text) & " <BR> Expected:" & cstr(dgItem.Cells(9).text) 
					  ' response.write(dgItem.Cells(8).text)
					   If Trim(dgItem.Cells(8).text)= "&nbsp;% as on &nbsp; <BR> Expected:&nbsp;"  Then
						dgItem.Cells(8).Text=""
						End if
						
						'dgItem.Cells(8).Text="np"
					  
					else
		               dgItem.Cells(8).Text=""
					end if
					
					if dgItem.Cells(7).text="&nbsp;" then
						dgItem.Cells(7).text="To Be Started"
					end If
					

					
				Next
			 
				For Each dgItem In dgEmpProject.Items
					check_ProjectManger(cint(dgItem.Cells(0).Text))
					if intProjManagerId=1 then
					   CType(dgItem.Cells(13).Controls(1), Button).visible=true
							dgEmpProject.Columns(13).Visible =true
					else
						  CType(dgItem.Cells(13).Controls(1), Button).visible=false
						  dgEmpProject.Columns(13).Visible =false
				    end if
					
				    if Session("dynoBugAdminSession")= 1 then
				 			  CType(dgItem.Cells(13).Controls(1), Button).visible=true
							  dgEmpProject.Columns(13).Visible =true 
				 
					else if Session("dynoBugAdminSession") <> 1
				              dgEmpProject.Columns(13).Visible =true 
				    end if
				   
				Next

	For Each dgItem In dgEmpProject.Items

Dim strsqloverall as String
					
                       Dim  dtroall as sqlDataReader
						Dim cmdoall as sqlcommand
						Dim con as SqlConnection
						Con=New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
					
						strsqloverall=""
						strsqloverall="select  count(codeRevId) as numRow,sum(floor(codingConventions/5.0*100.0))as  codingConventions, sum(floor(fileStructure/5.0*100.0)) as  fileStructure,sum(floor(codeOptimization/5.0*100.0)) as codeOptimization,sum(floor(codeDocumentation/5.0*100.0)) as codeDocumentation,sum(floor(dbStructure/5.0*100.0)) as dbStructure from tblcodeRevReport where  projectId="& dgItem.Cells(0).Text 
						
						'response.end
						Con.Open()

						cmdoall =  new SqlCommand(strsqloverall,Con)
					    dtroall =  cmdoall.ExecuteReader()
					
					    if dtroall.read Then

							  If dtroall("numRow")= 0 Then 
								dgItem.Cells(12).Text="---"
								else
								dgItem.Cells(12).Text=(FormatNumber((dtroall("codingConventions").ToString()/dtroall("numRow").ToString()) + (dtroall("fileStructure").ToString()/dtroall("numRow").ToString()) +( dtroall("codeOptimization").ToString()/dtroall("numRow").ToString()) + (dtroall("codeDocumentation").ToString()/dtroall("numRow").ToString()) + (dtroall("dbStructure").ToString()/dtroall("numRow").ToString()),0))/5 &"%"
						      End if
							
					     End if 	

					    cmdoall.Dispose()
					    Con.Close()						
			



	Next
				
				dim intCheck as integer=0
				   For Each dgItem In dgEmpProject.Items
					   if CType(dgItem.Cells(13).Controls(1), Button).visible=true
						  intCheck=1
					      exit for
				        end if
				   next
				   if intCheck<>1 then
					   dgEmpProject.Columns(13).Visible =false 
				   end if
				   
				   If  e.Item.ItemType <> ListItemType.Header AND  e.Item.ItemType <> ListItemType.Footer Then
		   				If ( e.item.cells(16).text <> "&nbsp;" and e.item.cells(5).text <> "&nbsp;" )

							 Dim compDays as integer
					         compDays = DateDiff(DateInterval.Day, CDate(e.item.cells(4).text), CDate(e.item.cells(5).text))
							 Dim nowDays as integer
							 nowDays = DateDiff(DateInterval.Day, CDate(e.item.cells(4).text),CDate(e.Item.Cells(16).text))

					 		if ( CDate(e.item.cells(16).text)  > CDate(e.item.cells(5).text) ) Then
								e.item.cells(9).text="100%"
							Else
								e.item.cells(9).text = Math.Round(((100/compDays)*nowDays), 2) & "%"
					        End If
				        End If
				
					  
					   Dim view as  Button = CType(e.Item.Cells(12).FindControl("view"), Button)
					   if e.Item.Cells(9).text<>"&nbsp;" then
							 view.Attributes.Add("onclick", "popupProjectDetail("& e.Item.Cells(0).text  &",'"& e.Item.Cells(9).text  &"'); return false;")
					   else
							 view.Attributes.Add("onclick", "popupProjectDetail("& e.Item.Cells(0).text  &",'N'); return false;")
					   end if
				
					   Dim proStatus as  Button = CType(e.Item.Cells(13).FindControl("proStatus"), Button)
					   if e.Item.Cells(7).text<>"&nbsp;" then
							proStatus.Attributes.Add("onclick", "popupProjectStatus("& e.Item.Cells(0).text  &",'"& e.Item.Cells(7).text  &"'); return false;")
					   else
							proStatus.Attributes.Add("onclick", "popupProjectStatus("& e.Item.Cells(0).text  &",'"& 1  &"'); return false;")
					   end If

					     Dim feedback as  Button = CType(e.Item.Cells(16).FindControl("btnfeedback"),Button)
						 feedback.Attributes.Add("onclick", "popupProjectDetailView("& e.Item.Cells(0).Text  &"); return false;")
				   End If 
			
				   If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
					
						Dim cellValue as integer = DataBinder.Eval(e.Item.DataItem, "projid")
	               Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
						Dim conn As SqlConnection = New SqlConnection(dsn1)
						Conn.Open()
						Dim strsqlDT as string="select empname  from employeemaster where empid in (select empid  from projectmember where projid=" & cellvalue & ") and empLeavingDate is NULL"
						Dim dtr As SqlDataReader
						Dim cmd As SqlCommand = New SqlCommand(strsqlDT,conn)
						Dim st as string
						dtr=Cmd.ExecuteReader()
						Dim i as integer
						st= "" 
						   While dtr.read 
								If st = "" Then 
									st =  "- " & dtr(0)
								Else
									st = st & "<BR>- " & dtr(0)
								End If 		
								i=i+1
						   end while		
						e.item.cells(10).text=st
						dtr.close
						

						'new  code 
						Dim strEmpSQL as String 
						Dim cmdst as sqlcommand
						Dim strData as String 
						Dim dtrst As SqlDataReader
						Dim strCSVData as  String 
						Dim arrEmp 
						Dim strempid as String
						If e.item.cells(11).text <> "" Or e.item.cells(11).text <> "Null" then 
							strCSVData = e.item.cells(11).text 
							arrEmp =  Split(strCSVData,",")
							strCSVData = ""
							For i= 0 To UBound(arrEmp)
								If arrEmp(i) <> ""  Then
									strSQL ="select empname  from employeemaster where  empid = '"& arrEmp(i)  &"'" 
									cmdst = New SqlCommand(strSQL, Conn)
                     				dtrst=Cmdst.ExecuteReader()	
									'response.write(strSQL)

									If dtrst.read  Then
										If strCSVData = "" Or  strCSVData="&nbsp" Then 
											strCSVData =  "- " & dtrst("empname")
										Else
											strCSVData = strCSVData & "<BR>- " & dtrst(0)
										End If 
									Else 
									e.item.cells(11).text=""
									End If
									dtrst.close()
									cmdst.dispose()
								End If 
							Next 
							e.item.cells(11).text=strCSVData
						Else
							e.item.cells(11).text=""
						End If 
						'response.End 
						' back up code 
						Dim strCRT1 as String
					'	Dim cmdst as sqlcommand
				'		Dim dtrst As SqlDataReader
					 '	Dim dsn1 As String = ConfigurationSettings.AppSettings("dsn")
						'Dim conn As SqlConnection = New SqlConnection(dsn1)
						'Dim j as integer
						'Dim strempid as String
						'Dim strSQL as string

						'If e.item.cells(11).text <> "" Or e.item.cells(11).text <> "Null" then 
						'strempid=e.item.cells(11).text 
						'Dim araay1
						'araay1 = Split(strempid,",")
						'strSQL="select empname  from employeemaster where 1=1 "
						'For i= 0 To UBound(araay1)
						'	If araay1(i) <> ""  Then
						'		strSQL = strSQL  &  "  and empid = '"& araay1(i)  &"' " 
						'	End If 
						'Next 
					  '  Else
						'	strSQL = "select empname  from employeemaster where empid in (0)"
						'End If
						'response.write(strSQL)
						'response.End 
						'cmdst = New SqlCommand(strSQL, Conn)
                      ' Conn.Open()						
						'dtrst=Cmdst.ExecuteReader()					
						'Dim st1 as string
                      '   st1= "" 
						 '  While dtrst.read 
						''		If st1 = "" Then 
							'		st1 =  "- " & dtrst(0)
							'	Else
								'	st1 = st1 & "<BR>- " & dtrst(0)
								'End If 		
							'	j=j+1
						   'end while		
						   'Response.Write(st1)
						  ' response.End()
						'e.item.cells(11).text=st1
						'dtrst.close
						'conn.close
						
						
						
						' end cback up code 

			       
				   End If

				    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

					
				End if

		 End Sub

		Sub check_ProjectManger(byval intPrjId as integer)

	           Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
		  Dim conn As SqlConnection = New SqlConnection(dsn1)
		  Dim dtrprojManag As SqlDataReader
		  Dim cmdProjManag As SqlCommand
		  intProjManagerId=0
		  strSQL="SELECT projId FROM projectmaster where projManager="& trim(empId) &" and projId="& trim(intPrjId) &""

		  cmdProjManag=new SqlCommand(strSQL,conn)
		  conn.open()
		  dtrprojManag=cmdProjManag.executereader()
		  while dtrprojManag.read
			  if isDbnull(dtrprojManag("projId"))=false then
					intProjManagerId=1
			  end if
		  end while
		  
		  conn.close()

		End Sub
		'============================================
		'END CHECK EMPLOYEE IS PROJECT MANAGER OR NOT
		'============================================
 
 
	   </script>
	</HEAD>
	<body>
		<form id="Form1" runat="server">
			<table height="0" cellSpacing="0" cellPadding="4" width="100%" border="0" style="border-collapse: collapse" bordercolor="#111111" align="center">
				<tr>
						<td>
							<table id="Table3" cellSpacing="0" cellPadding="2" width="100%" border="0" height="1">
								<tr>
									<td>
										<EMPHEADER:EMPHEADER id="Empheader" runat="server">
                                        </EMPHEADER:EMPHEADER></td>
								</tr>
								<tr>
									<td>
										<uc1:empMenuBar id="EmpMenuBar" runat="server">
                                        </uc1:empMenuBar></td>
								</tr>
							</table>
						</td>
				</tr>
		<tr>
				  <td >
					<table border="1" cellspacing="1" bordercolorlight="#000000" bordercolordark="#FFFFFF" width="100%">
						<tr>
							<td align="center" bgcolor="#C5D5AE"><font face="Verdana" color="#a2921e"><b>My Projects</b></font></td>
						</tr>
						<tr>
								<td colspan="4" align="right" bgcolor="#edf2e6"><b><font face="Verdana"  size="2" >
								<p align="left"><a href="empProject.aspx?search=Current"><font color="#A2921E">Current</font>
									</a>|<a href="empProject.aspx?search=Completed"><font color="#A2921E">Completed</font>
									</a>|<a href="empProject.aspx?search=Cancelled"><font color="#A2921E">Canceled</font></a>|
									<a href="empProject.aspx?search=All"><font color="#A2921E">All</font></a> 
									<a href="empDetail.aspx"></a></td>
						</tr>

					</table>					 
					<ASP:DataGrid id="dgEmpProject" runat="server" 	AllowSorting="true"  Width="100%" BackColor="white"  BorderColor="black"  ShowFooter="True" adding=2 CellSpacing="0" ShowHeader="true" Font-Name="Verdana" Font-Size="10pt" Headerstyle-BackColor="LightGray" Headerstyle-Font-Size="10pt" MaintainState="true" Footerstyle-HorizontalAlign="Right" 		AutoGenerateColumns="false"  OnItemDataBound="dgEmpProject_ItemDataBound">					
				 
				   <ItemStyle ForeColor="#000000" BackColor="#FFFFEE" VerticalAlign="top"></ItemStyle>
				   <HeaderStyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100%"></HeaderStyle>
				   <FooterStyle ForeColor="#a2921e" BackColor="#edf2e6"></FooterStyle>
				   <Columns>

					 <asp:BoundColumn visible="false" DataField="projId"/>

					 <asp:BoundColumn DataField="projName" HeaderText="Project  Title" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>

					 <asp:BoundColumn DataField="custName" HeaderText="Customer Name"  HeaderStyle-HorizontalAlign=Center visible="False" ></asp:BoundColumn>

					 <asp:BoundColumn DataField="empName" HeaderText="Project Manager" HeaderStyle-HorizontalAlign=Center ></asp:BoundColumn>

					 <asp:BoundColumn HeaderText="Start Date" DataField="projStartDate" DataFormatString = "{0:dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>

					 <asp:BoundColumn HeaderText="Exp Comp Date" DataField="projExpComp" DataFormatString = "{0:dd-MMM-yyyy}" HeaderStyle-HorizontalAlign=Center></asp:BoundColumn>
					

					 <asp:BoundColumn HeaderText="Act Comp Date" DataField="projActComp" DataFormatString = "{0:dd-MMM-yyyy}" HeaderStyle-HorizontalAlign=Center> </asp:BoundColumn>

					 <asp:BoundColumn HeaderText="Status" DataField="projStatusTDesc"  ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"></asp:BoundColumn>

					 <asp:BoundColumn HeaderText="Actual/Expected Completed" DataField=""  ItemStyle-HorizontalAlign="Left" >
					 </asp:BoundColumn>

					 <asp:BoundColumn HeaderText="Expected Completion" DataField="ExpCompleted"  ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" visible= False > </asp:BoundColumn>

					 <asp:BoundColumn HeaderText="Code Developer"></asp:BoundColumn>

                    <asp:BoundColumn HeaderText="Code Review" DataField="codeRevTeam"  ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle"></asp:BoundColumn>
					
                    <asp:BoundColumn HeaderText="Overall Rating"  ItemStyle-VerticalAlign="middle"    ItemStyle-HorizontalAlign="right" > </asp:BoundColumn>

					 <asp:TemplateColumn HeaderText="">
					 <ITEMSTYLE wrap="false" VerticalAlign="Middle"></ITEMSTYLE>
					 <ItemTemplate>
					 <asp:Button id="view" runat="server" CommandName="view"  Text="View" align="center" 
					  style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" font-bold="true" width="60">
					 </asp:Button>
					 </ItemTemplate>
					 </asp:TemplateColumn>

					 <asp:TemplateColumn HeaderText="">
					 <ITEMSTYLE wrap="false" VerticalAlign="Middle"></ITEMSTYLE>
					 <ItemTemplate>
					 <asp:Button id="proStatus" runat="server" CommandName="status" Text="Status" align="center" 
					  style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" font-bold="true" width="60">
					 </asp:Button>
					 </ItemTemplate>
					 </asp:TemplateColumn>  
					 
					 <asp:BoundColumn HeaderText="" DataField="projStatus"  ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"  Visible="false" />    
					 <asp:BoundColumn HeaderText="" DataField="proStatusDate" DataFormatString = "{0:dd-MMM-yyyy}" ItemStyle-HorizontalAlign="Center"  Visible="false"/>  


					 <asp:TemplateColumn HeaderText="">
					 <ITEMSTYLE wrap="false" VerticalAlign="Middle"></ITEMSTYLE>
					 <ItemTemplate>
					 <asp:Button id="btnfeedback" runat="server" CommandName="cmdFeedback" Text="Rating" align="center" 
					  style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" font-bold="true" width="70">
					 </asp:Button>
					 </ItemTemplate>
					 </asp:TemplateColumn> 
					</Columns>
				
				   </ASP:DataGrid>

				   <asp:label id="lbl" runat="server" ></asp:label>
				  </td>
				</tr>
			</table>
		</form>
	</body>
    </html>