<%@ Page Language="VB" AutoEventWireup="false" CodeFile="candSchSearch.aspx.vb" Inherits="emp_candSchSearch" %>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script language="JavaScript" type="text/javascript" src="../includes/CalendarControl.js" > </script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Candiate Schedule Search</title>
    <link rel="stylesheet" href="/includes/CalendarControl.css"  type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <table cellspacing="1" cellpadding="4"  align="center"  border="2" bordercolor="#F1F4EC" width="100%">
         
          <tr>
          <td colspan="3" >
            <table  cellspacing="0" cellpadding="2" width="100%" border="0" >
              <tr>
                <td ><EMPHEADER:empHeader id="Empheader" runat="server"></EMPHEADER:empHeader></td>
              </tr>
              <tr>
                <td ><uc1:empMenuBar id="EmpMenuBar" runat="server"></uc1:empMenuBar></td>
              </tr>
          </table></td>
         
        </tr>
          <tr>
          <td colspan="3"  align="center"  style="background: #C5D5AE" ><font  face="Verdana" color="#a2921e"><b>Search Candidate</b> </font> </td>
        </tr>
        <tr>
            <td style="background: #C5D5AE" width="25%" ><font  face="Verdana" size="2" color="#a2921e"><b>Date</b> </font> </td>
            <td width="20%">
                <asp:TextBox ID="txtdate" runat="server" onclick="popupCalender('txtdate');"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btnsearch" runat="server" Text="Search" BorderWidth="1px"  BackColor="#EDF2E6" BorderStyle="Groove" BorderColor="#A2921E"    />
			    </td>
			      <td style="background: #C5D5AE" width="25%" colspan="3" ><font  face="Verdana" size="2" color="#a2921e"><b>Month</b> </font> </td>
			    <td><select runat="server" id="dropempExpyears" name="empExpyears">
					                    <option value="0"  selected >Month</option>
					                    <option value="1" >JAN</option>
					                    <option value="2" >FEB</option>
					                    <option value="3" >MAR</option>
					                    <option value="4" >APR</option>
					                    <option value="5" >MAY</option>
					                    <option value="6" >JUN</option>
                                        <option value="7" >JUL</option>
					                    <option value="8" >AUG</option>
					                    <option value="9" >SEP</option>
					                    <option value="10">OCT</option>
					                    <option value="11">NOV</option>
					                    <option value="12">DEC</option>
                    					
					                    </select></td>
        </tr>
        <tr>
            <td colspan="3" >
        <asp:GridView ID="gridShowCandiate" runat="server" AutoGenerateColumns="False" BorderWidth="1px" BackColor="LightGoldenrodYellow"
            GridLines="both" CellPadding="2" BorderColor="Tan" ForeColor="Black" AllowPaging="True"  Width="100%"
            AllowSorting="True" EmptyDataText="No Record Found"    >
             <PagerStyle ForeColor="DarkSlateBlue" HorizontalAlign="Center" BackColor="PaleGoldenrod"></PagerStyle>
             <RowStyle   Font-Names="Verdana"  Font-Size="Small"  ForeColor="#000000" BackColor="#FFFFEE" VerticalAlign="Top" ></RowStyle>
             
           <HeaderStyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100px"></HeaderStyle>
            
            <Columns>
                <asp:BoundField  HeaderText="ID"   DataField="ccid"></asp:BoundField>
                  <asp:BoundField   HeaderText="Name"   DataField="ccname"></asp:BoundField>
                    <asp:BoundField   HeaderText="Post Applied"   DataField="ccpost"></asp:BoundField>
                      <asp:BoundField   HeaderText="Status"   DataField="ccstatus"></asp:BoundField>
                        <asp:BoundField   HeaderText="Scheduled date"   DataField="ccdate"></asp:BoundField>
              </Columns> 
              </asp:GridView> 
             </td>
        </tr>
        </table> 
    </div>
    </form>
</body>
</html>
