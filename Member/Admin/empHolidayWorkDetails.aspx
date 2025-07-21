<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empHolidayWorkDetails.aspx.vb"
    Inherits="Admin_empHolidayWorkDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Holiday Working Details</title>
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
    <script type="text/javascript">
        function checkLength(sender) {

            if (sender.value.length > 1000) {
                sender.value = sender.value.substr(0, 1000);
            }
        }

        function Confirm(id) {


            if (confirm('Are you sure to Create Comp Off?')) {
                //window.location="CreateCompOff.aspx?ID=" + id
                window.open("CreateCompOff.aspx?ID="+id,"_blank",'toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes, resizable=no, copyhistory=yes, width=500, height=600')
               // window.open('CreateCompOff.aspx?ID=' + id, 'dialogHeight:600px;dialogWidth:600px;')

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
                                <td bgcolor="#edf2e6" align="left">
                                    <b><%--<font face="Verdana" size="2"><a href="empattdetails.aspx" style="color: #A2921E;">
                                        <font color="#A2921E">Attendance</font></a>|<a href="empHolidayWorkDetails.aspx"
                                            style="color: #A2921E;"><font color="#A2921E">Holiday Working</font></a>
                                    --%></b>
                                </td>
                               
							 <td bgcolor="#edf2e6" align="right">
                                    <b><font face="Verdana" size="2"><asp:LinkButton ID="lnkPending" runat="server" OnClick="lnkPending_Click" Text="Pending" ForeColor="#A2921E"></asp:LinkButton>|
                                      <asp:LinkButton ID="lnkApproved" runat="server" OnClick="lnkApproved_Click" Text="Approved" ForeColor="#A2921E"></asp:LinkButton>|
                                      <asp:LinkButton ID="lnkRejected" runat="server" OnClick="lnkRejected_Click" Text="Rejected" ForeColor="#A2921E"></asp:LinkButton>|
                                      <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click" Text="Admin Cancel" ForeColor="#A2921E"></asp:LinkButton>
                                    </font> 
                                    </b>
                                </td>
                             
                            </tr>
                            <tr>
                                <td align="left" style="padding-top: 20px" colspan="2">
                                    <table id="Table1" bordercolor="#c5d5ae" cellspacing="0" cellpadding="4" border="1"
                                        width="55%">
                                          <tr>
                                            <td bgcolor="#edf2e6" align="left" width="25%">
                                               <b><font face="Verdana" color="#a2921e" size="2"> <asp:Label ID="lblLocation" Text ="Location:" runat="server"/></font></b>
                                             </td>
                                             <td bgcolor="#edf2e6" align="left"  colspan="3">
                                                <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass="b_dropdown"  Visible="false" AppendDataBoundItems="true" Width="200px">
                                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <b><font face="Verdana" color="#a2921e" size="2"> <asp:Label ID="lblLocationId" runat="server" Visible="false"/></font></b>
                                             </td>
        
                                        </tr>
                                        <tr>
                                            <td bgcolor="#edf2e6" align="left" width="25%">
                                                <b><font face="Verdana" color="#a2921e" size="2">Employee Name</font></b>
                                            </td>
                                            <td width="75%" align="left" colspan="3">
                                                <asp:DropDownList ID="ddlEmp" runat="server" AutoPostBack="false" Width="225">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="reqproj" runat="server" ErrorMessage="Please Select Employee."
                                                    ControlToValidate="ddlEmp" InitialValue="-1" Display="Dynamic" ValidationGroup="grpproj"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="textcolumn" align="left">
                                                Date
                                            </td>
                                            <td align="left">
                                                <font color="#a2921e" size="2" face="Verdana, Arial, Helvetica, sans-serif"><strong>
                                                    &nbsp;From&nbsp;</strong> </font>
                                                <asp:TextBox ID="txtFromDate" runat="server" size="7" Width="75px" onclick="popupCalender('txtFromDate');"
                                                    onkeypress="return false;"></asp:TextBox>
                                                <font color="#a2921e" size="2" face="Verdana, Arial, Helvetica, sans-serif"><strong>
                                                    &nbsp;To&nbsp;</strong></font>
                                                <asp:TextBox ID="txtToDate" runat="server" size="7" Width="75px" onclick="popupCalender('txtToDate');"
                                                    onkeypress="return false;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="75%" colspan="4" height="30" style="padding-left: 180px">
                                                <asp:Button ID="btnsearch" runat="server" Style="font-family: Verdana; font-size: 8pt;
                                                    color: #A2921E; font-weight: bold; background-color: #C5D5AE" Text="Search" CausesValidation="true"
                                                    ValidationGroup="grpproj"></asp:Button>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnReset" runat="server" Style="font-family: Verdana; font-size: 8pt;
                                                    color: #A2921E; font-weight: bold; background-color: #C5D5AE" Text="Reset"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="center" id="Usergrd" runat="server">
                                <td style="padding-top: 20px" colspan="2">
                                    <asp:DataGrid ID="dgHolidayWorkdtl" runat="server" BorderColor="Black" Font-Size="10pt"
                                        Width="100%" Height="100%" Font-Name="Verdana" ItemStyle-HorizontalAlign="Left"
                                        BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False" FooterStyle-HorizontalAlign="Right"
                                        AllowSorting="True" HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray"
                                        CellPadding="2" ShowFooter="True" PageSize="20" AllowPaging="true">
                                        <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="True" HorizontalAlign="Left">
                                        </ItemStyle>
                                        <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                            Font-Size="10pt"></HeaderStyle>
                                        <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False" Font-Bold="True"
                                            HorizontalAlign="Right"></FooterStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="Id" HeaderText="ID"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="false" DataField="Empid" HeaderText="empid"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="EmpName" SortExpression="EmpName" HeaderText="Employee Name"
                                                ItemStyle-BackColor="#FFFFEE">
                                                <ItemStyle HorizontalAlign="Left" Wrap="True" Width="15%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ProjName" SortExpression="ProjName" HeaderText="Project Name">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Left">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="ProjId" HeaderText="ProjID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="HolidayDate" SortExpression="HolidayDate" DataFormatString="{0:dd-MMM-yy}"
                                                HeaderText="Holiday Date">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False"></ItemStyle>
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
                                            <asp:TemplateColumn HeaderText="Admin Comment">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" Width="10%"></ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False">
                                                </HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtComment" Width="180" Height="50" runat="server" TextMode="MultiLine"
                                                        onkeyup="checkLength(this);">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" Width="10%"></ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False">
                                                </HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnApprove" runat="server" CommandName="Approve" Text="Approve" Style="font-family: Verdana;
                                                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                                                    </asp:Button>
                                                    <asp:Button ID="btnReject" runat="server" CommandName="Reject" Text="Reject" Style="font-family: Verdana;
                                                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                                                    </asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Left" ForeColor="#003399" BackColor="#C5D5AE" PageButtonCount="5"
                                            Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr align="center" id="Admingrd" runat="server">
                                <td style="padding-top: 20px" colspan="2">
                                    <asp:DataGrid ID="dgAdminGrd" runat="server" BorderColor="Black" Font-Size="10pt"
                                        Width="100%" Height="100%" Font-Name="Verdana" ItemStyle-HorizontalAlign="Left"
                                        BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False" FooterStyle-HorizontalAlign="Right"
                                        AllowSorting="True" HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray"
                                        CellPadding="2" ShowFooter="True" PageSize="20" AllowPaging="true">
                                        <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="True" HorizontalAlign="Left">
                                        </ItemStyle>
                                        <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                            Font-Size="10pt"></HeaderStyle>
                                        <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False" Font-Bold="True"
                                            HorizontalAlign="Right"></FooterStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="Id" HeaderText="ID"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="false" DataField="Empid" HeaderText="empid"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="EmpName" SortExpression="EmpName" HeaderText="Employee Name"
                                                ItemStyle-BackColor="#FFFFEE">
                                                <ItemStyle HorizontalAlign="Left" Wrap="True" Width="15%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ProjName" SortExpression="ProjName" HeaderText="Project Name">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" HorizontalAlign="Left">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False"
                                                    HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="ProjId" HeaderText="ProjID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="HolidayDate" SortExpression="HolidayDate" DataFormatString="{0:dd-MMM-yy}"
                                                HeaderText="Holiday Date">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False"></ItemStyle>
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
                                           
                                            <asp:BoundColumn DataField="UserReason" SortExpression="UserReason" HeaderText="Reason"
                                                ItemStyle-BackColor="#FFFFEE">
                                                <ItemStyle HorizontalAlign="Left" Wrap="True" Width="15%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
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
                                            <asp:TemplateColumn HeaderText="Admin Cancel Comment">
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="False" Width="10%"></ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="False">
                                                </HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCanComment" Width="180" Height="50" runat="server" TextMode="MultiLine"
                                                        onkeyup="checkLength(this);">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <ItemStyle ForeColor="Black" BackColor="#FFFFEE" Wrap="true" HorizontalAlign="Center">
                                                </ItemStyle>
                                                <HeaderStyle Font-Bold="True" ForeColor="#A2921E" BackColor="#C5D5AE" Wrap="true">
                                                </HeaderStyle>
                                                <FooterStyle ForeColor="#A2921E" BackColor="#EDF2E6" Wrap="False"></FooterStyle>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" Style="font-family: Verdana;
                                                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                                                    </asp:Button>
                                                      <asp:Button ID="btnComoff" runat="server"  Text="Add Comp Off" Style="font-family: Verdana;                                  
                                                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" 
                                                         CommandName="compoff" >
                                                    </asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Left" ForeColor="#003399" BackColor="#C5D5AE" PageButtonCount="5"
                                            Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                          <asp:HiddenField ID="hdnStatus" runat="server"/>
                          <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
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
