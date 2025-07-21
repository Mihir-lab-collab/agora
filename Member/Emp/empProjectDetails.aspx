<%@ Page Language="VB" %>
<%@ import Namespace="System.Data" %>
<%@import Namespace="System.Data.SqlClient"%>
<%@import Namespace="System.Drawing"%>

<html xmlns="http://www.w3.org/1999/xhtml" >

<head>
<meta name="GENERATOR" Conntent="Microsoft FrontPage 5.0" content="Microsoft FrontPage 5.0">
<meta name="ProgId" Conntent="FrontPage.Editor.Document" content="FrontPage.Editor.Document">
<meta http-equiv="Conntent-Type" Conntent="text/html; charset=windows-1252">
<title>Employee Project Details1</title>

</head>

<script language="VB" runat="server">

   Dim intProjId as integer
    
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
   Dim Conn As SqlConnection = New SqlConnection(dsn)
   Dim dtrItemDesc as SqlDataReader
   Dim cmdProjDetail as  SQLCommand
   Dim strProjDetail as string
    Dim dtrProjDetail As SqlDataReader
    Dim gf As New generalFunction
'================================================================
 'PAGE LOAD TO SEE ALL THE PROJECT DETAILS AT PAGE LOAD
 '================================================================
  Sub Page_Load(sender As object ,e As System.EventArgs)
        gf.checkEmpLogin()
	   intProjId = cint(Request.QueryString("projId"))
		     If Not IsPostBack Then
					
					
					if cStr(Session("dynoEmpIdSession")) = "" Then
						Response.Redirect ("emplogin.aspx")
					End If
				        show_Details()
				        show_ProjectMembers()
					       BindGrid()
			End if
		
			
	End Sub
	
	
'================================================================
 'FUNCTION FOR DISPLAY PROJECT DETAILS AT PAGE LOAD
 '================================================================
	Sub show_Details()
	     
		Dim intStartDate as date
		Dim intEndDate as date
			 Conn.Open()
			 
        strProjDetail = "Select PM.projName,PM.projDuration,PM.projStartDate,projStartDate+[dbo].[funprojectday](projDuration,projStartDate) " & _
    " AS projExpComp,PM.projActComp,CM.custId,CM.custName,CM.custCompany,CM.custAddress,EM.empid,EM.empName " & _
             " from projectMaster PM,customerMaster CM,employeeMaster EM " & _
             " where CM.custId=PM.custId and EM.empid=PM.projManager and PM.projId=" & Trim(intProjId) & ""
			 
			
			 cmdProjDetail=New SqlCommand(strProjDetail,Conn)
			 dtrProjDetail=cmdProjDetail.ExecuteReader
			
			 if dtrProjDetail.Read then

                lblprjName.Text = dtrProjDetail("projName")
                lblCustName.text=dtrProjDetail("custName") &"("& dtrProjDetail("custId") &")"
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

           
                lblProjMang.text=dtrProjDetail("empName") &"("& dtrProjDetail("empId") &")"
                lblStartDate.Text =Day(dtrProjDetail("projStartDate")) & "-" & MonthName(Month(dtrProjDetail("projStartDate")),2) & "-" & Year(dtrProjDetail("projStartDate")) &""
                lblExpDate.text=Day(dtrProjDetail("projExpComp")) & "-" & MonthName(Month(dtrProjDetail("projExpComp")),2) & "-" & Year(dtrProjDetail("projExpComp")) &""

'intStartDate=CDate(lblStartDate.Text)
'response.write(intStartDate)
'intEndDate =CDate(lblExpDate.text)
'response.write(intEndDate)
'response.End()

              if isDbNull(dtrProjDetail("projActComp"))=false then
                lblActCompDate.text=Day(dtrProjDetail("projActComp")) & "-" & MonthName(Month(dtrProjDetail("projActComp")),2) & "-" & Year(dtrProjDetail("projActComp")) &""

               end if
			 end if
			 

			
			

			 cmdProjDetail.dispose
			 dtrProjDetail.close
			Conn.close

			 Conn.Open()


				Dim compDays as integer
				compDays = DateDiff(DateInterval.Day,  intStartDate, intEndDate)
				Dim nowDays as integer
				nowDays = DateDiff(DateInterval.Day, intStartDate ,DateAndTime.Now() )

			'response.write(trim(intProjId))
			'response.End()
			strProjDetail="select * from  projectStatus where projId="& trim(intProjId) &" and proStatusDate=(Select max(proStatusDate) from projectStatus where projId="& trim(intProjId) &") " 
			'response.write(strProjDetail)
			'response.End()
