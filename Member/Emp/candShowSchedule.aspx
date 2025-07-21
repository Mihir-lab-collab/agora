<%@ Page Language="VB" AutoEventWireup="false" CodeFile="candShowSchedule.aspx.vb"
    Inherits="emp_candShowSchedule" %>

<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>candidate Show Schedule</title>
    <link rel="stylesheet" href="/includes/CalendarControl.css" type="text/css" />

    <script language="JavaScript" type="text/javascript" src="../includes/CalendarControl.js"> </script>

</head>
<body>
    <form id="form1" runat="server">
    <table id="Table1" bordercolor="#c5d5ae" cellspacing="0" cellpadding="4" border="1"
        width="850px">
        <tr>
            <td bgcolor="#edf2e6" style="white-space: nowrap" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Name</font></b>
            </td>
            <td style="white-space: nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="lblName" runat="server"></asp:Label></font>&nbsp;
            </td>
            <td style="white-space: nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Date of Birth</font></b>
            </td>
            <td style="white-space: nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="lblDate" runat="server"></asp:Label></font>&nbsp;
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Contact Number</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="candMobile" runat="server"></asp:Label></font>&nbsp;
            </td>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Refrence (Contact Number)</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="caneRelative" runat="server"></asp:Label></font>&nbsp;
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Post Applied For</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="lblPost" runat="server"></asp:Label></font>&nbsp;
            </td>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Address</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="lblPermAdd" runat="server"></asp:Label></font>&nbsp;
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Current Salary (Gr.)</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="lblCurrSal" runat="server"></asp:Label></font>&nbsp;
            </td>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Expected Salary (Gr.)</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="lblExpectedsal" runat="server"></asp:Label></font>&nbsp;
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Previous Employer</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="lblPrevious" runat="server"></asp:Label></font>&nbsp;
            </td>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Reason For Change the Job</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="lblReason" runat="server"></asp:Label></font>&nbsp;
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Total Experience in Yrs</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2">
                    <asp:Label ID="lblExp" runat="server"></asp:Label></font>&nbsp;
            </td>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                <b><font face="Verdana" color="#a2921e" size="2">Resume</font></b>
            </td>
            <td nowrap="nowrap" align="left">
                <font face="Verdana" size="2"></font>&nbsp;<asp:LinkButton ID="lnkResume" runat="server">View</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3" style="background: #C5D5AE">
                <font face="Verdana" color="#a2921e"><b>Candiate Schedule</b></font>
            </td>
            <td colspan="4" style="background: #C5D5AE">
                <font face="Verdana" color="#a2921e"><b>
                    <input type="button" borderwidth="1px" onclick="addsch();" backcolor="#EDF2E6" borderstyle="Groove"
                        bordercolor="#A2921E" id="btnAddSchedule" value="Add Schedule" /></b> </font>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:GridView ID="gridSchedule" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
                    BackColor="LightGoldenrodYellow" GridLines="both" CellPadding="2" BorderColor="Tan"
                    ForeColor="Black" AllowPaging="True" Width="100%" AllowSorting="True" EmptyDataText="No Record Found"
                    DataKeyNames="schId">
                    <PagerStyle ForeColor="DarkSlateBlue" HorizontalAlign="Center" BackColor="PaleGoldenrod">
                    </PagerStyle>
                    <RowStyle Font-Names="Verdana" Font-Size="Small" ForeColor="#000000" BackColor="#FFFFEE"
                        VerticalAlign="Top"></RowStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100px">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundField Visible="false" DataField="schId"></asp:BoundField>
                        <asp:BoundField ItemStyle-HorizontalAlign="center" ItemStyle-Width="15%" HeaderText="Schedule Date"
                            DataField="schDate"></asp:BoundField>
                        <asp:BoundField ItemStyle-HorizontalAlign="center" ItemStyle-Width="15%" HeaderText="Status"
                            DataField="candStstus"></asp:BoundField>
                        <asp:BoundField ItemStyle-HorizontalAlign="center" ItemStyle-Width="15%" HeaderText="Comment"
                            DataField="schComment"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table width="850px" style="display: none" id="addsch" border="0">
        <tr>
            <td align="center" colspan="4" style="background: #C5D5AE">
                <font face="Verdana" color="#a2921e"><b>Add Schedule</b></font>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left" style="width: 150px">
                <b><font face="Verdana" color="#a2921e" size="2">Candiate Status</font></b>
            </td>
            <td nowrap="nowrap" align="left" style="width: 200px">
                <font face="Verdana" size="2">
                    <select size="1" id="candStatus" runat="server">
                    </select>
                </font>
            </td>
            <td nowrap="nowrap" bgcolor="#edf2e6" align="left" style="width: 200px">
                <b><font face="Verdana" color="#a2921e" size="2">Date</font></b>
            </td>
            <td nowrap="nowrap" align="left" style="width: 200px">
                <font face="Verdana" size="2">
                    <asp:TextBox ID="txtschdate" runat="server" onclick="popupCalender('txtschdate');"
                        Rows="3"  Width="120px"></asp:TextBox></font>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" bgcolor="#edf2e6" colspan="1" align="left" style="width: 120px">
                <b><font face="Verdana" color="#a2921e" size="2">Add Comment</font></b>
            </td>
            <td colspan="3" align="left">
                <font face="Verdana" size="2">
                    <asp:TextBox ID="txtAddComment" runat="server" TextMode="MultiLine" Rows="3" Columns="50"></asp:TextBox></font>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="background: #C5D5AE">
                <font face="Verdana" color="#a2921e"><b>
                    <asp:Button BorderWidth="1px" OnClientClick="javascript:rreturn blankBlank();" BackColor="#EDF2E6"
                        BorderStyle="Groove" BorderColor="#A2921E" ID="Button1" runat="server" Text="Submit" />
                       <asp:Button BorderWidth="1px" OnClientClick="javascript:return Closesch();" BackColor="#EDF2E6"
                        BorderStyle="Groove" BorderColor="#A2921E" ID="Button2" runat="server" Text="Cancel" /> 
                        </b>
                </font>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script language="javascript" type="text/javascript">
    function Closesch() {
        document.getElementById("addsch").style.display = 'none';
        return false;
    }
    function addsch() {
        if (document.getElementById("addsch").style.display == 'none')
            document.getElementById("addsch").style.display = 'block';
        

    }
    function blankBlank() {
        if (document.getElementById("txtschdate").value == '')
            alert('Enter Date');

    }

</script>

