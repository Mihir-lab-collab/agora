<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adminPdfCreatorNew.aspx.cs"
    Inherits="Admin_pdfCreator" %>

<%@ OutputCache Duration="1" Location="None" NoStore="true" VaryByParam="none" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Registration Form </title>
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />

    <script language="JavaScript" type="text/javascript" src="/includes/CalendarControl.js">
    </script>

    <style type="text/css">
        .style3
        {
            width: 93px;
        }
    </style>
</head>
<body>
    <div id="wrapper">
        <form id="form1" runat="server">
        <table cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
            <tr>
                <td>
                    <a href="../emp/empHome.aspx" border="0">
                        <img src="../Images/dynamic_logo1.gif" border="0"></a>
                </td>
                <td colspan="2">
                    <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                        <tr>
                            <td width="100%" align="center" colspan="2">
                                <font size="4" face="Verdana"><b>
                                    <asp:Label ID="compName" runat="server" Text="Dynamic Web Technologies Pvt Ltd"></asp:Label>
                                </b></font>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="width: 100%;">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="20%">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <br />
        <br />
       <table cellspacing="0" cellpadding="0" width="70%" align="center" border="0">
            <tr>             
                <td align="right">
                 Form No :
                    <asp:Label ID="lblFormNumber" runat="server"></asp:Label>
                </td>            
            </tr>
        </table>
        <br />
        <br />
        <br />
        <br />
        <br />
        <table cellpadding="4" cellspacing="0" width="70%" border="0" align="center">
            <tr>
                <td colspan="2" align="center">
                    Receipt Details
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    Thanks for registering for the seminar.
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    Please find the seminar details mentioned below :
                </td>
            </tr>
             <tr>
                <td colspan="2" align="center">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <table>
                        <tr>
                            <td class="style3" valign="top" align="left">
                                Date :
                            </td>
                            <td align="left" valign="top">
                                <asp:Label ID="lblDOB" CssClass="text" runat="server">18th October 2008</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3" valign="top" align="left">
                                Time :
                            </td>
                            <td align="left" valign="top">
                                <asp:Label ID="Label1" CssClass="text" runat="server">10:00 AM</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3" valign="top" align="left">
                                Venue Details :
                            </td>
                            <td align="left" valign="top">
                                Dynamic Web Technologies Pvt. Ltd,
                                <br />
                                B-203, Sanpada Station Complex,<br />
                                Sanpada Navi Mumbai, 400705.
                            </td>
                        </tr>
                        <tr>
                            <td class="style3" valign="top">
                                &nbsp;
                            </td>
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>
