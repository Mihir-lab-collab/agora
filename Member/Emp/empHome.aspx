<%@ Page Language="vb" Debug="true" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Intelgain Technologies Pvt Ltd</title>
</head>
<body>
    <form id="Form1" method="post" runat="server">

        <table id="Table3" height="100%" cellspacing="0" cellpadding="2" width="100%" align="center"
            border="0">
            <tr height="10%">
                <td>
                    <EMPHEADER:empHeader ID="Empheader" runat="server"></EMPHEADER:empHeader>
                    <uc1:empMenuBar ID="EmpMenuBar" runat="server"></uc1:empMenuBar>
                </td>
            </tr>
            <tr height="90%">
                   <td>&nbsp;</td>
            </tr>
            </table>
           </form>
        </body>
</html>