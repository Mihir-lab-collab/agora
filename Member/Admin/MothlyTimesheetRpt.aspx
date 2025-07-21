<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MothlyTimesheetRpt.aspx.vb"
    Inherits="Admin_MothlyTimesheetRpt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Monthly Report</title>
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
    <script type="text/javascript">
        function checkLength(sender) {

            if (sender.value.length > 1000) {
                sender.value = sender.value.substr(0, 1000);
            }
        }
    </script>
</head>
<body>
    <script language="JavaScript" src="../Includes/CalendarControl.js" type="text/javascript">
    </script>
    <form id="form1" runat="server" method="post">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="4" width="100%" border="0" style="border-collapse: collapse;
        border-color: #111111;" align="center">
        <tr>
            <td>
                <uc1:adminMenu ID="AdminMenu2" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="pnl" runat="server">
                    <ContentTemplate>
                        <table cellspacing="0" cellpadding="4" width="100%" border="0" style="border-collapse: collapse;
                            border-color: #111111;" align="center">
                            <tr bgcolor="#edf2e6">
                                <td bgcolor="#edf2e6" align="left" colspan="2">
                                    <b><font face="Verdana" size="2"><a href="../Emp/emptimsheet.aspx" style="color: #A2921E;">
                                        <font color="#A2921E">Timesheet</font></a>|<a href="MothlyTimesheetRpt.aspx" style="color: #A2921E;"><font
                                            color="#A2921E">Monthly Report</font></a> </b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <font face="Verdana" size="2" color="#a2921e"><b>
                                        <asp:LinkButton ID="prevMonth" Text="<<" CommandArgument="prev" runat="server" OnClick="PagerButtonClick" />
                                        <asp:Label ID="lblMonth" runat="server"></asp:Label>
                                        <asp:LinkButton ID="nextMonth" Text=">>" CommandArgument="next" runat="server" OnClick="PagerButtonClick" />
                                    </b></font>
                                </td>
                                <td>
                                    <font face="Verdana" size="2" color="#a2921e"><b>Location:</b></font>
                                     <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass="c_dropdown"
                                      OnSelectedIndexChanged="dlLocation_SelectedIndexChanged"  Visible="false" AppendDataBoundItems="true" Width="200px">
                                      <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                               <font face="Verdana" size="2"> <asp:Label ID="lblLocationId" runat="server" Visible="false"/></font>
                                </td>
                            </tr>
                            <tr align="center" id="Usergrd" runat="server">
                                <td style="padding-top: 20px" colspan="2">
                                    <asp:DataGrid ID="dgMonthTimeRpt" runat="server" BorderColor="Black" Font-Size="10pt"
                                        Width="80%" Height="100%" Font-Name="Verdana" ItemStyle-HorizontalAlign="Left"
                                        BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False" FooterStyle-HorizontalAlign="Right"
                                        AllowSorting="True" HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray"
                                        CellPadding="2" ShowFooter="True">
                                        <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="True" HorizontalAlign="Left">
                                        </ItemStyle>
                                        <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                            Font-Size="10pt"></HeaderStyle>
                                        <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False" Font-Bold="True"
                                            HorizontalAlign="Right"></FooterStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="false" DataField="Empid" HeaderText="empid" ItemStyle-Width="5%"></asp:BoundColumn>
                                            
                                            <asp:TemplateColumn HeaderText="Employee Name" SortExpression="EmployeeName" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="HyperLink2" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem, "EmployeeName").ToString()%>'
                                                  NavigateUrl='<%# "MonthlyTimesheet.aspx?" + "empId=" + DataBinder.Eval(Container.DataItem,"Empid").ToString() + "&Month=" +  hdnmonth.Value()+ "&Year=" + hdnyear.Value() %>' 
                                                 onclick="window.open(this.href, 'Popupwindow','toolbar=yes,location=no,menubar=no,width=700,height=600,resizable=no,scrollbars=yes,top=200,left=250');return false;"/>
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="Role" SortExpression="Role" HeaderText="Role" ItemStyle-Width="25%">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Left">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="TotalHour" SortExpression="TotalHour" HeaderText="Total Hours" ItemStyle-Width="5%">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Leave" SortExpression="Leave" HeaderText="Leaves" ItemStyle-BackColor="#FFFFEE">
                                                <ItemStyle HorizontalAlign="Center" Wrap="True" Width="5%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="EntryDate" SortExpression="EntryDate" 
                                                HeaderText="Last Entry Date">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False" Width="20%"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <%--<FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>--%>
                                            </asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Left" ForeColor="#003399" BackColor="#C5D5AE" PageButtonCount="5"
                                            Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                         <asp:HiddenField ID="hdLocationId" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <input type="hidden" id="lId" runat="server" />
                <input type="hidden" id="curdate" runat="server" />
                <input type="hidden" id="hdnmonth" runat="server" />
                <input type="hidden" id="hdnyear" runat="server" />               
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
