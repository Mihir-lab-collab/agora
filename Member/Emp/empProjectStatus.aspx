<%@ Page Language="VB"%>
<%@ import Namespace="System.Data" %>
<%@import Namespace="System.Data.SqlClient"%>
<%@ import Namespace="System.IO"%>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
    <script language="JavaScript" src="../includes/CalendarControl.js" type="text/javascript">
    </script>
<%--<script language="JavaScript" src="/includes/calender.js">
</script>--%>
<title>Project Status</title>
</head>
<script language="javascript" >
function dateComp(date1,date2)
{
	var D0, D1, DD, DM, DY, LM = 365.25/12;
  	D0 = new Date(date1.replace(/\D+/g, "/"));
  	D1 = new Date(date2.replace(/\D+/g, "/"));
  	DD = Math.round((D1-D0)/864e5);
	return(DD);
}
// function to convert date from 02-Aug-2004 format to yyyy-mm-dd format 
function convertDate(d)
{
var d ;
var strConverDate ;
var strDate = d.replace("-","/").replace("-","/");   
	switch (strDate.substr(3,3))
	{
		case "Jan":
		 	strDate=strDate.replace("Jan","01");
		 	break;
		case "Feb":
			strDate=strDate.replace("Feb","02");
		 	break;
		case "Mar":
			strDate=strDate.replace("Mar","03");
			 break;
		case "Apr":
			strDate=strDate.replace("Apr","04");
		 break;
		case "May":
			strDate=strDate.replace("May","05");
		 	break;
		case "Jun":
			strDate=strDate.replace("Jun","06");
		 	break;
		case "Jul":
			strDate=strDate.replace("Jul","07");
		 	break;
		case "Aug":
			strDate=strDate.replace("Aug","08");
		 	break;
		case "Sep":
			strDate=strDate.replace("Sep","09");
		 	break;
		case "Oct":
			strDate=strDate.replace("Oct","10");
		 	break;
		case "Nov":
			strDate=strDate.replace("Nov","11");
			 break;
		case "Dec":
			strDate=strDate.replace("Dec","12");
		 	break;
		 	}
		 	strConverDate =  "20"+ strDate.substring(6,8) + '-' + strDate.substring(3,5) + '-' + strDate.substring(0,2)
			return(strConverDate );
}
</script>
<script language="javascript">
//TO CHECK NUMERIC VALUES VALIDATION AND COMPLETION STATUS LENGTH
function check_Numeric()
{
    	if((event.keyCode >=48 && event.keyCode <= 57)&&(document.forms[0].txtCompleted.value.length<=2))
		{
			hdnStatus = document.forms[0].txtCompleted.value
			alert(hdnstatus)
			return true;
		}
				
		else
		{
			return false;
		}
 }
 //TO COMPARE LAST STATUS AND ENTERED STATUS VALE
 function check_Status()
  {	
    var d = document.getElementById("txtnewDate").value ;
	//alert(d);
	var today = new Date()
	var year = today.getYear()
	if(year<1000) 
			year+=1900
	strConverDate = convertDate(d) ;
	
	//strCurDate=  today.getYear() + '-' + (today.getMonth()+ 1) + '-' + today.getDate(); // for yyyy-mm-dd format current date
	
 	if ( dateComp(strConverDate,strCurDate) <= -1 ) 
	{
	 	//alert("You can't Enter Next Date");
	 	//return false;
	}
	//alert( 'new Date ' + strConverDate + 'old date '+document.getElementById('hdnStatusDate1').value + (dateComp(strConverDate,document.getElementById('hdnStatusDate1').value) <= -1))	;
	if((document.forms[0].ddlProjStatusTyp.value!=4)&&(document.forms[0].ddlProjStatusTyp.value!=5))
	{    
			//var hdndate= ('0'+(document.getElementById('hdnStatusDate').value).substring(0,1)) + '/'+ ('0'+(document.getElementById('hdnStatusDate').value).substring(2,3))+ '/'+( (document.getElementById('hdnStatusDate').value).substring(6,8));   		
			//var hdndate = convertDate(document.getElementById('hdnStatusDate1').value);
			//if ((strDate)>=(hdndate))	
			if(dateComp(strConverDate,document.getElementById('hdnStatusDate1').value) <= -1)
			{// new Date Is Greater Than Old Date 
					if((parseInt(document.getElementById('hdnStatus').value))>(parseInt(document.forms[0].txtCompleted.value)))
         			{// old value  is greater tha new value 
							alert("Completion Status Must Be Greater than  " + document.getElementById('hdnStatus').value + '% ' );
							return false;
									
			 		}
	  				else 
					//old value is lees than new value 
					if((parseInt(document.forms[0].txtCompleted.value)>100)||(document.forms[0].txtCompleted.value.length==0))
		 			{		alert("Invalid Project Completion Status");
							return false;
			 		}
			 		else
				    {       return true;
		    		}
			}
			else
			{// new Date Is Less Than Old Date 
					if((parseInt(document.forms[0].txtCompleted.value)>100)||(document.forms[0].txtCompleted.value.length==0))
		 			{       alert("Invalid Project Completion Status");
							return false;
			 		}
			 		else
					{
							return true;
					}
			}			
	}
   	else
	{
		alert("Cancelled or hole ");
		//if ((strDate)>(document.getElementById('hdnStatusDate').value))
		  if((dateComp(strConverDate,document.getElementById('hdnStatusDate1').value) <= -1))	
			{ 
				//alert( 'new Date ' + strConverDate + 'old date ' + dateComp(strConverDate,document.getElementById('hdnStatusDate1').value) <= -1))				
					if(document.forms[0].txtCompleted.value.length==0)
						{
									alert("Invalid Project Completion Status");
									return false;
	    				}
	    			else
	   					{
		 							if(parseInt(document.getElementById('hdnStatus').value)>(parseInt(document.forms[0].txtCompleted.value)))
            						{
               								alert("Completion Status Must Be Greater Than "+document.getElementById('hdnStatus').value +'%');
			   								return false;
	         						}
	       							else
	         						{
                    						return true;
	           						}
	         		  }
			}
     	else 
			{
			      if(document.forms[0].txtCompleted.value.length==0)
						{
									alert("Invalid Project Completion Status");
									return false;
	    				}
				   //2
				   if((document.forms[0].ddlProjStatusTyp.value==4)&&(document.forms[0].ddlProjStatusTyp.value==5))
					{
							//alert("inside");
							if(parseInt(document.getElementById('hdnStatus').value)==100)
   								{
										return true;
   								}
   							else
   								{
    								if(parseInt(document.forms[0].txtCompleted.value)==100)
       								{
         										return true;
       								}
									else
									if (document.forms[0].ddlProjStatusTyp.value==4) 
									{
											if((parseInt(document.forms[0].txtCompleted.value)) > (document.getElementById('hdnStatus').value))
											{
														return true
											}
											else
											{
														alert("Completion Status Must Be Less than or Equal to " + document.getElementById('hdnStatus').value + '% ' );
														return false;
											}
									}
       								else
       								{
          									alert("Please Enter 100% Project Completion Status");
											return false;
       								}
   						}
  		}
				
	 }
   }
		
}
</script>	   
<script language="javascript">
function check()
{
	//alert("hi");
	return false;
}
//TO AVOID THE REMARK LENGTH FROM GREATER LENGTH TO DATABASE COLUMN LENGTH
function check_Remarklength()
{
if((document.getElementById('txtRemark').value.length)<255)
	{
	  //alert(document.getElementById('txtRemark').value)
	  return true;
	}
	else
	{
	return false;
	}
}
</script>
	
