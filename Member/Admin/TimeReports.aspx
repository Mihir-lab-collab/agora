<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TimeReports.aspx.vb" Inherits="TimeReports" %>
<%@ Register Src="../controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hours Report</title>
  <link rel="stylesheet" href="../css/style.css" type="text/css" />
</head>
 <%--<uc1:adminMenu ID="AdminMenu1" runat="server" />--%>
<body>
    <form id="form1" runat="server">
  
   
    <table width="100%">
    <tr style="background-color: #C5D5AE">
            <td align="left">
          <font color="#A2921E"></font><font face="Verdana"
										size="2" color="#A2921E"><b>
											<asp:Label ID="lblMonthName" runat="server"></asp:Label>
										</b></font>
            </td>
        </tr>
        <tr >
            <td align="left">
              <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="true" CssClass="manage">
               <HeaderStyle CssClass="hrdstyl" />
        </asp:GridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
