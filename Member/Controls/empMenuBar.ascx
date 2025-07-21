<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../Css/Style.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <%

        If Session("dynoEmpIdSession") & "" = "" Then
            Response.Redirect("../Member/Login.aspx")
        End If
    %>
    <table cellspacing="1" cellpadding="1" width="100%" align="center" border="0">
        <tr>
            <td align="center" style="height: 21px; font-size:12px; color:#A2921E; font-family: Verdana; background-color: #edf2e6;white-space:nowrap">
                <a href="../emp/HolidayWorking.aspx" style="color:#A2921E;">
                 <b>Holiday Working</b></a>
            </td>
             <%  If Session("dynoAdminSession") <> 0 Or Session("dynoEmpIdSession") = Constants.AccessRecruitmenEmpId Or Session("dynoEmpIdSession") = Constants.AccessRecuritmentEmpIdOne Then%>
            <td align="center" style="height: 21px; font-size:12px; color:#A2921E; font-family: Verdana; background-color: #edf2e6;white-space:nowrap">
                <a href="../emp/showCandiate.aspx" style="color:#A2921E;">
                 <b>Recruitment</b></a>
            </td>  
              <% End If%>
            <%
                If Session("dynoAdminSession") <> 0 Then
            %>
            <td align="center" style="height: 21px; font-size:12px; color:#A2921E; font-family: Verdana; background-color: #edf2e6;white-space:nowrap">
                 <a href="../admin/" style="color:#A2921E;">
                 <b>Admin</b></a>
            </td>
            <%
            End If
            %>
             
        </tr>
        <tr>
            <td align="left" bgcolor="#edf2e6" style="height: 18px" colspan="6">
                <font face="Verdana" color="#a2921e" size="2"><b>&nbsp;Welcome <font color="#990066">
                    <%=Session("dynoEmpNameSessionHeader")%>
                </font></b></font>
            </td>
        </tr>
    </table>
</body>
</html>
