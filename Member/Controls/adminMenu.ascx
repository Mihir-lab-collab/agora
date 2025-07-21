<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<%
    If Session("dynoEmpIdSession") & "" = "" Or Session("dynoAdminSession") = 0 Then
        Response.Redirect("/emp/emplogin.aspx")
    End If
%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dynamic Web Tech</title>
    <link href="../Css/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <table id="tblAdminMenu1" cellspacing="0" cellpadding="2" width="100%" style="border-color: #808080;
        height: 0%" border="1">
        <tr>
            <td colspan="14">
                <EMPHEADER:empHeader ID="Empheader1" runat="server"></EMPHEADER:empHeader>
                <br/>
            </td>
        </tr>
    </table>
    <table id="tblAdminMenu" runat="server" cellspacing="0" cellpadding="2" border="1"
        style="border-color: #808080;" width="100%" bordercolorlight="#C0C0C0" bordercolordark="#FFFFFF">
        <tr>
            <td colspan="15">
            </td>
        </tr>
        <tr>
            <td align="center" width="15%" bgcolor="#edf2e6">
                <b><font face="Verdana" size="2"><a href="../emp/emphome.aspx" style="color:#A2921E;"><font color="#A2921E">
                    Home</font></a></font></b></td>
            <td align="center" width="15%" bgcolor="#edf2e6">
                <b><font face="Verdana" size="2"><a href="../admin/holiday.aspx" style="color:#A2921E;"><font color="#A2921E">Holiday</font></a></font></b></td>
            <td align="left" wodth="15%" bgcolor="#edf2e6" colspan="1" style="padding-left: 8px">
                <b><font face="Verdana" size="2"><a href="../admin/compOff.aspx" style="color:#A2921E;"><font color="#A2921E">Comp Off</font></a></font></b>&nbsp;
            </td>     
              <td align="center" width="6%" bgcolor="#edf2e6">
                <b><font face="Verdana" size="2"><a href="../admin/empLeaveRequests.aspx" style="color:#A2921E;"><font color="#A2921E">
                    Leaves</font></a></font></b></td>
              <td align="left" wodth="15%" bgcolor="#edf2e6" style="padding-left: 8px">
                <b><font face="Verdana" size="2"><a href="../Admin/traineeApplication.aspx?search=All" style="color:#A2921E;"><font color="#A2921E">Trainee Application</font></a></font></b>&nbsp;
            </td> 
            <td align="left" wodth="15%" bgcolor="#edf2e6" style="padding-left: 8px">
                <b><font face="Verdana" size="2"><a href="../Admin/empHolidayWorkDetails.aspx" style="color:#A2921E;"><font color="#A2921E">Holiday Working</font></a></font></b>&nbsp;
            </td>          
            <td align="center" width="15%" bgcolor="#edf2e6">
                <b><font face="Verdana" size="2"><a href="../Member/Login.aspx?logout=true" style="color:#A2921E;"><font color="#A2921E">
                    Logout</font></a></font></b>&nbsp;
            </td> 
        </tr>
    </table>
</body>
</html>