'			strProjDetail="select * from projectStatus where projStatusId " & _
'			" =( select projStatusId from projectStatus WHERE " & _
'              " prostatusdate =( select Max(prostatusdate) from " & _
'                "projectStatus GROUP BY projId))and projId="& intProjId &""
			
			
			 cmdProjDetail=New SqlCommand(strProjDetail,Conn)
			 dtrProjDetail=cmdProjDetail.ExecuteReader
			
			 if dtrProjDetail.Read then
               
              
				  if trim(dtrProjDetail("projStatus"))=100 then
							lblprojStatus.text="Completed"
					 else
							  lblprojStatus.text=trim(dtrProjDetail("projStatus")) & "% as on "& Day(dtrProjDetail("proStatusDate")) & "-" & MonthName(Month(dtrProjDetail("proStatusDate")),2) & "-" & Year(dtrProjDetail("proStatusDate"))
               
                         if Request.QueryString("expCompleted")<>"N" then
							 lblExpProjStatus.Text=Request.QueryString("expCompleted") & " as on " & Day(dtrProjDetail("proStatusDate")) & "-" & MonthName(Month(dtrProjDetail("proStatusDate")),2) & "-" & Year(dtrProjDetail("proStatusDate"))
						 
			 'if cint(trim(Request.QueryString("expCompleted"))) > cint(trim(dtrProjDetail("projStatus"))) then
						    lblprojStatus.BackColor= Color.Red
						' end if
						 
						 else
							  lblExpProjStatus.Text=""
						 end if
                    end if
               end if
			
			 
			 cmdProjDetail.dispose
			 dtrProjDetail.close
			Conn.close
		
	End Sub


'================================================================
 'PROCEDURE FOR BIND PROJECT MEMBERS AT PAGE LOAD
 '================================================================
Sub show_ProjectMembers()
		  Dim di As DataGridItem
		  Dim daProjDetail as SqlDataAdapter
		  Dim dsProjMem as new dataset
		  conn.open()
		  
            daProjDetail = New SqlDataAdapter("select empId,empName +'('+ CONVERT(varchar, empId) +')' as empName from employeeMaster where empId in " & _
				"(select empId from projectMember where projId="& trim(intProjId) &")and empLeavingDate is null group by empId,empName", conn)
       
            Dim dt As DataTable = New DataTable("table1")
            Dim dc As DataColumn = New DataColumn("srno", GetType(Int32))
            dc.AutoIncrement = True
            dc.AutoIncrementSeed = 1
           dt.Columns.Add(dc)
            dsProjMem.Tables.Add(dt)
            daProjDetail.Fill(dsProjMem, "table1")
            dgProjectTeamMem.DataSource = dsProjMem
            dgProjectTeamMem.DataBind()
            'dgProjectTeamMem.DataKeyField = "empName"
            '"DataField="empName & empId" "
           
           conn.close()
	End Sub

'=============================
 'PAGING FOR PROJECT MEMBERS 
 '=============================
 
       '  Sub dgProjectTeamMem_PageIndexChanged(ByVal source As Object, ByVal e As 'System.Web.UI.WebControls.DataGridPageChangedEventArgs)
		'	dgProjectTeamMem.CurrentPageIndex = e.NewPageIndex
         '   show_ProjectMembers()
       
        'End Sub
        
    '================================================================
 'PROCEDURE FOR BIND PROJECT STATUS AT PAGE LOAD
 '================================================================    
        
		Sub BindGrid()
		  
