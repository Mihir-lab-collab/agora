<%@ Page Language="VB" ContentType="text/html" ResponseEncoding="iso-8859-1"  %>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>

<script language="VB" runat="server">
    Dim sql As String
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim conn As SqlConnection = New SqlConnection(dsn)
    Dim dr1 As SqlDataReader
    Dim cmd As SqlCommand
    Dim intempLeaveId As Integer
    Dim gf As New generalFunction
    Dim dateto As Date
    Dim datefrom As Date
    Dim s As String
    Dim strlevestatus As String
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        'If request.QueryString("empLeaveId")<>"" then
        gf.checkEmpLogin()
        intempLeaveId = CInt(Request.QueryString("empLeaveId"))
        strlevestatus = Request.QueryString("Status").ToString()
   
        If (strlevestatus.ToString() = "Pending") Then
            btndelete.Visible = True
        End If
        If Me.IsPostBack = False Then
           
            'cmd = New SqlCommand("select leave.*,convert(varchar(20),leave.leaveFrom,06) as fromDate,convert(varchar(20),leave.leaveEntryDate,06) as applydate,convert(varchar(20),leave.leaveTo,06) as todate,convert(varchar(20),leave.leaveSenctionedDate,06) as onDate, empStatus.statusDesc as desc1,case when leavestatus='p' then 'pending' when leavestatus=' r' then 'rejected' when leavestatus=' a' then 'approved' end as ls, emp.empId,emp.empName  from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid AND leave.empLeaveId=" & intempLeaveId & " order by leave.leaveFrom desc", conn)
            cmd = New SqlCommand("select leave.*,REPLACE(convert(varchar(11),leave.leaveFrom,106),' ','-') as fromDate,convert(varchar(20),leave.leaveEntryDate,06) as applydate,REPLACE(convert(varchar(11),leave.leaveTo,106),' ','-') as todate,convert(varchar(20),leave.leaveSenctionedDate,06) as onDate, empStatus.statusDesc as desc1,dbo.GetNoOfLeavesApplied(leave.leaveFrom,leave.leaveTo) as totleave,case when leavestatus='p' then 'pending' when leavestatus=' r' then 'rejected' when leavestatus=' a' then 'approved' end as ls, emp.empId,emp.empName  from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid AND leave.empLeaveId=" & intempLeaveId & " order by leave.leaveFrom desc", conn)
            'response.write(intempLeaveId)
            'response.end
            conn.Open()
            dr1 = cmd.ExecuteReader()
            While dr1.Read()
                lblempId.Text = dr1("empId")
                lblempName.Text = dr1("empName")
                lblLeaveEntryDate.Text = dr1("applydate")
                txtleaveFrom.Text = dr1("fromDate")
                txtleaveTo.Text = dr1("todate")
                
                TotalLeave.Text = dr1("totleave")
                'txtAdminComments.text=dr1("leaveComment")
                lblReasonofLeave.Text = dr1("leaveDesc")
                For Each item As ListItem In lstLeaveStatus.Items
                    If item.Value = dr1("leaveStatus") Then
                        lstLeaveStatus.SelectedValue = item.Value
                    End If
                Next
                For Each item As ListItem In dropLeaveType.Items
                    If item.Value = dr1("leaveId") Then
                        dropLeaveType.SelectedValue = item.Value
                    End If
                Next

                If IsDBNull(dr1.Item("leaveSenctionedDate")) Then
                Else
                    lblonDate.Text = dr1("onDate")
                End If

                If Session("dynoAdminSession") = 1 Then
                    lblLeaveSanctionBy.Text = Session("dynoEmpNameSession") & "(" & Session("dynoEmpIdSession") & ")"
                End If

                If IsDBNull(dr1.Item("leaveComment")) Then
                Else
                    txtAdminComments.Text = dr1("leaveComment")
                End If
            End While
            dr1.Close()
            cmd.Dispose()
            conn.Close()
        End If
    End Sub

    Sub btnsubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        
        Dim sp1 As String
        Dim date1 As String
        date1 = Month(Now()) & "-" & Day(Now()) & "-" & Year(Now)
        cmd = New SqlCommand("update empLeaveDetails set leaveStatus= '" & lstLeaveStatus.SelectedValue & "', leaveComment= '" & sqlsave(txtAdminComments.Text) & "', leaveSanctionBy='" & lblLeaveSanctionBy.Text & "',leaveFrom='" & Trim(txtleaveFrom.Text) & "',leaveTo='" & Trim(txtleaveTo.Text) & "',leaveSenctionedDate = '" & date1 & "'  where empLeaveId=" & intempLeaveId & " ", conn)
        conn.Open()
        cmd.ExecuteNonQuery()
        cmd.Dispose()
        conn.Close()

        If lstLeaveStatus.SelectedValue = "a" Then
            dateto = CDate(txtleaveFrom.Text)
            datefrom = CDate(txtleaveTo.Text)

            While DateDiff(DateInterval.Day, dateto, datefrom) >= 0
                Dim str1 As String
                str1 = "delete from empAtt where attDate=  '" & dateto & "' And empId='" & lblempId.Text & "'"
                Dim cmd2 As SqlCommand = New SqlCommand(str1, conn)
                conn.Open()
                Dim dtr1 As SqlDataReader
                dtr1 = cmd2.executereader()
                conn.Close()
                Dim str As String
                'str = " insert into empAtt(attStatus) values ('" & lblempId.text & "', '" & dateto & "', '" &   dropLeaveType.selectedvalue & "')"
                'str = " insert into empAtt(attStatus) values ('" & dropLeaveType.selectedvalue & "')"
                'str = " insert into empAtt (empId,attdate,attStatus) values (" & lblempId.text & ",'" & dateto & "', '" & dropLeaveType.selectedvalue & "')"
                Dim tempattip As String = "00.0.0.00"
                str = " insert into empAtt(empId,attDate,attStatus,attIP,attInTime,attOutTime,adminID,attComment) values (" & lblempId.Text & ", '" & dateto & "', '" & dropLeaveType.SelectedValue & "','" & tempattip & "','" & DateTime.Now.ToShortDateString() & "','" & DateTime.Now.ToShortDateString() & "','" & Session("dynoEmpIdSession").ToString() & "','" & txtAdminComments.Text.Replace("'", "''").ToString() & "')"
               			
                'cmd = new SqlCommand("insert into empAtt(attDate,attStatus)values('" & date1 & "', '" &   dropLeaveType.selectedvalue & "') where empId= " & lblempId.text & " ")
		
                Dim cmd1 = New SqlCommand(str, conn)
                conn.Open()
                dateto = dateto.AddDays(1)
                cmd1.ExecuteNonQuery()
                conn.Close()
            End While
            
             If (dropLeaveType.SelectedValue.Equals("CO"))
                ManagecompOffLeave() 'added by Dipti
            End If
        Else
    
            dateto = CDate(txtleaveFrom.Text)
            datefrom = CDate(txtleaveTo.Text)
         
            While DateDiff(DateInterval.Day, dateto, datefrom) >= 0
                Dim str3 As String
                str3 = "delete from empAtt where attDate=  '" & dateto & "' And empId='" & lblempId.Text & "'"
                Dim cmd3 = New SqlCommand(str3, conn)
                conn.Open()
                Dim dtr3 As SqlDataReader
                dtr3 = cmd3.executereader()
                dateto = dateto.AddDays(1)
                conn.Close()
            End While
        End If
       
        
        'sp1 = "<Script language=JavaScript>"
        'sp1 += "window.opener.location=window.opener.location;"
        'sp1 += "window.close();"
        ''sp1+ ="alert('in');"
        'sp1 += "</" + "script>"
        'response.write (sp1)
        ClientScript.RegisterStartupScript(Me.GetType(), "script123", "SetLeaveSerachValues();", True)
    End Sub

    'dipti
    Sub ManagecompOffLeave()
        Dim objCmd As SqlCommand
        Dim conn As SqlConnection = New SqlConnection(dsn)

        Try
            objCmd = New SqlCommand()
            conn.Open()
            objCmd.Connection = conn
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.CommandText = "SP_EmpLeave"
            objCmd.Parameters.Add(New SqlParameter("@Mode", SqlDbType.Char, 50)).Value = "MANAGECOMPOFFLEAVE"
            objCmd.Parameters.Add(New SqlParameter("@EmpID", SqlDbType.Int)).Value = lblempId.Text
            objCmd.Parameters.Add(New SqlParameter("@Status", SqlDbType.Int)).Value = 0
            objCmd.Parameters.Add(New SqlParameter("@Type", SqlDbType.Int)).Value = 0
            objCmd.Parameters.Add(New SqlParameter("@Month", SqlDbType.Int)).Value = 0
            objCmd.Parameters.Add(New SqlParameter("@LeaveFrom", SqlDbType.DateTime)).Value = Convert.ToDateTime(Trim(txtleaveFrom.Text))
            objCmd.Parameters.Add(New SqlParameter("@LeaveTo", SqlDbType.DateTime, 5)).Value = Convert.ToDateTime(Trim(txtleaveTo.Text))
            objCmd.Parameters.Add(New SqlParameter("@LeaveDesc", SqlDbType.VarChar, 5)).Value = ""
            objCmd.Parameters.Add(New SqlParameter("@EmpLeaveID", SqlDbType.Int)).Value = intempLeaveId
            
           
            objCmd.ExecuteNonQuery()
            conn.Close()
            objCmd.Dispose()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('" + ex.Message + "')", True)
            conn.Close()
            objCmd.Dispose()
        End Try

    End Sub
    'end dipti
    
   Function sqlsave(ByVal str as string)as String
   If str<>"" then
