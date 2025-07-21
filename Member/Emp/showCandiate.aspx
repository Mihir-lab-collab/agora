<%@ Page Language="VB" AutoEventWireup="false" CodeFile="showCandiate.aspx.vb" Inherits="emp_showCandiate" %>

<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Show Candidate Details</title>

    <script language="javascript" src="/includes/CalendarControl.js" type="text/javascript"></script>

    <link rel="stylesheet" href="/includes/CalendarControl.css" type="text/css" />
    <link rel="stylesheet" href="../css/style.css" type="text/css" />

    <script type="text/javascript">
        function showPopup(ID) {
            window.open('../emp/candShowSchedule.aspx?id=' + ID, 'null', 'scrollbars=yes,toolbar=no,menubar=no,location=right,resizable=no,width=900px,height=650px,left=80px,top=150px');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table cellspacing="1" cellpadding="4" align="center" border="2" bordercolor="#F1F4EC"
        width="100%">
        <tr>
            <td colspan="4">
                <table id="Table3" cellspacing="0" cellpadding="2" width="100%" border="0">
                    <tr>
                        <td>
                            <empheader:empheader id="Empheader" runat="server"></empheader:empheader>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:empmenubar id="EmpMenuBar" runat="server"></uc1:empmenubar>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table cellspacing="1" cellpadding="4" border="2" width="100%">
        <tr>
            <td colspan="5" align="center" style="background: #C5D5AE">
                <font face="Verdana" color="#a2921e"><b>Candidate Details</b></font>
            </td>
        </tr>
        <tr>
            <td colspan="5" style="background: #C5D5AE; height: 29px;">
                <font style="font-size: 12pt; color: #A2921E; font-family: Arial"><b>Candidate Search</b></font>
            </td>
        </tr>
        <tr>
            <td style="background: #C5D5AE; height: 29px;">
                <font style="font-size: 10pt; color: #A2921E; font-family: Arial">Date</font>
            </td>
            <td style="background: #C5D5AE; height: 29px;">
                <font style="font-size: 10pt; color: #A2921E; font-family: Arial">Name</font>
            </td>
            <td style="background: #C5D5AE; height: 29px;">
                <font style="font-size: 10pt; color: #A2921E; font-family: Arial">Current Status</font>
            </td>
             <td style="background: #C5D5AE; height: 29px;">
                <font style="font-size: 10pt; color: #A2921E; font-family: Arial">Post Applied For</font>
            </td>
            <td style="background: #C5D5AE; height: 29px;">
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtdate" runat="server" onclick="popupCalender('txtdate');" onkeypress="return false;"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
 
            </td>
            <td>
                <asp:DropDownList ID="drpStatus" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select one.." Value="" Selected="True"></asp:ListItem>
                </asp:DropDownList>
              </td>
              <td>
                 <asp:DropDownList ID="drpJobType" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select one.." Value="" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;<asp:Button ID="btnsearch" runat="server" Width="90px" BorderWidth="1px" BackColor="#EDF2E6"
                    BorderStyle="Groove" BorderColor="#A2921E" Text="Search"></asp:Button>
            </td>
            <td>
                <asp:Button ID="btnAdd" runat="server" Width="150px" BorderWidth="1px" BackColor="#EDF2E6"
                    BorderStyle="Groove" BorderColor="#A2921E" Text="Add New Candidate"></asp:Button>&nbsp;<asp:Button
                        ID="btnschSerch" runat="server" Width="150px" BorderWidth="1px" BackColor="#EDF2E6"
                        BorderStyle="Groove" BorderColor="#A2921E" Text="Schedule Search"></asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:GridView ID="gridShowCandiate" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
                    BackColor="LightGoldenrodYellow" GridLines="both" CellPadding="2" BorderColor="Tan"
                    ForeColor="Black" AllowPaging="True" Width="100%" AllowSorting="True" EmptyDataText="No Record Found"
                    OnRowDeleting="gwListAccessUnits_RowDeleting" DataKeyNames="candId" PageSize="50">
                    <PagerStyle ForeColor="DarkSlateBlue" HorizontalAlign="Center" BackColor="PaleGoldenrod">
                    </PagerStyle>
                    <RowStyle Font-Names="Verdana" Font-Size="Small" ForeColor="#000000" BackColor="#FFFFEE"
                        VerticalAlign="Top"></RowStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100px">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundField ReadOnly="True" HeaderText="ID" DataField="candid"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" SortExpression="candname" HeaderText="Name">
                            <ItemTemplate>
                                <div onclick='javascript:showPopup(<%#Eval("candid")%>);' style="cursor: pointer;
                                    text-decoration: underline;">
                                    <%#Eval("candname")%></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Date" DataField="datecand" ItemStyle-Wrap="false" SortExpression="datecand" DataFormatString="{0:d MMM, yyyy}"></asp:BoundField>
                        <asp:BoundField HeaderText="Contact No." DataField="candmobileno"></asp:BoundField>
                        <asp:BoundField HeaderText="Post Applied For" DataField="skill" ItemStyle-HorizontalAlign="Center">
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Experience in Yrs" ItemStyle-HorizontalAlign="Center"
                            DataField="totexp"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Technical Skill">
                            <ItemTemplate>
                                <%#getSkills(Eval("candid"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Current Status" DataField="candstatus" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField HeaderText="Comment" DataField="schComment" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnResume" runat="server">Resume</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button CommandArgument='<%# Eval("candId") %>' runat="server" ID="btnDelete"
                                    CommandName="Delete" Text="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script language="javascript" type="text/javascript">
    function popupProjectDetailView(Id) {
        window.open = "CandResume.aspx?Id=" + Id;
    } 
</script>

