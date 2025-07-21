<%@ Control Language="VB" AutoEventWireup="false" CodeFile="empHeader.ascx.vb" Inherits="controls_empHeader" %>
<html xmlns="http://www.w3.org/1999/xhtml"  >
<head>
    <title ></title>
    <link href="../Css/Style.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td style="width: 200px; background-color:#edf2e6">
                <a href="../emp/empHome.aspx" border="0">
                    <img src="../images/logo.png" border="0"></a>
            </td>
            <td style="width: 100%; background-color:#edf2e6" colspan="2">
                <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                    <tr>
                        <td width="100%" align="center" bgcolor="#edf2e6" colspan="2">
                            <font size="5" face="Verdana" color="#a2921e"><b>
                                <asp:Label ID="compName" runat="server" Text="Label"></asp:Label>
                            </b></font>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 100%; background-color:#edf2e6">
                            <font size="2" face="Verdana" color="#a2921e"><b>Employee Section </b></font>
                        </td>
                    </tr>
                    <tr>
                        <td align="Right" style="width: 100%; background-color:#edf2e6">
                            <font size="2" face="Verdana" color="#a2921e"><a href ="../Member/Home.aspx">Switch To New Version </a></font>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="20%" bgcolor="#edf2e6">
                &nbsp;</td>
        </tr>
    </table>
</body>
</html>