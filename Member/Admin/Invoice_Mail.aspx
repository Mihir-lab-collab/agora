<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invoice_Mail.aspx.cs" Inherits="Admin_Invoice_Mail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice Mail</title>
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style12.css" rel="stylesheet" type="text/css" />
    <link href="../Css/layout.css" rel="stylesheet" type="text/css" />
   
</head>
<body>
     <form id="frmInvMail" runat="server">
    <div>
        <table id="tblInvMail" align="center" height="100" border="0" cellpadding="3" 
            cellspacing="3" runat="server">
            <tr>
                <td>
                    To:
                </td>
                <td>
                    <asp:Label ID="lblTo" runat="server" Text=""></asp:Label>
                </td>
                
            </tr>
            <tr>
                <td>
                    Cc:
                </td>
                <td>
                    <asp:TextBox ID="txtCc" size="60" runat="server"></asp:TextBox>
                </td>
               
                <td>
                     Attached Documents:
                    <div ID="dvAttachnemt" runat="server"></div>
                   
                </td>
            </tr>
            <tr>
                <td>
                    Bcc:
                </td>
                <td>
                   <asp:TextBox ID="txtBcc" size="60" runat="server" Text = "accounts@intelegain.com" ></asp:TextBox>
                    
                </td>
                <td></td>
            </tr>
            <tr> 
                <td>
                    Subject:
                </td>
                <td>
                    <asp:TextBox ID="txtSubject" size="60" runat="server"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td valign="top" colspan="3">
                    <asp:TextBox ID="txtEmail" runat="server" Rows="15" Width="400"  TextMode="MultiLine"  ></asp:TextBox>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                 
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="custbutton" colspan="2">
                    <asp:Button ID="btnSendInvoice" runat="server" Text="Send Invoice" 
                        onclick="btnSendInvoice_Click" />
                </td>
            </tr>
            
        </table>
    </div>
    </form>
</body>
</html>
