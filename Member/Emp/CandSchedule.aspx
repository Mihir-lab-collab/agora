<%@ Page Language="VB" AutoEventWireup="false" CodeFile="candSchedule.aspx.vb" Inherits="emp_candSchedule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script language="JavaScript" type="text/javascript" src="../includes/CalendarControl.js" > </script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Candiate Schedule</title>
</head>
<body >
    <form id="form1" runat="server">
    <div>
    <table cellspacing="1" cellpadding="4"  align="center"  border="2" bordercolor="#F1F4EC" width="100%">
          <tr>
          <td colspan="3" align="center"  style="background: #C5D5AE" ><font  face="Verdana" color="#a2921e"><b>Schedule</b></font></td>
        </tr>
       
        
        <tr>
						<td align="center">
							<table id="Table1" borderColor="#c5d5ae" cellSpacing="0" cellPadding="4" border="1" width="100%">
								
								<tr>
									<td nowrap="nowrap" bgColor="#edf2e6"  align="left"  ><B><font face="Verdana" color="#a2921e" size="2">Candiate Name</font></B></td>
									<td nowrap="nowrap"   align="left" ><font face="Verdana"  size="2">
                                        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label></font>&nbsp;</td>
								</tr>
								
								<tr>
									<td nowrap="nowrap" bgColor="#edf2e6" align="left" ><b><font face="Verdana" color="#a2921e" size="2">Candiate Status</font></B></td>
									<td nowrap="nowrap"  align="left">
                                        <asp:DropDownList ID="ddlstatus" runat="server"> </asp:DropDownList> </td>
   							    </tr>
   							    
								<tr>
								    <td nowrap="nowrap" bgColor="#edf2e6" align="left" ><b><font face="Verdana" color="#a2921e" size="2">Schedule Date</font></B></td>
								    <td align="left">  <asp:TextBox ID="txtdate" runat="server"  Font-Size="Smaller" Font-Names="Verdana"  Columns="28" onclick="showCalendarControl(this);"></asp:TextBox>	</td>
								</tr>
								
								<tr>
								    <td nowrap="nowrap" bgColor="#edf2e6" align="left" valign="top" ><b><font face="Verdana" color="#a2921e" size="2">Schedule Description</font></B></td>
								    <td align="left">  <asp:TextBox ID="txtcomment" runat="server"  Font-Size="Smaller" TextMode="MultiLine" Rows="3"  Font-Names="Verdana"  Columns="60"  ></asp:TextBox>	</td>
								</tr>
								
								<tr align="center" >
							        <td colspan="2">
								     <asp:Button id="btnSave"   runat="server" Width="90px" BorderWidth="1px" BackColor="#EDF2E6" BorderStyle="Groove" BorderColor="#A2921E" Text="submit"></asp:Button> &nbsp;
								     <asp:Button id="btnClose"  onClientClick="javascript:window.close();return false;"  runat="server" Width="90px" BorderWidth="1px" BackColor="#EDF2E6" BorderStyle="Groove" BorderColor="#A2921E" Text="Close"></asp:Button>
								     </td>
								</tr>
						   </table> 
						  </td> 
			</tr> 
    </table> 
    </div>
    </form>
</body>
</html>
