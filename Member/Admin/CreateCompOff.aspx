<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CreateCompOff.aspx.vb" Inherits="Admin_CreateCompOff" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table border="1" cellpadding="4" style="border-color: #E8E8E8;">
        <tr>
            <td style="background-color: #edf2e6; width: 25%">
                <font face="Verdana" color="#a2921e" size="2"><b>Employee Name </b></font>
            </td>
            <td>
                <font face="Verdana" size="2" color="#A2921E"><b>
                    <div align="left">
                        <asp:Label ID="lblempname" runat="server"></asp:Label>
                    </div>
                </b></font>
            </td>
        </tr>
        <tr>
            <td style="background-color: #edf2e6; width: 25%">
                <font face="Verdana" color="#a2921e" size="2"><b>Comp Off Date</b></font>
            </td>
            <td>
                <font face="Verdana" size="2" color="#A2921E"><b>
                    <div align="left">
                        <asp:Label ID="lblCompofday" runat="server" ></asp:Label>
                    </div>
                </b></font>
            </td>
        </tr>
        <tr>
            <td style="background-color: #edf2e6; width: 25%">
                <font face="Verdana" color="#a2921e" size="2"><b>Project</b></font>
            </td>
            <td>
                <font face="Verdana" size="2" color="#A2921E"><b>
                    <div align="left">
                        <asp:Label ID="lblProject" runat="server"></asp:Label>
                    </div>
                </b></font>
            </td>
        </tr>
        <tr>
            <td style="background-color: #edf2e6; width: 25%">
                <font face="Verdana" color="#a2921e" size="2"><b>Comp Off Comment</b></font>
            </td>
            <td nowrap="nowrap">
                <asp:TextBox ID="txtComment" runat="server" Columns="30" Rows="5" TextMode="MultiLine"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valrDate" runat="server" Display="Dynamic" ControlToValidate="txtComment"
                    ErrorMessage="Required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" BackColor=" #C5D5AE" Font-Size="10pt"
                    ForeColor=" #A2921E" Font-Bold="true" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