'strProjDetail="SELECT *,DATEADD(m,projDuration,projStartDate) " & _ 
'			" AS projExpComp,(100*(DATEDIFF(day, projStartDate, 'PS.proStatusDate)))/DATEDIFF(day,DateADD(m,projDuration,projStartDate),PS.proStatusDate)as ExpCompleted FROM 'projectMaster PM LEFT JOIN " & _
'           "(select 'PST.projId,PSTM.projStatusTDesc,PST.projStatusId,PST.projStatus,PST.projStatusTId,PST.proStatusDate " & _
'           "from projstatustypemaster PSTM inner join (select * from projectStatus where projStatusId " & _
'          "in( select projStatusId from projectStatus WHERE " & _
'        "prostatusdate in( select Max(prostatusdate) from projectStatus GROUP BY projId)))PST " & _
'     "On PSTM.projStatusTId=PST.projStatusTId) PS ON PM.projId=PS.projId " & _
'" inner join customerMaster on PM.custId=customerMaster.custId inner join employeeMaster " & _
'" on PM.projManager=employeeMaster.empid ORDER BY projStartDate Desc "

Conn.Open()
Dim strdate as String
Dim diffdate as datetime
strdate="select top 1 prostatusdate ,projStartDate,projDuration from projectmaster as a, projectStatus as b " & _ 
"where a.projid=b.projid and a.projid= " & intProjId & " order by proStatusDate desc "

Dim datedtr as sqldatareader
Dim cmd as sqlcommand
cmd = New SqlCommand(strdate, conn)

        datedtr = cmd.ExecuteReader

dim startdate as datetime
Dim statusdate as datetime
 Dim duration as integer
        If datedtr.Read Then
		    startdate=(datedtr("projstartdate"))
			duration = datedtr("projduration")
		    statusdate = convert.Todatetime(datedtr("proStatusDate"))
            
		End If
		datedtr.close()	
            diffdate= startdate.addmonths(duration) 
            'response.write(statusdate)
			'response.write(diffdate.toshortdatestring)
			'response.End()
      ' If ( diffdate.toshortdatestring = statusdate.toshortdatestring ) Then
        'response.write(diffdate.toshortdatestring)
		'response.end
		'response.write(statusdate.toshortdatestring)
   try
          strProjDetail = "SELECT projStartDate,projDuration,proStatusDate,projStatus,projRemark,(100*(DATEDIFF(day,projStartDate,projectStatus.proStatusDate))),projectStatus.proStatusDate" & _
          " as ExpCompleted FROM projectMaster  LEFT JOIN  projectStatus on projectMaster.projId=projectStatus.projId where projectStatus.projId=" & intProjId & " order by projectStatus.proStatusDate desc "
      ' else
       
     '  End if
            'Response.Write(strProjDetail)
            'Response.End()
		    
		    
			Dim cmdProjDetail As SqlCommand = New SqlCommand(strProjDetail,conn)
			cmdProjDetail.parameters.add("@projId",intprojId)
			dtrProjDetail=cmdProjDetail.ExecuteReader()

			dgProjectDetail.DataSource = dtrProjDetail
		     dgProjectDetail.DataBind()
			
			Conn.close()
        Catch ex As Exception
            
            Conn.Open()
            strProjDetail = "SELECT projStartDate,projDuration,proStatusDate,projStatus,projRemark,(100*(DATEDIFF(day,projStartDate,projectStatus.proStatusDate)))/DATEDIFF(day,projStartDate+[dbo].[funprojectday](projDuration,projStartDate),projectStatus.proStatusDate)" & _
                " as ExpCompleted FROM projectMaster  LEFT JOIN  projectStatus on projectMaster.projId=projectStatus.projId where projectStatus.projId=" & intProjId & " order by projectStatus.proStatusDate desc"


            Dim cmdProjDetail As SqlCommand = New SqlCommand(strProjDetail, Conn)
            cmdProjDetail.Parameters.Add("@projId", intProjId)
            dtrProjDetail = cmdProjDetail.ExecuteReader()

            dgProjectDetail.DataSource = dtrProjDetail
            dgProjectDetail.DataBind()
			
            Conn.Close()

  End try
        End Sub
		



    Sub dgProjectDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) 


 If  e.Item.ItemType <> ListItemType.Header AND  e.Item.ItemType <> ListItemType.Footer Then
 ' response.write("<br>" & (e.item.cells(3).text))

				If ( e.item.cells(1).text <> "&nbsp;" and e.item.cells(2).text <> "&nbsp;" )
				 Dim compDays as integer
				compDays = DateDiff(DateInterval.Day, CDate(lblStartDate.Text),  CDate(lblExpDate.text) )
				
				 Dim nowDays as integer
				nowDays = DateDiff(DateInterval.Day, CDate(lblStartDate.Text) ,DateTime.Now() )
				
				 if ( CDate(e.item.cells(0).text)  > lblExpDate.text ) Then
					e.item.cells(1).text="100%"
				Else 
				Dim daysnow as integer
				daysnow=  DateDiff(DateInterval.Day,CDate(lblStartDate.Text) ,CDate(e.item.cells(0).text))
				Dim calc as double
				calc=(daysnow*(100/compDays))
				'response.End()
					e.item.cells(1).text = Math.Round(calc, 2) & "%"
				End if

                  e.item.cells(2).text= e.item.cells(2).text & "%"


		End If
