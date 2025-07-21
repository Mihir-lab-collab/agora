<%@ Page Language="vb" AutoEventWireup="false" EnableSessionState="true" EnableViewState="false" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>

<script language="VB" runat="server">
    Dim sql As String
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim objcmd As SqlCommand
    Dim currStartDate, currEndDate, empTotPLLbl As String
    Dim empJoiningDate, empConfDate As String
    Dim employeeid As Integer
    Dim gf As New generalFunction
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        gf.checkEmpLogin()
        Dim conn1 As SqlConnection = New SqlConnection(dsn)
        Dim empId As String = Session("dynoEmpIdSession")
        Dim currYear As String = Request.QueryString("currYear")

        Dim objDataReader1 As SqlDataReader
        If IsNumeric(currYear) Then
            currStartDate = "1-Apr-" & currYear
            currEndDate = "31-Mar-" & currYear + 1
        Else
            If Month(Now()) > 3 Then
                currStartDate = "1-Apr-" & Year(Now())
                currEndDate = "31-Mar-" & Year(Now()) + 1
            Else
                currStartDate = "1-Apr-" & Year(Now()) - 1
                currEndDate = "31-Mar-" & Year(Now())
            End If
        End If
        conn1.Open()

        If Not IsPostBack() Then
            sql = "SELECT *, DateAdd(month, empProbationPeriod, empJoiningDate) AS empConfDate " & _
            "FROM employeeMaster WHERE empLeavingDate IS NULL ORDER BY empJoiningDate"
            objcmd = New SqlCommand(sql, conn1)
            objDataReader1 = objcmd.ExecuteReader
            empDataGrid.DataSource = objDataReader1
            empDataGrid.DataBind()
            objDataReader1.Close()
        End If
	
        conn1.Close()
    End Sub

    Function test(ByVal empId As String, ByVal empJoiningDate As String, ByVal empConfDate As String) As String
        Dim conn As SqlConnection = New SqlConnection(dsn)
        Dim empTotCL, empTotSL, empTotPL, empTotCO As Integer
        employeeid = empId
	
        Dim empTotCLC, empTotSLC, empTotPLC, empTotCOC, empTotWL As String
        Dim availMonth As String
        Dim objdatareader As SqlDataReader
        Dim plDateStart, plAvailMonth As String
        conn.Close()
        conn.Open()
		
        plDateStart = DateAdd("m", 12, empJoiningDate)
		
        sql = "SELECT * FROM empStatus WHERE statusLimit > 0"
        objcmd = New SqlCommand(sql, conn)
        objdatareader = objcmd.ExecuteReader
        Do While objdatareader.Read()
            If objdatareader("statusId") = "CL" Then
                empTotCL = objdatareader("statusLimit")
            ElseIf objdatareader("statusId") = "SL" Then
                empTotSL = objdatareader("statusLimit")
            ElseIf objdatareader("statusId") = "PL" Then
                empTotPL = objdatareader("statusLimit")
            End If
        Loop
        objdatareader.Close()
		
        plAvailMonth = 0
        If (DateDiff("d", empConfDate, currStartDate)) < 0 Then
            availMonth = 11 - DateDiff("m", currStartDate, empConfDate)
            empTotCL = FormatNumber((empTotCL / 12) * availMonth, 0)
            empTotSL = FormatNumber((empTotSL / 12) * availMonth, 0)
        End If
		
        If DateDiff("d", plDateStart, currStartDate) > 0 Then
            empTotPL = empTotPL
        ElseIf plDateStart > currEndDate Then
            empTotPL = 0
        Else
            'response.write(plAvailMonth & "plAvailMonth")
            plAvailMonth = DateDiff("m", plDateStart, currEndDate)
            empTotPL = CInt(FormatNumber((empTotPL / 12) * plAvailMonth, 0))
	
        End If

        Dim a As Double '----- lalt to last total pl
        Dim diff As Double

        Dim plsused As Integer
        If Month(Now()) > 3 Then
            currStartDate = "1-Apr-" & Year(Now())
            currEndDate = "31-Mar-" & Year(Now())
        Else
            currStartDate = "1-Apr-" & Year(Now()) - 1
            currEndDate = "31-Mar-" & Year(Now()) - 1
        End If
       
 
        sql = "SELECT attStatus,count(*) as lCount FROM empAtt " & _
      "WHERE empId=" & empId & " AND attDate BETWEEN '" & empJoiningDate & _
      "' AND '" & currEndDate & "' GROUP BY attStatus"
	
        objcmd = New SqlCommand(sql, conn)
        objdatareader = objcmd.ExecuteReader
        Do While objdatareader.Read()
						
            If objdatareader("attStatus") = "PL" Then
                plsused = objdatareader("lCount")
            End If
        Loop
        objdatareader.Close()
 
        plAvailMonth = 0

        sql = "SELECT * FROM empStatus WHERE statusLimit > 0"
        objcmd = New SqlCommand(sql, conn)
        objdatareader = objcmd.ExecuteReader
        Do While objdatareader.Read()
            If objdatareader("statusId") = "PL" Then
                diff = (DateDiff("m", empJoiningDate, currEndDate) - 13) * (objdatareader("statusLimit") / 12)
                a = CInt((DateDiff("m", empJoiningDate, currStartDate) - 13) * (objdatareader("statusLimit") / 12))
                empTotPL = CInt(objdatareader("statusLimit") + (a - plsused))
            End If
        Loop
        objdatareader.Close()
		
        sql = "select empID,count(coID)as compOff from empCompOff  where empID=" & empId & "group by empID"
        objcmd = New SqlCommand(sql, conn)
        objdatareader = objcmd.ExecuteReader
        Do While objdatareader.Read()
            empTotCO = objdatareader("compOff")
        Loop
        objdatareader.Close()
		
        empTotCLC = 0
        empTotSLC = 0
        empTotPLC = 0
        empTotCOC = 0
        empTotWL = 0

        If Month(Now()) > 3 Then
            currStartDate = "1-Apr-" & Year(Now())
            currEndDate = "31-Mar-" & Year(Now()) + 1
        Else
            currStartDate = "1-Apr-" & Year(Now()) - 1
            currEndDate = "31-Mar-" & Year(Now())
        End If
        sql = "SELECT attStatus,count(*) as lCount FROM empAtt " & _
        "WHERE empId=" & empId & " AND attDate BETWEEN '" & currStartDate & _
        "' AND '" & currEndDate & "' GROUP BY attStatus"
		
        objcmd = New SqlCommand(sql, conn)
        objdatareader = objcmd.ExecuteReader
		
        Do While objdatareader.Read()
            If objdatareader("attStatus") = "CL" Then
                empTotCLC = objdatareader("lCount")
            ElseIf objdatareader("attStatus") = "SL" Then
                empTotSLC = objdatareader("lCount")
            ElseIf objdatareader("attStatus") = "PL" Then
                empTotPLC = objdatareader("lCount")
            ElseIf objdatareader("attStatus") = "CO" Then
                empTotCOC = objdatareader("lCount")
            ElseIf objdatareader("attStatus") = "WL" Then
                empTotWL = objdatareader("lCount")
            End If
        Loop
        objdatareader.Close()
		
        If DateAdd("m", 12, empJoiningDate) > Now() Then
            empTotPL = 0
            empTotPLC = 0
        End If

        If empConfDate > Now() Then
            empTotCL = 0
            empTotCLC = 0
        End If
        If empTotPL > 90 Then
            empTotPL = 90
        End If


        Return "<table width=100% style=""font-family: Verdana; font-size: 12""><tr><td width=8% align=center>" & _
        empTotCL & "</td><td width=8% align=center>" & empTotSL & "  </td> " & _
         "<td width=8% align=center><a id=" & employeeid & " href='' onmouseover=javascript:call_Ajax('" & empId & "'); onmouseout=javascript:hideDiv('" & empId & "'); > " & _
         "" & empTotPL & "</a></td><td width=8% align=center>" & empTotCO & "  </td><td width=8% align=center>" & empTotCLC & "</td>" & _
         "<td width=8% align=center>" & empTotSLC & "</td><td width=8% align=center>" & empTotPLC & _
         "</td><td width=8% align=center>" & empTotCOC & "  </td><td width=8% align=center>" & empTotCL - empTotCLC & "</td><td width=8% align=center>" & _
         empTotSL - empTotSLC & "</td><td width=8% align=center>" & empTotPL - empTotPLC & "</td><td width=8% align=center>" & empTotCO - empTotCOC & _
         "</td><td width=10% align=center style=""font-family: Verdana; font-size: 12;color:RED"">" & empTotWL & "</td></tr></table>" & _
        " <div style=width: 550px class=sample_attach1 id='src_floppy_child_" & empId & "'  style=position:absolute;display:none;left:+400px;top:absolute;> " & _
        "</div> "
            
    End Function