<script language="VB" runat="server">
   
   Dim intProjId as integer
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
   Dim Conn As SqlConnection = New SqlConnection(dsn)
   Dim dtrItemDesc as SqlDataReader
   Dim Cmd as  SQLCommand
   Dim strSql as string
    Dim gf As New generalFunction
'===========================================
 'PAGE LOAD TO SEE ALL THE PROJECT DETAILS 
 '==========================================
    Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        gf.checkEmpLogin()
        btninsert.Attributes.Add("onclick", "javascript:return check_Status();")
        ' btninsert.Attributes.Add("onClick", "javascript:return check_Status();")


        ' 	 =============change here============
        'txtCompleted.Attributes.Add("onkeypress", "return confirm(' hello ');")
		
        txtCompleted.Attributes.Add("onkeypress", "javascript:return check_Numeric();")
        txtRemark.Attributes.Add("onkeypress", "javascript:return check_Remarklength();")
        intProjId = CInt(Request.QueryString("projId"))
        If Not IsPostBack Then
	
            Bind_ProjStatusType()
	 
            If CStr(Session("dynoEmpIdSession")) = "" Then
                Response.Redirect("emplogin.aspx")
            End If
            Dim strItemDesc As String
            Dim dtrItemDesc As SqlDataReader
            Dim strProjName As String
            Dim dtrProjName As SqlDataReader
            Conn.Open()
            strProjName = "select * from projectMaster where projId=" & intProjId
			 
            Cmd = New SqlCommand(strProjName, Conn)
            dtrProjName = Cmd.ExecuteReader
            If dtrProjName.Read Then
                If IsDBNull(dtrProjName("projName")) = False Then
                    lblProjName.Text = dtrProjName("projName")
                End If
            End If
            Cmd.Dispose()
            dtrProjName.Close()
            Conn.Close()
			
            '    strItemDesc="select top 1 * from projectStatus where projStatusId " & _
            '	" in( select projStatusId from projectStatus WHERE " & _
            '     " prostatusdate in( select Max(prostatusdate) from " & _
            '       "projectStatus GROUP BY projId))and projId="& intProjId &"" -->
            'strItemDesc  ="select projStatus  from projectstatus  where projStatusId "& _ "in(select projStatusId 'from projectStatus wher " & _ " 
            ' where projId="& intProjId &")"
			
    
            strItemDesc = "select top 1 * from projectStatus where projId=" & intProjId & "  order by prostatusdate desc"
            Conn.Open()
			
            Cmd = New SqlCommand(strItemDesc, Conn)
            dtrItemDesc = Cmd.ExecuteReader

			
            If dtrItemDesc.Read Then
			        
  			   
                If IsDBNull(dtrItemDesc("projStatus")) Then
                Else
			 
                    lblLastStatus.Text = dtrItemDesc("projStatus") & "% as on " & Day(dtrItemDesc("proStatusDate")) & "-" & MonthName(Month(dtrItemDesc("proStatusDate")), 2) & "-" & Year(dtrItemDesc("proStatusDate")) & ""
                    hdnStatusDate.Value = Day(dtrItemDesc("proStatusDate")) & "/" & Month(dtrItemDesc("proStatusDate")) & "/" & Year(dtrItemDesc("proStatusDate"))
                    hdnStatusDate1.Value = Year(dtrItemDesc("proStatusDate")) & "-" & Month(dtrItemDesc("proStatusDate")) & "-" & Day(dtrItemDesc("proStatusDate"))
                    hdnStatus.Value = dtrItemDesc("projStatus")
                    ' response.write(lblLastStatus.Text )
                    'response.End()
                End If
			 
			   
            End If
		
            Cmd.Dispose()
            dtrItemDesc.Close()
			 
			
            Conn.Close()
        End If
			
    End Sub
	