str=str.Replace("'","''")
End if
return str
End function

    
    Sub btndelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sp1 As String
        Try
            
            cmd = New SqlCommand("DELETE FROM empLeaveDetails where empLeaveId=" & intempLeaveId, conn)
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            sp1 = "<Script language=JavaScript>"
            sp1 += "window.opener.location=window.opener.location;"
            sp1 += "window.close();"
            sp1 += "</" + "script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "script123", sp1)
        Catch ex As Exception
          
        End Try
        
    End Sub
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD html 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>applied Leave Details</title>
<link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
<script language="javascript" type="text/javascript">
    function fun_confirm() {
        var temp = confirm("Are you sure to delete record ?");
        if (temp == true) {
            return true;
        }
        else {
            return false;
        }
    }

    function CompDate(adate, bdate) {
        a = adate.split('-');
        b = bdate.split('-');
        var sDate = new Date(a[2], parseInt(getnummonth(a[1])) - 1, a[0]);
        var eDate = new Date(b[2], parseInt(getnummonth(b[1])) - 1, b[0]);
        alert(sDate);
        alert(eDate);
        if (sDate <= eDate) {

            return true;
        }
        else {
            alert("From date must be less then to date !");
            return false;
        }
    }
    function DateCompaire() {
        return CompDate(document.getElementById('txtleaveFrom').value, document.getElementById('txtleaveTo').value);
    }
    function getnummonth(strmonth) {
        var num;
        if (strmonth == "Jan") {
            num = "01"
            return num;
        }
        else if (strmonth == "Feb") {
            num = "02"
            return num;
        }
        else if (strmonth == "Mar") {
            num = "03"
            return num;
        }
        else if (strmonth == "Apr") {
            num = "04"
            return num;
        }
        else if (strmonth == "May") {
            num = "05"
            return num;
        }
        else if (strmonth == "Jun") {
            num = "06"
            return num;
        }
        else if (strmonth == "Jul") {
            num = "07"
            return num;
        }
        else if (strmonth == "Aug") {
            num = "08"
            return num;
        }
        else if (strmonth == "Sep") {
            num = "09"
            return num;
        }
        else if (strmonth == "Oct") {
            num = "10"
            return num;
        }
        else if (strmonth == "Nov") {
            num = "11"
            return num;
        }
        else if (strmonth == "Dec") {
            num = "12"
            return num;
        }
    }

    function SetLeaveSerachValues() {            
       window.opener.location = window.opener.location;
       if (window.opener && !window.opener.closed) {
           window.opener.document.getElementById('hdnStatus').value = "<%=Request.QueryString("Status").ToString().ToLower()%>"
           window.opener.document.getElementById('dlLocation').value = "<%=Request.QueryString("Loc").ToString().ToLower()%>"
           window.opener.document.getElementById('txtEmpName').value = "<%=Request.QueryString("Ename").ToString().ToLower()%>"
           window.opener.document.getElementById('txtFromDate').value = "<%=Request.QueryString("FDate").ToString().ToLower()%>"
           window.opener.document.getElementById('txtToDate').value = "<%=Request.QueryString("TDate").ToString().ToLower()%>"
           window.opener.document.getElementById("btnsearch").click();
       }
       window.close();
    }
