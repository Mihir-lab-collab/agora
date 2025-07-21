<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pdfCreatorNew.aspx.cs" Inherits="Admin_pdfCreator" %>

<%@ OutputCache Duration="1" Location="None" NoStore="true" VaryByParam="none" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Registration Form </title>
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />

    <script language="JavaScript" type="text/javascript" src="/includes/CalendarControl.js">
    </script>

    <style type="text/css">
        .style1
        {
            width: 341px;
        }
        .style2
        {
            width: 247px;
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
                        <img src="Images/dynamic_logo1.gif" border="0"></a>
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
        <table cellspacing="0" cellspacing="0" width="70%" border="0" align="center">
            <tr>
                <td align="right">
                    Form No :
                    <asp:Label ID="lblFormNumber" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table cellspacing="0" cellspacing="0" width="70%" border="0" align="center">
            <tr>
                <td>
                    Instruction :
                </td>
            </tr>
            <tr>
                <td>
                    <ul>
                        <li>Registration for the seminar will be on the basis of first come-first serve basis</li>
                        <li>Candidates can Fax the registration forms for the seminar to 022-41516101. On submission,
                            they will be provided with a confirmation number.</li>
                        <li> Candidates facing problem with Fax , can courier at Dynamic Web Technologies Pvt. Ltd. , B-203, Sanpada Station Complex, Navi Mumbai-400705 , Tel: +91(22) 41516100 . </li>
                    </ul>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table cellpadding="4" cellspacing="0" width="70%" border="0" align="center">
            <tr>
                <td colspan="4" align="center">
                    Registration Form Details
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center" class="style1">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2" valign="top">
                    Name
                </td>
                <td>
                    <asp:Label ID="lblFirstName" CssClass="text" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblMiddleName" CssClass="text" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblLastName" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Date Of Birth
                </td>
                <td colspan="3">
                    <asp:Label ID="lblDOB" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Gender
                </td>
                <td colspan="3">
                    <asp:Label ID="lblGender" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2" valign="top">
                    Address
                </td>
                <td colspan="3" valign="top">
                    <asp:Label ID="lblAddress" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    State
                </td>
                <td colspan="3">
                    <asp:Label ID="lblState" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    City
                </td>
                <td colspan="3">
                    <asp:Label ID="lblCity" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trPhoneNo" runat="server" visible="false">
                <td class="style2">
                    Phone No
                </td>
                <td colspan="3">
                    <asp:Label ID="lblPhoneNo" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Mobile No
                </td>
                <td colspan="3">
                    <asp:Label ID="lblMobileNo" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Email ID
                </td>
                <td colspan="3">
                    <asp:Label ID="lblEmail" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Education
                </td>
                <td colspan="3">
                    <asp:Label ID="lblEducation" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trOther" runat="server" visible="false">
                <td class="style2">
                    Other Details
                </td>
                <td colspan="3">
                    <asp:Label ID="lblOtherDetails" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2" valign="top">
                    I am interested in training because
                </td>
                <td colspan="3" valign="top">
                    <asp:Label ID="lblIntersted" CssClass="text" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>