'================================================================
' BIND PROJECT STATUS TYPE
'===============================================================
	     	Sub Bind_ProjStatusType()
				   ddlProjStatusTyp.items.clear()
                 
				   strSQL="select projStatusTId,projStatusTDesc from projStatusTypeMaster"
				                 
				  		Cmd=new SqlCommand(strSQL,Conn)
                   		Conn.open
                   		dtrItemDesc=Cmd.executereader
			        if(dtrItemDesc.hasrows)then
                      	while dtrItemDesc.read
                        		ddlProjStatusTyp.items.add(new listitem(dtrItemDesc("projStatusTDesc"),dtrItemDesc("projStatusTId")))
                     	end while
                   end if   
                 	'if Request.QueryString("Status")="Completed" Then
                  	'response.Write("<script language=javascript></scri>")
					'response.End()
					'ddlProjStatusTyp.items.remove("Started")
					'If request.QueryString("Status")<>"Started" then
                    'ddlProjStatusTyp.items.remove("Started")
					'End if
                    'else
                    'ddlProjStatusTyp.selectedindex=1
                  'end if
				  Conn.close
				  dtrItemDesc.close
				  Cmd.dispose
			End Sub
'================================================================
' END BIND PROJECT STATUS TYPE
'===============================================================
'====================================
 'INSERT THE RECORD IN BUTTON CLICK
 '===================================
