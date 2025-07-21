<%@ Page Language="C#" AutoEventWireup="true" CodeFile="thanksRegistration.aspx.cs"
    Inherits="thanksRegistration" %>
<%@ OutputCache Duration="1" Location="None" NoStore="true" VaryByParam="none" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Registration Form </title> 

    <style type="text/css">
        .text
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: normal;
            color: #141313;
        }
        .textbox
        {
            border: 1px #CCCCCC solid;          
            font-size: 12px;
            font-family: Helvetica, Arial, sans-serif;
            color: #777272;
            background-color: #FFFFFF;
        }
        .error
        {
            font-size: 12px;
            font-family: Helvetica, Arial, sans-serif;
            margin-left: 5px;
            color: #eb2929;
        }
        html, body, #wrapper
        {
            min-height: 100%; /*Sets the min height to the
                       height of the viewport.*/
            width: 100%;
            height: 100%; /*Effectively, this is min height
                   for IE5+/Win, since IE wrongly expands
                   an element to enclose its content.
                   This mis-behavior screws up modern
                   browsers*/
        }
        html > body, html > body #wrapper
        {
            height: auto; /*this undoes the IE hack, hiding it
                   from IE using the child selector*/
        }
        body
        {
            margin: 0 auto;
        }
        #wrapper
        {
            position: absolute;
            top: 0;
            left: 0;
            padding-bottom: 70px;
        }
        #footer
        {
            position: absolute;
            bottom: 0;
            height: 60px;
        }
        .style3
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: normal;
            color: #141313;
            width: 246px;
        }
         .heading
        {
            color: #333333;
            font-weight: bold;
            height: 18px;
        }
    </style>
</head>
<body>
    <div id="wrapper">
        <form id="form1" runat="server">
        <table cellspacing="0" cellpadding="0" width="70%" align="center" border="0">
            <tr>
                <td style="width: 200px;">
                    <a href="../emp/empHome.aspx" border="0">
                        <img src="../images/dynamic_logo1.gif" border="0"></a>
                </td>
                <td style="width: 100%;" colspan="2">
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
                                <font size="2" face="Verdana"><b>Trainee Registration </b></font>
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
        <br />
        <table cellpadding="4" cellspacing="0" width="50%" border="0" align="center">           
             <tr>
                <td colspan="5" align="center" class="heading">
                   Thank You
                </td>
            </tr>   
             <tr>
                <td colspan="5">
                   &nbsp;
                </td>
            </tr>   
            <tr >            
                <td colspan="5">
                    <p class="text">
                        Thank you for registering yourself.
                        <br /><br />
                        Once your registration is processed, we will revert you back with a receipt number,
                        which has to be carried for the seminar .
                        <br /><br /> Please Fax the form, as per the instruction
                        mentioned on the print format to register yourself.
                        <br /><br />
                        <a href="pdfConverter.aspx?RegID=<%=Session["LastInsertRegID"]%>"> Click here </a>
                        to download the PDF.<br /><br />
                        Regards,<br /><br />
                        Learning Center <br /><br />
                        Dynamic Web Technologies Pvt. Ltd.
                    </p>
                </td>                
            </tr>        
        </table>
        </form>
        <div id="footer">
            <table width="70%" align="center" style="margin:0 auto;">
                <tr>
                    <td align="center" class="text">
                        Dynamic Web Technologies Pvt. Ltd.
                    </td>
                </tr>
                <tr>
                    <td align="center" class="text">
                        B-203, Sanpada Station Complex, Navi Mumbai-400705, India
                    </td>
                </tr>
                <tr>
                    <td align="center" class="text">
                        Tel: +91(22) 41516100 Fax: 022-41516101 Email: <a style="text-decoration: none;"
                            href="mailto:Corp.training@dynamicwebtech.com">Corp.training@dynamicwebtech.com</a>
                        Web: <a href="http://www.dynamicwebtech.com" style="text-decoration: none;" target="_blank"
                            title="website link">www.dynamicwebtech.com</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</body>
</html>