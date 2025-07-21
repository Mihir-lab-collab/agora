<%@ Page Language="VB" AutoEventWireup="false"%>
<%@import namespace="system.data"%>
<%@import namespace="system.data.SQLClient"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>DWT Admin</title>
</head>
<body style="font-family:Verdana; font-size:small">
    <form id="form1" runat="server">
    <div>
    <% 
		Dim currDate = Request.QueryString("d")
		If NOT IsDate(currDate) Then
			currDate = Now()
		End If
		
		currDate = FormatDateTime(currDate,0)

        Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        Dim cmd As New SqlCommand
        Dim dr As SqlDataReader
        Dim sql As String
        Dim startDate As Date
        Dim endDate As Date
        Dim dayArr(31) As String
        startDate = "1-" & MonthName(Month(currDate)) & "-" & Year(currDate)
        endDate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, startDate))
        con.Open()
        sql = "SELECT * FROM employeeMaster LEFT JOIN empATT ON " & _
        "employeeMaster.empId=empATT.empID WHERE attStatus='P' AND " & _
        "empLeavingDate IS NULL AND attDate BETWEEN '" & startDate & "' AND '" & endDate & "' " & _
        "AND IsSuperAdmin = 0 " & _
        "ORDER BY employeeMaster.empName, attDate"
        cmd = New SqlCommand(sql, con)
        dr = cmd.ExecuteReader()
        Dim empId As String = ""
        Dim i As Integer = 0
 %>
<table border="1" cellspacing="0" cellpadding="2" >
    <tr>
        <td colspan="31"><%=Format(startDate, "MMM yyyy")%></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
<% 
    Do While (dr.Read)
        If empId <> "" And empId <> dr("empId").ToString() Then
            Exit Do
        End If
        dayArr(i) = Day(dr("attDate"))
%>
        <td><%=Day(dr("attDate"))%> <%=Format(dr("attDate"), "ddd")%></td>
<%      
		empId = dr("empId")
	    i = i + 1
	Loop
	dr.Close()
%>
    </tr>
    <tr>
<%        
    dr = cmd.ExecuteReader()
    empId = ""
    i = 0
    Do While (dr.Read)
        If empId <> dr("empID").ToString() Then
            Response.Write("</TR><TR><td>" & dr("empName") & "</td>")
            empId = dr("empID").ToString()
            i = 0
        End If
a:
        If dayArr(i) <> Day(dr("attDate")) Then
            Response.Write("<td align='center'><font color='red'>A</font></td>")
            i = i + 1
            GoTo a
        End If
        
        Dim inTime As Date = dr("attInTime")
        Dim outTime As Date = dr("attOutTime")
        Dim stay As Double = Math.Round(DateDiff("n", inTime, outTime) / 60, 1)
        If stay > 8 Or (stay > 6 And Weekday(inTime) = 7) Then
            Response.Write("<td align='center'>" & Format(inTime, "hh:mm") & " - " & _
                           Right("00" & Hour(outTime), 2) & ":" & Right("00" & Minute(outTime), 2) & "<BR/><b>" & _
                           stay & "hrs</b></td>")
        Else
            Response.Write("<td align='center'><font color='red'>" & Format(inTime, "hh:mm") & " - " & _
                           Right("00" & Hour(outTime), 2) & ":" & Right("00" & Minute(outTime), 2) & "<BR/><b>" & _
                           stay & "hrs</b></font></td>")
        End If
        i = i + 1
		Response.End
    Loop
    %>        
</table>    
    </div>
    </form>
</body>
</html>