Sub btninsert_Click(sender as object, e as System.EventArgs)
Dim strdate1 as String
       
        If CDate(txtnewDate.Value.ToString())> CDate(System.DateTime.Today.ToString()) Then
            Dim sp12 As String = ""
            sp12 = "<Script language=JavaScript>"
            sp12 += " alert('Date not greater than current date.');"
            sp12 += "</" + "script>"
            RegisterStartupScript("script123", sp12)
            Exit Sub
        End If
        strdate1 = Month(txtnewDate.Value) & "/" & Day(txtnewDate.Value) & "/" & Year(txtnewDate.Value)
          

       
        If CInt(ddlProjStatusTyp.SelectedItem.Value) = 4 Or CInt(ddlProjStatusTyp.SelectedItem.Value) = 5 Then
            Dim strSqlActComp = "update projectmaster set projActComp='" & txtnewDate.Value & "' where projId=" & Trim(intProjId)
            Conn.Open()
            Dim comActComp = New SqlCommand(strSqlActComp, Conn)
            comActComp.executeNonQuery()
            comActComp.dispose()
            Conn.Close()
        End If
        'new status date in dd/mm/yy format 
        Dim strproStatusDate = Month(txtnewDate.Value) & "/" & Day(txtnewDate.Value) & "/" & Year(txtnewDate.Value)
        '=======================================================
        'if Previous  Status is Complete then  not allow to update  Status 
        '=========================================================
        If (hdnStatusDate.Value < (Day(txtnewDate.Value) & "/" & Month(txtnewDate.Value) & "/" & Year(txtnewDate.Value))) Then
            'response.Write( Request.QueryString("Status")) 
            'response.End()
            If IsNumeric(Request.QueryString("Status")) Then
                'if  cint(Request.QueryString("Status")) = 5 and  cint(hdnStatus.value)=100 then 
                'if (cint(ddlProjStatusTyp.selectedItem.value) <> 4 and  cint(ddlProjStatusTyp.selectedItem.value) <> 5 ) then 
                'Dim sp1 As String
                'sp1 = "<Script language=JavaScript>"
                'sp1 += "alert(' Project Status Type should be Complete or On Hold')"
                'sp1 += "</" + "script>"
                'RegisterStartupScript("script123", sp1)
                'Exit sub 
                'end if 
                'end if 	
            Else
                If Request.QueryString("Status") = "Completed" And CInt(hdnStatus.Value) = 100 Then
                    If (CInt(ddlProjStatusTyp.SelectedItem.Value) <> 4 And CInt(ddlProjStatusTyp.SelectedItem.Value) <> 5) Then
                        Dim sp1 As String
                        sp1 = "<Script language=JavaScript>"
                        sp1 += "alert(' Project Status Type should be Complete or On Hold')"
                        sp1 += "</" + "script>"
                        RegisterStartupScript("script123", sp1)
                        Exit Sub
                    End If
                End If
            End If
        End If
        '================================================================================
        'code to validate  if user wants to  enter record of previous date  10/8/06
        '=================================================================================
        If (hdnStatusDate.Value > (Day(txtnewDate.Value) & "/" & Month(txtnewDate.Value) & "/" & Year(txtnewDate.Value))) Then
            Dim strNewStatusDate = Year(txtnewDate.Value) & "-" & Month(txtnewDate.Value) & "-" & Day(txtnewDate.Value)
            Dim strSqlCheckprevDate
            Dim strSqlCheckNextDate
            Dim readprjStatusPrev
            Dim readprjStatusNext
            Conn.Open()
            strSqlCheckNextDate = "select projStatus from projectStatus where "
            strSqlCheckNextDate = strSqlCheckNextDate & "datediff(day, proStatusDate ,'" & strNewStatusDate & "') ="
            strSqlCheckNextDate = strSqlCheckNextDate & "(select min(datediff(day, proStatusDate ,'" & strNewStatusDate & "')) from projectStatus"
            strSqlCheckNextDate = strSqlCheckNextDate & " where datediff(day, proStatusDate ,'" & strNewStatusDate & "')>0"
            strSqlCheckNextDate = strSqlCheckNextDate & "and projId = " & Trim(intProjId) & ") and projId = " & Trim(intProjId)
                
            strSqlCheckprevDate = "select projStatus from projectStatus where "
            strSqlCheckprevDate = strSqlCheckprevDate & "datediff(day, proStatusDate ,'" & strNewStatusDate & "') ="
            strSqlCheckprevDate = strSqlCheckprevDate & "(select max(datediff(day, proStatusDate ,'" & strNewStatusDate & "')) from projectStatus"
            strSqlCheckprevDate = strSqlCheckprevDate & "  where datediff(day, proStatusDate ,'" & strNewStatusDate & "')<0"
            strSqlCheckprevDate = strSqlCheckprevDate & "and projId = " & Trim(intProjId) & ") and projId = " & Trim(intProjId)
            Dim cmdActReadStatusPrev = New SqlCommand(strSqlCheckprevDate, Conn)
            Dim cmdActReadStatusNext = New SqlCommand(strSqlCheckNextDate, Conn)
            cmdActReadStatusPrev.executeNonQuery()
            readprjStatusPrev = cmdActReadStatusPrev.ExecuteReader()
            If (readprjStatusPrev.Read) Then
                If (readprjStatusPrev("projStatus") < CInt(txtCompleted.Text)) Then
                    Dim sp1 As String
                    Dim projectStatus = readprjStatusPrev("projStatus")
                    sp1 = "<Script language=JavaScript>"
                    sp1 += "alert(' Invalid Project Status Value value must be Less " & projectStatus & " %')"
                    sp1 += "</" + "script>"
                    RegisterStartupScript("script123", sp1)
                    Exit Sub
                End If
            End If
            readprjStatusPrev.close()
            cmdActReadStatusNext.executeNonQuery()
            readprjStatusNext = cmdActReadStatusNext.ExecuteReader()
            If (readprjStatusNext.Read) Then
                If (readprjStatusNext("projStatus") > CInt(txtCompleted.Text)) Then
                    Dim sp2 As String
                    Dim projectStatus = readprjStatusNext("projStatus")
                    sp2 = "<Script language=JavaScript>"
                    sp2 += "alert('Invalid Project Status value  must be Greater than" & projectStatus & " % ')"
                    sp2 += "</" + "script>"
                    RegisterStartupScript("script123", sp2)
                    Exit Sub
                End If
            End If
            readprjStatusNext.close()
            Conn.Close()
        End If
        'end code of validation  
        '==============================================================================================
        'code to check  date   9/8/06 
        '==============================================================================================
        Dim rsReadCheckDate
        'dim strproStatusDate = Month(txtnewdate.value) & "/" & Day(txtnewdate.value) & "/" & Year(txtnewdate.value)
        Dim strSqlActCheckdate = "Select * from  projectstatus where proStatusDate = '" & strproStatusDate & "' and  projId =  " & Trim(intProjId)
        Conn.Open()
        Dim cmdActCheckdate = New SqlCommand(strSqlActCheckdate, Conn)
        cmdActCheckdate.executeNonQuery()
        rsReadCheckDate = cmdActCheckdate.ExecuteReader()
        If (rsReadCheckDate.Read) Then
            '=============================================================
            ' Record already Exist  then Update Record
            '==============================================================
            cmdActCheckdate.dispose()
            'conn.close
            Dim strUpdate
            Dim cmdUpdate
            strUpdate = "Update projectstatus set projId = " & intProjId & " ,projStatus = " & txtCompleted.Text & ","
            strUpdate = strUpdate & " projRemark = ' " & sqlSafe(Trim(txtRemark.Text)) & "', projStatusTId= " & ddlProjStatusTyp.SelectedItem.Value
            strUpdate = strUpdate & "where proStatusDate = '" & strproStatusDate & "' and  projId =  " & Trim(intProjId)
            rsReadCheckDate.close()
            cmdUpdate = New SqlCommand(strUpdate, Conn)
            cmdUpdate.executeNonQuery()
            cmdUpdate.dispose()
        Else
            '=============================================
            'New Record then insert new record 
            '=============================================
            rsReadCheckDate.close()
            cmdActCheckdate.dispose()
            strSql = "INSERT INTO projectstatus (projId, proStatusDate, projStatus, projRemark, projStatusTId) values(" & intProjId & ", '" & strdate1 & "', " & txtCompleted.Text & ",'" & sqlSafe(Trim(txtRemark.Text)) & "'," & ddlProjStatusTyp.SelectedItem.Value & ")"
            Cmd = New SqlCommand(strSql, Conn)
            Cmd.ExecuteNonQuery()
            Cmd.Dispose()
        End If
        Conn.Close()
        'end code
        '================================
        'CLOSE WINDOW AFTER INSERT RECORD
        '=================================
        Dim sp As String
        sp = "<Script language=JavaScript>"
        sp += "window.opener.parent.location.href('empProject.aspx');"
        sp += "window.close();"
        sp += "</" + "script>"
        RegisterStartupScript("script123", sp)
    End Sub