</script>

<script language="javascript" type="text/javascript" src="../includes/dropdown.js"></script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dynamic Web Tech</title>
    <style type="text/css">
        div.sample_attach1
        {
            width: 400px;
            font-family: Verdana;
            background: #C5D5AE;
            padding: 0px 5px;
            font-weight: 900;
            color: #A2921E;
            background: #C5D5AE;
            border-top-color: #696;
            border-left-color: #696;
            border-right-color: #363;
            border-bottom-color: #363;
            font-size: 12;
        }
        input.btn
        {
            font-family: Verdana;
            font-weight: bold;
            border: 1px solid black;
            border: 1px solid;
            background: #C5D5AE;
            border-top-color: #696;
            border-left-color: #696;
            border-right-color: #363;
            border-bottom-color: #363;
        }
    </style>
</head>
<body>
    <ucl:adminMenu ID="adminMenu" runat="server" />
    <form id="Form1" runat="server">
    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <td style="height: 1px">
            <table width="190" border="0" cellpadding="0" cellspacing="0">
                <tr bgcolor="#edf2e6">
                    <td nowrap="nowrap"="true" align="left">
                        <b><font face="Verdana" size="2">
                            <p align="right">
                                |<a href="empLeaveRequests.aspx"><font color="#A2921E">Leave Requests</font></a>
                    </td>
                    <td nowrap="nowrap"="true" align="right">
                        <b><font face="Verdana" size="2">
                            <p align="left">
                                |<a href="empLeave.aspx"><font color="#A2921E">Annual Leaves</font></a>|
                    </td>
                </tr>
            </table>
        </td>
        <tr>
            <td align="center" height="90%" colspan="3" valign="top">
                <table cellspacing="0" cellpadding="2" border="1" bordercolor="#C5D5AE" width="100%">
                    <tr>
                        <td colspan="4" bgcolor="#edf2e6" height="53" width="394">
                            <b><font face="Verdana" color="#a2921e" size="2">Leave Details (<%=currStartDate%>
                                To
                                <%=currEndDate%>
                                )</font></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top">
                            <asp:DataGrid ID="empDataGrid" runat="server" Width="100%" CellPadding="2" Font-Name="Verdana"
                                Font-Size="10pt" MaintainState="true" AutoGenerateColumns="false" Font-Names="Verdana">
                                <HeaderStyle ForeColor="#a2921e" Font-Size="10pt" BackColor="#edf2e6"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="empId" HeaderText="Employee Id"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="empName" HeaderText="Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="empJoiningDate" HeaderText="Joining" DataFormatString="{0:dd-MMM-yy}">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="empConfDate" HeaderText="Confirmed" DataFormatString="{0:dd-MMM-yy}">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="<table width=100% style='font-family: Verdana; font-size: 12; color: #a2921e'><tr><td colspan=4 align=center width=33%>Total Leaves</td><td colspan=4 align=center width=33%>Consumed</td><td colspan=5 align=center width=33%>Balance</td></tr><tr><td width=8% align=center>CL</td><td width=8% align=center>SL</td><td width=8% align=center>PL</td><td width=8% align=center>CO</td><td width=8% align=center>CL</td><td width=8% align=center>SL</td><td width=8% align=center>PL</td><td width=8% align=center>CO</td><td width=8% align=center>CL</td><td width=8% align=center>SL</td><td width=8% align=center>PL</td><td width=8% align=center>CO</td><td width=10% style=font-family: Verdana; font-size: 12;color:RED align=center>WL</td></table>">
                                        <ItemTemplate>
                                            <%#test (Container.DataItem("empId"),Container.DataItem("empJoiningDate"),Container.DataItem("empConfDate"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script type="text/javascript">
   function ajaxFunction()
{  
  var xmlHttp;
  try
    {  
	  xmlHttp=new XMLHttpRequest();
	}
  catch (e)
    {    // Internet Explorer
	   try
      	{   
		   xmlHttp=new ActiveXObject("Msxml2.XMLHTTP"); 
		}
    	catch (e)
      	{    
			  try
        		{ 
					xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");
				}
     			catch (e)
        		{   
				 	alert("Your browser does not support AJAX!");   
					return false;
				}
	 	}  
	}
return xmlHttp;
}
function hideDiv(id)
{
	document.getElementById('src_floppy_child_'+id).style.display = "none" ;

}
function call_Ajax(id)
{
     var xmlHttp = ajaxFunction() ;
     var url="plleaveajax.aspx?empid="+id +"&sid="+Math.random();
         xmlHttp.open("GET",url,true);
	    xmlHttp.onreadystatechange=function()
        {
         if(xmlHttp.readyState==4)
          {
                var str =  xmlHttp.responseText;
          	    if (str != "") 
			    {
					//src_floppy_child_
					document.getElementById('src_floppy_child_'+id).innerhtml = str;
					document.getElementById('src_floppy_child_'+id).style.display = "" ;
			    }
		  }
      }

    xmlHttp.send(null);  
}


</script>