End If
		
		


	End Sub
	
      
     '=====================================
      'CALL BINDGRID FOR PAGING 
    '======================================
  ' Sub dgProjectDetail_PageIndexChanged(ByVal source As Object, ByVal e As 'System.Web.UI.WebControls.DataGridPageChangedEventArgs)
       ' dgProjectDetail.CurrentPageIndex = e.NewPageIndex
     ' '  BindGrid()
               
   ' End Sub




</script>








<BODY  topmargin="0" leftmargin="0" rightmargin="0" bottommargin="0">
<form id="Form1" runat="server">
	<!--<br>-->
	<TABLE cellSpacing="1" cellPadding="4" width="100%"  border="0" bordercolor="#F1F4EC">
      <tr>
			<td align="center" bgcolor="#C5D5AE" colspan="4"><font face="Verdana" color="#a2921e"><b>Project Details</b></font></td>
		</tr>
 	<tr>
		<td bgcolor="#C5D5AE" nowrap="nowrap" width="150" valign="top">
        <font style="color: #A2921E; font-family: Arial" size="4" ><b>Project Name:</b></font></TD>
		<TD bgcolor="#edf2e6" width="60%" valign="top"><font face="verdana" size="4"><b><asp:label id="lblprjName" runat="server" ></asp:label></b></font></TD>
	   	<td bgcolor="#C5D5AE" nowrap="nowrap"  width="150" valign="top"><b>
        <font style="font-size: 10pt; color: #A2921E; font-family: Arial">Project Status:</font></b></TD>
		<TD bgcolor="#edf2e6"  width="60%" valign="top"><font face="verdana" size="2"><asp:label id="lblprojStatus" runat="server" ></asp:label></font></TD>
		
   </tr>
	     <tr>
			<td  bgcolor="#C5D5AE" valign="top" nowrap="nowrap" width="150" >
            <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Customer Name:</b></font></TD>
			<TD bgcolor="#edf2e6" width="35%" valign="top" ><font face="verdana" size="2"><asp:label id="lblCustName" runat="server"></asp:label></font></TD>
		  	<td bgcolor="#C5D5AE" nowrap="nowrap"  width="150" valign="top"><b>
        <font style="font-size: 10pt; color: #A2921E; font-family: Arial">Exp Project Status:</font></b></TD>
		<TD bgcolor="#edf2e6"  width="35%" valign="top"><font face="verdana" size="2"><asp:label id="lblExpProjStatus" runat="server" ></asp:label></font></TD>
		
		</tr>
		<tr>
			<td bgcolor="#C5D5AE" valign="top" nowrap="nowrap" align="left" width="150">
            <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Customer Address:</b></font></TD>
			<TD  bgcolor="#edf2e6" width="35%" valign="top" ><font face="verdana" size="2"><asp:label id="lblCustAddress" runat="server"></asp:label></font></TD>
		    <td bgcolor="#C5D5AE" valign="top" nowrap="nowrap" width="150">
           <font style="color: #A2921E; font-family: Arial"><b>
           <font style="font-size: 10pt; color: #A2921E; font-family: Arial">Start 
           Date:</font></b><font size="2">
           </font>
		   </font>
		   </TD>
		   <TD  bgcolor="#edf2e6" width="35%" valign="top" ><font face="verdana" size="2"><asp:label id="lblStartDate" runat="server" ></asp:label></font></TD>
		
		</tr>
		<TR>
		   <td  bgcolor="#C5D5AE" valign="top" nowrap="nowrap" align="left" width="150">
           <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Project Duration:</b> </font>
		  </TD>
    	   <TD  bgcolor="#edf2e6" width="35%" valign="top" ><font face="verdana" size="2"><asp:label id="lblProjDurat" runat="server"></asp:label></font></TD>
		   <td  bgcolor="#C5D5AE" valign="top" nowrap="nowrap" width="150"><b>
           <font style="font-size: 10pt; color: #A2921E; font-family: Arial">Exp Comp Date: </font>
           </b>
		  <TD  bgcolor="#edf2e6" width="35%" valign="top"><font face="verdana" size="2"><asp:label id="lblExpDate" runat="server"></asp:label></font></TD>
		  
		</TR>

		<TR>
		   <td  bgcolor="#C5D5AE" valign="top" nowrap="nowrap" align="left" width="150" >
           <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Project Manager:</b> </font>
		  </TD>
    	   <TD  bgcolor="#edf2e6" width="35%" valign="top" ><font face="verdana" size="2"><asp:label id="lblProjMang" runat="server"></asp:label></font></TD>
		   <td  bgcolor="#C5D5AE" valign="top" nowrap="nowrap" width="150"><b>
           <font style="font-size: 10pt; color: #A2921E; font-family: Arial">Act Comp Date: </font>
           </b>
		  </TD>
		  <TD  bgcolor="#edf2e6" width="35%" valign="top" ><font face="verdana" size="2"><asp:label id="lblActCompDate" runat="server" ></asp:label></font></TD>
		</TR>
           <tr>
	            <td  colspan="4" align="center" bgcolor="#F1F4EC">&nbsp;</td>
				</tr>
           <tr>
	            <td  colspan="4" align="left" bgcolor="#F1F4EC"><font face="Verdana" color="#a2921e" size="2"><b>Project Team Member</b></font><b>
                </b>
					</td>
				</tr>
				<tr>
					<TD  align="left" bgcolor="#edf2e6" valign="top" width="100%" colspan="2">
					      
						  <ASP:DATAGRID id="dgProjectTeamMem" runat="server" BorderColor="Black" Font-Size="10pt" Font-Name="Verdana"
							BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"	Headerstyle-Font-Size="10pt" Headerstyle-BackColor="LightGray" CellPadding="2"
							width="100%" >
								<ItemStyle ForeColor="#000000" BackColor="#FFFFEE" VerticalAlign="Top"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100%"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="srno" HeaderText="Sr" 
										HeaderStyle-HorizontalAlign=Center ItemStyle-HorizontalAlign=Center>
									<HeaderStyle></HeaderStyle>
									</asp:BoundColumn>
														
									<asp:BoundColumn HeaderText="Employee" DataField="empName">
									<HeaderStyle></HeaderStyle>
									</asp:BoundColumn>
									
								</Columns>
							</ASP:DATAGRID>
						 </td>
						 <%
				   dim fldproject_id = cint (Request.QueryString("projId")) %>

       <%
	   Dim sSQL as String
	   Dim daReport as SqlDataAdapter
	   Dim cmd as sqlcommand
	   Dim ds as new dataset
	   Dim dr as  sqldatareader
	   Dim bugCount as integer
	    Dim statusArr(5) as integer
           Dim priorityArr(5) As Integer


		if isNumeric(fldproject_id ) Then

               'sSQL = "SELECT count(*) as bugCount FROM bugs WHERE project_id=" & fldproject_id
               sSQL = "SELECT count(*) as bugCount FROM bugs WHERE moduleid in (select moduleid from projectModuleMaster where projid=" & fldproject_id & ")"
			conn.open()
			cmd=new sqlcommand(sSQL,conn)
			bugcount=cmd.executescalar() 
			cmd.dispose()		

               'sSQL = "SELECT bugs.priority_id, count(*) as priorityCount FROM bugs,bugPriorities " & _
               '"WHERE bugs.priority_id=bugPriorities.priority_id AND project_Id=" & fldproject_id & _
               '" GROUP BY bugs.priority_Id " & _
               '"ORDER BY bugs.priority_id"
			
               sSQL = "SELECT bugs.priority_id, count(*) as priorityCount FROM bugs,bugPriorities " & _
   "WHERE bugs.priority_id=bugPriorities.priority_id AND moduleid in (select moduleid from projectModuleMaster where projid=" & fldproject_id & ")" & _
   " GROUP BY bugs.priority_Id " & _
   "ORDER BY bugs.priority_id"
               Dim pri, pricount, i
			
               cmd = New SqlCommand(sSQL, Conn)
               dr = cmd.ExecuteReader()
			
          
               While dr.Read()
                   'ReDim preserve priorityArr(dr("priority_Id"))
						
                   priorityArr(dr("priority_Id")) = (dr("priorityCount"))

               End While
			

               dr.Close()

		
               ' sSQL = "SELECT status,bugs.status_id, count(*) as statusCount FROM bugs,bugStatuses " & _
               '"WHERE bugs.status_id=bugStatuses.status_id AND project_Id=" & fldproject_id & _
               '" GROUP BY status,bugs.status_Id " & _
               '"ORDER BY bugs.status_id"
            
               sSQL = "SELECT status,bugs.status_id, count(*) as statusCount FROM bugs,bugStatuses " & _
               "WHERE bugs.status_id=bugStatuses.status_id AND bugs.moduleid in (select moduleid from projectModuleMaster where projid=" & fldproject_id & ")" & _
               " GROUP BY status,bugs.status_Id " & _
               "ORDER BY bugs.status_id"
               'response.write(sSQL)
               'response.End()
    
		 
               Dim stId, stcount
               Dim dr1 As SqlDataReader
               cmd = New SqlCommand(sSQL, Conn)
               dr1 = cmd.ExecuteReader()
		  
			
               While dr1.Read
                   'ReDim preserve statusArr(dr1("status_id"))
                   statusArr(dr1("status_id")) = (dr1("statusCount"))
						
               End While

       

               dr1.Close()
               Conn.Close()
           End If

