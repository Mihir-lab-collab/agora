<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HolidayWorking.aspx.vb"
    Inherits="Emp_HolidayWorking" %>

<%@ Register Src="~/controls/empMenuBar.ascx" TagName="empMenuBar" TagPrefix="uc1" %>
<%@ Register Src="~/controls/empHeader.ascx" TagName="empHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Holiday Working</title>
    <script type="text/javascript">
        function checkLength() {
            var sender = document.getElementById('<%=txtReason.ClientID %>');
            if (sender.value.length > 1000) {
                sender.value = sender.value.substr(0, 1000);
            }
        }
    </script>
</head>
<body>
    <form id="Attendence" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="4" width="100%" border="0" style="border-collapse: collapse;
        border-color: #111111;" align="center">
        <tr>
            <td>
                <asp:UpdatePanel ID="pnl" runat="server">
                    <ContentTemplate>
                        <table cellspacing="0" cellpadding="4" width="100%" border="0" style="border-collapse: collapse;
                            border-color: #111111;" align="center">
                            <tr>
                                <td>
                                    <uc2:empHeader ID="EmpHeader1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <uc1:empMenuBar ID="EmpMenuBar1" runat="server" />
                                </td>
                            </tr>
                            <tr bgcolor="#edf2e6">
                                <td align="left" style="height: 21px; font-size: 12px; color: #A2921E; font-family: Verdana;
                                    background-color: #edf2e6; white-space: nowrap">
                                    <%
                                        Dim strDate = Request.QueryString("strDate")
                                        'If Not IsDate(strDate) Or Month(strDate) = Month(Date.Today) Then
                                        '    strDate = Now()
                                        'End If
									
									
                                    %>
                                    <a href="empAtt.aspx?strDate=<% =strDate%>" style="color: #A2921E;"><b>Attendance</b></a>|
                                    <a href="HolidayWorking.aspx?strDate=<% =strDate%>" style="color: #A2921E;"><b>Holiday
                                        Working</b></a>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table id="Table1" bordercolor="#c5d5ae" cellspacing="0" cellpadding="4" border="1"
                                        width="60%">
                                        <tr>
                                            <td bgcolor="#c5d5ae" colspan="4" align="left">
                                                <b><font face="Verdana" color="#a2921e" size="2" style="text-align: left">Holiday Working
                                                    Request</font></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#edf2e6" align="left" width="25%">
                                                <b><font face="Verdana" color="#a2921e" size="2">Holiday Date</font></b>
                                            </td>
                                            <td width="75%" colspan="3" align="left">
                                                <asp:DropDownList ID="ddlHolidayDt" runat="server" AutoPostBack="false" Width="250">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="reqproj" runat="server" ErrorMessage="Please Select Holiday Date."
                                                    ControlToValidate="ddlHolidayDt" InitialValue="-1" Display="Dynamic" ValidationGroup="grpproj"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#edf2e6" align="left">
                                                <b><font face="Verdana" color="#a2921e" size="2">Project Name</font></b>
                                            </td>
                                            <td colspan="3" align="left">
                                                <asp:DropDownList ID="ddlProj" runat="server" AutoPostBack="false" Width="250">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Project."
                                                    ControlToValidate="ddlProj" InitialValue="-1" Display="Dynamic" ValidationGroup="grpproj"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#edf2e6" align="left">
                                                <b><font face="Verdana" color="#a2921e" size="2">Expected Hours</font></b>
                                            </td>
                                            <td colspan="3" align="left">
                                                <asp:DropDownList ID="ddlHours" runat="server" AutoPostBack="false" Width="250">
                                                    <asp:ListItem Value="-1" Selected="True" Text="Select"></asp:ListItem>
                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                    <asp:ListItem Value="7">7</asp:ListItem>
                                                    <asp:ListItem Value="8">8</asp:ListItem>
                                                    <asp:ListItem Value="9">9</asp:ListItem>
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="11">11</asp:ListItem>
                                                    <asp:ListItem Value="12">12</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Hours."
                                                    ControlToValidate="ddlHours" InitialValue="-1" Display="Dynamic" ValidationGroup="grpproj"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#edf2e6" align="left">
                                                <b><font face="Verdana" color="#a2921e" size="2">Reason</font></b>
                                            </td>
                                            <td colspan="3" align="left">
                                                <asp:TextBox ID="txtReason" runat="server" MaxLength="1000" TextMode="MultiLine"
                                                    Height="75px" Width="275px" onkeyup="checkLength();"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter Reason."
                                                    ControlToValidate="txtReason" Display="Dynamic" ValidationGroup="grpproj"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%" height="30" align="left" colspan="4" style="padding-left: 250px">
                                                <asp:Button ID="btnSave" runat="server" Style="font-family: Verdana; font-size: 8pt;
                                                    color: #A2921E; font-weight: bold; background-color: #C5D5AE" Text="Submit" CausesValidation="true"
                                                    ValidationGroup="grpproj"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="4" width="100%" style="padding-top: 20px">
                                    <asp:DataGrid ID="dgHolidayWorkdtl" runat="server" BorderColor="Black" Font-Size="10pt"
                                        Font-Name="Verdana" BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"
                                        FooterStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray"
                                        CellPadding="2" ShowFooter="True" Width="100%" PageSize="20" AllowPaging="true"
                                        AllowSorting="true">
                                        <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="True" HorizontalAlign="Left">
                                        </ItemStyle>
                                        <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                            Font-Size="10pt"></HeaderStyle>
                                        <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False" Font-Bold="True"
                                            HorizontalAlign="Right"></FooterStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="Id" HeaderText="ID"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="ProjId" HeaderText="ProjID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="HolidayDate" SortExpression="HolidayDate" DataFormatString="{0:dd-MMM-yy}"
                                                HeaderText="Holiday Date">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False"></ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ProjectName" SortExpression="ProjectName" HeaderText="Project Name">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Left">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ExpectedHours" SortExpression="ExpectedHours" HeaderText="Expected Hours">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <%--  <asp:TemplateColumn HeaderText="Hours" SortExpression="Hours">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblhours" Text='<%#container.dataitem("Hours")%>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                                            </ItemStyle>
                                                            <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                                HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False" HorizontalAlign="Center"
                                                                Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False"></FooterStyle>
                                                        </asp:TemplateColumn>--%>
                                            <asp:BoundColumn DataField="UserReason" SortExpression="UserReason" HeaderText="Reason"
                                                ItemStyle-BackColor="#FFFFEE">
                                                <ItemStyle HorizontalAlign="Left" Wrap="True" Width="15%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="UserEntryDate" SortExpression="UserEntryDate" DataFormatString="{0:dd-MMM-yy hh:mm tt}"
                                                HeaderText="Entry Date">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Statusflag" SortExpression="Statusflag" HeaderText="Status">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Center">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="AdminComment" SortExpression="AdminComment" HeaderText="Admin Comment">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Left">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                             <asp:BoundColumn DataField="AdminCanReason" SortExpression="AdminCanReason" HeaderText="Admin Cancel Comment">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Left">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="Status" HeaderText="Status"></asp:BoundColumn>
                                            <asp:TemplateColumn>
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" Width="10%"></ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False">
                                                </HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                                <ItemTemplate>
                                                    <asp:Button ID="update" runat="server" CommandName="update" Text="Update" Style="font-family: Verdana;
                                                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                                                    </asp:Button>
                                                    <asp:Button ID="Cancel" runat="server" CommandName="Cancel" Text="Cancel" Style="font-family: Verdana;
                                                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                                                    </asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Left" ForeColor="#003399" BackColor="#C5D5AE"  
                            Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <input type="hidden" id="lId" runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