</script>
</head>
<script language="JavaScript" src="../includes/CalendarControl.js" type="text/javascript">
</script>
<script type="text/javascript" src="../Javascript/json2.js"></script>
<script type="text/javascript">
    function ajaxFunction() {
        var xmlHttp;
        try {
            xmlHttp = new XMLHttpRequest();
        }
        catch (e) {    // Internet Explorer
            try {
                xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e) {
                    alert("Your browser does not support AJAX!");
                    return false;
                }
            }
        }
        return xmlHttp;
    }
    function setCalendarControlDate(year, month, day) {

        calendarControl.setDate(year, month, day);
        var offSt = document.getElementById("<%=txtleaveFrom.ClientID%>").value;
        var offEn = document.getElementById("<%=txtleaveTo.ClientID%>").value;
        var First = document.getElementById("<%= hdnf.ClientID %>").value;
        var Second = document.getElementById("<%= hdnS.ClientID %>").value;


        First = offSt;
        Second = offEn

        if (First != '' && Second != '') {

            // Below is for Convert 15-jan-2013 to 01/15/2013 for Comparison///
            var stMonthName = offSt.substring(3, 6);
            var stdt = new Date(stMonthName + '01, 1900');
            var stMonthNumber = stdt.getMonth() + 1;
            var NewStDt = stMonthNumber + '/' + offSt.substring(0, 2) + '/' + offSt.substring(7, 11)


            var enMonthName = offEn.substring(3, 6);
            var endt = new Date(enMonthName + '01, 1900');
            var enMonthName = endt.getMonth() + 1;
            var NewEnDt = enMonthName + '/' + offEn.substring(0, 2) + '/' + offEn.substring(7, 11)

            ///end///

            startDate = new Date(NewStDt);
            endDate = new Date(NewEnDt);



            if (startDate <= endDate) {
                var url = "../Webservice/Getleavecnt.ashx?Frmdt=" + First + "&Todt=" + Second;

                var xmlHttp = ajaxFunction();

                xmlHttp.open("GET", url, true);
                xmlHttp.onreadystatechange = function () {
                    if (xmlHttp.readyState == 4) {
                        var str = xmlHttp.responseText;
                        if (str != "") {
                            var obj = JSON.parse(str);
                            var Totleave = document.getElementById('<%=TotalLeave.ClientID%>');
                            Totleave.innerHTML = obj;
                        }
                    }

                }
                xmlHttp.send(null);
            }

            else {
                var Totleave = document.getElementById('<%=TotalLeave.ClientID%>');
                Totleave.innerHTML = '';

                alert("From date should be less than or equal to To date.");
            }

        }


    }

    function CheckDate() {


        var offSt = document.getElementById("<%=txtleaveFrom.ClientID%>").value;
        var offEn = document.getElementById("<%=txtleaveTo.ClientID%>").value;

        if (offSt != '' && offEn != '') {

            // Below is for Convert 15-jan-2013 to 01/15/2013 for Comparison///
            var stMonthName = offSt.substring(3, 6);
            var stdt = new Date(stMonthName + '01, 1900');
            var stMonthNumber = stdt.getMonth() + 1;
            var NewStDt = stMonthNumber + '/' + offSt.substring(0, 2) + '/' + offSt.substring(7, 11)


            var enMonthName = offEn.substring(3, 6);
            var endt = new Date(enMonthName + '01, 1900');
            var enMonthName = endt.getMonth() + 1;
            var NewEnDt = enMonthName + '/' + offEn.substring(0, 2) + '/' + offEn.substring(7, 11)

            ///end///

            startDate = new Date(NewStDt);
            endDate = new Date(NewEnDt);



            if (startDate > endDate) {
                var Totleave = document.getElementById('<%=TotalLeave.ClientID%>');
                Totleave.innerHTML = '';
                offSt.innerHTML = '';
                alert("From date should be less than or equal to To date.");
                return false;
            }
            else {
              return true;
            }


        }

      
    }
     
    </script>
