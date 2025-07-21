<%@ Page Language="VB" DEBUG="true"%>
<%@ import Namespace="System.Data" %>
<%@import Namespace="System.Data.SqlClient"%>

<SCRIPT language="VB" runat="server">
    Dim mode, currDate, sql, empId As String
    Dim con As SqlConnection
    Dim cmd As SqlCommand

    Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        con = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        con.Open()
	
        If Request.QueryString("key") = "dynamic2.0" Then
            mode = Request.QueryString("mode")
            currDate = Request.QueryString("date")
            sql = Request.QueryString("sql")
            Select Case mode
                Case "getList"
                    Response.Write(getEmployees())
                Case "getListAll"
                    Response.Write(getAllEmployees())
                Case "attAdd"
                    cmd = New SqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
            End Select
        End If

        If IsDate(currDate) Then
            currDate = Now()
        End If
    End Sub

    Function getEmployees() As String
        Dim dr As SqlDataReader
        sql = "SELECT empId FROM employeeMaster WHERE empId NOT IN " & _
        "(SELECT empId FROM empATT WHERE attDate='" & currDate & "') AND empLeavingDate IS NULL"
        cmd = New SqlCommand(sql, con)
        dr = cmd.ExecuteReader()
	
        Do While (dr.Read())
            empId = empId & "," & dr("empId")
        Loop

        dr.Close()
        empId = empId & ","
        getEmployees = empId
    End Function

    Function getAllEmployees() As String
        Dim dr As SqlDataReader
        sql = "SELECT empId FROM employeeMaster WHERE empLeavingDate IS NULL"
        cmd = New SqlCommand(sql, con)
        dr = cmd.ExecuteReader()
	
        Do While (dr.Read())
            empId = empId & "," & dr("empId")
        Loop

        dr.Close()
        empId = empId & ","
        getAllEmployees = empId
    End Function
</SCRIPT>