function sqlSafe(str)
	         if str & "" <> "" Then
		       str = Replace(str,"'","''")
	         End if
	           sqlSafe = str
End function
</script>
<BODY  topmargin="0" leftmargin="0" rightmargin="0">
<form id="Form1" name=frm runat="server">
	<TABLE height="0" cellSpacing="0" cellPadding="0" width="100%" height="100%" border="1" bordercolor ="#C5D5AE">
		<TR>
			<TD align="center">
				<TABLE id="Table1" borderColor="#c5d5ae" width="100%" cellSpacing="0" cellPadding="4" border="1">
					<TR>
						<TD bgColor="#c5d5ae" colSpan="4" rowspan="1" align="center">
							<FONT face="Verdana" color="#a2921e" ><B>Project Status</b></FONT></TD>
					</TR>
					<TR>
						<TD nowrap="nowrap" bgColor="#edf2e6"><B><FONT face="Verdana" color="#a2921e" size="2">Project:</FONT></B></TD>
						<TD nowrap="nowrap" width="75%" colSpan="3">
						<font face="verdana" size="2">
						<asp:label id="lblProjName" runat="server" ></asp:label></font></TD>
					</TR>
					<TR>
						<TD nowrap="nowrap" bgColor="#edf2e6"><B><FONT face="Verdana" color="#a2921e" size="2">Last Status:</FONT></B></TD>
						<TD><font face="verdana" size="2">
			            <asp:label id="lblLastStatus" runat="server" Width="408px"> </asp:label></font>
	                      </TD>
					</TR>
					<TR>
						<TD nowrap="nowrap" bgColor="#edf2e6" valign="top"><B><FONT face="Verdana" color="#a2921e" size="2">Current Status:</FONT></B>
							</TD>
							<TD>
							<TABLE id="Table1" borderColor="#c5d5ae"  cellSpacing="0" cellPadding="4" border="0">
								<tr>
									<TD nowrap="nowrap" align="left" width="137"><B><FONT face="Verdana" color="#a2921e" size="2">Date</FONT></B></TD>
				                   <TD align="left" width="586"><font face="verdana" size="2">
									<input type="text" id="txtnewDate" runat="server" onkeypress="return false;" onclick="popupCalender('txtnewDate')" size="10">
									<asp:TextBox ID="txtValidated" runat="server" Visible="false" size="14"></asp:TextBox>
									</font>
									<asp:TextBox id="txtDate"  runat="server" visible="false" size="14" >
                                    </asp:TextBox>
									</TD>
								</tr>
								<tr>
									<TD nowrap="nowrap" width="137"><B><FONT face="Verdana" color="#a2921e" size="2">Project Status Type</FONT></B></TD>
				                   <TD align="left" width="586"><font face="verdana" size="2">
				                   <asp:dropdownlist id="ddlProjStatusTyp" runat="server"/>
									</font></TD>
								</tr>
								<tr>
									<TD nowrap="nowrap" width="137"><B><FONT face="Verdana" color="#a2921e" size="2">Completed</FONT></B></TD>
				                   <TD align="left" width="586"><font face="verdana" size="2">
			                      <asp:textbox id="txtCompleted" runat="server" Width="156px" >
			                      </asp:textbox>%</font>
			                      </TD>
								</tr>
								<tr>
									<TD nowrap="nowrap" width="137"><B><FONT face="Verdana" color="#a2921e" size="2">Remark</FONT></B></TD>
				                   <TD align="left" width="586"><font face="verdana" size="2">
			                      <asp:textbox id="txtRemark" runat="server" Width="156px" TextMode="MultiLine" height="77">
				                      </asp:textbox></font></TD>
								</tr>
								</Table>
						</TD>
					</TR>
			 <tr>
				<td align="center" colspan="2">
					<asp:button id="btninsert" runat="server" Text="Submit" onclick="btninsert_Click" width="70" height="25"></asp:button>
					<input type=button id="btncancel" name=btncancel value="Close" style="width:70;height:25;" onclick="javascript: window.close();">
	            	<input type="hidden" id="hdnStatus" runat="server">
    	        	<input type="hidden" id="hdnStatusDate" runat="server">
					<input type="hidden" id="hdnStatusDate1" runat="server">
				</td>
			</tr>
		</TABLE>
	</TD>
	</TR>
  </FORM>
</BODY>
</html>