<body>
<form id=Form1 runat="server">
  <table width="100%"  border="0" height="100%" cellspacing="0" cellpadding="0" bgcolor="#FFFFEE">
    
    <tr align="center" bgcolor="#C5D5AE">
      <td height="19" valign="top">
	   <font face="Verdana" color="#a2921e" size="4"  ><b>
	  Leave Details </td>
    </tr>
    <tr align="center">
      <td height="19" valign="top">&nbsp;</td>
    </tr>
    <tr align="center">
      <td height="205" align="center">
      <table width="618"  border="1" cellspacing="0" cellpadding="0" height="244" bordercolor="#C5D5AE" >
        <tr>
          <td height="13" colspan="2"  bgcolor="#edf2e6" rowspan="2">
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  <div align="left">Employee Name </div></td>
          <td width="151" nowrap="nowrap"="true"  bgcolor="#FFFFEE" rowspan="2" height="13">
		  <font face="Verdana" size="2" color="#A2921E"><b>
          <asp:Label ID="lblempName" runat="server"  /></td>
          <td width="116" height="1"bgcolor="#edf2e6">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">Leave Type</div></td>
          <td width="149" height="1" bgcolor="#FFFFEE">
		   <font face="Verdana" size="2" color="#A2921E"><b>
           <asp:DropDownList ID="dropLeaveType" runat="server">
             <asp:ListItem value="CL">Casual Leave</asp:ListItem>
             <asp:ListItem value="PL">Paid Leave</asp:ListItem>
             <asp:ListItem value="SL">Sick Leave</asp:ListItem>
             <asp:ListItem value="WL">Leave (Without Pay)</asp:ListItem>
             <asp:ListItem Value="CO">Comp Off</asp:ListItem>
          
           </asp:DropDownList>
           <div align="center"></div></td>
        </tr>
        <tr>
          <td width="116" height="29"  bgcolor="#edf2e6">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">Leave Status </div></td>
          <td width="149" height="28" align="left" bgcolor="#FFFFEE"> <asp:DropDownList ID="lstLeaveStatus" runat="server">
            <asp:ListItem value="p">Pending</asp:ListItem>
            <asp:ListItem value="r">Rejected</asp:ListItem>
            <asp:ListItem value="a">Approved</asp:ListItem>
          </asp:DropDownList>
		    <div align="center"></div></td>
        </tr>
        <tr>
          <td height="32" colspan="2"  bgcolor="#edf2e6">
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  <div align="left">Employee ID </div></td>
          <td width="151" height="32" bgcolor="#FFFFEE">
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  <asp:Label ID="lblempId" runat="server" /></td>
          <td width="116"  bgcolor="#edf2e6" height="68">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">Admin Comments</div></td>
          <td  width="149" bgcolor="#FFFFEE" height="68"> <font face="Verdana" size="2" color="#A2921E"><b>
          <asp:TextBox ID="txtAdminComments" runat="server" TextMode="MultiLine" Font-Name="Vardana"  Height="80%"	   /></td>

        </tr>
        <tr>
          <td rowspan="2" width="59"  bgcolor="#edf2e6" height="40">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">Leave</div></td>
          <td width="82" height="28"  bgcolor="#edf2e6">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">From</div></td>
          <td width="151" nowrap="nowrap"="true" bgcolor="#FFFFEE" height="28"> <font face="Verdana" size="2" color="#A2921E"><b>
          <asp:TextBox id="txtleaveFrom" runat="server"   onclick="popupCalender('txtleaveFrom')" onkeypress="return false;"></asp:TextBox></td>
         <td height="45" bgcolor="#edf2e6" rowspan="2">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">No. Of Leaves Applied</div></td>
          <td width="151"  height="45" bgcolor="#FFFFEE" align="left" rowspan="2"> 
          <font face="Verdana" size="2" color="#A2921E" ><b>
         <%-- <span id="Totleave" style="font-size: 9pt; font-family: Verdana"></span>--%>
           <asp:Label ID="TotalLeave" Font-Names="Verdana" Font-Size="9pt" runat="server"> </asp:Label>
        </tr>
        <tr>
          <td width="82" height="28"  bgcolor="#edf2e6">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">To</div></td>
          <td width="151" nowrap="nowrap"="true" bgcolor="#FFFFEE" height="28"> <font face="Verdana" size="2" color="#A2921E"><b>
          <asp:TextBox id="txtleaveTo" runat="server"  onclick="popupCalender('txtleaveTo')" onkeypress="return false;"></asp:TextBox></td>
           
          
                  </tr>
       
        <tr>
          <td height="45" colspan="2"  bgcolor="#edf2e6">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">Reason of Leave </div></td>
          <td width="151"  height="45" bgcolor="#FFFFEE" align="left"> <font face="Verdana" size="2" color="#A2921E" ><b>
          <asp:Label ID="lblReasonofLeave" runat="server"  Width="80"/></td>
          <td width="116" height="45"  bgcolor="#edf2e6">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">Leave Sanctioned By </div></td>
          <td width="149" height="45"bgcolor="#FFFFEE"> <font face="Verdana" size="2" color="#A2921E"><b>
            <asp:Label ID="lblLeaveSanctionBy" runat="server" BorderColor="#C5D5AE" />            
            <div align="center"></div>
            <div align="center"></div></td>
        </tr>
       
        <tr>
          <td height="47" colspan="2" bgcolor="#edf2e6">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">Leave Entry On </div></td>
          <td width="151"  bgcolor="#FFFFEE" height="47"> <font face="Verdana" size="2" color="#A2921E"><b>
          <asp:Label ID="lblLeaveEntryDate" runat="server" /></td>
          <td width="116"  bgcolor="#edf2e6" height="47">
		   <font face="Verdana" size="2" color="#A2921E"><b>
		   <div align="left">Leave Sanctioned Date </div></td>
          <td width="149" bgcolor="#FFFFEE" height="47"><font face="Verdana" size="2" color="#A2921E"><b>
            <asp:Label ID="lblonDate" runat="server" /></td>
        </tr>
      </table></td>
    </tr>
	<tr align="center">
      <td height="19" valign="top">&nbsp;</td>
    </tr>
	<tr align="center" bgcolor="#C5D5AE">
      <td align="center">
      <asp:Button ID="btnsubmit" Text="Submit" runat="server" onclick="btnsubmit_Click" OnClientClick="javascript:return CheckDate()" />
    <asp:Button ID="btndelete" Text="Delete" runat="server" onclick="btndelete_Click"  Visible="false" OnClientClick="return fun_confirm()"/>
     </td>
      
      </tr>
  <tr align="center">
      <td height="19" valign="top">&nbsp; <asp:HiddenField ID="hdnf" runat="server" />
                                    <asp:HiddenField ID="hdnS" runat="server" /></td>
    </tr>
  </table>

</form>

</body>
</html>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     