%>





						 
      <td   align="left" bgcolor="#edf2e6" valign="top" width="100%" colspan="2"> 
        <table width="100%" border="1" cellspacing="2" cellpadding="2">
          <tr>
            <td>
              <table border="1" cellspacing="1" bordercolorlight="#000000" bordercolordark="#FFFFFF">
                <tr>
                  <td align="center" bgcolor="#C5D5AE" colspan="6" nowrap="nowrap"> <a name="Search"><b><font face="Verdana" color="#A2921E"> 
                   Task Report</font></b></a></td>
                </tr>
                <tr> 
                  <td bgcolor="#edf2e6" align="left" colspan="4" nowrap="nowrap"> <b><font face="Verdana" size="2" color="#A2921E">Total 
                    Task Reported</font></b></td>
                  <td colspan="2" align="right" nowrap="nowrap"> <font face="Arial" size="2"><%=bugCount%></font>&nbsp;</td>
                </tr>
                <tr> 
                  <td bgcolor="#edf2e6" align="left" colspan="4" nowrap="nowrap"> <b><font face="Verdana" size="2" color="#A2921E">Total 
                    Tasks Resolved (Terminated)</font></b></td>
                  <td colspan="2" align="right" nowrap="nowrap"> <font face="Arial" size="2"><%=statusArr(5)%></font>&nbsp;</td>
                </tr>
                <tr>
                  <td bgcolor="#edf2e6" align="left" rowspan="4"> <font face="Verdana" color="#a2921e" size="2"><b>Priority 
                    Wise</b></font></td>
                  <td bgcolor="#edf2e6" align="left" nowrap="nowrap"> <b><font face="Verdana" size="2" color="#A2921E">Showstopper</font></b></td>
                  <td width="50" nowrap="nowrap" align="right"> <font face="Arial" size="2"><%=priorityArr(2)%></font>&nbsp;</td>
                  <td rowspan="4" bgcolor="#edf2e6" align="left"> <font face="Verdana" size="2" color="#A2921E"><b>Status 
                    Wise</b></font></td>
                  <td bgcolor="#edf2e6" align="left" nowrap="nowrap"> <font face="Verdana" size="2" color="#A2921E"><b>Open</b></font></td>
                  <td width="50" nowrap="nowrap" align="right"> <font face="Arial" size="2"><%=statusArr(1)%></font>&nbsp;</td>
                </tr>
                <tr>
                  <td bgcolor="#edf2e6" align="left" nowrap="nowrap"> <b><font face="Verdana" size="2" color="#A2921E">Major</font></b></td>
                  <td width="50" nowrap="nowrap" align="right"> <font face="Arial" size="2"><%=priorityArr(3)%></font>&nbsp;</td>
                  <td bgcolor="#edf2e6" align="left" nowrap="nowrap"> <font face="Verdana" size="2" color="#A2921E"><b>In 
                    Progress</b></font></td>
                  <td width="50" nowrap="nowrap" align="right"> <font face="Arial" size="2"><%=statusArr(4)%></font>&nbsp;</td>
                </tr>
                <tr>
                  <td bgcolor="#edf2e6" align="left" nowrap="nowrap"> <b><font face="Verdana" size="2" color="#A2921E">Cosmetic</font></b></td>
                  <td width="50" nowrap="nowrap" align="right"> <font face="Arial" size="2"><%=priorityArr(4)%></font>&nbsp;</td>
                  <td bgcolor="#edf2e6" align="left" nowrap="nowrap"> <font face="Verdana" size="2" color="#A2921E"><b>Completed</b></font></td>
                  <td width="50" nowrap="nowrap" align="right"> <font face="Arial" size="2"><%=statusArr(3)%></font>&nbsp;</td>
                </tr>
                <tr> 
                  <td bgcolor="#edf2e6" align="left" nowrap="nowrap"> <b><font face="Verdana" size="2" color="#A2921E">Minor</font></b></td>
                   <td width="50" nowrap="nowrap" align="right"> <font face="Arial" size="2"><%=priorityArr(5)%></font>&nbsp;</td>
                  <td bgcolor="#edf2e6" align="left" nowrap="nowrap"> <b><font face="Verdana" size="2" color="#A2921E">On 
                    Hold</font></b></td>
                  <td width="50" nowrap="nowrap" align="right"> <font face="Arial" size="2"><%=statusArr(2)%></font>&nbsp;</td>
                </tr>
                <tr> 
                  <td align="right" colspan="6">&nbsp; </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td>
			<!--<iframe id="iFrameChart" style="BORDER-RIGHT: 2px; BORDER-TOP: 2px; BORDER-LEFT: 2px; WIDTH: 395px; BORDER-BOTTOM: 2px; HEIGHT: 250px" name="iFrameChart" align="middle" src="../test.asp" frameborder="No" scrolling="no" runat="server"></iframe>--></td>
          </tr>
        </table>
      </td>
						</tr>
		
		   <tr>
			   <td colspan=4 align="center" bgcolor="#F1F4EC">&nbsp;
				</td>
					</tr>
		
		   <tr>
			   <td colspan=4 align="center" bgcolor="#F1F4EC">
				<b>
				<font face="Verdana" color="#a2921e" size="2">Project Status</font>
                </b>
					</td>
					</tr>
				<tr>
					<td align="center" bgcolor="#edf2e6" valign="top" colspan="4">
			           
				<ASP:DATAGRID id="dgProjectDetail" runat="server" BorderColor="Black" Font-Size="10pt" Font-Name="Verdana"
							BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False" 
							Headerstyle-Font-Size="10pt" Headerstyle-BackColor="LightGray" CellPadding="2"
							Width="100%" OnItemDataBound="dgProjectDetail_ItemDataBound" >
                 			<ItemStyle ForeColor="#000000" BackColor="#FFFFEE" VerticalAlign="Top"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100%"></HeaderStyle>
								<Columns>
									
									<asp:BoundColumn DataField="proStatusDate" HeaderText="Date"   
										DataFormatString="{0:dd-MMM-yy}"  HeaderStyle-HorizontalAlign=Center ItemStyle-HorizontalAlign=Center ItemStyle-Width="10%">
										
									</asp:BoundColumn>
									
									<asp:BoundColumn DataField="ExpCompleted" HeaderText="Expected Completion" 
										HeaderStyle-HorizontalAlign=Center ItemStyle-HorizontalAlign=Center  ItemStyle-Width="20%">
									</asp:BoundColumn>

									<asp:BoundColumn DataField="projStatus" HeaderText="Actual Completed" 
										HeaderStyle-HorizontalAlign=Center ItemStyle-HorizontalAlign=Center  ItemStyle-Width="20%">
									
									</asp:BoundColumn>
														
									<asp:BoundColumn DataField="projRemark" HeaderText="Remark" 
									ItemStyle-Width="50%">
									
									</asp:BoundColumn>
									
														
								</Columns>
							</ASP:DATAGRID>
								
			</td>
        </tr>
     </TABLE>

  </FORM>
</BODY>
</html>