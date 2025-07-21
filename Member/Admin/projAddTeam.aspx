<%@ Page Language="VB" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Language" content="en-us"/>
    <title>Add Team</title>
</head>
<body>
    <%
        Dim projId As String = Request("projId")
        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        conn.Open()
        Dim sql As String
        Dim i As Integer
        If Request("submitBT") <> "" Then
            sql = "DELETE FROM projectMember WHERE projid=" & projId
            Dim Cmd1 As New SqlCommand(sql, conn)
            Cmd1.ExecuteNonQuery()
            Dim empIdArr = Split(Request("chk"), ",")
            For i = 0 To UBound(empIdArr)
                If IsNumeric(empIdArr(0)) Then
                    sql = "INSERT INTO projectMember VALUES(" & projId & "," & empIdArr(i) & ")"
                    Cmd1.CommandText = sql
                    Cmd1.ExecuteNonQuery()
                End If
            Next
    %>

    <script language="JAVASCRIPT" type="text/javascript">
        window.close()
        opener.location.href=opener.location.href
    </script>

    <%	
        Response.End()
    End If
    Dim projName As String = String.Empty
    Dim Rdr As SqlDataReader
    sql = "select * from projectMaster where projId=" & projId
    Dim Cmd As New SqlCommand(sql, conn)
    Rdr = Cmd.ExecuteReader()
    If Rdr.Read() Then
        projName = Rdr("projName")
    End If
    Rdr.Close()
    %>
    <form method="POST" action="projAddTeam.aspx?projid=<%=projid%>">
        <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
            bordercolor="#111111" width="100%">
            <tr>
                <td width="100%" colspan="4" align="center">
                    <b>Team Members</b></td>
            </tr>
            <tr>
                <td width="50%" colspan="2">
                    &nbsp;<b><%=projName%></b>&nbsp;</td>
                <td width="50%" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td width="100%" colspan="4" align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <%
                    sql = "SELECT * FROM employeeMaster LEFT JOIN (SELECT * FROM projectMember WHERE projId=" & projId & _
                    ") a ON employeeMaster.empId=a.empid WHERE empLeavingDate IS NULL ORDER BY empName"
                    Cmd.CommandText = sql
                    Rdr = Cmd.ExecuteReader()
                    Dim count = 0
                    Do While Rdr.Read()
                        If count Mod 2 = 0 And count > 0 Then
                %>
            </tr>
            <tr>
                <%
                End If
                %>
                <td width="25%">
                    <%=rdr("empName")%>
                    &nbsp;</td>
                <td width="25%" align="center">
                    <input type="checkbox" name="chk" value="<%=Rdr("empId")%>" <%if Rdr("projid") & "" = projid%>CHECKED<%END If%>></td>
                <%
                    count = count + 1
                Loop
                Rdr.Close()

                If count Mod 2 <> 0 Then
                %>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <%
                End If
                %>
            </tr>
            <tr>
                <td width="100%" colspan="4" align="center">
                    <input type="submit" value="Submit" name="submitBT"></td>
            </tr>
        </table>
    </form>
</body>
</html>
