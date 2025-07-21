<%@ Page Language="C#" AutoEventWireup="true" CodeFile="empAttendanceDetail.aspx.cs" Inherits="Admin_empAttendanceDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Attendance report</title>
     <link rel="stylesheet" href="../css/style.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%">
    <tr style="background-color: #C5D5AE">
    <td align="left" >
								
								 <font color="#A2921E"></font><font face="Verdana"
										size="2" color="#A2921E"><b>
										      
											<asp:Label ID="lblMonthName" runat="server"></asp:Label>
											
										</b>
											
											</td>
     </tr>
     <tr>
     <td>
    
         <asp:GridView ID="grdattReport" runat="server"  AutoGenerateColumns="False"    Width="500px" CssClass="manage">
             <Columns>
                <asp:BoundField ItemStyle-Width="50px"  DataField="EmpID" HeaderText="EmpID" />
                <asp:BoundField ItemStyle-Width="250px"  DataField="EmpName" HeaderText="Employee Name" />
                <asp:BoundField ItemStyle-Width="50px"  DataField="Late" HeaderText="Late" />
                <asp:BoundField ItemStyle-Width="50px"  DataField="NineHour" HeaderText="NineHour" />
                <asp:BoundField ItemStyle-Width="50px"  DataField="HalfDay" HeaderText="Half Day" />
                <asp:BoundField ItemStyle-Width="50px"  DataField="Zero" HeaderText="Zero" />
             </Columns>
             <HeaderStyle CssClass="hrdstyl" />
        </asp:GridView>

     </td>
     </tr>
   
    </table>
    </form>
</body>
</html>
