<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MonthlyTimesheet.aspx.vb"
    Inherits="Admin_MonthlyTimesheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">
        <tr>
            <td width="20%" height="41" align="center" bgcolor="#C5D5AE">
                <b><font face="Verdana" color="#a2921e" size="4">Monthly Timesheet Report</font></b>
            </td>
        </tr>
        <tr>
            <td width="20%" height="10%" align="left">
            <asp:Label ID="lblInfoMsg" Font-Names="Verdana" ForeColor="#A2921E"  
                    Text="No Data Found" runat="server"
            Visible="False" Font-Bold="True" Font-Size="10pt"></asp:Label>
                
            </td>
        </tr>
      <tr>
                <td>
                    <asp:DataGrid ID="dgMonthTimeSheet" runat="server" BorderColor="Black" Font-Size="10pt"
                        Width="100%" Height="100%" Font-Name="Verdana" ItemStyle-HorizontalAlign="Left"
                        BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False" FooterStyle-HorizontalAlign="Right"
                        AllowSorting="True" HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray"
                        CellPadding="2" ShowFooter="True" >
                        <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="True" HorizontalAlign="Left">
                        </ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                            Font-Size="10pt"></HeaderStyle>
                        <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False" Font-Bold="True"
                            HorizontalAlign="Right"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn DataField="tsDate" SortExpression="tsDate" DataFormatString="{0:dd-MMM-yy}"
                                HeaderText="Date">
                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False"></ItemStyle>
                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                    HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="projName" SortExpression="projName" HeaderText="Project Name">
                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Left">
                                </ItemStyle>
                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                    HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Module Name" SortExpression="moduleName" ItemStyle-BackColor="#FFFFEE">
                                <ItemTemplate>
                                    <asp:Label ID="lblModuleName" Text='<%#container.dataitem("moduleName")%>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbltitle" runat="server" Text="Total Hours" />
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <FooterStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour" SortExpression="tsHour">
                                <ItemTemplate>
                                    <asp:Label ID="lblhours" Text='<%#container.dataitem("tsHour")%>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%=lblhourstotal.text%>
                                </FooterTemplate>
                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                </ItemStyle>
                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False" HorizontalAlign="Center"
                                    Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False"></FooterStyle>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="tsComment" SortExpression="tsComment" HeaderText="Comment"
                                ItemStyle-BackColor="#FFFFEE">
                                <ItemStyle HorizontalAlign="Left" Wrap="True" Width="15%"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="tsEntryDate" SortExpression="tsEntryDate" DataFormatString="{0:dd-MMM-yy hh:mm tt}"
                                HeaderText="Entry Date">
                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                </ItemStyle>
                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                    HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#003399" BackColor="#C5D5AE" Mode="NumericPages">
                        </PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
          
            <tr>
                <td>
                    <asp:Label ID="lblhourstotal" runat="server" Visible="false" />
                </td>
            </tr>
    </table>
       
    </div>
    </form>
</body>
</html>
